using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
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
    public class PhotoRouteModel : ISimRouteModel
    {
        public Step GetNextStep(SimLot lot)
        {
            return lot.GetLot().Process.GetNextStep(lot.StepId); 
        }

        public void OnStepDone(SimLot lot)
        {
            //throw new NotImplementedException();
        }

        public void OnLotDone(SimLot lot)
        {
            //throw new NotImplementedException();
        }

        public void OnProcessed(SimLot lot, SimEquipment equipment)
        {
            //throw new NotImplementedException();
        }

        public void OnRelease(SimLot lot)
        {
            //throw new NotImplementedException();
        }

        public void OnProcessIn(SimLot lot, SimEquipment equipment)
        {
            //throw new NotImplementedException();
        }
        public bool IsSetup(SimLot lot, SimEquipment equipment)
        {
            //throw new NotImplementedException();
            return false; 
        }
        public void OnSetupOut(SimEquipment equipment, SimLot lot, EqpSchedule plan)
        {

        }
    }
}
