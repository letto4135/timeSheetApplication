using System;
using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class EmployeeModel : IdentityUser
    {
        public string firstName{get; set;}
        public string lastName{get; set;}
        public string? division{get; set;}
        public bool exempt{ get; set; } = false;
        public double rate{get;set;} = 0.0;

    }
}

