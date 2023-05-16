using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        [NonAction]
        private void UpdateUser(ref User oldUser, User user)
        {
            oldUser.Username = user.Username;
            oldUser.Password = user.Password;
            oldUser.Email = user.Email;
            oldUser.Birthday = user.Birthday;
            oldUser.Gender = user.Gender;
            oldUser.Name = user.Name;
            oldUser.Lastname = user.Lastname;
        }

        [HttpPost]
        public ActionResult Update(User user)
        {
            var oldUser = (User)Session["user"];
            if (DataControl.Users.Get().Find(t => t.Username == user.Username) != null && user.Username != oldUser.Username)
            {
                ViewBag.Poruka = "Postoji korisnicko ime";
                return View("~/Views/Auth/Index.cshtml");
            }
            UpdateUser(ref oldUser, user);
            DataControl.Users.Update(oldUser.ID, oldUser);
            return RedirectToAction("Index");
        }

        public ActionResult Register(User user)
        {
            if (DataControl.Users.Get().Find(t => t.Username == user.Username) != null)
            {
                ViewBag.Poruka = "Postoji korisnicko ime";
                return View("~/Views/Home/Register.cshtml");
            }
            DataControl.Users.Add(user);
            return RedirectToAction("Index", "Home"); ;
        }

        public ActionResult Login(string username, string password)
        {
            var user = DataControl.Users.Get().Find(t => !t.Deleted && t.Username == username && t.Password == password);
            if (user != null)
            {
                Session["user"] = user;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Poruka = "Pogresni podaci";
                return View("~/Views/Home/Login.cshtml");
            }
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}