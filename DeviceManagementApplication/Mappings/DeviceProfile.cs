using Domain.DTOs;
using AutoMapper;
using DeviceManagementDomain.Entities;

namespace Application.Mappings;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDTO>().ReverseMap();
        
        CreateMap<Device, DevicePatchDTO>().ReverseMap();
    }
}
