using DataMart.SqlMapper;
using FabSchedulerModel.Helper;
using FabSchedulerModel.ModelConfig;
using FabSchedulerModel;
using FabShedulerModel;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;

namespace EngineTrigger
{
    public class SimulationRunner
    {
        public void RunSimulation(PhotoSimulationOption option)
        {
            InputMart.Instance.LoadFromDatabase();
            InputMart.Instance.SetVersion(option.DispatchType.ToString());

            PhotoSimulationModel simulationModel = new PhotoSimulationModel();
            PhotoEquipmentModel equipmentModel = new PhotoEquipmentModel();
            PhotoLotModel lotModel = new PhotoLotModel();
            PhotoRouteModel routeModel = new PhotoRouteModel();
            PhotoDispatchModel dispatchModel = new PhotoDispatchModel();
            PhotoProcessModel processModel = new PhotoProcessModel();
            PhotoOffTimeModel offTimeModel = new PhotoOffTimeModel();

            IModelGroup modelGroup = new PhotoModelGroup(
                simulationModel, equipmentModel, lotModel,
                routeModel, dispatchModel, processModel, offTimeModel);

            OutputHelper.WriteEngineExecuteLog(option);

            SimFactory.InitializeInstance(modelGroup, option);
            SimFactory factory = SimFactory.Instance;
            factory.Initialize(modelGroup);
            factory.StartSimulation();
        }
    }
}
