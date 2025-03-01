using MySql.Data.MySqlClient;
using System;

namespace DataMart.SqlMapper
{
    public class TableLoadInfo<T>
    {
        public string Query { get; set; }
        public Func<MySqlDataReader, bool> IsLoadRow { get; set; }
        public Func<MySqlDataReader, T> CreateItem { get; set; }
    }
}
