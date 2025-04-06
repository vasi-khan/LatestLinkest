using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace smsSummary.Helper
{
    public static class database
    {
        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["eMIMPanel"].ConnectionString;
        }

        public static object GetScalarValue(string sql, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand(sql, cnn);
            object obj = cmd.ExecuteScalar();
            return obj;
        }
        public static object GetScalarValue(string sql)
        {
            object o;
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                //cnn.Open();
                //o = GetScalarValue(sql, cnn);
                //cnn.Close();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cnn.Open();
                    cmd.CommandTimeout = 1000;
                    o = cmd.ExecuteScalar();
                    cnn.Close();
                }
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
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sql;
                da.Fill(dt);
            }
            return dt;
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
    }
}