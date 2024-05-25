using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DASH_BOOKING.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginRegister()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }



        public ActionResult Events()
        {
            List<EventRequest> eventRequests = new List<EventRequest>
        {
            new EventRequest("Event 1", "Description for Event 1", new DateTime(2024, 6, 15), new TimeSpan(14, 0, 0), "Location 1", "organizer1@example.com", "123-456-7890"),
            new EventRequest("Event 2", "Description for Event 2", new DateTime(2024, 7, 20), new TimeSpan(16, 30, 0), "Location 2", "organizer2@example.com", "987-654-3210")
            // Add more events as needed
        };

            return View(eventRequests);
        }
    }

}