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

        public async Task<IActionResult> CurrentTimeSheet()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            var employee = await _employeeService.FindEmployeeById(currentUser.Id.ToString());
            var timeSheetData = await _timeSheetService.ViewTimeSheetAsync(currentUser);
            
            var model = new TimeSheetViewModel()
            {
                Items = timeSheetData,
                Employee = employee
            };
            string[] enter = new string[25];
            string[] exit = new string[25];
            string[] hoursworked = new string[25];
            string[] gross = new string[25];
            
            if(employee != null)
            {
                for(int i = 0; i < timeSheetData.Length; i++)
                {
                    enter[i] = timeSheetData[i].Enter.ToString("hh:mm:ss");
                    exit[i] = timeSheetData[i].Exit.Value.ToString("hh:mm:ss");
                    hoursworked[i] = timeSheetData[i].HoursWorked.Value.ToString(@"hh\:mm\:ss");
                    gross[i] = ((employee.rate.Value / 60.0) * (timeSheetData[i].HoursWorked.Value.Minutes)).ToString("0.00");
                }
            }

            ViewBag.gross = gross;
            ViewBag.enter = enter;
            ViewBag.exit = exit;
            ViewBag.hoursworked = hoursworked;
            ViewBag.gross = gross;

            //Console.WriteLine(model.ToString());
            return View(model);
        }

        public async Task<IActionResult> ClockIn()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var employeeClockIn = await _timeSheetService.ClockInAsync(new Guid(currentUser.Id));
            var currentTime = await _timeSheetService.CurrentClockInAsync(new Guid(currentUser.Id));
            var model = new CurrentClockInViewModel()
            {
                Item = currentTime
            };

            return View(model);
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