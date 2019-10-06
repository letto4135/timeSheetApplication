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
                .Where(x => x.Exit != null && x.EmployeeId.ToString().Equals(user.Id.ToString()))
                .ToArrayAsync();
        }


        //
        //Im pretty sure some of these methods below will actually go into the controller rather than this service class
        // clock in, clockout 
        //
        public async Task<bool> ClockInAsync(Guid id)
        {
            var alreadyClockedIn = await _context.TimeSheets
                .Where(x => x.Exit == null && x.EmployeeId.Equals(id))
                .SingleOrDefaultAsync();

            if(alreadyClockedIn != null)
            {
                return false;
            }
            else
            {
                var newTime = new TimeSheetModel();
                newTime.Approved = false;
                newTime.Id = new Guid();
                newTime.EmployeeId = id;
                newTime.statusMessage = "";
                newTime.Enter = DateTime.Now;

                _context.TimeSheets.Add(newTime);

                var saveResult = await _context.SaveChangesAsync();

                return saveResult != 0;
            }
        }

        public async Task<TimeSheetModel> CurrentClockInAsync(Guid id)
        {
            return await _context.TimeSheets
                .SingleOrDefaultAsync(x => x.EmployeeId.Equals(id)
                                      && x.Exit == null);
        }

        public async Task<bool> ClockOutAsync(Guid id)
        {
            var update = await _context.TimeSheets
                .SingleOrDefaultAsync(x => x.Exit == null &&
                                      x.EmployeeId.Equals(id));

            if(update != null)
            {
                DateTime exit = DateTime.Now;
                DateTime enter = update.Enter;
                update.Exit = exit;
                update.HoursWorked = exit.Subtract(enter);
                 
                if (update.HoursWorked > TimeSpan.FromHours(22))
                {
                    return false;
                }
                else
                {
                    var saveResult = await _context.SaveChangesAsync();
                    return saveResult != 0;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ApproveTimeAsync(Guid id)
        {
            var update = await _context.TimeSheets
                .Where(x => x.Id.Equals(id))
                .SingleOrDefaultAsync();

            if(update == null) return false;

            update.Approved = true;

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<TimeSheetModel[]> ListUnapproved()
        {
            return await _context.TimeSheets
                .Where(x => x.Approved == false && x.Exit != null)
                .ToArrayAsync();
        }
    }
}