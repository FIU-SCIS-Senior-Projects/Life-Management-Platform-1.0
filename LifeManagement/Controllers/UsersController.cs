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
    public class UsersController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
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
                return RedirectToAction("UserSetup","SprintActivities");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }


        //Beatriz' code starts here++++++++++
        // GET: 
        [HttpPost]
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


        // GET: Users/Create
        public ActionResult CreateAccount()
        {
           
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        FormsAuthentication.SetAuthCookie(user.username, false);
                        return RedirectToAction("Questionaire", user);

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

        //Im working on this+++++++++++++++++++++++++++++++++++++++++++++++++++
        [Authorize]
        public ActionResult Questionaire(UserViewModel user)
        {
            return View(user);
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


                ViewBag.Sprint0 = sprint0;
                ViewBag.questData = data;
                return View("SetupSprint");

            }

            ViewBag.ErrorMsg = "An unexpected error occurred, please try again later";
            return View("Error");
            
        }

        [Authorize]
        public ActionResult SetupSprint()
        {

            return View();
           
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++

        // GET: Users/Edit/5
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
