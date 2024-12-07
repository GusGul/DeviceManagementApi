using Application.DTOs;
using FluentValidation;

namespace DeviceManagementApplication.Validators;

public class DeviceDTOValidator : AbstractValidator<DeviceDTO>
{
    public DeviceDTOValidator()
    {
        RuleFor(device => device.Name)
            .NotEmpty().WithMessage("Device name is required.")
            .MaximumLength(100).WithMessage("Device name must not exceed 100 characters.");

        RuleFor(device => device.Brand)
            .NotEmpty().WithMessage("Device brand is required.")
            .MaximumLength(50).WithMessage("Device brand must not exceed 50 characters.");
    }
}
