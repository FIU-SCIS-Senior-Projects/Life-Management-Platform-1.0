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
        /*******************approve user activities******************/
        public PartialViewResult Revise()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public PartialViewResult PendingList()
        {
            var pending = db.Activities.Where(a => a.ApprovedBy == null);
            if (pending.Any())
                return PartialView("PendingList", pending.ToList());

            ViewBag.ErrorMsg = "No pending reviews";
            return PartialView("ErrorPartial");

        }

        public PartialViewResult Details(int id)
        {
            var review = db.Activities.Find(id);
            if (review == null)
            {
                ViewBag.ErrorMsg = "Invalid review";
                return PartialView("ErrorPartial");
            }
            return PartialView(review);
        }

        public PartialViewResult CompleteReview(int id, bool isApproved)
        {
            var admin = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower() && a.Role.Name == "Admin").FirstOrDefault();
            if (admin == null)
            {
                ViewBag.ErrorMsg = "Cannot approve or reject. Not an admin";
                return PartialView("ErrorPartial");
            }
            var act = db.Activities.Find(id);
            if (act == null)
            {
                ViewBag.ErrorMsg = "Invalid review";
                return PartialView("ErrorPartial");
            }
            db.Entry(act).State = EntityState.Modified;

            act.Approved = isApproved;
            act.DateApproved = DateTime.Now;
            act.ApprovedBy = admin.Id;
            db.SaveChanges();

          
            return PendingList();


        }
        /***************************customs************************/


        public PartialViewResult Joy()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Joy" && (a.Approved==true || a.CreatorId==user.Id)).ToList();
                return PartialView("Joy",activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }
        public PartialViewResult Passion()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Passion" && (a.Approved == true || a.CreatorId == user.Id)).ToList();
                return PartialView("Passion",activities);
            }
            catch (Exception e)
            {

                return PartialView(null);
            }

        }
        public PartialViewResult GivingBack()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            try
            {
                var activities = db.Activities.Where(a => a.Category.Name == "Giving Back" && (a.Approved == true || a.CreatorId == user.Id)).ToList();
                return PartialView("GivingBack",activities);
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
        public ActionResult CreateActivity(string Name, int CategoryId)
        {
   
            var file = Request.Files[0];

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
      
    
        // GET: Activities/Create
        [HttpPost]
        public PartialViewResult Create(string name, string category)
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            var cat = db.Categories.Where(a => a.Name.ToLower() == category.ToLower()).FirstOrDefault();
            if (cat == null)
            {
                ViewBag.ErrorMsg = "Invalid category";
                return PartialView("ErrorPartial");
            }

            var file = Request.Files[0];

            Activity activity = new Activity();
            activity.Name = name;
            activity.CategoryId = cat.Id;
            activity.Approved = null;
            activity.CreatorId = user.Id;
        

            if (common.saveImageBytes(activity, file))
            {

                activity.Img = common.ResizeImageFile(activity.Img, 200);

                db.Activities.Add(activity);
                db.SaveChanges();
                if (cat.Name == "Joy")
                    return Joy();
                if (cat.Name == "Passion")
                    return Passion();

                return GivingBack();

            }

            ViewBag.ErrorMsg = " An unexpected error has occurred, try again later";
            return PartialView("ErrorPartial");
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
        public PartialViewResult Delete()
        {
            return PartialView();
        }

        // POST: Activities/Delete/5
        [HttpPost]
        public bool DeleteConfirmed(int id)
        {
            try
            {
                Activity activity = db.Activities.Find(id);
                db.Activities.Remove(activity);
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public ActionResult DeleteSelectedActivity(int id)
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
