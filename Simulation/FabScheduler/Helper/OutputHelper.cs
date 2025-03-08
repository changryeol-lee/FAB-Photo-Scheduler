using DataMart.Output;
using DataMart.SqlMapper;
using FabSchedulerModel.ModelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.Helper
{
    public class OutputHelper
    {
        public static void WriteEngineExecuteLog(SimulationOption option)
        {
            ENGINE_EXECUTE_LOG el = new ENGINE_EXECUTE_LOG();
            el.SIMULATION_VERSION = InputMart.Instance.SimulationVersion;
            el.DISPATCH_TYPE = option.DispatchType.ToString();
            el.SIMULATION_START_TIME = option.SimulationStartTime;
            el.SIMULATION_END_TIME = option.SimulationEndTime;
            el.RUN_USER = option.RunUser;
            el.SIMULATION_EXECUTE_TIME = DateTime.Now;
            OutputMart.Instance.AddData(OutputTable.ENGINE_EXECUTE_LOG, el);
        }
    }
}
