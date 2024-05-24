using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASH_BOOKING.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string OrganizerEmail { get; set; }
        public string OrganizerPhone { get; set; }
        public List<string> Images { get; set; }
    }
}