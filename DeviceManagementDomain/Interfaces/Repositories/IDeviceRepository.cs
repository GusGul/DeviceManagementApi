using DeviceManagementDomain.Entities;

namespace DeviceManagementDomain.Interfaces.Repositories;

public interface IDeviceRepository
{
    Task<int> AddDeviceAsync(Device device);
    Task<Device?> GetDeviceAsync(int id);
    Task<IEnumerable<Device>> GetDevicesAsync();
    Task UpdateDeviceAsync(Device device);
    Task DeleteDeviceAsync(int id);
    Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand);
}
