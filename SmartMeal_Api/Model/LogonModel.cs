using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class LogonModel: UserModel
    {
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public int BirthYear { get; set; }

        public string Validate()
        {
            if (string.IsNullOrEmpty(Username)) return "Tài khoản không được để trống";
            if (Username.Length < 5) return "Độ dài tài khoản từ 5-32 ký tự";
            if (string.IsNullOrEmpty(Fullname)) return "Họ tên không được để trống";
            if (string.IsNullOrEmpty(Password1)) return "Mật khảu không được để trống";
            if (Sex < 0 || Sex > 1) return "Giá trị giới tính không hợp lệ";
            if (BirthYear < 1900 || BirthYear > DateTime.Now.Year) return "Năm sinh phải lớn hơn 1900 và nhỏ hơn hoặc bằng năm hiện tại";
            if (Password1.Length != 32) return "Mật khẩu không hợp lệ";
            if (RoleId < 0 || RoleId > 4) return "Vai trò tài khoản không hợp lệ";
            if (Password1 != Password2) return "Mật khẩu không trùng nhau";
            return "";
        }
    }
}
