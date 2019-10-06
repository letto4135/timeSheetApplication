using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Models;

namespace timeSheetApplication.Services
{
    public interface ITimeSheetService
    {
        Task<TimeSheetModel[]> ViewTimeSheetAsync(EmployeeModel user);

        Task<bool> ClockInAsync(Guid id);

        Task<TimeSheetModel> CurrentClockInAsync(Guid id);

        Task<bool> ClockOutAsync(Guid id);

        Task<bool> ApproveTimeAsync(Guid id);

        Task<TimeSheetModel[]> ListUnapproved();
    }
}