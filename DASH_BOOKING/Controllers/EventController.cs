using DASH_BOOKING.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DASH_BOOKING.Controllers
{
    public class EventController : Controller
    {
        // GET: Event


        public ActionResult Events()
        {
            return View();
        }


        public ActionResult EventDetails(int id)
        {
            // Assuming you have a method to fetch event details from your data source based on the event ID
            EventModel eventModel = GetEventDetailsById(id);

            if (eventModel == null)
            {
                return HttpNotFound();
            }

            return View(eventModel);

        }

        private EventModel GetEventDetailsById(int id)
        {
            // Your logic to fetch event details from data source based on the ID
            // Example: EventModel eventModel = yourDatabaseContext.Events.FirstOrDefault(e => e.Id == id);
            // Return eventModel
            return null; // Placeholder, replace with actual logic
        }
    }
}
