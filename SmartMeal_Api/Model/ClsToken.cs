using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;

namespace SmartMeal_Api.Model
{
    public static class ClsToken
    {
        private static string secret = "admin@123";

        public static string Get(string username)
        {
            int time = GetTime(DateTime.Now.AddMinutes(5));
            string header = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("{" + $"\"username\": \"{username}\", \"expired\": {time}" + "}"));
            string payload = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("{" + $"\"username\": \"{username}\"" + "}"));
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            string signature = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(header + "." + payload)));
            return $"{header}.{payload}.{signature}";
        }

        public static bool Verify(string token)
        {
            bool isVerified = false;
            try
            {
                string[] tmp = token.Split('.');
                if (tmp.Length != 3) return false;
                var hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
                string signature = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(tmp[0] + "." + tmp[1])));
                var objHeader = JsonSerializer.Deserialize<HeaderTokenModel>(Encoding.UTF8.GetString(Convert.FromBase64String(tmp[0])));
                if (tmp[2].Equals(signature) && objHeader.expired >= GetTime(DateTime.Now)) isVerified = true;

            }
            catch { isVerified = false; }
            return isVerified;
        }

        public static bool TryGetUser(string token, out string username)
        {
            username = "";
            try
            {
                string payload = token.Split('.')[1];
                var objPayload = JsonSerializer.Deserialize<PayloadTokenModel>(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
                username = objPayload.username;
                return true;
            }
            catch { return false; }
        }

        private static int GetTime(DateTime datetime)
        {
            TimeSpan interval = datetime.Subtract(new DateTime(2000, 1, 1));
            return (int)interval.TotalSeconds;
        }

        public class PayloadTokenModel
        {
            public string username { get; set; }
        }

        public class HeaderTokenModel
        {
            public string username { get; set; }
            public int expired { get; set; }
        }

    }
}
