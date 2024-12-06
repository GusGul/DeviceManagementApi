using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;

namespace Application.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<int> AddDevice(Device device)
        {
            return await _deviceRepository.AddDeviceAsync(device);
        }

        public async Task<Device?> GetDeviceAsync(int id)
        {
            return await _deviceRepository.GetDeviceAsync(id);
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            return await _deviceRepository.GetDevicesAsync(); 
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            await _deviceRepository.UpdateDeviceAsync(device);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            await _deviceRepository.DeleteDeviceAsync(id);
        }

        public async Task<IEnumerable<Device>> SearchDevicesByBrandAsync(string brand)
        {
            return await _deviceRepository.SearchDevicesByBrandAsync(brand);
        }
    }
}
