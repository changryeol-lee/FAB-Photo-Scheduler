using SimulationEngine.BaseEntity;
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
    public interface ISimRouteModel
    {
        void OnRelease(SimLot lot);
        //void OnDispatchIn

        //public void OnDispatched(SimLot lot, SimEquipment equipment);

        void OnProcessIn(SimLot lot, SimEquipment equipment);

        void OnProcessed(SimLot lot, SimEquipment equipment);

        void OnStepDone(SimLot lot);

        Step GetNextStep(SimLot lot);
        void OnLotDone(SimLot lot);
    }
}
