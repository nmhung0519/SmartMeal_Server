using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string Fullname { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public string Token { get; set; }

        public UserModel() { }
        public UserModel(DataRow dr)
        {
            Id = dr.Field<int>("Id");
            Username = dr.Field<string>("Username");
            RoleId = dr.Field<int>("RoleId");
            Fullname = dr.Field<string>("Fullname");
            Age = DateTime.Now.Year - dr.Field<int>("BirthYear");
            Sex = dr.Field<int>("Sex");
        }
        public void CreateToken()
        {
            Token = ClsToken.Get(Username);
        }
    }   
}
