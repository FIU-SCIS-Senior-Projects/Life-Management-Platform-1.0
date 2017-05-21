using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifeManagement.Models
{
    public class Constants
    {
          public struct ROLES
        {
            public static string GUEST = "Guest";
            public static string USER = "User";
            public static string ADMIN = "Admin";


        }
        public struct MSGS
        {
            public static string SUCCESS = "Success";
            public static string DUPLICATEUSERNAME = "Username already taken";
            public static string PASSWORDMISMATCH = "Passwords do not match";
            public static string INVALIDPASSWORD = "Password does not comply with requirements";
            public static string ROLENOTEXIST = "Role does not exist";


        }
    }
    public class Common
    {
      
        public bool IsAuthenticated
        {
         
            get
            {
                return HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity != null &&
                       HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        private bool saveImageBytes(ImageInterface s, HttpPostedFileBase image)
        {

            if (image != null)
            {
                s.ImageMimeType = image.ContentType;
                s.Bytes = new byte[image.ContentLength];
                image.InputStream.Read(s.Bytes, 0, image.ContentLength);
                return true;
            }
            return false;
        }
        private bool saveImageBytes(ImageInterface s, string image)
        {

            if (!String.IsNullOrEmpty(image))
            {
                byte[] data = Convert.FromBase64String(image);

                s.ImageMimeType = "PNG";
                s.Bytes = data;

                return true;
            }
            return false;
        }

    }
}