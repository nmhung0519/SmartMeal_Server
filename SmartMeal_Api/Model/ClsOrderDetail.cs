using SmartMeal_Api;
using SmartMeal_Api.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Model
{
    public class ClsOrderDetail
    {
        public string Insert(OrderDetailModel model) {
            var connection = new Connection();
            var ht = new Hashtable();
            try
            {
                ht.Add("OrderId", model.OrderId);
                ht.Add("ProductName", model.ProductName);
                ht.Add("ProductCount", model.ProductCount);
                ht.Add("ProductPrice", model.ProductPrice);
                int a = connection.ExecuteNonQuery("sp_OrderDetail_Insert", ht);
                if (a == 0) return "Xảy ra lỗi trong quá trình đặt món";
                return "";
            }
            catch (Exception ex) {
                return ex.Message;
            }
            finally {
                connection = null;
                ht.Clear();
            }
        }

        public string GetInfoPayment(int tableId, out List<PaymentItem> result) {
            result = new List<PaymentItem>();
            var connection = new Connection();
            var ht = new Hashtable();
            try {
                ht.Add("TableId", tableId);
                DataTable dt = new DataTable();
                string msg = connection.GetDatatableFromProc("sp_OrderDetail_GetInfoPayment", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                foreach (DataRow dr in dt.Rows) {
                    result.Add(new PaymentItem(dr));
                }
                return "";
            }
            catch (Exception ex) {
                return "GetInfoPayment_EX: " + ex.Message;
            }
            finally {
                connection = null;
                ht.Clear();
            }
        }
    }
}
