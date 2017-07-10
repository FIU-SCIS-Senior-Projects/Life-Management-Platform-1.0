using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using LifeManagement.Models;


namespace LifeManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private  SeniorDBEntities db = new SeniorDBEntities();
        // GET: Appointments
        public ActionResult UserCalendar()
        {
            return View();
        }
        public PartialViewResult Calendar()
        {
            return PartialView("Calendar");
        }
        public PartialViewResult SeeCalendar()
        {
            return PartialView();
        }

        public PartialViewResult Create(string title, DateTime start, int duration, string description)
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user != null)
            {
                user.Appointments.Add(new Appointment()
                {
                    allDay = true,
                    backgroundColor = "azure",
                    Description = description,
                    start = start,
                    end = start.AddHours(duration),
                    textColor = "black",
                    title = title,
                    url = "",

                });
                db.SaveChanges();
                return Calendar();
            }
            ViewBag.ErrorMsg = "Invalid User";
            return PartialView("ErrorPartial");
        }
        public PartialViewResult Delete(int id)
        {
          
                var appt = db.Appointments.Find(id);
                if (appt != null)
                {
                    db.Appointments.Remove(appt);
                    db.SaveChanges();
                }
         
              return Calendar();
        }
        public PartialViewResult Details(int id )
        {
            var app = db.Appointments.Find(id);
            if (app != null)
                return PartialView(app);
            ViewBag.ErrorMsg = "Could not get appointment details";
            return PartialView("ErrorPartial");
        }
        public JsonResult GetAppointments()
        {
            var user = db.Users.Where(a => a.username.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
            if (user != null)
            {
                var list = new List<CalendarModel>();
                foreach (var a in user.Appointments)
                {
                    list.Add(new CalendarModel()
                    {
                        allDay = true,
                     start = a.start,
                        title = a.title,
                        backgroundColor = a.backgroundColor,
                        textColor = a.textColor,
                        end = a.end,
                        description = a.Description,
                        apptId = a.Id
                    });
                }
                return Json(list,JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}