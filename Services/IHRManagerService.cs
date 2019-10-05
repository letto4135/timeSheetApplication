using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Models;

namespace timeSheetApplication.Services
{
    public interface IHRManagerService
    {
        Task<bool> CreateDivision(string divisionName);
        Task<bool> EditDivision(Guid id);
        Task<bool> SetEmployeeWage(int id);

        Task<DivisionModel[]> GetDivisionsAsync();
    }
}