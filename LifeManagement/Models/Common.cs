using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mail;
using System.Net.Mail;

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
        private const string SMTPSERVER = "smtp.gmail.com";
        private const int SMTPPORT = 587;
        private const string EmailAddress = "manage.life.team@gmail.com";
        private const string Pass = "052017befer";

        public bool IsAuthenticated
        {
         
            get
            {
                return HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity != null &&
                       HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        /***********************images processing******************************************/
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

        public  string SignatureImageStr64(Byte[] bytes, string mimetype)
        {
            string imageSrc = "/Imgs/noimage.jpg";

            if (bytes != null)
            {
                string imageBase64 = "";
                imageBase64 = Convert.ToBase64String(bytes);
                imageSrc = string.Format("data:{0};base64,{1}", mimetype, imageBase64);

            }
            return imageSrc;
        }
        /*************************************emails processing*******************************/
        public static void sendEmail(string strTo, string strSubject, string message)
        {
            try
            {
                sendEmail(EmailAddress, strTo, "", "", strSubject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sendEmail!!! Exception",
                            ex.ToString());
            }
        }
        public static bool sendEmail(string strFrom, string strTo, string strCC, string strBCC, string strSubject, string strBody)
        {


            //Create your message body. 
            MailMessage mailMsg = new MailMessage(strFrom, strTo);
            SmtpClient client = new SmtpClient();
            client.Port = SMTPPORT;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(strFrom, Pass);
            client.Host = SMTPSERVER;
            mailMsg.Subject = strSubject;
            mailMsg.Body = strBody;
            mailMsg.IsBodyHtml = true;

            /*  MailAddress copy = new MailAddress(strCC);      //To implement cc and bcc, they can't be empty, we don't need this rigth now
              mailMsg.CC.Add(copy);
              MailAddress Bcopy = new MailAddress(strBCC);
              mailMsg.Bcc.Add(Bcopy);
              */


            client.Send(mailMsg);

           


            return true;

        }


    }


}