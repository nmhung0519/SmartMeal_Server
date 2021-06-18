using System;
using System.Data;
namespace SmartMeal_Api.Model{
    public class OrderDetailModel{
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public int ProductPrice { get; set; }
        public DateTime CreatedTime { get; set; }

        public OrderDetailModel(DataRow dr)
        {
            if (dr.Table.Columns.Contains("Id")) Id = Convert.ToInt32(dr["Id"]);
            if (dr.Table.Columns.Contains("orderId")) OrderId = Convert.ToInt32(dr["orderId"]);
            if (dr.Table.Columns.Contains("productName")) ProductName = Convert.ToString(dr["productName"]);
            if (dr.Table.Columns.Contains("productCount")) ProductCount = Convert.ToInt32(dr["ProductCount"]);
            if (dr.Table.Columns.Contains("productPrice")) ProductPrice = Convert.ToInt32(dr["ProductPrice"]);
            if (dr.Table.Columns.Contains("createdTime")) CreatedTime = Convert.ToDateTime(dr["createdTime"]);
        }
    }
}