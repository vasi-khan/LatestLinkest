using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace eMIMPanel.Helper
{

    public class common
    {
        public class request
        {
            public int EmployeeId { get; set; }
            public string RequestNo { get; set; }
            public string CompanyName { get; set; }
            public string ClientName { get; set; }
            public string MobileNo { get; set; }
            public string EmailId { get; set; }
            public int ProductId { get; set; }
            public int TransTypeId { get; set; }
            public int PaymodeId { get; set; }
            public int AccountTypeId { get; set; }
            public int QTY { get; set; }
            public decimal Rate { get; set; }
            public string Attachment { get; set; }
            public string UserId { get; set; }
            public string Credentialtobeused { get; set; }
            public string senderid { get; set; }
            public string templateid { get; set; }
            public string SMSText { get; set; }
            public string PEID { get; set; }
        }
        public  String consts = "";
        public SqlConnection con;
        public SqlDataAdapter sda;
        public DataTable dtl;
        public SqlTransaction trans;
        public SqlCommand cmd;
        public common()
        {
             consts = ConfigurationManager.ConnectionStrings["eMIMPanel"].ConnectionString;
        }
        public void ConnectionOpen1()
        {
            con = new SqlConnection(consts);
            if (con.State!=ConnectionState.Open)
            {
                con.Open();

            }
        }
        public void ConnectionClose1()
        {
            con = new SqlConnection(consts);
            if (con.State != ConnectionState.Closed)
            {
                con.Close();
            }
        }


        public static string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["eMIMPanel"].ConnectionString;
        }
        public static void FillDropDown(DropDownList ddl, DataTable dt, string DisplayName, string ValueName, Char HeaderTitle)
        {
            if (dt != null)
            {
                DataRow dr;
                dr = dt.NewRow();
                if (HeaderTitle == 'S')
                {
                    dr[DisplayName] = "--Select--";
                    dr[ValueName] = "0";
                    dt.Rows.InsertAt(dr, 0);
                }
                else if (HeaderTitle == 'A')
                {
                    dr[DisplayName] = "--All--";
                    dr[ValueName] = "0";
                    dt.Rows.InsertAt(dr, 0);
                }
                else if (HeaderTitle == 'N')
                {
                    dr[DisplayName] = "--None--";
                    dr[ValueName] = "0";
                    dt.Rows.InsertAt(dr, 0);
                }
                else if (HeaderTitle == 'F')
                {
                    dr[DisplayName] = "--Franchisee Location--";
                    dr[ValueName] = "0";
                    dt.Rows.InsertAt(dr, 0);
                }
            }
            ddl.DataSource = dt;
            ddl.DataTextField = DisplayName;
            ddl.DataValueField = ValueName;
            ddl.DataBind();
        }

        public DataTable GetDataTable(string sql)
        {
            try
            {

                con = new SqlConnection();
                ConnectionOpen1();
             
                cmd = new SqlCommand(sql,con);
                DataTable dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                ConnectionClose1();
                return dt;
                
               
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
            finally
            {
                ConnectionClose1();
            }


        }
        
        public DataTable EmployeeLogin(string Email, string Password)
        {
            DataTable dt = new DataTable("dt");

            string sql = "Select [USER],RoleCode,isnull(employeeid,-1)employeeid,Name,LangCode from mlogin m join role r on r.roleid=m.roleid join employee e on e.employeecode=m.[user] join LangGroup l on l.groupid=e.langgroupid  where [user]='" + Email.Trim() + "' and RequestFormPWD='" + Password.Trim() + "' and lock=0";
            dt = GetDataTable(sql);
            return dt;
        }

        public DataTable EmployeeDetails(string EmpCode)
        {
            DataTable dt = new DataTable("dt");

            string sql = "Select RequestFormPwd from Employee with(nolock) where EmployeeCode='"+ EmpCode + "'";
            dt = GetDataTable(sql);
            return dt;
        }

        public string SaveRequest(request obj)
        {
            try
            {
                string msg = "";
                using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandText = "[SP_InserRequest]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("EmployeeId", obj.EmployeeId);
                    cmd.Parameters.AddWithValue("CompanyName", obj.CompanyName);
                    cmd.Parameters.AddWithValue("ClientName", obj.ClientName);
                    cmd.Parameters.AddWithValue("MobileNo", obj.MobileNo);
                    cmd.Parameters.AddWithValue("EmailId", obj.EmailId);
                    cmd.Parameters.AddWithValue("ProductId", obj.ProductId);
                    cmd.Parameters.AddWithValue("TransTypeId", obj.TransTypeId);
                    cmd.Parameters.AddWithValue("PaymodeId", obj.PaymodeId);
                    cmd.Parameters.AddWithValue("AccountTypeId", obj.AccountTypeId);
                    cmd.Parameters.AddWithValue("QTY", obj.QTY);
                    cmd.Parameters.AddWithValue("Rate", obj.Rate);
                    cmd.Parameters.AddWithValue("Attachment", obj.Attachment);
                    cmd.Parameters.AddWithValue("UserId", obj.UserId);

                    cmd.Parameters.AddWithValue("Credentialtobeused", obj.Credentialtobeused);
                    cmd.Parameters.AddWithValue("senderid", obj.senderid);
                    cmd.Parameters.AddWithValue("templateid", obj.templateid);
                    cmd.Parameters.AddWithValue("SMSText", obj.SMSText);
                    cmd.Parameters.AddWithValue("PEID", obj.PEID);

                    cmd.Parameters.AddWithValue("Msg", "");
                    cmd.Parameters["Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["Msg"].Size = 0x100;
                    cmd.ExecuteNonQuery();
                    msg = cmd.Parameters["Msg"].Value.ToString();
                    //trnscope.Complete();
                    cnn.Close();
                }
                return msg;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string SendEmail(string toAddress, string subject, string body, string UserId, string Pwd, string Host, string Port, List<string> CC, string Path)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = UserId; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                message.IsBodyHtml = true;
                for (int i = 0; i < CC.Count; i++)
                {
                    message.CC.Add(CC[i]);
                }
                //message.CC.Add("dsng25@gmail.com");

                // message.CC.Add("support@myinboxmedia.com");

                if (Path != "")
                {
                    Attachment item = new Attachment(Path);
                    message.Attachments.Add(item);
                }

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
                //Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public class mlogin
        {
            public long id { get; set; }
            public int employeeid { get; set; }
            public string role { get; set; }
            public string usernmae { get; set; }
            public string name { get; set; }
            public int password { get; set; }
        }



    }

    public class Security
    {
        private const string DEFAULT_KEY = "W3g$1%g$";

        public static string Encrypt(string stringToEncrypt)
        {
            string key = DEFAULT_KEY;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();

            CheckKey(ref key);
            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = Encoding.UTF8.GetBytes(stringToEncrypt);

            CryptoStream cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();


            return System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(memStream.ToArray()));
        }

        public static string Decrypt(string stringToDecrypt)
        {
            string key = DEFAULT_KEY;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CheckKey(ref key);

            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = Convert.FromBase64String(stringToDecrypt);

            CryptoStream cryptostream = new CryptoStream(memStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(inputBytes, 0, inputBytes.Length);
            cryptostream.FlushFinalBlock();

            Encoding encoding = Encoding.UTF8;
            return HttpUtility.HtmlDecode(encoding.GetString(memStream.ToArray()));
        }

        protected static void CheckKey(ref string keyToCheck)
        {
            keyToCheck = (keyToCheck.Length > 8 ? keyToCheck.Substring(0, 8) : keyToCheck);
            if (keyToCheck.Length < 8)
            {
                for (int i = keyToCheck.Length; i < 8; i++)
                {
                    keyToCheck += DEFAULT_KEY[i];
                }
            }
        }

        protected static byte[] HashKey(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);

            // Truncate hash
            byte[] truncateHash = new byte[length];
            Array.Copy(hash, 0, truncateHash, 0, length);
            return truncateHash;
        }

        protected static DataTable GetInformation(string encryptedStrings)
        {
            string decrypt = Decrypt(encryptedStrings);
            DataTable dtCrypt = new DataTable();
            dtCrypt.Columns.Add("Colname", typeof(System.String));
            dtCrypt.Columns.Add("Value", typeof(System.String));
            string[] strKeys = decrypt.Split(new char[] { '&' });
            foreach (string key in strKeys)
            {
                string[] keys = key.Split(new char[] { '=' });
                DataRow dr = dtCrypt.NewRow();
                dr[0] = keys[0];
                dr[1] = keys[1];
                dtCrypt.Rows.Add(dr);
            }
            return dtCrypt;
        }
    }
}