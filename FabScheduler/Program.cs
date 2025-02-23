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

            PhotoSimulationModel model = new PhotoSimulationModel();

            SimFactory.InitializeInstance(model, startDate, endDate);
            SimFactory factory = SimFactory.Instance;
            factory.Initialize();
            factory.StartSimulation();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();

        }
    }
}
