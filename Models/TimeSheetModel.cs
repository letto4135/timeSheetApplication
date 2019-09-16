using System;
using System.ComponentModel.DataAnnotations;

namespace Zeit.Models
{
    public class TimeSheet
    {
        public int TimeSheetId { get; set; }

        // Time entry includes start/stop time and calculation for hoursworked
        public Day day { get; set; }

        // Employee Id
        public Guid Id { get; set; }

    }
}

