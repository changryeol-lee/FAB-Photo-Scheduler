using SimulationEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.ProcessEntity
{
    public class EqpSchedule
    {
        public string ScheduleId { get; set; }
        public string EqpId { get; set; }
        public string ProductId { get; set; }
        public string LotId { get; set; }
        public int LotQty { get; set; }
        public string StepId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // 공정 진행시간(분)
        public double ProcessDuration => (EndTime - StartTime).TotalMinutes;
        // Lot이 실제로 공정 대기를 얼마나 했는지
        public double? WaitDuration { get; set; }
        public WorkType WorkType { get; set; }
    }
}
