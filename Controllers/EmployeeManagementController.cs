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
    [Authorize(Roles = Constants.HRManager + ", " + Constants.AdministratorRole + ", " + Constants.Manager)]
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
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployee(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();

            var success = await _EmployeeService.RemoveEmployeeAsync(id);

            if(!success)
            {
                return BadRequest("Could not delete user");
            }

            return RedirectToAction("EmployeeManagement");
        }

        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> UpdateEmployee(string Division, string Rate, string Exempt, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            
            var successful = await _HRManagerService.UpdateEmployee(Division, Rate, Exempt, id);
            return RedirectToAction("EmployeeManagement");
        }
    }
}