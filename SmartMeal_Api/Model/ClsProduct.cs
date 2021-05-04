using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ClsProduct
    {
        public string Insert(ProductModel model, out ProductModel product)
        {
            var connection = new Connection();
            Hashtable ht = new Hashtable(); 
            product = new ProductModel();
            try
            {
                ht.Add("Name", model.Name);
                ht.Add("ProductLineId", model.ProductLineId);
                ht.Add("Price", model.Price);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Product_Insert", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt == null || dt.Rows.Count == 0) return "Đã xảy ra lỗi khi thêm mới sản phẩm.";
                product.Name = model.Name;
                product.Price = model.Price;
                product.ProductLineId = model.ProductLineId;
                product.IsActive = 1;
                product.Id = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                return "";
            }
            catch (Exception ex) { return ex.Message; }
            finally
            {
                connection = null;
                ht = null;
            }
        }

        public string Search(SearchProductModel model, out List<ProductModel> products)
        {
            var connection = new Connection();
            Hashtable ht = new Hashtable();
            products = new List<ProductModel>();
            try
            {
                ht.Add("Id", model.Id);
                ht.Add("Name", model.Name);
                ht.Add("ProductLineId", model.ProductLineId);
                ht.Add("StartPrice", model.StartPrice);
                ht.Add("EndPrice", model.EndPrice);
                ht.Add("IsActive", model.IsActive);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Product_Search", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) products.Add(new ProductModel(dr));
                }
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string ChangeStatus(int id, int isActive, string username)
        {
            var connection = new Connection();
            Hashtable ht = new Hashtable();
            try
            {
                ht.Add("Id", id);
                ht.Add("StatusId", isActive);
                ht.Add("Username", username);
                if (connection.ExecuteNonQuery("sp_Product_ChangeStatus", ht) > 0) return "";
                return "Xảy ra lỗi trong quá trình cập nhật trạng thái sản phẩm";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string Update(ProductModel model, string username)
        {
            var connection = new Connection();
            Hashtable ht = new Hashtable();
            try
            {
                ht.Add("Id", model.Id);
                ht.Add("Name", model.Name);
                ht.Add("ProductLineId", model.ProductLineId);
                ht.Add("Price", model.Price);
                ht.Add("IsActive", model.IsActive);
                ht.Add("Username", username);
                if (connection.ExecuteNonQuery("sp_Product_Update", ht) > 1) return "";
                return "Xảy ra lỗi trong quá trình cập nhật sản phẩm";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}
