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
        
        public async Task<TimeSheetModel[]> ViewTimeSheetAsync(EmployeeModel user, DateTime currentDate)
        {
            DateTime dateToPull;
            if(currentDate.Day > 15)
            {
                dateToPull = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16);
            }
            else
            {
                dateToPull = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            return await _context.TimeSheets
                .Where(x => x.Exit != null && x.EmployeeId.ToString().Equals(user.Id.ToString()))
                .Where(x => x.Enter >= dateToPull)
                .OrderBy(x => x.Enter)
                .ToArrayAsync();
        }

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
                newTime.Approved = 0;
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

                if(update.HoursWorked.Value.Minutes < 1)
                {
                    _context.Remove(update);
                    var success = await _context.SaveChangesAsync();
                    return success == 1;
                }
                 
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

            update.Approved = 1;

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<TimeSheetModel[]> ListUnapprovedAsync()
        {
            return await _context.TimeSheets
                .Where(x => x.Approved == 0 && x.Exit != null)
                .ToArrayAsync();
        }

        public async Task<bool> MassApproveAsync(String[] id)
        {
            var timeSheets = await _context.TimeSheets
                .Where(x => x.Approved == 0)
                .ToArrayAsync();
            var saveResult = 1;
            foreach(var time in id)
            {
                for(int i = 0; i < timeSheets.Length; i++)
                {
                    if(timeSheets[i].Id.ToString().Equals(time))
                    {
                        if(saveResult == 1)
                        {
                            timeSheets[i].Approved = 1;
                            saveResult = await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
            }

            return saveResult == 1;
        }

       public async Task<bool> AddTimeSheet(TimeSheetModel timeSheet)
       {
           _context.TimeSheets.Add(timeSheet);
           return await _context.SaveChangesAsync() == 1;
       }
    }
}