using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class TimeSheetViewModel
    {
        public TimeSheetModel[] Items { get; set; }

        public EmployeeModel Employee { get; set; }
    }
}