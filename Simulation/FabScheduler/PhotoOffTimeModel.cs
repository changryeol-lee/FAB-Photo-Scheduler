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
        public List<OffTimeRule> GetOffTimeRules()
        {

            var offTimeList = InputMart.Instance.GetList<OFFTIME_INFO>(InputTable.OFFTIME_INFO);
            List<OffTimeRule> returnList = new List<OffTimeRule>();
            foreach (var offTime in offTimeList)
            {
                OffTimeRuleType type;
                TimeSpan startTime;
                TimeSpan endTime;

                //rule type은 필수 
                if (Enum.TryParse(offTime.RULE_TYPE, true, out type))
                {
                    OffTimeRule rule = new OffTimeRule();
                    rule.RuleType = type;
                    TimeSpan.TryParse(offTime.START_TIME, out startTime);
                    rule.StartTime = startTime;

                    rule.EndTime = string.IsNullOrEmpty(offTime.END_TIME) ? TimeSpan.Zero
                                        : (offTime.END_TIME == "24:00" ? TimeSpan.FromHours(24) : TimeSpan.ParseExact(offTime.END_TIME, @"hh\:mm", null));
                    rule.Days = offTime?.DAYS_OF_WEEK == null 
                                    ? null
                                    : offTime.DAYS_OF_WEEK
                                        .Split(',')
                                        .Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d))
                                        .ToArray();
                    rule.StartDateTime = offTime.START_DATE_TIME;
                    rule.EndDateTime = offTime.END_DATE_TIME;
                    returnList.Add(rule);
                }
                else
                {
                    continue;
                }
            }
            return returnList;
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