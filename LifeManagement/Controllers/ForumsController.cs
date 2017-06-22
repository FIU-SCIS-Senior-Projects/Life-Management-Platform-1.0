using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class ForumsController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        private Common common = new Common();

        // GET: Forums
   

        public ActionResult SeeChat()
        {
            return View();
        }

        public PartialViewResult ChatForums()
        {
            var user = db.Users.Where(a=>a.username.ToLower()== User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            var forums = db.Forums.Where(a => a.UserId == user.Id);
            return PartialView(forums);
        }
        public PartialViewResult CoachesSection(int forumid)
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Invalid user";
                return PartialView("ErrorPartial");
            }
            var forums = db.Forums.Where(a => a.UserId == user.Id);
            return PartialView(forums);
        }
        public PartialViewResult ForumConversations(int forumid)
        {
           var convos = db.Conversations.Where(a => a.ForumId == forumid);
            return PartialView(convos);
        }
    }
}