
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class UserProfileViewModel
    {

        public UserProfileViewModel()
        {
            Files = new List<HttpPostedFileBase>();
        }

        public List<HttpPostedFileBase> Files { get; set; }

        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DisplayName("Your vision")]
        public string Vision { get; set; }

        [Required]
        [DisplayName("How will you determine if your life is a success?")]
        public string LifeSuccess { get; set; }

        [Required]
        [DisplayName("Statement 1 on how your activities connect to our vision")]
        public string Statement1 { get; set; }

        [Required]
        [DisplayName("Statement 2 on how your activities connect to our vision")]
        public string Statement2 { get; set; }

        [Required]
        [DisplayName("Statement 3 on how your activities connect to our vision")]
        public string Statement3 { get; set; }
  
    }
}