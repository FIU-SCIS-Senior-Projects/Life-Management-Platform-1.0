using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LifeManagement.Models;

namespace LifeManagement
{
    class ClientForum
    {
        public int ForumId { get; set; }
        public List<string> ClientIds { get; set; }

    }
    public class ChatHub : Hub
    {

        static List<ClientForum> clients = new List<ClientForum>();
        private SeniorDBEntities db = new SeniorDBEntities();

        private void  RegisterConnection(int forumid)
        {
            var connid = Context.ConnectionId;
            var clientforum = clients.Where(a => a.ForumId == forumid).FirstOrDefault();
            if (clientforum != null)
            {
                var clientid = clientforum.ClientIds.Where(a => a.Equals(connid)).FirstOrDefault();
                if (clientid == null)
                {
                    clientforum.ClientIds.Add(connid);
                }
            }
            else
            {
                List<string> newids = new List<string>();
                newids.Add(connid);
                clients.Add(new ClientForum()
                {
                    ForumId = forumid,
                    ClientIds = newids

                });
            }
        }
        public void Send(int forumid, string message,int senderid, string isCoach)
        {

            RegisterConnection(forumid);

          
        
            db.Conversations.Add(new Conversation()
            {
                ForumId = forumid,
                Message = message,
                SenderID = senderid,
                isCoach = isCoach=="True",
                MsgDate = DateTime.Now

            });
            db.SaveChanges();
            // Call the broadcastMessage method to update clients.
            Clients.Clients(clients.Where(a=>a.ForumId==forumid).FirstOrDefault().ClientIds).broadcastMessage(Context.User.Identity.Name, message);
           
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            int forumid = -1;
            Int32.TryParse(Context.QueryString["forumid"], out forumid);
          RegisterConnection(forumid);
            return base.OnConnected();
           
        }
       
    }
}