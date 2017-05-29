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
        // GET: Questionnarie
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool collect_questionnarie(QuestionarieViewModel data)
        {
            
            
            return true;
        }
    }
}