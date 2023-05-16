using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.FitnessCenters = DataControl.FitnessCenter.Get().FindAll(t => !t.Deleted);
            return View();
        }

        public ActionResult Sort(string orderFrom, string orderBy)
        {
            var list = DataControl.FitnessCenter.Get().FindAll(t => !t.Deleted);
            if (orderFrom == "name")
            {
                if (orderBy == "asc")
                    ViewBag.FitnessCenters = list.OrderBy(t => t.Name).ToList();
                else
                    ViewBag.FitnessCenters = list.OrderByDescending(t => t.Name).ToList();
            }
            else if (orderFrom == "year")
            {
                if (orderBy == "asc")
                    ViewBag.FitnessCenters = list.OrderBy(t => t.OpeningYear).ToList();
                else
                    ViewBag.FitnessCenters = list.OrderByDescending(t => t.OpeningYear).ToList();
            }
            else
            {
                if (orderBy == "asc")
                    ViewBag.FitnessCenters = list.OrderBy(t => t.Address.ToString()).ToList();
                else
                    ViewBag.FitnessCenters = list.OrderByDescending(t => t.Address.ToString()).ToList();
            }
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Search(string name, string address, int min = -1, int max = -1)
        {
            if (min > max) return RedirectToAction("Index", "Home");
            var list = DataControl.FitnessCenter.Get().FindAll(t => !t.Deleted);
            ViewBag.FitnessCenters = list.FindAll(t => (string.IsNullOrWhiteSpace(name) ? true : t.Name == name)
             && (string.IsNullOrWhiteSpace(address) ? true : t.Address.ToString().Contains(address))
             && (min == -1 ? true : t.OpeningYear > min)
             && (max == -1 ? true : t.OpeningYear < max));

            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Details(int id)
        {
            var fc = DataControl.FitnessCenter.Get(id);
            ViewBag.FitnessCenter = fc;
            ViewBag.Owner = DataControl.Users.Get(fc.OwnerID);
            ViewBag.Trainings = DataControl.Trainings.Get().FindAll(t => t.FitnessCenterID == id && !t.Deleted 
                && t.TrainingTime > DateTime.Now);
            List<CommentDTO> comments = new List<CommentDTO>();
            DataControl.Comments.Get().FindAll(t => t.FitnessCenterID == id && !t.Deleted && t.Visible).ForEach(c => comments.Add(new CommentDTO(c)));
            ViewBag.Comments = comments;
            return View();
        }

    }
}