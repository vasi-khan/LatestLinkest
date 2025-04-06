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
 
namespace API2SMPP
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timerPROCESS_FirstCRY;
        private System.Timers.Timer timerPROCESSb4Q;
        private System.Timers.Timer timerPROCESSb4Q_Oth;
        private System.Timers.Timer timerPROCESSb4Q_UAE;
        private System.Timers.Timer timerPROCESSQ;
        private System.Timers.Timer timerPROCESSQ_OTP;
        private System.Timers.Timer timerDtTemp;  //5 minute timer
        private System.Timers.Timer timerPROCESSb4Q_1650;
        private System.Timers.Timer timerPROCESSb4Q_2201;
        private System.Timers.Timer timerPROCESSb4Q_HMISVR;
        private System.Timers.Timer timerPROCESSb4Q_HMIOTP;
        private System.Timers.Timer timerPROCESSb4Q_GSM;
        private System.Timers.Timer timerPROCESSQ_blkFail;
        private System.Timers.Timer timerPROCESSQ_blkDlr;
        private System.Timers.Timer timerPROCESSb4Q_API;
        private System.Timers.Timer timerPROCESS_DLRcallback;
        private System.Timers.Timer timerPROCESS_DLRcallback_pp;
        private System.Timers.Timer timerPROCESS_DLRcallback_FCP;
        private System.Timers.Timer timerPROCESS_SMPP_DtBind;  //Timer SMPP dt Bind
        bool IsprocessSMPP_DtBind = false;
        DataTable dtSMPPAccountTemplate = new DataTable();
        DataTable dtSMPPAccountUserID = new DataTable();
        DataTable dtSMPPAccountSender = new DataTable();
        DataTable dtSMPPSetting = new DataTable();

        DataTable dtDLRPushAPI = new DataTable();
        public bool dlrCallBackApplicable = false;
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();

        public string sms_systemid_TRANS = "";
        public string sms_rate = "0";
        public string sms_systemid_DNDSCRUB = "";
        public string sms_systemid_PROMO = "";
        public string sms_systemid_OTP = "";

        public string sms_systemid_UAE_TRANS = "";
        public string sms_systemid_UAE_PROMO = "";

        DataTable dt_FirstCryAccounts = new DataTable();

        public string bb = "MIM2101450";
        DataTable dtTemp = new DataTable();
        DataTable dtTemp1 = new DataTable();
        DataTable dtBlockNo = new DataTable();
        DataTable dtBlockPer = new DataTable();
        DataTable dtBlockPerSID = new DataTable();
        DataTable dtBlockPerTID = new DataTable();
        DataTable dtDND = new DataTable();
        DataTable dtPermamentErrNumListUAE = new DataTable();

        List<decimal> list; //= new List<string>();
        List<decimal> dndlist;
        List<decimal> permamentErrNumListUAE;

        bool processB4Q = false;
        bool processB4Q_Oth = false;

        bool processB4Q_API = false;

        bool processQ = false;
        bool processQ_OTP = false;
        bool processB4Q_GSM = false;
        bool processDLR_FCP = false;

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
        List<decimal> list_HMISVR;
        bool processB4Q_HMISVR = false;
        bool processB4Q_HMIOTP = false;

        public Form1()
        {
            InitializeComponent();
        }


        private void test()
        {
            DataTable dt = database.GetDataTable("Select * from SMppsetting");
            DataRow[] dr = dt.Select("For_OTP=1");
            if(dr.Length>0)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //test();

            try
            { database.homeCredit = Convert.ToString(ConfigurationManager.AppSettings["HOMECREDIT"]); }
            catch (Exception ex1) { }

            processSMPP_DtBind();
            getTemplate();
            processB4QUEUE_HMIOTP();
            processQUEUE_OTP();
            processQUEUE();

            //processB4QUEUE_API(); 
            //processB4QUEUE_HMISVR();
            //processB4QUEUE_1650();
            //processB4QUEUE_OTH();
            //processB4QUEUE();
            //processB4QUEUE_API();
            //processB4QUEUE();


            timerDtTemp = new System.Timers.Timer();
            this.timerDtTemp.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_DTTEMP"]);
            this.timerDtTemp.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDtTemp_Tick);
            timerDtTemp.Enabled = true;
            this.timerDtTemp.Start();

            #region < Queues >
            timerPROCESS_FirstCRY = new System.Timers.Timer();
            this.timerPROCESS_FirstCRY.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESS_FirstCRY.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_FirstCRY_Tick);
            if (database.homeCredit != "Y")
            {
                timerPROCESS_FirstCRY.Enabled = true;
                this.timerPROCESS_FirstCRY.Start();
            }
            timerPROCESSb4Q_Oth = new System.Timers.Timer();
            this.timerPROCESSb4Q_Oth.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q_OTH"]);
            this.timerPROCESSb4Q_Oth.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_OTH_Tick);
            timerPROCESSb4Q_Oth.Enabled = true;
            this.timerPROCESSb4Q_Oth.Start();

            //Add New timer for this table MSGQUEUEB4singleAPI
            timerPROCESSb4Q_API = new System.Timers.Timer();
            this.timerPROCESSb4Q_API.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q_API"]);
            this.timerPROCESSb4Q_API.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_API_Tick);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_API.Enabled = true;
                this.timerPROCESSb4Q_API.Start();
            }

            timerPROCESSb4Q_UAE = new System.Timers.Timer();
            this.timerPROCESSb4Q_UAE.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q_UAE"]);
            this.timerPROCESSb4Q_UAE.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_UAE_Tick);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_UAE.Enabled = true;
                this.timerPROCESSb4Q_UAE.Start();
            }

            timerPROCESSb4Q = new System.Timers.Timer();
            this.timerPROCESSb4Q.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q.Enabled = true;
                this.timerPROCESSb4Q.Start();
            }

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

            timerPROCESSb4Q_HMIOTP = new System.Timers.Timer();
            this.timerPROCESSb4Q_HMIOTP.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_B4Q"]);
            this.timerPROCESSb4Q_HMIOTP.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSb4Q_Tick_HMIOTP);
            if (database.homeCredit != "Y")
            {
                timerPROCESSb4Q_HMIOTP.Enabled = true;
                this.timerPROCESSb4Q_HMIOTP.Start();
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

            timerPROCESSQ_OTP = new System.Timers.Timer();
            this.timerPROCESSQ_OTP.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_Q"]);
            this.timerPROCESSQ_OTP.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_OTP_Tick);
            timerPROCESSQ_OTP.Enabled = true;
            this.timerPROCESSQ_OTP.Start();

            try
            {
                timerPROCESSQ_blkDlr = new System.Timers.Timer();
                this.timerPROCESSQ_blkDlr.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_BLK_Q"]);
                this.timerPROCESSQ_blkDlr.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_blkDlr_Tick);
                timerPROCESSQ_blkDlr.Enabled = true;
                this.timerPROCESSQ_blkDlr.Start();
            }
            catch (Exception) { }

            try
            {
                timerPROCESSQ_blkFail = new System.Timers.Timer();
                this.timerPROCESSQ_blkFail.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_BLK_Q"]);
                this.timerPROCESSQ_blkFail.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSQ_blkFail_Tick);
                timerPROCESSQ_blkFail.Enabled = true;
                this.timerPROCESSQ_blkFail.Start();
            }
            catch (Exception) { }


            try
            {
                try { if (Convert.ToString(ConfigurationManager.AppSettings["DLR_CALLBACK_APPLICABLE"]) == "Y") dlrCallBackApplicable = true; } catch (Exception e2) { }
                dtDLRPushAPI = getCallBackAPICustomers();

                timerPROCESS_DLRcallback = new System.Timers.Timer();
                this.timerPROCESS_DLRcallback.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_DLRCALLBACK"]);
                this.timerPROCESS_DLRcallback.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_DLRcallback_Tick);
                timerPROCESS_DLRcallback.Enabled = true;
                this.timerPROCESS_DLRcallback.Start();

                timerPROCESS_DLRcallback_pp = new System.Timers.Timer();
                this.timerPROCESS_DLRcallback_pp.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_DLRCALLBACK_PP"]);
                this.timerPROCESS_DLRcallback_pp.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_DLRcallback_pp_Tick);
                timerPROCESS_DLRcallback_pp.Enabled = true;
                this.timerPROCESS_DLRcallback_pp.Start();

                timerPROCESS_DLRcallback_FCP = new System.Timers.Timer();
                this.timerPROCESS_DLRcallback_FCP.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_DLRCALLBACK_FCP"]);
                this.timerPROCESS_DLRcallback_FCP.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_DLRcallback_FCP_Tick);
                timerPROCESS_DLRcallback_FCP.Enabled = true;
                this.timerPROCESS_DLRcallback_FCP.Start();

                timerPROCESS_SMPP_DtBind = new System.Timers.Timer();
                this.timerPROCESS_SMPP_DtBind.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_SMPP_DtBind"]);
                this.timerPROCESS_SMPP_DtBind.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_SMPP_DtBind_Tick);
                timerPROCESS_SMPP_DtBind.Enabled = true;
                this.timerPROCESS_SMPP_DtBind.Start();
            }
            catch (Exception) { }
        }



        #region <DLR Call Back Timer and Methods>
        public DataTable getCallBackAPICustomers()
        {
            DataTable dt = new DataTable();

            string sql = @"Select Username,DLRPushHookAPI,isnull(dlrHookApiHeader1,'')dlrHookApiHeader1,isnull(dlrHookApiHeader1val,'')dlrHookApiHeader1val,
            isnull(dlrHookApiHeader2,'')dlrHookApiHeader2,isnull(dlrHookApiHeader2val,'')dlrHookApiHeader2val,isnull(dlrHookApiHeader3,'')dlrHookApiHeader3,
            isnull(dlrHookApiHeader3val,'')dlrHookApiHeader3val from customer with (nolock) where active=1 and DLRPushHookAPI<>'' ";
            try
            {
                dt = database.GetDataTable(sql);
            }
            catch (Exception e) { }
            return dt;
        }

        private void timerPROCESS_DLRcallback_FCP_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                //if (!processDLR_FCP)
                //{
                //    processDLR_FCP = true;
                processDLRCallback_FCP();
                //    processDLR_FCP = false;
                //}
            }
            catch (Exception ex)
            {
                // processDLR_FCP = false;
                LogError("process_DLRcallback_FCP_" + ex.StackTrace, ex.Message);
            }
        }

        private void processDLRCallback_FCP()
        {
            string sql = "";
            try
            {
                //Process on each cycle
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback_FCP where picked_datetime is null "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(2000) DeliveryCallback_FCP set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from DeliveryCallback_FCP where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        processCallBack(Convert.ToString(dt.Rows[i]["DLVRSTATUS"]), Convert.ToDateTime(dt.Rows[i]["DLVRTIME"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToString(dt.Rows[i]["MSGID"]), Convert.ToString(dt.Rows[i]["err_code"]), Convert.ToString(dt.Rows[i]["PROFILEID"]), Convert.ToInt32(dt.Rows[i]["retry"]));
                }
                sql = sql + " delete from DeliveryCallback_FCP where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("FCP-", ex.Message); }
            }
        }

        private void timerPROCESS_DLRcallback_pp_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processDLRCallback_pp();
            }
            catch (Exception ex)
            {
                LogError("process_DLRcallback_pp_" + ex.StackTrace, ex.Message);
            }
        }
        private void processDLRCallback_pp()
        {
            string sql = "";
            try
            {
                //Process on each cycle
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback_pp where picked_datetime is null "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(1000) DeliveryCallback_pp set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from DeliveryCallback_pp where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        processCallBack(Convert.ToString(dt.Rows[i]["DLVRSTATUS"]), Convert.ToDateTime(dt.Rows[i]["DLVRTIME"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToString(dt.Rows[i]["MSGID"]), Convert.ToString(dt.Rows[i]["err_code"]), Convert.ToString(dt.Rows[i]["PROFILEID"]), Convert.ToInt32(dt.Rows[i]["retry"]));
                }
                sql = sql + " delete from DeliveryCallback_pp where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("p2", ex.Message); }
            }
        }

        private void timerPROCESS_DLRcallback_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processDLRCallback();
            }
            catch (Exception ex)
            {
                LogError("process_DLRcallback_" + ex.StackTrace, ex.Message);
            }
        }
        private void processDLRCallback()
        {
            string sql = "";
            try
            {
                //Process on each cycle
                Int32 ctn = Convert.ToInt32(database.GetScalarValue("Select count(*) from DeliveryCallback where picked_datetime is null "));
                if (ctn <= 0) return;

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(1000) DeliveryCallback set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from DeliveryCallback where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        processCallBack(Convert.ToString(dt.Rows[i]["DLVRSTATUS"]), Convert.ToDateTime(dt.Rows[i]["DLVRTIME"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToString(dt.Rows[i]["MSGID"]), Convert.ToString(dt.Rows[i]["err_code"]), Convert.ToString(dt.Rows[i]["PROFILEID"]), Convert.ToInt32(dt.Rows[i]["retry"]));
                }
                sql = sql + " delete from DeliveryCallback where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("p2", ex.Message); }
            }
        }

        public void processCallBack(string dlvStat, string d, string msgId, string errcd, string profileid, Int32 retry)
        {
            //process callback ------- >>>>
            if (dlrCallBackApplicable)
            {
                if (dtDLRPushAPI.Rows.Count > 0)
                {
                    DataRow[] dr = dtDLRPushAPI.Select("UserName = '" + profileid + "'");
                    string SubClientCode = "";
                    string url = Convert.ToString(dr[0]["DLRPushHookAPI"]);
                    //parkplus ,                                                                                                CWC 
                    if (profileid.ToUpper() == "MIM2300228" || profileid.ToUpper() == "MIM2300229" || profileid.ToUpper() == "MIM2002191" || url.ToLower().Contains("wzrkt") )
                    {
                        try { SubClientCode = Convert.ToString(database.GetScalarValue(@"select isnull(SubClientCode,'') SubClientCode from MSGSubClientCode with(nolock) where msgidClient='" + msgId + "'")); }
                        catch (Exception ee) { LogCBErr("Getting SubClientCode " + ee.Message); }
                    }

                    if (dr.Length > 0)
                    {
                        if (retry <= 6)
                            dlrCallBackAPI(dr, dlvStat, d, msgId, errcd, retry, SubClientCode, profileid);
                        else
                            LogAPIResp(msgId + " - after 7 retrials failed.");
                    }
                }
            }
        }

        //public void dlrCallBackAPIMethod(DataRow[] dr, string dlvStat, string d, string msgId, string errcd, Int32 retry, string SubClientCode = "", string profileid = "")
        //{
        //    dlrCallBackAPI(dr, dlvStat, d, msgId, errcd, retry, SubClientCode, profileid);
        //}

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
                if(url.ToLower().Contains("wzrkt"))
                {
                    string evnt = "";
                    string code = "";
                    string dscr = "";
                    msgId = (SubClientCode != "" ? SubClientCode : msgId);
                    DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                    // Calculate the Unix timestamp (number of seconds since the Unix Epoch)
                    long unixTimestamp = currentTime.ToUnixTimeSeconds();

                    if (dlvStat.ToLower() == "delivered" )
                    {
                        evnt = "delivered"; 
                        dscr = "Delivered to handset";
                    }
                    else
                    {
                        evnt = "fail";
                        dscr = "SMS Failed";
                        code = @", ""code"": """ + errcd + @"""";
                    }

                    body = @"[
                                {
                                    ""event"": """+ evnt + @""",
                                    ""data"": [
                                        {
                                            ""ts"": " + Convert.ToString(unixTimestamp) + @",
                                            ""meta"": """ + msgId + @""",
                                            ""description"": """ + dscr + @"""" + code + @"
                                        }
                                    ]
                                }
                            ]";
                }
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
                { LogError("insert into deliverycallbackcalled - ", ex.Message); }
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
                { LogError("insert into deliverycallbackcalled_ERR - ", ex.Message); }
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
                            if (x == 0) MSGid = drSMS["msgidClient"].ToString();
                            else MSGid = "S" + Convert.ToString(Guid.NewGuid());
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            string smsTex = "";
                            try
                            {
                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            }
                            catch (Exception ex) { smsTex = msg; }
                            sql = @" insert into notsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate,peid,templateid)
                                            VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" +
                                            drSMS["Profileid"].ToString() + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "') ; " +
                                 "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                 " VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                 + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1'," +
                                 " N'" + msg.Replace("'", "''") + "','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "' ) ; ";
                            database.ExecuteNonQuery(sql);
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
                            if (x == 0) MSGid = drSMS["msgidClient"].ToString();
                            else MSGid = "S" + Convert.ToString(Guid.NewGuid());
                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            string smsTex = "";
                            try
                            {
                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs2);
                            }
                            catch (Exception ex) { smsTex = msg; }
                            sql = @" insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smsrate,peid,templateid)
                                            VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                            + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "') ; " +
                                 "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,peid,templateid) " +
                                 " VALUES ('" + drSMS["id"].ToString() + "','" + drSMS["PROVIDER"].ToString() + "','" + drSMS["SMPPACCOUNTID"].ToString() + "','" + drSMS["Profileid"].ToString()
                                 + "',N'" + smsTex.Replace("'", "''") + "','"
                                 + drSMS["TOMOBILE"].ToString() + "','" + drSMS["SENDERID"].ToString() + "',GETDATE(),GETDATE(),'" + MSGid + "',getdate(),'1','1'," +
                                 " N'" + msg.Replace("'", "''") + "','0','" + drSMS["peid"].ToString() + "','" + drSMS["templateid"].ToString() + "' ) ; ";
                            database.ExecuteNonQuery(sql);
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
            try
            {
                if (database.homeCredit == "Y")
                    dtTemp = database.GetDataTable("SELECT TEMPLATEID,TEMPWORDS,TEMPWORDS2,msgtext,senderid FROM TEMPLATEID WHERE isnull(msgtext,'')<>'' order by len(TEMPWORDS2) desc");  //SENDERID='" + sender + "' and
                else
                {
                    dtTemp = database.GetDataTable("Select * from Templateid order by len(tempwords2) desc");
                    dtBlockPer = database.GetDataTable("select userid,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksms");
                    dtBlockPerSID = database.GetDataTable("select SID,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksmsSID");
                    dtBlockPerTID = database.GetDataTable("select TID,isnull(blockpercent,0) as blockpercent,isnull(failpercent,0) as failpercent from blocksmsTEMPLATEID");
                }
                dtBlockNo = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb + "' ");
                list = dtBlockNo.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dtBlockNo_1650 = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb_1650 + "' ");
                list_1650 = dtBlockNo_1650.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dtBlockNo_2201 = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='" + bb_2201 + "' ");
                list_2201 = dtBlockNo_2201.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dtBlockNo_HMISVR = database.GetDataTable("SELECT MOBILENO AS MOBILE FROM BLACKLISTNO WHERE USERID='' ");
                list_HMISVR = dtBlockNo_HMISVR.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dtDND = database.GetDataTable("SELECT TOMOBILE AS MOBILE FROM DNDNUMBERS2 where 1=0");
                dndlist = dtDND.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dtPermamentErrNumListUAE = database.GetDataTable("SELECT TOMOBILE AS MOBILE FROM USERMOBILE");
                permamentErrNumListUAE = dtPermamentErrNumListUAE.AsEnumerable().Select(r => r.Field<decimal>("MOBILE")).ToList();

                dt_FirstCryAccounts = database.GetDataTable("Select s.*,c.rate_normalsms rate from smppaccountuserid s with (nolock) inner join customer c with (nolock) on s.userid=c.username");
                if (dt_FirstCryAccounts.Rows.Count > 0)
                {
                    DataRow[] dr1 = dt_FirstCryAccounts.Select("Userid='MIM2201010'");
                    if (dr1.Length > 0)
                    {
                        sms_systemid_TRANS = dr1[0]["smppaccountid"].ToString();
                        sms_rate = dr1[0]["rate"].ToString();
                    }

                    DataRow[] dr2 = dt_FirstCryAccounts.Select("Userid='MIM2201011'");
                    if (dr2.Length > 0) sms_systemid_TRANS = dr2[0]["smppaccountid"].ToString();

                    DataRow[] dr3 = dt_FirstCryAccounts.Select("Userid='MIM2201009'");
                    if (dr3.Length > 0) sms_systemid_PROMO = dr3[0]["smppaccountid"].ToString();

                    DataRow[] dr4 = dt_FirstCryAccounts.Select("Userid='MIM2201194'");
                    if (dr4.Length > 0) sms_systemid_DNDSCRUB = dr4[0]["smppaccountid"].ToString();

                    DataRow[] dr5 = dt_FirstCryAccounts.Select("Userid='MIM2300064'");
                    if (dr5.Length > 0) sms_systemid_OTP = dr5[0]["smppaccountid"].ToString();

                    DataRow[] dr6 = dt_FirstCryAccounts.Select("Userid='MIM2300165'");
                    if (dr6.Length > 0) sms_systemid_UAE_TRANS = dr6[0]["smppaccountid"].ToString();

                    DataRow[] dr7 = dt_FirstCryAccounts.Select("Userid='MIM2300167'");
                    if (dr7.Length > 0) sms_systemid_UAE_PROMO = dr7[0]["smppaccountid"].ToString();

                }

                dtDLRPushAPI = getCallBackAPICustomers();
            }
            catch (Exception ex)
            {
                LogError("getTml", ex.Message + ex.StackTrace);
            }
        }

        private void timerPROCESS_FirstCRY_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processQUEUE_FirstCRY();
            }
            catch (Exception ex)
            {
                LogError("processQUEUE_FirstCRY_" + ex.StackTrace, ex.Message);
            }
        }

        public int GetMsgCount(string msg)
        {
            string q = msg.Trim();
            //if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
            int count_PIPE = q.Count(f => f == '|');
            int qlen = q.Length + count_PIPE;

            int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;

            int count1 = q.Count(f => f == '{'); qlen = qlen + count1;
            int count2 = q.Count(f => f == '}'); qlen = qlen + count2;
            int count3 = q.Count(f => f == '['); qlen = qlen + count3;
            int count4 = q.Count(f => f == ']'); qlen = qlen + count4;
            int count5 = q.Count(f => f == '^'); qlen = qlen + count5;
            int count6 = q.Count(f => f == '\\'); qlen = qlen + count6;

            int ln = qlen;
            int i = 0;
            if (ln >= 1) i = 1;
            if (ln > 160) i = 2;
            if (ln > 306) i = 3;
            if (ln > 459) i = 4;
            if (ln > 612) i = 5;
            if (ln > 765) i = 6;
            if (ln > 918) i = 7;
            if (ln > 1071) i = 8;
            if (ln > 1224) i = 9;
            if (ln > 1377) i = 10;
            if (ln > 1530) i = 11;
            if (ln > 1683) i = 12;
            if (ln > 1836) i = 13;
            if (ln > 1989) i = 14;
            if (ln > 2142) i = 15;
            if (ln > 2295) i = 16;
            if (ln > 2448) i = 17;
            if (ln > 2601) i = 18;
            if (ln > 2754) i = 19;
            if (ln > 2907) i = 20;
            if (ln > 3060) i = 21;
            if (ln > 3213) i = 22;
            if (ln > 3366) i = 23;
            if (ln > 3519) i = 24;
            if (ln > 3672) i = 25;
            if (ln > 3825) i = 26;
            if (ln > 3978) i = 27;
            if (ln > 4131) i = 28;

            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (qlen >= 1) i = 1;
                if (qlen > 70) i = 2;
                if (qlen > 133) i = 3;
                if (qlen > 196) i = 4;
                if (qlen > 259) i = 5;
                if (qlen > 322) i = 6;
                if (qlen > 385) i = 7;
                if (qlen > 448) i = 8;
                if (qlen > 536) i = 9;
                if (qlen > 511) i = 10;
                if (qlen > 574) i = 11;
                if (qlen > 637) i = 12;
                if (qlen > 700) i = 13;
                if (qlen > 763) i = 14;
                if (qlen > 826) i = 15;
                if (qlen > 889) i = 16;
                if (qlen > 952) i = 17;
                if (qlen > 1015) i = 18;
                if (qlen > 1078) i = 19;
                if (qlen > 1141) i = 20;
                if (qlen > 1204) i = 21;
                if (qlen > 1267) i = 22;
                if (qlen > 1330) i = 23;
                if (qlen > 1393) i = 24;
                if (qlen > 1456) i = 25;
                if (qlen > 1519) i = 26;
                if (qlen > 1582) i = 27;
                if (qlen > 1645) i = 28;
                if (qlen > 1708) i = 29;
                if (qlen > 1771) i = 30;
                if (qlen > 1834) i = 31;
                if (qlen > 1897) i = 32;
                if (qlen > 1960) i = 33;
                if (qlen > 2023) i = 34;
                if (qlen > 2086) i = 35;
                if (qlen > 2149) i = 36;
                if (qlen > 2212) i = 37;
                if (qlen > 2275) i = 38;
                if (qlen > 2338) i = 39;
                if (qlen > 2401) i = 40;
                if (qlen > 2464) i = 41;
                if (qlen > 2527) i = 42;
                if (qlen > 2590) i = 43;
                if (qlen > 2653) i = 44;
                if (qlen > 2716) i = 45;
                if (qlen > 2779) i = 46;
                if (qlen > 2842) i = 47;
                if (qlen > 2905) i = 48;
                if (qlen > 2968) i = 49;
                if (qlen > 3031) i = 50;
            }

            return i;
        }

        public bool isDND(string mobile)
        {
            int x = Convert.ToInt16(database.GetScalarValue("Select count(tomobile) from dndNumbers2 where tomobile=" + mobile));
            if (x > 0) return true; else return false;
        }

        private void processQUEUE_FirstCRY()
        {
            string q = "1";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUE_FC set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUE_FC where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string msg = Convert.ToString(dr["msgText"]);
                        int noofsms = GetMsgCount(msg.Trim());
                        bool ucs = false;
                        if (msg.Trim().Any(c => c > 126)) ucs = true;
                        string systemid = "";
                        string tblName = "MSGQUEUEB4single_Oth";

                        if (dr["ProfileId"].ToString() == "MIM2201009")
                        {
                            tblName = "MSGQUEUEB4single2201";
                            systemid = sms_systemid_PROMO;
                        }
                        else if (dr["ProfileId"].ToString() == "MIM2201194")
                        {
                            systemid = sms_systemid_DNDSCRUB;
                        }
                        else if (dr["ProfileId"].ToString() == "MIM2300064")
                        {
                            systemid = sms_systemid_OTP;
                        }
                        else if (dr["ProfileId"].ToString() == "MIM2300165")
                        {
                            tblName = "MSGQUEUEB4single_UAE";
                            systemid = sms_systemid_UAE_TRANS;
                        }
                        else if (dr["ProfileId"].ToString() == "MIM2300167")
                        {
                            tblName = "MSGQUEUEB4single_UAE";
                            systemid = sms_systemid_UAE_PROMO;
                        }
                        else
                            systemid = sms_systemid_TRANS;

                        bool dndno = false;
                        if (dr["ProfileId"].ToString() == "MIM2201194" || dr["ProfileId"].ToString() == "MIM2201009")
                        {
                            /* DND Check */
                            //decimal mobile = 0;
                            try
                            {
                                q = "2";
                                string mobn = Convert.ToString(dr["tomobile"]);
                                if (mobn.Length == 10) mobn = "91" + mobn; //.Substring(2, 10);
                                q = "3";
                                //mobile = Convert.ToDecimal(mobn);
                                //dndno = dndlist.Any(x => x == mobile);
                                dndno = isDND(mobn);
                                q = "4";
                            }
                            catch (Exception ex)
                            { }
                            if (dndno)
                            {
                                for (int x = 0; x < noofsms; x++)
                                {
                                    q = "5";
                                    string msgid = "";
                                    if (x == 0) msgid = dr["msgidClient"].ToString();
                                    else msgid = "S" + Convert.ToString(Guid.NewGuid());

                                    //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                    string smsTex = "";
                                    try
                                    {
                                        smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                    }
                                    catch (Exception ex) { smsTex = msg; }
                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,PEID,TEMPLATEID) " +
                                    " select '1' as id,'','" + systemid + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','9','DND','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                    try { q = "6"; database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message + " q" + q.ToString() + " sql - " + sql); }

                                    sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:103 text:' AS DLVRTEXT," +
                                    " '" + msgid + "', GETDATE(), 'Rejected','103',getdate() ; ";
                                    try { q = "7"; database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message + " q" + q.ToString()); }
                                    q = "8";
                                    database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + sms_rate + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                                    q = "9";
                                    try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "103"); }
                                    catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                }
                            }
                            /* DND Check */
                        }
                        if (!dndno)
                        {
                            q = "10";
                            sql = "insert into " + tblName + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                            " VALUES ('','" + systemid + "','" + dr["ProfileId"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["TOMOBILE"].ToString() + "','" + dr["SENDERID"].ToString() + "','1','" + Convert.ToString(dr["peid"]) + "','" + Convert.ToString(dr["templateid"]) + "','" + sms_rate + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                            q = "11";
                            database.ExecuteNonQuery(sql);
                            q = "12";
                        }
                    }
                }
                sql = @"delete from MSGQUEUE_FC where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("P1", ex.Message); }
            }
        }

        private void timerPROCESSb4Q_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q)
                {
                    processB4Q = true;
                    processB4QUEUE();
                    processB4Q = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q = false;
                LogError("processB4QUEUE_" + ex.StackTrace, ex.Message);
            }
        }

        private void processB4QUEUE()
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
                //sql = "Select * from MSGQUEUEB4single where picked_datetime='2023-01-01' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sender = dr["senderid"].ToString();
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,TEMPLATEID,PEID) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["templateid"].ToString() + "','" + dr["peid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;


                            // rabi for template DLT block 02/11/2022
                            string errcd_ = "5308";
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID.Contains("$"))
                                {
                                    string[] ar1 = templID.Split('$');
                                    templID = ar1[0];
                                    sender = ar1[1];
                                }

                                //if (templID != "")
                                //{
                                //    errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                //    if (errcd_ != "")
                                //    {
                                //        templID = "";
                                //    }
                                //}

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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }

                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "'"));
                                if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,TEMPLATEID,PEID) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + templID + "','" + dr["peid"].ToString() + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + sender + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }

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

        private void processB4QUEUE_OTH()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                //string pickdate = "2023-01-01 00:00:00.0000000"; 
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEB4single_OTH set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";

                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single_OTH where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sender = dr["senderid"].ToString();
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = Convert.ToString(dr["msgText"]);
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

                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" +
                                dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            string errcd_ = "5308";
                            // rabi for template DLT block 02/11/2022
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());

                                if (templID == "")
                                {
                                    bool reject = true;
                                    #region <HYUNDAI TEMPLATE >
                                    //periodic service 
                                    if (msg.ToUpper().Contains("DEAR CUSTOMER") && msg.ToUpper().Contains("PERIODIC") && msg.ToUpper().Contains("SERVICE OF YOUR CAR") && msg.ToUpper().Contains("PLEASE VISIT BELOW LINK") && msg.ToUpper().Contains("TO BOOK YOU"))
                                    {
                                        msg = msg.Replace("Service of your car", "Service of your Hyundai car");
                                        msg = msg.Replace("to book you", "to book your");
                                        msg = msg.Replace("For further details please contact ", "For further details please contact your Personal Mobility Advisor (PMA) ");
                                        msg = msg.Replace(" for appointment.", ". To get WhatsApp Notification, Kindly give missed call on 8367796869.");
                                        int p = msg.IndexOf("8367796869") + 11;
                                        try { msg = msg.Substring(0, p); } catch (Exception r) { }
                                        templID = "1107162824287096749";
                                        reject = false;
                                    }
                                    //else if (msg.ToUpper().Contains("DEAR CUSTOMER REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    else if (msg.ToUpper().Contains("REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    {   //vehicle ready
                                        msg = msg + " Hyundai";
                                        templID = "1107166919148623236";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WHICH IS HIGHER THAN OUR LIMIT FOR THIS MODEL") && msg.ToUpper().Contains("PLEASE LOOK INTO DETAILS OF THE SAME."))
                                    {   //bill amount higher than limit
                                        msg = "Dear Customer, " + msg + " Hyundai";
                                        templID = "1107166936627205655";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("PICK UP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //pickup schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936623571996";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR PICK-UP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for pickup
                                        msg = msg + " Hyundai";
                                        templID = "1107166936619220327";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DROP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //drop schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936613949914";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR DROP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for drop
                                        msg = msg + " Hyundai";
                                        templID = "1107166936593224635";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DEAR CUSTOMER, YOUR APPOINTMENT ON") && msg.ToUpper().Contains("WILL WAIT FOR YOU AT APPOINTMENT COUNTER") && (!msg.ToUpper().Contains("PAID SERVICE")))
                                    {   //appointment confirmation
                                        msg = msg + " Hyundai";
                                        templID = "1107167081856669406";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("REQUEST FOR ONLINE PAYMENT") && msg.ToUpper().Contains("KINDLY IGNORE IF PAYMENT ALREADY DONE"))
                                    {   //request for online payment
                                        msg = msg + " Hyundai";
                                        templID = "1107167081851720110";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("YOUR CUSTOMER WITH VEHICLE REG.NO") && msg.ToUpper().Contains("HAS GIVEN POOR RATING IN DSI"))
                                    {   //poor rating in DSI
                                        msg = msg + " Hyundai";
                                        templID = "1107167081854440883";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("OPT THE DRY WASH FOR YOUR CAR SERVICE AND HELP IN SAVING") && msg.ToUpper().Contains("BEYONDMOBILITY"))
                                    {   //Opt for dry wash
                                        msg = "Dear Customer, " + msg;
                                        templID = "1107168567279212434";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WITH DEALERSHIP TO CONFIRM YOUR CAR BOOKING IN SYSTEM"))
                                    {   //Opt for dry wash
                                        msg = msg + " -Hyundai";
                                        templID = "1107169650933028332";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    #endregion
                                    if (reject)
                                    {
                                        // process REJECTION ....//insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string msgid = "";
                                            if (x == 0) msgid = dr["msgidClient"].ToString();
                                            else msgid = "S" + Convert.ToString(Guid.NewGuid());

                                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            string smsTex = "";
                                            try
                                            {
                                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            }
                                            catch (Exception ex) { smsTex = msg; }
                                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                            database.ExecuteNonQuery(sql);
                                            sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";

                                            try { database.ExecuteNonQuery(sql); }
                                            catch (Exception E) { LogError_HMISVR("c1", E.Message); }
                                            try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                            catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }

                                        }
                                    }
                                }
                                else
                                {
                                    if (dr["senderid"].ToString().ToUpper() == "HMISVR")
                                    {
                                        string[] ar1 = templID.Split('$');
                                        templID = ar1[0];
                                        sender = ar1[1];
                                    }
                                }
                                if (templID != "")
                                {
                                    errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + sender + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "'"));
                                    if (errcd_ != "")
                                    {
                                        // process REJECTION ....
                                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED
                                        //for (int x = 0; x < noofsms; x++)
                                        //{
                                        //    string msgid = "";
                                        //    if (x == 0) msgid = dr["msgidClient"].ToString();
                                        //    else
                                        //        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                                        //        msgid = "S" + Convert.ToString(Guid.NewGuid());

                                        //    //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        //    string smsTex = "";
                                        //    try
                                        //    {
                                        //        smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        //    }
                                        //    catch (Exception ex) { smsTex = msg; }
                                        //    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        //    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        //    " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                        //    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        //    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        //    try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        //    sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        //    " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        //    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        //    " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        //    try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                        //    try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        //    catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                        //    templID = "";
                                        //}
                                    }
                                }

                                if (templID == "")
                                {

                                }
                                else
                                {
                                    if (templID.Contains("$"))
                                    {
                                        string[] ar1 = templID.Split('$');
                                        templID = ar1[0];
                                        //sender = ar1[1];
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + sender + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + dr["peid"].ToString() + "','" + templID + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + sender + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");

                    }
                }
                sql = @"delete from MSGQUEUEB4single_OTH where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("P1", ex.Message); }
            }
        }

        private void insertCallBack(string profileid, string txt, string msgid, string sid, string destno, string errcd)
        {
            string p = profileid.ToUpper();
            if (p == "MIM2300228" || p == "MIM2300229" || p == "MIM2301076" ||
                p == "MIM2201010" || p == "MIM2201011" || p == "MIM2201194" || p == "MIM2300064" || p == "MIM2300165" ||
                p == "MIM2300213" ||
                p == "MIM2300279" || p == "MIM2300280")
            {
                string sql = "Insert into DELIVERYcallback" + (profileid == "MIM2300228" ? "_PP" : (profileid == "MIM2201194" ? "_FCP" : "")) + " (PROFILEID,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno) values " +
                            "('" + p + "','" + msgid + "',getdate(),getdate(),'Rejected','" + errcd + "','" + sid + "','" + destno + "')";
                database.ExecuteNonQuery(sql);
            }
        }

        private void timerPROCESSb4Q_UAE_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                processB4QUEUE_UAE();
            }
            catch (Exception ex)
            {
                LogError("processB4QUEUE_UAE_" + ex.StackTrace, ex.Message);
            }
        }

        private void processB4QUEUE_UAE()
        {
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEB4single_UAE set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4single_UAE where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = Convert.ToString(dr["msgText"]);
                        bool ucs = (dr["datacode"].ToString() == "UCS2" ? true : false);
                        bool blacklist = false;
                        bool permamentErrNum = false;
                        decimal mobile = 0;
                        try
                        {
                            mobile = Convert.ToDecimal(dr["tomobile"]);
                            blacklist = list.Any(x => x == mobile);
                        }
                        catch (Exception ex) { }
                        if (blacklist)
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string msgid = "";
                                if (x == 0) msgid = dr["msgidClient"].ToString();
                                else
                                    msgid = "S" + Convert.ToString(Guid.NewGuid());

                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }

                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" +
                                dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message); }
                            }
                        }
                        if (!blacklist)
                        {
                            mobile = 0;
                            try
                            {
                                mobile = Convert.ToDecimal(dr["tomobile"]);
                                permamentErrNum = permamentErrNumListUAE.Any(x => x == mobile);
                            }
                            catch (Exception ex) { }
                            if (permamentErrNum)
                            {
                                for (int x = 0; x < noofsms; x++)
                                {
                                    string msgid = "";
                                    if (x == 0) msgid = dr["msgidClient"].ToString();
                                    else
                                        msgid = "S" + Convert.ToString(Guid.NewGuid());

                                    string smsTex = "";
                                    try
                                    {
                                        smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                    }
                                    catch (Exception ex) { smsTex = msg; }

                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','PERMA_ERR','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                    try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                    sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND,peid,templateid) " +
                                    " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" +
                                    dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                    try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                    sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:306 text:' AS DLVRTEXT," +
                                    " '" + msgid + "', GETDATE(), 'Undeliverable','306',getdate() ; ";
                                    try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B_perErr", ex.Message); }
                                }
                            }
                        }
                        if ((!blacklist) && (!permamentErrNum))
                        {
                            sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                            " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','','','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                            database.ExecuteNonQuery(sql);
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                    }
                }
                sql = @"delete from MSGQUEUEB4single_UAE where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("UAE PROCESS", ex.Message); }
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
            string sql = @"declare @i integer declare @s varchar(8) set @i=0
                while @i < 30
                begin select @s = left(NEWID(), 8) if not exists(select segment from mobtrackurl where segment = @s) break else begin set @s = '' set @i = @i + 1 end end
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

        #region < MAIN QUEUE 1 >
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

        private void processQUEUE()
        {
            string sql = "";
            try
            {
                //Process Single Queue on each cycle
                database.ExecuteNonQuery("if exists (SELECT * FROM sys.tables WHERE name='tmpMSGQUEUEsingle_NEW') DROP TABLE tmpMSGQUEUEsingle_NEW ");

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " UPDATE MSGQUEUEsingle SET picked_datetime = '" + pickdate + "' WHERE picked_datetime IS NULL ;" +
                    " SELECT * INTO tmpMSGQUEUEsingle_NEW FROM MSGQUEUEsingle WITH(NOLOCK) WHERE picked_datetime='" + pickdate + "' ; " +
                    " ALTER TABLE tmpMSGQUEUEsingle_NEW ADD IsProcessCheck BIT DEFAULT 0 NOT NULL ; ";
                //FOR TESTING
                //sql = " UPDATE MSGQUEUEsingle SET picked_datetime = '" + pickdate + "' WHERE picked_datetime ='2024-02-27 21:42:02.0770000' ;" +
                //    " SELECT * INTO tmpMSGQUEUEsingle_NEW FROM MSGQUEUEsingle WITH(NOLOCK) WHERE picked_datetime='" + pickdate + "' ; " +
                //    " ALTER TABLE tmpMSGQUEUEsingle_NEW ADD IsProcessCheck BIT DEFAULT 0 NOT NULL ; ";

                database.ExecuteNonQuery(sql);

                #region < SETTING SMPP ACCOUNT FOR SUBMISSION >
                //DataTable dtProfileID = database.GetDataTable("SELECT smppaccountid_1 FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) GROUP BY smppaccountid_1 ");

                //TEMPLATE ID WISE SMPPACCOUNT SETTING 
                DataTable dtTemplateId = database.GetDataTable("SELECT templateid FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) where isnull(templateid,'')<>'' and IsProcessCheck=0 GROUP BY templateid ");
                if (dtTemplateId.Rows.Count > 0)
                {
                    for (int q = 0; q < dtTemplateId.Rows.Count; q++)
                    {
                        string templateid = Convert.ToString(dtTemplateId.Rows[q]["templateid"]);
                        //Check User Wise
                        DataRow[] dtRows = dtSMPPAccountTemplate.Select("templateid = '" + templateid + "'");
                        if (dtRows.Length > 0)
                        {
                            string acID = "";
                            foreach (DataRow drr in dtRows)
                                acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                            DataTable dt = GetSMPPAccounts(acID);

                            if (dt.Rows.Count > 0)
                            {
                                Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) WHERE templateid='" + templateid + "' AND IsProcessCheck = 0  "));
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
                                        sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE templateid='" + templateid + "' AND smppaccountid IS NULL AND IsProcessCheck=0 ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                        }
                    }
                }


                //SENDER ID WISE SMPPACCOUNT SETTING
                DataTable dtSenderId = database.GetDataTable("SELECT SENDERID FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) where smppaccountid_1<>'11' AND IsProcessCheck = 0 GROUP BY SENDERID ");
                if (dtSenderId.Rows.Count > 0)
                {
                    for (int q = 0; q < dtSenderId.Rows.Count; q++)
                    {
                        string SENDERID = Convert.ToString(dtSenderId.Rows[q]["SENDERID"]);

                        //Check Sender Wise
                        DataRow[] dtRows = dtSMPPAccountSender.Select("SENDERID = '" + SENDERID + "'");
                        if (dtRows.Length > 0)
                        {
                            string acID = "";
                            foreach (DataRow drr in dtRows)
                                acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                            DataTable dt = GetSMPPAccounts(acID);
                            if (dt.Rows.Count > 0)
                            {
                                Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) WHERE SENDERID='" + SENDERID + "' AND IsProcessCheck = 0 and smppaccountid_1<>'11' "));
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
                                        sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE SENDERID='" + SENDERID + "' AND smppaccountid IS NULL AND IsProcessCheck=0 and smppaccountid_1<>'11' ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                        }
                    }
                }

                //USER ID WISE SMPPACCOUNT SETTING 
                DataTable dtProfileId = database.GetDataTable("SELECT PROFILEID FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) where IsProcessCheck=0  GROUP BY PROFILEID ");
                if (dtProfileId.Rows.Count > 0)
                {
                    for (int q = 0; q < dtProfileId.Rows.Count; q++)
                    {
                        string ProfileId = Convert.ToString(dtProfileId.Rows[q]["PROFILEID"]);
                        //Check User Wise
                        DataRow[] dtRows = dtSMPPAccountUserID.Select("userid = '" + ProfileId + "'");
                        if (dtRows.Length > 0)
                        {
                            string acID = "";
                            foreach (DataRow drr in dtRows)
                                acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                            DataTable dt = GetSMPPAccounts(acID);

                            if (dt.Rows.Count > 0)
                            {
                                Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) WHERE PROFILEID='" + ProfileId + "' AND IsProcessCheck = 0  "));
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
                                        sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE PROFILEID='" + ProfileId + "' AND smppaccountid IS NULL AND IsProcessCheck=0 ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                        }
                    }
                }

                //SMPPACCOUNT SETTING FOR OTP SMS
                DataTable dtotp = database.GetDataTable("SELECT * FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) where smppaccountid_1='11' AND IsProcessCheck = 0");
                if (dtotp.Rows.Count > 0)
                {
                    //Check FOR OTP Record
                    DataRow[] dtRows = dtSMPPSetting.Select("FOR_otp=1");
                    if (dtRows.Length > 0)
                    {
                        string acID = "";
                        foreach (DataRow drr in dtRows)
                            acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                        DataTable dt = GetSMPPAccounts(acID);

                        if (dt.Rows.Count > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) WHERE smppaccountid_1='11' AND IsProcessCheck = 0 "));
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
                                    sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE smppaccountid_1='11' AND smppaccountid IS NULL AND IsProcessCheck=0 ";
                                    database.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }

                //SMPPACCOUNT SETTING FOR TRANS SMS
                DataTable dtOth = database.GetDataTable("SELECT * FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) where IsProcessCheck=0");
                if (dtOth.Rows.Count > 0)
                {
                    //Check FOR OTP Record
                    DataRow[] dtRows = dtSMPPSetting.Select("API_ACTIVE=1");
                    if (dtRows.Length > 0)
                    {
                        string acID = "";
                        foreach (DataRow drr in dtRows)
                            acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                        DataTable dt = GetSMPPAccounts(acID);

                        if (dt.Rows.Count > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_NEW WITH(NOLOCK) WHERE IsProcessCheck = 0 "));
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
                                    sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE smppaccountid IS NULL AND IsProcessCheck=0 ";
                                    database.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region < BLOCKING THROUGH USERID >
                DataTable dtUser = database.GetDataTable("Select t.PROFILEID from tmpMSGQUEUEsingle_NEW t left join blocksmsSID b with (nolock) on t.SENDERID=b.SID where b.SID is null group by t.PROFILEID");
                if (dtUser.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUser.Rows)
                    {
                        DataRow[] drB = dtBlockPer.Select("userid='" + dr["Profileid"].ToString() + "'");
                        //DataTable dtB = GetBlockSMSper(dr["Profileid"].ToString());
                        if (drB.Length > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle_NEW where profileid='" + dr["Profileid"].ToString() + "' "));

                            double Bper = Convert.ToDouble(drB[0]["blockpercent"]);
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    "left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    "left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    "where t.profileid='" + dr["Profileid"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkDlr (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            double Fper = Convert.ToDouble(drB[0]["failpercent"]);  //GetBlockSMSper(userId, "F");
                            if (Fper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    " left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    " left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    " where t.profileid='" + dr["Profileid"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkFail (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                #endregion

                #region < BLOCKING THROUGH SENDERID >
                DataTable dtSender = database.GetDataTable("Select t.SENDERID from tmpMSGQUEUEsingle_NEW t INNER join blocksmsSID b with (nolock) on t.SENDERID=b.SID group by t.SENDERID");
                if (dtSender.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSender.Rows)
                    {
                        DataRow[] drB = dtBlockPerSID.Select("SID='" + dr["SENDERID"].ToString() + "'");
                        //DataTable dtB = GetBlockSMSper(dr["Profileid"].ToString());
                        if (drB.Length > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle_NEW where SENDERID='" + dr["SENDERID"].ToString() + "' "));

                            double Bper = Convert.ToDouble(drB[0]["blockpercent"]);
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    "left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    "left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    "where t.SENDERID='" + dr["SENDERID"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkDlr (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            double Fper = Convert.ToDouble(drB[0]["failpercent"]);  //GetBlockSMSper(userId, "F");
                            if (Fper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    " left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    " left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    " where t.SENDERID='" + dr["SENDERID"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkFail (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                #endregion

                //07-04-2023 NAVED ... Template ID ....
                #region < BLOCKING THROUGH TEMPLATEID >
                DataTable dtTemplateid = database.GetDataTable("Select t.templateid from tmpMSGQUEUEsingle_NEW t INNER join blocksmsTEMPLATEID b with (nolock) on t.templateid=b.TID group by T.templateid");
                if (dtTemplateid.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTemplateid.Rows)
                    {
                        DataRow[] drB = dtBlockPerTID.Select("TID='" + dr["templateid"].ToString() + "'");
                        //DataTable dtB = GetBlockSMSper(dr["Profileid"].ToString());
                        if (drB.Length > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("select count(*) from tmpMSGQUEUEsingle_NEW where templateid='" + dr["templateid"].ToString() + "' "));

                            double Bper = Convert.ToDouble(drB[0]["blockpercent"]);
                            if (Bper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Bper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    "left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    "left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    "where t.templateid='" + dr["templateid"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkDlr (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            double Fper = Convert.ToDouble(drB[0]["failpercent"]);  //GetBlockSMSper(userId, "F");
                            if (Fper != 0)
                            {
                                Int32 cnt20 = Convert.ToInt32(Convert.ToDouble(rowcnt) * Fper);

                                database.ExecuteNonQuery("if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_blk') drop table tmpMSGQUEUEsingle_blk ");

                                sql = " select top " + cnt20 + " t.* into tmpMSGQUEUEsingle_blk from tmpMSGQUEUEsingle_NEW t " +
                                    " left join blocksmswhitelist w on t.TOMOBILE=w.mobile and t.PROFILEID=w.userid " +
                                    " left join blocksmswhitelistglobal w1 on t.TOMOBILE=w1.mobile " +
                                    " where t.templateid='" + dr["templateid"].ToString() + "' and w.mobile is null and w1.mobile is null ORDER BY NEWID() " +
                                    " delete d from tmpMSGQUEUEsingle_NEW d inner join tmpMSGQUEUEsingle_blk t on d.tomobile = t.tomobile  ;  " +
                                    " insert into MSGQUEUEblkFail (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1) " +
                                    " select PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,templateid,smsrate,datacode,msgidClient,noofsms,smppaccountid_1 from tmpMSGQUEUEsingle_blk ; ";
                                database.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }
                #endregion

                #region < ADDING RECORDS TO TRAN TABLE & DELETE FROM QUEUE >
                DataTable dtAcn = database.GetDataTable("select left(t.smppaccountid,2) as acid,TranTableName,s.provider+'-'+s.systemid as provider from tmpMSGQUEUEsingle_NEW t join SMPPSETTING s on left(t.smppaccountid,2)=s.smppaccountid where active=1 group by left(t.smppaccountid,2),TranTableName,s.provider+'-'+s.systemid ");

                sql = "if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_NEW') ";
                foreach (DataRow dr in dtAcn.Rows)
                {
                    string acid = dr["acid"].ToString();
                    string TranTableName = dr["TranTableName"].ToString();
                    string provider = dr["provider"].ToString();
                    sql = sql + @" INSERT INTO " + TranTableName + @" (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid,msgid2)
                    select '" + provider + @"',smppaccountid,profileid,msgtext,TOMOBILE,SENDERID,CREATEDAT,'1' as fileid,peid,DATACODE,smsrate,templateid,msgidClient from tmpMSGQUEUEsingle_NEW t Where left(t.smppaccountid,2)=" + acid + "; ";
                }
                sql = sql + @"insert into SMSFailOverOBD (profileid,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS,isHMILOTP) 
                select  PROFILEID,MSGTEXT, TOMOBILE, GETDATE(), msgidClient, 'Queued','1' from tmpMSGQUEUEsingle_NEW tmp 
                inner join HMILOTPTemplates h on h.TEMPLATEID=tmp.templateid ; ";

                sql = sql + " delete from MSGQUEUEsingle where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
                #endregion

            }
            catch (Exception ex)
            {
                { LogError("p2", ex.Message); }
            }
        }
        #endregion

        #region < MAIN QUEUE 2 HMIOTP >
        private void timerPROCESSQ_OTP_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processQ_OTP)
                {
                    processQ_OTP = true;
                    processQUEUE_OTP();
                    processQ_OTP = false;
                }
            }
            catch (Exception ex)
            {
                processQ_OTP = false;
                LogError("processQUEUE_OTP" + ex.StackTrace, ex.Message);
            }
        }

        private void processQUEUE_OTP()
        {
            string sql = "";
            try
            {
                //Process Single Queue on each cycle
                database.ExecuteNonQuery("if exists (SELECT * FROM sys.tables WHERE name='tmpMSGQUEUEsingle_OTP_NEW') DROP TABLE tmpMSGQUEUEsingle_OTP_NEW ");

                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " UPDATE MSGQUEUEsingle_OTP SET picked_datetime = '" + pickdate + "' WHERE picked_datetime IS NULL ;" +
                    " SELECT * INTO tmpMSGQUEUEsingle_OTP_NEW FROM MSGQUEUEsingle_OTP WITH(NOLOCK) WHERE picked_datetime='" + pickdate + "' ; " +
                    " ALTER TABLE tmpMSGQUEUEsingle_OTP_NEW ADD IsProcessCheck BIT DEFAULT 0 NOT NULL ; ";
                //FOR TESTING
                //sql = " UPDATE MSGQUEUEsingle SET picked_datetime = '" + pickdate + "' WHERE picked_datetime ='2024-02-27 21:42:02.0770000' ;" +
                //    " SELECT * INTO tmpMSGQUEUEsingle_NEW FROM MSGQUEUEsingle WITH(NOLOCK) WHERE picked_datetime='" + pickdate + "' ; " +
                //    " ALTER TABLE tmpMSGQUEUEsingle_NEW ADD IsProcessCheck BIT DEFAULT 0 NOT NULL ; ";

                database.ExecuteNonQuery(sql);

                #region < SETTING SMPP ACCOUNT FOR SUBMISSION >
                

                DataTable dtotp = database.GetDataTable("SELECT * FROM tmpMSGQUEUEsingle_OTP_NEW WITH(NOLOCK) where smppaccountid_1='11' AND IsProcessCheck = 0");
                if (dtotp.Rows.Count > 0)
                {
                    //Check FOR OTP Record
                    DataRow[] dtRows = dtSMPPSetting.Select("FOR_otp2=1");
                    if (dtRows.Length > 0)
                    {
                        string acID = "";
                        foreach (DataRow drr in dtRows)
                            acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                        DataTable dt = GetSMPPAccounts(acID);

                        if (dt.Rows.Count > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_OTP_NEW WITH(NOLOCK) WHERE smppaccountid_1='11' AND IsProcessCheck = 0 "));
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
                                    sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_OTP_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE smppaccountid_1='11' AND smppaccountid IS NULL AND IsProcessCheck=0 ";
                                    database.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }

                DataTable dtOth = database.GetDataTable("SELECT * FROM tmpMSGQUEUEsingle_OTP_NEW WITH(NOLOCK) where IsProcessCheck=0");
                if (dtOth.Rows.Count > 0)
                {
                    //Check FOR OTP Record
                    DataRow[] dtRows = dtSMPPSetting.Select("API_ACTIVE=1");
                    if (dtRows.Length > 0)
                    {
                        string acID = "";
                        foreach (DataRow drr in dtRows)
                            acID = acID + Convert.ToString(drr["SMPPACCOUNTID"]) + "','";

                        DataTable dt = GetSMPPAccounts(acID);

                        if (dt.Rows.Count > 0)
                        {
                            Int32 rowcnt = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(*) FROM tmpMSGQUEUEsingle_OTP_NEW WITH(NOLOCK) WHERE IsProcessCheck = 0 "));
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
                                    sql = "UPDATE TOP (" + dt.Rows[i]["cnt"].ToString() + ") tmpMSGQUEUEsingle_OTP_NEW SET smppaccountid = '" + dt.Rows[i]["smppaccountid"].ToString() + "',IsProcessCheck=1 WHERE smppaccountid IS NULL AND IsProcessCheck=0 ";
                                    database.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }

                #endregion

                
                #region < ADDING RECORDS TO TRAN TABLE & DELETE FROM QUEUE >
                DataTable dtAcn = database.GetDataTable("select left(t.smppaccountid,2) as acid,TranTableName,s.provider+'-'+s.systemid as provider from tmpMSGQUEUEsingle_OTP_NEW t join SMPPSETTING s on left(t.smppaccountid,2)=s.smppaccountid where active=1 group by left(t.smppaccountid,2),TranTableName,s.provider+'-'+s.systemid ");

                sql = "if exists (select * from sys.tables where name='tmpMSGQUEUEsingle_OTP_NEW') ";
                foreach (DataRow dr in dtAcn.Rows)
                {
                    string acid = dr["acid"].ToString();
                    string TranTableName = dr["TranTableName"].ToString();
                    string provider = dr["provider"].ToString();
                    sql = sql + @" INSERT INTO " + TranTableName + @" (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,FILEID,peid,DATACODE,smsrate,templateid,msgid2)
                    select '" + provider + @"',smppaccountid,profileid,msgtext,TOMOBILE,SENDERID,CREATEDAT,'1' as fileid,peid,DATACODE,smsrate,templateid,msgidClient from tmpMSGQUEUEsingle_OTP_NEW t Where left(t.smppaccountid,2)=" + acid + " ";
                }

                sql = sql + @" insert into SMSFailOverOBD (profileid,MSGTEXT,TOMOBILE,SENTDATETIME,MSGID,DELIVERY_STATUS,isHMILOTP) 
                select  PROFILEID,MSGTEXT, TOMOBILE, GETDATE(), msgidClient, 'Queued','1' from tmpMSGQUEUEsingle_OTP_NEW tmp 
                inner join HMILOTPTemplates h on h.TEMPLATEID=tmp.templateid ; ";

                sql = sql + " delete from MSGQUEUEsingle_OTP where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
                #endregion

            }
            catch (Exception ex)
            {
                { LogError("p2", ex.Message); }
            }
        }
        #endregion

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
                    " and s.smppaccountid in ('" + accountid + "0')";
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
                            if (sender.ToUpper() == "HMISVR") templID = templID + "$" + dr["ALLSENDER"].ToString();
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
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

                            // rabi for template DLT block 02/11/2022
                            string errcd_ = "5308";
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());

                                if (templID != "")
                                {
                                    errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                    if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                if (errcd_ != "")
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

                                        // string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + dr["peid"].ToString() + "','" + templID + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
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
                sql = " Update top(5000) MSGQUEUEB4singleHMISVR set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4singleHMISVR where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sender = dr["senderid"].ToString();
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMISVR("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            string errcd_ = "5308";
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    bool reject = true;

                                    #region <HYUNDAI TEMPLATE >
                                    //periodic service 
                                    if (msg.ToUpper().Contains("DEAR CUSTOMER") && msg.ToUpper().Contains("PERIODIC") && msg.ToUpper().Contains("SERVICE OF YOUR CAR") && msg.ToUpper().Contains("PLEASE VISIT BELOW LINK") && msg.ToUpper().Contains("TO BOOK YOU"))
                                    {
                                        msg = msg.Replace("Service of your car", "Service of your Hyundai car");
                                        msg = msg.Replace("to book you", "to book your");
                                        msg = msg.Replace("For further details please contact ", "For further details please contact your Personal Mobility Advisor (PMA) ");
                                        msg = msg.Replace(" for appointment.", ". To get WhatsApp Notification, Kindly give missed call on 8367796869.");
                                        int p = msg.IndexOf("8367796869") + 11;
                                        try { msg = msg.Substring(0, p); } catch (Exception r) { }
                                        templID = "1107162824287096749";
                                        reject = false;
                                    }
                                    //else if (msg.ToUpper().Contains("DEAR CUSTOMER REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    else if (msg.ToUpper().Contains("REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    {   //vehicle ready
                                        msg = msg + " Hyundai";
                                        templID = "1107166919148623236";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WHICH IS HIGHER THAN OUR LIMIT FOR THIS MODEL") && msg.ToUpper().Contains("PLEASE LOOK INTO DETAILS OF THE SAME."))
                                    {   //bill amount higher than limit
                                        msg = "Dear Customer, " + msg + " Hyundai";
                                        templID = "1107166936627205655";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("PICK UP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //pickup schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936623571996";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR PICK-UP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for pickup
                                        msg = msg + " Hyundai";
                                        templID = "1107166936619220327";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DROP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //drop schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936613949914";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR DROP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for drop
                                        msg = msg + " Hyundai";
                                        templID = "1107166936593224635";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DEAR CUSTOMER, YOUR APPOINTMENT ON") && msg.ToUpper().Contains("WILL WAIT FOR YOU AT APPOINTMENT COUNTER") && (!msg.ToUpper().Contains("PAID SERVICE")))
                                    {   //appointment confirmation
                                        msg = msg + " Hyundai";
                                        templID = "1107167081856669406";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("REQUEST FOR ONLINE PAYMENT") && msg.ToUpper().Contains("KINDLY IGNORE IF PAYMENT ALREADY DONE"))
                                    {   //request for online payment
                                        msg = msg + " Hyundai";
                                        templID = "1107167081851720110";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("YOUR CUSTOMER WITH VEHICLE REG.NO") && msg.ToUpper().Contains("HAS GIVEN POOR RATING IN DSI"))
                                    {   //poor rating in DSI
                                        msg = msg + " Hyundai";
                                        templID = "1107167081854440883";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("OPT THE DRY WASH FOR YOUR CAR SERVICE AND HELP IN SAVING") && msg.ToUpper().Contains("BEYONDMOBILITY"))
                                    {   //Opt for dry wash
                                        msg = "Dear Customer, " + msg;
                                        templID = "1107168567279212434";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WITH DEALERSHIP TO CONFIRM YOUR CAR BOOKING IN SYSTEM"))
                                    {   //Opt for dry wash
                                        msg = msg + " -Hyundai";
                                        templID = "1107169650933028332";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    #endregion
                                    if (reject)
                                    {
                                        // process REJECTION ....//insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string msgid = "";
                                            if (x == 0) msgid = dr["msgidClient"].ToString();
                                            else msgid = "S" + Convert.ToString(Guid.NewGuid());

                                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            string smsTex = "";
                                            try
                                            {
                                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            }
                                            catch (Exception ex) { smsTex = msg; }
                                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                            database.ExecuteNonQuery(sql);
                                            sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";

                                            try { database.ExecuteNonQuery(sql); }
                                            catch (Exception E) { LogError_HMISVR("c1", E.Message); }
                                            try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                            catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                        }
                                    }
                                }
                                else
                                {
                                    string[] ar1 = templID.Split('$');
                                    templID = ar1[0];
                                    sender = ar1[1];
                                }
                            }
                            else
                            {
                                templID = templateid;
                            }

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + sender + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,PEID,TEMPLATEID) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + Convert.ToString(dr["peid"]) + "','" + templID + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        database.ExecuteNonQuery(sql);
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try
                                        {
                                            database.ExecuteNonQuery(sql);
                                        }
                                        catch (Exception E) { LogError_HMISVR("c1", E.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + sender + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
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

        //--------------------------New Code for HMI OTP  ----------------------------------//

        private void timerPROCESSb4Q_Tick_HMIOTP(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!processB4Q_HMIOTP)
                {
                    processB4Q_HMIOTP = true;
                    processB4QUEUE_HMIOTP();
                    processB4Q_HMIOTP = false;
                }
            }
            catch (Exception ex)
            {
                processB4Q_HMIOTP = false;
                LogError_HMIOTP("processB4QUEUE_HMIOTP_" + ex.StackTrace, ex.Message);
            }
        }

        private void processB4QUEUE_HMIOTP()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update top(5000) MSGQUEUEB4singleHMIOTP set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4singleHMIOTP where picked_datetime='" + pickdate + "' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sender = dr["senderid"].ToString();
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMIOTP("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMIOTP("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_HMIOTP("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMIOTP("dlrCallBack", E.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            string errcd_ = "5308";
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());
                                if (templID == "")
                                {
                                    bool reject = true;

                                    #region <HYUNDAI TEMPLATE >
                                    //periodic service 
                                    if (msg.ToUpper().Contains("DEAR CUSTOMER") && msg.ToUpper().Contains("PERIODIC") && msg.ToUpper().Contains("SERVICE OF YOUR CAR") && msg.ToUpper().Contains("PLEASE VISIT BELOW LINK") && msg.ToUpper().Contains("TO BOOK YOU"))
                                    {
                                        msg = msg.Replace("Service of your car", "Service of your Hyundai car");
                                        msg = msg.Replace("to book you", "to book your");
                                        msg = msg.Replace("For further details please contact ", "For further details please contact your Personal Mobility Advisor (PMA) ");
                                        msg = msg.Replace(" for appointment.", ". To get WhatsApp Notification, Kindly give missed call on 8367796869.");
                                        int p = msg.IndexOf("8367796869") + 11;
                                        try { msg = msg.Substring(0, p); } catch (Exception r) { }
                                        templID = "1107162824287096749";
                                        reject = false;
                                    }
                                    //else if (msg.ToUpper().Contains("DEAR CUSTOMER REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    else if (msg.ToUpper().Contains("REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    {   //vehicle ready
                                        msg = msg + " Hyundai";
                                        templID = "1107166919148623236";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WHICH IS HIGHER THAN OUR LIMIT FOR THIS MODEL") && msg.ToUpper().Contains("PLEASE LOOK INTO DETAILS OF THE SAME."))
                                    {   //bill amount higher than limit
                                        msg = "Dear Customer, " + msg + " Hyundai";
                                        templID = "1107166936627205655";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("PICK UP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //pickup schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936623571996";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR PICK-UP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for pickup
                                        msg = msg + " Hyundai";
                                        templID = "1107166936619220327";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DROP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //drop schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936613949914";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR DROP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for drop
                                        msg = msg + " Hyundai";
                                        templID = "1107166936593224635";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DEAR CUSTOMER, YOUR APPOINTMENT ON") && msg.ToUpper().Contains("WILL WAIT FOR YOU AT APPOINTMENT COUNTER") && (!msg.ToUpper().Contains("PAID SERVICE")))
                                    {   //appointment confirmation
                                        msg = msg + " Hyundai";
                                        templID = "1107167081856669406";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("REQUEST FOR ONLINE PAYMENT") && msg.ToUpper().Contains("KINDLY IGNORE IF PAYMENT ALREADY DONE"))
                                    {   //request for online payment
                                        msg = msg + " Hyundai";
                                        templID = "1107167081851720110";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("YOUR CUSTOMER WITH VEHICLE REG.NO") && msg.ToUpper().Contains("HAS GIVEN POOR RATING IN DSI"))
                                    {   //poor rating in DSI
                                        msg = msg + " Hyundai";
                                        templID = "1107167081854440883";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("OPT THE DRY WASH FOR YOUR CAR SERVICE AND HELP IN SAVING") && msg.ToUpper().Contains("BEYONDMOBILITY"))
                                    {   //Opt for dry wash
                                        msg = "Dear Customer, " + msg;
                                        templID = "1107168567279212434";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WITH DEALERSHIP TO CONFIRM YOUR CAR BOOKING IN SYSTEM"))
                                    {   //Opt for dry wash
                                        msg = msg + " -Hyundai";
                                        templID = "1107169650933028332";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    #endregion
                                    if (reject)
                                    {
                                        // process REJECTION ....//insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string msgid = "";
                                            if (x == 0) msgid = dr["msgidClient"].ToString();
                                            else msgid = "S" + Convert.ToString(Guid.NewGuid());

                                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            string smsTex = "";
                                            try
                                            {
                                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            }
                                            catch (Exception ex) { smsTex = msg; }
                                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                            database.ExecuteNonQuery(sql);
                                            sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";

                                            try { database.ExecuteNonQuery(sql); }
                                            catch (Exception E) { LogError_HMIOTP("c1", E.Message); }
                                            try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                            catch (Exception E) { LogError_HMIOTP("dlrCallBack", E.Message); }
                                        }
                                    }
                                }
                                else
                                {
                                    if (dr["senderid"].ToString() == "HMISVR")
                                    {
                                        string[] ar1 = templID.Split('$');
                                        templID = ar1[0];
                                        sender = ar1[1];
                                    }
                                }
                            }
                            else
                            {
                                templID = templateid;
                            }

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + sender + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,PEID,TEMPLATEID) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + Convert.ToString(dr["peid"]) + "','" + templID + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        database.ExecuteNonQuery(sql);
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try
                                        {
                                            database.ExecuteNonQuery(sql);
                                        }
                                        catch (Exception E) { LogError_HMIOTP("c1", E.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMIOTP("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle_OTP (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + sender + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                    }
                }
                sql = @"delete from MSGQUEUEB4singleHMIOTP where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError_HMIOTP("P1", ex.Message); }
            }
        }

        private void LogError_HMIOTP(string title, string msg)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"_Log_HMIOTP" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    //processB4Q_2201 = true;
                    processB4QUEUE_2201();
                    //processB4Q_2201 = false;
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
                sql = " Update top(5000) MSGQUEUEB4single2201 set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError_2201("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }

                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;


                            // rabi for template DLT block 02/11/2022
                            string errcd_ = "5308";
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());

                                if (templID != "")
                                {
                                    errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                    if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + Convert.ToString(dr["senderid"]) + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "' "));
                                if (errcd_ != "")
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        //Add peid and templateid ... Naved... 07/04/2023
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + dr["peid"].ToString() + "','" + templID + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);

                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
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

                                //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
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
                            //lastTemplateID = templateid;
                            //lastMessageText = msg;
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

        private bool IsBlackList(string mob)
        {
            return false;
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

        private void timerPROCESSb4Q_API_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                //if (!processB4Q_API)
                //{
                //processB4Q_API = true;
                processB4QUEUE_API();
                //processB4Q_API = false;
                //}
            }
            catch (Exception ex)
            {
                processB4Q_API = false;
                LogError("processB4QUEUE_API_" + ex.StackTrace, ex.Message);
            }
        }
        private void processB4QUEUE_API()
        {
            //string lastMessageText = "";
            //string lastTemplateID = "";
            string sql = "";
            try
            {
                string pickdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                sql = " Update TOP(5000) MSGQUEUEB4singleAPI set picked_datetime = '" + pickdate + "' where picked_datetime is null ;";
                database.ExecuteNonQuery(sql);
                sql = "Select * from MSGQUEUEB4singleAPI where picked_datetime='" + pickdate + "' ; ";
                //sql = "Select * from MSGQUEUEB4singleAPI where picked_datetime='2023-12-01 14:24:35.1166667' ; ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sender = dr["senderid"].ToString();
                        int noofsms = Convert.ToInt16(dr["noofsms"]);
                        string msg = Convert.ToString(dr["msgText"]);
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

                                string smsTex = "";
                                try
                                {
                                    smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                }
                                catch (Exception ex) { smsTex = msg; }
                                //Add peid and templateid ... Naved... 07/04/2023
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','BLACKLIST','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B1", ex.Message); }
                                sql = @" Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND,peid,templateid) " +
                                " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" +
                                dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B2", ex.Message); }
                                sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:250 text:' AS DLVRTEXT," +
                                " '" + msgid + "', GETDATE(), 'Undeliverable','250',getdate() ; ";
                                try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("B3", ex.Message); }
                                try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), "250"); }
                                catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                            }
                        }
                        else
                        {
                            string templID = "";
                            sql = "";
                            //if (msg == lastMessageText) templateid = lastTemplateID;

                            string errcd_ = "5308";
                            // rabi for template DLT block 02/11/2022
                            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                            {
                                templID = GetTemplateIDfromSMS(dr["senderid"].ToString(), dr["msgtext"].ToString());

                                if (templID == "")
                                {
                                    bool reject = true;
                                    #region <HYUNDAI TEMPLATE >
                                    //periodic service 
                                    if (msg.ToUpper().Contains("DEAR CUSTOMER") && msg.ToUpper().Contains("PERIODIC") && msg.ToUpper().Contains("SERVICE OF YOUR CAR") && msg.ToUpper().Contains("PLEASE VISIT BELOW LINK") && msg.ToUpper().Contains("TO BOOK YOU"))
                                    {
                                        msg = msg.Replace("Service of your car", "Service of your Hyundai car");
                                        msg = msg.Replace("to book you", "to book your");
                                        msg = msg.Replace("For further details please contact ", "For further details please contact your Personal Mobility Advisor (PMA) ");
                                        msg = msg.Replace(" for appointment.", ". To get WhatsApp Notification, Kindly give missed call on 8367796869.");
                                        int p = msg.IndexOf("8367796869") + 11;
                                        try { msg = msg.Substring(0, p); } catch (Exception r) { }
                                        templID = "1107162824287096749";
                                        reject = false;
                                    }
                                    //else if (msg.ToUpper().Contains("DEAR CUSTOMER REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    else if (msg.ToUpper().Contains("REPAIR OF YOUR VEHICLE IS ALMOST DONE AND WILL BE READY IN 45 MIN"))
                                    {   //vehicle ready
                                        msg = msg + " Hyundai";
                                        templID = "1107166919148623236";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WHICH IS HIGHER THAN OUR LIMIT FOR THIS MODEL") && msg.ToUpper().Contains("PLEASE LOOK INTO DETAILS OF THE SAME."))
                                    {   //bill amount higher than limit
                                        msg = "Dear Customer, " + msg + " Hyundai";
                                        templID = "1107166936627205655";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("PICK UP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //pickup schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936623571996";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR PICK-UP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for pickup
                                        msg = msg + " Hyundai";
                                        templID = "1107166936619220327";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DROP OF YOUR VEHICLE") && msg.ToUpper().Contains("SCHEDULED AT") && msg.ToUpper().Contains("THANK YOU FOR CHOOSING"))
                                    {   //drop schedule
                                        msg = msg + " Hyundai";
                                        templID = "1107166936613949914";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("NEW JOB REQUEST FOR DROP") && msg.ToUpper().Contains("PHONE NUMBER"))
                                    {   //job request for drop
                                        msg = msg + " Hyundai";
                                        templID = "1107166936593224635";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("DEAR CUSTOMER, YOUR APPOINTMENT ON") && msg.ToUpper().Contains("WILL WAIT FOR YOU AT APPOINTMENT COUNTER") && (!msg.ToUpper().Contains("PAID SERVICE")))
                                    {   //appointment confirmation
                                        msg = msg + " Hyundai";
                                        templID = "1107167081856669406";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("REQUEST FOR ONLINE PAYMENT") && msg.ToUpper().Contains("KINDLY IGNORE IF PAYMENT ALREADY DONE"))
                                    {   //request for online payment
                                        msg = msg + " Hyundai";
                                        templID = "1107167081851720110";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("YOUR CUSTOMER WITH VEHICLE REG.NO") && msg.ToUpper().Contains("HAS GIVEN POOR RATING IN DSI"))
                                    {   //poor rating in DSI
                                        msg = msg + " Hyundai";
                                        templID = "1107167081854440883";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("OPT THE DRY WASH FOR YOUR CAR SERVICE AND HELP IN SAVING") && msg.ToUpper().Contains("BEYONDMOBILITY"))
                                    {   //Opt for dry wash
                                        msg = "Dear Customer, " + msg;
                                        templID = "1107168567279212434";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    else if (msg.ToUpper().Contains("WITH DEALERSHIP TO CONFIRM YOUR CAR BOOKING IN SYSTEM"))
                                    {   //Opt for dry wash
                                        msg = msg + " -Hyundai";
                                        templID = "1107169650933028332";
                                        sender = "HYNDAI";
                                        reject = false;
                                    }
                                    #endregion
                                    if (reject)
                                    {
                                        // process REJECTION ....//insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string msgid = "";
                                            if (x == 0) msgid = dr["msgidClient"].ToString();
                                            else msgid = "S" + Convert.ToString(Guid.NewGuid());

                                            //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            string smsTex = "";
                                            try
                                            {
                                                smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                            }
                                            catch (Exception ex) { smsTex = msg; }
                                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                            database.ExecuteNonQuery(sql);
                                            sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";

                                            try { database.ExecuteNonQuery(sql); }
                                            catch (Exception E) { LogError_HMISVR("c1", E.Message); }
                                            try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                            catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                        }
                                    }
                                }
                                else
                                {
                                    if (dr["senderid"].ToString().ToUpper() == "HMISVR")
                                    {
                                        string[] ar1 = templID.Split('$');
                                        templID = ar1[0];
                                        sender = ar1[1];
                                    }
                                }

                                if (templID != "")
                                {

                                }
                            }
                            else
                                templID = templateid;

                            if (templID != "")
                            {
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + sender + "' and TemplateID='" + templID + "' and peid='" + Convert.ToString(dr["peid"]) + "'"));
                                if (errcd_ != "")
                                {
                                    templID = "";
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

                                        //string smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        string smsTex = "";
                                        try
                                        {
                                            smsTex = GetSMSText(msg, x + 1, noofsms, ucs);
                                        }
                                        catch (Exception ex) { smsTex = msg; }
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS,peid,templateid) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "01','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + dr["smsrate"].ToString() + "','REJECTED " + errcd_ + "','" + dr["peid"].ToString() + "','" + dr["templateid"].ToString() + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + smsTex + "','" + dr["tomobile"].ToString() + "','" + dr["senderid"].ToString() + "',GETDATE(),GETDATE(),GETDATE(),'" + dr["msgidClient"].ToString() + "',getdate(),'1','1' ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C3", ex.Message); }
                                        sql = @" Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + msgid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + msgid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        try { database.ExecuteNonQuery(sql); } catch (Exception ex) { LogError("C4", ex.Message); }
                                        try { insertCallBack(dr["profileid"].ToString(), "", msgid, dr["senderid"].ToString(), dr["tomobile"].ToString(), errcd_); }
                                        catch (Exception E) { LogError_HMISVR("dlrCallBack", E.Message); }
                                    }
                                }
                                else
                                {
                                    templateid = templID;
                                    sql = "insert into MSGQUEUEsingle (PROVIDER,SMPPACCOUNTID_1,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                                    " VALUES ('','" + dr["SMPPACCOUNTID"].ToString() + "','" + dr["profileid"].ToString() + "',N'" + msg.Replace("'", "''") + "','" + dr["tomobile"].ToString() + "','" + sender + "','1','" + dr["peid"].ToString() + "','" + templateid + "','" + dr["smsrate"].ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + dr["msgidClient"].ToString() + "')";
                                    database.ExecuteNonQuery(sql);
                                    //lastTemplateID = templateid;
                                    //lastMessageText = msg;
                                }
                            }
                        }
                        database.ExecuteNonQuery("update customer set balance = BALANCE - (" + noofsms.ToString() + " * (" + dr["smsrate"].ToString() + " * 10)) / 1000 where username = '" + dr["Profileid"].ToString() + "'");
                    }
                }
                sql = @"delete from MSGQUEUEB4singleAPI where picked_datetime='" + pickdate + "' ";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogError("P1", ex.Message); }
            }
        }

        //Bind datatable SMPPAccount Tables
        private void timerPROCESS_SMPP_DtBind_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!IsprocessSMPP_DtBind)
                {
                    IsprocessSMPP_DtBind = true;
                    processSMPP_DtBind();
                    IsprocessSMPP_DtBind = false;
                }
            }
            catch (Exception ex)
            {
                IsprocessSMPP_DtBind = false;
                LogError("timerPROCESS_SMPP_DtBind_Tick_ " + ex.StackTrace, ex.Message);
            }
        }

        private void processSMPP_DtBind()
        {
            try
            {
                dtSMPPAccountTemplate = database.GetDataTable("SELECT * FROM smppaccountTemplate WITH(NOLOCK) WHERE ACTIVE = 1");
                dtSMPPAccountUserID = database.GetDataTable("SELECT * FROM smppaccountuserid WITH(NOLOCK) WHERE ACTIVE = 1");
                dtSMPPAccountSender = database.GetDataTable("SELECT * FROM smppaccountsender WITH(NOLOCK) WHERE ACTIVE = 1");
                dtSMPPSetting = database.GetDataTable("SELECT * FROM SMPPSETTING WITH(NOLOCK) WHERE (BINDINGMODE='Transceiver' OR BINDINGMODE='Transmiter') AND ACTIVE=1");
            }
            catch (Exception ex)
            {
                { LogError("processSMPP_DtBind - ", ex.Message); }
            }
        }
    }

    public class Root
    {
        public string STATUS { get; set; }
    }
}