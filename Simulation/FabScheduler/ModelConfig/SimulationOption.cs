using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.ModelConfig
{
    public class PhotoSimulationOption : SimulationOption
    {
        public DispatchType DispatchType { get; set; }
        public string RunUser { get; set; }
        public int SimulationPeriod { get; set; }
    }
}
