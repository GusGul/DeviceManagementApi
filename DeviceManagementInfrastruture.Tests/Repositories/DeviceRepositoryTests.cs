using System.Data;
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
        _mockLogger = new Mock<ILogger<DeviceRepository>>();
        _deviceRepository = new DeviceRepository(new Mock<IConfiguration>().Object, _mockLogger.Object);
        
        _mockDbConnection = new Mock<IDbConnection>();
        _mockDbConnection.Setup(d => d.State).Returns(ConnectionState.Open);
    }
}