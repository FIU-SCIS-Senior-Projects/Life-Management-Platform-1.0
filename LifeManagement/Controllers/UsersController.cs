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
    [Authorize]
    public class UsersController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        private Common common = new Common();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.Where(a=>a.username.ToLower() == username.ToLower()).FirstOrDefault();
          
            if (user!=null && user.password == password)
            {
                FormsAuthentication.SetAuthCookie(user.username, false);
                return RedirectToAction("DashBoard");
            }

            ViewBag.Error = "Invalid Credentials";
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

    

        //Beatriz' code starts here++++++++++
        // GET: 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPass(string email)
        {
            Console.Out.WriteLine("This is email value: " + email);
            var user = db.Users.Where(e=>e.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if(user != null)
            {
                string subject = "Password reset requested";
                string message = "Dear " + user.FirstName + ": <br/>" + "<p> You are receiving this email because you forgot your password for the Life Management system," 
                    + " to reset your password please follow <a href= \"" + @Url.Action("PasswordRecovery", "Users", null, Request.Url.Scheme) + "/" + user.Id + "\">this link </a> and fill out the corresponding fields. </p> <br/>"
                    + "<p> Sincerely, <br/> The Life Management Team. </p>";

                Common.sendEmail(user.Email, subject, message);
            }
             else { 
                return new HttpStatusCodeResult(400, "");
            }
            

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult PasswordRecovery(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePass(string newpass, string newpass_conf, int id)
        {
    
     
            if (newpass != newpass_conf)
            {
                return new HttpStatusCodeResult(400, "");
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user != null)
            {
                user.password = newpass;
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            
            return RedirectToAction("Login"); ;
        }



        // GET: Users/Create
        [AllowAnonymous]
        public ActionResult CreateAccount()
        {
           
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateAccount( UserViewModel user)
        {
            if (ModelState.IsValid)
            {
               var role = db.Roles.Where(a => a.Name == Constants.ROLES.GUEST).FirstOrDefault();
                if (role != null)
                {
                    string result = CreateUser(user, role);

                    if (result == Constants.MSGS.SUCCESS)
                    {
                        TempData["UserName"] = user.FirstName;
                        TempData["LastName"] = user.LastName;
                        TempData.Keep("UserName");
                        TempData.Keep("LastName");
                        FormsAuthentication.SetAuthCookie(user.username, false);
                        return RedirectToAction("Questionaire");

                    }
                        
                 else
                 {
                     ViewBag.ErrorMsg = result;
                        return View(user);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = Constants.MSGS.ROLENOTEXIST;
                    return View(user);
                }
             
            }

           
            return View(user);
        }

        public string CreateUser(UserViewModel fromuser, Role role)
        {
            DateTime low = new DateTime(1900,1,1);
            DateTime high = DateTime.Now.Subtract(new TimeSpan(6570,0,0,0));
            if (db.Users.Where(a => a.username.ToLower() == fromuser.username.ToLower()).Count() > 0)
            {
                return Constants.MSGS.DUPLICATEUSERNAME;
            }
            if (db.Users.Where(a => a.Email.ToLower() == fromuser.Email.ToLower()).Count() > 0)
            {
                return Constants.MSGS.DUPLICATEEMAIL;
            }
            if (fromuser.password != fromuser.passwordconfirmation)
            {
                return Constants.MSGS.PASSWORDMISMATCH;
            }
            if (fromuser.DOB >high || fromuser.DOB<low)
            {
                return Constants.MSGS.INVALIDDOB;
            }

            var newuser = new User();
            newuser.RoleId = role.Id;
            newuser.FirstName = fromuser.FirstName;
            newuser.LastName = fromuser.LastName;
            newuser.DOB = fromuser.DOB;
            newuser.Email = fromuser.Email;
            newuser.username = fromuser.username;
            newuser.password = fromuser.password;
            newuser.Vision = "";
            newuser.LifeSuccess = "";
            newuser.Statement1 = "";
            newuser.Statement2 = "";
            newuser.Statement3 = "";
            newuser.DateCreated = DateTime.Now;
            db.Users.Add(newuser);
            db.SaveChanges();
            return Constants.MSGS.SUCCESS;
            ;



        }
        /**************dashboards*******************************/
        public ActionResult Dashboard()
        {
            return View(common.isAdmin());
        }

        public PartialViewResult UserDashBoard()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "There is no user logged in";
                return PartialView("ErrorPartial");
            }
            return PartialView(user);
        }
        public PartialViewResult AdminDashBoard()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "There is no user logged in";
                return PartialView("ErrorPartial");
            }
            return PartialView(user);
        }
        //Im working on this+++++++++++++++++++++++++++++++++++++++++++++++++++
        [Authorize]
        public ActionResult Questionaire()
        {
            return View();
        }


        [HttpPost]
        public ActionResult collect_questionnarie(QuestionarieViewModel data)
        {

            string name = User.Identity.Name;
            var user = db.Users.Where(atr => atr.username.ToLower() == name.ToLower()).FirstOrDefault();

            if (user != null)
            {
                int id = user.Id;

                var sprint0 = new Sprint();
                var g1 = new Goal();
                var g2 = new Goal();
                var g3 = new Goal();


                sprint0.DateFrom = DateTime.Now;
                sprint0.SprintGoal = data.vision;
                sprint0.UserId = id;
                db.Sprints.Add(sprint0);
                db.SaveChanges();

                g1.SprintId = sprint0.Id;
                g1.Description = data.goal_1;
                g1.CategoryId = 1;
                db.Goals.Add(g1);
                db.SaveChanges();

                if (data.goal_2 != null)
                {
                    g2.SprintId = sprint0.Id;
                    g2.Description = data.goal_2;
                    g2.CategoryId = 2;
                    db.Goals.Add(g2);
                    db.SaveChanges();
                }

                if (data.goal_3 != null)
                {
                    g3.SprintId = sprint0.Id;
                    g3.Description = data.goal_3;
                    g3.CategoryId = 3;
                    db.Goals.Add(g3);
                    db.SaveChanges();
                }

                user.Vision = data.vision;
                user.Statement1 = data.activity_1;

                if (data.activity_2 != null)
                    user.Statement2 = data.activity_2;

                if (data.activity_3 != null)
                    user.Statement3 = data.activity_3;

                user.LifeSuccess = data.determine_success;
                db.SaveChanges();

                ViewBag.questData = data;
                return RedirectToAction("UserSetup", "SprintActivities", new { sprint = sprint0 });

            }

            ViewBag.ErrorMsg = "An unexpected error occurred, please try again later";
            return View("Error");
            
        }

        [Authorize]
        public ActionResult SetupSprint()
        {

            return View();
           
        }

        /**************system generated*********************************************/
        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,DOB,RoleId,username,password,DateCreated")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult ProfilePic()
        {
            var user = db.Users.Where(c => c.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

            return PartialView(user);

        }


        public PartialViewResult UserEditProfile()
        {
            var user = db.Users.Where(c => c.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            var userModel = new UserProfileViewModel();
            userModel.Id = user.Id;
            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.Vision = user.Vision;
            userModel.LifeSuccess = user.LifeSuccess;
            userModel.password = user.password;
            userModel.Statement1 = user.Statement1;
            userModel.Statement2 = user.Statement2;
            userModel.Statement3 = user.Statement3;

            return PartialView(userModel);
        }

        [HttpPost]
        public ActionResult UserEditProfile(UserProfileViewModel usermodel)
        {
           
            if (ModelState.IsValid)
            {
                var file = usermodel.Files[0];
                var user = db.Users.Where(c => c.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    if (user.FirstName != usermodel.FirstName) user.FirstName = usermodel.FirstName;
                    if (user.LastName != usermodel.LastName) user.LastName = usermodel.LastName;
                    if (user.Vision != usermodel.Vision) user.Vision = usermodel.Vision;
                    if (user.LifeSuccess != usermodel.LifeSuccess) user.LifeSuccess = usermodel.LifeSuccess;
                    if (user.Statement1 != usermodel.Statement1) user.Statement1 = usermodel.Statement1;
                    if (user.Statement2 != usermodel.Statement2) user.Statement2 = usermodel.Statement2;
                    if (user.Statement3 != usermodel.Statement3) user.Statement3 = usermodel.Statement3;



                    if (file != null)
                    {
                        
                        if (common.saveImageBytesUser(user, file))
                        {
                            user.Avatar = common.ResizeImageFile(user.Avatar, 300);
                        }
                    }

                    db.SaveChanges();
                }
               return RedirectToAction("Dashboard");
            }
            return View(usermodel);
        }

        [HttpPost]
        public ActionResult ShareTabs(int sprintActId, int tab)
        {
            if( sprintActId >= 1 && tab >= 1) { 
            TempData["sprintActId"] = sprintActId;
            TempData["tabId"] = tab;
            TempData.Keep("activityList");
            TempData.Keep("tabId");
                return View();
            }
            ViewBag.ErrorMsg = "Error sharing tabs";
            return View("Error");
        }

        public ActionResult ShareTabsLink(int sprintActId, int tab)
        {
            var sprintAct = db.SprintActivities.Find(sprintActId);


            if (sprintAct != null)
            {
                int sId = db.Sprints.Find(sprintAct.SprintId).Id;
                var list = db.SprintActivities.Where(a => a.Sprint.Id == sId && a.Activity.CategoryId == tab);

                if (list != null && list.Count() > 0)
                {
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
