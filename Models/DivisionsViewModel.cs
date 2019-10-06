using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class DivisionsViewModel
    {
        public DivisionModel[] Divisions { get; set; }

        public EmployeeModel[] Managers { get; set; }

        public EmployeeModel[] Employees { get; set; }
    }
}