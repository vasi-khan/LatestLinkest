using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using eMIMPanel.Helper;
using System.Net.Mail;

namespace SMS_SCHEDULER
{
    public class Util
    {
        int addMinutes = 0;
        public string sql = "";
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public int LogErr = 1;
        public void Info(string msg) { LogError(msg); }
        public void LogError(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {

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

        public DataTable GetSMPPAccounts()
        {
            sql = "Select * from smppsetting where active=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        //public string SendEmail(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCMailId)
        //{
        //    string result = "Message Sent Successfully..!!";
        //    string senderID = MailFrom; // "info@emim.in";
        //    string senderPassword = Pwd; // "info";
        //    try
        //    {
        //        //Host = "mymail2889.com",

        //        SmtpClient smtp = new SmtpClient
        //        {
        //            Host = Host,
        //            Port = 25,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
        //            Timeout = 30000,
        //        };

        //        MailMessage message = new MailMessage(senderID, toAddress, subject, body);
        //        //message.CC.Add("dsng25@gmail.com");
        //        message.CC.Add(CCMailId);
        //        // message.CC.Add("support@myinboxmedia.com");

        //        //Attachment item = new Attachment(fn);
        //        //message.Attachments.Add(item);
        //        //smtp.EnableSsl = false;
        //        //smtp.UseDefaultCredentials = false;
        //        smtp.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Error sending email.!!! " + ex.Message;
        //        //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
        //    }
        //    return result;
        //}
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
        public string SendEmail(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCMailId, string Path, string path1, string path2, string path3, string path4, string path5, string path6)
        {
            string newbody = body + @"
                " + "Linkext Report: "
                + Path + @"
                " + "Honda Report: "
                + path1 + @" 
                " + "Hyundai Report: "
                + "HyundaiSales : "
                + path2 + @"
                " + "HyundaiService : "
                + path3 + @" 
                " + "HyundaiDealerSales : "
                + path4 + @" 
                " + "HyundaiDealerService : "
                + path5 + @"
                " + "HyundaiHASC : "
                + path6;
            string Port = "25";
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
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
                //for (int i = 0; i < CC.Count; i++)
                //{
                //    message.CC.Add(CC[i]);
                //}
                message.CC.Add(CCMailId);

                // message.CC.Add("support@myinboxmedia.com");

                //if (Path != "" && path1!="" && path2!="" && path3!="" && path4!="" && path5!="" && path6!="")
                //{
                //    Attachment item = new Attachment(Path);
                //    message.Attachments.Add(item);
                //    Attachment item1 = new Attachment(path1);
                //    message.Attachments.Add(item1);
                //    Attachment item2 = new Attachment(path2);
                //    message.Attachments.Add(item2);
                //    Attachment item3 = new Attachment(path3);
                //    message.Attachments.Add(item3);
                //    Attachment item4 = new Attachment(path4);
                //    message.Attachments.Add(item4);
                //    Attachment item5 = new Attachment(path5);
                //    message.Attachments.Add(item5);
                //    Attachment item6 = new Attachment(path5);
                //    message.Attachments.Add(item6);
                //}

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);

                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public string SendEmailExeNotification(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCMailId)
        {
            string newbody = body;
            string Port = "25";
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
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
                //for (int i = 0; i < CC.Count; i++)
                //{
                //    message.CC.Add(CC[i]);
                //}
                message.CC.Add(CCMailId);
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }
        public DataTable GetSMSRecords(int acId, int no_of_sms)
        {
            string s = "select top " + no_of_sms.ToString() + " * from MSGTRAN where SMPPACCOUNTID = '" + acId + "' AND picked_datetime IS NULL ORDER BY CREATEDAT";
            sql = @"BEGIN TRY
                        BEGIN TRANSACTION; " +
                        s + "; " + " ;WITH CTE AS  (" + s + ") UPDATE CTE SET picked_datetime=getdate() " +
                        @" COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                    END CATCH ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void AddInMsgSubmitted(string msgid, DataRow dr, DateTime sendTime)
        {
            sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID) " +
                "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "')";
            database.ExecuteNonQuery(sql);
        }

        public void RemoveFromMsgTran(string SMPPAccountID)
        {
            sql = "Delete from msgtran where SMPPACCOUNTID='" + SMPPAccountID + "' and picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }

        public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt)
        {
            sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('" + txt + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss") + "','" + dlvStat + "')";
            database.ExecuteNonQuery(sql);
        }

        public DataTable GetRequestForReport()
        {
            sql = "select * From DLRRequest where ISNULL(Generatedtime,'')=''";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetCustomerForReport()
        {
            sql = "select * From Customer with(nolock)  WHERE ReportonEmail=1 and isnull(ccemail,'')<>'' ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetSMSReport_user_newConsolidatedDETAIL(string f, string t, string user, string fileId, bool IsTdy)
        {

            string str = "";
            int SHOWMOBILEXXXX = Convert.ToInt16(database.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + user + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";
            string sql = "";

            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + user + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry with (nolock) where counryCode in(" + q1 + ")"));


            sql = @"select convert(varchar,sentdatetime,102) as SMSdate,ISNULL(s.campaignname,'') CampaignName,Convert(varchar,m.msgid) as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
DATEADD(MINUTE," + timedifferenceInMinute + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
case when d.insertdate is null then '' else DATEADD(MINUTE," + timedifferenceInMinute + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, Replace(Replace(isnull(d.dlvrtext,''),CHAR(10),''),CHAR(13),'') as RESPONSE FROM MSGSUBMITTED" + (IsTdy == false ? "Log" : "") + @"  m with (nolock) 
             left join DELIVERY" + (IsTdy == false ? "Log" : "") + @" d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
             left join SMSFILEUPLOAD s with (nolock) ON m.FILEID=s.ID
             where m.PROFILEID='" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
            if (fileId != "") sql = sql + @"and m.FILEID='" + fileId + "'";

            sql = sql + @" order by m.sentdatetime";

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport_user_newConsolidatedDETAIL_B4(string f, string t, string user, string fileId)
        {
            string str = "";
            int SHOWMOBILEXXXX = Convert.ToInt16(databaseB4.GetScalarValue("Select isnull(showmobilexxxx,0) from customer where username='" + user + "' "));
            if (SHOWMOBILEXXXX == 1) str = "' ' + left(convert(varchar, m.tomobile), len(convert(varchar, m.tomobile)) - 4) + 'XXXX'";
            else str = "convert(varchar,m.tomobile)";
            string sql = "";

            string q1 = "select defaultCountry from CUSTOMER with(nolock) where username='" + user + "' ";
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry with (nolock) where counryCode in(" + q1 + ")"));


            sql = @"select convert(varchar,sentdatetime,102) as SMSdate,ISNULL(s.campaignname,'') CampaignName,Convert(varchar,m.msgid) as MessageId, " + str + @" as MobileNo, m.senderid as Sender,
DATEADD(MINUTE," + timedifferenceInMinute + @",convert(varchar,m.sentdatetime,106) + ' ' + convert(varchar,m.sentdatetime,108)) as SentDate,
case when d.insertdate is null then '' else DATEADD(MINUTE," + timedifferenceInMinute + @",convert(varchar,d.insertdate,106) + ' ' + convert(varchar,d.insertdate,108)) end as DeliveredDate,
Replace(Replace(isnull(smstext,msgtext),CHAR(10),''),CHAR(13),'') as Message, CASE WHEN d.dlvrstatus is null then 'UNKNOWN' ELSE CASE WHEN d.dlvrstatus='Delivered' then 'DELIVERED' ELSE 'FAILED' END END 
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED m with (nolock) 
             left join DELIVERY d with (nolock) on m.msgid=d.msgid and convert(varchar,m.insertdate,102)=convert(varchar,d.insertdate,102)
             left join SMSFILEUPLOAD s with (nolock) ON m.FILEID=s.ID
             where m.PROFILEID='" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
            if (fileId != "") sql = sql + @"and m.FILEID='" + fileId + "'";

            sql = sql + @" order by m.sentdatetime";

            DataTable dt = databaseB4.GetDataTable(sql);
            return dt;
        }
        public DataTable GetClickReport(string userid, string s1 = "", string s2 = "")
        {
            // Add Country Parameter on 02-02-31
            // Add ClickDate column while filter type is creation date on 03-02-22
            //d b Y : h i p

            string sql = "";

            sql = @"  SELECT Distinct convert(varchar, m.mobile) as mobile,
                DATEADD(MINUTE, " + Global.addMinutes + @", convert(varchar, m.sentdate, 106) + ' ' + convert(varchar, m.sentdate, 108)) as smsdate, 
               DATEADD(MINUTE," + Global.addMinutes + @",convert(varchar,s.click_date,106) + ' ' + convert(varchar,s.click_date,108)) as ClickDate, 
                isnull(L.operator, '') as operator, isnull(L.RegionName, '') as Circle,s.Browser, s.Platform, s.IsMobileDevice, s.MobileDeviceManufacturer, s.MobileDeviceModel
                into #tclick
                FROM mobtrackurl m with(nolock) inner join mobstats s with(nolock) on m.urlid = s.shortUrl_id and m.id = s.urlid
                inner join SMSFILEUPLOAD F on F.ID = m.fileId
                inner join MSGSUBMITTEDLOG m1 with(nolock) on m1.FILEID = m.fileId and m1.TOMOBILE = m.mobile
                Left join DELIVERYLOG d with(nolock) on d.msgid = m1.msgid
                left join mstMCCMNC m2 with(nolock) on dbo.[NumericOnly](m2.mccmnc)=dbo.[NumericOnly](d.mccmnc)
                LEFT JOIN(select distinct regionName, segment,operator, mobile from iplocation with(nolock)) L on m.mobile = L.mobile and m.segment = L.segment
                where F.USERID = '" + userid + "' AND click_date Between '" + s1 + "' and '" + s2 + @"'
                
                select row_number() over(order by ClickDate) as SLNO,* from #tclick order by ClickDate desc
				drop table #tclick";


            DataTable dt = new DataTable("dt");
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSReport_user_FILEID(string f, string t, string user)
        {
            DataTable dt = new DataTable();
            string countQuery = @"Select  Count(*) FROM MSGSUBMITTEDLOG m with (nolock)
                                where m.PROFILEID = '" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
            int count = Convert.ToInt32(database.GetScalarValue(countQuery));
            if (count > 300000)
            {
                string sql = @"Select distinct m.FILEID FROM MSGSUBMITTEDLOG m with (nolock) 
                                where m.PROFILEID = '" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";

                dt = database.GetDataTable(sql);
            }
            return dt;
        }

        public DataTable GetSMSReport_user_FILEID_B4(string f, string t, string user)
        {
            DataTable dt = new DataTable();
            string countQuery = @"Select  Count(*) FROM MSGSUBMITTED m with (nolock)
                                where m.PROFILEID = '" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
            int count = Convert.ToInt32(databaseB4.GetScalarValue(countQuery));
            if (count > 300000)
            {
                string sql = @"Select distinct m.FILEID FROM MSGSUBMITTED m with (nolock) 
                                where m.PROFILEID = '" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";

                dt = databaseB4.GetDataTable(sql);
            }
            return dt;
        }

        public void SaveGenratedRequestPath(string startPath, string user, string id)
        {
            string sql = string.Format("update dlrrequest set generatedPath='{0}',generatedtime=getdate(),Active=1,expirytime=dateadd(HOUR, 48, getdate()) where userid='{1}' and id='{2}'", startPath, user, id);
            database.ExecuteNonQuery(sql);
        }

        public void DeactiveRequest()
        {
            string sql = string.Format("select IIF(Count(*)>0,'success','fail') from DLRRequest where Active=1 AND expirytime<getdate()");
            string result = Convert.ToString(database.GetScalarValue(sql));
            if (result == "success")
            {
                string sql1 = string.Format("update dlrrequest set Active=0 where Active=1 AND expirytime<getdate()");
                database.ExecuteNonQuery(sql1);
                DataTable dtPaths = database.GetDataTable("select Generatedpath from DLRRequest where Active=0 AND isnull(Generatedpath,'')<>'' AND expirytime<getdate()");
                foreach (DataRow dr in dtPaths.Rows)
                {
                    if (File.Exists(dr["Generatedpath"].ToString()))
                        File.Delete(dr["Generatedpath"].ToString());
                }

            }
        }


        #region  for <SMS SENDING - blk>
        public DataTable GetTemplateErrors()
        {
            sql = "select * from errorcodeTemplate with (nolock)";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetRequestForSMSTest(string picked_time)
        {
            sql = "select * from MSGTRANRequest with (nolock) where ISNULL(status,0)=0 and statusUpdDate is null and reqDate < '" + picked_time + "' ";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public double GetBlockSMSper(string userid, string typ)
        {
            DataTable dt = database.GetDataTable("select isnull(" + (typ == "B" ? "blockpercent" : "failpercent") + ",0) as bp from blocksms with (nolock) where userid='" + userid + "'");
            if (dt.Rows.Count > 0)
                return Convert.ToDouble(dt.Rows[0]["bp"]);
            else return 0;
        }
        public string getWhiteListNo(string userid)
        {
            string s = "'0'";
            DataTable dt = new DataTable();
            dt = database.GetDataTable("select mobile from blocksmswhitelist with (nolock) where userid='" + userid + "' union all select mobile from blocksmswhitelistglobal with (nolock) ");
            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++) s = s + ",'" + dt.Rows[k]["mobile"].ToString() + "'";
            }
            return s;
        }

        public string GetSMSText_1(string msg, int x, int noofsms, bool ucs2)
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

        public void SMSCampService()
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cnn.Open();
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SMSCampService";
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
        #endregion

        #region<--Abhishek-->


        public DataTable GetTemplateRemainder()
        {
            DataTable dt = new DataTable();
            string sql = "";
            sql = @"select  username,template,templateid from templaterequest  where   Convert(date,createdat)< convert(date,getdate()) and allotted is null and rejected is null";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSenderRemainder()
        {
            DataTable dt = new DataTable();
            string sql = "";
            sql = @"declare @previsedate varchar(10)
                      SELECT @previsedate= DATEADD(day, -1, CAST(GETDATE() AS date))
select  username,senderid, allotedsenderid from senderidrequeset  where   Convert(date,createdat,103)= @previsedate  and rejected is null  or rejected='0'";
            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetRecordDataTable(string sql)
        {
            DataTable dt = new DataTable();
            dt = database.GetDataTable(sql);
            return dt;

        }
        #endregion

        public DataTable getSMPPSETTING()
        {
            string sql = "select * from SMPPSETTING with(nolock) where BINDINGMODE in ('Transceiver', 'Transmiter') and ACTIVE=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable getUserDeliveryCheck()
        {
            string sql = "select * from UserDeliveryCheck with(nolock) where ACTIVE=1";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable getMsgSubmittedCount(string Accountid)
        {
            string sql = @"select Count(*) as MsgSubmitted, sum(case when d.msgid is null then 1 else 0 end) as DLR,
            sum(case when d.DLVRSTATUS ='Delivered' then 1 else 0 end  ) as Delivered
            from MSGSUBMITTED m with(nolock) 
            left join delivery d with(nolock) on d.MSGID=m.MSGID 
            where m.SMPPACCOUNTID like '" + Accountid + "__'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public void updateEmailSent(string Accountid)
        {
            string sql = "update SMPPSETTING set EmailSent=GetDate() where smppaccountid ='" + Accountid + "'";
            database.ExecuteNonQuery(sql);
        }
        public void updateEmailSent4Delivery(string Accountid)
        {
            string sql = "update SMPPSETTING set EmailSent4Delivery=GetDate() where smppaccountid ='" + Accountid + "'";
            database.ExecuteNonQuery(sql);
        }

        public void updateUserEmailSent(string Userid)
        {
            string sql = "update UserDeliveryCheck set EmailSent=GetDate() where Userid ='" + Userid + "'";
            database.ExecuteNonQuery(sql);
        }

        public void updateUserEmailSent4Delivery(string Userid)
        {
            string sql = "update UserDeliveryCheck set EmailSent4Delivery=GetDate() where Userid ='" + Userid + "'";
            database.ExecuteNonQuery(sql);
        }

        public string SendEmailDeliveryNotComingNotify(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCMailId, string ACID, string Provider, string SystemID, string Percent)
        {
            string newbody = body + @"
                " + "Account Id : "
                + ACID + @" 
                " + "Provider : "
                + Provider + @" 
                " + "System Id : "
                + SystemID + @" 
                " + "Total Percent : "
                + Percent;
            string Port = "25";
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
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
                //for (int i = 0; i < CC.Count; i++)
                //{
                //    message.CC.Add(CC[i]);
                //}
                message.CC.Add(CCMailId);
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);

                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }
        public string SendUserEmailDeliveryNotComingNotify(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, string CCMailId, string ACID, string DvPercent)
        {
            string newbody = body + @"
                " + "Account Id : "
                + ACID + @" 
                " + "Total Percent : "
                + DvPercent;
            string Port = "25";
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
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
                //for (int i = 0; i < CC.Count; i++)
                //{
                //    message.CC.Add(CC[i]);
                //}
                message.CC.Add(CCMailId);
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);

                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public string getEmailResetMinute()
        {
            string sql = "SELECT EmailResetMinute FROM Settings WITH(NOLOCK)";
            string EmailResetMinute = Convert.ToString(database.GetScalarValue(sql));
            return EmailResetMinute;
        }

        public string getEmailSent(string smppacid)
        {
            string sql = "SELECT EmailSent FROM smppsetting WITH(NOLOCK) WHERE smppaccountid ='" + smppacid + "' ";
            string date = Convert.ToString(database.GetScalarValue(sql));
            return date;
        }
        public string getEmailSent4Delivery(string smppacid)
        {
            string sql = "SELECT EmailSent4Delivery FROM smppsetting WITH(NOLOCK) WHERE smppaccountid ='" + smppacid + "' ";
            string date = Convert.ToString(database.GetScalarValue(sql));
            return date;
        }

        public string getUserEmailSent(string user)
        {
            string sql = "SELECT EmailSent FROM UserDeliveryCheck WITH(NOLOCK) WHERE UserId ='" + user + "' ";
            string date = Convert.ToString(database.GetScalarValue(sql));
            return date;
        }
        public string getUserEmailSent4Delivery(string user)
        {
            string sql = "select EmailSent4Delivery from UserDeliveryCheck where UserId ='" + user + "' ";
            string date = Convert.ToString(database.GetScalarValue(sql));
            return date;
        }

        public DataTable getMsgSubmittedCountUser(string USER, string IsDLT)
        {
            string sql = "";
            if (IsDLT == "True")
            {
                sql = @"select Count(*) as MsgSubmitted, sum(case when d.msgid is null then 1 else 0 end) as DLR,
            sum(case when d.DLVRSTATUS = 'Delivered' then 1 else 0 end) as Delivered from MSGSUBMITTED m with(nolock)
            left join delivery d with(nolock) on d.MSGID = m.MSGID
            inner join Customer c with(nolock) on m.PROFILEID = c.username where c.DlTNo = '" + USER + "'";
            }
            else
            {
                sql = @"select Count(*) as MsgSubmitted, sum(case when d.msgid is null then 1 else 0 end) as DLR,
            sum(case when d.DLVRSTATUS = 'Delivered' then 1 else 0 end) as Delivered from MSGSUBMITTED m with(nolock)
            left join delivery d with(nolock) on d.MSGID = m.MSGID
            inner join Customer c with(nolock) on m.PROFILEID = c.username where c.username = '" + USER + "'";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public string SendEmailComman(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, List<string> CC = null)
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
    }
}
