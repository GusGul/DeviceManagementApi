using Dapper;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace DeviceManagementInfrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<DeviceRepository> _logger;

    public DeviceRepository(IConfiguration configuration, ILogger<DeviceRepository> logger)
    {
        _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
        _logger = logger;
    }

    public async Task<int> AddDeviceAsync(Device device)
    {
        try
        {
            const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
            return await _dbConnection.ExecuteScalarAsync<int>(query, device);
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
            return await _dbConnection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
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
            return await _dbConnection.QueryAsync<Device>(query);
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
            await _dbConnection.ExecuteAsync(query, new { Id = id });
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
            await _dbConnection.ExecuteAsync(query, parameters);
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
            return await _dbConnection.QueryAsync<Device>(query, new { Brand = $"%{brand}%" });
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while trying to search devices by brand: {e.Message}");
            throw;
        }
    }
}
