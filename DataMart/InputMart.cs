﻿using DataMart.Input;
using DataMart.Input.Query;
using System.Collections.Generic;
using System.Configuration;


namespace DataMart.SqlMapper
{
    public class InputMart
    {
        private static readonly object lockObject = new object();
        private static volatile InputMart instance;
        private Dictionary<InputTable, object> dataLists;
        private readonly TableDataAccess dataAccess;
        private Dictionary<InputTable, object> tableQueries;

        public static InputMart Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new InputMart();
                        }
                    }
                }
                return instance;
            }
        }

        private InputMart()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            dataAccess = new TableDataAccess(connectionString);
            dataLists = new Dictionary<InputTable, object>();
            InitializeTableQueries();
        }

        private void InitializeTableQueries()
        {
            tableQueries = new Dictionary<InputTable, object>
        {
            { InputTable.PROCESS, new ProcessQuery() },
            { InputTable.PRODUCT, new ProductQuery() },
            { InputTable.EQUIPMENT, new EquipmentQuery() },
            { InputTable.STEP, new StepQuery() },
            { InputTable.STEP_ROUTE, new StepRouteQuery() },
            { InputTable.EQP_ARRANGE, new EqpArrangeQuery() },
        };
        }

        public void LoadFromDatabase()
        {
            LoadTable<PROCESS>(InputTable.PROCESS);
            LoadTable<PRODUCT>(InputTable.PRODUCT);
            LoadTable<STEP>(InputTable.STEP);
            LoadTable<STEP_ROUTE>(InputTable.STEP_ROUTE);
            LoadTable<EQUIPMENT>(InputTable.EQUIPMENT);
            LoadTable<EQP_ARRANGE>(InputTable.EQP_ARRANGE);
        }

        private void LoadTable<T>(InputTable table)
        {
            var query = tableQueries[table] as IQuery<T>; ;
            var loadInfo = query.GetLoadInfo();
            var list = dataAccess.LoadTableData<T>(
                loadInfo.Query,
                loadInfo.IsLoadRow,
                loadInfo.CreateItem);

            dataLists[table] = list;
        }

        public List<T> GetList<T>(InputTable table)
        {
            if (dataLists.ContainsKey(table))
                return dataLists[table] as List<T> ?? new List<T>();
            return new List<T>();
        }

        public void RefreshData()
        {
            LoadFromDatabase();
        }

        public void RefreshTableData<T>(InputTable table)
        {
            LoadTable<T>(table);
        }
    }
}
