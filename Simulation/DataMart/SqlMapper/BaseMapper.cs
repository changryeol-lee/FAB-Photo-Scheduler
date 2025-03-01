using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace DataMart.SqlMapper
{
    public abstract class BaseMapper<T> where T : new()
    {
        public T CreateItem(MySqlDataReader reader)
        {
            T item = new T();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (!HasColumn(reader, prop.Name))
                    continue;

                object value = GetValueFromReader(reader, prop);
                if (value != DBNull.Value)
                {
                    prop.SetValue(item, value);
                }
            }

            return item;
        }

        private bool HasColumn(MySqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private object GetValueFromReader(MySqlDataReader reader, PropertyInfo prop)
        {
            object value = reader[prop.Name];
            if (value == DBNull.Value)
                return value;

            Type propertyType = prop.PropertyType;

            // 기본 타입 변환
            if (propertyType == typeof(string))
                return value.ToString();
            if (propertyType == typeof(int))
                return Convert.ToInt32(value);
            if (propertyType == typeof(decimal))
                return Convert.ToDecimal(value);
            if (propertyType == typeof(DateTime))
                return Convert.ToDateTime(value);
            if (propertyType == typeof(bool))
                return Convert.ToBoolean(value);

            return value;
        }
    }
}
