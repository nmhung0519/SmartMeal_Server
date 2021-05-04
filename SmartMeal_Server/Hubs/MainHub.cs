using Microsoft.AspNetCore.SignalR;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartMeal_Server.Hubs
{
    public class MainHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async Task Notify(int objId, string objType, string actType, string content, int createdTime)
        {
            await Clients.All.SendAsync("Notify", objId, objType, actType, content, createdTime);
        }

        public async Task ReadNotify(int id)
        {
            try
            {
                var client = new RestClient("http://api.smartmeal.com:8080/Notify/Read");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "2", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                await Clients.All.SendAsync("ReadNotify", id);
            }
            catch { }
        }
    }
}
