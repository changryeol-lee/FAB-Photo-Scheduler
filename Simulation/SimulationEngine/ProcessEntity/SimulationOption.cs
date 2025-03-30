using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.ProcessEntity
{
    public class SimulationOption
    {
        public DateTimeOffset SimulationStartTime { get; set; }
        public DateTimeOffset SimulationEndTime { get; set; }
    }
}
