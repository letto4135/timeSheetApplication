using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Models;
using Zeit.Models;

namespace timeSheetApplication.Services
{
    public interface ITimeSheetService
    {
        Task<TimeSheetModel[]> ViewTimeSheetAsync(EmployeeModel user);

        Task<bool> ClockInAsync(TimeSheetModel newTime, EmployeeModel user);

        Task<bool> ClockOutAsync(Guid id, EmployeeModel user);

        Task<bool> ApproveTimeAsync(Guid id, EmployeeModel user);
    }
}