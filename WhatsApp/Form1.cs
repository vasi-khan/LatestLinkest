using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsApp
{
    public partial class Form1 : Form
    {
        int no_of_timer = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_timer"]);
        int no_of_record = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_record"]);

        string WAapikey = Convert.ToString(ConfigurationManager.AppSettings["WAapikey"]);
        string WAauthkey = Convert.ToString(ConfigurationManager.AppSettings["WAauthkey"]);

        Util _log = new Util();
        Util obU = new Util();

        bool IsRunning = false;

        private System.Timers.Timer timerWabaSession;
        private System.Timers.Timer timerWabaProcessTable;
        private System.Timers.Timer timerWaba_InProcess_Reply;
        private System.Timers.Timer timerSaveIncomingmessage;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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

                timerSaveIncomingmessage = new System.Timers.Timer();
                timerSaveIncomingmessage.Interval = 1000;
                timerSaveIncomingmessage.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSaveIncomingmessage_Tick);
                timerSaveIncomingmessage.Enabled = true;

            }
            catch (Exception ex)
            {
                obU._Log("Initialize_Session : " + ex.Message);
                throw;
            }
        }
        private void timerSaveIncomingmessage_Tick(object sender, EventArgs e)
        {
            GetIncomingMessage();
        }

        public void GetIncomingMessage()
        {
            try
            {
                Util ob = new Util();
                DataTable dt =ob.GetIncomingMessage();
            }
            catch (Exception ex)
            {
                obU._Log("GetSaveIncomingmessage : " + ex.Message);
                throw;
            }
        }
        private void timerWabaSession_Tick(object sender, EventArgs e)
        {
           // ResetSession();
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
                Util.dtWAChatProcess = database.GetDataTable("select ProcessNo,TypeText,NextProcessNo,MsgText from [processflow]");
            }
            catch (Exception ex)
            {
                obU._Log("GetChatProcessTable : " + ex.Message);
                throw;
            }
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
                        obU.Reply_Process(dr["MobileNo"].ToString(), int.Parse(dr["ProcessId"].ToString()), dr["MESSAGE_TEXT"].ToString(), dr["MESSAGE_Title"].ToString(), WAapikey, WAauthkey, dr["msgid"].ToString());
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
