using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.Schedule
{
    public class ScheduleManager
    {
        private SortedDictionary<DateTime, List<Action>> eventQueue = new SortedDictionary<DateTime, List<Action>>();
        private DateTime _currentTime;
        private DateTime _simulationEndTime;

        public ScheduleManager(DateTime simulationStartTime, DateTime simulationEndTime)
        {
            _currentTime = simulationStartTime;
            _simulationEndTime = simulationEndTime;
        }

        // 이벤트 등록 메서드
        public void AddEvent(DateTime time, Action action)
        {
            if (!eventQueue.ContainsKey(time))
            {
                eventQueue[time] = new List<Action>();
            }
            eventQueue[time].Add(action);
        }

        public void Run()
        {
            while (eventQueue.Count > 0) //  새로운 이벤트가 추가되더라도 계속 실행됨
            {
                var keys = new List<DateTime>(eventQueue.Keys); // 현재 이벤트 리스트를 복사
                foreach (var time in keys)
                {
                    _currentTime = time;
                    if (eventQueue.TryGetValue(time, out var actions))
                    {
                        foreach (var action in actions)
                        {
                            action?.Invoke();
                        }
                        eventQueue.Remove(time); //실행이 끝난 이벤트 삭제
                    }
                }
            }
        }
    }
}
