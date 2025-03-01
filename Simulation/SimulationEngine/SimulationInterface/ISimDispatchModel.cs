using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationLog;
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
        void FilterLot(SimEquipment equipment, List<SimLot> candidateLots, out List<SimLot> passedLots, out List<SimLot> excludedLots);
        SimLot SelectLot(SimEquipment equipment, List<SimLot> selectedLot);
        void WriteDispatchLog(DispatchLog log);
        //void OnDispatched(SimLot lot, SimEquipment equipment); 
    }
}
