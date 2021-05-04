using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server.Model
{
    public class SendModel
    {
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string ActionType { get; set; }
        public object Data { get; set; }
    }
}
