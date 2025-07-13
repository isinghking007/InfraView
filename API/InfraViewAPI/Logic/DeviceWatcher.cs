using InfraViewAPI.Model;
using System.Collections.Concurrent;
using System.Management;

namespace InfraViewAPI.Logic
{
    public class DeviceWatcher
    {
        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;
        private readonly ConcurrentDictionary<string, DeviceInfo> connectedDevices;
        private readonly ConcurrentBag<DeviceLog> deviceLogs;
        private readonly MachineLogic machine;
        private readonly string machineID;
        public DeviceWatcher(ConcurrentDictionary<string, DeviceInfo> deviceDict, ConcurrentBag<DeviceLog> logs,MachineLogic machine)
        {
           this.machine = machine ?? throw new ArgumentNullException(nameof(machine));
            this.machineID = GetMachineId();
            this.connectedDevices = deviceDict;
            this.deviceLogs = logs;
          //  this.machine = machine;
        }

        public void Start()
        {
            insertWatcher = new ManagementEventWatcher(new WqlEventQuery(
                "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'"));
            insertWatcher.EventArrived += (s, e) =>
            {
                var obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string id = obj["DeviceID"]?.ToString();
                string name = obj["Name"]?.ToString();
                string type = obj["PNPClass"]?.ToString() ?? "Unknown";

                var device = new DeviceInfo
                {
                    DeviceId = id,
                    DeviceName = name,
                    DeviceType = type,
                    ConnectedAt = DateTime.UtcNow,
                    MachineId = machineID
                };

                connectedDevices[id] = device;
                deviceLogs.Add(new DeviceLog
                {
                    DeviceId = id,
                    DeviceName = name,
                    DeviceType = type,
                    MachineId = machineID,
                    DeviceStatus = "Connected",
                    Timestamp = DateTime.UtcNow
                });
            };
            insertWatcher.Start();

            removeWatcher = new ManagementEventWatcher(new WqlEventQuery(
                "SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'"));
            removeWatcher.EventArrived += (s, e) =>
            {
                var obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string id = obj["DeviceID"]?.ToString();

                if (connectedDevices.TryRemove(id, out var removed))
                {
                    deviceLogs.Add(new DeviceLog
                    {
                        DeviceId = id,
                        DeviceName = removed.DeviceName,
                        DeviceType = removed.DeviceType,
                        MachineId = machineID,
                        DeviceStatus = "Disconnected",
                        Timestamp = DateTime.UtcNow
                    });
                }
            };
            removeWatcher.Start();
        }

        public void Stop()
        {
            insertWatcher?.Stop();
            removeWatcher?.Stop();
        }

        public string GetMachineId()
        {
            var machineId = machine.DetectMachineDetails().MachineActualID;
            if (machineId == null)
            {
                machineId = MachineLogic.GetStableMachineId(); // Fixed CS0176 by qualifying with the type name  
            }
            return machineId; // Ensure the method returns the machineId  
        }
    }
}
