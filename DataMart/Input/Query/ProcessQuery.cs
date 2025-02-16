using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class ProcessQuery : IQuery<PROCESS>
    {
        public string GetQuery()
        {
            return @"
                SELECT PROCESS_ID
                FROM TB_PROCESS
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class ProcessMapper : BaseMapper<PROCESS>
        {

        }

        public TableLoadInfo<PROCESS> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            ProcessMapper processMapper = new ProcessMapper();

            return new TableLoadInfo<PROCESS>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = processMapper.CreateItem
            };
        }
    }
}
