using Application.DTOs;
using AutoMapper;
using DeviceManagementDomain.Entities;

namespace Application.Mappings;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDTO>();

        CreateMap<DeviceDTO, Device>();
    }
}
