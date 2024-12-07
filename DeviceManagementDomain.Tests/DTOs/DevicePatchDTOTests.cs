using Domain.DTOs;

public class DevicePatchDTOTests
{
    [Fact]
    public void Device_ShouldInitializeCorrectly()
    {
        var device = new DevicePatchDTO() { Id = 1, Name = "Test Device", Brand = "Test Brand" };

        Assert.Equal(1, device.Id);
        Assert.Equal("Test Device", device.Name);
        Assert.Equal("Test Brand", device.Brand);
    }
}