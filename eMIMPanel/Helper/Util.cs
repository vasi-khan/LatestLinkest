using MIMPwdUtility;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace eMIMPanel.Helper
{
    public class Util
    {
        MIMUtil MIM = new MIMUtil();
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        string DebitedBalSendSMS = System.Configuration.ConfigurationManager.AppSettings["DebitedBalSendSMS"].ToString();
        string DebitedBalScheduleCreation = System.Configuration.ConfigurationManager.AppSettings["DebitedBalScheduleCreation"].ToString();
        string WithLastFileName = System.Configuration.ConfigurationManager.AppSettings["WithLastFileName"].ToString();
        string WithOutLastFileName = System.Configuration.ConfigurationManager.AppSettings["WithOutLastFileName"].ToString();
        public string db = "";
        public long smscount = 0;
        public long noof_message = 0;
        public double msg_rate = 0;
        //abhishek
        public DataTable dt;
        public SqlConnection con;
        public SqlCommand cmd;
        public SqlDataAdapter sda;


        public Util()
        {
            db = Convert.ToString(ConfigurationManager.AppSettings["dbName"]);
        }

        public class WABARoot
        {
            public string statusCode { get; set; }
            public string statusDesc { get; set; }
            public string mid { get; set; }
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public void Log(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public bool CheckMobileEmailDuplicate(string val, string type)
        {
            string sql = "SELECT Count(*) FROM customer where " + (type == "E" ? " EMAIL " : " MOBILE1 ") + " = '" + val + "'";
            int c = Convert.ToInt16(database.GetScalarValue(sql));
            if (c > 0) return true; else return false;
        }

        public bool IsOpenTempAc(string userid)
        {
            int c = Convert.ToInt16(database.GetScalarValue("Select count(*) from OPENTEMPLATEACCOUNT where userid='" + userid + "'"));
            return c == 0 ? false : true;
        }

        public DataTable ReadCSV(string path, string moblen)
        {
            try
            {
                DataTable dt = new DataTable();

                StreamReader sr = new StreamReader(path);

                string[] Headers = sr.ReadLine().Split(',');
                foreach (string header in Headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < Headers.Length; i++)
                    {
                        dr[i] = rows[i].Trim();
                    }
                    dt.Rows.Add(dr);
                }
                sr.Dispose();
                sr.Close();

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public DataTable ReadCSVNew(string path, string moblen)
        {
            try
            {
                DataTable dt = new DataTable();

                StreamReader sr = new StreamReader(path);

                string[] Headers = sr.ReadLine().Split(',');
                foreach (string header in Headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < Headers.Length; i++)
                    {
                        dr[i] = rows[i].Trim();
                    }
                    dt.Rows.Add(dr);
                }
                sr.Dispose();
                sr.Close();

                dt.Columns[0].ColumnName = "MobNo";
                HttpContext.Current.Session["UploadedCount"] = dt.Rows.Count;
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ReadTextFile(string path, string mobMIN, string mobMAX = "")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MobNo");

            List<string> lines = File.ReadAllLines(path).ToList();
            HttpContext.Current.Session["UploadedCount"] = lines.Count;
            lines = lines.Select(t => Regex.Replace(t, "[^0-9]", "")).ToList();
            lines.RemoveAll(x => x.Length < Convert.ToInt16(mobMIN));
            if (mobMAX == mobMIN)
            {
                lines = lines.Select(x => x.Substring(x.Length - Convert.ToInt16(mobMIN))).ToList();
            }

            lines.ForEach((item) => dt.Rows.Add(item));
            return dt;
        }

        public void RemoveDuplicateRowsFromTempTable(string user, string ext)
        {
            string sql = "";
            if (ext.ToUpper().Trim() != ".TXT")
            {
                string table1 = "tmp1_" + user;

                string sqlColumn1 = string.Format("Declare @columnName varchar(500) ; Select @columnName = Coalesce(@columnName + ',', '') +'convert(nvarchar(max),[' +Ltrim(column_name)+']) as [' +Ltrim(column_name)+'] '" +
                    " From information_schema.columns where table_name = '{0}'; select @columnName", table1);

                string sqlColumn2 = string.Format("Declare @columnName varchar(500) ; Select @columnName = Coalesce(@columnName + ',', '') +'convert(nvarchar(max),[' +Ltrim(column_name)+']) '" +
                    " From information_schema.columns where table_name = '{0}'; select @columnName", table1);

                string colnm1 = Convert.ToString(database.GetScalarValue(sqlColumn1));
                string colnm2 = Convert.ToString(database.GetScalarValue(sqlColumn2));

                string datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + table1 + @"' and ordinal_position = 1 "));

                if (datatype.ToLower() == "float")
                {
                    var regex = new Regex(Regex.Escape("nvarchar(max)"));
                    colnm1 = regex.Replace(colnm1, "numeric(18)", 1);
                    colnm2 = regex.Replace(colnm2, "numeric(18)", 1);
                }

                sql = string.Format(@";WITH cte AS(
                            SELECT {0},ROW_NUMBER() OVER (PARTITION BY {1} ORDER BY {2} ) row_num FROM {3}      
                              ) DELETE FROM cte WHERE row_num > 1;", colnm1, colnm2, colnm2, table1); // only for excel

                string table = "tmp_" + user;
                string sqlColumn = string.Format("Declare @columnName nvarchar(500) ; Select @columnName = Coalesce(@columnName + ',', '') +'Convert(nvarchar(max),[' +Ltrim(column_name)+']) as [' +Ltrim(column_name)+'] ' From information_schema.columns where table_name = '{0}'; select @columnName", table);
                string sqlColumn_1 = string.Format("Declare @columnName nvarchar(500) ; Select @columnName = Coalesce(@columnName + ',', '') +'Convert(nvarchar(max),[' +Ltrim(column_name)+']) ' From information_schema.columns where table_name = '{0}'; select @columnName", table);
                string colnm = Convert.ToString(database.GetScalarValue(sqlColumn));
                string colnm_1 = Convert.ToString(database.GetScalarValue(sqlColumn_1));

                sql += string.Format(@"WITH cte1 AS(
                            SELECT {0},ROW_NUMBER() OVER (PARTITION BY {1} ORDER BY {2} ) row_num FROM {3}      
                              ) DELETE FROM cte1 WHERE row_num > 1;", colnm, colnm_1, colnm_1, table);  // for txt file and excel

            }
            else
            {
                string table = "tmp_" + user;
                string sqlColumn = string.Format("Declare @columnName nvarchar(500) ; Select @columnName = Coalesce(@columnName + ',', '') +'[' +Ltrim(column_name)+']' From information_schema.columns where table_name = '{0}'; select @columnName", table);
                string colnm = Convert.ToString(database.GetScalarValue(sqlColumn));

                sql = string.Format(@"WITH cte AS(
                            SELECT {0},ROW_NUMBER() OVER (PARTITION BY {1} ORDER BY {2} ) row_num FROM tmp_{3}      
                              ) DELETE FROM cte WHERE row_num > 1;", colnm, colnm, colnm, user);  // for txt file and excel

            }
            database.ExecuteNonQuery(sql);

        }

        public string SaveBlackListMobileNo(string FilePath)
        {
            try
            {
                string moblen = "10";
                DataTable dt = ReadTextFile(FilePath, moblen);

                if (dt.Rows.Count == 0)
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                else
                {
                    string cc = "91";
                    int Y = dt.Rows.Count;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sql = string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile) VALUES('{1}') ;", dr["MobNo"], dr["MobNo"]);
                        sql += string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile) VALUES('{1}')", cc + dr["MobNo"], cc + dr["MobNo"]);

                        string sql128 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile) VALUES('{1}')", dr["MobNo"], dr["MobNo"]);
                        string sql17 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile) VALUES('{1}')", cc + dr["MobNo"], cc + dr["MobNo"]);

                        database.ExecuteNonQueryForMultipleConnection(sql, sql128, sql17);
                    }
                    return "RECORDCOUNT " + Y.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string SaveTempTable(string FilePath, string SheetName, string user, string extension, string folder, string filenm, string s = "", string moblen = "", string MobMIN = "", string MobMAX = "")
        {
            bool chkUnq = user.ToUpper() == "MIM2101371" ? false : false;
            string username = user;
            if (!File.Exists(FilePath))
            {
                return "There was some error on file upload. Please upload again.";
            }
            Log("FU-user-" + user + ". File-" + FilePath + " Extension-"+ extension );
            string user1 = "tmp1_" + user;
            user = "tmp_" + user;
            string colnm = "";
            string datatype = "";
            string sql = "";
            string ccode = Convert.ToString(database.GetScalarValue("SELECT defaultCountry FROM customer WITH(NOLOCK) WHERE USERNAME='" + username + "'"));
            if (extension.ToLower().Contains("xls"))
            {
                sql = @"if exists (SELECT * FROM sys.tables WHERE name='" + user1 + @"') drop table " + user1 + @" ;
                SELECT " + (chkUnq ? "DISTINCT" : "") + " * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception)
                {
                    return "Invalid file format.";
                }
                colnm = Convert.ToString(database.GetScalarValue("SELECT column_name FROM information_schema.columns WHERE table_name = '" + user1 + @"' and ordinal_position = 1 "));
                datatype = Convert.ToString(database.GetScalarValue("SELECT Data_Type FROM information_schema.columns WHERE table_name = '" + user1 + @"' and ordinal_position = 1 "));
                if (datatype.ToLower() != "float")
                {
                    Int64 cn1 = Convert.ToInt64(database.GetScalarValue("SELECT count(*) FROM " + user1 + @" WHERE [" + colnm + "] like '%.%e+%' "));
                    if (cn1 > 0) return "Error in file. Please check the file. ";
                }

                if (datatype.ToLower() != "float")
                {
                    sql = @"if exists (SELECT * FROM sys.tables WHERE name='" + user + @"') drop table " + user + @" ;
                    SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                }
                else
                {
                    sql = @"if exists (SELECT * FROM sys.tables WHERE name='" + user + @"') drop table " + user + @" ;
                    SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(isnull([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                }

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }
            else if (extension.ToLower().Contains("txt"))
            {
                try
                {
                    Log(" ReadTextFile start- MobMin - "+ MobMIN.ToString() + " MobMax - "+ MobMAX.ToString() );
                    DataTable dt = ReadTextFile(FilePath, MobMIN, MobMAX);
                    Log(" ReadTextFile count -" + FilePath + " -- " + dt.Rows.Count.ToString());
                    if (dt.Rows.Count == 0)
                        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                    else
                    {
                        sql = string.Format("if exists (select * from sys.tables WHERE name = '{0}') drop table {1}", user, user);
                        sql += string.Format(" ; Create table {0} (MobNo varchar(15) )", user);
                        Log(sql);
                        database.ExecuteNonQuery(sql);
                        database.BulkInsertData(dt, user); colnm = "MobNo";
                    }
                }
                catch (Exception e1)
                {
                    Log(" Error in ReadTextFile " + e1.Message + " - " + e1.StackTrace);
                }
            }
            //rabi 29/07/21
            else if (extension.ToLower().Contains("csv"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();
                    DataTable dt = null;
                    if (Convert.ToString(MobMIN) != "" && Convert.ToString(MobMAX) != "" && Convert.ToString(MobMIN) != "LongURLCheck")
                    {
                        dt = ReadCSVNew(FilePath, MobMIN);
                    }
                    else if (Convert.ToString(MobMIN) == "LongURLCheck")
                    {
                        dt = ReadCSV(FilePath, MobMIN);
                    }

                    if (dt.Rows.Count == 0)
                        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                    else
                    {
                        sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
                        string sql1 = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + "";

                        sb.Append(@"Create table " + user + "  (");
                        sb1.Append(@"Create table " + user1 + "  (");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i != dt.Columns.Count - 1)
                            {
                                sb.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                                sb1.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                            }
                            else
                            {
                                sb.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                                sb1.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                            }
                        }
                        sb.Append(")");
                        sb1.Append(")");

                        database.ExecuteNonQuery(sql + "  " + sb.ToString() + " " + sql1 + " " + sb1.ToString());
                        database.BulkInsertDataDynamic(dt, user1);
                        database.BulkInsertDataDynamic(dt, user);
                        colnm = Convert.ToString(database.GetScalarValue("SELECT column_name FROM information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                    }
                }
                catch (Exception)
                {

                }
            }
            Log("file uploading....");
            // Create new table with SELECT INTO query
            string selectIntoQuery = @"
                                      IF EXISTS (SELECT * FROM sys.tables WHERE name='tblIncorrect_" + Convert.ToString(HttpContext.Current.Session["UserID"]) + @"')
                                          DROP TABLE tblIncorrect_" + Convert.ToString(HttpContext.Current.Session["UserID"]) + @";
                                      SELECT * INTO tblIncorrect_" + Convert.ToString(HttpContext.Current.Session["UserID"]) + " FROM " + user;
            Log(selectIntoQuery);
            database.ExecuteNonQuery(selectIntoQuery);
            Log("selectIntoQuery executed");
            sql = @" DELETE FROM " + user + @" WHERE len([" + colnm + "]) < " + MobMIN + "; " +
                                      " UPDATE " + user + @" SET mobno = REPLACE(REPLACE(" + colnm + ", '+', ''), ' ', '') WHERE mobno LIKE '%+%' OR mobno LIKE '% %';" +
                                      " UPDATE tblIncorrect_" + Convert.ToString(HttpContext.Current.Session["UserID"]) + " SET mobno = REPLACE(REPLACE(" + colnm + ", '+', ''), ' ', '') WHERE mobno LIKE '%+%' OR mobno LIKE '% %';" +
                                      " DELETE FROM " + user + @" WHERE PATINDEX('%[^0-9]%', " + colnm + ") > 0;";
            Log(sql);
            database.ExecuteNonQuery(sql);
            Log("sel executed");
            if (Convert.ToString(MobMIN) != "" && Convert.ToString(MobMAX) != "")
            {
                database.ExecuteNonQuery("DELETE FROM " + user + @" WHERE len([" + colnm + "]) < " + MobMIN);
                if (Convert.ToString(MobMIN) == Convert.ToString(MobMAX))
                {
                    database.ExecuteNonQuery("UPDATE " + user + @" SET [" + colnm + "] = right([" + colnm + "], " + MobMIN + ") WHERE len([" + colnm + "]) > " + MobMIN);
                    database.ExecuteNonQuery("UPDATE tblIncorrect_" + Convert.ToString(HttpContext.Current.Session["UserID"]) + " SET [" + colnm + "] = right([" + colnm + "], " + MobMIN + ") WHERE len([" + colnm + "]) > " + MobMIN);
                }
                else
                {
                    database.ExecuteNonQuery("DELETE FROM " + user + @" WHERE len([" + colnm + "]) > " + MobMAX);
                }

                //Add By Vikas For Bajaj On 03-06-2024
                string BajajAllianzDLTNO = Convert.ToString(ConfigurationManager.AppSettings["BajajAllianzDLTNO"]);
                string[] BajajAllianzUsersArray = BajajAllianzDLTNO.Split(',');
                if (!BajajAllianzUsersArray.Contains(user.ToUpper()))
                {
                    database.ExecuteNonQuery("DELETE d FROM " + user + @" d inner join globalBlackListNo b on b.mobile=d.[" + colnm + "] ");
                }

                ///* rabi 15 july 21*/
                //database.ExecuteNonQuery(" IF EXISTS (select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + username + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + username + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

                //check column name is numeric or not
                sql = "SELECT ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
                Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
                if (x == 1) return "Column name is numeric. Cannot upload file.";

                try
                {
                    sql = "SELECT convert(numeric,[" + colnm + "]) FROM " + user;
                    DataTable dt = database.GetDataTable(sql);
                }
                catch (Exception ex)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }

            // CHECK FOR ALL NULL VALUES
            sql = "SELECT COUNT(distinct [" + colnm + "]) FROM " + user + " WHERE [" + colnm + "] IS NOT NULL ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            //Add By Vikas On 29_05_2024
            int WithOutDistinct = Convert.ToInt32(database.GetScalarValue("SELECT COUNT([" + colnm + "]) FROM " + user + " WHERE [" + colnm + "] IS NOT NULL "));
            if (Y <= 0)
            {
                HttpContext.Current.Session["DuplicateCount"] = 0;
                return "No Mobile Numbers found in the file";
            }
            else
            {
                HttpContext.Current.Session["DuplicateCount"] = WithOutDistinct - Y;
            }
            return "RECORDCOUNT " + Y.ToString();
        }

        public DataTable ReadCSV_Mobile(string path, string moblen)
        {
            try
            {
                DataTable dt = new DataTable();

                StreamReader sr = new StreamReader(path);

                string[] Headers = sr.ReadLine().Split(',');
                for (int i = 0; i < Headers.Length; i++)
                {
                    if (i == 0)
                    {
                        dt.Columns.Add("MobNo");
                    }
                    else
                    {
                        dt.Columns.Add(Headers[i]);
                    }
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]\"[^\"]\")[^\"]$)");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < Headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                sr.Dispose();
                sr.Close();

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SaveTempTable3(string FilePath, string SheetName, string user, string extension, string folder, string filenm, string mobLen = "", string MobMIN = "", string MobMAX = "")
        {
            string username = user;
            if (!File.Exists(FilePath))
            {
                return "There was some error on file upload. Please upload again.";
            }
            Log("FUxclud-user-" + user + ". File-" + FilePath);
            string maintable = "tmp_" + user;
            string maintable1 = "tmp1_" + user;
            string user1 = "tmp1Ex_" + user;
            user = "tmpEx_" + user;

            string colnm = "";
            string datatype = "";
            string sql = "";

            string ccode = Convert.ToString(database.GetScalarValue("Select defaultCountry from customer where username='" + username + "'"));
            string moblen = (ccode == "91" ? "10" : "9");

            if (extension.ToLower().Contains("xls"))
            {
                sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @" ;
                SELECT distinct * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex1)
                {
                    return "Invalid file format.";
                }
                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                if (datatype.ToLower() != "float")
                {
                    Int64 cn1 = Convert.ToInt64(database.GetScalarValue("Select count(*) from " + user1 + @" where [" + colnm + "] like '%.%e+%' "));
                    if (cn1 > 0) return "Error in file. Please check the file. ";
                }

                if (datatype.ToLower() != "float")
                {
                    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                }
                else
                {
                    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(isnull([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                }

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }

            }
            else if (extension.ToLower().Contains("txt"))
            {
                #region< Commented on 26-02-2021 >

                //sql = @"if exists (select * from sys.tables where name = '" + user1 + @"') drop table " + user1 + @"; 
                //select distinct * into " + user1 + " FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=No','SELECT * FROM " + filenm + "') ";
                //try
                //{
                //    database.ExecuteNonQuery(sql);
                //}
                //catch (Exception ex1)
                //{
                //    return "Invalid file format.";
                //}
                //colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                //datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

                //if (datatype.ToLower() != "float")
                //{
                //    Int64 cn1 = Convert.ToInt64(database.GetScalarValue("Select count(*) from " + user1 + @" where [" + colnm + "] like '%.%e+%' or  [" + colnm + "] like '%.%E+%'"));
                //    if (cn1 > 0) return "Error in file. Please check the file. ";
                //}

                //if (datatype.ToLower() != "float")
                //{
                //    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                //    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
                //}
                //else
                //{
                //    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                //    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
                //}
                //try
                //{
                //    database.ExecuteNonQuery(sql);
                //}
                //catch (Exception ex2)
                //{
                //    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                //}

                #endregion

                try
                {
                    DataTable dt = ReadTextFile(FilePath, MobMIN, MobMAX);
                    if (dt.Rows.Count == 0)
                        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                    else
                    {
                        sql = string.Format("if exists (select * from sys.tables where name = '{0}') drop table {1}", user, user);
                        sql += string.Format(" Create table {0} (MobNo varchar(15) )", user);

                        database.ExecuteNonQuery(sql);

                        database.BulkInsertData(dt, user); colnm = "MobNo";
                    }

                }
                catch (Exception ex)
                {

                }
            }
            else if (extension.ToLower().Contains("csv"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();

                    DataTable dt = ReadCSV(FilePath, MobMIN);
                    if (dt.Rows.Count == 0)
                        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                    else
                    {
                        sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
                        string sql1 = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + "";

                        sb.Append(@"Create table " + user + "  (");
                        sb1.Append(@"Create table " + user1 + "  (");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i != dt.Columns.Count - 1)
                            {
                                sb.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                                sb1.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                            }
                            else
                            {
                                sb.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                                sb1.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                            }
                        }
                        sb.Append(")");
                        sb1.Append(")");

                        database.ExecuteNonQuery(sql + "  " + sb.ToString() + " " + sql1 + " " + sb1.ToString());
                        database.BulkInsertDataDynamic(dt, user1);
                        database.BulkInsertDataDynamic(dt, user);
                        colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                    }
                }
                catch (Exception ex)
                {

                }
            }

            database.ExecuteNonQuery("delete from " + user + @"   where len([" + colnm + "]) < " + MobMIN);
            if (Convert.ToString(MobMIN) != Convert.ToString(MobMAX))
            {
                database.ExecuteNonQuery("update " + user + @" set [" + colnm + "] = right([" + colnm + "], " + MobMIN + ") where len([" + colnm + "]) > " + MobMIN);
            }
            else
            {
                database.ExecuteNonQuery("delete from " + user + @"   where len([" + colnm + "]) > " + MobMAX);
            }

            //check column name is numeric or not
            sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
            Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x == 1) return "Column name is numeric. Cannot upload file.";

            try
            {
                sql = "select convert(numeric,[" + colnm + "]) from " + user;
                DataTable dt = database.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            }

            // CHECK FOR ALL NULL VALUES
            sql = "select count(*) from " + user + " where [" + colnm + "] is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the file";

            // find matching of maintable and user table
            string colnmMainTable = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + maintable + @"' and ordinal_position = 1 "));
            Int32 matchingcnt = Convert.ToInt32(database.GetScalarValue("Select Count(*) from " + maintable + " m inner join " + user + " u on m.[" + colnmMainTable + "]=u.[" + colnm + "] "));

            database.ExecuteNonQuery("Delete from " + maintable + " where [" + colnmMainTable + "] in (select [" + colnm + "] from " + user + " )");

            database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + maintable1 + @"') Delete from " + maintable1 + " where [" + colnmMainTable + "] in (select [" + colnm + "] from " + user + " )");

            Int32 Z = Convert.ToInt32(database.GetScalarValue("select count(distinct [" + colnmMainTable + "]) from " + maintable + " where [" + colnmMainTable + "] is not null "));
            return "RECORDCOUNT," + Y.ToString() + "," + matchingcnt.ToString() + "," + Z;
        }

        public string SaveTempTable_old(string FilePath, string SheetName, string user, string extension, string folder, string filenm)
        {
            if (!File.Exists(FilePath))
            {
                return "There was some error on file upload. Please upload again.";
            }
            Log("FU-user-" + user + ". File-" + FilePath);
            string user1 = "tmp1_" + user;
            user = "tmp_" + user;
            string colnm = "";
            string sql = "";

            if (extension.ToLower().Contains("xls"))
            {
                sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @" ;
                SELECT distinct * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex1)
                {
                    return "Invalid file format.";
                }
                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

                sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }
            else if (extension.ToLower().Contains("txt"))
            {
                sql = @"if exists (select * from sys.tables where name = '" + user1 + @"') drop table " + user1 + @"; 
                select distinct * into " + user1 + " FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex1)
                {
                    return "Invalid file format.";
                }
                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

                sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }

            database.ExecuteNonQuery("update " + user + " set [" + colnm + "]=null where [" + colnm + "]='0'");
            //string colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 "));

            //check column name is numeric or not
            sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
            Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x == 1) return "Column name is numeric. Cannot upload file.";

            //update xls and set all space, dash to ""
            database.ExecuteNonQuery("update " + user + " set [" + colnm + "]= replace(replace([" + colnm + "],' ',''),'-','') ");
            //check all values are numeric 
            try
            {
                sql = "select convert(numeric,[" + colnm + "]) from " + user;
                DataTable dt = database.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            }

            // CHECK FOR ALL NULL VALUES
            sql = "select count(*) from " + user + " where [" + colnm + "] is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the file";

            //check minimum length of the column
            sql = "select isnull(min(len([" + colnm + "])),0) from " + user;
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x < 10) return "Mobile Number of less than 10 digit length found in the file. Please rectify the same and upload the file again.";
            if (x > 12) return "Mobile Numbers of more than 12 digit length found in the file. Please rectify the same and upload the file again. ";

            //check maximum length of the column
            sql = "select isnull(max(len([" + colnm + "])),0) from " + user;
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x > 12) return "Mobile Numbers of more than 12 digit length found in the file. Please rectify the same and upload the file again. ";

            //check 11 digit mobile nos
            sql = "select count(*) from " + user + " where len([" + colnm + "]) = 11 ";
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x > 0) return "Mobile Numbers of 11 digit length found in the file. Please rectify the same and upload the file again. ";

            return "RECORDCOUNT " + Y.ToString();
        }

        public string SaveTempTable2_NOTINUSE(string FilePath, string SheetName, string user, string extension, string folder, string filenm)
        {
            string sUserid = user;
            string user1 = "tmp1_" + user;
            user = "tmp_" + user;
            string colnm = "";
            string sql = "";

            if (extension.ToLower().Contains("xls"))
            {
                try
                {
                    sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @" ; ";
                    database.ExecuteNonQuery(sql);
                    sql = @"if exists(select * from sys.servers where name = N'" + sUserid + @"') begin EXEC sp_dropserver @server = N'" + sUserid + @"', @droplogins='droplogins' end ; ";
                    database.ExecuteNonQuery(sql);
                    sql = @"EXEC sp_addlinkedserver 
                @server = N'" + sUserid + @"',
                @srvproduct = N'Excel',
                @provider = N'Microsoft.ACE.OLEDB.12.0',
                @datasrc = N'" + FilePath + @"',
                @provstr = N'Excel 12.0'; ";
                    database.ExecuteNonQuery(sql);

                    //sql = @"EXEC sp_addlinkedsrvlogin '" + sUserid + @"', 'FALSE', 'emimpaneluser', 'WSRV33252-IND\inbox_usr', 'Nh2#42#%^*?RTf';";
                    //database.ExecuteNonQuery(sql);
                    sql = sql + @"SELECT distinct * into " + user1 + @" FROM OPENQUERY(" + sUserid + @", 'SELECT * FROM [" + SheetName + "]') ";

                    //SELECT distinct * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";

                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex1)
                {
                    Log("er - " + ex1.Message + " - " + sql);
                    return "Invalid file format.";
                }
                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

                //sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                //SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";

                sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENQUERY(" + sUserid + @", 'SELECT * FROM [" + SheetName + "]') ";

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }
            else if (extension.ToLower().Contains("txt"))
            {
                sql = @"if exists (select * from sys.tables where name = '" + user1 + @"') drop table " + user1 + @"; 
                select distinct * into " + user1 + " FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
                database.ExecuteNonQuery(sql);
                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

                sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2)
                {
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                }
            }

            database.ExecuteNonQuery("update " + user + " set [" + colnm + "]=null where [" + colnm + "]='0'");
            //string colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 "));

            //check column name is numeric or not
            sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
            Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x == 1) return "Column name is numeric. Cannot upload file.";

            //update xls and set all space, dash to ""
            database.ExecuteNonQuery("update " + user + " set [" + colnm + "]= replace(replace([" + colnm + "],' ',''),'-','') ");
            //check all values are numeric 
            try
            {
                sql = "select convert(numeric,[" + colnm + "]) from " + user;
                DataTable dt = database.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            }

            // CHECK FOR ALL NULL VALUES
            sql = "select count(*) from " + user + " where [" + colnm + "] is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the file";

            //check minimum length of the column
            sql = "select isnull(min(len([" + colnm + "])),0) from " + user;
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x < 10) return "Mobile Number of less than 10 digit length found in the file. Please rectify the same and upload the file again.";
            if (x > 12) return "Mobile Numbers of more than 12 digit length found in the file. Please rectify the same and upload the file again. ";

            //check maximum length of the column
            sql = "select isnull(max(len([" + colnm + "])),0) from " + user;
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x > 12) return "Mobile Numbers of more than 12 digit length found in the file. Please rectify the same and upload the file again. ";

            //check 11 digit mobile nos
            sql = "select count(*) from " + user + " where len([" + colnm + "]) = 11 ";
            x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x > 0) return "Mobile Numbers of 11 digit length found in the file. Please rectify the same and upload the file again. ";

            return "RECORDCOUNT " + Y.ToString();
        }

        public string SaveTempTableGroup(string user, List<string> grp)
        {
            string user1 = "tmp_" + user;
            string sql = "";

            sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
            database.ExecuteNonQuery(sql);
            try
            {
                for (int i = 0; i < grp.Count; i++)
                {
                    if (i != 0)
                    {
                        sql = sql + "  union all " + @" select CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL(d.mob,0),20,0)))) AS MobNo from grouphead h inner join groupdtl d on h.id=d.id where h.userid= '" + user + "' and h.id='" + grp[i] + "'";
                    }
                    else
                    {
                        sql = @" select CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL(d.mob,0),20,0)))) AS MobNo INTO " + user1 + @" from grouphead h inner join groupdtl d on h.id=d.id where h.userid= '" + user + "' and h.id='" + grp[i] + "'";
                    }
                }
                database.ExecuteNonQuery(sql);

            }
            catch (Exception ex2)
            {
                return "Mobile Numbers in the Group are not Numeric. Please check the file. ";
            }


            // CHECK FOR ALL NULL VALUES
            sql = "select count(*) from " + user1 + " where MobNo is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the GROUP";

            return "RECORDCOUNT " + Y.ToString();
        }

        public string SaveTempTable4Group(string user, string grp)
        {
            string user1 = "tmp_" + user;
            string sql = "";

            sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                select CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL(d.mob,0),20,0)))) AS mobile INTO " + user1 + @" from grouphead h inner join groupdtl d on h.id=d.id where h.userid= '" + user + "' and h.grpname='" + grp + "'";
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex2)
            {
                return "Mobile Numbers in the Group are not Numeric. Please check the file. ";
            }
            sql = "select count(*) from " + user1 + " where mobile is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the GROUP";

            return "RECORDCOUNT " + Y.ToString();
        }

        public DataTable GetSMSrecordsFromUSERTMP(string user)
        {
            user = "tmp_" + user;
            string sql = "if exists (select * from sys.tables where name='" + user + @"') Select * from " + user + @" else select '' ";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public byte[] convertStringtoByteArray(string s)
        {
            QRCodeEncoderDecoderLibrary.QREncoder QRCodeEncoder = new QRCodeEncoderDecoderLibrary.QREncoder();
            QRCodeEncoderDecoderLibrary.ErrorCorrection ErrorCorrection = (QRCodeEncoderDecoderLibrary.ErrorCorrection)0;
            Bitmap QRCodeImage;
            // encode data
            QRCodeEncoder.Encode(ErrorCorrection, s);

            // create bitmap
            QRCodeImage = QRCodeEncoderDecoderLibrary.QRCodeToBitmap.CreateBitmap(QRCodeEncoder, 4, 8);
            byte[] byteArray;
            using (MemoryStream stream2 = new MemoryStream())
            {
                Bitmap resized = new Bitmap(QRCodeImage, new Size(QRCodeImage.Width * 3, QRCodeImage.Height * 3));
                resized.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                byteArray = stream2.ToArray();
            }

            return byteArray;
        }
        public Bitmap convertStringtoBitMap(string s, int size)
        {
            QRCodeEncoderDecoderLibrary.QREncoder QRCodeEncoder = new QRCodeEncoderDecoderLibrary.QREncoder();
            QRCodeEncoderDecoderLibrary.ErrorCorrection ErrorCorrection = (QRCodeEncoderDecoderLibrary.ErrorCorrection)0;

            Bitmap QRCodeImage;
            // encode data
            QRCodeEncoder.Encode(ErrorCorrection, s);

            // create bitmap
            QRCodeImage = QRCodeEncoderDecoderLibrary.QRCodeToBitmap.CreateBitmap(QRCodeEncoder, 4, 8);
            //byte[] byteArray;
            using (MemoryStream stream2 = new MemoryStream())
            {
                Bitmap resized = new Bitmap(QRCodeImage, new Size(QRCodeImage.Width * size, QRCodeImage.Height * size));
                resized.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                return resized;
                //byteArray = stream2.ToArray();
            }

            //return byteArray;
        }
        public Bitmap ChangeColor(Bitmap image, Color fromColor, Color toColor)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetRemapTable(new ColorMap[]
            {
            new ColorMap()
            {
                OldColor = fromColor,
                NewColor = toColor,
            }
            }, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(
                    image,
                    new Rectangle(Point.Empty, image.Size),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return image;
        }

        public DataTable GetURLS4Report(string id, string url)
        {
            string sql = "SELECT long_url as LongURL,concat('" + url + "/', segment) as SmallURL FROM short_urls s where id='" + id + "'";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public string AddStats(string seg, string referer, string UserHostAddress, string browser, string platform, string ismobile, string manuf, string model)
        {
            string sql = "select id,long_url from short_urls where segment='" + seg + "'";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            if (dt.Rows.Count <= 0) return "Page_Not_Found_";
            string id = Convert.ToString(dt.Rows[0]["id"]);
            string longurl = Convert.ToString(dt.Rows[0]["long_url"]);
            sql = "Insert into stats (click_date, ip, referer, shortUrl_id, Browser, Platform, IsMobileDevice, MobileDeviceManufacturer, MobileDeviceModel)" +
                " values ('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + UserHostAddress + "','" + referer + "','" + id + "','" + browser + "','" + platform + "','" +
                    ismobile + "','" + manuf + "','" + model + "')";
            database.ExecuteNonQuery(sql);
            return longurl;
        }

        //RAVI 03-02-22
        public DataTable GetUserReport_B404_02_22(string userid, string url, string frm, string to, bool isMob, string richmedia = "", string filterMode = "")
        {
            // Add Country group by 02-02-22
            //d b Y : h i p

            string sql = "";
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));
            if (!isMob)
            {
                sql = @"SELECT row_number() over (order by U.added) AS SLNO,u.userid, U.long_url as LongURL,u.domainname + U.segment as SmallURL,
                         DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate,
                         '' as No_of_url_sent, COUNT(S.SHORTURL_ID) AS No_Of_Hits,'' as mobtrack,
                         U.id as URLID,U.SEGMENT FROM short_urls U left join stats S on U.ID = S.SHORTURL_ID
                         where U.userid = '" + userid + "' and U.added between '" + frm + "' and '" + to + @"' 
                         AND U.mobtrack<>'Y' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                         " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                         GROUP BY u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,U.ADDED,S.SHORTURL_ID order by U.added ";
            }
            else
            {
                sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                        DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                        COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                        sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,
                        U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME , /* cm.name */ ''  AS Country, DATEADD(MINUTE,0,convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as clkdate FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                       /* inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE */
                        left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                        where U.userid = '" + userid + "' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                        and U.added between '" + frm + "' and '" + to + @"'
                        " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                        GROUP BY U.long_url,u.domainname,U.SEGMENT,U.ADDED,m.urlid,U.id,U.URLNAME /* ,cm.name */ order by U.added,U.id ";
            }

            if (filterMode == "2")
            {
                if (!isMob)
                {
                    sql = @"SELECT row_number() over (order by U.added) AS SLNO,u.userid, U.long_url as LongURL,u.domainname + U.segment as SmallURL,
                           DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate,
                           '' as No_of_url_sent, COUNT(S.SHORTURL_ID) AS No_Of_Hits,'' as mobtrack,
                           U.id as URLID,U.SEGMENT FROM short_urls U left join stats S on U.ID = S.SHORTURL_ID
                           where U.userid = '" + userid + @"' and S.Click_date between '" + frm + "' and '" + to + @"' 
                          AND U.mobtrack<>'Y' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                          " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                          GROUP BY u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,U.ADDED,S.SHORTURL_ID order by S.click_date ";
                }
                else
                {
                    sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                            COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                            sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,convert(varchar,S.Click_date,102) as clkdate,
                            U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME  , /* cm.name */ '' AS Country FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                            /* inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE */
                            left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                            where U.userid = '" + userid + "' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + "' and S.Click_date between '" + frm + "' and '" + to + @"'
                            " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                            GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102) /* ,cm.name */ 
                            order by convert(varchar,S.Click_date,102) desc, U.id ";
                }

            }

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            //if (dt.Rows.Count > 0)
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //        dt.Rows[i]["SLNO"] = i + 1;
            return dt;
        }
        public DataTable GetClickMobileNumbers_B404_02_22(string userid, string id)
        {
            string user1 = "tmp_" + userid;
            database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @"; Create table " + user1 + @" (MobileNo numeric) ;  ");
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));

            string sql = "";
            sql = @"Insert into " + user1 + @" SELECT distinct m.mobile as mobileNo 
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
                where m.urlid = '" + id + @"' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"  ; 
                SELECT distinct convert(varchar,m.mobile) as mobile 
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
                where m.urlid = '" + id + "' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetUserReportDetail_B404_02_22(string userid, string id, bool mobtrk, string s1 = "", string s2 = "", string Country = "")
        {
            // Add Country Parameter on 02-02-31
            //d b Y : h i p
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));
            string sql = "";
            if (mobtrk)
            {
                sql = @"SELECT row_number() over ( order by click_date) as SLNO,convert(varchar,m.mobile) as mobile,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                isnull(L.operator,'') as operator, isnull(L.RegionName,'') as referer,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.urlid = s.shortUrl_id and m.id = s.urlid
                /* inner join SMSFILEUPLOAD F on F.ID = m.fileId inner join countrymast cm on cm.phonecode=F.COUNTRYCODE */
                LEFT JOIN (select distinct regionName,segment,operator,mobile from iplocation with(nolock)) L on m.mobile=L.mobile and m.segment=L.segment
                where m.urlid = '" + id + "' AND click_date Between '" + s1 + "' and '" + s2 + @"' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                /* and cm.name='" + Country + "' */ order by click_date desc";
            }
            else
            {
                sql = @"SELECT row_number() over ( order by s.click_date) As SLNO,'' as mobile,'' as smsdate, 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate,               
                            s.ip, isnull(L.city,'') + case when isnull(L.city,'')='' then ' ' else ', ' end + isnull(L.RegionName,'') referer,
                            s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel from stats s
                            inner join short_urls u with(nolock) on s.shorturl_id = u.id
                            LEFT JOIN  iplocation L with(nolock) on s.ip=L.query and u.segment=L.segment 
                            where s.shorturl_id = '" + id + @"' group by 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) , s.ip, 
                isnull(L.city, '') + case when isnull(L.city,'')= '' then ' ' else ', ' end + isnull(L.RegionName, '') ,
                s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel,s.click_date 
                            order by s.click_date desc";

            }
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserReport(string userid, string url, string frm, string to, bool isMob, string richmedia = "", string filterMode = "")
        {
            // Add Country group by 02-02-22
            string sql = "";
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));
            if (!isMob)
            {
                sql = @" SELECT row_number() over (order by U.added) AS SLNO,u.userid, U.long_url as LongURL,u.domainname + U.segment as SmallURL,
                         DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate,
                         '' AS No_of_url_sent, COUNT(S.SHORTURL_ID) AS No_Of_Hits,'' as mobtrack,
                         U.id as URLID,U.SEGMENT, U.buttonName as btn, U.pageName as pagename 
                         FROM short_urls U left join stats S on U.ID = S.SHORTURL_ID
                         WHERE U.userid = '" + userid + @"' AND U.added between '" + frm + @"' AND '" + to + @"' 
                         AND U.mobtrack<>'Y' AND U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                         " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                         GROUP BY u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,U.ADDED,S.SHORTURL_ID,U.buttonName, U.pageName order by U.added ";
            }
            else
            {
                sql = @" SELECT * INTO #T1 FROM short_urls WHERE userid = '" + userid + "' AND richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                         AND added between '" + frm + "' AND '" + to + @"';
                         SELECT * INTO #T2 FROM mobtrackurl WHERE urlid IN(SELECT ID FROM #T1);
                         SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                         DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                         COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                         SUM(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,
                         U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,convert(varchar,U.added,102) as clkdate,F.COUNTRYCODE 
                        FROM #T1 U
                        INNER JOIN #T2 m WITH(NOLOCK) ON U.ID = m.urlid
                        INNER JOIN SMSFILEUPLOAD F WITH(NOLOCK) ON F.ID = m.fileId AND F.USERID=u.userid 
                        INNER JOIN countrymast cm WITH(NOLOCK) ON cm.phonecode=F.COUNTRYCODE
                        LEFT JOIN mobstats S WITH(NOLOCK) ON U.ID = S.SHORTURL_ID AND s.urlid=m.id
                        WHERE U.userid = '" + userid + "' AND U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                        AND U.added between '" + frm + "' AND '" + to + @"'
                        " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                        GROUP BY U.long_url,u.domainname,U.SEGMENT,U.ADDED,m.urlid,U.id,U.URLNAME,cm.name,F.COUNTRYCODE order by U.added,U.id ";
            }

            if (filterMode == "2")
            {
                if (!isMob)
                {
                    sql = @"SELECT row_number() over (order by U.added) AS SLNO,u.userid, U.long_url as LongURL,u.domainname + U.segment as SmallURL,
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate,
                            '' as No_of_url_sent, COUNT(S.SHORTURL_ID) AS No_Of_Hits,'' as mobtrack,
                            U.id as URLID,U.SEGMENT FROM short_urls U 
                            left join stats S on U.ID = S.SHORTURL_ID
                            WHERE U.userid = '" + userid + @"' AND S.Click_date between '" + frm + @"' AND '" + to + @"' 
                            AND U.mobtrack<>'Y' AND U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                            " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                            GROUP BY u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,U.ADDED,S.SHORTURL_ID order by S.click_date ";
                }
                else
                {
                    sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                            COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                            sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,convert(varchar,S.Click_date,102) as clkdate,
                            U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                            inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
                            left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                            WHERE U.userid = '" + userid + @"' AND U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + @"' 
                            AND S.Click_date between '" + frm + @"' and '" + to + @"'
                            " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                            GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id ";
                }
            }

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserReportDemo(string userid, string url, DateTime frm, DateTime to, DateTime CretFrm, DateTime CretTo, string richmedia = "")
        {
            // Add Country group by 02-02-22
            //d b Y : h i p

            string sql = "";

            sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                      COUNT(DISTINCT m.mobile) AS No_of_url_sent, 
                      sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,
                 U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,convert(varchar,U.added,102) as clkdate,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                 inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
                 left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                 where U.userid = '" + userid + "'";
            if (url != "-1")
            {
                sql = sql + @"and U.id='" + url + "'";
            }
            else if (url == "-1")
            {
                if (Convert.ToString(CretFrm) != "" && Convert.ToString(CretTo) != "")
                    sql = sql + @"AND U.mobtrack<>'Y' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + "' U.added between '" + CretFrm + "' and '" + CretTo + @"' GROUP BY U.added,u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,S.SHORTURL_ID,U.buttonName, U.pageName order by U.added ";
                //else
                //    sql = sql + @"AND U.mobtrack<>'Y' and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + "' GROUP BY U.added, u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,S.SHORTURL_ID,U.buttonName, U.pageName order by U.added ";
            }

            if (Convert.ToString(frm) != "" && Convert.ToString(to) != "")
            {

                sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
COUNT(DISTINCT m.mobile) AS No_of_url_sent,
sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,convert(varchar,S.Click_date,102) as clkdate,
U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
where U.userid = '" + userid + "'";

                if (url != "-1")
                {
                    sql = sql + @"and U.id='" + url + "'";
                    sql = sql + @"GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id";

                }
                else if (url == "-1")
                {
                    if (Convert.ToString(CretFrm) != "" && Convert.ToString(CretTo) != "")
                    {
                        sql = sql + @"and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + "' and U.added between '" + CretFrm + "' and '" + CretTo + @"'  and S.Click_date between '" + frm + "' and '" + to + @"'
GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id ";
                    }
                    else
                    {
                        sql = sql + @"and U.richmediaurl='" + (richmedia == "Y" ? "1" : "0") + "' and S.Click_date between '" + frm + "' and '" + to + @"'
GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id ";
                    }

                }
            }

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            //if (dt.Rows.Count > 0)
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //        dt.Rows[i]["SLNO"] = i + 1;
            return dt;
        }

        public DataTable GetClickMobileNumbers(string userid, string id, string s1 = "", string s2 = "", string CountryCode = "")
        {
            // add two parameter clkdate and Country on 03-02-22
            string user1 = "tmp_" + userid;
            database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @"; Create table " + user1 + @" (MobileNo numeric) ;  ");

            string sql = "";
            sql = @"Insert into " + user1 + @" SELECT distinct m.mobile as mobileNo 
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid inner join SMSFILEUPLOAD F on F.ID = m.fileId 
                where m.urlid = '" + id + "' and f.COUNTRYCODE='" + CountryCode + "' AND click_date between '" + s1 + "' and '" + s2 + @"'   ; 
                SELECT distinct convert(varchar,m.mobile) as mobile 
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid inner join SMSFILEUPLOAD F on F.ID = m.fileId 
                where m.urlid = '" + id + "' and f.COUNTRYCODE='" + CountryCode + "' AND click_date between '" + s1 + "' and '" + s2 + @"'   ";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserReportDetail(string userid, string id, bool mobtrk, string s1 = "", string s2 = "", string CountryCode = "", string Checked = "Checked")
        {
            // Add Country Parameter on 02-02-31
            // Add ClickDate column while filter type is creation date on 03-02-22
            //d b Y : h i p

            string sql = "";
            if (mobtrk)
            {
                //' ' + left(convert(varchar,m.mobile),len(convert(varchar,m.mobile))-4) + 'XXXX'
                //sql = @"SELECT row_number() over ( order by click_date) as SLNO,convert(varchar,m.mobile) as mobile,
                //DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                //DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                //isnull(s.ip,'') as ip, isnull(L.city,'') + case when isnull(L.city,'')='' then ' ' else ', ' end + isnull(L.RegionName,'') as referer,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                //FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
                //LEFT JOIN (select distinct query,city,regionName,segment,operator from iplocation) L on s.ip=L.query and m.segment=L.segment
                //where m.urlid = '" + id + "' AND click_date Between '" + s1 + "' and '" + s2 + "' order by click_date desc";


                //sql = @"SELECT row_number() over ( order by click_date) as SLNO,convert(varchar,m.mobile) as mobile,
                //DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                //DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                //isnull(L.operator,'') as operator, isnull(L.RegionName,'') as Circle,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                //FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.urlid = s.shortUrl_id and m.id = s.urlid
                //inner join SMSFILEUPLOAD F on F.ID = m.fileId
                //LEFT JOIN (select distinct regionName,segment,operator,mobile from iplocation with(nolock)) L on m.mobile=L.mobile and m.segment=L.segment
                //where m.urlid = '" + id + "' AND click_date Between '" + s1 + "' and '" + s2 + "' and F.COUNTRYCODE='" + CountryCode + "' order by click_date desc";


                //Comment by naved khan at 06-06-2023
                //sql = @"SELECT row_number() over ( order by click_date) as SLNO, convert(varchar,m.mobile) as mobile,
                //            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                //            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                //            isnull(m2.Operator,'') as operator, isnull(m2.Circle,'') as Circle,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                //            FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.id = s.urlid
                //            inner join SMSFILEUPLOAD F on F.ID = m.fileId
                //            LEFT JOIN MOBILEMCCMNC d with(nolock) on d.MOBILENO = m.mobile
                //            --Left join ((select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,MOBILENO)MOBILENO from MOBILEMCCMNC with(nolock) union select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,destno)MOBILENO from delivery with(nolock))) d on d.MOBILENO = m.mobile
                //            left join(select Operator,Circle,MCCMNC from mstMCCMNC with(nolock)) m2 on m2.MCCMNC=d.MCCMNC      
                //            where m.urlid = '" + id + "' AND click_date Between '" + s1 + "' and '" + s2 + "' and F.COUNTRYCODE='" + CountryCode + @"'
                //            order by click_date desc "; // LEFT JOIN (select distinct regionName,segment,operator,mobile from iplocation with(nolock)) L on m.mobile=L.mobile and m.segment=L.segment

                if (Checked == "Unchecked")
                {
                    sql = @"SELECT row_number() over ( order by click_date) as SLNO, convert(varchar,m.mobile) as mobile,
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                            isnull(m2.Operator,'') as operator, isnull(m2.Circle,'') as Circle,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                            FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.id = s.urlid
                            inner join SMSFILEUPLOAD F on F.ID = m.fileId
                            LEFT JOIN MOBILEMCCMNC d with(nolock) on d.MOBILENO = m.mobile
                            --Left join ((select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,MOBILENO)MOBILENO from MOBILEMCCMNC with(nolock) union select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,destno)MOBILENO from delivery with(nolock))) d on d.MOBILENO = m.mobile
                            left join(select Operator,Circle,MCCMNC from mstMCCMNC with(nolock)) m2 on m2.MCCMNC=d.MCCMNC     
                            where m.urlid = '" + id + "' AND S.Click_date Between '" + s1 + "' and '" + s2 + @"'
                            order by click_date desc ";
                }
                else
                {
                    sql = @"SELECT row_number() over ( order by click_date) as SLNO, convert(varchar,m.mobile) as mobile,
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108)) as smsdate, 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                            isnull(m2.Operator,'') as operator, isnull(m2.Circle,'') as Circle,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                            FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.id = s.urlid
                            inner join SMSFILEUPLOAD F on F.ID = m.fileId
                            LEFT JOIN MOBILEMCCMNC d with(nolock) on d.MOBILENO = m.mobile
                            --Left join ((select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,MOBILENO)MOBILENO from MOBILEMCCMNC with(nolock) union select distinct convert(varchar,MCCMNC)MCCMNC,convert(varchar,destno)MOBILENO from delivery with(nolock))) d on d.MOBILENO = m.mobile
                            left join(select Operator,Circle,MCCMNC from mstMCCMNC with(nolock)) m2 on m2.MCCMNC=d.MCCMNC     
                            where m.urlid = '" + id + "' AND convert(date,S.Click_date) = '" + s1 + @"' --and F.COUNTRYCODE='" + CountryCode + @"'
                            order by click_date desc "; // LEFT JOIN (select distinct regionName,segment,operator,mobile from iplocation with(nolock)) L on m.mobile=L.mobile and m.segment=L.segment

                }
            }
            else
            {
                sql = @"SELECT row_number() over ( order by s.click_date) As SLNO,'' as mobile,'' as smsdate, 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate,               
                            s.ip, isnull(L.city,'') + case when isnull(L.city,'')='' then ' ' else ', ' end + isnull(L.RegionName,'') Circle,
                            s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel from stats s
                            inner join short_urls u with(nolock) on s.shorturl_id = u.id
                            LEFT JOIN  iplocation L with(nolock) on s.ip=L.query and u.segment=L.segment 
                            where s.shorturl_id = '" + id + @"' group by 
                            DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) , s.ip, 
                isnull(L.city, '') + case when isnull(L.city,'')= '' then ' ' else ', ' end + isnull(L.RegionName, '') ,
                s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel,s.click_date 
                            order by s.click_date desc";

            }
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }
        //-----------------

        public DataTable GetDemoUserReportDetail(string Mobile, string url, string frm, string to)
        {
            //d b Y : h i p
            //string sql = "SELECT long_url as LongURL,concat('http://emim.in/', segment) as SmallURL,DATE_FORMAT(added,'%d/%m/%Y') as CreationDate, num_of_clicks as No_Of_Hits FROM short_urls s where userid='" + userid + "'";
            string sql = @"select * from (
SELECT 0 AS SLNO, U.long_url as LongURL,concat('" + url + @"', U.segment) as SmallURL,
DATE_FORMAT(U.added, '%d-%b-%Y') as CreationDate,COUNT(S.SHORTURL_ID) AS No_Of_Hits,
U.id as URLID FROM short_urls U left join stats S on U.ID = S.SHORTURL_ID
where U.userid = '" + Mobile + "' and U.added between '" + frm + "' and '" + to + @" 23:59:59'
GROUP BY U.long_url,U.SEGMENT,U.ADDED,S.SHORTURL_ID
union all
SELECT 0 AS SLNO, U.long_url as LongURL,concat('" + url + @"', U.segment) as SmallURL,
DATE_FORMAT(U.added, '%d-%b-%Y') as CreationDate,COUNT(S.SHORTURL_ID) AS No_Of_Hits,
U.id as URLID FROM t_short_urls U left join stats S on U.ID = S.SHORTURL_ID
where U.userid = '" + Mobile + "' and U.added between '" + frm + "' and '" + to + @" 23:59:59'
GROUP BY U.long_url,U.SEGMENT,U.ADDED,S.SHORTURL_ID ) x
order by x.CreationDate";

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 0)
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["SLNO"] = i + 1;
            return dt;
        }

        public DataTable GetUserScheduledReport(string userid, string url, string frm, string to)
        {
            //d b Y : h i p

            string sql = "";
            if (userid == "0")
            {
                sql = @"SELECT row_number() over (order by U.added) AS SLNO,

where U.userid = '" + userid + "' and U.added between '" + frm + "' and '" + to + "' AND U.mobtrack<>'Y'  GROUP BY u.userid,u.id,U.long_url,u.domainname,U.SEGMENT,U.ADDED,S.SHORTURL_ID order by U.added ";
            }
            else
            {
                sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108) as CreationDate, 
COUNT(DISTINCT m.mobile) AS No_of_url_sent,
sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,
U.id as URLID FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
where U.userid = '" + userid + "' and  U.added between '" + frm + "' and '" + to + @"'
GROUP BY U.long_url,u.domainname,U.SEGMENT,U.ADDED,m.urlid,U.id order by U.added,U.id ";
            }
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            //if (dt.Rows.Count > 0)
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //        dt.Rows[i]["SLNO"] = i + 1;
            return dt;
        }

        public bool FileUploadStopped()
        {
            bool b = false;
            string sql = "Select stopFileUpload from SettingMast ";
            b = Convert.ToBoolean(dbmain.GetScalarValue(sql));
            //if (s == "1") b = true;
            return b;
        }

        public bool CheckAuthorization(string code)
        {
            bool b = false;
            string cd = Convert.ToString(ConfigurationManager.AppSettings["AuthorizationCode"]);
            if (code == cd) b = true;
            return b;
        }

        public bool UserExists(string UserID)
        {
            bool b = false;
            string sql = "Select UserID from " + db + ".user where UserID='" + UserID + "' order by userid LIMIT 1";
            string s = Convert.ToString(database.GetScalarValue(sql));
            if (s != "") b = true;
            return b;
        }

        public DataTable GetValidUser(string usr)
        {
            string sql = "Select UserID,noofurl,noofhit,DATE_FORMAT(createdon, '%d/%b/%Y') as createdon,DATE_FORMAT(validupto, '%d/%b/%Y') as validupto1,validupto,CASE WHEN ISNULL(mobtrack) THEN 'N' ELSE mobtrack end as mobtrack from " + db + ".user where UserID='" + usr + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void UpdateUser(string nourl, string nohit, string usr, string validity, bool mobtrk)
        {
            string sql = "";
            string s = (mobtrk ? "Y" : "N");
            if (validity == "")
                sql = "Update user set noofurl='" + nourl + "', noofhit='" + nohit + "', mobtrack='" + s + "' where UserID='" + usr + "'";
            else
                sql = "Update user set noofurl='" + nourl + "', noofhit='" + nohit + "', validupto = '" + validity + "', mobtrack='" + s + "' where UserID='" + usr + "'";
            database.ExecuteNonQuery(sql);
        }

        public string GetMobTrkOfUser(string UserID)
        {
            string sql = "Select isnull(mobtrack,'N') from customer where Username='" + UserID + "'";
            string s = Convert.ToString(database.GetScalarValue(sql));
            return s;
        }

        public bool Login(string UserID, string Password)
        {
            bool b = false;
            string sql = "Select UserID from " + db + ".user where UserID='" + UserID + "' order by userid LIMIT 1";
            string s = Convert.ToString(database.GetScalarValue(sql));
            if (s != "") b = true;
            return b;
        }

        public string GetURLbal(string UserID)
        {
            string sql = "Select noofurl from CUSTOMER where Username='" + UserID + "' ";
            string s = Convert.ToString(database.GetScalarValue(sql));
            return s;
        }

        public DataTable GetUserParameter(string usr)
        {
            string sql = "select * from CUSTOMER with(nolock) where username='" + usr + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserParameterAsPerCountry(string usr)
        {
            string sql = "select countrycode,rate_normalsms,rate_campaign,rate_smartsms,rate_otp,urlrate,dltcharge,c.name as countryName from smsrateaspercountry s with(nolock) " +
                "left join countryMast c ON s.countrycode=c.phonecode where username='" + usr + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetCountryName(string ccode)
        {
            return Convert.ToString(database.GetScalarValue("select top 1 name from countryMast where phonecode='" + ccode + "' "));
        }

        public bool CampaignExistsForDay(string user, string CampNm)
        {
            string sql = "Select Count(*) from smsfileupload where UserID='" + user + "' and convert(varchar,uploadtime,102) = convert(varchar,getdate(),102) and campaignname='" + CampNm + "'";
            int c = Convert.ToInt16(database.GetScalarValue(sql));
            if (c > 0) return true; else return false;
        }

        public DataTable GetUserSMPPACCOUNT(string DLT, string USER)
        {
            string sql = "select 'N' as GSM,* from SMPPSETTING where DLTNO ='" + DLT + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                sql = "select 'N' as GSM,* from SMPPSETTING where smppaccountid = (select top 1 smppaccountid from smppaccountuserid where userid='" + USER + "' ) ";
                dt = database.GetDataTable(sql);
            }
            return dt;
        }

        public DataTable GetUserSMPPACCOUNTCountry(string USER, string ccode)
        {
            string sql = "select 'N' as GSM,* from SMPPSETTING where smppaccountid = (select top 1 smppaccountid from smppaccountuserid where userid='" + USER + "' AND countrycode='" + ccode + "' ) and Active=1 ";
            DataTable dt = database.GetDataTable(sql);

            return dt;
        }

        public string GetLongURL(string segment)
        {
            string sql = "Select urlid from mobtrackurl where segment='" + segment + "'";
            int urlid = Convert.ToInt32(database.GetScalarValue(sql));
            sql = "Select long_url from short_urls where id='" + urlid.ToString() + "'";
            string lurl = Convert.ToString(database.GetScalarValue(sql));
            return lurl;
        }

        public string GetLongURLforQR(string segment)
        {
            string sql = "Select long_url from short_urls where segment='" + segment + "'";
            string lurl = Convert.ToString(database.GetScalarValue(sql));
            return lurl;
        }

        public bool SegmentExists(string seg)
        {
            string sql = "Select count(*) from short_urls where segment='" + seg + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows[0][0].ToString() == "0")
                return false;
            else
                return true;
        }

        public bool IsURLExists(string seg, string domain)
        {
            string sql = "Select segment from short_urls with (nolock) where segment='" + seg + "' and replace(replace(domainname,'http://',''),'https://','') = '" + domain.Replace("http://", "").Replace("https://", "") + "' union all " +
                "Select m.segment from mobtrackurl m with (nolock) inner join short_urls u with (nolock) on m.urlid=u.id where m.segment='" + seg + "' and replace(replace(u.domainname,'http://',''),'https://','') = '" + domain.Replace("http://", "").Replace("https://", "") + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count <= 0)
                return true;
            else return false;
        }

        public bool IsShowURL(string seg, string domain)
        {
            bool b = false;
            try
            {
                string sql = "";
                if (seg.Length == 8)
                {
                    b = true;
                    //sql = "select urlid from mobtrackurl where segment = '" + seg + "' ";
                    //string urlid = Convert.ToString(database.GetScalarValue(sql));
                    //sql = "Select id,userid from short_urls where id='" + urlid + "'";
                }
                else
                {
                    sql = "Select id,userid from short_urls with (nolock) where segment='" + seg + "' and mainurl=1 and replace(replace(domainname,'http://',''),'https://','') = '" + domain.Replace("http://", "").Replace("https://", "") + "'";
                    DataTable dt = database.GetDataTable(sql);
                    if (dt.Rows.Count <= 0)
                        b = true;
                    else
                    {
                        b = true;
                        //string id = dt.Rows[0]["id"].ToString();
                        //string usr = dt.Rows[0]["userid"].ToString();
                        //Int64 x = Convert.ToInt64(database.GetScalarValue("Select noofhit from customer where Username='" + usr + "'"));
                        ////Int64 y = Convert.ToInt64(database.GetScalarValue("SELECT count(shortUrl_id) FROM stats s where shortUrl_id = '" + id + "'"));
                        ////if (y <= x) b = true;
                        //if (x > 0)
                        //{
                        //    b = true;
                        //    database.ExecuteNonQuery("update customer set noofhit=noofhit-1 where username='" + usr + "'");
                        //}
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return b;
        }

        public bool Valid_VerificationLink(string segment)
        {
            if (Convert.ToInt16(database.GetScalarValue(" select count(*) from VerifyLink where segment='" + segment + "' and getdate()<=validhitrecdtime and hitrecdtime is null ")) > 0)
            {
                //update hittime
                database.ExecuteNonQuery("Update VerifyLink set hitrecdtime=getdate() where segment='" + segment + "' ");
                return true;
            }
            else return false;

        }

        public bool CreateUser(string UserID, string Password)
        {
            bool b = false;
            string sql = "insert into " + db + ".user (userid, password, validupto, createdon) values ('" + UserID + "','" + Password + "','" + DateTime.Now.AddYears(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "')";
            database.ExecuteNonQuery(sql);
            b = true;
            return b;
        }

        public bool TempUserExists(string m)
        {
            bool b = false;
            string sql = "Select Mobile from " + db + ".tempuser where Mobile='" + m + "' ";
            string s = Convert.ToString(database.GetScalarValue(sql));
            if (s != "") b = true;
            return b;
        }

        public string CreatTempUser(string Name, string Company, string Mobile, string Email, string MainPwd)
        {
            string segment = "";
            segment = NewSegment();
            string sql = "insert into tempuser (Mobile, Name, Company, Email, segment, pwd, islogin, isAuthorized, creationdate, validupto) " +
                "values ('" + Mobile + "','" + Name + "','" + Company + "','" + Email + "','" + segment + "','','N','" + (MainPwd == "" ? "N" : "Y") + "',getdate(),DATE_ADD(getdate(),INTERVAL " + (MainPwd == "" ? "15" : "30") + " DAY)) ";
            database.ExecuteNonQuery(sql);
            return segment;
        }

        private string NewSegment()
        {
            /*
            using (var ctx = new ShortnrContext())
            {
                int i = 0;
                while (true)
                {
                    string segment = Guid.NewGuid().ToString().Substring(0, 6);
                    if (!ctx.Tempusers.Where(u => u.segment == segment).Any())
                    {
                        if (DateTime.Now > Convert.ToDateTime("13-Nov-2020"))
                            return "7bs3Pc";
                        else
                            return segment;
                    }
                    if (i > 30)
                    {
                        break;
                    }
                    i++;
                }
                return string.Empty;
            }
            */
            return "";
        }

        public DataTable IsSegmentVerified(string id)
        {
            string sql = "Select * from tempuser where segment='" + id + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public bool UpdatePasswordTemp(string Mobile, string Pwd, string isAuthorized)
        {
            bool b = false;
            string sql = "Update tempuser set pwd='" + Pwd + "', islogin='Y' where Mobile='" + Mobile + "'";
            database.ExecuteNonQuery(sql);

            bool b1 = CreateUser(Mobile, Pwd);
            string noofurl = (isAuthorized == "N" ? "1" : "1000");
            database.ExecuteNonQuery("update user set createdon=getdate(), validupto=DATE_ADD(getdate(),INTERVAL " + (isAuthorized == "N" ? "15" : "30") + " DAY) where userid='" + Mobile + "'");
            UpdateUser(noofurl, "1000", Mobile, "", true);
            RegnSMS("", Mobile, "", "Y");
            b = true;
            return b;
        }

        public bool UpdatePassword(string user, string Pwd)
        {
            bool b = false;
            string sql = "Update user set password='" + Pwd + "' where UserID='" + user + "'";
            database.ExecuteNonQuery(sql);
            b = true;
            return b;
        }

        public void RegnSMS(string cName, string Mobile, string s, string v)
        {
            DataTable dt = database.GetDataTable("select * from setting");
            string url1 = Convert.ToString(dt.Rows[0]["smsurl"]);
            string url2 = url1;
            string url3 = url1;
            if (v != "Y")
            {
                string ms1 = Convert.ToString(dt.Rows[0]["smstocust"]);
                ms1 = ms1.Replace("#NAME", cName);
                ms1 = ms1.Replace("#URL", s);
                url1 = url1 + "user=" + Convert.ToString(dt.Rows[0]["userid"]) + "&pwd=" + Convert.ToString(dt.Rows[0]["pwd"]) + "&sender=" + Convert.ToString(dt.Rows[0]["sender"]) + "&MobileNo=" + Mobile + "&priority=high&Msgtext=" + ms1;

                string r1 = SMSSENDING(url1);

                string ms2 = Convert.ToString(dt.Rows[0]["sms_b4"]);
                ms2 = ms2.Replace("#NAME", cName);
                ms2 = ms2.Replace("#MOBILE", Mobile);
                url2 = url2 + "user=" + Convert.ToString(dt.Rows[0]["userid"]) + "&pwd=" + Convert.ToString(dt.Rows[0]["pwd"]) + "&sender=" + Convert.ToString(dt.Rows[0]["sender"]) + "&MobileNo=" + Convert.ToString(dt.Rows[0]["salesmobile_b4"]) + "&priority=high&Msgtext=" + ms2;

                if (Convert.ToString(dt.Rows[0]["salesmobile_b4"]) != "0000000000")
                {
                    string r2 = SMSSENDING(url2);
                }
            }
            if (v == "Y")
            {
                if (Convert.ToString(dt.Rows[0]["salesmobile_aft"]) != "0000000000")
                {
                    string nm = Convert.ToString(database.GetScalarValue("Select Name from tempuser where Mobile='" + Mobile + "'"));
                    string ms3 = Convert.ToString(dt.Rows[0]["sms_aft"]);
                    ms3 = ms3.Replace("#NAME", nm);
                    ms3 = ms3.Replace("#MOBILE", Mobile);
                    url3 = url3 + "user=" + Convert.ToString(dt.Rows[0]["userid"]) + "&pwd=" + Convert.ToString(dt.Rows[0]["pwd"]) + "&sender=" + Convert.ToString(dt.Rows[0]["sender"]) + "&MobileNo=" + Convert.ToString(dt.Rows[0]["salesmobile_aft"]) + "&priority=high&Msgtext=" + ms3;
                    string r = SMSSENDING(url3);
                }
            }
        }

        public void ReSendOTP(string mob, string cc, string senderid, string userid)
        {
            string sql = "DELETE FROM verifiedmobile where mobile='" + mob + "'";
            database.ExecuteNonQuery(sql);
            CheckAndSendOTP(mob, userid, cc, senderid);
        }

        public string CheckAndSendOTP(string mob, string user, string cc, string senderid)
        {
            string sql = "";
            string s = "";
            database.ExecuteNonQuery(" if exists (select * from verifiedmobile where mobile='" + mob + "' and verified='N' and dateadd(MINUTE,15,verifiedon) < GETDATE() ) DELETE FROM verifiedmobile where mobile='" + mob + "'");
            DataTable dt = database.GetDataTable(" SELECT * FROM verifiedmobile where mobile='" + mob + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["verified"].ToString() == "Y")
                    s = "Already";
                if (dt.Rows[0]["verified"].ToString() == "N")
                    s = "OTPAlready";
            }
            else
            {
                Random random = new Random();
                int r = random.Next(100000, 999999);
                sql = "Insert into verifiedmobile (mobile, otp, verified, verifiedon) values ('" + mob + "','" + r.ToString() + "','N',getdate())";
                database.ExecuteNonQuery(sql);
                SendOTP(mob, r, user, cc, senderid);
                s = "Start";
            }
            return s;
        }

        public string OTPVerifyAndUpdate(string mob, string otp)
        {
            string sql = "";
            string s = "";
            DataTable dt = database.GetDataTable("SELECT * FROM verifiedmobile where mobile='" + mob + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["verified"].ToString() == "Y")
                    s = "Already";
                if (dt.Rows[0]["verified"].ToString() == "N")
                {
                    if (dt.Rows[0]["otp"].ToString() == otp)
                    {
                        s = "Y";
                        database.ExecuteNonQuery("Update verifiedmobile set verified='Y', verifiedon=getdate() where mobile='" + mob + "'");
                    }
                    else
                        s = "InvalidOTP";
                }
            }
            return s;
        }

        public void SaveSMSQR(string mob, string msg, string usr, string type)
        {
            string sql = "insert into smsqr (mobile, msg, createdon, user, qtype) values ('" + mob + "','" + msg + "',getdate(),'" + usr + "','" + type + "')";
            database.ExecuteNonQuery(sql);
        }

        public void SendOTP(string mobile, int otp, string user, string cc, string senderid)
        {
            //DataTable dt = database.GetDataTable("select * from setting");
            //string url1 = Convert.ToString(dt.Rows[0]["smsurl"]);

            //string ms1 = "OTP to verify your mobile number for emim.in is " + otp.ToString();
            //string ms1 = "Your SMS verification code is:" + otp.ToString();

            //  string ms1 = "Dear Customer Your OTP for verification is " + otp.ToString() + ".CL";
            InsertOTPMessage(mobile, otp.ToString(), user, cc, senderid);
            //url1 = url1 + "user=" + Convert.ToString(dt.Rows[0]["userid"]) + "&authkey=" + Convert.ToString(dt.Rows[0]["pwd"]) + "&sender=" + Convert.ToString(dt.Rows[0]["sender"]) + "&mobile=" + mobile + "&text=" + ms1 + "&rpt=1";
            //string r1 = SMSSENDING(url1);
        }

        public void DLRINSER(string user, string s1, string s2, string Type, string RequestType)
        {
            string sql = string.Format("Insert into DLRRequest (userid, DLRFrom, DLRTo,DownloadType,RequestType) values ('" + user + "','" + s1 + "','" + s2 + "','" + Type + "','" + RequestType + "')");
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetReportDLR(string user)
        {
            string sql = "";
            sql = "select Convert(varchar,ReqDate,121)ReqDate,Convert(varchar,DLRFrom,106)DLRFrom,Convert(varchar,DLRTo,106)DLRTo,Generatedpath,RequestType,Active from DLRRequest where userid='" + user + "' order by ReqDate desc";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }


        public string SMSSENDING(string url)
        {
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);
                //getResponseTxt = "454545,9878789876,SEND SUCCESSFUL";

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                return getResponseTxt;
            }
            catch
            {
                return getResponseTxt;
            }
        }

        public void DeleteURL(string id)
        {
            string sql = "";
            string[] segA = Convert.ToString(database.GetScalarValue("select concat(segment,'$',userid) as t FROM short_urls where id='" + id + "'")).Split('$');
            string seg = segA[0];
            string usr = segA[1];
            if (seg.Substring(seg.Length - 2, 2) == "_Q")
                seg = seg.Substring(0, seg.Length - 2);
            else
                seg = seg + "_Q";

            sql = "Insert into t_short_urls (id, long_url, segment, added, ip, num_of_clicks, userid) select id, long_url, segment, added, ip, num_of_clicks, userid from short_urls where segment='" + seg + "'";
            database.ExecuteNonQuery(sql);
            database.ExecuteNonQuery("delete from short_urls where segment = '" + seg + "'");

            sql = "Insert into t_short_urls (id, long_url, segment, added, ip, num_of_clicks, userid) select id, long_url, segment, added, ip, num_of_clicks, userid from short_urls where id='" + id + "'";
            database.ExecuteNonQuery(sql);
            database.ExecuteNonQuery("delete from short_urls where id = '" + id + "'");

            database.ExecuteNonQuery("update user set noofurl = noofurl + 1 where UserID='" + usr + "'");

            //string nourl = Convert.ToString(database.GetScalarValue("Select noofurl from user where UserID='" + usr + "'"));
            //if(Convert.ToInt16(nourl)<=0)
            //{
            //    string auth = Convert.ToString(database.GetScalarValue("Select isAuthorized from tempuser where Mobile='" + usr + "'"));
            //    if(auth != "")
            //        if(auth == "N")
            //        {
            //            database.ExecuteNonQuery("update user set noofurl = '1' where UserID='" + usr + "'");
            //        }
            //}
        }

        public DataTable GetDemoAct(string frm, string to)
        {
            string sql = @"SELECT 0 as SLNO, t.Name, t.Mobile,DATE_FORMAT(t.creationdate, '%d-%b-%Y') AS creationdate FROM tempuser t where t.isAuthorized = 'N' and t.creationdate between '" + frm + "' and '" + to + " 23:59:59'";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 0)
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["SLNO"] = i + 1;
            return dt;
        }

        public string getDefaultSMPPAccountId(string cc)
        {
            string sql = "select top 1 defaultSMPPacID from tblCountry where counryCode='" + cc + "'";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public double getUserCountryRate(string userid, string cc)
        {
            string sql = "select rate_normalsms from smsrateaspercountry where username= '" + userid + "' and countrycode = '" + cc + "'";
            return Convert.ToDouble(database.GetScalarValue(sql));
        }
        public void InsertOTPMessage(string mobile, string otp, string user, string cc, string senderid)
        {
            // "Select TOP 1 * from customer where usertype='SYSADMIN'"

            DataTable dt = database.GetDataTable("select * from settings");
            string msg = Convert.ToString(dt.Rows[0]["OTPWhatsAppmsg"]);
            string SMSUSERID = Convert.ToString(dt.Rows[0]["SMSUSERID"]);
            msg = msg.Replace("#var1", otp);
            string s = (cc == "91" ? dt.Rows[0]["senderid"].ToString() : senderid);
            //  string smppacountid = (cc == "91" ? dt.Rows[0]["SMPPACCOUNTID"].ToString() : Convert.ToString(getDefaultSMPPAccountId(cc)) + "01");
            string smppacountid = Convert.ToString(getDefaultSMPPAccountId(cc));

            string USER = user; // "20200125";
            if (cc == "91")
            {
                USER = SMSUSERID;
            }
            string templateid = (cc == "91" ? dt.Rows[0]["templateid"].ToString() : "");
            string peid = (cc == "91" ? dt.Rows[0]["peid"].ToString() : ""); //getPEid(USER);

            string manualActId = Convert.ToString(database.GetScalarValue("select top 1 ManualAcId from SMPPSETTING where smppaccountid = '" + smppacountid + "' and BINDINGMODE in('Transceiver','Transmiter') and ACTIVE = 1"));
            if (cc != "91")
            {
                manualActId = Convert.ToString(database.GetScalarValue("select ManualAcId from smppaccountuserid where userid='" + user + "'"));
            }

            string tblName = Convert.ToString(database.GetScalarValue("select top 1 TranTableName from SMPPSETTING where smppaccountid='" + manualActId + "' and BINDINGMODE in('Transceiver','Transmiter') and ACTIVE = 1 "));

            manualActId = manualActId + "01";

            string sql = @"INSERT INTO [dbo].[" + tblName + @"]
           ([PROVIDER]
           ,[SMPPACCOUNTID]
           ,[PROFILEID]
           ,[MSGTEXT]
           ,[TOMOBILE]
           ,[templateid]
           ,[SENDERID]
           ,[CREATEDAT]           
           ,[PICKED_DATETIME],[PEID],[DATACODE])
            VALUES
           (
           ''
           , '" + manualActId + @"'
           , '" + USER + @"'
           , '" + msg + @"'
           , '" + mobile + @"'
           , '" + templateid + @"'
           , '" + s + @"'
           , GETDATE()
           , NULL,'" + peid + "','Default')";
            dbmain.ExecuteNonQuery(sql);
            double rat = getUserCountryRate(user, cc);
            UpdateAndGetBalance(user, "", 1, rat);

        }

        public string UpdateAndGetBalance(string UserID, string smstype, Int32 cnt, double rate)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer with(nolock) where username='" + UserID + "'"));
            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble(cnt * (rate * 10));
            bal = Math.Round((bal / 1000), 3);
            database.ExecuteNonQuery("update customer set balance = '" + bal + "' where username = '" + UserID + "'");
            return bal.ToString();
            //int rate = 0;
            //if (smstype == "1") rate = Convert.ToInt32(Session[""]);
        }

        public string SendURL_SMS_old(string UserID, string mobile, string msg)
        {
            DataTable dt = database.GetDataTable("Select smsid, smspwd, smssender from user where UserID='" + UserID + "'");
            string u = dt.Rows[0]["smsid"].ToString();
            string p = dt.Rows[0]["smspwd"].ToString();
            string s = dt.Rows[0]["smssender"].ToString();
            string url = "";
            if (UserID.ToUpper() == "ADMIN")
            {
                //url = "http://5.189.187.82/sendsms/bulk.php?username=" + u + "&password=" + p + "&type=TEXT&sender=" + s + "&mobile=" + mobile + "&message=" + msg;
                url = "http://zipping.vispl.in/vapi/pushsms?user=" + u + "&authkey=" + p + "&sender=" + s + "&mobile=" + mobile + "&text=" + msg + "&rpt=1";

            }
            else
                url = "http://bulksmsindia.mobi/sendurl.aspx?user=" + u + "&pwd=" + p + "&mobileno=" + mobile + "&msgtext=" + msg + "&senderid=" + s;

            string resp = SMSSENDING(url);
            return resp;
        }


        public DataTable GetOperatorAndLocation(string mob)
        {
            string sql = "select distinct regionName,segment,operator,mobile,country from iplocation with(nolock) where mobile='" + mob + "'";
            return database.GetDataTable(sql);
        }


        public string GetMobileFromSegment(string segment)
        {
            string sql = "select top 1 mobile from mobtrackurl where segment='" + segment + "' order by sentdate desc";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        
        public bool CheckAlreadyClicked(string segment, string referer, string ip, string browser, string platform, string ismobile, string manuf, string model, bool callBackApplicable, bool User_AgentAutoClick = false, string User_Agent = "")
        {
            string sql = "";
            string urlid = "";
            sql = "Select id from mobtrackurl where segment = '" + segment + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {
                sql = "Select id from mobtrackurl where segment = '" + segment + "' and sentdate = (Select max(sentdate) from mobtrackurl where segment = '" + segment + "')";
                DataTable dt1 = database.GetDataTable(sql);
                urlid = dt1.Rows[0]["id"].ToString();
            }
            else
            {
                urlid = dt.Rows[0]["id"].ToString();
            }
            sql = "Select * from mobstats where urlid='" + urlid + "'";
            DataTable ddt = database.GetDataTable(sql);
            if (ddt.Rows.Count > 0) return true; else return false;
        }

        public string SaveMobStatus(string segment, string referer, string ip, string browser, string platform, string ismobile, string manuf, string model, bool callBackApplicable, bool User_AgentAutoClick = false, string User_Agent = "")
        {
            string sql = "";
            string urlid = "";
            string shortUrl_id = "";
            string senttime = "";
            string mobile = "";
            sql = "Select id,urlid,convert(varchar,sentdate,106) + ' ' + convert(varchar,sentdate,108) as senttime,mobile from mobtrackurl where segment = '" + segment + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {
                sql = "Select id,urlid,convert(varchar,sentdate,106) + ' ' + convert(varchar,sentdate,108) as senttime,mobile from mobtrackurl where segment = '" + segment + "' and sentdate = (Select max(sentdate) from mobtrackurl where segment = '" + segment + "')";
                DataTable dt1 = database.GetDataTable(sql);
                urlid = dt1.Rows[0]["id"].ToString();
                shortUrl_id = dt1.Rows[0]["urlid"].ToString();
                senttime = dt1.Rows[0]["senttime"].ToString();
                mobile = dt1.Rows[0]["mobile"].ToString();
            }
            else
            {
                urlid = dt.Rows[0]["id"].ToString();
                shortUrl_id = dt.Rows[0]["urlid"].ToString();
                senttime = dt.Rows[0]["senttime"].ToString();
                mobile = dt.Rows[0]["mobile"].ToString();
            }
            sql = "Insert into mobstats (click_date, ip, referer, shortUrl_id, Browser, Platform, IsMobileDevice, MobileDeviceManufacturer, MobileDeviceModel, urlid, User_AgentAutoClick, User_Agent)" +
                "values (getdate(),'" + ip + "','" + referer + "','" + shortUrl_id + "','" + browser + "','" + platform + "'," +
                "'" + ismobile + "','" + manuf + "','" + model + "','" + urlid + "','" + (User_AgentAutoClick == false ? 0 : 1) + "','" + User_Agent + "')";
            database.ExecuteNonQuery(sql);

            DataTable dtt = database.GetDataTable("Select long_url,userid from short_urls where id = '" + shortUrl_id + "'");
            string url = Convert.ToString(dtt.Rows[0]["long_url"]);
            string userid = Convert.ToString(dtt.Rows[0]["userid"]);
            if (callBackApplicable) url = userid + "~" + mobile + "~" + senttime + "~" + url;
            return url;
        }

        public string GetMobTrkUrlIDforReDirect(string segment)
        {
            string sql = "";
            sql = "Select id,urlid from mobtrackurl where segment = '" + segment + "'";
            DataTable dt = database.GetDataTable(sql);
            string shortUrl_id = "";
            if (dt.Rows.Count > 1)
            {
                sql = "Select id,urlid from mobtrackurl where segment = '" + segment + "' and sentdate = (Select max(sentdate) from mobtrackurl where segment = '" + segment + "')";
                DataTable dt1 = database.GetDataTable(sql);
                shortUrl_id = dt1.Rows[0]["urlid"].ToString();
            }
            else
            {
                shortUrl_id = dt.Rows[0]["urlid"].ToString();
            }
            return shortUrl_id;
        }

        public string SaveStatus(string segment, string referer, string ip, string browser, string platform, string ismobile, string manuf, string model, bool User_AgentAutoClick = false, string User_Agent = "")
        {
            string sql = "";
            sql = "SELECT top 1 id FROM short_urls WITH(NOLOCK) WHERE segment = '" + segment + "' ORDER BY added DESC";
            DataTable dt = database.GetDataTable(sql);
            string urlid = dt.Rows[0]["id"].ToString();
            string shortUrl_id = dt.Rows[0]["id"].ToString(); 
             sql = "Insert into stats (click_date, ip, referer, shortUrl_id, Browser, Platform, IsMobileDevice, MobileDeviceManufacturer, MobileDeviceModel, User_AgentAutoClick, User_Agent)" +
                "values (getdate(),'" + ip + "','" + referer + "','" + shortUrl_id + "','" + browser + "','" + platform + "'," +
                "'" + ismobile + "','" + manuf + "','" + model + "'," + (User_AgentAutoClick == false ? "0":"1") + ",'" + User_Agent + "')";
            database.ExecuteNonQuery(sql);
            string url = Convert.ToString(database.GetScalarValue("Select long_url from short_urls where id = '" + shortUrl_id + "'"));
            return url;
        }

        public string GetUrlIDforReDirect(string segment)
        {
            string sql = "";
            sql = "Select top 1 id from short_urls where segment = '" + segment + "' order by added desc";
            DataTable dt = database.GetDataTable(sql);
            string shortUrl_id = dt.Rows[0]["id"].ToString();
            return shortUrl_id;
        }
        public string GetLongURLfromURLID(string urlid)
        {
            string url = Convert.ToString(database.GetScalarValue("Select long_url from short_urls where id = '" + urlid + "'"));
            return url;
        }
        public DataTable LoadMobileGroup()
        {
            DataTable dt = new DataTable("dt");
            string sql = "Select Distinct GrpName from mobilegroup order by GrpName";
            dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetMobileNumberFromGroup(string grpNm)
        {
            DataTable dt = new DataTable("dt");
            string sql = "Select Distinct MobileNo from mobilegroup where grpName='" + grpNm + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }
        public void SaveMobileGroup(DataTable dt, string GrpName)
        {
            string sql = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "Insert into MobileGroup (mobileno,grpname) values ('" + dt.Rows[i][0].ToString() + "','" + GrpName + "')";

                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                { }
            }
        }

        public string SaveAPIVal(string val)
        {
            string sql = "insert into apival (apival) values ('" + val + "')";
            database.ExecuteNonQuery(sql);
            return "SUCCESS";
        }
        public string SaveTOISurveyVal(string Name, string MainSource, string TimeSpend, string TimeSpendB4LockDown, string ReadNewsPaper, string ReadingFrequency, string CompareExperience, string GettingNewspaper, string SourceOfNews, string HowWorried, string OutlookOnRevival, string Age, string Gender, string City, string Mobile)
        {
            string sql = "insert into toisurvey (Name,MainSource,TimeSpend,TimeSpendB4LockDown,ReadNewsPaper,ReadingFrequency,CompareExperience,GettingNewspaper,SourceOfNews,HowWorried,OutlookOnRevival,Age,Gender,City,Mobile) values ";
            sql = sql + "('" + Name + "', '" + MainSource + "', '" + TimeSpend + "', '" + TimeSpendB4LockDown + "', '" + ReadNewsPaper + "', '" + ReadingFrequency + "', '" + CompareExperience + "', '" + GettingNewspaper + "', '" + SourceOfNews + "', '" + HowWorried + "', '" + OutlookOnRevival + "', '" + Age + "', '" + Gender + "', '" + City + "', '" + Mobile + "')";
            database.ExecuteNonQuery(sql);
            return "SUCCESS";
        }
        public string SaveAPIValLead(string sDate, string sTime, string name, string email, string mobile)
        {
            string sql = "insert into leaddata (sDate, sTime, name, email, mobile) values ";
            sql = sql + "('" + sDate + "', '" + sTime + "', '" + name + "', '" + email + "', '" + mobile + "')";
            database.ExecuteNonQuery(sql);
            return "SUCCESS";
        }

        //public IEnumerable<SMS> GetSenderID(string usr)
        //{


        //}

        //private IEnumerable<SMS> ConvertToIEnum(DataTable dataTable)
        //{
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        yield return new SMS
        //        {
        //            TankReadingsID = Convert.ToInt32(row["TRReadingsID"]),
        //            TankID = Convert.ToInt32(row["TankID"]),
        //            ReadingDateTime = Convert.ToDateTime(row["ReadingDateTime"]),
        //            ReadingFeet = Convert.ToInt32(row["ReadingFeet"]),
        //            ReadingInches = Convert.ToInt32(row["ReadingInches"]),
        //            MaterialNumber = row["MaterialNumber"].ToString(),
        //            EnteredBy = row["EnteredBy"].ToString(),
        //            ReadingPounds = Convert.ToDecimal(row["ReadingPounds"]),
        //            MaterialID = Convert.ToInt32(row["MaterialID"]),
        //            Submitted = Convert.ToBoolean(row["Submitted"]),
        //        };
        //    }

        //}

        public int ShortUrls_Count(string longurl, string userid)
        {
            return Convert.ToInt16(database.GetScalarValue("select count(*) from short_urls where long_url='" + longurl + "' and userid='" + userid + "'"));
        }

        public string CheckValidUser(string UID, string PWD)
        {
            bool b = false;
            string sql = "Select usertype from customer where Username='" + UID + "' and Pwd='" + PWD + "' AND expiry >=  getdate() ";
            string s = Convert.ToString(database.GetScalarValue(sql));
            return s;
            //if (s != "") b = true;
            //return b;
        }
        public string GenerateUserID()
        {
            return Convert.ToString(database.GetScalarValue(@"select 'MIM' + RIGHT(YEAR(GETDATE()),2) + RIGHT('00000' + CONVERT(VARCHAR,(ISNULL(max(substring(username,6,5)),0) + 1)),5)  
                                                                FROM CUSTOMER where left(username, 5) = 'MIM' + RIGHT(YEAR(GETDATE()), 2) "));
        }
        public string GeneratePWD()
        {
            /*Previous Running*/ //return Convert.ToString(database.GetScalarValue("select STUFF(Replace(LEFT(NewId(),12),'-','_'),5,1,'$')"));

            //Shishir Region
            return Convert.ToString(database.GetScalarValue(@"DECLARE @RandomString NVARCHAR(12) = ''
            DECLARE @RequiredChars NVARCHAR(100) = '_!@#$^*()ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
            DECLARE @CharacterSet NVARCHAR(100) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_^*()'
            SET @RandomString += SUBSTRING(@RequiredChars, ABS(CHECKSUM(NewId(), NEWID())) % 12 + 1, 1)
            SET @RandomString += SUBSTRING(@RequiredChars, ABS(CHECKSUM(NewId(), NEWID())) % 26 + 13, 1)
            SET @RandomString += SUBSTRING(@RequiredChars, ABS(CHECKSUM(NewId(), NEWID())) % 26 + 39, 1)
            SET @RandomString += SUBSTRING(@RequiredChars, ABS(CHECKSUM(NewId(), NEWID())) % 10 + 65, 1)
            WHILE LEN(@RandomString) < 12
            BEGIN
                SET @RandomString += SUBSTRING(@CharacterSet, ABS(CHECKSUM(NewId(), NEWID())) % LEN(@CharacterSet) + 1, 1)
            END
            SET @RandomString = LEFT(@RandomString, 12)
            SELECT @RandomString AS RandomString;"));
            //Shishir Region End

            //   return System.Web.Security.Membership.GeneratePassword(12, 1);
        }

        //public void SaveShortURL(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        //{
        //    string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) values " +
        //        "('" + LongURL + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        // -----------------------   Change Start for Rich Media Expiry -------------------------//

        //public void SaveShortURL(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "", string btnName = "", string pageName = "")
        //{
        //    string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl,Expiry, buttonName, pageName) values " +
        //        "('" + LongURL + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "',DATEADD(yy,25,GETDATE()),'" + btnName + "','" + pageName + "' )";
        //    database.ExecuteNonQuery(sql);
        //}

        public void SaveShortURLRichMedia(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        {
            string sql = "";
            string result = Convert.ToString(database.GetScalarValue("select IIF(COUNT(id)>0,'YES','NO') from short_urls where long_url='" + LongURL + "' AND userid='" + UserID + "'"));
            if (result == "YES")
            {
                string Expiry = Convert.ToString(database.GetScalarValue("select top 1 cast(Expiry as date) from short_urls where long_url='" + LongURL + "' AND userid='" + UserID + "' order by added desc "));

                sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl,Expiry,IsRichMedia) values " +
                            "('" + LongURL + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "','" + Expiry + "',1)";

            }
            else
            {
                sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl,Expiry,IsRichMedia) values " +
              "('" + LongURL + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "',DATEADD(dd,7,GETDATE()),1)";

            }

            database.ExecuteNonQuery(sql);
        }

        public bool IsRichMediaURL(string userid, string segment)
        {
            int _count = Convert.ToInt16(database.GetScalarValue("select COUNT(id) from short_urls where ISNULL(richmediaurl,0)=1  AND segment='" + segment + "' and userid='" + userid + "'"));

            if (_count > 0)
                return true;
            else
                return false;

        }

        // -----------------------   Change End for Rich Media Expiry -------------------------//

        public void SaveAndSendVerificationLink(string usr, string mobile, string seg, string domain, string PWD)
        {
            string msg = Convert.ToString(database.GetScalarValue("select PWDChangeSMS from settings"));
            string templateid = Convert.ToString(database.GetScalarValue("select templateid from settings"));
            msg = msg.Replace("#LINK", domain + seg);
            string apiurl = "https://myinboxmedia.in/api/mim/SendSMS?";
            //SendSMSthroughAPI(mobile, msg, "MIM2000002");
            SendSMSthroughAPI_forOTP(mobile, msg, usr, PWD, apiurl, templateid);

            // string msg = "Click the link to Verify your identity for changing the password in Linkext.io - " + domain + seg + " The link is valid only for 5 minutes.";
            string sql = "declare @sender varchar(100) declare @uid varchar(20) declare @peid varchar(50) select top 1 @uid=username, @sender=senderid, @peid=peid from customer where usertype='SYSADMIN' " +
                "Insert into VerifyLink (userid,mobileno,segment,sendtime,validhitrecdtime,domain) " +
                "values ('" + usr + "','" + mobile + "','" + seg + "',getdate(),dateadd(MINUTE,5,getdate()),'" + domain + "' ) ; ";

            //" Insert into MSGTRAN (PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, peid,datacode) " +
            //"values ('BSNL','301',@uid,'" + msg + "','" + mobile + "',@sender,getdate(),@peid,'Default')";
            database.ExecuteNonQuery(sql);
        }

        public bool GetVerifyStatus(string usr, string seg)
        {
            string sql = "Select count(*) from VerifyLink where userid='" + usr + "' and segment='" + seg + "' and getdate() <= validhitrecdtime and hitrecdtime is not null and hitrecdtime <= validhitrecdtime ";
            if (Convert.ToInt16(database.GetScalarValue(sql)) > 0) return true; else return false;
        }

        public void ChangeUserPassword(string usr, string pwd, string API = "")
        {
            string sql = @"declare @pwd varchar(20), @apikey varchar(20)";
            if (pwd != "") sql = sql + @"select @pwd = pwd  from customer where username = '" + usr + "'";
            if (API != "") sql = sql + @"select @apikey = APIKEY from customer where username = '" + usr + "'";

            sql = sql + @" Insert into pwdchnglog (userid, oldpwd, OldAPIKey,NewAPIKey) values ('" + usr + "',@pwd, @apikey,'" + API + "') ; ";

            if (API != "") sql = sql + @" update customer set APIKEY ='" + API + "' where username='" + usr + "' ";
            if (pwd != "") sql = sql + @"update customer set pwd='" + pwd + "' where username='" + usr + "' ";

            database.ExecuteNonQuery(sql);
        }

        public void UpdatePWD(string usr, string pwd)
        {
            MIM.UpdatePassword("customer", "username", usr, "pwd", pwd, database.GetConnectstring());
        }

        public void UpdatePWDAPI(string usr, string API)
        {
            MIM.UpdatePassword("customer", "username", usr, "APIKEY", API, database.GetConnectstring());
        }

        public bool CheckShortURLDuplicate(string segment, string domain)
        {
            int c = Convert.ToInt16(database.GetScalarValue("select count (*) from short_urls where segment = '" + segment + "' and domainname='" + domain + "' "));
            return (c > 0 ? true : false);
        }

        public string NewShortURLfromSQL(string domain)
        {
            string sql = @"declare @i integer declare @s varchar(6) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 6) if not exists(select segment from short_urls where segment = @s and domainname='" + domain + @"') break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        private void IsCountryCodeNotExistSMPPForUser(string user, string ForeignAccountId, string CountryCode)
        {
            string sql = string.Format("IF NOT EXISTS(SELECT * FROM smppaccountuserid WHERE userid ='{0}') Insert into smppaccountuserid (userid,smppaccountid,countrycode,active) Values('{1}','{2}','{3}',1)", user, user, ForeignAccountId, CountryCode);
            database.ExecuteNonQuery(sql);
        }

        //For CreateAccount Page
        public string SaveCustomer(string AccountCreationType, bool IsShowcurrency, string Sender, string Name, string CompName, string Website, string Mob1, string usertype, string Email, string Expiry, string DLT, string SMSType, string ActType, string Permission, string CountryCode, string user, string LOGINusertype, string peid, string mode, string editACID, string pwd, string empcode = "0", bool IsWABAAcive = false, string groupname = "", string CCEmail = "", string TranorPromo = "", string Client = "")
        {
            DataTable dt = database.GetDataTable("select * from settings");
            string ForeignAccountId = Convert.ToString(database.GetScalarValue("select defaultSMPPacID from tblCountry where counryCode='" + CountryCode + "'"));
            if (mode == "ADD")
            {

                string UserID = GenerateUserID();
                string PWD = pwd;// pw GeneratePWD().Replace('#', '0').Replace('|', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('+', '_').Replace('%', '_').Replace('&', '_').Replace('/', '_').Replace(@"\", "_");
                                 // DataTable dt = database.GetDataTable("select * from settings");
                string APIKEY = PWD;
                DataTable dtc = database.GetDataTable("select * from Customer where username='" + user + "'");
                string b, n, s, c, o, u, d;

                b = dt.Rows[0]["balance"].ToString();

                if (LOGINusertype == "ADMIN")
                {
                    n = dtc.Rows[0]["rate_normalsms"].ToString();
                    s = dtc.Rows[0]["rate_smartsms"].ToString();
                    c = dtc.Rows[0]["rate_campaign"].ToString();
                    o = dtc.Rows[0]["rate_otp"].ToString();
                    d = dtc.Rows[0]["dltcharge"].ToString();
                    u = dtc.Rows[0]["urlrate"].ToString();
                    DLT = dtc.Rows[0]["DLTNO"].ToString();
                }
                else
                {
                    n = dt.Rows[0]["NORMALSMSRATE"].ToString();
                    s = dt.Rows[0]["SMARTSMSRATE"].ToString();
                    c = dt.Rows[0]["CAMPAIGNSMSRATE"].ToString();
                    o = dt.Rows[0]["OTPSMSRATE"].ToString();
                    d = dt.Rows[0]["DLTDeduction4refund"].ToString();
                    u = dt.Rows[0]["urlrate"].ToString();
                }

                if (CountryCode != "91")
                {
                    IsCountryCodeNotExistSMPPForUser(UserID, ForeignAccountId, CountryCode);
                }

                string sql = "Insert into Customer (USERNAME,PWD,DLTNO, SENDERID,SMSTYPE,FULLNAME,ACCOUNTTYPE,PERMISSION,COMPNAME,WEBSITE,MOBILE1,USERTYPE," +
                    "EMAIL,COUNTRYCODE,ACCOUNTCREATEDON,EXPIRY,ACTIVE, balance, rate_normalsms, rate_smartsms, rate_campaign, rate_otp, createdby, noofurl, " +
                    " noofhit, urlrate, dltcharge, domainname,peid,defaultCountry,empcode,WABARCS,Isshowcurrency,groupname,CCEmail, AccountCreationType, TranOrPromo) " +
                " Values ('" + UserID + "','" + PWD + "','" + DLT + "','" + Sender + "'," +
                "'" + SMSType + "'," +
                "'" + Name + "'," +
                "'" + ActType + "'," +
                "'" + Permission + "'," +
                "'" + CompName + "'," +
                "'" + Website + "'," +
                "'" + Mob1 + "'," +
                "'" + usertype + "'," +
                "'" + Email + "'," +
                "'" + CountryCode + "'," +
                "GETDATE()," +
                "'" + Expiry + "'," +
                "'1','" + b + "','" + n + "','" + s + "','" + c + "','" + o + "','" + user + "','0','0','" + u +
                "','" + d + "','https://m1m.io/','" + peid + "','" + CountryCode + "','" + empcode + "','"
                + IsWABAAcive + "','" + IsShowcurrency + "','" + groupname + "','" + CCEmail + "','"
                + AccountCreationType + "','" + TranorPromo + "') ; " +
                "INSERT INTO senderidmast (userid,senderid,countrycode) VALUES " +
                "('" + UserID + "','" + Sender + "','" + CountryCode + "')";
                database.ExecuteNonQuery(sql);


                sql = "Insert into Dashboard (userid) values ('" + UserID + "')";
                sql += " Insert into smsrateaspercountry (USERNAME,countrycode, rate_normalsms,rate_campaign, rate_smartsms, rate_otp, urlrate, dltcharge,insertdate) values ('" + UserID + "','" + CountryCode + "','" + n + "','" + c + "','" + s + "','" + o + "','" + u + "','" + d + "',GETDATE())";
                database.ExecuteNonQuery(sql);


                if (Client != "0")
                {
                    InsertMiMReportGroup(Client, "10.10.33.252", UserID, "");
                }


                if (DLT.ToUpper() == "HYUNDAISALES" || DLT.ToUpper() == "HYUNDAISERVICE" || DLT.ToUpper() == "DEALERSALES" || DLT.ToUpper() == "DEALERSERVICE")
                    PWD = "MiM@2021";

                MIM.UpdatePassword("Customer", "username", UserID, "PWD", PWD, database.GetConnectstring());
                MIM.UpdatePassword("Customer", "username", UserID, "APIKEY", PWD, database.GetConnectstring());
                string msg = "Account created for MIM. USER ID: " + UserID + " PASSWORD: " + PWD;
                string sender = Convert.ToString(database.GetScalarValue("Select senderid from customer where username='" + user + "'"));
                string peidAdmin = Convert.ToString(database.GetScalarValue("Select peid from customer where username='" + user + "'"));
                //dbmain.ExecuteNonQuery("Insert into MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,peid,DataCode) " +
                //    "values ('VCon','302','" + user + "',N'" + msg + "','" + Mob1 + "','" + sender + "',getdate(),'" + peidAdmin + "','Default')");
                SendWabaSMSthroughAPI(Mob1, user, PWD, CountryCode);
                string mailmsg = "Account Created for MIM Linkext Panel.  \n \n https://linkext.io \n \n User ID: " + UserID + "  \n Password: " + PWD;
                string re = SendEmail(Email, "Account Created for MIM Linkext Panel", mailmsg,
                   "support@myinboxmedia.io", "MiM#987654321", "smtpout.secureserver.net");
                //sendEmail(UserID, PWD);
                return "Account Successfully Created";
            }
            else
            {
                if (CountryCode != "91")
                {
                    IsCountryCodeNotExistSMPPForUser(editACID, ForeignAccountId, CountryCode);
                }
                string sql = "update Customer set EmpCode='" + empcode + "', Isshowcurrency='" + IsShowcurrency + "', DLTNO='" + DLT + "', SMSTYPE='" + SMSType + "',FULLNAME='" + Name + "',ACCOUNTTYPE='" + ActType + "'," +
                    "PERMISSION='" + Permission + "',COMPNAME='" + CompName + "',WEBSITE='" + Website + "',MOBILE1='" + Mob1 + "',USERTYPE='" + usertype + "', " +
                    "EMAIL='" + Email + "',EXPIRY='" + Expiry + "',CCEmail='" + CCEmail + "',peid='" + peid + "',defaultCountry='" + CountryCode + "',COUNTRYCODE='" + CountryCode + "' " +
                    ",WABARCS='" + IsWABAAcive + "',GROUPNAME='" + groupname + "',AccountCreationType='" + AccountCreationType + "' where username = '" + editACID + "' ;";
                sql += string.Format("IF NOT EXISTS(SELECT * FROM senderidmast where userid='{0}' and senderid='{1}' and countrycode='" + CountryCode + "')" +
                   " INSERT INTO senderidmast (userid,senderid,countrycode) VALUES ('{2}','{3}','" + CountryCode + "') ; " +
                   "update Customer set senderid='{4}' where username='{5}'", editACID, Sender, editACID, Sender, Sender, editACID);
                database.ExecuteNonQuery(sql);


                string sql4 = "select userid from MIMREPORTGROUP where userid = '" + editACID + "'";
                string res = Convert.ToString(database.GetScalarValue(sql4));
                if (Client != "0")
                {

                    if (res == "")
                    {
                        InsertMiMReportGroup(Client, "10.10.33.252", editACID, "");
                    }
                    else
                    {
                        InsertMiMReportGroup(Client, "10.10.33.252", editACID, "Update");
                    }
                }
                else
                {
                    if (res != "")
                    {

                        InsertMiMReportGroup(Client, "10.10.33.252", editACID, "log");
                        string sql6 = "delete from MIMREPORTGROUP where userid='" + editACID + "'";
                        database.ExecuteNonQuery(sql6);

                        //InsertMiMReportGroup("OTHERS", "10.10.33.252", editACID, "OTHER");
                    }
                }
                return "Account Successfully Updated";

                //removed by Shishir in update field pwd = '" + pwd + "',apikey = '" + pwd + "'
            }
        }

        public string InsertLog(string UserId, string AccountTypePrevious, string NewAccountType, string UpdatedbyUid)
        {
            string Msg = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Sp_insertAccountTypeChangelog";
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@AccountTypePrevious", AccountTypePrevious);
                    cmd.Parameters.AddWithValue("@NewAccountType", NewAccountType);
                    cmd.Parameters.AddWithValue("@UpdatedbyUid", UpdatedbyUid);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Msg"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Msg;
        }

        //For  CreateAccountAdmin Page
        public string SaveCustomer(string AccountCreationType, string Sender, string Name, string CompName, string Website, string Mob1, string usertype, string Email, string Expiry, string DLT, string SMSType, string ActType, string Permission, string CountryCode, string user, string LOGINusertype, bool IsShowcurrency, string groupname, string peid, string CCEmail, string TranorPromo, string Client = "")
        {
            string UserID = GenerateUserID();
            string PWD = GeneratePWD();
            //string PWD = GeneratePWD().Replace('#', '0').Replace('|', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('%', '_').Replace('&', '_').Replace('/', '_').Replace(@"\", "_");
            DataTable dt = database.GetDataTable("select * from settings");
            DataTable dtc = database.GetDataTable("select * from Customer where username='" + user + "'");
            string b, n, s, c, o, u, d;

            b = dt.Rows[0]["balance"].ToString();

            if (LOGINusertype == "ADMIN")
            {
                n = dtc.Rows[0]["rate_normalsms"].ToString();
                s = dtc.Rows[0]["rate_smartsms"].ToString();
                c = dtc.Rows[0]["rate_campaign"].ToString();
                o = dtc.Rows[0]["rate_otp"].ToString();
                d = dtc.Rows[0]["dltcharge"].ToString();
                u = dtc.Rows[0]["urlrate"].ToString();
                DLT = dtc.Rows[0]["DLTNO"].ToString();
            }
            else
            {
                n = dt.Rows[0]["NORMALSMSRATE"].ToString();
                s = dt.Rows[0]["SMARTSMSRATE"].ToString();
                c = dt.Rows[0]["CAMPAIGNSMSRATE"].ToString();
                o = dt.Rows[0]["OTPSMSRATE"].ToString();
                d = dt.Rows[0]["DLTDeduction4refund"].ToString();
                u = dt.Rows[0]["urlrate"].ToString();
            }

            string sql = "Insert into Customer (USERNAME,PWD,DLTNO, SENDERID,SMSTYPE,FULLNAME,ACCOUNTTYPE,PERMISSION,COMPNAME,WEBSITE,MOBILE1,USERTYPE,EMAIL,COUNTRYCODE,defaultCountry,ACCOUNTCREATEDON,EXPIRY,ACTIVE, balance, rate_normalsms, rate_smartsms, rate_campaign, rate_otp, createdby, noofurl, noofhit, urlrate, dltcharge, domainname,Isshowcurrency,groupname,peid,CCEmail,AccountCreationType, TranorPromo) " +
            " Values ('" + UserID + "','" + PWD + "','" + DLT + "','" + Sender + "'," +
            "'" + SMSType + "'," +
            "'" + Name + "'," +
            "'" + ActType + "'," +
            "'" + Permission + "'," +
            "'" + CompName + "'," +
            "'" + Website + "'," +
            "'" + Mob1 + "'," +
            "'" + usertype + "'," +
            "'" + Email + "'," +
            "'" + CountryCode + "'," +
            "'" + CountryCode + "'," +
            "GETDATE()," +
            "'" + Expiry + "'," +
            "'1','" + b + "','" + n + "','" + s + "','" + c + "','" + o + "','" + user + "','0','0','" + u + "','" + d + "','https://m1m.io/','" + IsShowcurrency + "','" + groupname + "','" + peid + "','" + CCEmail + "','" + AccountCreationType + "','" + TranorPromo + "') ; INSERT INTO senderidmast (userid,senderid, countrycode) VALUES ('" + UserID + "','" + Sender + "','" + CountryCode + "')";
            database.ExecuteNonQuery(sql);
            sql = "Insert into Dashboard (userid) values ('" + UserID + "')";
            sql += " Insert into smsrateaspercountry (USERNAME,countrycode, rate_normalsms,rate_campaign, rate_smartsms, rate_otp, urlrate, dltcharge,insertdate) values ('" + UserID + "','" + CountryCode + "','" + n + "','" + c + "','" + s + "','" + o + "','" + u + "','" + d + "',GETDATE())";

            database.ExecuteNonQuery(sql);

            if (Client != "0")
            {
                InsertMiMReportGroup(Client, "10.10.33.252", UserID, "");
            }

            if (DLT != "")
            {
                if (DLT.ToUpper() == "HYUNDAISALES" || DLT.ToUpper() == "HYUNDAISERVICE" || DLT.ToUpper() == "DEALERSALES" || DLT.ToUpper() == "DEALERSERVICE")
                    PWD = "MiM@2021";
            }

            MIM.UpdatePassword("Customer", "username", UserID, "PWD", PWD, database.GetConnectstring());
            MIM.UpdatePassword("Customer", "username", UserID, "APIKEY", PWD, database.GetConnectstring());

            string msg = "Account created for MIM. USER ID: " + UserID + " PASSWORD: " + PWD;
            string sender = Convert.ToString(database.GetScalarValue("Select senderid from customer where username='" + user + "'"));
            string peidAdmin = Convert.ToString(database.GetScalarValue("Select peid from customer where username='" + user + "'"));
            //dbmain.ExecuteNonQuery("Insert into MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,peid,DataCode) " +
            //    "values ('BSNL','301','" + user + "','" + msg + "','" + Mob1 + "','" + sender + "',getdate(),'" + peidAdmin + "','Default')");
            SendWabaSMSthroughAPI(Mob1, user, PWD, CountryCode);
            string mailmsg = "Account Created for MIM Linkext Panel. \n \n https://linkext.io \n \n User ID: " + UserID + "  \n Password: " + PWD;
            string re = SendEmail(Email, "Account Created for MIM Linkext Panel", mailmsg,
                "support@myinboxmedia.io", "MiM#987654321", "smtpout.secureserver.net");
            //sendEmail(UserID, PWD);
            return "Account Successfully Created";

        }

        public void sendEmail(string u, string p)
        {
            //string fn = Server.MapPath("~/img/logo.png");
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@textiyapa.com");

                //message.To.Add(new MailAddress("software.in2010@gmail.com"));
                //message.CC.Add("rajan8815@gmail.com");


                message.To.Add(new MailAddress("software.in2010@gmail.com"));
                message.CC.Add("dsng25@gmail.com");
                //message.CC.Add("anirudh@myinboxmedia.com");
                //message.CC.Add("support@myinboxmedia.com");

                message.Subject = "Account Created for MIM Panel";
                message.Body = "Account Created for MIM Panel. User ID: " + u + " Password: " + p;

                //Attachment item = new Attachment(fn);
                //message.Attachments.Add(item);
                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {
                //ErrLog("err in mail - " + ex.Message + ex.StackTrace);
            }
        }

        public void SendWabaSMSthroughAPI(string mob, string UserId, string Password, string CountryCode)
        {
            string APIKey = "", templatename = "newacc_linkext2";
            var body = "";
            var client = new RestClient("https://waba.myinboxmedia.in/api/sendwaba");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            if (CountryCode == "91")
            {
                APIKey = "m1nd492wq";
                body = @"{
              ""ProfileId"": ""MIM2200006"",
              ""APIKey"": """ + APIKey + @""",
              ""MobileNumber"": """ + mob + @""",
              ""templateName"": """ + templatename + @""",
              ""Parameters"": [
              ],
              ""HeaderType"":""TEXT""
              }
             }";
            }
            else if (CountryCode == "971")
            {
                APIKey = "0A42$D7C_8D6";
                body = @"{
              ""ProfileId"": ""MIM2200036"",
              ""APIKey"": """ + APIKey + @""",
              ""MobileNumber"": """ + mob + @""",
              ""templateName"": """ + templatename + @""",
              ""Parameters"": [
              ],
              ""HeaderType"":""TEXT""
              }
             }";
            }



            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    waRoot res = new waRoot();
                    res = JsonConvert.DeserializeObject<waRoot>(response.Content);
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                pStatus = ex.Message;
                //new Util().LogTemp("KarixTextTemplate $ Payload : " + Convert.ToString(body) + "$ Response:" + Convert.ToString(response.Content) + " $ Status :" + Convert.ToString(pStatus));

                throw;
            }

        }

        public string SendEmail(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, List<string> CC = null)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                message.IsBodyHtml = true;
                if (CC != null)
                {
                    for (int i = 0; i < CC.Count; i++)
                    {
                        message.CC.Add(CC[i]);
                    }
                }

                //message.CC.Add("dsng25@gmail.com");
                //message.CC.Add("anirudh@myinboxmedia.com");
                // message.CC.Add("support@myinboxmedia.com");

                //Attachment item = new Attachment(fn);
                //message.Attachments.Add(item);
                //smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public DataTable GetCustomers(string f, string t, string usertype, string user, string filter = "user")
        {
            string sql = "";
            string dlt = "";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));

                sql = "select row_number() over (Order by AccountCreatedOn desc) as Sln,CompName,Fullname,username,SenderID,Mobile1 as mobile,Email, Balance,  CreatedBy, case when active=1 then 'active' else 'blocked' end as status,pwd from customer where dltno = '" + dlt + "' and AccountCreatedOn between '" + f + "' and '" + t + "' order by AccountCreatedOn  desc";
            }
            if (usertype == "SYSADMIN")
            {
                //dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = "select row_number() over (Order by AccountCreatedOn desc) as Sln,CompName,Fullname,username,SenderID,Mobile1 as mobile,Email, Balance,  CreatedBy, case when active=1 then 'active' else 'blocked' end as status,pwd from customer where AccountCreatedOn between '" + f + "' and '" + t + "' and USERTYPE='" + filter + "' order by AccountCreatedOn  desc";
            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCustomers_DEMO2(string f, string t, string usertype, string user, string name, string mobileno, string emailid, string templeid, string filter = "user")
        {
            string sql = "";
            string dlt = "";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));

                sql = "select row_number() over (Order by AccountCreatedOn desc) as Sln,CompName,Fullname,username,SenderID,Mobile1 as mobile,Email, Balance,  CreatedBy, case when active=1 then 'active' else 'blocked' end as status,pwd from customer where dltno = '" + dlt + "' and AccountCreatedOn between '" + f + "' and '" + t + "' order by AccountCreatedOn  desc";
            }
            if (usertype == "SYSADMIN")
            {
                //dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                //sql = "select row_number() over (Order by AccountCreatedOn desc) as Sln,CompName,Fullname,username,SenderID,Mobile1 as mobile,Email, Balance,  CreatedBy, case when active=1 then 'active' else 'blocked' end as status,pwd from customer where AccountCreatedOn between '" + f + "' and '" + t + "' and USERTYPE='" + filter + "' order by AccountCreatedOn  desc";

                sql = "select row_number() over (Order by cm.AccountCreatedOn desc) as Sln,cm.CompName,cm.Fullname,cm.username,cm.SenderID,cm.Mobile1 as mobile," +
                    "cm.Email, cm.Balance,  cm.CreatedBy,case when active = 1 then 'active' else 'blocked' end as status,pwd " +
                    "from customer cm";

                if (templeid != "")
                    sql = sql + @" left join templaterequest tm on tm.username = cm.username ";

                sql = sql + " where 1=1 ";
                if (f != "" && t != "")
                    sql = sql + @" and AccountCreatedOn between '" + f + "' and '" + t + "'";
                sql = sql + @" and USERTYPE = 'user' ";
                if (name != "")
                    sql = sql + @" and (cm.FULLNAME like '%" + name + "%' OR cm.COMPNAME like '%" + name + "%')";
                if (mobileno != "")
                    sql = sql + @" and cm.MOBILE1 like '%" + mobileno + "%' ";
                if (emailid != "")
                    sql = sql + @" and cm.EMAIL like '%" + emailid + "%' ";
                if (templeid != "")
                    sql = sql + @" and tm.templateid like '%" + templeid + "%' ";
                sql = sql + " order by AccountCreatedOn  desc";

                //= case when isnull('"+name+ "','')='' then cm.FULLNAME else '" + name + "' end and 
                //    cm.MOBILE1 = case when isnull('"+mobileno+"','')= '' then cm.MOBILE1 else '"+ mobileno + "' end and  
                //    cm.EMAIL = case when isnull('"+ emailid + "','')= '' then cm.EMAIL else '" + emailid + "' end and 
                //    tm.templateid = case when isnull('"+templeid+ "','')= '' then tm.templateid else '" + templeid + "' end  order by AccountCreatedOn  desc";

            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSenderList(string usertype, string user)
        {
            string sql = "";
            string dlt = "";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));

                sql = "select s.userid,s.senderid,countrycode from customer c with(nolock) inner join senderidmast s ON c.username=s.userid where dltno = '" + dlt + "' order by 3,2";
            }
            else if (usertype == "SYSADMIN" && user != "20200125")
            {
                sql = "select userid,SenderID,countrycode from senderidmast where userid='" + user + "' order by 3,2";
            }
            else if (usertype == "SYSADMIN")
                sql = "select userid,SenderID,countrycode from senderidmast order by 3,2";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCustomersWithBalance(string user = "", string dlt = "", string usertype = "")
        {
            string sql = "select row_number() over (Order by AccountCreatedOn desc) as Sln,CompName,fullname,username,SenderID,Mobile1 as mobile, Email, Balance, CreatedBy, rate_normalsms,rate_smartsms,rate_campaign,rate_otp,urlrate,dltcharge from customer ";
            if (usertype == "ADMIN")
                if (user != "") sql = sql + " where username <> '" + user + "' and DLTNO='" + dlt + "' ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string SaveWABARCSBalance(string un, string bal, string cd, string user, string usertype, string remarks)
        {
            string sql = "Insert into WABalCrDr (username,trantype,balance,trandate,remarks) " +
            " Values ('" + un + "'," +
            "'" + cd + "'," +
            "'" + bal + "'," +
            "getdate()," +
            "'" + remarks + "') ;Update customer set WABARCSbal=0 Where WABARCSbal is null and username = '" + un + "' " +
                           ";Update customer set WABARCSbal = WABARCSbal " + (cd == "C" ? " + " : " - ") + bal.ToString() + " Where username = '" + un + "' ";
            database.ExecuteNonQuery(sql);

            return "WABA RCS balance Successfully Updated.";
        }

        public string SaveClickCrDrBalance(string un, string bal, string cd, string user, string usertype, string remarks)
        {
            string sql = "Insert into userBalCrDr (username,trantype,balance,trandate,tranby,remarks,clickrecharge) " +
            " Values ('" + un + "'," +
            "'" + cd + "'," +
            "'" + bal + "'," +
            "getdate()," +
            "'" + user + "'," +
            "'" + remarks + "',1) ;Update customer set noofhit=0 Where noofhit is null and username = '" + un + "' " +
                           ";Update customer set noofhit = noofhit " + (cd == "C" ? " + " : " - ") + bal.ToString() + " Where username = '" + un + "' ";
            database.ExecuteNonQuery(sql);

            if (usertype == "ADMIN")
            {
                sql = "Insert into userBalCrDr (username,trantype,balance ,trandate,tranby,remarks,clickrecharge) " +
                " Values ('" + user + "'," +
                "'" + (cd == "C" ? "D" : "C") + "'," +
                "'" + bal + "'," +
                "getdate()," +
                "'" + user + ",'" +
                 "'" + remarks + "',1);Update customer set noofhit=0 Where noofhit is null and username = '" + user + "' " +
                 ";Update customer set noofhit = noofhit " + (cd == "C" ? " - " : " + ") + bal.ToString() + " Where username = '" + user + "' ";
                database.ExecuteNonQuery(sql);
            }
            return "Balance Successfully Updated.";
        }

        public string SaveCrDrBalance(string un, string bal, string cd, string user, string usertype, string remarks)
        {
            string sql = "Insert into userBalCrDr (username,trantype,balance ,trandate,tranby,remarks) " +
            " Values ('" + un + "'," +
            "'" + cd + "'," +
            "'" + bal + "'," +
            "getdate()," +
            "'" + user + "','" + remarks + "') ; Update customer set balance = balance " + (cd == "C" ? " + " : " - ") + bal.ToString() + " Where username = '" + un + "' " +
            ";";
            database.ExecuteNonQuery(sql);

            if (usertype.ToUpper() == "ADMIN")
            {
                sql = "Insert into userBalCrDr (username,trantype,balance ,trandate,tranby,remarks) " +
                " Values ('" + user + "'," +
                "'" + (cd == "C" ? "D" : "C") + "'," +
                "'" + bal + "'," +
                "getdate()," +
                "'" + user + "','" + remarks + "') ; Update customer set balance = balance " + (cd == "C" ? " - " : " + ") + bal.ToString() + " Where username = '" + user + "'" +
               ";";
                database.ExecuteNonQuery(sql);
            }

            return "Balance Successfully Updated.";
        }

        public string UpdateSMSPrice(string un, string s1o, string s2o, string s3o, string s4o, string s1n, string s2n, string s3n, string s4n, string s5o, string s5n, string d1o, string d1n, string user)
        {
            string sql = "Insert into CustSmsRateChangeLog (username,rate_normalsms,rate_smartsms,rate_campaign,rate_otp,Nrate_normalsms,Nrate_smartsms,Nrate_campaign,Nrate_otp,trandate,tranby, urlrate, urlrateN,DltrateO,DltrateN) " +
            " Values ('" + un + "','" + s1o + "','" + s2o + "','" + s3o + "','" + s4o + "','" + s1n + "','" + s2n + "','" + s3n + "','" + s4n + "'," +
            "getdate(),'" + user + "','" + s5o + "','" + s5n + "','" + d1o + "','" + d1n + "') ; Update customer set rate_normalsms='" + s1n + "',rate_smartsms='" + s2n + "',rate_campaign='" + s3n + "',rate_otp='" + s4n + "',urlrate = '" + s5n + "',dltcharge='" + d1n + "' Where username = '" + un + "' ";
            database.ExecuteNonQuery(sql);
            return "Rate Successfully Updated.";
        }

        public string UpdateURLPrice(string un, string urlrate, string user)
        {
            string sql = "Update customer set urlrate='" + urlrate + "' Where username = '" + un + "' ";
            database.ExecuteNonQuery(sql);
            return "Rate Successfully Updated.";
        }

        public DataTable GetDayWiseSMSSummary(string user, string f, string t, string senderId = "", string dltNo = " ", string usertype = " ", string empcode = " ")
        {
            string sql = "";
            string dlt = "";
            if (usertype == "BD")
            {
                sql = @"select smsdate as smsDate1,convert(varchar,smsdate,106) SMSDATE,userid,senderid,sum(submitted) Submitted,sum(delivered) Delivered,sum(Failed) Failed, sum(Unknown) Unknown,sum(DND) DND from DAYSUMMARY
where smsdate between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    //    dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                    sql = sql + " and userid = '" + user + "' ";
                }
                if (user == "" && empcode != "")
                {
                    sql = sql + " and userid IN (select username from CUSTOMER where empcode='" + empcode + "' )";
                }
                if (senderId != "")
                {
                    sql = sql + " and senderid = '" + senderId + "' ";
                }
                sql = sql + " group by smsdate,userid,senderid order by smsDate1";

            }
            else

            {
                sql = @"select smsdate as smsDate1,convert(varchar,smsdate,106) SMSDATE,userid,senderid,sum(submitted) Submitted,sum(delivered) Delivered,sum(Failed) Failed, sum(Unknown) Unknown, sum(DND) DND from DAYSUMMARY
where smsdate between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    //    dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                    sql = sql + " and userid = '" + user + "' ";
                }
                if (user == "" && dltNo != "")
                {
                    sql = sql + " and userid IN (select username from CUSTOMER where DLTNO='" + dltNo + "' )";
                }
                if (senderId != "")
                {
                    sql = sql + " and senderid = '" + senderId + "' ";
                }
                sql = sql + " group by smsdate,userid,senderid order by smsDate1";


            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetBalananceLogs(string username, string f, string t)
        {
            string sss = "select cast(trandate as date) as trandate,iif(trantype='C',balance,-balance) as balance from userBalCrDr with (nolock) where username='" + username + "' and trandate between '" + f + "' and '" + t + @"' order by trandate ";

            string sql1 = @"SELECT  ROW_NUMBER() OVER(ORDER BY baldate) AS Seq, balance,baldate into #tmp1 from customerBalLog with (nolock)
                            where userid='" + username + "' and baldate between '" + f + "' and '" + t + @"' order by baldate ";

            string sql2 = @"SELECT  ROW_NUMBER() OVER(ORDER BY baldate) AS Seq, balance,baldate into #tmp2 from customerBalLog with (nolock)
                            where userid='" + username + "' and baldate between DATEADD(dd,1, '" + f + "') and '" + t + @"' order by baldate ";

            string sql3 = "select cast(t1.baldate as date) as SMSDATE , t1.balance as amount ,(T1.balance - T2.balance) as Expenditure,t2.balance as NetAmount FROM #tmp1 t1 inner join #tmp2 t2 ON  t1.seq = t2.seq ";

            string sql = string.Format("{0}; {1} ; {2}", sql1, sql2, sql3);

            DataTable dt = database.GetDataTable(sql);

            DataTable dtRecharge = database.GetDataTable(sss);

            foreach (DataRow dr in dtRecharge.Rows)
            {
                var rowsToUpdate = dt.AsEnumerable().Where(r => r.Field<DateTime>("SMSDATE") == Convert.ToDateTime(dr["trandate"]));
                foreach (var row in rowsToUpdate)
                {
                    row.SetField("amount", Convert.ToDouble(row["amount"]) + (Convert.ToDouble(dr["balance"])));
                    row.SetField("Expenditure", Convert.ToDouble(row["Expenditure"]) + (Convert.ToDouble(dr["balance"])));
                }
                dt.AcceptChanges();
            }

            return dt;
        }

        public DataTable GetDayWiseSMSSummaryDetail(string user, string dat)
        {
            string sql = "";

            sql = @"SELECT smsdate AS smsDate1,convert(varchar,smsdate,106) SMSDATE,userid,SenderID,Submitted,Delivered,Failed,Unknown,DND FROM DAYSUMMARY WITH(NOLOCK)
                    WHERE userid='" + user + "' AND smsdate between '" + dat + "' AND '" + dat + @"'  ";
            sql = sql + " ORDER BY senderid ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        //abhishek 07-11-2022

        public string InsertHuundaiInsuranceSubClient(string CompCode, string CompName, string mastertype)
        {
            string msg = "";
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SP_InsertHyundaiInsurance";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COmpCOde", CompCode);
                cmd.Parameters.AddWithValue("@CompName", CompName);
                cmd.Parameters.AddWithValue("@masterType", mastertype);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 50);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                msg = Convert.ToString(cmd.Parameters["@Result"].Value);
                con.Close();



            }
            return msg;
        }

        public DataTable GetSMSReport(string f, string t, string usertype, string user, string empcode)
        {
            string sql = "";
            string dlt = "";

            sql = @"


        select row_number() over (Order by c.userName ) as Sln,
                         c.userName,s.SenderID,count(s.id) submitted,
        sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
        sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
        sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
                         from customer c with (nolock)
                        inner
                         join MSGSUBMITTED s with (nolock) on c.username = s.profileid /* and c.senderid = s.senderid */
                        left join delivery d with (nolock) on s.msgid = d.msgid
                        where s.sentdatetime between '" + f + "' and '" + t + @"' 
";

            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            else if (usertype == "BD")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.empcode = '" + empcode + "' ";
            }
            sql = sql + " group by c.userName,s.SenderID ";

            sql = sql + @"union all
        select row_number() over(Order by c.userName) as Sln,
                         c.userName,s.SenderID,count(s.id) submitted,
                 sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                 sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                 sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
                         from customer c with (nolock)
                        inner
                         join MSGSUBMITTEDlog s with (nolock) on c.username = s.profileid /* and c.senderid = s.senderid */
                        left
                         join deliverylog d with (nolock) on s.msgid = d.msgid
                        where s.sentdatetime between '" + f + "' and '" + t + @"'";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            else if (usertype == "BD")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.empcode = '" + empcode + "' ";
            }
            sql = sql + " group by c.userName,s.SenderID ";


            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserSMSReportDetailtoday(string user, string sender, string f, string t, string ReportType)
        {
            string sql = "";
            //Shishir 27/06/23 added union all with log tables
            sql = @"select m.msgid as MessageId, m.tomobile as MobileNo, m.senderid as Sender,'""'+msgtext+'""' as Message,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) as DeliveredDate,
CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, '""'+d.dlvrtext+'""' as RESPONSE FROM MSGSUBMITTED m with (nolock) left join DELIVERY d with (nolock) on m.msgid=d.msgid
where m.PROFILEID='" + user + "' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + "' and '" + t + @"' ";//order by m.sentdatetime
            if (ReportType == "Delivered")
            {
                sql = sql + "and d.dlvrstatus='Delivered'";
            }
            if (ReportType == "Rejected")
            {
                sql = sql + "and d.dlvrstatus<>'Delivered'";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserSMSReportDetail(string user, string sender, string f, string t, string camp = "", string n = "", string ReportType = "", string tempid = "", string campid = "", string mobono = "", string usertype = "", string DLTNO = "")
        {
            string sql = "";
            //Shishir 27/06/23 added union all with log tables
            if (Convert.ToString(sender) != "" && Convert.ToString(n) == "")
            {
                n = sender;
            }
            sql = @"select m.msgid as MessageId, m.tomobile as MobileNo, m.senderid as Sender,'""'+msgtext+'""' as Message,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) as DeliveredDate,
CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, '""'+d.dlvrtext+'""' as RESPONSE FROM MSGSUBMITTED m with (nolock) left join DELIVERY d with (nolock) on m.msgid=d.msgid inner join Customer c with (nolock) on c.username=m.PROFILEID";
            if (campid != "0" && campid != "")
            {
                sql = sql + " inner join smsfileupload su with(nolock) on su.USERID=c.username";
            }
            sql = sql + @" where 1=1 ";

            if (usertype == "ADMIN")
                sql = sql + " and C.DLTNO='" + DLTNO + "' ";

            if (usertype == "user" || usertype == "")
                sql = sql + " and m.PROFILEID='" + user + "' and m.senderid='" + sender + "' ";

            sql = sql + " and m.sentdatetime between '" + f + "' and '" + t + @"'";

            if (tempid != "0" && tempid != "")
            {
                sql = sql + @" and m.templateid ='" + tempid + "'";
            }
            if (campid != "0" && campid != "")
            {
                sql = sql + " and su.campaignname  = '" + campid + "'";
            }
            if (mobono != "")
            {
                sql = sql + "and m.tomobile like'%" + mobono + "%'";
            }

            sql = sql + @" union all select m.msgid as MessageId, m.tomobile as MobileNo, m.senderid as Sender,'""'+msgtext+'""' as Message,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) as DeliveredDate,
CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, '""'+d.dlvrtext+'""' as RESPONSE FROM MSGSUBMITTEDlog m with (nolock) left join DELIVERYlog d with (nolock) on m.msgid=d.msgid inner join Customer c with (nolock) on c.username=m.PROFILEID";//order by m.sentdatetime

            if (campid != "0" && campid != "")
            {
                sql = sql + " inner join smsfileupload su with(nolock) on su.USERID=c.username ";
            }
            sql = sql + @" where 1=1 ";

            if (usertype == "ADMIN")
                sql = sql + " and C.DLTNO='" + DLTNO + "' ";

            if (usertype == "user" || usertype == "")
                sql = sql + " and m.PROFILEID='" + user + "' and m.senderid=CASE WHEN ISNULL('" + n + "','')='' then m.senderid else '" + n + "' end ";

            sql = sql + " and m.sentdatetime between '" + f + "' and '" + t + @"' ";

            if (ReportType == "Delivered")
            {
                sql = sql + "and d.dlvrstatus='Delivered'";
            }
            if (ReportType == "Rejected")
            {
                sql = sql + " and d.dlvrstatus<>'Delivered'";
            }
            if (campid != "0" && campid != "")
            {
                sql = sql + " and su.campaignname  = '" + campid + "'";
            }
            if (tempid != "0" && tempid != "")
            {
                sql = sql + " and m.templateid = '" + tempid + "'";
            }
            if (mobono != "")
            {
                sql = sql + " and m.tomobile like'%" + mobono + "%'";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetUserSMSReportDetailSingle(string user, string sender, string f, string t, string uid, string sid, string ReportType, string tempid, string campid, string mobono, string usertype, string DLTNO)
        {
            string sql = "";
            //Shishir 27/06/23 added union all with log tables
            sql = @"select m.msgid as MessageId, m.tomobile as MobileNo, m.senderid as Sender,'""'+msgtext+'""' as Message,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) as DeliveredDate,
CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, '""'+d.dlvrtext+'""' as RESPONSE FROM MSGSUBMITTEDlog m with (nolock) left join DELIVERYlog d with (nolock) on m.msgid=d.msgid inner join Customer c with (nolock) on c.username=m.PROFILEID";//order by m.sentdatetime

            if (campid != "0")
            {
                sql = sql + " inner join smsfileupload su with(nolock) on su.USERID=c.username";
            }
            sql = sql + " where m.PROFILEID = CASE WHEN ISNULL('" + uid + "','')='NA' THEN m.PROFILEID ELSE '" + uid + "' END AND" +
" m.senderid = CASE WHEN ISNULL('" + sid + "','')='0' THEN m.senderid ELSE '" + sid + "' END and m.sentdatetime between '" + f + "' and '" + t + @"'  ";//order by m.sentdatetime"

            if (usertype == "ADMIN")
            {
                sql = sql + " and C.DLTNO='" + DLTNO + "' ";
            }
            if (ReportType == "Delivered")
            {
                sql = sql + "and d.dlvrstatus='Delivered'";
            }
            if (ReportType == "Rejected")
            {
                sql = sql + "and d.dlvrstatus<>'Delivered'";
            }
            if (campid != "0")
            {
                sql = sql + " and su.campaignname  = '" + campid + "'";
            }
            if (tempid != "0")
            {
                sql = sql + " and m.templateid = '" + tempid + "'";
            }
            if (mobono != "")
            {
                sql = sql + " and m.tomobile like'%" + mobono + "%'";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport_user(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = string.Format("select row_number() over (Order by s.createdat desc ) as Sln,s.msgid,s.tomobile as mobile,s.senderid, s.msgtext," +
               "DATEADD(MINUTE,{0} , convert(varchar, s.sentdatetime,106) + ' ' + convert(varchar, s.sentdatetime,108)) as senttime," +
                "d.DLVRSTATUS + '  ' + convert(varchar, d.DLVRTIME,106) + ' ' + convert(varchar, d.DLVRTIME,108) as dlrstat" +
                 " from customer c with (nolock) inner join MSGSUBMITTED s with (nolock) on c.username = s.profileid " +
                "left join delivery d with (nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)" +
                " where s.createdat between '{1}' and '{2}' and s.profileid='{3}'", Global.addMinutes, f, t, user);
            //if (usertype == "ADMIN")
            //{
            //    dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
            //    sql = sql + " and c.dltno = '" + dlt + "' ";
            //}

            //sql = sql + " group by c.userName,c.SenderID";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport_user_newConsolidated(string f, string t, string usertype, string user, string campnm, string sender)
        {
            //            string sql = @"select '" + Convert.ToDateTime(f).ToString("dd/MMM/yyyy") + "' as fromdate, '" + Convert.ToDateTime(t).ToString("dd/MMM/yyyy") + @"' as todate, '" + Convert.ToDateTime(f).ToString("dd/MMM/yy") + " TO " + Convert.ToDateTime(t).ToString("dd/MMM/yy") + @"' as submitdate,s.senderid as sender,
            //count(s.id) submitted,
            //sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            //sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            //sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '" + user + @"' as userid
            //from customer c with(nolock)
            //inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid ";
            //            if (campnm != "" && campnm != "0") sql = sql + @" inner join smsfileupload u on s.fileid=u.id ";
            //            sql = sql + @" left join delivery d with(nolock) on s.msgid = d.msgid 
            //where c.username = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' ";
            //            if (campnm != "" && campnm != "0") sql = sql + @" and u.campaignname = '" + campnm + "' ";
            //            if (sender != "" && sender != "0") sql = sql + " and s.senderid='" + sender + "' ";
            //            sql = sql + @" group by s.senderid order by s.senderid";
            //            DataTable dt = database.GetDataTable(sql);
            //return dt;

            string sql1 = @"select '" + Convert.ToDateTime(f).ToString("dd/MMM/yyyy") + "' as fromdate, '" + Convert.ToDateTime(t).ToString("dd/MMM/yyyy") + @"' as todate, '" + Convert.ToDateTime(f).ToString("dd/MMM/yy") + " TO " + Convert.ToDateTime(t).ToString("dd/MMM/yy") + @"' as submitdate,s.senderid as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '" + user + @"' as userid into #tbldlr
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid ";
            if (campnm != "" && campnm != "0") sql1 = sql1 + @" inner join smsfileupload u on s.fileid=u.id ";
            sql1 = sql1 + @" left join delivery d with(nolock) on s.msgid = d.msgid 
where c.username = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' ";
            if (campnm != "" && campnm != "0") sql1 = sql1 + @" and u.campaignname = '" + campnm + "' ";
            if (sender != "" && sender != "0") sql1 = sql1 + " and s.senderid='" + sender + "' ";
            sql1 = sql1 + @" group by s.senderid ";


            string sql2 = @" Union All select '" + Convert.ToDateTime(f).ToString("dd/MMM/yyyy") + "' as fromdate, '" + Convert.ToDateTime(t).ToString("dd/MMM/yyyy") + @"' as todate, '" + Convert.ToDateTime(f).ToString("dd/MMM/yy") + " TO " + Convert.ToDateTime(t).ToString("dd/MMM/yy") + @"' as submitdate,s.senderid as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED_B418FEB s with(nolock) on c.username = s.profileid ";
            if (campnm != "" && campnm != "0") sql2 = sql2 + @" inner join smsfileupload u on s.fileid=u.id ";
            sql2 = sql2 + @" left join delivery_B418FEB d with(nolock) on s.msgid = d.msgid 
where c.username = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' ";
            if (campnm != "" && campnm != "0") sql2 = sql2 + @" and u.campaignname = '" + campnm + "' ";
            if (sender != "" && sender != "0") sql2 = sql2 + " and s.senderid='" + sender + "' ";
            sql2 = sql2 + @" group by s.senderid ;

            select fromdate,todate,submitdate,sender,sum(submitted)submitted,sum(delivered)delivered,sum(failed)failed ,sum(unknown)unknown,userid
 from #tbldlr group by fromdate,todate,submitdate, sender,userid  order by sender drop table #tbldlr";

            DataTable dt = database.GetDataTable(sql1 + sql2);
            return dt;
        }

        public DataTable GetSMSReport_user_newConsolidatedDETAIL(string f, string t, string sender, string user, string campnm = "")
        {
            string str = "";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + user + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";
            //' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'
            string sql = "";
            string sql_union = "";
            if (user == "MIM2101371")
            {
                sql = @"select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
case when d.dlvrtime is null then '' else convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) end as DeliveredDate,
Replace(Replace(msgtext,CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED m with (nolock) ";
                if (campnm != "" && campnm != "0") sql = sql + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql = sql + @" left join DELIVERY d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql = sql + @" and u.campaignname = '" + campnm + "' ";
                sql = sql + @" order by m.sentdatetime";

                sql_union = @" union All select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
case when d.dlvrtime is null then '' else convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) end as DeliveredDate,
Replace(Replace(msgtext,CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED_B418FEB m with (nolock) ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql_union = sql_union + @" left join DELIVERY_B418FEB d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" and u.campaignname = '" + campnm + "' ";
                sql_union = sql_union + @" order by m.sentdatetime ";
            }
            else
            {

                sql = @"select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",m.sentdatetime),120) as SentDate,
case when d.insertdate is null then '' else Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",d.insertdate),120) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED m with (nolock) ";
                if (campnm != "" && campnm != "0") sql = sql + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql = sql + @" left join DELIVERY d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql = sql + @" and u.campaignname = '" + campnm + "' ";
                sql = sql + @" ";

                sql_union = @" union All select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",m.sentdatetime),120) as SentDate,
case when d.insertdate is null then '' else Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",d.insertdate),120) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED_B418FEB m with (nolock) ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql_union = sql_union + @" left join DELIVERY_B418FEB d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" and u.campaignname = '" + campnm + "' ";
                sql_union = sql_union + @" order by SMSdate";
            }

            DataTable dt = database.GetDataTable(sql + sql_union);
            return dt;
        }

        public DataTable GetSMSReport_user_new(string f, string t, string usertype, string user, string campnm = "")
        {
            string sql = "";
            sql = @"select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,'' as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where c.username = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and s.FILEID = '1'
union all
select '2' as sr,'1' AS SL,'' as submitdate,'ENTRY' as reqsrc,'' as filenm,'' as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,0 as fileid,'" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
LEFT JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
where c.username ='" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and SS.campaignname='Manual' AND isnull(SS.FILENM,'')=''
UNION ALL
select '2' as sr,'1' AS SL, Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,f.UPLOADTIME,106) + ' ' + convert(varchar,f.UPLOADTIME,108)),120) as submitdate,'FILE' as reqsrc,f.FILENM as filenm,f.senderid as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, left(s.smsTEXT, 2) as msg,f.id as fileid, '" + user + @"' as userid
from customer c with(nolock)
INNER JOIN SMSFILEUPLOAD f on c.username = f.USERID
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid and s.FILEID = f.ID
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where c.username  ='" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and s.FILEID > 1 AND f.campaignname <> 'Manual' ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            sql = sql + @" group by f.UPLOADTIME,f.FILENM,f.senderid,left(s.smsTEXT, 2),f.id
order by sr,sl,submitdate";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetSMSSubClientReport_user_new3(string f, string t, string usertype, string user, string campnm = "", string mob = "")
        {
            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);

            string cd = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dlrTable = cd == f ? "delivery" : "deliverylog";
            string submitTable = cd == f ? "msgsubmitted" : "msgsubmittedlog";

            string sql = "";
            sql = @"
             WITH CTE AS(select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,s.SENDERID as sender,s.SubClientCode+' ('+ isnull(mst.SMS_IC_Name,'Others') +')' as SubClientCode,s.SubClientCode as SubClientID,
             sum(1 ) submitted,
             sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
             sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
             sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
             from MSGSUBClientCode s with(nolock) 
             inner join " + submitTable + @" m on s.msgidClient=m.msgid
             left join " + dlrTable + @" d with(nolock) on s.msgidClient = d.msgid 
             left JOIN mstSubClientCode mst ON s.SubClientCode=mst.SMS_IC_Code
             where m.profileid = '" + user + @"' and m.sentdatetime between " + fDate + @" and " + tDate + (mob != "" ? " and m.tomobile LIKE '%" + mob + "%' " : " ") + @"
             and mst.SMS_IC_Name is null
             group by s.SENDERID,s.SubClientCode,isnull(mst.SMS_IC_Name,'Others')
             UNION ALL
             SELECT '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,m.SENDERID as sender,' ('+ 'Others' +')' as SubClientCode,'Others' as SubClientID,
             sum(1) submitted,
             sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
             sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
             sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
             FROM " + submitTable + @" m WITH(NOLOCK)
             left join MSGSUBClientCode s WITH(NOLOCK) ON s.msgidClient=m.msgid
             left join " + dlrTable + @" d WITH(NOLOCK) ON m.msgid= d.msgid
             left JOIN mstSubClientCode mst WITH(NOLOCK) ON s.SubClientCode=mst.SMS_IC_Code
             WHERE m.profileid = '" + user + @"' and m.SENTDATETIME between " + fDate + @" and " + tDate + (mob != "" ? " and m.tomobile LIKE '%" + mob + "%' " : " ") + @"
             and m.FILEID=1 and s.msgidClient IS NULL
             GROUP BY m.SENDERID)

             SELECT '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,s.SENDERID as sender,s.SubClientCode+' ('+ mst.SMS_IC_Name +')' as SubClientCode,s.SubClientCode as SubClientID,
             sum(1) submitted,
             sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
             sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
             sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
             from MSGSUBClientCode s with(nolock) 
             inner join " + submitTable + @" m on s.msgidClient=m.msgid
             left join " + dlrTable + @" d with(nolock) on s.msgidClient = d.msgid 
             inner JOIN mstSubClientCode mst ON s.SubClientCode=mst.SMS_IC_Code
             where m.profileid = '" + user + @"' and m.sentdatetime between " + fDate + @" and " + tDate + (mob != "" ? " and m.tomobile LIKE '%" + mob + "%' " : " ") + @"
             group by s.SENDERID,s.SubClientCode,mst.SMS_IC_Name
             UNION ALL
             SELECT sr,SL,submitdate,reqsrc,filenm,sender,SubClientCode,SubClientID,SUM(submitted) AS submitted,SUM(delivered) AS delivered,SUM(failed) AS failed,SUM(unknown) AS unknown,msg,fileid,userid FROM CTE group by sr,SL,submitdate,reqsrc,filenm,sender,SubClientCode,SubClientID,msg,fileid,userid";

            /*
            "SELECT FILEID,SubClientCode,COUNT(*) AS SUBMITTED,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, max(s.MSGTEXT) as msg INTO #T1
            FROM MSGSUBClientCode s with(nolock) 
            JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
            left join " + dlrTable + @" d with(nolock) on s.msgidClient = d.msgid --and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
            where S.PROFILEID  ='" + user + @"' and s.CREATEDAT between " + fDate + @" and " + tDate + @" and s.FILEID > 1 and SS.campaignname<>'Manual'";
                        if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

                        sql = sql + @"group by S.FILEID ,S.SubClientCode
            select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,s.SENDERID as sender,s.SubClientCode+' ('+mst.SMS_IC_Name+')' as SubClientCode,s.SubClientCode as SubClientID,
            count(s.id) submitted,
            sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
            from customer c with(nolock)
            inner join MSGSUBClientCode s with(nolock) on c.username = s.profileid
            JOIN mstSubClientCode mst ON s.SubClientCode=mst.SMS_IC_Code
             join " + dlrTable + @" d with(nolock) on s.msgidClient = d.msgid and convert(varchar,s.CREATEDAT,102)=convert(varchar,d.insertdate,102)
            where c.username = '" + user + @"' and s.CREATEDAT between " + fDate + @" and " + tDate + @" and s.FILEID = '1' AND s.SubClientCode<>''";
                        if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

                        sql = sql + @" group by s.SENDERID,s.SubClientCode,mst.SMS_IC_Name";


                        sql = sql + @" UNION
            select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,s.SENDERID as sender,'Other' as SubClientCode,'Other' as SubClientID,
            count(s.id) submitted,
            sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
            from customer c with(nolock)
            inner join MSGSUBClientCode s with(nolock) on c.username = s.profileid  
            left join " + dlrTable + @" d with(nolock) on s.msgidClient = d.msgid and convert(varchar,s.CREATEDAT,102)=convert(varchar,d.insertdate,102)
            where c.username = '" + user + @"'  AND s.SubClientCode not in (SELECT SMS_IC_Code FROM mstSubClientCode) and s.CREATEDAT between " + fDate + @" and " + tDate + @" and s.FILEID = '1'";
                        if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

                        sql = sql + @" group by s.SENDERID";
                        */


            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetSMSReport_user_new3(string f, string t, string usertype, string user, string campnm = "", string mob = "", string TempIdAndName = "")
        {
            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);

            string sql = "";
            sql = @"

SELECT FILEID,COUNT(*) AS SUBMITTED,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, max(s.smsTEXT) as msg INTO #T1
FROM MSGSUBMITTED s with(nolock) 
JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
left join delivery d with(nolock) on s.msgid = d.msgid --and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where S.PROFILEID  ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and s.FILEID > 1 and SS.campaignname<>'Manual'";
            if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";

            sql = sql + @"group by S.FILEID 
select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,s.SENDERID as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where c.username = '" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and s.FILEID = '1'";
            if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";

            sql = sql + @" group by s.SENDERID union all
select '2' as sr,'1' AS SL,'' as submitdate,'ENTRY' as reqsrc,'' as filenm,'' as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,0 as fileid,'" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
LEFT JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
/* inner join FileProcess fp with (nolock) ON fp.id = ss.FileProcessId */
where c.username ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and SS.campaignname='Manual' AND isnull(SS.FILENM,'')='' ";
            if (f == @"02/JAN/1900") sql = sql + " and 1=0 ";
            if (campnm != "" && campnm != "0") sql = sql + @" and ss.campaignname = '" + campnm + "' "; //Add By Naved
            if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";

            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";

            sql = sql + @" UNION ALL
select '2' as sr,'1' AS SL, Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,f.UPLOADTIME,106) + ' ' + convert(varchar,f.UPLOADTIME,108)),120) as submitdate,'FILE' as reqsrc,f.FILENM as filenm,f.senderid as sender,

case when fp.scheduleDeletedTime is not null then 0 else
case when fp.isschedule=1 and scheduletime>getdate() then 0 else";
            if (mob != "")
                sql = sql + @" ISNULL(T.SUBMITTED,0) end end AS submitted, ";
            else
                sql = sql + @" CASE WHEN ISNULL(T.SUBMITTED,0) > (fp.noofrecord * FP.NOOFSMS) THEN ISNULL(T.SUBMITTED,0) ELSE (fp.noofrecord  * FP.NOOFSMS) END end end AS submitted, ";

            sql = sql + @" t.delivered, t.failed, ";

            if (mob != "")
                sql = sql + @" ISNULL(T.SUBMITTED,0) -  t.delivered - t.failed as unknown,";
            else
                sql = sql + @" CASE WHEN ISNULL(T.SUBMITTED,0) > (fp.noofrecord * FP.NOOFSMS) THEN ISNULL(T.SUBMITTED,0) ELSE (fp.noofrecord  * FP.NOOFSMS) END -  t.delivered - t.failed as unknown,";

            sql = sql + @" t.msg,F.ID, '" + user + @"' as userid
from customer c with(nolock)
inner join FileProcess fp with (nolock) ON FP.profileid=C.USERNAME 
LEFT JOIN SMSFILEUPLOAD f on c.username = f.USERID AND  fp.id = f.FileProcessId
LEFT join #t1 t on t.fileid=f.id
where c.username  ='" + user + @"' AND f.campaignname <> 'Manual' and isnull(f.schedule,f.UPLOADTIME) between " + fDate + @" and " + tDate + @" ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            //if (mob != "") sql = sql + @" and TOMOBILE = '" + mob + "' ";

            sql = sql + @" order by sr,sl,submitdate";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetSMSReport_user_new2(string f, string t, string usertype, string user, string campnm = "")
        {
            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);

            string sql = "";
            sql = @"

SELECT FILEID,COUNT(*) AS SUBMITTED,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, max(s.smsTEXT) as msg INTO #T1
FROM MSGSUBMITTED s with(nolock) 
JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
left join delivery d with(nolock) on s.msgid = d.msgid --and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where S.PROFILEID  ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and s.FILEID > 1 and SS.campaignname<>'Manual'
group by S.FILEID ;


select '1' as sr,'1' AS SL,'' as submitdate,'API' as reqsrc,'' as filenm,'' as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,1 as fileid, '" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
where c.username = '" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and s.FILEID = '1'
union all
select '2' as sr,'1' AS SL,'' as submitdate,'ENTRY' as reqsrc,'' as filenm,'' as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,0 as fileid,'" + user + @"' as userid
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
LEFT JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
/* inner join FileProcess fp with (nolock) ON fp.id = ss.FileProcessId */
where c.username ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and SS.campaignname='Manual' AND isnull(SS.FILENM,'')='' ";
            if (f == @"02/JAN/1900") sql = sql + " and 1=0 ";
            if (campnm != "" && campnm != "0") sql = sql + @" and ss.campaignname = '" + campnm + "' "; //Add By Naved

            sql = sql + @" UNION ALL
select '2' as sr,'1' AS SL, Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,f.UPLOADTIME,106) + ' ' + convert(varchar,f.UPLOADTIME,108)),120) as submitdate,'FILE' as reqsrc,f.FILENM as filenm,f.senderid as sender,

case when fp.scheduleDeletedTime is not null then 0 else
case when fp.isschedule=1 and scheduletime>getdate() then 0 else
CASE WHEN ISNULL(T.SUBMITTED,0) > (fp.noofrecord * FP.NOOFSMS) THEN ISNULL(T.SUBMITTED,0) ELSE (fp.noofrecord  * FP.NOOFSMS) END end end AS submitted, 

t.delivered, t.failed, 
CASE WHEN ISNULL(T.SUBMITTED,0) > (fp.noofrecord * FP.NOOFSMS) THEN ISNULL(T.SUBMITTED,0) ELSE (fp.noofrecord  * FP.NOOFSMS) END -  t.delivered - t.failed as unknown, t.msg,t.fileid, '" + user + @"' as userid
from customer c with(nolock)
inner join FileProcess fp with (nolock) ON FP.profileid=C.USERNAME 
LEFT JOIN SMSFILEUPLOAD f on c.username = f.USERID AND  fp.id = f.FileProcessId
LEFT join #t1 t on t.fileid=f.id
where c.username  ='" + user + @"' AND f.campaignname <> 'Manual' and isnull(f.schedule,f.UPLOADTIME) between " + fDate + @" and " + tDate + @" ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            //sql = sql + @" group by fp.noofrecord,f.UPLOADTIME,f.FILENM,f.senderid,left(s.smsTEXT, 2),f.id
            sql = sql + @" order by sr,sl,submitdate";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetSMSReport_user_newConsolidated_new2(string f, string t, string usertype, string user, string campnm, string sender)
        {


            string sql1 = @"select '" + Convert.ToDateTime(f).ToString("dd/MMM/yyyy") + "' as fromdate, '" + Convert.ToDateTime(t).ToString("dd/MMM/yyyy") + @"' as todate, '" + Convert.ToDateTime(f).ToString("dd/MMM/yy") + " TO " + Convert.ToDateTime(t).ToString("dd/MMM/yy") + @"' as submitdate,s.senderid as sender,
count(s.id) submitted,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '" + user + @"' as userid into #tbldlr
from customer c with(nolock)
inner join MSGSUBMITTED s with(nolock) on c.username = s.profileid ";
            if (campnm != "" && campnm != "0") sql1 = sql1 + @" inner join smsfileupload u on s.fileid=u.id ";
            sql1 = sql1 + @" left join delivery d with(nolock) on s.msgid = d.msgid 
where c.username = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' ";
            if (campnm != "" && campnm != "0") sql1 = sql1 + @" and u.campaignname = '" + campnm + "' ";
            if (sender != "" && sender != "0") sql1 = sql1 + " and s.senderid='" + sender + "' ";
            sql1 = sql1 + @" group by s.senderid ; ";


            sql1 = sql1 + @" select fromdate,todate,submitdate,sender,sum(submitted)submitted,sum(delivered)delivered,sum(failed)failed ,sum(unknown)unknown,userid
 from #tbldlr group by fromdate,todate,submitdate, sender,userid  order by sender drop table #tbldlr";

            DataTable dt = database.GetDataTable(sql1);
            return dt;
        }

        public DataTable GetSMSSubClientReportDetail_user_new2(string userid, string fileid, string sender, string f, string t, string reqsrc = "", string mob = "", string SubClientID = "")
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            string cd = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dlrTable = cd == f ? "delivery" : "deliverylog";
            string submitTable = cd == f ? "msgsubmitted" : "msgsubmittedlog";

            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);

            if (fileid == "1" && SubClientID != "Other" && SubClientID != "Others") // api msg
            {
                //' ' + left(convert(varchar,m.tomobile),len(convert(varchar,m.tomobile))-4) + 'XXXX'
                sql = @"select m.msgidClient+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+m.msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBClientCode m with (nolock) 
                inner join " + submitTable + @" s on m.msgidClient=s.msgid
                left join " + dlrTable + @" d with(nolock) on s.msgid = d.msgid
                and convert(varchar,m.CREATEDAT,102)=convert(varchar,d.insertdate,102) 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SubClientCode='" + SubClientID + "' and m.CREATEDAT between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" order by m.CREATEDAT ";

            }
            else if (fileid == "1" && SubClientID == "Others")
            {
                sql = @"select m.msgidClient+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+m.msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBClientCode m with (nolock) 
                inner join " + submitTable + @" s on m.msgidClient=s.msgid
                left join " + dlrTable + @" d with(nolock) on s.msgid = d.msgid
                and convert(varchar,m.CREATEDAT,102)=convert(varchar,d.insertdate,102) 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SubClientCode='" + SubClientID + "' and m.CREATEDAT between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" UNION ALL 
                select m.msgid+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+ m.msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE 
                FROM " + submitTable + @" m with (nolock) 
                left join MSGSUBClientCode s on s.msgidClient=m.msgid
                left join " + dlrTable + @" d with(nolock) on m.msgid = d.msgid
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SENTDATETIME between " + fDate + " and " + tDate + " and m.fileid=1 " +
                " and s.msgidClient is null";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
            }
            else
            {
                sql = @"select m.msgidClient+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+ m.msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBClientCode m with (nolock) 
                inner join " + submitTable + @" s on m.msgidClient=s.msgid
                left join " + dlrTable + @" d with(nolock) on s.msgid = d.msgid
                and convert(varchar,m.CREATEDAT,102)=convert(varchar,d.insertdate,102) 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SubClientCode not in (SELECT SMS_IC_Code FROM mstSubClientCode) and m.CREATEDAT between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" order by m.CREATEDAT ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReportDetail_user_new2(string userid, string fileid, string sender, string f, string t, string reqsrc = "", string mob = "")
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);
            if (reqsrc.ToUpper() == "ENTRY") // entry msg
            {
                sql = @"SELECT m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+ msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,d.err_code AS ErrorCode,ec.descr AS ErrorDescription
                FROM MSGSUBMITTED m with (nolock)
                LEFT JOIN DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                LEFT JOIN (SELECT CODE, MAX(descr) descr FROM ERRORCODE WITH(NOLOCK) GROUP BY CODE) ec  ON d.err_code=ec.code
                inner join SMSFILEUPLOAD u with (nolock) on u.id = m.fileid AND u.USERID=m.PROFILEID
                where m.PROFILEID='" + userid + "' /* and m.senderid='" + sender + "' */ and m.sentdatetime between " + fDate + " and " + tDate + " and isnull(u.campaignname,'')='Manual' AND isnull(u.FILENM,'')=''";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" order by m.sentdatetime ";

            }
            else if (fileid == "1") // api msg
            {
                sql = @"SELECT m.msgid+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,d.err_code AS ErrorCode,ec.descr AS ErrorDescription
                FROM MSGSUBMITTED m with (nolock)
                LEFT JOIN DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                LEFT JOIN (SELECT CODE, MAX(descr) descr FROM ERRORCODE WITH(NOLOCK) GROUP BY CODE) ec ON d.err_code=ec.code
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" order by m.sentdatetime ";
            }
            else
            {
                sql = @"SELECT m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                    '""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,d.err_code AS ErrorCode,ec.descr AS ErrorDescription
                FROM MSGSUBMITTED m with (nolock)
                LEFT JOIN DELIVERY d with (nolock) on m.msgid=d.msgid AND convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN (SELECT CODE, MAX(descr) descr FROM ERRORCODE WITH(NOLOCK) GROUP BY CODE) ec  ON d.err_code=ec.code
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=" + fileid + "";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + @" order by m.sentdatetime ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport_user_newConsolidatedDETAIL_new2(string f, string t, string sender, string user, string campnm = "")
        {
            string str = "";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + user + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";
            //' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'
            string sql = "";
            string sql_union = "";
            if (user == "MIM2101371")
            {
                sql = @"select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
case when d.dlvrtime is null then '' else convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) end as DeliveredDate,
Replace(Replace(msgtext,CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED m with (nolock) ";
                if (campnm != "" && campnm != "0") sql = sql + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql = sql + @" left join DELIVERY d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql = sql + @" and u.campaignname = '" + campnm + "' ";
                sql = sql + @" order by m.sentdatetime";

                sql_union = @" union All select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108) as SentDate,
case when d.dlvrtime is null then '' else convert(varchar,d.dlvrtime,106) + ' ' + convert(varchar,d.dlvrtime,108) end as DeliveredDate,
Replace(Replace(msgtext,CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED_B418FEB m with (nolock) ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql_union = sql_union + @" left join DELIVERY_B418FEB d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" and u.campaignname = '" + campnm + "' ";
                sql_union = sql_union + @" order by m.sentdatetime ";
            }
            else
            {

                sql = @"select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",m.sentdatetime),120) as SentDate,
case when d.insertdate is null then '' else Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",d.insertdate),120) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED m with (nolock) ";
                if (campnm != "" && campnm != "0") sql = sql + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql = sql + @" left join DELIVERY d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql = sql + @" and u.campaignname = '" + campnm + "' ";
                sql = sql + @" ";

                sql_union = @" union All select convert(varchar,sentdatetime,102) as SMSdate, m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",m.sentdatetime),120) as SentDate,
case when d.insertdate is null then '' else Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",d.insertdate),120) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED_B418FEB m with (nolock) ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" inner join smsfileupload u with (nolock) on m.fileid=u.id ";
                sql_union = sql_union + @" left join DELIVERY_B418FEB d with (nolock) on m.msgid=d.msgid  
where m.PROFILEID='" + user + @"' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
                if (campnm != "" && campnm != "0") sql_union = sql_union + @" and u.campaignname = '" + campnm + "' ";
                sql_union = sql_union + @" order by SMSdate";
            }

            DataTable dt = database.GetDataTable(sql + sql_union);
            return dt;
        }

        public DataTable GetSMSSubClientReportDetail_user_new(string userid, string fileid, string sender, string f, string t, string reqsrc = "", string mob = "", string SubClientID = "")
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            string cd = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dlrTable = cd == f ? "delivery" : "deliverylog";
            string submitTable = cd == f ? "msgsubmitted" : "msgsubmittedlog";

            if (fileid == "1" && SubClientID != "Other") // api msg
            {
                //' ' + left(convert(varchar,m.tomobile),len(convert(varchar,m.tomobile))-4) + 'XXXX'
                sql = @"select m.msgidClient as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,SubClientCode,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                m.msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE 
                FROM MSGSUBClientCode m with (nolock) 
                inner join " + submitTable + @" s on m.msgidClient=s.msgid
                left join " + dlrTable + @" d with(nolock) on s.msgid = d.msgid 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SubClientCode='" + SubClientID + "' and m.CREATEDAT between '" + f + "' and '" + t + "' ";
                if (mob != "") sql = sql + @" and s.TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + " order by m.CREATEDAT ";

            }
            else
            {
                sql = @"select m.msgidClient as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,SubClientCode,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.CREATEDAT,106) + ' ' + convert(varchar,m.CREATEDAT,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                m.msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE 
                FROM MSGSUBClientCode m with (nolock) 
                inner join " + submitTable + @" s on m.msgidClient=s.msgid
                left join " + dlrTable + @" d with(nolock) on s.msgid = d.msgid
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' AND m.SubClientCode not in (SELECT SMS_IC_Code FROM mstSubClientCode) and m.CREATEDAT between '" + f + "' and '" + t + "' ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + " order by m.CREATEDAT ";
            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReportDetail_user_new(string userid, string fileid, string sender, string f, string t, string reqsrc = "", string mob = "")
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";
            if (reqsrc.ToUpper() == "ENTRY") // entry msg
            {
                //' ' + left(convert(varchar,m.tomobile),len(convert(varchar,m.tomobile))-4) + 'XXXX'
                sql = @"select m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBMITTED m with (nolock)
                left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                inner join SMSFILEUPLOAD u with (nolock) on u.id = m.fileid AND u.USERID=m.PROFILEID
                where m.PROFILEID='" + userid + "' and m.sentdatetime between '" + f + "' and '" + t + "' and isnull(u.campaignname,'')='Manual' AND isnull(u.FILENM,'')=''";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + " order by m.sentdatetime ";

            }
            else if (fileid == "1") // api msg
            {
                //' ' + left(convert(varchar,m.tomobile),len(convert(varchar,m.tomobile))-4) + 'XXXX'
                sql = @"select m.msgid as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBMITTED m with (nolock) left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + "' and '" + t + "' and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                sql = sql + " order by m.sentdatetime ";

            }
            else
            {
                //' ' + left(convert(varchar,m.tomobile),len(convert(varchar,m.tomobile))-4) + 'XXXX'
                sql = @"select m.msgid as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                    msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBMITTED m with (nolock) left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between '" + f + "' and '" + t + "' and m.fileid=" + fileid + "";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%'";
                sql = sql + " order by m.sentdatetime ";

            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport4MOBILE_user(string f, string t, string mob, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @"select row_number() over (Order by s.createdat desc ) as Sln,
                    s.msgid,' ' + convert(varchar,s.tomobile) as mobile,s.senderid, s.msgtext,convert(varchar, s.sentdatetime,106) + ' ' + convert(varchar, s.sentdatetime,108) as senttime,
                d.DLVRSTATUS + '  ' + convert(varchar, d.DLVRTIME,106) + ' ' + convert(varchar, d.DLVRTIME,108) as dlrstat 
                 from customer c with (nolock)
                inner
                 join MSGSUBMITTED s with (nolock) on c.username = s.profileid 
                left join delivery d with (nolock) on s.msgid = d.msgid
                where s.sentdatetime between '" + f + "' and '" + t + @"' and s.profileid='" + user + "' ";
            if (mob != "")
                sql = sql + " and s.tomobile='91" + mob + "'";
            //if (usertype == "ADMIN")
            //{
            //    dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
            //    sql = sql + " and c.dltno = '" + dlt + "' ";
            //}

            //sql = sql + " group by c.userName,c.SenderID";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogDetailSMS(string f, string t, string user)
        {
            string sql = @"select case when trantype='C' then 'Credit' else 'Debit' end as TranType, convert(numeric(10),round(balance/smsrate,0)) as amount, convert(varchar,trandate,106) + ' ' + convert(varchar(5),trandate,108) as trandate,Remarks
from userBalCrDr where username = '" + user + "' and trandate between '" + f + "' and '" + t + @"' order by trandate desc";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogDetailSMS(string f, string t)
        {
            //Fixed compname

            string sql = @"select u.UserName,c.compname ,convert(varchar,u.trandate,106) + ' ' + convert(varchar(5),u.trandate,108) as TransactionDate, case when u.trantype='C' then 'Credit' else 'Debit' end as TransactionType, convert(numeric(10),u.balance) as Amount,c.rate_normalsms [NormalRate] ,u.Remarks 
from userBalCrDr u with (nolock) Inner join customer c with (nolock) ON u.UserName = c.UserName  where trandate between '" + f + "' and '" + t + @"' order by trandate ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogDetailSMS_Admin(string f, string t, string usertype, string dlt)
        {
            string sql = @"select u.UserName, convert(varchar,u.trandate,106) + ' ' + convert(varchar(5),u.trandate,108) as TransactionDate, case when u.trantype='C' then 'Credit' else 'Debit' end as TransactionType, convert(numeric(10),u.balance) as Amount,c.rate_normalsms [NormalRate],u.Remarks 
from userBalCrDr u with (nolock) Inner join customer c with (nolock) ON u.UserName = c.UserName 
where trandate between '" + f + "' and '" + t + @"'";
            if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dlt + "' ";
            sql = sql + "order by trandate";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        //abhishek 07-11-2022
        public bool CheckUserName(string username)
        {
            bool a = false;
            string sql = @"select count(*) as count from  dlrdownloadXLUser where userid='" + username + "'";
            DataTable dt = database.GetDataTable(sql);

            if (dt != null)
            {
                if (dt.Rows[0]["count"].ToString() != "0")
                {
                    a = true;
                }
            }
            return a;
        }

        public DataTable GetClickReort(string f, string t, string user, string usertype, string dlt, string empcode)
        {
            t = t + " 23:59:59";
            string sql = @"select row_number() over (Order by U.USERID) as Sln, U.USERID AS USERNAME, '" + "" + @"/' + U.segment as SmallURL,COUNT(S.SHORTURL_ID) AS No_Of_Hits
FROM short_urls U 
inner join customer c on u.userid=c.username
left join stats S on U.ID = S.SHORTURL_ID 
where U.added between '" + f + "' and '" + t + @"' 
AND U.mobtrack <> 'Y' and S.SHORTURL_ID <>0";
            if (usertype == "ADMIN")
            {
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            else if (usertype == "BD")
            {
                sql = sql + " and c.empcode = '" + empcode + "' ";
            }
            sql = sql + " GROUP BY U.USERID,U.SEGMENT ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        //        public DataTable GetClickReort(string f, string t, string user, string usertype, string dlt,string empcode)
        //        {
        //            t = t + " 23:59:59";
        //            string sql = @"select row_number() over (Order by U.USERID) as Sln, U.USERID AS USERNAME, '" + "" + @"/' + U.segment as SmallURL,COUNT(S.SHORTURL_ID) AS No_Of_Hits
        //FROM short_urls U 
        //inner join customer c on u.userid=c.username
        //left join stats S on U.ID = S.SHORTURL_ID 
        //where U.added between '" + f + "' and '" + t + @"' 
        //AND U.mobtrack <> 'Y' ";
        //            if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dlt + "' ";
        //            sql = sql + " GROUP BY U.USERID,U.SEGMENT ";
        //            DataTable dt = database.GetDataTable(sql);
        //            return dt;
        //        }

        public DataTable GetUserReportDetail(string userid, string id)
        {
            bool mobtrk = false;
            //d b Y : h i p

            string sql = "";
            if (mobtrk)
            {
                sql = @"SELECT 0 as SLNO,m.mobile,DATE_FORMAT(DATE_ADD(m.sentdate, INTERVAL 750 MINUTE), '%d-%b-%Y %h:%i %p') as smsdate,
DATE_FORMAT(DATE_ADD(s.click_date, INTERVAL 750 MINUTE), '%d-%b-%Y %h:%i %p')
as ClickDate, s.ip, s.referer,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
FROM mobtrackurl m left join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
where m.urlid = '" + id + "' order by click_date Limit 1000000";
            }
            else
            {
                sql = @"select row_number() over (Order by click_date desc) as Sln, convert(varchar,click_date,106) + ' ' + convert(varchar,click_date,108)  as ClickDate, ip, referer,
            Browser, Platform, IsMobileDevice, MobileDeviceManufacturer, MobileDeviceModel from stats
            where shorturl_id = (select id from short_urls where segment ='" + id.Replace(@"/", "") + "') order by click_date desc ";
            }
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);

            return dt;
        }

        public string getIPapikey()
        {
            return database.getIPapikey();
        }

        public void SaveIPLocation(DataTable dt, string segment, string mobstat, string ip, string ipall, string iprem)
        {
            if (dt.Rows.Count > 0)
            {
                string sql = " INSERT INTO iplocation (Atas,city,country,countryCode,isp,lat,lon,org,query,region,regionName,status,timezone,zip,segment,mobtrk,ipadd,ipall,iprem) values " +
                    "('" + dt.Rows[0]["as"].ToString() +
                    "','" + (dt.Rows[0]["countryCode"].ToString().Trim().ToUpper() == "CA" ? "India" : dt.Rows[0]["city"].ToString()) +
                    "','" + dt.Rows[0]["country"].ToString() +
                    "','" + dt.Rows[0]["countryCode"].ToString() +
                    "','" + dt.Rows[0]["isp"].ToString() +
                    "','" + dt.Rows[0]["lat"].ToString() +
                    "','" + dt.Rows[0]["lon"].ToString() +
                    "','" + dt.Rows[0]["org"].ToString() +
                    "','" + dt.Rows[0]["query"].ToString() +
                    "','" + dt.Rows[0]["region"].ToString() +
                    "','" + (dt.Rows[0]["countryCode"].ToString().Trim().ToUpper() == "CA" ? "India" : dt.Rows[0]["regionName"].ToString()) +
                    "','" + dt.Rows[0]["status"].ToString() +
                    "','" + dt.Rows[0]["timezone"].ToString() +
                    "','" + dt.Rows[0]["zip"].ToString() +
                    "','" + segment +
                    "','" + mobstat +
                    "','" + ip +
                    "','" + ipall +
                    "','" + iprem +
                    "')";
                database.ExecuteNonQuery(sql);
            }
        }

        public void SaveIPLocationNew(string mobile, string segment, string operators, bool ported, bool roaming, bool permanent, string countryName, string cityName)
        {
            string sql = string.Format("INSERT INTO iplocation(mobile,segment,operator,ported,roaming,permanent,country, regionName) values('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}')", mobile, segment, operators, ported == false ? 0 : 1, roaming == false ? 0 : 1, permanent == false ? 0 : 1, countryName, cityName);
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetSenderIdList(string f, string t, string usertype, string useridlogin, string user, string comp)
        {
            string sql = "";
            string dlt = "";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + useridlogin + "'"));
                sql = @"select row_number() over (Order by ACCOUNTCREATEDON DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile,C.email,C.senderid as sender,C.username from customer C where dltno = '" + dlt + "' AND username like '%" + user + "%' and compname like '%" + comp + "%' ORDER BY ACCOUNTCREATEDON DESC "; /*ACCOUNTCREATEDON BETWEEN '" + f + "' AND '" + t + "' */
            }
            if (usertype == "SYSADMIN")
            {
                //dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = @"select row_number() over (Order by ACCOUNTCREATEDON DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile,C.email,C.senderid as sender,C.username from customer C where username like '%" + user + "%' and compname like '%" + comp + "%' ORDER BY ACCOUNTCREATEDON DESC "; /*where ACCOUNTCREATEDON BETWEEN '" + f + "' AND '" + t + "'*/
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public bool CheckSenderIDforAdmin(string s, string admin)
        {
            int x = 0;
            x = Convert.ToInt16(database.GetScalarValue("Select count(SENDERID) FROM senderidmast WHERE SENDERID = '" + s + "' and userid='" + admin + "' "));
            return (x == 0 ? false : true);
        }

        public bool CheckSenderID(string s)
        {
            int x = 0;
            x = Convert.ToInt16(database.GetScalarValue("Select count(SENDERID) FROM senderidmast WHERE SENDERID = '" + s + "' "));
            return (x == 0 ? false : true);
        }

        public void UpdateSender(string un, string s, string user, string updtype, string ccode)
        {
            //"UPDATEONLY"
            string sql = "" +
                " DECLARE @SENDER VARCHAR(100)" +
                " SELECT @SENDER = SENDERID FROM CUSTOMER WHERE USERNAME = '" + un + "'; " +
                " Update customer set senderid = '" + s + "' where username ='" + un + "'; " +
                " INSERT INTO SENDERIDALLOTLOG (OLDSENDERID, NEWSENDERID, ALLOTBYUSER) VALUES (@SENDER, '" + s + "','" + user + "'); " +
                " IF NOT EXISTS (SELECT * FROM senderidmast WHERE USERID='" + un + "' AND SENDERID='" + s + "' AND countrycode='" + ccode + "' ) INSERT INTO senderidmast (USERID, SENDERID,countrycode) VALUES ('" + un + "','" + s + "','" + ccode + "') ";
            if (updtype == "UPDATEANDINSERT") sql = sql + " insert into senderidmaster (senderid, createdby, createddate,countrycode) values ('" + s + "','" + user + "',getdate(),'" + ccode + "'); ";
            database.ExecuteNonQuery(sql);
        }

        public int CountSenderId(string s)
        {
            return Convert.ToInt16(database.GetScalarValue(string.Format("Select count(SENDERID) FROM senderidmast WHERE userid = '{0}'", s)));
        }

        public void RemoveSender(string s, string user)
        {
            string sql = string.Format("delete from senderidmast WHERE SENDERID = '{0}' AND userid='{1}'", s, user);
            sql += " DECLARE @SENDER VARCHAR(100) ";
            sql += string.Format("SELECT top 1 @SENDER = senderid from senderidmast WHERE userid = '{0}' ;", user);
            sql += " Update customer set senderid =  @SENDER where username ='" + user + "'; ";
            sql += " INSERT INTO SENDERIDREMOVELOG (SENDERID, REMOVEBYUSER) VALUES ('" + s + "','" + user + "'); ";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetSenderIdListForApproval(string f, string t, string usertype, string dltno)
        {
            string sql = "";

            sql = @"select row_number() over (Order by s.createdat DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile,C.email,s.senderid as sender,C.username,s.filepath,s.countrycode " +
            " from customer C inner join senderidrequeset s on c.username=s.username where (isnull(s.rejected,0)=0 and isnull(s.allotedsenderid,'')='') and  s.createdat BETWEEN '" + f + "' AND '" + t + "' ";


            if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dltno + "' ";

            sql = sql + " ORDER BY s.createdat DESC";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void ApproveRejectSenderId(string sender, string username, string user, string type, string ccode)
        {
            string sql = "";
            if (type == "REJECT")
                sql = "Update senderidrequeset set REJECTed=1, ALLOTEDBY='" + user + "', ALLOTEDON=GETDATE() WHERE USERNAME='" + username + "' AND SENDERID='" + sender + "' AND countrycode='" + ccode + "' ";
            if (type == "APPROVE")
                sql = "Update senderidrequeset set REJECTed=0, ALLOTEDBY='" + user + "', ALLOTEDON=GETDATE(),ALLOTEDSENDERID='" + sender + "' WHERE USERNAME='" + username + "' AND SENDERID='" + sender + "' AND countrycode='" + ccode + "' ; if not exists (select * from senderidmast where userid='" + username + "' and senderid='" + sender + "' AND countrycode='" + ccode + "' ) insert into senderidmast (userid,senderid,countrycode) values ('" + username + "','" + sender + "','" + ccode + "')  ";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetTemplateListForApproval(string f, string t, string usertype, string user, string dltno)
        {
            string sql = "";

            sql = @"select row_number() over (Order by s.createdat DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile, C.email, isnull(s.Templateid,'') as templateid, s.Template as template,C.username,s.filepath,s.senderId,s.TempName " +
            " from customer C inner join templaterequest s on c.username=s.username where (isnull(s.rejected,0)=0 and isnull(s.allotted,'')='') and  s.createdat BETWEEN '" + f + "' AND '" + t + "'";


            //if (usertype == "USER") sql = sql + " and c.USERNAME = '" + user + "' ";
            if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dltno + "' ";

            //if (usertype == "BD")
            //{
            //    sql = sql + "and c.empcode='" + empcode + "' ";
            //}
            sql = sql + " ORDER BY s.createdat DESC";



            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateId1(string userid)
        {
            string sql = "";
            sql = @"select isnull(tempname,'') [TemplateID],TemplateID as template from templaterequest where username='" + userid + "' and isnull(allotted ,0)=1 and isnull(TemplateID,'')<>'' order by templateid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateId(string userid, string hidetempid = "")
        {
            string sql = "";
            if (hidetempid == "True")
            {
                sql = @"select isnull(tempname,'') [TemplateID], TemplateID as template,TemplateID as onlyTemplateID from templaterequest where username='" + userid + "' and isnull(allotted ,0)=1 and isnull(IsWhatsapp,0)=0 and isnull(TemplateID,'')<>'' order by templateid";

            }
            else
            {
                sql = @"select Concat(TemplateID ,' ' ,isnull(tempname,'')) [TemplateID],TemplateID as template,TemplateID as onlyTemplateID from templaterequest where username='" + userid + "' and isnull(allotted ,0)=1 and isnull(IsWhatsapp,0)=0 and isnull(TemplateID,'')<>'' order by templateid";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        //public void ApproveRejectTemplate(string template, string username, string user, string type)
        //{
        //    string sql = "";
        //    if (type == "REJECT")
        //        sql = "Update templaterequest set REJECTed=1, doneby='" + user + "', donedate=GETDATE() WHERE USERNAME='" + username + "' AND template=N'" + template.Replace("'", "''") + "' ";
        //    if (type == "APPROVE")
        //        sql = "Update templaterequest set allotted=1, doneby='" + user + "', donedate=GETDATE() WHERE USERNAME='" + username + "' AND template=N'" + template.Replace("'", "''") + "' ";
        //    database.ExecuteNonQuery(sql);
        //}
        public void ApproveRejectTemplate(string template, string username, string user, string type, string senderId = "", string templateName = "", string templateId = "")
        {
            string sql = "";
            if (type == "REJECT")
                sql = "Update templaterequest set REJECTed=1, doneby='" + user + "', donedate=GETDATE() WHERE USERNAME='" + username + "' AND template=N'" + template.Replace("'", "''") + "' ";
            if (type == "APPROVE")
            {
                sql = "Update templaterequest set allotted=1, doneby='" + user + "', donedate=GETDATE() WHERE USERNAME='" + username + "' AND template=N'" + template.Replace("'", "''") + "' ";

                string tmpWord = template.Replace("{#var#}", ";").Replace(" ;", ";").Replace("; ", ";").Replace(";;", ";").Replace(Environment.NewLine, ";").Replace(";;", ";");

                SaveTemplateInTemplateId(senderId, template, tmpWord, templateName, templateId);
            }
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetUserListForBlockUnBlock(string usertype, string user, string status)
        {
            string f = "", t = "";
            string sql = "";
            string dlt = "";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = @"select row_number() over (Order by ACCOUNTCREATEDON DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile,C.email,C.senderid as sender,C.username,'" + status + "' as status from customer C where dltno = '" + dlt + "' /* AND ACCOUNTCREATEDON BETWEEN '" + f + "' AND '" + t + "' */ and active = " + (status == "BLOCKED" ? "0" : "1") + " ORDER BY ACCOUNTCREATEDON DESC ";
            }
            if (usertype == "SYSADMIN")
            {
                //dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = @"select row_number() over (Order by ACCOUNTCREATEDON DESC) as Sln, C.compname,C.mobile1 as mobile,C.email,C.senderid as sender,C.username,'" + status + "' as status from customer C  where /* ACCOUNTCREATEDON BETWEEN '" + f + "' AND '" + t + "' and */ active = " + (status == "BLOCKED" ? "0" : "1") + " ORDER BY ACCOUNTCREATEDON DESC ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void BlockUnblockUser(string username, string user, string type)
        {
            string sql = "";
            sql = "Update Customer set active=" + (type == "BLOCK" ? "0" : "1") + " WHERE USERNAME='" + username + "' ; insert into blockunblocklog (username,blockunblock,doneby,donedate) values ('" + username + "','" + (type == "BLOCK" ? "B" : "U") + "','" + user + "',getdate()) ; ";
            database.ExecuteNonQuery(sql);
        }

        //--------------------DASHBARD ----
        public DataTable GetDashboardSummary(string user)
        {

            string sql = @"select Convert(varchar,isnull(DATEADD(mi," + Global.addMinutes + @", updtime),getdate()),106) + ' ' + Convert(varchar(5),isnull(DATEADD(mi," + Global.addMinutes + @", updtime),getdate()),108) as updtime,
isnull(smssubmitted,0) as smssubmitted,isnull(smsdelivered,0) as smsdelivered,isnull(smsfailed,0) as smsfailed,
isnull(smsunknown,0) as smsunknown,isnull(links,0) as links,isnull(clicks,0) as clicks,isnull(smsclicks,0) as smsclicks from dashboard where userid='" + user + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void UpdateDashboard(string user, string s, string d, string f, string l, string c, string m)
        {
            string sql = @"Update Dashboard set updtime=getdate(),smssubmitted='" + s + "',smsdelivered='" + d + "',smsfailed='" + f + "',smsunknown='" + 0 + "',links='" + l + "',clicks='" + c + "',smsclicks='" + m + "' where userid='" + user + "'";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetSMSSummary(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            //            sql = @"select count(s.id) submitted, 
            //sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
            //sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            //sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
            //                 from customer c with(nolock)
            //                inner
            //                 join MSGSUBMITTED s with(nolock) on c.username = s.profileid 
            //                left join delivery d with(nolock) on s.msgid = d.msgid
            //                where s.createdat between '" + f + "' and '" + t + @"' ";
            sql = @"select count(s.id) submitted, 
            sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
            sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
                             from customer c 
                            inner
                             join MSGSUBMITTED s  on c.username = s.profileid 
                            left join delivery d  on s.msgid = d.msgid
                            where s.SENTDATETIME between '" + f + "' and '" + t + @"' ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            if (usertype == "BD")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 EMPCODE from customer where username='" + user + "'"));
                sql = sql + " and c.EMPCODE = '" + dlt + "' ";
            }
            if (usertype == "USER")
            {
                sql = sql + " and c.USERNAME = '" + user + "' ";
            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetURLSummary(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            //            sql = @"select count(distinct U.id) urls, 
            //sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
            //                 from customer c with(nolock)
            //                inner join short_urls U with(nolock) on c.username = U.userid 
            //                left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
            //                where U.ADDED between '" + f + "' and '" + t + @"' ";

            sql = @"select count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
                 from customer c 
                inner join short_urls U  on c.username = U.userid 
                left join stats S  on U.ID = S.SHORTURL_ID 
                where U.ADDED between '" + f + "' and '" + t + @"' ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            if (usertype == "USER")
            {

                sql = sql + " and c.username = '" + user + "' ";
            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSClickSummary(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            //            sql = @"SELECT COUNT (MS.ID) FROM CUSTOMER C with(nolock)
            //inner join short_urls U with(nolock) on c.username = U.userid
            //INNER JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID
            //                where ms.click_date between '" + f + "' and '" + t + @"' ";
            sql = @"SELECT COUNT (MS.ID) FROM CUSTOMER C 
inner join short_urls U on c.username = U.userid
INNER JOIN MOBSTATS MS ON U.ID=MS.SHORTURL_ID
                where ms.click_date between '" + f + "' and '" + t + @"' ";
            if (usertype == "ADMIN")
            {
                //dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer with(nolock) where username='" + user + "'"));
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            if (usertype == "USER")
            {
                sql = sql + " and c.username = '" + user + "' ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetAccountSummary(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @" SELECT COUNT (c.username) FROM CUSTOMER c
                where c.ACCOUNTCREATEDON between '" + f + "' and '" + t + @"' ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        public string GetAccountSummaryLastMonth(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @" SELECT COUNT (c.username) FROM CUSTOMER c
                where convert(varchar,c.ACCOUNTCREATEDON,102) >= CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),102)
                and convert(varchar,c.ACCOUNTCREATEDON,102) <= CONVERT(varchar,dateadd(d,-(day(getdate())),getdate()),102) ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        public string GetCreditSummary(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @" select sum(u.balance) from userBalCrDr u inner join customer c on u.username=c.username
                where u.trantype='C' and u.Trandate between '" + f + "' and '" + t + @"' ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        public string GetCreditSummaryLastMonth(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @" select sum(u.balance) from userBalCrDr u inner join customer c on u.username=c.username
                where u.trantype='C' and convert(varchar,u.trandate,102) >= CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),102)
                and convert(varchar,u.trandate,102) <= CONVERT(varchar,dateadd(d,-(day(getdate())),getdate()),102) ";
            if (usertype == "ADMIN")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        public string GetActiveAccountSummary(string f, string t, string usertype, string user, string stat)
        {
            string sql = "";
            string dlt = "";
            string ActiveCriteriaDays = Convert.ToString(database.GetScalarValue("select ActiveCriteriaDays from settings with(nolock)"));
            if (stat.ToString() == "1")
            {
                //sql = @"select count(distinct PROFILEID) from MSGSUBMITTEDLOG with(nolock) where SENTDATETIME >= DATEADD(D,-" + ActiveCriteriaDays + ",GETDATE())";
                sql = @"select count(*) from  settings";
            }
            else
            {
                sql = @"select count(*) from customer c left join
(
select userid from daysummary where smsdate>=DATEADD(D,-" + ActiveCriteriaDays + @",GETDATE())
group by userid) m on c.username=m.userid
where m.userid is null";

                //sql = @" SELECT count(c.username) FROM CUSTOMER c
                //where c.active = " + stat;
                if (usertype == "ADMIN")
                {
                    dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where username='" + user + "'"));
                    sql = sql + " and c.dltno = '" + dlt + "' ";
                }
            }
            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        //--------------------------------------USER PANEL

        public DataTable GetActiveCountry(string usr)
        {
            DataTable dt = new DataTable("dt");
            string sql = "select s.countrycode,c.name + ' - ' + s.countrycode as name from smsrateaspercountry s inner join countryMast c ON s.countrycode = c.phonecode where s.username ='" + usr + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSenderId(string usr, string ccode = "")
        {
            DataTable dt = new DataTable("dt");
            string sql = "";
            sql = "Select Senderid from senderidmast where userid='" + usr + "'";
            if (ccode != "")
            {
                sql = sql + " and countrycode='" + ccode + "'";
            }
            sql = sql + " order by 1";
            //}
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSenderIduservalnull(string ccode = "")
        {
            DataTable dt = new DataTable("dt");
            string sql = "Select Senderid from senderidmast ";
            if (ccode != "")
            {
                sql = sql + " and countrycode='" + ccode + "'";
            }
            sql = sql + " order by 1";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSType(string usr)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"
SELECT 'Premium' AS SMSTYPE, '1' AS SMSVAL
                            UNION ALL
SELECT 'Link Text' AS SMSTYPE, '2' AS SMSVAL 
                            UNION ALL
                            SELECT 'Google RCS' AS SMSTYPE, '7' AS SMSVAL 
                            UNION ALL
                            SELECT 'Flash SMS' AS SMSTYPE, '8' AS SMSVAL 
                            UNION ALL
                            SELECT 'Promotional' AS SMSTYPE, '6' AS SMSVAL ";
            if (Convert.ToInt16(database.GetScalarValue("Select count(*) from customer with(nolock) where username='" + usr + "' and campaign_applicable=1 ")) > 0)
                sql = sql + @" UNION ALL 
                SELECT 'Campaign' AS SMSTYPE, '3' AS SMSVAL ";

            //sql = sql + @" UNION ALL SELECT 'Flash SMS' AS SMSTYPE, '5' AS SMSVAL ";

            //sql = sql + @" UNION ALL 
            //    SELECT 'Campaign' AS SMSTYPE, '3' AS SMSVAL ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public Int32 GetUrlID(string UserID, string segm)
        {
            Int32 s = Convert.ToInt32(database.GetScalarValue("select id from short_urls where segment='" + segm + "' and userid = '" + UserID + "'"));
            return s;
        }

        public string NewSegment8()
        {
            string segment = Guid.NewGuid().ToString().Substring(0, 8);
            return segment;
        }

        public DataTable GetCampaignAccounts()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT SMPPACCOUNTID FROM SMPPSETTING WHERE  ACTIVE=1";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetPromotionAccounts()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT SMPPACCOUNTID FROM SMPPSETTING WHERE Forpromotional=1 AND ACTIVE=1";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetGoogleRCSAccounts()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT SMPPACCOUNTID FROM SMPPSETTING WHERE ForRCS=1 AND ACTIVE=1";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetFlashSMSAccounts()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT SMPPACCOUNTID FROM SMPPSETTING WHERE ForFlash=1 AND ACTIVE=1";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetUniversalTemplateId()
        {
            return Convert.ToString(database.GetScalarValue("select universalTempId from settings"));
        }

        public string GetSqlAccounts(DataTable dtAc)
        {
            string sql = "";
            if (dtAc.Rows.Count > 0)
            {
                sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where n.active=1 and S.ACTIVE=1 AND isnull(s.dltno,'') = '" + dtAc.Rows[0]["DLTNO"].ToString() + "' ";
                if (dtAc.Rows[0]["GSM"].ToString() == "Y")
                    sql = sql + " and s.smppaccountid in (" + dtAc.Rows[0]["smppaccountidall"].ToString() + ") ";
                else
                    sql = sql + " and s.smppaccountid = '" + dtAc.Rows[0]["smppaccountid"].ToString() + "'  ";   //and right(n.sessionid,2)<>'05'
            }
            else
                sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.active=1 and n.active=1 AND isnull(s.dltno,'') = '' and s.forfile='1' ";
            return sql;
        }

        public string GetTemplateTestAccounts()
        {
            string sql = "Select top 1 n.sessionid as smppaccountid from smppsetting s inner join smppsession n " +
                          "ON s.smppaccountid=n.smppaccountid and s.ACTIVE=n.ACTIVE " +
                          "where n.active = 1 and S.ACTIVE = 1 and s.forfile = 1";
            return Convert.ToString(database.GetScalarValue(sql));

        }

        public void checkNumberDigitsAndUpdate(string user, string colnm)
        {
            Int32 cn14 = Convert.ToInt32(database.GetScalarValue("Select max(len([" + colnm + "])) from " + user));
            if (cn14 > 12)
            {
                database.ExecuteNonQuery("Update " + user + " set [" + colnm + "] = right([" + colnm + "],12)");
            }
        }

        public void GetSchedule_SMS(List<string> liScheduleDates, string userId, string country_code)
        {
            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + userId + "' ";
            double timedifferenceInMinute = Convert.ToDouble(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in (" + q1 + ")"));

            string firstsch = liScheduleDates.FirstOrDefault();

            string firstSchdate = Convert.ToDateTime(firstsch, CultureInfo.InvariantCulture).AddMinutes(Math.Abs(timedifferenceInMinute)).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            string sqlTemp = string.Format("Select * into #tempSchedule From MsgSchedule where CONVERT(varchar(16),schedule,20)='{0}' AND createdat > dateadd(minute, -30, GetDate()) AND PROFILEID='{1}'", firstSchdate, userId);
            string sql1 = "";
            foreach (string scheduleDate in liScheduleDates.Skip(1))
            {
                //  sql1 += string.Format("update #tempSchedule set schedule='{0}';", scheduleDate);

                sql1 += "INSERT INTO MsgSchedule(PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,TemplateID,msgid,blockno,blockfail,ERR_CODE,TranTableName) " +
                        " SELECT PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + scheduleDate + "'),MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,TemplateID,msgid,blockno,blockfail,ERR_CODE,TranTableName from #tempSchedule ;";
                // sql1 += " TRUNCATE TABLE #tempSchedule ";
                double bal = CalculateSMSCost(noof_message, msg_rate);
                // double bal = CalculateAmount(userId, noof_message, msg_rate, 1);
                sql1 = sql1 + " ;update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
            }
            string sql = string.Format("{0} ; {1}", sqlTemp, sql1);
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in GetSchedule_SMS method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        // --->-----------------SMS SENDING METHODS ---------------
        public void Schedule_SMS(string UserID, string mobile, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, string segment, double rate, string SMSType, DataTable dtAc, bool ucs2, string TemplateID, string country_code)
        {
            mobile = country_code + mobile;
            if (mobile.Length > 12)
                mobile = mobile.Substring(mobile.Length - 12);
            //change SMPP ACCOUNT based on SMSTYPE
            msg = msg.Replace("'", "''");
            string ACid = "101";

            if (dtAc.Rows.Count > 0) ACid = dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + "01";
            string peid = getPEid(UserID);

            bool b = checkblacklistno(UserID, mobile);
            //if(b) b= checkwhitelistno(UserID, mobile);

            string dataCode = "";
            if (SMSType == "8")
            {
                if (ucs2)
                    dataCode = "UnicodeFlashSMS";
                else
                    dataCode = "DefaultFlashSMS";
            }
            else
            {
                if (ucs2)
                    dataCode = "UCS2";
                else
                    dataCode = "Default";
            }

            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + UserID + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));
            //RACHIT 03-02-22
            // --->>
            //string sql = @"INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,peid,datacode,blacklist,TemplateID)
            //VALUES ('VCON','" + ACid + "','" + UserID + "',N'" + msg + "','" + mobile + "','" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
            //(shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "','" + segment + "','" + rate.ToString() + "','" + SMSType + "','" + domain + "','" + peid + "','" + dataCode + "','" + (b ? 1 : 0) + "','" + TemplateID + "')";

            string sql = " Insert into SMSFILEUPLOAD (USERID,senderid,schedule,campaignname,SMSRATE,shortURLId,COUNTRYCODE) values ('" + UserID + "','" + s + "','" + schdate + "','Manual','" + rate + "','" + Convert.ToString(shortURLId) + "','" + country_code + "')" +
                  " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ;";

            sql = sql + @" INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,peid,datacode,blacklist,TemplateID,FILEID)
            VALUES ('VCON','" + ACid + "','" + UserID + "',N'" + msg + "','" + mobile + "','" + s + "',GETDATE(),dateadd(minute, " + timedifferenceInMinute + ", '" + schdate + "'),'" +
            (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "','" + segment + "','" + rate.ToString() + "','" + SMSType + "','" + domain + "','" + peid + "','" + dataCode + "','" + (b ? 1 : 0) + "','" + TemplateID + "',@id)";
            // <<----


            double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
            //  double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), 1);
            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Schedule_SMS method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void SendURL_SMS(string UserID, string mobile, string msg, string s, DataTable dtAc, bool ucs2, bool bulk, double rate, int noofsms, string TemplateID, string countryCode, string SMSType, string fileId = "")
        {
            mobile = countryCode + mobile;
            if (mobile.Length > 12)
                mobile = mobile.Substring(mobile.Length - 12);
            //change SMPP ACCOUNT based on SMSTYPE
            string peid = getPEid(UserID);
            msg = msg.Replace("'", "''");
            string ACid = "";
            if (bulk)
            {
                ACid = "105";
                if (dtAc.Rows.Count > 0) ACid = dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + "01";
            }
            else
            {
                ACid = "301";
                if (dtAc.Rows.Count > 0)
                {
                    ACid = dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + "05";
                }

            }


            string sql = "";

            if (checkblacklistno(UserID, mobile))
            {
                sql = @"DECLARE @nid varchar (100)  ";
                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + @" select @nid=newid()
                    Insert into msgsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smstext, smsrate) " +
                    " select '1' as id,'vcon','" + ACid + "','" + UserID + "',N'" + smsTex + "','" + mobile + "' as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),@nid,getdate(),'" + fileId + "' as fileid,'1'," +
                    " N'" + msg + "','" + rate.ToString() + "' from settings ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + @nid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, @nid, GETDATE(), 'BlackList','250',getdate()
                    FROM settings ";
                }
            }
            else
            {
                string st1 = checMobProcessNo(UserID, mobile);
                if (st1 != "")
                {
                    string[] st2 = st1.Split(';');

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + @" select @nid=newid()
                        Insert into msgsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smstext, smsrate) " +
                        " select '1' as id,'vcon','" + ACid + "','" + UserID + "',N'" + smsTex + "','" + mobile + "' as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),@nid,getdate(),'" + fileId + "' as fileid,'1'," +
                        " N'" + msg + "','" + rate.ToString() + "' from settings ; ";
                        if (st2[1] == "F")
                        {
                            sql = sql + @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + @nid + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + st2[0] + @" text:' AS DLVRTEXT, @nid, GETDATE(), 'Undeliverable','" + st2[0] + @"',getdate()
                            FROM settings ";
                        }
                        else
                        {
                            sql = sql + @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + @nid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, @nid, GETDATE(), 'Delivered','000',getdate()
                            FROM settings ";
                        }
                    }
                }

                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }
                sql = @"INSERT INTO [dbo].[MSGTRAN]
                ([PROVIDER],[SMPPACCOUNTID],[PROFILEID],[MSGTEXT],[TOMOBILE],[SENDERID],[CREATEDAT],[PICKED_DATETIME],[peid],[DATACODE],[smsrate],[TemplateID],FILEID) VALUES
                ('BSNL', '" + ACid + "', '" + UserID + @"',N'" + msg.Trim() + @"', '" + mobile + @"', '" + s + @"', GETDATE(), NULL,'" + peid + "','" + dataCode + "','" + rate.ToString() + "','" + TemplateID + "','" + fileId + "')";
            }

            // double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), smscount);
            double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));

            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in SendURL_SMS method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }

        }

        public void Schedule_SMS_BULK(string UserID, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, double rate, string SMSType, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, List<string> mobList, string manual, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "")
        {
            string user = "tmp_" + UserID;

            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + m + "')");
                }
                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + UserID + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + UserID + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");
            }


            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + UserID + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));


            string colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));

            string sqlUpd = "if exists (select * from sys.tables where name='" + user + @"') update " + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ";
            database.ExecuteNonQuery(sqlUpd);

            checkNumberDigitsAndUpdate(user, colnm);

            string sql = GetSqlAccounts(dtAc);

            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
                " alter table " + user + @" add smppaccountid numeric(10)"); //add new column

            //  database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }
            double CampaignCost = PrevBalance - AvailableBalance;
            //change SMPP ACCOUNT based on SMSTYPE
            msg = msg.Replace("'", "''");
            string peid = getPEid(UserID);
            sql = @"Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,schedule,campaignname,SMSRATE,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values ('" + UserID + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + schdate + "','" + campnm + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; " +
                " INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,TemplateID) " +
            " select 'VCON',u.smppaccountid,'" + UserID + "',N'" + msg + "',u.[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
            (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "'," + (shortURLId > 0 ? "left(NEWID(),8)" : "null") + ",'" + rate.ToString() + "','" + SMSType + "','" +
            domain + "',@id,'" + peid + "','" + (ucs2 ? "UCS2" : "Default") + "',case when b.mobileno is not null then 1 else 0 end as blacklist,'" + TemplateID + "' as TemplateID from " + user + " u left join blacklistno b on u.[" + colnm + "]=b.mobileno and b.userid='" + UserID + "' where u.[" + colnm + "] is not null ; ";

            double Bper = GetBlockSMSper(UserID, "B");
            if (Bper != 0)
            {
                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockno=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(UserID) + ") ";
            }
            double Fper = GetBlockSMSper(UserID, "F");
            if (Fper != 0)
            {
                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockfail=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 AND isnull(BLOCKNO,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(UserID) + ") ";
            }
            bool ClkBlk = ClickListUser(UserID);
            string isdlr = "";
            if ((ClkBlk && SMSType == "2") || (ClkBlk && (SMSType == "3" || SMSType == "6") && shortURLId > 0))
            {
                DataTable dtCl = database.GetDataTable("Select * from ClickDataBlock where userid='" + UserID + "'");
                string noofdays = dtCl.Rows[0]["noofdays"].ToString();
                isdlr = dtCl.Rows[0]["ProcessType"].ToString();
                sql = sql + " select t.* into #t12 from MsgSchedule t inner join MobTrackURL m on m.urlid='" + shortURLId.ToString() + "' and t.TOMOBILE=m.mobile inner join (select distinct urlid from mobstats where shorturl_id='" + shortURLId.ToString() + "') s on m.id=s.urlid where t.profileid='" + UserID + "' and t.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and m.sentdate >= DateAdd(Day,-" + noofdays + ",getdate()) ";

                if (isdlr.ToUpper() == "DELIVERED")
                {
                    sql = sql + " update MsgSchedule set blockno=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and tomobile in (select tomobile from #t12) ";
                }
                if (isdlr.ToUpper() == "NOSUBMISSION")
                {
                    sql = sql + " delete d from MsgSchedule d inner join #t12 t on d.tomobile = t.tomobile  where d.profileid='" + UserID + "' and d.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ;  ";
                }
            }
            if (SMSType == "2" || ((SMSType == "3" || SMSType == "6") && shortURLId > 0))
            {
                sql = sql + "; Update MsgSchedule set MSGTEXT=replace(MSGTEXT,shorturl,domain+newsegment) where PROFILEID='" + UserID + "' and SCHEDULE=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ; ";
            }

            bool isMobProcess = MobProcessUser(UserID);
            if (isMobProcess)
            {
                //PROCESS 
                sql = sql + "Update Msgschedule set blockno=1 from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + UserID + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='D' ; ";
                sql = sql + "Update Msgschedule set blockfail=1,ERR_CODE=S.ERR_CODE from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + UserID + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='F' ; ";
            }

            // double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), smscount);
            double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Schedule_SMS_BULK method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void InsertTemplateSMSrecordsFromUSERTMP(string userId, string s, string SMSType, string msg, string filenm, string filenmext, DataTable dtAc, DataTable dtCols, ListBox lstMappedFields, string campnm, bool ucs2, double rate, int noofsms, string schdate, List<string> tempFields, string TemplateID, string country_code, string shUrl = "", string domName = "", int shortURLId = 0, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "")
        {
            string user1 = "tmp1_" + userId;
            string user = "tmp_" + userId;
            string colnm = "";
            string sql = "";
            string peid = getPEid(userId);

            string dataCode = "";
            if (SMSType == "8")
            {
                if (ucs2)
                    dataCode = "UnicodeFlashSMS";
                else
                    dataCode = "DefaultFlashSMS";
            }
            else
            {
                if (ucs2)
                    dataCode = "UCS2";
                else
                    dataCode = "Default";
            }

            sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
                SELECT * INTO " + user + @" FROM " + user1;
            database.ExecuteNonQuery(sql);
            colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
            database.ExecuteNonQuery("update " + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            sql = GetSqlAccounts(dtAc);
            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
                " alter table " + user + @" add smppaccountid numeric(10) , shortsegment varchar(10) "); //add new column

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }

            if (shUrl != "") database.ExecuteNonQuery("update " + user + @" set shortsegment=left(NEWID(),8) ");

            //prepare query with template msg text and field value
            string m = msg.Replace("'", "''");
            if (lstMappedFields.Items.Count > 0)
            {
                foreach (ListItem li in lstMappedFields.Items)
                {
                    string[] s1 = li.Text.Replace(" ---->> ", "$").Split('$');

                    string coltype = Convert.ToString(database.GetScalarValue("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + user + @"' and COLUMN_NAME = '" + s1[1] + "'"));
                    if (coltype.ToUpper().Contains("DATE"))
                        m = m.Replace(s1[0], "' + convert(varchar,[" + s1[1] + "],106) + N'");
                    else if (coltype.ToUpper().Contains("FLOAT"))
                        m = m.Replace(s1[0], "' + CONVERT(nvarchar(255), LTRIM(RTRIM(str(ISNULL([" + s1[1] + "], 0), 20, 0)))) + N'");
                    else
                        m = m.Replace(s1[0], "' + convert(nvarchar(max),[" + s1[1] + "]) + N'");
                    //m = m.Replace(s1[0], "' + [" + s1[1] + "] + N'");
                }
            }
            for (int j = 0; j < tempFields.Count; j++)
                m = m.Replace(tempFields[j], ""); filenm = filenm.Replace("'", "''");

            double CampaignCost = PrevBalance - AvailableBalance;

            if (schdate == "")
            {
                //2 feb change ccode
                sql = @"if exists (select * from sys.tables where name='" + user + @"')  " +
                    " begin " +
                    " Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,TEMPLATEID,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + TemplateID + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
                    " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; ";

                //call sms test function test - get true / false
                if (Global.Istemplatetest == false)
                {
                    sql = sql + " select t.* into #t_012 from " + user + " t " +
                       " delete d from " + user + " d inner join #t_012 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                       " alter table #t_012 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t_012 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t_012 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                     FROM #t_012 ";
                    }
                    if (shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                           "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from " + user + " where [" + colnm + "] is not null ; ";
                    }
                    sql = sql + " end ";
                }
                else
                {
                    double Bper = GetBlockSMSper(userId, "B");
                    if (Bper != 0)
                    {
                        Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                        sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                            " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t10 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t10 set msgid=newid() " +
                            @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate.ToString() + "' from #t10 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," + (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t10 ; ";
                        }
                    }
                    double Fper = GetBlockSMSper(userId, "F");
                    if (Fper != 0)
                    {
                        Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                        sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                            " delete d from " + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t101 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t101 set msgid=newid() " +
                            @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " ,[" + colnm + "] as TOMOBILE," +
                            "'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate.ToString() + "' from #t101 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," + (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t101 ; ";
                        }
                    }
                    bool blu = blacklistuser(userId);
                    if (blu)
                    {
                        sql = sql + " select t.* into #t11 from " + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + userId + "'  where b.MOBILENO is not null and t.[" + colnm + "] is not null " +
                            " delete d from " + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t11 add msgid varchar (100) ; ";
                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                        }
                    }

                    bool isMobProcess = MobProcessUser(userId);
                    if (isMobProcess)
                    {
                        //PROCESS FAIL
                        sql = sql + " select t.*,s.err_code into #mt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                            " delete d from " + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #mt11 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #mt11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #mt11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                            FROM #mt11 ; ";
                        }

                        //PROCESS DELIVERY
                        sql = sql + " select t.* into #Dmt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                            " delete d from " + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #Dmt11 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #Dmt11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #Dmt11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                            FROM #Dmt11 ; ";
                        }
                    }


                    sql = sql + " INSERT INTO MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid)  " +
                        " Select 'VCON' as PROVIDER, smppaccountid, '" + userId + @"' as PROFILEID, " +
                        (shUrl != "" ? " REPLACE(N'" + m + "', '" + shUrl + "', '" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE
                , '" + s + @"' as SENDERID, GETDATE() as CREATEDAT,@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' AS templateid from " + user + " where [" + colnm + "] is not null ; ";


                    if (Bper != 0 && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t10 where [" + colnm + "] is not null ; ";
                    }
                    if (Fper != 0 && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t101 where [" + colnm + "] is not null ; ";
                    }
                    if (blu && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t11 where [" + colnm + "] is not null ; ";
                    }
                    if (shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                           "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from " + user + " where [" + colnm + "] is not null ; ";
                    }
                    sql = sql + " end ";

                    //  double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                    double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                    sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
                }


            }
            else
            {
                string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + userId + "' ";
                int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));

                // int shortURLId = 0;
                string shortURL = ""; string domain = "";
                shortURL = shUrl; domain = domName;
                sql = @"Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,schedule,campaignname,TEMPLATEID,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + schdate + "','" + campnm + "','" + TemplateID + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; " +
                " INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,templateid) " +
                " select 'VCON',u.smppaccountid,'" + userId + "',N'" + m + "',CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL(u.[" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
                (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "'," + (shortURLId > 0 ? "left(NEWID(),8)" : "null") + ",'" + rate.ToString() + "','" + SMSType + "','" +
                domain + "',@id,'" + peid + "','" + dataCode + "',case when b.mobileno is not null then 1 else 0 end as blacklist,'" + TemplateID + "' AS templateid from " + user + " u left join blacklistno b on u.[" + colnm + "]=b.mobileno and b.userid='" + userId + "' where u.[" + colnm + "] is not null ; ";

                double Bper = GetBlockSMSper(userId, "B");
                if (Bper != 0)
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockno=1 where profileid='" + userId + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(userId) + ") ";
                }
                double Fper = GetBlockSMSper(userId, "F");
                if (Fper != 0)
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockFAIL=1 where profileid='" + userId + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and isnull(blockno,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(userId) + ") ";
                }
                if ((SMSType == "2" || SMSType == "3" || SMSType == "6") && shortURLId > 0)
                {
                    sql = sql + "; Update MsgSchedule set MSGTEXT=replace(MSGTEXT,shorturl,domain+newsegment) where PROFILEID='" + userId + "' and SCHEDULE=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ; ";
                }
                bool isMobProcess = MobProcessUser(userId);
                if (isMobProcess)
                {
                    //PROCESS 
                    sql = sql + "Update Msgschedule set blockno=1 from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + userId + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='D' ; ";
                    sql = sql + "Update Msgschedule set blockfail=1,ERR_CODE=S.ERR_CODE from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + userId + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='F' ; ";
                }
                // double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in InsertTemplateSMSrecordsFromUSERTMP method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void InsertSMSrecordsFromUSERTMP(string userId, string s, string SMSType, string msg, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, int noofsms, double Drate, List<string> mobList, string manual, string retarget, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "")
        {
            //change SMPP ACCOUNT based on SMSTYPE
            string user = "tmp_" + userId;
            string sql;

            string colnm = "";

            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + country_code + m + "')");
                }
                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + userId + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + userId + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

            }

            colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));
            if (country_code != "" && manual != "MANUAL")
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') update " + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            string peid = getPEid(userId);

            sql = GetSqlAccounts(dtAc);
            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
               " alter table " + user + @" add smppaccountid numeric(10)"); //add new column

            //  database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }
            msg = msg.Replace("'", "''"); filenm = filenm.Replace("'", "''");

            double CampaignCost = PrevBalance - AvailableBalance;

            sql = @"if exists (select * from sys.tables where name='" + user + @"')  " +
                " begin " +
                " Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,smsrate,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + Drate + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; ";

            string rate = Drate.ToString();

            //call sms test function test - get true / false
            if (Global.Istemplatetest == false)
            {
                sql = sql + " select t.* into #t_011 from " + user + " t " +
                   " delete d from " + user + " d inner join #t_011 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                   " alter table #t_011 add msgid varchar (100) ;  ";

                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + " update #t_011 set msgid=newid() " +
                    " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                    " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                    " N'" + msg + "','" + rate + "' from #t_011 ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                    FROM #t_011 end ";
                }
            }
            else
            {

                double Bper = GetBlockSMSper(userId, "B");
                if (Bper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t101 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t101 set msgid=newid() " +
                            @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                      select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "'," +
                          "GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t101 ; " +
                             " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                             " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                             " N'" + msg + "','" + rate + "' from #t101 ; ";
                    }
                }
                double Fper = GetBlockSMSper(userId, "F");
                if (Fper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t10 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t10 set msgid=newid() " +
                            @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                      select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
                             " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                             " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                             " N'" + msg + "','" + rate + "' from #t10 ; ";
                    }
                }
                if (blacklistuser(userId))
                {
                    sql = sql + " select t.* into #t11 from " + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + userId + "' where b.MOBILENO is not null " +
                        " delete d from " + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t11 add msgid varchar (100) ; ";
                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #t11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                    }
                }

                bool isMobProcess = MobProcessUser(userId);
                if (isMobProcess)
                {
                    //PROCESS FAIL
                    sql = sql + " select t.*,s.err_code into #mt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                        " delete d from " + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #mt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #mt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #mt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                    FROM #mt11 ; ";
                    }

                    //PROCESS DELIVERY
                    sql = sql + " select t.* into #Dmt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                        " delete d from " + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #Dmt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #Dmt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #Dmt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                    FROM #Dmt11 ; ";
                    }
                }
                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }

                sql = sql + " INSERT INTO MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,Templateid)  " +
                    " Select 'VCON' as PROVIDER, smppaccountid, '" + userId + @"' as PROFILEID, N'" + msg + @"' as MSGTEXT, [" + colnm + @"] as TOMOBILE
                , '" + s + @"' as SENDERID, GETDATE() as CREATEDAT,@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' as templateid from " + user + " where [" + colnm + "] is not null ; " +
                    " end ";

                //  double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in InsertSMSrecordsFromUSERTMP method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void Insert_SMS_BULK_4url(string UserID, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, double rate, string SMSType, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, int noofsms, List<string> mobList, string manual, string retarget, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "")
        {
            string user = "tmp_" + UserID;

            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + country_code + m + "')");
                }

                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                string sqlRestict = @"if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + UserID + "' AND TYPE='U')" +
                    " delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id 
                     WHERE UserId='" + UserID + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S')" +
                     " delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'";
                database.ExecuteNonQuery(sqlRestict);

            }

            string colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));
            if (country_code != "" && manual != "MANUAL")
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') update " + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            string peid = getPEid(UserID);
            string sql;
            sql = GetSqlAccounts(dtAc);
            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));
            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }

            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;

            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
                " alter table " + user + @" add smppaccountid numeric(10) , shortsegment varchar(10) "); //add new column

            //database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10), shortsegment varchar(10) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }

            sql = "update " + user + @" set shortsegment=left(NEWID(),8) ";
            database.ExecuteNonQuery(sql);
            //change SMPP ACCOUNT based on SMSTYPE

            double CampaignCost = PrevBalance - AvailableBalance;

            msg = msg.Replace("'", "''"); filenm = filenm.Replace("'", "''");
            sql = @" Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values ('" + UserID + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; ";

            //call sms test function test - get true / false
            if (Global.Istemplatetest == false)
            {
                sql = sql + " select t.* into #t_01 from " + user + " t " +
                    " delete d from " + user + " d inner join #t_01 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                    " alter table #t_01 add msgid varchar (100) ;  ";

                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + " update #t_01 set msgid=newid() " +
                    " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                    " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                    " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t_01 ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                    FROM #t_01 ; ";
                }

                sql = sql + " insert into mobtrackurl(urlid, mobile, sentdate, segment,fileid,templateid) select " +
               "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid from #t_01 where [" + colnm + "] is not null ";

            }
            else
            {
                double Bper = GetBlockSMSper(UserID, "B");
                if (Bper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(UserID) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t10 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t10 set msgid=newid() " +
                        @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,'" + rate + "' from #t10 ; ";
                    }
                }
                double Fper = GetBlockSMSper(UserID, "F");
                if (Fper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(UserID) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t101 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t101 set msgid=newid() " +
                        @" insert into FAILSUBMITTED (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE," +
                            "'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t101 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,'" + rate + "' from #t101 ; ";
                    }
                }
                bool blu = blacklistuser(UserID);
                if (blu)
                {
                    sql = sql + " select t.* into #t11 from " + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + UserID + "' where b.MOBILENO is not null " +
                        " delete d from " + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                    }
                }

                bool isMobProcess = MobProcessUser(UserID);
                if (isMobProcess)
                {
                    //PROCESS FAIL
                    sql = sql + " select t.*,s.err_code into #mt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + UserID + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                        " delete d from " + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #mt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #mt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #mt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                    FROM #mt11 ; ";
                    }

                    //PROCESS DELIVERY
                    sql = sql + " select t.* into #Dmt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + UserID + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                        " delete d from " + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #Dmt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #Dmt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #Dmt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                    FROM #Dmt11 ; ";
                    }
                }

                bool ClkBlk = ClickListUser(UserID);
                string isdlr = "";
                if (ClkBlk)
                {
                    DataTable dtCl = database.GetDataTable("Select * from ClickDataBlock where userid='" + UserID + "'");
                    string noofdays = dtCl.Rows[0]["noofdays"].ToString();
                    isdlr = dtCl.Rows[0]["ProcessType"].ToString();
                    sql = sql + " select t.* into #t12 from " + user + " t inner join MobTrackURL m on m.urlid='" + shortURLId.ToString() + "' and t.[" + colnm + "]=m.mobile inner join (select distinct urlid from mobstats where shorturl_id='" + shortURLId.ToString() + "') s on m.id=s.urlid where m.sentdate >= DateAdd(Day,-" + noofdays + ",getdate()) ";
                    sql = sql + " delete d from " + user + " d inner join #t12 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  alter table #t12 add msgid varchar (100) ;  ";
                    if (isdlr.ToUpper() == "DELIVERED")
                    {
                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t12 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t12 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                        ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                        FROM #t12 ; ";
                        }
                    }
                    //if (isdlr.ToUpper() == "NOSUBMISSION")
                    //{
                    //    sql = sql + " delete d from " + user + " d inner join #t12 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  ";
                    //}
                }

                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }

                sql = sql + @" INSERT INTO MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid)
                select 'VCON',smppaccountid,'" + UserID + "',REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' as templateid from " + user + " where [" + colnm + "] is not null ";
                //database.ExecuteNonQuery(sql);
                if (Bper != 0 && retarget == "")
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid from #t10 where [" + colnm + "] is not null ";
                }
                if (Fper != 0 && retarget == "")
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid   from #t101 where [" + colnm + "] is not null ";
                }
                if (blu)
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t11 where [" + colnm + "] is not null ";
                }
                if (ClkBlk)
                {
                    if (isdlr.ToUpper() == "DELIVERED")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                        "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t12 where [" + colnm + "] is not null ";
                    }
                }
                sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from " + user + " where [" + colnm + "] is not null ";

                // double bal = CalculateAmount(UserID, noof_message, rate, smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";

            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Insert_SMS_BULK_4url method !  ", ex.Message + " - " + ex.StackTrace + " -- " + sql);
                throw ex;
            }

        }
        // ---<-------------- SMS SENDING METHODS ----------------


        // --->-----------------SMS SENDING METHODS ---------------

        public void B4SEND_Schedule_SMS(string UserID, string mobile, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, string segment, double rate, string SMSType, DataTable dtAc, bool ucs2, string TemplateID, string country_code, string fileId = "", string dlrcode = "")
        {
            mobile = country_code + mobile;
            if (mobile.Length > 12)
                mobile = mobile.Substring(mobile.Length - 12);
            //change SMPP ACCOUNT based on SMSTYPE
            msg = msg.Replace("'", "''");
            string ACid = "101";
            string TranTableName = "MsgTran_91FU1";

            if (dtAc.Rows.Count > 0)
            {
                //ACid = dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + "01";

                string NewACid = database.GetScalarValue("select ManualAcId from SMPPSETTING where smppaccountid=" + dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + " and active=1 ").ToString();
                if (NewACid != "")
                {
                    string trantbl = database.GetScalarValue("select TranTableName from SMPPSETTING where smppaccountid=" + NewACid + " and active=1 ").ToString();
                    if (trantbl != "")
                    {
                        TranTableName = trantbl;
                    }

                    ACid = NewACid + "01";
                }
            }
            else
            {
                string fileAcId = Convert.ToString(database.GetScalarValue("select top 1 smppaccountid from SMPPSETTING where active=1 and forfile=1 "));
                string NewACid = database.GetScalarValue("select top 1 manualacid from SMPPSETTING where smppaccountid=" + fileAcId + " and active=1 ").ToString();
                if (NewACid != "")
                {
                    string trantbl = database.GetScalarValue("select TranTableName from SMPPSETTING where smppaccountid=" + NewACid + " and active=1 ").ToString();

                    if (trantbl != "")
                    {
                        TranTableName = trantbl;
                    }

                    ACid = NewACid + "01";
                }
            }

            string peid = getPEid(UserID);

            bool b = checkblacklistno(UserID, mobile);
            bool bs = checkblacklistnoSender(s, mobile);
            //if(b) b= checkwhitelistno(UserID, mobile);

            string dataCode = "";
            if (SMSType == "8")
            {
                if (ucs2)
                    dataCode = "UnicodeFlashSMS";
                else
                    dataCode = "DefaultFlashSMS";
            }
            else
            {
                if (ucs2)
                    dataCode = "UCS2";
                else
                    dataCode = "Default";
            }

            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + UserID + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));
            //RACHIT 03-02-22
            // --->>
            //string sql = @"INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,peid,datacode,blacklist,TemplateID)
            //VALUES ('VCON','" + ACid + "','" + UserID + "',N'" + msg + "','" + mobile + "','" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
            //(shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "','" + segment + "','" + rate.ToString() + "','" + SMSType + "','" + domain + "','" + peid + "','" + dataCode + "','" + (b ? 1 : 0) + "','" + TemplateID + "')";

            //string sql = " Insert into SMSFILEUPLOAD (USERID,senderid,schedule,campaignname,SMSRATE,shortURLId,COUNTRYCODE,dlrcode) values ('" + UserID + "','" + s + "','" + schdate + "','Manual','" + rate + "','" + Convert.ToString(shortURLId) + "','" + country_code + "','" + dlrcode + "')" +
            //      " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ;";

            string sql = @" INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,peid,datacode,blacklist,TemplateID,FILEID,TranTableName,dlrcode)
            VALUES ('VCON','" + ACid + "','" + UserID + "',N'" + msg + "','" + mobile + "','" + s + "',GETDATE(),dateadd(minute, " + timedifferenceInMinute + ", '" + schdate + "'),'" +
            (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "','" + segment + "','" + rate.ToString() + "','" + SMSType + "','" + domain + "','" + peid + "','" + dataCode + "','" + (b || bs ? 1 : 0) + "','" + TemplateID + "','" + fileId + "','" + TranTableName + "','" + dlrcode + "')";
            // <<----


            double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
            //  double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), 1);
            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "' ;" +
                " INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal + "',GETDATE(),'" + UserID + "','" + DebitedBalScheduleCreation.Replace("{1}", "Manual").Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Schedule_SMS method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void B4SEND_SendURL_SMS(string UserID, string mobile, string msg, string s, DataTable dtAc, bool ucs2, bool bulk, double rate, int noofsms, string TemplateID, string countryCode, string SMSType, string fileId = "", string HeroDlrCode = "")
        {
            mobile = countryCode + mobile;
            if (countryCode != "49")  //add by vikas
            {
                if (mobile.Length > 12)
                    mobile = mobile.Substring(mobile.Length - 12);
            }
            //change SMPP ACCOUNT based on SMSTYPE
            string peid = getPEid(UserID);
            msg = msg.Replace("'", "''");
            string ACid = "";
            string TranTableName = "MsgTran_91FU1";

            if (bulk)
            {
                ACid = "101";
                if (dtAc.Rows.Count > 0) ACid = dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + "01";
            }
            else
            {
                ACid = "301";
                if (dtAc.Rows.Count > 0)
                {
                    string NewACid = database.GetScalarValue("select ManualAcId from SMPPSETTING where smppaccountid=" + dtAc.Rows[0]["SMPPACCOUNTID"].ToString() + " and active=1").ToString();
                    if (NewACid != "")
                    {

                        string trantbl = database.GetScalarValue("select TranTableName from SMPPSETTING where smppaccountid=" + NewACid + " and active=1 ").ToString();

                        if (trantbl != "")
                        {
                            TranTableName = trantbl;
                        }

                        ACid = NewACid + "01";
                    }
                }
                else
                {
                    string fileAcId = Convert.ToString(database.GetScalarValue("select top 1 smppaccountid from SMPPSETTING where active=1 and forfile=1 "));
                    string NewACid = database.GetScalarValue("select top 1 manualacid from SMPPSETTING where smppaccountid=" + fileAcId + " and active=1 ").ToString();
                    if (NewACid != "")
                    {
                        string trantbl = database.GetScalarValue("select TranTableName from SMPPSETTING where smppaccountid=" + NewACid + " and active=1 ").ToString();

                        if (trantbl != "")
                        {
                            TranTableName = trantbl;
                        }

                        ACid = NewACid + "01";
                    }
                }

            }

            string sql = "";

            if (checkblacklistno(UserID, mobile))
            {
                sql = @"DECLARE @nid varchar (100)  ";
                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + @" select @nid=newid()
                    Insert into msgsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smstext, smsrate,DLRCODE) " +
                    " select '1' as id,'vcon','" + ACid + "','" + UserID + "',N'" + smsTex + "','" + mobile + "' as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),@nid,getdate(),'" + fileId + "' as fileid,'1'," +
                    " N'" + msg + "','" + rate.ToString() + "','" + HeroDlrCode + "' from settings ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + @nid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, @nid, GETDATE(), 'BlackList','250',getdate()
                    FROM settings ";
                }
            }
            else if (checkblacklistnoSender(s, mobile))
            {
                sql = @"DECLARE @nid varchar (100)  ";
                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + @" select @nid=newid()
                    Insert into msgsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smstext, smsrate,DLRCODE) " +
                    " select '1' as id,'vcon','" + ACid + "','" + UserID + "',N'" + smsTex + "','" + mobile + "' as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),@nid,getdate(),'" + fileId + "' as fileid,'1'," +
                    " N'" + msg + "','" + rate.ToString() + "','" + HeroDlrCode + "' from settings ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + @nid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, @nid, GETDATE(), 'BlackList','250',getdate()
                    FROM settings ";
                }
            }
            else
            {
                string st1 = checMobProcessNo(UserID, mobile);
                if (st1 != "")
                {
                    string[] st2 = st1.Split(';');

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + @" select @nid=newid()
                        Insert into msgsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smstext, smsrate,DLRCODE) " +
                        " select '1' as id,'vcon','" + ACid + "','" + UserID + "',N'" + smsTex + "','" + mobile + "' as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),@nid,getdate(),'" + fileId + "' as fileid,'1'," +
                        " N'" + msg + "','" + rate.ToString() + "','" + HeroDlrCode + "' from settings ; ";
                        if (st2[1] == "F")
                        {
                            sql = sql + @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + @nid + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + st2[0] + @" text:' AS DLVRTEXT, @nid, GETDATE(), 'Undeliverable','" + st2[0] + @"',getdate()
                            FROM settings ";
                        }
                        else
                        {
                            sql = sql + @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + @nid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, @nid, GETDATE(), 'Delivered','000',getdate()
                            FROM settings ";
                        }
                    }
                }

                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }
                sql = @"INSERT INTO [dbo].[" + TranTableName + @"]([PROVIDER],[SMPPACCOUNTID],[PROFILEID],[MSGTEXT],[TOMOBILE],[SENDERID],[CREATEDAT],[PICKED_DATETIME],[peid],[DATACODE],[smsrate],[TemplateID],FILEID,DLRCODE) VALUES
                ('BSNL', '" + ACid + "', '" + UserID + @"',N'" + msg.Trim() + @"', '" + mobile + @"', '" + s + @"', GETDATE(), NULL,'" + peid + "','" + dataCode + "','" + rate.ToString() + "','" + TemplateID + "','" + fileId + "','" + HeroDlrCode + "')";
            }

            // double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), smscount);
            double bal = CalculateSMSCost(noofsms, Convert.ToDouble(rate));

            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "' ; " +
                " INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                 "VALUES('" + UserID + "','D','" + bal + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", "Manual").Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in SendURL_SMS method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }

        }

        //methods not in use here. These are used in the "Submit To Tran" utiltiy----->>>>
        public void B4SEND_Schedule_SMS_BULK(string UserID, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, double rate, string SMSType, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, List<string> mobList, string manual, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "", string tblnm = "", string dbname = "", int ProcessfileId = 0)
        {
            //string user = "tmp_" + UserID;
            string user = tblnm;

            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + m + "')");
                }
                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + UserID + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + UserID + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");
            }


            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + UserID + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));


            string colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));

            string sqlUpd = "if exists (select * from sys.tables where name='" + user + @"') update dbo." + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ";
            database.ExecuteNonQuery(sqlUpd);

            checkNumberDigitsAndUpdate(user, colnm);

            string sql = GetSqlAccounts(dtAc);

            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from dbo." + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'dbo." + user + "')) " +
                 " alter table dbo." + user + @" add smppaccountid numeric(10),TranTableName varchar(150)  "); //add new colum

            //  database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") dbo." + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',TranTableName = '" + dt.Rows[i]["TranTableName"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }
            double CampaignCost = PrevBalance - AvailableBalance;
            //change SMPP ACCOUNT based on SMSTYPE
            msg = msg.Replace("'", "''");
            string peid = getPEid(UserID);
            sql = @"Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,schedule,campaignname,SMSRATE,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE,FileProcessId) values ('" + UserID + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + schdate + "','" + campnm + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "'," + ProcessfileId + ") " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; " +
                " INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,TemplateID,TranTableName) " +
            " select 'VCON',u.smppaccountid,'" + UserID + "',N'" + msg + "',u.[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
            (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "'," + (shortURLId > 0 ? "left(NEWID(),8)" : "null") + ",'" + rate.ToString() + "','" + SMSType + "','" +
            domain + "',@id,'" + peid + "','" + (ucs2 ? "UCS2" : "Default") + "',case when b.mobileno is not null then 1 else 0 end as blacklist,'" + TemplateID + "' as TemplateID,TranTableName from dbo." + user + " u left join blacklistno b on u.[" + colnm + "]=b.mobileno and b.userid='" + UserID + "' where u.[" + colnm + "] is not null ; ";

            double Bper = GetBlockSMSper(UserID, "B");
            if (Bper != 0)
            {
                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockno=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(UserID) + ") ";
            }
            double Fper = GetBlockSMSper(UserID, "F");
            if (Fper != 0)
            {
                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockfail=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 AND isnull(BLOCKNO,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(UserID) + ") ";
            }
            bool ClkBlk = ClickListUser(UserID);
            string isdlr = "";
            if ((ClkBlk && SMSType == "2") || (ClkBlk && (SMSType == "3" || SMSType == "6") && shortURLId > 0))
            {
                DataTable dtCl = database.GetDataTable("Select * from ClickDataBlock where userid='" + UserID + "'");
                string noofdays = dtCl.Rows[0]["noofdays"].ToString();
                isdlr = dtCl.Rows[0]["ProcessType"].ToString();
                sql = sql + " select t.* into #t12 from MsgSchedule t inner join MobTrackURL m on m.urlid='" + shortURLId.ToString() + "' and t.TOMOBILE=m.mobile inner join (select distinct urlid from mobstats where shorturl_id='" + shortURLId.ToString() + "') s on m.id=s.urlid where t.profileid='" + UserID + "' and t.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and m.sentdate >= DateAdd(Day,-" + noofdays + ",getdate()) ";

                if (isdlr.ToUpper() == "DELIVERED")
                {
                    sql = sql + " update MsgSchedule set blockno=1 where profileid='" + UserID + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and tomobile in (select tomobile from #t12) ";
                }
                if (isdlr.ToUpper() == "NOSUBMISSION")
                {
                    sql = sql + " delete d from MsgSchedule d inner join #t12 t on d.tomobile = t.tomobile  where d.profileid='" + UserID + "' and d.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ;  ";
                }
            }
            if (SMSType == "2" || ((SMSType == "3" || SMSType == "6") && shortURLId > 0))
            {
                sql = sql + "; Update MsgSchedule set MSGTEXT=replace(MSGTEXT,shorturl,domain+newsegment) where PROFILEID='" + UserID + "' and SCHEDULE=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ; ";
            }

            bool isMobProcess = MobProcessUser(UserID);
            if (isMobProcess)
            {
                //PROCESS 
                sql = sql + "Update Msgschedule set blockno=1 from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + UserID + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='D' ; ";
                sql = sql + "Update Msgschedule set blockfail=1,ERR_CODE=S.ERR_CODE from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + UserID + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='F' ; ";
            }

            // double bal = CalculateAmount(UserID, noof_message, Convert.ToDouble(rate), smscount);
            double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
            sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Schedule_SMS_BULK method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void B4SEND_InsertTemplateSMSrecordsFromUSERTMP(string userId, string s, string SMSType, string msg, string filenm, string filenmext, DataTable dtAc, DataTable dtCols, List<string> lstMappedFields, string campnm, bool ucs2, double rate, int noofsms, string schdate, List<string> tempFields, string TemplateID, string country_code, string shUrl = "", string domName = "", int shortURLId = 0, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "", string tblnm = "", string dbname = "", int ProcessfileId = 0)
        {
            //string user1 = "tmp1_" + userId;
            //string user = "tmp_" + userId;
            string user = tblnm;
            string user1 = tblnm;

            string colnm = "";
            string sql = "";
            string peid = getPEid(userId);

            string dataCode = "";
            if (SMSType == "8")
            {
                if (ucs2)
                    dataCode = "UnicodeFlashSMS";
                else
                    dataCode = "DefaultFlashSMS";
            }
            else
            {
                if (ucs2)
                    dataCode = "UCS2";
                else
                    dataCode = "Default";
            }


            //sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
            //    SELECT * INTO " + user + @" FROM " + user1;

            //database.ExecuteNonQuery(sql);

            colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
            database.ExecuteNonQuery("update dbo." + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            sql = GetSqlAccounts(dtAc);
            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from dbo." + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'dbo." + user + "')) " +
                " alter table dbo." + user + @" add smppaccountid numeric(10),TranTableName varchar(150) , shortsegment varchar(10) "); //add new colum


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") dbo." + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',TranTableName = '" + dt.Rows[i]["TranTableName"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }

            if (shUrl != "") database.ExecuteNonQuery("update dbo." + user + @" set shortsegment=left(NEWID(),8) ");

            //prepare query with template msg text and field value
            string m = msg.Replace("'", "''");
            if (lstMappedFields.Count > 0)
            {
                foreach (string li in lstMappedFields)
                {
                    string[] s1 = li.Replace(" ---->> ", "$").Split('$');

                    string coltype = Convert.ToString(database.GetScalarValue("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + user + @"' and COLUMN_NAME = '" + s1[1] + "'"));
                    if (coltype.ToUpper().Contains("DATE"))
                        m = m.Replace(s1[0], "' + convert(varchar,[" + s1[1] + "],106) + N'");
                    else if (coltype.ToUpper().Contains("FLOAT"))
                        m = m.Replace(s1[0], "' + CONVERT(nvarchar(255), LTRIM(RTRIM(str(ISNULL([" + s1[1] + "], 0), 20, 0)))) + N'");
                    else
                        m = m.Replace(s1[0], "' + convert(nvarchar(max),[" + s1[1] + "]) + N'");
                    //m = m.Replace(s1[0], "' + [" + s1[1] + "] + N'");
                }
            }
            for (int j = 0; j < tempFields.Count; j++)
                m = m.Replace(tempFields[j], ""); filenm = filenm.Replace("'", "''");

            double CampaignCost = PrevBalance - AvailableBalance;

            if (schdate == "")
            {
                //2 feb change ccode
                sql = @"if exists (select * from sys.tables where name='" + user + @"')  " +
                    " begin " +
                    " Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,TEMPLATEID,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE,FileProcessId) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + TemplateID + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "'," + ProcessfileId + ") " +
                    " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; ";

                //call sms test function test - get true / false
                if (Global.Istemplatetest == false)
                {
                    sql = sql + " select t.* into #t_012 from dbo." + user + " t " +
                       " delete d from dbo." + user + " d inner join #t_012 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                       " alter table #t_012 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t_012 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t_012 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                     FROM #t_012 ";
                    }
                    if (shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                           "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from dbo." + user + " where [" + colnm + "] is not null ; ";
                    }
                    sql = sql + " end ";
                }
                else
                {
                    double Bper = GetBlockSMSper(userId, "B");
                    if (Bper != 0)
                    {
                        Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                        sql = sql + " select top " + cnt20 + " * into #t10 from dbo." + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                            " delete d from dbo." + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t10 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t10 set msgid=newid() " +
                            @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate.ToString() + "' from #t10 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," + (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t10 ; ";
                        }
                    }
                    double Fper = GetBlockSMSper(userId, "F");
                    if (Fper != 0)
                    {
                        Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                        sql = sql + " select top " + cnt20 + " * into #t101 from dbo." + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                            " delete d from dbo." + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t101 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t101 set msgid=newid() " +
                            @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " ,[" + colnm + "] as TOMOBILE," +
                            "'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate.ToString() + "' from #t101 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," + (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t101 ; ";
                        }
                    }
                    bool blu = blacklistuser(userId);
                    if (blu)
                    {
                        sql = sql + " select t.* into #t11 from dbo." + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + userId + "'  where b.MOBILENO is not null and t.[" + colnm + "] is not null " +
                            " delete d from dbo." + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #t11 add msgid varchar (100) ; ";
                        for (int x = 0; x < noofsms; x++)
                        {
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #t11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                        }
                    }

                    bool isMobProcess = MobProcessUser(userId);
                    if (isMobProcess)
                    {
                        //PROCESS FAIL
                        sql = sql + " select t.*,s.err_code into #mt11 from dbo." + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                            " delete d from dbo." + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #mt11 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #mt11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #mt11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                            FROM #mt11 ; ";
                        }

                        //PROCESS DELIVERY
                        sql = sql + " select t.* into #Dmt11 from dbo." + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                            " delete d from dbo." + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            " alter table #Dmt11 add msgid varchar (100) ;  ";

                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #Dmt11 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + userId + "'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as msgtext,CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            (shUrl != "" ? " REPLACE(N'" + m + "','" + shUrl + "','" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + ",'" + rate.ToString() + "' from #Dmt11 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                            FROM #Dmt11 ; ";
                        }
                    }

                    DataTable dttrantblname = database.GetDataTable("select distinct trantablename from dbo." + user + @" ");

                    for (int i = 0; i < dttrantblname.Rows.Count; i++)
                    {
                        sql = sql + " INSERT INTO " + dttrantblname.Rows[i]["tranTablename"].ToString() + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid)  " +
                                              " Select 'VCON' as PROVIDER, smppaccountid, '" + userId + @"' as PROFILEID, " +
                                              (shUrl != "" ? " REPLACE(N'" + m + "', '" + shUrl + "', '" + domName + "' + shortsegment collate SQL_Latin1_General_CP1_CI_AS) " : "N'" + m + "'") + " as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + @"],0),20,0)))) as TOMOBILE
                , '" + s + @"' as SENDERID, GETDATE() as CREATEDAT,@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' AS templateid from dbo." + user + " where [" + colnm + "] is not null and TranTableName= '" + dttrantblname.Rows[i]["tranTablename"].ToString() + "' ; ";
                    }


                    if (Bper != 0 && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t10 where [" + colnm + "] is not null ; ";
                    }
                    if (Fper != 0 && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t101 where [" + colnm + "] is not null ; ";
                    }
                    if (blu && shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                       "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t11 where [" + colnm + "] is not null ; ";
                    }
                    if (shUrl != "")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                           "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from " + user + " where [" + colnm + "] is not null ; ";
                    }
                    sql = sql + " end ";

                    //  double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                    double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                    sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
                }


            }
            else
            {
                string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + userId + "' ";
                int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + q1 + ")"));

                // int shortURLId = 0;
                string shortURL = ""; string domain = "";
                shortURL = shUrl; domain = domName;
                sql = @"Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,schedule,campaignname,TEMPLATEID,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE,FileProcessId) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + schdate + "','" + campnm + "','" + TemplateID + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "'," + ProcessfileId + ") " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; " +
                " INSERT INTO MsgSchedule (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,SMSTYPE,DOMAIN,FILEID,peid,DATACODE,blacklist,templateid,TranTableName) " +
                " select 'VCON',u.smppaccountid,'" + userId + "',N'" + m + "',CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL(u.[" + colnm + @"],0),20,0)))) as TOMOBILE,'" + s + "',GETDATE(),dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "'),'" +
                (shortURLId > 0 ? "Y" : "N") + "','" + shortURL + "','" + shortURLId.ToString() + "'," + (shortURLId > 0 ? "left(NEWID(),8)" : "null") + ",'" + rate.ToString() + "','" + SMSType + "','" +
                domain + "',@id,'" + peid + "','" + dataCode + "',case when b.mobileno is not null then 1 else 0 end as blacklist,'" + TemplateID + "' AS templateid,TranTableName from dbo." + user + " u left join blacklistno b on u.[" + colnm + "]=b.mobileno and b.userid='" + userId + "' where u.[" + colnm + "] is not null ; ";

                double Bper = GetBlockSMSper(userId, "B");
                if (Bper != 0)
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockno=1 where profileid='" + userId + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(userId) + ") ";
                }
                double Fper = GetBlockSMSper(userId, "F");
                if (Fper != 0)
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " update top (" + cnt20 + ") MsgSchedule set blockFAIL=1 where profileid='" + userId + "' and schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and isnull(blacklist,0)=0 and isnull(blockno,0)=0 and fileid=@id and Tomobile not in (" + getWhiteListNo(userId) + ") ";
                }
                if ((SMSType == "2" || SMSType == "3" || SMSType == "6") && shortURLId > 0)
                {
                    sql = sql + "; Update MsgSchedule set MSGTEXT=replace(MSGTEXT,shorturl,domain+newsegment) where PROFILEID='" + userId + "' and SCHEDULE=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') ; ";
                }
                bool isMobProcess = MobProcessUser(userId);
                if (isMobProcess)
                {
                    //PROCESS 
                    sql = sql + "Update Msgschedule set blockno=1 from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + userId + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='D' ; ";
                    sql = sql + "Update Msgschedule set blockfail=1,ERR_CODE=S.ERR_CODE from msgschedule m inner join BLOCKSMSERROR s with (nolock) on s.profileid=m.profileid inner join UserMobile b with (nolock) on m.tomobile=b.tomobile and m.profileid=b.profileid AND B.ERR_CODE=S.ERR_CODE where m.profileid='" + userId + "' and m.schedule=dateadd(minute, " + Convert.ToString(Math.Abs(timedifferenceInMinute)) + ", '" + schdate + "') and s.DORF='F' ; ";
                }
                // double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "'";
            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in InsertTemplateSMSrecordsFromUSERTMP method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void B4SEND_InsertSMSrecordsFromUSERTMP(string userId, string s, string SMSType, string msg, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, int noofsms, double Drate, List<string> mobList, string manual, string retarget, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "", string tblnm = "", string dbname = "", int ProcessfileId = 0)
        {
            //change SMPP ACCOUNT based on SMSTYPE
            //string user = "tmp_" + userId;

            string user = tblnm;

            string sql;

            string colnm = "";

            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + country_code + m + "')");
                }
                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + userId + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + userId + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

            }


            colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));
            if (country_code != "" && manual != "MANUAL")
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') update " + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            string peid = getPEid(userId);

            sql = GetSqlAccounts(dtAc);

            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));

            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }
            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;
            //database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
            //   " alter table " + user + @" add smppaccountid numeric(10)"); //add new column


            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
               " alter table " + user + @" add smppaccountid numeric(10),TranTableName varchar(150) "); //add new colum

            //  database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10) ");



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',TranTableName = '" + dt.Rows[i]["TranTableName"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }



            msg = msg.Replace("'", "''"); filenm = filenm.Replace("'", "''");

            double CampaignCost = PrevBalance - AvailableBalance;

            sql = @"if exists (select * from sys.tables where name='" + user + @"')  " +
                " begin " +
                " Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,smsrate,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE,FileProcessId) values ('" + userId + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + Drate + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "'," + ProcessfileId + ") " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + userId + "' ; ";

            string rate = Drate.ToString();

            //call sms test function test - get true / false
            if (Global.Istemplatetest == false)
            {
                sql = sql + " select t.* into #t_011 from " + user + " t " +
                   " delete d from " + user + " d inner join #t_011 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                   " alter table #t_011 add msgid varchar (100) ;  ";
                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + " update #t_011 set msgid=newid() " +
                    " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                    " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                    " N'" + msg + "','" + rate + "' from #t_011 ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                    FROM #t_011 end ";
                }
            }
            else
            {

                double Bper = GetBlockSMSper(userId, "B");
                if (Bper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t101 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t101 set msgid=newid() " +
                            @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                      select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "'," +
                          "GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t101 ; " +
                             " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                             " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                             " N'" + msg + "','" + rate + "' from #t101 ; ";
                    }
                }
                double Fper = GetBlockSMSper(userId, "F");
                if (Fper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
                        " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t10 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t10 set msgid=newid() " +
                            @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                      select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
                             " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                             " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                             " N'" + msg + "','" + rate + "' from #t10 ; ";
                    }
                }
                if (blacklistuser(userId))
                {
                    sql = sql + " select t.* into #t11 from " + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + userId + "' where b.MOBILENO is not null " +
                        " delete d from " + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t11 add msgid varchar (100) ; ";
                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #t11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                    }
                }

                bool isMobProcess = MobProcessUser(userId);
                if (isMobProcess)
                {
                    //PROCESS FAIL
                    sql = sql + " select t.*,s.err_code into #mt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                        " delete d from " + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #mt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #mt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #mt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                    FROM #mt11 ; ";
                    }

                    //PROCESS DELIVERY
                    sql = sql + " select t.* into #Dmt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                        " delete d from " + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #Dmt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #Dmt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " N'" + msg + "','" + rate + "' from #Dmt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                    FROM #Dmt11 ; ";
                    }
                }
                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }

                DataTable dttrantblname = database.GetDataTable("select distinct trantablename from " + user + @" ");

                for (int i = 0; i < dttrantblname.Rows.Count; i++)
                {
                    sql = sql + " INSERT INTO " + dttrantblname.Rows[i]["TranTableName"].ToString() + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,Templateid)  " +
                                      " Select 'VCON' as PROVIDER, smppaccountid, '" + userId + @"' as PROFILEID, N'" + msg + @"' as MSGTEXT, [" + colnm + @"] as TOMOBILE
                , '" + s + @"' as SENDERID, GETDATE() as CREATEDAT,@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' as templateid from dbo." + user + " where [" + colnm + "] is not null and trantablename='" + dttrantblname.Rows[i]["tranTablename"].ToString() + "'";

                }



                //  double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "' end";
                //sql = sql + " select * from MSGTRAN_91FU1 ";
            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in InsertSMSrecordsFromUSERTMP method !  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public void B4SEND_Insert_SMS_BULK_4url(string UserID, string msg, string s, string schdate, int shortURLId, string shortURL, string domain, double rate, string SMSType, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, int noofsms, List<string> mobList, string manual, string retarget, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "", string tblnm = "", string dbname = "", int ProcessfileId = 0)
        {
            //string user = "tmp_" + UserID;
            string user = tblnm;
            if (manual == "MANUAL")
            {
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                foreach (var m in mobList)
                {
                    database.ExecuteNonQuery(" Insert into " + user + @" values ('" + country_code + m + "')");
                }

                database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
                /*rabi 14 jul 21*/
                string sqlRestict = @"if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + UserID + "' AND TYPE='U')" +
                    " delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id 
                     WHERE UserId='" + UserID + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S')" +
                     " delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'";
                database.ExecuteNonQuery(sqlRestict);

            }

            string colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));
            if (country_code != "" && manual != "MANUAL")
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') update dbo." + user + @" set [" + colnm + "] = '" + country_code + "'+convert(varchar,convert(bigint,[" + colnm + "])) ");

            checkNumberDigitsAndUpdate(user, colnm);

            string peid = getPEid(UserID);
            string sql;
            sql = GetSqlAccounts(dtAc);
            DataTable dt = database.GetDataTable(sql);

            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from dbo." + user + @" else select 0 "));
            dt.Columns.Add("cnt", typeof(string));

            int totalPDU = 0;
            for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
            Int32 totcnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
                Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
                totcnt += cntrow;
                dt.Rows[i]["cnt"] = cntrow.ToString();
            }
            int dif = 0;
            if (totcnt < rowcnt)
            {
                dif = rowcnt - totcnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
            }

            //Int32 cnt = (rowcnt / dt.Rows.Count);
            //if (rowcnt % dt.Rows.Count > 0) cnt++;

            database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'dbo." + user + "')) " +
               " alter table dbo." + user + @" add smppaccountid numeric(10),TranTableName varchar(150) , shortsegment varchar(10) "); //add new colum

            //database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10), shortsegment varchar(10) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") dbo." + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',TranTableName = '" + dt.Rows[i]["TranTableName"].ToString() + "' where smppaccountid is null ";
                database.ExecuteNonQuery(sql);
            }

            sql = "update dbo." + user + @" set shortsegment=left(NEWID(),8) ";
            database.ExecuteNonQuery(sql);
            //change SMPP ACCOUNT based on SMSTYPE

            double CampaignCost = PrevBalance - AvailableBalance;

            msg = msg.Replace("'", "''"); filenm = filenm.Replace("'", "''");
            sql = @" Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,smsrate,shortURLId,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE,FileProcessId) values ('" + UserID + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + rate + "','" + Convert.ToString(shortURLId) + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "'," + ProcessfileId + ") " +
                " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; ";

            //call sms test function test - get true / false
            if (Global.Istemplatetest == false)
            {
                sql = sql + " select t.* into #t_01 from dbo." + user + " t " +
                    " delete d from dbo." + user + " d inner join #t_01 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                    " alter table #t_01 add msgid varchar (100) ;  ";

                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                    sql = sql + " update #t_01 set msgid=newid() " +
                    " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                    " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                    " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t_01 ; " +
                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                    @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
                    FROM #t_01 ; ";
                }

                sql = sql + " insert into mobtrackurl(urlid, mobile, sentdate, segment,fileid,templateid) select " +
               "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid from #t_01 where [" + colnm + "] is not null ";

            }
            else
            {
                double Bper = GetBlockSMSper(UserID, "B");
                if (Bper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                    sql = sql + " select top " + cnt20 + " * into #t10 from dbo." + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(UserID) + ") ORDER BY NEWID() " +
                        " delete d from dbo." + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t10 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t10 set msgid=newid() " +
                        @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,'" + rate + "' from #t10 ; ";
                    }
                }
                double Fper = GetBlockSMSper(UserID, "F");
                if (Fper != 0 && retarget == "")
                {
                    Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
                    sql = sql + " select top " + cnt20 + " * into #t101 from dbo." + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(UserID) + ") ORDER BY NEWID() " +
                        " delete d from dbo." + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t101 add msgid varchar (100) ; ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t101 set msgid=newid() " +
                        @" insert into FAILSUBMITTED (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                        select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE," +
                            "'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t101 ; " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,'" + rate + "' from #t101 ; ";
                    }
                }
                bool blu = blacklistuser(UserID);
                if (blu)
                {
                    sql = sql + " select t.* into #t11 from dbo." + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + UserID + "' where b.MOBILENO is not null " +
                        " delete d from dbo." + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #t11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #t11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                    FROM #t11 ; ";
                    }
                }

                bool isMobProcess = MobProcessUser(UserID);
                if (isMobProcess)
                {
                    //PROCESS FAIL
                    sql = sql + " select t.*,s.err_code into #mt11 from dbo." + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + UserID + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
                        " delete d from dbo." + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #mt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #mt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #mt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
                    FROM #mt11 ; ";
                    }

                    //PROCESS DELIVERY
                    sql = sql + " select t.* into #Dmt11 from dbo." + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + UserID + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
                        " delete d from dbo." + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                        " alter table #Dmt11 add msgid varchar (100) ;  ";

                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                        sql = sql + " update #Dmt11 set msgid=newid() " +
                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                        " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                        " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #Dmt11 ; " +
                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                    FROM #Dmt11 ; ";
                    }
                }

                bool ClkBlk = ClickListUser(UserID);
                string isdlr = "";
                if (ClkBlk)
                {
                    DataTable dtCl = database.GetDataTable("Select * from ClickDataBlock where userid='" + UserID + "'");
                    string noofdays = dtCl.Rows[0]["noofdays"].ToString();
                    isdlr = dtCl.Rows[0]["ProcessType"].ToString();
                    sql = sql + " select t.* into #t12 from dbo." + user + " t inner join MobTrackURL m on m.urlid='" + shortURLId.ToString() + "' and t.[" + colnm + "]=m.mobile inner join (select distinct urlid from mobstats where shorturl_id='" + shortURLId.ToString() + "') s on m.id=s.urlid where m.sentdate >= DateAdd(Day,-" + noofdays + ",getdate()) ";
                    sql = sql + " delete d from dbo." + user + " d inner join #t12 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  alter table #t12 add msgid varchar (100) ;  ";
                    if (isdlr.ToUpper() == "DELIVERED")
                    {
                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            sql = sql + " update #t12 set msgid=newid() " +
                            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            " select @id as id,'vcon',smppaccountid,'" + UserID + "',REPLACE(N'" + smsTex + "','" + shortURL + "','" + domain + "' + shortsegment),[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            " REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment),'" + rate + "' from #t12 ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                        ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
                        FROM #t12 ; ";
                        }
                    }
                    //if (isdlr.ToUpper() == "NOSUBMISSION")
                    //{
                    //    sql = sql + " delete d from " + user + " d inner join #t12 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  ";
                    //}
                }

                string dataCode = "";
                if (SMSType == "8")
                {
                    if (ucs2)
                        dataCode = "UnicodeFlashSMS";
                    else
                        dataCode = "DefaultFlashSMS";
                }
                else
                {
                    if (ucs2)
                        dataCode = "UCS2";
                    else
                        dataCode = "Default";
                }


                DataTable dttrantblname = database.GetDataTable("select distinct trantablename from dbo." + user + @" ");

                for (int i = 0; i < dttrantblname.Rows.Count; i++)
                {
                    sql = sql + @" INSERT INTO " + dttrantblname.Rows[i]["TranTableName"].ToString() + @" (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid)
                     select 'VCON',smppaccountid,'" + UserID + "',REPLACE(N'" + msg + "','" + shortURL + "','" + domain + "' + shortsegment) ,[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),@id as fileid," +
                     "'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' as templateid from dbo." + user + " where [" + colnm + "] is not null and trantablename='" + dttrantblname.Rows[i]["TranTableName"].ToString() + "' ";
                }

                //database.ExecuteNonQuery(sql);
                if (Bper != 0 && retarget == "")
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid from #t10 where [" + colnm + "] is not null ";
                }
                if (Fper != 0 && retarget == "")
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid   from #t101 where [" + colnm + "] is not null ";
                }
                if (blu)
                {
                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t11 where [" + colnm + "] is not null ";
                }
                if (ClkBlk)
                {
                    if (isdlr.ToUpper() == "DELIVERED")
                    {
                        sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                        "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from #t12 where [" + colnm + "] is not null ";
                    }
                }
                sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid,templateid) select " +
                   "'" + shortURLId.ToString() + "', [" + colnm + "] , getdate(),shortsegment,@id as fileid,'" + TemplateID + "' as templateid  from dbo." + user + " where [" + colnm + "] is not null ";

                // double bal = CalculateAmount(UserID, noof_message, rate, smscount);
                double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
                sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + UserID + "'";
            }
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Error in Insert_SMS_BULK_4url method !  ", ex.Message + " - " + ex.StackTrace + " -- " + sql);
                throw ex;
            }

        }
        //methods not in use here. These are used in the "Submit To Tran" utiltiy   <<<<<<<----------
        // ---<-------------- SMS SENDING METHODS ----------------

        public double CalculateAmount(string UserID, long cnt, double rate, double smscnt)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer with (nolock) where username='" + UserID + "'"));
            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble((cnt * (rate * 10)));
            bal = Math.Round((bal / 1000), 3);
            return bal;
        }

        public double CalculateSMSCost(long cnt, double rate)
        {
            double bal = 0;
            bal = (cnt * rate) / 100;
            return Convert.ToDouble(Math.Round(bal, 3));
        }

        public DataTable GetSMSRateAsPerCountry(string User, string ccode)
        {
            DataTable dt = new DataTable("dt");
            string sql = "";
            sql = "select * from smsrateaspercountry where username='" + User + "' and countrycode='" + ccode + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetSMSText(string msg, int x, int noofsms, bool ucs2)
        {
            string ret = "";
            msg = msg.Replace("''", "'");
            if (ucs2)
            {
                if (noofsms == 1) ret = msg;
                if (x == 1) { if (msg.Length > 70) ret = msg.Substring(0, 70); else ret = msg.Substring(0); }
                if (x == 2) { if (msg.Length > 134) ret = msg.Substring(70, 64); else ret = msg.Substring(70); }
                if (x == 3) { if (msg.Length > 201) ret = msg.Substring(134, 67); else ret = msg.Substring(134); }
                if (x == 4) { if (msg.Length > 268) ret = msg.Substring(201, 67); else ret = msg.Substring(201); }
                if (x == 5) { if (msg.Length > 335) ret = msg.Substring(268, 67); else ret = msg.Substring(268); }
                if (x == 6) { if (msg.Length > 402) ret = msg.Substring(335, 67); else ret = msg.Substring(335); }
                if (x == 7) { if (msg.Length > 469) ret = msg.Substring(402, 67); else ret = msg.Substring(402); }
                if (x == 8) { if (msg.Length > 536) ret = msg.Substring(469, 67); else ret = msg.Substring(469); }
                if (x == 9) { if (msg.Length > 603) ret = msg.Substring(536, 67); else ret = msg.Substring(536); }
                if (x == 10) { if (msg.Length > 670) ret = msg.Substring(603, 67); else ret = msg.Substring(603); }
            }
            else
            {
                if (noofsms == 1) ret = msg;
                if (x == 1) { if (msg.Length > 160) ret = msg.Substring(0, 160); else ret = msg.Substring(0); }
                if (x == 2) { if (msg.Length > 306) ret = msg.Substring(160, 146); else ret = msg.Substring(160); }
                if (x == 3) { if (msg.Length > 459) ret = msg.Substring(306, 153); else ret = msg.Substring(306); }
                if (x == 4) { if (msg.Length > 612) ret = msg.Substring(459, 153); else ret = msg.Substring(459); }
                if (x == 5) { if (msg.Length > 765) ret = msg.Substring(612, 153); else ret = msg.Substring(612); }
                if (x == 6) { if (msg.Length > 918) ret = msg.Substring(765, 153); else ret = msg.Substring(765); }
                if (x == 7) { if (msg.Length > 1071) ret = msg.Substring(918, 153); else ret = msg.Substring(918); }
                if (x == 8) { if (msg.Length > 1224) ret = msg.Substring(1071, 153); else ret = msg.Substring(1071); }
                if (x == 9) { if (msg.Length > 1377) ret = msg.Substring(1224, 153); else ret = msg.Substring(1224); }
                if (x == 10) { if (msg.Length > 1530) ret = msg.Substring(1377, 153); else ret = msg.Substring(1377); }
            }
            return ret.Replace("'", "''");
        }

        public double GetBlockSMSper(string userid, string typ)
        {
            DataTable dt = database.GetDataTable("select isnull(" + (typ == "B" ? "blockpercent" : "failpercent") + ",0) as bp from blocksms where userid='" + userid + "'");
            if (dt.Rows.Count > 0)
                return Convert.ToDouble(dt.Rows[0]["bp"]);
            else return 0;
        }

        public string getWhiteListNo(string userid)
        {
            string s = "'0'";
            DataTable dt = new DataTable();
            dt = database.GetDataTable("select mobile from blocksmswhitelist where userid='" + userid + "' union all select mobile from blocksmswhitelistglobal");
            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++) s = s + ",'" + dt.Rows[k]["mobile"].ToString() + "'";
            }
            return s;
        }

        public bool blacklistuser(string userid)
        {
            if (Convert.ToInt64(database.GetScalarValue("Select count(*) as cnt from blacklistno where userid='" + userid + "'")) > 0)
                return true;
            else return false;
        }

        public bool ClickListUser(string userid)
        {
            if (Convert.ToInt64(database.GetScalarValue("Select count(*) as cnt from ClickDataBlock where userid='" + userid + "'")) > 0)
                return true;
            else return false;
        }

        public bool MobProcessUser(string userid)
        {
            if (Convert.ToInt64(database.GetScalarValue("Select count(*) as cnt from BLOCKSMSERROR with (nolock) where profileid='" + userid + "'")) > 0)
                return true;
            else return false;
        }

        public string GetDataType(string userId, string colnm)
        {
            string user = "tmp1_" + userId;
            string coltype = Convert.ToString(database.GetScalarValue("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + user + @"' and COLUMN_NAME = '" + colnm + "'"));
            if (coltype.ToUpper().Contains("DATE"))
                return "DATE";
            else
                return "";
        }

        public bool checkblacklistno(string UserID, string mobile)
        {
            return Convert.ToInt64(database.GetScalarValue("select count(*) from blacklistno where userid='" + UserID + "' and mobileno='" + mobile + "' ")) > 0 ? true : false;
        }

        public bool checkblacklistnoSender(string SenderID, string mobile)
        {
            return Convert.ToInt64(database.GetScalarValue("select count(*) from blacklistnoSender with (nolock) where senderid='" + SenderID + "' and mobile='" + mobile + "' ")) > 0 ? true : false;
        }

        public string checMobProcessNo(string UserID, string mobile)
        {
            string sql = "Select S.ERR_CODE + ';' + S.DORF from usermobile u with (nolock) inner join BLOCKSMSERROR S ON U.PROFILEID=S.PROFILEID AND U.ERR_CODE=S.ERR_CODE where U.profileid='" + UserID + "' and U.tomobile='" + mobile + "'";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string getPEid(string user)
        {
            return Convert.ToString(database.GetScalarValue("Select isnull(PEID,'') from customer where username='" + user + "'"));
        }

        public string GetMainSenderId(string user)
        {
            return Convert.ToString(database.GetScalarValue("Select senderid from customer where username='" + user + "'"));
        }

        public void DropUserTmpTable(string user)
        {
            string table = "tmp_" + user;
            string sql = @"if exists (select * from sys.tables where name='" + table + @"') drop table " + table;
            database.ExecuteNonQuery(sql);

            string table1 = "tmp1_" + user;
            sql = @"if exists (select * from sys.tables where name='" + table1 + @"') drop table " + table1;
            database.ExecuteNonQuery(sql);

            string table2 = "tmpEx_" + user;
            sql = @"if exists (select * from sys.tables where name='" + table2 + @"') drop table " + table2;
            database.ExecuteNonQuery(sql);

            string table3 = "tmp1Ex_" + user;
            sql = @"if exists (select * from sys.tables where name='" + table3 + @"') drop table " + table3;
            database.ExecuteNonQuery(sql);
        }

        public void SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp, string countryCode, string fileid, string templateid)
        {
            mobile = countryCode + mobile;
            string sql = "insert into mobtrackurl (urlid, mobile, sentdate, segment, msgid, dlvrd,fileid, templateid) values " +
                "('" + urlid.ToString() + "','" + mobile + "',getdate(),'" + mseg + "','" + resp + "','','" + fileid + "','" + templateid + "')";
            database.ExecuteNonQuery(sql);
        }

        public string UdateAndGetURLbal(string UserID)
        {
            return "0";
        }

        public string UdateAndGetURLbal1(string UserID, string shorturl)
        {
            string b = "1000";
            string sql = "update customer set balance = balance - urlrate, noofhit=noofhit+" + b + " where Username='" + UserID + "' ; " +
                "insert into ClickBalanceLog (userid, shorturl, clickbal) values ('" + UserID + "','" + shorturl + "','" + b + "') ";
            database.ExecuteNonQuery(sql);
            sql = "Select balance from customer where Username='" + UserID + "' ";
            string s = Convert.ToString(database.GetScalarValue(sql));
            return s;
        }

        public string GetClickBalance(string UserID)
        {
            return Convert.ToString(database.GetScalarValue("select noofhit from cusstomer where username='" + UserID + "'"));
        }

        public DataTable GetURLSofUser_4QR(string User, string shUrl, string domain)
        {
            DataTable dt = new DataTable("dt");
            string sh = domain;
            string sql = "";
            if (shUrl == "")
                sql = "Select '" + sh + "' + segment as shorturl from short_urls where userid='" + User + "' AND RIGHT(SEGMENT,2)<>'_Q' AND MAINURL=1 order by added desc";
            else
                sql = "Select long_url from short_urls where userid='" + User + "' and '" + sh + "' + segment = '" + shUrl + "'";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetURLSofUser_4SMSSEND(string User, string shUrl, string domain)
        {
            DataTable dt = new DataTable("dt");
            string sh = domain;
            string sql = "";
            if (shUrl == "")
                sql = "Select ISNULL(urlname,'') + '    ' + '" + sh + "' + segment as shorturlDISP, '" + sh + "' + segment as shorturl from short_urls where userid='" + User + "' AND RIGHT(SEGMENT,2)<>'_Q' AND MAINURL=0 order by added desc";
            else
                sql = "Select long_url from short_urls where userid='" + User + "' and '" + sh + "' + segment = '" + shUrl + "'";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetURLSofUser(string User, string shUrl, string domain)
        {
            DataTable dt = new DataTable("dt");
            string sh = domain;
            string sql = "";
            if (shUrl == "")
                sql = "Select '" + sh + "' + segment as shorturl from short_urls where userid='" + User + "' AND RIGHT(SEGMENT,2)<>'_Q' order by added desc";
            else
                sql = "Select long_url from short_urls where userid='" + User + "' and '" + sh + "' + segment = '" + shUrl + "'";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public void STOREURL(string userid, string LongURL, string ip, string ShortURL, string mobTrk)
        {
            string sql = "insert into SHORT_URLS (long_url,segment,added,ip,num_of_clicks,userid,mobtrack) VALUES " +
                "('" + LongURL + "','" + ShortURL + "',GETDATE(),'" + ip + "','0','" + userid + "','N')";
            database.ExecuteNonQuery(sql);

        }

        public string GetExistingShortURL(string longurl, string userid)
        {
            return Convert.ToString(database.GetScalarValue("select isnull(segment,'') from short_urls where userid='" + userid + "' and long_url='" + longurl + "'"));
        }

        public DataTable GetSMPP_Account_Load()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select s.SMPPACCOUNTID,count(t.id) as cnt from SMPPSETTING s inner join MSGTRAN t on s.SMPPACCOUNTID=t.SMPPACCOUNTID
                where s.ACTIVE = 1 group by s.SMPPACCOUNTID";
            dt = dbmain.GetDataTable(sql);
            return dt;
        }

        public void DeleteTemplateInTemplateId(string senderId, string tempid)
        {
            string sql = string.Format("Delete from TemplateID where senderid ='{0}' AND templateid ='{1}' ", senderId, tempid);
            database.ExecuteNonQuery(sql);
        }

        public void DeleteTemplateInRequest(string username, string tempid)
        {
            string sql = string.Format("Delete from templaterequest where username ='{0}' AND templateid ='{1}' ", username, tempid);
            database.ExecuteNonQuery(sql);
        }

        //public void SaveTemplateInTemplateId(string senderId, string msg, string tempWord, string tempname, string tempid)
        //{
        //    string sql = @"IF NOT EXISTS (SELECT * FROM TemplateID 
        //           WHERE senderid = '" + senderId + @"'  AND templateid = '" + tempid + @"')
        //            insert into TemplateID (senderid,msgtext,insertdate,tempwords,templatename,templateid)
        //    values('" + senderId + "', N'" + msg.Replace("'", "''") + "', getdate(), N'" + tempWord.Replace("'", "''") + "','" + tempname + "','" + tempid + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        public void SaveTemplateInTemplateId(string senderId, string msg, string tempWord, string tempname, string tempid, string AllSender = "")
        {
            string sql = @"IF NOT EXISTS (SELECT * FROM TemplateID 
                   WHERE senderid = '" + senderId + @"'  AND templateid = '" + tempid + @"')
                    INSERT INTO TemplateID (senderid,msgtext,insertdate,tempwords,templatename,templateid,AllSender)
            values('" + senderId + "', N'" + msg.Replace("'", "''") + "', getdate(), N'" + tempWord.Replace("'", "''") + "','" + tempname + "','" + tempid + "','" + AllSender + "')";
            database.ExecuteNonQuery(sql);

            if (senderId == "HYNDAI")
            {
                senderId = "HMISVR";
                AllSender = "HYNDAI";
                sql = @"IF NOT EXISTS (SELECT * FROM TemplateID 
                   WHERE senderid = '" + senderId + @"'  AND templateid = '" + tempid + @"')
                    INSERT INTO TemplateID (senderid,msgtext,insertdate,tempwords,templatename,templateid,AllSender)
            values('" + senderId + "', N'" + msg.Replace("'", "''") + "', getdate(), N'" + tempWord.Replace("'", "''") + "','" + tempname + "','" + tempid + "','" + AllSender + "')";
                database.ExecuteNonQuery(sql);
            }
        }

        public DataTable GetTemplateListOfAPI(string senderId)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select templateID  ,templateName [tempname],msgtext [template],CONVERT(VARCHAR,insertdate,106) AS insertdate , insertdate AS INSERTDATETIME from TemplateID where senderid = '" + senderId + "' order by INSERTDATETIME desc";
            //string sql = @"select templateID  ,templateName +space(5)+ Convert(varchar,insertdate) [tempname] ,msgtext [template] from TemplateID where senderid = '" + senderId + "' order by insertdate desc";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetDLTofUser(string user)
        {
            return Convert.ToString(database.GetScalarValue("Select DLTNO from customer where username='" + user + "'"));
        }

        public void SaveMultipleSenderIdRequest(string query)
        {
            database.ExecuteNonQuery(query);
        }

        #region Comment by Rachit 25-01-22

        //public void SaveSenderIDRequest(string user, string senderid, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void SaveSenderIDRequest1(string user, string senderid, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest2(string user, string senderid1, string senderid2, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest3(string user, string senderid1, string senderid2, string senderid3, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest4(string user, string senderid1, string senderid2, string senderid3, string senderid4, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void SaveSenderIDRequest5(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void SaveSenderIDRequest6(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest7(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest8(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid8, string senderid5, string senderid6, string senderid7, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest9(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void SaveSenderIDRequest10(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void SaveSenderIDRequest11(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest12(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string senderid12, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest13(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string senderid12, string senderid13, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid12 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid13 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest14(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string senderid12, string senderid13, string senderid14, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid12 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid13 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid14 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest15(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string senderid12, string senderid13, string senderid14, string senderid15, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid12 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid13 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid14 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid15 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}
        //public void SaveSenderIDRequest16(string user, string senderid1, string senderid2, string senderid3, string senderid4, string senderid5, string senderid6, string senderid7, string senderid8, string senderid9, string senderid10, string senderid11, string senderid12, string senderid13, string senderid14, string senderid15, string senderid16, string fn)
        //{
        //    string sql = @"insert into senderidrequeset (username,senderid,createdat,filepath)
        //    values('" + user + "', '" + senderid1 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid2 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid3 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid4 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid5 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid6 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid7 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid8 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid9 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid10 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid11 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid12 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid13 + "', getdate(), '" + fn + "'),('" + user + "', '" + senderid14 + "', getdate(),('" + user + "', '" + senderid15 + "', getdate(), '" + fn + "'), ('" + user + "', '" + senderid16 + "', getdate(), '" + fn + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        #endregion

        //public void SaveTemplateRequest(string user, string msg, string fn, string tempname, string tempid, bool isNewTemplate = false)
        //{
        //    if (isNewTemplate)
        //    {
        //        string sql = @"insert into templaterequest (username,template,createdat,filepath,tempname,templateid,allotted)
        //    values('" + user + "', N'" + msg.Replace("'", "''") + "', getdate(), '" + fn + "','" + tempname + "','" + tempid + "',1)";
        //        database.ExecuteNonQuery(sql);
        //    }
        //    else
        //    {
        //        string sql = @"insert into templaterequest (username,template,createdat,filepath,tempname,templateid)
        //    values('" + user + "', N'" + msg.Replace("'", "''") + "', getdate(), '" + fn + "','" + tempname + "','" + tempid + "')";
        //        database.ExecuteNonQuery(sql);
        //    }

        //}
        public void SaveTemplateRequest(string user, string msg, string fn, string tempname, string tempid, bool isNewTemplate = false, string senderId = "")
        {
            if (isNewTemplate)
            {
                string sql = @"insert into templaterequest (username,template,createdat,filepath,tempname,templateid,allotted,senderId)
            values('" + user + "', N'" + msg.Replace("'", "''") + "', getdate(), '" + fn + "','" + tempname + "','" + tempid + "',1, '" + senderId + "')";
                database.ExecuteNonQuery(sql);
            }
            else
            {
                string sql = @"insert into templaterequest (username,template,createdat,filepath,tempname,templateid,senderId)
            values('" + user + "', N'" + msg.Replace("'", "''") + "', getdate(), '" + fn + "','" + tempname + "','" + tempid + "', '" + senderId + "')";
                database.ExecuteNonQuery(sql);
            }
        }

        public DataTable GetTemplateList(string usernm)
        {
            DataTable dt = new DataTable("dt");
            //string sql = @"select templateId,tempname,template from templaterequest where username = '" + usernm + "' and isnull(allotted,0)=1 order by createdat desc";
            string sql = @"select  templateId,tempname [tempname],Convert(varchar,createdat,106) insertdate,template, createdat AS INSERTDATETIME from templaterequest  where username = '" + usernm + "' and isnull(allotted,0)=1 order by INSERTDATETIME desc";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateListDownload(string usernm)
        {
            DataTable dt = new DataTable("dt");
            //string sql = @"select templateId,tempname,template from templaterequest where username = '" + usernm + "' and isnull(allotted,0)=1 order by createdat desc";
            string sql = @"select templateId,tempname [tempname], template, createdat AS INSERTDATETIME from templaterequest  where username = '" + usernm + "' and isnull(allotted,0)=1 order by INSERTDATETIME desc";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataSet ValidateTemplateRequest(string tempId, string templateName, string user)
        {
            string sql = string.Format("Select Count(templateid) [TemplateId] from templaterequest where username='{0}' AND templateid='{1}' ;", user, tempId);
            string sql1 = string.Format("Select Count(tempname) [TemplateName] from templaterequest where username='{0}' AND tempname='{1}' ;", user, templateName);
            return database.GetDataSet(sql + sql1);
        }

        public DataTable ValidateTemplateIdforAPI(string tempId, string templateName, string senderId)
        {
            string sql = string.Format("Select Count(templateid) [TemplateId] from TemplateID where senderid='{0}' AND templateid='{1}' ;", senderId, tempId);
            return database.GetDataTable(sql);
        }

        public int GetCountForSubmittedTemplate(string username, string senderId, string currentTime)
        {
            string sql = string.Format("Select Count(id) [Total] from msgsubmitted with(nolock) where senderid='{0}' AND profileid='{1}' AND insertDate>'{2}' ;", senderId, username, currentTime);
            return Convert.ToInt32(database.GetScalarValue(sql));

        }

        public DataTable GetSApprovedTemplateOfUser(string usernm, string hidetempid = "")
        {
            DataTable dt = new DataTable("dt");
            string sql = "";
            if (hidetempid == "True")
            {
                sql = @"select  template,isnull(tempname,'') TemplateID, TemplateID as onlyTemplateID from templaterequest where username = '" + usernm + "' and isnull(allotted,0)=1 and isnull(IsWhatsapp,0)=0";
            }
            else
            {
                sql = @"select  template,Concat(TemplateID ,' ' ,isnull(tempname,'')) TemplateID,TemplateID as onlyTemplateID from templaterequest where username = '" + usernm + "' and isnull(allotted,0)=1 and isnull(IsWhatsapp,0)=0";
            }
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateSMS(string usernm, string templatenm)
        {
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string sql = @"Select Template,templateid from templaterequest where username = '" + usernm + "' and templateid = '" + templatenm + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateSMSfromID(string usernm, string templateID)
        {
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string sql = @"Select Template,templateid from templaterequest where username = '" + usernm + "' and templateid = '" + templateID + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetFieldsOfUserUploadedXLS(string usernm)
        {
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string sql = @"select column_name From information_schema.columns where table_name = '" + user + @"' ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetMaxLenFieldsOfUserUploadedXLS(string usernm, DataTable dtC)
        {
            string col = "";
            //foreach (DataRow dr in dtC.Rows) col = col + " max(len([" + dr[0].ToString() + "])) as [" + dr[0].ToString() + "],";
            foreach (DataRow dr in dtC.Rows) col = col + " max(Datalength([" + dr[0].ToString() + "])/2) as [" + dr[0].ToString() + "],";
            if (col != "") col = col.Substring(0, col.Length - 1);
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string sql = @"select " + col + " From " + user;
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTopRecordOfUserUploadedXLS(string usernm)
        {
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 "));
            string sql = string.Format("select top 1 * from {0} Where [{1}] IS NOT NULL", user, colnm);
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetGroup(string user)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select grpname,id from grouphead where userid= '" + user + "' order by grpname";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable TotalGroups(string user)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select count(*) as TotalGroups from grouphead where userid= '" + user + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public bool GroupExists4User(string user, string grp, string excludegrp)
        {
            int c = 0;
            string sql = @"select count(*) from grouphead where userid= '" + user + "' and grpname='" + grp + "'";
            if (excludegrp != "") sql = sql + " and grpname<>'" + excludegrp + "'";
            c = Convert.ToUInt16(database.GetScalarValue(sql));
            return (c > 0 ? true : false);
        }

        public void CreateGroup(string user, string grp)
        {
            string sql = @"insert into grouphead (userid,grpname) values ('" + user + "','" + grp + "')";
            database.ExecuteNonQuery(sql);
        }

        public void UpdateGroup(string user, string grp, string oldgrp)
        {
            string sql = @"update grouphead set grpname='" + grp + "' where userid='" + user + "' and grpname='" + oldgrp + "'";
            database.ExecuteNonQuery(sql);
        }

        //Change By Vikas On 24_06_2024 Add GroupID Prameter
        public void AddMobileInGroup(List<string> mobList, string cc, string user, string grp, bool isUpload, string GroupID = "0")
        {
            string id = Convert.ToString(database.GetScalarValue("Select ID from grouphead where userid='" + user + "' and grpname='" + grp + "'"));
            if (GroupID != "0")
            {
                id = GroupID;
            }
            if (isUpload)
            {
                try
                {
                    string user1 = "tmp_" + user;
                    string sql1 = "delete " + user1 + " FROM  " + user1 + " T join  groupdtl D  on D.Mob=T.MobNo where D.Id=" + id + "";
                    database.ExecuteNonQuery(sql1);
                    string colnm = Convert.ToString(database.GetScalarValue("if exists (SELECT * FROM sys.tables WHERE name='" + user1 + @"') SELECT column_name FROM information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 else select '' "));

                    string sql = string.Format("INSERT INTO groupdtl (id,mob) SELECT distinct '{0}' AS ID,[" + colnm + "] From {1} ", id, user1);
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception) { }
            }
            else
            {
                foreach (var m in mobList)
                {
                    string num = m;
                    try
                    {
                        database.ExecuteNonQuery("INSERT INTO groupdtl (id,mob) values ('" + id + "','" + num + "') ");
                    }
                    catch (Exception) { }
                }
            }
        }

        public DataTable GetMobileNumbersOfGroup(string user, string grp)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select h.GrpName as [Group], d.mob as Mobile_Number from grouphead h inner join groupdtl d on h.id=d.id where h.userid= '" + user + "' and h.grpname='" + grp + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public void InsertInApollo()
        {
            try
            {
                string sql = "Insert into apollo (instime) values (getdate()) ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }

        public bool restrictClick(string IP)
        {
            try
            {
                string sql = "Select count(ipaddress) from RestrictClickFromIP with (nolock) where ipaddress='" + IP + "'";
                int c = Convert.ToInt16(database.GetScalarValue(sql));
                if (c > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public void UpdateCustomer(string User, string Senderid, string Website, bool dnd)
        {
            string sql = @"update customer set senderid='" + Senderid + "',website= '" + Website + "',PERMISSION='" + (dnd ? "1" : "2") + "' where username='" + User + "'";
            database.ExecuteNonQuery(sql);
        }
        //
        //Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));

        //svh ------------->

        public bool TestSmsbeforeSend(string profileid, string templateId, string msg, string senderId, string peid)
        {
            string _response = SendSMSthroughAPI(profileid, templateId, msg, senderId, peid);
            if (_response != "")
            {
                List<string> liMessageId = new List<string>();
                if (_response.Contains("Message ID:"))
                {
                    int freq = Regex.Matches(_response, ":").Count;
                    if (freq == 1)
                    {
                        liMessageId.Add(_response.Split(':')[1].Trim().Replace(@"\", "").Replace('"', ' ').Trim());
                    }
                    else
                    {
                        // string getResponseTxt = "SMS Submitted Successfully.MobileNo: 9773937533 Message ID: S21810447121147427436539144, MobileNo: 9870333974 Message ID: S218104471211474211728639145";
                        string[] arr = _response.Split(',');
                        foreach (string mid in arr)
                        {
                            liMessageId.Add(mid.Split(':')[2].Trim().Replace(@"\", "").Replace('"', ' ').Trim());
                        }
                    }

                    System.Threading.Thread.Sleep(10000);//wait 10 second
                    foreach (string msgId in liMessageId)
                    {
                        string errorCode = Convert.ToString(database.GetScalarValue(string.Format("select err_code from delivery where msgid ='{0}'", msgId)));
                        int tempErrorCode = Convert.ToInt16(database.GetScalarValue(string.Format("select Count(err_code) from errorcodeTemplate where err_code ='{0}'", errorCode)));

                        if (tempErrorCode > 0)
                        {
                            Global.templateErrorCode = errorCode;
                            return false;
                        }
                    }

                }
            }

            return true;

        }

        public string SendSMSthroughAPI(string profileid, string templateId, string msg, string senderId, string peid)
        {
            msg = HttpUtility.UrlEncode(msg);

            // msg = msg.Replace("%", "%25").Replace("#", "%23").Replace("&", "%26").Replace("+", "%2B");

            DataTable dt = database.GetDataTable("Select * from SMSCheckSetting where profileid='" + profileid + "'");
            if (dt.Rows.Count > 0)
            {
                string _userid = "MIM2000002";

                string sql = "IF NOT EXISTS (SELECT * FROM SENDERIDMAST WHERE senderid = '" + senderId + "' AND userid='" + _userid + "')" +
                  " INSERT INTO SENDERIDMAST(userid,senderid) values('" + _userid + "','" + senderId + "')";

                database.ExecuteNonQuery(sql);
                string pwd = Convert.ToString(database.GetScalarValue("select pwd from CUSTOMER where username='" + _userid + "'"));
                DataRow dr = dt.Rows[0];
                string mob = Convert.ToString(dr["mob1"]) + "," + Convert.ToString(dr["mob2"]) + "," + Convert.ToString(dr["mob3"]) + "," + Convert.ToString(dr["mob4"]) + "," + Convert.ToString(dr["mob5"]);
                mob = mob.TrimEnd(',');
                string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + _userid + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + senderId + "&msg=" + msg + "&msgtype=13&peid=" + peid + "&templateid=" + templateId;
                string getResponseTxt = "";
                string getStatus = "";
                WinHttp.WinHttpRequest objWinRq;
                objWinRq = new WinHttp.WinHttpRequest();
                try
                {
                    objWinRq.Open("GET", url, false);
                    objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                    objWinRq.Send(null);

                    while (!(getStatus != "" && getResponseTxt != ""))
                    {
                        getStatus = objWinRq.Status + objWinRq.StatusText;
                        getResponseTxt = objWinRq.ResponseText;

                    }
                    return getResponseTxt;
                    //getResponseTxt = "[" + getResponseTxt + "]";
                }
                catch (Exception EX)
                {
                    throw EX;
                }
            }
            return "";
        }

        public void SendSVH_OTP(string mob, string name, string client, string email = "", string ccode = "", string dealer = "")
        {
            string sql = "select count(*) from svhotp where mob ='" + mob + "' and isnull(client,'')='" + client + "' ";
            int cnt = Convert.ToInt16(database.GetScalarValue(sql));
            Int64 rnd = 0;
            string msg = "";
            if (cnt == 0)
            {
                string acode = Convert.ToString(database.GetScalarValue("select TOP 1 A.acode from agentmast A LEFT JOIN SVHOTP S ON S.DealerCode = A.DealerCode AND S.acode = A.ACode AND S.client='" + client + "' where A.DealerCode = '" + dealer + "' GROUP BY A.ACode ORDER BY COUNT(A.ACODE)"));
                rnd = (new Random()).Next(1, 99999);
                sql = " Insert into svhotp (fullname, mob, otp, client,email,countrycode,dealercode,acode) values ('" + name + "','" + mob + "','" + rnd.ToString() + "','" + client + "','" + email + "','" + ccode + "','" + dealer + "','" + acode + "')";
                database.ExecuteNonQuery(sql);
                DataTable dtA = database.GetDataTable("Select * from agentmast where acode='" + acode + "' and dealercode='" + dealer + "'");
                if (dtA.Rows.Count > 0)
                {
                    if (ccode == "91")
                    {
                        msg = "Prospect for EMAAR - Name- " + name + " Mobile- " + ccode + " " + mob + " email- " + email;
                        SendSMSthroughAPI(dtA.Rows[0]["Amob1"].ToString(), msg, client);
                    }
                    else
                    {
                        //msg = "Hello!+Your+One+Time+Password+is+" + rnd.ToString() + "+.+Kindly+fill+to+process+your+request.";
                        msg = "Prospect+for+EMAAR+-+Name-+" + name + "+Mobile-++" + ccode + "+" + mob + "+email-+" + email;
                        SendSMSthroughAPICountrycode(dtA.Rows[0]["Amob1"].ToString(), msg, dtA.Rows[0]["countrycode"].ToString(), "");
                    }
                }
            }
            else
            {
                sql = "select top 1 otp from svhotp where mob ='" + mob + "' and isnull(client,'')='" + client + "' ";
                rnd = Convert.ToInt64(database.GetScalarValue(sql));
            }

            string clientNm = GetClientName(client, 1);

            msg = "Dear " + name + ", " + rnd.ToString() + " is OTP for your mobile number verification for " + clientNm;


            if (clientNm == "EMAAR")
            {

                if (ccode == "91")
                {
                    msg = "Your SMS verification code is:" + rnd.ToString();
                    SendSMSthroughAPI(mob, msg, client);
                }
                else
                {
                    msg = "Hello!+Your+One+Time+Password+is+" + rnd.ToString() + "+.+Kindly+fill+to+process+your+request.";
                    SendSMSthroughAPICountrycode(mob, msg, ccode);
                }
            }
            else
            {
                SendSMSthroughAPI(mob, msg, client);
            }
            #region <commented >
            // sql = @"INSERT INTO [dbo].[MSGTRAN]
            //([PROVIDER]
            //,[SMPPACCOUNTID]
            //,[PROFILEID]
            //,[MSGTEXT]
            //,[TOMOBILE]
            //,[SENDERID]
            //,[CREATEDAT]
            //,[PICKED_DATETIME],[PEID],[DataCode])
            // VALUES
            //(
            //'VCON', '301'
            //, '" + USER + @"'
            //, '" + msg + @"'
            //, '" + mob + @"'
            //, '" + s + @"'
            //, GETDATE()
            //, NULL,'" + peid + "','Default')";
            // dbmain.ExecuteNonQuery(sql);
            #endregion

        }

        public void SendSMSthroughAPI(string mob, string msg, string client)
        {
            string clientID = "";
            if (client.ToUpper() == "MIM2000002")
                clientID = client;
            else
                clientID = GetClientName(client, 2);
            DataTable dt = GetUserParameter(clientID);



            DataRow dr = dt.Rows[0];
            string countryCode = Convert.ToString(dt.Rows[0]["defaultCountry"]);
            //string apikey = ob.getIPapikey();
            string url = "";
            if (countryCode != "91")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + dr["pwd"].ToString() + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=16";
            else
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + dr["pwd"].ToString() + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=13&peid=" + dr["PEID"].ToString();
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                getResponseTxt = "[" + getResponseTxt + "]";
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public void SendSMSthroughAPI_forOTP(string mob, string msg, string client, string pwd, string apiurl, string TemplateId)
        {

            DataTable dt = GetUserParameter(client);
            DataRow dr = dt.Rows[0];
            string countryCode = Convert.ToString(dt.Rows[0]["defaultCountry"]);
            try
            {
                if (countryCode != "91")
                {
                    MIM.SendOTPthroughAPI(apiurl, mob, dr["username"].ToString(), dr["pwd"].ToString(), dr["senderid"].ToString(), msg, countryCode, dr["PEID"].ToString(), TemplateId);
                }
                else
                {
                    dt = GetUserParameter("MIM2000002");
                    dr = dt.Rows[0];
                    MIM.SendOTPthroughAPI(apiurl, mob, dr["username"].ToString(), dr["pwd"].ToString(), dr["senderid"].ToString(), msg, countryCode, dr["PEID"].ToString(), TemplateId);
                }

                //DataTable dt = GetUserParameter(client);
                //DataRow dr = dt.Rows[0];
                //string countryCode = Convert.ToString(dt.Rows[0]["defaultCountry"]);
                ////string apikey = ob.getIPapikey();
                //string url = "";
                //if (countryCode != "91")
                //{
                //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=16";//dr["pwd"].ToString() Shishir
                //}
                //else
                //{
                //    dt = GetUserParameter("MIM2000002");
                //    dr = dt.Rows[0];
                //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=13&peid=" + dr["PEID"].ToString();//dr["pwd"].ToString()

                //}
                //string getResponseTxt = "";
                //string getStatus = "";
                //WinHttp.WinHttpRequest objWinRq;
                //objWinRq = new WinHttp.WinHttpRequest();
                //try
                //{
                //    objWinRq.Open("GET", url, false);
                //    objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                //    objWinRq.Send(null);

                //    while (!(getStatus != "" && getResponseTxt != ""))
                //    {
                //        getStatus = objWinRq.Status + objWinRq.StatusText;
                //        getResponseTxt = objWinRq.ResponseText;
                //    }
                //    getResponseTxt = "[" + getResponseTxt + "]";
                //}
                //catch (Exception EX)
                //{
                //    throw EX;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendSMSthroughAPICountrycode(string mob, string msg, string countrycode, string executivesms = "")
        {
            //countrycode = "91";
            //string clientID = GetClientName(client, 2);

            string sqlCC = "select * from clientlogin where userid = 'emaar1'";
            DataTable dt = database.GetDataTable(sqlCC);
            string sender = Convert.ToString(dt.Rows[0]["SENDERID"]);
            if (executivesms == "Y") sender = Convert.ToString(dt.Rows[0]["EXECUTIVE_SENDERID"]);
            // string ccCode = Convert.ToString(dt.Rows[0]["COUNTRYCODE"]);
            //if (countrycode == "44")
            //    sender = "PREMIUM";
            //else if (countrycode == "971")
            //    sender = "MYINBXMEDIA";
            //   if (ccCode == "") ccCode = countrycode;
            //DataTable dt = GetUserParameter(clientID);
            //DataRow dr = dt.Rows[0];;
            //string apikey = ob.getIPapikey();
            if (countrycode == "971")
            {
                if (executivesms != "Y") msg = msg + "+OPTOUT+7726";
            }

            if (mob.StartsWith("+"))
            {
                mob.Replace("+", "");
                mob = mob.Substring(countrycode.Length + 1);
            }
            //string url = "http://bulksmsindia.mobi/sendurlcomma.aspx?user=20095087&pwd=Vip@123456&senderid=" + sender + "&CountryCode=" + countrycode + "&mobileno=" + mob + "&msgtext=" + msg + "&smstype=0/4/3";
            //string s1 = "http://bulksmsindia.mobi/sendurlcomma.aspx?user=20095087&pwd=Vip@123456&senderid=PREMIUM&CountryCode=44&mobileno=7961526996&msgtext=test&smstype=0/4/3";

            string url = "https://mshastra.com/sendurl.aspx?user=20095087&pwd=Vip@123456&senderid=" + sender + "&mobileno=" + mob + "&msgtext=" + msg + "&CountryCode=+" + countrycode;

            // string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + dr["pwd"].ToString() + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=13&peid=" + dr["PEID"].ToString();
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                getResponseTxt = "[" + getResponseTxt + "]";
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public void ResendOldOTP(string client, string mob, string name, string ccode = "")
        {
            string sql = "select top 1 otp from svhotp where right(mob," + mob.Length + ") ='" + mob + "' and isnull(client,'')='" + client + "' ";
            var otp = database.GetScalarValue(sql);
            string rnd = Convert.ToString(otp);
            if (!string.IsNullOrEmpty(rnd))
            {
                string clientNm = GetClientName(client, 1);

                string msg = "Dear " + name + ", " + rnd.ToString() + " is OTP for your mobile number verification for " + clientNm;

                if (clientNm == "EMAAR")
                {
                    if (ccode == "91")
                    {
                        msg = "Your SMS verification code is:" + rnd.ToString();
                        SendSMSthroughAPI(mob, msg, client);
                    }
                    else
                    {
                        msg = "Hello! Your One Time Password is " + rnd.ToString() + " . Kindly fill to process your request.";
                        SendSMSthroughAPICountrycode(mob, msg, ccode);
                    }
                }
                else
                {
                    SendSMSthroughAPI(mob, msg, client);
                }
            }
        }

        public bool SVHSubmit(string name, string mob, string otp, string client, string ccode = "", string dealer = "")
        {
            string sql = "select count(*) from svhotp where right(mob," + mob.Length + ") ='" + mob + "' and otp = '" + otp + "' and isnull(client,'')='" + client + "'";
            int cnt = Convert.ToInt16(database.GetScalarValue(sql));
            if (cnt == 0) return false;
            else
            {
                sql = "select count(*) from svhotp where right(mob," + mob.Length + ") ='" + mob + "' and otp = '" + otp + "' and isnull(client,'')='" + client + "' and verifiedon is null";
                int cn = Convert.ToInt16(database.GetScalarValue(sql));
                if (cn == 1)
                {
                    string sqlCC = string.Format("select * from clientlogin where userid ='{0}'", client);
                    DataTable dt = database.GetDataTable(sqlCC);
                    database.ExecuteNonQuery("Update svhotp set verifiedon=getdate() where right(mob," + mob.Length + ") ='" + mob + "' and otp = '" + otp + "' and isnull(client,'')='" + client + "'");
                    string clientNm = GetClientName(client, 1);
                    string cc = GetClientName(client, 3);
                    // string Shapoor_pwd = "i6;J8K._(DWu";
                    // password default  "IP#396395"                    
                    string mailmsg = clientNm + " Registration. Name : " + name + ". Mobile: +" + ccode + " " + mob;
                    string re = "";
                    if (clientNm == "EMAAR")
                    {
                        re = SendEmailSVH(Convert.ToString(dt.Rows[0]["ToEmail"]), clientNm + " Registration", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net", Convert.ToString(dt.Rows[0]["CCEmail"]));
                        //971
                        //SendSMSthroughAPICountrycode(Convert.ToString(dt.Rows[0]["Mob"]), mailmsg, ccode);
                    }
                    else if (clientNm == "SHAPOORJI_BANDRA")
                        re = SendEmailSVH("singh.prashant2608@gmail.com", clientNm + " Registration", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net", cc);
                    else if (clientNm == "SHAPOORJI")
                        re = SendEmailSVH("vipin@myinboxmedia.com", clientNm + " Registration", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net", "BISHNU.PUROHIT@shapoorji.com");
                    else if (clientNm == "360EDGE")
                        re = SendEmailSVH("avinash.verma@360realtors.com", clientNm + " Registration", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net", cc);
                    else
                        re = SendEmailSVH("vipin@myinboxmedia.com", clientNm + " Registration", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net", cc);


                }
                return true;
            }
        }

        public string GetClientName(string client, int type)
        {
            string clientNm = "";
            string clientID = "";
            string cc = "";
            if (client == "goldenbricks") { clientNm = "Golden Bricks"; clientID = "MIM2002171"; cc = "libin.goldenbricks@gmail.com"; }
            if (client == "drio") { clientNm = "DRIO"; clientID = "MIM2002176"; cc = ""; }
            if (client == "sgm") { clientNm = "SGM GROUP"; clientID = "MIM2002114"; cc = ""; }
            if (client == "birla") { clientNm = "BIRLA"; clientID = "MIM2000025"; cc = ""; }
            if (client == "mapsko") { clientNm = "MAPSKO"; clientID = "MIM2002173"; cc = ""; }
            //add client here for test
            if (client == "MIM") { clientNm = "MyInboxMedia"; clientID = "MIM2000002"; cc = ""; }
            if (client == "Shapoorji") { clientNm = "SHAPOORJI"; clientID = "MIM2101267"; cc = ""; }
            if (client == "Shapoorji_bandra") { clientNm = "SHAPOORJI_BANDRA"; clientID = "MIM2101267"; cc = ""; }
            if (client == "360Edge") { clientNm = "360EDGE"; clientID = "MIM2101278"; cc = ""; }
            if (client == "emaar") { clientNm = "EMAAR"; clientID = "MIM2100015"; cc = ""; }
            if (client == "emaar2") { clientNm = "EMAAR"; clientID = "MIM2100015"; cc = ""; }
            if (client == "emaar1") { clientNm = "EMAAR"; clientID = "MIM2100015"; cc = ""; }
            if (client == "emaar_ruba") { clientNm = "EMAAR"; clientID = "MIM2100015"; cc = ""; }

            return (type == 1 ? clientNm : (type == 2 ? clientID : cc));
        }

        public string SendEmailSVH(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string cc)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                body = "Hello This is test email for Notification !!";
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                //message.CC.Add("software.in2010@gmail.com");
                //message.CC.Add("manojramavtar@gmail.com");
                if (cc != "")
                    message.CC.Add(cc);
                else
                    //360 edge client
                    //if (toAddress == "avinash.verma@360realtors.com" || toAddress == "akshel.kuruvilla@shapoorji.com")
                    message.CC.Add("vipin@myinboxmedia.com");

                //message.CC.Add("vikas.walia@myinboxmedia.com");
                message.Bcc.Add("anirudh@myinboxmedia.com");
                message.Bcc.Add("dilip@myinboxmedia.com");
                //Attachment item = new Attachment(fn);
                //message.Attachments.Add(item);
                //smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        //svh <-------------

        public DataTable GetScheduleLogReport(string user, string f, string t)
        {
            DataTable dt = new DataTable("dt");

            //            string sql = @"select DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,Schedule,106) + ' ' + convert(varchar(5),schedule,108)) as Schedule, count(*) as mobiles,isnull(fileid,0) as fileid,smsrate,
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 160 then 1 else
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 306 then 2 else
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 459 then 3 else
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 612 then 4 else
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 765 then 5 else
            //case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 918 then 6 else 7 end end end end end end 
            //as noofsms from msgschedule where profileid='" + user + "' and schedule between '" + f + "' and DateAdd(MINUTE," + Math.Abs(Global.addMinutes) + @",'" + t + @"') and picked_datetime is null 
            //group by DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,Schedule,106) + ' ' + convert(varchar(5),schedule,108)),isnull(fileid,0),smsrate,len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) order by Schedule";

            string sql = @"select DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.Schedule,106) + ' ' + convert(varchar(5),m.schedule,108)) as Schedule, count(*) as mobiles,m.smsrate,
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 160 then 1 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 306 then 2 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 459 then 3 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 612 then 4 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 765 then 5 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 918 then 6 else 7 end end end end end end 
as noofsms,f.fileName,f.campname,f.msg,ISNULL(m.FILEID,0) AS FILEID  from msgschedule m with (nolock) 
left join SMSFILEUPLOAD u with (nolock) on u.id=m.FILEID and u.USERID=m.PROFILEID
left join FileProcess f with (nolock) on u.fileprocessid=f.id
where m.profileid='" + user + @"'
and m.schedule between '" + f + "' and DateAdd(MINUTE," + Math.Abs(Global.addMinutes) + @",'" + t + @"') and picked_datetime is null
group by DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.Schedule,106) + ' ' + convert(varchar(5),m.schedule,108)),m.smsrate,len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) ,f.fileName,f.campname,f.msg,ISNULL(m.FILEID,0) order by Schedule
";

            dt = dbmain.GetDataTable(sql);
            return dt;
        }

        public DataTable GetMobileNumbersOfSchedule(string user, string sch)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select tomobile,'""'+msgtext+'""', DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,Schedule,106) + ' ' + convert(varchar(5),schedule,108)) as schedule from msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + sch + "') ";
            dt = dbmain.GetDataTable(sql);
            return dt;
        }

        public Int64 GetNoOfMobiles4FileID(string user, string schdt)
        {
            string sql = @"select count(*) from msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + "') and picked_datetime is null";
            return Convert.ToInt64(dbmain.GetScalarValue(sql));
        }

        public DataSet GetCampaignWiseDataOld(string user, string f, string t, string camp)
        {

            DataSet ds = new DataSet("ds");
            string sql = "";
            sql = @";with ReportCTE as ( select f.UPLOADTIME as requestdate,f.FILENM, f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
count(s.id) * isnull(f.smsrate,0) as credit,count(s.id) as smscount,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
left join delivery d with(nolock) on s.msgid = d.msgid 
left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
left join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE
left join mobstats ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and f.campaignname is not null";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by f.UPLOADTIME,f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate )
            select RequestDate,FILENM,Campaign,sender,message,credit,smscount, convert(varchar,delivered) + '  (' + cast((delivered * 100/smscount) as varchar) + '% )' [delivered], 

            convert(varchar,failed) + '  (' + cast((failed * 100/smscount) as varchar) + '% )' [failed],
			 convert(varchar,AWAITED) + '  (' + cast((AWAITED * 100/smscount) as varchar) + '% )' [AWAITED],
			convert(varchar,openmsg) + '  (' + cast((openmsg * 100/smscount) as varchar) + '% )' [openmsg]
		from ReportCTE order by requestdate";

            sql += @" ;with ReportCTE1 as ( select f.UPLOADTIME as requestdate,f.FILENM, f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
count(s.id) * isnull(f.smsrate,0) as credit,count(s.id) as smscount,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
left join delivery d with(nolock) on s.msgid = d.msgid 
left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
left join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE
left join mobstats ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and f.campaignname is not null";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by f.UPLOADTIME,f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate,s.fileid,s.createdat )
            select requestdate,FILENM,campaign,sender,message,credit,smscount, convert(varchar,delivered) + '  (' + cast((delivered * 100/smscount) as varchar) + '% )' [delivered], 
			convert(varchar,failed) + '  (' + cast((failed * 100/smscount) as varchar) + '% )' [failed],
			 convert(varchar,AWAITED) + '  (' + cast((AWAITED * 100/smscount) as varchar) + '% )' [AWAITED],
			convert(varchar,openmsg) + '  (' + cast((openmsg * 100/smscount) as varchar) + '% )' [openmsg]
		from ReportCTE1 order by requestdate";


            ds = database.GetDataSet(sql);
            return ds;
        }

        public DataSet GetCampaignWiseData(string user, string f, string t, string camp)
        {

            DataSet ds = new DataSet("ds");
            string sql = "";
            sql = @"select mbs.shorturl_id,mbs.urlid into #tmp1 from mobstats mbs with(nolock) inner join short_urls su with(nolock) on mbs.shortUrl_id=su.id 
where su.userid='" + user + @"' and mbs.click_date>='" + f + @"' group by mbs.shorturl_id,mbs.urlid 

;with ReportCTE as ( select CONVERT(varchar(10),s.sentdatetime,105) as requestdate,f.FILENM, f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
left join delivery d with(nolock) on s.msgid = d.msgid 
left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)<=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME  
left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by CONVERT(varchar(10),s.sentdatetime,105),f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate )
            select RequestDate,FILENM,Campaign,sender,message,credit,smscount, delivered, (delivered * 100/smscount) [delivered_p], 
            failed,  (failed * 100/smscount) [failed_p],
			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
			openmsg,  (openmsg * 100/smscount) [openmsg_p]
		from ReportCTE order by requestdate desc";




            //
            // AND m.fileid=s.fileid
            //            sql += @" ;with ReportCTE1 as ( select convert(varchar,s.createdat,106) + ' ' + convert(varchar(5),s.createdat,108) as requestdate,f.FILENM, 
            //f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
            //Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
            //sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            //sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
            //,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
            //sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
            //from smsfileupload f with(nolock)
            //inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
            //left join delivery d with(nolock) on s.msgid = d.msgid 
            //left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
            //LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME
            //left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
            //where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            //            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            //            sql = sql + @" group by f.UPLOADTIME,f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate,s.fileid,s.createdat )
            //            select DATEADD(MINUTE," + Global.addMinutes + @",RequestDate) as RequestDate,FILENM,campaign,sender,message,credit,smscount,  delivered, (delivered * 100/smscount) [delivered_p], 
            //            failed,  (failed * 100/smscount) [failed_p],
            //			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
            //			openmsg,  (openmsg * 100/smscount) [openmsg_p]
            //		from ReportCTE1 order by requestdate desc";

            sql += @" ;with ReportCTE1 as ( select case when s1.schedule IS NULL then convert(varchar,s.SENTDATETIME,102) else s1.schedule end  as requestdate,f.FILENM, 
f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
LEFT join MsgSchedule s1 with(nolock) on  s.TOMOBILE=s1.TOMOBILE and s.FILEID=s1.FILEID and convert(varchar,s1.schedule,102)=convert(varchar,s.sentdatetime,102)
left join delivery d with(nolock) on s.msgid = d.msgid 
left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)<=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME 
left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate,s.fileid,
            case when s1.schedule IS NULL then convert(varchar,s.SENTDATETIME,102) else s1.schedule end)
            select DATEADD(MINUTE," + Global.addMinutes + @",RequestDate) as RequestDate,FILENM,campaign,sender,message,credit,smscount,  delivered, (delivered * 100/smscount) [delivered_p], 
            failed,  (failed * 100/smscount) [failed_p],
			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
			openmsg,  (openmsg * 100/smscount) [openmsg_p]
		from ReportCTE1 order by requestdate desc";

            ds = database.GetDataSet(sql);
            return ds;
        }

        public DataSet GetCampaignWiseData_new(string user, string f, string t, string camp)
        {

            DataSet ds = new DataSet("ds");
            string sql = "";
            sql = @"select mbs.shorturl_id,mbs.urlid into #tmp1 from mobstats mbs with(nolock) inner join short_urls su with(nolock) on mbs.shortUrl_id=su.id 
where su.userid='" + user + @"' and mbs.click_date>='" + f + @"' group by mbs.shorturl_id,mbs.urlid 

select CONVERT(varchar(10),s.sentdatetime,105) as requestdate,cast(s.sentdatetime as date) ordDate,
f.FILENM, f.campaignname as campaign ,f.senderid as sender,f.id,
Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount
,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered
,sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED
into #tmpDelv
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.id=s.fileid and f.userid = s.profileid
left join delivery d with(nolock) on s.msgid = d.msgid 
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by CONVERT(varchar(10),s.sentdatetime,105),cast(s.sentdatetime as date)
,f.FILENM,f.campaignname,f.senderid,
f.reccount,f.smsrate ,f.id;

select CONVERT(varchar(10),s.sentdatetime,105) as requestdate,
f.FILENM, f.campaignname as campaign ,f.senderid as sender,f.id
,sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
 into #tmpCamp 
 from smsfileupload f with(nolock)
 inner join MSGSUBMITTED s with(nolock) on f.id=s.fileid and f.userid = s.profileid 
 left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
 LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND F.ID=M.fileId 
 left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @"
            group by CONVERT(varchar(10),s.sentdatetime,105)
,f.FILENM,f.campaignname,f.senderid,
f.reccount,f.smsrate ,f.id;

select t.RequestDate,t.FILENM,t.Campaign,t.sender,credit,smscount,
delivered, (delivered * 100/smscount) [delivered_p],
failed,  (failed * 100/smscount) [failed_p],
AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
openmsg,  (openmsg * 100/smscount) [openmsg_p]
from #tmpDelv t INNER JOIN #tmpCamp c ON t.id =c.ID
order by t.ordDate";

            //sql += @" ;
            //select t.RequestDate,t.FILENM,t.Campaign,t.sender,credit,smscount,
            //delivered, (delivered * 100/smscount) [delivered_p],
            //failed,  (failed * 100/smscount) [failed_p],
            //AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
            //openmsg,  (openmsg * 100/smscount) [openmsg_p]
            //from #tmpDelv t INNER JOIN #tmpCamp c ON t.id =c.ID
            //order by t.ordDate";


            ds = database.GetDataSet(sql);
            return ds;
        }

        public DataTable GetCampaignWiseDataDownload(string user, string f, string t, string camp)
        {

            DataTable dt = new DataTable("dt");
            string sql = "";
            sql = @"select mbs.shorturl_id,mbs.urlid into #tmp1 from mobstats mbs with(nolock) inner join short_urls su with(nolock) on mbs.shortUrl_id=su.id 
where su.userid='" + user + @"' and mbs.click_date>='" + f + @"' group by mbs.shorturl_id,mbs.urlid 

;with ReportCTE as ( select CONVERT(varchar(10),s.sentdatetime,105) as requestdate,f.FILENM, f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
from smsfileupload f with(nolock)
inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
left join delivery d with(nolock) on s.msgid = d.msgid 
left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)<=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME  
left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id
where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
            sql = sql + @" group by CONVERT(varchar(10),s.sentdatetime,105),f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate )
            select RequestDate,FILENM,Campaign,sender,message,credit,smscount, delivered, (delivered * 100/smscount) [delivered_p], 
            failed,  (failed * 100/smscount) [failed_p],
			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
			openmsg,  (openmsg * 100/smscount) [openmsg_p]
		from ReportCTE order by requestdate desc";

            dt = database.GetDataTable(sql);
            return dt;
        }

        //        public DataSet GetCampaignWiseData(string user, string f, string t, string camp)
        //        {

        //            DataSet ds = new DataSet("ds");
        //            string sql = "";
        //            sql = @"select mbs.shorturl_id,mbs.urlid into #tmp1 from mobstats mbs with(nolock) inner join short_urls su with(nolock) on mbs.shortUrl_id=su.id 
        //where su.userid='" + user + @"' and mbs.click_date>='" + f + @"' group by mbs.shorturl_id,mbs.urlid 

        //;with ReportCTE as ( select cast(s.sentdatetime as date) as requestdate,f.FILENM, f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
        //Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
        //sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
        //sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
        //,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
        //sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
        //from smsfileupload f with(nolock)
        //inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
        //left join delivery d with(nolock) on s.msgid = d.msgid 
        //left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
        //LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)<=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME  
        //left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id
        //where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
        //            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
        //            sql = sql + @" group by cast(s.sentdatetime as date),f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate )
        //            select  RequestDate,FILENM,Campaign,sender,message,credit,smscount, delivered, (delivered * 100/smscount) [delivered_p], 
        //            failed,  (failed * 100/smscount) [failed_p],
        //			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
        //			openmsg,  (openmsg * 100/smscount) [openmsg_p]
        //		from ReportCTE order by requestdate desc";

        //            //
        //            // AND m.fileid=s.fileid
        //            //            sql += @" ;with ReportCTE1 as ( select convert(varchar,s.createdat,106) + ' ' + convert(varchar(5),s.createdat,108) as requestdate,f.FILENM, 
        //            //f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
        //            //Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
        //            //sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
        //            //sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
        //            //,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
        //            //sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
        //            //from smsfileupload f with(nolock)
        //            //inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
        //            //left join delivery d with(nolock) on s.msgid = d.msgid 
        //            //left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
        //            //LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME
        //            //left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
        //            //where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
        //            //            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
        //            //            sql = sql + @" group by f.UPLOADTIME,f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate,s.fileid,s.createdat )
        //            //            select DATEADD(MINUTE," + Global.addMinutes + @",RequestDate) as RequestDate,FILENM,campaign,sender,message,credit,smscount,  delivered, (delivered * 100/smscount) [delivered_p], 
        //            //            failed,  (failed * 100/smscount) [failed_p],
        //            //			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
        //            //			openmsg,  (openmsg * 100/smscount) [openmsg_p]
        //            //		from ReportCTE1 order by requestdate desc";

        //            sql += @" ;with ReportCTE1 as ( select case when s1.schedule IS NULL then convert(varchar,s.SENTDATETIME,102) else s1.schedule end  as requestdate,f.FILENM, 
        //f.campaignname as campaign ,f.senderid as sender,left(s.smsTEXT,30) as message,
        //Cast(round(count(s.id) * isnull(f.smsrate,0)/100,2) as decimal(10,2)) as credit,count(s.id) as smscount,
        //sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
        //sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
        //,sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as AWAITED,
        //sum(case when ms.SHORTURL_ID is null then 0 else 1 end) AS openmsg
        //from smsfileupload f with(nolock)
        //inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
        //LEFT join MsgSchedule s1 with(nolock) on  s.TOMOBILE=s1.TOMOBILE and s.FILEID=s1.FILEID and convert(varchar,s1.schedule,102)=convert(varchar,s.sentdatetime,102)
        //left join delivery d with(nolock) on s.msgid = d.msgid 
        //left join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
        //LEFT join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE AND CONVERT(VARCHAR,F.UPLOADTIME,102)<=CONVERT(VARCHAR,M.SENTDATE,102) AND M.SENTDATE>F.UPLOADTIME 
        //left join #tmp1 ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id 
        //where f.userid = '" + user + @"' and s.sentdatetime between '" + f + @"' and '" + t + @"' and isnull(f.campaignname,'')<>'' ";
        //            if (camp != "" && camp != "0") sql = sql + @" and f.campaignname = '" + camp + "' ";
        //            sql = sql + @" group by f.FILENM,f.campaignname,f.senderid,left(s.smsTEXT, 30),f.reccount,f.smsrate,s.fileid,
        //            case when s1.schedule IS NULL then convert(varchar,s.SENTDATETIME,102) else s1.schedule end)
        //            select DATEADD(MINUTE," + Global.addMinutes + @",RequestDate) as RequestDate,FILENM,campaign,sender,message,credit,smscount,  delivered, (delivered * 100/smscount) [delivered_p], 
        //            failed,  (failed * 100/smscount) [failed_p],
        //			 AWAITED, (AWAITED * 100/smscount) [AWAITED_p],
        //			openmsg,  (openmsg * 100/smscount) [openmsg_p]
        //		from ReportCTE1 order by requestdate desc";

        //            ds = database.GetDataSet(sql);
        //            return ds;
        //        }

        public DataTable GetCampaignToday(string user)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select campaignname from smsfileupload where userid='" + user + "' and isnull(campaignname,'') <> '' and convert(varchar,uploadtime,102)=convert(varchar,getdate(),102) ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCampaignAll(string user)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select distinct campaignname from smsfileupload where userid='" + user + "' and isnull(campaignname,'') <> '' order by campaignname ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCampaignAllNull()
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select distinct campaignname from smsfileupload where isnull(campaignname,'') <> '' order by campaignname ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public void UpdateLastActivity(string user)
        {
            string sql = "Update logindtl set lastactivitytime=getdate() where userid = '" + user + "'";
            database.ExecuteNonQuery(sql);

        }

        public void InsertMsgTomsgtranForTemplateTest(string smppAcId, string profileId, string msg, string mobile, string senderId, string fileId, string peid, string templId, string dataCode)
        {
            string sql = string.Format("INSERT INTO MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,Templateid)" +
                " Values('Test', '{0}','{1}', N'{2}', '{3}','{4}',GETDATE(),'{5}','{6}','{7}','{8}')", smppAcId, profileId, msg, mobile, senderId, fileId, peid, dataCode, templId);
            database.ExecuteNonQuery(sql);
        }
        // --------------add new method here

        public DataTable Daily()
        {
            DataTable dtSetting = database.GetDataTable("select DailyNotify,convert(varchar(16),DailySentOn,120) [DailySentOn] from settings");
            string UpDate = ""; string userid = "";
            string sql = "";
            string final = "";
            sql = @"Select Email,Mobileno,UserName,convert(varchar(16),dailySenton,120) [dailySenton],convert(varchar(16),getdate(),120) [tdyDate] from notification where Daily=1";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string Email = dr["Email"].ToString();
                string Mobileno = dr["Mobileno"].ToString();
                string username = dr["UserName"].ToString();
                //  string daily = dr["dailySenton"].ToString();
                if (Convert.ToBoolean(dtSetting.Rows[0]["DailyNotify"]) == true && Convert.ToDateTime(dtSetting.Rows[0]["dailySenton"]) == Convert.ToDateTime(dr["tdyDate"]))
                {
                    string check = @"select DATEDIFF(Day,dailySenton,GETDATE()) AS date from notification where UserName='" + username + "'";
                    DataTable dc = new DataTable("dc");
                    try
                    {
                        dc = database.GetDataTable(check);
                        if (Convert.ToInt16(dc.Rows[0]["date"]) >= 1)
                        {
                            final = @"select SMSDATE,USERID,sum(SUBMITTED) as SUBMITTED ,sum(DELIVERED) as DELIVERED ,sum(FAILED) as FAILED ,sum(UNKNOWN) as UNKNOWN  from daysummary where USERID ='" + username + "' and SMSDATE=Convert(Date,'2021-06-08')  Group by SMSDATE,USERID";
                            DataTable d = new DataTable("d");
                            d = database.GetDataTable(final);
                            String a = " UserId: ";
                            String b = " SMSDATE: ";
                            String c = " SUBMITTED: ";
                            String r = " DELIVERED: ";
                            String f = " FAILED: ";
                            string m = " UNKNOWN: ";
                            string message = "";
                            if (d.Rows.Count > 0)
                                message = a + d.Rows[0]["USERID"].ToString() + b + d.Rows[0]["SMSDATE"].ToString() + c + d.Rows[0]["SUBMITTED"].ToString()
                                    + r + d.Rows[0]["DELIVERED"].ToString() + f + d.Rows[0]["FAILED"].ToString() + m + d.Rows[0]["UNKNOWN"].ToString();
                            smssending(Mobileno, username, message);
                            SendEmailSVH(Email, "Daily Report", message, "noreply@textiyapa.com", "IP#396395", "Host", "");
                            UpDate = @"Update notification set dailySenton= getdate() where UserName='" + username + "'";
                            database.ExecuteNonQuery(UpDate);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            return dt;
        }
        public DataTable Weekly()
        {
            DataTable dtSetting = database.GetDataTable("select WeeklyNotify,convert(varchar(16),WeeklySentOn,120) [WeeklySentOn] from settings");

            string UpDate = "";
            string sql = "";
            string final = "";
            string check = "";
            sql = @"Select Email,Mobileno,UserName,convert(varchar(16),WeeklySentOn,120) [WeeklySentOn],convert(varchar(16),getdate(),120) [tdyDate] from notification where weekly=1";

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string Email = dr["Email"].ToString();
                string Mobileno = dr["Mobileno"].ToString();
                string userName = dr["UserName"].ToString();
                //if (Convert.ToBoolean(dtSetting.Rows[0]["WeeklyNotify"]) == true && Convert.ToDateTime(dtSetting.Rows[0]["WeeklySenton"]) == Convert.ToDateTime(dr["tdyDate"]))
                {
                    // string week = dr["dailySenton"].ToString();
                    check = @"select DATEDIFF(Week,weeklysenton,GETDATE()) AS date from notification where UserName='" + userName + "'";
                    DataTable dc = new DataTable("dc");
                    dc = database.GetDataTable(check);
                    try
                    {
                        if (Convert.ToInt16(dc.Rows[0]["date"]) >= 1)
                        {
                            final = @"select convert(varchar(11),SMSDATE,105) [SMSDATE],USERID,sum(SUBMITTED) as SUBMITTED ,sum(DELIVERED) as DELIVERED ,sum(FAILED) as FAILED ,sum(UNKNOWN) as UNKNOWN  from dAysUmMary where USERID ='" + userName + "' and SMSDATE >=DATEADD(day,-7,Convert (Date,GetDate())) Group by SMSDATE,USERID";
                            DataTable d = new DataTable("d");
                            d = database.GetDataTable(final);
                            String a = " UserId: ";
                            String b = " SMSDATE: ";
                            String c = " SUBMITTED: ";
                            String r = " DELIVERED: ";
                            String f = " FAILED: ";
                            string m = " UNKNOWN: ";
                            string toDate = d.AsEnumerable().Last()["SMSDATE"].ToString();
                            object submitted;
                            submitted = d.Compute("Sum(SUBMITTED)", string.Empty);
                            object delivered;
                            delivered = d.Compute("Sum(DELIVERED)", string.Empty);
                            object failed;
                            failed = d.Compute("Sum(FAILED)", string.Empty);
                            object unknown;
                            unknown = d.Compute("Sum(UNKNOWN)", string.Empty);

                            string message = "";
                            if (d.Rows.Count > 0)
                                message = a + d.Rows[0]["USERID"].ToString() + b + d.Rows[0]["SMSDATE"].ToString() + " - " + toDate + c + submitted.ToString()
                                    + r + delivered.ToString() + f + failed.ToString() + m + unknown.ToString();
                            smssending(Mobileno, userName, message);
                            SendEmailSVH(Email, "Weekly Report", message, "noreply@textiyapa.com", "IP#396395", "Host", "");
                            //        UpDate = @"Update notification set weeklysenton= getdate() where UserName='" + userName + "'";
                            database.ExecuteNonQuery(UpDate);
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return dt;
        }

        public DataTable Monthly()
        {
            DataTable dtSetting = database.GetDataTable("select MonthlyNotify,convert(varchar(16),MonthlySentOn,120) [MonthlySentOn] from settings");

            string premonth = "";
            string UpDate = "";
            string sql = "";
            string final = "";
            string check = "";
            sql = @"Select *,convert(varchar(16),getdate(),120)[tdyDate] from notification where Monthly=1 ";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string Email = dr["Email"].ToString();
                string Mobileno = dr["Mobileno"].ToString();
                string value = dr["UserName"].ToString();
                //string week = dr["weeklysenton"].ToString();
                // if (Convert.ToBoolean(dtSetting.Rows[0]["MonthlyNotify"]) == true && Convert.ToDateTime(dtSetting.Rows[0]["monthlySenton"]) == Convert.ToDateTime(dr["tdyDate"]))
                {
                    check = @"select LEFT(datename(month,dateadd(month,0,monthlySenton)),3) as month from  notification where UserName='" + value + "'";
                    DataTable dP = new DataTable("dP");
                    dP = database.GetDataTable(check);
                    premonth = @"select LEFT(datename(month,dateadd(month,-1,getdate())),3) as month ";
                    DataTable dS = new DataTable("dS");
                    dS = database.GetDataTable(premonth);
                    try
                    {
                        if ((dP.Rows[0]["month"]) != (dS.Rows[0]["month"]))
                        {
                            final = @"select SMSDATE,USERID,sum(SUBMITTED) as SUBMITTED ,sum(DELIVERED) as DELIVERED ,sum(FAILED) as FAILED ,sum(UNKNOWN) as UNKNOWN  from dAysUmMary where USERID ='" + value + "' and SMSDATE >=DATEADD(day,-30,Convert (Date,GetDate())) Group by SMSDATE,USERID";
                            DataTable d = new DataTable("d");
                            d = database.GetDataTable(final);
                            object submitted;
                            submitted = d.Compute("Sum(SUBMITTED)", string.Empty);
                            object delivered;
                            delivered = d.Compute("Sum(DELIVERED)", string.Empty);
                            object failed;
                            failed = d.Compute("Sum(FAILED)", string.Empty);
                            object unknown;
                            unknown = d.Compute("Sum(UNKNOWN)", string.Empty);

                            String a = " UserId: ";
                            String b = " SMSDATE: ";
                            String c = " SUBMITTED: ";
                            String r = " DELIVERED: ";
                            String f = " FAILED: ";
                            string m = " UNKNOWN: ";
                            string userid = "";
                            string From = " FROMDATE: ";
                            string T = " TODATE: ";
                            string FD = @"select DATEADD(Month,DATEDIFF(MONTH,0,GETDATE())-1,0) AS FROMDATE,DATEADD(Month,DATEDIFF(MONTH,-1,GETDATE())-1,-1) as TODATE";
                            DataTable dF = new DataTable("dF");
                            dF = database.GetDataTable(FD);
                            string Record = a + d.Rows[0]["USERID"].ToString() + From + dF.Rows[0]["FROMDATE"].ToString() + T + dF.Rows[0]["TODATE"].ToString() + c + submitted.ToString()
                                 + r + delivered.ToString() + f + failed.ToString() + m + unknown.ToString();
                            smssending(Mobileno, userid, Record);
                            SendEmailSVH(Email, "Weekly Report", Record, "noreply@textiyapa.com", "IP#396395", "Host", "");
                            UpDate = @"Update notification set monthlySenton= getdate() where UserName='" + value + "'";
                            database.ExecuteNonQuery(UpDate);

                        }

                    }
                    catch (Exception ex)
                    { }
                }

            }
            return dt;


        }

        public void smssending(string Mobileno, string userid, string Record)
        {
            SendSMSthroughAPI(Mobileno, Record, userid);
            string a = "Send sms sucessfully";
        }

        public void emailsending(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string cc)
        {
            //"noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net"
            SendEmailSVH(toAddress, subject, body, MailFrom, Pwd, Host, cc);
        }

        public DataTable GetNotification(string userid, string richmedia = "")
        {
            string sql = "";

            sql = @"SELECT username, 
                    STUFF(
                    ISNULL(',' + Mobileno, '') + 
                    ISNULL(',' + mob1, '') + 
                    ISNULL(',' + mob2, '') + 
                    ISNULL(',' + mob3, '') +
                    ISNULL(',' + mob4, ''),1,1,'') as [MobileDetails],
                    STUFF(
                    ISNULL(',' + Email, '') + 
                    ISNULL(',' + Email1, '') + 
                    ISNULL(',' + Email2, '') + 
                    ISNULL(',' + Email3, '') +
                    ISNULL(',' + Email4, ''),1,1,'') as [EmailDetails],
                    Daily,Weekly,Monthly from notification where UserName='" + userid + "' ";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable UpdateNotification(string userid, string url, string Email, string MobileNo, bool Daily, bool Weekly, bool Monthly, string richmedia = "")
        {
            string count = "";
            count = @"select count(*) as cnt from Notification where Username='" + userid + "'";
            DataTable d = new DataTable("dt");
            d = database.GetDataTable(count);

            if (Convert.ToInt16(d.Rows[0]["cnt"]) == 0)
            {
                string sql = "";

                sql = @"Insert into notification(UserName,Mobileno,Email,Daily,Weekly,Monthly,dailySenton,weeklysenton,monthlySenton)
                      Values ('" + userid + "','" + MobileNo + "','" + Email + "','" + Daily + "','" + Weekly + "','" + Monthly + "',dateadd(d,-1,getdate()),dateadd(d,-7,getdate()),dateadd(m,-1,getdate()))";
                DataTable dt = new DataTable("dt");
                dt = database.GetDataTable(sql);
                return dt;

            }
            else
            {
                string sql = "";

                sql = @"update Notification set Mobileno='" + MobileNo + "',Email='" + Email + "',Daily='" + Daily + "',Weekly='" + Weekly + "',Monthly='" + Monthly + "' where UserName='" + userid + "'";
                DataTable dt = new DataTable("dt");
                dt = database.GetDataTable(sql);
                return dt;
            }
        }

        // --------------END new method here
        public string SaveBlackListMobileNo(string FilePath, string Operator, string UpdateDate, string EXT)
        {
            try
            {
                string moblen = "10";
                DataTable dt = new DataTable();
                if (EXT.Contains("TXT"))
                {
                    dt = ReadTextFile(FilePath, moblen);
                }
                else if (EXT.Contains("XLS"))
                {
                    dt = ReadExcel(FilePath, moblen);

                }

                if (dt.Rows.Count == 0)
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                else
                {
                    int Y = dt.Rows.Count;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sql = string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}') ;", dr["MobNo"], dr["MobNo"], Operator, UpdateDate);
                        sql += string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", dr["MobNo"], dr["MobNo"], Operator, UpdateDate);

                        //string sql128 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", cc + dr["MobNo"], cc + dr["MobNo"], Operator, UpdateDate);
                        //string sql17 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", cc + dr["MobNo"], cc + dr["MobNo"], Operator, UpdateDate);

                        //database.ExecuteNonQueryForMultipleConnection(sql, sql128, sql17);
                        database.ExecuteNonQuery(sql);
                    }
                    return "RECORDCOUNT " + Y.ToString();

                }

            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string SaveBlackListMobileNoEntry(List<string> moblist, string Operator, string UpdateDate)
        {
            try
            {
                string moblen = "10";


                if (moblist.Count == 0)
                    return "Mobile Numbers are not numeric. Please check. ";
                else
                {
                    //string cc = "91";
                    int Y = moblist.Count;
                    for (int i = 0; i < moblist.Count; i++)
                    {
                        string sql = string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}') ;", moblist[i], moblist[i], Operator, UpdateDate);
                        sql += string.Format("IF NOT EXISTS (SELECT * FROM globalblacklistno WHERE mobile = '{0}') INSERT INTO globalblacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", moblist[i], moblist[i], Operator, UpdateDate);

                        //string sql128 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", cc + moblist[i], cc + moblist[i], Operator, UpdateDate);
                        //string sql17 = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE mobile = '{0}') INSERT INTO blacklistno(mobile,Operator,UpdateDate) VALUES('{1}','{2}','{3}')", cc + moblist[i], cc + moblist[i], Operator, UpdateDate);

                        //database.ExecuteNonQueryForMultipleConnection(sql, sql128, sql17);
                        database.ExecuteNonQuery(sql);
                    }

                    return "RECORDCOUNT " + Y.ToString();

                }

            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string SaveBlackListMobileNo(string FilePath, string Operator, string UpdateDate, string UserId, string EXT) // As per User
        {
            try
            {
                string moblen = "10";
                DataTable dt = new DataTable();
                if (EXT.Contains("TXT"))
                {
                    dt = ReadTextFile(FilePath, moblen);
                }
                else if (EXT.Contains("XLS"))
                {
                    dt = ReadExcel(FilePath, moblen);
                }

                if (dt.Rows.Count == 0)
                    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
                else
                {
                    string cc = Convert.ToString(database.GetScalarValue("Select COUNTRYCODE from customer where userNAME='" + UserId + "'"));
                    int Y = dt.Rows.Count;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sql = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE MOBILENO = '{0}') INSERT INTO blacklistno(MOBILENO,Operator,UpdateDate,UserId) VALUES('{0}','{1}','{2}','{3}')", cc + dr["MobNo"], Operator, UpdateDate, UserId);
                        database.ExecuteNonQuery(sql);
                    }
                    return "RECORDCOUNT " + Y.ToString();

                }

            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string SaveBlackListMobileNoEntry(List<string> moblist, string Operator, string UpdateDate, string UserId) // As per User
        {
            try
            {
                if (moblist.Count == 0)
                    return "Mobile Numbers are not numeric. Please check. ";
                else
                {
                    string cc = Convert.ToString(database.GetScalarValue("Select COUNTRYCODE from customer where userNAME='" + UserId + "'"));
                    int Y = moblist.Count;
                    for (int i = 0; i < moblist.Count; i++)
                    {
                        string sql = string.Format("IF NOT EXISTS (SELECT * FROM blacklistno WHERE MOBILENO = '{0}') INSERT INTO blacklistno(MOBILENO,Operator,UpdateDate,UserId) VALUES('{0}','{1}','{2}','{3}')", cc + moblist[i], Operator, UpdateDate, UserId);

                        database.ExecuteNonQuery(sql);
                    }

                    return "RECORDCOUNT " + Y.ToString();

                }

            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public DataTable GetOperator()
        {
            string sql = "";

            sql = @"select provider,id from Operator ";
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable ReadExcel(string path, string moblen)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtresult = new DataTable();
                dtresult.Columns.Add("MobNo");

                OleDbDataAdapter OleDa;
                string excelcon = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 8.0", path);
                using (OleDbConnection con = new OleDbConnection(excelcon))
                {
                    con.Open();
                    DataTable dtschema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dtschema != null && dtschema.Rows.Count > 0)
                    {
                        string sheetname = dtschema.Rows[0]["TABLE_NAME"].ToString();
                        OleDa = new OleDbDataAdapter("Select * from [" + sheetname + "]", con);
                        OleDa.Fill(dt);
                    }
                }
                List<string> lines = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lines.Add(dt.Rows[i][0].ToString().Trim());
                }
                lines = lines.Select(t => Regex.Replace(t, "[^0-9]", "")).ToList();
                lines.RemoveAll(x => x.Length < Convert.ToInt16(moblen));
                lines = lines.Select(x => x.Substring(x.Length - Convert.ToInt16(moblen))).ToList();
                lines.ForEach((item) => dtresult.Rows.Add(item));
                return dtresult;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public DataTable GetCountryTimeZone(string cc)
        {
            string sql = "SELECT smsFromTime,smsToTime,timedifferenceInMinute,mobLength,CURRENCY,subDomain,SubCurrency,maximgfilesize," +
                "maxvideofilesize,isnull(NoticeMsg,'') NoticeMsg,promoStartTime,promoEndTime,MobMIN,MobMAX FROM tblCountry WITH(NOLOCK) WHERE counryCode='" + cc + "'";
            return database.GetDataTable(sql);
        }

        public DataTable GetSMPPSettingTimeZone(string type)
        {
            string sql = "";
            if (type == "6")
                sql = "Select smsFromTime,smsToTime from SMPPSETTING where isnull(Forpromotional,0)=1";
            else if (type == "3")
                sql = "Select smsFromTime,smsToTime from SMPPSETTING where isnull(FORCAMPAIGN,0)=1";
            return database.GetDataTable(sql);
        }

        public DataTable GetSMPPAccountIdTimeZone(string userid)
        {
            string sql = "select smsFromTime,smsToTime From SMPPSETTING s INNER JOIN smppaccountuserid a ON s.smppaccountid=a.smppaccountid where a.userid='" + userid + "' AND s.active=1";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count == 0)
                return database.GetDataTable("Select Top 1 smsFromTime,smsToTime from SMPPSETTING where isnull(FORFILE,0)=1");
            else
                return dt;
        }

        public string ValidateDLT(string username)
        {
            return Convert.ToString(database.GetScalarValue("SELECT DLTNO FROM CUSTOMER where username='" + username + "'"));
        }
        //abhishek -07-11-2022
        public string ValidateEmpCode(string username)
        {
            return Convert.ToString(database.GetScalarValue("SELECT empcode FROM CUSTOMER where username='" + username + "'"));
        }

        public DataTable GetDayWiseReport(string f, string t)
        {
            string sql = @"select FORMAT(CAST(SMSDATE as date), 'dd-MM-yyyy') SMSDATE, SUM(SUBMITTED) Submitted,SUM(DELIVERED) DELIVERED,SUM(FAILED) FAILED,sum(UNKNOWN) UNKNOWN from DAYSUMMARY where SMSDATE between '" + f + @"' and '" + t + "' group by CAST(SMSDATE as date) order by 1 desc";

            return database.GetDataTable(sql);

        }

        public void LogError(string title, string msg)
        {
            try
            {

                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH1"].ToString() + @"Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteSenderIdRequest(string query)
        {
            if (!string.IsNullOrEmpty(query))
                database.ExecuteNonQuery(query);
        }

        public DataTable GetPendingSenderIdList(string username)
        {
            string sql = "";
            sql = @"select row_number() over (Order by s.createdat DESC) as Sln, C.compname,C.fullname,C.mobile1 as mobile,C.email,s.senderid as sender,C.username as userid,s.filepath,s.countrycode " +
            " from customer C inner join   s on c.username=s.username where (isnull(s.rejected,0)=0 and isnull(s.allotedsenderid,'')='') AND s.username='" + username + "' ORDER BY s.createdat DESC ";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetApprovedSenderIdList(string username)
        {
            string sql = "select userid,senderid as sender,countrycode from senderidmast where userid='" + username + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetRejectedSenderIdList(string username)
        {
            string sql = "select username as userid,senderid as sender,countrycode from senderidrequeset where username='" + username + "' and isnull(rejected,0)=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DateTime Setdate(string Cdate)
        {
            char[] a = { ',' };
            string[] date = Cdate.Split('-');
            DateTime dt = new DateTime(int.Parse(date[0].ToString()), int.Parse(date[1].ToString()), int.Parse(date[2].ToString()));
            return dt;
        }

        public DataSet SP_GetCampaignWiseDatap(string user, DateTime s1, DateTime s2, string camp)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Getcampaigrpt";

                cmd.Parameters.AddWithValue("usr", user);
                cmd.Parameters.AddWithValue("Fdate", s1);
                cmd.Parameters.AddWithValue("tdate", s2);
                cmd.Parameters.AddWithValue("camp", camp);

                da.Fill(ds);
            }
            return ds;
        }

        public void UpdateRateAspercountry(string sql)
        {
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetWATemplateText(string usernm, string templatenm)
        {
            DataTable dt = new DataTable("dt");
            string user = "tmp1_" + usernm;
            string sql = @"Select Template,templateid,TempName from templaterequest where username = '" + usernm + "' and IsWhatsapp=1 and templateid = '" + templatenm + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public void sendWhatsapp(string msgtext, List<string> MobList, string profileid, string Sender, string TemplateId, string TemplateName, string campname = "")
        {
            try
            {
                string Insertsql = @" declare @id int=0
                select @id = isnull(max(fileid),0)+1 from [WARCSfile] ;
                insert into warcsfile(fileid,CommType,userid,campaignname,templatename)
                select @id,'WA','" + profileid + "','" + campname + "','" + TemplateName + "' ";


                Insertsql = Insertsql + @" INSERT INTO [dbo].[WAMSGTRAN] (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid,TemplateName)";
                string seletsql = "";
                for (int p = 0; p < MobList.Count; p++)
                {
                    if (p > 0)
                    {
                        seletsql = seletsql + @" UNION ALL  Select 'VCON' as PROVIDER, 0, '" + profileid + @"' as PROFILEID, '" +
                    msgtext + "' as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL('" + MobList[p].Trim() + @"',0),20,0)))) as TOMOBILE
                , '" + Sender + @"' as SENDERID, GETDATE() as CREATEDAT,@id,'NA' as peid,'Default' AS DATACODE,12 as smsrate,'" + TemplateId + "' AS templateid ,'" + TemplateName + "' ";
                    }
                    else
                    {
                        seletsql = seletsql + @" Select 'VCON' as PROVIDER, 0, '" + profileid + @"' as PROFILEID, '" +
                    msgtext + "' as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL('" + MobList[p].Trim() + @"',0),20,0)))) as TOMOBILE
                , '" + Sender + @"' as SENDERID, GETDATE() as CREATEDAT,@id,'NA' as peid,'Default' AS DATACODE,12 as smsrate,'" + TemplateId + "' AS templateid ,'" + TemplateName + "' ";
                    }

                }

                string sql = Insertsql + seletsql;
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void sendRCS(string msgtext, List<string> MobList, string profileid, string Sender, string TemplateId, string TemplateName, string campname = "")
        {
            try
            {
                string Insertsql = @" declare @id int=0
                select @id = isnull(max(fileid),0)+1 from [WARCSfile] ;
                insert into warcsfile(fileid,CommType,userid,campaignname,templatename)
                select @id,'RCS','" + profileid + "','" + campname + "','" + TemplateName + "' ";

                Insertsql = Insertsql + @" INSERT INTO [dbo].[RCSMSGTRAN] (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid,TemplateName,MSGID2)";
                string seletsql = "";
                for (int p = 0; p < MobList.Count; p++)
                {
                    if (p > 0)
                    {
                        seletsql = seletsql + @" UNION ALL  Select 'VCON' as PROVIDER, 0, '" + profileid + @"' as PROFILEID, '" +
                    msgtext + "' as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL('" + MobList[p].Trim() + @"',0),20,0)))) as TOMOBILE
                , '" + Sender + @"' as SENDERID, GETDATE() as CREATEDAT,@id,'NA' as peid,'Default' AS DATACODE,12 as smsrate,'" + TemplateId + "' AS templateid ,'" + TemplateName + "',newid() ";
                    }
                    else
                    {
                        seletsql = seletsql + @" Select 'VCON' as PROVIDER, 0, '" + profileid + @"' as PROFILEID, '" +
                    msgtext + "' as MSGTEXT, CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL('" + MobList[p].Trim() + @"',0),20,0)))) as TOMOBILE
                , '" + Sender + @"' as SENDERID, GETDATE() as CREATEDAT,@id,'NA' as peid,'Default' AS DATACODE,12 as smsrate,'" + TemplateId + "' AS templateid ,'" + TemplateName + "',newid() ";
                    }

                }

                string sql = Insertsql + seletsql;
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public bool IsWABARCS(string username)
        {
            return Convert.ToBoolean(database.GetScalarValue("SELECT isnull(wabarcs,0) wabarcs FROM CUSTOMER WHERE username='" + username + "'"));
        }

        public void WABARCSBalUpdate(string userid, int cnt)
        {
            string sql = "UPDATE CUSTOMER SET WABARCSbal = WABARCSbal - " + Convert.ToString(cnt) + " WHERE username = '" + userid + "'";
            database.ExecuteNonQuery(sql);
        }

        public bool isBalanceAvailable_WABARCS(string userid, int cnt)
        {
            string sql = "select WABARCSbal from customer with (nolock) WHERE username = '" + userid + "'";
            int b = Convert.ToInt16(database.GetScalarValue(sql));
            return (b >= cnt);
        }

        public DataTable Get_WA_Report(string f, string t, string usertype, string user, string campnm = "")
        {
            string sql = "";
            sql = @" select convert(varchar(30),s.inserttime,103) as submitdate,convert(date,s.inserttime) inserttime,
count(s.messageId) submitted,
sum(case when isnull(d.status_groupName, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.status_groupName, '') <> 'Delivered' AND d.status_groupName IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.status_groupName IS NULL then 1 else 0 end) as unknown, profileid as userid
from customer c with(nolock)
INNER JOIN WAsubmitted s with(nolock) on c.username = s.profileid
INNER JOIN WARCSfile f with(nolock) on f.fileid = s.fileid and CommType='WA'
LEFT JOIN  [10.10.31.35].[SMPPMAIN_TX].[dbo].[DeliveryWABA] d with(nolock) on s.messageId = d.msgid and convert(varchar,s.inserttime,102)=convert(varchar,d.inserttime,102)
where c.username = '" + user + @"' and s.inserttime between '" + f + @"' and '" + t + @"' ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            sql = sql + @" group by profileid,convert(varchar(30),s.inserttime,103),convert(date,s.inserttime)
order by convert(date,s.inserttime)";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable Get_RCS_Report(string f, string t, string usertype, string user, string campnm = "")
        {
            string sql = "";
            sql = @" select convert(varchar(30),s.inserttime,103) as submitdate,convert(date,s.inserttime) inserttime,count(s.messageId) submitted,
sum(case when isnull(d.status_groupName, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.status_groupName, '') <> 'Delivered' AND d.status_groupName IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.status_groupName IS NULL then 1 else 0 end) as unknown
, profileid as userid from customer c with(nolock)
INNER JOIN rcssubmitted s with(nolock) on c.username = s.profileid
INNER JOIN WARCSfile f with(nolock) on f.fileid = s.fileid and CommType='RCS'
LEFT JOIN  [10.10.31.35].[SMPPMAIN_TX].[dbo].[DeliveryRCS] d with(nolock) on s.messageId = d.msgid and convert(varchar,s.inserttime,102)=convert(varchar,d.inserttime,102)
where c.username = '" + user + @"' and s.inserttime between '" + f + @"' and '" + t + @"' ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            sql = sql + @" group by profileid,convert(varchar(30),s.inserttime,103),convert(date,s.inserttime)
order by convert(date,s.inserttime)";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetWADetail(string date, string userid)
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            sql = @" select m.MessageId as MessageId, " + str + @" as MobileNo,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.inserttime,106) + ' ' + convert(varchar,m.inserttime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.inserttime,106) + ' ' + convert(varchar,d.inserttime,108)) as DeliveredDate,watext as Message,
                CASE WHEN d.status_GroupName is null then 'UNKNOWN' ELSE CASE WHEN d.status_GroupName='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.status_description as RESPONSE FROM wasubmitted m with (nolock)
                left join [10.10.31.35].[SMPPMAIN_TX].[dbo].[DeliveryWABA] d with (nolock) on m.MessageId=d.msgid and convert(varchar,m.inserttime,102)=convert(varchar,d.inserttime,102) 
                inner join WARCSfile u with (nolock) on u.fileid = m.fileid AND u.USERID=m.PROFILEID
                where m.PROFILEID='" + userid + "'  and convert(date, m.inserttime)=convert(date, '" + date + "') order by m.inserttime ";


            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetRCSDetail(string date, string userid)
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            sql = @" select m.MessageId as MessageId, " + str + @" as MobileNo,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.inserttime,106) + ' ' + convert(varchar,m.inserttime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.inserttime,106) + ' ' + convert(varchar,d.inserttime,108)) as DeliveredDate,msgtext as Message,
                CASE WHEN d.status_GroupName is null then 'UNKNOWN' ELSE CASE WHEN d.status_GroupName='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.status_description as RESPONSE FROM rcssubmitted m with (nolock)
                left join [10.10.31.35].[SMPPMAIN_TX].[dbo].[DeliveryRCS] d with (nolock) on m.MessageId=d.msgid and convert(varchar,m.inserttime,102)=convert(varchar,d.inserttime,102) 
                inner join WARCSfile u with (nolock) on u.fileid = m.fileid AND u.USERID=m.PROFILEID
                where m.PROFILEID='" + userid + "'  and convert(date, m.inserttime)=convert(date, '" + date + "') order by m.inserttime ";


            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSettingDetails()
        {
            return database.GetDataTable("select * from settings");
        }

        public DataTable GetDayWiseReportAdminWise(string f, string t, string UserName, string usertype)
        {
            string sql = "";
            //Abhishek 07-11-2022-usertype=BD
            if (usertype == "BD")
            {
                sql = @" declare @empcode varchar(550)
set @empcode=(select empcode from customer where username='" + UserName + @"');
select FORMAT(CAST(SMSDATE as date), 'dd-MM-yyyy') SMSDATE, SUM(SUBMITTED) Submitted,SUM(DELIVERED) DELIVERED,SUM(FAILED) FAILED,sum(UNKNOWN) UNKNOWN from DAYSUMMARY
where userid in (select username from customer where empcode=@empcode  )
and SMSDATE between '" + f + @"' and '" + t + "' group by CAST(SMSDATE as date) order by 1 desc ";
            }
            else
            {
                sql = @" declare @dltno varchar(550)
                set @dltno=(select dltno from customer where username='" + UserName + @"');
                SELECT FORMAT(CAST(SMSDATE as date), 'dd-MM-yyyy') SMSDATE, SUM(SUBMITTED) Submitted,SUM(DELIVERED) DELIVERED,SUM(FAILED) FAILED,sum(UNKNOWN) UNKNOWN from DAYSUMMARY
                where userid in (select username from customer where dltno=@dltno and usertype='user')
                and SMSDATE>='" + f + @"' and SMSDATE<='" + t + "' group by CAST(SMSDATE as date) order by 1 desc ";
            }
            return database.GetDataTable(sql);
        }

        public void sendotpforchagepwd(string mobile, string user, string cc, string domain, string seg)
        {

            DataTable dt = database.GetDataTable("select * from settings");

            string msg = dt.Rows[0]["PWDChangeSMS"].ToString();// Convert.ToString(database.GetScalarValue("select PWDChangeSMS from settings"));
            msg = msg.Replace("#LINK", domain + seg);

            // "Select TOP 1 * from customer where usertype='SYSADMIN'"



            //string s = (cc == "91" ? dt.Rows[0]["senderid"].ToString() : senderid);

            string s = Convert.ToString(database.GetScalarValue("select SENDERID from customer where username='" + user + "'"));


            string smppacountid = (cc == "91" ? dt.Rows[0]["SMPPACCOUNTID"].ToString() : Convert.ToString(getDefaultSMPPAccountId(cc)) + "01");

            string USER = user; // "20200125";
            string templateid = (cc == "91" ? dt.Rows[0]["templateid"].ToString() : "");
            string peid = (cc == "91" ? dt.Rows[0]["peid"].ToString() : ""); //getPEid(USER);
            string sql = @"INSERT INTO [dbo].[MSGTRAN]
           ([PROVIDER]
           ,[SMPPACCOUNTID]
           ,[PROFILEID]
           ,[MSGTEXT]
           ,[TOMOBILE]
           ,[templateid]
           ,[SENDERID]
           ,[CREATEDAT]           
           ,[PICKED_DATETIME],[PEID],[DATACODE])
            VALUES
           (
           ''
           , '" + smppacountid + @"'
           , '" + USER + @"'
           , '" + msg + @"'
           , '" + mobile + @"'
           , '" + templateid + @"'
           , '" + s + @"'
           , GETDATE()
           , NULL,'" + peid + "','Default')";
            dbmain.ExecuteNonQuery(sql);
            double rat = getUserCountryRate(user, cc);
            UpdateAndGetBalance(user, "", 1, rat);



            string sql1 = "declare @sender varchar(100) declare @uid varchar(20) declare @peid varchar(50) select top 1 @uid=username, @sender=senderid, @peid=peid from customer where usertype='SYSADMIN' " +
                "Insert into VerifyLink (userid,mobileno,segment,sendtime,validhitrecdtime,domain) " +
                "values ('" + user + "','" + mobile + "','" + seg + "',getdate(),dateadd(MINUTE,5,getdate()),'" + domain + "' ) ; ";

            //" Insert into MSGTRAN (PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, peid,datacode) " +
            //"values ('BSNL','301',@uid,'" + msg + "','" + mobile + "',@sender,getdate(),@peid,'Default')";
            database.ExecuteNonQuery(sql1);
        }

        public DataTable Inserttempdata(string userid, string rcstype, string TemplateName = "", string TemplateText = "", string FileUrl = "", string FilePath = "", string CardWidth = "", string CardOrientation = "", string CardAlignment = "", string CardTitle = "", string CardDesc = "", string CardHeight = "", string SuggestionText = "", string SuggestionType = "", string SuggestionUrl = "", string SuggestionPhone = "", string SuggestionLatitude = "", string SuggestionLongitude = "")
        {


            string user1 = "Tmp_Template_" + userid;
            string user2 = "Tmp_Templateheader_" + userid;
            string sql = "";

            string sql1 = "";
            string sql2 = "";
            sql1 = @"IF EXISTS( SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo." + user1 + "')) BEGIN select * from " + user1 + " END;";

            //sql1 = @"select * from " + user1 + "";
            sql2 = @"select * from sys.tables where name='" + user2 + "'";

            DataTable dtc = new DataTable();

            dtc = database.GetDataTable(sql1);
            if (dtc.Rows.Count >= 10)
            {
                return dtc;
            }

            sql = @"if exists (select * from sys.tables where name='" + user1 + @"')  " +
                    " begin " +
                   " Insert into " + user1 + @"(RCSType, TemplateText, FileUrl, FilePath, CardWidth, CardTitle, CardDesc, CardHeight, SuggestionText, SuggestionType, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) Values('" + rcstype + "', '" + TemplateText + "', '" + FileUrl + "', '" + FilePath + "', '" + CardWidth + "', '" + CardTitle + "', '" + CardDesc + "', '" + CardHeight + "', '" + SuggestionText + "', '" + SuggestionType + "', '" + SuggestionUrl + "', '" + SuggestionPhone + "', '" + SuggestionLatitude + "', '" + SuggestionLongitude + "', '" + userid + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "')" +
                   " end else begin Create table " + user1 + @" (RCSType VARCHAR(1),TemplateName VARCHAR(MAX), ID INT IDENTITY(10001,1),TemplateText VARCHAR(MAX),FileUrl VARCHAR(MAX),FilePath VARCHAR(MAX),CardWidth VARCHAR(1),CardOrientation VARCHAR(1),CardAlignment VARCHAR(1),CardTitle NVARCHAR(200),CardDesc NVARCHAR(2000),CardHeight VARCHAR(1),SuggestionText VARCHAR(25),SuggestionType VARCHAR(1),SuggestionUrl VARCHAR(MAX),SuggestionPhone VARCHAR(15),SuggestionLatitude VARCHAR(100),SuggestionLongitude VARCHAR(100),USERID VARCHAR(100),CreatedDate DateTime)" +
                   "Insert into " + user1 + @"(RCSType, TemplateText, FileUrl, FilePath, CardWidth, CardTitle, CardDesc, CardHeight, SuggestionText, SuggestionType, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) Values('" + rcstype + "', '" + TemplateText + "', '" + FileUrl + "', '" + FilePath + "', '" + CardWidth + "', '" + CardTitle + "', '" + CardDesc + "', '" + CardHeight + "', '" + SuggestionText + "', '" + SuggestionType + "', '" + SuggestionUrl + "', '" + SuggestionPhone + "', '" + SuggestionLatitude + "', '" + SuggestionLongitude + "', '" + userid + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "') end" +
                   " Select * from " + user1 + " ";


            sql2 = @"if exists (select * from sys.tables where name='" + user2 + @"')  " +
                    " begin " +
                   " update " + user2 + @" set TemplateName = '" + TemplateName + "' where USERID = '" + userid + "' ;" +
                   " end else begin Create table " + user2 + @" (RCSType VARCHAR(1),TemplateID int IDENTITY(10001,1),TemplateName VARCHAR(MAX), Active INT,Userid VARCHAR(100),CreatedDate DateTime)" +
                   "Insert into " + user2 + @"(RCSType,TemplateName, Active, Userid, CreatedDate) Values('" + rcstype + "', '" + TemplateName + "', 1, '" + userid + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "') end " +
                   " Select * from " + user2 + "";


            DataTable dt = new DataTable("dt");
            DataTable dt1 = new DataTable();
            dt = database.GetDataTable(sql);
            dt1 = database.GetDataTable(sql2);
            return dt;


        }

        // RCS PAGE CODE 21/03/22
        //public string SaveTempTable(string FilePath, string SheetName, string user, string extension, string folder, string filenm, string s = "", string moblen = "", string fileUname = "")
        //{
        //    // bool chkUnq = isAllowDuplicate == true ? false : true;
        //    bool chkUnq = user.ToUpper() == "MIM2101371" ? false : false;
        //    string username = user;
        //    if (!File.Exists(FilePath))
        //    {
        //        return "There was some error on file upload. Please upload again.";
        //    }
        //    Log("FU-user-" + user + ". File-" + FilePath);
        //    string user1 = fileUname;
        //    user = fileUname;

        //    string colnm = "";
        //    string datatype = "";
        //    string sql = "";

        //    string ccode = Convert.ToString(database.GetScalarValue("Select defaultCountry from customer where username='" + username + "'"));

        //    // string moblen = (ccode == "91" ? "10" : (ccode == "971" ? "9" : "9"));

        //    if (extension.ToLower().Contains("xls"))
        //    {
        //        sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @" ;
        //        SELECT " + (chkUnq ? "DISTINCT" : "") + " * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
        //        try
        //        {
        //            database.ExecuteNonQuery(sql);
        //        }
        //        catch (Exception ex1)
        //        {
        //            return "Invalid file format.";
        //        }
        //        colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
        //        datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
        //        if (datatype.ToLower() != "float")
        //        {
        //            Int64 cn1 = Convert.ToInt64(database.GetScalarValue("Select count(*) from " + user1 + @" where [" + colnm + "] like '%.%e+%' "));
        //            if (cn1 > 0) return "Error in file. Please check the file. ";
        //        }

        //        if (datatype.ToLower() != "float")
        //        {
        //            //sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ; SELECT distinct right([" + colnm + "],10) AS [" + colnm + "] INTO " + user + @" FROM  " + user1 + " ; ";
        //            sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
        //            SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
        //        }
        //        else
        //        {
        //            sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
        //            SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(isnull([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
        //        }

        //        try
        //        {
        //            database.ExecuteNonQuery(sql);
        //        }
        //        catch (Exception ex2)
        //        {
        //            return "Mobile Numbers in the file are not Numeric. Please check the file. ";
        //        }

        //    }
        //    else if (extension.ToLower().Contains("txt"))
        //    {
        //        #region< Commented on 26-02-2021 >
        //        //sql = @"if exists (select * from sys.tables where name = '" + user1 + @"') drop table " + user1 + @"; 
        //        //select distinct * into " + user1 + " FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=No','SELECT * FROM " + filenm + "') ";
        //        //try
        //        //{
        //        //    database.ExecuteNonQuery(sql);
        //        //}
        //        //catch (Exception ex1)
        //        //{
        //        //    return "Invalid file format.";
        //        //}
        //        //colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
        //        //datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));

        //        //if (datatype.ToLower() != "float")
        //        //{
        //        //    Int64 cn1 = Convert.ToInt64(database.GetScalarValue("Select count(*) from " + user1 + @" where [" + colnm + "] like '%.%e+%' or  [" + colnm + "] like '%.%E+%'"));
        //        //    if (cn1 > 0) return "Error in file. Please check the file. ";
        //        //}

        //        //if (datatype.ToLower() != "float")
        //        //{
        //        //    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
        //        //    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
        //        //}
        //        //else
        //        //{
        //        //    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
        //        //    SELECT distinct CONVERT(nvarchar(255),LTRIM(RTRIM(str(ISNULL([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Text;Database=" + folder + ";HDR=NO','SELECT * FROM " + filenm + "') ";
        //        //}
        //        //try
        //        //{
        //        //    database.ExecuteNonQuery(sql);
        //        //}
        //        //catch (Exception ex2)
        //        //{
        //        //    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
        //        //}
        //        #endregion
        //        try
        //        {
        //            DataTable dt = ReadTextFile(FilePath, moblen);

        //            if (dt.Rows.Count == 0)
        //                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
        //            else
        //            {
        //                sql = string.Format("if exists (select * from sys.tables where name = '{0}') drop table {1}", user, user);
        //                sql += string.Format(" Create table {0} (MobNo varchar(15) )", user);

        //                database.ExecuteNonQuery(sql);
        //                database.BulkInsertData(dt, user); colnm = "MobNo";

        //                //if (isAllowDuplicate == true)
        //                //{
        //                //    database.BulkInsertData(dt, user); colnm = "MobNo";
        //                //}
        //                //else
        //                //{
        //                //    database.BulkInsertData(dt.DefaultView.ToTable(true, "MobNo"), user); colnm = "MobNo";
        //                //}
        //            }

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    //rabi 29/07/21
        //    else if (extension.ToLower().Contains("csv"))
        //    {
        //        try
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            StringBuilder sb1 = new StringBuilder();

        //            DataTable dt = ReadCSV(FilePath, moblen);
        //            if (dt.Rows.Count == 0)
        //                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
        //            else
        //            {
        //                sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
        //                string sql1 = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + "";

        //                sb.Append(@"Create table " + user + "  (");
        //                sb1.Append(@"Create table " + user1 + "  (");
        //                for (int i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    if (i != dt.Columns.Count - 1)
        //                    {
        //                        sb.Append("[" + dt.Columns[i] + "] nvarchar (max),");
        //                        sb1.Append("[" + dt.Columns[i] + "] nvarchar (max),");
        //                    }
        //                    else
        //                    {
        //                        sb.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
        //                        sb1.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
        //                    }
        //                }
        //                sb.Append(")");
        //                sb1.Append(")");
        //                string str = sql + "  " + sb.ToString();
        //                database.ExecuteNonQuery(sql + "  " + sb.ToString());
        //                //database.BulkInsertDataDynamic(dt, user1);
        //                database.BulkInsertDataDynamic(dt, user);
        //                colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 "));

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }


        //    database.ExecuteNonQuery("delete from " + user + @"   where len([" + colnm + "]) < " + moblen);
        //    database.ExecuteNonQuery("update " + user + @" set [" + colnm + "] = right([" + colnm + "], " + moblen + ") where len([" + colnm + "]) > " + moblen);

        //    database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.[" + colnm + "] ");
        //    /* rabi 15 july 21*/
        //    /* Kuldeep 08 Mar 22*/
        //    //database.ExecuteNonQuery("if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + username + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + username + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

        //    //check column name is numeric or not
        //    sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
        //    Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
        //    if (x == 1) return "Column name is numeric. Cannot upload file.";

        //    try
        //    {
        //        sql = "select convert(numeric,[" + colnm + "]) from " + user;
        //        DataTable dt = database.GetDataTable(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
        //    }

        //    // CHECK FOR ALL NULL VALUES
        //    sql = "select count(*) from " + user + " where [" + colnm + "] is not null ";
        //    Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
        //    if (Y <= 0) return "No Mobile Numbers found in the file";
        //    return "RECORDCOUNT " + Y.ToString();
        //}

        //public double CalculateRCSAmount(string UserID, long cnt, double rate, double smscnt)
        //{
        //    string b = Convert.ToString(database.GetScalarValue("Select rcsbalance from customer with (nolock) where username='" + UserID + "'"));
        //    double bal = Convert.ToDouble(b);
        //    bal = bal - Convert.ToDouble((cnt * (rate)));
        //    bal = Math.Round((bal), 3);
        //    return bal;
        //}
        //public void InsertSMSrecordsFromUSERTMP(string userId, int length, int trcs, string s, string SMSType, string msg, string filenm, string filenmext, DataTable dtAc, string campnm, bool ucs2, int noofsms, double Drate, List<string> mobList, string manual, string retarget, string TemplateID, string country_code, double PrevBalance = 0, double AvailableBalance = 0, string tmpfilenm = "", string imagefilepath = "", string videopath = "", string templateId = "", string fileurl = "")
        //{
        //    //change SMPP ACCOUNT based on SMSTYPE
        //    string user = tmpfilenm;
        //    string sql;

        //    string colnm = "";

        //    if (manual == "MANUAL")
        //    {
        //        database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobNo numeric) ;  ");
        //        foreach (var m in mobList)
        //        {
        //            database.ExecuteNonQuery(" Insert into " + user + @" values ('" + m + "')");
        //        }
        //        //database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");
        //        /*rabi 14 jul 21*/
        //        //database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + userId + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + userId + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.MobileNo  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

        //    }

        //    colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));
        //    if (country_code != "" && manual != "MANUAL")
        //        // database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') update " + user + @" set [" + colnm + "] =convert(varchar,convert(bigint,[" + colnm + "])) ");

        //        checkNumberDigitsAndUpdate(user, colnm);

        //    string peid = getPEid(userId);

        //    sql = GetSqlAccounts(dtAc);
        //    DataTable dt = database.GetDataTable(sql);

        //    Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select count(*) from " + user + @" else select 0 "));

        //    dt.Columns.Add("cnt", typeof(string));

        //    int totalPDU = 0;
        //    for (int i = 0; i < dt.Rows.Count; i++) totalPDU += Convert.ToInt16(dt.Rows[i]["PDUSIZE"]);
        //    Int32 totcnt = 0;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        double per = (Convert.ToDouble(dt.Rows[i]["PDUSIZE"]) * 100) / Convert.ToDouble(totalPDU);
        //        Int32 cntrow = Convert.ToInt32(rowcnt * (per / 100));
        //        totcnt += cntrow;
        //        dt.Rows[i]["cnt"] = cntrow.ToString();
        //    }
        //    int dif = 0;
        //    if (totcnt < rowcnt)
        //    {
        //        dif = rowcnt - totcnt;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //            dt.Rows[i]["cnt"] = Convert.ToString(Convert.ToInt32(dt.Rows[i]["cnt"]) + 1);
        //    }
        //    //Int32 cnt = (rowcnt / dt.Rows.Count);
        //    //if (rowcnt % dt.Rows.Count > 0) cnt++;
        //    //database.ExecuteNonQuery("IF not EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SMPPACCOUNTID' AND Object_ID = Object_ID(N'" + user + "')) " +
        //    //   " alter table " + user + @" add smppaccountid numeric(10)"); //add new column

        //    //  database.ExecuteNonQuery("alter table " + user + @" add smppaccountid numeric(10) ");

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") " + user + @" set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid is null ";
        //        database.ExecuteNonQuery(sql);
        //    }
        //    msg = msg.Replace("'", "''"); filenm = filenm.Replace("'", "''");

        //    double CampaignCost = PrevBalance - AvailableBalance;



        //    sql = @"if exists (select * from sys.tables where name='" + user + @"')  " +
        //        " begin " +
        //        //" Insert into SMSFILEUPLOAD (USERID,FILENM,EXTENSION,RECCOUNT,senderid,campaignname,smsrate,PrevBalance,CampaignCost,AvailableBalance,tmpFN,COUNTRYCODE) values" +
        //        //" ('" + user + "','" + filenm + "','" + filenmext + "','" + rowcnt.ToString() + "','" + s + "','" + campnm + "','" + Drate + "','" + PrevBalance + "','" + CampaignCost + "','" + AvailableBalance + "','" + tmpfilenm + "','" + country_code + "') " +
        //        " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + user + "' ; ";

        //    string rate = Drate.ToString();

        //    //call sms test function test - get true / false
        //    if (Global.Istemplatetest == false)
        //    {
        //        sql = sql + " select t.* into #t_011 from " + user + " t " +
        //           " delete d from " + user + " d inner join #t_011 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //           " alter table #t_011 add msgid varchar (100) ;  ";

        //        for (int x = 0; x < noofsms; x++)
        //        {
        //            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //            sql = sql + " update #t_011 set msgid=newid() " +
        //            " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //            " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //            " N'" + msg + "','" + rate + "' from #t_011 ; " +
        //            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //            @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(CONVERT(VARCHAR,getdate(),108),':','') + 
        //            ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Global.templateErrorCode + @" text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable','" + Global.templateErrorCode + @"',getdate()
        //            FROM #t_011 end ";
        //        }
        //    }
        //    else
        //    {

        //        double Bper = GetBlockSMSper(userId, "B");
        //        if (Bper != 0 && retarget == "")
        //        {
        //            Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
        //            sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
        //                " delete d from " + user + " d inner join #t101 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //                " alter table #t101 add msgid varchar (100) ; ";

        //            for (int x = 0; x < noofsms; x++)
        //            {
        //                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //                sql = sql + " update #t101 set msgid=newid() " +
        //                    @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
        //              select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "'," +
        //                  "GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t101 ; " +
        //                     " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                     " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //                     " N'" + msg + "','" + rate + "' from #t101 ; ";
        //            }
        //        }
        //        double Fper = GetBlockSMSper(userId, "F");
        //        if (Fper != 0 && retarget == "")
        //        {
        //            Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);
        //            sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + getWhiteListNo(userId) + ") ORDER BY NEWID() " +
        //                " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //                " alter table #t10 add msgid varchar (100) ; ";

        //            for (int x = 0; x < noofsms; x++)
        //            {
        //                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //                sql = sql + " update #t10 set msgid=newid() " +
        //                    @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
        //              select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
        //                     " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                     " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //                     " N'" + msg + "','" + rate + "' from #t10 ; ";
        //            }
        //        }
        //        if (blacklistuser(userId))
        //        {
        //            sql = sql + " select t.* into #t11 from " + user + " t left join BLACKLISTNO b on t.[" + colnm + "]=b.MOBILENO and b.userid='" + userId + "' where b.MOBILENO is not null " +
        //                " delete d from " + user + " d inner join #t11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //                " alter table #t11 add msgid varchar (100) ; ";
        //            for (int x = 0; x < noofsms; x++)
        //            {
        //                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //                sql = sql + " update #t11 set msgid=newid() " +
        //                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //                " N'" + msg + "','" + rate + "' from #t11 ; " +
        //                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //                @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
        //            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
        //            FROM #t11 ; ";
        //            }
        //        }

        //        bool isMobProcess = MobProcessUser(userId);
        //        if (isMobProcess)
        //        {
        //            //PROCESS FAIL
        //            sql = sql + " select t.*,s.err_code into #mt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='F' " +
        //                " delete d from " + user + " d inner join #mt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //                " alter table #mt11 add msgid varchar (100) ;  ";

        //            for (int x = 0; x < noofsms; x++)
        //            {
        //                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //                sql = sql + " update #mt11 set msgid=newid() " +
        //                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //                " N'" + msg + "','" + rate + "' from #mt11 ; " +
        //                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //                @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
        //            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + convert(varchar,err_code COLLATE SQL_Latin1_General_CP1_CI_AS) + ' text:' AS DLVRTEXT, MSGID, GETDATE(), 'Undeliverable',err_code,getdate()
        //            FROM #mt11 ; ";
        //            }

        //            //PROCESS DELIVERY
        //            sql = sql + " select t.* into #Dmt11 from " + user + " t inner join BLOCKSMSERROR s with (nolock) on s.profileid='" + userId + "' inner join UserMobile b with (nolock) on t.[" + colnm + "]=b.TOMOBILE and b.Profileid=s.profileid and s.err_code=b.err_code where s.DORF='D' " +
        //                " delete d from " + user + " d inner join #Dmt11 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
        //                " alter table #Dmt11 add msgid varchar (100) ;  ";

        //            for (int x = 0; x < noofsms; x++)
        //            {
        //                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
        //                sql = sql + " update #Dmt11 set msgid=newid() " +
        //                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
        //                " N'" + msg + "','" + rate + "' from #Dmt11 ; " +
        //                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //                @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
        //            ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DELIVRD err:000 text:' AS DLVRTEXT, MSGID, GETDATE(), 'Delivered','000',getdate()
        //            FROM #Dmt11 ; ";
        //            }
        //        }
        //        string dataCode = "";
        //        if (SMSType == "8")
        //        {
        //            if (ucs2)
        //                dataCode = "UnicodeFlashSMS";
        //            else
        //                dataCode = "DefaultFlashSMS";
        //        }
        //        else
        //        {
        //            if (ucs2)
        //                dataCode = "UCS2";
        //            else
        //                dataCode = "Default";
        //        }

        //        sql = sql + "Insert into tblRCSMSGRCVD (CountryCode,MsgType,MobNo,MsgText,ImageFilePath,VedioFilePath,UploadFileName,DataTableName,NoOfRecds,UserId,CreatedDate,Noofrcs,Rate,Campaign,NoofChar,TNoOfRcs,TemplateID,imagefileurl)" +
        //       " values ('" + country_code + "','" + SMSType + "','" + rowcnt.ToString() + "','" + msg + "','" + imagefilepath + "','" + videopath + " ','" + filenm + "','" + tmpfilenm + "','" + rowcnt.ToString() + "','" + userId + "',GETDATE(),'" + noofsms + "','" + Drate + "','" + campnm + "','" + length + "','" + trcs + "','" + templateId + "','" + fileurl + "') " +
        //       "select @id = max(id) from tblRCSMSGRCVD where userid='" + user + "' update customer set rcsbalance='" + AvailableBalance + "' where username='" + userId + "' ";

        //        //sql = sql + " INSERT INTO MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,Templateid)  " +
        //        //    " Select 'VCON' as PROVIDER, smppaccountid, '" + userId + @"' as PROFILEID, N'" + msg + @"' as MSGTEXT, [" + colnm + @"] as TOMOBILE
        //        //, '" + s + @"' as SENDERID, GETDATE() as CREATEDAT,@id as fileid,'" + peid + "' as peid,'" + dataCode + "' AS DATACODE,'" + rate.ToString() + "' as smsrate,'" + TemplateID + "' as templateid from " + user + " where [" + colnm + "] is not null ; " +
        //        //    " end ";

        //        //  double bal = CalculateAmount(userId, noof_message, Convert.ToDouble(rate), smscount);
        //        double bal = CalculateSMSCost(noof_message, Convert.ToDouble(rate));
        //        sql = sql + " update customer set balance = balance - '" + bal + "' where username = '" + userId + "' end";
        //    }

        //    try
        //    {
        //        database.ExecuteNonQuery(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError("Error in InsertSMSrecordsFromUSERTMP method !  ", ex.Message + " - " + ex.StackTrace);
        //        throw ex;
        //    }
        //}

        //public DataTable GetRCSRateAsPerCountry(string User, string ccode)
        //{
        //    DataTable dt = new DataTable("dt");
        //    string sql = "";
        //    sql = "select * from tblrcsratemaster where username='" + User + "' and countrycode='" + ccode + "'";
        //    dt = database.GetDataTable(sql);
        //    return dt;
        //}
        //public DataTable GetTemplateId(string userid, int id)
        //{
        //    string sql = "";
        //    // sql = @"select Concat(TemplateID ,' ' ,isnull(tempname,'')) [TemplateID],TemplateID as template,TemplateID as onlyTemplateID from templaterequest where username='" + userid + "' and isnull(allotted ,0)=1 and isnull(IsWhatsapp,0)=0 and isnull(TemplateID,'')<>'' order by templateid";
        //    //sql = @"Select * from RcsTemplateHeader where userid = '" + userid + "' and rcstype='" + id + "'";
        //    //sql = @"select c.phonecode as countrycode,c.name +' - ' +cast(c.phonecode as varchar) as name from countryMast c";
        //    sql = @"select cast(templateid as varchar) +' ( ' + templatename +')' as name,* from  RcsTemplateHeader where userid = '" + userid + "' and rcstype='" + id + "'";
        //    DataTable dt = database.GetDataTable(sql);
        //    return dt;
        //}

        public DataTable GetValueForURL(string user)
        {
            string sql = "select id,long_url,CONCAT(domainname,segment) as shortURL, FORMAT(added,'dd-MM-yyyy') createdDate, FORMAT(expiry,'dd-MM-yyyy') expiry from Short_urls where isnull(IsRichMedia,0)=1 AND userid ='" + user + "' and DATEDIFF(DD,GETDATE(), expiry) <= '5' AND DATEDIFF(DD,GETDATE(), expiry) >='0' ";
            return database.GetDataTable(sql);
        }

        public void RenewShortURL(string user, string id)
        {
            string sql = "update Short_urls set expiry = DATEADD(DD,7,expiry) Where userid ='" + user + "' AND Id ='" + id + "' ";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetURLSofUser_4SMSSEND(string User, string domain)
        {
            DataTable dt = new DataTable("dt");
            string sh = domain;
            string sql = "Select ISNULL(urlname,'') + '    ' + '" + sh + "' + segment as shorturlDISP, '" + sh + "' + segment as shorturl from short_urls where userid='" + User + "' AND RIGHT(SEGMENT,2)<>'_Q' AND MAINURL=0 AND Cast(isnull(Expiry,dateadd(yy,25,GETDATE())) as date) >=Cast(GETDATE() as date) order by added desc";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetMostActiveUserr(int noofrecord, string userType, string dltNO)
        {
            string sql = @"SELECT top " + noofrecord + @" COMPNAME,s.profileid as userid,count(s.id) submitted,
                        sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                        sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                        sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
                                        from MSGSUBMITTED s with(nolock) 
                                        inner join customer c with(nolock) ON c.username = s.profileid
                                        left join DELIVERY d with(nolock) on s.msgid = d.msgid 
                                        where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";
            if (userType.ToLower() != "sysadmin")
            {
                sql = sql + " AND dltno = '" + dltNO + "' ";
            }
            sql = sql + "group by COMPNAME,s.profileid order by submitted desc";
            return database.GetDataTable(sql);

        }

        public class RCSRoot
        {
            public List<RCSMessage> messages { get; set; }
        }

        public class RCSMessage
        {
            public string to { get; set; }
            public int messageCount { get; set; }
            public string messageId { get; set; }
            public Status status { get; set; }
        }

        public void Info_Err(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }

        public int RCSApiAN(string mob, string authkey, string msgtext, string userid, string acid, string apiurl)
        {


            //msgtext = HttpUtility.UrlEncode(msgtext);
            //msgtext = "text by ajay";
            //msgtext = Replace(Replace([msgtext], Chr(10), ""), Chr(13), ""));
            // msgtext.Replace(Convert.ToChar(13), @"\n");

            //var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var body = @"{
             ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n") + @""",
               ""type"": ""TEXT""
             }
           }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            RCSRoot res = new RCSRoot();

            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    return 1;
                }
                else
                {

                    Info_Err(response.Content, 0);
                    return 0;
                }


            }
            catch (Exception ex)
            {

                return 0;
            }

        }

        public void DeleteGroup(string Userid, string grpname)
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = "Sp_GroupDeleted";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Userid", Userid);
                cmd.Parameters.AddWithValue("grpname", grpname);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

        }

        public DataTable SP_GetCountry()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetCountry";
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetRoute()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetValueForRoute";
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetValueForGrid(string CountryCode, string SmppAccountId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetValueForSenderIdMappingGird";
                cmd.Parameters.AddWithValue("CountryCode", CountryCode);
                cmd.Parameters.AddWithValue("SmppAccountId", SmppAccountId);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_InsertValueForSenderMapping(string SmppAccountId, string Systemid, string SenderId, string CountryCode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_InsertValueForSenderMapping";
                cmd.Parameters.AddWithValue("SmppAccountId", SmppAccountId);
                cmd.Parameters.AddWithValue("systemid", Systemid);
                cmd.Parameters.AddWithValue("SenderId", SenderId);
                cmd.Parameters.AddWithValue("CountryCode", CountryCode);
                //cmd.ExecuteNonQuery();
                da.Fill(dt);
                // msg = cmd.Parameters["Result"].Value.ToString();

            }
            return dt;
        }

        public DataTable GetDELIVERYREPORTTODAYSYSADMIN(string UserType, string UserName, string EmpCode, string Fromdate, string Todate, string ReportType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DELIVERYREPORTTODAYSYSADMIN";
                cmd.Parameters.AddWithValue("@UserType", UserType);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@Todate", Todate);
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetDELIVERYREPORTTODAYSYSADMINFILTER(string FromDate, string ToDate, string SenderId, string CampaignWise, string TemplateId, string UserId, string MasterType, string ReportType, string UserType, string EmpCode, string mobno, string DLTNO)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_GetDELIVERYREPORTTODAYSYSADMINFILTER";
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@Todate", ToDate);
                    cmd.Parameters.AddWithValue("@SenderId", SenderId);
                    cmd.Parameters.AddWithValue("@CampaignWise", CampaignWise);
                    cmd.Parameters.AddWithValue("@TemplateId", TemplateId);
                    cmd.Parameters.AddWithValue("@USERID", UserId);
                    cmd.Parameters.AddWithValue("@MasterType", MasterType);
                    cmd.Parameters.AddWithValue("@ReportType", ReportType);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
                    cmd.Parameters.AddWithValue("@mobno", mobno);
                    cmd.Parameters.AddWithValue("@DLTNO", DLTNO);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetDELIVERYREPORTTODAYSYSADMINSingle(string FromDate, string ToDate, string SenderId, string CampaignWise, string TemplateId, string UserId, string MasterType, string ReportType, string UserType, string EmpCode, string mobno, string DLTNO)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetDELIVERYREPORTTODAYSYSADMINSingle";
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@Todate", ToDate);
                cmd.Parameters.AddWithValue("@SenderId", SenderId);
                cmd.Parameters.AddWithValue("@CampaignWise", CampaignWise);
                cmd.Parameters.AddWithValue("@TemplateId", TemplateId);
                cmd.Parameters.AddWithValue("@USERID", UserId);
                cmd.Parameters.AddWithValue("@MasterType", MasterType);
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                cmd.Parameters.AddWithValue("@UserType", UserType);
                cmd.Parameters.AddWithValue("@mobno", mobno);
                cmd.Parameters.AddWithValue("@DLTNO", DLTNO);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable DeleteFromSenderMapping(string SmppAccountId, string Systemid, string SenderId, string CountryCode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_DeleteValueFromSenderMapping";
                cmd.Parameters.AddWithValue("SmppAccountId", SmppAccountId);
                cmd.Parameters.AddWithValue("systemid", Systemid);
                cmd.Parameters.AddWithValue("SenderId", SenderId);
                cmd.Parameters.AddWithValue("CountryCode", CountryCode);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable SP_GetProfileDetailsForGrid(string Profileid = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetProfileDetailsForGird";
                cmd.Parameters.AddWithValue("Profileid", Profileid);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetProfileDetailsForGrid_API(string Profileid = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetProfileDetailsForGird_API";
                cmd.Parameters.AddWithValue("Profileid", Profileid);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetProfileDetailsForSenderGrid(string Profileid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetProfileDetailsForSenderGrid";
                cmd.Parameters.AddWithValue("Profileid", Profileid);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetProfileDetails(string Profileid = "", string Name = "", string CompanyName = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetProfileDetails";
                cmd.Parameters.AddWithValue("Profileid", Profileid);
                cmd.Parameters.AddWithValue("Name", Name);
                cmd.Parameters.AddWithValue("CompanyName", CompanyName);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetProfileDetails_API(string Profileid = "", string Name = "", string CompanyName = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetProfileDetails_API";
                cmd.Parameters.AddWithValue("Profileid", Profileid);
                cmd.Parameters.AddWithValue("Name", Name);
                cmd.Parameters.AddWithValue("CompanyName", CompanyName);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable SP_GetAllSenderID(string SmppAccountid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetSenderId";
                cmd.Parameters.AddWithValue("SMPPACCOUNTID", SmppAccountid);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public string SP_InsertValueForSmppAccId(string Userid, DataTable dt)
        {
            string msg = "";
            bool IsFirst = true;
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Sp_InsertValueInSmppAccounttbl";
                        cmd.Parameters.AddWithValue("Userid", Userid);
                        cmd.Parameters.AddWithValue("SmppAccountId", dt.Rows[i]["smppaccountid"].ToString());
                        cmd.Parameters.AddWithValue("CountryCode", dt.Rows[i]["CountryCode"].ToString());
                        cmd.Parameters.AddWithValue("IsFirst", IsFirst);
                        cmd.Parameters.AddWithValue("Result", "");
                        cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                        cmd.Parameters["Result"].Size = 0x100;
                        cmd.ExecuteNonQuery();
                        msg = cmd.Parameters["Result"].Value.ToString();

                        IsFirst = false;
                    }
                }
            }
            return msg;
        }

        public string SP_InsertValueForSmppAccId_API(string Userid, DataTable dt)
        {
            string msg = "";
            bool IsFirst = true;
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Sp_InsertValueInSmppAccounttbl_API";
                        cmd.Parameters.AddWithValue("Userid", Userid);
                        cmd.Parameters.AddWithValue("SmppAccountId", dt.Rows[i]["smppaccountid"].ToString());
                        cmd.Parameters.AddWithValue("CountryCode", dt.Rows[i]["CountryCode"].ToString());
                        cmd.Parameters.AddWithValue("IsFirst", IsFirst);
                        cmd.Parameters.AddWithValue("Result", "");
                        cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                        cmd.Parameters["Result"].Size = 0x100;
                        cmd.ExecuteNonQuery();
                        msg = cmd.Parameters["Result"].Value.ToString();

                        IsFirst = false;
                    }
                }
            }
            return msg;
        }

        public string SP_InsertValueForSenderIdMast(string Userid, DataTable dt1)
        {
            string msg = "";
            bool IsFirst = true;
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string input = dt1.Rows[i]["Route"].ToString();
                        string Provider = input.Split('-').First().Trim();
                        string Systemid = input.Split('-')[1].Trim();

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Sp_InsertValueInSenderIdMast";
                        cmd.Parameters.AddWithValue("Userid", Userid);
                        cmd.Parameters.AddWithValue("CountryCode", dt1.Rows[i]["CountryCode"].ToString());
                        cmd.Parameters.AddWithValue("Senderid", dt1.Rows[i]["Senderid"].ToString());
                        cmd.Parameters.AddWithValue("Provider", Provider);
                        cmd.Parameters.AddWithValue("Systemid", Systemid);
                        cmd.Parameters.AddWithValue("IsFirst", IsFirst);
                        cmd.Parameters.AddWithValue("Result", "");
                        cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                        cmd.Parameters["Result"].Size = 0x100;
                        cmd.ExecuteNonQuery();
                        msg = cmd.Parameters["Result"].Value.ToString();

                        IsFirst = false;
                    }
                }
            }
            return msg;
        }

        #region < Custom Report >
        public void Executeprocedure(string sp_name, List<SqlParameter> prams = null)
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandText = sp_name;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (prams != null)
                    {
                        foreach (SqlParameter a in prams)
                        {
                            cmd.Parameters.Add(a);
                        }

                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cnn.Close();


                }


                catch (Exception ex)
                {
                    throw ex;

                }

            }


        }

        public DataTable ProcedureDatatable(string sp_name, List<SqlParameter> prams = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 3600;
                cmd.CommandText = sp_name;
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                if (prams != null)
                {
                    foreach (SqlParameter a in prams)
                    {
                        cmd.Parameters.Add(a);

                    }

                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                cmd.Parameters.Clear();
                cnn.Close();

            }

            return dt;
        }

        public bool ShowMsgExecuteprocedure(string sp_name, List<SqlParameter> prams = null)
        {
            bool flag = true;
            try
            {

                Executeprocedure(sp_name, prams);

            }
            catch (Exception ex)
            {
                flag = false;
                throw ex;


            }


            return flag;

        }

        #endregion

        public string InsertSignUp(string Name, string MobileNo, string EmailId, string Designation, string CompanyName, string counryCode)
        {
            try
            {
                string msg = "";
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandText = "[SP_InsertSignUp]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Name", Name);
                    cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                    cmd.Parameters.AddWithValue("EmailId", EmailId);
                    cmd.Parameters.AddWithValue("Designation", Designation);
                    cmd.Parameters.AddWithValue("Company", CompanyName);
                    cmd.Parameters.AddWithValue("counryCode", counryCode);
                    cmd.Parameters.AddWithValue("Msg", "");
                    cmd.Parameters["Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["Msg"].Size = 0x100;
                    cmd.ExecuteNonQuery();
                    msg = cmd.Parameters["Msg"].Value.ToString();
                    cnn.Close();
                }
                return msg;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataTable GetCountryForSignUp()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_GetCountryForSignUp";

                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetMonthSummary(string id, string UserType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetMonthSummary";
                cmd.Parameters.AddWithValue("Id", id);
                cmd.Parameters.AddWithValue("UserType", UserType);

                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Message
        {
            public string to { get; set; }
            public Status status { get; set; }
            public string messageId { get; set; }
        }

        public class OBDRoot
        {
            public string bulkId { get; set; }
            public List<Message> messages { get; set; }
        }

        public class Status
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public void obdpostapi_Bulk(DataTable dtmob, string ScenarioKey = "", string authkey = "", string DIDNUMBER = "")
        {
            string Destination = "";

            for (int i = 0; i < dtmob.Rows.Count; i++)
            {

                if (i == 0)
                {
                    Destination = Destination + @"{
                    ""to"": """ + dtmob.Rows[i]["TOMOBILE"].ToString() + @"""
                        }";
                }
                else
                {
                    Destination = Destination + @",{
                    ""to"": """ + dtmob.Rows[i]["TOMOBILE"].ToString() + @"""
                        }";
                }
            }

            //4472461
            var client = new RestClient("https://vj1kpv.api.infobip.com/voice/ivr/1/messages");
            client.Timeout = -1;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", authkey);
            request.AddHeader("Content-Type", "application/json");
            //911408852313
            var body = @"{
    ""messages"": [
        {
            ""scenarioId"": """ + ScenarioKey + @""",
            ""from"": """ + DIDNUMBER + @""",
            ""destinations"": [" + Destination + @"],
            ""notifyUrl"": ""http://103.205.64.220:17250/obdapi/api/OBD/GetDLR"",
            ""notifyContentType"": ""application/json"",
            ""validityPeriod"": 720,
            ""retry"": {
                ""minPeriod"": 1,
                ""maxPeriod"": 5,
                ""maxCount"": 5
            },
            ""record"": false,
            ""deliveryTimeWindow"": {
                ""from"": {
                    ""hour"": 0,
                    ""minute"": 1
                },
                ""to"": {
                    ""hour"": 23,
                    ""minute"": 59
                },
                ""days"": [
                    ""MONDAY"",
                    ""TUESDAY"",
                    ""WEDNESDAY"",
                    ""THURSDAY"",
                    ""FRIDAY"",
                    ""SATURDAY"",
                    ""SUNDAY""
                ]
            }
        }
    ]
}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            IRestResponse response = client.Execute(request);

            try
            {
                OBDRoot res = JsonConvert.DeserializeObject<OBDRoot>(response.Content);
                Message msg = new Message();


                String EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");


                for (int i = 0; i < res.messages.Count; i++)
                {
                    msg = res.messages[i];
                    Status st = new Status();
                    st = msg.status;

                    // saveapiresp(msg.messageId, msg.to, st.groupName, st.description, "", ScenarioKey, FileId, SessionId, StartTime, EndTime);
                    string strs = InsertSignUpVoiceResp(msg.messageId, msg.to, st.groupName, st.description, "", ScenarioKey);
                }

            }
            catch (Exception ex)
            {
                // saveapiresp("", "", "", "", response.Content, ScenarioKey, FileId, SessionId, StartTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            //Console.WriteLine(response.Content);
        }

        public void saveapiresp(string messageId, string mob, string groupName, string description, string resp, string ScenarioKey, string FileId, string SessionId, string StartTime, string EndTime)
        {
            string str = "";
            try
            {
                string sql = @"insert into obdResplog1 (msgid,mobno,groupName,description,response,ScenarioKey,FileId,SessionId,StartTime,EndTime)
                values('" + messageId + "','" + mob + "','" + groupName + "','" + description + "','" + resp + "','" + ScenarioKey + "','" + FileId + "','" + SessionId + "','" + StartTime + "','" + EndTime + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string InsertSignUpVoiceResp(string msgid, string mob, string groupName, string description, string resp, string ScenarioKey)
        {
            try
            {
                string msg = "";
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandText = "[InsertSignUpVoiceResp]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("msgid", msgid);
                    cmd.Parameters.AddWithValue("mob", mob);
                    cmd.Parameters.AddWithValue("groupName", groupName);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("resp", resp);
                    cmd.Parameters.AddWithValue("ScenarioKey", ScenarioKey);
                    cmd.Parameters.AddWithValue("Msg", "");
                    cmd.Parameters["Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["Msg"].Size = 0x100;
                    cmd.ExecuteNonQuery();
                    msg = cmd.Parameters["Msg"].Value.ToString();
                    cnn.Close();
                }
                return msg;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetValueForResendSMS(string FileId, string Userid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetValueForResendSMS";
                cmd.Parameters.AddWithValue("FileId", FileId);
                cmd.Parameters.AddWithValue("Userid", Userid);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetSApprovedTemplateOfUserResend(string usernm, string templateId)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select  template,Concat(TemplateID ,' ' ,isnull(tempname,'')) TemplateID,TemplateID as onlyTemplateID from templaterequest 
                where username = '" + usernm + "' and isnull(allotted,0)=1 and isnull(IsWhatsapp,0)=0 and templateid='" + templateId + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public string SaveTempTableResendSMS(string user, string fileId, string s, string moblen = "")
        {
            bool chkUnq = user.ToUpper() == "MIM2101371" ? false : false;
            string username = user;
            string user1 = "tmp1_" + user;
            user = "tmp_" + user;
            string colnm = "";
            string datatype = "";
            string sql = "";

            string ccode = Convert.ToString(database.GetScalarValue("Select defaultCountry from customer where username='" + username + "'"));
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();

                DataTable dt = GetMobileToResendSMS(username, fileId);
                if (dt.Rows.Count > 0)
                {

                    sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
                    string sql1 = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + "";

                    sb.Append(@"Create table " + user + "  (");
                    sb1.Append(@"Create table " + user1 + "  (");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != dt.Columns.Count - 1)
                        {
                            sb.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                            sb1.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                        }
                        else
                        {
                            sb.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                            sb1.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                        }
                    }
                    sb.Append(")");
                    sb1.Append(")");

                    database.ExecuteNonQuery(sql + "  " + sb.ToString() + " " + sql1 + " " + sb1.ToString());
                    database.BulkInsertDataDynamic(dt, user1);
                    database.BulkInsertDataDynamic(dt, user);
                    colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                }
            }
            catch (Exception ex)
            {

            }


            database.ExecuteNonQuery("delete from " + user + @"   where len([" + colnm + "]) < " + moblen);
            database.ExecuteNonQuery("update " + user + @" set [" + colnm + "] = right([" + colnm + "], " + moblen + ") where len([" + colnm + "]) > " + moblen);

            database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.[" + colnm + "] ");
            /* rabi 15 july 21*/
            database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + username + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + username + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

            //check column name is numeric or not
            sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
            Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
            if (x == 1) return "Column name is numeric. Cannot upload file.";

            try
            {
                sql = "select convert(numeric,[" + colnm + "]) from " + user;
                DataTable dt = database.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            }

            // CHECK FOR ALL NULL VALUES
            sql = "select count(distinct [" + colnm + "]) from " + user + " where [" + colnm + "] is not null ";
            Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            if (Y <= 0) return "No Mobile Numbers found in the file";
            return "RECORDCOUNT " + Y.ToString();
        }

        public DataTable GetMobileToResendSMS(string username, string fileid)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select tomobile from MSGSUBMITTEDLOG with(nolock) where PROFILEID='" + username + "' and FILEID='" + fileid + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public void UpdateLastLoginDate(string UserId)
        {
            string sql = @"update LoginEntry set LastLoginDate=getdate() where UserId='" + UserId.ToString() + "'";
            database.ExecuteNonQuery(sql);
        }

        public void InsertIpMac(string Userid, string MacId, string IpAddress)
        {
            string sql = "insert into LoginLog (UserId, MacAddress, IpAddress) values('" + Userid + "','" + MacId + "','" + IpAddress + "') ";
            database.ExecuteNonQuery(sql);
        }

        public bool GetIPWhiteListing(string Userid)
        {
            bool IPWhiteListing = Convert.ToBoolean(database.GetScalarValue("Select IPWhitelisting from Customer with(nolock) where username ='" + Userid + "'"));
            return IPWhiteListing;
        }

        public DataTable GetIP(string Userid)
        {
            DataTable dt = database.GetDataTable("select IpAddress from CustomerIp with(nolock) where UserId='" + Userid + "'");
            return dt;
        }

        //modified by vikas on 13-03-2023
        public void InsertCredentialsInLoginEntry(string UserId)
        {
            string sql = @"Insert into LoginEntry(UserId,EntryDate,LastLoginDate) values('" + UserId + "',getdate(),getdate())";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetRecord(string proc, List<SqlParameter> pram = null)
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                try
                {

                    cnn.Open();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandText = proc;
                    cmd.Parameters.Clear();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (pram != null)
                    {
                        foreach (SqlParameter p in pram)
                        {
                            if (p != null)
                            {
                                cmd.Parameters.Add(p);

                            }

                        }

                    }


                    da.Fill(dt);


                    return dt;


                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    cnn.Close();
                }

            }
        }

        public DataTable GetMonthSummaryBD(string EmpCode, string UserType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetMonthSummaryBD";
                cmd.Parameters.AddWithValue("EmpCode", EmpCode);
                cmd.Parameters.AddWithValue("UserType", UserType);

                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetDashboardSummaryBD(string EmpCode)
        {

            string sql = @"select Convert(varchar,isnull(DATEADD(mi," + Global.addMinutes + @", updtime),getdate()),106) + ' ' + Convert(varchar(5),isnull(DATEADD(mi," + Global.addMinutes + @", updtime),getdate()),108) as updtime,
isnull(smssubmitted,0) as smssubmitted,isnull(smsdelivered,0) as smsdelivered,isnull(smsfailed,0) as smsfailed,
isnull(smsunknown,0) as smsunknown,isnull(links,0) as links,isnull(clicks,0) as clicks,isnull(smsclicks,0) as smsclicks from dashboard d with (nolock) 
join customer c on d.userid=c.username where c.EmpCode='" + EmpCode + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSSummaryBD(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @"select count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
                 from customer c with(nolock)
                inner
                 join MSGSUBMITTED s with(nolock) on c.username = s.profileid 
                left join delivery d with(nolock) on s.msgid = d.msgid
                where s.createdat between '" + f + "' and '" + t + @"' ";
            if (usertype == "A")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where EmpCode='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }
            if (usertype == "E")
            {
                sql = sql + " and c.EmpCode = '" + user + "' ";
            }

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string GetAccountSummaryBD(string f, string t, string usertype, string user)
        {
            string sql = "";
            string dlt = "";

            sql = @" SELECT COUNT (c.username) FROM CUSTOMER c
                where c.ACCOUNTCREATEDON between '" + f + "' and '" + t + @"' ";
            if (usertype == "A")
            {
                dlt = Convert.ToString(database.GetScalarValue("Select Top 1 dltno from customer where EmpCode='" + user + "'"));
                sql = sql + " and c.dltno = '" + dlt + "' ";
            }

            string dt = Convert.ToString(database.GetScalarValue(sql));
            return dt;
        }

        #region<--Abhishek-->
        public DataTable GetMostActiveUserEmpCode(int noofrecord, string userType, string empcode)
        {
            string sql = @"SELECT top " + noofrecord + @" COMPNAME,s.profileid as userid,count(s.id) submitted,
                        sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                        sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                        sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
                                        from MSGSUBMITTED s with(nolock) 
                                        inner join customer c with(nolock) ON c.username = s.profileid
                                        left join DELIVERY d with(nolock) on s.msgid = d.msgid 
                                        where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";
            if (userType.ToLower() != "sysadmin")
            {
                sql = sql + " AND dltno = '" + empcode + "' ";
            }
            sql = sql + "group by COMPNAME,s.profileid order by submitted desc";
            return database.GetDataTable(sql);

        }

        public DataTable GetDataTableRecord(string proc_name, List<SqlParameter> pram = null)
        {
            try
            {
                using (con = new SqlConnection(database.GetConnectstring()))
                {
                    con.Open();
                    cmd = new SqlCommand();
                    cmd.CommandText = proc_name;
                    cmd.Connection = con;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (pram != null)
                    {
                        foreach (SqlParameter p in pram)
                        {
                            if (p != null)
                            {
                                cmd.Parameters.Add(p);

                            }

                        }


                    }

                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                con.Close();

            }
        }

        public string ExecuteRecord(string proc_name, List<SqlParameter> pram = null)
        {
            string msg = "";
            try
            {

                using (con = new SqlConnection(database.GetConnectstring()))
                {
                    con.Open();
                    cmd = new SqlCommand();
                    cmd.CommandText = proc_name;
                    cmd.Connection = con;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (pram != null)
                    {
                        foreach (SqlParameter p in pram)
                        {
                            if (p != null)
                            {
                                cmd.Parameters.Add(p);

                            }

                        }


                    }

                    cmd.ExecuteNonQuery();
                    msg = "SuccessFully";


                }


            }
            catch (Exception ex)
            {
                msg = "error";
                throw ex;

            }
            finally
            {

                con.Close();

            }
            return msg;
        }

        public string SP_Deletesmppaccountuserid(string Userid, string countrycode, string smppaccountid, string procname)
        {
            string msg = "";
            bool IsFirst = true;
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();

                try
                {


                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;
                    cmd.Parameters.AddWithValue("@userid", Userid);
                    cmd.Parameters.AddWithValue("@CountryCode", countrycode);
                    cmd.Parameters.AddWithValue("@smppaccountid", smppaccountid);
                    //cmd.Parameters.AddWithValue("CountryCode", );
                    //cmd.Parameters.AddWithValue("IsFirst", IsFirst);
                    //cmd.Parameters.AddWithValue("Result", "");
                    //cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                    //cmd.Parameters["Result"].Size = 0x100;
                    cmd.ExecuteNonQuery();
                    //msg = cmd.Parameters["Result"].Value.ToString();

                    cnn.Close();
                    msg = "Sucessfully";

                }
                catch (Exception ex)
                {
                    msg = "Error";
                    throw ex;


                }



            }
            return msg;
        }

        public DataTable GetRecordDataTableSql(string sql)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(sql))
            {
                dt = database.GetDataTable(sql);
            }

            return dt;

        }

        public DateTime DateChangeFormat(string date)
        {
            DateTime datep;
            string[] pdate = date.Split('-');

            return datep = Convert.ToDateTime(pdate[2].ToString() + "/" + pdate[1].ToString() + "/" + pdate[0].ToString());

        }

        #endregion

        public bool CheckMobileEmailDuplicateBD(string val, string type)
        {
            string sql = "Select Count(*) from bdcustomerrequest where " + (type == "E" ? " EMAIL " : " MOBILE1 ") + " = '" + val + "'";
            int c = Convert.ToInt16(database.GetScalarValue(sql));
            if (c > 0) return true; else return false;
        }

        public string SaveCustomerBD(string Name, string CompName, string Mob1, string usertype, string Email, string ActType, string user, string LOGINusertype, string EmpCode)
        {
            string UserID = GenerateUserID();
            string PWD = GeneratePWD();
            //DataTable dt = database.GetDataTable("select * from settings");
            //DataTable dtc = database.GetDataTable("select * from Customer where username='" + user + "'");
            //string b, n, s, c, o, u, d;

            //b = dt.Rows[0]["balance"].ToString();

            //if (LOGINusertype == "ADMIN")
            //{
            //    n = dtc.Rows[0]["rate_normalsms"].ToString();
            //    s = dtc.Rows[0]["rate_smartsms"].ToString();
            //    c = dtc.Rows[0]["rate_campaign"].ToString();
            //    o = dtc.Rows[0]["rate_otp"].ToString();
            //    d = dtc.Rows[0]["dltcharge"].ToString();
            //    u = dtc.Rows[0]["urlrate"].ToString();
            //    DLT = dtc.Rows[0]["DLTNO"].ToString();
            //}
            //else
            //{
            //    n = dt.Rows[0]["NORMALSMSRATE"].ToString();
            //    s = dt.Rows[0]["SMARTSMSRATE"].ToString();
            //    c = dt.Rows[0]["CAMPAIGNSMSRATE"].ToString();
            //    o = dt.Rows[0]["OTPSMSRATE"].ToString();
            //    d = dt.Rows[0]["DLTDeduction4refund"].ToString();
            //    u = dt.Rows[0]["urlrate"].ToString();
            //}

            string sql = "Insert into BDCustomerRequest (USERNAME,PWD,FULLNAME,ACCOUNTTYPE,COMPNAME,MOBILE1,USERTYPE,EMAIL,ACCOUNTCREATEDON,ACTIVE,EmpCode) " +
            " Values ('" + UserID + "','" + PWD + "'," + "'" + Name + "'," + "'" + ActType + "'," + "'" + CompName + "'," + "'" + Mob1 + "'," + "'" + usertype + "'," + "'" + Email + "'," +
            "GETDATE(),1," + "'" + EmpCode + "')";
            database.ExecuteNonQuery(sql);
            //sql = "Insert into Dashboard (userid) values ('" + UserID + "')";
            //sql += " Insert into smsrateaspercountry (USERNAME,countrycode, rate_normalsms,rate_campaign, rate_smartsms, rate_otp, urlrate, dltcharge,insertdate) values ('" + UserID + "','" + CountryCode + "','" + n + "','" + c + "','" + s + "','" + o + "','" + u + "','" + d + "',GETDATE())";

            //database.ExecuteNonQuery(sql);
            //string msg = "Account created for MIM. USER ID: " + UserID + " PASSWORD: " + PWD;
            //string sender = Convert.ToString(database.GetScalarValue("Select senderid from customer where username='" + user + "'"));
            //string peidAdmin = Convert.ToString(database.GetScalarValue("Select peid from customer where username='" + user + "'"));
            //string mailmsg = "Account Created for MIM Panel. \n \n User ID: " + UserID + "  \n Password: " + PWD;
            //string re = SendEmail(Email, "Account Created for MIM Panel", mailmsg,
            //    "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net");
            //sendEmail(UserID, PWD);
            return "Account Successfully Created";

        }

        public DataTable GetValueGroupName(int noofrecord, string userType, string dltNO, string empcode, string grpname)
        {
            //string sql = @"SELECT top " + noofrecord + @" c.username as Username,count(s.id) submitted,
            //            sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            //            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            //            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
            //                            from MSGSUBMITTED s with(nolock) 
            //                            inner join customer c with(nolock) ON c.username = s.profileid
            //                            left join DELIVERY d with(nolock) on s.msgid = d.msgid 
            //                            where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";


            string sql = @"SELECT top " + noofrecord + @" c.username as Username,c.DLTNO as dltno,SUM(s.smssubmitted) as submitted,
SUM(s.smsdelivered) as delivered,SUM(s.smsfailed) as failed,(sum(s.smssubmitted)-sum(s.smsdelivered)-sum(s.smsfailed)) as unknown                                      
from dashboard s with(nolock) inner join customer c with(nolock) ON c.username = s.userid where s.updtime >= CONVERT(varchar,GETDATE(),102) ";
            if (userType.ToUpper() == "BD")
            {
                sql = sql + " AND c.EmpCode = '" + empcode + "' and c.groupname='" + grpname + "' and c.USERTYPE='admin' ";
            }

            if (userType.ToUpper() == "SYSADMIN" || userType.ToUpper() == "ADMIN")
            {
                sql = sql + " AND c.dltno = '" + dltNO + "' ";
            }
            //sql = sql + "group by c.username order by submitted desc";
            sql = sql + " GROUP BY GROUPNAME,c.username,c.DLTNO  order by submitted desc";
            return database.GetDataTable(sql);
        }

        public DataTable GetValueForNestedGridTwo(int noofrecord, string userType, string dltNO, string empcode, string grpname, string Username)
        {
            string EWdlt = Convert.ToString(database.GetScalarValue("select dltno from customer with(nolock) where username='" + Username + "' "));
            //string sql = @"SELECT top " + noofrecord + @" c.username as Username,count(s.id) submitted,
            //            sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            //            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            //            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
            //                            from MSGSUBMITTED s with(nolock) 
            //                            inner join customer c with(nolock) ON c.username = s.profileid
            //                            left join DELIVERY d with(nolock) on s.msgid = d.msgid 
            //                            where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";




            string sql = @"SELECT top " + noofrecord + @" c.username as Username,SUM(s.smssubmitted) as submitted,
                         SUM(s.smsdelivered) as delivered,SUM(s.smsfailed) as failed,(sum(s.smssubmitted)-sum(s.smsdelivered)-sum(s.smsfailed)) as unknown                                      
                         from dashboard s with(nolock) 
                         inner join customer c with(nolock) ON c.username = s.userid where s.updtime >= CONVERT(varchar,GETDATE(),102)";
            if (userType.ToUpper() == "BD")
            {
                sql = sql + " AND c.dltno = '" + EWdlt + "' and c.usertype='user' ";
            }

            if (userType.ToUpper() == "SYSADMIN" || userType.ToUpper() == "ADMIN")
            {
                sql = sql + " AND c.dltno = '" + dltNO + "' ";
            }
            //sql = sql + "group by c.username order by submitted desc";
            sql = sql + " AND smssubmitted<>0 GROUP BY GROUPNAME,c.username  order by submitted desc";
            return database.GetDataTable(sql);
        }

        public DataTable GetValueGroupWise(int noofrecord, string userType, string empcode)
        {
            string sql = @" SELECT   Top " + noofrecord + @"  GROUPNAME,SUM(smssubmitted) submitted,SUM(smsdelivered) delivered,SUM(smsfailed) failed,SUM(smssubmitted-smsdelivered-smsfailed) unknown FROM dashboard    d  with(nolock)
 join CUSTOMER r ON  r.username=d.userid  WHERE  USERTYPE='" + userType + @"'  AND EmpCode='" + empcode + @"' AND GROUPNAME is not null AND smssubmitted<>0  AND  d.updtime >= CONVERT(varchar,GETDATE(),102) GROUP BY GROUPNAME ORDER BY submitted DESC
";

            return database.GetDataTable(sql);
        }

        public DataTable GetValueAdminWise(int noofrecord, string userType, string groupName)
        {
            string sql = @"
  SELECT  Top " + noofrecord + @" userid,DLTNO,SUM(smssubmitted) submitted,SUM(smsdelivered) delivered,SUM(smsfailed) failed,SUM(smssubmitted-smsdelivered-smsfailed) unknown FROM dashboard    d  with(nolock)
 join CUSTOMER r ON  r.username=d.userid  WHERE  USERTYPE='" + userType + @"'  AND GROUPNAME='" + groupName + @"' AND GROUPNAME is not null AND smssubmitted<>0  AND   d.updtime >= CONVERT(varchar,GETDATE(),102) GROUP BY  userid,DLTNO ORDER BY submitted DESC";

            return database.GetDataTable(sql);
        }

        public DataTable GetValueUserWise(int noofrecord, string userType, string DLTNo)
        {
            string sql = @"
  SELECT  Top " + noofrecord + @"  userid,COMPNAME,SUM(smssubmitted) submitted,SUM(smsdelivered) delivered,SUM(smsfailed) failed,SUM(smssubmitted-smsdelivered-smsfailed) unknown FROM dashboard    d  with(nolock)
 join CUSTOMER r ON  r.username=d.userid  WHERE  USERTYPE='" + userType + @"'  AND DLTNO='" + DLTNo + @"' AND GROUPNAME is not null AND smssubmitted<>0 AND   d.updtime >= CONVERT(varchar,GETDATE(),102) GROUP BY  userid,COMPNAME ORDER BY submitted DESC";

            return database.GetDataTable(sql);
        }

        public DataTable GetDealerWiseList(string UserID, string GroupByCode, DateTime FromDate, DateTime ToDate, string DealerCode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetDealerWiseList";
                //cmd.Parameters.AddWithValue("Datewise", Datewise);
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.Parameters.AddWithValue("GroupByCode", GroupByCode);
                cmd.Parameters.AddWithValue("FromDate", FromDate);
                cmd.Parameters.AddWithValue("ToDate", ToDate);
                cmd.Parameters.AddWithValue("DealerCode", DealerCode);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetSMSReport2(string UserID, string MobileNo, string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetSMSReport2";
                //cmd.Parameters.AddWithValue("Datewise", Datewise);
                cmd.Parameters.AddWithValue("ProfileID", UserID);
                cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("FromDate", FromDate);
                cmd.Parameters.AddWithValue("ToDate", ToDate);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public string SP_InsertCampaignEntryDetails(string Userid, DateTime reqdate, string campname, string smscredit, string submitted, string delivered, string dlvrper, string failed, string failedper, string awaited, string awaitedper, string hitcount, string hitcountper, string filename, string serderid, string smstext)
        {
            string msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_insertcampaignentrydetails";
                cmd.Parameters.AddWithValue("UserId", Userid);
                cmd.Parameters.AddWithValue("RequestDate", reqdate);
                cmd.Parameters.AddWithValue("CampaignName", campname);
                cmd.Parameters.AddWithValue("SMSCredit", smscredit);
                cmd.Parameters.AddWithValue("Submitted", submitted);
                cmd.Parameters.AddWithValue("Delivered", delivered);
                cmd.Parameters.AddWithValue("DlvrPer", dlvrper);
                cmd.Parameters.AddWithValue("Failed", failed);
                cmd.Parameters.AddWithValue("failedper", failedper);
                cmd.Parameters.AddWithValue("Awaited", awaited);
                cmd.Parameters.AddWithValue("Awaitedper", awaitedper);
                cmd.Parameters.AddWithValue("HitCount", hitcount);
                cmd.Parameters.AddWithValue("HitCountper", hitcountper);
                cmd.Parameters.AddWithValue("Filename", filename);
                cmd.Parameters.AddWithValue("Senderid", serderid);
                cmd.Parameters.AddWithValue("SMSText", smstext);
                cmd.Parameters.AddWithValue("Result", "");
                cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["Result"].Size = 0x100;
                cmd.ExecuteNonQuery();
                msg = cmd.Parameters["Result"].Value.ToString();

            }
            return msg;
        }

        public DataTable GetAllCampaignentrytbl(string user)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select distinct campaignname from CampaignEntry with(nolock) where isnull(campaignname,'') <> '' order by campaignname ";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataSet SP_GetCampaignEntryData(string user, DateTime s1, DateTime s2, string camp)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetcampaigEntryrpt";

                cmd.Parameters.AddWithValue("usr", user);
                cmd.Parameters.AddWithValue("Fdate", s1);
                cmd.Parameters.AddWithValue("tdate", s2);
                cmd.Parameters.AddWithValue("camp", camp);

                da.Fill(ds);
            }
            return ds;
        }

        public string SP_InsertFileUCampDetails()
        {
            string msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_InsertFileUCampDetails";
                cmd.Parameters.AddWithValue("Result", "");
                cmd.Parameters["Result"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["Result"].Size = 0x100;
                cmd.ExecuteNonQuery();
                msg = cmd.Parameters["Result"].Value.ToString();

            }
            return msg;
        }

        public DataTable GetRecordDataTable(string proc_name)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc_name;

                da.Fill(dt);
            }
            return dt;
        }

        #region<--Abhishek-->

        public void SendWABAthroughAPI(string mob, string WABATemplateName, string WABAVariables, string wabaProfileId, string wabaPwd, string HeaderType = "", string imgURL = "")
        {
            try
            {
                string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string[] separatingStrings = { "$," };
                string[] ar = WABAVariables.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                string param = "";
                if (WABAVariables != "")
                {
                    for (int i = 0; i < ar.Length; i++)
                    {
                        param = param + @"""" + ar[i].Replace(',', ' ') + @"""";
                        if (i != ar.Length - 1) param = param + ",";
                    }
                }

                var client = new RestClient("http://103.205.64.220:17250/wabaapi/api/sendwaba");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
" + "\n" +
                @"    ""ProfileId"": """ + wabaProfileId + @""",
" + "\n" +
                @"    ""APIKey"": """ + wabaPwd + @""",
" + "\n" +
                @"    ""MobileNumber"": """ + mob + @""",
" + "\n" +
                @"    ""templateName"": """ + WABATemplateName + @""",
" + "\n" +
                @"    ""Parameters"": [ " + param + @"
" + "\n" +
                @"    ],
" + "\n" +
                @"    ""HeaderType"": """ + HeaderType + @""",
" + "\n" +
                @"    ""Text"": """",
" + "\n" +
                @"    ""MediaUrl"": """ + imgURL + @""",
" + "\n" +
                @"    ""isTemplate"": ""true""
" + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                logAPIwaba("req-" + body + " resp-" + response.Content);
                //Console.WriteLine(response.Content);
            }
            catch (Exception e1) { }
        }

        public void Info_Err3(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }

        public void logAPIwaba(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"wabaAPI_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"wabaAPI_catch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }
        #endregion

        //------Add Code BY Vikas 06-03-2023 --------  for Bind Dropdown TempName And TempId-----------
        public DataTable GetTempIdAndName(string usr)
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT templateid,templateid+'('+tempname+')' AS tempname FROM templaterequest WHERE ISNULL(templateid,'')<>''";
            if (usr != "")
            {
                sql = sql + " AND username='" + usr + "'";
            }
            sql = sql + " ORDER BY templateid";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTempIdAndNamenull()
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT templateid,templateid+'('+tempname+')' AS tempname FROM templaterequest WHERE ISNULL(templateid,'')<>''";
            sql = sql + " ORDER BY templateid";

            dt = database.GetDataTable(sql);
            return dt;
        }

        //------Add Code BY Vikas 07-03-2023 --------  for Datatable Template WIse-----------
        public DataTable GetTemplateWiseSMSSummary(string user, string f, string t, string TempIdAndName = "", string Action = "")
        {
            string sql = "";
            string dlt = "";
            if (Action == "1")
            {
                sql = @"SELECT convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
                      SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                      SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                      SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
                      FROM customer c inner join MSGSUBMITTED s ON c.username = s.profileid
                      LEFT JOIN delivery d ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106)";
            }
            else if (Action == "2")
            {
                sql = @"SELECT convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
                      SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                      SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                      SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
                      FROM customer c inner join MSGSUBMITTED s ON c.username = s.profileid
                      LEFT JOIN delivery d ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid UNION ALL ";
                sql = sql + @"SELECT convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
                       SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                       SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                       SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
                       FROM customer c inner join MSGSUBMITTEDLOG s ON c.username = s.profileid
                       LEFT JOIN DELIVERYLOG d ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106) desc";
            }
            else if (Action == "3")
            {
                sql = @"SELECT convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
                        SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                        SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                        SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
                        FROM customer c inner join MSGSUBMITTEDLOG s ON c.username = s.profileid
                        LEFT JOIN DELIVERYLOG d ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106) desc";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        //------Add Code BY Vikas 09-03-2023 --------  for Sent Login OTP For Customer Wise-----------
        public void SendOTPthroughAPI(string CountryCode, string mob, string Userid, string pwd, string SenderId, string OtpSms, string Peid, string TemplateId, string Otp)
        {
            //string apikey = ob.getIPapikey();
            string url = "";
            if (CountryCode == "91")
            {
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + mob + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("{#var#}", Otp) + "&msgtype=13&PEID=" + Peid.ToString() + "&Templateid=" + TemplateId.ToString();
            }
            else if (CountryCode == "971")
            {
                string mobno = "";
                if ((CountryCode + mob).Length > 12) mobno = mob; else mobno = CountryCode + mob;
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + mobno + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("{#var#}", Otp) + "&msgtype=16";
            }
            else
            {
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + CountryCode + mob + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("{#var#}", Otp) + "&msgtype=15";

            }
            Log_(url);
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                Log_(objWinRq.ResponseText);
                getResponseTxt = "[" + getResponseTxt + "]";
                database.ExecuteNonQuery("EXEC SP_InsertAndUpdateLoginOTP '" + Userid.ToString() + "','" + Otp.ToString() + "'");
            }
            catch (Exception EX)
            {
                Log_(EX.Message);
            }
        }

        //------Add Code BY Vikas 09-03-2023 -----------
        public void Log_(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"SentOTP_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"SentOTP_catch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }

        public DataTable GetDayWiseSMSSummaryDetail2(string user, string dat)
        {
            string sql = "";

            sql = @"select smsdate as smsDate1,convert(varchar,smsdate,106) SMSDATE,userid,SenderID,Submitted,Delivered,Failed, Unknown from DAYSUMMARY 
                    where userid='" + user + "' and smsdate between '" + dat + "' and '" + dat + @"'  ";

            sql = sql + " order by senderid ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        //------Add Code BY Vikas 13-03-2023 for Click_Reportu1ShortURL Data-----------

        public DataTable GetUserReportShortURlWise(string userid, string url, string frm, string to, string Checked = "Checked")
        {
            string sql = "";
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));
            if (Checked == "Unchecked")
            {
                sql = @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                sum(case when S.SHORTURL_ID is null then 0 else 1 end) AS No_Of_Hits,
                U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
                left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                where U.userid = '" + userid + "' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"";
            }
            else
            {
                sql = @"DECLARE @NO_Of_Hits INT;
                     SELECT @NO_Of_Hits=COUNT(*) FROM mobstats WITH(NOLOCK) WHERE 1=1 ";
                if (url != "-1")
                {
                    sql = sql + @" and shortUrl_id='" + url + "'";
                }
                if (frm.ToString() != "01-Jan-01 12:00:00 AM" && to.ToString() != "01-Jan-01 12:00:00 AM")
                {
                    sql = sql + @" and Click_date between '" + frm + "' and '" + to + @"';";
                }
                sql = sql + @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                  DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                  COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                  @NO_Of_Hits AS No_Of_Hits,convert(varchar,S.Click_date,102) as clkdate,
                  U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                  inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
                  left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                  where U.userid = '" + userid + "' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"";
            }
            if (url != "-1")
            {
                sql = sql + @" AND U.id='" + url + "'";
            }
            if (Checked == "Unchecked")
            {
                sql = sql + @"GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,cm.name,F.COUNTRYCODE order by U.id ";
            }
            else
            {
                sql = sql + @"GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id ";
            }

            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDataTempWise(string userid, string TempId, string frm, string to)
        {
            string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + userid + "'"));
            string sql = "";
            sql = @"DECLARE @NO_Of_Hits INT;
                     SELECT @NO_Of_Hits=COUNT(*) FROM mobstats WITH(NOLOCK) WHERE 1=1 ";
            sql = sql + @"SELECT row_number() over (order by U.added,U.id) AS SLNO, U.long_url as LongURL, u.domainname + U.segment as SmallURL,
                  DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,U.added,106) + ' ' + convert(varchar,U.added,108)) as CreationDate, 
                  COUNT(DISTINCT m.mobile) AS No_of_url_sent,
                  @NO_Of_Hits AS No_Of_Hits,convert(varchar,S.Click_date,102) as clkdate,
                  U.id as URLID,ISNULL(U.URLNAME,'') AS URLNAME ,cm.name Country,F.COUNTRYCODE FROM short_urls U inner join mobtrackurl m on U.ID = m.urlid
                  inner join SMSFILEUPLOAD F on F.ID = m.fileId AND F.USERID=u.userid inner join countrymast cm on cm.phonecode=F.COUNTRYCODE
                  left join mobstats S on U.ID = S.SHORTURL_ID and s.urlid=m.id
                  where U.userid = '" + userid + "' and m.templateid='" + TempId + "' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"";
            if (frm.ToString() != "01-Jan-01 12:00:00 AM" && to.ToString() != "01-Jan-01 12:00:00 AM")
            {
                sql = sql + @" and Click_date between '" + frm + "' and '" + to + @"'";
            }

            if (TempId != null)
            {
                sql = sql + @"GROUP BY U.long_url,u.domainname,U.SEGMENT,u.added,m.urlid,U.id,U.URLNAME,convert(varchar,S.Click_date,102),cm.name,F.COUNTRYCODE order by convert(varchar,S.Click_date,102) desc, U.id ";

            }
            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }


        // Add By Vikas 14-03-2023 for Bind User Login SMS Setting 
        public DataTable GetDataUserSetting(string usr)
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT Email,CCEmail,ReportonEmail,OTP_VERIFICATION_REQD,Login_OTP_Template_ID,Login_OTP_Sender_ID,Login_OTP_SMSWABA FROM customer WHERE username='" + usr + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        //------Add Code BY Vikas 15-03-2023 --------  for Sent Login OTP For Send Whatsapp-----------
        public string WABA_MIM_Media_Template_LoginOTP_Message_Gupshup(string mob, string acid, string authkey, string msgtext, string Otp)
        {
            string WABAProfileID = Convert.ToString(ConfigurationManager.AppSettings["WABAProfileID"]);
            string WABAAPIKEY = Convert.ToString(ConfigurationManager.AppSettings["WABAAPIKEY"]);
            string WABATemplateName = Convert.ToString(ConfigurationManager.AppSettings["WABATemplateName"]);
            var clientOptIn = new RestClient("https://waba.myinboxmedia.in/api/sendwabamsg?ProfileId=" + WABAProfileID + "&APIKey=" + WABAAPIKEY + "&MobileNumber=" + mob + @"&TemplateName=" + WABATemplateName + "&Parameters=" + Otp + "&HeaderType=TEXT&HeaderText=" + msgtext + "&MediaUrl=&Latitude=&Longitude=&isTemplate=true");
            clientOptIn.Timeout = -1;
            var requestOptIn = new RestRequest(Method.GET);
            IRestResponse responseOptIn = clientOptIn.Execute(requestOptIn);
            Log_("api_response -" + responseOptIn.Content);
            string pStatus = responseOptIn.StatusCode.ToString();
            database.ExecuteNonQuery("EXEC SP_InsertAndUpdateLoginOTP '" + acid.ToString() + "','" + Otp.ToString() + "'");
            return pStatus;
        }

        // Add By Vikas 15-03-2023 for Get SMS template for Login 
        public DataTable GetDataLoginSMSFortemplate(string usr, string TemplateId)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT template FROM templaterequest WHERE username='" + usr + "' and templateid='" + TemplateId + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        // Add By Vikas 31-03-2023 for Get Short URl
        public DataTable GetShortUrl(string UserLinkextID)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT id,urlname+('  ')+domainname+segment as shorturl FROM short_urls WITH(NOLOCK) WHERE userid='" + UserLinkextID + "'";
            dt = database.GetDataTable(sql);
            return dt;
        }



        // Add By Vikas 31-03-2023 for Get WABA Template
        public DataTable GetwaTemplateId(string userid)
        {
            string WABALINKDB = ConfigurationManager.AppSettings["WABALINKDB"].ToString();
            string sql = "";
            sql = @"select a.tName as name1,a.nid TemplateID from " + WABALINKDB + "template a with (nolock) where a.userid = '" + userid + "' and IsActive = 1 and AllotorReject = 'A' order by a.tName";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        // Add By Vikas 31-03-2023 for Get WABA Template
        public DataTable GetSendWabaLinkClick()
        {
            string sql = @"SELECT * FROM SendWABAOnLinkClick WITH(NOLOCK)";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        // Add By Vikas 03-04-2023 for Insert SendWabaOnLinkClick
        public void SaveSMSLinktoWABA(string LinkUserId, string ShortsUrlID, string ShortsUrl, string Segment, string WABAUserID, string WabaApiKey, string TempIdName, string TempType, string Url)
        {
            string str = "";
            try
            {
                string sql = @"INSERT INTO SendWABAOnLinkClick (LinkextUserId,ShortUrlID,ShortUrl,Segment,WABAUserID,WABAUserAPIKey,WABATemplateName,WABATemplateTempType,Url,InsertDateTime,Active) 
                           VALUES('" + LinkUserId + "','" + ShortsUrlID + "','" + ShortsUrl + "','" + Segment + "','" + WABAUserID + "','" + WabaApiKey + "','" + TempIdName + "','" + TempType + "','" + Url + "',CURRENT_TIMESTAMP,'1')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region<---Abhishek G-----09-05-2023-->
        public DataTable GetDataTableProc(string procname, List<SqlParameter> pram)
        {
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;
                    if (pram != null)
                    {
                        foreach (SqlParameter a in pram)
                        {
                            if (a != null)
                            {
                                cmd.Parameters.Add(a);

                            }

                        }
                    }
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    con.Close();
                }



            }

        }
        #endregion

        public DataTable filterTemplaterequest(string TempId, string TempName, string TemplateTxt, string userid, string senderid, string mastertype)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "";
                cmd.Connection = con;
                cmd.CommandText = "sp_filterTemplaterequest";
                cmd.Parameters.AddWithValue("@TemplateId", TempId);
                cmd.Parameters.AddWithValue("@TemplateName", TempName);
                cmd.Parameters.AddWithValue("@TempleteText", TemplateTxt);
                cmd.Parameters.AddWithValue("@Usernm", userid);
                cmd.Parameters.AddWithValue("@Senderid", senderid);
                cmd.Parameters.AddWithValue("@MasterType", mastertype);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public DataTable filterTemplatepopup(string userid, string TempId, string TempName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "";
                cmd.Connection = con;
                cmd.CommandText = "sp_TemplateFilter";
                cmd.Parameters.AddWithValue("@UserName", userid);
                cmd.Parameters.AddWithValue("@TemplateId", TempId);
                cmd.Parameters.AddWithValue("@TempName", TempName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public string InsertDealerMasterDltNoBased(string DealerCode, string DealerName, double DealerMobile, string SMSUserID, string smspwd, string smssender, string PEID, string SMSDomain, int Inactive, string BalUrl)
        {
            string msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(database.GetConnectstring_HyundaiDb()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "Sp_DEALERMASTERDLTNOINSERT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerCode", DealerCode);
                    cmd.Parameters.AddWithValue("@DealerName", DealerName);
                    cmd.Parameters.AddWithValue("@DealerMobile", DealerMobile);
                    cmd.Parameters.AddWithValue("@SMSuserId", SMSUserID);
                    cmd.Parameters.AddWithValue("@SMSPwd", smspwd);
                    cmd.Parameters.AddWithValue("@Smssender", smssender);
                    cmd.Parameters.AddWithValue("@PEID", PEID);
                    cmd.Parameters.AddWithValue("@SmsDomain", SMSDomain);
                    cmd.Parameters.AddWithValue("@Inactive", Inactive);
                    cmd.Parameters.AddWithValue("@Balurl", BalUrl);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 50);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    msg = Convert.ToString(cmd.Parameters["@Result"].Value);
                    con.Close();
                }
            }
            catch (Exception E) { }
            return msg;
        }

        #region Naved Khan..13/06/2023

        public DataTable GetSMSReportMobileWise(string f, string t, string mob)
        {
            string sql = "";
            sql = @"select m.msgid as MessageId,m.TOMOBILE as MobileNo, m.senderid as Sender,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                    msgtext as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE FROM MSGSUBMITTED m with (nolock) left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                where m.sentdatetime between '" + f + "' and '" + t + "' and TOMOBILE LIKE '%" + mob + "%' order by m.sentdatetime ";


            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        #endregion

        public DataTable GetSenderIdDLTNoWise(string DLTNO)
        {
            string sql = "";
            sql = @"SELECT DISTINCT SENDERID FROM CUSTOMER WITH(NOLOCK) WHERE DLTNO IN(" + DLTNO + ") ORDER BY SENDERID";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateIdSenderWise(string SenderID)
        {
            string sql = "";
            sql = @"SELECT Concat(TemplateID ,' ' ,isnull(templateName,'')) AS TemplateName,TemplateID as template from TemplateID WHERE senderid='" + SenderID + "' AND isnull(TemplateID,'')<>'' order by templateid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetHMILSMSSummary(string f, string t, string to, string DLTNO = "", string senderId = "", string TemplateID = "")
        {
            string sql = "";
            sql = @"SELECT '" + f + "' AS FromDATE,'" + to + "' AS ToDATE,FULLNAME,userid,sum(submitted) Submitted,sum(delivered) Delivered,sum(Failed) Failed, sum(Unknown) Unknown," +
                   "COMPNAME,'" + senderId + "' AS SenderId,'" + TemplateID + "' AS TemplateID " +
                     //"smsrate.rate_normalsms AS deliveredRate,(smsrate.rate_normalsms * SUM(delivered)) AS deliveredCharge,smsrate.dltcharge AS dltrate,(smsrate.dltcharge * SUM(submitted)) AS dltCharge,COMPNAME " +
                     "FROM CUSTOMER Cus WITH(NOLOCK) " +
                     "INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' ";
            // "INNER JOIN smsrateaspercountry smsrate WITH(NOLOCK) ON Cus.username=smsrate.username and smsrate.countrycode='91'";
            if (TemplateID != "" && TemplateID != "0")
            {
                sql = sql + " INNER JOIN TemplateID TempID WITH(NOLOCK) ON TempID.senderid=DS.SENDERID ";
            }
            sql = sql + "WHERE smsdate >= '" + f + "' and smsdate <='" + t + @"'";
            if (DLTNO != "")
            {
                sql = sql + " and DLTNO IN(" + DLTNO + ")";
            }
            if (senderId != "" && senderId != "0")
            {
                sql = sql + " and DS.senderid = '" + senderId + "' ";
            }
            if (TemplateID != "" && TemplateID != "0")
            {
                sql = sql + " and TempID.templateID = '" + TemplateID + "' ";
            }
            sql = sql + " group by FULLNAME,userid,COMPNAME ORDER BY 1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetMessageIdBaseSearch(string MessagesId)
        {
            string sql = "";
            sql = @"SELECT SMPPACCOUNTID,case when SMPPACCOUNTID = '0' then '0' else LEFT(SMPPACCOUNTID, LEN(SMPPACCOUNTID) - 2) end AS SMPPACCOUNTID,
                    (CASE WHEN ISNULL(s.msgid2,'')<>'' then s.msgid2 ELSE s.MSGID END) AS MSGID,c.username AS ProfileID,c.COMPNAME AS ClientName,s.SENDERID,
                    s.MSGTEXT,s.CREATEDAT,s.SENTDATETIME,s.templateid,(CASE WHEN s.FILEID='1' THEN 'API' ELSE 'Pannel' END) AS SubmissionType,d.DLVRSTATUS AS Status,
                    d.donedate AS DLRTime,d.err_code,ec.descr,s.smstext FROM CUSTOMER c WITH(NOLOCK)
                    INNER JOIN MSGSUBMITTED s WITH(NOLOCK) on c.username = s.profileid
                    LEFT JOIN  delivery d WITH(NOLOCK) on s.msgid = d.msgid
                    LEFT JOIN ERRORCODE ec WITH(NOLOCK) on d.err_code=ec.code
                    WHERE s.MSGID='" + MessagesId + "'" +
                    "UNION ALL " +
                    "SELECT SMPPACCOUNTID,case when SMPPACCOUNTID = '0' then '0' else LEFT(SMPPACCOUNTID, LEN(SMPPACCOUNTID) - 2) end AS SMPPACCOUNTID," +
                    "(CASE WHEN ISNULL(s.msgid2,'')<>'' then s.msgid2 ELSE s.MSGID END) AS MSGID,c.username AS ProfileID,c.COMPNAME AS ClientName,s.SENDERID," +
                    "s.MSGTEXT,s.CREATEDAT,s.SENTDATETIME," +
                    "s.templateid,(CASE WHEN s.FILEID='1' THEN 'API' ELSE 'Pannel' END) AS SubmissionType,d.DLVRSTATUS AS Status,d.donedate AS DLRTime,d.err_code," +
                    "ec.descr,s.smstext FROM CUSTOMER c WITH(NOLOCK) " +
                    "INNER JOIN MSGSUBMITTEDLOG s WITH(NOLOCK) on c.username = s.profileid " +
                    "LEFT JOIN  DELIVERYLOG d WITH(NOLOCK) on s.msgid = d.msgid " +
                    "LEFT JOIN ERRORCODE ec WITH(NOLOCK) on d.err_code=ec.code " +
                    "WHERE s.MSGID='" + MessagesId + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDayWiseMonthlySummary(string f, string t, string to = "", string dltNo = "", int action = 1)
        {
            string sql = "";
            if (action == 1)
            {
                sql = @"SELECT '" + f + "' AS FromDATE,'" + to + "' AS ToDATE,c.COMPNAME,c.FULLNAME,M.PROFILEID AS userid,COUNT(M.ID) AS SUBMITTED," +
                    "SUM(case when isnull(d.dlvrstatus,'')<>'Delivered' then 0 else 1 end) AS DELIVERED," +
                    "SUM(case when isnull(d.dlvrstatus,'')<>'Delivered' then 1 else 0 end) AS FAILED " +
                    "FROM MSGSUBMITTEDLOG M WITH (NOLOCK) " +
                    "INNER JOIN customer c on m.PROFILEID=c.username " +
                    "LEFT JOIN DELIVERYLOG d with(nolock) on M.msgid = d.msgid WHERE M.SENTDATETIME between '" + f + "' and '" + t + @"' AND 
                     C.DLTNO IN ('1201159142280175288','Honda','HONDA_CRM_ACCOUNTS') GROUP BY c.FULLNAME,M.PROFILEID,c.COMPNAME
                     ORDER BY M.PROFILEID";
            }
            else
            {
                sql = @"SELECT '" + f + "' AS FromDATE,'" + to + "' AS ToDATE,FULLNAME,COMPNAME,userid,sum(submitted) Submitted,sum(delivered) Delivered,sum(Failed) Failed, sum(Unknown) Unknown,COMPNAME " +
                    "FROM CUSTOMER Cus WITH(NOLOCK) " +
                    "INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid WHERE smsdate between '" + f + "' and '" + t + @"' ";
                if (dltNo != "")
                {
                    sql = sql + " and DLTNO ='" + dltNo + "' ";
                }
                sql = sql + " group by FULLNAME,userid,COMPNAME ORDER BY userid";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public Int64 GetNoOfMobilesForFileID(string user, string FileId)
        {
            string sql = @"SELECT count(*) FROM msgschedule WITH(NOLOCK) WHERE profileid='" + user + "' and FILEID='" + FileId + "' and picked_datetime is null";
            return Convert.ToInt64(dbmain.GetScalarValue(sql));
        }

        public string UpdateAndGetBalanceSchedule(string UserID, string smstype, Int32 cnt, double rate, string campname)
        {
            string CreditBalScheduleCancellation = System.Configuration.ConfigurationManager.AppSettings["CreditBalScheduleCancellation"].ToString();
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer with(nolock) where username='" + UserID + "'"));

            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble(cnt * (rate * 10));
            bal = Math.Round((bal / 1000), 3);

            double addbal = (-1) * (Convert.ToDouble((cnt * rate) / 100));

            database.ExecuteNonQuery("update customer set balance = '" + bal + "' where username = '" + UserID + "'; " +
                "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                "VALUES('" + UserID + "','C','" + addbal + "',GETDATE(),'" + UserID + "','" + CreditBalScheduleCancellation.Replace("{1}", campname).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')");
            return bal.ToString();
        }

        public void RemoveFromSchedule(string user, string schdt)
        {
            //string sql = @" declare @FileProcessId int=0
            //set @FileProcessId=(select top 1 FileProcessId from SMSFILEUPLOAD where ID='" + fileid + @"')

            //update FileProcess set scheduleDeletedTime =GETDATE() where id=@FileProcessId

            //delete from msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + "') and picked_datetime is null and isnull(fileid,0) = '" + fileid + "'";
            //dbmain.ExecuteNonQuery(sql);

            string sql = @"update FileProcess set scheduleDeletedTime =GETDATE() where scheduletime=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + @"') 
            INSERT INTO EMIMPANEL..msgscheduleLog (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,PICKED_DATETIME,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,
            SMSTYPE,domain,FILEID,peid,templateid,datacode,blacklist,msgid,blockno,blockfail,ERR_CODE,TranTableName,DNDFILTER,scratchurl,offercode,scratchClient,LogDatetime) 
            SELECT ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SCHEDULE,PICKED_DATETIME,MOBTRK,SHORTURL,URLID,NEWSEGMENT,SMSRATE,
            SMSTYPE,domain,FILEID,peid,templateid,datacode,blacklist,msgid,blockno,blockfail,ERR_CODE,TranTableName,DNDFILTER,scratchurl,offercode,scratchClient,GETDATE() 
            FROM msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + "') and picked_datetime is null ; " +
            " DELETE FROM msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + "') and picked_datetime is null;";
            dbmain.ExecuteNonQuery(sql);
        }

        public DataTable GetScheduleLogReportFileId(string user, string schdt)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select MAX(m.smsrate) AS smsrate,
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 160 then 1 else
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 306 then 2 else
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 459 then 3 else
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 612 then 4 else
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 765 then 5 else
            case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 918 then 6 else 7 end end end end end end 
            as noofsms from msgschedule m with (nolock) 
            left join SMSFILEUPLOAD u with (nolock) on u.id=m.FILEID and u.USERID=m.PROFILEID
            left join FileProcess f with (nolock) on u.fileprocessid=f.id
            where m.profileid='" + user + @"'
            and m.schedule=DateAdd(MINUTE, " + Math.Abs(Global.addMinutes) + " ,'" + schdt + "') and picked_datetime is null " +
            "group by len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~','')))";
            dt = dbmain.GetDataTable(sql);
            return dt;
        }

        public DataTable GetScheduleLogsData(string user, string from, string to)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetScheduleLogsData";
                cmd.Parameters.AddWithValue("user", user);
                cmd.Parameters.AddWithValue("f", from);
                cmd.Parameters.AddWithValue("t", to);
                cmd.Parameters.AddWithValue("addMinutes", Global.addMinutes);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable GetErrorCode(string UserType, string DltNo, string USERID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_ErrorCode";
                cmd.Parameters.AddWithValue("@UserType", UserType);
                cmd.Parameters.AddWithValue("@DltNo", DltNo);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetLoginDetails(string UserName, string Password)
        {
            DataTable dt = new DataTable();
            string userid = Convert.ToString(database.GetScalarValue(@"SELECT Username from CUSTOMER with(nolock) where Username='" + UserName + "'"));
            if (userid != "")
            {
                bool IsValid = MIM.isPwdMatched("customer", "username", UserName.Trim(), "PWD", Password.Trim(), database.GetConnectstring());
                if (IsValid == true)
                {
                    dt = MIM.GetUserParameter("customer", "username", UserName.Trim(), "PWD", Password.Trim(), database.GetConnectstring());
                }
            }
            else
            {
                dt = null;
            }
            return dt;
        }

        //Add By Vikas ON 09-08-2023 Check Last Insert File Process Table
        public string CheckLastProcessTime(string UserID, string filenm = "")
        {
            string LastInsertTime = Convert.ToString(database.GetScalarValue("SELECT TOP 1 InsertTime FROM FileProcess WITH(NOLOCK) WHERE profileid='" + UserID + "' ORDER BY InsertTime DESC"));
            if (LastInsertTime != "")
            {
                string LastFileName = "";
                DateTime lastInsertTime = Convert.ToDateTime(LastInsertTime);
                DateTime currentTime = DateTime.Now;
                TimeSpan timeSinceLastInsert = currentTime - lastInsertTime;
                if (filenm != "")
                {
                    LastFileName = Convert.ToString(database.GetScalarValue("SELECT TOP 1 fileName FROM FileProcess WITH(NOLOCK) WHERE profileid='" + UserID + "' AND fileName='" + filenm.Trim() + "' ORDER BY InsertTime DESC"));
                }
                if (LastFileName != "")
                {
                    if (timeSinceLastInsert.TotalSeconds < Convert.ToInt32(WithLastFileName))
                    {
                        return "FileName";
                    }
                }
                else
                {
                    if (timeSinceLastInsert.TotalSeconds < Convert.ToInt32(WithOutLastFileName))
                    {
                        return "TimeDiff";
                    }
                }
            }
            return "Ok";
        }

        public DataTable GetCRDRLog(string f, string t, string user, string usertype, string dlt, string empcode, int IntoCreditDebit = 0, string userfilter = "")
        {
            string t1 = t;
            t = t + " 23:59:59";
            string sql = @"select row_number() over (Order by c.userName) as Sln,c.username, c.SenderID, c.compname, c.email, c.mobile1,c.balance, 
            sum(cr) as cr,sum(dr) as dr,'" + f + "' + ' to ' + '" + t + @"' as tdate, '" + f + "' as frmdate, '" + t + @"' as todate FROM
            (
            SELECT d.username, sum(case when d.trantype='C' then d.balance else 0 end) as cr,sum(case when d.trantype='D' then d.balance else 0 end) AS dr,
            max(convert(varchar,d.trandate,102)) AS tdate
            FROM userBalCrDr d WITH(NOLOCK) WHERE d.trandate between '" + f + "' AND '" + t + @"'";
            if (IntoCreditDebit == 0) sql = sql + " AND REMARKS NOT LIKE 'Being balance debited towards Campaign ID -%' AND REMARKS NOT LIKE 'Being balance debited towards SMS sent through API on%' " +
                       " AND REMARKS NOT LIKE 'Being balance debited towards SMS Campaign -%' AND REMARKS NOT LIKE 'Being balance credited towards Cancellation of SMS Campaign -%' ";
            sql = sql + " GROUP BY d.username )AS x INNER JOIN customer c WITH(NOLOCK) ON x.username=c.username ";
            if (usertype == "USER") sql = sql + " AND c.USERNAME = '" + user + "' ";
            if (usertype == "ADMIN")
            {
                sql = sql + " AND c.dltno = '" + dlt + "'";
                if (userfilter != "")
                {
                    sql = sql + " AND c.USERNAME = '" + userfilter + "'";
                }
            }
            if (usertype == "SYSADMIN" && userfilter != "") sql = sql + " AND c.USERNAME = '" + userfilter + "' ";
            if (usertype == "BD")
            {
                sql = sql + " AND c.empcode='" + empcode + "' ";
            }
            sql = sql + " GROUP BY c.username,c.SenderID, c.compname, c.email, c.mobile1,c.balance,tdate ORDER BY tdate,c.username";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogDetail(string f, string t, string user, int IntoCreditDebit = 0)
        {
            string sql = @"SELECT CASE WHEN trantype='C' THEN 'Credit' ELSE 'Debit' END AS TranType, balance as amount, convert(varchar,trandate,106) + ' ' + convert(varchar(5),trandate,108) as trandate, trandate as trandate1,ISNULL(Remarks,'') Remarks
            FROM userBalCrDr WITH(NOLOCK) WHERE username = '" + user + "' AND trandate between '" + f + "' and '" + t + @"' ";
            if (IntoCreditDebit == 0) sql = sql + " AND REMARKS NOT LIKE 'Being balance debited towards Campaign ID -%' AND REMARKS NOT LIKE 'Being balance debited towards SMS sent through API on%' " +
                       " AND REMARKS NOT LIKE 'Being balance debited towards SMS Campaign -%' AND REMARKS NOT LIKE 'Being balance credited towards Cancellation of SMS Campaign -%' ";
            sql = sql + " order by trandate1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogNew(string f, string t, string user, string usertype, string dlt, string sms, string url, string clk, string all, string currency, int IntoCreditDebit = 0)
        {
            if (all == "Y")
            {
                sms = "Y"; url = "Y"; clk = "Y";
            }
            string t1 = t;
            t = t + " 23:59:59";
            string sql = "";

            if (f == "02/JAN/1900") sql = "Select '' as product, 0 as cr, 0 as dr,'' as fromdate,'' as todate,'' as remarks ";
            if (sms == "Y")
            {
                sql = @"select 'SMS' AS PRODUCT,'" + currency + "'+ convert(varchar,sum(cr)) as cr,'" + currency + "'+ convert(varchar,sum(dr)) as dr,'" + f + "' as fromdate, '" + t1 + @"' as todate,x.remarks from
                (
                select d.username, sum(case when d.trantype='C' then d.balance else 0 end) as cr,sum(case when d.trantype='D' then d.balance else 0 end) as dr,
                max(convert(varchar,d.trandate,102)) as tdate,isnull(REMARKS,'') as REMARKS
                from userBalCrDr d where d.trandate between '" + f + "' and '" + t + @"' AND CLICKRECHARGE=0";
                if (IntoCreditDebit == 0) sql = sql + " AND REMARKS NOT LIKE 'Being balance debited towards Campaign ID -%' AND REMARKS NOT LIKE 'Being balance debited towards SMS sent through API on%' " +
                       " AND REMARKS NOT LIKE 'Being balance debited towards SMS Campaign -%' AND REMARKS NOT LIKE 'Being balance credited towards Cancellation of SMS Campaign -%' ";
                sql = sql + " group by d.username,REMARKS )as x inner join customer c on x.username=c.username WHERE 1=1 ";
                if (usertype == "USER") sql = sql + " and c.USERNAME = '" + user + "' ";
                if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dlt + "' ";
                sql = sql + " group by x.remarks ";
            }

            if (url == "Y")
            {
                if (sms == "Y") sql = sql + " Union All ";
                sql = sql + @"select 'URL' AS PRODUCT,'" + currency + "'+ as cr,'" + currency + "'+ + convert(varchar,count(x.ID) * c.urlrate) as dr,'" + f + "' as fromdate, '" + t1 + @"' as todate,'' as remarks from
                (
                select D.USERID,D.ID FROM SHORT_UrLS d where d.ADDED between '" + f + "' and '" + t + @"' 
                group by D.USERID,D.ID
                )as x inner join customer c on x.USERID=c.username WHERE 1=1 ";
                if (usertype == "USER") sql = sql + " and c.USERNAME = '" + user + "' ";
                if (usertype == "ADMIN") sql = sql + " and c.dltno = '" + dlt + "' ";
                sql = sql + " group by  c.urlrate ";
            }

            if (clk == "Y")
            {
                string User_AgentAutoClick = Convert.ToString(database.GetScalarValue("SELECT TOP 1 userid FROM showClickFromBot WITH(NOLOCK) WHERE userid='" + user + "'"));
                if (sms == "Y" || url == "Y") sql = sql + " Union All ";

                sql = sql + @"select 'Click' as Product,Convert(varchar, 1000 * (select count(*) from short_urls where userid = '" + user + @"' and added between '" + f + "' and '" + t + @"')) cr, convert(varchar,sum (cnt)) as dr, '" + f + "' as fromdate, '" + t1 + @"' as todate,'' as remarks from (
                select count(*) as cnt from short_urls u inner join stats s on u.id = s.shortUrl_id where s.click_date between '" + f + "' and '" + t + @"' 
                and u.userid = '" + user + @"' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @"
                union all
                select count(*) as cnt from short_urls u inner join mobstats s on u.id = s.shortUrl_id where s.click_date between '" + f + @"' and '" + t + @"' 
                and u.userid = '" + user + @"' " + (string.IsNullOrEmpty(User_AgentAutoClick) ? " AND S.User_AgentAutoClick = 0 " : "") + @" ) x ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCRDRLogNewDETAIL(string user, string usertype, string f, string t, string prod, int IntoCreditDebit = 0)
        {
            //string sql = @"SELECT USERNAME,TRANTYPE,ROUND(SUM(BALANCE), 2) AS AMOUNT, TDATE, REMARKS FROM
            //(
            //select d.username, d.trantype, d.balance,
            //convert(varchar, d.trandate, 102) as tdate, isnull(REMARKS, '') as REMARKS
            //from userBalCrDr d where username = '" + user + @"' and d.trandate between '" + f + "' and '" + t + @"' AND CLICKRECHARGE = 0
            //union all
            //select userid as username, 'D' as trantype, SUM(SUBMITTED * (rate / 100)) as balance, convert(varchar, SMSDATE, 102) as tdate, '' as remarks from DAYSUMMARY
            //where userid = '" + user + @"' and SMSDATE between '" + f + "' and '" + t + @"'
            //GROUP BY userid, SMSDATE
            //UNION ALL
            //SELECT S.USERID AS USERNAME, 'D' AS TRANTYPE, C.URLRATE * COUNT(S.ID)AS BALANCE, convert(varchar, S.ADDED, 102) as tdate, '' as remarks from
            //    short_urls S INNER JOIN CUSTOMER C ON S.USERID = C.USERNAME WHERE userid = '" + user + @"' AND S.ADDED  between '" + f + "' and '" + t + @"'
            //GROUP BY S.USERID, convert(varchar, S.ADDED, 102), C.urlrate
            //) Y GROUP BY USERNAME, TRANTYPE, TDATE, REMARKS
            //order by tdate";
            string sql = "";
            if (prod == "SMS")
            {
                sql = @"SELECT USERNAME,TRANTYPE,ROUND(SUM(BALANCE), 2) AS AMOUNT, TDATE, REMARKS FROM
            (
            select d.username, d.trantype,case WHEN d.trantype='C' THEN sum(case when d.trantype='C' then d.balance else 0 end)
            WHEN d.trantype='D' THEN  sum(case when d.trantype='D' then d.balance else 0 end) END AS BALANCE,
            convert(varchar, d.trandate, 102) as tdate, isnull(REMARKS, '') as REMARKS
            from userBalCrDr d where username = '" + user + @"' and d.trandate between '" + f + "' and '" + t + @"' AND CLICKRECHARGE = 0 ";
                if (IntoCreditDebit == 0) sql = sql + " AND REMARKS NOT LIKE 'Being balance debited towards Campaign ID -%' AND REMARKS NOT LIKE 'Being balance debited towards SMS sent through API on%' " +
                           " AND REMARKS NOT LIKE 'Being balance debited towards SMS Campaign -%' AND REMARKS NOT LIKE 'Being balance credited towards Cancellation of SMS Campaign -%' ";
                sql = sql + " GROUP By d.username,d.trantype,convert(varchar, d.trandate, 102),REMARKS ) Y GROUP BY USERNAME, TRANTYPE, TDATE, REMARKS order by tdate desc";
            }
            else
            {
                sql = @"SELECT USERNAME,TRANTYPE,ROUND(SUM(BALANCE), 2) AS AMOUNT, TDATE, REMARKS FROM
            (
            select d.username, d.trantype,case WHEN d.trantype='C' THEN sum(case when d.trantype='C' then d.balance else 0 end)
            WHEN d.trantype='D' THEN  sum(case when d.trantype='D' then d.balance else 0 end) END AS BALANCE,
            convert(varchar, d.trandate, 102) as tdate, isnull(REMARKS, '') as REMARKS
            from userBalCrDr d where username = '" + user + @"' and d.trandate between '" + f + "' and '" + t + @"' AND CLICKRECHARGE = 1 ";
                if (IntoCreditDebit == 0) sql = sql + " AND REMARKS NOT LIKE 'Being balance debited towards Campaign ID -%' AND REMARKS NOT LIKE 'Being balance debited towards SMS sent through API on%' " +
                           " AND REMARKS NOT LIKE 'Being balance debited towards SMS Campaign -%' AND REMARKS NOT LIKE 'Being balance credited towards Cancellation of SMS Campaign -%' ";
                sql = sql + "GROUP By d.username,d.trantype,convert(varchar, d.trandate, 102),REMARKS ) Y GROUP BY USERNAME, TRANTYPE, TDATE, REMARKS order by tdate desc";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable ShowGird(string userid, string fromdate, string todate, string DistinctCount, int Action = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandTimeout = 3600;
                    SqlDataAdapter sd = new SqlDataAdapter();
                    sd.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_GetDeliveredCount";
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);
                    cmd.Parameters.AddWithValue("@DistinctCount", DistinctCount);
                    cmd.Parameters.AddWithValue("@Action", Action);
                    sd.Fill(dt);
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProvider(string MsterType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    SqlDataAdapter sd = new SqlDataAdapter();
                    sd.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_GetProvider";
                    cmd.Parameters.AddWithValue("@MsterType", MsterType);
                    sd.Fill(dt);
                    cnn.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public string PWDCHECK(string UserName, string Password)
        {
            bool IsValid = MIM.isPwdMatched("customer", "username", UserName.Trim(), "PWD", Password.Trim(), database.GetConnectstring());
            //string sql = "if exists(select * from customer where Active ='1' and Username COLLATE Latin1_General_CS_AS='" + UserName + "' and PWD COLLATE Latin1_General_CS_AS='" + Password + "') Select 'True'";
            //string IsValid = Convert.ToString(database.GetScalarValue(sql));
            return Convert.ToString(IsValid);
        }

        public string INSERTUAEAPIAccounts(string UserId, string senderid, string provider, bool Active, string MasterType)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_INSERTUAEAPIAccounts";
                cmd.Parameters.AddWithValue("@userid", UserId);
                cmd.Parameters.AddWithValue("@senderId", senderid);
                cmd.Parameters.AddWithValue("@provider", provider);
                cmd.Parameters.AddWithValue("@active", Active);
                cmd.Parameters.AddWithValue("@MasterType", MasterType);
                cmd.Parameters.AddWithValue("@Msg", "");
                cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@Msg"].Size = 56;
                cmd.ExecuteNonQuery();
                Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                cnn.Close();
            }
            return Msg;
        }

        public string INSERTUAEAPIAccountslog(string UserId, string senderid, string insertDatetime, string provider, bool Active, string MasterType)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_INSERTUAEAPIAccountslog";
                cmd.Parameters.AddWithValue("@userid", UserId);
                cmd.Parameters.AddWithValue("@senderId", senderid);
                cmd.Parameters.AddWithValue("@insertDatetime", insertDatetime);
                cmd.Parameters.AddWithValue("@provider", provider);
                cmd.Parameters.AddWithValue("@active", Active);
                cmd.Parameters.AddWithValue("@MasterType", MasterType);
                cmd.Parameters.AddWithValue("@Msg", "");
                cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@Msg"].Size = 56;
                cmd.ExecuteNonQuery();
                Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                cnn.Close();
            }
            return Msg;
        }

        public string Insertsmppaccountuserid(string UserId, string smppaccountid, string countrycode, bool Active, string ManualAcid)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Insertsmppaccountuserid";
                cmd.Parameters.AddWithValue("@userid", UserId);
                cmd.Parameters.AddWithValue("@smppaccountid", smppaccountid);
                cmd.Parameters.AddWithValue("@countrycode", countrycode);
                cmd.Parameters.AddWithValue("@active", Active);
                cmd.Parameters.AddWithValue("@ManualAcid", ManualAcid);
                cmd.Parameters.AddWithValue("@Msg", "");
                cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@Msg"].Size = 56;
                cmd.ExecuteNonQuery();
                Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                cnn.Close();
            }
            return Msg;
        }

        public string Insertsmppaccountuseridlog(string UserId, string smppaccountid, string countrycode, bool Active, string ManualAcid)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Insertsmppaccountuseridlog";
                cmd.Parameters.AddWithValue("@userid", UserId);
                cmd.Parameters.AddWithValue("@smppaccountid", smppaccountid);
                cmd.Parameters.AddWithValue("@countrycode", countrycode);
                cmd.Parameters.AddWithValue("@active", Active);
                cmd.Parameters.AddWithValue("@ManualAcid", ManualAcid);
                cmd.Parameters.AddWithValue("@Msg", "");
                cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@Msg"].Size = 56;
                cmd.ExecuteNonQuery();
                Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                cnn.Close();
            }
            return Msg;
        }

        public string CountryCode(string userid)
        {
            string sql = "select defaultCountry from customer sm with(nolock) where username ='" + userid + "'";
            string CountryCode = Convert.ToString(database.GetScalarValue(sql));
            return CountryCode;
        }

        public DataTable GetAPITran()
        {
            string sql = "select * from UAEAPIAccounts sm with(nolock)";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetAPIPromo()
        {
            string sql = "select * from UAEAPIACCOUNTPROMO sm with(nolock)";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetPaneldata()
        {
            string sql = @"select sm.userid, sm.smppaccountid, st.PROVIDER+'--'+st.SYSTEMID+'--'+Convert(varchar(20), st.smppaccountid) as Provider,
            sm.CountryCode,st1.PROVIDER+'--'+st1.SYSTEMID+'--'+Convert(varchar(20), st1.smppaccountid) as ManualAcId, sm.ManualAcId as ManualAcId_id, sm.ACTIVE 
            from smppaccountuserid sm with(nolock)
            inner join SMPPSETTING st on Convert(varchar(50),st.smppaccountid) = Convert(varchar(50),sm.smppaccountid) 
            left join SMPPSETTING st1 on Convert(varchar(50),st1.smppaccountid) = Convert(varchar(50),sm.ManualAcId) and st1.ACTIVE=1
            where st.ACTIVE=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void deleteUAEAPIAccountsTRAN(string userid, string insertdate, string provider)
        {
            string sql = "delete from UAEAPIAccounts where userid='" + userid + "' and Convert(varchar(50),insertdate,120)='" + insertdate + "'and ACCOUNT='" + provider + "'";
            database.ExecuteNonQuery(sql);
        }

        public void deleteUAEAPIAccountsPROMO(string userid, string insertdate, string provider)
        {
            string sql = "delete from UAEAPIACCOUNTPROMO where userid='" + userid + "' and Convert(varchar(50),insertdate,120)='" + insertdate + "'and ACCOUNT='" + provider + "'";
            database.ExecuteNonQuery(sql);
        }

        public void deletePanelData(string userid, string smppaccountid, string countrycode, string ManualAcId)
        {
            string sql = "delete from smppaccountuserid where userid='" + userid + "' and smppaccountid='" + smppaccountid + "'and countrycode='" + countrycode + "' and ManualAcId='" + ManualAcId + "'";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetSystemId()
        {
            string sql = "select distinct SystemId, smppaccountid from smppsetting where Active = 1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetOperatorSenderId()
        {
            string sql = @"select Provider,SystemId,count(SID) as SID from OperatorSenderId 
                           where Active = 1 group by Provider, SystemId ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string InsertOperatorSenderId(string Provider, string SystemId, string SenderId)
        {
            string Msg = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertOperatorSenderId";
                    cmd.Parameters.AddWithValue("@Provider", Provider);
                    cmd.Parameters.AddWithValue("@SystemId", SystemId);
                    cmd.Parameters.AddWithValue("@SenderId", SenderId);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Msg"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Msg;
        }

        public DataTable GetOperatorSenderID(string Provider)
        {
            string sql = @"select * from OperatorSenderId where Provider='" + Provider + "'" +
                " order by SID Asc";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetOperatorSenderIDFilter(string Provider, string Senderid)
        {
            string sql = @"select * from OperatorSenderId where Provider='" + Provider + "' " +
                "and SID Like '%" + Senderid + "%' order by SID Asc";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public void Inactive(string Id)
        {
            string sql = "update OperatorSenderId set Active=0 where Id = '" + Id + "'";
            database.ExecuteNonQuery(sql);
        }

        public string CheckActive(string Id)
        {
            string sql = "Select Active from OperatorSenderId where Id = '" + Id + "'";
            string Active = Convert.ToString(database.GetScalarValue(sql));
            return Active;
        }

        public void Active(string Id)
        {
            string sql = "update OperatorSenderId set Active=1 where Id = '" + Id + "'";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetGroupOnDLTNO(string FromMonth, string ToMonth, string Year, string UserId, string DltNo, int DNDorNot = 0)
        {
            string sql = "", sql1 = "", sql3 = "";
            sql = "Create table #tab (ProfileID VARCHAR(50),COMPNAME VARCHAR(200),defaultcountry varchar(10),SUBMITTED numeric(18,0),DELIVERED numeric(18,0),FAILED numeric(18,0),DND VARCHAR(50) )";
            //sql += @"INSERT INTO #tab(ProfileID,COMPNAME,defaultcountry,SUBMITTED,DELIVERED,FAILED,DND)
            //      select c.DLTNO ProfileID,c.DLTNO COMPNAME,'' defaultCountry,Isnull(sum(SUBMITTED),0) as SUBMITTED ,
            //      sum(DELIVERED + UNKNOWN) as DELIVERED,Isnull(sum(FAILED),0) as FAILED,'' as DND from DAYSUMMARY D with(nolock) 
            //      inner join CUSTOMER C on D.USERID=C.username
            //      where DATENAME(MONTH,SMSDATE) between '" + FromMonth + "' and '" + ToMonth + @"' and c.usertype='user' ";

            sql += @"INSERT INTO #tab(ProfileID,COMPNAME,defaultcountry,SUBMITTED,DELIVERED,FAILED,DND)
                  select D.USERID as ProfileID,COMPNAME,defaultCountry,Isnull(sum(SUBMITTED),0) as SUBMITTED ,
                  sum(DELIVERED + UNKNOWN) as DELIVERED,Isnull(sum(FAILED),0) as FAILED,'' as DND from DAYSUMMARY D with(nolock) 
                  inner join CUSTOMER C on D.USERID=C.username
                  where DATENAME(MONTH,SMSDATE) between '" + FromMonth + "' and '" + ToMonth + @"' and c.usertype='user' ";
            if (Year != "0")
            {
                sql = sql + " and year(D.SMSDATE)='" + Year + "'";
            }
            if (UserId != "")
            {
                sql = sql + " and d.userid='" + UserId + "'";
            }
            if (DltNo != "")
            {
                sql = sql + " and c.DLTNO='" + DltNo + "'";
            }
            sql = sql + " group by DLTNO,defaultCountry,D.USERID,COMPNAME having COUNT(c.DLTNO)=1 Union all ";

            sql += @"select c.DLTNO ProfileID,c.DLTNO COMPNAME,defaultCountry,Isnull(sum(SUBMITTED),0) as SUBMITTED ,
                  sum(DELIVERED + UNKNOWN) as DELIVERED,Isnull(sum(FAILED),0) as FAILED,'' as DND from DAYSUMMARY D with(nolock) 
                  inner join CUSTOMER C on D.USERID=C.username
                  where DATENAME(MONTH,SMSDATE) between '" + FromMonth + "' and '" + ToMonth + @"' and c.usertype='user' ";
            if (Year != "0")
            {
                sql = sql + " and year(D.SMSDATE)='" + Year + "'";
            }
            if (UserId != "")
            {
                sql = sql + " and d.userid='" + UserId + "'";
            }
            if (DltNo != "")
            {
                sql = sql + " and c.DLTNO='" + DltNo + "'";
            }
            sql = sql + " group by DLTNO,defaultCountry having COUNT(c.DLTNO)>1";

            if (DNDorNot == 1)
            {
                sql1 = @" update #tab set DND=P.DND FROM 
                        (select ProfileID,c.DLTNO as DLTNO,COUNT(*) as DND from customer C with(nolock) 
                        inner join MSGSUBMITTEDLOG M with(nolock) on c.username=M.PROFILEID 
                        left join DELIVERYLOG D with(nolock) on M.MSGID=D.MSGID
                        where d.err_code='103' and DATENAME(MONTH,SENTDATETIME) between '" + FromMonth + "' and '" + ToMonth + @"' and c.usertype='user' ";
                if (Year != "0")
                {
                    sql1 = sql1 + " and year(M.SENTDATETIME)='" + Year + "'";
                }
                if (UserId != "")
                {
                    sql1 = sql1 + " and m.PROFILEID='" + UserId + "'";
                }
                if (DltNo != "")
                {
                    sql1 = sql1 + " and c.DLTNO='" + DltNo + "'";
                }
                sql1 = sql1 + " group by c.DLTNO,ProfileID)P JOIN #TAB t1 on t1.ProfileID=P.ProfileID OR t1.ProfileID=P.DLTNO collate SQL_Latin1_General_CP1_CI_AS";
            }

            sql3 = sql + sql1 + " select * from #tab " +
                "drop table #tab";
            DataTable dt = database.GetDataTable(sql3);
            return dt;
        }

        public string InsertMiMReportGroup(string Client, string Serverip, string userid = "", string MasterType = "")
        {
            string Msg = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertMiMReportGroup";
                    cmd.Parameters.AddWithValue("@Client", Client);
                    cmd.Parameters.AddWithValue("@serverip", Serverip);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@MasterType", MasterType);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Msg"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Msg;
        }

        public DataTable GetMiMreportGroup()
        {
            string sql = @"select m.Client, s.ServerName as Server, sum(case when isnull(userid,'')='' then 0 else 1 end) as NoAcc from MIMREPORTGROUP m 
            inner join Server s on s.Serverip = m.serverip group by m.Client, s.ServerName";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable getMiMReportGroup()
        {
            string sql = "select distinct(client) from MIMREPORTGROUP where serverip ='10.10.33.252'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string getMiMReportGroupBind(string userid)
        {
            string sql = "select distinct(client) from MIMREPORTGROUP where serverip ='10.10.33.252' and userid ='" + userid + "'";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string GetLastLoginTime(string user)
        {
            string timespam = "";
            string sql = "SELECT top(1) LASTLOGINDATE FROM LoginEntry where UserId ='" + user + "' order by LASTLOGINDATE desc ";
            timespam = Convert.ToString(database.GetScalarValue(sql));
            return timespam;
        }

        public string GetLastLoginDetails(string user)
        {
            string timespam = "";
            string sql = "select LastActiveDate from InactiveAccounts where UserId ='" + user + "'";
            timespam = Convert.ToString(database.GetScalarValue(sql));
            return timespam;
        }

        public DataTable GetLastLogindt(string user)
        {
            string sql = @"select userid, MAX(SMSDATE) as SMSDATE from DAYSUMMARY ds WITH(NOLOCK) 
            inner join CUSTOMER c WITH(NOLOCK) on c.username = ds.USERID where ds.USERID = '" + user + "' group by userid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string SendEmaillogin(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCAddress)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                //message.CC.Add("dsng25@gmail.com");
                string[] p = CCAddress.Split(';');
                for (int i = 0; i < p.Length; i++)
                {
                    message.CC.Add(p[i]);
                }
                /*CCAddress*/
                // message.CC.Add("support@myinboxmedia.com");

                //Attachment item = new Attachment(fn);
                //message.Attachments.Add(item);
                //smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
                Log(message + "---" + toAddress + "--" + MailFrom + Pwd + Host + CCAddress);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace + "--" + toAddress + "--" + subject + body + MailFrom + Pwd + Host + CCAddress);
            }
            return result;
        }

        public void UpdateEmailSent(string UserId)
        {
            string sql = "Update LoginEntry set EmailSent = GETDATE() where UserId='" + UserId + "'";
            database.ExecuteNonQuery(sql);
        }

        public string GetEmailSent(string UserId)
        {
            string sql = "select top 1 EmailSent from LoginEntry where UserId='" + UserId + "' order by LastLoginDate desc ";
            string EmailSent = Convert.ToString(database.GetScalarValue(sql));
            return EmailSent;
        }

        #region Add Code by Vikas On 28-09-2023 New Report For Hero MotoCorp

        public DataTable GetEvents()
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT EventID,EventName FROM EventMast WITH(NOLOCK) WHERE Active=1 ORDER BY EventName";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCategory(string userId)
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT CategoryID,CategoryName FROM CategoryMast innner join HeroAccountMapping WITH(NOLOCK) on CategoryId=GroupLocationCode WHERE userId='" + userId + "' and  Active=1 ORDER BY CategoryName";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetLocation(string userId, string CategoryId = "0")
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT LocationID,LocationName FROM LocationMast innner join HeroAccountMapping WITH(NOLOCK) on CategoryId=GroupLocationCode WHERE userId='" + userId + "' and   Active=1 ";
            if (CategoryId != "0")
            {
                sql = sql + " AND CategoryID='" + CategoryId + "'";
            }
            sql = sql + " ORDER BY LocationName";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSubLocation(string userId, string CategoryId = "0", string LocationId = "0")
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT SubLocationID,SubLocationName FROM SubLocationMast  innner join HeroAccountMapping WITH(NOLOCK) on CategoryId=GroupLocationCode WHERE  userId='" + userId + "' and   Active=1  ";
            if (CategoryId != "0")
            {
                sql = sql + " AND CategoryID='" + CategoryId + "'";
            }
            if (LocationId != "0")
            {
                sql = sql + " AND LocationID='" + LocationId + "'";
            }
            sql = sql + " ORDER BY SubLocationName";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDealerCode(string userId, string CategoryId = "0", string LocationId = "0", string SubLocationId = "0")
        {
            DataTable dt = new DataTable("dt");
            string sql = @"SELECT DLRCODE,LEFT(DLRName+replicate('.',50),31) + left('{' +  ISNULL(DLRCODE,'') + replicate('.',50),21) + '}' AS DLRName FROM DealerMast  inner join HeroAccountMapping WITH(NOLOCK) on CategoryId=GroupLocationCode WHERE  userId='" + userId + "' and   Active=1  ";
            if (CategoryId != "0")
            {
                sql = sql + " AND CategoryID='" + CategoryId + "'";
            }
            if (LocationId != "0")
            {
                sql = sql + " AND LocationID='" + LocationId + "'";
            }
            if (SubLocationId != "0")
            {
                sql = sql + " AND SubLocationID='" + SubLocationId + "'";
            }
            sql = sql + " ORDER BY DLRName";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReportuserMotoCorp(string f, string t, string usertype, string user, string campnm = "", string mob = "", string TempIdAndName = "", string CategoryID = "0", string LocationID = "0", string SubLocationID = "0", string Dealer = "0")
        {
            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);

            string sql = "";
            sql = @"
                   SELECT FILEID,COUNT(*) AS SUBMITTED,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                   sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                   sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, max(s.smsTEXT) as msg,DLRName,CategoryName,
                   LocationName,SubLocationName,DM.DLRCODE INTO #T1
                   FROM MSGSUBMITTED s with(nolock) 
                   JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
                   left JOIN delivery d with(nolock) on s.msgid = d.msgid --and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
                   left JOIN DealerMast DM WITH(NOLOCK) ON s.DLRCode = DM.DLRCODE
                   LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                   LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                   LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                   WHERE S.PROFILEID  ='" + user + @"' AND s.sentdatetime between " + fDate + @" AND " + tDate + @" AND s.FILEID > 1 AND SS.campaignname<>'Manual'";
            if (mob != "") sql = sql + @" AND DM.TOMOBILE='" + mob + "' ";
            if (CategoryID != "0") sql = sql + @" AND DM.CategoryID='" + CategoryID + "' ";
            if (LocationID != "0") sql = sql + @" AND DM.LocationID='" + LocationID + "' ";
            if (SubLocationID != "0") sql = sql + @" AND DM.SubLocationID='" + SubLocationID + "' ";
            if (Dealer != "0") sql = sql + @" AND DM.DLRCODE='" + Dealer + "' ";

            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";

            sql = sql + @"group by S.FILEID,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE UNION ALL
                   SELECT FILEID,COUNT(*) AS SUBMITTED,sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                   sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                   sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, max(s.smsTEXT) as msg,DLRName,CategoryName,
                   LocationName,SubLocationName,DM.DLRCODE
                   FROM MSGSUBMITTEDLOG s with(nolock) 
                   JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
                   left JOIN deliveryLOG d with(nolock) on s.msgid = d.msgid --and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
                   left JOIN DealerMast DM WITH(NOLOCK) ON s.DLRCode = DM.DLRCODE
                   LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                   LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                   LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                   WHERE S.PROFILEID  ='" + user + @"' AND s.sentdatetime between " + fDate + @" AND " + tDate + @" AND s.FILEID > 1 AND SS.campaignname<>'Manual'";
            if (mob != "") sql = sql + @" AND DM.TOMOBILE='" + mob + "' ";
            if (CategoryID != "0") sql = sql + @" AND DM.CategoryID='" + CategoryID + "' ";
            if (LocationID != "0") sql = sql + @" AND DM.LocationID='" + LocationID + "' ";
            if (SubLocationID != "0") sql = sql + @" AND DM.SubLocationID='" + SubLocationID + "' ";
            if (Dealer != "0") sql = sql + @" AND DM.DLRCODE='" + Dealer + "' ";

            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";

            sql = sql + @"group by S.FILEID,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE
                   
                   select '2' as sr,'1' AS SL,convert(varchar,s.SENTDATETIME,23) as submitdate,'ENTRY' as reqsrc,'' as filenm,'' as sender,
                   count(s.id) submitted,
                   SUM(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                   SUM(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                   SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,0 as fileid,'" + user + @"' as userid,
                   DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE
                   FROM customer c with(nolock)
                   INNER JOIN MSGSUBMITTED s with(nolock) on c.username = s.profileid
                   LEFT JOIN delivery d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
                   LEFT JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID
                   LEFT JOIN DealerMast DM WITH(NOLOCK) ON s.DLRCode = DM.DLRCODE
                   LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                   LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                   LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                   /* inner join FileProcess fp with (nolock) ON fp.id = ss.FileProcessId */
                   WHERE c.username ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and SS.campaignname='Manual' AND isnull(SS.FILENM,'')='' ";
            if (f == @"02/JAN/1900") sql = sql + " and 1=0 ";
            if (campnm != "" && campnm != "0") sql = sql + @" and ss.campaignname = '" + campnm + "' "; //Add By Naved
            if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
            if (CategoryID != "0") sql = sql + @" AND DM.CategoryID='" + CategoryID + "' ";
            if (LocationID != "0") sql = sql + @" AND DM.LocationID='" + LocationID + "' ";
            if (SubLocationID != "0") sql = sql + @" AND DM.SubLocationID='" + SubLocationID + "' ";
            if (Dealer != "0") sql = sql + @" AND DM.DLRCODE='" + Dealer + "' ";
            //Add code by vikas at 07-03-2023
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";
            sql = sql + @" group by DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE,convert(varchar,s.SENTDATETIME,23) ";
            sql = sql + @"UNION ALL
                   select '2' as sr,'1' AS SL,convert(varchar,s.SENTDATETIME,23) as submitdate,'ENTRY' as reqsrc,'' as filenm,'' as sender,
                   count(s.id) submitted,
                   SUM(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                   SUM(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                   SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown, '' as msg,0 as fileid,'" + user + @"' as userid,
                   DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE
                   FROM customer c with(nolock)
                   INNER JOIN MSGSUBMITTEDLOG s with(nolock) on c.username = s.profileid
                   LEFT JOIN deliveryLOG d with(nolock) on s.msgid = d.msgid and convert(varchar,s.insertdate,102)=convert(varchar,d.insertdate,102)
                   LEFT JOIN SMSFILEUPLOAD SS with(nolock) on ss.id = s.FILEID AND s.profileid = ss.USERID 
                   LEFT JOIN DealerMast DM WITH(NOLOCK) ON s.DLRCode = DM.DLRCODE
                   LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                   LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                   LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                   WHERE c.username ='" + user + @"' and s.sentdatetime between " + fDate + @" and " + tDate + @" and SS.campaignname='Manual' AND isnull(SS.FILENM,'')='' ";
            if (f == @"02/JAN/1900") sql = sql + " and 1=0 ";
            if (campnm != "" && campnm != "0") sql = sql + @" and ss.campaignname = '" + campnm + "' ";
            if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
            if (CategoryID != "0") sql = sql + @" AND DM.CategoryID='" + CategoryID + "' ";
            if (LocationID != "0") sql = sql + @" AND DM.LocationID='" + LocationID + "' ";
            if (SubLocationID != "0") sql = sql + @" AND DM.SubLocationID='" + SubLocationID + "' ";
            if (Dealer != "0") sql = sql + @" AND DM.DLRCODE='" + Dealer + "' ";
            if (TempIdAndName != "") sql = sql + @" and s.templateid='" + TempIdAndName + "'";
            sql = sql + @" group by DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE,convert(varchar,s.SENTDATETIME,23) ";
            sql = sql + @" UNION ALL
                  select '2' as sr,'1' AS SL, Convert(varchar,DATEADD(MINUTE," + Global.addMinutes + @",
                  convert(varchar,f.UPLOADTIME,106) + ' ' + convert(varchar,f.UPLOADTIME,108)),120) as submitdate,'FILE' as reqsrc,f.FILENM as filenm,
                  f.senderid as sender,ISNULL(T.SUBMITTED,0) AS submitted,t.delivered , t.failed ,ISNULL(T.SUBMITTED,0) -  t.delivered - t.failed as unknown,";

            sql = sql + @" t.msg,F.ID, '" + user + @"' as userid,DLRName,CategoryName,LocationName,SubLocationName,t.DLRCODE from customer c with(nolock)
                   inner join FileProcess fp with (nolock) ON FP.profileid=C.USERNAME 
                   LEFT JOIN SMSFILEUPLOAD f on c.username = f.USERID AND  fp.id = f.FileProcessId
                   LEFT join #t1 t on t.fileid=f.id
                   where c.username  ='" + user + @"' AND f.campaignname <> 'Manual' and isnull(f.schedule,f.UPLOADTIME)
                   between " + fDate + @" and " + tDate + @" ";
            if (campnm != "" && campnm != "0") sql = sql + @" and f.campaignname = '" + campnm + "' ";
            sql = sql + @" order by sr,sl,submitdate";
            DataTable dt = database.GetDataTable(sql);

            var rows = dt.Select("submitted = 0");
            foreach (var row in rows)
                row.Delete();
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetSMSReportDetailUserMotoCorp(string userid, string fileid, string sender, string f, string t, string reqsrc = "", string mob = "", string DealerCode = "")
        {
            string sql = "";
            string str = "";
            string str2 = "convert(varchar,m.tomobile)";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + userid + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";

            string fDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, f);
            string tDate = string.Format("DATEADD(MINUTE,{0},'{1}')", Global.addMinutes, t);
            if (reqsrc.ToUpper() == "ENTRY") // entry msg
            {
                sql = @"select m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+ msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE 
                FROM MSGSUBMITTED m with (nolock)
                LEFT JOIN DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN SMSFILEUPLOAD u with (nolock) on u.id = m.fileid AND u.USERID=m.PROFILEID
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                WHERE m.PROFILEID='" + userid + "' /* and m.senderid='" + sender + "' */ and m.sentdatetime between " + fDate + " and " + tDate + " and isnull(u.campaignname,'')='Manual' AND isnull(u.FILENM,'')=''";
                if (mob != "") sql = sql + @" AND TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
                sql = sql + @" UNION ALL select m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+ msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE FROM MSGSUBMITTEDlog m with (nolock)
                LEFT JOIN DELIVERYlog d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN SMSFILEUPLOAD u with (nolock) on u.id = m.fileid AND u.USERID=m.PROFILEID
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                WHERE m.PROFILEID='" + userid + "' /* and m.senderid='" + sender + "' */ and m.sentdatetime between " + fDate + " and " + tDate + " and isnull(u.campaignname,'')='Manual' AND isnull(u.FILENM,'')=''";
                if (mob != "") sql = sql + @" AND TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
                //sql = sql + @" order by m.sentdatetime ";

            }
            else if (fileid == "1") // api msg
            {
                sql = @"select m.msgid+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE FROM MSGSUBMITTED m with (nolock) 
                left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102) 
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
                sql = sql + @" UNION ALL select m.msgid+'''' as MessageId, " + str2 + @" as MobileNo, m.senderid as Sender,
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,                
                DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,'""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE 
                FROM MSGSUBMITTEDlog m with (nolock)
                left join DELIVERYlog d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=1 ";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
            }
            else
            {
                sql = @"select m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                    '""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE 
                FROM MSGSUBMITTED m with (nolock) 
                LEFT JOIN DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN SMSFILEUPLOAD U ON U.ID=M.FILEID
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                WHERE m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=" + fileid + "";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
                sql = sql + @" UNION ALL select m.msgid+'''' as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
                    DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) as DeliveredDate,
                    '""'+msgtext+'""' as Message,
                CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
                AS MessageState, d.dlvrtext as RESPONSE,DLRName,CategoryName,LocationName,SubLocationName,DM.DLRCODE FROM MSGSUBMITTEDlog m with (nolock) 
                LEFT JOIN DELIVERYlog d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
                LEFT JOIN SMSFILEUPLOAD U ON U.ID=M.FILEID
                LEFT JOIN DealerMast DM WITH(NOLOCK) ON m.DLRCode = DM.DLRCODE
                LEFT JOIN CategoryMast CM ON CM.CategoryID=DM.CategoryID
                LEFT JOIN LocationMast LM ON LM.LocationID=DM.LocationID
                LEFT JOIN SubLocationMast SLM ON SLM.SubLocationID=DM.SubLocationID
                where m.PROFILEID='" + userid + "' and m.senderid='" + sender + "' and m.sentdatetime between " + fDate + " and " + tDate + " and m.fileid=" + fileid + "";
                if (mob != "") sql = sql + @" and TOMOBILE LIKE '%" + mob + "%' ";
                if (DealerCode != "") sql = sql + @" AND DM.DLRCODE='" + DealerCode + "' ";
                //sql = sql + @" order by m.sentdatetime ";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataSet GetCampaignReportMotoCorp(string user, DateTime s1, DateTime s2, string camp, string Event, string CategoryId, string LocationId, string SubLocationId, string DealerCode)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetCampaignReportMotoCorp";
                cmd.Parameters.AddWithValue("UserName", user);
                cmd.Parameters.AddWithValue("Fdate", s1);
                cmd.Parameters.AddWithValue("tdate", s2);
                cmd.Parameters.AddWithValue("Campaign", camp);
                cmd.Parameters.AddWithValue("Event", Event);
                cmd.Parameters.AddWithValue("CategoryID", CategoryId);
                cmd.Parameters.AddWithValue("LocationId", LocationId);
                cmd.Parameters.AddWithValue("SubLocationId", SubLocationId);
                cmd.Parameters.AddWithValue("DealerCode", DealerCode);
                da.Fill(ds);
            }
            return ds;
        }

        public DataSet GetCampaignWiseDetailsMotoCorp(string user, DateTime s1, DateTime s2, string camp, string Event, string CategoryId, string LocationId, string SubLocationId, string DealerCode)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetCampaigrptDetailsMotoCorp";
                cmd.Parameters.AddWithValue("usr", user);
                cmd.Parameters.AddWithValue("Fdate", s1);
                cmd.Parameters.AddWithValue("tdate", s2);
                cmd.Parameters.AddWithValue("camp", camp);
                cmd.Parameters.AddWithValue("Event", camp);
                cmd.Parameters.AddWithValue("CategoryID", camp);
                cmd.Parameters.AddWithValue("LocationId", camp);
                cmd.Parameters.AddWithValue("SubLocationId", camp);
                cmd.Parameters.AddWithValue("DealerCode", camp);
                da.Fill(ds);
            }
            return ds;
        }

        #endregion

        public string INSERTHEROMAST(string EventID, string EventName, string CategoryID, string CategoryName, string LocationID,
            string LocationName, string SubLocationID, string SubLocationName, string DLRCODE, string DLRName, string MASTERTYPE)
        {
            string Msg = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_INSERTHEROMAST";
                    cmd.Parameters.AddWithValue("@EventID", EventID);
                    cmd.Parameters.AddWithValue("@EventName", EventName);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@LocationName", LocationName);
                    cmd.Parameters.AddWithValue("@SubLocationID", SubLocationID);
                    cmd.Parameters.AddWithValue("@SubLocationName", SubLocationName);
                    cmd.Parameters.AddWithValue("@DLRCODE", DLRCODE);
                    cmd.Parameters.AddWithValue("@DLRName", DLRName);
                    cmd.Parameters.AddWithValue("@MASTERTYPE", MASTERTYPE);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@MSG"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@MSG"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@MSG"].Value.ToString().Trim();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Msg;
        }

        public string CheckDubliEventMast(string EventCode, string EventName)
        {
            string sql = "if exists(select * from EventMast where EventID='" + EventCode + "' or EventName='" + EventName + "') begin select 'True' end";
            string ret = Convert.ToString(database.GetScalarValue(sql));
            return ret;
        }

        public string CheckDubliCategoryMast(string CategoryCode, string CategoryName)
        {
            string sql = "if exists(select * from CategoryMast where CategoryID='" + CategoryCode + "' or CategoryName='" + CategoryName + "') begin select 'True' end";
            string ret = Convert.ToString(database.GetScalarValue(sql));
            return ret;
        }

        public string CheckDubliLocationMast(string LocationCode, string LocationName)
        {
            string sql = "if exists(select * from LocationMast where LocationID='" + LocationCode + "' or LocationName='" + LocationName + "') begin select 'True' end";
            string ret = Convert.ToString(database.GetScalarValue(sql));
            return ret;
        }

        public string CheckDubliSubLocationMast(string SubLocationCode, string SubLocationName)
        {
            string sql = "if exists(select * from SubLocationMast where SubLocationID='" + SubLocationCode + "' or SubLocationName='" + SubLocationName + "') begin select 'True' end";
            string ret = Convert.ToString(database.GetScalarValue(sql));
            return ret;
        }

        public string CheckDubliDealerMast(string DLRCODE, string DLRName)
        {
            string sql = "if exists(select * from DealerMast where DLRCODE='" + DLRCODE + "' or DLRName='" + DLRName + "') begin select 'True' end";
            string ret = Convert.ToString(database.GetScalarValue(sql));
            return ret;
        }

        public DataTable GetEventMast(string EventCode)
        {
            string sql = "";
            if (EventCode != "")
            {
                sql = "select * from EventMast WITH(NOLOCK) where EventID='" + EventCode + "' order by EventName";
            }
            else
            {
                sql = "select * from EventMast WITH(NOLOCK) order by EventName";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetCategoryMast(string CatCode)
        {
            string sql = "";
            if (CatCode != "")
            {
                sql = "select * from CategoryMast WITH(NOLOCK) where CategoryID='" + CatCode + "' order by CategoryName";
            }
            else
            {
                sql = "select * from CategoryMast WITH(NOLOCK) order by CategoryName";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetLocationMast(string LocationCode)
        {
            string sql = "";
            if (LocationCode != "")
            {
                sql = "select * from LocationMast Lm WITH(NOLOCK) inner join CategoryMast Cm WITH(NOLOCK) on CM.CategoryID=Lm.CategoryID where Lm.LocationID='" + LocationCode + "' order by Lm.LocationName";
            }
            else
            {
                sql = "select * from LocationMast Lm WITH(NOLOCK) inner join CategoryMast Cm WITH(NOLOCK) on CM.CategoryID=Lm.CategoryID order by Lm.LocationName";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSubLocationMast(string SubLocationCode)
        {
            string sql = "";
            if (SubLocationCode != "")
            {
                sql = @"select * from SubLocationMast Sm WITH(NOLOCK) inner join LocationMast Lm WITH(NOLOCK) on Lm.LocationID= Sm.LocationID
                        inner join CategoryMast Cm WITH(NOLOCK) on Cm.CategoryID = Sm.CategoryID where Sm.SubLocationID='" + SubLocationCode + "' order by Sm.SubLocationName";
            }
            else
            {
                sql = @"select * from SubLocationMast Sm WITH(NOLOCK) inner join LocationMast Lm WITH(NOLOCK) on Lm.LocationID=Sm.LocationID
                        inner join CategoryMast Cm WITH(NOLOCK) on Cm.CategoryID = Sm.CategoryID order by Sm.SubLocationName";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDealerMast(string DLRCODE)
        {
            string sql = "";
            if (DLRCODE != "")
            {
                sql = @"select * from DealerMast dm WITH(NOLOCK) 
                        inner join SubLocationMast Sm WITH(NOLOCK) on Sm.SubLocationID=dm.SubLocationID
                        inner join LocationMast Lm WITH(NOLOCK) on Lm.LocationID=dm.LocationID
                        inner join CategoryMast Cm WITH(NOLOCK) on CM.CategoryID = dm.CategoryID where dm.DLRCODE='" + DLRCODE + "' order by dm.DLRName";
            }
            else
            {
                sql = @"select * from DealerMast dm WITH(NOLOCK) 
                        inner join SubLocationMast Sm WITH(NOLOCK) on Sm.SubLocationID=dm.SubLocationID
                        inner join LocationMast Lm WITH(NOLOCK) on Lm.LocationID=dm.LocationID
                        inner join CategoryMast Cm WITH(NOLOCK) on CM.CategoryID = dm.CategoryID order by dm.DLRName";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable getLocation(string CategoryCode)
        {
            string sql = "select * from LocationMast WITH(NOLOCK) where CategoryID = '" + CategoryCode + "' order by LocationName";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable getSubLocation(string LocationID)
        {
            string sql = "select * from SubLocationMast WITH(NOLOCK) where LocationID= '" + LocationID + "' order by SubLocationName ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void InsertIntoHERODLRDATA(string dbName, string user)
        {
            //string InsDLR = @"DECLARE @maxid int 
            //                            SET @maxid=(select max(id) From FileProcess)
            //                            insert into HERODLRDATA(FileProcessId,MobileNo,DLRCode)
            //                            select @maxid,MobNo,DlrCode from " + dbName + ".dbo." + user + "";
            //database.ExecuteNonQuery(InsDLR);
        }

        public DataTable GetUserLoginEntry()
        {
            string sql = "select UserId from LoginEntry group by UserId";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetInactiveAccounts()
        {
            string sql = "select i.userID,c.Email,i.LastActiveDate,i.LastUsedDate,i.LastSubmitted,i.Last3MonthAvg from InactiveAccounts i inner join customer c on i.UserID=c.UserName";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void ActivateAccount(string userid)
        {
            string sql = "delete from InactiveAccounts where userID='" + userid + "'";
            database.ExecuteNonQuery(sql);
        }

        public string InsertInactiveAccounts(string UserID, DateTime LastActiveDate, DateTime LastUsedDate, string LastSubmitted,
            string Last3MonthAvg, DateTime insertdateTime, string MsterType)
        {
            string Msg = "";

            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InactiveAccounts";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@LastActiveDate", LastActiveDate);
                    cmd.Parameters.AddWithValue("@LastUsedDate", LastUsedDate);
                    cmd.Parameters.AddWithValue("@LastSubmitted", LastSubmitted);
                    cmd.Parameters.AddWithValue("@Last3MonthAvg", Last3MonthAvg);
                    cmd.Parameters.AddWithValue("@insertdateTime", insertdateTime);
                    cmd.Parameters.AddWithValue("@MsterType", MsterType);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Msg"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@Msg"].ToString().Trim();
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    cnn.Close();
                    throw ex;
                }
            }
            return Msg;
        }

        public DataTable GetInactiveAccountsByUser(string USERID)
        {
            string sql = @"select Userid, convert(varchar(20), LastActiveDate, 23) LastActiveDate, 
            convert(varchar(20), LastUsedDate, 23) as LastUsedDate,
            LastSubmitted, Last3MonthAvg, convert(varchar(20), insertdateTime, 21) as insertdateTime from InactiveAccounts where UserID ='" + USERID + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetHeroMonthlyReportMotoCorp(string UserName, string Year, string Month, string CategoryID = "0", string LocationID = "0", string SubLocationID = "0", string Dealer = "0")
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    SqlDataAdapter sd = new SqlDataAdapter();
                    sd.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_GetHeroMonthlyReport";
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@Month", Month);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@SubLocationID", SubLocationID);
                    cmd.Parameters.AddWithValue("@Dealer", Dealer);
                    sd.Fill(dt);
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public string Insert_MSGTRAN_91M(string PROFILEID, string MSGTEXT, string TOMOBILE, string SENDERID, string peid, string templateid)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Insert_MSGTRAN_91M";
                    cmd.Parameters.AddWithValue("@PROFILEID", PROFILEID);
                    cmd.Parameters.AddWithValue("@MSGTEXT", MSGTEXT);
                    cmd.Parameters.AddWithValue("@TOMOBILE", TOMOBILE);
                    cmd.Parameters.AddWithValue("@SENDERID", SENDERID);
                    cmd.Parameters.AddWithValue("@peid", peid);
                    cmd.Parameters.AddWithValue("@templateid", templateid);
                    cmd.ExecuteNonQuery();
                    Msg = "Saved Successfully !";
                }
                catch (Exception ex)
                {
                    Log("Insert_MSGTRAN_91M -- 2 Factor Authentication : " + ex.ToString());
                }
                return Msg;
            }
        }

        public string GetTranTableName(string SmppAcid)
        {
            string sql = "select TranTableName From SMPPSETTING with(nolock) where smppaccountid='" + SmppAcid + "' and Active = 1";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string InsertintoTranTable(string table, string SMPPACCOUNTID, string PROFILEID, string MSGTEXT, string TOMOBILE, string SENDERID, string peid, string templateid)
        {
            string res = "";
            string sql = @"INSERT INTO " + table + "(PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, FILEID, peid, templateid, datacode) " +
                "values('', '" + SMPPACCOUNTID + "', '" + PROFILEID + "', '" + MSGTEXT + "', '" + TOMOBILE + "', '" + SENDERID + "', GETDATE(), 0, '" + peid + "', '" + templateid + "', 'Default')";
            try
            {
                database.ExecuteNonQuery(sql);
                res = "Success";
            }
            catch (Exception ex)
            {
                Log("InsertintoTranTable -- 2 Factor Authentication : " + ex.ToString());
                res = "Error";
            }
            return res;
        }

        public string CheckUser(string user)
        {
            string sql = "if exists(select * from CUSTOMER where username='" + user + "') begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public DataTable GetMakerCheckerDetails(string UserName, string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SP_GetMakerCheckerDetails";
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                //cmd.Parameters.AddWithValue("@CampName", CampDate);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable Getfailbreakup(string FromDate, string ToDate, string UserName, string CampName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SP_Getfailbreakup";
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@CampName", CampName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetGroupDetailsByUserId(string UserName, string GroupID = "0", string ActionType = "0", string SearchText = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SP_GetGroupDetailsByUserId";
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@ActionType", ActionType);
                cmd.Parameters.AddWithValue("@SearchText", SearchText);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }

        public void UpdateGroupName(string User, string grp, string GroupId)
        {
            string sql = @"UPDATE grouphead SET grpname='" + grp + "' WHERE userid='" + User + "' AND id='" + GroupId + "'";
            database.ExecuteNonQuery(sql);
        }

        public void DeleteGroupByID(string Userid, string GroupId)
        {
            string sql = @"DELETE FROM grouphead WHERE userid='" + Userid + "' AND id='" + GroupId + "'";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetMakerData(string user)
        {
            string sql = "select *,ApprovedRejected as Status from fileprocessMaker with(nolock) where profileid ='" + user + "' and isprocessed=0 and processedtime is null";
            return database.GetDataTable(sql);
        }
        public DataTable GetCheckerData(string user)
        {
            string sql = "select *,ApprovedRejected as Status from fileprocessMaker with(nolock) where CheckerId ='" + user + "' and ApprovedRejected='Pending'";
            return database.GetDataTable(sql);
        }

        public string SendOTPOnWABA(string WABAMobileNo, string otp)
        {
            string res = "";
            try
            {
                string apiUrl = "https://qawaba.myinboxmedia.in/api/sendwaba";
                var client = new RestClient(apiUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("ProfileId", "MIM2200038");
                request.AddParameter("APIKey", "FE1F$FD9_738");
                request.AddParameter("MobileNumber", WABAMobileNo);
                request.AddParameter("TemplateName", "wabaotptemplate");
                request.AddParameter("Parameters", otp);
                request.AddParameter("HeaderType", "TEXT");
                request.AddParameter("HeaderText", "");
                request.AddParameter("MediaUrl", "");
                request.AddParameter("Latitude", 0);
                request.AddParameter("Longitude", 0);
                request.AddParameter("IsTemplate", true);
                request.AddParameter("ButtonOrListJSON", "");
                IRestResponse response = client.Execute(request);
                Log("response: " + response.Content);
                string pStatus = response.StatusCode.ToString();
                if (pStatus.ToUpper() == "OK")
                {
                    Response responseObj = JsonConvert.DeserializeObject<Response>(response.Content);
                    string mid = responseObj.mid.ToString();
                    string StatusCode = responseObj.statusCode.ToString();
                    if (StatusCode.Contains("success"))
                    {
                        res = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Exception occurred: {ex.Message}");
                res = "";
            }
            return res;
        }

        public DataTable GetwaTemplateId(string userid, string id)
        {
            string sql = "";
            sql = @"SELECT a.tName+' : ' +(
                    case 
                    when ISNULL(tHeadType,'')='t' then 'Text'
                    when ISNULL(tHeadType,'')='i' then 'Image'
                    when ISNULL(tHeadType,'')='v' then 'Video'
                    when ISNULL(tHeadType,'')='d' then 'Document'
                    when ISNULL(tHeadType,'')='ca' then 'Call_To_Action'
                    when ISNULL(tHeadType,'')='qb' then 'Quick_Reply_Button'
                    when ISNULL(tHeadType,'')='ub' then 'URL_Button'
                    when ISNULL(tHeadType,'')='Carousel' then 'Carousel'
                    when ISNULL(tHeadType,'')='Catalog' then 'Catalog'
                    else '' end ) as name1, cast(a.nid as varchar) +' ( ' + a.tName +')' AS name,a.nid TemplateID,
                    (case 
                    when ISNULL(tHeadType,'')='t' then 'Text'
                    when ISNULL(tHeadType,'')='i' then 'Image'
                    when ISNULL(tHeadType,'')='v' then 'Video'
                    when ISNULL(tHeadType,'')='d' then 'Document'
                    when ISNULL(tHeadType,'')='ca' then 'Call_To_Action'
                    when ISNULL(tHeadType,'')='qb' then 'Quick_Reply_Button'
                    when ISNULL(tHeadType,'')='ub' then 'URL_Button'
                    when ISNULL(tHeadType,'')='Carousel' then 'Carousel'
                    when ISNULL(tHeadType,'')='Catalog' then 'Catalog'
                     else '' end ) TempType
                    FROM WhatsAppBot..template a WITH(NOLOCK)
                    WHERE a.userid ='" + userid + "' and ISNULL(tHeadType,'')<>'Carousel'";
            if (Convert.ToString(id) != "")
            {
                sql = sql + "AND a.tType = '" + id + "'";
            }
            sql = sql + "AND IsActive=1 AND AllotorReject='A' ORDER BY a.tName";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void SaveSMSLinkToWABADetails(string UserID, string ShortURLId, string ShortURL, string Segment, string WABAUserId, string WABAUserApiKey, string WABATemplateName, string WABATemplateTempType, string URL, string Delimeters = "")
        {
            try
            {
                string sql = "Insert into SendWABAOnLinkClick (LinkextUserId, ShortUrlID, ShortUrl, Segment, WABAUserID, WABAUserAPIKey, WABATemplateName, WABATemplateTempType, Url, InsertDateTime, Active, WabaParamWithDelimiter) values " +
                "('" + UserID + "','" + ShortURLId + "','" + ShortURL + "','" + Segment + "','" + WABAUserId + "','" + WABAUserApiKey + "','" + WABATemplateName + "','" + WABATemplateTempType + "','" + URL + "',getdate(),1,'" + Delimeters + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Log($"Exception occurred: {ex.Message}");
            }
        }

        public void UpdateCheckerMakerStatus(string UserID, string AppOrRej, string Reason, string ID)
        {
            try
            {
                string sql = @"UPDATE fileprocessMaker SET CheckerId='" + UserID + "',ApprovedRejected='" + AppOrRej + "',ApprovedRejectedReason='" + Reason + "',ApprovedRejectedDateTime=getdate() WHERE Profileid='" + UserID + "' AND id='" + ID + "'";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Log($"Exception occurred: {ex.Message}");
            }
        }

        public void SendEmailInCaseOfMaker(string user, string DLTNO, string CampaignName, string FileName, int cnt, string templateID, string Msg)
        {
            DataTable dt = database.GetDataTable("SELECT * FROM settingmast");
            //DataTable dtCust = database.GetDataTable(@"select top 1 * from CUSTOMER where DLTNO='" + Convert.ToString(DLTNO) + "' and username <> '" + user + "' and Active=1 order by NEWID()");
            DataTable dtCust = database.GetDataTable(@"select top 1 * from CUSTOMER where DLTNO='" + Convert.ToString(DLTNO) + "' and Active=1 order by NEWID()");
            if (dtCust != null && dtCust.Rows.Count > 0)
            {
                if (Convert.ToString(dtCust.Rows[0]["Email"]) != "")
                {
                    SendEmail(dtCust.Rows[0]["Email"].ToString(), "SMS Campaign created by " + user + "", @"Hello," + " \n Email Campaign has been created by " + user + ". \n Campaign details – \n \n Campaign Name: " + Convert.ToString(CampaignName) + " \n Campaign Creation Date Time: " + DateTime.Now.ToString() + " \n File Name: " + FileName + " \n Mobile Numbers Count: " + cnt + " \n Template ID: " + templateID + " \n Template Text: " + Msg + "." + " \n \n Please login to Linkext and Approve the campaign.",
                                                dt.Rows[0]["userid"].ToString(), dt.Rows[0]["password"].ToString(), dt.Rows[0]["host"].ToString());
                    string sql = @"update fileprocessMaker set CheckerId='" + dtCust.Rows[0]["username"].ToString() + "' where profileid='" + user + "' and campname='" + CampaignName + "' and Convert(date,InsertTime)=Convert(date,getdate())";
                    database.ExecuteNonQuery(sql);
                }
                else
                {
                    new Util().Log("Email is empty in customer table : " + Convert.ToString(dtCust.Rows[0]["username"]));
                }
            }
        }

        public DataTable GetShortURL(string user)
        {
            string sql = @" SELECT ID,WABATemplateName,LTRIM(ShortUrl) AS ShortURL,InsertDateTime AS InsertDate FROM SendWABAOnLinkClick WITH(NOLOCK)
                              WHERE LinkextUserId='" + user + "' AND (shorturl <> '' AND shorturl IS NOT NULL)";
            return database.GetDataTable(sql);
        }

        #region Add By Vikas Insert ShortURL On 10_07_2024
        public void SaveShortURL(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "", string btnName = "", string pageName = "", string RichMediaGroupRows = "")
        {
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Sp_SaveShortURL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@LongURL", LongURL);
                cmd.Parameters.AddWithValue("@UserHostAddress", UserHostAddress);
                cmd.Parameters.AddWithValue("@ShortURL", ShortURL);
                cmd.Parameters.AddWithValue("@mobTrk", mobTrk);
                cmd.Parameters.AddWithValue("@mainurl", mainurl == "Y" ? "1" : "0");
                cmd.Parameters.AddWithValue("@domain", domain);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@richmediaurl", richmediaurl == "Y" ? "1" : "0");
                cmd.Parameters.AddWithValue("@btnName", btnName);
                cmd.Parameters.AddWithValue("@pageName", pageName);
                cmd.Parameters.AddWithValue("@RichMediaGroupRows", RichMediaGroupRows);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        //Add By Vikas ON 15_07_2024
        public DataTable GetURLShortsUser(string User, string domain)
        {
            DataTable dt = new DataTable("dt");
            string sh = domain;
            string sql = @" SELECT ISNULL(urlname,'') + '    ' + '" + sh + @"' + segment AS shorturlDISP, '" + sh + @"' + segment AS shorturl FROM short_urls WITH(NOLOCK) 
                            WHERE userid='" + User + @"' AND RIGHT(SEGMENT,2)<>'_Q' AND MAINURL=0 AND Cast(isnull(Expiry,dateadd(yy,25,GETDATE())) as date) >=Cast(GETDATE() as date) 
                            AND id NOT IN (SELECT ShortUrlID FROM SendWABAOnLinkClick WITH(NOLOCK) WHERE LinkextUserId='" + User + @"')
                            ORDER BY added DESC";
            dt = database.GetDataTable(sql);
            return dt;
        }


        public void AddBlackListNumbers(List<string> mobList, string user)
        {
            foreach (var m in mobList)
            {
                string num = m;
                int checkExist = Convert.ToInt32(database.GetScalarValue("select count(1) from blacklistno with(nolock) where userid='" + user + "' and MobileNo='" + num + "'"));
                if (checkExist == 0)
                {
                    try
                    {
                        database.ExecuteNonQuery("INSERT INTO blacklistno (userid,mobileno,operator,CurrentDate,UpdateDate) values ('" + user + "','" + num + "',0,getdate(),Convert(date,getdate()))");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void AddBlackListNumbersUsingDLT(List<string> mobList, string DltNo)
        {
            DataTable dt = database.GetDataTable("select username from Customer with(nolock) where DLTNO='" + DltNo + "' and Active=1 and usertype='user'");
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < mobList.Count; i++)
                {
                    string num = mobList[i];
                    int checkExist = Convert.ToInt32(database.GetScalarValue("select count(1) from blacklistno with(nolock) where userid='" + dr["Username"].ToString() + "' and MobileNo='" + num + "'"));
                    if (checkExist == 0)
                    {
                        try
                        {
                            database.ExecuteNonQuery("INSERT INTO blacklistno (userid,mobileno,operator,CurrentDate,UpdateDate) values ('" + dr["Username"].ToString() + "','" + num + "',0,getdate(),Convert(date,getdate()))");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        public DataTable GetBlackListNumbersUsingDLTNO(string DltNo)
        {
            string sql = @"select * from blacklistno with(nolock) where userid in 
                          (select username from Customer with(nolock) where dltno = '" + DltNo + "' and Active = 1 and UserType = 'User') " +
                          "order by CurrentDate desc";
            return database.GetDataTable(sql);
        }

        public DataTable GetBlackListNumbers(string user)
        {
            string sql = "select * from blacklistno with(nolock) where userid='" + user + "' order by CurrentDate desc";
            return database.GetDataTable(sql);
        }

        public DataTable GetRichMediaButtonDetails(string UserName, string RichMediaGroupRows, string ButtonName = "", string id = "", int Action = 0)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "Sp_GetRichMediaButtonDetails";
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@RichMediaGroupRows", RichMediaGroupRows);
                cmd.Parameters.AddWithValue("@ButtonName", ButtonName);
                cmd.Parameters.AddWithValue("@GlobalAddMinutes", Global.addMinutes);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }

        public void InsertWhenIsReminderTrue(string UserId)
        {
            try
            {
                database.ExecuteNonQuery("INSERT INTO ReminderDetails (UserId,EmailDate,LastReminderDate,Status) values ('" + UserId + "',getdate(),getdate(),'P')");
            }
            catch (Exception ex)
            {
                Log("Catch Exception : " + ex.Message.ToString());
            }
        }

        public string InsertInMobStatusClick(string segment, string referer, string ip, string browser, string platform, string ismobile, string manuf, string model, bool callBackApplicable, bool User_AgentAutoClick = false, string User_Agent = "")
        {
            string sql = "";
            string urlid = "";
            string shortUrl_id = "";
            string senttime = "";
            string mobile = "";
            sql = "Select id,urlid,convert(varchar,sentdate,106) + ' ' + convert(varchar,sentdate,108) as senttime,mobile from mobtrackurl where segment = '" + segment + "'";
            DataTable dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {
                sql = "Select id,urlid,convert(varchar,sentdate,106) + ' ' + convert(varchar,sentdate,108) as senttime,mobile from mobtrackurl where segment = '" + segment + "' and sentdate = (Select max(sentdate) from mobtrackurl where segment = '" + segment + "')";
                DataTable dt1 = database.GetDataTable(sql);
                urlid = dt1.Rows[0]["id"].ToString();
                shortUrl_id = dt1.Rows[0]["urlid"].ToString();
                senttime = dt1.Rows[0]["senttime"].ToString();
                mobile = dt1.Rows[0]["mobile"].ToString();
            }
            else
            {
                urlid = dt.Rows[0]["id"].ToString();
                shortUrl_id = dt.Rows[0]["urlid"].ToString();
                senttime = dt.Rows[0]["senttime"].ToString();
                mobile = dt.Rows[0]["mobile"].ToString();
            }
            sql = "Insert into mobstatsclick (click_date, ip, referer, shortUrl_id, Browser, Platform, IsMobileDevice, MobileDeviceManufacturer, MobileDeviceModel, urlid, User_AgentAutoClick, User_Agent)" +
                "values (getdate(),'" + ip + "','" + referer + "','" + shortUrl_id + "','" + browser + "','" + platform + "'," +
                "'" + ismobile + "','" + manuf + "','" + model + "','" + urlid + "','" + (User_AgentAutoClick == false ? 0 : 1) + "','" + User_Agent + "')";
            database.ExecuteNonQuery(sql);

            DataTable dtt = database.GetDataTable("Select long_url,userid from short_urls where id = '" + shortUrl_id + "'");
            string url = Convert.ToString(dtt.Rows[0]["long_url"]);
            string userid = Convert.ToString(dtt.Rows[0]["userid"]);
            if (callBackApplicable) url = userid + "~" + mobile + "~" + senttime + "~" + url;
            return url;
        }
    }

    public class Response
    {
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
        public string mid { get; set; }
    }
}