using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class VisitorController : Controller
    {
        // GET: Visitor
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            var trainings = new List<TrainingDTO>();
            if (Session["visitorList"] != null)
            {
                ((List<Training>)Session["visitorList"]).ForEach(tr => trainings.Add(new TrainingDTO(tr)));
                Session["visitorList"] = null;
            }
            else
            {
                DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Visitor_TrainingsIDs.Contains(t.ID)
                    && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now).ForEach(tr => trainings.Add(new TrainingDTO(tr)));
            }
            ViewBag.Trainings = trainings;
            var fcs = new List<FitnessCenter>();
            foreach (var t in trainings)
               if (!fcs.Contains(t.FitnessCenter))
                    fcs.Add(t.FitnessCenter);
            ViewBag.FitnessCenters = fcs;
            return View();
        }

        [HttpPost]
        public ActionResult TrainingEntry(int id)
        {
            User user = (User)Session["user"];
            user.Visitor_TrainingsIDs.Add(id);
            var training = DataControl.Trainings.Get(id);
            training.VisitorsIDs.Add(user.ID);
            DataControl.Trainings.Update(training.ID, training);
            DataControl.Users.Update(user.ID, user);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search(string name, string type, string centerName)
        {
            User user = (User)Session["user"];
            var trainings = DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Visitor_TrainingsIDs.Contains(t.ID)
                && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now);
            Session["visitorList"] = trainings.FindAll(t => (string.IsNullOrWhiteSpace(name) || t.Name == name) &&
                (string.IsNullOrWhiteSpace(type) || Enum.GetName(typeof(TrainingType), t.Type) == type) &&
                (string.IsNullOrWhiteSpace(centerName) || centerName == DataControl.FitnessCenter.Get(t.FitnessCenterID).Name));
            return RedirectToAction("Index");
        }

        public ActionResult Sort(string orderFrom, string orderBy)
        {
            User user = (User)Session["user"];
            var trainings = DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Visitor_TrainingsIDs.Contains(t.ID)
               && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now);
            if (orderFrom == "name")
            {
                if (orderBy == "asc")
                    Session["visitorList"] = trainings.OrderBy(t => t.Name).ToList();
                else
                    Session["visitorList"] = trainings.OrderByDescending(t => t.Name).ToList();

            }
            else if (orderFrom == "date")
            {
                if (orderBy == "asc")
                    Session["visitorList"] = trainings.OrderBy(t => t.TrainingTime).ToList();
                else
                    Session["visitorList"] = trainings.OrderByDescending(t => t.TrainingTime).ToList();
            }
            else
            {
                if (orderBy == "asc")
                    Session["visitorList"] = trainings.OrderBy(t => t.Type).ToList();
                else
                    Session["visitorList"] = trainings.OrderByDescending(t => t.Type).ToList();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Comment(Comment c)
        {
            c.VisitorID = ((User)Session["user"]).ID;
            DataControl.Comments.Add(c);
            return RedirectToAction("Index");
        }

    }
}