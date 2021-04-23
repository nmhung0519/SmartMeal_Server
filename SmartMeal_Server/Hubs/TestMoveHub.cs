using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server.Hubs
{
    public class TestMoveHub:Hub
    {
        public async Task MoveViewFromServer(float newX, float newY)
        {
            Console.WriteLine("Receive position from Server app: " + newX + "/" + newY);
            await Clients.Others.SendAsync("ReceiveNewPosition", newX, newY);
        }
    }
}
