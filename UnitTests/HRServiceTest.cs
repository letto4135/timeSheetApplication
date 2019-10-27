// using System;
// using Xunit;
// using timeSheetApplication.Models;
// using timeSheetApplication.Data;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity;
// using timeSheetApplication.Services;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit.Abstractions;

// namespace UnitTests
// {
//     public class HRServiceTest
//     {
//         private ServiceCollection _serviceCollection;
//         private readonly ITestOutputHelper _output;
        
//         public HRServiceTest(ITestOutputHelper output)
//         {
//             _output = output;
//             _serviceCollection = new ServiceCollection();
//             _serviceCollection.AddIdentity<EmployeeModel, IdentityRole>()
//                 .AddEntityFrameworkStores<IdentityDbContext<EmployeeModel>>()
//                 .AddDefaultTokenProviders();
//             _serviceCollection.AddDbContext<IdentityDbContext<EmployeeModel>>(options =>
//             {
//                 options.UseInMemoryDatabase(databaseName:"Test_CreateDivision");
//             });
//         }

//         [Fact]
//         public async Task CreateDivisionTest()
//         {
//             var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//             .UseInMemoryDatabase(databaseName:"Test_CreateDivision").Options;

//             using (var context = new ApplicationDbContext(options))
//             {
//                  var store = new UserStore<EmployeeModel>(context);
//                 var userManager = _serviceCollection.BuildServiceProvider().GetService<UserManager<EmployeeModel>>();
//                 await userManager.CreateAsync(new EmployeeModel{
//                     firstName = "name",
//                     lastName = "asdnjasd",
//                     division = "TestName",
//                     Email = "testManager@test.com",
//                     exempt = true
//                 });

//                 await context.SaveChangesAsync();

//                 var service = new HRService(context, userManager);
//                 try{
//                     var success = await service.CreateDivision("testManager@test.com", "TestName");
//                 if(success)
//                 {
//                     _output.WriteLine("Could not save to db.");
//                     //Environment.Exit(1);
//                 }
//                 }catch(NullReferenceException e){
//                     _output.WriteLine("Some sort of error creating division that wasn't caught...");
//                     _output.WriteLine(e.StackTrace);
//                     //Environment.Exit(1);
//                 }
                
//             }

//             using (var context = new ApplicationDbContext(options))
//             {
//                 var itemsInDatabase = await context
//                 .Divisions.CountAsync();
                
//                 Assert.Equal(1, itemsInDatabase);
//                 var item = await context.Divisions.FirstAsync();
//                 Assert.Equal("Test Division", item.Division);
//             }

//             using (var context = new ApplicationDbContext(options))
//             {
//                 var itemsInDatabase = await context
//                 .Divisions.CountAsync();
                
//                 Assert.Equal(1, itemsInDatabase);
//                 var item = await context.Divisions.FirstAsync();
//                 Assert.Equal("TestName", item.Division);
//             }
//         }
//     }
// }