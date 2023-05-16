using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            ViewBag.FitnessCenters = DataControl.FitnessCenter.Get().FindAll(t => !t.Deleted &&
                user.Owner_FitnessCentersIDs.Contains(t.ID));
            if(Session["ownerFC"] != null)
            {
                ViewBag.FitnessCenter = ((FitnessCenter)Session["ownerFC"]).Clone();
                Session["ownerFC"] = null;
            }
            return View();
        }

        public ActionResult Trainers()
        {
            User user = (User)Session["user"];
            var fcs = DataControl.FitnessCenter.Get().FindAll(t => !t.Deleted &&
                user.Owner_FitnessCentersIDs.Contains(t.ID));
            var trainers = new List<User>();
            foreach (var i in fcs)
                DataControl.Users.Get().FindAll(t => !t.Deleted 
                    && t.Trainer_FitnessCenterID == i.ID).ForEach(u => trainers.Add(u));
            ViewBag.FitnessCenters = fcs;
            ViewBag.Trainers = trainers;
            return View();
        }

        [HttpPost]
        public ActionResult Block(int id)
        {
            DataControl.Users.Delete(id);
            return RedirectToAction("Trainers", "Owner");
        }


        [HttpPost]
        public ActionResult Create(FitnessCenter f, Address a)
        {
            User user = (User)Session["user"];
            f.Address = a;
            f.OwnerID = user.ID;
            DataControl.FitnessCenter.Add(f);
            user.Owner_FitnessCentersIDs.Add(DataControl.FitnessCenter.Get().Count - 1);
            DataControl.Users.Update(user.ID, user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (DataControl.Trainings.Get().Find(t => !t.Deleted && t.TrainingTime > DateTime.Now && t.FitnessCenterID == id) != null)
                return RedirectToAction("Index");
            DataControl.FitnessCenter.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            Session["ownerFC"] = DataControl.FitnessCenter.Get(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(FitnessCenter f, Address a)
        {
            var fc = DataControl.FitnessCenter.Get(f.ID);
            fc.Name = f.Name;
            fc.Address = a;
            fc.OpeningYear = f.OpeningYear;
            fc.YearFee = f.YearFee;
            fc.MonthFee = f.MonthFee;
            fc.GroupFee = f.GroupFee;
            fc.TrainingFee = f.TrainingFee;
            fc.PersonalFee = f.PersonalFee;
            DataControl.FitnessCenter.Update(fc.ID, fc);
            return RedirectToAction("Index");
        }

        public ActionResult Comments()
        {
            User user = (User)Session["user"];
            var comments = new List<CommentDTO>();
            DataControl.Comments.Get().FindAll(t => !t.Deleted && user.Owner_FitnessCentersIDs.Contains(t.FitnessCenterID) 
                && !t.Visible).ForEach(c => comments.Add(new CommentDTO(c)));
            ViewBag.Comments = comments;
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int id, string visible)
        {
            var c = DataControl.Comments.Get(id);
            if (string.IsNullOrWhiteSpace(visible))
                c.Deleted = true;
            else
                c.Visible = true;
            DataControl.Comments.Update(c.ID, c);
            return RedirectToAction("Comments");
        }
    }
}