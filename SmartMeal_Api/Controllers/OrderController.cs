using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartMeal_Api;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHubContext<MainHub> _hub;

        public OrderController(IHubContext<MainHub> hub)
        {
            _hub = hub;
        }

        [Route("GetPreWithTable")]
        [HttpPost]
        [Authen]
        public ResponseModel GetPreWithTable([FromBody] OrderModel model)
        {
            var clsOrder = new ClsOrder();
            OrderModel order;
            string msg = clsOrder.GetPreOrderForTable(model.TableId, out order);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            return new ResponseModel(true, order);
        }

        [Route("Confirm")]
        [HttpPost]
        [Authen]
        public async Task<ResponseModel> Confirm([FromBody] OrderModel model)
        {
            var clsOrder = new ClsOrder();
            string msg = clsOrder.Confirm(model.Id);
            if (string.IsNullOrEmpty(msg))
            {
                model.TableId = clsOrder.GetTableIdById(model.Id);
                await _hub.Clients.Clients(UserManager.GetAllConnectionId()).SendAsync("Table", model.TableId.ToString(), "1");
                return new ResponseModel(true, "");
            }

            return new ResponseModel(false, msg);
        }
    }
}
