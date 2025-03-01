using SimulationEngine.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.InputEntity
{
    public class PhotoEquipment : Equipment 
    {
        public DateTime nextPMTime; 
        //public double PreventDispatchingThreshold { get; set; } = 30; // minutes
    }
}
