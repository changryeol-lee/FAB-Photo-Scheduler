using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel
{
    public class PhotoModelGroup : IModelGroup
    {
        public ISimulationModel SimulationModel { get; private set; }
        public ISimEquipmentModel EquipmentModel { get; private set; }

        public PhotoModelGroup(ISimulationModel simModel, ISimEquipmentModel eqpModel)
        {
            SimulationModel = simModel;
            EquipmentModel = eqpModel;
        }
    }
}
