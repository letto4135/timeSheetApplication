using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using timeSheetApplication.Models;
using timeSheetApplication.Services;


namespace timeSheetApplication.Controllers
{
    [Authorize(Roles = Constants.HRManager + ", " + Constants.AdministratorRole)]
    public class EmployeeManagementController : Controller
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly IHRManagerService _HRManagerService;

        private readonly IEmployeeService _EmployeeService;

        public EmployeeManagementController(UserManager<EmployeeModel> userManager, IHRManagerService hrManagerService, IEmployeeService service)
        {
            _userManager = userManager;
            _HRManagerService = hrManagerService;
            _EmployeeService = service;
        }
     
     /// <summary>
     /// This method gets the employee email, the Division along with the wage and exempt status
     /// and allows you to update their division, wage, exempt status, or you can delete an employee from here also
     /// </summary>
     /// <returns>ViewModel</returns>
        public async Task<IActionResult> EmployeeManagement()
        {
            var divisions = await _HRManagerService.GetDivisionsAsync();
            EmployeeModel[] managers = new EmployeeModel[100];

            for(int i = 0; i < divisions.Length; i++)
            {
                managers[i] = await _userManager.FindByIdAsync(divisions.ElementAt(i).managerId.ToString());
            }

            var employees = await _EmployeeService.ViewEmployeesAsync();

            var model = new DivisionsViewModel()
            {
                Divisions = divisions,
                Managers = managers,
                Employees = employees
            };

            return View(model);
        }
        
        /// <summary>
        /// this method searches for the id being passed into the method, if the id is found and is not
        /// the admin Id or manager ID it will remove the employee, otherwise it will return a badrequest
        /// if successful it will redirect to EmployeeManagement and display the update
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task</returns>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployee(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();

            var success = await _EmployeeService.RemoveEmployeeAsync(id);

            if(!success)
            {
                return BadRequest("Could not delete user, are they a manager? Managers cannot be deleted. Change manager first.");
            }

            return RedirectToAction("EmployeeManagement");
        }

        /// <summary>
        /// This method allows you to update an employee, their division, rate and exempt status, and if successful
        /// it will redirect to EmployeeManagement and display the result
        /// </summary>
        /// <param name="Division"></param>
        /// <param name="Rate"></param>
        /// <param name="Exempt"></param>
        /// <param name="id"></param>
        /// <returns>Task</returns>
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> UpdateEmployee(string Division, string Rate, string Exempt, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            
            var successful = await _EmployeeService.UpdateEmployee(Division, Rate, Exempt, id);

            if(! successful)
            {
                return BadRequest("Is the employee a manager? You may be trying to remove them from the division they manage.");
            }
            return RedirectToAction("EmployeeManagement");
        }
    }
}