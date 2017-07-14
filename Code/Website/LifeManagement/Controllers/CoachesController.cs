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
        /*********************coach reviews***********************************/

        public ActionResult SeeReviews(int id)
        {
            return View(id);
        }

        public PartialViewResult CoachReviews(int id)
        {
            var coach = db.Coaches.Find(id);
            if (coach == null)
            {
                ViewBag.ErrorMsg = "Invalid coach";
                return  PartialView("ErrorPartial");
            }
            return PartialView(coach);
        }
        public PartialViewResult ReviewsList(int id)
        {
            var reviews = db.CoachReviews.Where(a=>a.CoachId==id && a.Approved==true);
            if (!reviews.Any())
            {
                ViewBag.NoReviews = "There are no reviews yet";

            }
            return PartialView(reviews.ToList());
        }
     
        public PartialViewResult WriteReview(int id)
        {
            return PartialView(id);
        }
   
        [HttpPost]
        public string SaveReview(int coachid, string review, int score)
        {
            if (String.IsNullOrEmpty(review))
            {
                Response.StatusCode = 500;
                return "Review cannot be empty";
            }

            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                Response.StatusCode = 500;
                return "Need to be a user";
            }
              
           
            var coach = db.Coaches.Find(coachid);
            if (coach == null)
            {
                Response.StatusCode = 500;
                return "Invalid coach";
            }
                
            
            coach.CoachReviews.Add(new CoachReview()
            {
                Review = review,
                Score = score,
                UserId = user.Id
                
            });
            db.SaveChanges();
            return "Review saved successfully";


        }
        /*********************************************************************/
        // GET: Coaches
        [Authorize(Roles = "Admin")]
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
                    coachOb.Password = Security.HashSHA1(coach.Password);
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
                       || coachData.Email == null)
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

                if (coach.Username != coachData.Username) {

                        if (db.Coaches.Where(a => a.Username.ToLower() == coachData.Username.ToLower()).Count() > 0)
                        {
                            ViewBag.DuplicateUserName = "The username you enter is already being used for another coach, try another one";
                            return View(coachData);
                        }
                        coach.Username = coachData.Username;
                    }

                    if (coach.Email != coachData.Email)
                    {

                        if (db.Coaches.Where(a => a.Email.ToLower() == coachData.Email.ToLower()).Count() > 0)
                        {
                            ViewBag.DuplicateEmail = "The email you enter is already being used for another coach, try another one";
                            return View(coachData);
                        }
                        coach.Email = coachData.Email;
                    }


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
            db.CoachReviews.RemoveRange(db.CoachReviews.Where(r => r.CoachId == coach.Id));
            db.SaveChanges();

            var forums = db.Forums.Where(r => r.CoachId == coach.Id).ToList();

            foreach (Forum f in forums)
            {
                db.ForumFiles.RemoveRange(db.ForumFiles.Where(ff => ff.ForumId == f.Id));
                db.Conversations.RemoveRange(db.Conversations.Where(c => c.ForumId == f.Id));
                db.SaveChanges();
                db.Forums.Remove(f);
            }

            db.SaveChanges();

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

            if (coach != null && coach.Password == Security.HashSHA1(Password))
            {
                FormsAuthentication.SetAuthCookie(coach.Username, false);
                return RedirectToAction("DashBoard");
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        public ActionResult DashBoard()
        {
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
                coach.Password = Security.HashSHA1(newpass);
                db.SaveChanges();
            }

            return RedirectToAction("LoginCoach"); ;
        }

        public PartialViewResult ProfilePic()
        {
            var coach = db.Coaches.Where(c => c.Username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            return PartialView(coach);

        }

        [HttpPost]
        public ActionResult ShareProgress(int id)
        {
            TempData["idSprint"] = id;
            TempData.Keep("idSprint");
             return View();
        }

        public PartialViewResult SelectCoach(int id)
        {
            //share score
            if (TempData["idSprint"] != null)
            {
                int sprintId = (int)TempData["idSprint"];

                var sprint = db.Sprints.Find(sprintId);
                var coach = db.Coaches.Find(id);
                var user = db.Users.Where(u => u.Id == sprint.UserId).FirstOrDefault();

                if (coach != null)
                {
                    string subject = "User " + user.FirstName + " " + user.LastName + " has shared his/her score summary with you!";
                    string message = "Dear " + coach.FirstName + ": <br/>" + "<p> You are receiving this email because user " + user.FirstName + " " + user.LastName + " wants you to review his/her performance. <br/>"
                        + " To see his/her scores, please follow <a href= \"" + @Url.Action("ScoreSummaryCoaches", "Sprints", null, Request.Url.Scheme) + "/" + sprint.Id + "\">this link. </a>  </p> <br/>"
                        + "<p> Best, <br/> The Life Management Team. </p>";

                    Common.sendEmail(coach.Email, subject, message);
                }
                TempData.Remove("idSprint");
            }

            //Share tabs
            else if(TempData["sprintActId"] != null && TempData["tabId"] != null)
            {
                
                int sprintActId = (int)TempData["sprintActId"];
                int tabId = (int)TempData["tabId"];

                var coach = db.Coaches.Find(id);
                var sprintAct = db.SprintActivities.Find(sprintActId);
                var user = sprintAct.Sprint.User;
                if (coach != null)
                {
                    string subject = "User " + user.FirstName + " " + user.LastName + " has shared his/her performance with you!";
                    string message = "Dear " + coach.FirstName + ": <br/>" + "<p> You are receiving this email because user <strong>" + user.FirstName + " " + user.LastName + " </strong>wants you to review his/her performance. <br/>"
                        + " To see his/her scores, please follow <a href= \"" + @Url.Action("ShareTabsLink", "Coaches", null, Request.Url.Scheme) + "?sprintActId=" + sprintActId + "&tab=" + tabId + "\">this link. </a>  </p> <br/>"
                        + "<p> Best, <br/> The Life Management Team. </p>";

                    Common.sendEmail(coach.Email, subject, message);
                }
                TempData.Remove("sprintActId");
                TempData.Remove("tabId");

            }

            return PartialView();
        }

        public PartialViewResult SeeCoachesUsers()
        {
            return PartialView();
        }

        public PartialViewResult SeeCoachesNoSelect()
        {
            return PartialView();
        }

        public PartialViewResult CoachesListGuest(string filter)
        {

            if (String.IsNullOrEmpty(filter))
                return PartialView("CoachesList", db.Coaches.ToList());

            return PartialView("CoachesList", db.Coaches.Where(a => a.Email.Contains(filter) || a.FirstName.Contains(filter) || a.LastName.Contains(filter) || a.Skills.Contains(filter)).ToList());
        }

        public PartialViewResult CoachesListUsers(string filter)
        {
            
            if(String.IsNullOrEmpty(filter))
                return PartialView(db.Coaches.ToList());

            return PartialView(db.Coaches.Where(a=>a.Email.Contains(filter) || a.FirstName.Contains(filter)||a.LastName.Contains(filter)||a.Skills.Contains(filter)).ToList());
        }

        public PartialViewResult CoachesListNoSelect(string filter)
        {

            if (String.IsNullOrEmpty(filter))
                return PartialView(db.Coaches.ToList());

            return PartialView(db.Coaches.Where(a => a.Email.Contains(filter) || a.FirstName.Contains(filter) || a.LastName.Contains(filter) || a.Skills.Contains(filter)).ToList());
        }

        [AllowAnonymous]
        public ActionResult ShareTabsLink(int sprintActId, int tab)
        {
            var sprintAct = db.SprintActivities.Find(sprintActId);


            if (sprintAct != null)
            {
                Sprint sId = db.Sprints.Find(sprintAct.SprintId);
                var list = db.SprintActivities.Where(a => a.Sprint.Id == sId.Id && a.Activity.CategoryId == tab);

                if (list != null && list.Count() > 0)
                {
                    ViewBag.NameUser = sId.User.FirstName + " " + sId.User.LastName;
                    return View(list.ToList());
                }
                return View();
            }
            ViewBag.ErrorMsg = "Error sharing tabs";
            return View("Error");
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