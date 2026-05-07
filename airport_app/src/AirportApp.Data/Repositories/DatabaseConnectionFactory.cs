namespace AirportApp.Data.Repositories
{
    public class DatabaseConnectionFactory
    {
        private readonly string connectionString;

        public DatabaseConnectionFactory()
        {
            connectionString = @"Server=DESKTOP-1JCJMN6\SQLEXPRESS;Initial Catalog=AirportDB;Integrated Security=true;TrustServerCertificate=True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
