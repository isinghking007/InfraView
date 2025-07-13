using InfraViewAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfraViewAPI.Controllers
{
    public class DeviceController : ControllerBase
    {
        private readonly IDevice _deviceService;
        public DeviceController(IDevice deviceService)
        {
            _deviceService = deviceService;
        }

        #region Get Methods
        [HttpGet("allDevices")]
        public IActionResult GetConnectedDevices()
        {
            var devices = _deviceService.GetConnectedDevices();
            return Ok(devices);
        }

        [HttpGet("getDeviceLogs")]
        public IActionResult GetDeviceLogs()
        {
            var logs = _deviceService.GetDeviceLogs();
            return Ok(logs);
        }
        #endregion Get Methods
    }
}
