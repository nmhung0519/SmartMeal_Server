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
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public ResponseModel Login([FromBody] LoginModel model)
        {
            var clsUser = new ClsUser();
            if (clsUser.CheckLogin(model))
            {
                return new ResponseModel(true, clsUser.GetUserByUsername(model.Username));
            }
            return new ResponseModel(false, "Username or Password not correct");
        }

        [HttpPost]
        [Route("Logon")]
        [Authen]
        public ResponseModel Logon([FromBody] LogonModel model)
        {
            string msg = model.Validate();
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            var clsUser = new ClsUser();
            if (clsUser.Insert(model))
                return new ResponseModel(true, clsUser.GetUserByUsername(model.Username));
            return new ResponseModel(false, "Đăng ký không thành công");
        }

        [HttpPost]
        [Route("Status")]
        [Authen]
        public ResponseModel Status([FromBody] UpdateStatusUserModel model)
        {
            var clsUser = new ClsUser();
            if (clsUser.UpdateStatus(model.Id, model.Status))
                return new ResponseModel(true, ((model.Status == 1) ? clsUser.GetUserById(model.Id) : null));
            return new ResponseModel(false, null);
        }
    }

    public class UpdateStatusUserModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
    }
}
