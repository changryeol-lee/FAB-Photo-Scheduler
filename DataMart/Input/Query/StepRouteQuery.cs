using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class StepRouteQuery : IQuery<STEP_ROUTE>
    {
        public string GetQuery()
        {
            return @"
                SELECT PROCESS_ID
                     , STEP_ID
                     , STEP_SEQ
                FROM TB_STEP_ROUTE
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class StepRouteMapper : BaseMapper<STEP_ROUTE>
        {

        }

        public TableLoadInfo<STEP_ROUTE> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            StepRouteMapper stepMapper = new StepRouteMapper();

            return new TableLoadInfo<STEP_ROUTE>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = stepMapper.CreateItem
            };
        }
    }
}
