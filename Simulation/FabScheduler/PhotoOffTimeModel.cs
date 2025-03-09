using DataMart.Input;
using DataMart.Output;
using DataMart.SqlMapper;
using FabSchedulerModel.ModelConfig;
using SimulationEngine.Common;
using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationLog;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using SimulationEngine.ProcessEntity;
using FabSchedulerModel.Helper;

namespace FabSchedulerModel
{
    public class PhotoOffTimeModel : ISimOffTimeModel
    {

        //
        //    new OffTimeRule
        //        {
        //            RuleType = OffTimeRuleType.Weekly,
        //            Days = new[] { DayOfWeek.Sunday
        //},
        //            StartTime = TimeSpan.FromHours(0),
        //            EndTime   = TimeSpan.FromHours(24)
        //        }

        public List<OffTimeRule> GetOffTimeRules()
        {
            return new List<OffTimeRule>
            {
                new OffTimeRule
                {
                    RuleType = OffTimeRuleType.Daily,
                    StartTime = new TimeSpan(13, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0)
                }
            };
        }
        public void WriteOffTimeLog(List<(DateTime Start, DateTime End)> offTimeList)
        {
            List<EQUIPMENT> eqpList = InputMart.Instance.GetList<EQUIPMENT>(InputTable.EQUIPMENT);
            foreach (var eqp in eqpList)
            {
                foreach (var off in offTimeList)
                {
                    EQP_SCHEDULE es = new EQP_SCHEDULE();
                    es.SIMULATION_VERSION = InputMart.Instance.SimulationVersion;
                    string scheduleId = eqp.EQP_ID + "-" + "OFF-" + OutputHelper.GenerateRandom8Digits();
                    es.SCHEDULE_ID = scheduleId;
                    es.EQP_ID = eqp.EQP_ID;
                    es.START_TIME = off.Start;
                    es.END_TIME = off.End;
                    es.WORK_TYPE = WorkType.OFF.ToString();
                    OutputMart.Instance.AddData(OutputTable.EQP_SCHEDULE, es);
                }
            }
        }
    }
}