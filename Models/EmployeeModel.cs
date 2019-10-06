using System;
using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class EmployeeModel : IdentityUser
    {
        public string firstName{get; set;}
        public string lastName{get; set;}
        public string? division{get; set;}
        public bool? exempt{ get; set; }
        public double? rate{get;set;}

    }
}

