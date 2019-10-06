using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timeSheetApplication.Models;

namespace timeSheetApplication.Controllers
{
    public class TimeSheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}