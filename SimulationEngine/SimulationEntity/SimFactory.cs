using SimulationEngine.Agents;
using SimulationEngine.Schedule;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationEntity
{
    public class SimFactory
    {
        private ScheduleManager _scheduleManager;
        private EqpManager _eqpManager;
        private ISimulationModel _model;

        public DateTime _simulationStartTime;
        public DateTime _simulationEndTime;
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
            _model = model.SimulationModel;
            _simulationStartTime = startTime;
            _simulationEndTime = endTime;
            _scheduleManager = new ScheduleManager(_simulationStartTime, _simulationEndTime);
            _eqpManager = new EqpManager(model.EquipmentModel); 
        }

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

        public void Initialize()
        {
            _model.OnBeginInitialize();
            _eqpManager.SimEqpInit(); 
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

            // 실행
            _scheduleManager.Run();
        }
    }
}
