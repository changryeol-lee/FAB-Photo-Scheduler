using SimulationEngine.Common;
using SimulationEngine.ProcessEntity;
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
        private Dictionary<string, EqpSchedule> _schedules = new Dictionary<string, EqpSchedule>();

        public ProcessManager(ISimProcessModel model)
        {
            _model = model;
        }

        public void Process(SimEquipment equipment, SimLot lot, DateTime startTime)
        {
            TrackIn(equipment, lot, startTime);


            double procTime = _model.GetProcessTime(equipment, lot);

            DateTime finishTime = startTime.AddHours(procTime);
            equipment.GetEquipment().State = EqpState.BUSY; 

            _scheduleManager.AddEvent(finishTime, () =>
            {
                TrackOut(equipment, lot, finishTime);
                
                SimFactory.Instance._routeManager.Processed(lot, equipment, finishTime);
            });
        }

        public void TrackIn(SimEquipment equipment, SimLot lot, DateTime startTime)
        {
            equipment.GetEquipment().State = EqpState.BUSY;
            lot.GetLot().State = LotState.RUN;

            string scheduleId = equipment.EqpId + "-" + Utils.GenerateRandom8Digits();

            EqpSchedule schedule = new EqpSchedule
            {
                ScheduleId = scheduleId,
                EqpId = equipment.EqpId,
                ProductId = lot.GetLot().ProductId,
                LotId = lot.LotId,
                StepId = lot.GetLot().StepId,
                StartTime = startTime
            };
            equipment.SetCurrentPlan(schedule);
            _schedules.Add(scheduleId, schedule);
            _model.OnTrackIn(equipment, lot);
        }

        public double GetProcessTime(SimEquipment equipment, SimLot lot)
        {
            return _model.GetProcessTime(equipment, lot);
        }

        public void TrackOut(SimEquipment equipment, SimLot lot, DateTime finishiTime)
        {
            equipment.GetEquipment().State = EqpState.IDLE;
            lot.GetLot().State = LotState.WAIT;
            EqpSchedule plan =  equipment.GetCurrentPlan();
            plan.EndTime = finishiTime;
            equipment.SetPreviousPlan(plan);
            equipment.SetCurrentPlan(null);
            _model.OnTrackOut(equipment, lot, plan);
        }
    }
}
