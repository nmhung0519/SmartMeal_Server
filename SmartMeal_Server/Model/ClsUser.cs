using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ClsUser
    {
        public bool CheckLogin(LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "admin") return true;
            return false;
        }

        public UserModel GetUser(string username)
        {
            var user = new UserModel();
            user.Username = username;
            user.Id = 1;
            user.RoleId = 1;
            return user;
        }
    }
}
