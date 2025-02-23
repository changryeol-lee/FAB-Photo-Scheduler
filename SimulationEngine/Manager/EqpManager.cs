using SimulationEngine.BaseEntity;
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
        private List<SimEquipment> _simEquipments = new List<SimEquipment>(); 

        private readonly ISimEquipmentModel _model;
        public EqpManager(ISimEquipmentModel model)
        {
            _model = model;
        }

        public void SimEqpInit()
        {
            List<Equipment> equipments = _model.GetEqpList();  // 사용자가 제공한 장비 리스트

            foreach (var equipment in equipments)
            {
                _simEquipments.Add(new SimEquipment(equipment));  // SimEquipment로 변환하여 저장
            }
        }
    }
}
