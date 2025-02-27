using SimulationEngine.SimulationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationLog
{
    public class DispatchLog
    {
        public string EqpId { get; set; }
        public string StepId { get; set; }
        public DateTime DispatchingTime { get; set; }
        public SimLot SelectedLot { get; set; }
        public List<SimLot> CandidateLots { get; set; }      
        public List<SimLot> FilterLots { get; set; }
    }
}
