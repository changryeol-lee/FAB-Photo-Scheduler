using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMart.Output
{
    public class EQP_SCHEDULE
    {
        public string SIMULATION_VERSION { get; set; }
        public string SCHEDULE_ID { get; set; }
        public string EQP_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string LOT_ID { get; set; }
        public int LOT_QTY { get; set; }
        public string STEP_ID { get; set; }
        public DateTime START_TIME { get; set; }
        public DateTime END_TIME { get; set; }
        // 공정 진행시간(분)
        public double TOTAL_PROCESS_DURATION { get; set; }
        // OFF를 제외한 공정 진행시간(분) 
        public double PROCESS_DURATION { get; set; }
        // Lot이 실제로 공정 대기를 얼마나 했는지
        public double? WAIT_DURATION { get; set; }
        public string WORK_TYPE { get; set; }
        public string IS_DONE { get; set; }
    }
}
