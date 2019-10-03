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
    [Authorize(Roles = Constants.HRManager + " , " + Constants.AdministratorRole)]
    public class HRManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHRManagerService _HRManagerService;

        public HRManagerController(UserManager<IdentityUser> userManager )
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateDivision(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            string name = "";
            var successful = await _HRManagerService.CreateDivision(id, name);
            return View();
        }

        public async Task<IActionResult> EditDivision(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();

            var successful = await _HRManagerService.EditDivision(id);
            return View();
        }

        public async Task<IActionResult> SetEmployeeWage(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == null) return Challenge();
            
            var successful = await _HRManagerService.SetEmployeeWage(id);
            return View();
        }
    }
}