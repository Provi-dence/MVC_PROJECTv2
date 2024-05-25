using System;
using System.ComponentModel.DataAnnotations;

namespace DASH_BOOKING.Models
{
    public class EventRequest
    {
        [Key]
        public int EventRequestId { get; set; }

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
    }
}
