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


namespace MimSenddata 
{
    public partial class Service1 : ServiceBase
    {
        Util obU = new Util();
        public DataTable dtSession = null;
        bool IsFailOverRunning = false;
        bool IsFailOverRunningWABA = false;
        bool IsFailOverRunningEmail = false;
        bool ISWabaFailOverOBDRunning = false;
        private Timer timerSMSFailOverOBD = null;
        private Timer timerSMSFailOverWABA = null;
        private Timer timerSMSFailOverEmail = null;
        private Timer timerWabaFailOverOBD = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogError("Serv Start", "");
            
            timerSMSFailOverOBD = new Timer();
            this.timerSMSFailOverOBD.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerSMSFailOverOBD.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSFailOverOBD_Tick);
            timerSMSFailOverOBD.Enabled = true;
            this.timerSMSFailOverOBD.Start();

            timerSMSFailOverWABA = new Timer();
            this.timerSMSFailOverWABA.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerSMSFailOverWABA.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSFailOverWABA_Tick);
            timerSMSFailOverWABA.Enabled = true;
            this.timerSMSFailOverWABA.Start();

            timerSMSFailOverEmail = new Timer();
            this.timerSMSFailOverEmail.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerSMSFailOverEmail.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSMSFailOverEmail_Tick);
            timerSMSFailOverEmail.Enabled = true;
            this.timerSMSFailOverEmail.Start();

            //Add By Vikas For WabaFailOverOBD On 25-10-2023
            timerWabaFailOverOBD = new Timer();
            this.timerWabaFailOverOBD.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerWabaFailOverOBD.Elapsed += new System.Timers.ElapsedEventHandler(this.timerWabaFailOverOBD_Tick);
            timerWabaFailOverOBD.Enabled = true;
            this.timerWabaFailOverOBD.Start();
        }

        protected override void OnStop()
        {
            this.timerSMSFailOverOBD.Stop();
            this.timerSMSFailOverWABA.Stop();
            LogError("Serv Stop.", "");
        }
 
        private void timerSMSFailOverOBD_Tick(object sender, ElapsedEventArgs e)
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
                IsFailOverRunning = false;
                LogError("FailOverTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerSMSFailOverWABA_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!IsFailOverRunningWABA)
                {
                    IsFailOverRunningWABA = true;
                    obU.FetchfailOverRecordWABA();
                    IsFailOverRunningWABA = false;
                }
            }
            catch (Exception ex)
            {
                IsFailOverRunningWABA = false;
                LogError("FailOverWABATimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerSMSFailOverEmail_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!IsFailOverRunningEmail)
                {
                    IsFailOverRunningEmail = true;
                    obU.FetchfailOverRecordEmail();
                    IsFailOverRunningEmail = false;
                }
            }
            catch (Exception ex)
            {
                IsFailOverRunningEmail = false;
                LogError("timerSMSFailOverEmail_Tick_" + ex.StackTrace, ex.Message);
            }
        }

        public void Debug()
        {
            //obU.FetchfailOverRecord();
            //obU.FetchfailOverRecordWABA();
            obU.FetchWABAfailOverOBDRecord();

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

        private void writedata(string str)
        {
            try
            {
                if (1 == 1)
                {

                    FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"txt" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter m_stramWriter = new StreamWriter(fs);
                    m_stramWriter.BaseStream.Seek(0, SeekOrigin.End);
                    m_stramWriter.WriteLine(Convert.ToString(DateTime.Now) + "_" + str);
                    m_stramWriter.Flush();
                    m_stramWriter.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void timerWabaFailOverOBD_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!ISWabaFailOverOBDRunning)
                {
                    ISWabaFailOverOBDRunning = true;
                    obU.FetchWABAfailOverOBDRecord();
                    ISWabaFailOverOBDRunning = false;
                }
            }
            catch (Exception ex)
            {
                ISWabaFailOverOBDRunning = false;
                LogError("WABAFailOverOBDTimer_" + ex.StackTrace, ex.Message);
            }
        }
    }
}
