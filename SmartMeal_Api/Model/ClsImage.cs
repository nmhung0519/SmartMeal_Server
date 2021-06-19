using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SmartMeal_Api.Model {
    public class ClsImage {
        public string Upload(string base64) {
            try {
                byte[] bytes = Convert.FromBase64String(base64);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                string filePath = "/home/hung/Documents/GitHub/SmartMeal_Server/SmartMeal_Api/UploadImage/" + fileName;
                File.WriteAllBytes(filePath, bytes);
                return "http://192.168.1.190/UploadImage/" + fileName;
            }
            catch {
                throw;
            }
        }
    }
}