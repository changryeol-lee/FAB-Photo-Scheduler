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

                if (eventTime > _simulationEndTime) break;

                SimFactory.Instance.currentTime = eventTime;

                // 실행 도중 eventQueue[eventTime]이 변경될 수 있으므로 별도로 보관
                List<Action> actions = new List<Action>(firstEntry.Value);

                // 이벤트 실행
                foreach (var action in actions)
                {
                    action?.Invoke();
                }

                if (eventQueue.TryGetValue(eventTime, out List<Action> newActions) && newActions.Count > actions.Count)
                {
                    eventQueue[eventTime] = newActions.Skip(actions.Count).ToList();
                }
                else
                {
                    eventQueue.Remove(eventTime);
                }
            }
        }

        public bool HasEvent(DateTime time, Action action)
        {
            if (!eventQueue.ContainsKey(time))
                return false;

            // 이미 등록된 이벤트 리스트에서 같은 메서드가 있는지 확인
            return eventQueue[time].Any(existingAction => existingAction.Method == action.Method || // 직접 참조된 메서드
                    (existingAction.Target is Action targetAction && targetAction.Method == action.Method) // 람다식 내부 메서드까지 비교
                    );
        }

    }
}
