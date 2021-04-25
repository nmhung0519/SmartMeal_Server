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
    }
}
