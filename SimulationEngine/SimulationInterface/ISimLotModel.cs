using SimulationEngine.BaseEntity;
using SimulationEngine.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimLotModel
    {
        IEnumerable<Lot> GetLots(ProcessManager processManager);
        //bool CheckLotHold(Lot lot);
        //TimeSpan CalculateHoldTime(Lot lot);
    }
}
