using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SmartMeal_Api.Model;
using SmartMeal_Server;
using SmartMeal_Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IHubContext<MainHub> _hub;

        public TableController(IHubContext<MainHub> hub)
        {
            _hub = hub;
        }

        [Route("GetAllActive")]
        [HttpGet]
        [Authen]
        public ResponseModel GetAllActive()
        {
            var clsTable = new ClsTable();
            List<TableModel> tables;
            string msg = clsTable.GetAllActive(out tables);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            return new ResponseModel(true, tables);
        }

        [Route("GetById")]
        [HttpPost]
        [Authen]
        public ResponseModel GetById([FromBody]TableModel model)
        {
            var clsTable = new ClsTable();
            TableModel table;
            string msg = clsTable.GetById(model.Id, out table);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            return new ResponseModel(true, table);
        }

        [Route("Insert")]
        [HttpPost]
        [Authen]
        public async Task<ResponseModel> Insert([FromBody] string tableName)
        {
            var clsTable = new ClsTable();
            int tableId;
            string msg = clsTable.Insert(tableName, out tableId);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            TableModel table;
            msg = clsTable.GetById(tableId, out table);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            var hubConnectionBuilder = new HubConnectionBuilder();
            hubConnectionBuilder.WithUrl("http://api.smartmeal.com/tablehub");
            var hubConnection = hubConnectionBuilder.Build();
            try
            {
                var task = Task.Run(() => hubConnection.StartAsync());
                task.Wait();
                task = Task.Run(() => hubConnection.InvokeAsync("Insert", table));
                task.Wait();
            }
            catch { }
            return new ResponseModel(true, table);
        }

        [Route("Order")]
        [HttpPost]
        [Authen]
        public async Task<ResponseModel> Order(OrderModel model)
        {
            var clsOrder = new ClsOrder();
            string msg = clsOrder.InsertOrUpdate(model);
            if (string.IsNullOrEmpty(msg))
            {
                object objTableId = model.TableId;
                object objTypeId = 1;
                object[] prams = { objTableId, objTypeId };
                await _hub.Clients.All.SendAsync("Table", model.TableId.ToString(), "1");
                return new ResponseModel(true, "");
            }
            return new ResponseModel(false, msg);
        }
    }
}
