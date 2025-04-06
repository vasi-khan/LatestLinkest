using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Transactions;
using System.Configuration;
using eMIMPanel.Helper;
namespace eMIMPanel.rcscode
{
    public static class database
    {
        public static string TemplateIDMandatory()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["TemplateMandatory"]);
        }
        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["dbRcs"].ConnectionString;
        }
        public static string getIPapikey()
        {
            return ConfigurationManager.AppSettings["IPAPIKEY"].ToString();
        }
        public static object GetScalarValue(string sql, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandTimeout = 3600;
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
            //using (TransactionScope trnscope = new TransactionScope())
            //{
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = Sql;
                cmd.ExecuteNonQuery();
                //trnscope.Complete();
                cnn.Close();
            }
            //}
        }
        
        public static void ExecuteNonQueryForMultipleConnection(string Sql, string Sql128, string Sql17)
        {
            string connection17 = ConfigurationManager.ConnectionStrings["blackList17"].ConnectionString;
            string connection128 = ConfigurationManager.ConnectionStrings["blackList128"].ConnectionString;

            ExecuteNonQuery(Sql);

            using (SqlConnection cnn = new SqlConnection(connection17))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = Sql17;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            using (SqlConnection cnn = new SqlConnection(connection128))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = Sql128;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
               
        public static DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sql;
                da.Fill(ds);
            }
            return ds;
        }

        // for bulk insert
        public static void BulkInsertData(DataTable dt, string tableName)
        {
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                SqlBulkCopy objbulk = new SqlBulkCopy(cnn);
                //assign Destination table name  
                objbulk.DestinationTableName = tableName;
                objbulk.ColumnMappings.Add("MobNo", "MobNo");

                cnn.Open();
                //insert bulk Records into DataBase.  
                objbulk.WriteToServer(dt);
                cnn.Close();
            }
        }

        public static void BulkInsertDataDynamic(DataTable dt, string tableName)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
                {
                    SqlBulkCopy objbulk = new SqlBulkCopy(cnn);
                    //assign Destination table name  
                    objbulk.DestinationTableName = tableName;
                    foreach (DataColumn item in dt.Columns)
                    {
                        objbulk.ColumnMappings.Add("[" + item + "]", item.ColumnName);
                    }

                    cnn.Open();
                    //insert bulk Records into DataBase.  
                    objbulk.WriteToServer(dt);
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static int ExecuteSqlSP(SqlParameter[] aarmprm, string strName)
        {

            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();

                                    cnn.Open();
                                    cmd.Connection = cnn;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = strName;
                                    for (int i = 0; i < aarmprm.Length; i++)
                                    {
                                        cmd.Parameters.Add(aarmprm[i]);
                                    }
                                    cmd.ExecuteNonQuery();
                                    int intresult = Int32.Parse(cmd.Parameters["@intresult"].Value.ToString());
                                    cnn.Close();
                                    return intresult;
                }
                catch (Exception)
                {

                    throw;
                }
                


            }

        }
    }
}
