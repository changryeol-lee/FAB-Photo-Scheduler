using DataMart.Output;
using DataMart.SqlMapper;
using FabSchedulerModel.ModelConfig;
using SimulationEngine.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.Helper
{
    public class OutputHelper
    {
        public static void WriteEngineExecuteLog(PhotoSimulationOption option)
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

        public static void WriteEqpSchedule(EqpSchedule plan)
        {
            EQP_SCHEDULE es = new EQP_SCHEDULE();
            es.SIMULATION_VERSION = InputMart.Instance.SimulationVersion;
            es.SCHEDULE_ID = plan.ScheduleId;
            es.EQP_ID = plan.EqpId;
            es.PRODUCT_ID = plan.ProductId;
            es.LOT_ID = plan.LotId;
            es.LOT_QTY = plan.LotQty;
            es.STEP_ID = plan.StepId;
            es.START_TIME = plan.StartTime;
            es.END_TIME = plan.EndTime;
            es.PROCESS_DURATION = plan.ProcessDuration;
            es.WAIT_DURATION = plan.WaitDuration;
            es.WORK_TYPE = plan.WorkType.ToString();
            OutputMart.Instance.AddData(OutputTable.EQP_SCHEDULE, es);
        }
    }
}
