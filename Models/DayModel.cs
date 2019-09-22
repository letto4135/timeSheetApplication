using System;

namespace Zeit.Models
{
    public class DayModel
    {
        Guid DayId { get; }

        DateTime Enter { get; set;}

        DateTime Exit { get; set; }
        
        string? statusMessage{get; set;}

        bool? Approved{get; set;}

        float HoursWorked { get; set; }
    }   
}