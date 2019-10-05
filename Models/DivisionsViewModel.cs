using Microsoft.AspNetCore.Identity;

namespace timeSheetApplication.Models
{
    public class DivisionsViewModel
    {
        public DivisionModel[] Divisions { get; set; }

        public IdentityUser[] Managers { get; set; }

        public IdentityUser[] Employees { get; set; }
    }
}