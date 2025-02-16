using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class StepQuery : IQuery<STEP>
    {
        public string GetQuery()
        {
            return @"
                SELECT STEP_ID
                     , STEP_NAME
                FROM TB_STEP
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class StepMapper : BaseMapper<STEP>
        {

        }

        public TableLoadInfo<STEP> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            StepMapper stepMapper = new StepMapper();

            return new TableLoadInfo<STEP>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = stepMapper.CreateItem
            };
        }
    }
}
