using AutoMapper;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using Domain.DTOs;

namespace Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public DeviceService(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<int> AddDevice(DeviceDTO deviceDto)
    {
        Device device = _mapper.Map<Device>(deviceDto);
        return await _deviceRepository.AddDeviceAsync(device);
    }

    public async Task<Device?> GetDeviceByIdAsync(int id)
    {
        return await _deviceRepository.GetDeviceAsync(id);
    }

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        return await _deviceRepository.GetDevicesAsync(); 
    }

    public async Task UpdateDeviceAsync(DeviceDTO deviceDto)
    {
        Device device = _mapper.Map<Device>(deviceDto);
        await _deviceRepository.UpdateDeviceAsync(device);
    }

    public async Task UpdateDevicePartialAsync(DevicePatchDTO devicePatchDto)
    {
        Device device = _mapper.Map<Device>(devicePatchDto);
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
