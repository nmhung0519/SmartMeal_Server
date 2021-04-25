using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ResponseModel
    {
        public bool status { get; set; }
        public object content { get; set; }

        public ResponseModel() { }
        public ResponseModel(bool status, object content)
        {
            this.status = status;
            this.content = content;
        }
    }
}
