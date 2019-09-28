using System;

namespace timeSheetApplication.Models
{
    public class DivisionModel
    {
        public Guid id { get; set; }
        public string Division { get; set; }
        public Guid managerId { get; set; }
    }
}