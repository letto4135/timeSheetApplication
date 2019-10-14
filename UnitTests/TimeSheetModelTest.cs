using System;
using Xunit;
using timeSheetApplication.Models;
using timeSheetApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace UnitTests
{
    public class TimeSheetModelTest
    {
        [Fact]
        public void TimeSheetModelCreate()
        {
            var time = DateTime.Now;
            var timesheet = new TimeSheetModel
            {
                Id = new Guid(),
                EmployeeId = new Guid(),
                Enter = time,
                Exit = time.AddHours(1),
                statusMessage = null,
                Approved = 0,
                HoursWorked = time.AddHours(2).Subtract(time)
            };

            Guid guidResult;
            Assert.True(Guid.TryParse(timesheet.Id.ToString(), out guidResult));
            Assert.True(Guid.TryParse(timesheet.EmployeeId.ToString(), out guidResult));
            Assert.Equal(time, timesheet.Enter);
            Assert.Equal(time.AddHours(1), timesheet.Exit);
            Assert.Null(timesheet.statusMessage);
            Assert.Equal(0, timesheet.Approved);
            Assert.Equal(time.AddHours(2).Subtract(time), timesheet.HoursWorked);
        }
    }
}