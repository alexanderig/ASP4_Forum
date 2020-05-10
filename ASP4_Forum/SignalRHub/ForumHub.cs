using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.AspNet.SignalR;
//using Microsoft.AspNet.Identity.Owin;
using ASP4_Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using ASP4_Forum.Controllers;
//using System.Web.UI;
//using Microsoft.AspNet.Identity;

namespace ASP4_Forum.SignalRHub
{
    public class ConnectedUser
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }

    public class ForumHub : Hub
    {
        public void ChangeAvatar(string avapath, string userId)
        {
            
            if (!string.IsNullOrEmpty(avapath))
            {
                //var controller = DependencyResolver.Current.GetService<ManageController>();
                
                //controller.ControllerContext = new ControllerContext(_httpContextAccessor.Request.RequestContext, controller);
                
                
                var myusermanager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ApplicationUser user = myusermanager.FindById(userId);
                user.AvatarURL = avapath;
                myusermanager.Update(user);              
                
                Clients.Caller.onchangeAvatar(avapath);
            }
        }

        static List<ConnectedUser> Users = new List<ConnectedUser>();




        // Отправка сообщений
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new ConnectedUser { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                //Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            Users.Add( new ConnectedUser { ConnectionId = Context.ConnectionId});
            return base.OnConnected();
        }


        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }

    }
   
}