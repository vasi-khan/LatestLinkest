using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;


namespace ReportService
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

        //public void AddInMsgSubmitted(string msgid, DataRow dr, DateTime sendTime)
        //{
        //    sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID) " +
        //        "values ('" + dr["id"].ToString() + @"',
        //        '" + dr["PROVIDER"].ToString() + @"',
        //        '" + dr["SMPPACCOUNTID"].ToString() + @"',
        //        '" + dr["PROFILEID"].ToString() + @"',
        //        '" + dr["MSGTEXT"].ToString() + @"',
        //        '" + dr["TOMOBILE"].ToString() + @"',
        //        '" + dr["SENDERID"].ToString() + @"',
        //        '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
        //        '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void RemoveFromMsgTran(string SMPPAccountID)
        //{
        //    sql = "Delete from msgtran where SMPPACCOUNTID='" + SMPPAccountID + "' and picked_datetime is not null ";
        //    database.ExecuteNonQuery(sql);
        //}

        //public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt)
        //{
        //    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('" + txt + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss") + "','" + dlvStat + "')";
        //    database.ExecuteNonQuery(sql);
        //}

        public DataTable GetRequestForReport()
        {
            sql = "select * From DLRRequest where ISNULL(Generatedtime,'')=''";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetCustomerForReport()
        {
            sql = "select * From Customer with(nolock)  WHERE ReportonEmail=1";
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
AS MessageState, isnull(d.dlvrtext,'') as RESPONSE FROM MSGSUBMITTED" + (IsTdy == false ? "Log" : "") + @"  m with (nolock) 
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
            string countQuery = @"Select  Count(*) FROM MSGSUBMITTED m with (nolock)
                                where m.PROFILEID = '" + user + "' and m.sentdatetime between '" + f + @"' and '" + t + @"' ";
            int count = Convert.ToInt32(database.GetScalarValue(countQuery));
            if (count > 300000)
            {
                string sql = @"Select distinct m.FILEID FROM MSGSUBMITTED m with (nolock) 
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


        //#region  for <SMS SENDING - blk>
        //public DataTable GetTemplateErrors()
        //{
        //    sql = "select * from errorcodeTemplate with (nolock)";
        //    DataTable dt = database.GetDataTable(sql);
        //    return dt;
        //}

        //public DataTable GetRequestForSMSTest(string picked_time)
        //{
        //    sql = "select * from MSGTRANRequest with (nolock) where ISNULL(status,0)=0 and statusUpdDate is null and reqDate < '" + picked_time + "' ";
        //    DataTable dt = database.GetDataTable(sql);
        //    return dt;
        //}
        //public double GetBlockSMSper(string userid, string typ)
        //{
        //    DataTable dt = database.GetDataTable("select isnull(" + (typ == "B" ? "blockpercent" : "failpercent") + ",0) as bp from blocksms with (nolock) where userid='" + userid + "'");
        //    if (dt.Rows.Count > 0)
        //        return Convert.ToDouble(dt.Rows[0]["bp"]);
        //    else return 0;
        //}
        //public string getWhiteListNo(string userid)
        //{
        //    string s = "'0'";
        //    DataTable dt = new DataTable();
        //    dt = database.GetDataTable("select mobile from blocksmswhitelist with (nolock) where userid='" + userid + "' union all select mobile from blocksmswhitelistglobal with (nolock) ");
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int k = 0; k < dt.Rows.Count; k++) s = s + ",'" + dt.Rows[k]["mobile"].ToString() + "'";
        //    }
        //    return s;
        //}

        //public string GetSMSText_1(string msg, int x, int noofsms, bool ucs2)
        //{
        //    string ret = "";
        //    msg = msg.Replace("''", "'");
        //    if (ucs2)
        //    {
        //        if (noofsms == 1) ret = msg;
        //        if (x == 1) { if (msg.Length > 70) ret = msg.Substring(0, 70); else ret = msg.Substring(0); }
        //        if (x == 2) { if (msg.Length > 134) ret = msg.Substring(70, 64); else ret = msg.Substring(70); }
        //        if (x == 3) { if (msg.Length > 201) ret = msg.Substring(134, 67); else ret = msg.Substring(134); }
        //        if (x == 4) { if (msg.Length > 268) ret = msg.Substring(201, 67); else ret = msg.Substring(201); }
        //        if (x == 5) { if (msg.Length > 335) ret = msg.Substring(268, 67); else ret = msg.Substring(268); }
        //        if (x == 6) { if (msg.Length > 402) ret = msg.Substring(335, 67); else ret = msg.Substring(335); }
        //        if (x == 7) { if (msg.Length > 469) ret = msg.Substring(402, 67); else ret = msg.Substring(402); }
        //        if (x == 8) { if (msg.Length > 536) ret = msg.Substring(469, 67); else ret = msg.Substring(469); }
        //        if (x == 9) { if (msg.Length > 603) ret = msg.Substring(536, 67); else ret = msg.Substring(536); }
        //        if (x == 10) { if (msg.Length > 670) ret = msg.Substring(603, 67); else ret = msg.Substring(603); }
        //    }
        //    else
        //    {
        //        if (noofsms == 1) ret = msg;
        //        if (x == 1) { if (msg.Length > 160) ret = msg.Substring(0, 160); else ret = msg.Substring(0); }
        //        if (x == 2) { if (msg.Length > 306) ret = msg.Substring(160, 146); else ret = msg.Substring(160); }
        //        if (x == 3) { if (msg.Length > 459) ret = msg.Substring(306, 153); else ret = msg.Substring(306); }
        //        if (x == 4) { if (msg.Length > 612) ret = msg.Substring(459, 153); else ret = msg.Substring(459); }
        //        if (x == 5) { if (msg.Length > 765) ret = msg.Substring(612, 153); else ret = msg.Substring(612); }
        //        if (x == 6) { if (msg.Length > 918) ret = msg.Substring(765, 153); else ret = msg.Substring(765); }
        //        if (x == 7) { if (msg.Length > 1071) ret = msg.Substring(918, 153); else ret = msg.Substring(918); }
        //        if (x == 8) { if (msg.Length > 1224) ret = msg.Substring(1071, 153); else ret = msg.Substring(1071); }
        //        if (x == 9) { if (msg.Length > 1377) ret = msg.Substring(1224, 153); else ret = msg.Substring(1224); }
        //        if (x == 10) { if (msg.Length > 1530) ret = msg.Substring(1377, 153); else ret = msg.Substring(1377); }
        //    }
        //    return ret.Replace("'", "''");
        //}

        //public void SMSCampService()
        //{
        //    using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = cnn;
        //        cnn.Open();
        //        cmd.CommandTimeout = 3600;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "SP_SMSCampService";
        //        cmd.ExecuteNonQuery();
        //        cnn.Close();
        //    }
        //}
        //#endregion

    }
}
