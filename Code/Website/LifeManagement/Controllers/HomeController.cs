using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class HomeController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        private void EncriptAll()
        {
            foreach (var u in db.Users)
            {
                u.password = Security.HashSHA1(u.password);

            }
            foreach (var u in db.Coaches)
            {
                u.Password = Security.HashSHA1(u.Password);

            }
            db.SaveChanges();
        }
        public ActionResult Index()
        {
           
           return View();
        }
        [Authorize(Roles="Owners")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Quiz()
        {
            return View();
        }
        public ActionResult QuizResult(int quiztotal)
        {

            return View(quiztotal);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Videos()
        {
            

            return View();
        }
    }
}