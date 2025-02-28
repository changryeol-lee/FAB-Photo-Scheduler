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

namespace SimulationEngine.Agents
{
    public class EqpManager
    {
        private readonly Dictionary<string, SimEquipment> _simEquipments;

        private readonly ISimEquipmentModel _model;
        public EqpManager(ISimEquipmentModel model)
        {
            _model = model;
            _simEquipments = new Dictionary<string, SimEquipment>();
        }

        public void SimEqpInit()
        {
            IEnumerable<Equipment> equipments = _model.GetEqpList();  // 사용자가 제공한 장비 리스트

            foreach (var equipment in equipments)
            {
                _simEquipments.Add(equipment.EqpId, new SimEquipment(equipment));  // SimEquipment로 변환하여 저장
            }
        }
        public SimEquipment GetEquipment(string eqpId)
        {
            return _simEquipments.TryGetValue(eqpId, out var eqp) ? eqp : null; 
        }
        public IEnumerable<SimEquipment> GetLoadableEqpList(SimLot lot)
        {
            var eqpIds = _model.GetLoadableEqpIds(lot);
            List<SimEquipment> result = new List<SimEquipment>();

            foreach (var eqpId in eqpIds)
            {
                SimEquipment eqp = GetEquipment(eqpId); 
                if (eqp != null)
                    result.Add(eqp);
            }

            return result; 
        }

        internal List<SimEquipment> GetIdleEqpList()
        {
            return _simEquipments.Values.Where(eqp => eqp.GetEquipment().State.Equals(EqpState.IDLE)).ToList();
        }
    }
}
