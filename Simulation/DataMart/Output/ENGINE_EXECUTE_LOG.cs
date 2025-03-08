using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMart.Output
{
    public class ENGINE_EXECUTE_LOG
    {
        public string SIMULATION_VERSION { get; set; }
        public string DISPATCH_TYPE { get; set; }
        public DateTime SIMULATION_START_TIME { get; set; }
        public DateTime SIMULATION_END_TIME { get; set; }
        public string RUN_USER { get; set; }
        public DateTime SIMULATION_EXECUTE_TIME { get; set; }
        
    }
}
