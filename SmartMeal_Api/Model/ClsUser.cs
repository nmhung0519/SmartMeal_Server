using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ClsUser
    {
        public bool CheckLogin(LoginModel model)
        {
            try
            {
                var connection = new Connection();
                Hashtable ht = new Hashtable();
                ht.Add("Username", model.Username);
                ht.Add("Password", model.Password);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_User_CheckLogin", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) throw new Exception(msg);
                var tmp = dt.Rows[0].ItemArray[0].ToString();
                return (dt.Rows[0].ItemArray[0].ToString() == "1");
            }
            catch { throw; }
        }

        public UserModel GetUserByUsername(string username)
        {
            try
            {
                var connection = new Connection();
                Hashtable ht = new Hashtable();
                ht.Add("Username", username);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_User_GetByUsername", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) throw new Exception(msg);
                var user = new UserModel(dt.Rows[0]);
                user.CreateToken();
                return user;
            }
            catch { throw; }
        }

        public UserModel GetUserById(int id)
        {
            try
            {
                var connection = new Connection();
                Hashtable ht = new Hashtable();
                ht.Add("Id", id);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_User_GetById", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) throw new Exception(msg);
                var user = new UserModel(dt.Rows[0]);
                user.CreateToken();
                return user;
            }
            catch { throw; }
        }

        public bool Insert(LogonModel model)
        {
            try
            {
                var connection = new Connection();
                Hashtable ht = new Hashtable();
                ht.Add("Username", model.Username);
                ht.Add("Password", model.Password1);
                ht.Add("Fullname", model.Fullname);
                ht.Add("BirthYear", model.BirthYear);
                ht.Add("Sex", model.Sex);
                ht.Add("RoleId", model.RoleId);
                if (connection.ExecuteNonQuery("sp_User_Insert", ht) == 0) return false;
                return true;
            }
            catch { throw; }
        }

        public bool UpdateStatus(int id, int status)
        {
            try
            {
                var connection = new Connection();
                Hashtable ht = new Hashtable();
                ht.Add("Id", id);
                ht.Add("Status", status);
                if (connection.ExecuteNonQuery("sp_User_UpdateStatus", ht) == 0) return false;
                return true;
            }
            catch { throw; }
        }
    }
}
