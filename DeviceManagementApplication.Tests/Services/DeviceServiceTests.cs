using Moq;
using Application.Services;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using FluentAssertions;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _mockRepository;
    private readonly DeviceService _service;

    public DeviceServiceTests()
    {
        _mockRepository = new Mock<IDeviceRepository>();
        _service = new DeviceService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetDevices_ShouldReturnAllDevices()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Device 1", Brand = "Brand A" },
            new Device { Id = 2, Name = "Device 2", Brand = "Brand B" }
        };
        _mockRepository.Setup(r => r.GetDevicesAsync()).ReturnsAsync(devices);

        // Act
        var result = await _service.GetDevicesAsync();

        // Assert
        result.Should().HaveCount(2).And.Contain(devices);
    }
}