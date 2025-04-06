using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Transactions;
using System.Configuration;

namespace eMIMPanel.Helper
{
    public class dbHistorical
    {
        public static string TemplateIDMandatory()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["TemplateMandatory"]);
        }
        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["dbHistorical"].ConnectionString;
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


        public static DataSet GetDataSp(string strpName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strpName;
                cmd.CommandTimeout = 300;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                _DataSet = new DataSet();
                da.Fill(_DataSet);
                return _DataSet;
            }

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



        // create by Kuldeep 16-Feb-2022
        private static SqlCommand _mDataCom;
        public static SqlCommand mDataCom
        {
            get { return _mDataCom; }
            set { _mDataCom = value; }
        }
        private static DataSet _DataSet;
        public static DataSet DataSet
        {
            get
            {
                if (_DataSet == null)
                {
                    _DataSet = new DataSet();
                }
                return DataSet;
            }
            set { _DataSet = value; }
        }
        private static SqlConnection _mCon;
        public static SqlConnection mCon
        {
            get { return _mCon; }
            set { _mCon = value; }
        }
        public static DataSet GetDataSetSp(SqlParameter[] sqlParameters, string prcName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cnn.Open();
                    cmd.Connection = cnn;

                    for (int i = 0; i < sqlParameters.Length; i++)
                    {
                        cmd.Parameters.Add(sqlParameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = prcName;
                    cmd.CommandTimeout = 300;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    _DataSet = new DataSet();
                    da.Fill(_DataSet);
                    cnn.Close();
                    // da.Fill(_DataSet);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cnn.Close();
                }
                return _DataSet;

            }

        }

        public static DataTable GetDataTableSp(SqlParameter[] aarPrm, string strpName)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cnn.Open();
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = strpName;
                    cmd.CommandTimeout = 1200;
                    for (int i = 0; i < aarPrm.Length; i++)
                    {
                        cmd.Parameters.Add(aarPrm[i]);
                    }
                    cmd.ExecuteNonQuery();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    cnn.Close();
                    //return dt;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cnn.Close();
                }
                return dt;

            }


        }

        public static int ExecuteSqlSP(SqlParameter[] aarmprm, string strName)
        {

            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
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

        }

    }
}