using System;
using System.ComponentModel.DataAnnotations;

namespace timeSheetApplication.Models
{
    public class TimeSheetModel
    {
        public Guid Id { get; set; }

        // Employee Guid Id NOT THE HUMAN READABLE ID
        public Guid EmployeeId { get; set; }

        // Time entry includes start/stop time and calculation for hoursworked
        public DateTime Enter { get; set;}

        public DateTime? Exit { get; set; }
        public string statusMessage{get; set;} = "";

        public int Approved{get; set;} = 0;

        public TimeSpan? HoursWorked { get; set; }
    }
}
