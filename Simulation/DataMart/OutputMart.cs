using DataMart.Input;
using DataMart.Input.Query;
using DataMart.Output;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace DataMart.SqlMapper
{
    public class OutputMart
    {
        private static readonly object lockObject = new object();
        private Dictionary<OutputTable, object> dataLists;
        private readonly TableDataAccess dataAccess;
        private Dictionary<OutputTable, object> tableQueries;
        private readonly string _connectionString;
        private static volatile OutputMart instance;


        public static OutputMart Initialize(string connectionString)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new OutputMart(connectionString);
                    }
                }
            }

            return instance;
        }
        public static OutputMart Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("OutputMart is not initialized. Call Initialize() first.");
                }
                return instance;
            }
        }
        private OutputMart(string connectionString)
        {
            _connectionString = connectionString;
            dataAccess = new TableDataAccess(connectionString);
            dataLists = new Dictionary<OutputTable, object>();
        }

        public void AddData<T>(OutputTable table, T item)
        {
            if (!dataLists.ContainsKey(table))
            {
                dataLists[table] = new List<T>();
            }
            var dataList = dataLists[table] as List<T>;
            dataList.Add(item);
        }


        public void SaveAll()
        {
            foreach (var kvp in dataLists)
            {
                OutputTable table = kvp.Key;
                object listObj = kvp.Value;

                switch(table)
                {
                    case OutputTable.DISPATCH_LOG:
                        var dispatchLogList = listObj as List<DISPATCH_LOG>;
                        string insertDispatchLogSql =
                            "INSERT INTO TB_DISPATCH_LOG (SIMULATION_VERSION, EQP_ID, STEP_ID, DISPATCHING_TIME, LOT_ID, " +
                            "CANDIDATE_LOTS, PASSED_LOTS, EXCLUDE_LOTS, DISPATCH_INFO) " +
                            "VALUES (@SimulationVersion, @EqpId, @StepId, @DispatchingTime, @LotId, " +
                            "@CandidateLots, @PassedLots, @ExcludeLots, @DispatchInfo)";

                        int dispatchRows = dataAccess.InsertRows(insertDispatchLogSql, dispatchLogList, (cmd, item) =>
                        {
                            cmd.Parameters.AddWithValue("@SimulationVersion", item.SIMULATION_VERSION);
                            cmd.Parameters.AddWithValue("@EqpId", item.EQP_ID);
                            cmd.Parameters.AddWithValue("@StepId", item.STEP_ID);
                            cmd.Parameters.AddWithValue("@DispatchingTime", item.DISPATCHING_TIME);
                            cmd.Parameters.AddWithValue("@LotId", item.LOT_ID);
                            cmd.Parameters.AddWithValue("@CandidateLots", item.CANDIDATE_LOTS);
                            cmd.Parameters.AddWithValue("@PassedLots", item.PASSED_LOTS);
                            cmd.Parameters.AddWithValue("@ExcludeLots", item.EXCLUDE_LOTS);
                            cmd.Parameters.AddWithValue("@DispatchInfo", item.DISPATCH_INFO);
                        });
                        break;
                    case OutputTable.EQP_SCHEDULE:
                        var scheduleList = listObj as List<EQP_SCHEDULE>;
                        string insertEqpScheduleSql =
                            "INSERT INTO TB_EQP_SCHEDULE (SIMULATION_VERSION, SCHEDULE_ID, EQP_ID, PRODUCT_ID, LOT_ID, LOT_QTY, STEP_ID, START_TIME, END_TIME, " +
                            "TOTAL_PROCESS_DURATION, PROCESS_DURATION, WAIT_DURATION, WORK_TYPE) " +
                            "VALUES (@SimulationVersion, @ScheduleId, @EqpId, @ProductId, @LotId, @LotQty, @StepId, @StartTime, @EndTime, " +
                            "@TotalProcessDuration, @ProcessDuration, @WaitDuration, @WorkType)";

                        int eqpRows = dataAccess.InsertRows(insertEqpScheduleSql, scheduleList, (cmd, item) =>
                        {
                            cmd.Parameters.AddWithValue("@SimulationVersion", item.SIMULATION_VERSION);
                            cmd.Parameters.AddWithValue("@ScheduleId", item.SCHEDULE_ID);
                            cmd.Parameters.AddWithValue("@EqpId", item.EQP_ID);
                            cmd.Parameters.AddWithValue("@ProductId", item.PRODUCT_ID);
                            cmd.Parameters.AddWithValue("@LotId", item.LOT_ID);
                            cmd.Parameters.AddWithValue("@LotQty", item.LOT_QTY);
                            cmd.Parameters.AddWithValue("@StepId", item.STEP_ID);
                            cmd.Parameters.AddWithValue("@StartTime", item.START_TIME);
                            cmd.Parameters.AddWithValue("@EndTime", item.END_TIME);
                            cmd.Parameters.AddWithValue("@TotalProcessDuration", item.TOTAL_PROCESS_DURATION);
                            cmd.Parameters.AddWithValue("@ProcessDuration", item.PROCESS_DURATION);
                            cmd.Parameters.AddWithValue("@WaitDuration", item.WAIT_DURATION);
                            cmd.Parameters.AddWithValue("@WorkType", item.WORK_TYPE);
       
                        });
                        break;
                    case OutputTable.ENGINE_EXECUTE_LOG:
                        var engineExecuteLogList = listObj as List<ENGINE_EXECUTE_LOG>;
                        string insertEngineExecuteSql =
                            "INSERT INTO TB_ENGINE_EXECUTE_LOG (SIMULATION_VERSION, DISPATCH_TYPE, SIMULATION_START_TIME, SIMULATION_END_TIME, RUN_USER, SIMULATION_EXECUTE_TIME)" +
                            "VALUES (@SimulationVersion, @DispatchType, @SimulationStartTime, @SimulationEndTime, @RunUser, @SimulationExecuteTime)";

                        int executeRows = dataAccess.InsertRows(insertEngineExecuteSql, engineExecuteLogList, (cmd, item) =>
                        {
                            cmd.Parameters.AddWithValue("@SimulationVersion", item.SIMULATION_VERSION);
                            cmd.Parameters.AddWithValue("@DispatchType", item.DISPATCH_TYPE);
                            cmd.Parameters.AddWithValue("@SimulationStartTime", item.SIMULATION_START_TIME);
                            cmd.Parameters.AddWithValue("@SimulationEndTime", item.SIMULATION_END_TIME);
                            cmd.Parameters.AddWithValue("@RunUser", item.RUN_USER);
                            cmd.Parameters.AddWithValue("@SimulationExecuteTime", item.SIMULATION_EXECUTE_TIME);
                        });
                        break;


                }
            }
        }

        public void ClearData()
        {
            dataLists.Clear();
        }
    }
}
