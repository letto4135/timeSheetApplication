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

namespace DivisionModelTest
{
    public class DivisonTest : IDisposable
    {
        private ServiceCollection _serviceCollection;
        private readonly ITestOutputHelper _output;
         private readonly DbContext _db;
        public DivisonTest(ITestOutputHelper output)
        {
            _output = output;
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddIdentity<EmployeeModel, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext<EmployeeModel>>()
                .AddDefaultTokenProviders();
            _serviceCollection.AddDbContext<IdentityDbContext<EmployeeModel>>(options =>
            {
                options.UseInMemoryDatabase(databaseName:"Test_CreateDivision");
            });
        }

        public void Dispose()
        {
            //_db.CleanDb();
        }

        [Fact]
        public void DivisionCreationTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName:"Test_CreateDivision").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var store = new UserStore<EmployeeModel>(context);
                var userManager = _serviceCollection.BuildServiceProvider().GetService<UserManager<EmployeeModel>>();
                var employeeService = _serviceCollection.BuildServiceProvider().GetService<IEmployeeService>();
                var timeSheetService = _serviceCollection.BuildServiceProvider().GetService<ITimeSheetService>();
                var HRManagerService = _serviceCollection.BuildServiceProvider().GetService<IHRManagerService>();
                
                var user = new EmployeeModel{ UserName = "test@email.com", Email = "test@email.com"};
                userManager.CreateAsync(user, "Test12!");
                context.SaveChanges();
                HRManagerService.CreateDivision(user.Email, "Test Division");
                context.SaveChanges();
            };

            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase =  context.Users.Count();

                Assert.Equal(1, itemsInDatabase);
                //var item = context.Divisions.FirstOrDefault();
                //Assert.Equal("Test Division", item.Division);
            }
   
            var division = new DivisionModel
            {
                id = new Guid(),
                Division = "Computers",
                managerId = new Guid() 
            };
            Guid guidResult;
            Assert.True(Guid.TryParse(division.id.ToString(), out guidResult));
            Assert.NotNull(division.Division);
            Assert.True(Guid.TryParse(division.managerId.ToString(), out guidResult));
            Assert.Equal("Computers", division.Division);
        }
    }
}
