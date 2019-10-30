using System;
using Xunit;
using timeSheetApplication.Models;
using timeSheetApplication.Data;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using timeSheetApplication.Services;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DivisionModelTest
{
    public class DivisionModelTest
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IHRManagerService _hrManagerService;
        public DivisionModelTest()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName:"DivisionTest"));
            services.AddIdentity<EmployeeModel, IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IHRManagerService, HRService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITimeSheetService, TimeSheetService>();
            var serv = services.BuildServiceProvider();
            _userManager = services.BuildServiceProvider().GetRequiredService<UserManager<EmployeeModel>>();
            _timeSheetService = serv.GetRequiredService<ITimeSheetService>();
            _hrManagerService = serv.GetRequiredService<IHRManagerService>();
        }
        [Fact]
        public async Task AddNewDivision()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DivisionTest").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var HRService = new HRService(context, _userManager);
                var fakeUser = new EmployeeModel {Id = new Guid().ToString(), Email = "FakeEmail@fake.com", UserName = "Fake1"};
                await context.AddAsync(fakeUser);
                await context.SaveChangesAsync();
                var emp = await context.Employees.FirstOrDefaultAsync();

                Assert.True(emp.Email.Equals("FakeEmail@fake.com"));

                await context.AddAsync(new DivisionModel {id=new Guid(), managerId=new Guid(emp.Id), Division="TestDivision"});
                await context.SaveChangesAsync();

                var div = await context.Divisions.FirstOrDefaultAsync();
                Assert.True(div.Division.Equals("TestDivision"));
            }

            using (var context = new ApplicationDbContext(options))
            {
                
            }
        }
    }
}
