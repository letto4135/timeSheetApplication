using System;
using Xunit;
using timeSheetApplication.Models;
using timeSheetApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace DivisionModelTest
{
    public class DivisonTest
    {

        [Fact]
        public void DivisionCreationTest()
        {
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
