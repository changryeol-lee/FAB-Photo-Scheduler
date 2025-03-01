using SimulationEngine.Agents;
using SimulationEngine.Manager;
using SimulationEngine.Schedule;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationEntity
{
    public class SimFactory
    {
        internal ScheduleManager _scheduleManager;
        internal EqpManager _eqpManager;
        internal LotManager _lotManager;
        internal RouteManager _routeManager;
        internal DispatchManager _dispatchManager;
        internal ProcessManager _processManager;
        private ISimulationModel _model;

        public DateTime _simulationStartTime;
        public DateTime _simulationEndTime;
        public DateTime currentTime; 
        private static SimFactory _instance;

        public static SimFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("SimFactory is not initialized. Call InitializeInstance first.");
                }
                return _instance;
            }
        }

        private SimFactory(IModelGroup model, DateTime startTime, DateTime endTime)
        {
            currentTime = startTime;
            _simulationStartTime = startTime;
            _simulationEndTime = endTime;
            _model = model.SimulationModel;
        }
        private SimFactory() { }

        public static void InitializeInstance(IModelGroup modelGroup, DateTime startTime, DateTime endTime)
        {
            if (_instance == null)
            {
                _instance = new SimFactory(modelGroup, startTime, endTime);
            }
            else
            {
                throw new InvalidOperationException("SimFactory instance is already initialized.");
            }
        }
        public void InitializeManagers(IModelGroup model)
        {
            _scheduleManager = new ScheduleManager(_simulationStartTime, _simulationEndTime);
            _eqpManager = new EqpManager(model.EquipmentModel);
            _lotManager = new LotManager(model.LotModel);
            _dispatchManager = new DispatchManager(model.DispatchModel);
            _processManager = new ProcessManager(model.ProcessModel);
            _routeManager = new RouteManager(model.RouteModel);
        }

        public void Initialize(IModelGroup modelGroup)
        {
            _model.OnBeginInitialize();
            InitializeManagers(modelGroup);

            _processManager.SimBomInit();
            _eqpManager.SimEqpInit();
            _lotManager.SimLotInit();
            _model.OnEndInitialize();
        }

        public void StartSimulation()
        {
            // OnStart 이벤트 등록 (시뮬레이션 시작 시각)
            _scheduleManager.AddEvent(_simulationStartTime, () => _model.OnStart());

            // OnDayChanged 이벤트 등록 (매일 00:00)
            DateTime nextDayTime = _simulationStartTime.Date.AddDays(1);
            while (nextDayTime <= _simulationEndTime)
            {
                DateTime eventTime = nextDayTime; // new DateTime(nextDayTime.Ticks); // 개별적으로 값을 복사
                _scheduleManager.AddEvent(eventTime, () => _model.OnDayChanged(eventTime));
                nextDayTime = nextDayTime.AddDays(1);
            }

            // OnDone 이벤트 등록 (시뮬레이션 종료 시각)
            _scheduleManager.AddEvent(_simulationEndTime, () => _model.OnDone());

            var initialLots = _lotManager.GetInitialLots();
            _scheduleManager.AddEvent(_simulationStartTime, () =>
            {
                _routeManager.Release(initialLots); 
            });
            // 실행
            _scheduleManager.Run();
        }
    }
}
