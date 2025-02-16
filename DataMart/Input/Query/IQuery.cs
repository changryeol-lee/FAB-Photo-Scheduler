using DataMart.SqlMapper;
using MySql.Data.MySqlClient;

namespace DataMart.Input.Query
{
    public interface IQuery<T>
    {
        string GetQuery();
        bool IsLoadRow(MySqlDataReader reader);
        TableLoadInfo<T> GetLoadInfo();
    }
}
