using Microsoft.AspNetCore.SignalR;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartMeal_Server.Model;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SmartMeal_Server.Hubs
{
    public class TableHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            var tmp = base.Context;
            return base.OnConnectedAsync();
        }
        public async Task Insert(TableModel model)
        {
            string tmp = JsonSerializer.Serialize(model);
            await Clients.All.SendAsync("Insert", model.Id, model.TableName, model.Status, model.IsActive);
        }

        public async Task Update(TableModel model)
        {
            await Clients.All.SendAsync("Update", model.Id, model.TableName, model.Status, model.IsActive);
        }

        public async Task Order(OrderModel model)
        {
            await Clients.All.SendAsync("Order", model.TableId, model.StatusId, (model.StatusId == 2) ? model.StartTime.ToString("yyyy/MM/dd hh:mm:ss") : "", (model.StatusId == 2) ? model.EndTime.ToString("yyyy/MM/dd hh:mm:ss") : "");
        }
    }
}
