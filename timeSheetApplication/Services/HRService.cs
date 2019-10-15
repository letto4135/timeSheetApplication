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
        private readonly UserManager<EmployeeModel> _userManager;
        public HRService(ApplicationDbContext context, UserManager<EmployeeModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> CreateDivision(string ManagerEmail, string Division)
        {
            // Set division id
            var newDivision = new DivisionModel();
            newDivision.id = new Guid();
            // get the user that is going to be the manager
            var manager = await _userManager.FindByEmailAsync(ManagerEmail);
            // get a list of all divisions
            var divisions = await _context.Divisions.ToArrayAsync();

            // compare new manager id to current managers, and division name to current divisions
            // if new manager is already a manager or division alread exists return false
            foreach(var division in divisions)
            {
                if(manager.Id.Equals(division.managerId.ToString()) || division.Division.Equals(Division))
                {
                    return false;
                }
            }

            // Set manager
            newDivision.managerId = new Guid(manager.Id);
            
            // Set division
            newDivision.Division = Division;

            // Set employee division to division they are a manager of
            manager.division = Division;
            manager.exempt = true;
            // update employee
            _context.Employees.Update(manager);
            // save changes
            var save = await _context.SaveChangesAsync();

            // Add division to db
            _context.Divisions.Add(newDivision);
            // Save db
            var saveResult = await _context.SaveChangesAsync();
            // save of employee and division must be true
            return saveResult == 1 && save == 1;
        }
        public async Task<bool> UpdateDivision(string Manager, string Division)
        {
            // get division being updated
            var division = await _context.Divisions
                .Where(x => x.Division.Equals(Division))
                .FirstAsync();

            // if not found return false
            if(division == null) return false;

            // get the employee who is going to be the new manager
            var manager = await _userManager.FindByEmailAsync(Manager);

            // get all the divisions
            var divisions = await _context.Divisions.ToArrayAsync();
            // check that the employee about to become a manager is not already a manager
            foreach(var division2 in divisions)
            {
                if(manager.Id.Equals(division2.managerId.ToString()))
                {
                    return false;
                }
            }

            // get the old division manager
            var oldManager = await _userManager.FindByIdAsync(division.managerId.ToString());
            // set the old managers exempt status to false and save
            oldManager.exempt = false;
            var saveOldManager = await _context.SaveChangesAsync();
            // set the division managerId to be the id of the new manager and save
            division.managerId = new Guid(manager.Id);
            var saveNewManagerToDivision = await _context.SaveChangesAsync();
            // set the new managers exempt to true and save
            manager.exempt = true;
            manager.division = Division;
            var saveEmployeeInstance = await _context.SaveChangesAsync();
            // ensure that all 3 saves completed successfully.
            return saveEmployeeInstance == 1 && saveOldManager == 1 && saveNewManagerToDivision == 1;
        }

        public async Task<bool> RemoveDivision(DivisionModel division)
        {
            // remove the division by instance
            _context.Divisions.Remove(division);
            // save db
            var success = await _context.SaveChangesAsync();
            
            return success == 1;
        }

        public async Task<DivisionModel[]> GetDivisionsAsync()
        {
            // get list of all divisions
            return await _context.Divisions
                .OrderBy(x => x.Division)
                .ToArrayAsync();
        }

        public async Task<DivisionModel> GetDivisionAsync(string id)
        {
            // get division by id
            return await _context.Divisions
                .Where(x => x.id.Equals(new Guid(id)))
                .SingleOrDefaultAsync();
        }
    }
}