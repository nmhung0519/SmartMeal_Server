using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal_Api.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMeal_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        [Route("GetAllActive")]
        [HttpGet]
        [Authen]
        public ResponseModel GetAllActive()
        {
            var clsTable = new ClsTable();
            List<TableModel> tables;
            string msg = clsTable.GetAllActive(out tables);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            return new ResponseModel(true, tables);
        }

        [Route("Insert")]
        [HttpPost]
        [Authen]
        public ResponseModel Insert([FromBody] string tableName)
        {
            var clsTable = new ClsTable();
            int tableId;
            string msg = clsTable.Insert(tableName, out tableId);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            TableModel table;
            msg = clsTable.GetById(tableId, out table);
            if (!string.IsNullOrEmpty(msg)) return new ResponseModel(false, msg);
            return new ResponseModel(true, table);
        }
    }
}
