using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using timeSheetApplication;
using timeSheetApplication.Data;
using timeSheetApplication.Models;
using timeSheetApplication.Services;


namespace timeSheetApplication
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<EmployeeModel>>();
            var timeSheetManager = services.GetRequiredService<ITimeSheetService>();
            await EnsureTestAdminAsync(userManager, timeSheetManager);

            
        }
        
        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
            
            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
            await roleManager.CreateAsync(new IdentityRole(Constants.HRManager));
            await roleManager.CreateAsync(new IdentityRole(Constants.Manager));
            await roleManager.CreateAsync(new IdentityRole(Constants.EmployeeRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<EmployeeModel> userManager, ITimeSheetService timeSheetManager)// passing in the Timesheet interface in order to get access to the method addtimesheet
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@timesheet.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new EmployeeModel { UserName = "admin@timesheet.local", Email = "admin@timesheet.local", rate=30.5 };
            await userManager.CreateAsync(testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);

            var timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(4), 
                                                HoursWorked = DateTime.Now.AddHours(4).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet); // method created in order to add the timesheetModel to the database context, Method is located in timesheetService

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(3), 
                                                HoursWorked = DateTime.Now.AddHours(3).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = new DateTime(DateTime.Now.Year, 9, 1), 
                                                Exit = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3), 
                                                HoursWorked = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3).Subtract(new DateTime(DateTime.Now.Year, 9, 1)),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            testAdmin = new EmployeeModel { UserName = "cnicho14@wvup.edu", Email = "cnicho14@wvup.edu", rate=25.0 };
            await userManager.CreateAsync(testAdmin, "Test1!");
            await userManager.AddToRoleAsync(testAdmin, Constants.HRManager);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = new DateTime(DateTime.Now.Year, 9, 1), 
                                                Exit = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3), 
                                                HoursWorked = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3).Subtract(new DateTime(DateTime.Now.Year, 9, 1)),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            testAdmin = new EmployeeModel { UserName = "alife1@wvup.edu", Email = "alife1@wvup.edu", rate=23.0 };
            await userManager.CreateAsync(testAdmin, "Test1!");
            await userManager.AddToRoleAsync(testAdmin, Constants.Manager);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = new DateTime(DateTime.Now.Year, 9, 1), 
                                                Exit = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3), 
                                                HoursWorked = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3).Subtract(new DateTime(DateTime.Now.Year, 9, 1)),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(15), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(1), 
                                                HoursWorked = DateTime.Now.AddHours(1).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            testAdmin = new EmployeeModel { UserName = "check@local.com", Email = "check@local.com", rate=20.0 };
            await userManager.CreateAsync(testAdmin, "Test1!");
            await userManager.AddToRoleAsync(testAdmin, Constants.EmployeeRole);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = new DateTime(DateTime.Now.Year, 9, 1), 
                                                Exit = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3), 
                                                HoursWorked = new DateTime(DateTime.Now.Year, 9, 1).AddHours(3).Subtract(new DateTime(DateTime.Now.Year, 9, 1)),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(6), 
                                                HoursWorked = DateTime.Now.AddHours(6).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(6), 
                                                HoursWorked = DateTime.Now.AddHours(6).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(7), 
                                                HoursWorked = DateTime.Now.AddHours(7).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(5), 
                                                HoursWorked = DateTime.Now.AddHours(5).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(10), 
                                                HoursWorked =DateTime.Now.AddHours(10).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);

            timeSheet = new TimeSheetModel {Id = new Guid(), 
                                                Enter = DateTime.Now, 
                                                Exit = DateTime.Now.AddHours(8), 
                                                HoursWorked = DateTime.Now.AddHours(8).Subtract(DateTime.Now),
                                                EmployeeId = new Guid(testAdmin.Id)};
            await timeSheetManager.AddTimeSheet(timeSheet);
        }
    }
}