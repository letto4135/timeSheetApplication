using System;
using System.Collections.Generic;
using System.Linq;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using Microsoft.EntityFrameworkCore;
using Zeit.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace timeSheetApplication.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ApplicationDbContext _context;
        public TimeSheetService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<TimeSheetModel[]> ViewTimeSheetAsync(EmployeeModel user)
        {
            return await _context.TimeSheets
                .Where(x => x.EmployeeId == user.employeeID)
                .ToArrayAsync();
        }

        public async Task<bool> ClockInAsync(TimeSheetModel newTime, EmployeeModel user)
        {
            newTime.Approved = false;
            newTime.EmployeeId = user.employeeID;
            newTime.Id = new Guid();
            newTime.statusMessage = "";
            newTime.Enter = DateTime.Now;

            _context.TimeSheets.Add(newTime);

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<bool> ClockOutAsync(Guid id, EmployeeModel user)
        {
            var update = await _context.TimeSheets
                .SingleOrDefaultAsync(x => x.EmployeeId == user.employeeID
                                      && x.Id == id);

            if(update != null)
            {
                DateTime exit = DateTime.Now;
                DateTime enter = update.Enter;
                update.Exit = exit;
                update.HoursWorked = exit.Subtract(enter);

                var saveResult = await _context.SaveChangesAsync();
                
                return saveResult == 1;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ApproveTimeAsync(Guid id, EmployeeModel user)
        {
            var update = await _context.TimeSheets
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            if(update == null) return false;

            update.Approved = true;

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<TimeSheetModel[]> ListUnapproved()
        {
            return await _context.TimeSheets
                .Where(x => x.Approved == false)
                .ToArrayAsync();
        }
    }
}