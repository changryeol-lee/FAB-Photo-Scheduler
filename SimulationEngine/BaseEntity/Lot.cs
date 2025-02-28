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
        public Product Product { get; set; }

        public Process Process { get; set; }
        public Step Step { get; set; }

        public int LotQty { get; set; }
        public LotState State { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
