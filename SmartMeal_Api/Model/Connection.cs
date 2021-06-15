using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace SmartMeal_Api.Model
{
    public class Connection
    {
        private string cnnString { get; set; }
        private MySqlConnection conn { get; set; }
        public Connection()
        {
            cnnString = "server=127.0.0.1;Database=smartmeal;user id=user; PWD=Admin@123";
            conn = new MySqlConnection(cnnString);
        }

        private void Connect()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        private void Disconnect()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public string GetDatatableFromProc(string procName, Hashtable ht, out DataTable dt)
        {
            dt = new DataTable();
            try
            {
                Connect();
                MySqlCommand cmd = new MySqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue("p" + item.Key.ToString(), item.Value);
                }
                MySqlDataAdapter adp = new MySqlDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(dt);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
            finally { Disconnect(); }
        }

        public int ExecuteNonQuery(string procName, Hashtable ht)
        {
            try
            {
                Connect();
                MySqlCommand cmd = new MySqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue("p" + item.Key.ToString(), item.Value);
                }
                return cmd.ExecuteNonQuery();
            }
            catch { throw; }
        }
    }
}
