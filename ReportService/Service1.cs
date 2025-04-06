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
using System.Text.RegularExpressions;



namespace ReportService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timerPROCESSReport = null;
        bool bProcessReport = false;

        public Service1()
        {
            InitializeComponent();
        }
        public void Debug()
        {
            DownLoad_Report();
        }
        protected override void OnStart(string[] args)
        {
            LogError("Serv Started", "");
            timerPROCESSReport = new Timer();

            this.timerPROCESSReport.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_Report"]);
            this.timerPROCESSReport.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSReport_Tick);
            timerPROCESSReport.Enabled = true;
            this.timerPROCESSReport.Start();
        }
        private void timerPROCESSReport_Tick(object sender, System.Timers.ElapsedEventArgs e)
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
                                        fn = WriteData(mappath, dt, fn);
                                    }
                                }
                                else
                                {
                                    DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(date, date + " 23:59:59.997", user, "", IsTdy);
                                    fn = WriteData(mappath, dt, fn);
                                }
                            }
                           
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
                            fn = WriteData(mappath, dt, fn);
                        }
                    }
                    else
                    {
                        DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(F_date, To_Date + " 23:59:59.997", user, "", false);
                        fn = WriteData(mappath, dt, fn);
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

        protected override void OnStop()
        {
            this.timerPROCESSReport.Stop();
            LogError("Serv Stop.", "");
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
    }
}
