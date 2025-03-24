using DataMart.SqlMapper;
using FabSchedulerModel.ModelConfig;
using FabShedulerModel;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSchedulerModel.Helper
{
    internal class SimHelper
    {
        internal static void InitializeMart(string connectionString)
        {
            InputMart.Initialize(connectionString);
            OutputMart.Initialize(connectionString);
            InputMart.Instance.LoadFromDatabase();
        }

        internal static IModelGroup CreateModelGroup()
        {
            return new PhotoModelGroup(
                new PhotoSimulationModel(),
                new PhotoEquipmentModel(),
                new PhotoLotModel(),
                new PhotoRouteModel(),
                new PhotoDispatchModel(),
                new PhotoProcessModel(),
                new PhotoOffTimeModel()
            );
        }

        internal static PhotoSimulationOption CreateSimulationOption()
        {
            return new PhotoSimulationOption
            {
                SimulationStartTime = DateTime.Today,
                SimulationEndTime = DateTime.Today.AddDays(7),
                DispatchType = DispatchType.MIN_SETUP,
                RunUser = "Engine"
            };
        }

        internal static void RunSimulation(IModelGroup modelGroup, PhotoSimulationOption option)
        {
            SimFactory.InitializeInstance(modelGroup, option);
            SimFactory factory = SimFactory.Instance;
            factory.Initialize(modelGroup);
            factory.StartSimulation();
        }
    }
}
