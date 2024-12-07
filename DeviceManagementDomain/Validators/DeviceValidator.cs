using DeviceManagementDomain.Entities;
using FluentValidation;

namespace DeviceManagementDomain.Validators;

public class DeviceValidator : AbstractValidator<Device>
{
    public DeviceValidator()
    {
        RuleFor(device => device.Name)
            .NotEmpty().WithMessage("Device name is required.")
            .MaximumLength(100).WithMessage("Device name must not exceed 100 characters.");

        RuleFor(device => device.Brand)
            .NotEmpty().WithMessage("Device brand is required.")
            .MaximumLength(50).WithMessage("Device brand must not exceed 50 characters.");
    }
}
