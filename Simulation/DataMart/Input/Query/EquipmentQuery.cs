using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class EquipmentQuery : IQuery<EQUIPMENT>
    {
        public string GetQuery()
        {
            return @"
                SELECT EQP_ID
                     , EQP_TYPE
                     , EQP_STATE
                FROM TB_EQUIPMENT
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class EquipmentMapper : BaseMapper<EQUIPMENT>
        {

        }

        public TableLoadInfo<EQUIPMENT> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            EquipmentMapper equipmentMapper = new EquipmentMapper();

            return new TableLoadInfo<EQUIPMENT>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = equipmentMapper.CreateItem
            };
        }
    }
}
