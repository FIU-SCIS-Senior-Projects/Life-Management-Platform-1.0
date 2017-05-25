﻿using System;
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
                return RedirectToAction("Dashboard","Users");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dashboard()
        {
            
            return View();
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
                  
                 if (result==Constants.MSGS.SUCCESS)
                        return RedirectToAction("Questionaire");
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
            newuser.Vision = "";
            newuser.DateCreated = DateTime.Now;
            db.Users.Add(newuser);
            db.SaveChanges();
            return Constants.MSGS.SUCCESS;
            ;



        }

        public ActionResult Questionaire()
        {
            return View();
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
