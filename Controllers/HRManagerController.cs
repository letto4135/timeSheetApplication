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
    //[Authorize(Roles = Constants.HRManager + ", " + Constants.AdministratorRole)]
    public class HRManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHRManagerService _HRManagerService;

        private readonly IEmployeeService _EmployeeService;

        public HRManagerController(UserManager<IdentityUser> userManager, IHRManagerService hrManagerService, IEmployeeService service)
        {
            _userManager = userManager;
            _HRManagerService = hrManagerService;
            _EmployeeService = service;
        }

        public async Task<IActionResult> HRMainPage()
        {
            var divisions = await _HRManagerService.GetDivisionsAsync();
            //var managerName = await _EmployeeService.FindEmployeeById(_userManager.GetUserId());
            IdentityUser[] managers = new IdentityUser[100];

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

        public async Task<IActionResult> CreateDivision(string Division, string Manager)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            var successful = await _HRManagerService.CreateDivision(Manager, Division);

            if(!successful)
            {
                return BadRequest("Could not create the division");
            }

            return RedirectToAction("HRMainPage");
        }

        public async Task<IActionResult> UpdateDivision(string Division, string Manager)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();

            var successful = await _HRManagerService.UpdateDivision(Manager, Division);

            if(!successful)
            {
                return BadRequest("Could not update the division");
            }

            return RedirectToAction("HRMainPage");
        }

        public async Task<IActionResult> SetEmployeeWage(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            
            var successful = await _HRManagerService.SetEmployeeWage(id);
            return View();
        }

        public async Task<IActionResult> RemoveDivision(Guid id)
        {
            var division = await _HRManagerService.GetDivisionAsync(id.ToString());

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

                return RedirectToAction("HRMainPage");
            }
        }
    }
}