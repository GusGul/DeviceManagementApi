using System.Data;
using Dapper;
using DeviceManagementDomain.Entities;
using DeviceManagementInfrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

public class DeviceRepositoryTests
{
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly Mock<ILogger<DeviceRepository>> _mockLogger;
    private readonly DeviceRepository _deviceRepository;

    public DeviceRepositoryTests()
    {
        _mockDbConnection = new Mock<IDbConnection>();
        _mockLogger = new Mock<ILogger<DeviceRepository>>();
        _deviceRepository = new DeviceRepository(new Mock<IConfiguration>().Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AddDeviceAsync_ShouldReturnDeviceId()
    {
        var device = new Device { Name = "Device1", Brand = "Brand1" };
        const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
        _mockDbConnection.Setup(d => d.ExecuteScalarAsync<int>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(1);

        var result = await _deviceRepository.AddDeviceAsync(device);

        Assert.Equal(1, result);
        _mockDbConnection.Verify(db => db.ExecuteScalarAsync<int>(
            It.IsAny<string>(), It.IsAny<object>(),
            It.IsAny<IDbTransaction>(), It.IsAny<int>(),
            It.IsAny<CommandType>()), Times.Once);
    }

    [Fact]
    public async Task AddDeviceAsync_ShouldLogErrorOnException()
    {
        var device = new Device { Name = "Device1", Brand = "Brand1" };
        const string query = "INSERT INTO Devices (Name, Brand) VALUES (@Name, @Brand) RETURNING Id;";
        _mockDbConnection.Setup(d => d.ExecuteScalarAsync<int>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _deviceRepository.AddDeviceAsync(device));
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetDeviceAsync_ShouldReturnDevice()
    {
        var device = new Device { Id = 1, Name = "Device1", Brand = "Brand1" };
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Id = @Id;";
        _mockDbConnection.Setup(d => d.QueryFirstOrDefaultAsync<Device>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(device);

        var result = await _deviceRepository.GetDeviceAsync(1);

        Assert.Equal(device, result);
    }

    [Fact]
    public async Task GetDevicesAsync_ShouldReturnDevices()
    {
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Device1", Brand = "Brand1" },
            new Device { Id = 2, Name = "Device2", Brand = "Brand2" }
        };
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices;";
        _mockDbConnection.Setup(d => d.QueryAsync<Device>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(devices);

        var result = await _deviceRepository.GetDevicesAsync();

        Assert.Equal(devices.Count, result.ToList().Count);
    }

    [Fact]
    public async Task DeleteDeviceAsync_ShouldNotThrowError()
    {
        const int deviceId = 1;
        const string query = "DELETE FROM Devices WHERE Id = @Id;";
        _mockDbConnection.Setup(d => d.ExecuteAsync(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(1);

        await _deviceRepository.DeleteDeviceAsync(deviceId);
        _mockDbConnection.Verify(db => db.ExecuteAsync(
            It.IsAny<string>(), It.IsAny<object>(),
            It.IsAny<IDbTransaction>(), It.IsAny<int>(),
            It.IsAny<CommandType>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDeviceAsync_ShouldLogErrorOnException()
    {
        const int deviceId = 1;
        const string query = "DELETE FROM Devices WHERE Id = @Id;";
        _mockDbConnection.Setup(d => d.ExecuteAsync(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _deviceRepository.DeleteDeviceAsync(deviceId));
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDeviceAsync_ShouldUpdateDevice()
    {
        var device = new Device { Id = 1, Name = "UpdatedDevice", Brand = "UpdatedBrand" };
        const string query = "UPDATE Devices SET Name = @Name, Brand = @Brand WHERE Id = @Id;";
        _mockDbConnection.Setup(d => d.ExecuteAsync(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(1);

        await _deviceRepository.UpdateDeviceAsync(device);

        _mockDbConnection.Verify(db => db.ExecuteAsync(
            It.IsAny<string>(), It.IsAny<object>(),
            It.IsAny<IDbTransaction>(), It.IsAny<int>(),
            It.IsAny<CommandType>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDeviceAsync_ShouldThrowErrorIfNoValidFieldsProvided()
    {
        var device = new Device { Id = 1, Name = "", Brand = "" };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _deviceRepository.UpdateDeviceAsync(device));
        Assert.Equal("No valid fields were provided for the update.", exception.Message);
    }

    [Fact]
    public async Task UpdateDeviceAsync_ShouldLogErrorOnException()
    {
        var device = new Device { Id = 1, Name = "UpdatedDevice", Brand = "UpdatedBrand" };
        const string query = "UPDATE Devices SET Name = @Name, Brand = @Brand WHERE Id = @Id;";
        _mockDbConnection.Setup(d => d.ExecuteAsync(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _deviceRepository.UpdateDeviceAsync(device));
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SearchDevicesByBrandAsync_ShouldReturnDevices()
    {
        var brand = "Brand1";
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Device1", Brand = "Brand1" },
            new Device { Id = 2, Name = "Device2", Brand = "Brand1" }
        };
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Brand ILIKE @Brand;";
        _mockDbConnection.Setup(d => d.QueryAsync<Device>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ReturnsAsync(devices);

        var result = await _deviceRepository.SearchDevicesByBrandAsync(brand);

        Assert.Equal(devices.Count, result.ToList().Count);
    }

    [Fact]
    public async Task SearchDevicesByBrandAsync_ShouldLogErrorOnException()
    {
        const string query = "SELECT Id, Name, Brand, CreationTime FROM Devices WHERE Brand ILIKE @Brand;";
        _mockDbConnection.Setup(d => d.QueryAsync<Device>(
                It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<IDbTransaction>(), It.IsAny<int>(),
                It.IsAny<CommandType>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _deviceRepository.SearchDevicesByBrandAsync("Brand1"));
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
    }
}