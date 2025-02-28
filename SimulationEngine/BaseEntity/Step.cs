using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.BaseEntity
{
    public abstract class Step
    {
        public string StepId { get; set; }
        public int StepSeq { get; set; }
    }
}
