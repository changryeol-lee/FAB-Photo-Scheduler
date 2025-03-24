﻿using SimulationEngine.BaseEntity;
using SimulationEngine.Schedule;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;

namespace SimulationEngine.Manager
{
    public class RouteManager
    {
        private readonly ISimRouteModel _model;
        private DateTime _currentTime => SimFactory.Instance.currentTime;
        private ScheduleManager _scheduleManager;
        private DispatchManager _dispatchManager;
        private ProcessManager _processManager;

        // Release 시각별로 Lots를 묶어 저장
        private Dictionary<DateTime, List<SimLot>> _batchReleaseDict = new Dictionary<DateTime, List<SimLot>>();


        public RouteManager(ISimRouteModel model)
        {
            _model = model;
            _scheduleManager = SimFactory.Instance._scheduleManager;
            _dispatchManager = SimFactory.Instance._dispatchManager;
            _processManager = SimFactory.Instance._processManager;
        }

        public void AddPendingRelease(DateTime time, SimLot lot)
        {
            if (!_batchReleaseDict.ContainsKey(time))
                _batchReleaseDict[time] = new List<SimLot>();

            _batchReleaseDict[time].Add(lot);

            // 시각 time에 ReleaseBatch 이벤트가 이미 예약되어 있지 않다면 예약
            // (중복 예약 방지)
            if (_batchReleaseDict[time].Count == 1)
            {
                _scheduleManager.AddEvent(time, () => ReleaseBatch(time));
            }
        }

        private void ReleaseBatch(DateTime time)
        {
            if (!_batchReleaseDict.ContainsKey(time)) return;

            List<SimLot> releaseLots = _batchReleaseDict[time];
            _batchReleaseDict.Remove(time); // 한번 사용 후 제거

            // 한꺼번에 Release
            Release(releaseLots);
        }

        public void Release(List<SimLot> relaseLots)
        {
            foreach (SimLot lot in relaseLots)
            {
                Lot actualLot = lot.GetLot();
                if (actualLot.Step == null)
                {
                    actualLot.Step = actualLot.Process.GetFirstStep();
                }

                actualLot.ArrivalTime = _currentTime;
                _dispatchManager.AddWaitingLot(lot);
                _dispatchManager.AddToDispatchLot(lot);
                _model.OnRelease(lot);
            }

            // Lot이 추가되었으므로 Dispatching 시도

            if (!_scheduleManager.HasEvent(_currentTime.AddMilliseconds(30), DispatchIn))
            {
                _scheduleManager.AddEvent(_currentTime.AddMilliseconds(30), DispatchIn);
            }
        }


        public void DispatchIn()
        {
            _scheduleManager.AddEvent(_currentTime, () => _dispatchManager.Dispatch());
        }

        public void Dispatched(SimLot selectedLot, SimEquipment equipment)
        {
            if (_model.IsSetup(selectedLot, equipment))
            {
                _scheduleManager.AddEvent(_currentTime, () => SetupIn(selectedLot, equipment, _currentTime));
            }
            else
            {
                _scheduleManager.AddEvent(_currentTime, () => ProcessIn(selectedLot, equipment, _currentTime));
            }

            //_model.OnDispatched(lot, equipment);
        }

        public void SetupIn(SimLot lot, SimEquipment equipment, DateTime currentTime)
        {
            //_model.OnProcessIn(lot, equipment);
            _scheduleManager.AddEvent(currentTime, () =>
            {
                _processManager.ProcessSetup(equipment, lot, currentTime);
            });
        }

        public void SetupOut(SimLot lot, SimEquipment equipment, DateTime finishTime)
        {
            EqpSchedule plan = equipment.GetCurrentPlan();
            plan.EndTime = finishTime;
            equipment.SetPreviousPlan(plan);
            equipment.SetCurrentPlan(null);
            _model.OnSetupOut(equipment, lot, plan);
            _scheduleManager.AddEvent(_currentTime, () => ProcessIn(lot, equipment, finishTime));
        }

        public void ProcessIn(SimLot lot, SimEquipment equipment, DateTime currentTime)
        {
            _model.OnProcessIn(lot, equipment);
            _scheduleManager.AddEvent(currentTime, () =>
            {
                _processManager.Process(equipment, lot, currentTime);
            });

            //이벤트로 등록시, disaptch와 동일 시간인 경우
            //_processManager.Process(equipment, lot, currentTime);
        }

        public void Processed(SimLot lot, SimEquipment equipment, DateTime finishTime, double processDuration)
        {
            EqpSchedule plan = equipment.GetCurrentPlan();
            plan.EndTime = finishTime;
            plan.ProcessDuration = processDuration;
            equipment.SetPreviousPlan(plan);
            equipment.SetCurrentPlan(null);
            _model.OnProcessed(lot, equipment);

            // 다음 공정 이동 여부 판단
            Step nextStep = GetNextStep(lot);

            if (nextStep == null)
            {
                lot.IsDone = true; 
                _model.OnLotDone(lot, plan); 
            }
            else
            {
                _model.OnStepDone(lot, plan);
                Transfer(lot, nextStep, finishTime); 
            }

            //이 시점에 장비가 wait되서, dispatch
            if (!_scheduleManager.HasEvent(_currentTime.AddMilliseconds(30), DispatchIn))
            {
                _scheduleManager.AddEvent(_currentTime.AddMilliseconds(30), DispatchIn);
            }
        }

        public void Transfer(SimLot lot, Step nextStep, DateTime finishTime)
        {
            //단위 초(second)  
            double transferTime = _model.GetTransferTime(lot, lot.GetLot().Step, nextStep);
            lot.GetLot().Step = nextStep;
            DateTime nextStepTime = finishTime.AddSeconds(transferTime);

            //lot의 release 시점 
            AddPendingRelease(nextStepTime, lot);
        }

        //public bool OnStepDone(SimLot lot)
        //{
        //    return _model.OnStepDone(lot);
        //}

        public Step GetNextStep(SimLot lot)
        {
            return _model.GetNextStep(lot);
        }
    }
}
