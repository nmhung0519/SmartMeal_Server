using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductLineId { get; set; }
        public int ProductPrice { get; set; }
        public int IsActive { get; set; }
        public string Image { get; set; }

        public ProductModel() { }
        public ProductModel(DataRow dr)
        {
            if (dr.Table.Columns.Contains("Id")) Id = Convert.ToInt32(dr["Id"]);
            if (dr.Table.Columns.Contains("ProductName")) ProductName = Convert.ToString(dr["ProductName"]);
            if (dr.Table.Columns.Contains("ProductLineId")) ProductLineId = Convert.ToInt32(dr["ProductLineId"]);
            if (dr.Table.Columns.Contains("ProductPrice")) ProductPrice = Convert.ToInt32(dr["ProductPrice"]);
            if (dr.Table.Columns.Contains("IsActive")) IsActive = Convert.ToInt32(dr["IsActive"]);
            if (dr.Table.Columns.Contains("Image")) Image = Convert.ToString(dr["Image"]);
        }
    }
}
