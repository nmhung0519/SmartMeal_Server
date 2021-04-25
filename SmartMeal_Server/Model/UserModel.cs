using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
    }
}
