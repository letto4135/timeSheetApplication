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
    [Authorize(Roles = Constants.HRManager + ", " + Constants.AdministratorRole)] // authorize only HrManager and Admin roles to acess this
    public class HRManagerController : Controller
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly IHRManagerService _HRManagerService;
        private readonly IEmployeeService _EmployeeService;

        public HRManagerController(UserManager<EmployeeModel> userManager, IHRManagerService hrManagerService, IEmployeeService service)
        {
            _userManager = userManager;
            _HRManagerService = hrManagerService;
            _EmployeeService = service;
        }
        /// <summary>
        /// This method allows you to create a division and select a manager
        /// or it will allow you to update a division
        /// /// </summary>
        /// <returns>ViewModel Of the divisions</returns>
        public async Task<IActionResult> HRMainPage()
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
        public async Task<IActionResult> CreateDivision(string Division, string ManagerEmail)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            var successful = await _HRManagerService.CreateDivision(ManagerEmail, Division);

            if(!successful)
            {
                return BadRequest("Could not create the division");
            }

            return RedirectToAction("HRMainPage");
        }
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> UpdateDivision(string Division, string ManagerEmail)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();

            var successful = await _HRManagerService.UpdateDivision(ManagerEmail, Division);

            if(!successful)
            {
                return BadRequest("Could not update the division");
            }

            return RedirectToAction("HRMainPage");
        }

        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> RemoveDivision(Guid id)
        {
            var division = await _HRManagerService.GetDivisionAsync(id.ToString());
            var employees = await _EmployeeService.ViewEmployeesAsync();

            if(division == null)
            {
                return BadRequest("Could not locate a user to delete user.");
            }
            else
            {
                var successful = await _HRManagerService.RemoveDivision(division);

                if(!successful)
                {
                    return BadRequest("Could not delete division.");
                }

                foreach(var employee in employees)
                {
                    if(employee.division.Equals(division.Division))
                    {
                        await _EmployeeService.UpdateEmployee("No Division", employee.rate.ToString(), "False", employee.Id);
                    }
                }

                return RedirectToAction("HRMainPage");
            }
        }
    }
}