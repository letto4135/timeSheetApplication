using System;
using System.Collections.Generic;
using System.Linq;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using Microsoft.EntityFrameworkCore;
using Zeit.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace timeSheetApplication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        Task<EmployeeModel[]> ViewEmployeesAsync()
        {
            
        }

        Task<bool> AddEmployeeAsync(IdentityUser newEmployee)
        {

        }

        Task<bool> RemoveEmployeeAsync(Guid id)
        {
            
        }
    }
}