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
        public string Name { get; set; }
        public int ProductLineId { get; set; }
        public int Price { get; set; }
        public int IsActive { get; set; }

        public ProductModel() { }
        public ProductModel(DataRow dr)
        {
            Id = int.Parse(dr.Field<object>("Id").ToString());
            Name = dr.Field<string>("Name");
            ProductLineId = int.Parse(dr.Field<object>("ProductLineId").ToString());
            Price = int.Parse(dr.Field<object>("Price").ToString());
            IsActive = int.Parse(dr.Field<object>("IsActive").ToString());
        }
    }
}
