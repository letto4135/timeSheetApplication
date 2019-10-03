using System;
using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class EmployeeModel : IdentityUser
    {
        public int employeeID{get;set;}
        public string firstName{get; set;}
        public string lastName{get; set;}
        public string divison{get; set;}
        public bool exempt { get; set; }
    }
}

