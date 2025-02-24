using SimulationEngine.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimLotModel
    {
        List<Lot> GetLots();
        //bool CheckLotHold(Lot lot);
        //TimeSpan CalculateHoldTime(Lot lot);
    }
}
