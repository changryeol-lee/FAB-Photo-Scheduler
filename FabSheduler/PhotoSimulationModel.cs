using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabShedulerModel
{
    public class PhotoSimulationModel : ISimulationModel
    {
        public void OnBeginInitialize() => Console.WriteLine("[FabModel] OnBeginInitialize triggered.");
        public void OnEndInitialize() => Console.WriteLine("[FabModel] OnEndInitialize triggered.");
        public void OnStart() => Console.WriteLine("[FabModel] OnStart triggered.");
        public void OnDayChanged(DateTime date) => Console.WriteLine($"[FabModel] OnDayChanged triggered for {date.ToShortDateString()}.");
        public void OnDone() => Console.WriteLine("[FabModel] OnDone triggered.");
    }
}
