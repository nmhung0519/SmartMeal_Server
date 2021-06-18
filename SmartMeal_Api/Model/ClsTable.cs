using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace SmartMeal_Api.Model
{
    public class ClsTable
    {
        public string GetAllActive(out List<TableModel> tables)
        {
            tables = new List<TableModel>();
            try
            {
                var connection = new Connection();
                DataTable dt;
                string msg = connection.GetDatatableFromProc("sp_Table_GetAllActive", new Hashtable(), out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tables.Add(new TableModel(dr));
                    }
                }
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string GetById(int id, out TableModel table)
        {
            table = null;
            try
            {
                var connection = new Connection();
                DataTable dt;
                Hashtable ht = new Hashtable();
                ht.Add("Id", id);
                string msg = connection.GetDatatableFromProc("sp_Table_GetById", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt == null || dt.Rows.Count == 0) return "Không tìm thấy dữ liệu bàn";
                table = new TableModel(dt.Rows[0]);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }
        
        public string Insert(string tableName, out int tableId)
        {
            tableId = 0;
            try
            {
                var connection = new Connection();
                DataTable dt;
                Hashtable ht = new Hashtable();
                ht.Add("TableName", tableName);
                string msg = connection.GetDatatableFromProc("sp_Table_Insert", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = dt.Rows[0].ItemArray[0].ToString();
                }
                if (int.TryParse(msg, out tableId)) return "";
                return msg;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string Fill(OrderModel model)
        {
            var connection = new Connection();
            var ht = new Hashtable();
            try
            {
                DataTable dt = new DataTable();
                ht.Add("TableId", model.TableId);
                ht.Add("CustomerName", model.CustomerName);
                ht.Add("CustomerContact", model.CustomerContact);
                ht.Add("CreatorId", 1);
                string msg = connection.GetDatatableFromProc("sp_Table_Fill", ht, out dt);
                if (!string.IsNullOrEmpty(msg)) return msg;
                return Convert.ToString(dt.Rows[0].ItemArray[0]);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection = null;
            }
        }

        public string GetPaymentInfo(int tableId, out DataPaymentModel model) {
            model = new DataPaymentModel();
            try {
                TableModel table;
                string msg = GetById(tableId, out table);
                if (!string.IsNullOrEmpty(msg)) return msg;
                model.TableName = table.TableName;
                OrderModel order;
                var clsOrder = new ClsOrder();
                msg = clsOrder.GetPreOrderForTable(tableId, out order);
                if (!string.IsNullOrEmpty(msg)) return msg;
                model.CustomerName = order.CustomerName;
                model.CustomerPhone = order.CustomerContact;
                model.OrderId = order.Id;
                var clsOrderDetail = new ClsOrderDetail();
                List<PaymentItem> paymentItems;
                msg = clsOrderDetail.GetInfoPayment(tableId, out paymentItems);
                if (!string.IsNullOrEmpty(msg)) return msg;
                model.Data = paymentItems;
                return "";
            }
            catch (Exception ex) {
                return "GetPaymentInfo_Exception: " + ex.Message;
            }
        }

        public string Pay(int tableId) 
        {
            var connection = new Connection();
            var ht = new Hashtable();
            try
            {
                ht.Add("TableId", tableId);
                int a = connection.ExecuteNonQuery("sp_Table_Pay", ht);
                if (a <= 0) 
                {
                    return "Xảy ra lỗi trong quá trình thanh toán";
                }
                return "";
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
            finally 
            {
                connection = null;
                ht.Clear();
            }
        }
    }
}
