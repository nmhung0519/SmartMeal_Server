using SmartMeal_Api;
using SmartMeal_Api.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Server.Model
{
    public class ClsOrder
    {
        public string InsertOrUpdate(OrderModel model)
        {
            var connection = new Connection();
            try
            {
                var ht = new Hashtable();
                ht.Add("Id", model.Id);
                ht.Add("TableId", model.TableId);
                ht.Add("StartTime", model.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                ht.Add("CustomerName", model.CustomerName);
                ht.Add("CustomerContact", model.CustomerContact);
                ht.Add("CreatorId", 0);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Order_InsertOrUpdate", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                return "";

            }
            catch (Exception ex)
            {
                return "Xảy ra lỗi trong quá trình đặt bàn. Chi tiết: " + ex.Message;
            }
            finally
            {
                connection = null;
            }
        }

        public string GetPreOrderForTable(int tableId, out OrderModel order)
        {
            var connection = new Connection();
            var ht = new Hashtable();
            order = null;
            try
            {
                ht.Add("TableId", tableId);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Order_GetPreOrderForTable", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt != null && dt.Rows.Count > 0)
                    order = new OrderModel(dt.Rows[0]);
                return "";
            }
            finally
            {
                ht.Clear();
                connection = null;
            }
        }

        public string Confirm(int id)
        {
            var connection = new Connection();
            var ht = new Hashtable();
            try
            {
                ht.Add("Id", id);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Order_Confirm", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt != null && dt.Rows.Count > 0) return Convert.ToString(dt.Rows[0].ItemArray[0]);
                return "Xảy ra lỗi trong quá trình xác nhận";
            }
            finally
            {
                ht.Clear();
                connection = null;
            }
        }

        public int GetTableIdById(int id)
        {
            var connection = new Connection();
            var ht = new Hashtable();
            try
            {
                ht.Add("Id", id);
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Order_GetTableIdById", ht, out dt);
                if (!string.IsNullOrEmpty(msg) || dt == null || dt.Rows.Count == 0) return 0;
                return Convert.ToInt32("0" + dt.Rows[0].ItemArray[0]);
            }
            finally
            {
                ht.Clear();
                connection = null;
            }
        }
    }
}
