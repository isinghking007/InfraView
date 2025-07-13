using System.Runtime.CompilerServices;

namespace InfraViewAPI.Model
{
    public class MachineDetails
    {
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string OS { get; set; }
        public string MachineActualID { get; set; }
        public string IPAddress { get; set; }
        public DateTime RegisterdAt { get; set; }
    }
}
