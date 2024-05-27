using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DASH_BOOKING.Models;
using System.IO;
using System.Data.Entity;

namespace DASH_BOOKING.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts/LoginRegister
        public ActionResult LoginRegister()
        {
            return View();
        }

        public ActionResult Deactivated()
        {
            return View();
        }

        // GET: Event/Add
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvent(EventModel eventModel, HttpPostedFileBase[] imageFiles)
        {
            if (ModelState.IsValid)
            {
                if (imageFiles != null && imageFiles.Length > 0)
                {
                    eventModel.Images = new List<EventImage>();
                    foreach (var file in imageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var image = new EventImage
                            {
                                Image = SaveImage(file)
                            };
                            eventModel.Images.Add(image);
                        }
                    }
                }

                db.EventModels.Add(eventModel);
                db.SaveChanges();
                return RedirectToAction("EventList");
            }

            return View(eventModel);
        }

        private string SaveImage(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(Server.MapPath("~/Upload pictures/"), fileName); // Update the folder path if needed
            file.SaveAs(filePath);
            return "~/Upload pictures/" + fileName;
        }


        public ActionResult Panel()
        {
            List<EventRequest> eventRequests = db.EventRequests.ToList();  // Fetch event requests from the database
            return View(eventRequests);
        }

        [HttpPost]
        public ActionResult UpdateEventRequestStatus(int id, string status)
        {
            var eventRequest = db.EventRequests.Find(id);
            if (eventRequest != null)
            {
                eventRequest.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Panel");
        }

        public ActionResult EventList()
        {
            List<EventModel> eventModels = db.EventModels.ToList();
            return View(eventModels);  // Pass the list of events to the view
        }

        public ActionResult UserList()
        {
            List<User> users = db.Users.ToList();  // Fetch users from the database
            System.Diagnostics.Debug.WriteLine("Users Count: " + users.Count);  // Debugging: Check if users are being retrieved
            return View(users);  // Pass the list of users to the view
        }

        // POST: Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                if (existingUser != null)
                {
                    if (existingUser.Status == "Inactive")
                    {
                        return RedirectToAction("Deactivated", "Accounts");
                    }

                    if (existingUser.Role == "Admin")
                    {
                        return RedirectToAction("Panel", "Accounts");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View("LoginRegister");
        }


        // POST: Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string UserName, string Email, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (Password == ConfirmPassword)
                {
                    var user = new User
                    {
                        UserName = UserName,
                        Email = Email,
                        Password = Password,
                        Role = "User"  // Assign the default role as User
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("LoginRegister", "Accounts");
                }
                else
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                }
            }
            return View("LoginRegister");
        }

        public ActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                try
                {
                    // Update user status to inactive
                    user.Status = "Inactive";

                    // Save changes to the database
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the error (you can log to a file, event log, etc.)
                    System.Diagnostics.Debug.WriteLine("Error deactivating user: " + ex.Message);

                    // Optionally, return an error view
                    return View("Error", new HandleErrorInfo(ex, "Accounts", "DeleteUser"));
                }
            }
            return RedirectToAction("UserList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Event List Edit & Delete function
        // GET: Event/Edit
        public ActionResult Edit(int id)
        {
            var eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound(); // Handle not found case
            }
            return View(eventModel);
        }

        // POST: Event/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventModel eventModel, HttpPostedFileBase[] imageFiles)
        {
            if (ModelState.IsValid)
            {
                // Update the event model properties
                db.Entry(eventModel).State = EntityState.Modified;

                // Handle image file updates if needed
                if (imageFiles != null && imageFiles.Length > 0)
                {
                    // Remove existing images
                    eventModel.Images.Clear();

                    // Add new images
                    foreach (var file in imageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var image = new EventImage
                            {
                                Image = SaveImage(file)
                            };
                            eventModel.Images.Add(image);
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("EventList");
            }

            return View(eventModel);
        }

        // GET: Event/Delete
        public ActionResult Delete(int id)
        {
            var eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound(); // Handle not found case
            }
            return View(eventModel);
        }

        // POST: Event/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var eventModel = db.EventModels.Find(id);
            if (eventModel != null)
            {
                db.EventModels.Remove(eventModel);
                db.SaveChanges();
            }
            return RedirectToAction("EventList");
        }

    }
}
