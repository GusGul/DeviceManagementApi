using System.Data;
using DeviceManagementInfrastructure.Factories;
using DeviceManagementInfrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

public class DeviceRepositoryTests
{
    private readonly Mock<IDatabaseFactory> _mockDatabaseFactory;
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly Mock<ILogger<DeviceRepository>> _mockLogger;
    private readonly DeviceRepository _deviceRepository;

    public DeviceRepositoryTests()
    {
        _mockDatabaseFactory = new Mock<IDatabaseFactory>();
        _mockLogger = new Mock<ILogger<DeviceRepository>>();
        _mockDbConnection = new Mock<IDbConnection>();

        _mockDatabaseFactory.Setup(factory => factory.CreateConnection()).Returns(_mockDbConnection.Object);
        
        _deviceRepository = new DeviceRepository(_mockDatabaseFactory.Object, _mockLogger.Object);
    }
}