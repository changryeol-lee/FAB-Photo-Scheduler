using SimulationEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.BaseEntity
{
    public abstract class Lot
    {
        public string LotId { get; set; }
        public string ProductId { get; set; }
        public string ProcessId { get; set; }
        public string StepId { get; set; }
        public string EqpId { get; set; }
        public int LotQty { get; set; }
        public LotState State { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
