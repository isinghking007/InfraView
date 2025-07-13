using InfraViewAPI.Model;
using System.Management;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using InfraViewAPI.Interfaces;

namespace InfraViewAPI.Logic
{
    public class MachineLogic:IMachine
    {
        public MachineLogic()
        {

        }

        public  MachineDetails DetectMachineDetails()
        {
            var info = new MachineDetails
            {
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                OS = RuntimeInformation.OSDescription,
                MachineActualID = GetStableMachineId(),
                IPAddress = GetLocalIPAddress(),
                RegisterdAt = DateTime.Now
            };
            return info;
        }
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        public static string GetStableMachineId()
        {
            string cpuId = GetWmiValue("Win32_Processor", "ProcessorId");
            string boardSerial = GetWmiValue("Win32_BaseBoard", "SerialNumber");
            string biosUuid = GetWmiValue("Win32_BIOS", "SerialNumber");

            string rawId = $"{cpuId}-{boardSerial}-{biosUuid}";
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(rawId));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private static string GetWmiValue(string wmiClass, string property)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher($"SELECT {property} FROM {wmiClass}"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj[property]?.ToString() ?? "";
                    }
                }
            }
            catch { }
            return "";
        }

        MachineDetails IMachine.GetMachineDetails(int id)
        {
            throw new NotImplementedException();
        }

      
    }
}
