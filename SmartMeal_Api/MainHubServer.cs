using Microsoft.AspNetCore.SignalR;
using SmartMeal_Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server
{
    public class MainHubServer
    {
        private readonly IHubContext<MainHub> _hub;
        public MainHubServer(IHubContext<MainHub> hub)
        {
            _hub = hub;
        }


    }
}
