using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Mail;

namespace API2SMPP
{
    public partial class Form1 : Form
    {
        int NotificationQryTime = Convert.ToInt32(ConfigurationManager.AppSettings["NotificationQryTime"].ToString());
        string Subject = ConfigurationManager.AppSettings["Subject"].ToString();
        string Body = ConfigurationManager.AppSettings["Body"].ToString();
        string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
        string Pwd = ConfigurationManager.AppSettings["Pwd"].ToString();
        string Host = ConfigurationManager.AppSettings["Host"].ToString();
        int PORT = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"].ToString());
        string ToMail = ConfigurationManager.AppSettings["ToMail"].ToString();

        private System.Timers.Timer timerPROCESSb4Q;
        private System.Timers.Timer timerPROCESSb4Q_Oth;
        private System.Timers.Timer timerPROCESSQ;
        private System.Timers.Timer timerDtTemp;

        private System.Timers.Timer timerPROCESSb4Q_1650;
        private System.Timers.Timer timerPROCESSb4Q_2201;
        private System.Timers.Timer timerPROCESSb4Q_HMISVR;
        private System.Timers.Timer timerPROCESSb4Q_GSM;

        private System.Timers.Timer timerPROCESSQ_blkFail;
        private System.Timers.Timer timerPROCESSQ_blkDlr;

        private System.Timers.Timer timerPROCESS_DLRcallback;
        private System.Timers.Timer timerPROCESS_CLICKcallback;

        public string otpAC = "14";
        public string bb = "MIM2101450";
        DataTable dtTemp = new DataTable();
        DataTable dtTemp1 = new DataTable();
        DataTable dtBlockNo = new DataTable();
        DataTable dtBlockPer = new DataTable();
        DataTable dtBlockPerSID = new DataTable();
        List<decimal> list; //= new List<string>();
        bool processB4Q = false;
        bool processB4Q_Oth = false;
        bool processQ = false;
        bool processB4Q_GSM = false;

        public string bb_1650 = "MIM2101650";
        DataTable dtTemp_1650 = new DataTable();
        DataTable dtBlockNo_1650 = new DataTable();
        List<decimal> list_1650;
        bool processB4Q_1650 = false;

        public string bb_2201 = "MIM2102201";
        bool processB4Q_2201 = false;
        DataTable dtBlockNo_2201 = new DataTable();
        List<decimal> list_2201;

        DataTable dtBlockNo_HMISVR = new DataTable();

        DataTable dtDLRPushAPI = new DataTable();
        DataTable dtClkPushAPI = new DataTable();
        public bool dlrCallBackApplicable = false;
        public bool clkCallBackApplicable = false;
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();

        List<decimal> list_HMISVR;
        bool processB4Q_HMISVR = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getTemplate();

            try
            { database.homeCredit = Convert.ToString(ConfigurationManager.AppSettings["HOMECREDIT"]); }
            catch (Exception ex1) { }

            //dtDLRPushAPI = getCallBackAPICustomers();
            //processDLRCallback();


            //processB4QUEUE_HMISVR();
            //processB4QUEUE_1650();
            //processB4QUEUE_OTH();
            //processB4QUEUE();

            timerDtTemp = new System.Timers.Timer();
            this.timerDtTemp.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_DTTEMP"]);
            this.timerDtTemp.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDtTemp_Tick);
            timerDtTemp.Enabled = true;
            this.timerDtTemp.Start();

            timerPROCESSb4Q_Oth = new System.Timers.Timer();
            this.timerPROCESSb4Q_Oth.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q_OTH"]);
            this.timerPROCESSb4Q_Oth.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_OTH_Tick);
            timerPROCESSb4Q_Oth.Enabled = true;
            this.timerPROCESSb4Q_Oth.Start();

            timerPROCESSb4Q = new System.Timers.Timer();
            this.timerPROCESSb4Q.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick);
            timerPROCESSb4Q.Enabled = true;
            this.timerPROCESSb4Q.Start();

            #region < Queues >

            timerPROCESSb4Q_1650 = new System.Timers.Timer();
            this.timerPROCESSb4Q_1650.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q_1650.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick_1650);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_1650.Enabled = true;
                this.timerPROCESSb4Q_1650.Start();
            }

            timerPROCESSb4Q_2201 = new System.Timers.Timer();
            this.timerPROCESSb4Q_2201.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q_2201.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick_2201);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_2201.Enabled = true;
                this.timerPROCESSb4Q_2201.Start();
            }

            timerPROCESSb4Q_HMISVR = new System.Timers.Timer();
            this.timerPROCESSb4Q_HMISVR.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q_HMISVR.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick_HMISVR);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_HMISVR.Enabled = true;
                this.timerPROCESSb4Q_HMISVR.Start();
            }

            timerPROCESSb4Q_GSM = new System.Timers.Timer();
            this.timerPROCESSb4Q_GSM.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q_GSM.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick_GSM);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_GSM.Enabled = true;
                this.timerPROCESSb4Q_GSM.Start();
            }
            #endregion

            timerPROCESSQ = new System.Timers.Timer();
            this.timerPROCESSQ.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_Q"]);
            this.timerPROCESSQ.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_Tick);
            timerPROCESSQ.Enabled = true;
            this.timerPROCESSQ.Start();

            try
            {
                timerPROCESSQ_blkDlr = new System.Timers.Timer();
                this.timerPROCESSQ_blkDlr.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_BLK_Q"]);
                this.timerPROCESSQ_blkDlr.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_blkDlr_Tick);
                timerPROCESSQ_blkDlr.Enabled = true;
                this.timerPROCESSQ_blkDlr.Start();
            }
            catch (Exception ex1) { }

            try
            {
                timerPROCESSQ_blkFail = new System.Timers.Timer();
                this.timerPROCESSQ_blkFail.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_BLK_Q"]);
                this.timerPROCESSQ_blkFail.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_blkFail_Tick);
                timerPROCESSQ_blkFail.Enabled = true;
                this.timerPROCESSQ_blkFail.Start();
            }
            catch (Exception ex1) { }

            try
            {
                try { if (Convert.ToString(ConfigurationManager.AppSettings["DLR_CALLBACK_APPLICABLE"]) == "Y") dlrCallBackApplicable = true; } catch (Exception e2) { }
                dtDLRPushAPI = getCallBackAPICustomers();

                timerPROCESS_DLRcallback = new System.Timers.Timer();
                this.timerPROCESS_DLRcallback.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_DLRCALLBACK"]);
                this.timerPROCESS_DLRcallback.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_DLRcallback_Tick);
                timerPROCESS_DLRcallback.Enabled = true;
                this.timerPROCESS_DLRcallback.Start();
            }
            catch (Exception ex1) { }

            try
            {
                try { if (Convert.ToString(ConfigurationManager.AppSettings["CLK_CALLBACK_APPLICABLE"]) == "Y") clkCallBackApplicable = true; } catch (Exception e2) { }
                dtClkPushAPI = getClickCallBackAPICustomers();

                timerPROCESS_CLICKcallback = new System.Timers.Timer();
                this.timerPROCESS_CLICKcallback.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_CLKCALLBACK"]);
                this.timerPROCESS_CLICKcallback.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_CLICKcallback_Tick);
                timerPROCESS_CLICKcallback.Enabled = true;
                this.timerPROCESS_CLICKcallback.Start();
            }
            catch (Exception ex1) { }

        }

        #region < CLICK CALLBACK Timer and Methods >
        private void timerPROCESS_CLICKcallback_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processClickCallBack();
            }
            catch (Exception ex)
            {
                LogErrorDLRCallBack("process_DLRcallback_" + ex.StackTrace, ex.Message);
            }
        }

        public void processClickCallBack()
        {
            string sql = "";
            try
            {
                //Process on each cycle
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from CLICKcallback where picked_datetime is null "));
                //Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback where picked_datetime='2023-11-20 17:33:11.777' "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(1000) CLICKcallback set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select userid,mobile,msgid,senttime,shorturl,longurl,clkTime,INSERTDATE,URL,APIRESP from CLICKcallback where picked_datetime='" + pickdate + "' ; ";
                //sql = "Select DLVRSTATUS,isnull(DLVRTIME,getdate())DLVRTIME,MSGID,err_code,PROFILEID from DeliveryCallback where picked_datetime='2023-11-20 17:33:11.777' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        processCallBackClick(Convert.ToString(dt.Rows[i]["userid"]), Convert.ToString(dt.Rows[i]["mobile"]),
                            Convert.ToString(dt.Rows[i]["senttime"]), Convert.ToString(dt.Rows[i]["shorturl"]), Convert.ToString(dt.Rows[i]["longurl"]),
                            Convert.ToString(dt.Rows[i]["clkTime"]), Convert.ToString(dt.Rows[i]["MSGID"]));
                }
                sql = " delete from CLICKcallback where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogCBErr1("p2 - "  + ex.Message); }

            }
        }

        public void processCallBackClick(string userid, string mobile, string senttime, string shorturl, string longurl, string clkTime, string msgid)
        {
            try
            {
                DataRow[] dr = dtClkPushAPI.Select("UserName = '" + userid + "'");
                if (dr.Length > 0)
                {
                    string url = Convert.ToString(dr[0]["ClickDataPushHookAPI"]);
                    if (url != "")
                    {
                        clkCallBackAPI(url, mobile, senttime, clkTime, shorturl, longurl, msgid);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void clkCallBackAPI(string url, string mobile, string senttime, string clkTime, string shorturl, string longurl, string msgid)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            //request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{
                            ""STATUS"" : ""SUCCESS"",
                            ""LONG_URL"": """ + longurl + @""",
                            ""SHORT_URL"": """ + shorturl + @""",
                            ""MOBILE"": """ + mobile + @""",
                            ""MSGID"": """ + msgid + @""",
                            ""SMS_DATE"": """ + senttime + @""",
                            ""CLICK_DATE"": """ + clkTime + @"""
                        }";
            IRestResponse response = null;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            try
            {
                LogErrClkCallBack(url, body, "");

                response = client.Execute(request);

                insertClickCallBackCalled(url, mobile, senttime, clkTime, shorturl, longurl, msgid, Convert.ToString(response.Content));
                //WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
                Root res = JsonConvert.DeserializeObject<Root>(response.Content);

                LogErrClkCallBack("", "", res.STATUS);
            }
            catch (Exception EX)
            {
                if (response.ErrorException.InnerException.Message != null)
                    LogCBErr1("url- " + url + " body- " + body + " excep-" + EX.Message + " RespException-" + Convert.ToString(response.ErrorException.InnerException.Message));
                else
                    LogCBErr1("url- " + url + " body- " + body + " excep-" + EX.Message + " Response-" + Convert.ToString(response.Content));
            }
        }
        public void LogErrClkCallBack(string url, string body, string status)
        {
            
            try
            {

                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"CLK_URLCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {

                FileStream filestrm = new FileStream(fn + @"CLK_URLcatch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                strmwriter.Flush();
                strmwriter.Close();
            }
        }
        public void LogCBErr1(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"CLKCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"CLKcatch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception exx) { }
            }
        }
        public void insertClickCallBackCalled(string url, string mobile, string senttime, string clkTime, string shorturl, string longurl, string msgid, string response)
        {
            string sql = "";
            try
            {
                if (response.Length > 4000) response = response.Substring(0, 3998);
                sql = "insert into clickcallbackCalled (longurl,shorturl,mobile,msgid,senttime,clkTime,URL,APIRESP) " +
                    "values ('" + longurl + "','" + shorturl + "','" + mobile + "','" + msgid + "','" + senttime + "','" + clkTime + "','" + url + "','" + response + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogCBErr1("insert into clickcallbackCalled - " + ex.Message); }
            }
        }
        public DataTable getClickCallBackAPICustomers()
        {
            DataTable dt = new DataTable();

            string sql = "Select Username,ClickDataPushHookAPI from customer with (nolock) where active=1 and isnull(ClickDataPushHookAPI,'')<>'' ";
            try
            {
                dt = database.GetDataTable(sql);
            }
            catch (Exception e) { }
            return dt;
        }
        #endregion

        #region <DLR Call Back Timer and Methods>
        public DataTable getCallBackAPICustomers()
        {
            DataTable dt = new DataTable();

            string sql = "Select Username,DLRPushHookAPI,sendPartCnt from customer with (nolock) where active=1 and DLRPushHookAPI<>'' ";
            try
            {
                dt = database.GetDataTable(sql);
            }
            catch (Exception e) { }
            return dt;
        }
        private void timerPROCESS_DLRcallback_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processDLRCallback();
            }
            catch (Exception ex)
            {
                LogErrorDLRCallBack("process_DLRcallback_" + ex.StackTrace, ex.Message);
            }
        }
        private void processDLRCallback()
        {
            string sql = "";
            try
            {
                //Process on each cycle
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback where picked_datetime is null "));
                //Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback where picked_datetime='2023-11-20 17:33:11.777' "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(1000) DeliveryCallback set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select DLVRSTATUS,isnull(DLVRTIME,getdate())DLVRTIME,MSGID,err_code,PROFILEID from DeliveryCallback where picked_datetime='" + pickdate + "' ; ";
                //sql = "Select DLVRSTATUS,isnull(DLVRTIME,getdate())DLVRTIME,MSGID,err_code,PROFILEID from DeliveryCallback where picked_datetime='2023-11-20 17:33:11.777' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        processCallBack(Convert.ToString(dt.Rows[i]["DLVRSTATUS"]), Convert.ToDateTime(dt.Rows[i]["DLVRTIME"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToString(dt.Rows[i]["MSGID"]), Convert.ToString(dt.Rows[i]["err_code"]), Convert.ToString(dt.Rows[i]["PROFILEID"]));
                }
                sql = " delete from DeliveryCallback where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogErrorDLRCallBack("p2", ex.Message); }
            }
        }

        public void processCallBack(string dlvStat, string d, string msgId, string errcd, string profileid)
        {
            //process callback ------- >>>>
            if (dlrCallBackApplicable)
            {
                if (dtDLRPushAPI.Rows.Count > 0)
                {
                    DataRow[] dr = dtDLRPushAPI.Select("UserName = '" + profileid + "'");
                    if (dr.Length > 0)
                        dlrCallBackAPI(dr[0]["DLRPushHookAPI"].ToString(), dlvStat, d, msgId, errcd, profileid, dr[0]["sendPartCnt"].ToString());
                }
            }
        }

        public void dlrCallBackAPI(string url, string dlvStat, string d, string msgId, string errcd, string profileid, string sendPartCnt)
        {
            string Status = "";
            string line7 = "";
            int x = 1;
            try
            {
                if (errcd.Trim() == "000") errcd = "0000";
                var client = new RestClient(url);
                x = 2;
                client.Timeout = 30000;  // Set the timeout for 30 seconds 
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                x = 3;
                string sPartCnt = "";
                if(sendPartCnt == "Y")
                {
                    sPartCnt = @", ""PARTCOUNT"": """ + getPartCount(msgId) + @"""";
                }
                var body = @"{
                          ""STATUS"": """ + dlvStat + @""",
                          ""STATUSDATETIME"": """ + d + @""",
                          ""MSGID"": """ + msgId + @""",
                          ""ERRORCODE"": """ + errcd + @"""" + sPartCnt + @"
                        }";
                x = 4;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                x = 5;
                string CC = ConfigurationManager.AppSettings["CCMail"].ToString();
                List<string> ListCC = new List<string>();
                ListCC = Convert.ToString(CC).Split(';', ',').ToList();
                try
                {
                    string stt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
                    IRestResponse response = client.Execute(request);
                    string ent = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
                    string scod = "";
                    string scodTxt = Convert.ToString(response.StatusCode);
                    try { scod = Convert.ToString(Convert.ToInt16(response.StatusCode)); } catch (Exception ee) { }
                    if (scod != "200")
                    {
                        LogOTHErr("Log- call start-" + stt + " call end-" + ent + " id-"+ msgId + " StatusCode="+ scod + " StatusText=" + scodTxt + " response-"+ response.Content.ToString());
                        insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, " StatusCode=" + scod + " StatusText=" + scodTxt + " response-" + response.Content.ToString());
                    }
                    Root res = JsonConvert.DeserializeObject<Root>(response.Content);
                    //LogOTHErr("LG statuscode" + Convert.ToString(response.StatusCode));
                    x = 6;
                    
                    x = 7;
                    if (res != null)
                    {
                        if (res.STATUS != null)
                        {
                            try
                            {
                                Status = Convert.ToString(res.STATUS);
                                LogOTHErr("LG 4 : Status = " + Status);
                            }
                            catch (Exception EX)
                            {
                                LogOTHErr("LOG 4 : sTATUS");
                            }
                        }
                    }
                    if (Convert.ToString(response.StatusCode).ToUpper() != "OK")
                    {
                        string qry = @"SELECT TOP 1 CASE WHEN DATEDIFF(MINUTE, LastCheckDatetime, GETDATE()) > " + NotificationQryTime + @" THEN 1 ELSE 0 END AS IsRecent FROM NotificationEmailLog 
                                  ORDER BY LastCheckDatetime DESC; ";
                        string checklastnotifacation = Convert.ToString(database.GetScalarValue(qry));

                        if (checklastnotifacation == "1" || checklastnotifacation == "")
                        {
                            x = 8;
                            string re = SendEmail(ToMail, Subject, Body, MailFrom, Pwd, Host, PORT, ListCC);
                            database.ExecuteNonQuery("INSERT INTO NotificationEmailLog (BodyMailSMPP,LastCheckDatetime) VALUES('" + Status + "',GETDATE())");
                            LogOTHErr("SendEmail : " + re);
                            x = 9;
                        }
                        LogOTHErr("s : " + Body);
                    }
                    else if (Status.ToUpper() != "OK")
                    {
                        string qry = @"SELECT TOP 1 CASE WHEN DATEDIFF(MINUTE, LastCheckDatetime, GETDATE()) > " + NotificationQryTime + @" THEN 1 ELSE 0 END AS IsRecent FROM NotificationEmailLog 
                                  ORDER BY LastCheckDatetime DESC; ";
                        string checklastnotifacation = Convert.ToString(database.GetScalarValue(qry));

                        x = 10;
                        string NewBody = Body + "\n" + res.STATUS;
                        if (checklastnotifacation == "1" || checklastnotifacation == "")
                        {
                            string re = SendEmail(ToMail, Subject, NewBody, MailFrom, Pwd, Host, PORT, ListCC);
                            database.ExecuteNonQuery("INSERT INTO NotificationEmailLog (BodyMailSMPP,LastCheckDatetime) VALUES('" + Status + "',GETDATE())");
                            LogOTHErr("SendEmail Status Not Ok : " + re);
                        }
                        LogOTHErr("resp_Staus: " + res.STATUS);
                        x = 11;
                    }
                    else if (response.IsSuccessful)
                    {
                        x = 12;
                        insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, Convert.ToString(response.Content));
                        LogErrDlrCallBack(url, body, " call start - " + stt + " call end - " + ent + " res-" + res.STATUS);
                        x = 13;
                    }
                    else
                    {
                        x = 14;
                        insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, Convert.ToString(response.Content));
                        LogErrDlrCallBack(url, body, res.STATUS);
                        x = 15;
                    }
                }
                catch (System.Net.WebException ex)
                {
                    string re = SendEmail(ToMail, Subject, Body, MailFrom, Pwd, Host, PORT, ListCC);
                    LogOTHErr("Timeout Exception Message : " + ex.Message + " SendEmail : " + re);
                }
                catch (Exception ex)
                {
                    LogOTHErr("ExceptionMsg : " + ex.Message + " " + ex.StackTrace + " Line NO : " + x + " " + "After Line 7" + line7);
                }
            }
            catch (Exception EX)
            {
                try
                {
                    insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, EX.Message);
                }
                catch (Exception e) { LogCBErr(" insert call back error - " + e.Message + " URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString()); }
                LogCBErr(EX.Message + " URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString());
            }
        }

        string getPartCount (string msgId)
        {
            int ss = 1;
            try
            {
                ss = Convert.ToInt32(database.GetScalarValue("select count(msgid) from msgsubmitted where msgid='" + msgId + "'"));
            }
            catch(Exception e) { }
            return Convert.ToString(ss);
        }

        //public void dlrCallBackAPI(string url, string dlvStat, string d, string msgId, string errcd)
        //{
        //    int x = 1;
        //    try
        //    {
        //        if (errcd.Trim() == "000") errcd = "0000";
        //        var client = new RestClient(url);
        //        x = 2;
        //        client.Timeout = 5000;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddHeader("Accept", "application/json");
        //        x = 3;
        //        var body = @"{
        //                  ""STATUS"": """ + dlvStat + @""",
        //                  ""STATUSDATETIME"": """ + d + @""",
        //                  ""MSGID"": """ + msgId + @""",
        //                  ""ERRORCODE"": """ + errcd + @"""
        //                }";
        //        x = 4;
        //        request.AddParameter("application/json", body, ParameterType.RequestBody);
        //        x = 5;
        //        IRestResponse response = client.Execute(request);
        //        x = 6;
        //        //WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
        //        Root res = JsonConvert.DeserializeObject<Root>(response.Content);
        //        x = 7;
        //        insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, Convert.ToString(response.Content));
        //        LogErrDlrCallBack(url, body, res.STATUS);
        //        x = 8;
        //    }
        //    catch (Exception EX)
        //    {
        //        try
        //        {
        //            insertCallBackAPICalled(dlvStat, d, msgId, errcd, url, EX.Message);
        //        }
        //        catch (Exception e) { LogCBErr(" insert call back error - " + e.Message + " URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString()); }
        //        LogCBErr(EX.Message + " URL-" + url + " dlrStat-" + dlvStat + " dlrdate-" + d + " msgid-" + msgId + " errcd-" + errcd + " x-" + x.ToString());
        //    }
        //}


        public void LogErrDlrCallBack(string url, string body, string status)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"URLCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"URLcatch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                        FileStream filestrm = new FileStream(fn + @"URLcatch2_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2) {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"URLcatch3_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3) {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"URLcatch4_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4) {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"URLcatch5_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                FileStream filestrm = new FileStream(fn + @"CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                        FileStream filestrm = new FileStream(fn + @"catch2_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2) {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"catch3_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3) {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"catch4_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4) {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"catch5_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void LogOTHErr(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"ERR_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"ERR_catchLog" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                        FileStream filestrm = new FileStream(fn + @"ERR_catch2Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception e2) {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"ERR_catch3Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception e3) {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"ERR_catch4Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception e4) {
                                try
                                {
                                    FileStream filestrm = new FileStream(fn + @"ERR_catch5Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void insertCallBackAPICalled(string dlvStat, string d, string msgId, string errcd, string url, string response)
        {
            string sql = "";
            try
            {
                if (response.Length > 4000) response = response.Substring(0, 3998);
                sql = "insert into deliverycallbackcalled (MSGID,DLVRTIME,DLVRSTATUS,err_code,URL,APIRESP) " +
                    "values ('" + msgId + "','" + d + "','" + dlvStat + "','" + errcd + "','" + url + "','" + response + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogErrorDLRCallBack("insert into deliverycallbackcalled - ", ex.Message); }
            }
        }
        #endregion

        #region <Block & BlockFail Timer and Methods >
        private void timerPROCESSQ_blkDlr_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processBlkDlrQ();
            }
            catch (Exception ex)
            {
                LogError("processBlkDlrQ_" + ex.StackTrace, ex.Message);
            }
        }
        public void processBlkDlrQ()
        {
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEblkDlr set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEblkDlr where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow drSMS in dt.Rows)
                    {
                        string msg = drSMS["msgtext"].ToString().Trim();
                        int noofsms = GetNoOfSMS(msg);
                        bool ucs2 = false;
                        if (msg.Any(c => c > 126)) ucs2 = true;
                        for (int x = 0; x < noofsms; x++)
                        {
                            string MSGid = "";
                            //if (x == 0)
                            MSGid = drSMS["msgidClient"].ToString();
                            //else MSGid = "S" + Convert.ToString(Guid.NewGuid());

                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);

                            string sql_1 = "";
                            if (x == 0)
                            {
                                sql_1 = @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate,peid,templateid)
                                            VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" +
                                                drSMS["Profileid"].ToString() + "',N'" + smsTex.Replace("'", "''") + "','"
                                     + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "') ; ";
                            }
                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                 " VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                 + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1'," +
                                 " N'" + msg.Replace("'", "''") + "','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "' ) ; ";
                            database.ExecuteNonQuery(sql_1 + sql);
                        }
                    }
                }
                sql = @"delete from MSGQUEUEblkDlr where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("processBlkDlrQ qry -", ex.Message); }
            }
        }
        private void timerPROCESSQ_blkFail_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processBlkFailQ();
            }
            catch (Exception ex)
            {
                LogError("processBlkFailQ_" + ex.StackTrace, ex.Message);
            }
        }
        public void processBlkFailQ()
        {
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEblkFail set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEblkFail where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow drSMS in dt.Rows)
                    {
                        string msg = drSMS["msgtext"].ToString().Trim();
                        int noofsms = GetNoOfSMS(msg);
                        bool ucs2 = false;
                        if (msg.Any(c => c > 126)) ucs2 = true;
                        for (int x = 0; x < noofsms; x++)
                        {
                            string MSGid = "";
                            //if (x == 0)
                            MSGid = drSMS["msgidClient"].ToString();
                            //else MSGid = "S" + Convert.ToString(Guid.NewGuid());
                            string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            string sql_1 = "";
                            if (x == 0)
                            {
                                sql_1 = @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate,peid,templateid)
                                            VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                            + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "') ; ";
                            }
                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                 " VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                 + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1'," +
                                 " N'" + msg.Replace("'", "''") + "','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "' ) ; ";
                            database.ExecuteNonQuery(sql_1 + sql);
                        }
                    }
                }
                sql = @"delete from MSGQUEUEblkFail where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("processBlkFailQ qry -", ex.Message); }
            }
        }
        #endregion

        private void timerDtTemp_Tick(object sender, ElapsedEventArgs e)
        {
            getTemplate();
        }
        public void getTemplate()
        {
            if (database.homeCredit == "Y")
            {
                dtTemp = database.GetDataTable("SELECT TEMPLATEID,TEMPWORDS,TEMPWORDS2,msgtext,senderid FROM TEMPLATEID WHERE isnull(msgtext,'')<>'' order by len(TEMPWORDS2) desc");  //SENDERID='" + sender + "' and
                dtBlockPer = database.GetDataTable("select userid,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksms");
                dtBlockPerSID = database.GetDataTable("select SID,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksmsSID");
            }
            else
                dtTemp = database.GetDataTable("Select * from Templateid order by len(tempwords2) desc");

            dtBlockNo = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb + "' ");
            list = dtBlockNo.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

            dtBlockNo_1650 = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb_1650 + "' ");
            list_1650 = dtBlockNo_1650.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

            dtBlockNo_2201 = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb_2201 + "' ");
            list_2201 = dtBlockNo_2201.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

            dtBlockNo_HMISVR = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='' ");
            list_HMISVR = dtBlockNo_HMISVR.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();
        }

        private void timerPROCESSb4Q_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processB4QUEUE();   
            }
            catch (Exception ex)
            {
                processB4Q = false;
                LogError("processB4QUEUE_" + ex.StackTrace, ex.Message);
            }
        }

        private void processB4QUEUE_OLD_B4_HC()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update MSGQUEUEB4single set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        bool blacklist = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list.Any(x => x == mobile);
                        }
                        catch (Exception ex)
                        { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    // process REJECTION ....
                                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string msgid = "";
                                        if (x == 0) msgid = dr["msgidClient"].ToString();
                                        else
                                            //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                            msgid = "S" + Convert.ToString(Guid.NewGuid());

                                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                templateid = templID;
                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);
                                //lastTemplateID = templateid;
                                //lastMessageText = msg;
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4single where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("P1", ex.Message); }
            }
        }

        //home credit
        private void processB4QUEUE()
        {
            string domain = Convert.ToString(ConfigurationManager.AppSettings["HOMECREDITdomain"]); ;
            string sql = "";
            try
            {
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from MSGQUEUEB4single where picked_datetime is null "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(1000) MSGQUEUEB4single set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                //sql = " Update MSGQUEUEB4single_OTH set picked_datetime = '" + pickdate + "' where senderid='CWCMSG' and picked_datetime is not null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        string userid = dr["Profileid"].ToString();
                        string msg1 = msg;

                        string[] link = new string[10];
                        int totlinks = 0;
                        int lnk = 0;
                        if (userid.ToUpper() != "MIM2000048")
                        {
                            if (msg1.ToLower().Contains("http"))
                            {
                                string[] ar1 = msg1.Split(' ');
                                for (int w = 0; w < ar1.Length; w++)
                                    if (ar1[w].ToLower().Contains("http"))
                                    {
                                        link[lnk] = ar1[w].Trim();
                                        lnk++;
                                    }
                            }
                        }
                        totlinks = lnk;

                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        string templID = "";
                        sql = "";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                        {
                            templID = GetTemplateIDfromSMS_HC(dr["senderid"].ToString(), dr["msgtext"].ToString(), ucs, "Y");
                            string ercode = "";
                            // rabi for template block 05/07/2022
                            if (templID != "")
                            {
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                string e_tempid = ar1[0];
                                string e_peid = Convert.ToString(dr["peid"]);
                                string e_sender = Convert.ToString(dr["senderid"]); ;

                                ercode = Convert.ToString(database.GetScalarValue("select isnull(errorcode,'') as errorcode from errorlog with (nolock) where senderid='" + e_sender + "' and TemplateID='" + e_tempid + "' and peid='" + e_peid + "'"));
                                if (ercode != "")
                                {
                                    templID = "";
                                }
                            }

                            if (templID == "")
                            {
                                // process REJECTION ....
                                //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                for (int x = 0; x < noofsms; x++)
                                {
                                    string msgid = "";
                                    if (x == 0) msgid = dr["msgidClient"].ToString();
                                    else
                                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                        msgid = "S" + Convert.ToString(Guid.NewGuid());

                                    string smsTex = "";
                                    try
                                    {
                                        smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                    }
                                    catch (Exception ex) { smsTex = msg; }

                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                    database.ExecuteNonQuery(sql);

                                    sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + (ercode == "" ? "5308" : ercode) + " text:' AS DLVRTEXT," +
                                    " '" + msgid + "', GETDATE(), 'Rejected','" + (ercode == "" ? "5308" : ercode) + "',getdate() ; ";

                                    database.ExecuteNonQuery(sql);

                                    if (x == 0)
                                    {
                                        sql = @" Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) select '" + dr["profileid"].ToString() + @"',
                                        'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + (ercode == "" ? "5308" : ercode) + @" text:' AS DLVRTEXT, 
                                        '" + msgid + "', GETDATE(), GETDATE(),  'Rejected','" + (ercode == "" ? "5308" : ercode) + "'  ";
                                        database.ExecuteNonQuery(sql);
                                    }

                                }
                            }
                            else
                            {
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                templateid = ar1[0];

                                if (totlinks > 0)
                                {
                                    string msTxt = ar1[1].ToLower();
                                    for (int x = 0; x < totlinks; x++)
                                        if (msTxt.Contains(link[x].ToLower()))
                                            link[x] = "";
                                    //for (int x = 0; x < totlinks; x++)
                                    //    if (link[x] != "")
                                    //        msg1 = msg1.Replace(link[x], domain + "12345678");
                                    //noofsms = GetMsgCount(msg1);
                                }
                                //templateid = templID;
                                int total_lnk = 0;
                                if (totlinks > 0)
                                    for (int x = 0; x < totlinks; x++)
                                        if (link[x] != "") total_lnk++;

                                string _shortUrlId = "";
                                string[] shortUrlIdAr = new string[totlinks];
                                if (total_lnk > 0)
                                {
                                    for (int x = 0; x < totlinks; x++)
                                    {
                                        if (link[x] != "")
                                        {
                                            string sUrl = NewShortURLfromSQL4HC(domain);
                                            _shortUrlId = SaveShortURL4HC(userid, link[x], "", sUrl, "Y", "Y", domain);
                                            shortUrlIdAr[x] = _shortUrlId;
                                        }
                                    }
                                }
                                DataTable dtMobTrackUrl = new DataTable();
                                dtMobTrackUrl.Columns.Add("Mob");
                                dtMobTrackUrl.Columns.Add("segment");
                                //Add Short URL Id
                                DataColumn newColumn = new DataColumn("shorturlid");
                                newColumn.DefaultValue = _shortUrlId;
                                dtMobTrackUrl.Columns.Add(newColumn);

                                string msg2 = msg;
                                if (total_lnk > 0)
                                    msg2 = SetMultipleShortURLAll(ref msg, link, "N", domain, dtMobTrackUrl, dr["TOMOBILE"].ToString(), msg, shortUrlIdAr, totlinks);

                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID, createdat,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg2.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','" + Convert.ToDateTime(dr["CreatedAt"]).ToString("yyyy-MM-dd HH:mm:ss.fff") + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);

                                if (dtMobTrackUrl != null)
                                {
                                    foreach (DataRow dr2 in dtMobTrackUrl.Rows)
                                    {
                                        sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,templateId) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr2["shorturlid"], dr2["Mob"], dr2["segment"], templateid);
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                            database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                        }
                    }
                }
                sql = @"delete from MSGQUEUEB4single where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogError("Oth-", ex.Message);
            }
        }

        private bool IsBlackList(string mob)
        {
            return false;
        }

        private void timerPROCESSb4Q_OTH_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                //if (!processB4Q_Oth)
                //{
                //processB4Q_Oth = true;
                processB4QUEUE_OTH();
                //processB4Q_Oth = false;
                //}
            }
            catch (Exception ex)
            {
                processB4Q_Oth = false;
                LogError("processB4QUEUE_OTH_" + ex.StackTrace, ex.Message);
            }
        }
        
        //home credit
        private void processB4QUEUE_OTH()
        {
            int y = 0;
            string domain = Convert.ToString(ConfigurationManager.AppSettings["HOMECREDITdomain"]); ;
            y = 1;
            string sql = "";
            try
            {
                y = 2;
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from MSGQUEUEB4single_OTH where picked_datetime is null "));
                y = 3;
                if (ctn <= 0) return;
                y = 4;
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEB4single_OTH set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                //sql = " Update MSGQUEUEB4single_OTH set picked_datetime = '" + pickdate + "' where senderid='CWCMSG' and picked_datetime is not null ;";
                database.ExecuteNonQuery(sql);
                y = 5;
                sql = "Select * from MSGQUEUEB4single_OTH where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                y = 6;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        y = 7;
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        string userid = dr["Profileid"].ToString();
                        string msg1 = msg;
                        y = 8;
                        string[] link = new string[10];
                        int totlinks = 0;
                        int lnk = 0;
                        if (userid.ToUpper() != "MIM2000048")
                        {
                            if (msg1.ToLower().Contains("http"))
                            {
                                y = 9;
                                string[] ar1 = msg1.Split(' ');
                                y = 10;
                                for (int w = 0; w < ar1.Length; w++)
                                    if (ar1[w].ToLower().Contains("http"))
                                    {
                                        y = 11;
                                        link[lnk] = ar1[w].Trim();
                                        lnk++;
                                        y = 12;
                                    }
                                y = 13;
                            }
                        }
                        totlinks = lnk;
                        y = 14;
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        string templID = "";
                        sql = "";
                        y = 15;
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                        {
                            y = 16;
                            templID = GetTemplateIDfromSMS_HC(dr["senderid"].ToString(), dr["msgtext"].ToString(), ucs, "Y");
                            y = 17;
                            string ercode = "";
                            // rabi for template block 05/07/2022
                            if (templID != "")
                            {
                                y = 18;
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                string e_tempid = ar1[0];
                                string e_peid = Convert.ToString(dr["peid"]);
                                string e_sender = Convert.ToString(dr["senderid"]); ;
                                y = 19;
                                ercode = Convert.ToString(database.GetScalarValue("select isnull(errorcode,'') as errorcode from errorlog with (nolock) where senderid='" + e_sender + "' and TemplateID='" + e_tempid + "' and peid='" + e_peid + "'"));
                                if (ercode != "")
                                {
                                    y = 20;
                                    templID = "";
                                }
                            }
                            y = 21;

                            if (templID == "")
                            {
                                y = 22;
                                // process REJECTION ....
                                //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                for (int x = 0; x < noofsms; x++)
                                {
                                    y = 23;
                                    string msgid = "";
                                    if (x == 0) msgid = dr["msgidClient"].ToString();
                                    else
                                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                        msgid = "S" + Convert.ToString(Guid.NewGuid());
                                    y = 24;
                                    string smsTex = "";
                                    try
                                    {
                                        smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                    }
                                    catch (Exception ex) { smsTex = msg; }
                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                    database.ExecuteNonQuery(sql);

                                    sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + (ercode == "" ? "5308" : ercode) + " text:' AS DLVRTEXT," +
                                    " '" + msgid + "', GETDATE(), 'Rejected','" + (ercode == "" ? "5308" : ercode) + "',getdate() ; ";
                                    try
                                    {
                                        database.ExecuteNonQuery(sql);
                                        if (x == 0)
                                        {
                                            sql = @" Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) select '" + dr["profileid"].ToString() + @"',
                                        'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + (ercode == "" ? "5308" : ercode) + @" text:' AS DLVRTEXT, 
                                        '" + msgid + "', GETDATE(), GETDATE(),  'Rejected','" + (ercode == "" ? "5308" : ercode) + "'  ";
                                            database.ExecuteNonQuery(sql);
                                        }

                                    }
                                    catch (Exception ex) { }
                                    y = 25;
                                }
                            }
                            else
                            {
                                y = 26;
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                templateid = ar1[0];
                                y = 27;
                                if (totlinks > 0)
                                {
                                    string msTxt = ar1[1].ToLower();
                                    y = 28;
                                    for (int x = 0; x < totlinks; x++)
                                        if (msTxt.Contains(link[x].ToLower()))
                                            link[x] = "";
                                    //for (int x = 0; x < totlinks; x++)
                                    //    if (link[x] != "")
                                    //        msg1 = msg1.Replace(link[x], domain + "12345678");
                                    //noofsms = GetMsgCount(msg1);
                                }
                                y = 29;
                                //templateid = templID;
                                int total_lnk = 0;
                                if (totlinks > 0)
                                    for (int x = 0; x < totlinks; x++)
                                        if (link[x] != "") total_lnk++;
                                y = 30;
                                string _shortUrlId = "";
                                string[] shortUrlIdAr = new string[totlinks];
                                if (total_lnk > 0)
                                {
                                    y = 31;
                                    for (int x = 0; x < totlinks; x++)
                                    {
                                        y = 32;
                                        if (link[x] != "")
                                        {
                                            y = 33;
                                            string sUrl = NewShortURLfromSQL4HC(domain);
                                            _shortUrlId = SaveShortURL4HC(userid, link[x], "", sUrl, "Y", "Y", domain);
                                            shortUrlIdAr[x] = _shortUrlId;
                                            y = 34;
                                        }
                                    }
                                }
                                y = 35;
                                DataTable dtMobTrackUrl = new DataTable();
                                dtMobTrackUrl.Columns.Add("Mob");
                                dtMobTrackUrl.Columns.Add("segment");
                                //Add Short URL Id
                                DataColumn newColumn = new DataColumn("shorturlid");
                                newColumn.DefaultValue = _shortUrlId;
                                dtMobTrackUrl.Columns.Add(newColumn);
                                y = 36;
                                string msg2 = msg;
                                if (total_lnk > 0)
                                {
                                    y = 37;
                                    msg2 = SetMultipleShortURLAll(ref msg, link, "N", domain, dtMobTrackUrl, dr["TOMOBILE"].ToString(), msg, shortUrlIdAr, totlinks);
                                }
                                y = 38;
                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID, createdat,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg2.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','" + Convert.ToDateTime(dr["CreatedAt"]).ToString("yyyy-MM-dd HH:mm:ss.fff") + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);
                                y = 39;
                                if (dtMobTrackUrl != null)
                                {
                                    foreach (DataRow dr2 in dtMobTrackUrl.Rows)
                                    {
                                        y = 40;
                                        sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,templateId) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr2["shorturlid"], dr2["Mob"], dr2["segment"], templateid);
                                        database.ExecuteNonQuery(sql);
                                        y = 41;
                                    }
                                }

                            }
                            y = 42;
                            database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                            y = 43;
                        }
                    }
                }
                y = 44;
                sql = @"delete from MSGQUEUEB4single_OTH where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
                y = 45;
            }
            catch (Exception ex)
            {
                LogError("Oth-", ex.Message + " y=" + y.ToString() + " sql = " + sql);
            }
        }

        private string SetMultipleShortURLAll(ref string msg, string[] link, string existingURL, string domain, DataTable dtMobTrackUrl, string mobile, string originalmsg, string[] shortUrlIdAr, int totlinks)
        {
            for (int x = 0; x < totlinks; x++)
            {
                if (link[x] != "")
                {
                    string _segment = NewShortURLforMobTrkSQL();
                    string lblShortURL = "";

                    lblShortURL = domain + _segment;
                    DataRow row = dtMobTrackUrl.NewRow();
                    row["Mob"] = mobile;
                    row["segment"] = _segment;
                    dtMobTrackUrl.Rows.Add(row);

                    if (originalmsg.ToLower().Contains(link[x].ToLower()))
                    {
                        originalmsg = originalmsg.Trim().Replace(link[x], lblShortURL);
                    }
                    else
                    {
                        msg = originalmsg.Trim() + " " + lblShortURL;
                    }
                }
            }
            msg = originalmsg;
            return msg;
        }

        public string NewShortURLforMobTrkSQL()
        {
            string sql = @"declare @i integer declare @s varchar(6) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 6) if not exists(select segment from mobtrackurl where segment = @s) break else begin set @s = '' set @i = @i + 1 end end
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
        public string SaveShortURL4HC(string UserID, string LongURL, string UserHostAddress, string ShortURL, string mobTrk, string mainurl, string domain, string name = "", string richmediaurl = "")
        {
            string sql = "Insert into Short_urls (long_url, segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) values " +
                "('" + LongURL.Replace("'", "''") + "','" + ShortURL + "',getdate(),'" + UserHostAddress + "','0','" + UserID + "','" + mobTrk + "','" + (mainurl == "Y" ? "1" : "0") + "','" + domain + "','" + name + "','" + (richmediaurl == "Y" ? "1" : "0") + "'); " +
                " SELECT MAX(ID) FROM Short_urls ";
            string id = Convert.ToString(database.GetScalarValue(sql));
            return id;
        }
        private void timerPROCESSQ_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processQ)
                {
                    processQ = true;
                    processQUEUE();
                    processQ = false;
                }
            }
            catch (Exception ex)
            {
                processQ = false;
                LogError("processQUEUE_" + ex.StackTrace, ex.Message);
            }
        }
        //home credit
        private void processQUEUE()
        {
            string sql = "";
            try
            {
                //Process Single Queue on each cycle
                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle') drop table tmpMSGQUEUEsingle ");

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                //sql = " Update MSGQUEUEsingle set picked_datetime = '" + pickdate + "' where picked_datetime is null ;" +
                //    " Select * into tmpMSGQUEUEsingle from MSGQUEUEsingle with (nolock) where picked_datetime='" + pickdate + "' ; ";
                sql = " Update MSGQUEUEsingle set picked_datetime = '" + pickdate + "' where picked_datetime is null ;" +
                    " Select M.ID,M.PROVIDER,M.SMPPACCOUNTID,M.PROFILEID,M.MSGTEXT,M.TOMOBILE,M.SENDERID,M.CREATEDAT,M.PICKED_DATETIME,M.FILEID,M.peid,M.templateid," +
                    "M.smsrate,M.datacode,M.msgidClient,M.noofsms, CASE WHEN T.templateid IS NULL THEN M.smppaccountid_1 ELSE T.smppaccountid END AS smppaccountid_1 " +
                    "into tmpMSGQUEUEsingle from MSGQUEUEsingle M with (nolock) LEFT JOIN SMPPACCOUNTTEMPLATE T WITH (NOLOCK) ON M.templateid=T.TEMPLATEID " +
                    "where M.picked_datetime='" + pickdate + "' ; ";
                database.ExecuteNonQuery(sql);

                #region < SETTING SMPP ACCOUNT FOR SUBMISSION >
                DataTable dtProfileID = database.GetDataTable("Select smppaccountid_1 from tmpMSGQUEUEsingle group by smppaccountid_1 ");

                for (int q = 0; q < dtProfileID.Rows.Count; q++)
                {
                    string profID = dtProfileID.Rows[q]["smppaccountid_1"].ToString();

                    DataTable dt = GetSMPPAccounts(profID);

                    Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle where smppaccountid_1='" + profID + "' "));
                    if (rowcnt > 0)
                    {
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
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sql = "update top (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle set smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "' where smppaccountid_1='" + profID + "' and smppaccountid is null ";
                            database.ExecuteNonQuery(sql);
                        }
                    }
                }
                #endregion

                #region < BLOCKING THROUGH USERID >
                DataTable dtUser = database.GetDataTable("Select t.PROFILEID from tmpMSGQUEUEsingle t left join blocksmsSID b with (nolock) on t.SENDERID=b.SID where b.SID is null and t.smppaccountid_1<>'" + otpAC + "'  group by t.PROFILEID");
                if (dtUser.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUser.Rows)
                    {
                        DataRow[] drB = dtBlockPer.Select("userid='" + dr["Profileid"].ToString() + "'");
                        //DataTable dtB = GetBlockSMSper(dr["Profileid"].ToString());
                        if (drB.Length > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle where profileid='" + dr["Profileid"].ToString() + "' and smppaccountid_1<>'" + otpAC + "'"));

                            double Bper = Convert.ToDouble(drB[0]["blockpercent"]);
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle t " +
                                    "left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    "left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    "where t.profileid='" + dr["Profileid"].ToString() + "' and t.smppaccountid_1<>'" + otpAC + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkDlr (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            double Fper = Convert.ToDouble(drB[0]["failpercent"]);  //GetBlockSMSper(userId, "F");
                            if (Fper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle t " +
                                    " left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    " left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    " where t.profileid='" + dr["Profileid"].ToString() + "' and t.smppaccountid_1<>'" + otpAC + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkFail (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                #endregion

                #region < BLOCKING THROUGH SENDERID >
                DataTable dtSender = database.GetDataTable("Select t.SENDERID from tmpMSGQUEUEsingle t INNER join blocksmsSID b with (nolock) on t.SENDERID=b.SID group by t.SENDERID");
                if (dtSender.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSender.Rows)
                    {
                        DataRow[] drB = dtBlockPerSID.Select("SID='" + dr["SENDERID"].ToString() + "'");
                        //DataTable dtB = GetBlockSMSper(dr["Profileid"].ToString());
                        if (drB.Length > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle where SENDERID='" + dr["SENDERID"].ToString() + "' "));

                            double Bper = Convert.ToDouble(drB[0]["blockpercent"]);
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle t " +
                                    "left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    "left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    "where t.SENDERID='" + dr["SENDERID"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkDlr (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            double Fper = Convert.ToDouble(drB[0]["failpercent"]);  //GetBlockSMSper(userId, "F");
                            if (Fper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle t " +
                                    " left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    " left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    " where t.SENDERID='" + dr["SENDERID"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkFail (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                #endregion

                //int count1 = Convert.ToString("");
                //  tmpMSGQUEUEsingle
                //DataTable dtAcn = database.GetDataTable("select left(t.smppaccountid,2) as acid,TranTableName from tmpMSGQUEUEsingle t join SMPPSETTING s on left(t.smppaccountid,2)=s.smppaccountid where active=1 group by left(t.smppaccountid,2),TranTableName  ");
                DataTable dtAcn = database.GetDataTable("select left(t.smppaccountid,2) as acid,TranTableName,s.provider+'-'+s.systemid as provider from tmpMSGQUEUEsingle t join SMPPSETTING s on left(t.smppaccountid,2)=s.smppaccountid where active=1 group by left(t.smppaccountid,2),TranTableName,s.provider+'-'+s.systemid ");
                sql = "if exists (select * from sys.tables where name='tmpMSGQUEUEsingle') ";
                foreach (DataRow dr in dtAcn.Rows)
                {
                    string acid = dr["acid"].ToString();
                    string TranTableName = dr["TranTableName"].ToString();
                    string provider = dr["provider"].ToString();
                    sql = sql + @" INSERT INTO " + TranTableName + @" (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid,msgid2)
                    select '" + provider + @"',smppaccountid,profileid,msgtext,TOMOBILE,SENDERID,CREATEDAT,'1' as fileid,peid,DATACODE,smsrate,templateid,msgidClient from tmpMSGQUEUEsingle t Where left(t.smppaccountid,2)=" + acid + " ";
                }
                sql = sql + " delete from MSGQUEUEsingle where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("p2", ex.Message); }
            }
        }

        public int GetNoOfSMS(string Msg)
        {
            int noofsms = 0;
            bool ucs2 = false;
            string q = Msg.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = Msg.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;
            ucs2 = false;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 160) noofsms = 2;
            if (qlen > 306) noofsms = 3;
            if (qlen > 459) noofsms = 4;
            if (qlen > 612) noofsms = 5;
            if (qlen > 765) noofsms = 6;
            if (qlen > 918) noofsms = 7;
            if (qlen > 1071) noofsms = 8;
            if (qlen > 1224) noofsms = 9;
            if (qlen > 1377) noofsms = 10;
            if (qlen > 1530) noofsms = 11;
            if (qlen > 1683) noofsms = 12;

            if (q.Any(c => c > 126))
            {
                // unicode = y
                ucs2 = true;
                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
            }
            return noofsms;
        }
        public DataTable GetBlockSMSper()
        {
            DataTable dt = database.GetDataTable("select userid,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksms");
            return dt;
        }
        public DataTable GetSMPPAccounts(string accountid)
        {
            string sql = "";
            //string cond = "";
            //if (AcType != "") cond = " and s.ACCOUNTTYPE='" + AcType + "' ";

            //string ACID = "";
            ////ACID = Convert.ToString(database.GetScalarValue("SELECT smppaccountid FROM SMPPACCOUNTUSERID WHERE USERID = '" + ProfileID + "' AND active = '1'"));

            //ACID = Convert.ToString(database.GetDataTable("Declare @AcId varchar(100) ;" +
            //    "  SELECT @AcId = COALESCE(@AcId + ''',''', '') + smppaccountid FROM SMPPACCOUNTUSERID WHERE USERID = '" + ProfileID + "' AND active = '1' ; " +
            //    "  select '''' + @AcId + ''''").Rows[0][0]);

            sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE,row_number() over (order by  n.sessionid) as rownum,s.smppaccountid as AccountID,s.SMSExpiryMinute " +
                    " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.active=1 and n.active=1 and (s.BINDINGMODE='Transceiver' or s.BINDINGMODE='Transmiter') " +
                    " and s.smppaccountid = '" + accountid + "'";
            //if (ACID != "") sql = sql + " and s.smppaccountid = '" + ACID + "' ";
            //if (ACID != "") sql = sql + " and s.smppaccountid in (" + ACID + ") ";
            //else
            //{
            //    if (AcType != "") sql = sql + " and s.forfile='1' ";
            //}
            //FOR testing 
            /*
            sql = @"Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE,row_number() over (order by  n.sessionid) as rownum,s.smppaccountid as AccountID,s.SMSExpiryMinute " +
                   " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where 1=1 " +
                   " AND N.SESSIONID='2401' ";
*/
            sql = sql + " order by smppaccountid";

            DataTable dt = database.GetDataTable(sql);
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
        public string GetTemplateIDfromSMS(string sender, string msg)
        {
            msg = msg.Trim().ToUpper();
            string colnm = "TEMPWORDS";
            if (sender.ToUpper() != "JKCMNT")
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                msg = rgx.Replace(msg, "");
                colnm = "TEMPWORDS2";
            }
            string templID = "";
            //DataTable dt = database.GetDataTable("SELECT TEMPLATEID,dbo.AlphaNumericOnly4API(UPPER(TEMPWORDS)) AS TEMPWORDS FROM TEMPLATEID WHERE SENDERID='" + sender + "' and isnull(msgtext,'')<>'' ");
            // DataTable dt = database.GetDataTable("SELECT TEMPLATEID," + colnm + " AS TEMPWORDS FROM TEMPLATEID WHERE SENDERID='" + sender + "' and isnull(msgtext,'')<>'' order by len(" + colnm + ") desc");
            DataRow[] drs = dtTemp.Select("Senderid='" + sender + "'");
            if (drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    string stmp = "";
                    if (sender.ToUpper() != "JKCMNT")
                        stmp = dr["TEMPWORDS2"].ToString().ToUpper();
                    else
                        stmp = dr["TEMPWORDS"].ToString().ToUpper();
                    if (stmp.Trim() != "")
                    {
                        string[] words = stmp.Split(';');
                        int k = 0;
                        bool isIncorrectMSG = false;
                        for (int j = 0; j < words.Length; j++)
                        {
                            if (msg.Contains(words[j]) && msg.IndexOf(words[j], k) + 1 > k)
                            {

                            }
                            else
                            {
                                isIncorrectMSG = true; break;
                            }
                            k = msg.IndexOf(words[j]) + 1;
                        }
                        if (isIncorrectMSG == false)
                        {
                            templID = dr["TEMPLATEID"].ToString();
                            break;
                        }
                    }
                }
            }
            return templID;
        }

        public string GetTemplateIDfromSMS_HC(string sender, string msg, bool ucs, string retMsgText = "")
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
            //DataTable dt = database.GetDataTable("SELECT TEMPLATEID," + colnm + " AS TEMPWORDS,msgtext FROM TEMPLATEID WHERE SENDERID='" + sender + "' and isnull(msgtext,'')<>'' order by len(" + colnm + ") desc");
            if (dtTemp.Rows.Count > 0)
            {
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    string stmp = dtTemp.Rows[i][colnm].ToString().ToUpper();
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
                            templID = dtTemp.Rows[i]["TEMPLATEID"].ToString() + (retMsgText == "Y" ? "#$" + dtTemp.Rows[i]["msgtext"].ToString() : "");
                            break;
                        }
                    }
                }
            }
            return templID;
        }
        private void LogError(string title, string msg)
        {
            try
            {

                //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + @"\Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }
        private void LogErrorDLRCallBack(string title, string msg)
        {
            try
            {

                //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + @"\Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_LogDLRCB_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }

        #region  < NOT IN USE >
        //--------------------------New Code for userid MIM2101650  ----------------------------------//

        private void timerPROCESSb4Q_Tick_1650(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q_1650)
                {
                    processB4Q_1650 = true;
                    processB4QUEUE_1650();
                    processB4Q_1650 = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q_1650 = false;
                LogError_1650("processB4QUEUE_1650_" + ex.StackTrace, ex.Message);
            }
        }
        private void processB4QUEUE_1650()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update MSGQUEUEB4single1650 set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single1650 where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        bool blacklist = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list_1650.Any(x => x == mobile);
                        }
                        catch (Exception ex)
                        { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_1650("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_1650("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_1650("B3", ex.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    // process REJECTION ....
                                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string msgid = "";
                                        if (x == 0) msgid = dr["msgidClient"].ToString();
                                        else
                                            //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                            msgid = "S" + Convert.ToString(Guid.NewGuid());

                                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                templateid = templID;
                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);
                                //lastTemplateID = templateid;
                                //lastMessageText = msg;
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4single1650 where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError_1650("P1", ex.Message); }
            }
        }

        private void LogError_1650(string title, string msg)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log_1650" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }

        //--------------------------New Code for userid HMISVR  ----------------------------------//

        private void timerPROCESSb4Q_Tick_HMISVR(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q_HMISVR)
                {
                    processB4Q_HMISVR = true;
                    processB4QUEUE_HMISVR();
                    processB4Q_HMISVR = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q_HMISVR = false;
                LogError_HMISVR("processB4QUEUE_HMISVR_" + ex.StackTrace, ex.Message);
            }
        }
        private void processB4QUEUE_HMISVR()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update MSGQUEUEB4singleHMISVR set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4singleHMISVR where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        bool blacklist = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list_HMISVR.Any(x => x == mobile);
                        }
                        catch (Exception ex)
                        { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B3", ex.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    // process REJECTION ....
                                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string msgid = "";
                                        if (x == 0) msgid = dr["msgidClient"].ToString();
                                        else
                                            //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                            msgid = "S" + Convert.ToString(Guid.NewGuid());

                                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        database.ExecuteNonQuery(sql);
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                        try
                                        {
                                            database.ExecuteNonQuery(sql);
                                        }
                                        catch (Exception E) { LogError_HMISVR("c1", E.Message); }
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                templateid = templID;
                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);
                                //lastTemplateID = templateid;
                                //lastMessageText = msg;
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4singleHMISVR where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError_HMISVR("P1", ex.Message); }
            }
        }

        private void LogError_HMISVR(string title, string msg)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log_HMISVR" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }

        //--------------------------New Code for userid MIM2102201  ----------------------------------//

        private void timerPROCESSb4Q_Tick_2201(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q_2201)
                {
                    processB4Q_2201 = true;
                    processB4QUEUE_2201();
                    processB4Q_2201 = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q_2201 = false;
                LogError_2201("processB4QUEUE_2201_" + ex.StackTrace, ex.Message);
            }
        }
        private void processB4QUEUE_2201()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update MSGQUEUEB4single2201 set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single2201 where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        bool blacklist = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list_2201.Any(x => x == mobile);
                        }
                        catch (Exception ex)
                        { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B3", ex.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    // process REJECTION ....
                                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string msgid = "";
                                        if (x == 0) msgid = dr["msgidClient"].ToString();
                                        else
                                            //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                            msgid = "S" + Convert.ToString(Guid.NewGuid());

                                        string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED 5308' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                templateid = templID;
                                sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                database.ExecuteNonQuery(sql);
                                //lastTemplateID = templateid;
                                //lastMessageText = msg;
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4single2201 where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError_2201("P1", ex.Message); }
            }
        }

        private void LogError_2201(string title, string msg)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log_2201" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }

        //--------------------------New Code for GSM Route (msgtype=47)  ----------------------------------//

        private void timerPROCESSb4Q_Tick_GSM(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q_GSM)
                {
                    processB4Q_GSM = true;
                    processB4QUEUE_GSM();
                    processB4Q_GSM = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q_GSM = false;
                LogError_GSM("processB4QUEUE_GSM_" + ex.StackTrace, ex.Message);
            }
        }
        private void processB4QUEUE_GSM()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update MSGQUEUEB4singleGSM set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4singleGSM where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = dr["msgText"].ToString();
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        string templateid = Convert.ToString(dr["templateid"]);
                        bool blacklist = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list_2201.Any(x => x == mobile);
                        }
                        catch (Exception ex)
                        { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_GSM("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_GSM("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_GSM("B3", ex.Message); }
                            }
                        }
                        else
                        {

                            templateid = "";
                            sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                            " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                            database.ExecuteNonQuery(sql);
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4singleGSM where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError_GSM("P1", ex.Message); }
            }
        }

        private void LogError_GSM(string title, string msg)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log_GSM" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_stramWriter = new StreamWriter(fs);
                m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                m_stramWriter.Flush();
                m_stramWriter.Close();

            }
            catch (Exception ex)
            {

            }
        }
        //-----------------------------------------------------------------------------------------------------
        #endregion
        public string SendEmail(string toAddress, string subject, string body, string MailFrom, string Pwd, string Host, int PORT, List<string> CC = null)
        {
            LogOTHErr("SendEmail StaRTING");
            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom;
            string senderPassword = Pwd;
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = PORT,
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
                LogOTHErr("SendEmail eNDING");

            }
            catch (Exception ex)
            {
                LogOTHErr("SendEmail eNDING" + ex.Message + ex.StackTrace);
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
