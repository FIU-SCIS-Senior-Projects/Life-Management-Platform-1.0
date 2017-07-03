using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;
using System.Data.Entity;

namespace LifeManagement.Controllers
{
    public class CoachReviewsController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        // GET: CoachReviews
        public PartialViewResult Revise()
        {
            return PartialView();
        }
        [Authorize(Roles="Admin")]
        public PartialViewResult PendingList()
        {
            var pending = db.CoachReviews.Where(a => a.ApprovedBy == null);
            if (pending.Any())
                return PartialView("PendingList",pending.ToList());

            ViewBag.ErrorMsg = "No pending reviews";
            return PartialView("ErrorPartial");

        }

        public PartialViewResult Details(int id)
        {
            var review = db.CoachReviews.Find(id);
            if (review == null)
            {
                ViewBag.ErrorMsg = "Invalid review";
                return PartialView("ErrorPartial");
            }
            return PartialView(review);
        }

        public PartialViewResult CompleteReview(int id, bool isApproved)
        {
            var admin = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower() && a.Role.Name=="Admin").FirstOrDefault();
            if (admin == null)
            {
                ViewBag.ErrorMsg = "Cannot approve or reject. Not an admin";
                return PartialView("ErrorPartial");
            }
            var review = db.CoachReviews.Find(id);
            if (review == null)
            {
                ViewBag.ErrorMsg = "Invalid review";
                return PartialView("ErrorPartial");
            }
            db.Entry(review).State = EntityState.Modified;

            review.Approved = isApproved;
            review.DateApproved=DateTime.Now;
            review.ApprovedBy = admin.Id;
            db.SaveChanges();

            recalculateScore(review.CoachId);
            
            return PendingList();


        }

        private void recalculateScore(int coachid)
        {
            var coach = db.Coaches.Find(coachid);
            if (coach != null)
            {
                var reviews = coach.CoachReviews.Where(a => a.Approved == true);
                coach.ReviewScore = (int) (reviews.Sum(a => a.Score) / reviews.Count());
                db.SaveChanges();
            }
        }
    }
}