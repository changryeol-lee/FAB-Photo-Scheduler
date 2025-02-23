using SimulationEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.BaseEntity
{
    public abstract class Equipment
    {
        public string EqpId { get; set; }
        public EqpState State { get; set; }
        //public double PreventDispatchingThreshold { get; set; } = 30; // minutes

        //private DateTime? _nextPMTime;
        //public DateTime? GetNextPMTime() => _nextPMTime;
        //public void SetNextPMTime(DateTime time) => _nextPMTime = time;
    }
}
