using System;
using System.Collections.Generic;
using System.Linq;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace timeSheetApplication.Services
{
    
    public class HRService : IHRManagerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HRService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> CreateDivision(string divisionName)
        {
            var newDivision = new DivisionModel();
            newDivision.id = new Guid();
            newDivision.Division = divisionName;
            _context.Divisions.Add(newDivision);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
        public async Task<bool> EditDivision(Guid id)
        {
            return false;
        }
        public async Task<bool> SetEmployeeWage(int id)
        {
            return false;
        }

        public async Task<DivisionModel[]> GetDivisionsAsync()
        {
            return await _context.Divisions
                .ToArrayAsync();
        }
    }
}