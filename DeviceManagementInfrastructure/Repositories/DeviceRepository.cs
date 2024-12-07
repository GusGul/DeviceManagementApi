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
        const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
        return await _dbConnection.ExecuteScalarAsync<int>(query, device);
    }

    public async Task<Device?> GetDeviceAsync(int id)
    {
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Id = @Id;";
        return await _dbConnection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
    }

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices;";
        return await _dbConnection.QueryAsync<Device>(query);
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
            _logger.LogError($"Erro ao tentar deletar o dispositivo com Id {id}: {ex.Message}");
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
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Brand ILIKE @Brand;";
        return await _dbConnection.QueryAsync<Device>(query, new { Brand = $"%{brand}%" });
    }
}
