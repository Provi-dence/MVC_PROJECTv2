using DASH_BOOKING.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DASH_BOOKING.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Events()
        {
            // Fetch EventImages along with their related EventModels
            var eventImages = db.EventImages.Include("EventModel").ToList();
            return View(eventImages);
        }

        public ActionResult EventDetails(int id)
        {
            var eventModel = GetEventDetailsById(id);
            if (eventModel == null)
            {
                return HttpNotFound();
            }
            return View(eventModel);
        }

        private EventModel GetEventDetailsById(int id)
        {
            // Corrected to use the EventModels DbSet
            return db.EventModels.Find(id); 
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(EventRequest eventRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    eventRequest.Status = "Pending";
                    db.EventRequests.Add(eventRequest);
                    db.SaveChanges();

                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = true, message = "Event request successfully submitted. Thank you!" });
                    }

                    TempData["SuccessMessage"] = "Event request successfully submitted. Thank you!";
                    return RedirectToAction("Contact", "Home");
                }
                catch (Exception ex)
                {
                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = false, message = "An error occurred while processing your request." });
                    }

                    TempData["ErrorMessage"] = "An error occurred while processing your request.";
                }
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = false, message = "Invalid data submitted." });
            }

            return View(eventRequest);
        }


        public ActionResult GenerateTicket(int id)
        {
            // Retrieve event details based on id
            var eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound();
            }

            // Generate random reference number
            string referenceNumber = RandomReferenceGenerator.GenerateRandomReference();

            // Pass eventModel and referenceNumber to the ticket generation view
            ViewBag.ReferenceNumber = referenceNumber;
            return View(eventModel);
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

    public class RandomReferenceGenerator
    {
        private const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ReferenceLength = 8;

        public static string GenerateRandomReference()
        {
            Random random = new Random();
            char[] referenceNumber = new char[ReferenceLength];

            for (int i = 0; i < ReferenceLength; i++)
            {
                referenceNumber[i] = CharSet[random.Next(CharSet.Length)];
            }

            return new string(referenceNumber);
        }
    }
}
