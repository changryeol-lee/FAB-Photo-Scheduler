using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class OffTimeInfoQuery : IQuery<OFFTIME_INFO>
    {
        public string GetQuery()
        {
            return @"
                SELECT RULE_TYPE
                     , START_TIME
                     , END_TIME
                     , DAYS_OF_WEEK
                     , START_DATE_TIME
                     , END_DATE_TIME 
                FROM TB_OFFTIME_INFO
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class OffTimeInfoMapper : BaseMapper<OFFTIME_INFO>
        {

        }

        public TableLoadInfo<OFFTIME_INFO> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            OffTimeInfoMapper offtimeMapper = new OffTimeInfoMapper();

            return new TableLoadInfo<OFFTIME_INFO>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = offtimeMapper.CreateItem
            };
        }
    }
}
