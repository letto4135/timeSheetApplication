using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timeSheetApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using timeSheetApplication.Services;
using Zeit.Models;

namespace timeSheetApplication.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeSheetService _timeSheetService;

        public EmployeeController(UserManager<IdentityUser> userManager, IEmployeeService employeeService, ITimeSheetService service)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _timeSheetService = service;
        }

        public async Task<IActionResult> Index()
        {
            /**
            index will default to clock in clock out
             */
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            return View(); // pass in clock in view
        }

        public async Task<IActionResult> CurrentTimeSheet()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var timeSheetData = _timeSheetService.ViewTimeSheetAsync(currentUser.Id);
            
            return View(timeSheetData);
        }

        public async Task<IActionResult> ClockIn()
        {
            Console.WriteLine("employeeClockIn");
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var employeeClockIn = await _timeSheetService.ClockInAsync(new Guid(currentUser.Id));
            return View();
        }

        public async Task<IActionResult> ClockOut()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var employeeClockOut = await _timeSheetService.ClockOutAsync(new Guid(currentUser.Id));

            if(!employeeClockOut)
            {
                return BadRequest("Could not clock out properly.");
            }
            return View();
        }
    }
}