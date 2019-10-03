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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            /**
            index will default to clock in clock out
             */
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            /*else
            {
                RedirectToAction("");
            }*/
            //var clockin = await _timeSheetService.ClockInAsync();

            return View(); // pass in clock in view
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrentTimeSheet()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var timeSheetData = _timeSheetService.ViewTimeSheetAsync(currentUser.Id);
            
            return View(timeSheetData);
        }

        public async Task<IActionResult> ClockInAsync()
        {
            Console.WriteLine("employeeClockIn");
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            //newTime.Enter(DateTime.Now);
            //var timeSheet = _timeSheetService.ViewTimeSheetAsync(currentUser.Id);

            var employeeClockIn = await _timeSheetService.ClockInAsync(new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClockOutAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var employeeClockOut = _timeSheetService.ClockOutAsync(currentUser.Id);

            return View(employeeClockOut);
        }
    }
}