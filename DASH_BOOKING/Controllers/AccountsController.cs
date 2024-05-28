using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DASH_BOOKING.Models;
using System.IO;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

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
                // Check if the event already exists based on EventName and EventDate
                bool eventExists = db.EventModels.Any(e =>
                    e.EventName.Equals(eventModel.EventName, StringComparison.OrdinalIgnoreCase) &&
                    e.EventDate.Date == eventModel.EventDate.Date);

                if (eventExists)
                {
                    ModelState.AddModelError("", "Event with the same name and date already exists.");
                    return View(eventModel); // Return to the form with validation error
                }

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

                // Ensure the event category is not empty
                if (string.IsNullOrWhiteSpace(eventModel.EventCategory))
                {
                    ModelState.AddModelError("", "Event category is required.");
                    return View(eventModel); // Return to the form with validation error
                }

                db.EventModels.Add(eventModel);
                db.SaveChanges();
                return RedirectToAction("EventList");
            }

            return View(eventModel); // Return to the form with model errors
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
        // POST: Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    ModelState.AddModelError("", "Email and Password are required.");
                    return View("LoginRegister");
                }

                try
                {
                    // Case-insensitive comparison of email
                    var existingUser = db.Users.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

                    if (existingUser != null)
                    {
                        bool passwordMatches = false;
                        string hashedPassword = PasswordHasher.HashPassword(Password);

                        // Check if the existing password is already hashed
                        if (existingUser.Password == hashedPassword)
                        {
                            passwordMatches = true;
                        }
                        // Check if the existing password is plain text and matches the entered password
                        else if (existingUser.Password == Password)
                        {
                            passwordMatches = true;

                            // Update the password to the hashed version
                            existingUser.Password = hashedPassword;
                            db.SaveChanges();
                        }

                        if (passwordMatches)
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
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    System.Diagnostics.Debug.WriteLine("Error during login: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while processing your request.");
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
                    // Hash the password before saving
                    string hashedPassword = PasswordHasher.HashPassword(Password);

                    var user = new User
                    {
                        UserName = UserName,
                        Email = Email,
                        Password = hashedPassword, // Save the hashed password
                        IsPasswordHashed = true,   // Set the flag to true
                        Role = "User"
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


        [HttpPost]
        public JsonResult Delete(int id)
        {
            var eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return Json(new { success = false, message = "Event not found" });
            }

            db.EventModels.Remove(eventModel);
            db.SaveChanges();

            return Json(new { success = true });
        }

    }

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }


}
