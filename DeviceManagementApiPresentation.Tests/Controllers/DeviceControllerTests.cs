using Domain.DTOs;
using Application.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DeviceManagementApiPresentation.Controllers;
using DeviceManagementDomain.Entities;

public class DeviceControllerTests
{
    private readonly Mock<IDeviceService> _mockService;
    private readonly DeviceController _controller;

    public DeviceControllerTests()
    {
        _mockService = new Mock<IDeviceService>();
        _controller = new DeviceController(_mockService.Object);
    }

    [Fact]
    public async Task AddDevice_ShouldReturnCreatedResult_WhenDeviceIsAdded()
    {
        var deviceDto = new DeviceDTO { Id = 1, Name = "Test Device", Brand = "Test Brand" };
        _mockService.Setup(s => s.AddDevice(It.IsAny<DeviceDTO>())).ReturnsAsync((int)deviceDto.Id);

        var result = await _controller.AddDevice(deviceDto);

        Assert.IsType<CreatedAtActionResult>(result);
        var createdResult = result as CreatedAtActionResult;
        Assert.Equal(deviceDto.Id, createdResult?.RouteValues["id"]);
    }
    
    [Fact]
    public async Task AddDevice_ShoulReturnBadRequestResult_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Name is required.");

        var result = await _controller.AddDevice(new DeviceDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task GetDevice_ShouldReturnOkResult_WhenDeviceExists()
    {
        var device = new Device { Id = 1, Name = "Test Device", Brand = "Test Brand" };
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync(device);

        var result = await _controller.GetDeviceById(device.Id);

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(device, okResult?.Value);
    }
    
    [Fact]
    public async Task GetDevice_ShouldReturnNotFoundResult_WhenDeviceDoesNotExist()
    {
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync((Device)null);

        var result = await _controller.GetDeviceById(1);

        Assert.IsType<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.Equal("Device with ID 1 not found.", notFoundResult?.Value);
    }
    
    [Fact]
    public async Task GetDevices_ShouldReturnOkResult_WhenDevicesExist()
    {
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Test Device 1", Brand = "Test Brand 1" },
            new Device { Id = 2, Name = "Test Device 2", Brand = "Test Brand 2" }
        };
        _mockService.Setup(s => s.GetDevicesAsync()).ReturnsAsync(devices);

        var result = await _controller.GetDevices();

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(devices, okResult?.Value);
    }
    
    [Fact]
    public async Task GetDevices_ShouldReturnOkResult_WhenNoDevicesExist()
    {
        _mockService.Setup(s => s.GetDevicesAsync()).ReturnsAsync(new List<Device>());

        var result = await _controller.GetDevices();

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Empty(okResult?.Value as IEnumerable<Device>);
    }
    
    [Fact]
    public async Task UpdateDevice_ShouldReturnOkResult_WhenDeviceIsUpdated()
    {
        var deviceDto = new DeviceDTO { Id = 1, Name = "Test Device", Brand = "Test Brand" };
        var existingDevice = new Device { Id = 1, Name = "Existing Device", Brand = "Existing Brand" };
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync(existingDevice);

        var result = await _controller.UpdateDevice((int)deviceDto.Id, deviceDto);

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(deviceDto, okResult?.Value);
    }
    
    [Fact]
    public async Task UpdateDevice_ShouldReturnNotFoundResult_WhenDeviceDoesNotExist()
    {
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync((Device)null);

        var result = await _controller.UpdateDevice(1, new DeviceDTO());

        Assert.IsType<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.Equal("Device with ID 1 not found.", notFoundResult?.Value);
    }
    
    [Fact]
    public async Task UpdateDevice_ShouldReturnBadRequestResult_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Name is required.");

        var result = await _controller.UpdateDevice(1, new DeviceDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateDevicePartial_ShouldReturnOkResult_WhenDeviceIsUpdated()
    {
        var devicePatchDto = new DevicePatchDTO { Id = 1, Name = "Test Device" };
        var existingDevice = new Device { Id = 1, Name = "Existing Device", Brand = "Existing Brand" };
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync(existingDevice);

        var result = await _controller.UpdateDevicePartial((int)devicePatchDto.Id, devicePatchDto);

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(existingDevice, okResult?.Value);
    }
    
    [Fact]
    public async Task UpdateDevicePartial_ShouldReturnNotFoundResult_WhenDeviceDoesNotExist()
    {
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync((Device)null);

        var result = await _controller.UpdateDevicePartial(1, new DevicePatchDTO());

        Assert.IsType<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.Equal("Device with ID 1 not found.", notFoundResult?.Value);
    }
    
    [Fact]
    public async Task DeleteDevice_ShouldReturnNoContent_WhenDeviceIsDeleted()
    {
        var existingDevice = new Device { Id = 1, Name = "Existing Device", Brand = "Existing Brand" };
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync(existingDevice);

        var result = await _controller.DeleteDevice(existingDevice.Id);

        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task DeleteDevice_ShouldReturnNotFoundResult_WhenDeviceDoesNotExist()
    {
        _mockService.Setup(s => s.GetDeviceByIdAsync(It.IsAny<int>())).ReturnsAsync((Device)null);

        var result = await _controller.DeleteDevice(1);

        Assert.IsType<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.Equal("Device with ID 1 not found.", notFoundResult?.Value);
    }
    
    [Fact]
    public async Task SearchDevicesByBrand_ShouldReturnOkResult_WhenDevicesExist()
    {
        var devices = new List<Device>
        {
            new Device { Id = 1, Name = "Test Device 1", Brand = "Test Brand 1" },
            new Device { Id = 2, Name = "Test Device 2", Brand = "Test Brand 2" }
        };
        _mockService.Setup(s => s.SearchDevicesByBrandAsync(It.IsAny<string>())).ReturnsAsync(devices);

        var result = await _controller.SearchDevicesByBrand("Test Brand");

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(devices, okResult?.Value);
    }
    
    [Fact]
    public async Task SearchDevicesByBrand_ShouldReturnOkResult_WhenNoDevicesExist()
    {
        _mockService.Setup(s => s.SearchDevicesByBrandAsync(It.IsAny<string>())).ReturnsAsync(new List<Device>());

        var result = await _controller.SearchDevicesByBrand("Test Brand");

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Empty(okResult?.Value as IEnumerable<Device>);
    }
    
    [Fact]
    public async Task SearchDevicesByBrand_ShouldReturnBadRequestResult_WhenBrandIsNotProvided()
    {
        var result = await _controller.SearchDevicesByBrand(string.Empty);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}