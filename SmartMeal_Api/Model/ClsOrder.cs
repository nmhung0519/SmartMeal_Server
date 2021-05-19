﻿using SmartMeal_Api;
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
    }
}