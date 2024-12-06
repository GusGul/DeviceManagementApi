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

        public Task DeleteDeviceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Device?> GetDeviceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetDevicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDeviceAsync(Device device)
        {
            throw new NotImplementedException();
        }
    }
}
