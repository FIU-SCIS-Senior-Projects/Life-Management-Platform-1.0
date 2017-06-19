using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeManagement.Models;

namespace LifeManagement.Controllers
{
    public class CoachesController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
       /**************working with coaches fernando******************/
        public JsonResult GetCoaches()
        {
            var coaches = db.Coaches.ToList();
            return Json(coaches, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CoachesList()
        {
            return PartialView();
        }

        public ActionResult SeeCoaches()
        {
            return View();
        }
        /*************************************************************/
    }
}