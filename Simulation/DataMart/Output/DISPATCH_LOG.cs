using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMart.Output
{
    public class DISPATCH_LOG
    {
        public string SIMULATION_VERSION { get; set; }  
        public string EQP_ID { get; set; }
        public string STEP_ID { get; set; }
        public DateTime DISPATCHING_TIME { get; set; }
        public string LOT_ID { get; set; }
        public string CANDIDATE_LOTS { get; set; }
        public string PASSED_LOTS { get; set; }
        public string EXCLUDE_LOTS { get; set; }
        public string DISPATCH_INFO { get; set; }
    }
}
