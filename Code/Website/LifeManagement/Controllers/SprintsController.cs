using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class SprintsController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        // GET: Sprints
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sprints/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public PartialViewResult NewSprint()
        {
            return PartialView();
        }

        public PartialViewResult ScoreSummary(int id)
        {
            var sprint = db.Sprints.Find(id);
            if(sprint!=null)
            return PartialView(sprint);

            ViewBag.ErrorMsg = "Could not get summary";
            return PartialView("ErrorPartial");
        }

        public ActionResult ScoreSummaryCoaches(int id)
        {
            var sprint = db.Sprints.Find(id);
           
            if (sprint != null) {
                var user = db.Users.Find(sprint.UserId);

                if (user != null)
                {
                    ViewBag.NameUser = user.FirstName + " " + user.LastName;
                }
                return View(sprint);

            }

            ViewBag.ErrorMsg = "Could not get summary";
            return View("Error");
        }

        // GET: Sprints/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Sprints/Create
        [HttpPost]
        public ActionResult Create(Sprint newSprint)
        {
            try
            {
                // TODO: Add insert logic here
                var user = (db.Users.Where(a => a.username.ToLower() == (User.Identity.Name).ToLower()).FirstOrDefault());
                var lastsprint = db.Sprints.Where(a => a.UserId == user.Id).OrderByDescending(a => a.DateFrom).FirstOrDefault();

                if (user != null)
                {
                    newSprint.UserId = user.Id;
                    db.Sprints.Add(newSprint);
                    db.SaveChanges();

                    var sprintActivities = lastsprint.SprintActivities;

                    foreach (SprintActivities sp in sprintActivities.ToList())
                    {
                        var newsprintA = new SprintActivities();
                        newsprintA = sp;
                        newsprintA.SprintId = newSprint.Id;
                        db.SprintActivities.Add(newsprintA);
                        db.SaveChanges();
                    }

                    var sprintGoals = lastsprint.Goals;
                    var goal1 = new Goal();
                    var goal2 = new Goal();
                    var goal3 = new Goal();

                    foreach (Goal g in sprintGoals.ToList())
                    {
                        var newGoal = new Goal();
                        newGoal = g;
                        newGoal.SprintId = newSprint.Id;
                        db.Goals.Add(newGoal);
                        db.SaveChanges();

                        switch (newGoal.CategoryId)
                        {
                            case 1: goal1 = newGoal; break;
                            case 2: goal2 = newGoal; break;
                            case 3: goal3 = newGoal; break;
                        }
                    }

                    ViewBag.goalJoy = goal1.Description;
                    ViewBag.goalPassion = goal2.Description;
                    ViewBag.goalGB = goal3.Description;

                    TempData["newSprint"] = newSprint;
                    TempData.Keep("newSprint");

                    return View("ConfigureNewSprint", newSprint);
                }
                else
                {
                    ViewBag.ErrorMsg = "There is no user logged in";
                    return PartialView("ErrorPartial");
                }

            }
            catch
            {
                ViewBag.ErrorMsg = "There was an unexpected error";
                return PartialView("ErrorPartial");
            }
        }

        public ActionResult ConfigureNewSprint()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            var lastsprint =
                db.Sprints.Where(a => a.UserId == user.Id).OrderByDescending(a => a.DateFrom).FirstOrDefault();

            var newSprint = new Sprint();
            newSprint.SprintGoal = lastsprint.SprintGoal;
            newSprint.DateFrom = DateTime.Now;
            newSprint.UserId = user.Id;
            db.Sprints.Add(newSprint);
            db.SaveChanges();

            var sprintActivities = lastsprint.SprintActivities;
            
            foreach(SprintActivities sp in sprintActivities.ToList())
            {
                var newsprintA = new SprintActivities();
                newsprintA = sp;
                newsprintA.SprintId = newSprint.Id;
                db.SprintActivities.Add(newsprintA);
                db.SaveChanges();
            }

            var sprintGoals = lastsprint.Goals;
            var goal1 = new Goal();
            var goal2 = new Goal();
            var goal3 = new Goal();

            foreach (Goal g in sprintGoals.ToList())
            {
                var newGoal = new Goal();
                newGoal = g;
                newGoal.SprintId = newSprint.Id;
                db.Goals.Add(newGoal);
                db.SaveChanges();

                switch (newGoal.CategoryId)
                {
                    case 1: goal1 = newGoal; break;
                    case 2: goal2 = newGoal; break;
                    case 3: goal3 = newGoal; break;
                }
            }

            ViewBag.goalJoy = goal1.Description;
            ViewBag.goalPassion = goal2.Description;
            ViewBag.goalGB = goal3.Description;

            TempData["newSprint"] = newSprint;
            TempData.Keep("newSprint");

            return View(newSprint);

        }

        [HttpPost]
        public ActionResult ConfigureNewSprint( QuestionarieViewModel quest)
        {
            if(TempData["newSprint"] != null)
            {
                var sprint = TempData["newSprint"] as Sprint;

                Sprint newSprint = db.Sprints.Find(sprint.Id);

                if (quest.vision != newSprint.SprintGoal)
                {
                    newSprint.SprintGoal = quest.vision;        //Im using vision as the SprintGoal to reuse the previos QuestionnaireViewModel
                    db.SaveChanges();
                }

                var sprintGoals = newSprint.Goals;
     
            string goalJoy = quest.goal_1;
            string goalPassion = quest.goal_2;
            string goalGB= quest.goal_3;

                foreach (Goal g in sprintGoals.ToList())
                {
                    Goal newGoal = db.Goals.Find(g.Id);
                    
                    if(newGoal.CategoryId == 1 && newGoal.Description != goalJoy)
                    { 
                    newGoal.Description = goalJoy;
                    db.SaveChanges();
                    }
                    else if (newGoal.CategoryId == 2 && newGoal.Description != goalPassion)
                    {
                        newGoal.Description = goalPassion;
                        db.SaveChanges();
                    }
                    else if (newGoal.CategoryId == 3 && newGoal.Description != goalGB)
                    {
                        newGoal.Description = goalGB;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Dashboard", "Users");
            }
            ViewBag.ErrorMsg = "There was an unexpected error while configuring your productivity week, try again later";
            return View("Error");
        }

            public ActionResult SetupSprint()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetupSprint(SprintViewModel model)
        {
            if(model != null)
            return View();

            ViewBag.ErrorMsg = "An error ocurred";
            return View("Error");
        }

        // GET: Sprints/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sprints/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sprints/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sprints/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
