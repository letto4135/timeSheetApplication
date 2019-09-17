using System;

namespace Zeit.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; }
      
        public string firstName{get; set;}
        public string lastName{get; set;}
        public string divison{get; set;}
        public bool exempt { get; set; }
    }
}

