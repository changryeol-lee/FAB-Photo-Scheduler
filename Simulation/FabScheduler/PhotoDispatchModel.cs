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

namespace FabSchedulerModel
{
    public class PhotoDispatchModel : ISimDispatchModel
    {
        public void FilterLot(SimEquipment equipment, List<SimLot> candidateLots, out List<SimLot> passedLots, out List<SimLot> excludedLots)
        {
            passedLots = new List<SimLot>();
            excludedLots = new List<SimLot>();

            foreach (var lot in candidateLots)
            {
                if (lot.GetLot().State == LotState.WAIT)
                    passedLots.Add(lot);
                else
                    excludedLots.Add(lot);
            }
        }

        public bool FilterEqp(SimEquipment equipment, DateTime currentTime)
        {
            return false;
        }

        //1. optino이 없다면 기본적으로 fifo. 
        //2. min setup인 경우, product id가 동일하면서 가장 빨리 도착한 lot 선택. 
        //2.1 product id가 동일한 lot이 없는 경우, fifo로 진행. 
        public SimLot SelectLot(SimEquipment equipment, List<SimLot> filteredList)
        {
            PhotoSimulationOption option = SimFactory.Instance.option as PhotoSimulationOption;
            if (option.DispatchType == DispatchType.FIFO)
            {
                return filteredList.OrderBy(x => x.GetLot().ArrivalTime).First();
            }
            else if (option.DispatchType == DispatchType.MIN_SETUP && equipment.GetPreviousPlan() != null)
            {
                List<SETUP_INFO> setupInfoList = InputMart.Instance.GetList<SETUP_INFO>(InputTable.SETUP_INFO);
                SETUP_INFO info = setupInfoList.Where(x => x.EQP_ID.Equals(equipment.EqpId)).FirstOrDefault();
                if(info != null && info.SETUP_CONDITION == "PRODUCT_CHANGE") {
                    SimLot lot = filteredList.Where(x => x.ProductId == equipment.GetPreviousPlan().ProductId)
                                             .OrderBy(x => x.GetLot().ArrivalTime).FirstOrDefault();
                    if (lot != null)
                        return lot;
                }
            }
            return filteredList.OrderBy(x => x.GetLot().ArrivalTime).First();
        }

        public void WriteDispatchLog(DispatchLog log)
        {
            DISPATCH_LOG dl = new DISPATCH_LOG();
            dl.SIMULATION_VERSION = InputMart.Instance.SimulationVersion;
            dl.EQP_ID = log.EqpId;
            dl.STEP_ID = log.StepId;
            dl.DISPATCHING_TIME = log.DispatchingTime;
            dl.LOT_ID = log.SelectedLot.LotId;
            dl.CANDIDATE_LOTS = string.Join(";", log.CandidateLots.Select(x => x.LotId));
            dl.PASSED_LOTS = string.Join(";", log.PassedLots.Select(x => x.LotId));
            dl.EXCLUDE_LOTS = string.Join(";", log.ExcludedLots.Select(x => x.LotId));
            string dispatchInfo = string.Join(";", log.PassedLots.Select(x => $"{x.GetLot().LotId};{x.GetLot().ArrivalTime:yyyy-MM-dd HH:mm:ss}"));
            dl.DISPATCH_INFO = dispatchInfo;

            OutputMart.Instance.AddData(OutputTable.DISPATCH_LOG, dl);
        }
    }
}