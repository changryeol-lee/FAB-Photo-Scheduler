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
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SimHelper.InitializeMart(connectionString);

            //이 시점부터 input 데이터 접근 가능 
            //var products = InputMart.Instance.GetList<PRODUCT>(InputTable.PRODUCT);

            IModelGroup modelGroup = SimHelper.CreateModelGroup();
            //옵션 파라미터 설정
            PhotoSimulationOption simulationOption = SimHelper.CreateSimulationOption();
            InputMart.Instance.SetVersion(simulationOption.DispatchType.ToString());
            OutputHelper.WriteEngineExecuteLog(simulationOption);

            //시뮬레이션 실행 
            SimHelper.RunSimulation(modelGroup, simulationOption);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
