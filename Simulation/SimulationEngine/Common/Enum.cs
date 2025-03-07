using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.Common
{
    public enum EqpState
    {
        IDLE,
        SETUP,
        BUSY,
        DOWN
    }

    public enum LotState
    {
        WAIT,
        RUN
    }
    public enum WorkType
    {
        PLAN,
        REWORK,
        SETUP,
        OFF
    }
}
