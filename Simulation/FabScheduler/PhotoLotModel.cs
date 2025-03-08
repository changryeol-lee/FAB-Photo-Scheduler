using DataMart.Input;
using DataMart.SqlMapper;
using FabSchedulerModel.InputEntity;
using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using SimulationEngine.Manager;
using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel
{
    public class PhotoLotModel : ISimLotModel
    {
        public IEnumerable<Lot> GetLots(ProcessManager processManager)
        {
            //List<EQP_ARRANGE> arrangeList = InputMart.Instance.GetList<EQP_ARRANGE>(InputTable.EQP_ARRANGE);

            List<PhotoLot> returnList = new List<PhotoLot>();
            for(int i=1; i< 6; i++)
            {
                PhotoLot pl = new PhotoLot();
                pl.LotId = $"LOT_{i:D2}";
                pl.Product = processManager.GetProduct("PROD_01");
                pl.Process = processManager.GetProcess("PROC_01");
                pl.LotQty = 300;
                pl.State = LotState.WAIT;
                returnList.Add(pl);
            }

            for (int i = 6; i < 11; i++)
            {
                PhotoLot pl = new PhotoLot();
                pl.LotId = $"LOT_{i:D2}";
                pl.Product = processManager.GetProduct("PROD_02");
                pl.Process = processManager.GetProcess("PROC_01");
                pl.LotQty = 300;
                pl.State = LotState.WAIT;
                returnList.Add(pl);
            }

            for (int i = 11; i < 16; i++)
            {
                PhotoLot pl = new PhotoLot();
                pl.LotId = $"LOT_{i:D2}";
                pl.Product = processManager.GetProduct("PROD_01");
                pl.Process = processManager.GetProcess("PROC_01");
                pl.LotQty = 300;
                pl.State = LotState.WAIT;
                returnList.Add(pl);
            }

            for (int i = 16; i < 21; i++)
            {
                PhotoLot pl = new PhotoLot();
                pl.LotId = $"LOT_{i:D2}";
                pl.Product = processManager.GetProduct("PROD_02");
                pl.Process = processManager.GetProcess("PROC_01");
                pl.LotQty = 300;
                pl.State = LotState.WAIT;
                returnList.Add(pl);
            }

            //PhotoLot pl = new PhotoLot();
            //pl.LotId = "LOT_01";
            //pl.Product = processManager.GetProduct("PROD_01");
            //pl.Process = processManager.GetProcess("PROC_01");
            //pl.LotQty = 100;
            //pl.State = LotState.WAIT;

            //PhotoLot pl2 = new PhotoLot();
            //pl2.LotId = "LOT_02";
            //pl2.Product = processManager.GetProduct("PROD_01");
            //pl2.Process = processManager.GetProcess("PROC_01");
            //pl2.LotQty = 100;
            //pl2.State = LotState.WAIT;

            //PhotoLot pl3 = new PhotoLot();
            //pl3.LotId = "LOT_03";
            //pl3.Product = processManager.GetProduct("PROD_02");
            //pl3.Process = processManager.GetProcess("PROC_01");
            //pl3.LotQty = 100;
            //pl3.State = LotState.WAIT;

            //PhotoLot pl4 = new PhotoLot();
            //pl4.LotId = "LOT_04";
            //pl4.Product = processManager.GetProduct("PROD_02");
            //pl4.Process = processManager.GetProcess("PROC_01");
            //pl4.LotQty = 100;
            //pl4.State = LotState.WAIT;

            //returnList.Add(pl);
            //returnList.Add(pl2);
            //returnList.Add(pl3);
            //returnList.Add(pl4); 

            //List<EQUIPMENT> eqpList = InputMart.Instance.GetList<EQUIPMENT>(InputTable.EQUIPMENT);
            //foreach (EQUIPMENT eqp in eqpList)
            //{
            //    PhotoEquipment pe = new PhotoEquipment();
            //    pe.EqpId = eqp.EQP_ID;
            //    Enum.TryParse<EqpState>(eqp.EQP_STATE, true, out EqpState eqpState);
            //    pe.State = eqpState;

            //    returnList.Add(pe); 
            //}

            return returnList;
        }
    }
}
