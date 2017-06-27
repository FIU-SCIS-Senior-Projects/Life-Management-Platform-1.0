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
        private Common common = new Common();
        /**************working with coaches fernando******************/
        public JsonResult GetCoaches()
        {
            var coaches = db.Coaches;
            List<CoachListVM> result = new List<CoachListVM>();
            foreach (var a in coaches)
            {
                var newcoach = new CoachListVM()
                {
                    CoachId = a.Id,
                    AvatarStr64 = common.SignatureImageStr64(a.Avatar, a.AvatarMime),
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Skills = a.Skills,
                    ReviewScore = a.ReviewScore
                };
                result.Add(newcoach);
            }
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CoachesListAngular()
        {
            return PartialView();
        }
        public PartialViewResult CoachesChatList()
        {
            var coaches = db.Coaches.ToList();
            return PartialView(coaches);
        }
        public PartialViewResult CoachesList()
        {
            var coaches = db.Coaches.ToList();
            return PartialView(coaches);
        }
        public ActionResult SeeCoaches()
        {
            return View();
        }
       /********************************************************/
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

        // This is the page Users see
        public PartialViewResult CoachDetailsPage(int? id)
        {
            if (id == null)
            {
                return PartialView("ErrorPartial");
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return PartialView("ErrorPartial");
            }
            return PartialView(coach);
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

                if (db.Coaches.Where(a => a.Email.ToLower() == coach.Email.ToLower()).Count() > 0)
                {
                    ViewBag.DuplicateEmail = "This email address is already registered in the system.";
                    return View(coach);
                }

                if (db.Coaches.Where(a => a.Username.ToLower() == coach.Username.ToLower()).Count() > 0)
                {
                    ViewBag.DuplicateUserName = "That username is already taken, try another one";
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
                    coachOb.Email = coach.Email;

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
        public ActionResult Edit( Coach coachData)
        {
            if (coachData.FirstName == null || coachData.LastName == null || coachData.Biography == null
                       || coachData.Skills == null || coachData.Username == null
                       || coachData.Password == null)
            {
                ViewBag.ErrorMsg = "Error! There can not be empty fields";
                return View(coachData);
            }

                if (ModelState.IsValid)
            {
                var coach = db.Coaches.Find(coachData.Id);

                if(coach != null) { 
                if (coach.FirstName != coachData.FirstName) coach.FirstName = coachData.FirstName;
                if (coach.LastName != coachData.LastName) coach.LastName = coachData.LastName;
                if (coach.Biography != coachData.Biography) coach.Biography = coachData.Biography;
                if (coach.Skills != coachData.Skills) coach.Skills = coachData.Skills;
                if (coach.Username != coachData.Username) coach.Username = coachData.Username;
                if (coach.Password != coachData.Password) coach.Password = coachData.Password;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = "Error! FirstName, LastName, Password, and UserName must have at most 20 characters, \n " +
                            "please check your changes and try again.";
                    return View(coachData);
                }
                    return RedirectToAction("Index");
                }
            }
            return View(coachData);
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

        public PartialViewResult CoachEditProfile()
        {
            var coach = db.Coaches.Where(c => c.Username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            return PartialView(coach);
        }

        [HttpPost]
        public ActionResult CoachEditProfile(string FirstName, string LastName, string Biography, string Skills)
        {


            var coach = db.Coaches.Where(c => c.Username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            if (coach != null)
            {
                if (coach.FirstName != FirstName) coach.FirstName = FirstName;
                if (coach.LastName != LastName) coach.LastName = LastName;
                if (coach.Biography != Biography) coach.Biography = Biography;
                if (coach.Skills != Skills) coach.Skills = Skills;

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (common.saveImageBytesCoach(coach, file))
                    {
                        coach.Avatar = common.ResizeImageFile(coach.Avatar, 300);
                    }
                }

                db.SaveChanges();
            }
            return View();
        }


        [AllowAnonymous]
        public ActionResult LoginCoach()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoginCoach(string Username, string Password)
        {
            var coach = db.Coaches.Where(a => a.Username.ToLower() == Username.ToLower()).FirstOrDefault();

            if (coach != null && coach.Password == Password)
            {
                FormsAuthentication.SetAuthCookie(coach.Username, false);
                return RedirectToAction("DashBoard", "Users");
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPass(string email)
        {

            var coach = db.Coaches.Where(e => e.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (coach != null)
            {
                string subject = "Password reset requested";
                string message = "Dear " + coach.FirstName + ": <br/>" + "<p> You are receiving this email because you forgot your password for the Life Management system,"
                    + " to reset your password please follow <a href= \"" + @Url.Action("PasswordRecovery", "Coaches", null, Request.Url.Scheme) + "/" + coach.Id + "\">this link </a> and fill out the corresponding fields. </p> <br/>"
                    + "<p> Sincerely, <br/> The Life Management Team. </p>";

                Common.sendEmail(coach.Email, subject, message);
            }
            else
            {
                return new HttpStatusCodeResult(400, "");
            }


            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult PasswordRecovery(int? id)
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePass(string newpass, string newpass_conf, int id)
        {


            if (newpass != newpass_conf)
            {
                return new HttpStatusCodeResult(400, "");
            }

            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }

            if (coach != null)
            {
                coach.Password = newpass;
                db.SaveChanges();
            }

            return RedirectToAction("LoginCoach"); ;
        }

        public PartialViewResult ProfilePic()
        {
            var coach = db.Coaches.Where(c => c.Username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            return PartialView(coach);

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