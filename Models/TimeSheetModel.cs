using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheet
    {
        public int TimeSheetId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? TotalHours { get; set; }
        public int UserId { get; set; }

    }
}

