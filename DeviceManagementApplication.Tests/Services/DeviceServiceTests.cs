using Application.Mappings;
using Moq;
using Application.Services;
using AutoMapper;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using Domain.DTOs;
using FluentAssertions;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _mockRepository;
    private readonly DeviceService _service;

    public DeviceServiceTests()
    {
        _mockRepository = new Mock<IDeviceRepository>();
        _service = new DeviceService(_mockRepository.Object, new Mapper(new MapperConfiguration(c => c.AddProfile<DeviceProfile>())));
    }
    
    [Fact]
    public async Task AddDevice_ShouldReturnDeviceId_WhenDeviceIsAdded()
    {
        var deviceDto = new DeviceDTO { Name = "Device 1", Brand = "Brand A" };
        var device = new Device { Id = 1, Name = "Device 1", Brand = "Brand A" };
        _mockRepository.Setup(r => r.AddDeviceAsync(It.IsAny<Device>())).ReturnsAsync(device.Id);

        var result = await _service.AddDevice(deviceDto);

        result.Should().Be(device.Id);
    }
    
    [Fact]
    public async Task GetDeviceAsync_ShouldReturnDevice_WhenDeviceExists()
    {
        var device = new Device { Id = 1, Name = "Device 1", Brand = "Brand A" };
        _mockRepository.Setup(r => r.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(device);

        var result = await _service.GetDeviceByIdAsync(1);

        result.Should().Be(device);
    }
    
    [Fact]
    public async Task GetDevicesAsync_ShouldReturnDevices_WhenDevicesExist()
    {
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Device 1", Brand = "Brand A" },
            new Device { Id = 2, Name = "Device 2", Brand = "Brand B" }
        };
        _mockRepository.Setup(r => r.GetDevicesAsync()).ReturnsAsync(devices);

        var result = await _service.GetDevicesAsync();

        result.Should().BeEquivalentTo(devices);
    }
    
    [Fact]
    public async Task GetDevicesAsync_ShouldReturnEmptyList_WhenNoDevicesExist()
    {
        _mockRepository.Setup(r => r.GetDevicesAsync()).ReturnsAsync(new List<Device>());

        var result = await _service.GetDevicesAsync();

        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task DeleteDeviceAsync_ShouldDeleteDevice_WhenDeviceExists()
    {
        _mockRepository.Setup(r => r.DeleteDeviceAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        await _service.DeleteDeviceAsync(1);

        _mockRepository.Verify(r => r.DeleteDeviceAsync(1), Times.Once);
    }
    
    [Fact]
    public async Task SearchDevicesByBrandAsync_ShouldReturnDevices_WhenDevicesExist()
    {
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Device 1", Brand = "Brand A" },
            new Device { Id = 2, Name = "Device 2", Brand = "Brand A" }
        };
        _mockRepository.Setup(r => r.SearchDevicesByBrandAsync(It.IsAny<string>())).ReturnsAsync(devices);

        var result = await _service.SearchDevicesByBrandAsync("Brand A");

        result.Should().BeEquivalentTo(devices);
    }
    
    [Fact]
    public async Task SearchDevicesByBrandAsync_ShouldReturnEmptyList_WhenNoDevicesExist()
    {
        _mockRepository.Setup(r => r.SearchDevicesByBrandAsync(It.IsAny<string>())).ReturnsAsync(new List<Device>());

        var result = await _service.SearchDevicesByBrandAsync("Brand A");

        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task SearchDevicesByBrandAsync_ShouldReturnEmptyList_WhenBrandIsNull()
    {
        _mockRepository.Setup(r => r.SearchDevicesByBrandAsync(It.IsAny<string>())).ReturnsAsync(new List<Device>());

        var result = await _service.SearchDevicesByBrandAsync(null);

        result.Should().BeEmpty();
    }
}