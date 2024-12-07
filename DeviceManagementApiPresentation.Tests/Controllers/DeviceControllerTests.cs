using Application.DTOs;
using Application.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DeviceManagementApiPresentation.Controllers;

public class DeviceControllerTests
{
    private readonly Mock<DeviceService> _mockService;
    private readonly DeviceController _controller;

    public DeviceControllerTests()
    {
        _mockService = new Mock<DeviceService>();
        _controller = new DeviceController(_mockService.Object);
    }

    [Fact]
    public async Task AddDevice_ShouldReturnCreatedResult_WhenDeviceIsAdded()
    {
        // Arrange
        var deviceDto = new DeviceDTO { Id = 1, Name = "Test Device", Brand = "Test Brand" };
        _mockService.Setup(s => s.AddDevice(It.IsAny<DeviceDTO>())).ReturnsAsync((int)deviceDto.Id);

        // Act
        var result = await _controller.AddDevice(deviceDto);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        var createdResult = result as CreatedAtActionResult;
        Assert.Equal(deviceDto.Id, createdResult?.RouteValues["id"]);
    }
}