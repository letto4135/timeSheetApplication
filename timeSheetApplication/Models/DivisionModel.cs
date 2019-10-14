using System;
using System.Collections.Generic;

namespace timeSheetApplication.Models
{
    public class DivisionModel
    {
        public Guid id { get; set; }
        public string Division { get; set; }
        public Guid managerId { get; set; }
       // public List<EmployeeModel> assignedEmployees {get; set;} = new List<EmployeeModel>();
    }
}