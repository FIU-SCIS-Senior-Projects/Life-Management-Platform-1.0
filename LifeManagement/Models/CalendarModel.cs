using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class CalendarModel
    {
        public string title { get; set;}
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string url { get; set; }
        public bool allDay { get; set; }
        public string backgroundColor { get; set; }
        public string textColor { get; set; }
        public string description { get; set; }
        public int apptId { get; set; }


    }
}