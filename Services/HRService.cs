using System;
using System.Collections.Generic;
using System.Linq;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public async Task<bool> CreateDivision(string Manager, string Division)
        {
            // Set division id
            var newDivision = new DivisionModel();
            newDivision.id = new Guid();

            var manager = await _userManager.FindByEmailAsync(Manager);

            var divisions = await _context.Divisions.ToArrayAsync();

            // compare new manager id to current managers, and division name to current divisions
            // if new manager is already a manager or division alread exists return false
            foreach(var division in divisions)
            {
                if(manager.Id.Equals(division.managerId.ToString()))
                {
                    return false;
                }
            }

            // Set manager
            newDivision.managerId = new Guid(manager.Id);
            
            // Set division
            newDivision.Division = Division;

            // Add to db
            _context.Divisions.Add(newDivision);
            // Save db
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
        public async Task<bool> UpdateDivision(string Manager, string Division)
        {
            var division = await _context.Divisions
                .Where(x => x.Division.Equals(Division))
                .FirstAsync();

            if(division == null) return false;

            var manager = await _userManager.FindByEmailAsync(Manager);

            var divisions = await _context.Divisions.ToArrayAsync();

            foreach(var division2 in divisions)
            {
                if(manager.Id.Equals(division2.managerId.ToString()))
                {
                    return false;
                }
            }

            division.managerId = new Guid(manager.Id);

            var saveResult = await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveDivision(DivisionModel division)
        {
            _context.Divisions.Remove(division);

            var success = await _context.SaveChangesAsync();
            
            return success == 1;
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

        public async Task<DivisionModel> GetDivisionAsync(string id)
        {
            return await _context.Divisions
                .Where(x => x.id.Equals(new Guid(id)))
                .SingleOrDefaultAsync();
        }
    }
}