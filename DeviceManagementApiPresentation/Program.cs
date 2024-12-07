using Application.Services;
using DeviceManagementDomain.Interfaces.Repositories;
using DeviceManagementInfrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using System.Data;
using DeviceManagementApplication.Validators;
using DeviceManagementInfrastructure.Factories;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<DeviceValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeviceDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DevicePatchDTOValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<IDbConnection>(_ =>
    new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();