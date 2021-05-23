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
            if (dr.Table.Columns.Contains("Id")) Id = Convert.ToInt32(dr["Id"]);
            if (dr.Table.Columns.Contains("TableId")) TableId = Convert.ToInt32(dr["TableId"]);
            if (dr.Table.Columns.Contains("StatusId")) StatusId = Convert.ToInt32(dr["StatusId"]);
            if (dr.Table.Columns.Contains("StartTime")) StartTime = Convert.ToDateTime(dr["StartTime"]);
            if (dr.Table.Columns.Contains("EndTime")) EndTime = Convert.ToDateTime(dr["EndTime"]);
            if (dr.Table.Columns.Contains("CustomerName")) CustomerName = Convert.ToString(dr["CustomerName"]);
            if (dr.Table.Columns.Contains("CustomerContact")) CustomerContact = Convert.ToString(dr["CustomerContact"]);
            if (dr.Table.Columns.Contains("CreatorId")) CreatorId = Convert.ToInt32(dr["CreatorId"]);

        } 
    }
}
