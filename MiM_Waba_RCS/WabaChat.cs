using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TechXureWinApp
{
    public partial class WabaChat : Form
    {
        int no_of_timer = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_timer"]);
        int no_of_record = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_record"]);

        string WAapikey = Convert.ToString(ConfigurationManager.AppSettings["WAapikey"]);
        string WAauthkey = "";
        string WabaNo = "";
        string WabaURL = "";

        Util _log = new Util();
        Util obU = new Util();

        bool IsRunning = false;

        private System.Timers.Timer timerWabaSession;
        private System.Timers.Timer timerWabaProcessTable;
        private System.Timers.Timer timerWaba_InProcess_Reply;

        public WabaChat()
        {
           
            InitializeComponent();
        }

        private void WabaChat_Load(object sender, EventArgs e)
        {

            GetChatProcessTable();
            Initialize_Session();

        }

        public void Initialize_Session()
        {
            obU._Log("Service started.");
            try
            {
                timerWaba_InProcess_Reply = new System.Timers.Timer();
                timerWaba_InProcess_Reply.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["WABA_REPLY_INTERVAL"]);
                timerWaba_InProcess_Reply.Elapsed += new System.Timers.ElapsedEventHandler(this.timerWaba_InProcess_Reply_Tick);
                timerWaba_InProcess_Reply.Enabled = true;

                timerWabaSession = new System.Timers.Timer();
                timerWabaSession.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["WABA_SESSION_INTERVAL"]);
                timerWabaSession.Elapsed += new System.Timers.ElapsedEventHandler(this.timerWabaSession_Tick);
                timerWabaSession.Enabled = true;

                timerWabaProcessTable = new System.Timers.Timer();
                timerWabaProcessTable.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["WABA_PROCESS_TABLE_INTERVAL"]);
                timerWabaProcessTable.Elapsed += new System.Timers.ElapsedEventHandler(this.timerWabaProcessTable_Tick);
                timerWabaProcessTable.Enabled = true;
            }
            catch (Exception ex)
            {
                obU._Log("Initialize_Session : " + ex.Message);

                throw;
            }


        }

        private void timerWabaSession_Tick(object sender, EventArgs e)
        {
            ResetSession();
        }

        private void timerWabaProcessTable_Tick(object sender, EventArgs e)
        {
            GetChatProcessTable();
        }

        private void timerWaba_InProcess_Reply_Tick(object sender, EventArgs e)
        {
            WABA_InProcess_Reply();
        }

        public void ResetSession()
        {
            try
            {
                obU.ResetWabaSession("");
            }
            catch (Exception ex)
            {
                obU._Log("WABA_InProcess_Reply : " + ex.Message);

                throw;
            }
        }

        public void GetChatProcessTable()
        {
            try
            {
                DataTable dtcr = database.GetDataTable(@"select ApiKey_waba,ApiUrl_waba,WabaNo from CUSTOMER with (nolock) where username='MIM2200006'
");
                WAauthkey = Convert.ToString(dtcr.Rows[0]["ApiKey_waba"]);
                WabaURL = Convert.ToString(dtcr.Rows[0]["ApiUrl_waba"]);
                WabaNo = Convert.ToString(dtcr.Rows[0]["WabaNo"]);

                Util.dtWAChatProcess=database.GetDataTable("select ProcessNo,msgtext,reqtype,NextProcessNo from [WAChatProcess]");
            }
            catch (Exception ex)
            {
                obU._Log("GetChatProcessTable : " + ex.Message);
                throw;
            }
        }





        public DataTable GetDT(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sql;

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

            }
            return dt;
        }

        public string ProcessIncWhatsapp()
        {
            string res = "";
            try
            {
                DataTable IncRecords = GetDT("SP_GetWabaInRecordForTacoBell");

                if (IncRecords.Rows.Count > 0)
                {
                    obU.Log("Start Submit : Rec " + IncRecords.Rows.Count.ToString() + "_" + DateTime.Now.ToString());

                    for (int i = 0; i < IncRecords.Rows.Count; i++)
                    {
                       //Code Send Whatsapp Api
                       obj.SendIncMissedCallGupshup("0", IncRecords.Rows[i]["fromMob"].ToString(), CustDetails.Rows[0]["ApiKey_waba"].ToString(), IncRecords.Rows[i]["message_text"].ToString(), CustDetails.Rows[0]["username"].ToString(), "0", CustDetails.Rows[0]["WabaNo"].ToString(), CustDetails.Rows[0]["ApiUrl_waba"].ToString());
                       database.ExecuteNonQuery("update settings set lastfetchedtime = getdate()");
                        
                    }

                }
            }
            catch (Exception ex)
            {
                obU.Log("Error: " + ex.Message);
                //throw;
            }
            return res;
        }

        public void WABA_InProcess_Reply()
        {
            try
            {
                if (!IsRunning)
                {
                    IsRunning = true;

                    DataTable dt = obU.GetWabaInProcessRecord();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["MESSAGE_TEXT"].ToString()=="I am interested for a demo. Kindly contact me.")
                        {
                            obU.wachatapi_karix(0, dr["MobileNo"].ToString(), "Greetings!!! Thanks for connecting with MiM®. Please specify your requirement our team will get in touch with you shortly.", WAapikey, WAauthkey, WabaNo,0, WabaURL);
                        }
                        else
                        {
                            obU.Reply_Process(dr["MobileNo"].ToString(), dr["ProcessId"].ToString(), dr["MESSAGE_TEXT"].ToString(), dr["MESSAGE_Title"].ToString(), WAapikey, WAauthkey, dr["msgid"].ToString(),WabaURL,WabaNo);
                        }
                    }

                    IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                obU._Log("WABA_InProcess_Reply : " + ex.Message);

                throw;
            }
        }

    }
}
