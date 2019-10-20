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
            // dateToPull is the first date of the payperiod, either the 1st or the 16th
            DateTime dateToPull;
            // check which payperiod were in
            if(currentDate.Day > 15)
            {
                dateToPull = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16);
            }
            else
            {
                dateToPull = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            // get list of timesheets where Enter date is >= first date of payperiod,
            // exit has a value, and employee id is equal to user id
            return await _context.TimeSheets
                .Where(x => x.Exit != null && x.EmployeeId.ToString().Equals(user.Id.ToString()))
                .Where(x => x.Enter >= dateToPull)
                .OrderBy(x => x.Enter)
                .ToArrayAsync();
        }

        public async Task<bool> ClockInAsync(Guid id)
        {
            // try and find a time where the user is already clocked in but has
            // not clocked out.
            var alreadyClockedIn = await _context.TimeSheets
                .Where(x => x.Exit == null && x.EmployeeId.Equals(id))
                .SingleOrDefaultAsync();

            // if there is a date without an exit value return false
            if(alreadyClockedIn != null)
            {
                return false;
            }
            // otherwise create a new timesheet entry
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
            // gets current clock in where exit == null, for displaying how
            // long employee has been at work
            return await _context.TimeSheets
                .SingleOrDefaultAsync(x => x.EmployeeId.Equals(id)
                                      && x.Exit == null);
        }

        public async Task<bool> ClockOutAsync(Guid id)
        {
            // find a timesheet where the exit value == null for current employee
            var update = await _context.TimeSheets
                .SingleOrDefaultAsync(x => x.Exit == null &&
                                      x.EmployeeId.Equals(id));    

            // if exit == null is found, update the timesheet
            if(update != null)
            {
                DateTime exit = DateTime.Now;
                DateTime enter = update.Enter;
                // use current time as exit, and calculate
                // hours worked with exit - enter
                update.Exit = exit;
                update.HoursWorked = exit.Subtract(enter);

                // if the hours worked < 1 minute, delete the timesheet
                if(update.HoursWorked.Value.Minutes < 1)
                {
                    _context.Remove(update);
                    var success = await _context.SaveChangesAsync();
                    return success == 1;
                }
                
                // if theyve been clocked in for more than 22 hours refuse
                // to update
                if (update.HoursWorked > TimeSpan.FromHours(22))
                {
                    return false;
                }
                // otherwise save the result
                else
                {
                    var saveResult = await _context.SaveChangesAsync();
                    return saveResult != 0;
                }
            }
            // if a timesheet could not be found where exit == null
            else
            {
                return false;
            }
        }

        public async Task<bool> ApproveTimeAsync(Guid id)
        {
            // get timesheet by id
            var update = await _context.TimeSheets
                .Where(x => x.Id.Equals(id))
                .SingleOrDefaultAsync();

            // if cannot be found return false
            if(update == null) return false;

            // change approved to "true"
            update.Approved = 1;
            // save entry
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<TimeSheetModel[]> ListUnapprovedAsync()
        {
            // returns all unapproved timesheets,
            // controller output is limited to 10 at a time
            return await _context.TimeSheets
                .Where(x => x.Approved == 0 && x.Exit != null)
                .ToArrayAsync();
        }
        /// <summary>
        /// this method makes sure that the id is not null and generates a new GUID 
        /// if the save result is equal to 1, we set the approved property to true and save the changes
        /// to the database each time otherwise we return false if an error occurs
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task</returns>
        public async Task<bool> MassApproveAsync(String[] id)
        {
            // saveResult used to make sure previous save was success
            var saveResult = 1;
            // id is a list of unapproved timesheets
            foreach(var time in id)
            {
                // 10 items are passed in, reguardless of whether a value
                // exists, so make sure that time exists before continuing
                // if not, let the loop finish without returning or throwing errors
                if(time != null)
                {
                    // timesheet is a specific instance of a timesheet where
                    // the id of the timesheet is == to time
                    var timesheet = await _context.TimeSheets.FindAsync(new Guid(time));
    
                    // as long as the previous save result was success, update
                    // timesheet and change saveResult to the value of savechanges
                    if(saveResult == 1)
                    {
                        timesheet.Approved = 1;
                        saveResult = await _context.SaveChangesAsync();
                    }
                    // if previous save could not be done stop the loop by returning
                    else
                    {
                        return false;
                    }
                }
            }
            // ensure the final save was success
            return saveResult == 1;
        }

        public async Task<TimeSheetModel[]> GetUnapprovedById(Guid id)
        {
            var time = await _context.TimeSheets
                .Where(x => x.Id.ToString().Equals(id.ToString()))
                .ToArrayAsync();
            
            return time;
        }

        public async Task<bool> DenyTime(TimeSheetModel time)
        {
            // get timesheet by id
            var update = await _context.TimeSheets
                .Where(x => x.Id.ToString().Equals(time.Id.ToString()))
                .SingleOrDefaultAsync();

            // if cannot be found return false
            if(update == null) return false;

            // change approved to "denied"
            update.Approved = 2;
            // save entry
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        /// <summary>
        /// This method is used for adding timesheets into the database context for when we are generating 
        /// seed data from the seed data class @see SeedData.cs
        /// </summary>
        /// <param name="timeSheet"></param>
        /// <returns></returns>
       public async Task<bool> AddTimeSheet(TimeSheetModel timeSheet)
       {
           // add new timesheetmodel to the timesheets
           _context.TimeSheets.Add(timeSheet);
           return await _context.SaveChangesAsync() == 1;
       }
    }
}