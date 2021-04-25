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
        public bool Insert(string name, int parentId, out string msg)
        {
            msg = "";

            var connection = new Connection();
            Hashtable ht = new Hashtable();
            try
            {
                ht.Add("pName", name);
                ht.Add("pParentId", parentId);
                if (connection.ExecuteNonQuery("sp_ProductLine_Insert", ht) > 0) return true;
                return false;
            }
            catch (Exception ex) {
                msg = ex.Message;
                return false;
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
    }
}
