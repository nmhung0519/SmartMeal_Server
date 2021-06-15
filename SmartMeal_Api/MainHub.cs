using Microsoft.AspNetCore.SignalR;
using SmartMeal_Api.Controllers;
using SmartMeal_Api.Model;
using SmartMeal_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api
{
    public class MainHub: Hub
    {
        
        public async Task Register(string username, string token)
        {
            try
            {
                if (ClsToken.Verify(token) && ClsToken.TryGetUser(token, out username) && username.Equals(username))
                {
                    UserManager.Add(username, Context.ConnectionId);
                    await Clients.Caller.SendAsync("VerifyConnection", true);
                    return;
                }
            }
            catch { }
            await Clients.Caller.SendAsync("VerifyConnection", false);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //UserManager.DeleteByConnectionId(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetToken()
        {
            if (!UserManager.CheckExists(Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("Unauthorized");
                await Clients.Caller.SendAsync("Table", 1, 1);
                return;
            }

            var ctlToken = new TokenController();
            await Clients.All.SendAsync("Token", ClsToken.Get(UserManager.GetUserName(Context.ConnectionId)));
        }

        public async Task Message(string receivers, string message)
        {
            if (!UserManager.CheckExists(Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("Unauthorized");
                return;
            }

            string sender = UserManager.GetUserName(Context.ConnectionId);

            if (receivers == "")
            {
                await Clients.All.SendAsync("Message", sender, message);
                return;
            }

            var sendTo = receivers.Split(';');
            foreach (var receiver in sendTo)
            {
                var userSendTo = UserManager.GetConnectionIds(receiver);
                if (userSendTo == null) continue;
                await Clients.Clients(userSendTo).SendAsync("Message", sender, message);
            }
        }

        public void Logout()
        {
            UserManager.DeleteByConnectionId(Context.ConnectionId);
        }

        public async Task LogoutAll()
        {
            if (!UserManager.CheckExists(Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("Unauthorized");
                return;
            }

            string sender = UserManager.GetUserName(Context.ConnectionId);

            await Clients.Clients(UserManager.GetConnectionIds(sender)).SendAsync("Logout");
            UserManager.DeleteByUserName(sender);
        }
    }
}
