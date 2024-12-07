using Application.Services;
using DeviceManagementDomain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementApiPresentation.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DeviceController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<IActionResult> AddDevice([FromBody] Device device)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var deviceId = await _deviceService.AddDevice(device);

        return CreatedAtAction(nameof(GetDeviceById), new { id = deviceId }, device);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeviceById(int id)
    {
        var device = await _deviceService.GetDeviceAsync(id);
        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        return Ok(device);
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await _deviceService.GetDevicesAsync();
        return Ok(devices);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] Device device)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingDevice = await _deviceService.GetDeviceAsync(id);
        if (existingDevice == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        device.Id = id;
        await _deviceService.UpdateDeviceAsync(device);

        return Ok(device);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateDevicePartial(int id, [FromBody] Device device)
    {
        var existingDevice = await _deviceService.GetDeviceAsync(id);
        if (existingDevice == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        if (!string.IsNullOrEmpty(device.Name))
        {
            existingDevice.Name = device.Name;
        }
        if (!string.IsNullOrEmpty(device.Brand))
        {
            existingDevice.Brand = device.Brand;
        }

        await _deviceService.UpdateDeviceAsync(existingDevice);

        return Ok(existingDevice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var existingDevice = await _deviceService.GetDeviceAsync(id);
        if (existingDevice == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        await _deviceService.DeleteDeviceAsync(id);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchDevicesByBrand([FromQuery] string brand)
    {
        var devices = await _deviceService.SearchDevicesByBrandAsync(brand);
        return Ok(devices);
    }
}
