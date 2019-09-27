using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheet
    {
        public Guid TimesheetId { get; }

        // Employee Id
        public Guid Id { get; set; }
        // Time entry includes start/stop time and calculation for hoursworked

        DateTime Enter { get; set;}

        DateTime Exit { get; set; }
        
        string? statusMessage{get; set;}

        bool? Approved{get; set;}

        float HoursWorked { get; set; }
    }
}

