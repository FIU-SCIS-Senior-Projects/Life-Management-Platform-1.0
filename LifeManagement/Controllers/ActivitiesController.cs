using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace LifeManagement.Controllers
{
    public class ActivitiesController : Controller
    {
        private Common common = new Common();
        private SeniorDBEntities db = new SeniorDBEntities();
        /***************************customs************************/
        public PartialViewResult Joy()
        {
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Joy").ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }
        public PartialViewResult Passion()
        {
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Passion").ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }
        public PartialViewResult GivingBack()
        {
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Giving Back").ToList();
                return PartialView(activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }

        public PartialViewResult CatDisplay(string cat)
        {
            var activities = db.Activities.Where(a => a.Category.Name.ToLower() == cat.ToLower());
            if (activities.Any())
                return PartialView(activities.ToList());

            ViewBag.ErrorMsg = "Could not find any activity with the given category";
            return PartialView("ErrorPartial");
        }
        /****************Testing Bea******************/
        public PartialViewResult CreateActivity()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateActivity(string Name, int CategoryId, HttpPostedFileBase file)
        {

            Activity activity = new Activity();
            activity.Name = Name;
            activity.CategoryId = CategoryId;

            if (common.saveImageBytes(activity, file))
                {

                    activity.Img = common.ResizeImageFile(activity.Img, 200);

                    db.Activities.Add(activity);
                    db.SaveChanges();
                    ViewBag.Msg = "Activity Successfully saved";
                    return RedirectToAction("Dashboard", "Users");
            }

            ViewBag.ErrorMsg = " An unexpected error has occurred, try again later";
            return View("Error");

        }



        /*********************system generated**********************/
        // GET: Activities
        public ActionResult Index()
        {
            var activities = db.Activities.Include(a => a.Category);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId,Img,ImgMime")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", activity.CategoryId);
            return View(activity);
        }

        public PartialViewResult EditActivity()
        {

            return PartialView();
        }
        public PartialViewResult EditSelectedActivity(int id)
        {
            var act = db.Activities.Find(id);
            if (act == null)
            {
                ViewBag.ErrorMsg = "Could not find activity";
                return PartialView("ErrorPartial");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", act.CategoryId);
            return PartialView(act);
        }
        [HttpPost]
        public bool SaveEditActivity(int id,string name,int cat)
        {
            try
            {
                var act = db.Activities.Find(id);
                if (act == null)
                {
                    return false;
                }
                act.Name = name;
                act.CategoryId = cat;
                var file = Request.Files[0];
                if (common.saveImageBytes(act, file))
                {

                    act.Img = common.ResizeImageFile(act.Img, 200);


                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", activity.CategoryId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,Img,ImgMime")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", activity.CategoryId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
