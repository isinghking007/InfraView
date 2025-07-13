using InfraViewAPI.Controllers;
using InfraViewAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace InfraViewAPI.Interfaces
{
    public interface IMachine
    {
        MachineDetails GetMachineDetails(int id);
        MachineDetails DetectMachineDetails();
    }
}
