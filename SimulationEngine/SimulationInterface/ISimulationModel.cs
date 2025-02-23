using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimulationModel
    {
        void OnBeginInitialize();
        void OnEndInitialize();
        void OnStart();
        void OnDone();
        void OnDayChanged(DateTime currentDate);

    }
}
