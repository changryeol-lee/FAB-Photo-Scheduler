using SimulationEngine.BaseEntity;
using SimulationEngine.ProcessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface ISimOffTimeModel
    {
        List<OffTimeRule> GetOffTimeRules();
        void WriteOffTimeLog(List<(DateTime Start, DateTime End)> offTimeList);
    }
}
