namespace InfraViewAPI.Model
{
    public class DeviceLog
    {
       
            public string DeviceId { get; set; }
            public string DeviceName { get; set; }
            public string DeviceType { get; set; }
            public string MachineId { get; set; }
            public string DeviceStatus { get; set; } // Connected or Disconnected
            public DateTime Timestamp { get; set; }
        
    }
}
