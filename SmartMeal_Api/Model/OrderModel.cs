using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public int CreatorId { get; set; }

        public OrderModel() { }
        public OrderModel(DataRow dr)
        {
            Id = dr.Field<int>("Id");
            TableId = dr.Field<int>("TableId");
            StatusId = dr.Field<int>("StatusId");
        } 
    }
}
