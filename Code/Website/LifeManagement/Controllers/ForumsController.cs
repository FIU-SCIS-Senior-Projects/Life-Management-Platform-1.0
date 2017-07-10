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

        public PartialViewResult Chat()
        {
            
            return PartialView();
        }

        public PartialViewResult ConversationUsers()
        {
            if (common.isCoach())
            {
                return CoachesSection();
            }
            return UsersSection();
        }
        public PartialViewResult CoachesSection()
        {
            IEnumerable<Forum> forums;
         
          
                var coach = db.Coaches.Where(a => a.Username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                if (coach == null)
                {
                    ViewBag.ErrorMsg = "Invalid user";
                    return PartialView("ErrorPartial");
                }
                 forums = db.Forums.Where(a => a.CoachId == coach.Id);
            
         
            return PartialView("CoachesSection",forums);
        }
        public PartialViewResult UsersSection()
        {
            IEnumerable<Forum> forums;
          
                var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.ErrorMsg = "Invalid user";
                    return PartialView("ErrorPartial");
                }
                forums = db.Forums.Where(a => a.UserId == user.Id);
           
            return PartialView("UsersSection",forums);
        }

        public PartialViewResult ConvosSection(int forumid)
        {
            var isCoach = common.isCoach();
            var senderid = -1;
            if (isCoach)
            {
                var coach = db.Coaches.Where(a => a.Username == User.Identity.Name).FirstOrDefault();
                if (coach != null)
                {
                    isCoach = true;
                    senderid = coach.Id;
                }
            }
            else
            {
                var user = db.Users.Where(a => a.username == User.Identity.Name).FirstOrDefault();
                if (user != null)
                {
                    senderid = user.Id;
                }

            }

            ViewBag.SenderId = senderid;
            ViewBag.isCoach = isCoach;

            var convos = db.Conversations.Where(a => a.ForumId == forumid);
            return PartialView(convos);
        }
        public PartialViewResult FilesSection(int forumid)
        {
            var files = db.ForumFiles.Where(a => a.ForumId == forumid);
            return PartialView(files);
        }


        [HttpPost]
        public bool Create(int coachid)
        {
            if (!common.isCoach())
            {
                var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    db.Forums.Add(new Forum()
                    {
                        CoachId = coachid,
                        UserId = user.Id,
                        ForumDate = DateTime.Now
                    });
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}