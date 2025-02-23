using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
using SimulationEngine.SimulationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationObject
{
    public class SimEquipment
    {
        private readonly Equipment _equipment;
        private readonly List<DispatchInfo> _dispatchHistory;
        private LoadInfo _previousPlan;

        public string EqpId => _equipment.EqpId;
        public DateTime NowDT { get; set; }

        public SimEquipment(Equipment equipment)
        {
            _equipment = equipment;
            _dispatchHistory = new List<DispatchInfo>();
        }

        public void AddDispatchInfo(List<SimLot> candidates, SimLot selected, DateTime dispatchTime)
        {
            var info = new DispatchInfo
            {
                EqpId = this.EqpId,
                DispatchingTime = dispatchTime,
                SelectInfo = selected?.LotId,
                CandidateLots = candidates
            };

            _dispatchHistory.Add(info);
        }

        public LoadInfo GetPreviousPlan()
        {
            return _previousPlan;
        }

        public void SetPreviousPlan(LoadInfo plan)
        {
            _previousPlan = plan;
        }

        public Equipment GetEquipment() => _equipment;
    }
}
