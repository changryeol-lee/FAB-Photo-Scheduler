﻿using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using SimulationEngine.Schedule;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationLog;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.Manager
{
    public class DispatchManager
    {
        private readonly ISimDispatchModel _model;
        private DateTime _currentTime => SimFactory.Instance.currentTime;
        private ScheduleManager _scheduleManager;
        private List<SimLot> _waitingLotList = new List<SimLot>();
        private readonly Dictionary<SimEquipment, List<SimLot>> _dispatchLotList;

        public DispatchManager(ISimDispatchModel model)
        {
            _model = model;
            _dispatchLotList = new Dictionary<SimEquipment, List<SimLot>>();
            _scheduleManager = SimFactory.Instance._scheduleManager;
        }

        public void AddWaitingLot(SimLot lot)
        {
            _waitingLotList.Add(lot);
        }

        public void AddToDispatchLot(SimLot lot)
        {
            var availableEquipments = SimFactory.Instance._eqpManager.GetLoadableEqpList(lot);
            if (availableEquipments == null || availableEquipments.Count() == 0)
            {
                Console.WriteLine($"{_currentTime} {lot.LotId} not availabe equip"); 
            }

            foreach (var eqp in availableEquipments)
            {
                if (!_dispatchLotList.ContainsKey(eqp))
                {
                    _dispatchLotList[eqp] = new List<SimLot>();
                }
                _dispatchLotList[eqp].Add(lot);
            }
        }

        public void Dispatch()
        {
            List<SimEquipment> idleEquipments = SimFactory.Instance._eqpManager.GetIdleEqpList();

            if (idleEquipments == null || idleEquipments.Count() == 0)
            {
                Console.WriteLine($"{_currentTime}  not availabe idle equip");
            }


            foreach (var equipment in idleEquipments)
            {
                if (_model.FilterEqp(equipment, _currentTime)) continue;

                if (!_dispatchLotList.ContainsKey(equipment) || _dispatchLotList[equipment].Count == 0) continue;
                
                List<SimLot> passedLots, excludedLots;
                List<SimLot> candidateLots = _dispatchLotList[equipment];
                _model.FilterLot(equipment, candidateLots, out passedLots, out excludedLots);

                SimLot selectedLot = _model.SelectLot(equipment, passedLots);
                if (selectedLot != null)
                {
                    DispatchLog log = WriteDispatchLog(equipment, selectedLot, candidateLots, passedLots, excludedLots);
                    equipment.AddDispatchHistory(log);
                    _model.WriteDispatchLog(log); 
                    RemoveLotFromAllQueues(selectedLot);
                    _waitingLotList.Remove(selectedLot);
                    SimFactory.Instance._routeManager.Dispatched(selectedLot, equipment);
                }
            }
        }

        public DispatchLog WriteDispatchLog(SimEquipment eqp, SimLot lot, List<SimLot> candidateLots, List<SimLot> passedLots, List<SimLot> excludedLots) {
            DispatchLog log = new DispatchLog();
            log.EqpId = eqp.EqpId;
            log.StepId = lot.StepId;
            log.DispatchingTime = _currentTime;
            log.CandidateLots = candidateLots;
            log.PassedLots = passedLots;
            log.ExcludedLots = excludedLots;
            log.SelectedLot = lot;

            return log; 
        }



        public void RemoveLotFromAllQueues(SimLot lot)
        {
            foreach (var queue in _dispatchLotList.Values)
            {
                queue.Remove(lot);
            }
        }
       

        //public void WriteDispatchLog(SimEquipment equipment, List<SimLot> candidates, SimLot selected);
    }
}
