using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public UserModel Post([FromBody] LoginModel model)
        {
            var clsUser = new ClsUser();
            if (clsUser.CheckLogin(model))
            {
                return clsUser.GetUser(model.Username);
            }
            return null;
        }
    }
}
