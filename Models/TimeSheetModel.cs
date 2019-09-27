using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheetModel
    {
        public int Id { get; set; }

        // Employee Id
        public int EmployeeId { get; set; }

        // Time entry includes start/stop time and calculation for hoursworked
        public string Enter { get; set;}

        public string Exit { get; set; }
        public string? statusMessage{get; set;}

        public bool? Approved{get; set;}

        public float HoursWorked { get; set; }
    }
}
