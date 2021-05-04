using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class SearchProductModel : ProductModel
    {
        public int StartPrice { get; set; }
        public int EndPrice { get; set; }
    }
}