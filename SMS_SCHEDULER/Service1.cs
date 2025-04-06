using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
//using System.Windows.Forms;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.Net.Mail;
using System.IO.Compression;
using Ionic.Zip;
using System.Text.RegularExpressions;
using RestSharp;
using Newtonsoft.Json;
using MailKit.Security;
using MimeKit;

namespace SMS_SCHEDULER
{
    public partial class Service1 : ServiceBase
    {
        public int isHOMECR = 0;
        private Timer timerPROCESS = null;
        private Timer timerDELIVERY = null;
        private Timer timerDashBoard = null;
        private Timer timerSMSSummary = null;
        private Timer timerSMSLinkToWabaReminderEmail = null;
        private Timer timerSMSClickCountProcess = null;
        private Timer timerUnknownToDLR = null;
        private Timer timerUnknownToDLR4AC = null;
        private Timer timerPROCESSTest = null;
        private Timer timerPROCESSReport = null;

        private Timer timerwhishes = null;
        private Timer timersmscampaign = null;

        private Timer timerSMSLowBal1 = null;
        private Timer timerEmailReminderTemplateApproval = null;
        private Timer timerAutoSendRpt = null;

        //monthly report
        private Timer TimerForReportSending = null;
        bool bTimerForReportSending = false;
        //monthly report

        //DLRPercentNotificationTime 
        private Timer TimerForPercentNotiication = null;
        bool bTimerForPercentNotiication = false;
        //DLRPercentNotificationTime

        //EXE Notification Timer Start
        private Timer TimerForExeNotification = null;
        bool bTimerForExeNotification = false;
        //EXE Notification Timer End

        bool bProcessTEMPLATEreminder = false;
        bool bProcessLowBal = false;
        bool bProcessRpt = false;

        bool bProcessDelivery = false;
        int ProcessDLRCallBack = 0;

        bool bProcessUnknownToDLR = false;
        bool bProcessUnknownToDLR4AC = false;
        bool bProcessReport = false;

        bool bProcessTest = false;
        bool bSMSClickCountProcess = false;

        public string ServiceForReject = "N";
        public string SMS_LowBal1 = "";
        public string AutoSendRpt = "";
        public string SMS_LowBal2 = "";
        public string SMS_Summary = "";
        public string SMSLINKTOWABA_TIME = "";
        public string SMSClickCountProcess = "";
        public string unknown2DLR = "";
        public string unknown2DLR4AC = "";
        public string unknown2DLR4Account = "";
        public string TimeRemainder1 = " ";
        public string TimeRemainder2 = " ";
        string path0 = "";
        string path1 = "";
        string path2 = "";
        string path3 = "";
        string path4 = "";
        string path5 = "";
        string path6 = "";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogError("Serv Start", "");
                try { ProcessDLRCallBack = Convert.ToInt16(ConfigurationManager.AppSettings["PROCESS_DLR_CALLBACK"]); } catch (Exception ex1) { }
                try { isHOMECR = Convert.ToInt16(ConfigurationManager.AppSettings["IS_HOME_CREDIT"]); } catch (Exception ex1) { }

                timerDELIVERY = new Timer();
                this.timerDELIVERY.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_DELIVERY"]);
                this.timerDELIVERY.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDELIVERY_Tick);
                timerDELIVERY.Enabled = true;
                this.timerDELIVERY.Start();

                timerDashBoard = new Timer();
                this.timerDashBoard.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_DASHBOARD"]);
                this.timerDashBoard.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDashBoard_Tick);
                timerDashBoard.Enabled = true;
                this.timerDashBoard.Start();

                SMS_Summary = Convert.ToString(ConfigurationManager.AppSettings["SMS_SUMMARY"]);

                timerSMSSummary = new Timer();
                this.timerSMSSummary.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SMS_SUMMARY_INTERVAL"]);
                this.timerSMSSummary.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSSummary_Tick);
                timerSMSSummary.Enabled = true;
                this.timerSMSSummary.Start();

                unknown2DLR = Convert.ToString(ConfigurationManager.AppSettings["UNKNOWN2DLR"]);

                timerUnknownToDLR = new Timer();
                this.timerUnknownToDLR.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["UNKNOWN2DLR_INTERVAL"]);
                this.timerUnknownToDLR.Elapsed += new System.Timers.ElapsedEventHandler(this.timerUnknownToDLR_Tick);
                timerUnknownToDLR.Enabled = true;
                this.timerUnknownToDLR.Start();
                if (isHOMECR == 0)
                {
                    unknown2DLR4AC = Convert.ToString(ConfigurationManager.AppSettings["UNKNOWN2DLR4AC"]);
                    unknown2DLR4Account = Convert.ToString(ConfigurationManager.AppSettings["UNKNOWN2DLR4ACCOUNT"]);
                    timerUnknownToDLR4AC = new Timer();
                    this.timerUnknownToDLR4AC.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["UNKNOWN2DLR_INTERVAL"]);
                    this.timerUnknownToDLR4AC.Elapsed += new System.Timers.ElapsedEventHandler(this.timerUnknownToDLR4AC_Tick);
                    timerUnknownToDLR4AC.Enabled = true;
                    this.timerUnknownToDLR4AC.Start();

                    timerPROCESS = new Timer();
                    this.timerPROCESS.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
                    this.timerPROCESS.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_Tick);  //PRocess Schedule
                    timerPROCESS.Enabled = true;
                    this.timerPROCESS.Start();
                    ProcessSchedule();

                    timerPROCESSReport = new Timer();
                    this.timerPROCESSReport.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_Report"]);
                    this.timerPROCESSReport.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSReport_Tick);
                    timerPROCESSReport.Enabled = true;
                    this.timerPROCESSReport.Start();

                    //Exe Notification start ...
                    TimerForExeNotification = new Timer();
                    this.TimerForExeNotification.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["AutoNotificationSendingTime"]);
                    this.TimerForExeNotification.Elapsed += new System.Timers.ElapsedEventHandler(this.TimerForExeNotification_Tick);
                    TimerForExeNotification.Enabled = true;
                    this.TimerForExeNotification.Start();
                    //End

                    timerwhishes = new Timer();
                    timerwhishes.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["TIMERWHISHES_INTERVAL"]); // 55 * 1000; //Interval 1 hour
                    timerwhishes.Elapsed += new ElapsedEventHandler(this.timerwhishes_Tick);
                    timerwhishes.Enabled = true;
                    timerwhishes.Start();

                    timersmscampaign = new Timer();
                    timersmscampaign.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["TIMERSMSCAMP_INTERVAL"]); // 55 * 1000; //Interval 1 hour
                    timersmscampaign.Elapsed += new ElapsedEventHandler(this.timersmscampaign_Tick);
                    timersmscampaign.Enabled = true;
                    timersmscampaign.Start();


                    AutoSendRpt = Convert.ToString(ConfigurationManager.AppSettings["AutoSendRpt"]);
                    timerAutoSendRpt = new Timer();
                    this.timerAutoSendRpt.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SMS_LOWBAL_INTERVAL"]);
                    this.timerAutoSendRpt.Elapsed += new System.Timers.ElapsedEventHandler(this.timerAutoSendRpt_Tick);
                    timerAutoSendRpt.Enabled = true;
                    this.timerAutoSendRpt.Start();

                    SMS_LowBal1 = Convert.ToString(ConfigurationManager.AppSettings["SMS_LOWBAL1"]);
                    SMS_LowBal2 = Convert.ToString(ConfigurationManager.AppSettings["SMS_LOWBAL2"]);

                    timerSMSLowBal1 = new Timer();
                    this.timerSMSLowBal1.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SMS_LOWBAL_INTERVAL"]);
                    this.timerSMSLowBal1.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSLowBal_Tick);
                    timerSMSLowBal1.Enabled = true;
                    this.timerSMSLowBal1.Start();

                    TimeRemainder1 = Convert.ToString(ConfigurationManager.AppSettings["TimeRemainder1"]);
                    TimeRemainder2 = Convert.ToString(ConfigurationManager.AppSettings["TimeRemainder2"]);

                    timerEmailReminderTemplateApproval = new Timer();
                    this.timerEmailReminderTemplateApproval.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["EMAIL_TEMPLATE_INTERVAL"]);
                    this.timerEmailReminderTemplateApproval.Elapsed += new System.Timers.ElapsedEventHandler(this.timerEmailReminderTemplateApproval_Tick);
                    //timerEmailReminderTemplateApproval.Enabled = true;
                    //this.timerEmailReminderTemplateApproval.Start();

                    //monthly report timer ...
                    TimerForReportSending = new Timer();
                    this.TimerForReportSending.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["AutoReportSendingTime"]);
                    this.TimerForReportSending.Elapsed += new System.Timers.ElapsedEventHandler(this.TimerForReportSending_Tick);
                    //  TimerForReportSending.Enabled = true;
                    //  this.TimerForReportSending.Start();
                    //End

                    //DLRPercentNotificationTime  ...
                    TimerForPercentNotiication = new Timer();
                    this.TimerForPercentNotiication.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["DLRPercentNotificationTime"]);
                    this.TimerForPercentNotiication.Elapsed += new System.Timers.ElapsedEventHandler(this.TimerForPercentNotiication_Tick);
                    //   TimerForPercentNotiication.Enabled = true;
                    //   this.TimerForPercentNotiication.Start();
                    //DLRPercentNotificationTime 

                }

                SMSLINKTOWABA_TIME = Convert.ToString(ConfigurationManager.AppSettings["SMSLINKTOWABA_TIME"]);
                timerSMSLinkToWabaReminderEmail = new Timer();
                this.timerSMSLinkToWabaReminderEmail.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SMSLINKTOWABA_INTERVAL"]);
                this.timerSMSLinkToWabaReminderEmail.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSLinkToWABA_Tick);
                timerSMSLinkToWabaReminderEmail.Enabled = true;
                this.timerSMSLinkToWabaReminderEmail.Start();


                //we are inserting records in mobstats which is not clicked
                SMSClickCountProcess = Convert.ToString(ConfigurationManager.AppSettings["SMSClickCountProcess_INTERVAL"]);
                timerSMSClickCountProcess = new Timer();
                this.timerSMSClickCountProcess.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SMSClickCountProcess_INTERVAL"]);
                this.timerSMSClickCountProcess.Elapsed += new System.Timers.ElapsedEventHandler(this.timerClickCountProcess_Tick);
                timerSMSClickCountProcess.Enabled = true;
                this.timerSMSClickCountProcess.Start();

                LogError("Serv Started", "");
            }
            catch (Exception ex)
            {
                LogError("Serv Start failed ", ex.Message + " - " + ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            this.timerPROCESS.Stop();
            this.timerDELIVERY.Stop();
            LogError("Serv Stop.", "");
        }

        public void Debug()
        {
            string tim2 = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
            //DLRPercentNotification();
            //DeliveryPercentNotification();
            //UserDLRCheck();
            // UserDeliveryCheck();
            //DownLoad_Report();
            //SendSMSReport();
            //ProcessHyundaiFailedSMSforTTSobd();
            //DownLoad_Report();
            //string SMS_CAMP = Convert.ToString(ConfigurationManager.AppSettings["SMS_CAMP"]);
            //string asdas = DateTime.Now.ToString("HH:mm");

            //SentSMSLowBal();
            //sentwhatsapp();
            //SentOBD();
            //smscampreport();
            // SentSMSLowBal();
            //TestSMS();
            //DownLoad_Report();

            //timerSMSClickCountProcesses();

            // sendwish();
            //string msgid2 = "";
            //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted where msgid2='454'"));
            //SendLinkextmonthlyReport();
            //ProcessDashBoard();
            //SendLinkextmonthlyReport();
            //ProcessSchedule(); //Commentted by naved at 22-03-2023
            //SendLinkextmonthlyReport();
            //SendHondatmonthlyReport();
            //SendHyundaiSalesMonthlyReport();
            //SendHyundaiServiceMonthlyReport();
            //SendHyundaiDealerSalesMonthlyReport();
            //SendHyundaiDealerServiceMonthlyReport();
            //SendHyundaiHASCMonthlyReport();
            //SendWhitePannelmonthlyReport();
            //SendSMSReport();
            //timerSMSLinkToWABAEmailReminder(); //Created by Naved
            //ExeNotification();
            //ProcessDelivery();
            //ProcessFailDelivery();
            //ProcessUnknown2DLR();
            //ProcessSMSSummary();

            // unknown2DLR4Account = Convert.ToString(ConfigurationManager.AppSettings["UNKNOWN2DLR4ACCOUNT"]);
            //ProcessUnknown2DLR4AC(unknown2DLR4Account);
            // sendmail(unknown2DLR4Account, "support.mim@myinboxmedia.com", "Mim@support", "smtp.gmail.com", "25");

            //database.RemoveRestrictedMobile();

            //ExeNotification();
            //StartProcess(@"D:\Vikas\WabaV2\WhatsApp\bin\Release\WhatsApp.exe");
            //ExeNotification();
        }

        // rachit 06-12-2021 for Low Balance
        private void SendSMSthroughAPI(string username, string senderid, string pid, string pwd, string mob, string msg, string tempateid)
        {
            string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + username.Trim() + "&pwd=" + pwd.Trim() + "&mobile=" + mob.Trim() + "&sender=" + senderid.Trim() + "&msg=" + msg + "&msgtype=13&peid=" + pid.Trim() + "&templateid=" + tempateid.Trim();
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
                Log("", getResponseTxt);
            }
            catch (Exception EX)
            {
                throw EX;
            }

        }

        public void Email(string To, string CC, string Subject, string body, string UserId, string Password, string Host, string Port)
        {
            string result = "";

            string ToEmailId = To;
            // ToEmailId = "rachit@myinboxmedia.com";

            //string Subject = "Low Balance Report for " + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Host = "smtpout.secureserver.net";
            Port = "25";
            UserId = "support@myinboxmedia.io";
            Password = "MiM#987654321";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(UserId, Password),
                    Timeout = 30000
                };
                //smtp.UseDefaultCredentials = false;

                //smtp.Credentials = new System.Net.NetworkCredential(UserId, Password);
                MailMessage message = new MailMessage(UserId, ToEmailId, Subject, body);
                message.CC.Add(CC);
                message.IsBodyHtml = true;

                smtp.EnableSsl = true;
                smtp.Send(message);
                result = "Sent Successfully..!!";
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
            }

        }

        private void timerAutoSendRpt_Tick(object sender, ElapsedEventArgs e)
        {
            if (!bProcessRpt)
            {
                bProcessRpt = true;
                LogError("--AutoSendRpt-- ", "processStarted");
                if (AutoSendRpt == DateTime.Now.ToString("HH:mm"))
                {
                    LogError("--AutoSendRpt-- ", " MATCHED ");
                    SendSMSReport();
                }
                bProcessRpt = false;
            }
        }

        private void timerSMSLowBal_Tick(object sender, ElapsedEventArgs e)
        {

            try
            {
                if (!bProcessLowBal)
                {
                    bProcessLowBal = true;
                    if (SMS_LowBal1 == DateTime.Now.ToString("HH:mm"))
                    {
                        SentSMSLowBal();
                        SentOBD();
                        sentwhatsapp();
                    }
                    if (SMS_LowBal2 == DateTime.Now.ToString("HH:mm"))
                    {
                        SentSMSLowBal();
                        SentOBD();
                        sentwhatsapp();
                    }
                    bProcessLowBal = false;
                }

            }
            catch (Exception ex)
            {
                bProcessLowBal = false;
                LogError("PROCES_SMS_LowBalance_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerEmailReminderTemplateApproval_Tick(object sender, ElapsedEventArgs e)
        {

            try
            {
                if (!bProcessTEMPLATEreminder)
                {
                    bProcessTEMPLATEreminder = true;
                    if (TimeRemainder1 == DateTime.Now.ToString("HH:mm"))
                    {
                        SendEmail();
                    }
                    if (TimeRemainder2 == DateTime.Now.ToString("HH:mm"))
                    {
                        SendEmail();
                    }
                    bProcessTEMPLATEreminder = false;
                }
            }
            catch (Exception ex)
            {
                bProcessTEMPLATEreminder = false;
                LogError("PROCES_Template_Reminder_" + ex.StackTrace, ex.Message);
            }
        }


        public void SendEmail()
        {
            Util ob = new Util();
            DataTable dt = ob.GetRecordDataTable("select * from  settings");
            string to = "";
            string cc = "";
            string subject = "";
            string body = "";
            string senderTo = "";
            string sendercc = "";
            string SenderSub = "";
            string senderbody = "";

            if (dt != null && dt.Rows.Count > 0)
            {
                to = Convert.ToString(dt.Rows[0]["SettingRequestTemplateTO_Escalate"]);
                cc = Convert.ToString(dt.Rows[0]["SettingRequestTemplateCC"]);
                subject = "Escalation - Template Request Pending"; // Convert.ToString(dt.Rows[0]["SettingRequestTemplateSUB"]);                

                senderTo = Convert.ToString(dt.Rows[0]["SettingRequestSIDTO_Escalate"]);
                sendercc = Convert.ToString(dt.Rows[0]["SettingRequestSIDCC"]);
                SenderSub = "Escalation - Sender Request Pending";


                //body = dt.ToString();                
            }
            // Send Email Template Approve Remainder;
            DataTable dtt = ob.GetTemplateRemainder();
            //List<TemplateRemainder> TemR = new List<TemplateRemainder>();
            StringBuilder sb = new StringBuilder();
            if (dtt != null && dtt.Rows.Count > 0)
            {
                sb.Append("Following Template requests are pending for Approval");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append(dr["username"].ToString() + "Temp_ID " + dr["template"].ToString() + "Temp_Text-" + dr["templateid"].ToString());
                    sb.Append(Environment.NewLine);
                }
                sb.Append("Please approve");
                body = sb.ToString();
                Email(to, cc, subject, body);
            }
            //Send Email Sender Approve Remainder;
            StringBuilder sb2 = new StringBuilder();
            DataTable SendDt = ob.GetSenderRemainder();
            if (SendDt != null && SendDt.Rows.Count > 0)
            {
                sb2.Append("Following SenderID requests are pending for Approval");
                foreach (DataRow dr in SendDt.Rows)
                {
                    sb2.Append(dr["username"].ToString() + "SID-" + dr["senderid"].ToString());
                    sb2.Append(Environment.NewLine);
                }
                sb2.Append("Please approve");
                senderbody = sb2.ToString();
            }
            if (dtt.Rows.Count > 0 || SendDt.Rows.Count > 0)
                Email(senderTo, sendercc, SenderSub, senderbody);
        }
        public void Email(string To, string CC, string Subject, string body)
        {
            string result = "";

            string ToEmailId = To;
            string Host = "";
            string Port = "";
            string UserId = "";
            string Password = "";
            // ToEmailId = "rachit@myinboxmedia.com";

            //string Subject = "Low Balance Report for " + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Host = "smtpout.secureserver.net";
            Port = "25";
            UserId = "support@myinboxmedia.io";
            Password = "MiM#987654321";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(UserId, Password),
                    Timeout = 30000
                };
                //smtp.UseDefaultCredentials = false;

                //smtp.Credentials = new System.Net.NetworkCredential(UserId, Password);
                MailMessage message = new MailMessage(UserId, ToEmailId, Subject, body);
                message.CC.Add(CC);
                message.IsBodyHtml = true;

                smtp.EnableSsl = true;
                smtp.Send(message);
                result = "Sent Successfully..!!";
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
            }

        }


        public void SendWhatsappLowBal(string mob, string apikey, string userId, string ProfileId, string templateName, string balance)
        {

            var client = new RestClient(" https://waba.myinboxmedia.in/api/sendwaba");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);
            // request.AddHeader("Authorization", "" + authkey + "");

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            var body = @"{

                          ""ProfileId"": """ + ProfileId + @""",
                          ""APIKey"": """ + apikey + @""",
                          ""MobileNumber"": """ + mob + @""",
                          ""templateName"": """ + templateName + @""",
                           ""Parameters"": [
                                                ""Linkext"",
                                                """ + userId + @""",
                                                """ + balance + @"""
                                                ],

                          ""HeaderType"": ""Text"",
                          ""MediaUrl"": """",
                          ""isTemplate"": ""true"",
                          ""Latitude"": ""0"",
                          ""Longitude"": ""0"",

                          ""isHSM"": ""true"",
                          ""ButtonOrListJSON"" : """"

                        }";

            request.AddParameter("application/json", body.Replace("\r\n", ""), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
            new Util().Log(response.Content.ToString());

        }



        //public void waapi(string mob, string watext, string apikey, string authkey)
        //{

        //    var client = new RestClient("https://dm3gpv.api.infobip.com/omni/1/advanced");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
        //    request.AddHeader("Authorization", "" + authkey + "");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Accept", "application/json");

        //    var body = @"{

        //                  ""scenarioKey"": """ + apikey + @""",
        //                  ""destinations"": [
        //                    {
        //                      ""to"": {
        //                        ""phoneNumber"": """ + mob + @"""
        //                      }
        //                }
        //                  ],
        //                    ""whatsApp"": {
        //                    ""text"": """ + watext + @"""
        //                  }
        //                }";


        //    request.AddParameter("application/json", body.Replace("\r\n", ""), ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);

        //}
        private void sentwhatsapp()
        {
            DataTable dtSet = database.GetDataTable("select * From settings");
            string pwd = Convert.ToString(dtSet.Rows[0]["pwdLowBal"]);
            string userid = Convert.ToString(dtSet.Rows[0]["userIdLowBal"]);
            string peId = Convert.ToString(dtSet.Rows[0]["peIdLowBal"]);
            string templateId = Convert.ToString(dtSet.Rows[0]["templateIdLowBal"]);
            string sender = Convert.ToString(dtSet.Rows[0]["SenderLowBal"]);
            string msg1 = Convert.ToString(dtSet.Rows[0]["templateLowBal"]);
            string body = Convert.ToString(dtSet.Rows[0]["EmailBodyLowBal"]);
            string subject = Convert.ToString(dtSet.Rows[0]["SubjectLowBal"]);
            string CC = Convert.ToString(dtSet.Rows[0]["CCLowBal"]);
            double NotificationSMSRate = Convert.ToDouble(dtSet.Rows[0]["NotificationSMSRate"]);


            double NotificationEmailRate = Convert.ToDouble(dtSet.Rows[0]["NotificationEmailRate"]);
            double NotificationWhatsappRate = Convert.ToDouble(dtSet.Rows[0]["NotificationWhatsappRate"]) / 100;
            double NotificationOBDRate = Convert.ToDouble(dtSet.Rows[0]["NotificationOBDRate"]);
            string Whatsappapikey = Convert.ToString(ConfigurationManager.AppSettings["Whatsappapikey"]);
            string Whatsappauth = Convert.ToString(ConfigurationManager.AppSettings["Whatsappauth"]);
            string ProfileId = Convert.ToString(ConfigurationManager.AppSettings["ProfileId"]);
            string templateName = Convert.ToString(ConfigurationManager.AppSettings["templateName"]);

            //for whatsapp
            string sql = "select m.name,m.Mobile,n.UserName,n.MinBal,n.lastrechargeper from notificationmember m left join notification n on m.Username=n.UserName where whatsapp=1";
            DataTable dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string msg = msg1;
                    //for customer Balance
                    string bal = "select balance from CUSTOMER where username='" + dr["UserName"].ToString() + "' ";
                    double balance = Convert.ToDouble(database.GetScalarValue(bal));
                    //for credit balance
                    string Cr = "select top(1) balance from userBalCrDr where username='" + dr["UserName"].ToString() + "' and trantype='C' order by trandate desc ";
                    double Credit = Convert.ToDouble(database.GetScalarValue(Cr));
                    double min = Convert.ToDouble(dr["MinBal"]);
                    double amount = Credit * (Convert.ToDouble(dr["lastrechargeper"])) / 100;
                    double max = Math.Max(min, amount);
                    if (balance < max)
                    {
                        string qry = "IF NOT EXISTS(select userid from senderidmast where userid='" + dr["UserName"].ToString() + "' and senderid='" + sender + "')" +
                        " insert into senderidmast(userid,senderid) values('" + dr["UserName"].ToString() + "','" + sender + "')";
                        database.ExecuteNonQuery(qry);
                        //msg = msg.Replace("#var1", balance.ToString());
                        //msg = msg.Replace("#var2", dr["UserName"].ToString());
                        SendWhatsappLowBal(dr["Mobile"].ToString(), Whatsappapikey, dr["UserName"].ToString(), ProfileId, templateName, balance.ToString());

                        string qry1 = "Update customer set balance=" + balance + "-" + NotificationWhatsappRate + "  where username='" + dr["UserName"].ToString() + "'";
                        // database.ExecuteNonQuery(qry1);
                    }
                }
                catch (Exception ex)
                {

                }

            }



        }


        public void obd_knowlarityapi(string mob, string obdtext, string authkey, string body)
        {
            System.Threading.Thread.Sleep(50);
            var client = new RestClient("https://kpi.knowlarity.com/Basic/v1/account/call/campaign");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            // request.AddHeader("x-api-key", apikey);
            request.AddHeader("Authorization", authkey);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            body = body.Replace("###STARTTIME###", DateTime.Now.AddMinutes(2).ToString("yyy-MM-dd HH:mm"));
            body = body.Replace("###MOBILENUMBER###", mob);
            body = body.Replace("###MESSAGE###", obdtext);


            //            var body = @"{
            //        " + "\n" +
            //            @"""ivr_id"": ""1000065122"",
            //        " + "\n" +
            //            @"""timezone"": ""Asia/Kolkata"",
            //        " + "\n" +
            //            @"""priority"": ""1"",
            //        " + "\n" +
            //            @"""order_throttling"": ""10"",
            //        " + "\n" +
            //            @"""retry_duration"": ""15"",
            //        " + "\n" +
            //            @"""start_time"": """ + DateTime.Now.AddMinutes(2).ToString("yyy-MM-dd HH:mm") + @""",
            //        " + "\n" +
            //            @"""max_retry"": ""2"",
            //        " + "\n" +
            //            @"""call_scheduling"": ""[1, 1, 1, 1, 1, 0, 0]"",
            //        " + "\n" +
            //@"""call_scheduling_start_time"": ""09:00"",
            //        " + "\n" +
            //@"""call_scheduling_stop_time"": ""21:00"",
            //        " + "\n" +
            //         @"""k_number"": ""+918047275779"",
            //        " + "\n" +
            //            @"""additional_number"": """ + mob + @"," + obdtext + @""",
            //        " + "\n" +
            //            @"""is_transactional"": ""True""
            //        " + "\n" +
            //            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            obdresp res = JsonConvert.DeserializeObject<obdresp>(response.Content);

        }

        public void obd_infobip_api(string mob, string obdtext, string authkey)
        {
            //System.Threading.Thread.Sleep(50);
            var client = new RestClient("https://vj1kpv.api.infobip.com/tts/3/single");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);
            request.AddHeader("Authorization", authkey);
            request.AddHeader("Content-Type", "application/json");

            var body = @"{
                        ""text"": """ + obdtext.Replace("\r\n", "") + @""",
                        ""language"": ""en-in"",
                        ""voice"": {
                            ""name"": ""Raveena"",
                            ""gender"": ""female""
                        },
                        ""from"": ""912271897425"",
                        ""to"": """ + mob + @"""
                        }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            obdresponse res = JsonConvert.DeserializeObject<obdresponse>(response.Content);
        }

        //For OBD Call
        private void SentOBD()
        {
            DataTable dtSet = database.GetDataTable("select * From settings");
            string pwd = Convert.ToString(dtSet.Rows[0]["pwdLowBal"]);
            string userid = Convert.ToString(dtSet.Rows[0]["userIdLowBal"]);
            string peId = Convert.ToString(dtSet.Rows[0]["peIdLowBal"]);
            string templateId = Convert.ToString(dtSet.Rows[0]["templateIdLowBal"]);
            string sender = Convert.ToString(dtSet.Rows[0]["SenderLowBal"]);
            string msg1 = Convert.ToString(dtSet.Rows[0]["templateLowBal"]);
            string body = Convert.ToString(dtSet.Rows[0]["EmailBodyLowBal"]);
            string subject = Convert.ToString(dtSet.Rows[0]["SubjectLowBal"]);
            string CC = Convert.ToString(dtSet.Rows[0]["CCLowBal"]);
            double NotificationSMSRate = Convert.ToDouble(dtSet.Rows[0]["NotificationSMSRate"]);


            double NotificationEmailRate = Convert.ToDouble(dtSet.Rows[0]["NotificationEmailRate"]);
            double NotificationWhatsappRate = Convert.ToDouble(dtSet.Rows[0]["NotificationWhatsappRate"]);
            double NotificationOBDRate = Convert.ToDouble(dtSet.Rows[0]["NotificationOBDRate"]) / 100;
            string OBDsetting = Convert.ToString(ConfigurationManager.AppSettings["OBDsetting"]);

            //for voice
            string sql = "select m.name,m.Mobile,n.UserName,n.MinBal,n.lastrechargeper from notificationmember m left join notification n on m.Username=n.UserName where voice=1";
            DataTable dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string msg = msg1;
                    //for customer Balance
                    string bal = "select balance from CUSTOMER where username='" + dr["UserName"].ToString() + "' ";
                    double balance = Convert.ToDouble(database.GetScalarValue(bal));
                    //for credit balance
                    string Cr = "select top(1) balance from userBalCrDr where username='" + dr["UserName"].ToString() + "' and trantype='C' order by trandate desc ";
                    double Credit = Convert.ToDouble(database.GetScalarValue(Cr));
                    double min = Convert.ToDouble(dr["MinBal"]);
                    double amount = Credit * (Convert.ToDouble(dr["lastrechargeper"])) / 100;
                    double max = Math.Max(min, amount);
                    if (balance < max)
                    {
                        string qry = "IF NOT EXISTS(select userid from senderidmast where userid='" + dr["UserName"].ToString() + "' and senderid='" + sender + "')" +
                        " insert into senderidmast(userid,senderid) values('" + dr["UserName"].ToString() + "','" + sender + "')";
                        database.ExecuteNonQuery(qry);
                        msg = msg.Replace("#var1", balance.ToString());
                        msg = msg.Replace("#var2", dr["UserName"].ToString());
                        // SendSMSthroughAPI(dr["UserName"].ToString(), sender, peId, pwd, dr["Mobile"].ToString(), msg, templateId);
                        if (OBDsetting == "1")
                        {
                            obd_infobip_api(dr["Mobile"].ToString(), msg, "Basic TXlpbmJveHRyYW5zdm9pY2U6U2hpdmFAMTkwNg==");

                            string qry1 = "Update customer set balance=" + balance + "-" + NotificationOBDRate + "  where username='" + dr["UserName"].ToString() + "'";
                            // database.ExecuteNonQuery(qry1);
                        }
                        else
                        {
                            string knowalarity_req_body = System.IO.File.ReadAllText(@"" + Convert.ToString(ConfigurationManager.AppSettings["REQBODY"]) + "knowalarity.txt");

                            obd_knowlarityapi(dr["Mobile"].ToString(), msg, "Basic TXlpbmJveHRyYW5zdm9pY2U6U2hpdmFAMTkwNg==", knowalarity_req_body);
                            string qry1 = "Update customer set balance=" + balance + "-" + NotificationOBDRate + "  where username='" + dr["UserName"].ToString() + "'";
                            // database.ExecuteNonQuery(qry1);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void SentSMSLowBal()
        {
            DataTable dtSet = database.GetDataTable("select * From settings");
            string pwd = Convert.ToString(dtSet.Rows[0]["pwdLowBal"]);
            string userid = Convert.ToString(dtSet.Rows[0]["userIdLowBal"]);
            string peId = Convert.ToString(dtSet.Rows[0]["peIdLowBal"]);
            string templateId = Convert.ToString(dtSet.Rows[0]["templateIdLowBal"]);
            string sender = Convert.ToString(dtSet.Rows[0]["SenderLowBal"]);
            string msg1 = Convert.ToString(dtSet.Rows[0]["templateLowBal"]);
            string body1 = Convert.ToString(dtSet.Rows[0]["EmailBodyLowBal"]);
            string subject = Convert.ToString(dtSet.Rows[0]["SubjectLowBal"]);
            string CC = Convert.ToString(dtSet.Rows[0]["CCLowBal"]);
            double NotificationSMSRate = Convert.ToDouble(dtSet.Rows[0]["NotificationSMSRate"]) / 100;

            double SMSRate = 10.00;
            double NotificationEmailRate = Convert.ToDouble(dtSet.Rows[0]["NotificationEmailRate"]) / 100;
            double NotificationWhatsappRate = Convert.ToDouble(dtSet.Rows[0]["NotificationWhatsappRate"]) / 100;
            double NotificationOBDRate = Convert.ToDouble(dtSet.Rows[0]["NotificationOBDRate"]) / 100;

            // SMS 
            // string sql = "Select * From CUSTOMER where isnull(smsonlowBalance,0)=1 ;";
            string sql = "select m.name,m.Mobile,n.UserName,n.MinBal,n.lastrechargeper from notificationmember m left join notification n on m.Username=n.UserName where sms=1";
            DataTable dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string body = body1;
                    string msg = msg1;
                    //for customer Balance
                    sql = "select * from CUSTOMER with (nolock) where username='" + dr["UserName"].ToString() + "' ";
                    DataTable dtC = database.GetDataTable(sql);
                    double balance = Convert.ToDouble(dtC.Rows[0]["balance"]);
                    SMSRate = Convert.ToDouble(dtC.Rows[0]["rate_normalsms"]) / 100;
                    pwd = Convert.ToString(dtC.Rows[0]["pwd"]);
                    //for credit balance
                    string Cr = "select top(1) balance from userBalCrDr where username='" + dr["UserName"].ToString() + "' and trantype='C' order by trandate desc ";
                    double Credit = Convert.ToDouble(database.GetScalarValue(Cr));
                    double min = Convert.ToDouble(dr["MinBal"]);
                    double amount = Credit * (Convert.ToDouble(dr["lastrechargeper"])) / 100;
                    double max = Math.Max(min, amount);
                    if (balance < max)
                    //double LowBalanceAmt = Convert.ToDouble(dr["LowBalanceAmt"]);
                    //double bal = Convert.ToDouble(dr["balance"]);
                    //if (bal <= LowBalanceAmt)
                    //    SendSMSthroughAPI(userid, sender, peId, pwd, dr["Mobile1"].ToString(), msg, templateId);
                    {
                        string qry = "IF NOT EXISTS(select userid from senderidmast with (nolock) where userid='" + dr["UserName"].ToString() + "' and senderid='" + sender + "')" +
                        " insert into senderidmast(userid,senderid) values('" + dr["UserName"].ToString() + "','" + sender + "')";
                        database.ExecuteNonQuery(qry);
                        msg = msg.Replace("#var1", balance.ToString());
                        msg = msg.Replace("#var2", dr["UserName"].ToString());
                        SendSMSthroughAPI(dr["UserName"].ToString(), sender, peId, pwd, dr["Mobile"].ToString(), msg, templateId);
                        string qry1 = "Update customer set balance=balance + " + SMSRate + " - " + NotificationSMSRate + " where username='" + dr["UserName"].ToString() + "'";
                        // database.ExecuteNonQuery(qry1);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            //Email 
            //string sql1 = "Select * From CUSTOMER where isnull(EmailOnLowBalance,0)=1 ;";
            string sql1 = "select m.name,m.Mobile,n.UserName,m.email,n.MinBal,n.lastrechargeper from notificationmember m left join notification n on m.Username=n.UserName where Emailid=1 ;";
            DataTable dt1 = database.GetDataTable(sql1);
            foreach (DataRow ro in dt1.Rows)
            {
                try
                {
                    string body = body1;

                    string bal1 = "select balance from CUSTOMER with (nolock) where username='" + ro["UserName"].ToString() + "' ";
                    double balance1 = Convert.ToDouble(database.GetScalarValue(bal1));
                    //for credit balance
                    string Cr1 = "select top(1) balance from userBalCrDr where username='" + ro["UserName"].ToString() + "' and trantype='C' order by trandate desc ";
                    double Credit1 = Convert.ToDouble(database.GetScalarValue(Cr1));
                    double min1 = Convert.ToDouble(ro["MinBal"]);
                    double amount1 = Credit1 * (Convert.ToDouble(ro["lastrechargeper"])) / 100;
                    double max1 = Math.Max(min1, amount1);
                    if (balance1 < max1)
                    //double LowBalanceAmt = Convert.ToDouble(ro["LowBalanceAmt"]);
                    //double bal = Convert.ToDouble(ro["balance"]);
                    //if (bal <= LowBalanceAmt)
                    //    Email(Convert.ToString(ro["Email"]), CC, subject, body, "support.mim@myinboxmedia.com", "Mim@support", "smtp.gmail.com", "25");
                    {
                        body = body.Replace("#var1", balance1.ToString());
                        body = body.Replace("#var2", ro["UserName"].ToString());
                        //Email(Convert.ToString(ro["Email"]), CC, subject, body, "support.mim@myinboxmedia.com", "Mim@support", "smtp.gmail.com", "25");
                        SendEMAILthroughAPI("MYINBOXMEDIA<supportmim@myinboxmedia.io>", Convert.ToString(ro["Email"]), CC, subject, body.ToString());
                        string qry1 = "Update customer set balance=" + balance1 + "-" + NotificationEmailRate + "  where username='" + ro["UserName"].ToString() + "'";
                        // database.ExecuteNonQuery(qry1);
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        private void SentSMSLowBal_b4_rohitwork()
        {
            DataTable dtSet = database.GetDataTable("select * From settings");
            string pwd = Convert.ToString(dtSet.Rows[0]["pwdLowBal"]);
            string userid = Convert.ToString(dtSet.Rows[0]["userIdLowBal"]);
            string peId = Convert.ToString(dtSet.Rows[0]["peIdLowBal"]);
            string templateId = Convert.ToString(dtSet.Rows[0]["templateIdLowBal"]);
            string sender = Convert.ToString(dtSet.Rows[0]["SenderLowBal"]);
            string msg = Convert.ToString(dtSet.Rows[0]["templateLowBal"]);
            string body = Convert.ToString(dtSet.Rows[0]["EmailBodyLowBal"]);
            string subject = Convert.ToString(dtSet.Rows[0]["SubjectLowBal"]);
            string CC = Convert.ToString(dtSet.Rows[0]["CCLowBal"]);

            // SMS 
            string sql = "Select * From CUSTOMER where isnull(smsonlowBalance,0)=1 ;";
            DataTable dt = database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    double LowBalanceAmt = Convert.ToDouble(dr["LowBalanceAmt"]);
                    double bal = Convert.ToDouble(dr["balance"]);
                    if (bal <= LowBalanceAmt)
                        SendSMSthroughAPI(userid, sender, peId, pwd, dr["Mobile1"].ToString(), msg, templateId);
                }
                catch (Exception ex)
                {

                }
            }

            //Email 
            string sql1 = "Select * From CUSTOMER where isnull(EmailOnLowBalance,0)=1 ;";
            DataTable dt1 = database.GetDataTable(sql1);
            foreach (DataRow ro in dt1.Rows)
            {
                try
                {
                    double LowBalanceAmt = Convert.ToDouble(ro["LowBalanceAmt"]);
                    double bal = Convert.ToDouble(ro["balance"]);
                    if (bal <= LowBalanceAmt)
                        Email(Convert.ToString(ro["Email"]), CC, subject, body, "support.mim@myinboxmedia.com", "Mim@support", "smtp.gmail.com", "25");
                }
                catch (Exception ex)
                {

                }
            }

        }

        //------------------------------- END -----------------------------------------------------

        private void timerSMSSummary_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (SMS_Summary == DateTime.Now.ToString("HH:mm"))
                    ProcessSMSSummary();
            }
            catch (Exception ex)
            {
                LogError("PROCES_SMS_Summary_" + ex.StackTrace, ex.Message);
            }
        }
        private void ProcessSMSSummary()
        {
            string sql = "select count(*) from daysummary where convert(varchar,smsdate,102) = convert(varchar,dateadd(day,-1,getdate()),102) ";
            Int64 c = Convert.ToInt64(database.GetScalarValue(sql));
            if (c == 0)
            {
                sql = @"INSERT INTO DAYSUMMARY (SMSDATE,USERID,SENDERID,SUBMITTED,DELIVERED,FAILED,UNKNOWN,RATE)
SELECT CONVERT(VARCHAR, M.SENTDATETIME,102) AS SMSDATE, M.PROFILEID as USERID,M.SENDERID,COUNT(M.ID) AS SUBMITTED,
  sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown,0 as RATE
FROM MSGSUBMITTED M WITH(NOLOCK)
LEFT JOIN delivery d with(nolock) on M.msgid = d.msgid
WHERE M.SENTDATETIME >= replace(convert(varchar,dateadd(day,-1,getdate()),102),'.','-') and M.SENTDATETIME < replace(convert(varchar,getdate(),102),'.','-') /* AND ISNULL(M.MSGID,'')<> '' */
GROUP BY CONVERT(VARCHAR, M.SENTDATETIME, 102),M.PROFILEID,M.SENDERID ;";

                sql = sql + " insert into [customerBalLog] (userid,balance,baldate) select username,balance,GETDATE() from CUSTOMER  WITH(NOLOCK)";

                database.ExecuteNonQuery(sql);
            }
        }

        private void timerUnknownToDLR_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bProcessUnknownToDLR)
                {
                    bProcessUnknownToDLR = true;
                    if (unknown2DLR == DateTime.Now.ToString("HH:mm"))
                    {
                        ProcessUnknown2DLR();
                        ProcessRestrictedMobile();
                    }
                    bProcessUnknownToDLR = false;
                }
            }
            catch (Exception ex)
            {
                bProcessUnknownToDLR = false;
                LogError("PROCESunknown2DLR_" + ex.StackTrace, ex.Message);
            }
        }

        private void ProcessUnknown2DLR()
        {
            string sql = @"update MSGSUBMITTED set msgid=newid() where convert(varchar,SENTDATETIME,102) = convert(varchar,DateAdd(D,-1,getdate()),102) and msgid='';

select m.* into #t from MSGSUBMITTED m with (nolock) left join delivery d with (nolock) on m.msgid=d.msgid where 
convert(varchar,SENTDATETIME,102)=convert(varchar,DateAdd(D,-1,getdate()),102) and d.msgid is null;

 Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE)  
 select 'id:' + t.msgid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,min(t.sentdatetime),112),6) + REPLACE(cONVERT(VARCHAR,min(t.sentdatetime),108),':','') + 
 ' done date:' + RIGHT(cONVERT(VARCHAR, dateadd(MINUTE,2,min(t.sentdatetime)), 112), 6) + REPLACE(cONVERT(VARCHAR, dateadd(MINUTE,2,min(t.sentdatetime)), 108), ':', '') + 
 ' stat:DELIVRD err:000 text:' AS DLVRTEXT, T.MSGID, dateadd(MINUTE,2,min(t.sentdatetime)), 'Delivered','000',min(t.sentdatetime) FROM #T t left join delivery d with (nolock) on t.msgid=d.msgid where d.msgid is null 
 group by t.msgid ; ";

            if (ProcessDLRCallBack == 1)
            {
                sql = sql + @" Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code) select t.Profileid,
                'id:' + t.msgid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,min(t.sentdatetime),112),6) + REPLACE(cONVERT(VARCHAR,min(t.sentdatetime),108),':','') + 
                 ' done date:' + RIGHT(cONVERT(VARCHAR, dateadd(MINUTE,2,min(t.sentdatetime)), 112), 6) + REPLACE(cONVERT(VARCHAR, dateadd(MINUTE,2,min(t.sentdatetime)), 108), ':', '') + 
                 ' stat:DELIVRD err:000 text:' AS DLVRTEXT, T.MSGID, dateadd(MINUTE,2,min(t.sentdatetime)),'Delivered','0000' FROM #T t left join delivery d with (nolock) on t.msgid=d.msgid where d.msgid is null 
                 group by t.Profileid,t.msgid  ";
                try
                {
                    dbmain.ExecuteNonQuery(sql);
                }
                catch (Exception e4) { LogError("PROCESunknown2DLR_DELIVERYcallback", e4.Message + e4.StackTrace); }
            }
            else
            {
                dbmain.ExecuteNonQuery(sql);
            }



        }

        private void ProcessRestrictedMobile()
        {
            database.RemoveRestrictedMobile();
            DataTable dt = database.GetDataTable("Select distinct userid from SMSRestriction where userid is not null and type='U'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string sql = @"insert into [SMSRestrictMobile] SELECT '" + dr["id"].ToString() + @"',TOMOBILE,MAX(SENTDATETIME) FROM MSGSUBMITTED M WITH(NOLOCK) INNER JOIN DELIVERY D WITH(NOLOCK) ON M.MSGID = D.MSGID
WHERE M.PROFILEID = '" + dr["userid"].ToString() + @"' AND CONVERT(VARCHAR, M.SENTDATETIME,102)= CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 102) AND D.DLVRSTATUS = 'Delivered' and M.NSEND = 0
group by TOMOBILE ";
                    database.ExecuteNonQuery(sql);
                }
            }

            dt = database.GetDataTable("Select distinct senderid from SMSRestriction where senderid is not null and type='S'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string sql = @"insert into [SMSRestrictMobile] SELECT '" + dr["id"].ToString() + @"',TOMOBILE,MAX(SENTDATETIME) FROM MSGSUBMITTED M WITH(NOLOCK) INNER JOIN DELIVERY D WITH(NOLOCK) ON M.MSGID = D.MSGID
WHERE M.Senderid = '" + dr["senderid"].ToString() + @"' AND CONVERT(VARCHAR, M.SENTDATETIME,102)= CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 102) AND D.DLVRSTATUS = 'Delivered' and M.NSEND = 0
group by TOMOBILE ";
                    database.ExecuteNonQuery(sql);
                }
            }
        }

        private void timerUnknownToDLR4AC_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bProcessUnknownToDLR4AC)
                {
                    bProcessUnknownToDLR4AC = true;
                    if (unknown2DLR4AC == DateTime.Now.ToString("HH:mm"))
                    {
                        ProcessUnknown2DLR4AC(unknown2DLR4Account);
                        sendmail(unknown2DLR4Account, "support.mim@myinboxmedia.com", "Mim@support", "smtp.gmail.com", "25");
                    }
                    bProcessUnknownToDLR4AC = false;
                }
            }
            catch (Exception ex)
            {
                bProcessUnknownToDLR4AC = false;
                LogError("PROCESunknown2DLR4AC_" + ex.StackTrace, ex.Message);
            }
        }
        private void ProcessUnknown2DLR4AC(string id)
        {
            string sql = @"update MSGSUBMITTED set msgid=newid() where profileid='" + id + @"' and convert(varchar,SENTDATETIME,102) = convert(varchar,DateAdd(D,-1,getdate()),102) and msgid='';

select m.* into #t from MSGSUBMITTED m with (nolock) left join delivery d with (nolock) on m.msgid=d.msgid where m.profileid='" + id + @"' and 
convert(varchar,SENTDATETIME,102)=convert(varchar,getdate(),102) and d.msgid is null;

 Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE)  
 select 'id:' + t.msgid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,t.sentdatetime,112),6) + REPLACE(cONVERT(VARCHAR,t.sentdatetime,108),':','') + 
 ' done date:' + RIGHT(cONVERT(VARCHAR, dateadd(MINUTE,2,t.sentdatetime), 112), 6) + REPLACE(cONVERT(VARCHAR, dateadd(MINUTE,2,t.sentdatetime), 108), ':', '') + 
 ' stat:DELIVRD err:000 text:' AS DLVRTEXT, T.MSGID, dateadd(MINUTE,2,t.sentdatetime), 'Delivered','000',t.sentdatetime FROM #T t left join delivery d on t.msgid=d.msgid where d.msgid is null ";
            dbmain.ExecuteNonQuery(sql);
        }

        private void timerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessSchedule();
            }
            catch (Exception ex)
            {
                LogError("PROCESSTimer_" + ex.StackTrace, ex.Message);
            }
        }
        #region <<  PERCENT NOTIFICATION  >>
        public void TimerForPercentNotiication_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bTimerForPercentNotiication)
                {
                    bTimerForPercentNotiication = true;
                    DLRPercentNotification();
                    DeliveryPercentNotification();
                    UserDLRCheck();
                    UserDeliveryCheck();
                    bTimerForPercentNotiication = false;
                }
            }
            catch (Exception ex)
            {
                bTimerForPercentNotiication = false;
                LogError("TimerForPercentNotiication_" + ex.StackTrace, ex.Message);
            }
        }

        public void DLRPercentNotification()
        {
            Util obj = new Util();
            string GetTimeDiff = obj.getEmailResetMinute();
            int TimeDiff = 0;
            if (GetTimeDiff != "")
            {
                TimeDiff = Convert.ToInt32(GetTimeDiff);
            }
            DateTime CurrentTimeSpam = DateTime.Now;
            DataTable dtSmppAccountId = obj.getSMPPSETTING();
            try
            {
                for (int i = 0; i < dtSmppAccountId.Rows.Count; i++)
                {
                    string getEmailSentT = obj.getEmailSent(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                    if (getEmailSentT == "")
                    {
                        getEmailSentT = "1900-01-01";
                    }
                    DateTime getEmailSentTime = Convert.ToDateTime(getEmailSentT);
                    if (Convert.ToString(getEmailSentTime) != "01/01/1900 12:00:00 AM" && Convert.ToString(getEmailSentTime) != "1900-01-01")
                    {
                        TimeSpan Timedif = CurrentTimeSpam - getEmailSentTime;
                        int NTimeDiff = Convert.ToInt32(Timedif.TotalMinutes);

                        if (NTimeDiff >= TimeDiff && CurrentTimeSpam > getEmailSentTime)
                        {
                            DataTable dt = obj.getMsgSubmittedCount(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());

                            if (dt.Rows[0]["MsgSubmitted"].ToString() == "0")
                            {
                                continue;
                            }
                            double sbcount = Convert.ToDouble(dt.Rows[0]["MsgSubmitted"].ToString());
                            double dlrcount = 0;
                            double dlrPercent = 0;
                            double AvgDLRPercent = 0;
                            double MiMNoOfSMS4DLRNotify = 0;
                            if (Convert.ToString(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"]) != "")
                            {
                                MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"].ToString());
                            }
                            //DLR PERCENT START

                            if (Convert.ToString(dtSmppAccountId.Rows[i]["AvgDLRPercent"]) != "")
                            {
                                AvgDLRPercent = Convert.ToDouble(dtSmppAccountId.Rows[i]["AvgDLRPercent"].ToString());
                            }
                            if (Convert.ToString(dt.Rows[0]["DLR"]) != "")
                            {
                                dlrcount = Convert.ToDouble(dt.Rows[0]["DLR"].ToString());
                            }
                            dlrPercent = (dlrcount / sbcount) * 100;
                            if (Convert.ToString(dlrPercent) == "NaN")
                            {
                                dlrPercent = 0;
                            }

                            if ((dlrPercent < AvgDLRPercent && MiMNoOfSMS4DLRNotify > sbcount) || dlrcount == 0)
                            {
                                string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                                string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                                string body = ConfigurationManager.AppSettings["BodyPercentNotification"].ToString();
                                string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                                string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                                string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                                obj.updateEmailSent(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                                obj.SendEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtSmppAccountId.Rows[i]["smppaccountid"].ToString(), dtSmppAccountId.Rows[i]["PROVIDER"].ToString(), dtSmppAccountId.Rows[i]["SYSTEMID"].ToString(), Convert.ToString("DLR % :" + dlrPercent));
                            }
                        }
                    }

                    else
                    {
                        DataTable dt = obj.getMsgSubmittedCount(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());

                        if (dt.Rows[0]["MsgSubmitted"].ToString() == "0")
                        {
                            continue;
                        }
                        double sbcount = Convert.ToDouble(dt.Rows[0]["MsgSubmitted"].ToString());
                        double dlrcount = 0;
                        double dlrpercent = 0;
                        double AvgDLRPercent = 0;
                        double MiMNoOfSMS4DLRNotify = 0;
                        if (Convert.ToString(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"]) != "")
                        {
                            MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"].ToString());
                        }
                        if (Convert.ToString(dtSmppAccountId.Rows[i]["AvgDLRPercent"]) != "")
                        {
                            AvgDLRPercent = Convert.ToDouble(dtSmppAccountId.Rows[i]["AvgDLRPercent"].ToString());
                        }
                        if (Convert.ToString(dt.Rows[0]["DLR"]) != "")
                        {
                            dlrcount = Convert.ToDouble(dt.Rows[0]["DLR"].ToString());
                        }

                        dlrpercent = (dlrcount / sbcount) * 100;
                        if (Convert.ToString(dlrpercent) == "NaN")
                        {
                            dlrpercent = 0;
                        }
                        if ((dlrpercent < AvgDLRPercent && MiMNoOfSMS4DLRNotify > sbcount) || dlrcount == 0)
                        {
                            string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                            string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                            string body = ConfigurationManager.AppSettings["BodyPercentNotification"].ToString();
                            string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                            string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                            string Host = ConfigurationManager.AppSettings["Host"].ToString();
                            string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                            obj.updateEmailSent(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                            obj.SendEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtSmppAccountId.Rows[i]["smppaccountid"].ToString(), dtSmppAccountId.Rows[i]["PROVIDER"].ToString(), dtSmppAccountId.Rows[i]["SYSTEMID"].ToString(), Convert.ToString("DLR % :" + dlrpercent));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DLRPercentNotiication_" + ex.StackTrace, ex.Message);
            }
        }

        public void DeliveryPercentNotification()
        {
            Util obj = new Util();
            string GetTimeDiff = obj.getEmailResetMinute();
            int TimeDiff = 0;
            if (GetTimeDiff != "")
            {
                TimeDiff = Convert.ToInt32(GetTimeDiff);
            }
            DateTime CurrentTimeSpam = DateTime.Now;
            DataTable dtSmppAccountId = obj.getSMPPSETTING();
            try
            {
                for (int i = 0; i < dtSmppAccountId.Rows.Count; i++)
                {

                    string getEmailSentTime4D = obj.getEmailSent4Delivery(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                    if (getEmailSentTime4D == "")
                    {
                        getEmailSentTime4D = "1900-01-01";
                    }
                    DateTime getEmailSentTime4Delivery = Convert.ToDateTime(getEmailSentTime4D);
                    if (Convert.ToString(getEmailSentTime4Delivery) != "01/01/1900 12:00:00 AM" && Convert.ToString(getEmailSentTime4Delivery) != "1900-01-01")
                    {
                        TimeSpan Timedif = CurrentTimeSpam - getEmailSentTime4Delivery;
                        int NTimeDiff = Convert.ToInt32(Timedif.TotalMinutes);

                        if (NTimeDiff >= TimeDiff && CurrentTimeSpam > getEmailSentTime4Delivery)
                        {

                            double deliverycount = 0;
                            double deliverypercent = 0;
                            double AvgDeliveryPercent = 0;

                            DataTable dtt = obj.getMsgSubmittedCount(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());//Total Count
                            double MiMNoOfSMS4DLRNotify = 0;
                            if (Convert.ToString(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"]) != "")
                            {
                                MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"].ToString());
                            }
                            if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                            {
                                continue;
                            }
                            double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                            if (Convert.ToString(dtSmppAccountId.Rows[i]["AvgDeliveryPercent"]) != "")
                            {
                                AvgDeliveryPercent = Convert.ToDouble(dtSmppAccountId.Rows[i]["AvgDeliveryPercent"].ToString());
                            }

                            if (Convert.ToString(dtt.Rows[0]["Delivered"]) != "")
                            {
                                deliverycount = Convert.ToDouble(dtt.Rows[0]["Delivered"].ToString());
                            }

                            deliverypercent = (deliverycount / sbcounts) * 100;
                            if (Convert.ToString(deliverypercent) == "NaN")
                            {
                                deliverypercent = 0;
                            }

                            if ((deliverypercent < AvgDeliveryPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || deliverycount == 0)
                            {
                                string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                                string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                                string body = ConfigurationManager.AppSettings["BodyPercentNotification"].ToString();
                                string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                                string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                                string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                                obj.updateEmailSent4Delivery(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                                obj.SendEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtSmppAccountId.Rows[i]["smppaccountid"].ToString(), dtSmppAccountId.Rows[i]["PROVIDER"].ToString(), dtSmppAccountId.Rows[i]["SYSTEMID"].ToString(), Convert.ToString("Delivery % :" + deliverypercent));
                            }
                        }
                    }
                    else
                    {
                        double deliverycount = 0;
                        double deliverypercent = 0;
                        double AvgDeliveryPercent = 0;

                        DataTable dtt = obj.getMsgSubmittedCount(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());//Total Count
                        double MiMNoOfSMS4DLRNotify = 0;
                        if (Convert.ToString(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"]) != "")
                        {
                            MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtSmppAccountId.Rows[i]["MiMNoOfSMS4DLRNotify"].ToString());
                        }
                        if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                        {
                            continue;
                        }
                        double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                        if (Convert.ToString(dtSmppAccountId.Rows[i]["AvgDeliveryPercent"]) != "")
                        {
                            AvgDeliveryPercent = Convert.ToDouble(dtSmppAccountId.Rows[i]["AvgDeliveryPercent"].ToString());
                        }

                        if (Convert.ToString(dtt.Rows[0]["Delivered"]) != "")
                        {
                            deliverycount = Convert.ToDouble(dtt.Rows[0]["Delivered"].ToString());
                        }

                        deliverypercent = (deliverycount / sbcounts) * 100;
                        if (Convert.ToString(deliverypercent) == "NaN")
                        {
                            deliverypercent = 0;
                        }

                        if ((deliverypercent < AvgDeliveryPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || deliverycount == 0)
                        {
                            string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                            string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                            string body = ConfigurationManager.AppSettings["BodyPercentNotification"].ToString();
                            string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                            string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                            string Host = ConfigurationManager.AppSettings["Host"].ToString();
                            string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                            obj.updateEmailSent4Delivery(dtSmppAccountId.Rows[i]["smppaccountid"].ToString());
                            obj.SendEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtSmppAccountId.Rows[i]["smppaccountid"].ToString(), dtSmppAccountId.Rows[i]["PROVIDER"].ToString(), dtSmppAccountId.Rows[i]["SYSTEMID"].ToString(), Convert.ToString("Delivery % :" + deliverypercent));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DeliveryPercentNotiication_" + ex.StackTrace, ex.Message);
            }
        }

        public void UserDLRCheck()
        {
            Util obj = new Util();
            string GetTimeDiff = obj.getEmailResetMinute();
            int TimeDiff = 0;
            if (GetTimeDiff != "")
            {
                TimeDiff = Convert.ToInt32(GetTimeDiff);
            }
            DateTime CurrentTimeSpam = DateTime.Now;
            DataTable dtuserId = obj.getUserDeliveryCheck();
            try
            {
                for (int i = 0; i < dtuserId.Rows.Count; i++)
                {

                    string getUserEmailS = obj.getUserEmailSent(dtuserId.Rows[i]["UserId"].ToString());
                    if (getUserEmailS == "")
                    {
                        getUserEmailS = "1900-01-01";
                    }
                    DateTime getUserEmailSent = Convert.ToDateTime(getUserEmailS);
                    if (Convert.ToString(getUserEmailSent) != "01/01/1900 12:00:00 AM" && Convert.ToString(getUserEmailSent) != "1900-01-01")
                    {
                        TimeSpan Timedif = CurrentTimeSpam - getUserEmailSent;
                        int NTimeDiff = Convert.ToInt32(Timedif.TotalMinutes);

                        if (NTimeDiff >= TimeDiff && CurrentTimeSpam > getUserEmailSent)
                        {

                            double dlrcount = 0;
                            double dlrpercent = 0;
                            double AvgDLRPercent = 0;

                            DataTable dtt = obj.getMsgSubmittedCountUser(dtuserId.Rows[i]["UserId"].ToString(), dtuserId.Rows[i]["IsDLT"].ToString());//Total Count
                            double MiMNoOfSMS4DLRNotify = 0;
                            if (Convert.ToString(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"]) != "")
                            {
                                MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"].ToString());
                            }
                            if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                            {
                                continue;
                            }
                            double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                            if (Convert.ToString(dtuserId.Rows[i]["AvgDLRPercent"]) != "")
                            {
                                AvgDLRPercent = Convert.ToDouble(dtuserId.Rows[i]["AvgDLRPercent"].ToString());
                            }

                            if (Convert.ToString(dtt.Rows[0]["DLR"]) != "")
                            {
                                dlrcount = Convert.ToDouble(dtt.Rows[0]["DLR"].ToString());
                            }

                            dlrpercent = (dlrcount / sbcounts) * 100;
                            if (Convert.ToString(dlrpercent) == "NaN")
                            {
                                dlrpercent = 0;
                            }

                            if ((dlrpercent < AvgDLRPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || dlrcount == 0)
                            {
                                string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                                string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                                string body = ConfigurationManager.AppSettings["BodyPercentNotificationUSER"].ToString();
                                string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                                string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                                string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                                obj.updateUserEmailSent(dtuserId.Rows[i]["UserId"].ToString());
                                obj.SendUserEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtuserId.Rows[i]["UserId"].ToString(), Convert.ToString("DLR % :" + dlrpercent));
                            }

                        }
                    }
                    else
                    {
                        double dlrcount = 0;
                        double dlrpercent = 0;
                        double AvgDLRPercent = 0;

                        DataTable dtt = obj.getMsgSubmittedCountUser(dtuserId.Rows[i]["UserId"].ToString(), dtuserId.Rows[i]["IsDLT"].ToString());//Total Count
                        double MiMNoOfSMS4DLRNotify = 0;
                        if (Convert.ToString(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"]) != "")
                        {
                            MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"].ToString());
                        }
                        if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                        {
                            continue;
                        }
                        double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                        if (Convert.ToString(dtuserId.Rows[i]["AvgDLRPercent"]) != "")
                        {
                            AvgDLRPercent = Convert.ToDouble(dtuserId.Rows[i]["AvgDLRPercent"].ToString());
                        }

                        if (Convert.ToString(dtt.Rows[0]["DLR"]) != "")
                        {
                            dlrcount = Convert.ToDouble(dtt.Rows[0]["DLR"].ToString());
                        }

                        dlrpercent = (dlrcount / sbcounts) * 100;
                        if (Convert.ToString(dlrpercent) == "NaN")
                        {
                            dlrpercent = 0;
                        }

                        if ((dlrpercent < AvgDLRPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || dlrcount == 0)
                        {
                            string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                            string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                            string body = ConfigurationManager.AppSettings["BodyPercentNotificationUSER"].ToString();
                            string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                            string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                            string Host = ConfigurationManager.AppSettings["Host"].ToString();
                            string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                            obj.updateUserEmailSent(dtuserId.Rows[i]["UserId"].ToString());
                            obj.SendUserEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtuserId.Rows[i]["UserId"].ToString(), Convert.ToString("DLR % :" + dlrpercent));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DLRUserPercentNotiication_" + ex.StackTrace, ex.Message);
            }
        }

        public void UserDeliveryCheck()
        {
            Util obj = new Util();
            string GetTimeDiff = obj.getEmailResetMinute();
            int TimeDiff = 0;
            if (GetTimeDiff != "")
            {
                TimeDiff = Convert.ToInt32(GetTimeDiff);
            }
            DateTime CurrentTimeSpam = DateTime.Now;
            DataTable dtuserId = obj.getUserDeliveryCheck();
            try
            {
                for (int i = 0; i < dtuserId.Rows.Count; i++)
                {

                    string getEmailSent4D = obj.getUserEmailSent4Delivery(dtuserId.Rows[i]["UserId"].ToString());
                    if (getEmailSent4D == "")
                    {
                        getEmailSent4D = "1900-01-01";
                    }
                    DateTime getEmailSent4Delivery = Convert.ToDateTime(getEmailSent4D);
                    if (Convert.ToString(getEmailSent4Delivery) != "01/01/1900 12:00:00 AM" && Convert.ToString(getEmailSent4Delivery) != "1900-01-01")
                    {
                        TimeSpan Timedif = CurrentTimeSpam - getEmailSent4Delivery;
                        int NTimeDiff = Convert.ToInt32(Timedif.TotalMinutes);

                        if (NTimeDiff >= TimeDiff && CurrentTimeSpam > getEmailSent4Delivery)
                        {

                            double deliverycount = 0;
                            double deliverypercent = 0;
                            double AvgdeliveryPercent = 0;

                            DataTable dtt = obj.getMsgSubmittedCountUser(dtuserId.Rows[i]["UserId"].ToString(), dtuserId.Rows[i]["IsDLT"].ToString());//Total Count
                            double MiMNoOfSMS4DLRNotify = 0;
                            if (Convert.ToString(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"]) != "")
                            {
                                MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"].ToString());
                            }
                            if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                            {
                                continue;
                            }
                            double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                            if (Convert.ToString(dtuserId.Rows[i]["AvgDeliveryPercent"]) != "")
                            {
                                AvgdeliveryPercent = Convert.ToDouble(dtuserId.Rows[i]["AvgDeliveryPercent"].ToString());
                            }

                            if (Convert.ToString(dtt.Rows[0]["Delivered"]) != "")
                            {
                                deliverycount = Convert.ToDouble(dtt.Rows[0]["Delivered"].ToString());
                            }

                            deliverypercent = (deliverycount / sbcounts) * 100;
                            if (Convert.ToString(deliverypercent) == "NaN")
                            {
                                deliverypercent = 0;
                            }

                            if ((deliverypercent < AvgdeliveryPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || deliverycount == 0)
                            {
                                string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                                string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                                string body = ConfigurationManager.AppSettings["BodyPercentNotificationUSER"].ToString();
                                string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                                string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                                string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                                obj.updateUserEmailSent4Delivery(dtuserId.Rows[i]["UserId"].ToString());
                                obj.SendUserEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtuserId.Rows[i]["UserId"].ToString(), Convert.ToString("Delivery % :" + deliverypercent));
                            }

                        }
                    }
                    else
                    {
                        double deliverycount = 0;
                        double deliverypercent = 0;
                        double AvgdeliveryPercent = 0;

                        DataTable dtt = obj.getMsgSubmittedCountUser(dtuserId.Rows[i]["UserId"].ToString(), dtuserId.Rows[i]["IsDLT"].ToString());//Total Count
                        double MiMNoOfSMS4DLRNotify = 0;
                        if (Convert.ToString(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"]) != "")
                        {
                            MiMNoOfSMS4DLRNotify = Convert.ToDouble(dtuserId.Rows[i]["MinNoOfSMS4DLRNotify"].ToString());
                        }
                        if (dtt.Rows[0]["MsgSubmitted"].ToString() == "0")
                        {
                            continue;
                        }
                        double sbcounts = Convert.ToDouble(dtt.Rows[0]["MsgSubmitted"].ToString());
                        if (Convert.ToString(dtuserId.Rows[i]["AvgdeliveryPercent"]) != "")
                        {
                            AvgdeliveryPercent = Convert.ToDouble(dtuserId.Rows[i]["AvgdeliveryPercent"].ToString());
                        }

                        if (Convert.ToString(dtt.Rows[0]["Delivered"]) != "")
                        {
                            deliverycount = Convert.ToDouble(dtt.Rows[0]["Delivered"].ToString());
                        }

                        deliverypercent = (deliverycount / sbcounts) * 100;
                        if (Convert.ToString(deliverypercent) == "NaN")
                        {
                            deliverypercent = 0;
                        }

                        if ((deliverypercent < AvgdeliveryPercent && MiMNoOfSMS4DLRNotify >= sbcounts) || deliverycount == 0)
                        {
                            string toAddress = ConfigurationManager.AppSettings["ToMailIdPercentNotification"].ToString();
                            string subject = ConfigurationManager.AppSettings["SubjectPercentNotification"].ToString();
                            string body = ConfigurationManager.AppSettings["BodyPercentNotificationUSER"].ToString();
                            string MailFrom = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
                            string Pwd = ConfigurationManager.AppSettings["PWDNotification"].ToString();
                            string Host = ConfigurationManager.AppSettings["Host"].ToString();
                            string CCMailId = ConfigurationManager.AppSettings["CCMailIdPercentNotification"].ToString();
                            obj.updateUserEmailSent4Delivery(dtuserId.Rows[i]["UserId"].ToString());
                            obj.SendUserEmailDeliveryNotComingNotify(toAddress, subject, body, MailFrom, Pwd, Host, CCMailId, dtuserId.Rows[i]["UserId"].ToString(), Convert.ToString("Delivery % :" + deliverypercent));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DeliveryUserPercentNotiication_" + ex.StackTrace, ex.Message);
            }
        }
        #endregion

        #region <<  MONTHLY REPORT  >>
        private void TimerForReportSending_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bTimerForReportSending)
                {
                    bTimerForReportSending = true;
                    SendLinkextmonthlyReport();
                    bTimerForReportSending = false;
                }

            }
            catch (Exception ex)
            {
                bTimerForReportSending = false;
                LogError("TimerForReportSending_" + ex.StackTrace, ex.Message);
            }
        }

        protected void SendLinkextmonthlyReport()
        {
            string user = "";
            string Day = ConfigurationManager.AppSettings["Day"].ToString();
            string Hour = ConfigurationManager.AppSettings["Hour"].ToString();
            string Minute = ConfigurationManager.AppSettings["Minute"].ToString();
            if (DateTime.Now.Day == Convert.ToInt64(Day) && DateTime.Now.Hour == Convert.ToInt64(Hour) && DateTime.Now.Minute == Convert.ToInt64(Minute))
            {
                Util obj = new Util();
                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                string SenderID = ConfigurationManager.AppSettings["SenderID"].ToString();
                string PWD = ConfigurationManager.AppSettings["PWD"].ToString();
                string Subject = ConfigurationManager.AppSettings["Subject"].ToString();
                string ToMailId = ConfigurationManager.AppSettings["ToMailId"].ToString();
                string CCMailId = ConfigurationManager.AppSettings["CCMailId"].ToString();
                string Body = ConfigurationManager.AppSettings["Body"].ToString();
                DataTable dt = new DataTable();
                dt = database.GetDataTable(@"select d.userid,
                sum(submitted) Submitted ,
                sum(delivered) Delivered,
                sum(Failed) Failed, 
                sum(Unknown) Unknown from DAYSUMMARY d with(nolock)
                inner join CUSTOMER c on c.username=d.USERID
                where c.usertype='user' and c.DLTNO<>'1201159142280175288' and c.DLTNO<>'HYUNDAISALES' and c.DLTNO<>'DEALERSALES' 
                and c.DLTNO<>'HYUNDAISERVICE' and c.DLTNO<>'DEALERSERVICE' and c.DLTNO<>'HASC'
                and convert(date,d.smsdate) between DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
                and EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)))
                group by d.userid order by userid");

                string subPath = "MonthlysummaryReportsLinkext/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                bool exists = System.IO.Directory.Exists(mappath);
                if (!exists) System.IO.Directory.CreateDirectory(mappath);
                if (dt.Rows.Count > 0)
                {
                    DataTable data = dt.AsEnumerable().CopyToDataTable();
                    StringBuilder sbText = new StringBuilder();
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        sbText.Append(data.Columns[i].ColumnName + ",");
                        sbText.Append("\t");
                    }
                    sbText.Append(Environment.NewLine);

                    foreach (DataRow dr in data.Rows)
                    {
                        for (int i = 0; i < data.Columns.Count; i++)
                        {
                            if (dr[i].ToString() == "")
                            {
                                sbText.Append(String.Format("{0}", ","));
                                sbText.Append("\t");
                            }
                            else
                            {
                                sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                                sbText.Append("\t");
                            }
                        }
                        sbText.Append(Environment.NewLine);
                    }
                    StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                    {
                        sw.Write(sbText.ToString());
                        sw.Close();
                    }
                    sbText.Clear();
                }

                string startPath = mappath;//folder to add
                //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
                string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_NyyyyMMddHHmmss") + ".zip";//URL for your ZIP file
                if (File.Exists(zipPath)) File.Delete(zipPath);

                System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                System.IO.Directory.Delete(mappath, true);//DeleteFolder
                path0 = zipPath;
                string mailmsg = Body;
                SendHondatmonthlyReport();
                SendHyundaiSalesMonthlyReport();
                SendHyundaiServiceMonthlyReport();
                SendHyundaiDealerSalesMonthlyReport();
                SendHyundaiDealerServiceMonthlyReport();
                SendHyundaiHASCMonthlyReport();
                string re = obj.SendEmail(ToMailId, Subject, mailmsg, SenderID, PWD, Host, CCMailId, path0, path1, path2, path3, path4, path5, path6);
            }
        }

        protected void SendHondatmonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT M.PROFILEID AS ACCOUNTID,COUNT(M.ID) AS SUBMITTED,
                sum(case when isnull(d.dlvrstatus,'')<>'Delivered' then 0 else 1 end) as DELIVERED,
                sum(case when isnull(d.dlvrstatus,'')<>'Delivered' then 1 else 0 end) as FAILED
                FROM MSGSUBMITTEDLOG M WITH (NOLOCK) inner join customer c on m.PROFILEID=c.username
                LEFT JOIN DELIVERYLOG d with(nolock) on M.msgid = d.msgid
                WHERE Convert(Date,M.SENTDATETIME) >=
                DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))  
                and Convert(Date,M.SENTDATETIME) <= EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)))
                and C.DLTNO='1201159142280175288'
                GROUP BY M.PROFILEID
                ORDER BY M.PROFILEID");

            string subPath = "HondasummaryReportsMonthly/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path1 = zipPath;
        }

        protected void SendHyundaiSalesMonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT FULLNAME,userid, 'HYUNDAISALES' as Type,
            sum(submitted) Submitted,
            sum(delivered) Delivered,
            sum(Failed) Failed, 
            sum(Unknown) Unknown,COMPNAME FROM CUSTOMER Cus WITH(NOLOCK) 
            INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' 
            WHERE smsdate >= DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
            and smsdate <=EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))) and DLTNO IN('HYUNDAISALES') 
            group by FULLNAME,userid,COMPNAME ORDER BY 1");

            string subPath = "HyundaiSalesMonthlyReports/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_1yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path2 = zipPath;
        }

        protected void SendHyundaiServiceMonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT FULLNAME,userid,
            sum(submitted) Submitted, 'HYUNDAISERVICE' as Type,
            sum(delivered) Delivered,
            sum(Failed) Failed, 
            sum(Unknown) Unknown,COMPNAME FROM CUSTOMER Cus WITH(NOLOCK) 
            INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' 
            WHERE smsdate >= DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
            and smsdate <=EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))) 
            and DLTNO IN('HYUNDAISERVICE') 
            group by FULLNAME,userid,COMPNAME ORDER BY 1");

            string subPath = "HyundaiServiceMonthly/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_2yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path3 = zipPath;
        }

        protected void SendHyundaiDealerSalesMonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT FULLNAME,userid, 'DEALERSALES' as Type,
            sum(submitted) Submitted,
            sum(delivered) Delivered,
            sum(Failed) Failed, 
            sum(Unknown) Unknown,COMPNAME FROM CUSTOMER Cus WITH(NOLOCK) 
            INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' 
            WHERE smsdate >= DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
            and smsdate <=EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))) 
            and DLTNO IN('DEALERSALES') 
            group by FULLNAME,userid,COMPNAME ORDER BY 1");

            string subPath = "HyundaiDealerSalesMonthly/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_3yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path4 = zipPath;
        }

        protected void SendHyundaiDealerServiceMonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT FULLNAME,userid, 'DEALERSERVICE' as Type,
            sum(submitted) Submitted,
            sum(delivered) Delivered,
            sum(Failed) Failed, 
            sum(Unknown) Unknown,COMPNAME FROM CUSTOMER Cus WITH(NOLOCK) 
            INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' 
            WHERE smsdate >= DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
            and smsdate <=EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))) 
            and DLTNO IN('DEALERSERVICE') 
            group by FULLNAME,userid,COMPNAME ORDER BY 1");

            string subPath = "HyundaiDealerServiceMonthlyReport/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_4yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path5 = zipPath;
        }

        protected void SendHyundaiHASCMonthlyReport()
        {
            string user = "";

            Util obj = new Util();
            DataTable dt = new DataTable();
            dt = database.GetDataTable(@"SELECT FULLNAME,userid, 'HASC' as Type,
            sum(submitted) Submitted,
            sum(delivered) Delivered,
            sum(Failed) Failed, 
            sum(Unknown) Unknown,COMPNAME FROM CUSTOMER Cus WITH(NOLOCK) 
            INNER JOIN DAYSUMMARY DS WITH(NOLOCK) ON Cus.username=DS.userid and cus.usertype='User' 
            WHERE smsdate >= DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date)) 
            and smsdate <=EOMONTH(DATEADD(month,-1,CAST(CAST(YEAR(getdate()) AS varchar(4)) + '/' + CAST(MONTH(getdate()) as varchar(2)) + '/01' AS date))) 
            and DLTNO IN('HASC') 
            group by FULLNAME,userid,COMPNAME ORDER BY 1");

            string subPath = "HyundaiHASCMonthly/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);
            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                StringBuilder sbText = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sbText.Append(data.Columns[i].ColumnName + ",");
                    sbText.Append("\t");
                }
                sbText.Append(Environment.NewLine);

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (dr[i].ToString() == "")
                        {
                            sbText.Append(String.Format("{0}", ","));
                            sbText.Append("\t");
                        }
                        else
                        {
                            sbText.Append(String.Format("{0}", dr[i].ToString().Replace(",", " ") + ","));
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(Environment.NewLine);
                }
                StreamWriter sw = new StreamWriter(mappath + @"\" + user + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
            }

            string startPath = mappath;//folder to add
                                       //string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH1"].ToString() + DateTime.Now.ToString("_5yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);//DeleteFolder
            path6 = zipPath;
        }

        #endregion

        #region << EXE NOTIFICATION >>
        private void TimerForExeNotification_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bTimerForExeNotification)
                {
                    bTimerForExeNotification = true;
                    ExeNotification();
                    bTimerForExeNotification = false;
                }

            }
            catch (Exception ex)
            {
                bProcessReport = false;
                LogError("PROCESSTimerReport_" + ex.StackTrace, ex.Message);
            }
        }

        protected void ExeNotification()
        {
            string Host = ConfigurationManager.AppSettings["HostNotification"].ToString();
            string SenderID = ConfigurationManager.AppSettings["SenderIDNotification"].ToString();
            string PWD = ConfigurationManager.AppSettings["PWDNotification"].ToString();
            string Subject = ConfigurationManager.AppSettings["SubjectNotification"].ToString();
            string ToMailId = ConfigurationManager.AppSettings["ToMailIdNotification"].ToString();
            string CCMailId = ConfigurationManager.AppSettings["CCMailIdNotification"].ToString();
            string Body = ConfigurationManager.AppSettings["BodyNotification"].ToString();
            Util obj = new Util();
            int stat = Convert.ToInt16(database.GetScalarValue("select CASE WHEN SMPPClientRunning = '1' AND DATEDIFF(SECOND, SMPPClientRunningDateTime, GETDATE()) > 100 THEN 1 ELSE 0 END AS STAT from ExeNotification "));
            if (stat == 1)
            {
                database.ExecuteNonQuery("update ExeNotification set SMPPClientRunning = 0 ");
                string GetData = Convert.ToString(database.GetScalarValue("sp_ExeNotification"));
                string re = obj.SendEmailExeNotification(ToMailId, Subject, Body, SenderID, PWD, Host, CCMailId);
                // Get the path to the executable
                string exePath = ConfigurationManager.AppSettings["SMPPClientRunning1"].ToString();

                // Start the executable
                StartProcess(exePath);

            }
            int stat2 = Convert.ToInt16(database.GetScalarValue("select CASE WHEN SMPPClientRunning2 = '1' AND DATEDIFF(SECOND, SMPPClientRunningDateTime2, GETDATE()) > 100 THEN 1 ELSE 0 END AS STAT from ExeNotification "));
            if (stat2 == 1)
            {
                database.ExecuteNonQuery("update ExeNotification set SMPPClientRunning2 = 0 ");
                string GetData = Convert.ToString(database.GetScalarValue("sp_ExeNotification"));
                string re = obj.SendEmailExeNotification(ToMailId, Subject, Body, SenderID, PWD, Host, CCMailId);

                // Get the path to the executable
                string exePath = ConfigurationManager.AppSettings["SMPPClientRunning2"].ToString();

                // Start the executable
                StartProcess(exePath);
            }
        }
        #endregion

        private void timerPROCESSReport_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bProcessReport)
                {
                    bProcessReport = true;
                    DownLoad_Report();
                    bProcessReport = false;
                }
            }
            catch (Exception ex)
            {
                bProcessReport = false;
                LogError("PROCESSTimerReport_" + ex.StackTrace, ex.Message);
            }
        }

        protected void DownLoad_Report()
        {
            string requestDate1 = "";
            string requestDate2 = "";
            string user = "";
            string DownloadType = "";

            Util ob = new Util();
            DataTable dtRequest = ob.GetRequestForReport();
            if (dtRequest.Rows.Count > 0)
            {
                DownloadType = dtRequest.Rows[0]["DownloadType"].ToString().Trim();
                requestDate1 = Convert.ToString(dtRequest.Rows[0]["DLRfrom"].ToString().Trim());
                requestDate2 = Convert.ToString(dtRequest.Rows[dtRequest.Rows.Count - 1]["DLRTo"].ToString().Trim());
                user = Convert.ToString(dtRequest.Rows[0]["userid"].ToString().Trim());

                if (DownloadType == "D")
                {
                    foreach (DataRow dr in dtRequest.Rows)
                    {
                        requestDate1 = Convert.ToString(dr["DLRFrom"]);
                        requestDate2 = Convert.ToString(dr["DLRTo"]);
                        user = Convert.ToString(dr["userid"]);
                        string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");

                        for (var day = Convert.ToDateTime(requestDate1).Date; day.Date <= Convert.ToDateTime(requestDate2).Date; day = day.AddDays(1))
                        {
                            DateTime fromDate = day;
                            string date = fromDate.ToString("yyyy-MM-dd");
                            int fn = 1;

                            bool IsTdy = false;
                            string cDate = DateTime.Now.ToString("yyyy-MM-dd");
                            if (date == cDate)
                            {
                                IsTdy = true;
                            }

                            string cMonth = DateTime.Now.ToString("MM");
                            string rMonth = fromDate.ToString("MM");
                            //if (cMonth == rMonth)
                            {
                                DataTable dtFileId = ob.GetSMSReport_user_FILEID(date, date + " 23:59:59.997", user);
                                if (dtFileId.Rows.Count > 0)
                                {
                                    foreach (DataRow drow in dtFileId.Rows)
                                    {
                                        DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(date, date + " 23:59:59.997", user, drow["FILEID"].ToString(), IsTdy);
                                        DataSplit(dt, mappath);
                                    }
                                }
                                else
                                {
                                    DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(date, date + " 23:59:59.997", user, "", IsTdy);
                                    DataSplit(dt, mappath);
                                }
                            }
                            //else
                            //{
                            //    DataTable dtFileId = ob.GetSMSReport_user_FILEID_B4(date, date + " 23:59:59.997", user);
                            //    if (dtFileId.Rows.Count > 0)
                            //    {
                            //        foreach (DataRow drow in dtFileId.Rows)
                            //        {
                            //            DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL_B4(date, date + " 23:59:59.997", user, drow["FILEID"].ToString());
                            //            fn = WriteData(mappath, dt, fn);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL_B4(date, date + " 23:59:59.997", user, "");
                            //        fn = WriteData(mappath, dt, fn);
                            //    }
                            //}
                        }

                        string startPath = mappath;//folder to add
                        string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + user + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
                        if (File.Exists(zipPath)) File.Delete(zipPath);
                        System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                        System.IO.Directory.Delete(mappath, true);

                        ob.SaveGenratedRequestPath(zipPath, user, dr["id"].ToString());
                        ob.DeactiveRequest();
                    }
                }
                else
                {
                    string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");

                    DateTime fromDate = Convert.ToDateTime(requestDate1).Date;
                    DateTime ToDate = Convert.ToDateTime(requestDate2).Date;
                    string F_date = fromDate.ToString("yyyy-MM-dd");
                    string To_Date = ToDate.ToString("yyyy-MM-dd");
                    int fn = 1;

                    DataTable dtFileId = ob.GetSMSReport_user_FILEID(F_date, To_Date + " 23:59:59.997", user);
                    if (dtFileId.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dtFileId.Rows)
                        {
                            DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(F_date, To_Date + " 23:59:59.997", user, drow["FILEID"].ToString(), false);
                            DataSplit(dt, mappath);
                        }
                    }
                    else
                    {
                        DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(F_date, To_Date + " 23:59:59.997", user, "", false);
                        int a = dt.Rows.Count;

                        DataSplit(dt, mappath);
                    }
                    string startPath = mappath;//folder to add
                    string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + user + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
                    if (File.Exists(zipPath)) File.Delete(zipPath);
                    System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                    System.IO.Directory.Delete(mappath, true);
                    foreach (DataRow dr in dtRequest.Rows)
                    {
                        ob.SaveGenratedRequestPath(zipPath, user, dr["id"].ToString());
                    }
                    ob.DeactiveRequest();
                }
            }
        }

        #region < Commented >
        //protected void SendClickReport()
        //{

        //    string user = ""; 

        //    Util ob = new Util();
        //    DataTable dtRequest = ob.GetCustomerForReport();
        //    if (dtRequest.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtRequest.Rows.Count; i++)
        //        {

        //            user = Convert.ToString(dtRequest.Rows[0]["username"].ToString().Trim());

        //            string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + "ClickReport" + DateTime.Now.ToString("ddMMyyyyHHmmss");

        //            DateTime fromDate = DateTime.Now.AddDays(-1);
        //            DateTime ToDate = DateTime.Now.AddDays(-1);
        //            string F_date = fromDate.ToString("yyyy-MM-dd");
        //            string To_Date = ToDate.ToString("yyyy-MM-dd");
        //            int fn = 1;
        //            DataTable dt = ob.GetClickReport(user,F_date, To_Date + " 23:59:59.997");
        //            DataView view = new DataView(dt);
        //            dt = view.ToTable(true, "Slno", "mobile", "smsDate", "ClickDate", "operator", "Circle");
        //            fn = newWriteData(mappath, dt, fn, "ClickDate");
        //            string startPath = mappath;//folder to add
        //            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + user + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".zip";//URL for your ZIP file
        //            if (File.Exists(zipPath)) File.Delete(zipPath);
        //            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
        //            System.IO.Directory.Delete(mappath, true);
        //            if (bool.Parse(dtRequest.Rows[i]["ReportonEmail"].ToString()) == true && dtRequest.Rows[i]["CCEmail"].ToString() != "" && dtRequest.Rows[i]["Email"].ToString() != "")
        //            {
        //                string[] CCEmail = dtRequest.Rows[i]["CCEmail"].ToString().Split(';');
        //                string Subject = "Click Report for " + fromDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                StringBuilder body = new StringBuilder();
        //                body.Append("Dear Customer,  <br /> Please find attached report for <br /> Click Report for " + fromDate.ToString("yyyy-MMM-dd") + " ");
        //                DWReportsendmail(zipPath, dtRequest.Rows[i]["EMAIL"].ToString(), "support@myinboxmedia.io", "MiM#987654321", "smtpout.secureserver.net", "25", CCEmail, fromDate, Subject, body);
        //            }
        //        }
        //    }
        //}
        #endregion

        protected void DataSplit(DataTable dt, string mappath)
        {
            if (dt.Rows.Count < 1 || dt == null) return;
            int fn = 1;
            string S = System.Configuration.ConfigurationManager.AppSettings["DLRFileDownloadSize"].ToString();
            int size = Convert.ToInt32(S);
            double filecount = Convert.ToDouble(dt.Rows.Count) / size;
            DataTable dt1;
            DataTable dt2 = new DataTable();
            int filecount1 = Convert.ToInt32((int)Math.Ceiling(filecount));
            for (int i = 0; i < filecount; i++)
            {

                if (dt2.Rows.Count > 0 && dt2 != null)
                {
                    dt = dt2;
                }
                dt1 = dt.AsEnumerable().Take(size).CopyToDataTable();
                fn = WriteData(mappath, dt1, fn);
                if (dt.Rows.Count < size) break;
                dt2 = dt.AsEnumerable().Skip(size).CopyToDataTable();
            }
        }

        protected void SendSMSReport()
        {
            string AutoSentRptMonthly = ConfigurationManager.AppSettings["AutoSendRptMonthlyDate"].ToString(); //05
            string user = "";
            string DownloadType = "";
            bool MonthlyRptTorF = true;
            DateTime fromDate = DateTime.Now.AddDays(-1);
            DateTime ToDate = DateTime.Now.AddDays(-1);
            string F_date = "";
            string To_Date = "";
            string Subject = "";
            try
            {
                Util ob = new Util();
                DataTable dtRequest = ob.GetCustomerForReport();
                if (dtRequest.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRequest.Rows.Count; i++)
                    {
                        DownloadType = "S";
                        user = Convert.ToString(dtRequest.Rows[i]["username"].ToString().Trim());
                        string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + "Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        if (user == "MIM2200832" || user == "MIM2200812")
                        {
                            DateTime currentDate = DateTime.Now; //22-03-2023 15:44:01
                            string day = currentDate.ToString("dd"); //05
                            if (day == AutoSentRptMonthly)
                            {
                                fromDate = DateTime.Now.AddMonths(-1); //{22-02-2023 15:51:41
                                ToDate = DateTime.Now;
                                F_date = fromDate.ToString("yyyy-MM-dd");
                                To_Date = ToDate.ToString("yyyy-MM-dd");
                                MonthlyRptTorF = true;
                            }
                            else
                            {
                                fromDate = DateTime.Now.AddDays(-1);
                                ToDate = DateTime.Now.AddDays(-1);
                                F_date = fromDate.ToString("yyyy-MM-dd");
                                To_Date = ToDate.ToString("yyyy-MM-dd");
                                MonthlyRptTorF = true;
                            }
                        }
                        else
                        {
                            fromDate = DateTime.Now.AddDays(-1);
                            ToDate = DateTime.Now.AddDays(-1);
                            F_date = fromDate.ToString("yyyy-MM-dd");
                            To_Date = ToDate.ToString("yyyy-MM-dd");
                            MonthlyRptTorF = true;
                        }
                        if (MonthlyRptTorF == true)
                        {
                            int fn = 1;
                            DataTable dtFileId = ob.GetSMSReport_user_FILEID(F_date, To_Date + " 23:59:59.997", user);

                            bool norecord = true;
                            if (dtFileId.Rows.Count > 0)
                            {
                                foreach (DataRow drow in dtFileId.Rows)
                                {
                                    DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(F_date, To_Date + " 23:59:59.997", user, drow["FILEID"].ToString(), false);
                                    if (dt.Rows.Count > 0) norecord = false;
                                    fn = newWriteData(mappath, dt, fn, "smsdate");
                                }
                            }
                            else
                            {
                                DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(F_date, To_Date + " 23:59:59.997", user, "", false);
                                if (dt.Rows.Count > 0) norecord = false;
                                fn = newWriteData(mappath, dt, fn, "smsdate");
                            }
                            if (norecord == false)
                            {
                                string startPath = mappath;//for SMS Report
                                string smspath = user + DateTime.Now.ToString("_yyyyMMddHHmmss") + "_SMSReport" + ".zip";//URL for your ZIP file
                                string smszipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + smspath;
                                string smsserverzipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATHserver"].ToString() + smspath;
                                if (File.Exists(smszipPath)) File.Delete(smszipPath);
                                System.IO.Compression.ZipFile.CreateFromDirectory(startPath, smszipPath, CompressionLevel.Fastest, true);
                                System.IO.Directory.Delete(mappath, true);


                                DataTable dtclc = ob.GetClickReport(user, F_date, To_Date + " 23:59:59.997");
                                DataView view = new DataView(dtclc);
                                dtclc = view.ToTable(true, "Slno", "mobile", "smsDate", "ClickDate", "operator", "Circle");
                                fn = newWriteData(mappath, dtclc, fn, "ClickDate");

                                string clckstartPath = mappath;//for Click Report
                                string clcpath = user + DateTime.Now.ToString("_yyyyMMddHHmmss") + "_ClickReport" + ".zip";//URL for your ZIP file
                                string clckzipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + clcpath;
                                string clcserverkzipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATHserver"].ToString() + clcpath;
                                if (File.Exists(clckzipPath)) File.Delete(clckzipPath);
                                System.IO.Compression.ZipFile.CreateFromDirectory(clckstartPath, clckzipPath, CompressionLevel.Fastest, true);
                                System.IO.Directory.Delete(mappath, true);

                                if (bool.Parse(dtRequest.Rows[i]["ReportonEmail"].ToString()) == true && dtRequest.Rows[i]["CCEmail"].ToString() != "" && dtRequest.Rows[i]["Email"].ToString() != "")
                                {
                                    string[] CCEmail = dtRequest.Rows[i]["CCEmail"].ToString().Split(';');
                                    if (user == "MIM2200832" || user == "MIM2200812")
                                    {
                                        Subject = "SMS Report for " + fromDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture) + " to " + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture) + "";
                                    }
                                    else
                                    {
                                        Subject = "SMS Report for " + fromDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    StringBuilder body = new StringBuilder();
                                    body.Append("Dear Customer,  <br /> Please find below link for <br /> SMS DLR for " + fromDate.ToString("yyyy-MMM-dd") + " <br />" + smsserverzipPath);
                                    if (dtclc.Rows.Count > 0)
                                        body.Append(" <br />CLICK REPORT for " + fromDate.ToString("yyyy-MMM-dd") + "  <br /> " + clcserverkzipPath + "");

                                    //DWReportsendmail(dtRequest.Rows[i]["EMAIL"].ToString(), "support@myinboxmedia.io", "MiM#987654321", "smtpout.secureserver.net", "587", CCEmail, fromDate, Subject, body);
                                    //DWReportsendmail(dtRequest.Rows[i]["EMAIL"].ToString(), "supportmim@myinboxmedia.io", "H5#p2$9#yR4x", "mx.myinboxmedia.io", "587", CCEmail, fromDate, Subject, body);
                                    //sendThroughSMTP_MIMEKIT("supportmim@myinboxmedia.io", "H5#p2$9#yR4x", "587", "mx.myinboxmedia.io", "", "", dtRequest.Rows[i]["EMAIL"].ToString(), Convert.ToString(body), Subject, "", "", "", dtRequest.Rows[i]["CCEmail"].ToString(), "");
                                    SendEMAILthroughAPI("MYINBOXMEDIA<supportmim@myinboxmedia.io>", dtRequest.Rows[i]["EMAIL"].ToString(), dtRequest.Rows[i]["CCEmail"].ToString(), Subject, body.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("EmailReport_" + ex.StackTrace, ex.Message);
            }
        }

        public string sendThroughSMTP_MIMEKIT(string Email_From, string Email_Password, string Email_Port, string Email_Host, string Email_EnableSSL, string USEDEFAULTCREDENTIALS,
            string To, string Body, string subject, string ReplyTo, string displayname, string Attach, string sCC, string bcc)
        {

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(Email_Host, Convert.ToInt16(Email_Port), SecureSocketOptions.None);
                }
                catch (Exception ex)
                {
                    LogError("EmailReport Mail Failed as mail server connection issue_" + ex.StackTrace, ex.Message);
                    //MessageBox.Show("Connection issue - " + ex.Message + " - ST " + ex.StackTrace);
                    return "fail";
                }

                client.Authenticate(Email_From, Email_Password);

                MimeMessage mm = new MimeMessage();
                mm.From.Add(new MailboxAddress(displayname, Email_From));
                mm.To.Add(new MailboxAddress("", To));

                //==================================//

                if (sCC.Trim() != string.Empty)
                    mm.Cc.Add(new MailboxAddress("", sCC));

                if (bcc.Trim() != string.Empty)
                    mm.Bcc.Add(new MailboxAddress("", bcc));

                mm.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = Body;
                mm.Body = builder.ToMessageBody();
                mm.Headers.Add("Identity", Convert.ToString(Guid.NewGuid()));
                string res = "";
                try
                {
                    client.Send(mm);
                    LogError("email Sent ", "to:" + To + " sub " + subject + " BODY " + Body.ToString());
                    res = "Mail Sent";
                }
                catch (Exception e5) { res = "Mail failed. " + e5.Message; LogError("Mail Failed " + e5.StackTrace, e5.Message); }
                return res;
            }
        }

        #region Add By Vikas Send EMail Api
        public void SendEMAILthroughAPI(string From, string To, string CC, string Subject, string msgtext)
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
                @"    ""ProfileId"": """ + "MIM2400428" + @""",
" + "\n" +
                @"    ""APIKey"": """ + "H5#p2$9#yR4x" + @""",
" + "\n" +
                @"    ""From"": """ + From + @""",
" + "\n" +
                @"    ""To"": """ + To + @""",
" + "\n" +
                @"    ""CC"": """ + CC + @""",
" + "\n" +
                @"    ""Subject"": """ + Subject + @""",
" + "\n" +
                @"    ""Text"": """ + "" + @""",
" + "\n" +
                @"    ""html"": """ + msgtext + @"""
" + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                LogError("req-" + body, " resp-" + response.Content);
                try
                {

                }
                catch (Exception ex)
                {
                    LogError("SendEMAILthroughAPI 1 - ", ex.Message);
                }
            }
            catch (Exception e1) { LogError("SendEMAILthroughAPI 2 - ", e1.Message); }
        }
        #endregion.

        private static int newWriteData(string mappath, DataTable dt, int fn, string Datename)
        {
            DataView dv = new DataView(dt);
            DataTable dtDates = dv.ToTable(true, Datename);

            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);

            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                string mydate = dtDates.Rows[0][Datename].ToString();

                StringBuilder sbText = new StringBuilder();
                //write column header names
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        sbText.Append(",");
                    }
                    sbText.Append(data.Columns[i].ColumnName);
                }
                sbText.Append(Environment.NewLine);

                //Write data
                foreach (DataRow row in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbText.Append(",");
                        }
                        if (row[i].ToString().Contains(","))
                        {
                            sbText.Append(String.Format("\"{0}\"", row[i].ToString()));
                        }
                        else
                        {
                            if (i == 2)
                                sbText.Append(String.Format("'{0}", row[i].ToString()));
                            else
                                sbText.Append(row[i].ToString());
                        }
                        // sbText.Append(row[i].ToString());
                    }
                    sbText.Append(Environment.NewLine);
                }

                StreamWriter sw = new StreamWriter(mappath + @"\" + mydate.Replace("/", "-").Replace(".", "-").Replace(":", "_") + "_" + fn.ToString() + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                fn++;
            }

            return fn;
        }

        private static int WriteData(string mappath, DataTable dt, int fn)
        {
            DataView dv = new DataView(dt);
            DataTable dtDates = dv.ToTable(true, "smsdate");

            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);

            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                string mydate = dtDates.Rows[0]["SMSdate"].ToString();

                StringBuilder sbText = new StringBuilder();
                //write column header names
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        sbText.Append(",");
                    }
                    sbText.Append(data.Columns[i].ColumnName);
                }
                sbText.Append(Environment.NewLine);

                //Write data
                foreach (DataRow row in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbText.Append(",");
                        }
                        if (row[i].ToString().Contains(","))
                        {
                            sbText.Append(String.Format("\"{0}\"", row[i].ToString()));
                        }
                        else
                        {
                            if (i == 2)
                                sbText.Append(String.Format("'{0}", row[i].ToString()));
                            else
                                sbText.Append(row[i].ToString());
                        }
                        // sbText.Append(row[i].ToString());
                    }
                    sbText.Append(Environment.NewLine);
                }

                StreamWriter sw = new StreamWriter(mappath + @"\" + mydate.Replace(".", "-") + "_" + fn.ToString() + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                fn++;
            }
            return fn;
        }

        //private void ProcessSchedule() B4 07/03/22
        //{
        //    string sql = "";
        //    string tim = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
        //    string tim2 = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
        //    Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and PICKED_DATETIME is null"));
        //    if (c > 0)
        //    {
        //        c = 0;
        //        c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blacklist,0)=1 "));

        //        sql = "Update MsgSchedule set PICKED_DATETIME='" + tim2 + "',msgid=newid() where schedule='" + tim + "' and PICKED_DATETIME is null ;  ";
        //        database.ExecuteNonQuery(sql);

        //        string strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 ; ";
        //        DataTable dt1 = database.GetDataTable(strsq);

        //        if (c > 0)
        //        {
        //            if (dt1.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
        //                    bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
        //                    for (int x = 0; x < noofsms; x++)
        //                    {
        //                        database.ExecuteNonQuery("Update MsgSchedule set msgid = newid() WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 ");
        //                        string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
        //                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                        " select ID,PROVIDER,SMPPACCOUNTID,PROFILEID," + strmsg + ",TOMOBILE,SENDERID,CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1'," +
        //                        " MSGTEXT,smsrate from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; " +
        //                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //                        @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
        //                        ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
        //                        from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; ";
        //                        database.ExecuteNonQuery(sql);
        //                    }
        //                }
        //            }
        //        }

        //        c = 0;
        //        c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blockno,0)=1 "));
        //        if (c > 0)
        //        {
        //            strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockno,0)=1 ; ";
        //            dt1 = database.GetDataTable(strsq);

        //            if (dt1.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
        //                    bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
        //                    for (int x = 0; x < noofsms; x++)
        //                    {
        //                        database.ExecuteNonQuery("Update MsgSchedule set msgid = newid() WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockno,0)=1 ");
        //                        string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
        //                        sql = @" insert into notsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smsrate)
        //                        select ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',smsrate from msgschedule where schedule='" + tim + "' and isnull(blockno,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; " +
        //                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                        " select ID, PROVIDER, SMPPACCOUNTID, PROFILEID," + strmsg + ",TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',MSGTEXT,smsrate from msgschedule where schedule='" + tim + "' and isnull(blockno,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "'; ";
        //                        database.ExecuteNonQuery(sql);
        //                    }
        //                }
        //            }
        //        }
        //        c = 0;
        //        c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 "));
        //        if (c > 0)
        //        {
        //            strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockFAIL,0)=1 ; ";
        //            dt1 = database.GetDataTable(strsq);

        //            if (dt1.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
        //                    bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
        //                    for (int x = 0; x < noofsms; x++)
        //                    {
        //                        database.ExecuteNonQuery("Update MsgSchedule set msgid = newid() WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockFAIL,0)=1 ");
        //                        string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
        //                        sql = @" insert into FAILSUBMITTED(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smsrate)
        //                        select ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',smsrate from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' and err_code is null ; " +
        //                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
        //                        " select ID, PROVIDER, SMPPACCOUNTID, PROFILEID," + strmsg + ",TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',MSGTEXT,smsrate from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "'; ";
        //                        sql = sql + @" Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,INSERTDATE) " +
        //                           @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + 
        //                            ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + err_code + ' text:' ,
        //                         MSGID, GETDATE() , GETDATE() ,'Undeliverable' ,err_code,getdate() " +
        //                        @" from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' and err_code is not null " ; 
        //                        database.ExecuteNonQuery(sql);
        //                    }
        //                }
        //            }
        //        }

        //        sql = " insert into msgtran (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,DataCode,smsrate,Templateid) " +
        //            " SELECT PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,ISNULL(FILEID,0) AS FILEID,peid,DataCode,smsrate,Templateid " +
        //            " FROM MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=0 and isnull(blockno,0)=0 and isnull(blockFAIL,0)=0 ; ";

        //        sql = sql + "insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid) select URLID, tomobile, getdate() as sentdate, newsegment as segment,fileid " +
        //            " from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and newsegment is not null ";
        //        database.ExecuteNonQuery(sql);
        //    }
        //}

        private void ProcessSchedule()
        {
            string sql = "";

            string tim = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
            string tim2 = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            //tim = "05-Jan-2023 09:30";
            //tim2 = "05-Jan-2023 09:30:00";
            Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and PICKED_DATETIME is null"));
            if (c > 0)
            {
                c = 0;
                c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blacklist,0)=1 "));

                sql = "Update MsgSchedule set PICKED_DATETIME='" + tim2 + "',msgid=replace(newid(),'-','')  where schedule='" + tim + "' and PICKED_DATETIME is null ;  ";
                database.ExecuteNonQuery(sql);

                string strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 ; ";
                DataTable dt1 = database.GetDataTable(strsq);

                if (c > 0)
                {
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
                            bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                            for (int x = 0; x < noofsms; x++)
                            {
                                //Add peid and templateid at 10-04-2023 by Naved
                                database.ExecuteNonQuery("Update MsgSchedule set msgid = replace(newid(),'-','') WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 ");
                                string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                " select ID,PROVIDER,SMPPACCOUNTID,PROFILEID," + strmsg + ",TOMOBILE,SENDERID,CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1'," +
                                " MSGTEXT,smsrate,peid,templateid from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                @" select 'id:' + MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                                ' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:BlackList err:250 text:' AS DLVRTEXT, MSGID, GETDATE(), 'BlackList','250',getdate()
                                from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }

                c = 0;
                c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blockno,0)=1 "));
                if (c > 0)
                {
                    strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockno,0)=1 ; ";
                    dt1 = database.GetDataTable(strsq);

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
                            bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                            for (int x = 0; x < noofsms; x++)
                            {
                                //Add peid and templateid at 10-04-2023 by Naved
                                database.ExecuteNonQuery("Update MsgSchedule set msgid = replace(newid(),'-','') WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockno,0)=1 ");
                                string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
                                sql = @" insert into notsubmitted(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smsrate)
                                select ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',smsrate from msgschedule where schedule='" + tim + "' and isnull(blockno,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ; " +
                                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                " select ID, PROVIDER, SMPPACCOUNTID, PROFILEID," + strmsg + ",TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',MSGTEXT,smsrate,peid,templateid from msgschedule where schedule='" + tim + "' and isnull(blockno,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "'; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                c = 0;
                c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 "));
                if (c > 0)
                {
                    strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockFAIL,0)=1 ; ";
                    dt1 = database.GetDataTable(strsq);

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
                            bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                            for (int x = 0; x < noofsms; x++)
                            {
                                //Add peid and templateid at 10-04-2023 by Naved
                                database.ExecuteNonQuery("Update MsgSchedule set msgid = replace(newid(),'-','') WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blockFAIL,0)=1 ");
                                string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
                                sql = @" insert into FAILSUBMITTED(ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT, SENTDATETIME, MSGID, INSERTDATE, FILEID, NSEND, smsrate)
                                select ID, PROVIDER, SMPPACCOUNTID, PROFILEID, MSGTEXT, TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',smsrate from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' and err_code is null ; " +
                                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                " select ID, PROVIDER, SMPPACCOUNTID, PROFILEID," + strmsg + ",TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',MSGTEXT,smsrate,peid,templateid from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "'; ";
                                sql = sql + @" Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,INSERTDATE) " +
                                   @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + 
                                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:' + err_code + ' text:' ,
                                 MSGID, GETDATE() , GETDATE() ,'Undeliverable' ,err_code,getdate() " +
                                @" from msgschedule where schedule='" + tim + "' and isnull(blockFAIL,0)=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' and err_code is not null ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }

                c = 0;
                c = Convert.ToInt64(database.GetScalarValue("select count(*) from MsgSchedule where schedule='" + tim + "' and DNDFilter=1 "));
                if (c > 0)
                {
                    strsq = "Select distinct len(msgtext) as msglen, DataCode from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and DNDFilter=1 ; ";
                    dt1 = database.GetDataTable(strsq);

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            int noofsms = GetNoOfSMS(Convert.ToInt16(dt1.Rows[i]["msglen"]), dt1.Rows[i]["DataCode"].ToString());
                            bool ucs2 = (dt1.Rows[i]["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                            for (int x = 0; x < noofsms; x++)
                            {
                                //Add peid and templateid at 10-04-2023 by Naved
                                database.ExecuteNonQuery("Update MsgSchedule set msgid = replace(newid(),'-','') WHERE PICKED_DATETIME='" + tim2 + "' and DNDFilter=1  ");
                                string strmsg = GetSMSText(Convert.ToInt16(dt1.Rows[i]["msglen"]), x + 1, noofsms, ucs2);
                                sql = @" Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                " select ID, PROVIDER, SMPPACCOUNTID, PROFILEID," + strmsg + ",TOMOBILE, SENDERID, CREATEDAT,GETDATE(),msgid,getdate(),fileid,'1',MSGTEXT,smsrate,peid,templateid from msgschedule where schedule='" + tim + "' and DNDFilter=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "'; ";
                                sql = sql + @" Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,INSERTDATE) " +
                                   @" select 'id:' + MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + 
                                    ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:DND err:103 text:' ,
                                 MSGID, GETDATE() , GETDATE() ,'DND' ,'103',getdate() " +
                                @" from msgschedule where schedule='" + tim + "' and DNDFilter=1 and len(msgtext) ='" + dt1.Rows[i]["msglen"].ToString() + "' and DataCode='" + dt1.Rows[i]["DataCode"].ToString() + "' ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }

                DataTable tranttbl = database.GetDataTable("select Distinct TranTablename FROM MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and isnull(blacklist,0)=0 and isnull(blockno,0)=0 and isnull(blockFAIL,0)=0 ;");

                sql = "";
                for (int i = 0; i < tranttbl.Rows.Count; i++)
                {
                    sql = sql + " insert into " + tranttbl.Rows[i]["TranTableName"].ToString() + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,DataCode,smsrate,Templateid) " +
                          " SELECT PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,ISNULL(FILEID,0) AS FILEID,peid,DataCode,smsrate,Templateid " +
                          " FROM MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and TranTableName='" + tranttbl.Rows[i]["TranTableName"].ToString() + "' and isnull(blacklist,0)=0 and isnull(blockno,0)=0 and isnull(blockFAIL,0)=0 ; ";
                }

                sql = sql + "insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid) select URLID, tomobile, getdate() as sentdate, newsegment as segment,fileid " +
                    " from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and newsegment is not null and scratchurl is null ; ";

                sql = sql + " Insert into Short_urls (long_url,segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,urlname,richmediaurl) " +
                " select scratchurl,newsegment,getdate(),'','0',profileid,'N','1','https://m1m.io/','','0' from MsgSchedule WHERE PICKED_DATETIME='" + tim2 + "' and newsegment is not null and scratchurl is not null ; ";

                database.ExecuteNonQuery(sql);

                string sc_dbnm = ConfigurationManager.AppSettings["SCRATCHCARD_DBNAME"].ToString();
                //string strApolloClient = Convert.ToString(database.GetScalarValue("SELECT TOP 1 CLIENT FROM (SELECT DENSE_RANK() OVER (PARTITION BY USERID ORDER BY INSERTDATE DESC) AS SL ,* FROM " + sc_dbnm + "clientsetting WHERE USERID='MIM2000014') A WHERE SL=1"));
                sql = " insert into " + sc_dbnm + "TBLSCRATCHVOUCHEROFFERCODE (CLIENT,VOUCHERCODE,OFFERCODE,EXPDATE,ISSUEDATE,mobile,segment) " +
                            "Select scratchClient,right(newid(),11),offercode, dateadd(Year,1,getdate()),getdate(),tomobile,newsegment " +
                            "from MsgSchedule WHERE PICKED_DATETIME = '" + tim2 + "' and newsegment is not null and scratchurl is not null; ";
                database.ExecuteNonQuery(sql);
            }
        }

        public string GetSMSText(int msgLength, int x, int noofsms, bool ucs2)
        {
            string ret = "";
            if (ucs2)
            {
                if (noofsms == 1) ret = "Substring(MSGTEXT,1,70)";
                if (x == 1) { if (msgLength > 70) ret = "Substring(MSGTEXT,1,70)"; else ret = "Substring(MSGTEXT,1,1000)"; }
                if (x == 2) { if (msgLength > 134) ret = "Substring(MSGTEXT,71,64)"; else ret = "Substring(MSGTEXT,71,1000)"; }
                if (x == 3) { if (msgLength > 201) ret = "Substring(MSGTEXT,135,67)"; else ret = "Substring(MSGTEXT,135,1000)"; }
                if (x == 4) { if (msgLength > 268) ret = "Substring(MSGTEXT,202,67)"; else ret = "Substring(MSGTEXT,202,1000)"; }
                if (x == 5) { if (msgLength > 335) ret = "Substring(MSGTEXT,269,67)"; else ret = "Substring(MSGTEXT,269,1000)"; }
                if (x == 6) { if (msgLength > 402) ret = "Substring(MSGTEXT,336,67)"; else ret = "Substring(MSGTEXT,336,1000)"; }
                if (x == 7) { if (msgLength > 469) ret = "Substring(MSGTEXT,403,67)"; else ret = "Substring(MSGTEXT,403,1000)"; }
                if (x == 8) { if (msgLength > 536) ret = "Substring(MSGTEXT,470,67)"; else ret = "Substring(MSGTEXT,470,1000)"; }
                if (x == 9) { if (msgLength > 603) ret = "Substring(MSGTEXT,537,67)"; else ret = "Substring(MSGTEXT,537,1000)"; }
                if (x == 10) { if (msgLength > 670) ret = "Substring(MSGTEXT,604,67)"; else ret = "Substring(MSGTEXT,604,1000)"; }
            }
            else
            {
                if (noofsms == 1) ret = "Substring(MSGTEXT,1,160)";
                if (x == 1) { if (msgLength > 160) ret = "Substring(MSGTEXT,1,160)"; else ret = "Substring(MSGTEXT,1,1000)"; }
                if (x == 2) { if (msgLength > 306) ret = "Substring(MSGTEXT,161,146)"; else ret = "Substring(MSGTEXT,161,1000)"; }
                if (x == 3) { if (msgLength > 459) ret = "Substring(MSGTEXT,307,153)"; else ret = "Substring(MSGTEXT,307,1000)"; }
                if (x == 4) { if (msgLength > 612) ret = "Substring(MSGTEXT,460,153)"; else ret = "Substring(MSGTEXT,460,1000)"; }
                if (x == 5) { if (msgLength > 765) ret = "Substring(MSGTEXT,613,153)"; else ret = "Substring(MSGTEXT,613,1000)"; }
                if (x == 6) { if (msgLength > 918) ret = "Substring(MSGTEXT,766,153)"; else ret = "Substring(MSGTEXT,766,1000)"; }
                if (x == 7) { if (msgLength > 1071) ret = "Substring(MSGTEXT,919,153)"; else ret = "Substring(MSGTEXT,919,1000)"; }
                if (x == 8) { if (msgLength > 1224) ret = "Substring(MSGTEXT,1072,153)"; else ret = "Substring(MSGTEXT,1072,1000)"; }
                if (x == 9) { if (msgLength > 1377) ret = "Substring(MSGTEXT,1225,153)"; else ret = "Substring(MSGTEXT,1225,1000)"; }
                if (x == 10) { if (msgLength > 1530) ret = "Substring(MSGTEXT,1378,153)"; else ret = "Substring(MSGTEXT,1378,1000)"; }
            }
            return ret;
        }
        public int GetNoOfSMS(int qlen, string datacode)
        {
            int noofsms = 0;
            if (datacode.ToString().ToUpper() == "DEFAULT")
            {
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
            }
            else
            {
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

        #region < PROCESS DELIVERY NSEND >
        private void timerDELIVERY_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bProcessDelivery)
                {
                    bProcessDelivery = true;
                    ProcessDelivery();
                    bProcessDelivery = false;
                }
            }
            catch (Exception ex)
            {
                bProcessDelivery = false;
                LogError("DELIVERYTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void ProcessDelivery()
        {
            string sql = "";
            string tim = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
            string tim2 = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            Int64 c = Convert.ToInt64(dbmain.GetScalarValue("select count(*) from NOTSUBMITTED where SENTDATETIME < DATEADD(MINUTE,-3,GETDATE()) AND DELIVERY_DATETIME IS NULL"));
            if (c > 0)
            {
                dbmain.ExecuteNonQuery("update NOTSUBMITTED set DELIVERY_DATETIME ='" + tim2 + "' where SENTDATETIME < DATEADD(MINUTE,-3,GETDATE()) AND DELIVERY_DATETIME IS NULL");

                sql = @" Insert into Delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) SELECT  
                'id:' + N.MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,N.SENTDATETIME,112),6) + REPLACE(cONVERT(VARCHAR,N.SENTDATETIME,108),':','') + 
                ' done date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + ' stat:DELIVRD err:000 text:' + substring(N.MsgText,1,20) AS DLVRTEXT, 
                N.MSGID, GETDATE(), 'Delivered','000',getdate()
                    FROM NOTSUBMITTED N where N.DELIVERY_DATETIME = '" + tim2 + "'";
                dbmain.ExecuteNonQuery(sql);

                if (ProcessDLRCallBack == 1)
                {
                    sql = @" Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno) select N.Profileid,
                    'id:' + N.MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,N.SENTDATETIME,112),6) + REPLACE(cONVERT(VARCHAR,N.SENTDATETIME,108),':','') + 
                    ' done date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + ' stat:DELIVRD err:000 text:' + substring(N.MsgText,1,20) AS DLVRTEXT, 
                    N.MSGID, GETDATE(), GETDATE(), 'Delivered','000', N.SENDERID, N.TOMOBILE FROM NOTSUBMITTED N WITH (NOLOCK) INNER JOIN CUSTOMER C WITH (NOLOCK) ON N.Profileid=C.USERNAME
                    where C.DLRPushHookAPI<>'' AND N.profileid<>'MIM2201194' and N.DELIVERY_DATETIME = '" + tim2 + "' ";
                    try
                    {
                        dbmain.ExecuteNonQuery(sql);
                    }
                    catch (Exception e4) { LogError("DELIVERYcallback", e4.Message + e4.StackTrace); }

                    sql = @" Insert into deliverycallback_FCP (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno) select Profileid,
                'id:' + N.MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,N.SENTDATETIME,112),6) + REPLACE(cONVERT(VARCHAR,N.SENTDATETIME,108),':','') + 
                ' done date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + ' stat:DELIVRD err:000 text:' + substring(N.MsgText,1,20) AS DLVRTEXT, 
                N.MSGID, GETDATE(), GETDATE(), 'Delivered','000', SENDERID, TOMOBILE FROM NOTSUBMITTED N where profileid='MIM2201194' and N.DELIVERY_DATETIME = '" + tim2 + "' ";
                    try
                    {
                        dbmain.ExecuteNonQuery(sql);
                    }
                    catch (Exception e4) { LogError("DELIVERYcallback", e4.Message + e4.StackTrace); }

                }
                #region < Commented >
                /*
                DataTable dt = database.GetDataTable("select * from NOTSUBMITTED where SENTDATETIME < DATEATDD(MINUTE,GETDATE()) AND DELIVERY_DATETIME IS NULL");
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        string errcd = Convert.ToString(database.GetScalarValue("Select err_code from DLVERRDATA wehre tomobile='" + dr["TOMOBILE"].ToString() + "'"));
                        string txt = "";
                        string stat = "";
                        if (errcd == "003")
                        {
                            txt = "id:" + dr["MSGID"].ToString() + " sub:001 dlvrd:000 submit date:" + Convert.ToDateTime(dr["SENTDATETIME"]).ToString("yyMMddHHmmss") + " done date:" + DateTime.Now.ToString("yyMMddHHmmss") + " stat:DND§check§failed err:" + errcd + " text:";
                            stat = "None";
                        }
                        else
                        {
                            txt = "id:" + dr["MSGID"].ToString() + " sub:001 dlvrd:000 submit date:" + Convert.ToDateTime(dr["SENTDATETIME"]).ToString("yyMMddHHmmss") + " done date:" + DateTime.Now.ToString("yyMMddHHmmss") + " stat:UNDELIV err:" + errcd + " text:" + (dr["MsgText"].ToString().Length > 20 ? dr["MsgText"].ToString().Substring(0, 20) : dr["MsgText"].ToString());
                            stat = "Undeliverable";
                        }
                        sql = "Insert into Delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) values ('" + txt + "','" + dr["MSGID"].ToString() + "', getdate(),'" + stat + "','" + errcd + "',getdate())";
                        database.ExecuteNonQuery(sql);
                    }

                }
                */
                #endregion
            }
            ProcessFailDelivery();
        }

        private void ProcessFailDelivery()
        {
            string sql = "";
            string tim = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string tim2 = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            Int64 c = Convert.ToInt64(dbmain.GetScalarValue("select count(*) from FailSUBMITTED where SENTDATETIME < DATEADD(MINUTE,-6,GETDATE()) AND DELIVERY_DATETIME IS NULL"));
            if (c > 0)
            {
                dbmain.ExecuteNonQuery("update FailSUBMITTED set DELIVERY_DATETIME ='" + tim2 + "' where SENTDATETIME < DATEADD(MINUTE,-6,GETDATE()) AND DELIVERY_DATETIME IS NULL");

                sql = @"select 'id:' + N.MSGID + ' sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,N.SENTDATETIME,112),6) + REPLACE(cONVERT(VARCHAR,N.SENTDATETIME,108),':','') + 
                       ' done date:' + RIGHT(CONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + ' stat:UNDELIV err:#ERRCD text:' AS DLVRTEXT,
                        N.MSGID, GETDATE() as DLVRTIME, GETDATE() as donedate,'Undeliverable' as DLVRSTATUS,dbo.GetRandomErrorCode() as err_code
                        into #t3 FROM FailSUBMITTED N where N.DELIVERY_DATETIME = '" + tim2 + "' ; " +
                        "UPDATE #t3 SET DLVRTEXT =REPLACE(DLVRTEXT, '#ERRCD' , err_code) ; " +
                        "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,INSERTDATE) " +
                        "select DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,getdate() from #t3 ; drop table #t3 ; ";

                dbmain.ExecuteNonQuery(sql);

                //if (ProcessDLRCallBack == 1)
                //{
                //    sql = @" Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno) select Profileid,
                //'id:' + N.MSGID + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,N.SENTDATETIME,112),6) + REPLACE(cONVERT(VARCHAR,N.SENTDATETIME,108),':','') + 
                //' done date:' + RIGHT(cONVERT(VARCHAR,GETDATE(),112),6) + REPLACE(cONVERT(VARCHAR,GETDATE(),108),':','') + ' stat:DELIVRD err:000 text:' + substring(N.MsgText,1,20) AS DLVRTEXT, 
                //N.MSGID, GETDATE(), GETDATE(), 'Delivered','000', SENDERID, TOMOBILE FROM NOTSUBMITTED N where N.DELIVERY_DATETIME = '" + tim2 + "' ";
                //    try
                //    {
                //        dbmain.ExecuteNonQuery(sql);
                //    }
                //    catch (Exception e4) { LogError("DELIVERYcallback", e4.Message + e4.StackTrace); }
                //}
            }
        }
        #endregion

        private void timerDashBoard_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                ServiceForReject = "";
                try
                {
                    ServiceForReject = Convert.ToString(ConfigurationManager.AppSettings["ServiceForReject"]);
                }
                catch (Exception ex)
                {
                    LogError("DASHBOARDTimer_" + ex.StackTrace, ex.Message);
                }

                if (ServiceForReject.Trim().ToUpper() == "Y")
                {
                    ProcessDashBoard_WhitePanel();
                }
                else
                {
                    ProcessDashBoard();
                }
            }
            catch (Exception ex)
            {
                LogError("DASHBOARDTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void ProcessDashBoard()
        {
            string sql = @"
/* sms USER */ 
UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT c.username, count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from customer c with(nolock)
LEFT join MSGSUBMITTED s with(nolock) on convert(DATE,s.SENTDATETIME) = convert(DATE,getdate()) and c.username = s.profileid 
left join delivery d with(nolock) on s.msgid = d.msgid
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* SMS ADMIN */

UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT u.USERNAME,u.DLTNO,x.submitted,x.delivered,x.failed,x.unknown FROM CUSTOMER u WITH (NOLOCK) LEFT join 
(
select c.DLTNO, count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from customer c with(nolock)
LEFT join MSGSUBMITTED s with(nolock) on convert(DATE,s.SENTDATETIME) = convert(DATE,getdate()) and c.username = s.profileid 
left join delivery d with(nolock) on s.msgid = d.msgid
where 1=1
group by c.DLTNO
) x on u.dltno=x.dltno 
where u.usertype='ADMIN'
)Z ON B.userid=Z.username;


/* SMS BD */

UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT u.USERNAME,u.EmpCode,x.submitted,x.delivered,x.failed,x.unknown FROM CUSTOMER u WITH (NOLOCK) LEFT join 
(
select c.EmpCode, count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from customer c with(nolock)
LEFT join MSGSUBMITTED s with(nolock) on convert(DATE,s.SENTDATETIME) = convert(DATE,getdate()) and c.username = s.profileid 
left join delivery d with(nolock) on s.msgid = d.msgid
where 1=1
group by c.EmpCode
) x on u.EmpCode=x.EmpCode 
where u.usertype='BD'
)Z ON B.userid=Z.username;




/* SMS SYSADMIN */

UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
select count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from MSGSUBMITTED s with(nolock) left join delivery d with(nolock) on s.msgid = d.msgid
where convert(DATE,s.SENTDATETIME) = convert(DATE,getdate())
)Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ;

/* URL USER */

UPDATE DASHBOARD SET links=Z.urls, CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(select c.username,count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* URL ADMIN */

UPDATE DASHBOARD SET links=Z.urls, 
CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT M.USERNAME,X.URLS,X.CLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
( select c.DLTNO,count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
group by c.DLTNO
) X ON X.DLTNO=M.DLTNO
WHERE M.USERTYPE = 'ADMIN'
) Z ON B.userid=Z.username ;


/* URL BD */

UPDATE DASHBOARD SET links=Z.urls, 
CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT M.USERNAME,X.URLS,X.CLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
(select c.EmpCode,count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
group by c.EmpCode
) X ON X.EmpCode=M.EmpCode
WHERE M.USERTYPE = 'BD'
) Z ON B.userid=Z.username ;



/* URL SMSADMIN */

UPDATE DASHBOARD SET links=Z.urls, 
CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
( select count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
) Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ;

/* CLK USER */

UPDATE DASHBOARD SET SMSCLICKS=Z.CN
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT C.USERNAME, COUNT(MS.ID) AS CN FROM CUSTOMER C with(nolock)
LEFT join short_urls U with(nolock) on c.username = U.userid 
LEFT JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID AND convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* CLK ADMIN */

/*
UPDATE DASHBOARD SET SMSCLICKS=Z.SMSCLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT M.USERNAME,X.SMSCLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
(
SELECT C.DLTNO, COUNT(MS.ID) AS SMSCLICKED FROM CUSTOMER C with(nolock)
LEFT join short_urls U with(nolock) on c.username = U.userid 
LEFT JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID AND convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
where 1=1
group by c.DLTNO
)X ON X.DLTNO=M.DLTNO
WHERE M.USERTYPE = 'ADMIN'
) Z ON B.userid=Z.username ;
*/

/* CLK BD */

UPDATE DASHBOARD SET SMSCLICKS=Z.SMSCLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT M.USERNAME,X.SMSCLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
(SELECT C.EmpCode, COUNT(MS.ID) AS SMSCLICKED FROM CUSTOMER C with(nolock)
LEFT join short_urls U with(nolock) on c.username = U.userid 
LEFT JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID AND convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
where 1=1
group by c.EmpCode
)X ON X.EmpCode=M.EmpCode
WHERE M.USERTYPE = 'BD'
) Z ON B.userid=Z.username ;


/* CLK SYSADMIN */

UPDATE DASHBOARD SET SMSCLICKS=Z.CN
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT COUNT(MS.ID) AS CN FROM CUSTOMER C with(nolock)
inner join short_urls U with(nolock) on c.username = U.userid
INNER JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID
where convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
) Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ";
            database.ExecuteNonQuery(sql);
        }

        private void ProcessDashBoard_WhitePanel()
        {
            string sql = @"
/* sms USER */ 
UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed,smsrejected=Z.rejected
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT c.username, count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL and er.errcode IS NULL then 1 else 0 end) as failed,
sum(case when er.errcode IS NOT NULL then 1 else 0 end) as rejected,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from customer c with(nolock)
LEFT join MSGSUBMITTED s with(nolock) on convert(varchar,s.INSERTDATE,102) = convert(varchar,getdate(),102) and c.username = s.profileid 
left join delivery d with(nolock) on s.msgid = d.msgid
left join tblRejectedCode er with(nolock) on er.errcode=d.err_code
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* SMS ADMIN */

UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed,SMSREJECTED=Z.rejected
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT u.USERNAME,u.DLTNO,x.submitted,x.delivered,x.failed,x.rejected,x.unknown FROM CUSTOMER u WITH (NOLOCK) LEFT join 
(
select c.DLTNO, count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL and er.errcode IS NULL then 1 else 0 end) as failed,
sum(case when er.errcode IS NOT NULL then 1 else 0 end) as rejected,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from customer c with(nolock)
LEFT join MSGSUBMITTED s with(nolock) on convert(varchar,s.INSERTDATE,102) = convert(varchar,getdate(),102) and c.username = s.profileid 
left join delivery d with(nolock) on s.msgid = d.msgid
left join tblRejectedCode er with(nolock) on er.errcode=d.err_code
where 1=1
group by c.DLTNO
) x on u.dltno=x.dltno 
where u.usertype='ADMIN'
)Z ON B.userid=Z.username;

/* SMS SYSADMIN */

UPDATE DASHBOARD SET updtime= GETDATE(), smssubmitted=Z.submitted, 
SMSDELIVERED=Z.delivered, SMSFAILED= Z.failed
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
select count(s.id) submitted, 
sum(case when isnull(d.dlvrstatus,'')='Delivered' then 1 else 0 end) as delivered,
sum(case when isnull(d.dlvrstatus,'')<>'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown
from MSGSUBMITTED s with(nolock) left join delivery d with(nolock) on s.msgid = d.msgid
where convert(varchar,s.INSERTDATE,102) = convert(varchar,getdate(),102)
)Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ;

/* URL USER */

UPDATE DASHBOARD SET links=Z.urls, CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(select c.username,count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* URL ADMIN */

UPDATE DASHBOARD SET links=Z.urls, 
CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT M.USERNAME,X.URLS,X.CLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
( select c.DLTNO,count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
group by c.DLTNO
) X ON X.DLTNO=M.DLTNO
WHERE M.USERTYPE = 'ADMIN'
) Z ON B.userid=Z.username ;

/* URL SMSADMIN */

UPDATE DASHBOARD SET links=Z.urls, 
CLICKS=Z.CLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
( select count(distinct U.id) urls, 
sum(case when isnull(S.SHORTURL_ID, 0) = 0 then 0 else 1 end) as CLICKED
from customer c with(nolock)
left join short_urls U with(nolock) on c.username = U.userid and convert(varchar(6),U.ADDED,112) =convert(varchar(6),getdate(),112)
left join stats S with(nolock) on U.ID = S.SHORTURL_ID 
where 1=1
) Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ;

/* CLK USER */

UPDATE DASHBOARD SET SMSCLICKS=Z.CN
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT C.USERNAME, COUNT(MS.ID) AS CN FROM CUSTOMER C with(nolock)
LEFT join short_urls U with(nolock) on c.username = U.userid 
LEFT JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID AND convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
where 1=1
and c.USERTYPE = 'USER'
group by c.username
) Z ON B.userid=Z.username ;

/* CLK ADMIN */

UPDATE DASHBOARD SET SMSCLICKS=Z.SMSCLICKED
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(
SELECT M.USERNAME,X.SMSCLICKED FROM CUSTOMER M WITH (NOLOCK) LEFT join 
(
SELECT C.DLTNO, COUNT(MS.ID) AS SMSCLICKED FROM CUSTOMER C with(nolock)
LEFT join short_urls U with(nolock) on c.username = U.userid 
LEFT JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID AND convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
where 1=1
group by c.DLTNO
)X ON X.DLTNO=M.DLTNO
WHERE M.USERTYPE = 'ADMIN'
) Z ON B.userid=Z.username ;

/* CLK SYSADMIN */

UPDATE DASHBOARD SET SMSCLICKS=Z.CN
from DASHBOARD B WITH (NOLOCK) INNER JOIN 
(SELECT COUNT(MS.ID) AS CN FROM CUSTOMER C with(nolock)
inner join short_urls U with(nolock) on c.username = U.userid
INNER JOIN MOBSTATS MS with(nolock) ON U.ID=MS.SHORTURL_ID
where convert(varchar(6),ms.click_date,112)=convert(varchar(6),getdate(),112)
) Z ON 1=1 WHERE B.userid=(SELECT USERNAME FROM CUSTOMER WHERE USERTYPE='SYSADMIN') ";
            database.ExecuteNonQuery(sql);
        }

        public void sendmail(string id, string UserId, string Password, string Host, string Port)
        {
            string result = "";
            StringBuilder body = new StringBuilder();
            List<string> CC = new List<string>();

            DataSet ds = database.GetSMSSummary(id);

            DataTable dt = ds.Tables[0]; ;
            DataTable dtnotification = ds.Tables[1];

            string ToEmailId = dtnotification.Rows[0]["Email"].ToString().Trim();

            string Subject = "Big Bazar SMS Report for " + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            body.Append("<b>Today's SMS Report (" + DateTime.Now.ToString("dd-MM-yyy") + ") </b> <br>");
            body.Append("<b>Submitted : </b>" + dt.Rows[0]["submitted"].ToString().Trim() + "<br>");
            body.Append("<b>Delivered : </b>" + dt.Rows[0]["delivered"].ToString().Trim() + "<br>");
            body.Append("<b>Failed : </b>" + dt.Rows[0]["failed"].ToString().Trim() + "<br>");
            body.Append("<b>Today's Spent : </b> Rs. " + dt.Rows[0]["amount"].ToString().Trim() + "/-<br>");
            body.Append("<br>");
            body.Append("<br>");
            body.Append("This is a system generated email<br>");
            body.Append("Report generated at : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));

            if (dtnotification.Rows.Count > 0)
            {
                if (dtnotification.Rows[0]["CC1"] != null && dtnotification.Rows[0]["CC1"].ToString() != "")
                {
                    CC.Add(dtnotification.Rows[0]["CC1"].ToString().Trim());
                }
                if (dtnotification.Rows[0]["CC2"] != null && dtnotification.Rows[0]["CC2"].ToString() != "")
                {
                    CC.Add(dtnotification.Rows[0]["CC2"].ToString().Trim());
                }
                if (dtnotification.Rows[0]["CC3"] != null && dtnotification.Rows[0]["CC3"].ToString() != "")
                {
                    CC.Add(dtnotification.Rows[0]["CC3"].ToString().Trim());
                }
                if (dtnotification.Rows[0]["CC4"] != null && dtnotification.Rows[0]["CC4"].ToString() != "")
                {
                    CC.Add(dtnotification.Rows[0]["CC4"].ToString().Trim());
                }
                if (dtnotification.Rows[0]["CC5"] != null && dtnotification.Rows[0]["CC5"].ToString() != "")
                {
                    CC.Add(dtnotification.Rows[0]["CC5"].ToString().Trim());
                }
            }
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //Credentials = new System.Net.NetworkCredential(UserId, Password),
                    Timeout = 30000
                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(UserId, Password);
                MailMessage message = new MailMessage(UserId, ToEmailId, Subject, body.ToString());
                message.IsBodyHtml = true;
                for (int i = 0; i < CC.Count; i++)
                {
                    message.CC.Add(CC[i]);
                }
                smtp.EnableSsl = true;
                smtp.Send(message);
                result = "Sent Successfully..!!";
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
            }
        }

        public void DWReportsendmail(string Email, string UserId, string Password, string Host, string Port, string[] CCEmail, DateTime FromDate, string Subject, StringBuilder body)
        {
            string result = "";

            List<string> CC = new List<string>();

            string ToEmailId = Email;


            if (CCEmail.Length > 0)
            {
                for (int i = 0; i < CCEmail.Length; i++)
                {
                    CC.Add(CCEmail[i]);
                }
            }
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //Credentials = new System.Net.NetworkCredential(UserId, Password),
                    Timeout = 30000
                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(UserId, Password);
                MailMessage message = new MailMessage(UserId, ToEmailId, Subject, body.ToString());
                message.IsBodyHtml = true;
                //Attachment item = new Attachment(Attachment);
                //message.Attachments.Add(item);
                for (int i = 0; i < CC.Count; i++)
                {
                    message.CC.Add(CC[i]);
                }
                smtp.EnableSsl = true;
                smtp.Send(message);
                LogError("email Sent ", "to:" + Email + " sub " + Subject + " BODY " + body.ToString());
                result = "Sent Successfully..!!";

            }
            catch (Exception ex)
            {
                LogError("--Error sending email-- ", ex.Message);
                result = "Error sending email.!!! " + ex.Message;
            }

        }
        private void LogError(string title, string msg)
        {
            try
            {
                if (1 == 1)
                {
                    //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + @"\Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter m_stramWriter = new StreamWriter(fs);
                    m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                    m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                    m_stramWriter.Flush();
                    m_stramWriter.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        // rabi 08/04/2021
        private void timerwhishes_Tick(object source, ElapsedEventArgs e)
        {
            //sendwish();
        }

        private void sendwish_OLD()
        {
            string smstext = "";
            string[] arr = null;
            string Sent_Daily = Convert.ToString(database.GetScalarValue("select convert(varchar(5), WishDailySentOn, 108) [DailySentOn] from settings"));
            if (Sent_Daily == DateTime.Now.ToString("HH:mm"))
            {
                DataTable dt = database.GetDataTable("select MobileNo,Name,WishDate,Wishes,templateID,senderid,msgtext,WishName from wish w join TemplateID t on t.templateName=w.wishes where CONVERT(date,WishDate)=CONVERT(date,getdate())");
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        smstext = dt.Rows[i]["msgtext"].ToString();
                        smstext = smstext.Replace("{#var#}", ";");
                        arr = smstext.Split(';');
                        for (int j = 0; j < arr.Length; j++)
                        {
                            if (j == 0)
                            {
                                smstext = arr[j].Trim() + " " + dt.Rows[i]["Name"].ToString();
                            }
                            else if (j == 1)
                            {
                                smstext = smstext.Trim() + " " + arr[j].Trim() + " " + dt.Rows[i]["WishName"].ToString();
                            }
                            else
                            {
                                smstext = smstext.Trim() + " " + arr[j].Trim();
                            }
                        }
                        DataTable dtacc = database.GetDataTable("select username,PWD,peid from CUSTOMER where SENDERID='" + dt.Rows[i]["SenderId"].ToString() + "'");

                        if (dtacc != null && dtacc.Rows.Count > 0)
                        {
                            SendSMSthroughAPI(dtacc.Rows[0]["username"].ToString(), dt.Rows[i]["SenderId"].ToString(), dtacc.Rows[0]["peid"].ToString(), dtacc.Rows[0]["PWD"].ToString(), dt.Rows[i]["MobileNo"].ToString(), smstext, dt.Rows[i]["templateid"].ToString());
                        }
                    }
                }
            }
        }

        private void sendwish()
        {
            string smstext = "";
            string[] arr = null;

            DataTable setting = database.GetDukeDataTable("select username,senderid,peid,templateID,smstext,WishDailySentOn,pwd from [setting]");
            string Sent_Daily = setting.Rows[0]["WishDailySentOn"].ToString();
            if (Sent_Daily == DateTime.Now.ToString("HH:mm"))
            {
                DataTable dt = database.GetDukeDataTable("select MobileNo,Name,Wishname from [wishdata]  where CONVERT(date,WishDate)=CONVERT(date,getdate())");

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        smstext = setting.Rows[0]["smstext"].ToString();
                        smstext = smstext.Replace("{#var#}", ";");
                        arr = smstext.Split(';');
                        for (int j = 0; j < arr.Length; j++)
                        {
                            if (j == 0)
                            {
                                smstext = arr[j].Trim() + " " + dt.Rows[i]["Name"].ToString();
                            }
                            else if (j == 1)
                            {
                                smstext = smstext.Trim() + " " + arr[j].Trim() + " " + dt.Rows[i]["WishName"].ToString();
                            }
                            else
                            {
                                smstext = smstext.Trim() + " " + arr[j].Trim();
                            }
                        }
                        SendSMSthroughAPI(setting.Rows[0]["username"].ToString(), setting.Rows[0]["senderid"].ToString(), setting.Rows[0]["peid"].ToString(), setting.Rows[0]["pwd"].ToString(), dt.Rows[i]["MobileNo"].ToString().ToString(), smstext, setting.Rows[0]["templateID"].ToString());
                    }
                }
            }
        }

        private void Log(string title, string msg)
        {
            try
            {
                if (1 == 1)
                {
                    //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + @"\Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter m_stramWriter = new StreamWriter(fs);
                    m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                    m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + title + "_" + msg);
                    m_stramWriter.Flush();
                    m_stramWriter.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }


        // ----   TEST SMS rachit 21-10-2021

        private void timerPROCESSTest_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bProcessTest)
                {
                    bProcessTest = true;
                    //TestSMS();
                    bProcessTest = false;
                }

            }
            catch (Exception ex)
            {
                bProcessTest = false;
                LogError("PROCESSTimerSMSTest_" + ex.StackTrace, ex.Message);
            }
        }
        public void TestSMS()
        {
            Util ob = new Util();
            DataTable dtTemplateErrors = ob.GetTemplateErrors();
            string picked_time = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

            DataTable dtRequest = ob.GetRequestForSMSTest(picked_time);
            if (dtRequest.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRequest.Rows)
                {
                    string sql = "";
                    string colnm = "";
                    string fileid = Convert.ToString(dr["fileid"]);
                    string user = Convert.ToString(dr["tblName"]);
                    string userId = Convert.ToString(dr["PROFILEID"]);
                    string s = Convert.ToString(dr["SENDERID"]);
                    int noofsms = Convert.ToInt16(dr["noofsms"]);
                    string msg = Convert.ToString(dr["MSGTEXT"]);
                    bool isMobTrackUrl = Convert.ToBoolean(dr["isMobTrkURL"]);
                    bool ucs2 = Convert.ToString(dr["datacode"]).ToUpper() == "DEFAULT" ? false : true;
                    double rate = Convert.ToDouble(dr["rate"]);
                    string shortURLId = Convert.ToString(dr["urlid"]);
                    string shortURL = Convert.ToString(dr["shorturl"]);
                    string domain = Convert.ToString(dr["domain"]);
                    bool ISschedule = Convert.ToBoolean(dr["ISschedule"]);

                    DataTable dtStatus = TestSmsbeforeSend(userId, Convert.ToString(dr["templateid"]), msg, s, Convert.ToString(dr["PEID"]));
                    DataRow[] drS = dtStatus.Select("errCode='0'");
                    if (drS.Length > 0)
                    {
                        if (ISschedule)
                            ProcessScheduleTest(userId, fileid, false);
                        else
                        {
                            //colnm = Convert.ToString(database.GetScalarValue("if exists (select * from sys.tables where name='" + user + @"') select column_name From information_schema.columns where table_name = '" + user + @"' and ordinal_position = 1 else select '' "));

                            double Bper = ob.GetBlockSMSper(userId, "B");
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(dr["CountRecord"]) * Bper);
                                sql = sql + " select top " + cnt20 + " * into #t101 from " + user + " where tomobile is not null and tomobile not in (" + ob.getWhiteListNo(Convert.ToString(dr["PROFILEID"])) + ") ORDER BY NEWID() " +
                                    " delete d from " + user + " d inner join #t101 t on d.tomobile = t.tomobile  ;  " +
                                    " alter table #t101 add msgid varchar (100) ; ";

                                for (int x = 0; x < noofsms; x++)
                                {
                                    // string smsTex = ob.GetSMSText_1(msg, x + 1, noofsms, ucs2);

                                    if (ucs2)
                                    {
                                        sql = sql + " update #t101 set msgid=newid() " +
                                      @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                                    select '1' as id,'vcon',smppaccountid,'" + userId + "',msgtext , TOMOBILE,'" + s + "'," +
                                      "GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1','" + rate + "' from #t101 ; " +
                                       " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                                       " select '1' as id,'vcon',smppaccountid,'" + userId + "'," +
                                      " case when len(msgtext)<=70 then MSGTEXT else " +
                                        "  when '" + x + 1 + "' = 1 then (case when len(msgtext) > 70 then SUBSTRING(msgtext,1, 70) else SUBSTRING(msgtext,71, 64) end) " +
                                        "  when '" + x + 1 + "' = 2 then (case when len(msgtext) > 134 then SUBSTRING(msgtext,71, 64) else SUBSTRING(msgtext,71, 67) end) " +
                                        "  when '" + x + 1 + "' = 3 then (case when len(msgtext) > 201 then SUBSTRING(msgtext,135, 67) else SUBSTRING(msgtext,201, 67) end) " +
                                        "  when '" + x + 1 + "' = 4 then (case when len(msgtext) > 268 then SUBSTRING(msgtext,202, 67) else SUBSTRING(msgtext,268, 67) end) " +
                                        "  when '" + x + 1 + "' = 5 then (case when len(msgtext) > 335 then SUBSTRING(msgtext,269, 67) else SUBSTRING(msgtext,269, 67) end) " +
                                        "  when '" + x + 1 + "' = 6 then (case when len(msgtext) > 402 then SUBSTRING(msgtext,336, 67) else SUBSTRING(msgtext,336, 67) end) " +
                                        "  when '" + x + 1 + "' = 7 then (case when len(msgtext) > 469 then SUBSTRING(msgtext,403, 67) else SUBSTRING(msgtext,403, 67) end) " +
                                        "  when '" + x + 1 + "' = 8 then (case when len(msgtext) > 536 then SUBSTRING(msgtext,470, 67) else SUBSTRING(msgtext,470, 67) end) " +
                                        "  when '" + x + 1 + "' = 9 then (case when len(msgtext) > 603 then SUBSTRING(msgtext,537, 67) else SUBSTRING(msgtext,537, 67) end) " +
                                        "  when '" + x + 1 + "' = 10 then (case when len(msgtext) > 670 then SUBSTRING(msgtext,604, 67) else SUBSTRING(msgtext,604, 67) end) " +
                                        "  when '" + x + 1 + "' = 11 then (case when len(msgtext) > 737 then SUBSTRING(msgtext,671, 67) else SUBSTRING(msgtext,671, 67) end) " +
                                        "  when '" + x + 1 + "' = 12 then (case when len(msgtext) > 804 then SUBSTRING(msgtext,738, 67) else SUBSTRING(msgtext,738, 67) end) " +
                                        "  when '" + x + 1 + "' = 13 then (case when len(msgtext) > 871 then SUBSTRING(msgtext,805, 67) else SUBSTRING(msgtext,805, 67) end) " +
                                        "  when '" + x + 1 + "' = 14 then (case when len(msgtext) > 938 then SUBSTRING(msgtext,872, 67) else SUBSTRING(msgtext,872, 67) end) " +
                                        "  when '" + x + 1 + "' = 15 then (case when len(msgtext) > 1005 then SUBSTRING(msgtext,939, 67) else SUBSTRING(msgtext,939, 67) end) " +
                                        " end end " +
                                       ",TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1'," +
                                       " msgtext ,'" + rate + "' from #t101 ; ";
                                    }
                                    else
                                    {
                                        sql = sql + " update #t101 set msgid=newid() " +
                                       @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                                    select '1' as id,'vcon',smppaccountid,'" + userId + "',msgtext, TOMOBILE,'" + s + "'," +
                                       "GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1','" + rate + "' from #t101 ; " +
                                        " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                                        " select '1' as id,'vcon',smppaccountid,'" + userId + "'," +
                                        " case when len(msgtext)<=160 then MSGTEXT else " +
                                        " case when '" + x + 1 + "' = 1 then (case when len(msgtext) > 160 then SUBSTRING(msgtext,1, 160) else SUBSTRING(msgtext,161, 160) end) else" +
                                        " case when '" + x + 1 + "' = 2 then (case when len(msgtext) > 306 then SUBSTRING(msgtext,161, 146) else SUBSTRING(msgtext,161, 160) end) else" +
                                        " case when '" + x + 1 + "' = 3 then (case when len(msgtext) > 459 then SUBSTRING(msgtext,307, 153) else SUBSTRING(msgtext,307, 160) end) else" +
                                        " case when '" + x + 1 + "' = 4 then (case when len(msgtext) > 612 then SUBSTRING(msgtext,460, 153) else SUBSTRING(msgtext,460, 160) end) else" +
                                        " case when '" + x + 1 + "' = 5 then (case when len(msgtext) > 765 then SUBSTRING(msgtext,613, 153) else SUBSTRING(msgtext,613, 160) end) else" +
                                        " case when '" + x + 1 + "' = 6 then (case when len(msgtext) > 918 then SUBSTRING(msgtext,766, 153) else SUBSTRING(msgtext,766, 160) end) else" +
                                        " case when '" + x + 1 + "' = 7 then (case when len(msgtext) > 1071 then SUBSTRING(msgtext,919, 153) else SUBSTRING(msgtext,919, 160) end) else" +
                                        " case when '" + x + 1 + "' = 8 then (case when len(msgtext) > 1224 then SUBSTRING(msgtext,1072, 153) else SUBSTRING(msgtext,1072, 160) end) " +
                                        " end end end end end end end end end " +
                                        ",TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1'," +
                                        " msgtext ,'" + rate + "' from #t101 ; ";
                                    }

                                }
                                if (isMobTrackUrl)
                                {
                                    sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid) select '" + shortURLId + "',TOMOBILE,getdate(),shortsegment,FILEID from #t101 ";
                                }

                            }
                            #region < Commented >
                            //  double Fper = ob.GetBlockSMSper(userId, "F");
                            //  if (Fper != 0)
                            //  {
                            //      Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(dr["CountRecord"]) * Fper);
                            //      sql = sql + " select top " + cnt20 + " * into #t10 from " + user + " where [" + colnm + "] is not null and [" + colnm + "] not in (" + ob.getWhiteListNo(Convert.ToString(dr["PROFILEID"])) + ") ORDER BY NEWID() " +
                            //          " delete d from " + user + " d inner join #t10 t on d.[" + colnm + "] = t.[" + colnm + "]  ;  " +
                            //          " alter table #t10 add msgid varchar (100) ; ";

                            //      for (int x = 0; x < noofsms; x++)
                            //      {
                            //          string smsTex = GetSMSText(msg.Length, x + 1, noofsms, ucs2);
                            //          sql = sql + " update #t10 set msgid=newid() " +
                            //              @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate)
                            //select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1','" + rate + "' from #t10 ; " +
                            //               " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                            //               " select @id as id,'vcon',smppaccountid,'" + userId + "',N'" + smsTex + "',[" + colnm + "] as TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),@id as fileid,'1'," +
                            //               " N'" + msg + "','" + rate + "' from #t10 ; ";
                            //      }
                            //  }
                            #endregion

                            sql = sql + "insert into MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,MSGID2) select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,MSGID2 from " + user;

                            if (isMobTrackUrl)
                            {
                                sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid) select '" + shortURLId + "',TOMOBILE,getdate(),shortsegment,FILEID from " + user;
                            }
                        }
                    }
                    else
                    {
                        bool processed = false;
                        if (dtStatus.Rows.Count > 0)
                        {
                            if (ISschedule)
                                ProcessScheduleTest(userId, fileid, true);
                            else
                            {
                                foreach (DataRow dsr in dtStatus.Rows)
                                {
                                    DataRow[] dr1 = dtTemplateErrors.Select("err_code='" + dsr["errCode"].ToString() + "'");
                                    if (dr1.Length > 0)
                                    {
                                        processed = true;
                                        //to fail all sms with this errCode

                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string smsTex = ob.GetSMSText_1(msg, x + 1, noofsms, ucs2);

                                            if (ucs2)
                                            {
                                                sql = sql + " update " + user + " set msgid2=newid() ; " +
                                               " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                                                  " select '1' as id,'vcon',smppaccountid,'" + userId + "'," +
                                                       " case when len(msgtext)<=70 then MSGTEXT else " +
                                                       "  when '" + x + 1 + "' = 1 then (case when len(msgtext) > 70 then SUBSTRING(msgtext,1, 70) else SUBSTRING(msgtext,71, 64) end) " +
                                                       "  when '" + x + 1 + "' = 2 then (case when len(msgtext) > 134 then SUBSTRING(msgtext,71, 64) else SUBSTRING(msgtext,71, 67) end) " +
                                                       "  when '" + x + 1 + "' = 3 then (case when len(msgtext) > 201 then SUBSTRING(msgtext,135, 67) else SUBSTRING(msgtext,201, 67) end) " +
                                                       "  when '" + x + 1 + "' = 4 then (case when len(msgtext) > 268 then SUBSTRING(msgtext,202, 67) else SUBSTRING(msgtext,268, 67) end) " +
                                                       "  when '" + x + 1 + "' = 5 then (case when len(msgtext) > 335 then SUBSTRING(msgtext,269, 67) else SUBSTRING(msgtext,269, 67) end) " +
                                                       "  when '" + x + 1 + "' = 6 then (case when len(msgtext) > 402 then SUBSTRING(msgtext,336, 67) else SUBSTRING(msgtext,336, 67) end) " +
                                                       "  when '" + x + 1 + "' = 7 then (case when len(msgtext) > 469 then SUBSTRING(msgtext,403, 67) else SUBSTRING(msgtext,403, 67) end) " +
                                                       "  when '" + x + 1 + "' = 8 then (case when len(msgtext) > 536 then SUBSTRING(msgtext,470, 67) else SUBSTRING(msgtext,470, 67) end) " +
                                                       "  when '" + x + 1 + "' = 9 then (case when len(msgtext) > 603 then SUBSTRING(msgtext,537, 67) else SUBSTRING(msgtext,537, 67) end) " +
                                                       "  when '" + x + 1 + "' = 10 then (case when len(msgtext) > 670 then SUBSTRING(msgtext,604, 67) else SUBSTRING(msgtext,604, 67) end) " +
                                                       "  when '" + x + 1 + "' = 11 then (case when len(msgtext) > 737 then SUBSTRING(msgtext,671, 67) else SUBSTRING(msgtext,671, 67) end) " +
                                                       "  when '" + x + 1 + "' = 12 then (case when len(msgtext) > 804 then SUBSTRING(msgtext,738, 67) else SUBSTRING(msgtext,738, 67) end) " +
                                                       "  when '" + x + 1 + "' = 13 then (case when len(msgtext) > 871 then SUBSTRING(msgtext,805, 67) else SUBSTRING(msgtext,805, 67) end) " +
                                                       "  when '" + x + 1 + "' = 14 then (case when len(msgtext) > 938 then SUBSTRING(msgtext,872, 67) else SUBSTRING(msgtext,872, 67) end) " +
                                                       "  when '" + x + 1 + "' = 15 then (case when len(msgtext) > 1005 then SUBSTRING(msgtext,939, 67) else SUBSTRING(msgtext,939, 67) end) " +
                                                       " end end " +
                                                       ",TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1'," +
                                                       " msgtext ,'" + rate + "' from " + user + " ; " +
                                               " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                               @" select 'id:' + MSGID2 + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                                        ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + dsr["errCode"].ToString() + " text:' AS DLVRTEXT, MSGID2, GETDATE(), 'Undeliverable','" + dsr["errCode"].ToString() + @"',getdate()
                                        FROM " + user + " ; ";
                                            }
                                            else
                                            {
                                                sql = sql + " update " + user + " set msgid2=newid() ; " +
                                                " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate) " +
                                                " select '1' as id,'vcon',smppaccountid,'" + userId + "'," +
                                                    " case when len(msgtext)<=160 then MSGTEXT else " +
                                                    " case when '" + x + 1 + "' = 1 then (case when len(msgtext) > 160 then SUBSTRING(msgtext,1, 160) else SUBSTRING(msgtext,161, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 2 then (case when len(msgtext) > 306 then SUBSTRING(msgtext,161, 146) else SUBSTRING(msgtext,161, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 3 then (case when len(msgtext) > 459 then SUBSTRING(msgtext,307, 153) else SUBSTRING(msgtext,307, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 4 then (case when len(msgtext) > 612 then SUBSTRING(msgtext,460, 153) else SUBSTRING(msgtext,460, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 5 then (case when len(msgtext) > 765 then SUBSTRING(msgtext,613, 153) else SUBSTRING(msgtext,613, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 6 then (case when len(msgtext) > 918 then SUBSTRING(msgtext,766, 153) else SUBSTRING(msgtext,766, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 7 then (case when len(msgtext) > 1071 then SUBSTRING(msgtext,919, 153) else SUBSTRING(msgtext,919, 160) end) else" +
                                                    " case when '" + x + 1 + "' = 8 then (case when len(msgtext) > 1224 then SUBSTRING(msgtext,1072, 153) else SUBSTRING(msgtext,1072, 160) end) " +
                                                    " end end end end end end end end end " +
                                                    ",TOMOBILE,'" + s + "',GETDATE(),GETDATE(),msgid,getdate(),'" + fileid + "' as fileid,'1'," +
                                                    " msgtext ,'" + rate + "' from " + user + " ; " +
                                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                                @" select 'id:' + MSGID2 + ' sub:001 dlvrd:001 submit date:' + RIGHT(CONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + 
                                        ' done date:' + RIGHT(CONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + dsr["errCode"].ToString() + " text:' AS DLVRTEXT, MSGID2, GETDATE(), 'Undeliverable','" + dsr["errCode"].ToString() + @"',getdate()
                                        FROM " + user + " ; ";
                                            }
                                        }
                                    }
                                    if (processed) break;
                                }
                            }
                        }
                        if (!processed)
                        {
                            sql = sql + "insert into MSGTRAN (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,MSGID2) select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,MSGID2 from " + user;
                            if (isMobTrackUrl)
                            {
                                sql = sql + " insert into mobtrackurl (urlid, mobile, sentdate, segment,fileid) select '" + shortURLId + "',TOMOBILE,getdate(),shortsegment,FILEID from " + user;
                            }
                        }
                    }
                    database.ExecuteNonQuery(sql);

                    database.ExecuteNonQuery("drop table " + user + "");
                }

                database.ExecuteNonQuery("UPDATE MSGTRANRequest SET status=1,statusUpdDate=GETDATE() where ISNULL(status,0)=0 and statusUpdDate is null and reqDate < '" + picked_time + "' ");
            }

        }


        public DataTable TestSmsbeforeSend(string profileid, string templateId, string msg, string senderId, string peid)
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("errCode");
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
                        string[] arr = _response.Split(',');
                        foreach (string mid in arr)
                        {
                            liMessageId.Add(mid.Split(':')[2].Trim().Replace(@"\", "").Replace('"', ' ').Trim());
                        }
                    }

                    System.Threading.Thread.Sleep(10000);//wait 10 second
                    foreach (string msgId in liMessageId)
                    {
                        int errorCode = Convert.ToInt32(database.GetScalarValue(string.Format("select err_code from delivery with (nolock) where msgid ='{0}'", msgId)));
                        DataRow dr = dt.NewRow();
                        dr["errCode"] = errorCode.ToString();
                        dt.Rows.Add(dr);
                        //if (errorCode == 0) { return true; }
                    }
                }
            }

            return dt;
        }

        public string SendSMSthroughAPI(string profileid, string templateId, string msg, string senderId, string peid)
        {
            msg = HttpUtility.UrlEncode(msg);

            // msg = msg.Replace("%", "%25").Replace("#", "%23").Replace("&", "%26").Replace("+", "%2B");

            DataTable dt = database.GetDataTable("Select * from SMSCheckSetting with (nolock) where profileid='" + profileid + "'");
            if (dt.Rows.Count > 0)
            {
                string _userid = "MIM2000002";

                string sql = "IF NOT EXISTS (SELECT * FROM SENDERIDMAST with (nolock) WHERE senderid = '" + senderId + "' AND userid='" + _userid + "')" +
                  " INSERT INTO SENDERIDMAST(userid,senderid) values('" + _userid + "','" + senderId + "')";

                database.ExecuteNonQuery(sql);
                string pwd = Convert.ToString(database.GetScalarValue("select pwd from CUSTOMER with (nolock) where username='" + _userid + "'"));
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
                }
                catch (Exception EX)
                {
                    throw EX;
                }
            }
            return "";
        }


        private void ProcessScheduleTest(string UserID, string fileID, bool ISFail)
        {
            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from MsgSchedule with (nolock) where profileid='" + UserID + "' and fileid='" + fileID + "'"));
            Util ob = new Util();
            double Bper = ob.GetBlockSMSper(UserID, "B");
            if (Bper != 0)
            {
                string sql = "";
                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);
                if (ISFail)
                    sql = " update MsgSchedule set blockfail=1 where profileid='" + UserID + "' and isnull(blacklist,0)=0 and fileid='" + fileID + "' ";
                else
                    sql = " update top (" + cnt20 + ") MsgSchedule set blockno=1 where profileid='" + UserID + "' and isnull(blacklist,0)=0 and fileid='" + fileID + "' and Tomobile not in (" + ob.getWhiteListNo(UserID) + ") ";

                database.ExecuteNonQuery(sql);
            }
        }

        private void timersmscampaign_Tick(object source, ElapsedEventArgs e)
        {
            string SMS_CAMP = Convert.ToString(ConfigurationManager.AppSettings["SMS_CAMP"]);
            if (SMS_CAMP == DateTime.Now.ToString("HH:mm"))
            {
                smscampreport();
            }

        }
        private void smscampreport()
        {
            new Util().SMSCampService();

        }

        #region SMSLINKTOWABA_EMAIL_REMINDER

        private void timerSMSLinkToWABA_Tick(object sender, ElapsedEventArgs e)
        {
            timerSMSLinkToWABAEmailReminder();
        }

        private void timerSMSLinkToWABAEmailReminder()
        {
            try
            {
                if (SMSLINKTOWABA_TIME == DateTime.Now.ToString("HH:mm"))
                {
                    DataTable dtReminder = database.GetDataTable(@"select * from ReminderDetails with(nolock) where Status='P'");
                    if (dtReminder != null && dtReminder.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReminder.Rows.Count; i++)
                        {
                            DataTable dtChk = database.GetDataTable(@"select * from customer where username='" + dtReminder.Rows[i]["UserId"].ToString() + "' and (WabaProfileId is null OR WabaProfileId = '') and (WabaPwd is null OR WabaPwd = '') and Active=1");
                            if (dtChk != null && dtChk.Rows.Count > 0)
                            {
                                List<string> ccs = new List<string>();
                                DataTable dt = database.GetDataTable("SELECT * FROM settingmast WITH(NOLOCK)");
                                DateTime ReminderDate = Convert.ToDateTime(dtReminder.Rows[i]["LastReminderDate"]).AddDays(Convert.ToInt32(dt.Rows[0]["ReminderDaysSMSLinkToWABA"])).Date;
                                DateTime currentDate = DateTime.Now.Date;
                                if (ReminderDate == currentDate)
                                {
                                    string cc1 = Convert.ToString(dt.Rows[0]["cc1"]);
                                    string cc2 = Convert.ToString(dt.Rows[0]["cc2"]);
                                    string cc3 = Convert.ToString(dt.Rows[0]["cc3"]);
                                    string cc4 = Convert.ToString(dt.Rows[0]["cc4"]);
                                    string cc5 = Convert.ToString(dt.Rows[0]["cc5"]);

                                    if (!string.IsNullOrEmpty(cc1.Trim()))
                                    {
                                        ccs.Add(cc1);
                                    }
                                    if (!string.IsNullOrEmpty(cc2.Trim()))
                                    {
                                        ccs.Add(cc2);
                                    }
                                    if (!string.IsNullOrEmpty(cc3.Trim()))
                                    {
                                        ccs.Add(cc3);
                                    }
                                    if (!string.IsNullOrEmpty(cc4.Trim()))
                                    {
                                        ccs.Add(cc4);
                                    }
                                    if (!string.IsNullOrEmpty(cc5.Trim()))
                                    {
                                        ccs.Add(cc5);
                                    }
                                    string CCEmail = string.Join(",", ccs);
                                    List<string> CC = CCEmail.Split(',').Select(t => Convert.ToString(t)).ToList();

                                    string EmailSubject = Convert.ToString(dt.Rows[0]["WabaLinkEmailSubject"]);
                                    string EmailBody = Convert.ToString(dt.Rows[0]["WabaLinkEmailBody"]);
                                    DateTime dateTime = Convert.ToDateTime(dtReminder.Rows[i]["EmailDate"]);
                                    EmailBody = EmailBody.Replace("#USERID#", Convert.ToString(dtReminder.Rows[i]["UserId"])).Replace("#CLIENTNAME#", Convert.ToString(dtChk.Rows[0]["FULLNAME"])).Replace("#REQUESTDATE#", dateTime.ToString("dd/MM/yyyy")).Replace("#REQUESTTIME#", dateTime.ToString("hh:mm tt"));
                                    string result = new Util().SendEmailComman(Convert.ToString(dt.Rows[0]["ToEmailId"]), EmailSubject, EmailBody, Convert.ToString(dt.Rows[0]["userid"]), Convert.ToString(dt.Rows[0]["password"]), Convert.ToString(dt.Rows[0]["Host"]), CC);
                                    string sql = @"update ReminderDetails set LastReminderDate=getdate() where UserId='" + dtReminder.Rows[i]["UserId"].ToString() + "' AND STATUS='P'";
                                    database.ExecuteNonQuery(sql);
                                }
                            }
                            else
                            {
                                string sql = @"update ReminderDetails set Status='A' where UserId='" + dtReminder.Rows[i]["UserId"].ToString() + "'";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("PROCES_SMSLinkToWABAemailScheduler_" + ex.StackTrace, ex.Message);
            }
        }


        private void timerClickCountProcess_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!bSMSClickCountProcess)
                {
                    bSMSClickCountProcess = true;
                    LogError("--SMSClickCountProcess-- ", "processStarted");
                    timerSMSClickCountProcesses();
                    bSMSClickCountProcess = false;
                }
            }
            catch (Exception ex)
            {
                bSMSClickCountProcess = false;
                LogError("--CatchException-- ", ex.Message.ToString());
            }
        }

        private void timerSMSClickCountProcesses()
        {
            try
            {
                DataTable dt = database.GetDataTable(@"select * from ClickCountProcess with(nolock) where Active=1");
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string UserId = Convert.ToString(dt.Rows[i]["ProfileId"]);
                        int processfilafterminute = Convert.ToInt32(dt.Rows[i]["ProcessFileAfterMin"]);
                        int processfilafterminute15 = Convert.ToInt32(dt.Rows[i]["ProcessFileAfterMin"]) + 15;

                        DataTable dtrec = database.GetDataTable(@"select * from fileprocess with(nolock) where noofrecord > 10 and shorturlid <> 0 
                           and ((isschedule=0 and GETDATE() >= DATEADD(minute," + processfilafterminute + ",processedtime) and GETDATE() < DATEADD(minute," + processfilafterminute15 + ",processedtime)) " +
                           "OR (isschedule=1 and GETDATE() >= DATEADD(minute," + processfilafterminute + ",scheduletime) and GETDATE() <  DATEADD(minute," + processfilafterminute15 + @",scheduletime)))
                            and profileid='" + UserId + "' and ClickCountProcessed IS NULL");

                        //DataTable dtrec = database.GetDataTable(@"select * from fileprocess with(nolock) where profileid='" + UserId + "' and id='76845'");
                        if (dtrec != null && dtrec.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtrec.Rows.Count; i++)
                            {
                                string sql = @"INSERT INTO ClickCountUpdated (ProfileId,FileId,ProcessDateTinme) VALUES('" + Convert.ToString(dtrec.Rows[i]["profileid"]) + "','" + Convert.ToString(dtrec.Rows[i]["id"]) + "',GETDATE())";
                                database.ExecuteNonQuery(sql);
                                string fileprocessId = Convert.ToString(database.GetScalarValue(@"select ID from smsfileupload with(nolock) where fileprocessid='" + Convert.ToString(dtrec.Rows[i]["id"]) + "'"));
                                int Totalrec = Convert.ToInt32(database.GetScalarValue(@"select count(*) from MSGSUBMITTED with(nolock) where fileid='" + fileprocessId + "' and NSend=0"));
                                int ClickCount = Convert.ToInt32(database.GetScalarValue(@"select count(*) from smsfileupload f with(nolock)
                                   inner join MSGSUBMITTED s with(nolock) on f.userid = s.profileid and f.id=s.fileid 
                                   inner join delivery d with(nolock) on s.msgid = d.msgid 
                                   inner join short_urls sh with(nolock) on sh.userid = f.userid and f.shortURLId = sh.id
                                   inner join mobtrackurl m with(nolock) on sh.ID = m.urlid and m.mobile=s.TOMOBILE and m.fileid=s.fileid
                                   inner join mobstats ms on sh.ID = ms.SHORTURL_ID and ms.urlid=m.id
                                   where s.fileid='" + fileprocessId + "' and s.NSEND=0"));

                                double clickCountPer = Convert.ToDouble(dt.Rows[i]["ClickCountPer"]); //4.28350
                                double percent = (clickCountPer / 100) * Totalrec; //42.835
                                int gotrecords = Math.Abs((int)Math.Round(percent) - ClickCount); //42-30
                                if (gotrecords > 0)
                                {
                                    string SqlQry = @"CREATE TABLE #temp1 (slno INT,click_date DATETIME,ip VARCHAR(100),shortUrl_id VARCHAR(100),Browser VARCHAR(100),Platform VARCHAR(100),
                                    IsMobileDevice VARCHAR(100),MobileDeviceManufacturer VARCHAR(100),MobileDeviceModel VARCHAR(100),urlid VARCHAR(100));
                                    INSERT INTO #temp1 (slno, click_date, shortUrl_id, urlid) 
                                    SELECT 
                                        ROW_NUMBER() OVER (ORDER BY s.sentdatetime) AS slno,DATEADD(MINUTE, 18, s.SENTDATETIME) AS click_date,sh.id AS shortUrl_id,m.id AS urlid FROM 
                                        smsfileupload f WITH (NOLOCK)
                                        INNER JOIN MSGSUBMITTED s WITH (NOLOCK) ON f.userid = s.profileid AND f.id = s.fileid 
                                        INNER JOIN delivery d WITH (NOLOCK) ON s.msgid = d.msgid 
                                        INNER JOIN short_urls sh WITH (NOLOCK) ON sh.userid = f.userid AND f.shortURLId = sh.id
                                        INNER JOIN mobtrackurl m WITH (NOLOCK) ON sh.ID = m.urlid AND m.mobile = s.TOMOBILE AND m.fileid = s.fileid
                                        LEFT JOIN mobstats ms with (nolock) ON sh.ID = ms.SHORTURL_ID AND ms.urlid = m.id
                                    WHERE s.fileid = '" + fileprocessId + @"' AND s.NSEND = 0 AND ms.urlid IS NULL; 
                                    UPDATE #temp1 
                                    SET ip = A.ip,Browser = A.Browser,Platform = A.Platform,IsMobileDevice = A.IsMobileDevice,MobileDeviceManufacturer = A.MobileDeviceManufacturer,MobileDeviceModel = A.MobileDeviceModel
                                    FROM 
                                        (SELECT TOP " + Totalrec + @"
                                    	    ROW_NUMBER() OVER (ORDER BY newid()) AS slno,ip,Browser,Platform,IsMobileDevice,MobileDeviceManufacturer,MobileDeviceModel FROM Mobstats with(nolock) where shortUrl_id <> '" + dtrec.Rows[j]["shorturlid"] + @"' 
                                        ) A
                                    JOIN 
                                        #temp1 t ON t.slno = A.slno;
                                    	INSERT INTO mobstats (click_date,ip,referer,shortUrl_id,Browser,Platform, IsMobileDevice,MobileDeviceManufacturer,MobileDeviceModel,urlid,User_AgentAutoClick)
                                    	select click_date,ip,'',shortUrl_id,Browser,Platform, IsMobileDevice,MobileDeviceManufacturer,MobileDeviceModel,urlid,'' from #temp1";
                                    try
                                    {
                                        LogError("SqlQry ", SqlQry.ToString());
                                        database.ExecuteNonQuery(SqlQry);
                                        string sqlupd = "UPDATE fileprocess SET ClickCountProcessed = getdate() WHERE id = '" + Convert.ToString(dtrec.Rows[i]["id"]) + "'";
                                        database.ExecuteNonQuery(sqlupd);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError("CatchException ", ex.Message.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("PROCES_ClickInMobStats_" + ex.StackTrace, ex.Message);
            }
        }
    }
    #endregion

    #region <<<<----   HYUNDAI FAILEDS SMS TTS OBD PROCESS  not in use   ---- >>>>
    private void ProcessHyundaiFailedSMSforTTSobd()
    {
        string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("Select LastHyundaiFailedSMSProcessedTime from settings")).ToString("yyyy-MM-dd HH:mm:ss.fff");
        string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        database.ExecuteNonQuery("Update settings set LastHyundaiFailedSMSProcessedTime='" + currTime + "'");

        string APIKEY = Convert.ToString(ConfigurationManager.AppSettings["APIKEY_OBD_INFOBIP"]);
        string URL = Convert.ToString(ConfigurationManager.AppSettings["URL_OBD_INFOBIP"]);

        //fetch Failed Records of SMS
        string sql = @"select top 2
m.PROFILEID,m.SENDERID,/*m.*/ '919870333974' as TOMOBILE,m.smstext,m.MSGID
from msgsubmitted m with(nolock) inner join delivery d with(nolock) on m.msgid = d.msgid inner join customer c with(nolock) on m.profileid = c.username
inner join HYUNDAIttsErrCode e on d.err_code = e.errcode
where d.INSERTDATE >= '" + LastProcessedTime + "' and d.INSERTDATE < '" + currTime + @"'
and c.dltno in ('DEALERSERVICE', 'HYUNDAISALES', 'HYUNDAISERVICE', 'DEALERSALES', 'HASC')
group by m.PROFILEID,m.SENDERID,m.TOMOBILE,m.smstext,m.MSGID";
        DataTable dt = database.GetDataTable(sql);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                System.Threading.Thread t = new System.Threading.Thread(() =>
                {
                    SendOBDHyundai(dr, APIKEY, URL);
                });
                t.Start();
            }
        }
    }
    public void SendOBDHyundai(DataRow dr, string APIKEY, string URL)
    {

        //Insert into OBDsent table 
        string sql = "Insert into OBDSent (profileid,tomobile,obdtext,msgid) " +
            "values('" + dr["PROFILEID"].ToString() + "','" + dr["TOMOBILE"].ToString() + "',N'" + dr["smstext"].ToString() + "','" + dr["MSGID"].ToString() + "') ";

        database.ExecuteNonQuery(sql);

        obdresponse res = obd_infobip_api_for_HMIL(dr["TOMOBILE"].ToString(), dr["smstext"].ToString(), APIKEY, URL);

        try
        {
            OBDMessage ms = new OBDMessage();
            ms = res.messages[0];

            OBDStatus st = new OBDStatus();
            st = ms.status;

            sql = @"insert into [obdapisubmitted](bulkId,messageId,tomobile,obdtext,groupId,groupName,Id,name,description,profileid,fileid,cnt)
                select '" + res.bulkId + "','" + ms.messageId + "','" + dr["TOMOBILE"].ToString() + "',N'" + dr["smstext"].ToString() + "'," + st.groupId + ",'" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + dr["profileid"].ToString() + "','0' ,'1' ";

            database.ExecuteNonQuery(sql);
        }
        catch (Exception)
        {
            throw;
        }

    }
    public obdresponse obd_infobip_api_for_HMIL(string mob, string obdtext, string authkey, string url)
    {
        //System.Threading.Thread.Sleep(50);
        var client = new RestClient(url);
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        //request.AddHeader("x-api-key", apikey);
        request.AddHeader("Authorization", authkey);
        request.AddHeader("Content-Type", "application/json");

        var body = @"{
                        ""text"": """ + obdtext.Replace("\r\n", "") + @""",
                        ""language"": ""en-in"",
                        ""voice"": {
                            ""name"": ""Raveena"",
                            ""gender"": ""female""
                        },
                        ""from"": ""912271897425"",
                        ""to"": """ + mob + @"""
                        }";

        request.AddParameter("application/json", body, ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        obdresponse res = JsonConvert.DeserializeObject<obdresponse>(response.Content);
        return res;
    }
    #endregion <<<<----   HYUNDAI FAILEDS SMS TTS OBD PROCESS  not in use   ---- >>>>


    #region Exe Auto Start After Close EXE ADD By Vikas 28-11-2023
    private void StartProcess(string processPath)
    {
        try
        {
            //Process.Start(processPath);
            LogError("starting process: ", DateTime.Now.ToString());
        }
        catch (Exception ex)
        {
            LogError("Error starting process: " + ex.StackTrace, ex.Message);
        }
    }
    #endregion
    //classes --->

    public class To
    {
        public string phoneNumber { get; set; }
    }
    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class TemplateRemainder
    {
        public string username { set; get; }
        public string template { set; get; }
        public string templateid { set; get; }
    }

    public class Message
    {
        public To to { get; set; }
        public Status status { get; set; }
        public string messageId { get; set; }
    }

    public class WARoot
    {
        public List<Message> messages { get; set; }
    }

    public class OBDStatus
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class OBDMessage
    {
        public string to { get; set; }
        public OBDStatus status { get; set; }
        public string messageId { get; set; }
    }
    public class obdresp
    {
        public int order_id { get; set; }
        public string result { get; set; }
        public int status_code { get; set; }
    }
    public class obdresponse
    {
        public string bulkId { get; set; }
        public List<OBDMessage> messages { get; set; }
    }
}
}
