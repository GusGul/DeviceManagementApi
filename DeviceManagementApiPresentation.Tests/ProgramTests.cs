using System.Data;
using Application.Services;
using DeviceManagementApplication.Validators;
using DeviceManagementDomain.Entities;
using DeviceManagementDomain.Interfaces.Repositories;
using DeviceManagementInfrastructure.Repositories;
using DeviceManagementInfrastructure.Factories;
using Domain.DTOs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using Serilog;

namespace DeviceManagementTests;

public class ProgramTests
{
    private readonly ServiceProvider _serviceProvider;

    public ProgramTests()
    {
        var serviceCollection = new ServiceCollection();
        var builder = WebApplication.CreateBuilder(new string[] { });

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<DeviceValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<DeviceDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<DevicePatchDTOValidator>();
        builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddSingleton<LogService>();
        builder.Services.AddSingleton<IDbConnection>(_ =>
            new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        _serviceProvider = builder.Services.BuildServiceProvider();
    }

    [Fact]
    public void Should_Register_Services_Correctly()
    {
        var deviceRepository = _serviceProvider.GetService<IDeviceRepository>();
        var deviceService = _serviceProvider.GetService<IDeviceService>();
        var logService = _serviceProvider.GetService<LogService>();
        var dbConnection = _serviceProvider.GetService<IDbConnection>();

        Assert.NotNull(deviceRepository);
        Assert.NotNull(deviceService);
        Assert.NotNull(logService);
        Assert.NotNull(dbConnection);
    }

    [Fact]
    public void Should_Register_Validators_Correctly()
    {
        var deviceValidator = _serviceProvider.GetService<IValidator<Device>>();
        var deviceDTOValidator = _serviceProvider.GetService<IValidator<DeviceDTO>>();

        Assert.NotNull(deviceValidator);
        Assert.NotNull(deviceDTOValidator);
    }

    [Fact]
    public void Should_Register_Logger_Correctly()
    {
        var logger = _serviceProvider.GetService<ILogger<LogService>>();

        Assert.NotNull(logger);
    }

    [Fact]
    public void Should_Configure_Serilog_Logger()
    {
        var loggerMock = new Mock<ILogger<LogService>>();
        
        var logService = new LogService(loggerMock.Object);
        logService.LogMessage("Test log");

        loggerMock.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((state, t) => state.ToString() == "Test log"), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public void Should_Register_Database_Connection()
    {
        var dbConnection = _serviceProvider.GetService<IDbConnection>();

        Assert.IsType<NpgsqlConnection>(dbConnection);
        Assert.NotNull(dbConnection.ConnectionString);
    }
}
