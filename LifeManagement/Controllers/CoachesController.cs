using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;
using System.Web.Security;

namespace LifeManagement.Controllers
{
    public class CoachesController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();

        // GET: Coaches
        public ActionResult Index()
        {
            return View(db.Coaches.ToList());
        }

        // GET: Coaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // GET: Coaches/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoachViewModel coach)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles.Where(a => a.Name == Constants.ROLES.COACH).FirstOrDefault();

                if (db.Coaches.Where(a => a.Username.ToLower() == coach.Username.ToLower()).Count() > 0)
                {
                    ViewBag.ErrorMsg = "That username is already taken, try another one";
                    return View(coach);
                }

                if (role != null)
                {

                    var coachOb = new Coach();
                    coachOb.FirstName = coach.FirstName;
                    coachOb.LastName = coach.LastName;
                    coachOb.ReviewScore = 5;    //5 by default
                    coachOb.Biography = coach.Biography;
                    coachOb.Skills = coach.Skills;
                    coachOb.Username = coach.Username;
                    coachOb.Password = coach.Password;
                    coachOb.RoleId = role.Id;

                    db.Coaches.Add(coachOb);
                    db.SaveChanges();
                
                    return RedirectToAction("DashBoard", "Users");
                }
                else
                {
                    ViewBag.ErrorMsg = "Authentication error!";
                    View("Error");
                }
            }

            return View();
        }

        // GET: Coaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            
            return View(coach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReviewScore,Biography,Skills,Username,Password,FirstName,LastName,Avatar,AvatarMime, RoleId")] Coach coach)
        {
            if (ModelState.IsValid)
            {

                db.Coaches.Attach(coach);

                var entry = db.Entry(coach);
                entry.State = EntityState.Modified;

                entry.Property(e => e.RoleId).IsModified = false;

                try { 
                db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = "Error! There can not be empty fields";
                    return View(coach);
                }

                return RedirectToAction("Index");
            }
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coach coach = db.Coaches.Find(id);
            db.Coaches.Remove(coach);
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
