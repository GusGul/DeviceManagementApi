using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;

namespace Application.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddDevice(Device device)
        {
            return await _repository.AddDeviceAsync(device);
        }
    }
}
