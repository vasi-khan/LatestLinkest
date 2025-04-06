using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;

using Newtonsoft.Json;
using RestSharp;
using System.Data.SqlClient;
using System.Net.Mail;

namespace API_DLR_RECEIPT
{
    public class Util
    {
        
        public string sql = "";
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public int LogErr = 1;
        public static DataTable tblerror = null;
        public static DataTable dtDLRCallBackCust;
        public void Info(string msg, string client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        } //{ LogError(msg); }

        public void Info2(string msg) { LogError(msg); }

        public void Info_Client(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void Info_Submit(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogSubmit_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_LogSubmit_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void Info_Failed(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogFailed_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_LogFailed_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                { }
            }
        }


        public void LogError(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Conn_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_Conn_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception e) { }
            }
        }
        public void LogDlvError(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Dlv_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"Dlv_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception e4)
                { }
            }
        }
        public string GetFileHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public DataTable GetDLRCallBackCustomers()
        {
            DataTable dt = database.GetDataTable("Select UserName from Customer where DLRPushHookAPI <> ''");
            return dt;
        }
        public DataTable GetSMPPAccountsAPI()
        {
            sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.ACTIVE=1 and n.active=1 and s.BindingMode='Receiver' order by n.sessionid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable getCallBackAPICustomers()
        {
            DataTable dt = new DataTable();

            sql = @"Select Username,DLRPushHookAPI,isnull(dlrHookApiHeader1,'')dlrHookApiHeader1,isnull(dlrHookApiHeader1val,'')dlrHookApiHeader1val,
            isnull(dlrHookApiHeader2, '')dlrHookApiHeader2,isnull(dlrHookApiHeader2val, '')dlrHookApiHeader2val,isnull(dlrHookApiHeader3, '')dlrHookApiHeader3,
            isnull(dlrHookApiHeader3val, '')dlrHookApiHeader3val from customer with(nolock) where active= 1 and DLRPushHookAPI<>'' ";
            try
            {
                dt = databasePanel.GetDataTable(sql);
            }
            catch (Exception e) { }
            return dt;
        }

        public DataTable GetSMSRecords(int acId, int no_of_sms)
        {
            string sender = @"declare @sender varchar(6) 
                            select top 1 @sender = SENDERID from MSGTRAN where SMPPACCOUNTID = '" + acId + "' AND picked_datetime IS NULL  group by SENDERID ORDER BY MIN(CREATEDAT) ";
            string s = " select top " + no_of_sms.ToString() + " * from MSGTRAN where SMPPACCOUNTID = '" + acId + "' AND senderid = @sender and picked_datetime IS NULL ORDER BY CREATEDAT";
            sql = @"
declare @cnt numeric(7)
select @cnt=count(*) from MSGTRAN WHERE PICKED_DATETIME IS NULL
IF @cnt >0 
BEGIN
                    BEGIN TRY
                        BEGIN TRANSACTION; " +
                            sender + s + "; " + " ;WITH CTE AS  (" + s + ") UPDATE CTE SET picked_datetime=getdate() " +
                        @" COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                    END CATCH 
END";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void AddInMsgSubmitted(string msgid, DataRow dr, DateTime sendTime)
        {
            try
            {
                sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                try
                {
                    string st = "";
                    if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                    else st = ex.StackTrace;
                    sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,ERRMSG,FILEID) " +
                        "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2) { }
            }
        }

        public void AddInMsgSubmittedInvalidSender(string msgid, DataRow dr, DateTime sendTime)
        {
            try
            {
                sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,FILEID) " +
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
                try
                {
                    string st = "";
                    if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
                    else st = ex.StackTrace;
                    sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,ERRMSG,FILEID) " +
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
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex2) { }
            }
        }

        public void RemoveFromMsgTran(string SMPPAccountID)
        {
            sql = "Delete from msgtran where SMPPACCOUNTID='" + SMPPAccountID + "' and picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }

        public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt, string errcd, string MCCMNC, string sid, string destno, string dlrCallBack)
        {
            string d = "";
            try
            {
                if (txt.Contains(" done date:"))
                {
                    int p = txt.IndexOf(" done date:") + 11;
                    d = txt.Substring(p, 12);
                    //if (d.Substring(10, 2).ToUpper() == " S") d.ToUpper().Replace(" S", "00");
                    if (d.Substring(10, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    if (d.Substring(d.Length - 2, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    d = "20" + d.Substring(0, 2) + "-" + d.Substring(2, 2) + "-" + d.Substring(4, 2) + " " + d.Substring(6, 2) + ":" + d.Substring(8, 2) + ":" + d.Substring(10, 2);
                }
            }
            catch (Exception ex)
            {
                d = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            }
            string msgid2 = "", templateid = "", peid = "", msgtext = "", profileid = "", sentdatetime = DateTime.Now.ToString();
            try
            {
                string sql;
                
                // 04/07/2022
                // msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));

                DataTable dtDl = database.GetDataTable("select top 1 isnull(msgid,'') msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'");
                if (dtDl.Rows.Count > 0)
                {
                    msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                    templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                    peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                    msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                    profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                    sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                }

                if (msgid2 != "")
                {
                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                        "values ('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                    if (drCallBack.Length != 0)
                        sql = sql + "; Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP28" : "") + " (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + profileid + "','" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                }
                else
                {
                    dtDl = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                    if (dtDl.Rows.Count > 0)
                    {
                        msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                        templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                        peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                        msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                        profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                        sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                    }

                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                        "values ('" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                    if (drCallBack.Length != 0)
                        sql = sql + "; Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP28" : "") + " (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                        "values ('" + profileid + "','" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                }
                database.ExecuteNonQuery(sql);
                //LogCBErr("b4 1");
                // Rabi for template block email 05/07/2022
                string sqlerr = "";
                string w = "0";
                try
                {
                    
                    DataRow[] founderror = tblerror.Select("err_code = '" + errcd + "'");
                    w = "1";
                    if (founderror.Length != 0)
                    {
                        w = "1.1";
                        if (dtDl.Rows.Count==0)
                        {
                            w = "1.2";
                            DataTable dtDl1 = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                            if (dtDl1.Rows.Count > 0)
                            {
                                w = "1.3";
                                msgid2 = Convert.ToString(dtDl1.Rows[0]["msgid"]);
                                templateid = Convert.ToString(dtDl1.Rows[0]["templateid"]);
                                peid = Convert.ToString(dtDl1.Rows[0]["peid"]);
                                msgtext = Convert.ToString(dtDl1.Rows[0]["msgtext"]);
                                profileid = Convert.ToString(dtDl1.Rows[0]["profileid"]);
                                sentdatetime = Convert.ToString(dtDl1.Rows[0]["sentdatetime"]);
                            }
                        }

                        w = "2";
                        sqlerr = "Insert into errorlog (dlrtime,msgId,mobileno,senderid,templateid,peid,msgtext,profileid,senddate,errorcode,errordesc,errortype) " +
                        "values ('" + d + "','" + msgId + "','" + destno + @"','" +
                        sid + "','" + templateid + "','" + peid + "','" + msgtext.Replace("'", "''") + "','" + profileid + "','" + Convert.ToDateTime(sentdatetime).ToString("dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "','" + errcd + "','" + founderror[0]["descreption"].ToString() + "','')";
                        w = "3";
                        database.ExecuteNonQuery(sqlerr);
                        w = "4";
                        //UpdateOpenTemplate();
                        send_errormail(d, msgId, msgid2, destno, sid, templateid, peid, msgtext.Replace("'", "''"), profileid, sentdatetime, errcd, founderror[0]["descreption"].ToString());
                        w = "5";
                    }
                }
                catch (Exception ex3)
                {
                    LogDlvError("Dlv Err 8 - " + ex3.Message + " - " + ex3.StackTrace + " W=" + w.ToString() + " SQL- " + sqlerr + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    //throw;
                }
                //LogCBErr("b4 - w " + w);
                if (profileid == "MIM2300228")
                {
                    //LogCBErr("b44 - w " + w);
                    d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //LogCBErr("b444 - w " + w);
                    processCallBack(dlvStat, d, (msgid2 != "" ? msgid2 : msgId), errcd, profileid, 0);
                    //LogCBErr("b4444 - w " + w);
                }
            }
            catch (Exception ex2)
            {
                try
                {
                    LogDlvError("Dlv Err - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    string sql;

                    //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                    DataTable dtDl = database.GetDataTable("select top 1 isnull(msgid,'') msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'");
                    if (dtDl.Rows.Count > 0)
                    {
                        msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                        templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                        peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                        msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                        profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                        sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                    }
                    if (msgid2 != "")
                    {
                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                        DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                        if (drCallBack.Length != 0)
                            sql = sql + "; Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP28" : "") + " (PROFILEID, DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + profileid + "','" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    }
                    else
                    {
                        dtDl = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                        if (dtDl.Rows.Count > 0)
                        {
                            msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                            templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                            peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                            msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                            profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                            sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                        }

                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                        DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                        if (drCallBack.Length != 0)
                            sql = sql + "; Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP28" : "") + " (PROFILEID, DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + profileid + "','" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    }
                    database.ExecuteNonQuery(sql);
                    //LogCBErr("b4 2");
                    // Rabi for template block email 05/07/2022
                    try
                    {
                        DataRow[] founderror = tblerror.Select("err_code = '" + errcd + "'");
                        if (founderror.Length != 0)
                        {
                            if (dtDl.Rows.Count == 0)
                            {
                                DataTable dtDl1 = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                                if (dtDl1.Rows.Count > 0)
                                {
                                    msgid2 = Convert.ToString(dtDl1.Rows[0]["msgid"]);
                                    templateid = Convert.ToString(dtDl1.Rows[0]["templateid"]);
                                    peid = Convert.ToString(dtDl1.Rows[0]["peid"]);
                                    msgtext = Convert.ToString(dtDl1.Rows[0]["msgtext"]);
                                    profileid = Convert.ToString(dtDl1.Rows[0]["profileid"]);
                                    sentdatetime = Convert.ToString(dtDl1.Rows[0]["sentdatetime"]);
                                }
                            }

                            string sqlerr = "Insert into errorlog (dlrtime,msgId,mobileno,senderid,templateid,peid,msgtext,profileid,senddate,errorcode,errordesc,errortype) " +
                            "values ('" + d + "','" + msgId + "','" + destno + @"','" +
                            sid + "','" + templateid + "','" + peid + "','" + msgtext.Replace("'", "''") + "','" + profileid + "','" + Convert.ToDateTime(sentdatetime).ToString("dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "','" + errcd + "','" + founderror[0]["descreption"].ToString() + "','')";
                            database.ExecuteNonQuery(sqlerr);
                            //UpdateOpenTemplate();
                            send_errormail(d, msgId, msgid2, destno, sid, templateid, peid, msgtext.Replace("'", "''"), profileid, sentdatetime, errcd, founderror[0]["descreption"].ToString());
                        }
                    }
                    catch (Exception ex3)
                    {
                        LogDlvError("Dlv Err 9 - " + ex3.Message + " - " + ex3.StackTrace + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    }
                    //LogCBErr("b4 3 " + profileid);
                    //processCallBack(dlvStat, d, (msgid2 != "" ? msgid2 : msgId), errcd);
                    if (profileid == "MIM2300228")
                    {
                        //LogCBErr("b4 4");
                        d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //LogCBErr("b4 5");
                        processCallBack(dlvStat, d, (msgid2 != "" ? msgid2 : msgId), errcd, profileid, 0);
                        //LogCBErr("b4 6");
                    }
                }
                catch (Exception ex3)
                {
                    LogDlvError("Dlv Err 7 - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                }
            }
        }

        #region <CALLBACK METHODS >
        public void processCallBack(string dlvStat, string d, string msgId, string errcd, string profileid, Int32 retry)
        {
            //process callback ------- >>>>
            //LogCBErr("CNT - " + clsCheck.dtDLRPushAPI.Rows.Count.ToString());
            if (clsCheck.dtDLRPushAPI.Rows.Count > 0)
            {
                //LogCBErr("b1");
                DataRow[] dr = clsCheck.dtDLRPushAPI.Select("UserName = '" + profileid + "'");
                string SubClientCode = "";
                if (profileid.ToUpper() == "MIM2300228" || profileid.ToUpper() == "MIM2300229")
                {
                    //LogCBErr("b2");
                    try { SubClientCode = Convert.ToString(database.GetScalarValue(@"select isnull(SubClientCode,'') SubClientCode from MSGSubClientCode with(nolock) where msgidClient='" + msgId + "'")); }
                    catch (Exception ee) { LogCBErr("Getting SubClientCode " + ee.Message); }
                }
                //LogCBErr("b3");
                if (dr.Length > 0)
                {
                    //LogCBErr("b4");
                    if (retry <= 6)
                        dlrCallBackAPI(dr, dlvStat, d, msgId, errcd, retry, SubClientCode, profileid);
                    else
                        LogAPIResp(msgId + " - after 7 retrials failed.");
                }
            }
            
        }
        public void dlrCallBackAPI(DataRow[] dr, string dlvStat, string d, string msgId, string errcd, Int32 retry, string SubClientCode = "", string profileid = "")
        {
            string url = Convert.ToString(dr[0]["DLRPushHookAPI"]);
            string dlrHookApiHeader1 = Convert.ToString(dr[0]["dlrHookApiHeader1"]);
            string dlrHookApiHeader1val = Convert.ToString(dr[0]["dlrHookApiHeader1val"]);
            string dlrHookApiHeader2 = Convert.ToString(dr[0]["dlrHookApiHeader2"]);
            string dlrHookApiHeader2val = Convert.ToString(dr[0]["dlrHookApiHeader2val"]);
            string dlrHookApiHeader3 = Convert.ToString(dr[0]["dlrHookApiHeader3"]);
            string dlrHookApiHeader3val = Convert.ToString(dr[0]["dlrHookApiHeader3val"]);
            string ClientCodes = "";
            int x = 1;
            try
            {
                LogCBErr("b5");
                var client = new RestClient(url);
                x = 2;
                client.Timeout = 5000;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
                //request.AddHeader("Authorization", "" + authkey + "");

                if (dlrHookApiHeader1 != "") request.AddHeader(dlrHookApiHeader1, dlrHookApiHeader1val);
                if (dlrHookApiHeader2 != "") request.AddHeader(dlrHookApiHeader2, dlrHookApiHeader2val);
                if (dlrHookApiHeader3 != "") request.AddHeader(dlrHookApiHeader3, dlrHookApiHeader3val);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                x = 3;

                if (SubClientCode != "")
                {
                    ClientCodes = @", ""SUBCLIENTCODE"": """ + SubClientCode + @"""";
                }
                if (errcd == "000") errcd = "0000";
                var body = @"{
                          ""STATUS"": """ + dlvStat + @""",
                          ""STATUSDATETIME"": """ + d + @""",
                          ""MSGID"": """ + msgId + @""",
                          ""ERRORCODE"": """ + errcd + @"""" + ClientCodes + @"
                        }";
                x = 44;
                if (profileid == "MIM2301076")
                    body = @"{
                          ""Status"": """ + errcd + @""",
                          ""MessageId"": """ + msgId + @""",
                          ""Reason"": """ + dlvStat + @"""" + ClientCodes + @"
                        }";
                x = 4;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                x = 5;
                string stt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
                IRestResponse response = client.Execute(request);
                string ent = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
                x = 6;

                string scod = "";
                try { scod = Convert.ToString(Convert.ToInt16(response.StatusCode)); } catch (Exception ee) { }

                if (scod != "200")
                {
                    insertCallBackAPICalled_ERR(dlvStat, d, msgId, errcd, SubClientCode, profileid, url, retry + 1, " Started-" + stt + " Done-" + ent + " StatusCode=" + scod + " StatusText=" + Convert.ToString(response.StatusCode) + " Response-" + Convert.ToString(response.Content));
                }
                else
                {
                    ////WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
                    //Root res = JsonConvert.DeserializeObject<Root>(response.Content);
                    //x = 7;
                    insertCallBackAPICalled(dlvStat, d, msgId, errcd, SubClientCode, profileid, url, retry + 1, Convert.ToString(response.Content));
                }
                LogErrDlrCallBack(url, body, " Started-" + stt + " Done-" + ent + " StatusCode=" + scod + " StatusText=" + Convert.ToString(response.StatusCode) + " Response-" + Convert.ToString(response.Content));
                x = 8;
            }
            catch (Exception EX)
            {
                LogCBErr(EX.Message + " - " + EX.StackTrace + " . URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString());
            }
        }

        public void processCallBack_OLD_NOT_IN_USE(string dlvStat, string d, string msgId, string errcd)
        {
            //process callback ------- >>>>
            if (clsCheck.dlrCallBackApplicable)
            {
                if (clsCheck.dtDLRPushAPI.Rows.Count > 0)
                {
                    string profileid = "";
                    profileid = Convert.ToString(database.GetScalarValue("Select TOP 1 ProfileID from Msgsubmitted where msgid ='" + msgId + "'"));
                    DataRow[] dr = clsCheck.dtDLRPushAPI.Select("UserName = '" + profileid + "'");
                    if (dr.Length > 0)
                    {
                        dlrCallBackAPI_OLD_NOT_IN_USE(dr[0]["DLRPushHookAPI"].ToString(), dlvStat, d, msgId, errcd);
                    }
                }
            }
        }
        public void dlrCallBackAPI_OLD_NOT_IN_USE(string url, string dlvStat, string d, string msgId, string errcd)
        {
            int x = 1;
            try
            {
                var client = new RestClient(url);
                x = 2;
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
                //request.AddHeader("Authorization", "" + authkey + "");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                x = 3;
                var body = @"{
                          ""STATUS"": """ + dlvStat + @""",
                          ""STATUSDATETIME"": """ + d + @""",
                          ""MSGID"": """ + msgId + @""",
                          ""ERRORCODE"": """ + errcd + @"""
                        }";
                x = 4;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                x = 5;
                IRestResponse response = client.Execute(request);
                x = 6;
                //WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
                Root res = JsonConvert.DeserializeObject<Root>(response.Content);
                x = 7;

                LogErrDlrCallBack(url, body, res.STATUS);
                x = 8;
            }
            catch (Exception EX)
            {
                LogCBErr(EX.Message + " URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString());
            }
        }

        public void LogErrDlrCallBack(string url, string body, string status)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"DLRCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"DLRcatch1_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception e1)
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"DLRcatch2_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2)
                    {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"DLRcatch3_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3)
                        {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"DLRcatch4_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4)
                            {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"DLRcatch5_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                    StreamWriter strmwriter = new StreamWriter(filestrm);
                                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                                    strmwriter.Flush();
                                    strmwriter.Close();
                                }
                                catch (Exception e5) { }
                            }
                        }
                    }
                }
            }
        }
        public void LogCBErr(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"ErrCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"ErrCatch1_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception e1)
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"ErrCatch2_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2)
                    {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"ErrCatch3_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3)
                        {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"ErrCatch4_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4)
                            {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"ErrCatch5_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                    StreamWriter strmwriter = new StreamWriter(filestrm);
                                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                    strmwriter.Flush();
                                    strmwriter.Close();
                                }
                                catch (Exception e5) { }
                            }
                        }
                    }
                }
            }
        }
        public void LogAPIResp(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"ApiErr_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"ApiErrCh1_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception e1)
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"ApiErrCh2_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2)
                    {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"ApiErrCh3_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3)
                        {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"ApiErrCh4_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4)
                            {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"ApiErrCh5_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                    StreamWriter strmwriter = new StreamWriter(filestrm);
                                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                    strmwriter.Flush();
                                    strmwriter.Close();
                                }
                                catch (Exception e5) { }
                            }
                        }
                    }
                }
            }
        }
        public void insertCallBackAPICalled(string dlvStat, string d, string msgId, string errcd, string SubClientCode, string profileid, string url, Int32 retry, string response)
        {
            string sql = "";
            try
            {
                if (response.Length > 4000) response = response.Substring(0, 3998);
                sql = "insert into deliverycallbackcalled (MSGID,DLVRTIME,DLVRSTATUS,err_code,SUBCLIENTCODE,PROFILEID,URL,APIRESP,retry) " +
                    "values ('" + msgId + "','" + d + "','" + dlvStat + "','" + errcd + "','" + SubClientCode + "','" + profileid + "','" + url + "','" + response + "','" + Convert.ToString(retry) + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("insert into deliverycallbackcalled - " + ex.Message); }
            }
        }
        public void insertCallBackAPICalled_ERR(string dlvStat, string d, string msgId, string errcd, string SubClientCode, string profileid, string url, Int32 retry, string response)
        {
            string sql = "";
            try
            {
                try
                {
                    if (profileid == "MIM2300228" || profileid == "MIM2300229")
                    {
                        sql = "Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP" : "") + " (PROFILEID,MSGID,DLVRTIME,DLVRSTATUS,err_code,retry) values " +
                                "('" + profileid + "','" + msgId + "','" + d + "','" + dlvStat + "','" + errcd + "','" + Convert.ToString(retry) + "')";
                        database.ExecuteNonQuery(sql);
                    }
                }
                catch (Exception e1) { }

                if (response.Length > 4000) response = response.Substring(0, 3998);
                sql = "insert into deliverycallbackcalled_ERR (MSGID,DLVRTIME,DLVRSTATUS,err_code,SUBCLIENTCODE,PROFILEID,URL,APIRESP,retry) " +
                    "values ('" + msgId + "','" + d + "','" + dlvStat + "','" + errcd + "','" + SubClientCode + "','" + profileid + "','" + url + "','" + response + "','" + Convert.ToString(retry) + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("insert into deliverycallbackcalled_ERR - " + ex.Message); }
            }
        }
        #endregion

        public void UpdateDeliveryQueue(string msgId, DateTime donedate, string dlvStat, string txt, string errcd, string MCCMNC, string sid, string destno)
        {
            string d = "";
            try
            {
                if (txt.Contains(" done date:"))
                {
                    int p = txt.IndexOf(" done date:") + 11;
                    d = txt.Substring(p, 12);
                    //if (d.Substring(10, 2).ToUpper() == " S") d.ToUpper().Replace(" S", "00");
                    if (d.Substring(10, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    if (d.Substring(d.Length - 2, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    d = "20" + d.Substring(0, 2) + "-" + d.Substring(2, 2) + "-" + d.Substring(4, 2) + " " + d.Substring(6, 2) + ":" + d.Substring(8, 2) + ":" + d.Substring(10, 2);
                }
            }
            catch (Exception ex)
            {
                d = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            }

            try
            {
                string sql;
                string msgid2 = "";
                msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                if (msgid2 != "")
                    sql = "Insert into DELIVERYQUEUE (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                        "values ('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                else
                    sql = "Insert into DELIVERYQUEUE (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                        "values ('" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex2)
            {
                try
                {
                    LogDlvError("Dlv Err - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    string sql;
                    string msgid2 = "";
                    msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                    if (msgid2 != "")
                        sql = "Insert into DELIVERYQUEUE (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    else
                        sql = "Insert into DELIVERYQUEUE (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) " +
                            "values ('" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex3)
                {
                    LogDlvError("Dlv Q Err 1 - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                }
            }
        }

        public void send_errormail(string dlrtime, string msgId, string msgClientId, string mobileno, string senderid, string templateid, string peid, string msgtext, string profileid, string senddate, string errorcode, string errordesc)
        {
            StringBuilder sb = new StringBuilder();
            List<string> To = new List<string>();
            List<string> CC = new List<string>();

            DataTable dt = database.GetDataTable("SELECT * FROM [SettingMast] with (nolock)");

            DataTable dtTo = database.GetDataTable(@"select e.EmailId BDEmailId,r.EmailId ManagerEmailId,COMPNAME from emimpanel..customer c with (nolock) 
join emimpanel..employee e with (nolock) on c.EmpCode=e.EmployeeCode 
left join emimpanel..employee r with (nolock) on e.ReportToId=r.EmployeeCode and e.ReportToId is not null
where username='" + profileid + "'");

            
            string Subject = errorcode + " " + errordesc;
            Subject = Subject.Replace('\r', ' ').Replace('\n', ' ');

            sb.Append("<b>dlrtime :</b>" + dlrtime + "<br>");
            sb.Append("<b>msgId :</b>" + msgId + "<br>");
            sb.Append("<b>msgClientId :</b>" + msgClientId + "<br>");
            sb.Append("<b>senderid :</b>" + senderid + "<br>");
            sb.Append("<b>templateid :</b>" + templateid + "<br>");
            sb.Append("<b>peid :</b>" + peid + "<br>");
            sb.Append("<b>msgtext :</b>" + msgtext + "<br>");
            sb.Append("<b>profileid :</b>" + profileid + "<br>");
            sb.Append("<b>senddate :</b>" + senddate + "<br>");
            sb.Append("<b>errorcode :</b>" + errorcode + "<br>");
            sb.Append("<b>errordesc :</b>" + errordesc + "<br><br><br>");

            sb.Append("This is a system generated email.<br><br>");
            sb.Append("Generated at : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));

            if (dtTo.Rows.Count > 0)
            {
                if (dtTo.Rows[0]["BDEmailId"] != null && dtTo.Rows[0]["BDEmailId"].ToString() != "")
                {
                    To.Add(dtTo.Rows[0]["BDEmailId"].ToString().Trim());
                }
                if (dtTo.Rows[0]["ManagerEmailId"] != null && dtTo.Rows[0]["ManagerEmailId"].ToString() != "")
                {
                    To.Add(dtTo.Rows[0]["ManagerEmailId"].ToString().Trim());
                }

                if(To.Count<1) To.Add(dt.Rows[0]["CC1"].ToString().Trim());
            }
            else
            {
                To.Add(dt.Rows[0]["CC1"].ToString().Trim());
            }
               
            if (dt.Rows.Count > 0)
              {
                if (dt.Rows[0]["CC1"] != null && dt.Rows[0]["CC1"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC1"].ToString().Trim());
                }
                if (dt.Rows[0]["CC2"] != null && dt.Rows[0]["CC2"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC2"].ToString().Trim());
                }
                if (dt.Rows[0]["CC3"] != null && dt.Rows[0]["CC3"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC3"].ToString().Trim());
                }
                if (dt.Rows[0]["CC4"] != null && dt.Rows[0]["CC4"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC4"].ToString().Trim());
                }
                if (dt.Rows[0]["CC5"] != null && dt.Rows[0]["CC5"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC5"].ToString().Trim());
                }
            }
            if (To.Count>0)
            {
                string str = SendEmail_( Subject,sb.ToString(), dt.Rows[0]["UserId"].ToString().Trim(), dt.Rows[0]["Password"].ToString().Trim(), dt.Rows[0]["host"].ToString().Trim(), dt.Rows[0]["PortNo"].ToString().Trim(), To, CC);
            }
            
        }

        public string SendEmail(string toAddress, string subject, string body, string UserId, string Pwd, string Host, string Port, List<string> CC)
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
                
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                LogDlvError_mail("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public string SendEmail_(string subject, string body, string UserId, string Pwd, string Host, string Port, List<string> To, List<string> CC, List<string> BCC=null)
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

                //MailMessage message = new MailMessage();
            
                MailMessage message = new MailMessage(senderID, Convert.ToString(To[0]), subject, body);

                message.IsBodyHtml = true;

                if (To != null)
                {
                    for (int i = 1; i < To.Count; i++)
                    {
                        message.To.Add(To[i]);
                    }
                }

                if (CC!=null)
                {
                    for (int i = 0; i < CC.Count; i++)
                    {
                        message.CC.Add(CC[i]);
                    }
                }

                if (BCC != null)
                {
                    for (int i = 0; i < BCC.Count; i++)
                    {
                        message.Bcc.Add(BCC[i]);
                    }
                }

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                LogDlvError_mail("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }


        public void LogDlvError_mail(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"EMAIL_Dlv_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"EMAIL_Dlv_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2) { }
            }
        }
        public void UpdateOpenTemplate()
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cnn.Open();
                    cmd.CommandTimeout = 100;
                    cmd.Connection = cnn;
                    cmd.CommandText = "SP_UpdateOpenTemplate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        public string SendEmail(string MailFrom, string Pwd, string Host, string toAddress, string subject, string body, List<string> CC = null)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom;
            string senderPassword = Pwd;
            try
            {
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
                if (CC != null && CC.Count > 0)
                {
                    for (int i = 0; i < CC.Count; i++)
                    {
                        message.CC.Add(CC[i]);
                    }
                }
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
            }
            return result;
        }
    }

    public class Root
    {
        public string STATUS { get; set; }
    }
}
