using InfraViewAPI.Interfaces;
using InfraViewAPI.Model;
using System.Collections.Concurrent;
using System.Management;

namespace InfraViewAPI.Logic
{
    public class DeviceLogic:IDevice
    {

        private readonly ConcurrentDictionary<string, DeviceInfo> connectedDevices;
        private readonly ConcurrentBag<DeviceLog> deviceLogs;
        private readonly MachineLogic machine;
        public DeviceLogic(ConcurrentDictionary<string, DeviceInfo> deviceDict, ConcurrentBag<DeviceLog> logs,MachineLogic machine)
        {
            this.machine = machine;
            this.connectedDevices = deviceDict;
            this.deviceLogs = logs;
        }

        public List<DeviceInfo> GetConnectedDevices()
        {
            var machineId = machine.DetectMachineDetails().MachineActualID;
            return connectedDevices.Values
                .Where(d => d.MachineId == machineId)
                .ToList();
        }

        public List<DeviceLog> GetDeviceLogs()
        {
            var machineId = machine.DetectMachineDetails().MachineActualID;
            return deviceLogs
                .Where(l => l.MachineId == machineId)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }
    }
}
