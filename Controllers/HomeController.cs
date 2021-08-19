using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wall.Models;

namespace Wall.Controllers
{
    public class HomeController : Controller
    {
        private WallContext db;
        public HomeController(WallContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(db.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View("Index");
                }
            }

            if(ModelState.IsValid == false)
            {
                return View("Index");
            }
            
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserID", newUser.UserID);
            HttpContext.Session.SetString("FirstName", newUser.FirstName);

            return RedirectToAction("Dashboard", "Wall");
        }

        [HttpPost("login/user")]
        public IActionResult LoginUser(LoginUser logUser)
        {
            if(ModelState.IsValid == false)
            {
                return View("Index");
            }

            User dbUser = db.Users.FirstOrDefault(user => user.Email == logUser.LogEmail);
            if(dbUser == null)
            {
                ModelState.AddModelError("LogEmail", "Email not found.");
                return View("Index");
            }

            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult pwCompare = hasher.VerifyHashedPassword(logUser, dbUser.Password, logUser.LogPassword);

            if(pwCompare == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                return View("Index");
            }


            HttpContext.Session.SetInt32("UserID", dbUser.UserID);
            HttpContext.Session.SetString("FirstName", dbUser.FirstName);

            return RedirectToAction("Dashboard", "Wall");
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
