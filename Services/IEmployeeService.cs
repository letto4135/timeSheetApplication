using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Zeit.Models;

namespace timeSheetApplication.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeModel[]> ViewEmployeesAsync();

        Task<bool> RemoveEmployeeAsync(String id);
    }
}