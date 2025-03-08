using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.ModelConfig
{
    public class SimulationOption
    {
        public DateTime SimulationStartTime { get; set; }
        public DateTime SimulationEndTime { get; set; }
        public DispatchType DispatchType { get; set; }
        public string RunUser { get; set; }

    }
}
