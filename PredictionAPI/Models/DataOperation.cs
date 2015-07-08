using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PredictionAPI.Models
{
    public class DataOperation
    {
        private const string conStr = ConfigurationManager.ConnectionStrings["Prediction"].ConnectionString;
        private SqlConnection conn;
        private DataTable dt;
        public DataOperation()
        {
            this.conn = new SqlConnection(conStr);
            dt = new DataTable();
        }

        public DataTable turnTo104Score(Ast ast)
        {

            return null;
        }
    }
}