using Microsoft.AspNetCore.Mvc;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController: ControllerBase
    {
        [Route("Verify")]
        [HttpPost]
        public string Verify([FromBody] VerifyTokenModel model)
        {
            string username;
            if (ClsToken.Verify(model.token) && ClsToken.TryGetUser(model.token, out username) && username.Equals(model.username)) return "1";
            return "0";
        }
    }

    public class VerifyTokenModel
    {
        public string username { get; set; }
        public string token { get; set; }
        public VerifyTokenModel() { }
        public VerifyTokenModel(string username, string token)
        {
            this.username = username;
            this.token = token;
        }
    }
}
