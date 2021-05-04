using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ClsProductLine
    {
        public string Insert(ProductLineModel model, string username, out ProductLineModel productLine)
        {
            productLine = null;
            var connection = new Connection();
            Hashtable ht = new Hashtable();
            try
            {
                ht.Add("Name", model.Name);
                ht.Add("ParentId", model.ParentId);
                ht.Add("Username", username);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_ProductLine_Insert", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt == null || dt.Rows.Count == 0) return "Xảy ra lỗi trong quá trình thêm mới sản phẩm";
                int plId = 0;
                if (!int.TryParse(dt.Rows[0].ItemArray[0].ToString(), out plId)) return dt.Rows[0].ItemArray[0].ToString();
                msg = GetById(plId, out productLine);
                if (!string.IsNullOrEmpty(msg)) return msg;
                return "";
            }
            catch (Exception ex) {
                return ex.Message;
            }
            finally
            {
                connection = null;
                ht = null;
            }
        }

        public string Search(ProductLineModel model, out List<ProductLineModel> pls)
        {
            var connection = new Connection();
            Hashtable ht = new Hashtable();
            pls = new List<ProductLineModel>();
            try
            {
                ht.Add("Id", model.Id);
                ht.Add("Name", model.Name);
                ht.Add("ParentId", model.ParentId);
                ht.Add("IsActive", model.IsActive);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_ProductLine_Search", ht, out dt);
                if (!string.IsNullOrEmpty(msg) || dt == null || dt.Rows.Count == 0) return msg;
                foreach (DataRow dr in dt.Rows)
                {
                    pls.Add(new ProductLineModel(dr));
                }
                return "";
            }
            catch (Exception ex) { return ex.Message; }
            finally{
                connection = null;
                ht = null;
            }
        }

        public string GetById(int id, out ProductLineModel productLine)
        {
            productLine = null;
            try
            {
                var model = new ProductLineModel();
                model.Id = id;
                model.ParentId = -1;
                model.IsActive = -1;
                List<ProductLineModel> pls;
                string msg = Search(model, out pls);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (pls == null) return "Lỗi trong quá trình lấy thông tin dòng sản phẩm";
                productLine = pls.FirstOrDefault();
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}
