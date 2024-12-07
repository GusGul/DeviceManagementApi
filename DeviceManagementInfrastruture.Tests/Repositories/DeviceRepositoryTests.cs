// using Npgsql;
// using Dapper;
// using DeviceManagementDomain.Entities;
// using DeviceManagementInfrastructure.Repositories;
//
// public class DeviceRepositoryTests
// {
//     private readonly DeviceRepository _repository;
//     private readonly NpgsqlConnection _connection;
//
//     public DeviceRepositoryTests()
//     {
//         var connectionString = "Host=localhost;Database=testdb;Username=postgres;Password=postgres";
//         _connection = new NpgsqlConnection(connectionString);
//         _repository = new DeviceRepository(() => _connection);
//     }
//
//     [Fact]
//     public async Task AddDevice_ShouldInsertDeviceIntoDatabase()
//     {
//         // Arrange
//         var device = new Device { Name = "Test Device", Brand = "Test Brand" };
//
//         // Act
//         var deviceId = await _repository.AddDeviceAsync(device);
//
//         // Assert
//         var insertedDevice = await _connection.QueryFirstOrDefaultAsync<Device>(
//             "SELECT * FROM Devices WHERE Id = @Id", new { Id = deviceId });
//         Assert.NotNull(insertedDevice);
//         Assert.Equal(device.Name, insertedDevice.Name);
//     }
// }