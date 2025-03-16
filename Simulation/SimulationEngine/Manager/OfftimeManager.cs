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
        // 사용자에게 입력받은 offTimeList
        private readonly List<(DateTime Start, DateTime End)> _offTimeList;
        // off끼리 겹치거나 예외처리 등을 진행 한 후, offtimeManager에서 실질적으로 계산에 사용하는 list 
        private List<(DateTime Start, DateTime End)> _mergedOffTimeList;

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
            _mergedOffTimeList = MergeOffTimeIntervals();
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
            return _mergedOffTimeList.Any(t =>
                // t.Start < end AND t.End > start 이면 겹침
                t.Start < end && t.End > start
            );
        }

        public DateTime GetAdjustedEndTime(DateTime taskStart, DateTime taskEnd)
        {
            // 만약 taskEnd > simEnd면, 어차피 시뮬끝까지만 계획
            if (taskEnd > SimFactory.Instance._simulationEndTime) taskEnd = SimFactory.Instance._simulationEndTime;

            if (taskStart >= taskEnd) return taskEnd; // 잘못된 구간이거나 0시간

            // 교집합 길이 계산
            TimeSpan overlap = CalculateOverlapDuration(taskStart, taskEnd);

            var newEnd = taskEnd.Add(overlap);
            // 만약 오프타임 적용 후 종료가 시뮬 끝을 넘어가면 simEnd로 제한
            if (newEnd > SimFactory.Instance._simulationEndTime)
                newEnd = SimFactory.Instance._simulationEndTime;

            return newEnd;
        }

        // 오프타임 구간들이 서로 겹치면 합쳐서 하나의 구간으로 만든다.
        // 예) [13~15], [14~16] → [13~16]
        private List<(DateTime Start, DateTime End)> MergeOffTimeIntervals()
        {
            var validIntervals = new List<(DateTime Start, DateTime End)>();

            // 먼저 시뮬 범위 clip 및 잘못된 구간 제거
            foreach (var item in _offTimeList)
            {
                // 잘못된 구간 (End <= Start) 무시
                if (item.End <= item.Start)
                    continue;
                if (item.End < SimFactory.Instance._simulationStartTime || item.Start > SimFactory.Instance._simulationEndTime)
                    continue;

                // 클리핑: 시뮬레이션 범위 내부로 제한
                var clippedStart = (item.Start < SimFactory.Instance._simulationStartTime) ? SimFactory.Instance._simulationStartTime : item.Start;
                var clippedEnd = (item.End > SimFactory.Instance._simulationEndTime) ? SimFactory.Instance._simulationEndTime : item.End;

                // 0분짜리(동일 시각) 제외
                if (clippedEnd <= clippedStart)
                    continue;

                validIntervals.Add((clippedStart, clippedEnd));
            }

            validIntervals = validIntervals.OrderBy(x => x.Start).ToList();

            // 3) 병합 로직
            var merged = new List<(DateTime Start, DateTime End)>();
            if (validIntervals.Count == 0)
                return merged; 

            var current = validIntervals[0];
            for (int i = 1; i < validIntervals.Count; i++)
            {
                var next = validIntervals[i];
                if (next.Start <= current.End)
                {
                    // 겹침 -> 병합
                    current = (current.Start, (next.End > current.End) ? next.End : current.End);
                }
                else
                {
                    merged.Add(current);
                    current = next;
                }
            }
            merged.Add(current);

            return merged.OrderBy(x => x.Start).ToList();
        }

        private TimeSpan CalculateOverlapDuration(DateTime taskStart, DateTime taskEnd)
        {
            TimeSpan totalOverlap = TimeSpan.Zero;
            TimeSpan overlap = TimeSpan.Zero;

            DateTime currentEnd = taskEnd; 
            foreach (var off in _mergedOffTimeList)
            {
                if (off.End < taskStart) continue;

                // off.Start >= taskEnd 면, 이후 구간은 볼 필요 없음
                if (off.Start >= currentEnd) break;

                // 교집합 = [max(taskStart, off.Start), min(taskEnd, off.End))
                DateTime overlapStart = (off.Start > taskStart) ? off.Start : taskStart;
                DateTime overlapEnd = off.End; 

                if (overlapEnd > overlapStart)
                {
                    overlap = (overlapEnd - overlapStart);
                    totalOverlap += overlap;
                }
                currentEnd.Add(overlap); 
            }
            return totalOverlap;
        }

        public void WriteOffTimeLog()
        {
            _model.WriteOffTimeLog(_mergedOffTimeList); 
        }
    }
}
