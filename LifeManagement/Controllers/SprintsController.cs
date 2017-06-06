using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class SprintsController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        // GET: Sprints
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sprints/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sprints/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Sprints/Create
        [HttpPost]
        public ActionResult Create(Sprint newSprint)
        {
            try
            {
                // TODO: Add insert logic here
                var user = (db.Users.Where(a => a.username.ToLower() == (User.Identity.Name).ToLower()).FirstOrDefault());
                if(user != null)
                {
                    newSprint.UserId = user.Id;
                    db.Sprints.Add(newSprint);
                    db.SaveChanges();
                    ViewBag.newSprint = newSprint;
                    return View("SetupSprint");
                }
                else
                {
                    ViewBag.ErrorMsg = "There is no user logged in";
                    return PartialView("ErrorPartial");
                }

            }
            catch
            {
                ViewBag.ErrorMsg = "There was an unexpected error";
                return PartialView("ErrorPartial");
            }
        }


        public ActionResult SetupSprint()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetupSprint(SprintViewModel model)
        {
            if(model != null)
            return View();

            ViewBag.ErrorMsg = "An error ocurred";
            return View("Error");
        }

        // GET: Sprints/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sprints/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sprints/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sprints/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
