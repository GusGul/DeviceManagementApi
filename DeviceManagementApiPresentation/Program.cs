using Application.Services;
using DeviceManagementDomain.Interfaces.Repositories;
using DeviceManagementInfrastucture.Repositories;
using FluentValidation.AspNetCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Plan to change this (method is obsolet, there is a new method to use), but for now, it's fine
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<Program>();
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<DeviceService>();

builder.Services.AddSingleton<IDbConnection>(_ =>
    new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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