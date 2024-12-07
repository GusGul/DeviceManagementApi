using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace DeviceManagementInfrastructure.Factories
{
    public interface IDatabaseFactory
    {
        IDbConnection CreateConnection();
    }

    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IConfiguration _configuration;

        public DatabaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString);
        }
    }
}