using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using timeSheetApplication.Models;
using timeSheetApplication.Services;

namespace timeSheetApplication.Controllers
{
    [Authorize(Roles= Constants.AdministratorRole + ", " + Constants.HRManager + ", " + Constants.Manager)]
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
            var currentUser = await _userManager.GetUserAsync(User);
            var employees = await _employeeService.ViewEmployeesAsync();
            if (currentUser == null) return Challenge();

            var timeSheetData = await _timeSheetService.ListUnapprovedAsync();
            if(timeSheetData.Length != 0)
            {
                var model = new AllTimeSheetsViewModel()
                {
                    Items = timeSheetData
                };

                string[] enter = new string[10];
                string[] exit = new string[10];
                string[] hoursworked = new string[10];
                string[] date = new string[10];
                string[] rates = new string[10];
                string[] gross = new string[10];
                string[] employee = new string[10];
                for(int i = 0; i < timeSheetData.Length && i < 10; i++)
                {
                    var emp = await _employeeService.FindEmployeeById(timeSheetData[i].EmployeeId.ToString());
                    if(emp.division.Equals(currentUser.division) || User.IsInRole(Constants.AdministratorRole) || User.IsInRole(Constants.HRManager))
                    {
                        date[i] = timeSheetData[i].Enter.Date.ToString("MM/dd/yyyy");
                        enter[i] = timeSheetData[i].Enter.ToString("hh:mm");
                        exit[i] = timeSheetData[i].Exit.Value.ToString("hh:mm");
                        hoursworked[i] = timeSheetData[i].HoursWorked.Value.ToString(@"hh\:mm");
                        rates[i] = _employeeService.FindEmployeeById(timeSheetData[i].EmployeeId.ToString()).Result.rate.ToString();
                        gross[i] = ((Double.Parse(rates[i]) / 60.0) * Math.Round(timeSheetData[i].HoursWorked.Value.TotalMinutes)).ToString("0.00");
                        employee[i] = _employeeService.FindEmployeeById(timeSheetData[i].EmployeeId.ToString()).Result.Email.ToString();
                    }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyTime(string Message, string TimeSheetId)
        {
            var TimeSheetData = await _timeSheetService.GetUnapprovedById(new Guid(TimeSheetId));
            var success = await _timeSheetService.DenyTime(TimeSheetData[0]);

            if(success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var routeValues = new RouteValueDictionary
                {
                    {"error", "Could not deny time."}
                };
                return RedirectToAction("Error", "Home", routeValues);
            }
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MassApprove()
        {
            var timeSheetData = await _timeSheetService.ListUnapprovedAsync();
            string[] timesheets = new string[10];

            for(int i = 0; i < timeSheetData.Length && i < 10; i++)
            {
                timesheets[i] = timeSheetData[i].Id.ToString();
            }

            var success = await _timeSheetService.MassApproveAsync(timesheets);

            if(!success)
            {
                var routeValues = new RouteValueDictionary
                {
                    {"error", "Could not approve TimeSheets."}
                };
                return RedirectToAction("Error", "Home", routeValues);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.AdministratorRole + ", " + Constants.HRManager)]
        public IActionResult PrintChecks()
        {
            EmployeeModel[] employees = _employeeService.GetAllEmployees();
            TimeSheetModel[] timesheets;
            
            if(DateTime.Now.Day > 15)
            {
                timesheets = _timeSheetService.GetAllWithinRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), 
                                                                 new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15));
            }
            else
            {
                timesheets = _timeSheetService.GetAllWithinRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16), 
                                                                 new DateTime(DateTime.Now.Year, DateTime.Now.Month, 
                                                                              DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)));
            }

            string[] employeeEmails = new string[employees.Length];
            double[] totalGross = new double[employees.Length];

            for(int i = 0; i < employees.Length; i++)
            {
                double gross = 0;
                for(int j = 0; j < timesheets.Length; j++)
                {
                    if(employees[i].Id.ToString().Equals(timesheets[j].EmployeeId.ToString()))
                    {
                        gross += ((employees[i].rate / 60.0) * Math.Round(timesheets[j].HoursWorked.Value.TotalMinutes));
                    }
                }

                employeeEmails[i] = employees[i].Email;
                totalGross[i] = gross;
            }
            string[] humanizedTotalGross = new string[totalGross.Length];
            for(int i = 0; i < totalGross.Length; i++)
            {
                string forHumanize = totalGross[i].ToString();
                string humanized = "";
                if(forHumanize.Contains('.'))
                {
                    int beforeDot = Int32.Parse(forHumanize.Substring(0, forHumanize.IndexOf('.')));
                    int afterDot = Int32.Parse(forHumanize.Substring(forHumanize.IndexOf('.') + 1));
                    humanized = beforeDot.ToWords() + " and " + afterDot.ToWords() + " cents";
                }
                else
                {
                    humanized = Int32.Parse(forHumanize).ToWords().Titleize();
                }
                humanizedTotalGross[i] = humanized.Titleize();
                //s = Regex.Replace(s, @"(^\w)|(\s\w)", m => m.Value.ToUpper()); << under the hood
            }

            ViewBag.humanizedTotalGross = humanizedTotalGross;
            ViewBag.date = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.employeeEmails = employeeEmails;
            ViewBag.totalGross = totalGross;
            return View("~/Views/HR/PrintChecks.cshtml");
        }
    }
}