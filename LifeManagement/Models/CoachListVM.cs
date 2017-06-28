using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class CoachListVM
    {
       public int CoachId { get; set; }
        public string AvatarStr64{ get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

      
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Score")]
        public int ReviewScore { get; set; }
        public string Skills { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}