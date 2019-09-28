using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheetModel
    {
        public Guid Id { get; set; }

        // Employee Id
        public int EmployeeId { get; set; }

        // Time entry includes start/stop time and calculation for hoursworked
        public DateTime Enter { get; set;}

        public DateTime? Exit { get; set; }
        public string? statusMessage{get; set;}

        public bool Approved{get; set;}

        public TimeSpan? HoursWorked { get; set; }
    }
}
