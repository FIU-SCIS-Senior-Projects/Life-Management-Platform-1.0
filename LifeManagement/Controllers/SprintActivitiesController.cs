using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class SprintActivitiesController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        /****************************setup joy passion giving back*********************************************************/
        [Authorize]
        public ActionResult UserSetup()
        {

            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
           
            var sprint = db.Sprints.Where(a => a.UserId == user.Id).OrderByDescending(a=>a.DateFrom).FirstOrDefault();
            if(sprint!=null)
                return View(sprint);
            ViewBag.ErrorMsg = "This user does not have an sprint set up";
            return View("Error");

        }
        public PartialViewResult Joy(int id)
        {
            try
            {
                var activities = db.SprintActivities.Where(a=>a.Activity.Category.Name=="Joy" && a.Sprint.Id==id).ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {
               
                return PartialView(null);
            }
          
        }
        public PartialViewResult Passion(int id)
        {
            try
            {
                var activities = db.SprintActivities.Where(a => a.Activity.Category.Name == "Passion" && a.Sprint.Id == id).ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }
        public PartialViewResult GivingBack(int id)
        {
            try
            {
                var activities = db.SprintActivities.Where(a => a.Activity.Category.Name == "Giving Back" && a.Sprint.Id == id).ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }

      
        [HttpPost]
        public PartialViewResult UpdateSprint(Act[] activities,string cat)
        { var newacts = new List<SprintActivities>();

           if (activities == null || !activities.Any())
                return PartialView("ErrorPartial");
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            var sprint = db.Sprints.Find(activities[0].sprintId);
            var activity = db.Activities.Find(activities[0].activityId);
            if (sprint == null || activity == null || user==null)
                return PartialView("ErrorPartial");


            db.SprintActivities.RemoveRange(db.SprintActivities.Where(a => a.SprintId ==sprint.Id && a.Activity.Category.Id == activity.Category.Id).AsEnumerable().ToList());
          
                db.SaveChanges();
            foreach (var act in activities)
            {
              

                var newSprintAct = new SprintActivities();
                newSprintAct.SprintId = act.sprintId;
                newSprintAct.ActivityId = act.activityId;
                newSprintAct.Specifics = act.spec;
                db.SprintActivities.Add(newSprintAct);
                newacts.Add(newSprintAct);
              
              
            }
            if (newacts.Any())
            {
                db.SprintActivities.AddRange(newacts);
                db.SaveChanges();
            }
            var res = db.SprintActivities.Where(a => a.Activity.Category.Name == "Giving Back" && a.Sprint.Id == sprint.Id).Include(a => a.Activity);

            if (cat == "joy")
            {
                 res = db.SprintActivities.Where(a => a.Activity.Category.Name == "Joy" && a.Sprint.Id == sprint.Id).Include(a => a.Activity);
                return PartialView("Joy", res.ToList());
            }
            if (cat == "passion")
            {
                 res = db.SprintActivities.Where(a => a.Activity.Category.Name == "Passion" && a.Sprint.Id == sprint.Id).Include(a=>a.Activity);
                return PartialView("Passion", res.ToList());
            }
            return PartialView("GivingBack", res.ToList());

        }
        /************************************system generated*************************************/
        // GET: SprintActivities
        public ActionResult Index()
        {
            var sprintActivities = db.SprintActivities.Include(s => s.Sprint).Include(s => s.Activity);
            return View(sprintActivities.ToList());
        }

        // GET: SprintActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SprintActivities sprintActivities = db.SprintActivities.Find(id);
            if (sprintActivities == null)
            {
                return HttpNotFound();
            }
            return View(sprintActivities);
        }

        // GET: SprintActivities/Create
        public ActionResult Create()
        {
            ViewBag.SprintId = new SelectList(db.Sprints, "Id", "SprintGoal");
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            return View();
        }

        // POST: SprintActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Specifics,SprintId,ActivityId")] SprintActivities sprintActivities)
        {
            if (ModelState.IsValid)
            {
                db.SprintActivities.Add(sprintActivities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SprintId = new SelectList(db.Sprints, "Id", "SprintGoal", sprintActivities.SprintId);
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", sprintActivities.ActivityId);
            return View(sprintActivities);
        }

        // GET: SprintActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SprintActivities sprintActivities = db.SprintActivities.Find(id);
            if (sprintActivities == null)
            {
                return HttpNotFound();
            }
            ViewBag.SprintId = new SelectList(db.Sprints, "Id", "SprintGoal", sprintActivities.SprintId);
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", sprintActivities.ActivityId);
            return View(sprintActivities);
        }

        // POST: SprintActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Specifics,SprintId,ActivityId")] SprintActivities sprintActivities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sprintActivities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SprintId = new SelectList(db.Sprints, "Id", "SprintGoal", sprintActivities.SprintId);
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", sprintActivities.ActivityId);
            return View(sprintActivities);
        }

        // GET: SprintActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SprintActivities sprintActivities = db.SprintActivities.Find(id);
            if (sprintActivities == null)
            {
                return HttpNotFound();
            }
            return View(sprintActivities);
        }

        // POST: SprintActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SprintActivities sprintActivities = db.SprintActivities.Find(id);
            db.SprintActivities.Remove(sprintActivities);
            db.SaveChanges();
            return RedirectToAction("Index");
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
