using LifeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeManagement.Controllers
{
    public class AttachmentsController : Controller
    {
        private SeniorDBEntities db = new SeniorDBEntities();
        // GET: Attachments
        public FileContentResult GetFile(int id)
        {
            var file = db.ForumFiles.Find(id);
            if (file != null)
            {
                 return File(file.FileBytes,file.MimeType);
            }
            return null;

        }

        public PartialViewResult FileViewer(int id)
        {
          return PartialView(id);
        }
        [HttpPost]
        public bool SaveFile(int forumid)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                return SaveFile(forumid, file);
            }
            return false;
        }
        private bool SaveFile(int forumid, HttpPostedFileBase file)
        {
            var forum = db.Forums.Find(forumid);
            if (forum == null)
                return false;
        
            if (file != null)
            {
                try
                {
                    var newForumFile = new ForumFile();
                    newForumFile.MimeType = file.ContentType;
                    newForumFile.FileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(newForumFile.FileBytes, 0, file.ContentLength);
                    forum.ForumFiles.Add(newForumFile);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
             

            }
            return false;
        }

    }
}