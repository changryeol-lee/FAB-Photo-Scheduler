using SimulationEngine.BaseEntity;
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
    public class RouteManager
    {
        private readonly ISimRouteModel _model;
        private DateTime _currentTime = SimFactory.Instance.currentTime;
        private ScheduleManager _scheduleManager = SimFactory.Instance._scheduleManager;
        private DispatchManager _dispatchManager = SimFactory.Instance._dispatchManager;
        private ProcessManager _processManager = SimFactory.Instance._processManager;

        // Release 시각별로 Lots를 묶어 저장
        private Dictionary<DateTime, List<SimLot>> _batchReleaseDict = new Dictionary<DateTime, List<SimLot>>();


        public RouteManager(ISimRouteModel model)
        {
            _model = model;
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

        public void Release(SimLot lot)
        {
            Lot actualLot = lot.GetLot();
            actualLot.ArrivalTime = _currentTime;
            _dispatchManager.AddWaitingLot(lot);
            _dispatchManager.AddToDispatchLot(lot); 
            _model.OnRelease(lot);
            // Lot이 추가되었으므로 Dispatching 시도
            _scheduleManager.AddEvent(_currentTime, () => DispatchIn());
        }
        public void Release(List<SimLot> relaseLots)
        {
            foreach (SimLot lot in relaseLots)
            {
                Lot actualLot = lot.GetLot();
                actualLot.ArrivalTime = _currentTime;
                _dispatchManager.AddWaitingLot(lot);
                _dispatchManager.AddToDispatchLot(lot);
                _model.OnRelease(lot);
            }

            // Lot이 추가되었으므로 Dispatching 시도
            _scheduleManager.AddEvent(_currentTime, () => DispatchIn());
        }


        public void DispatchIn()
        {
            _scheduleManager.AddEvent(_currentTime, () => _dispatchManager.Dispatch());
        }

        public void Dispatched(SimLot selectedLot, SimEquipment equipment)
        {
            _scheduleManager.AddEvent(_currentTime, () => StartTask(selectedLot, equipment, _currentTime));
            //_model.OnDispatched(lot, equipment);
        }

        public void StartTask(SimLot lot, SimEquipment equipment, DateTime currentTime)
        {
            _model.OnStartTask(lot, equipment);
            _scheduleManager.AddEvent(currentTime, () => _processManager.TrackIn(equipment, lot));
        }

        public void EndTask(SimLot lot, SimEquipment equipment, DateTime currentTime)
        {
            _model.OnEndTask(lot, equipment);

            string nextStep = _model.GetNextStep(lot);
            if (string.IsNullOrEmpty(nextStep))
            {
                _model.OnDone(lot);
            }
            else
            {
                DateTime nextStepTime = currentTime.AddHours(1);  // 다음 공정 이동 시간 (기본 1시간 후)
                _scheduleManager.AddEvent(nextStepTime, () => Release(lot));
            }
        }

        public bool IsDone(SimLot lot)
        {
            return _model.IsDone(lot);
        }

        public string GetNextStep(SimLot lot)
        {
            return _model.GetNextStep(lot);
        }
    }
}
