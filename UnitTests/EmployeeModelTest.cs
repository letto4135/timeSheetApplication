// using System;
// using Xunit;
// using timeSheetApplication.Models;
// using timeSheetApplication.Data;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity;

// namespace UnitTests
// {
//     public class EmployeeModelTest
//     {
//         private readonly DbContext _db;
//         [Fact]
//         public void EmployeeCreateModel()
//         {
//             var employee = new EmployeeModel
//             {
//                 firstName = "chris",
//                 lastName  = "Nicholson",
//                 division = "Dogs",
//                 exempt = false,
//                 rate = 23.0
//             };

//             _db.Add(employee);
//             Assert.NotNull(employee.firstName);
//             Assert.NotNull(employee.lastName);
//             Assert.NotNull(employee.division);
//             Assert.False(employee.exempt);
//             Assert.True(employee.rate > 0);
//             Assert.Equal("chris", employee.firstName);
//             Assert.Equal("Nicholson", employee.lastName);
//             Assert.Equal("Dogs", employee.division);

//         }
//     }
// }