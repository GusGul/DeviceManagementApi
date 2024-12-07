using DeviceManagementDomain.Entities;
using DeviceManagementApplication.Validators;
using FluentValidation.TestHelper;

public class DeviceValidatorTests
{
    private readonly DeviceValidator _validator;

    public DeviceValidatorTests()
    {
        _validator = new DeviceValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var device = new Device { Name = "", Brand = "Brand" };

        var result = _validator.TestValidate(device);

        result.ShouldHaveValidationErrorFor(d => d.Name)
              .WithErrorMessage("Device name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var device = new Device { Name = new string('A', 101), Brand = "Brand" };

        var result = _validator.TestValidate(device);

        result.ShouldHaveValidationErrorFor(d => d.Name)
              .WithErrorMessage("Device name must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Brand_Is_Empty()
    {
        var device = new Device { Name = "Device", Brand = "" };

        var result = _validator.TestValidate(device);

        result.ShouldHaveValidationErrorFor(d => d.Brand)
              .WithErrorMessage("Device brand is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Brand_Exceeds_Max_Length()
    {
        var device = new Device { Name = "Device", Brand = new string('B', 51) };

        var result = _validator.TestValidate(device);

        result.ShouldHaveValidationErrorFor(d => d.Brand)
              .WithErrorMessage("Device brand must not exceed 50 characters.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_And_Brand_Are_Valid()
    {
        var device = new Device { Name = "Valid Device Name", Brand = "Valid Brand" };

        var result = _validator.TestValidate(device);

        result.ShouldNotHaveValidationErrorFor(d => d.Name);
        result.ShouldNotHaveValidationErrorFor(d => d.Brand);
    }
}
