using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class SetupInfoQuery : IQuery<SETUP_INFO>
    {
        public string GetQuery()
        {
            return @"
                SELECT EQP_ID
                     , SETUP_CONDITION
                     , SETUP_TIME
                FROM TB_SETUP_INFO
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class SetupInfoMapper : BaseMapper<SETUP_INFO>
        {

        }

        public TableLoadInfo<SETUP_INFO> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            SetupInfoMapper stepMapper = new SetupInfoMapper();

            return new TableLoadInfo<SETUP_INFO>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = stepMapper.CreateItem
            };
        }
    }
}
