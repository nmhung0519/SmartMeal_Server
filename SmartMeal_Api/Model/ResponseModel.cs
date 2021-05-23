using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ResponseModel
    {
        public bool status { get; set; }
        public string content { get; set; }

        public ResponseModel() { }
        public ResponseModel(bool status, string content) 
        {
            this.status = status;
            this.content = content;
        }
        public ResponseModel(bool status, object obj)
        {
            this.status = status;
            if (obj != null) this.content = JsonConvert.SerializeObject(obj);
        }
    }
}
