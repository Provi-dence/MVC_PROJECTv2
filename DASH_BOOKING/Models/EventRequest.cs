using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DASH_BOOKING.Models
{
    // This is for contact.cshtml
    public class EventRequest
    {
        [Required]
        public string EventName { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EventTime { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [EmailAddress]
        public string OrganizerEmail { get; set; }

        [Phone]
        public string OrganizerPhone { get; set; }

        public EventRequest() { }

        public EventRequest(string eventName, string description, DateTime eventDate, TimeSpan eventTime, string location, string organizerEmail, string organizerPhone)
        {
            EventName = eventName;
            Description = description;
            EventDate = eventDate;
            EventTime = eventTime;
            Location = location;
            OrganizerEmail = organizerEmail;
            OrganizerPhone = organizerPhone;
        }
    }
}