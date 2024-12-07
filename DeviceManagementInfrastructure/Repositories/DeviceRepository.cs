using System.Data;
using System.Data.Common;
using Dapper;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using DeviceManagementInfrastructure.Factories;

namespace DeviceManagementInfrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<DeviceRepository> _logger;

    public DeviceRepository(IDatabaseFactory databaseFactory, ILogger<DeviceRepository> logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
    }
    
    private IDbConnection DbConnection => _databaseFactory.CreateConnection();

    public async Task<int> AddDeviceAsync(Device device)
    {
        try
        {
            const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
            using (var connection = DbConnection)
            {
                return await connection.ExecuteScalarAsync<int>(query, device);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while trying to add a new device: {e.Message}");
            throw;
        }
    }

    public async Task<Device?> GetDeviceAsync(int id)
    {
        try
        {
            const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Id = @Id;";
            using (var connection = DbConnection)
            {
                return await connection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while trying to get the device with Id {id}: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        try
        {
            const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices;";
            using (var connection = DbConnection)
            {
                return await connection.QueryAsync<Device>(query);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while trying to get all devices: {e.Message}");
            throw;
        }
    }

    public async Task DeleteDeviceAsync(int id)
    {
        try
        {
            const string query = "DELETE FROM Devices WHERE Id = @Id;";
            using (var connection = DbConnection)
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while trying to delete the device with Id {id}: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateDeviceAsync(Device device)
    {
        try
        {
            var updates = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(device.Name))
            {
                updates.Add("Name = @Name");
                parameters.Add("Name", device.Name);
            }

            if (!string.IsNullOrWhiteSpace(device.Brand))
            {
                updates.Add("Brand = @Brand");
                parameters.Add("Brand", device.Brand);
            }

            if (!updates.Any())
            {
                throw new ArgumentException("No valid fields were provided for the update.");
            }

            parameters.Add("Id", device.Id);

            var query = $"UPDATE Devices SET {string.Join(", ", updates)} WHERE Id = @Id;";
            using (var connection = DbConnection)
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while trying to update the device with Id {device.Id}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand)
    {
        try
        {
            const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Brand ILIKE @Brand;";
            using (var connection = DbConnection)
            {
                return await connection.QueryAsync<Device>(query, new { Brand = $"%{brand}%" });
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while trying to search devices by brand: {e.Message}");
            throw;
        }
    }
}
