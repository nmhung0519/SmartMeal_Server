using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartMeal_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderDetailController : ControllerBase
    {
        [Route("Insert")]
        [HttpPost]
        [Authen]
        public ResponseModel Insert(IEnumerable<OrderDetailModel> model) {
            try {
                var cls = new ClsOrderDetail();
                foreach (var item in model) {
                    string msg = cls.Insert(item);
                    if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
                }
                return new ResponseModel(true, "");
            }
            catch (Exception ex) {
                return new ResponseModel(false, ex.Message);
            }
        }
    }
}
