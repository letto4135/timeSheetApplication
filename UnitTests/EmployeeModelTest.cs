using System;
using Xunit;
using timeSheetApplication.Models;
using timeSheetApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using timeSheetApplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    public class EmployeeModelTest
    {
      
        public EmployeeModelTest()
        {
           var services = new ServiceCollection();
           services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName:"EmployeeTest"));
        }


        [Fact]
        public void EmployeeCreateModel()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "EmployeeTest").Options;
            using (var context = new ApplicationDbContext(options))
            {
               var employee = new EmployeeModel
                {
                    firstName = "chris",
                    lastName  = "Nicholson",
                    division = "Dogs",
                    exempt = false,
                    rate = 23.0

                };

                var newemployee = new EmployeeModel
                {
                    firstName = "name",
                    lastName  = "lastname",
                    division = "Computers",
                    exempt = true,
                    rate = 30
                };

                context.Add(employee);
                Assert.Equal(EntityState.Added, context.Entry(employee).State);
                Assert.NotEmpty(employee.firstName);
                Assert.NotEmpty(employee.lastName);
                Assert.True(employee.rate > 0);
                Assert.Matches("Dogs", employee.division);
                context.SaveChanges();
                Assert.Equal(EntityState.Unchanged, context.Entry(employee).State);
                context.Add(newemployee);
                Assert.Equal(EntityState.Added, context.Entry(newemployee).State);
                employee.firstName = "bob";
                Assert.Equal(EntityState.Modified, context.Entry(employee).State);
                Assert.True(employee.rate < newemployee.rate);
                context.Remove(employee);
                Assert.Equal(EntityState.Deleted, context.Entry(employee).State);
                Assert.True(newemployee.exempt);
                Assert.Matches("Computers", newemployee.division);
                context.Remove(newemployee);
                context.Dispose();

            }
        }
    }
}