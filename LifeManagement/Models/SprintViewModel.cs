using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class SprintViewModel
    {

  
        public string SprintGoal { get; set; }

        public string JoyGoal { get; set; }

        public string PassionGoal { get; set; }

        public string GiveBGoal { get; set; }

        public Activity ActJoy1 { get; set; }

        public Activity ActJoy2 { get; set; }

        public Activity ActJoy3 { get; set; }

        public Activity ActPassion1 { get; set; }

        public Activity ActPassion2 { get; set; }

        public Activity ActGiveB1 { get; set; }

        public Activity ActGiveB2 { get; set; }


    }
}