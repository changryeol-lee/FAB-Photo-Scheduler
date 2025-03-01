using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class EqpArrangeQuery : IQuery<EQP_ARRANGE>
    {
        public string GetQuery()
        {
            return @"
                SELECT PRODUCT_ID
                     , PROCESS_ID
                     , STEP_ID
                     , EQP_ID
                     , TACT_TIME
                     , PROC_TIME
                FROM TB_EQP_ARRANGE
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class EqpArrangeMapper : BaseMapper<EQP_ARRANGE>
        {

        }

        public TableLoadInfo<EQP_ARRANGE> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            EqpArrangeMapper eqpArrangeMapper = new EqpArrangeMapper();

            return new TableLoadInfo<EQP_ARRANGE>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = eqpArrangeMapper.CreateItem
            };
        }
    }
}
