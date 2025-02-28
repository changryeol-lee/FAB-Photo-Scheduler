using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationEntity
{
    public class SimLot
    {
        private readonly Lot _lot;
        public List<LoadInfo> Plans { get; }
        public LoadInfo CurrentPlan { get; set; }
        public LoadInfo PreviousPlan { get; set; }

        public string ProductId => _lot.Product.ProductId; 
        public string ProcessId => _lot.Process.ProcessId;
        public string StepId => _lot.Step.StepId; 
        public string EqpId { get; set; }

        public string LotId => _lot.LotId;
        //public DateTime ArrivalTime => _lot.ArrivalTime;

        public SimLot(Lot lot)
        {
            _lot = lot;
            Plans = new List<LoadInfo>();
        }

        public Lot GetLot() => _lot;

        public double GetFactorValue()
        {
            // 작업물의 factor 값 계산 로직
            return 0.0;
        }
    }
}
