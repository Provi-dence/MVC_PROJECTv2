using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DASH_BOOKING.Models
{
    public class EventModel
    {
        [Key]
        public int Id { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EventTime { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string OrganizerEmail { get; set; }

        public string OrganizerPhone { get; set; }

        public virtual List<EventImage> Images { get; set; }
    }

    public class EventImage
    {
        [Key]
        public int EventImageId { get; set; }

        public int EventModelId { get; set; }

        public string Image { get; set; }

        public virtual EventModel EventModel { get; set; }
    }
}
