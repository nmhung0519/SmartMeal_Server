using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server
{
    public class UserManager
    {
        private static List<UserRegister> users = new List<UserRegister>();
        
        public static bool CheckExists(string connectionId)
        {
            if (users.Where(x => x.ConnectionId == connectionId).Count() == 0) return false;
            return true;
        }
        public static List<string> GetAllConnectionId()
        {
            return users.Select(x => x.ConnectionId).ToList();
        }

        public static string GetConnectionId(string userName)
        {
            return users.Where(x => x.Username == userName).Select(x => x.ConnectionId).FirstOrDefault();
        }

        public static string GetUserName(string connectionId)
        {
            return users.Where(x => x.ConnectionId == connectionId).Select(x => x.Username).FirstOrDefault();
        }

        public static void DeleteByConnectionId(string connectionId)
        {
            var user = users.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
            if (user == null) return;
            users.Remove(user);
        }

        public static void DeleteByUserName(string userName)
        {
            var user = users.Where(x => x.Username == userName).FirstOrDefault();
            if (user == null) return;
            users.Remove(user);
        }

        public static void Add(string username, string connectionId)
        {
            users.Add(new UserRegister(connectionId, username));
        }

    }

    class UserRegister
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public UserRegister() { }
        public UserRegister(string id, string username)
        {
            ConnectionId = id;
            Username = username;
        }
    }
}
