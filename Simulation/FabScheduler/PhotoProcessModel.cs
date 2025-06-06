﻿using DataMart.Input;
using DataMart.SqlMapper;
using FabSchedulerModel.Helper;
using FabSchedulerModel.InputEntity;
using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FabSchedulerModel
{
    public class PhotoProcessModel : ISimProcessModel
    {
        public IEnumerable<Step> GetSteps()
        {
            List<PhotoStep> returnList = new List<PhotoStep>();
            List<STEP> stepList = InputMart.Instance.GetList<STEP>(InputTable.STEP);
            foreach (STEP step in stepList)
            {
                PhotoStep ps = new PhotoStep();
                ps.StepId = step.STEP_ID;
                ps.StepName = step.STEP_NAME;
                returnList.Add(ps);
            }
            return returnList;
        }

        public IEnumerable<Process> GetProcesses(IEnumerable<Step> steps)
        {
            List<PhotoProcess> returnList = new List<PhotoProcess>();
            List<PROCESS> processes = InputMart.Instance.GetList<PROCESS>(InputTable.PROCESS);
            List<STEP_ROUTE> routeList = InputMart.Instance.GetList<STEP_ROUTE>(InputTable.STEP_ROUTE);

            foreach (PROCESS process in processes)
            {

                PhotoProcess proc = new PhotoProcess();
                proc.ProcessId = process.PROCESS_ID;

                var stepRoutes = routeList
                    .Where(r => r.PROCESS_ID == process.PROCESS_ID)
                    .OrderBy(r => r.STEP_SEQ);

                foreach (var stepRoute in stepRoutes)
                {
                    Step step = steps.Where(s=>s.StepId == stepRoute.STEP_ID).FirstOrDefault();
                    if (step != null)
                    {
                        step.StepSeq = stepRoute.STEP_SEQ;
                        proc.AddStep(step);
                    }
                }
                returnList.Add(proc);
            }

            return returnList;
        }
        public IEnumerable<Product> GetProducts(IEnumerable<Process> processes)
        {
            List<PhotoProduct> returnList = new List<PhotoProduct>();
            List<PRODUCT> productList = InputMart.Instance.GetList<PRODUCT>(InputTable.PRODUCT);
            foreach (PRODUCT product in productList)
            {
                PhotoProduct pp = new PhotoProduct();
                pp.ProductId = product.PRODUCT_ID;
                Process process = processes.Where(p => p.ProcessId == product.PROCESS_ID).FirstOrDefault();
                if(process != null)
                {
                    pp.Process = process;
                }
                returnList.Add(pp);
            }
            return returnList;
        }
        public double GetProcessTime(SimEquipment equipment, SimLot lot)
        {
            List<EQP_ARRANGE> arrangeList = InputMart.Instance.GetList<EQP_ARRANGE>(InputTable.EQP_ARRANGE);
            string stepId = lot.StepId;
            string prodId = lot.ProductId;
            string procId = lot.ProcessId;
            string eqpId = equipment.EqpId;
            double procTime = arrangeList.FindAll(x => x.STEP_ID.Equals(stepId) && x.PRODUCT_ID.Equals(prodId) && x.PROCESS_ID.Equals(procId) && x.EQP_ID.Equals(eqpId)).Select(x => x.TACT_TIME).First();
            double tactTime = lot.GetLot().LotQty * procTime;
            return tactTime;
        }

        public void OnTrackIn(SimEquipment equipment, SimLot lot) => Console.WriteLine($"{lot.LotId} OnTrackIn triggered. ");

        public void OnTrackOut(SimEquipment equipment, SimLot lot)
        {
            Console.WriteLine($"{lot.LotId} OnTrackOut triggered. ");
            //OutputHelper.WriteEqpSchedule(plan, lot);
        }

        public double GetSetupTime(SimEquipment equipment, SimLot lot)
        {
            List<SETUP_INFO> setupInfoList = InputMart.Instance.GetList<SETUP_INFO>(InputTable.SETUP_INFO);
            SETUP_INFO info = setupInfoList.Where(x => x.EQP_ID.Equals(equipment.EqpId)).FirstOrDefault();
            if (info != null) return info.SETUP_TIME;

            return 0;
        }

    }
}