using DataMart.Input;
using DataMart.SqlMapper;
using FabSchedulerModel.InputEntity;
using SimulationEngine.BaseEntity;
using SimulationEngine.Common;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel
{
    public class PhotoEquipmentModel : ISimEquipmentModel
    {
        public IEnumerable<Equipment> GetEqpList()
        {
            List<PhotoEquipment> returnList = new List<PhotoEquipment>();
            List<EQUIPMENT> eqpList = InputMart.Instance.GetList<EQUIPMENT>(InputTable.EQUIPMENT);
            foreach (EQUIPMENT eqp in eqpList)
            {
                PhotoEquipment pe = new PhotoEquipment();
                pe.EqpId = eqp.EQP_ID;
                Enum.TryParse<EqpState>(eqp.EQP_STATE, true, out EqpState eqpState);
                pe.State = eqpState;

                returnList.Add(pe); 
            }
            return returnList;
        }

        public IEnumerable<string> GetLoadableEqpIds(SimLot lot)
        {
            List<EQP_ARRANGE> arrangeList = InputMart.Instance.GetList<EQP_ARRANGE>(InputTable.EQP_ARRANGE);
            string stepId = lot.StepId;
            string prodId = lot.ProductId;
            string procId = lot.ProcessId;
            List<string> photoEqpIds = arrangeList.FindAll(x => x.STEP_ID.Equals(stepId) && x.PRODUCT_ID.Equals(prodId) && x.PROCESS_ID.Equals(procId)).Select(x=>x.EQP_ID).ToList();
            if (photoEqpIds == null)
            {
                Console.WriteLine("arrnage");
            }
            return photoEqpIds; 
        }
    }
}
