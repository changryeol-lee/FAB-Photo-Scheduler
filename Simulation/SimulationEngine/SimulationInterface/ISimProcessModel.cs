using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
