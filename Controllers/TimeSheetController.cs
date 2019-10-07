using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using timeSheetApplication.Models;
using timeSheetApplication.Services;

namespace timeSheetApplication.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeSheetService _timeSheetService;

        public TimeSheetController(UserManager<EmployeeModel> userManager, IEmployeeService employeeService, ITimeSheetService service)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _timeSheetService = service;
        }
        
        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(User);
            var employees = await _employeeService.ViewEmployeesAsync();
            if (currentUser == null) return Challenge();

            var timeSheetData = await _timeSheetService.ListUnapprovedAsync();
            if(timeSheetData.Length != 0)
            {
                var model = new AllTimeSheetsViewModel()
                {
                    Items = timeSheetData
                };

                string[] enter = new string[25];
                string[] exit = new string[25];
                string[] hoursworked = new string[25];
                string[] date = new string[25];
                string[] rates = new string[25];
                string[] gross = new string[25];
                string[] employee = new string[25];
                for(int i = 0; i < timeSheetData.Length && i < 25; i++)
                {
                    date[i] = timeSheetData[i].Enter.Date.ToString("MM/dd/yyyy");
                    enter[i] = timeSheetData[i].Enter.ToString("hh:mm");
                    exit[i] = timeSheetData[i].Exit.Value.ToString("hh:mm");
                    hoursworked[i] = timeSheetData[i].HoursWorked.Value.ToString(@"hh\:mm");
                    rates[i] = _employeeService.FindEmployeeById(timeSheetData[i].EmployeeId.ToString()).Result.rate.ToString();
                    gross[i] = ((Double.Parse(rates[i]) / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes)).ToString("0.00");
                    employee[i] = _employeeService.FindEmployeeById(timeSheetData[i].EmployeeId.ToString()).Result.Email.ToString();
                }

                ViewBag.date = date;
                ViewBag.enter = enter;
                ViewBag.exit = exit;
                ViewBag.hoursworked = hoursworked;
                ViewBag.rates = rates;
                ViewBag.gross = gross;
                ViewBag.employee = employee;

                return View("~/Views/TimeSheet/Index.cshtml" ,model);
            }
            else
            {
                return View("~/Views/TimeSheet/NothingHere.cshtml");
            }
            
        }

        public async Task<IActionResult> DenyTime(string Message, string TimeSheetId)
        {
            var TimeSheetData = await _timeSheetService.ListUnapprovedAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MassApprove()
        {
            var timeSheetData = await _timeSheetService.ListUnapprovedAsync();
            string[] timesheets = new string[25];

            for(int i = 0; i < timeSheetData.Length && i < 25; i++)
            {
                timesheets[i] = timeSheetData[i].Id.ToString();
            }

            var success = await _timeSheetService.MassApproveAsync(timesheets);

            if(!success)
            {
                return BadRequest("Could not approve TimeSheets.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrintChecks()
        {
            return View("~/Views/HR/PrintChecks.cshtml");
        }
    }
}