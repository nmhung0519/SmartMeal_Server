using Microsoft.AspNetCore.SignalR;
using SmartMeal_Api.Controllers;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api
{
    public class MainHub: Hub
    {
        private static List<UserRegister> users = new List<UserRegister>();
        public async Task Register(string username, string token)
        {
            try
            {
                if (ClsToken.Verify(token) && ClsToken.TryGetUser(token, out username) && username.Equals(username))
                {
                    users.Add(new UserRegister(Context.ConnectionId, username));
                    await Clients.Caller.SendAsync("VerifyConnection", true);
                    return;
                }
            }
            catch { }
            await Clients.Caller.SendAsync("VerifyConnection", false);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            users.Remove(users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Message(string receivers, string message)
        {
            var user = users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user == null)
            {
                await Clients.Caller.SendAsync("Unauthorized");
                return;
            }

            if (receivers == "")
            {
                await Clients.All.SendAsync("Message", user.Username, message);
                return;
            }

            var sendTo = receivers.Split(';');
            foreach (var receiver in sendTo)
            {
                var userSendTo = users.Where(x => x.Username == receiver).FirstOrDefault();
                if (userSendTo == null) continue;
                string connectionSendToId = userSendTo.ConnectionId;
                await Clients.Client(connectionSendToId).SendAsync("Message", user.Username, message);
            }
        }
    }

    class UserRegister
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public UserRegister() { }
        public UserRegister(string id, string username)
        {
            ConnectionId = id;
            Username = username;
        }
    }
}
