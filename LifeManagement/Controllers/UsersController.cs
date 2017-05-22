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
                return RedirectToAction("Index","Home");
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
        public ActionResult ForgotPassword()
        {
            return View();
        }

        public PartialViewResult ResetPass()
        {
            return PartialView();
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
        public PartialViewResult CreateAccount()
        {
           
            return PartialView();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public PartialViewResult SignUp()
        {
           
            return PartialView();
        }

        [HttpPost]
       public PartialViewResult SignUp( UserViewModel user)
        {
            if (ModelState.IsValid)
            {
               var role = db.Roles.Where(a => a.Name == Constants.ROLES.GUEST).FirstOrDefault();
                if (role != null)
                {
                    string result = CreateUser(user, role);
                  
                 if (result==Constants.MSGS.SUCCESS)
                        return PartialView("Success","User successfully created");
                 else
                 {
                     ViewBag.ErrorMsg = result;
                        return PartialView(user);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = Constants.MSGS.ROLENOTEXIST;
                    return PartialView(user);
                }
             
            }

           
            return PartialView(user);
        }

        public string CreateUser(UserViewModel fromuser, Role role)
        {
            if (db.Users.Where(a => a.username.ToLower() == fromuser.username).Count() > 0)
            {
                return Constants.MSGS.DUPLICATEUSERNAME;
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
            newuser.DateCreated = DateTime.Now;
            db.Users.Add(newuser);
            db.SaveChanges();
            return Constants.MSGS.SUCCESS;
            ;



        }

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
