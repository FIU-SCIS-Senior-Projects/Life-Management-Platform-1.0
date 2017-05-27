using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class QuestionarieViewModel
    {

  
        public string vision { get; set; }

        public string goal_1 { get; set; }

        public string goal_2 { get; set; }

        public string goal_3 { get; set; }

        public string determine_success { get; set; }

        public string activity_1 { get; set; }

        public string activity_2 { get; set; }

        public string activity_3 { get; set; }


    }
}