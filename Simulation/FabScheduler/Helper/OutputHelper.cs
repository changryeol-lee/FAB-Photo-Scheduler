using DataMart.Output;
using DataMart.SqlMapper;
using FabSchedulerModel.InputEntity;
using FabSchedulerModel.ModelConfig;
using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using SimulationEngine.SimulationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FabSchedulerModel.Helper
{
    public class OutputHelper
    {
        private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());
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

        public static void WriteEqpSchedule(EqpSchedule plan, SimLot lot)
        {
            EQP_SCHEDULE es = new EQP_SCHEDULE();
            es.SIMULATION_VERSION = InputMart.Instance.SimulationVersion;
            es.SCHEDULE_ID = plan.ScheduleId;
            es.EQP_ID = plan.EqpId;
            es.PRODUCT_ID = plan.ProductId;
            es.PROCESS_ID =  plan.ProcessId;
            es.LOT_ID = plan.LotId;
            es.LOT_QTY = plan.LotQty;
            es.STEP_ID = plan.StepId;
            es.START_TIME = plan.StartTime;
            es.END_TIME = plan.EndTime;
            es.TOTAL_PROCESS_DURATION = plan.TotalProcessDuration;
            es.PROCESS_DURATION = plan.ProcessDuration;
            es.WAIT_DURATION = plan.WaitDuration;

            string workType;
            if ((lot.GetLot() as PhotoLot).IsRework && plan.WorkType == WorkType.PLAN)
            {
                workType = "REWORK";
            }
            else
            {
                workType = plan.WorkType.ToString();
            }

            es.WORK_TYPE = workType;
            es.IS_DONE = lot.IsDone ? "Y" : "N"; 
            OutputMart.Instance.AddData(OutputTable.EQP_SCHEDULE, es);
        }
        public static string GenerateRandom8Digits()
        {
            return _random.Value.Next(10000000, 99999999).ToString();
        }
    }
}
