using DeviceManagementDomain.Entities;
using Domain.DTOs;

namespace Application.Services;

public interface IDeviceService
{
    Task<int> AddDevice(DeviceDTO deviceDto);
    Task<Device?> GetDeviceAsync(int id);
    Task<IEnumerable<Device>> GetDevicesAsync();
    Task UpdateDeviceAsync(DeviceDTO deviceDto);
    Task UpdateDevicePartialAsync(DevicePatchDTO devicePatchDto);
    Task DeleteDeviceAsync(int id);
    Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand);
}
