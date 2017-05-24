using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mail;

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
        private const string SMTPPORT = "25";
        private const bool SMTPSSL = true;

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
        public static void sendEmail(string strTo, string strSubject, string message)
        {
            try
            {
                sendEmail("Travels@miami-airport.com", strTo, "", "", strSubject, message);
            }
            catch (Exception e)
            {

            }
        }
        public static bool sendEmail(string strFrom, string strTo, string strCC, string strBCC, string strSubject, string strBody)
        {


            //Create your message body. 
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = strFrom;
            mailMsg.To = strTo;
            mailMsg.Cc = strCC;
            mailMsg.Bcc = strBCC;
            mailMsg.Subject = strSubject;
            mailMsg.Body = strBody;
            mailMsg.BodyFormat = MailFormat.Html;

            // SMTP SERVER
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
            {
                SmtpMail.SmtpServer = SMTPSERVER;
            }

            // Send the email 
            SmtpMail.Send(mailMsg);
            // Normal Completion 


            return true;

        }


    }


}