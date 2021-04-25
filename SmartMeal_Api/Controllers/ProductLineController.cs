using Microsoft.AspNetCore.Http;
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
    public class ProductLineController : ControllerBase
    {
        [Route("Search")]
        [HttpPost]
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
    }
}
