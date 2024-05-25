using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DASH_BOOKING.Models;

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

        public ActionResult Panel()
        {
            List<EventRequest> eventRequests = db.EventRequests.ToList();  // Fetch event requests from the database
            return View(eventRequests);
        }



        public ActionResult EventList()
        {
            return View();
        }

        public ActionResult UserList()
        {
            return View();
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
