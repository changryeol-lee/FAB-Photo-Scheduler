using DataMart.Output;
using DataMart.SqlMapper;
using SimulationEngine.Common;
using SimulationEngine.ProcessEntity;
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

        public SimLot SelectLot(SimEquipment equipment, List<SimLot> filteredList)
        {
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
            dl.CANDIDATE_LOTS = string.Join(";", log.CandidateLots.Select(x=>x.LotId));
            dl.PASSED_LOTS = string.Join(";", log.PassedLots.Select(x => x.LotId));
            dl.EXCLUDE_LOTS = string.Join(";", log.ExcludedLots.Select(x => x.LotId));
            string dispatchInfo = string.Join(";", log.PassedLots.Select(x => $"{x.GetLot().LotId};{x.GetLot().ArrivalTime:yyyy-MM-dd HH:mm:ss}"));
            dl.DISPATCH_INFO = dispatchInfo;

            OutputMart.Instance.AddData(OutputTable.DISPATCH_LOG, dl);
        }
    }
}