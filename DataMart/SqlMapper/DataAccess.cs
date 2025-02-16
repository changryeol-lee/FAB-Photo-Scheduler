namespace DataMart.SqlMapper
{
    public abstract class DataAccess
    {
        protected string connectionString;

        protected DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

    }
}
