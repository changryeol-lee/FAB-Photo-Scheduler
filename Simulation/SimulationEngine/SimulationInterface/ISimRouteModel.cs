using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationObject;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimRouteModel
    {
        void OnRelease(SimLot lot);
        //void OnDispatchIn

        //public void OnDispatched(SimLot lot, SimEquipment equipment);
        bool IsSetup(SimLot lot, SimEquipment equipment);
        void OnProcessIn(SimLot lot, SimEquipment equipment);

        void OnProcessed(SimLot lot, SimEquipment equipment);

        void OnStepDone(SimLot lot, EqpSchedule plan);

        Step GetNextStep(SimLot lot);
        void OnLotDone(SimLot lot, EqpSchedule plan);
        void OnSetupOut(SimEquipment equipment, SimLot lot, EqpSchedule plan);
        double GetTransferTime(SimLot lot, Step step, Step nextStep);
    }
}
