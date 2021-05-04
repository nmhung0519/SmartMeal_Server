using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductLineController : ControllerBase
    {
        [Route("Search")]
        [HttpPost]
        [Authen]
        public ResponseModel Search([FromBody]ProductLineModel model)
        {
            try
            {
                var cls = new ClsProductLine();
                List<ProductLineModel> pls;
                string msg = cls.Search(model, out pls);
                if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
                return new ResponseModel(true, pls);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); } 
        }

        [Route("Insert")]
        [HttpPost]
        [Authen]
        public ResponseModel Insert([FromBody] ProductLineModel model)
        {
            try
            {
                StringValues token;
                HttpContext.Request.Headers.TryGetValue("Authorization", out token);
                string username;
                if (!ClsToken.TryGetUser(token, out username))
                {
                    return new ResponseModel(false, "Xảy ra lỗi trong quá trình xác thực không hợp lệ");
                }
                var cls = new ClsProductLine();
                ProductLineModel pls;
                string msg = cls.Insert(model, username, out pls);
                if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
                return new ResponseModel(true, pls);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }
    }
}
