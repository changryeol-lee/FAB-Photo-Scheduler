using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.InputEntity
{
    public class PhotoLot : Lot
    {
        public Step initStep { get; set; }
        public PhotoLot(int lotQty = 10)
        {
            State = LotState.WAIT;
            LotQty = lotQty;
            initStep = null; 
        }
        public PhotoLot(Step initStep, Equipment equipment, LotState state)
        {
            this.initStep = initStep;
        }
    }
}
