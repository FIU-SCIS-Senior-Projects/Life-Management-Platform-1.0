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

        public ActionResult UserSetup()
        {
            return View();
        }
        public PartialViewResult Joy()
        {
            try
            {
                var activities = db.Activities.ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {
               
                return PartialView(null);
            }
          
        }

        public bool UpdateSprint(int activityId, int sprintId,bool isInsert)
        {
            var activity = db.Activities.Find(activityId);
            var sprint = db.SprintActivities.Find(sprintId);
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if(user==null || activity ==null || sprint==null)
            return false;
            return true;



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
