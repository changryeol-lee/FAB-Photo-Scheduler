using SimulationEngine.Common;
using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationEngine.ProcessEntity;
using System.Runtime.InteropServices;

namespace SimulationEngine.Manager
{
    public class OffTimeManager
    {
        private readonly List<(DateTime Start, DateTime End)> _offTimeList;
        private ISimOffTimeModel _model; 

        public OffTimeManager(ISimOffTimeModel model)
        {
            _model = model;
            _offTimeList = new List<(DateTime, DateTime)>();
            DateTime simStart = SimFactory.Instance._simulationStartTime;
            DateTime simEnd = SimFactory.Instance._simulationEndTime;

            // 1) model에게 규칙을 가져옴
            var rules = model.GetOffTimeRules();

            // 2) 각 Rule을 실제 DateTime 구간으로 변환
            foreach (var rule in rules)
            {
                switch (rule.RuleType)
                {
                    case OffTimeRuleType.Daily:
                        GenerateDailyOffTime(rule, simStart, simEnd);
                        break;
                    case OffTimeRuleType.Weekly:
                        GenerateWeeklyOffTime(rule, simStart, simEnd);
                        break;
                    case OffTimeRuleType.Once:
                        // 한번만
                        var onceStart = rule.StartDateTime;
                        var onceEnd = rule.EndDateTime;

                        // 시뮬레이션 기간 내로 클리핑
                        if (onceEnd > simStart && onceStart < simEnd)
                        {
                            _offTimeList.Add((onceStart < simStart ? simStart : onceStart,
                                              onceEnd > simEnd ? simEnd : onceEnd));
                        }
                        break;
                }
            }

            // 필요하다면 오프타임 목록을 시간순 정렬
            _offTimeList = _offTimeList.OrderBy(x => x.Start).ToList();
        }

        private void GenerateDailyOffTime(OffTimeRule rule, DateTime simStart, DateTime simEnd)
        {
            // 매일 rule.StartTime~rule.EndTime을 off time으로 생성
            DateTime day = simStart.Date;
            while (day <= simEnd)
            {
                var start = day + rule.StartTime;
                var end = day + rule.EndTime;
                // 시뮬 기간 교집합 체크
                if (end > simStart && start < simEnd)
                {
                    _offTimeList.Add((start < simStart ? simStart : start,
                                      end > simEnd ? simEnd : end));
                }
                day = day.AddDays(1);
            }
        }

        private void GenerateWeeklyOffTime(OffTimeRule rule, DateTime simStart, DateTime simEnd)
        {
            // 특정 요일 + 시간대
            DateTime day = simStart.Date;
            while (day <= simEnd)
            {
                if (rule.Days.Contains(day.DayOfWeek))
                {
                    var start = day + rule.StartTime;
                    var end = day + rule.EndTime;
                    if (end > simStart && start < simEnd)
                    {
                        _offTimeList.Add((start < simStart ? simStart : start,
                                          end > simEnd ? simEnd : end));
                    }
                }
                day = day.AddDays(1);
            }
        }

        public bool IsOffTimeRange(DateTime start, DateTime end)
        {
            // 어떤 오프타임 t와도 겹치는지 판단
            return _offTimeList.Any(t =>
                // t.Start < end AND t.End > start 이면 겹침
                t.Start < end && t.End > start
            );
        }

        public DateTime GetAdjustedEndTime(DateTime start, DateTime end)
        {
            // 1) off time과 부분 겹침이 있는지 먼저 확인
            var offTimes = _offTimeList.Where(t => t.Start < end && t.End > start)
                                       .OrderBy(t => t.Start).ToList();

            // 2) 겹치는 오프타임이 없다면 원래 end 반환
            if (offTimes.Count == 0)
                return end;

            // 3) 예: 첫 번째 off time이 4~5이고, 작업은 3~6이라면
            //    off time 1시간만큼 뒤로 밀림 → 실제 종료시각은 7시가 될 수도 있음
            //    (단, 중간에 여러 off time이 있을 수도 있으면 더 복잡해짐)
            double totalOffTimeMinutes = 0.0;
            DateTime current = start;

            foreach (var off in offTimes)
            {
                // t가 (4~5)
                // if current < t.Start => 작업 진행
                if (off.Start > current && off.Start < end)
                {
                    // 작업은 current ~ t.Start 동안 진행 가능
                    current = off.Start;
                }

                // t가 (4~5) -> 1시간 off
                if (off.End > current && off.End < end)
                {
                    // off time만큼 push
                    totalOffTimeMinutes += (off.End - current).TotalMinutes;
                    current = off.End;
                }
            }

            // 4) 실제 종료시각 = end + totalOffTimeMinutes
            return end.AddMinutes(totalOffTimeMinutes);
        }

        public void WriteOffTimeLog()
        {
            _model.WriteOffTimeLog(_offTimeList); 
        }
    }
}
