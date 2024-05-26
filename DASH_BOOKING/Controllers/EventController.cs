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
        private ApplicationDbContext db = new ApplicationDbContext();


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

        public ActionResult CreateEvent()
        {
            return View();
        }

        // POST: Event/CreateEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(EventRequest eventRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure the status is set to "Pending"
                    eventRequest.Status = "Pending";

                    // Save event request to database or perform other actions
                    db.EventRequests.Add(eventRequest);
                    db.SaveChanges();

                    // Set success message in TempData
                    TempData["SuccessMessage"] = "Event request successfully submitted. Thank you!";
                }
                catch (Exception ex)
                {
                    // Set error message in TempData if an exception occurs
                    TempData["ErrorMessage"] = "An error occurred while processing your request.";
                }

                // Redirect to a success page or take appropriate action
                return RedirectToAction("Contact", "Home");
            }

            // If model is not valid, return to the same view with validation errors
            return View(eventRequest);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
