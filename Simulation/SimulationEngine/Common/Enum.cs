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

    public enum OffTimeRuleType
    {
        Daily,        // 요일과 무관하게 매일 특정 시간
        Weekly,       // 특정 요일+시간 (예: 월수금 13:00~14:00)
        Once          // 단발성 (DateTime Start~End)
    }
}
