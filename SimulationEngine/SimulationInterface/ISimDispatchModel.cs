using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimDispatchModel
    {
        bool FilterEqp(SimEquipment equipment, DateTime currentTime);
        List<SimLot> FilterLot(SimEquipment equipment, List<SimLot> possibleLots);
        SimLot SelectLot(SimEquipment equipment, List<SimLot> selectedLot);
        //void OnDispatched(SimLot lot, SimEquipment equipment); 
    }
}
