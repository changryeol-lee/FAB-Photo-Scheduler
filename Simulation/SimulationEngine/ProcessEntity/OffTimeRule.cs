using SimulationEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.ProcessEntity
{
    public class OffTimeRule
    {
        public OffTimeRuleType RuleType { get; set; }

        // [Daily / Weekly] 시: 해당 시간을 TimeSpan으로 정의
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Weekly일 경우, 적용할 요일들
        public DayOfWeek[] Days { get; set; }

        // Once일 경우, 구체적인 DateTime 범위
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
