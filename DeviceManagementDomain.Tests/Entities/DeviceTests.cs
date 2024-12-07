using DeviceManagementDomain.Entities;

public class DeviceTests
{
    [Fact]
    public void Device_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var device = new Device { Id = 1, Name = "Test Device", Brand = "Test Brand" };

        // Assert
        Assert.Equal(1, device.Id);
        Assert.Equal("Test Device", device.Name);
        Assert.Equal("Test Brand", device.Brand);
    }
}