using Application.DTOs;
using FluentValidation;

namespace DeviceManagementApplication.Validators;

public class DevicePatchDTOValidator : AbstractValidator<DeviceDTO>
{
    public DevicePatchDTOValidator()
    {
        RuleFor(device => device.Name)
            .MaximumLength(100).WithMessage("Device name must not exceed 100 characters.");

        RuleFor(device => device.Brand)
            .MaximumLength(50).WithMessage("Device brand must not exceed 50 characters.");
    }
}
