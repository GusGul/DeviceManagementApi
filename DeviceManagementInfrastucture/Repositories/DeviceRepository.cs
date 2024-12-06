using Dapper;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace DeviceManagementInfrastucture.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDbConnection _dbConnection;

        public DeviceRepository(IConfiguration configuration)
        {
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> AddDeviceAsync(Device device)
        {
            const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
            return await _dbConnection.ExecuteScalarAsync<int>(query, device);
        }

        public Task<Device?> GetDeviceAsync(int id)
        {
            const string query = "SELECT Id, Name, Brand, CreatedAt FROM Devices WHERE Id = @Id;";
            return _dbConnection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
        }

        public Task<IEnumerable<Device>> GetDevicesAsync()
        {
            const string query = "SELECT Id, Name, Brand, CreatedAt FROM Devices;";
            return _dbConnection.QueryAsync<Device>(query);
        }

        public Task DeleteDeviceAsync(int id)
        {
            const string query = "DELETE FROM Devices WHERE Id = @Id;";
            return _dbConnection.ExecuteAsync(query, new { Id = id });
        }

        public Task UpdateDeviceAsync(Device device)
        {
            const string query = "UPDATE Devices SET Name = @Name, Brand = @Brand WHERE Id = @Id;";
            return _dbConnection.ExecuteAsync(query, device);
        }

        public Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand)
        {
            const string query = "SELECT Id, Name, Brand, CreatedAt FROM Devices WHERE Brand = @Brand;";
            return _dbConnection.QueryAsync<Device>(query, new { Brand = brand });
        }
    }
}
