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

namespace FabSchedulerModel
{
internal class Program
{
        static void Main(string[] args)
        {
      
            InputMart.Instance.LoadFromDatabase();
            //var products = InputMart.Instance.GetList<PRODUCT>(InputTable.PRODUCT);

            DateTime startDate = new DateTime(2025, 3, 1, 15, 0, 0);
            DateTime endDate = new DateTime(2025, 3, 5, 15, 0, 0);

            PhotoSimulationModel simulationModel = new PhotoSimulationModel();
            PhotoEquipmentModel equipmentModel = new PhotoEquipmentModel();
            PhotoLotModel lotModel = new PhotoLotModel();
            PhotoRouteModel routeModel = new PhotoRouteModel();
            PhotoDispatchModel dispatchModel = new PhotoDispatchModel(); 
            PhotoProcessModel processModel = new PhotoProcessModel();

     
            IModelGroup modelGroup = new PhotoModelGroup(simulationModel, equipmentModel, lotModel, routeModel, dispatchModel, processModel);


            SimFactory.InitializeInstance(modelGroup, startDate, endDate);
            SimFactory factory = SimFactory.Instance;
            factory.Initialize(modelGroup);
            factory.StartSimulation();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();

        }
    }
}
