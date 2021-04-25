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
    public class ProductController : ControllerBase
    {
        [Route("Insert")]
        [HttpPost]
        [Authen]
        public ResponseModel Insert([FromBody] ProductModel model)
        {
            try
            {
                var cls = new ClsProduct();
                ProductModel product;
                string msg = cls.Insert(model, out product);
                if (string.IsNullOrEmpty(msg)) return new ResponseModel(true, product);
                return new ResponseModel(false, msg);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }

        [Route("Search")]
        [HttpPost]
        [Authen]
        public ResponseModel Search([FromBody] SearchProductModel model)
        {
            try
            {
                var cls = new ClsProduct();
                List<ProductModel> products;
                string msg = cls.Search(model, out products);
                if (string.IsNullOrEmpty(msg)) return new ResponseModel(true, products);
                return new ResponseModel(false, msg);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }

        [Route("ChangeStatus")]
        [HttpPost]
        [Authen]
        public ResponseModel ChangeStatus([FromBody] SearchProductModel model)
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
                var cls = new ClsProduct();
                string msg = cls.ChangeStatus(model.Id, model.IsActive, username);
                if (string.IsNullOrEmpty(msg)) return new ResponseModel(true, null);
                return new ResponseModel(false, msg);
            }
            catch (Exception ex) { return new ResponseModel(false, ex.Message); }
        }
    }
}
