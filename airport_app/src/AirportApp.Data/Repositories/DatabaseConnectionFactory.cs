namespace AirportApp.Data.Repositories
{
    public class DatabaseConnectionFactory
    {
        private readonly string connectionString;

        public DatabaseConnectionFactory()
        {
            // connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=AirportDB;Integrated Security=true;TrustServerCertificate=True";
            connectionString = @"Server=(localdb)\MSSQLLocalDB; Database = AirportDB; Trusted_Connection = True; TrustServerCertificate = True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
