using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Transactions;
using System.Configuration;

namespace eMIMapi.Helper
{
    public static class database
    {
        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["eMIMPanel"].ConnectionString;

        }
        public static string getIPapikey()
        {
            return ConfigurationManager.AppSettings["IPAPIKEY"].ToString();

        }
        public static object GetScalarValue(string sql, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandTimeout = 1000;
            object obj = cmd.ExecuteScalar();
            return obj;
        }
        public static object GetScalarValue(string sql)
        {
            object o;
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                o = GetScalarValue(sql, cnn);
                cnn.Close();
            }
            return o;
        }
        public static DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 1000;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sql;
                da.Fill(dt);
            }
            return dt;
        }
        public static void ExecuteNonQuery(string Sql)
        {
            //using (TransactionScope trnscope = new TransactionScope())
            //{
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = Sql;
                cmd.CommandTimeout = 1000;
                cmd.ExecuteNonQuery();
               
                cnn.Close();
            }
            //}
        }
        public static DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 1000;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sql;
                da.Fill(ds);
            }
            return ds;
        }
    }
}
