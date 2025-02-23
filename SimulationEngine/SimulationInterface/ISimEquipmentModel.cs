using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimEquipmentModel
    {
        List<Equipment> GetEqpList();
        List<SimEquipment> GetLoadableEqpList(SimLot lot);
    }
}
