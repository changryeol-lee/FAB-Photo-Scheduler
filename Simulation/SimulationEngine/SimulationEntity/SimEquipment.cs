using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationLog;
using System;
using System.Collections.Generic;

namespace SimulationEngine.SimulationObject
{
    public class SimEquipment
    {
        private readonly Equipment _equipment;
        private readonly List<DispatchLog> _dispatchHistory;
        private EqpSchedule _previousPlan;
        private EqpSchedule _currentPlan;

        public string EqpId => _equipment.EqpId;
        public DateTime NowDT { get; set; }

        public SimEquipment(Equipment equipment)
        {
            _equipment = equipment;
            _dispatchHistory = new List<DispatchLog>();
        }

        public void AddDispatchHistory(DispatchLog dispatchLog)
        {
            _dispatchHistory.Add(dispatchLog);
        }

        public EqpSchedule GetPreviousPlan()
        {
            return _previousPlan;
        }

        public void SetPreviousPlan(EqpSchedule plan)
        {
            _previousPlan = plan;
        }

        public EqpSchedule GetCurrentPlan()
        {
            return _currentPlan;
        }

        public void SetCurrentPlan(EqpSchedule plan)
        {
            _currentPlan = plan;
        }

        public Equipment GetEquipment() => _equipment;
    }
}
