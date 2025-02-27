using SimulationEngine.SimulationEntity;
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
        //public DateTime currentTime = SimFactory.Instance.currentTime;
        private DateTime _simulationEndTime;

        public ScheduleManager(DateTime simulationStartTime, DateTime simulationEndTime)
        {
            //_currentTime = simulationStartTime;
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
            while (eventQueue.Count > 0)
            {
                var firstEntry = eventQueue.First();
                DateTime eventTime = firstEntry.Key;
                SimFactory.Instance.currentTime = eventTime;

                // 이벤트 실행
                foreach (var action in firstEntry.Value)
                {
                    action?.Invoke();
                }
                // 실행 끝난 이벤트 제거
                eventQueue.Remove(eventTime);
            }
        }
    }
}
