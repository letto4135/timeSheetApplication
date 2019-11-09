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
        private ServiceProvider service;
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
            service = services.BuildServiceProvider();
            _userManager = services.BuildServiceProvider().GetRequiredService<UserManager<EmployeeModel>>();
            _timeSheetService = service.GetRequiredService<ITimeSheetService>();
            _hrManagerService = service.GetRequiredService<IHRManagerService>();
            
        }
        [Fact]
        public async Task AddNewDivision()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DivisionTest").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var _hrManagerService = service.GetRequiredService<IHRManagerService>();
                _hrManagerService.CreateDivision("Fake@fake.com", "Division");

                Assert.NotNull(context.Divisions.FirstOrDefaultAsync());
                var _HRService = new HRService(context, _userManager);
                var fakeUser = new EmployeeModel {Id = new Guid().ToString(), Email = "FakeEmail@fake.com", UserName = "Fake1"};
                await context.AddAsync(fakeUser);
                await context.SaveChangesAsync();
                var emp = await context.Employees.FirstOrDefaultAsync();

                Assert.Matches("FakeEmail@fake.com", emp.Email);

                await context.AddAsync(new DivisionModel {id=new Guid(), managerId=new Guid(emp.Id), Division="TestDivision"});
                await context.SaveChangesAsync();

                var div = await context.Divisions.FirstOrDefaultAsync();
                Assert.Matches("TestDivision", div.Division);
            }

            using (var context = new ApplicationDbContext(options))
            {
                
            }
        }
    }
}
