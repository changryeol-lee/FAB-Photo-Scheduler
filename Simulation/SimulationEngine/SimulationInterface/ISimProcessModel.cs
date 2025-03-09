using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationObject;
using System.Collections.Generic;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimProcessModel
    {
        void OnTrackIn(SimEquipment equipment, SimLot lot);
        double GetProcessTime(SimEquipment equipment, SimLot lot);
        void OnTrackOut(SimEquipment equipment, SimLot lot, EqpSchedule plan);
        IEnumerable<Step> GetSteps();
        IEnumerable<Process> GetProcesses(IEnumerable<Step> steps);
        IEnumerable<Product> GetProducts(IEnumerable<Process> processes);
        double GetSetupTime(SimEquipment equipment, SimLot lot);
    }
}
