using System;
using System.Data;
using System.Collections.Generic;

namespace SmartMeal_Api 
{
    public class DataPaymentModel {
        public string TableName { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get;set; }
        public string CustomerPhone { get; set; }
        public List<PaymentItem> Data { get; set; }
    }

    public class PaymentItem {
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public int ProductPrice { get; set; }

        public PaymentItem() {}
        public PaymentItem(DataRow dr) {
            if (dr.Table.Columns.Contains("productName")) ProductName = Convert.ToString(dr["productName"]);
            if (dr.Table.Columns.Contains("productPrice")) ProductPrice = Convert.ToInt32(dr["productPrice"]);
            if (dr.Table.Columns.Contains("productCount")) ProductCount = Convert.ToInt32(dr["productCount"]);
        }
    }
}

