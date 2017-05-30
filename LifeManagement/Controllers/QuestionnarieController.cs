using LifeManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace LifeManagement.Controllers
{
    public class QuestionnarieController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        
        // GET: Questionnarie
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool collect_questionnarie(QuestionarieViewModel data)
        {

            string name = User.Identity.Name;
            var user = db.Users.Where(atr => atr.username == name).FirstOrDefault();
            
            if(user != null)
            {
                int id = user.Id;
            }

            var g1 = new Goal();
            g1.Description = data.goal_1;
            g1.
            newuser.FirstName = fromuser.FirstName;
            newuser.LastName = fromuser.LastName;
            newuser.DOB = fromuser.DOB;
            newuser.Email = fromuser.Email;
            newuser.username = fromuser.username;
            newuser.password = fromuser.password;
            newuser.Vision = "";
            newuser.LifeSuccess = "";
            newuser.DateCreated = DateTime.Now;
            db.Users.Add(newuser);
            db.SaveChanges();
            return Constants.MSGS.SUCCESS;
            return true;
        }

        public ActionResult CollectPassion()
        {
            return View();
        }
    }
}