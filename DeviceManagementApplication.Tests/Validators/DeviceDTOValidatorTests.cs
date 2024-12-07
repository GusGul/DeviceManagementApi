using DeviceManagementApplication.Validators;
using Domain.DTOs;
using FluentValidation.TestHelper;

public class DeviceDTOValidatorTests
{
    private readonly DeviceDTOValidator _validator;

    public DeviceDTOValidatorTests()
    {
        _validator = new DeviceDTOValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var deviceDto = new DeviceDTO { Name = string.Empty, Brand = "Valid Brand" };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldHaveValidationErrorFor(d => d.Name)
              .WithErrorMessage("Device name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Brand_Is_Empty()
    {
        var deviceDto = new DeviceDTO { Name = "Valid Device", Brand = string.Empty };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldHaveValidationErrorFor(d => d.Brand)
              .WithErrorMessage("Device brand is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var deviceDto = new DeviceDTO { Name = new string('A', 101), Brand = "Valid Brand" };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldHaveValidationErrorFor(d => d.Name)
              .WithErrorMessage("Device name must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Brand_Exceeds_Max_Length()
    {
        var deviceDto = new DeviceDTO { Name = "Valid Device", Brand = new string('B', 51) };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldHaveValidationErrorFor(d => d.Brand)
              .WithErrorMessage("Device brand must not exceed 50 characters.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var deviceDto = new DeviceDTO { Name = "Valid Device", Brand = "Valid Brand" };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldNotHaveValidationErrorFor(d => d.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Brand_Is_Valid()
    {
        var deviceDto = new DeviceDTO { Name = "Valid Device", Brand = "Valid Brand" };

        var result = _validator.TestValidate(deviceDto);

        result.ShouldNotHaveValidationErrorFor(d => d.Brand);
    }
}
