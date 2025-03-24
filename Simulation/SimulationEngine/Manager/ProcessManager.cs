using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using SimulationEngine.Schedule;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.Manager
{
    public class ProcessManager
    {
        private readonly ISimProcessModel _model;
        private ScheduleManager _scheduleManager = SimFactory.Instance._scheduleManager;
        private OffTimeManager _offTimeManager = SimFactory.Instance._offTimeManager;

        private Dictionary<string, EqpSchedule> _schedules = new Dictionary<string, EqpSchedule>();
        private readonly Dictionary<string, Step> _simSteps = new Dictionary<string, Step>();
        private readonly Dictionary<string, Process> _simProcesses = new Dictionary<string, Process>();
        private readonly Dictionary<string, Product> _simProducts = new Dictionary<string, Product>();

        internal ProcessManager(ISimProcessModel model)
        {
            _model = model;
        }

        internal void SimBomInit()
        {

            IEnumerable<Step> steps = _model.GetSteps();

            foreach (var step in steps)
            {
                _simSteps.Add(step.StepId, step);
            }
            IEnumerable<Process> processes = _model.GetProcesses(steps);

            foreach (var process in processes)
            {
                _simProcesses.Add(process.ProcessId, process);
            }
            IEnumerable<Product> products = _model.GetProducts(processes);

            foreach (var product in products)
            {
                _simProducts.Add(product.ProductId, product);
            }
        }

        internal void ProcessSetup(SimEquipment equipment, SimLot lot, DateTime startTime)
        {
            double setupTime = _model.GetSetupTime(equipment, lot);

            equipment.GetEquipment().State = EqpState.SETUP;
            DateTime finishTime = startTime.AddMinutes(setupTime);

            double processDuration = setupTime;
            if (_offTimeManager.IsOffTimeRange(startTime, finishTime))
            {
                //offTime 반영한 finish 조정
                DateTime adjustedFinishTime = _offTimeManager.GetAdjustedEndTime(startTime, finishTime);
                TimeSpan gap = adjustedFinishTime - finishTime;
                //off 제외한 순수 작업시간 (이론상 procTime과 동일해야함) 
                processDuration = (adjustedFinishTime - startTime - gap).TotalMinutes;
                finishTime = adjustedFinishTime;
            }

            string scheduleId = equipment.EqpId + "-" + Utils.GenerateRandom8Digits();
            EqpSchedule schedule = new EqpSchedule
            {
                ScheduleId = scheduleId,
                EqpId = equipment.EqpId,
                ProductId = lot.ProductId,
                ProcessId = lot.ProcessId,
                StartTime = startTime,
                EndTime = finishTime,
                ProcessDuration = processDuration,
                WorkType = WorkType.SETUP
            };
            equipment.SetCurrentPlan(schedule);

            _scheduleManager.AddEvent(finishTime, () =>
            {
                SimFactory.Instance._routeManager.SetupOut(lot, equipment, finishTime);
            });
        }


        internal void Process(SimEquipment equipment, SimLot lot, DateTime startTime)
        {
            TrackIn(equipment, lot, startTime);

            double procTime = _model.GetProcessTime(equipment, lot);
            DateTime finishTime = startTime.AddSeconds(procTime);
            double processDuration = procTime; 

            if (_offTimeManager.IsOffTimeRange(startTime, finishTime))
            {
                //offTime 반영한 finish 조정
                DateTime adjustedFinishTime = _offTimeManager.GetAdjustedEndTime(startTime, finishTime);
                TimeSpan gap = adjustedFinishTime - finishTime;
                //off 제외한 순수 작업시간 (이론상 procTime과 동일해야함) 
                processDuration = (adjustedFinishTime - startTime - gap).TotalMinutes;
                finishTime = adjustedFinishTime;
            }

            _scheduleManager.AddEvent(finishTime, () =>
            {
                TrackOut(equipment, lot, finishTime);
                
                SimFactory.Instance._routeManager.Processed(lot, equipment, finishTime, processDuration);
            });
        }

        internal void TrackIn(SimEquipment equipment, SimLot lot, DateTime startTime)
        {
            equipment.GetEquipment().State = EqpState.BUSY;
            lot.GetLot().State = LotState.RUN;

            string scheduleId = equipment.EqpId + "-" + Utils.GenerateRandom8Digits();
            double waitMinutes = (startTime - lot.GetLot().ArrivalTime).TotalMinutes;
            EqpSchedule schedule = new EqpSchedule
            {
                ScheduleId = scheduleId,
                EqpId = equipment.EqpId,
                ProductId = lot.ProductId,
                ProcessId = lot.ProcessId,
                LotId = lot.LotId,
                StepId = lot.StepId,
                StartTime = startTime,
                LotQty = lot.GetLot().LotQty,
                WaitDuration = waitMinutes,
                WorkType = WorkType.PLAN
            };
            equipment.SetCurrentPlan(schedule);
            _schedules.Add(scheduleId, schedule);
            _model.OnTrackIn(equipment, lot);
        }

        internal double GetProcessTime(SimEquipment equipment, SimLot lot)
        {
            return _model.GetProcessTime(equipment, lot);
        }

        internal void TrackOut(SimEquipment equipment, SimLot lot, DateTime finishTime)
        {
            equipment.GetEquipment().State = EqpState.IDLE;
            lot.GetLot().State = LotState.WAIT;

            _model.OnTrackOut(equipment, lot);
        }


        //유일하게 모델의 GetLot에 직접 노출되는 함수 
        public Step GetStep(string stepId)
        {
            return _simSteps.TryGetValue(stepId, out var step) ? step : null;
        }
        public Process GetProcess(string processId)
        {
            return _simProcesses.TryGetValue(processId, out var process) ? process : null;
        }
        public Product GetProduct(string productId)
        {
            return _simProducts.TryGetValue(productId, out var product) ? product : null;
        }
    }
}
