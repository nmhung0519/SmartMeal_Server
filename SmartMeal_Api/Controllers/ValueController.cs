using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [Route("Get")]
        [HttpPost]
        public ResponseModel Get([FromBody] string name)
        {
            return new ResponseModel(true, "");
        }
    }
}
