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
        Task<TimeSheetModel[]> ViewTimeSheetAsync(string user);

        Task<bool> ClockInAsync(Guid id);

        Task<bool> ClockOutAsync(Guid id);

        Task<bool> ApproveTimeAsync(Guid id);

        Task<TimeSheetModel[]> ListUnapproved();
    }
}