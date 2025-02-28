using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


namespace DataMart.SqlMapper
{
    public class TableDataAccess : DataAccess
    {
        public TableDataAccess(string connectionString) : base(connectionString) { }

        public List<T> LoadTableData<T>(
            string query,
            Func<MySqlDataReader, bool> isLoadRow,
            Func<MySqlDataReader, T> createItem)
        {
            var list = new List<T>();

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (isLoadRow(reader))
                        {
                            var item = createItem(reader);
                            list.Add((T)item);
                        }
                    }
                }
            }

            return list;
        }

        public int InsertRows<T>(string insertSql, IEnumerable<T> dataItems, Action<MySqlCommand, T> bindParameters)
        {
            int totalRows = 0;

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(insertSql, conn))
            {
                conn.Open();
                foreach (var item in dataItems)
                {
                    cmd.Parameters.Clear();
                    bindParameters(cmd, item);

                    totalRows += cmd.ExecuteNonQuery();
                }
            }
            return totalRows;
        }
    }
}