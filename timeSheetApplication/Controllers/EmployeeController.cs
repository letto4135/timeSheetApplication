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
using Microsoft.AspNetCore.Routing;

namespace timeSheetApplication.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeSheetService _timeSheetService;

        public EmployeeController(UserManager<EmployeeModel> userManager, IEmployeeService employeeService, ITimeSheetService service)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _timeSheetService = service;
        }
        /// <summary>
        /// This method finds the employees timesheets and displays the information
        /// the enter time and the exit time along with the hours worked in that time period, along with the gross 
        /// </summary>
        /// <returns>ViewModel of the employees timesheet</returns>
        public async Task<IActionResult> CurrentTimeSheet()
        {
            // ensure current user is logged in
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            // find current user and their timesheet data
            var employee = await _employeeService.FindEmployeeById(currentUser.Id.ToString());
            var timeSheetData = await _timeSheetService.ViewTimeSheetAsync(currentUser, DateTime.Now); // pass in the current user and the date time 
            
            var model = new TimeSheetViewModel() //creating a TimeSheetViewModel to store the timesheetdata, and the employee
            {
                Items = timeSheetData,
                Employee = employee
            };
            // current time period should not be anymore than 15 days, making 20 just in case employee error
            string[] enter = new string[20];
            string[] exit = new string[20];
            string[] hoursworked = new string[20];
            string[] gross = new string[20];
            string[] date = new string[20];
            TimeSpan totalHours = new TimeSpan(0);
            double totalGross = 0;
            if(employee != null) //checking to make sure that the employee is not null
            {
                for(int i = 0; i < timeSheetData.Length; i++)
                {
                    totalHours = totalHours.Add(timeSheetData[i].HoursWorked.Value);
                    if(timeSheetData[i].Approved != 2)
                    {
                        totalGross += ((employee.rate / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes));
                    }
                    date[i] = timeSheetData[i].Enter.Date.ToString("MM/dd/yyyy"); //formatting the date time and storing it in our string array dates
                    enter[i] = timeSheetData[i].Enter.ToString("hh:mm");
                    exit[i] = timeSheetData[i].Exit.Value.ToString("hh:mm");
                    hoursworked[i] = timeSheetData[i].HoursWorked.Value.ToString(@"hh\:mm");
                    totalHours.Add(timeSheetData[i].HoursWorked.Value);
                    gross[i] = ((employee.rate / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes)).ToString("0.00"); // we are using rounding here in order to only pay employees by the minute rather than by the second
                }
            }
            ViewBag.totalHours = ((totalHours.Days * 24) + totalHours.Hours).ToString() + ":" + totalHours.Minutes.ToString("00");
            ViewBag.totalGross = totalGross;
            ViewBag.beginDate = date[0];
            ViewBag.endDate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.date = date;
            ViewBag.enter = enter;
            ViewBag.exit = exit;
            ViewBag.hoursworked = hoursworked;
            ViewBag.gross = gross;

            return View(model);
        }

        public async Task<IActionResult> PreviousTimeSheet(DateTime timeRecord)
        {
            if(timeRecord.Year == 0001)
            {
                return Redirect("/Employee/CurrentTimeSheet");
            }
            // ensure current user is logged in
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            // find current user and their timesheet data
            var employee = await _employeeService.FindEmployeeById(currentUser.Id.ToString());
            // timeRecord is the 'oldest' date in the current time period 
            var timeSheetData = await _timeSheetService.ViewPastTimeSheetAsync(currentUser, timeRecord);
            
            var model = new TimeSheetViewModel()
            {
                Items = timeSheetData,
                Employee = employee
            };

            string[] enter = new string[20];
            string[] exit = new string[20];
            string[] hoursworked = new string[20];
            string[] gross = new string[20];
            string[] date = new string[20];
            TimeSpan totalHours = new TimeSpan(0);
            double totalGross = 0;
            if(employee != null) //checking to make sure that the employee is not null
            {
                for(int i = 0; i < timeSheetData.Length; i++)
                {
                    totalHours = totalHours.Add(timeSheetData[i].HoursWorked.Value);
                    totalGross += ((employee.rate / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes));
                    //formatting the date time and storing it in our string array dates
                    date[i] = timeSheetData[i].Enter.Date.ToString("MM/dd/yyyy"); 
                    enter[i] = timeSheetData[i].Enter.ToString("hh:mm");
                    exit[i] = timeSheetData[i].Exit.Value.ToString("hh:mm");
                    hoursworked[i] = timeSheetData[i].HoursWorked.Value.ToString(@"hh\:mm");
                    // we are using rounding here in order to only pay employees by the minute rather than by the second
                    gross[i] = ((employee.rate / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes)).ToString("00.0"); 
                }
                if(timeSheetData.Length > 0)
                {
                    if(timeSheetData[0].Enter.Day < 16)
                    {
                        ViewBag.endDate = new DateTime(timeSheetData[0].Enter.Year, 
                        timeSheetData[0].Enter.Month, 15).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        ViewBag.endDate = new DateTime
                        (timeSheetData[0].Enter.Year, timeSheetData[0].Enter.Month, 
                        DateTime.DaysInMonth(timeSheetData[0].Enter.Year,
                        timeSheetData[0].Enter.Month)).ToString("MM/dd/yyyy");
                    }
                }

            }

            ViewBag.beginDate = date[0];
            ViewBag.totalHours = ((totalHours.Days * 24) + totalHours.Hours).ToString() + ":" + totalHours.Minutes.ToString("00");
            ViewBag.totalGross = totalGross;
            ViewBag.date = date;
            ViewBag.enter = enter;
            ViewBag.exit = exit;
            ViewBag.hoursworked = hoursworked;
            ViewBag.gross = gross;

            return View(model);
        }

        /// <summary>
        /// Method checks to see if a user is logged in, and if not forces the user to log in
        /// allows a user to clock in generates a GUID with the current users Id
        /// </summary>
        /// <returns>ViewModel</returns>
        [ValidateAntiForgeryToken] 
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
        /// <summary>
        /// Allows an employee to clock out as long as they havent
        /// already clocked out, if so it will return a bad request  
        /// </summary>
        /// <returns>Task</returns>
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> ClockOut()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var employeeClockOut = await _timeSheetService.ClockOutAsync(new Guid(currentUser.Id));

            if(!employeeClockOut)
            {
                var routeValues = new RouteValueDictionary
                {
                    {"error", "Could not clock out. Were you not clocked in?"}
                };
                return RedirectToAction("Error", "Home", routeValues);
            }
            return View();
        } 
    }
}