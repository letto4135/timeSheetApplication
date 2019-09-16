using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheet
    {
        public Guid TimesheetId { get; }

        // Time entry includes start/stop time and calculation for hoursworked
        public DayModel day { get; set; }

        // Employee Id
        public Guid Id { get; set; }

    }
}

