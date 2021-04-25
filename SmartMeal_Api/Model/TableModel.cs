using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class TableModel
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }

        public TableModel() { }
        public TableModel(DataRow dr)
        {
            Id = dr.Field<int>("Id");
            TableName = dr.Field<string>("TableName");
            Status = int.Parse(dr.Field<object>("Status").ToString());
            IsActive = int.Parse(dr.Field<object>("IsActive").ToString());
        }
    }
}
