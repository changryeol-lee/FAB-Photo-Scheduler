using FabSchedulerModel.InputEntity;
using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel
{
    public class PhotoModel : ISimEquipmentModel
    {
        public List<Equipment> GetEqpList()
        {
            throw new NotImplementedException();
        }

        public List<SimEquipment> GetLoadableEqpList(SimLot lot)
        {
            throw new NotImplementedException();
        }
    }
}
