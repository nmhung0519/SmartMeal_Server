using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ProductLineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int IsActive { get; set; }

        public ProductLineModel() { }
        public ProductLineModel(DataRow dr) {
            Id = int.Parse(dr.Field<object>("Id").ToString());
            Name = dr.Field<string>("Name").ToString();
            ParentId = int.Parse(dr.Field<object>("ParentId").ToString());
            Id = int.Parse(dr.Field<object>("IsActive").ToString());
        }
    }
}
