using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Models;

namespace timeSheetApplication.Services
{
    public interface IHRManagerService
    {
        Task<bool> CreateDivision(string Manager, string Division);
        Task<bool> UpdateDivision(string Manager, string Division);
        Task<bool> RemoveDivision(DivisionModel division);
        Task<bool> UpdateEmployee(string Division, string Rate, string Exempt, string id);
        Task<DivisionModel[]> GetDivisionsAsync();
        Task<DivisionModel> GetDivisionAsync(string id);
    }
}