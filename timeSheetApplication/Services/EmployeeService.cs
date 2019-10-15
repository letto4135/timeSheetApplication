using System;
using System.Collections.Generic;
using System.Linq;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
 
namespace timeSheetApplication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;

        public EmployeeService(ApplicationDbContext context, UserManager<EmployeeModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<EmployeeModel[]> ViewEmployeesAsync()
        {
            // returns all employees ordered by email address
            return await _context.Employees
                .OrderBy(x => x.Email)
                .ToArrayAsync();
        }

        public async Task<bool> RemoveEmployeeAsync(String id)
        {
            // get the user to be removed
            var user = await _userManager.FindByIdAsync(id);

            // if user cannot be found return false
            if(user == null)
            {
                return false;
            }
            // if user is main admin account return false
            else if(user.Email.Equals("admin@timesheet.local"))
            {
                return false;
            }
            else
            {
                // get the divisions
                var divisions = await _context.Divisions
                .ToArrayAsync();

                // loop through divisions
                foreach(var division in divisions)
                {
                    // checking if the manager is the employee being deleted
                    if(division.managerId.ToString().Equals(id))
                    {
                        // if it is, dont delete, return false instead.
                        return false;
                    }
                }
                // delete employee
                await _userManager.DeleteAsync(user);
                var success = await _context.SaveChangesAsync();
                return success == 1;
            }
            
        }

        public async Task<EmployeeModel> FindEmployeeById(String id)
        {
            // find user where id passed in == user.Id
            var user = await _context.Employees
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
            // if user is not found return null
            if (user == null)
            {
               return null;
            }
            // else return the user
            else
            {
                return user;
            }
        }

        public async Task<bool> UpdateEmployee(string Division, string Rate, string Exempt, string id)
        {
            // get employee to be updated by Id
            var employee = await _context.Employees
                .Where(x => x.Id.ToString().Equals(id))
                .FirstOrDefaultAsync();
            // get divisions
            var divisions = await _context.Divisions
                .ToArrayAsync();

            // loop through divisions
            foreach(var division in divisions)
            {
                // checking if the manager is the employee being updated
                if(division.managerId.ToString().Equals(id))
                {
                    // if manager is the employee to be updated
                    // check that the division is not being updated
                    // if it is return false
                    if(! division.Division.Equals(Division))
                    {
                        return false;
                    }
                }
            }
            // set division, rate, and exempt reguardless of whether they
            // actually changed or not. It's easier to set all 3 than to
            // parse which ones have changed from the input.
            employee.division = Division;
            employee.rate = Double.Parse(Rate);
            employee.exempt = Boolean.Parse(Exempt);

            var success = await _context.SaveChangesAsync();

            return success == 1;
        }
    }
}