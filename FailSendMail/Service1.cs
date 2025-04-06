using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Configuration;
using FailSendMail;

namespace FailSendMail
{
    public partial class Service1 : ServiceBase
    {
        Util obU = new Util();
        public DataTable dtSession = null;
        bool IsFailOverRunning = false;

        private Timer timerSMSFailOver = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogError("Serv Start", "");

            timerSMSFailOver = new Timer();
            this.timerSMSFailOver.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerSMSFailOver.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSFailOver_Tick);
            timerSMSFailOver.Enabled = true;
            this.timerSMSFailOver.Start();
        }

        protected override void OnStop()
        {
            this.timerSMSFailOver.Stop();
            LogError("Serv Stop.", "");
        }

        private void timerSMSFailOver_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!IsFailOverRunning)
                {
                    IsFailOverRunning = true;
                    obU.FetchfailOverRecord();
                    IsFailOverRunning = false;
                }
            }
            catch (Exception ex)
            {
                LogError("FailOverTimer_" + ex.StackTrace, ex.Message);
            }
        }

        public void Debug()
        {
            obU.FetchfailOverRecord();
        }

        private void LogError(string title, string msg)
        {
            try
            {
                if (1 == 1)
                {

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
    }
}
