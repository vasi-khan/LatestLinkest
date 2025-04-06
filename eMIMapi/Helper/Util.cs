using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Net.Mail;
using Inetlab.SMPP.PDU;
using System.Text.RegularExpressions;
using System.Globalization;
using RestSharp;
using Inetlab.SMPP.Common;
using System.Security.Authentication;
using Inetlab.SMPP;
using Inetlab.SMPP.PDU;
using System.Net.Http;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace eMIMapi.Helper
{
    public class Util
    {
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public string db = "";
        public string bb = "MIM2101450";
        #region < Lic Content>
        //2.9.13 
        public string licenseContent = @"-----BEGIN INETLAB LICENSE------
O7EOHWG2QFLNSCAMPAFAYSLOMV2GYYLC
FZJU2UCQAISFIZLDNBXG62DJOZSSAU3P
NR2XI2LPNZZSAUDSNF3GC5DFEBGGS3LJ
ORSWIAY5ORSWW3TPNBUXMZJOONXWY5LU
NFXW442AM5WWC2LMFZRW63IECFZWQYLS
MVUXILJWHAYDKMRTGI4TGBMA4A6KH3LD
3AEANAFAAMOL7AWZBCAACRRPS3OPJDRU
NBGQBR64NSQ5HTVM3O6NYMPAKDHJQZ6J
UPDOQ2WAQAG7LLASKNSFTAMZ4OLOFBXE
JDRT7YOGQYGTAZFJRWZZ3MUTLR67XPFK
OT3HBKF6NSCNAZ4JQUUGBVRDCOXGKRTS
BKLQ2HFP24WCNNNLGMK6JHFAK4NYRDBO
XU67JUWNEAD3HDEDUZOO7LZUJPH3GPLO
SE======
-----END INETLAB LICENSE--------";
        #endregion
        //public string sql = "";
        public Util()
        {

        }
        public DataTable SMSSummaryReport4API(string userid, string frmdate, string todate)
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
                cmd.CommandText = "SP_SMSSummaryReport4API";
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@frmdate", frmdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                da.Fill(dt);
            }
            return dt;
        }
        public void Info(string msg) { } //{ LogError(msg); }

        public void Log(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        }
        public void LogHMIL(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"HMIL_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"HMIL_catch_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        }
        public void BalLog(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"BAL_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"BAL_catch_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        }
        public DataTable GetUserParameterSecure(string usr, string pwd)
        {
            MIMPwdUtility.MIMUtil obMu = new MIMPwdUtility.MIMUtil();
            DataTable dt = obMu.GetUserParameter("CUSTOMER", "USERNAME", usr, "APIKEY", pwd, database.GetConnectstring());
            return dt;
        }
        public DataTable GetUserParameterSecurePWD(string usr, string pwd)
        {
            MIMPwdUtility.MIMUtil obMu = new MIMPwdUtility.MIMUtil();
            DataTable dt = obMu.GetUserParameter("CUSTOMER", "USERNAME", usr, "PWD", pwd, database.GetConnectstring());
            return dt;
        }
        public DataTable GetUserParameterWithAPIKeySecure(string usr, string pwd, string apikey)
        {
            MIMPwdUtility.MIMUtil obMu = new MIMPwdUtility.MIMUtil();
            DataTable dt = obMu.GetUserParameter("CUSTOMER", "USERNAME", usr, "PWD", pwd, "APIKEY", apikey, database.GetConnectstring());
            return dt;
        }

        public DataTable GetUserParameter(string usr)
        {
            string sql = "select * from CUSTOMER with (nolock) where username='" + usr + "' and active=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetUserParameterScrach(string usr)
        {
            string sql = "select * from CUSTOMER with (nolock) where username='" + usr + "' and active=1";
            DataTable dt = dbScrach.GetDataTable(sql);
            return dt;
        }
        public string getPEid(string user)
        {
            return Convert.ToString(database.GetScalarValue("Select isnull(PEID,'') from customer with (nolock) where username='" + user + "' and active=1"));
        }
        public bool CheckSenderId(string userid, string sender)
        {
            int x = Convert.ToInt16(database.GetScalarValue("select count(id) from senderidmast with (nolock) where userid='" + userid + "' and senderid='" + sender + "'"));
            return (x > 0 ? true : false);
        }
        public bool CheckSenderIdScrach(string userid, string sender)
        {
            int x = Convert.ToInt16(dbScrach.GetScalarValue("select count(id) from senderidmast with (nolock) where userid='" + userid + "' and senderid='" + sender + "'"));
            return (x > 0 ? true : false);
        }

        public bool invalidMobileCheck(List<string> mob)
        {
            foreach (string m in mob)
            {
                bool isNumeric = long.TryParse(Convert.ToString(m).Trim(), out long n);
                if (!isNumeric) return true;
            }
            return false;
        }

        public bool CheckTemplateIdSenderId(string userid, string sender, string TemplateId)
        {
            int x = Convert.ToInt16(database.GetScalarValue("select count(*) from TemplateID where senderid='" + sender + "' and templateid='" + TemplateId + "'"));
            return (x > 0 ? true : false);
        }

        public string GetTemplateIDfromSMS(string sender, string msg, bool ucs, string retMsgText = "")
        {
            msg = msg.Trim().ToUpper();
            string colnm = "TEMPWORDS";
            if (!ucs)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                msg = rgx.Replace(msg, "");
                colnm = "TEMPWORDS2";
            }

            string templID = "";
            //DataTable dt = database.GetDataTable("SELECT TEMPLATEID,dbo.AlphaNumericOnly4API(UPPER(TEMPWORDS)) AS TEMPWORDS FROM TEMPLATEID WHERE SENDERID='" + sender + "' and isnull(msgtext,'')<>'' ");
            DataTable dt = database.GetDataTable("SELECT TEMPLATEID," + colnm + " AS TEMPWORDS,msgtext FROM TEMPLATEID WHERE SENDERID='" + sender + "' and isnull(msgtext,'')<>'' order by len(" + colnm + ") desc");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string stmp = dt.Rows[i]["TEMPWORDS"].ToString().ToUpper();
                    if (stmp.Trim() != "")
                    {
                        string[] words = stmp.Split(';');
                        int k = 0;
                        bool isIncorrectMSG = false;
                        for (int j = 0; j < words.Length; j++)
                        {
                            if (msg.Contains(words[j]) && msg.IndexOf(words[j], k) + 1 > k)
                            { }
                            else
                            { isIncorrectMSG = true; break; }
                            k = msg.IndexOf(words[j]) + 1;
                        }
                        if (isIncorrectMSG == false)
                        {
                            templID = dt.Rows[i]["TEMPLATEID"].ToString() + (retMsgText == "Y" ? "#$" + dt.Rows[i]["msgtext"].ToString() : "");
                            break;
                        }
                    }
                }
            }
            return templID;
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
        public string UpdateAndGetBalance(string UserID, string smstype, Int32 cnt, double rate)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer where username='" + UserID + "'"));
            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble(cnt * (rate * 10));
            bal = Math.Round((bal / 1000), 3);
            string SQ = "update customer set balance = '" + bal + "' where username = '" + UserID + "'";
            try
            {
                database.ExecuteNonQuery(SQ);
            }
            catch (Exception e1) { BalLog(e1.Message + " - " + SQ); }
            return bal.ToString();
        }
        public string UpdateAndGetBalanceScrach(string UserID, string smstype, Int32 cnt, double rate)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer where username='" + UserID + "'"));
            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble(cnt * (rate * 10));
            bal = Math.Round((bal / 1000), 3);
            dbScrach.ExecuteNonQuery("update customer set balance = '" + bal + "' where username = '" + UserID + "'");
            return bal.ToString();
        }

        public string GetBalance(string UserID)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer where username='" + UserID + "'"));
            return b.ToString();
        }

        

        public void AddInMsgSubmittedMulti(string userid, string sender, string mobile, string msgt, string msgtype, string msgid, string msgstat, string smppaccountid, string peid, string templateid, double rate, string msg, bool ucs, string provider)
        {
            try
            {
                Int32 rnd = (new Random()).Next(1, 999999);

                //string s = string.Format("Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID," +
                //    "FILEID,DELIVERY_STATUS,peid,templateid,smsrate,smstext,datacode)" +
                //    " values('{0}',{1},'{2}','{3}',N'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", rnd, string.Empty, 
                //    smppaccountid, userid, msgt.Replace("'", "''"), mobile, sender, "getdate()", "getdate()", msgid,1, msgstat, peid, templateid, rate.ToString(), msg.Replace("'", "''"), (ucs ? "UCS2" : "Default"));

                string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,peid,templateid,smsrate,smstext,datacode) " +
                "values ('" + rnd + "','" + provider + @"',
                    '" + smppaccountid + @"',
                    '" + userid + @"',
                    N'" + msgt.Replace("'", "''") + @"',
                    '" + mobile + @"',
                    '" + sender + @"',getdate(),getdate(),'" + msgid + "','1','" + msgstat + "','" + peid + "','" + templateid + "','" + rate.ToString() + "',N'" + msg.Replace("'", "''") + "','" + (ucs ? "UCS2" : "Default") + "')";
                database.ExecuteNonQuery(sql);

            }
            catch (Exception ex)
            {
                try
                {

                    string st = "";
                    if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                    else st = ex.StackTrace;
                    Int32 rnd = (new Random()).Next(1, 999999);
                    string sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,ERRMSG,peid,templateid) " +
                    "values ('" + rnd + "','" + provider + @"',
                '" + smppaccountid + @"',
                '" + userid + @"',
                N'" + msgt.Replace("'", "''") + @"',
                '" + mobile + @"',
                '" + sender + @"',getdate(),getdate(),'" + msgid + "','1','" + msgstat + "','" + ex.Message + " - " + st + "','" + peid + "','" + templateid + "')";
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2) { throw ex2; }
                throw ex;
            }

        }
        public void AddInFailOver(string userid, string mobile, string msg, string MessageId, string Status, string FailOver, string WABATemplateName, string WABAVariables = "", string emailFrom = "", string emailTo = "", string emailCC = "", string emailSubject = "", string FailOver2 = "")
        {
            string sql = "";
            if (FailOver.ToUpper() == "OBD" || FailOver.ToUpper() == "BOTH" || FailOver.ToUpper() == "ALL")
            {
                sql = sql + " Insert into SMSFailOverOBD (PROFILEID,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS) " +
                "values ('" + userid + @"',N'" + msg + @"','" + mobile + @"', getdate(),'" + MessageId + "','" + Status + "') ; ";
            }

            if (FailOver.ToUpper() == "WABA" || FailOver.ToUpper() == "BOTH" || FailOver.ToUpper() == "ALL")
            {
                if (FailOver2.ToUpper() == "OBD")
                {
                    sql = sql + " Insert into SMSFailOverWABA (PROFILEID,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS,WABATEMPLATENAME,WABAVariables,FailOver) " +
                "values ('" + userid + @"',N'" + msg + @"','" + mobile + @"', getdate(),'" + MessageId + "','" + Status + "','" + WABATemplateName + "','" + WABAVariables + "','" + FailOver2 + "') ; ";
                }
                else
                {
                    sql = sql + " Insert into SMSFailOverWABA (PROFILEID,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS,WABATEMPLATENAME,WABAVariables) " +
                "values ('" + userid + @"',N'" + msg + @"','" + mobile + @"', getdate(),'" + MessageId + "','" + Status + "','" + WABATemplateName + "','" + WABAVariables + "') ; ";
                }
            }

            if (FailOver.ToUpper() == "MAIL" || FailOver.ToUpper() == "ALL")
            {
                sql = sql + " Insert into SMSFailOverEMAIL (PROFILEID,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS,EMAIL_FROM,EMAIL_TO,EMAIL_CC,EMAIL_SUBJECT) " +
                "values ('" + userid + @"',N'" + msg + @"','" + mobile + @"', getdate(),'" + MessageId + "','" + Status + "','" + emailFrom + "','" + emailTo + "','" + emailCC + "','" + emailSubject + "') ; ";
            }

            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }
        public void AddInMsgSubmitted(IList<SubmitSmResp> resp, string userid, string sender, string mobile, string msg, string msgtype, string msgid, string msgstat, string smppaccountid, string peid, string templateid, double rate, bool ucs, string provider, DataTable dtMobtrackURL = null, string createdAt = "")
        {
            try
            {
                for (int i = 0; i < resp.Count; i++)
                {
                    Int32 rnd = (new Random()).Next(1, 999999);
                    string mobn = resp[i].Request.DestinationAddress.Address;
                    string msgt = resp[i].Request.MessageText.ToString();

                    string sql = "";
                    sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,peid,templateid,smsrate,smstext,datacode,msgid2) " +
                "values ('" + rnd + "','" + provider + @"',
                    '" + smppaccountid + @"',
                    '" + userid + @"',
                    N'" + msgt.Replace("'", "''") + @"',
                    '" + mobn + @"',
                    '" + sender + @"'," + (createdAt == "" ? "getdate()" : "'" + createdAt + "'") + ",getdate(),'" + resp[i].MessageId + "','1','" + msgstat + "','" + peid + "','" + templateid + "','" + rate.ToString() + "',N'" + msg.Replace("'", "''") + "','" + (ucs ? "UCS2" : "Default") + "','" + resp[i].MessageId + "')";
                    try
                    {
                        database.ExecuteNonQuery(sql);
                    }

                    catch (Exception ex)
                    {
                        Log("PROFILEID-" + userid + " - " + ex.Message + " - " + ex.StackTrace + " -- " + sql);
                        try
                        {
                            string st = "";
                            if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                            else st = ex.StackTrace;
                            Log("PROFILEID-" + userid + " - " + ex.Message + " - " + ex.StackTrace);
                            sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,ERRMSG,peid,templateid) " +
                            "values ('" + rnd + "','" + provider + @"',
                '" + smppaccountid + @"',
                '" + userid + @"',
                N'" + msgt.Replace("'", "''") + @"',
                '" + mobn + @"',
                '" + sender + @"',getdate(),getdate(),'" + resp[i].MessageId + "','1','" + msgstat + "','" + ex.Message + " - " + st + "','" + peid + "','" + templateid + "')";
                            database.ExecuteNonQuery(sql);
                        }
                        catch (Exception ex2) { throw ex2; }
                        throw ex;
                    }
                }

                if (dtMobtrackURL != null)
                {
                    string sql = "";
                    foreach (DataRow dr in dtMobtrackURL.Rows)
                    {
                        sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,templateId) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr["shorturlid"], dr["Mob"], dr["segment"], templateid);
                        database.ExecuteNonQuery(sql);
                    }
                }
            }
            catch (Exception e)
            {
                Log("PROFILEID-" + userid + " - " + e.Message + " - " + e.StackTrace);
            }
        }

        public void AddInMsgSubmittedScrach(IList<SubmitSmResp> resp, string userid, string sender, string mobile, string msg, string msgtype, string msgid, string msgstat, string smppaccountid, string peid, string templateid, double rate, bool ucs, string provider, DataTable dtMobtrackURL = null)
        {
            for (int i = 0; i < resp.Count; i++)
            {
                Int32 rnd = (new Random()).Next(1, 999999);
                string mobn = resp[i].Request.DestinationAddress.Address;
                string msgt = resp[i].Request.MessageText.ToString();

                string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,peid,templateid,smsrate,smstext,datacode,msgid2) " +
                "values ('" + rnd + "','" + provider + @"',
                    '" + smppaccountid + @"',
                    '" + userid + @"',
                    N'" + msgt.Replace("'", "''") + @"',
                    '" + mobn + @"',
                    '" + sender + @"',getdate(),getdate(),'" + resp[i].MessageId + "','1','" + msgstat + "','" + peid + "','" + templateid + "','" + rate.ToString() + "',N'" + msg.Replace("'", "''") + "','" + (ucs ? "UCS2" : "Default") + "','" + resp[i].MessageId + "')";
                try
                {
                    dbScrach.ExecuteNonQuery(sql);
                }

                catch (Exception ex)
                {
                    try
                    {
                        string st = "";
                        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                        else st = ex.StackTrace;

                        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,DELIVERY_STATUS,ERRMSG,peid,templateid) " +
                        "values ('" + rnd + "','" + provider + @"',
                '" + smppaccountid + @"',
                '" + userid + @"',
                N'" + msgt.Replace("'", "''") + @"',
                '" + mobn + @"',
                '" + sender + @"',getdate(),getdate(),'" + resp[i].MessageId + "','1','" + msgstat + "','" + ex.Message + " - " + st + "','" + peid + "','" + templateid + "')";
                        dbScrach.ExecuteNonQuery(sql);
                    }
                    catch (Exception ex2) { throw ex2; }
                    throw ex;
                }
            }

            if (dtMobtrackURL != null)
            {
                string sql = "";
                foreach (DataRow dr in dtMobtrackURL.Rows)
                {
                    sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,SCRATCHCARDCODE) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr["shorturlid"], dr["Mob"], dr["segment"], dr["coupanNo"]);
                    dbScrach.ExecuteNonQuery(sql);
                }

            }
        }


        public void AddInMsgSubmittedInvalidSender(string msgid, DataRow dr, DateTime sendTime)
        {
            try
            {
                string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','ESME_RINVSRCADR - INVALID SENDERID','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";
                sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + msgid + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";
                database.ExecuteNonQuery(sql);

            }
            catch (Exception ex)
            {
                Log("Invalid Sender --" + ex.Message + " - " + ex.StackTrace);
                try
                {
                    string st = "";
                    if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                    else st = ex.StackTrace;
                    string sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,ERRMSG,FILEID) " +
                        "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','ESME_RINVSRCADR - INVALID SENDERID','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";

                    sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + msgid + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";

                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2) { }
            }
        }
        public DataTable GetDelivery(string msgid)
        {
            string sql = @"select top 1 m.PROFILEID,m.MSGID,isnull(d.DLVRSTATUS,'') as dlvrstat, isnull(d.err_code,'') as errcd, d.DLVRTIME 
                from MSGSUBMITTED m WITH (NOLOCK) left join Delivery d WITH (NOLOCK) on m.MSGID = d.MSGID where m.msgid = '" + msgid + "' " +
                " UNION ALL " +
                @"select top 1 m.PROFILEID,m.MSGID,isnull(d.DLVRSTATUS,'') as dlvrstat, isnull(d.err_code,'') as errcd, d.DLVRTIME 
                from MSGSUBMITTEDLOG m WITH (NOLOCK) left join DeliveryLOG d WITH (NOLOCK) on m.MSGID = d.MSGID where m.msgid = '" + msgid + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetDeliveryWithCode(string msgid)
        {
            string sql = @"SELECT SubClientCode FROM MSGUAESubClientCode WHERE msgidClient = '" + msgid + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDeliveryWithSubClientCode(string msgid)
        {
            string sql = @"SELECT SubClientCode FROM MSGSubClientCode WHERE msgidClient = '" + msgid + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataSet GetDeliveryWithWABA(string msgid)
        {
            string WABADBO = ConfigurationManager.AppSettings["WABADBO"].ToString();
            string SMPPMAINDBO = ConfigurationManager.AppSettings["SMPPMAINDBO"].ToString();
            string res = "";
            string sql = "";
            DataSet ds = new DataSet();
            sql = @"select top 1 m.PROFILEID,m.MSGID,isnull(d.DLVRSTATUS,'') as dlvrstat, isnull(d.err_code,'') as errcd, d.DLVRTIME 
                from MSGSUBMITTED m WITH (NOLOCK) left join Delivery d WITH (NOLOCK) on m.MSGID = d.MSGID where m.msgid = '" + msgid + "' " +
               " UNION ALL " +
               @"select top 1 m.PROFILEID,m.MSGID,isnull(d.DLVRSTATUS,'') as dlvrstat, isnull(d.err_code,'') as errcd, d.DLVRTIME 
                from MSGSUBMITTEDLOG m WITH (NOLOCK) left join DeliveryLOG d WITH (NOLOCK) on m.MSGID = d.MSGID where m.msgid = '" + msgid + "'";
            //DataTable dt = database.GetDataTable(sql);

            string messageid = Convert.ToString(database.GetScalarValue(@"select wabamsgid from SMSFailOverWABAsentResponse with(nolock) where MSGID='" + msgid + "'"));
            if (messageid != "")
            {
                sql += @"select distinct MSGID, isnull(e.mimErrDesc,'') Status,inserttime DeliveryTime,isnull(MiMErrCode,'') Code, 'C' Chargable
from " + WABADBO + @" tblWABASubmitted s with (nolock)
  JOIN " + SMPPMAINDBO + @" DeliveryWABA d WITH (NOLOCK) ON s.messageid=d.msgid 
 JOIN " + WABADBO + @" CUSTOMER c WITH (NOLOCK) ON c.username=s.profileid 
 left JOIN " + WABADBO + @" tblWabaDlrErrCode e WITH (NOLOCK) ON e.providername=c.providername and e.dlrErrCode=d.status_groupId 
   WHERE MSGID='" + messageid + "' order by d.inserttime desc";
                //msgdt = database.GetDataTable(sql1);
            }
            else
            {
                res = "Waba Not Sent";
            }

            ds = database.GetDataSet(sql);
            return ds;
        }

        public DataTable GetDeliveryScratch(string msgid)
        {
            string sql = @"select top 1 m.PROFILEID,m.MSGID,isnull(d.DLVRSTATUS,'') as dlvrstat, isnull(d.err_code,'') as errcd, d.DLVRTIME 
                from MSGSUBMITTED m left join Delivery d on m.MSGID = d.MSGID where m.msgid = '" + msgid + "'";
            DataTable dt = dbScrach.GetDataTable(sql);
            return dt;
        }

        public void InsertInAPiLog(string userid, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string msgid = "")
        {
            try
            {
                string sql = "Insert into apiCallLog (userid, mobile, senderid, msg, msgtype, peid, templateid, msgid) " +
                   "values ('" + userid + "','" + mobile + "','" + sender + "',N'" + msg.Replace("'", "") + "','" + msgtype + "','" + peid + "','" + templateid + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }



        public void AddInMsgQueue_HC(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid, DataTable dtMobtrackURL, string createdAt)
        {
            try
            {
                string tablenm = (msgtype == "33" ? "MSGQUEUEB4single" : "MSGQUEUEB4single_Oth");
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','" + createdAt + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                sql = "insert into MSGQUEUELOG (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT, PICKED_DATETIME,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','" + createdAt + "',GETDATE(),'1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                if (dtMobtrackURL != null)
                {
                    if (dtMobtrackURL.Rows.Count > 0)
                        foreach (DataRow dr in dtMobtrackURL.Rows)
                        {
                            sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,templateId) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr["shorturlid"], dr["Mob"], dr["segment"], templateid);
                            database.ExecuteNonQuery(sql);
                        }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void AddInMsgQueueOTP(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "")
        {
            try
            {
                string tablenm = "MSGQUEUEB4single";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }
        public void AddInMsgQueue(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "")
        {
            try
            {
                string tablenm = (userid.Trim().ToUpper() == bb ? "MSGQUEUEB4single" : "MSGQUEUEB4single_Oth");
                if (msgtype == "25") tablenm = "MSGQUEUEB4singleHMIOTP";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }

        public void AddInMsgQueue1(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "", string SubClientCode = "")
        {
            try
            {
                string tablenm = "MSGQUEUEB4single1650";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                if (SubClientCode != "")
                {
                    addInMSGSubClientCode(userid, sender, m, msg, msgid, sms_systemid, peid, rate, noofsms, ucs, templateid, SubClientCode);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void AddInMsgQueue2(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "", string SubClientCode = "", string FailOver = "")
        {
            try
            {
                string tablenm = "";
                string sql = "";
                tablenm = "MSGQUEUEB4singleHMISVR";
                if (userid == "MIM2300228" || userid == "MIM2300229") tablenm = "MSGQUEUEB4single_Oth";

                sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                //sender.ToUpper() == "HIIBPL" ||
                if (userid == "MIM2201048" || userid == "MIM2201104" || userid == "MIM2300228" || userid == "MIM2300229")
                {
                    if (SubClientCode != "")
                    {
                        addInMSGSubClientCode(userid, sender, m, msg, msgid, sms_systemid, peid, rate, noofsms, ucs, templateid, SubClientCode);
                    }
                    if (FailOver != "")
                    {
                        AddInFailOver(userid, m, msg, msgid, "Queued", FailOver, "");
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        public void addInMSGSubClientCode(string userid, string sender, string m, string msg, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "", string SubClientCode = "")
        {
            if (SubClientCode != "")
            {
                string sql = "insert into MSGSubClientCode (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient,SubClientCode) " +
                  " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "','" + SubClientCode + "')";
                database.ExecuteNonQuery(sql);
            }
        }
        public void AddInMsgQueue3(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "")
        {
            try
            {
                string tablenm = "MSGQUEUEB4single2201";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }

        public void AddInMsgQueueGSM(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "")
        {
            try
            {
                string tablenm = "MSGQUEUEB4singleGSM";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

            }
        }

        public void AddInMsgQueueAPI(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, 
            int noofsms, bool ucs, string templateid = "",string SubClientCode = "", string FailOver = "", string WABATemplateName="", string WABAVariables = "", 
            string emailFrom = "", string emailTo = "", string emailCC = "", string emailSubject = "", string FailOver2 = "")
        {
            try
            {
                string tablenm = "";
                if (msgtype == "15" || msgtype == "16" || msgtype == "19" || msgtype == "22" || msgtype == "20" || msgtype == "30" || msgtype == "40" || msgtype == "50")
                    tablenm = "MSGQUEUEB4single_UAE";
                else
                {
                    if (msgtype == "33")
                        tablenm = "MSGQUEUEB4single";
                    else if (msgtype == "25")
                        tablenm = "MSGQUEUEB4singleHMIOTP";
                    else
                        tablenm = "MSGQUEUEB4singleAPI";
                }
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                if (SubClientCode != "")
                {
                    addInMSGSubClientCode(userid, sender, m, msg, msgid, sms_systemid, peid, rate, noofsms, ucs, templateid, SubClientCode);
                }
                if (FailOver != "")
                {
                    AddInFailOver(userid, m, msg, msgid, "", FailOver, WABATemplateName, WABAVariables, emailFrom, emailTo, emailCC, emailSubject, FailOver2);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AddInMsgQueueTEST(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate, int noofsms, bool ucs, string templateid = "", string SubClientCode = "")
        {
            try
            {
                string tablenm = "MSGQUEUEB4single_OTHTEST"; // "MSGQUEUEB4singleHMISVR";
                string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                if (sender.ToUpper() == "HIIBPL" || userid == "MIM2201048" || userid == "MIM2201104")
                {
                    tablenm = "MSGSubClientCode";
                    sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient,SubClientCode) " +
                   " VALUES ('','" + sms_systemid + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','" + templateid + "','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid + "','" + SubClientCode + "')";
                    database.ExecuteNonQuery(sql);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public DataTable GetClickReport(string userid, string datefrom, string dateto)
        {
            string s1 = Convert.ToDateTime(datefrom, CultureInfo.InvariantCulture).AddDays(-10).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = Convert.ToDateTime(dateto, CultureInfo.InvariantCulture).AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Convert.ToDateTime(datefrom).AddDays(-10);
            string qry = @"select TOMOBILE,FILEID,SENTDATETIME,templateid 
                into #tmpTemplateId from (
                select mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date) SENTDATETIME,mm.templateid from MSGSUBMITTED mm with (nolock)
				where PROFILEID='" + userid + @"' and SENTDATETIME >='" + s1 + @"'  and SENTDATETIME< '" + s2 + @"'
				group by mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date),mm.templateid
                UNION ALL
                select mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date) SENTDATETIME,mm.templateid from MSGSUBMITTEDlog mm with (nolock)
				where PROFILEID='" + userid + @"' and SENTDATETIME >='" + s1 + @"'  and SENTDATETIME< '" + s2 + @"'
				group by mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date),mm.templateid
                ) X ";

            string sql = qry + @"
SELECT row_number() over ( order by convert(varchar,s.click_date,25) desc) as SL_NO,isnull(u.urlname,'') as URL_NAME, u.LONG_URL,u.domainname + u.segment as SHORT_URL,
convert(varchar,m.mobile) as MOBILE,convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108) as SMS_DATE,
                convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108) as CLICK_DATE, s.IP as IP_ADDRESS, isnull(L.city,'') + case when isnull(L.city,'')='' then ' ' else ', ' end + isnull(L.RegionName,'') as REGION,
				s.BROWSER, s.PLATFORM, s.ISMOBILEDEVICE AS IS_MOBILE_DEVICE, s.MOBILEDEVICEMANUFACTURER AS MOBILE_DEVICE_MANUFACTURER, s.MobileDeviceModel AS MOBILE_DEVICE_MODEL,isnull(f.TEMPLATEID,'') TEMPLATEID
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
				inner join short_urls u on  U.ID = m.urlid 
				LEFT join #tmpTemplateId f with(nolock) ON f.FILEID=isnull(m.fileId,1) AND right(f.TOMOBILE,10) = right(m.mobile,10) and  f.SENTDATETIME=CAST(m.sentdate as date)
                LEFT JOIN iplocation L on s.ip=L.query and m.segment=L.segment
                where u.userid='" + userid + "' and s.click_date between '" + datefrom + "' and '" + dateto + " 23:59:59' " +
                @" group by u.urlname,u.LONG_URL,u.domainname + u.segment,convert(varchar,m.mobile) ,convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108) ,
                convert(varchar, s.click_date, 106) + ' ' + convert(varchar, s.click_date, 108) , s.ip, 
				isnull(L.city, '') + case when isnull(L.city,'')= '' then ' ' else ', ' end + isnull(L.RegionName, '') ,
				s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel,s.click_date,isnull(f.TEMPLATEID,'') " +
                " order by convert(varchar,s.click_date,25) desc";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetClickReport2(string userid, string datefrom, string dateto)
        {
            string s1 = Convert.ToDateTime(datefrom, CultureInfo.InvariantCulture).AddDays(-10).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = Convert.ToDateTime(dateto, CultureInfo.InvariantCulture).AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Convert.ToDateTime(datefrom).AddDays(-10);
            string qry = @"select TOMOBILE,FILEID,SENTDATETIME,templateid 
                into #tmpTemplateId from (
                select mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date) SENTDATETIME,mm.templateid from MSGSUBMITTED mm with (nolock)
				where PROFILEID='" + userid + @"' and SENTDATETIME >='" + s1 + @"'  and SENTDATETIME< '" + s2 + @"'
				group by mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date),mm.templateid
                UNION ALL
                select mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date) SENTDATETIME,mm.templateid from MSGSUBMITTEDlog mm with (nolock)
				where PROFILEID='" + userid + @"' and SENTDATETIME >='" + s1 + @"'  and SENTDATETIME< '" + s2 + @"'
				group by mm.TOMOBILE,mm.FILEID,cast(mm.SENTDATETIME as date),mm.templateid
                ) X ";

            string sql = qry + @"
SELECT row_number() over ( order by convert(varchar,s.click_date,25) desc) as SL_NO,isnull(u.urlname,'') as URL_NAME, u.LONG_URL,u.domainname + u.segment as SHORT_URL,
convert(varchar,m.mobile) as MOBILE,convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108) as SMS_DATE,
                convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108) as CLICK_DATE, s.IP as IP_ADDRESS, 
ISNULL(C.Operator,'') + case when isnull(C.Operator,'')='' then ' ' else ', ' end + isnull(C.Circle,'') as REGION,
				s.BROWSER, s.PLATFORM, s.ISMOBILEDEVICE AS IS_MOBILE_DEVICE, s.MOBILEDEVICEMANUFACTURER AS MOBILE_DEVICE_MANUFACTURER, s.MobileDeviceModel AS MOBILE_DEVICE_MODEL,isnull(f.TEMPLATEID,'') TEMPLATEID
                FROM mobtrackurl m inner join mobstats s on m.urlid = s.shortUrl_id and m.id = s.urlid
				inner join short_urls u on U.ID = m.urlid 
				LEFT join #tmpTemplateId f with(nolock) ON f.FILEID=isnull(m.fileId,1) AND right(f.TOMOBILE,10) = right(m.mobile,10) and f.SENTDATETIME=CAST(m.sentdate as date)
                LEFT JOIN MOBILEMCCMNC L on f.TOMOBILE = L.MOBILENO
				LEFT JOIN mstMCCMNC C ON DBO.NumericOnly(L.MCCMNC)=DBO.NumericOnly(C.MCCMNC)
                where u.userid='" + userid + "' and s.click_date between '" + datefrom + "' and '" + dateto + "' " +
                @" group by u.urlname,u.LONG_URL,u.domainname + u.segment,convert(varchar,m.mobile) ,convert(varchar,m.sentdate,106) + ' ' + convert(varchar,m.sentdate,108) ,
                convert(varchar, s.click_date, 106) + ' ' + convert(varchar, s.click_date, 108) , s.ip, 
				ISNULL(C.Operator,'') + case when isnull(C.Operator,'')='' then ' ' else ', ' end + isnull(C.Circle,''),
				s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel,s.click_date,isnull(f.TEMPLATEID,'') " +
                " order by convert(varchar,s.click_date,25) desc";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void SendSMSthroughAPI(string mob, string msg, string clientID, string apikey, string sender, string peid)
        {
            string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + clientID + "&pwd=" + apikey + "&mobile=" + mob + "&sender=" + sender + "&msg=" + msg + "&msgtype=13&peid=" + peid;
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

        public string NewShortURLfromSQL(string domain)
        {
            string sql = @"declare @i integer declare @s varchar(6) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 6) if not exists(select segment from short_urls where segment = @s and domainname='" + domain + @"') break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }
        public string NewShortURLfromSQLHondaHmi(string domain)
        {
            string sql = @"declare @i integer declare @s varchar(7) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 7) if not exists(select segment from short_urls where segment = @s and domainname='" + domain + @"') break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }
        public string NewShortURLfromSQL4HC(string domain)
        {
            string sql = @"declare @s varchar(50)
                select @s = REPLACE(NEWID() , '-','') 
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string NewShortURLfromSQLScrach(string domain)
        {
            string sql = @"declare @i integer declare @s varchar(6) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 6) if not exists(select segment from short_urls where segment = @s and domainname='" + domain + @"') break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(dbScrach.GetScalarValue(sql));
        }



        public bool CheckLongURL(string userid, string LongURL)
        {
            int cnt = Convert.ToInt16(database.GetScalarValue("Select count(*) from short_urls where userid='" + userid + "' and upper(long_url)='" + LongURL.ToUpper() + "'"));
            if (cnt > 0) return true; else return false;
        }
        public string GetLastShortURLId()
        {
            string sql = "select SCOPE_IDENTITY()";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string NewShortURLforMobTrkSQL()
        {
            string sql = @"declare @i integer declare @s varchar(8) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 8) if not exists(select segment from mobtrackurl where segment = @s) break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string NewShortURLforMobTrkSQL_6char()
        {
            string sql = @"declare @i integer declare @s varchar(6) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 6) if not exists(select segment from mobtrackurl where segment = @s) break else begin set @s = '' set @i = @i + 1 end end
                select @s";
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string GetShortUrlIdFromLongURL(string longURL, string userId)
        {
            string sql = string.Format("select top 1 id from short_urls where upper(long_url)='{0}' AND userid='{1}' order by added desc", longURL.ToUpper(), userId);
            return Convert.ToString(database.GetScalarValue(sql));
        }

        public string GetShortUrlIdFromLongURLScrach(string longURL, string userId)
        {
            string sql = string.Format("select top 1 id from short_urls where upper(long_url)='{0}' AND userid='{1}' order by added desc", longURL.ToUpper(), userId);
            return Convert.ToString(dbScrach.GetScalarValue(sql));
        }

        public void SaveShortURL(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        {
            string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) values " +
                "('" + LongURL.Replace("'", "''") + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "')";
            database.ExecuteNonQuery(sql);
        }
        public string SaveShortURL4HC(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        {
            string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) values " +
                "('" + LongURL.Replace("'", "''") + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "'); " +
                " SELECT MAX(ID) FROM Short_urls ";
            string id = Convert.ToString(database.GetScalarValue(sql));
            return id;
        }

        public void SaveShortURLScrach(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        {
            string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) values " +
                "('" + LongURL.Replace("'", "''") + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "')";
            dbScrach.ExecuteNonQuery(sql);
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

        public string GetShortURLofLongURL(string UserID, string longURL)
        {
            return Convert.ToString(database.GetScalarValue("select domainname+Segment from short_urls where userid='" + UserID + "' and long_url='" + longURL.Replace("'", "''") + "' "));
        }


        public void SendSMSthroughAPICallingApp(string mob, string msg, string client)
        {
            DataTable dt = GetUserParameterCallingApp(client);
            DataRow dr = dt.Rows[0];
            //string apikey = ob.getIPapikey();
            string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + dr["username"].ToString() + "&pwd=" + dr["pwd"].ToString() + "&mobile=" + mob + "&sender=" + dr["senderid"].ToString() + "&msg=" + msg + "&msgtype=13&peid=" + dr["PEID"].ToString();
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
                //throw EX;
            }
        }

        public string SendEmailCallingApp(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host)
        {
            string result = " Message Sent Successfully..!!";
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

        public DataTable GetUserParameterCallingApp(string clientId)
        {
            string sql = "select * from clientMast where ClientID='" + clientId + "'";
            DataTable dt = Callingdatabase.GetDataTable(sql);
            return dt;
        }

        public byte[] getTMID(string provider)
        {
            //public static string tmid = "1702157302357160700";
            if (provider == "AIRTEL") return System.Text.Encoding.UTF8.GetBytes("1702157302357160700");
            else if (provider == "AIRTEL_T") return System.Text.Encoding.UTF8.GetBytes("1702157302357160700");
            else return System.Text.Encoding.UTF8.GetBytes("");
        }

        public byte[] getPEID(string provider, string peid)
        {
            string sp = peid;
            try
            {
                if (provider == "AIRTEL")
                {
                    if (peid.Length > 0) sp = peid.Substring(0, 19);
                }
                return System.Text.Encoding.UTF8.GetBytes(sp);
            }
            catch (Exception ex)
            {
                return System.Text.Encoding.UTF8.GetBytes(sp);
            }
        }

        public byte[] getTEMPLATEID(string provider, string tid)
        {
            string st = tid;
            try
            {
                if (provider == "AIRTEL")
                {
                    if (tid.Length > 0) st = tid.Substring(0, 19);
                }
                return System.Text.Encoding.UTF8.GetBytes(st);
            }
            catch (Exception ex)
            {
                return System.Text.Encoding.UTF8.GetBytes(st);
            }
        }

        public void SendSMSthroughAPI(string userid, string pwd, string mob, string sender, string msg, string peid, string templateId)
        {
            try
            {

                string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + userid + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + sender + "&msg=" + msg + "&msgtype=13&peid=" + peid + "&templateid=" + templateId + "";

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
            catch (Exception r4) { }
        }
        public string UpdateAndGetBalanceSch(string UserID, string smstype, Int32 cnt, double rate)
        {
            string b = Convert.ToString(database.GetScalarValue("Select balance from customer with(nolock) where username='" + UserID + "'"));
            double bal = Convert.ToDouble(b) * 1000;
            bal = bal - Convert.ToDouble(cnt * (rate * 10));
            bal = Math.Round((bal / 1000), 3);
            database.ExecuteNonQuery("update customer set balance = '" + bal + "' where username = '" + UserID + "'");
            return bal.ToString();
        }
        public void RemoveFromSchedule(string user, string fileid, string schdt)
        {
            string sql = @" declare @FileProcessId int=0
set @FileProcessId=(select top 1 FileProcessId from SMSFILEUPLOAD where ID='" + fileid + @"')

update FileProcess set scheduleDeletedTime =GETDATE() where id=@FileProcessId

delete from msgschedule where profileid='" + user + "' and schedule=DateAdd(MINUTE, " + "0" + " ,'" + schdt + "') and picked_datetime is null and isnull(fileid,0) = '" + fileid + "'";
            database.ExecuteNonQuery(sql);
        }
        public DataTable GetScheduleRecords(string user, string f, string t)
        {
            DataTable dt = new DataTable("dt");
            string sql = @"select DATEADD(MINUTE," + "0" + @",convert(varchar,m.Schedule,106) + ' ' + convert(varchar(5),m.schedule,108)) as Schedule, count(*) as mobiles,isnull(fileid,0) as fileid,m.smsrate,
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 160 then 1 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 306 then 2 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 459 then 3 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 612 then 4 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 765 then 5 else
case when  len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) <= 918 then 6 else 7 end end end end end end 
as noofsms,f.fileName,f.campname,f.msg  from msgschedule m with (nolock) 

join SMSFILEUPLOAD u with (nolock) on u.id=m.FILEID and u.USERID=m.PROFILEID
join FileProcess f with (nolock) on u.fileprocessid=f.id
where m.profileid='" + user + @"'
and m.schedule between '" + f + "' and DateAdd(MINUTE," + "0" + @",'" + t + @"') and picked_datetime is null
group by DATEADD(MINUTE," + "0" + @",convert(varchar,m.Schedule,106) + ' ' + convert(varchar(5),m.schedule,108)),isnull(fileid,0),m.smsrate,len(msgtext) + (len(msgtext) - len(replace(msgtext,'|',''))) + (len(msgtext) - len(replace(msgtext,'~',''))) ,f.fileName,f.campname,f.msg order by Schedule
";

            dt = database.GetDataTable(sql);
            return dt;
        }
        #region Add By Vikas Send EMail Api
        public void SendEMAILthroughAPI(string profileid, string ApiKey, string From, string To, string CC, string Subject, string msgtext)
        {
            try
            {
                string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var client = new RestClient("http://emailapi.myinboxmedia.in/api/SendEmailAPI");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
" + "\n" +
                @"    ""ProfileId"": """ + profileid + @""",
" + "\n" +
                @"    ""APIKey"": """ + ApiKey + @""",
" + "\n" +
                @"    ""From"": """ + From + @""",
" + "\n" +
                @"    ""To"": """ + To + @""",
" + "\n" +
                @"    ""CC"": """ + CC + @""",
" + "\n" +
                @"    ""Subject"": """ + Subject + @""",
" + "\n" +
                @"    ""Text"": """ + msgtext + @"""
" + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Log("req-" + body + " resp-" + response.Content);
                try
                {

                }
                catch (Exception ex)
                {
                    Log("SendEMAILthroughAPI 1 - " + ex.Message);
                }
            }
            catch (Exception e1) { Log("SendEMAILthroughAPI 2 - " + e1.Message); }
        }
        #endregion.

        #region < SMPP EVENTS >
        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private async void client_evDisconnected(object sender)
        {

            //_log.Info("SmppClient disconnected");

            //SmppClient client = (SmppClient)sender;
            //string s = client.SystemID;
            //int i = -1;
            //for (int j = 0; j < dt.Rows.Count; j++)
            //{
            //    if (dt.Rows[j]["SYSTEMID"].ToString() == s)
            //    { i = j; break; }
            //}
            //if (i >= 0)
            //{
            //    DataRow[] dr = dt.Select("SYSTEMID = '" + s + "'");
            //    string[] arg = (
            //    i.ToString() + "$" + "CONNECT" +      // 0, 1
            //            Convert.ToString(dr[0]["SMPPACCOUNTID"]) + "$" +   //2
            //            Convert.ToString(dr[0]["PROVIDER"]) + "$" +        //3
            //            Convert.ToString(dr[0]["ACCOUNTTYPE"]) + "$" +     //4
            //            Convert.ToString(dr[0]["HOSTNAME"]) + "$" +        //5
            //            Convert.ToString(dr[0]["PORT"]) + "$" +            //6
            //            Convert.ToString(dr[0]["USESSL"]) + "$" +          //7
            //            Convert.ToString(dr[0]["SYSTEMID"]) + "$" +        //8
            //            Convert.ToString(dr[0]["PASSWORD"]) + "$" +        //9
            //            Convert.ToString(dr[0]["BINDINGMODE"]) + "$" +     //10
            //            Convert.ToString(dr[0]["SYSTEMTYPE"]) + "$" +      //11
            //            Convert.ToString(dr[0]["ADDRESS_TON"]) + "$" +     //12
            //            Convert.ToString(dr[0]["ADDRESS_NPI"]) + "$" +     //13
            //            Convert.ToString(dr[0]["SOURCE_ADDRESS"]) + "$" +  //14
            //            Convert.ToString(dr[0]["TON_S"]) + "$" +           //15
            //            Convert.ToString(dr[0]["NPI_S"]) + "$" +           //16
            //            Convert.ToString(dr[0]["SERVICE"]) + "$" +         //17
            //            Convert.ToString(dr[0]["DESTNATION_ADDRESS"]) + "$" + //18
            //            Convert.ToString(dr[0]["TON_D"]) + "$" +           //19
            //            Convert.ToString(dr[0]["NPI_D"]) + "$" +           //20
            //            Convert.ToString(dr[0]["DATACODING"]) + "$" +      //21
            //            Convert.ToString(dr[0]["MODE"])).Split('$');

            //    await Disconnect(arg);

            //    for (int m = 0; m < grid.Rows.Count; m++)
            //        if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
            //            grid.Rows[m].Cells[3].Value = "DisConnected";

            //    await Connect(arg);
            //}


            //Sync(this, () =>
            //{
            //    bConnect.Enabled = true;
            //    bDisconnect.Enabled = false;
            //    bSubmit.Enabled = false;
            //    cbReconnect.Enabled = true;
            //});

        }

        private void ClientOnRecoverySucceeded(object sender, BindResp data)
        {
            //_log.Info("Connection has been recovered.");

            //Sync(this, () =>
            //{
            //    bConnect.Enabled = false;
            //    bDisconnect.Enabled = true;
            //    bSubmit.Enabled = true;
            //    cbReconnect.Enabled = false;
            //});

        }

        private async Task UnBind(string[] arg)
        {
            //int i = Convert.ToInt16(arg[0]);
            //_log.Info("Unbind SmppClient " + arg[5]);
            //UnBindResp resp = await client.UnbindAsync();

            //switch (resp.Header.Status)
            //{
            //    case CommandStatus.ESME_ROK:
            //        //_log.Info("UnBind succeeded: Status: " + resp.Header.Status);
            //        break;
            //    default:
            //        //_log.Info("UnBind failed: Status: " + resp.Header.Status);
            //        await client.DisconnectAsync();
            //        break;
            //}

        }

        private void client_evDeliverSm(object sender, DeliverSm data)
        {
            try
            {
                //Check if we received Delivery Receipt
                if (data.MessageType == MessageTypes.SMSCDeliveryReceipt)
                {
                    //Get MessageId of delivered message
                    string messageId = data.Receipt.MessageId;
                    DateTime donedate = data.Receipt.DoneDate;
                    string deliveryStatus = Convert.ToString(data.Receipt.State);
                    string errcd = Convert.ToString(data.Receipt.ErrorCode);
                    Util ob = new Util();
                    //System.Threading.Thread t = new System.Threading.Thread(() =>
                    //{
                    //    ob.UpdateDelivery(messageId, donedate, deliveryStatus, data.Receipt.ToString(), errcd);
                    //});
                    //t.Start();


                    //_log.Info("Delivery Receipt received: " + data.Receipt.ToString());

                }
                else
                {
                    #region < Commented
                    /*
                    // Receive incoming message and try to concatenate all parts
                    if (data.Concatenation != null)
                    {
                        _messageComposer.AddMessage(data);

                        _log.Info("DeliverSm part received: Sequence: {0}, SourceAddress: {1}, Concatenation ( {2} )" +
                                " Coding: {3}, Text: {4}",
                                data.Header.Sequence, data.SourceAddress, data.Concatenation, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                    }
                    else
                    {
                        _log.Info("DeliverSm received : Sequence: {0}, SourceAddress: {1}, Coding: {2}, Text: {3}",
                            data.Header.Sequence, data.SourceAddress, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                    }

                    // Check if an ESME acknowledgement is required
                    if (data.Acknowledgement != SMEAcknowledgement.NotRequested)
                    {
                        // You have to clarify with SMSC support what kind of information they request in ESME acknowledgement.

                        string messageText = data.GetMessageText(_client.EncodingMapper);

                        var smBuilder = SMS.ForSubmit()
                            .From(data.DestinationAddress)
                            .To(data.SourceAddress)
                            .Coding(data.DataCoding)
                            .Concatenation(ConcatenationType.UDH8bit, _client.SequenceGenerator.NextReferenceNumber())
                            .Set(m => m.MessageType = MessageTypes.SMEDeliveryAcknowledgement)
                            .Text(new Receipt
                            {
                                DoneDate = DateTime.Now,
                                State = MessageState.Delivered,
                                //  MessageId = data.Response.MessageId,
                                ErrorCode = "0",
                                SubmitDate = DateTime.Now,
                                Text = messageText.Substring(0, Math.Min(20, messageText.Length))
                            }.ToString()
                            );



                        _client.SubmitAsync(smBuilder).ConfigureAwait(false);
                    }
                    */
                    #endregion
                }
            }
            catch (Exception ex)
            {
                data.Response.Header.Status = CommandStatus.ESME_RX_T_APPN;
                //_log.Info("Failed to process DeliverSm. " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void client_evDataSm(object sender, DataSm data)
        {
            //_log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Incomplete message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", Text: " + args.Text);
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Full message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", To: " + args.GetFirst<DeliverSm>().DestinationAddress + ", Text: " + args.Text);
        }

        private void client_evEnquireLink(object sender, EnquireLink data)
        {
            // _log.Info("EnquireLink received");
        }

        private void client_evUnBind(object sender, UnBind data)
        {
            //_log.Info("UnBind request received");
        }
        #endregion

        public void AddInMsgQueueAPIRabbitMQ(string userid, string sender, string m, string msg, string msgtype, string msgid, string sms_systemid, string peid, double rate,
            int noofsms, bool ucs, string templateid = "", string SubClientCode = "", string FailOver = "", string WABATemplateName = "", string WABAVariables = "",
            string emailFrom = "", string emailTo = "", string emailCC = "", string emailSubject = "", string FailOver2 = "")
        {
            try
            {
                string tablenm = "";
                if (msgtype == "15" || msgtype == "16" || msgtype == "19" || msgtype == "22" || msgtype == "20" || msgtype == "30" || msgtype == "40" || msgtype == "50")
                    tablenm = "MSGQUEUEB4single_UAE";
                else
                {
                    if (msgtype == "33")
                        tablenm = "MSGQUEUEB4single";
                    else
                        tablenm = "MSGQUEUEB4singleAPI";
                }

                var channel = RabbitMQSingleton.Instance.GetRabbitMQChannel();
                // Declare the queue
                channel.QueueDeclare(queue: tablenm, durable: true, exclusive: false, autoDelete: false, arguments: null);
                clsMsg ob = new clsMsg
                {
                    Provider = "",
                    SmppAccountId = sms_systemid,
                    ProfileId = userid,
                    MsgText = msg,
                    ToMobile = m,
                    SenderId = sender,
                    Createdat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    FileId = "1",
                    PeId = peid,
                    TemplateId = templateid,
                    SmsRate = rate,
                    NoOfSms = noofsms,
                    DataCode = ucs ? "UCS2" : "Default",
                    MsgIdClient = msgid
                };

                string jsonMessage = JsonConvert.SerializeObject(ob);
                var body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish(exchange: "", routingKey: tablenm, basicProperties: null, body: body);
                if (SubClientCode != "")
                {
                    addInMSGSubClientCode(userid, sender, m, msg, msgid, sms_systemid, peid, rate, noofsms, ucs, templateid, SubClientCode);
                }
                if (FailOver != "")
                {
                    AddInFailOver(userid, m, msg, msgid, "", FailOver, WABATemplateName, WABAVariables, emailFrom, emailTo, emailCC, emailSubject, FailOver2);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class smsCounter_CS
    {
        public int CalculateSmsCount(string text)
        {
            const int latinByteQuotaPerLargeSms = 153;
            const int non_latinByteQuotaPerLargeSms = 67;
            const int latinByteQuotaPerSms = 160;
            const int non_latinByteQuotaPerSms = 70;

            int count = 0;
            if (GSMChar(text))
            {
                count = text.Length <= latinByteQuotaPerSms ? 1 : ((int)Math.Ceiling((float)text.Length / latinByteQuotaPerLargeSms));
            }
            else
            {
                count = text.Length <= non_latinByteQuotaPerSms ? 1 : ((int)Math.Ceiling((float)text.Length / non_latinByteQuotaPerLargeSms));
            }
            return count;
        }
        private bool IsLatin(string text)
        {
            bool isLatin = false;
            byte[] textToBytes = Encoding.ASCII.GetBytes(text);
            isLatin = text.Length != textToBytes.Length ? false : true;
            return isLatin;
        }
        public bool GSMChar(string PlainText)
        {
            //derived from https://sites.google.com/site/freesmsuk/gsm7-encoding
            // ` is not a conversion, just a untranslatable letter
            string strGSMTable = "";
            strGSMTable += "@£$¥èéùìòÇ`Øø`Åå";
            strGSMTable += "Δ_ΦΓΛΩΠΨΣΘΞ`ÆæßÉ";
            strGSMTable += " !\"#¤%&'()*=,+-./";
            strGSMTable += "0123456789:;<=>?";
            strGSMTable += "¡ABCDEFGHIJKLMNO";
            strGSMTable += "PQRSTUVWXYZÄÖÑÜ`";
            strGSMTable += "¿abcdefghijklmno";
            strGSMTable += "pqrstuvwxyzäöñüà";

            string strExtendedTable = "";
            strExtendedTable += "````````````````";
            strExtendedTable += "````^```````````";
            strExtendedTable += "````````{}`````\\";
            strExtendedTable += "````````````[~]`";
            strExtendedTable += "|```````````````";
            strExtendedTable += "````````````````";
            strExtendedTable += "`````€``````````";
            strExtendedTable += "````````````````";

            string strGSMOutput = "";
            //foreach (char cPlainText in PlainText.ToCharArray())
            //{
            //    int intGSMTable = strGSMTable.IndexOf(cPlainText);
            //    if (intGSMTable != -1)
            //    {
            //        strGSMOutput += intGSMTable.ToString("X2");
            //        continue;
            //    }
            //    int intExtendedTable = strExtendedTable.IndexOf(cPlainText);
            //    if (intExtendedTable != -1)
            //    {
            //        strGSMOutput += (27).ToString("X2");
            //        strGSMOutput += intExtendedTable.ToString("X2");
            //    }
            //}
            foreach (char cPlainText in PlainText.ToCharArray())
            {
                int intGSMTable = strGSMTable.IndexOf(cPlainText);
                if (intGSMTable == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class smsCounter
    {
        public int GSM_CHARSET_7BIT = 0;
        public int GSM_CHARSET_UNICODE = 2;
        private char GSM_7BIT_ESC = '\u001b';

        private HashSet<string> GSM7BIT = new HashSet<string>() {
            "@", "£", "$", "¥", "è", "é", "ù", "ì", "ò", "Ç", "\n", "Ø", "ø", "\r", "Å", "å",
            "Δ", "_", "Φ", "Γ", "Λ", "Ω", "Π", "Ψ", "Σ", "Θ", "Ξ", "\u001b", "Æ", "æ", "ß", "É",
            " ", "!", "'", "#", "¤", "%", "&", "\"", "(", ")", "*", "+", ",", "-", ".", "/",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?",
            "¡", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
            "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Ä", "Ö", "Ñ", "Ü", "§",
            "¿", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o",
            "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "ä", "ö", "ñ", "ü", "à"
        };

        private HashSet<string> GSM7BITEXT = new HashSet<string>() { "\f", "^", "{", "}", "\\", "[", "~", "]", "|", "€" };

        public int getCharset(string content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (!GSM7BIT.Contains(content[i].ToString()))
                {
                    if (!GSM7BITEXT.Contains(content[i].ToString()))
                    {
                        return GSM_CHARSET_UNICODE;
                    }
                }
            }
            return GSM_CHARSET_7BIT;
        }

        private int getPartCount7bit(String content)
        {
            StringBuilder content7bit = new StringBuilder();

            // Add escape characters for extended charset
            for (int i = 0; i < content.Length; i++)
            {
                if (!GSM7BITEXT.Contains(content[i].ToString() + ""))
                {
                    content7bit.Append(content[i].ToString());
                }
                else
                {
                    content7bit.Append('\u001b');
                    content7bit.Append(content[i]);
                }
            }

            if (content7bit.Length <= 160)
            {
                return 1;
            }
            else
            {
                // Start counting the number of messages
                int parts = (int)Math.Ceiling(content7bit.Length / 153.0);
                int free_chars = content7bit.Length - (int)Math.Floor(content7bit.Length / 153.0) * 153;

                // We have enough free characters left, don't care about escape character at the end of sms part
                if (free_chars >= parts - 1)
                {
                    return parts;
                }

                // Reset counter
                parts = 0;
                while (content7bit.Length > 0)
                {
                    // Advance sms counter
                    parts++;

                    // Check for trailing escape character
                    if (content7bit.Length >= 152 && content7bit[152] == GSM_7BIT_ESC)
                    {
                        content7bit.Remove(0, 152);
                    }
                    else
                    {
                        if (content7bit.Length >= 153)
                            content7bit.Remove(0, 153);
                        else
                            content7bit.Remove(0, content7bit.Length);
                    }
                }
                return parts;
            }
        }

        public int CalculateSmsCount(string content)
        {
            //return content.length();
            int charset = getCharset(content);
            if (charset == GSM_CHARSET_7BIT)
            {
                return getPartCount7bit(content);
            }
            else if (charset == GSM_CHARSET_UNICODE)
            {
                if (content.Length <= 70)
                {
                    return 1;
                }
                else
                {
                    return (int)Math.Ceiling(content.Length / 67.0);
                }
            }
            return -1;
        }
       
    }

    public class clsMsg
    {
        public string Provider { get; set; }
        public string SmppAccountId { get; set; }
        public string ProfileId { get; set; }
        public string MsgText { get; set; }
        public string ToMobile { get; set; }
        public string SenderId { get; set; }
        public string Createdat { get; set; }
        public string FileId { get; set; }
        public string PeId { get; set; }
        public string TemplateId { get; set; }
        public double SmsRate { get; set; }
        public int NoOfSms { get; set; }
        public string DataCode { get; set; }
        public string MsgIdClient { get; set; }
    }
}
