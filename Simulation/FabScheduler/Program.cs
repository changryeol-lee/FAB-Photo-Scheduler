using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMart.SqlMapper;
using DataMart.Input;
using FabShedulerModel;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using System.Diagnostics;
using FabSchedulerModel.ModelConfig;
using FabSchedulerModel.Helper; 

namespace FabSchedulerModel
{
internal class Program
{
        static void Main(string[] args)
        {
      
            InputMart.Instance.LoadFromDatabase();
            //var products = InputMart.Instance.GetList<PRODUCT>(InputTable.PRODUCT);


            PhotoSimulationModel simulationModel = new PhotoSimulationModel();
            PhotoEquipmentModel equipmentModel = new PhotoEquipmentModel();
            PhotoLotModel lotModel = new PhotoLotModel();
            PhotoRouteModel routeModel = new PhotoRouteModel();
            PhotoDispatchModel dispatchModel = new PhotoDispatchModel(); 
            PhotoProcessModel processModel = new PhotoProcessModel();
            IModelGroup modelGroup = new PhotoModelGroup(simulationModel, equipmentModel, lotModel, routeModel, dispatchModel, processModel);

            //엔진 실행자로부터 입력받아야할 값을 담은 객체 
            PhotoSimulationOption simulationOption = new PhotoSimulationOption();
            simulationOption.SimulationStartTime = DateTime.Today; 
            simulationOption.SimulationEndTime = DateTime.Today.AddDays(7);
            simulationOption.DispatchType = DispatchType.FIFO;
            simulationOption.RunUser = "Engine";
            InputMart.Instance.SetVersion(simulationOption.DispatchType.ToString());

            OutputHelper.WriteEngineExecuteLog(simulationOption); 

            SimFactory.InitializeInstance(modelGroup, simulationOption);
            SimFactory factory = SimFactory.Instance;
            factory.Initialize(modelGroup);
            factory.StartSimulation();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();

        }
    }
}
