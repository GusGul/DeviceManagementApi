using Application.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementApiPresentation.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<IActionResult> AddDevice([FromBody] DeviceDTO deviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var deviceId = await _deviceService.AddDevice(deviceDto);

        return CreatedAtAction(nameof(GetDeviceById), new { id = deviceId }, deviceDto);
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
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDto)
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

        deviceDto.Id = id;
        await _deviceService.UpdateDeviceAsync(deviceDto);

        return Ok(deviceDto);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateDevicePartial(int id, [FromBody] DevicePatchDTO devicePatchDto)
    {
        var existingDevice = await _deviceService.GetDeviceAsync(id);
        if (existingDevice == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        devicePatchDto.Id = id;
        await _deviceService.UpdateDevicePartialAsync(devicePatchDto);

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
        if (string.IsNullOrEmpty(brand))
        {
            return BadRequest("Brand query parameter is required.");
        }
        
        var devices = await _deviceService.SearchDevicesByBrandAsync(brand);
        return Ok(devices);
    }
}
