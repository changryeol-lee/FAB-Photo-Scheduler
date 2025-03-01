using DataMart.SqlMapper;
using MySql.Data.MySqlClient;
using System;

namespace DataMart.Input.Query
{
    public class ProductQuery : IQuery<PRODUCT>
    {
        public string GetQuery()
        {
            return @"
                SELECT PRODUCT_ID
                     , PROCESS_ID
                     , LOT_SIZE
                FROM TB_PRODUCT
            ";
        }

        public bool IsLoadRow(MySqlDataReader reader)
        {
            return true;
        }

        public class ProductMapper : BaseMapper<PRODUCT>
        {

        }

        public TableLoadInfo<PRODUCT> GetLoadInfo()
        {
            string query = GetQuery();
            Func<MySqlDataReader, bool> func = IsLoadRow;
            ProductMapper productMapper = new ProductMapper();

            return new TableLoadInfo<PRODUCT>
            {
                Query = query,
                IsLoadRow = func,
                CreateItem = productMapper.CreateItem
            };
        }
    }
}
