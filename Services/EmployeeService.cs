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
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<EmployeeModel[]> ViewEmployeesAsync()
        {
            return await _context.Employees
                .ToArrayAsync();
        }

        // dont need add employee, identity can handle it.
        public async Task<bool> RemoveEmployeeAsync(String id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return false;
            }
            else if(user.Email.Equals("admin@timesheet.local"))
            {
                return false;
            }
            else
            {
                var success = await _userManager.DeleteAsync(user);

                if(!success.Succeeded)
                {
                    return false;
                }
                return true;
            }
            
        }
    }
}