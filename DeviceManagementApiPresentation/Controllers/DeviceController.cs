using Application.Services;
using DeviceManagementDomain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementApiPresentation.Controllers
{
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
            var id = await _deviceService.AddDevice(device);
            return Ok();
            //return CreatedAtAction(nameof(GetDevice), new { id }, null);
        }
    }
}
