using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class NotifyModel
    {
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string ActionType { get; set; }
        public string Content { get; set; }
        public int CreatedTime { get; set; }
    }
}
