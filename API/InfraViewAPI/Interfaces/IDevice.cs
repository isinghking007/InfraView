using InfraViewAPI.Model;

namespace InfraViewAPI.Interfaces
{
    public interface IDevice
    {
        List<DeviceInfo> GetConnectedDevices();
        List<DeviceLog> GetDeviceLogs(); 
    }
}
