
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        [Authen]
        [Route("Insert")]
        [HttpPost]
        public ResponseModel Insert([FromBody] NotifyModel model)
        {
            try
            {
                var hubConnectionBuilder = new HubConnectionBuilder();
                var hubConnection = hubConnectionBuilder.WithUrl("http://hub.smartmeal.com/mainhub").Build();
                hubConnection.SendAsync("Notify", model.ObjectId, model.ObjectType, model.ActionType, model.Content, model.CreatedTime);
                return new ResponseModel(true, null);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }

        [Route("Read")]
        [HttpPost]
        public ResponseModel Read([FromBody] int id)
        {
            try
            {
                //Update status
                return new ResponseModel(true, null);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }
    }
}
