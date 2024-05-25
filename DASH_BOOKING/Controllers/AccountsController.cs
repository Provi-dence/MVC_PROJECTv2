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
            return View();
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
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                if (existingUser != null)
                {
                    // Logic for successful login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View("LoginRegister", user);
        }

        // POST: Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                // Logic for successful registration
                return RedirectToAction("LoginRegister");
            }
            return View("LoginRegister", user);
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
