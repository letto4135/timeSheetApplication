using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Zeit.Models;

namespace timeSheetApplication.Services
{
    public interface IHRManagerService
    {
        Task<bool> CreateDivision(Guid managerId, string divisionName);
        Task<bool> EditDivision(Guid id);
        Task<bool> SetEmployeeWage(int id);
    }
}