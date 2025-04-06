using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CustomerDataSYNC.Helper
{
    public class database
    {
        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["eMIMPanel"].ConnectionString;
        }

        public static string GetConnectstring_HyundaiDb()
        {
            return ConfigurationManager.ConnectionStrings["HyundaiDb"].ConnectionString;
        }

        public static void ExecuteNonQuery(string Sql)
        {
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = Sql;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
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
    }
}