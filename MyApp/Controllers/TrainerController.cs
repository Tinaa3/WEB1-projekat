using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class TrainerController : Controller
    {
        // GET: Trainer
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            var trainings = new List<TrainingDTO>();
            DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Trainer_TrainingsIDs.Contains(t.ID) 
                && t.TrainingTime.AddMinutes(t.DurationTime) > DateTime.Now).ForEach(tr => trainings.Add(new TrainingDTO(tr)));
            ViewBag.Trainings = trainings;
            if (Session["trainerTraining"] != null)
            {
                ViewBag.Training = ((Training)Session["trainerTraining"]).Clone();
                Session["trainerTraining"] = null;
            }
            return View();
        }

        public ActionResult History()
        {
            User user = (User)Session["user"];
            var trainings = new List<TrainingDTO>();
            if (Session["trainerHistory"] == null)
            {
                DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Trainer_TrainingsIDs.Contains(t.ID)
                    && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now).ForEach(tr => trainings.Add(new TrainingDTO(tr)));
            }
            else
            {
                ((List<Training>)Session["trainerHistory"]).ForEach(tr => trainings.Add(new TrainingDTO(tr)));
                Session["trainerHistory"] = null;
            }
            ViewBag.Trainings = trainings;
            return View();
        }

        public ActionResult Search(string name, string type, string min, string max)
        {
            User user = (User)Session["user"];
            var trainings = DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Trainer_TrainingsIDs.Contains(t.ID)
                && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now);
            Session["trainerHistory"] = trainings.FindAll(t => (string.IsNullOrWhiteSpace(name) || t.Name == name) &&
                (string.IsNullOrWhiteSpace(type) || Enum.GetName(typeof(TrainingType), t.Type) == type) &&
                (string.IsNullOrWhiteSpace(min) || DateTime.Parse(min) < t.TrainingTime) &&
                (string.IsNullOrWhiteSpace(max) || DateTime.Parse(max) > t.TrainingTime));
            return RedirectToAction("History");
        }

        public ActionResult Sort(string orderFrom, string orderBy)
        {
            User user = (User)Session["user"];
            var trainings = DataControl.Trainings.Get().FindAll(t => !t.Deleted && user.Trainer_TrainingsIDs.Contains(t.ID)
               && t.TrainingTime.AddMinutes(t.DurationTime) < DateTime.Now);
            if (orderFrom == "name")
            {
                if (orderBy == "asc")
                    Session["trainerHistory"] = trainings.OrderBy(t => t.Name).ToList();
                else
                    Session["trainerHistory"] = trainings.OrderByDescending(t => t.Name).ToList();

            }
            else if (orderFrom == "date")
            {
                if (orderBy == "asc")
                    Session["trainerHistory"] = trainings.OrderBy(t => t.TrainingTime).ToList();
                else
                    Session["trainerHistory"] = trainings.OrderByDescending(t => t.TrainingTime).ToList();
            }
            else
            {
                if (orderBy == "asc")
                    Session["trainerHistory"] = trainings.OrderBy(t => t.Type).ToList();
                else
                    Session["trainerHistory"] = trainings.OrderByDescending(t => t.Type).ToList();
            }
            return RedirectToAction("History");
        }

        public ActionResult Visitors(int id)
        {
            ViewBag.Training = new TrainingDTO(DataControl.Trainings.Get(id));
            return View();
        }//al

        [HttpPost]
        public ActionResult Create(Training t)
        {
            User user = (User)Session["user"];
            if (t.TrainingTime < DateTime.Now.AddDays(3))
                return RedirectToAction("Index", "Trainer");
            t.FitnessCenterID = user.Trainer_FitnessCenterID;
            DataControl.Trainings.Add(t);
            user.Trainer_TrainingsIDs.Add(DataControl.Trainings.Get().Count - 1);
            DataControl.Users.Update(user.ID, user);
            return RedirectToAction("Index", "Trainer");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            DataControl.Trainings.Delete(id);
            return RedirectToAction("Index", "Trainer");
        }

        public ActionResult Update(int id)
        {
            Session["trainerTraining"] = DataControl.Trainings.Get(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(Training t)
        {
            var training = DataControl.Trainings.Get(t.ID);
            training.Name = t.Name;
            training.MaxVisitors = t.MaxVisitors;
            training.DurationTime = t.DurationTime;
            training.TrainingTime = t.TrainingTime;
            training.Type = t.Type;
            DataControl.Trainings.Update(training.ID, training);
            return RedirectToAction("Index", "Trainer");
        }
    }
}