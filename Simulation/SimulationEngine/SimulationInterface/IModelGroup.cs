using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.SimulationInterface
{
    public interface IModelGroup
    {
        ISimulationModel SimulationModel { get; }
        ISimEquipmentModel EquipmentModel { get; }
        ISimLotModel LotModel { get; }
        ISimRouteModel RouteModel { get; }
        ISimDispatchModel DispatchModel { get; }
        ISimProcessModel ProcessModel { get; }
        ISimOffTimeModel OffTimeModel { get; }

    }
}
