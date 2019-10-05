using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Models;

namespace timeSheetApplication.Services
{
    public interface IEmployeeService
    {
        Task<IdentityUser[]> ViewEmployeesAsync();

        Task<bool> RemoveEmployeeAsync(String id);

       // Task<EmployeeModel> FindEmployeeById(String id);
    }
}