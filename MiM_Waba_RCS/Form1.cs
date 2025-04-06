using TechXureWinApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TechXureWinApp
{
    public partial class Form1 : Form
    {
        int no_of_timer = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_timer"]);
        int no_of_record = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_record"]);



        string api_provider = "";
        string Infobip_req_body = "";
        string knowalarity_req_body = "";

        string apikey = Convert.ToString(ConfigurationManager.AppSettings["apikey"]);
        string knowalarity_authkey = Convert.ToString(ConfigurationManager.AppSettings["knowalarity_authkey"]);
        string infobip_authkey = Convert.ToString(ConfigurationManager.AppSettings["infobip_authkey"]);

        string WAapikey = Convert.ToString(ConfigurationManager.AppSettings["WAapikey"]);
        string WAauthkey = Convert.ToString(ConfigurationManager.AppSettings["WAauthkey"]);


        string RCSauthkey = Convert.ToString(ConfigurationManager.AppSettings["RCSauthkey"]);

        private System.Timers.Timer[] timerPROCESS;
        private System.Timers.Timer[] WAtimerPROCESS;
        private System.Timers.Timer[] RCStimerPROCESS;
        private System.Timers.Timer[] EmailtimerPROCESS;

        private System.Timers.Timer timerEXCEP;
        private System.Timers.Timer timerapi_provider;
    
        Util _log = new Util();
        Util obU = new Util();

        public Form1()
        {
            InitializeComponent();
        }
        public void Initialize_Session()
        {
            _log.Info("Service started.");
            //create dynamic timer for each session. 
            try
            {
                //timerPROCESS = new System.Timers.Timer[no_of_timer];
                //for (int i = 1; i < no_of_timer; i++)
                //{
                //    System.Threading.Thread.Sleep(2000);
                //    timerPROCESS[i] = new System.Timers.Timer();
                //    timerPROCESS[i].Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]) + i*3;
                //    timerPROCESS[i].Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_Tick);
                //    timerPROCESS[i].Enabled = true;
                //}

                WAtimerPROCESS = new System.Timers.Timer[no_of_timer];
                for (int i = 1; i < no_of_timer; i++)
                {
                    System.Threading.Thread.Sleep(2000);
                    WAtimerPROCESS[i] = new System.Timers.Timer();
                    WAtimerPROCESS[i].Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]) + i * 3;
                    WAtimerPROCESS[i].Elapsed += new System.Timers.ElapsedEventHandler(this.WAtimerPROCESS_Tick);
                    WAtimerPROCESS[i].Enabled = true;
                }

                RCStimerPROCESS = new System.Timers.Timer[no_of_timer];
                for (int i = 1; i < no_of_timer; i++)
                {
                    System.Threading.Thread.Sleep(2000);
                    RCStimerPROCESS[i] = new System.Timers.Timer();
                    RCStimerPROCESS[i].Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]) + i * 3;
                    RCStimerPROCESS[i].Elapsed += new System.Timers.ElapsedEventHandler(this.RCStimerPROCESS_Tick);
                    RCStimerPROCESS[i].Enabled = true;
                }

                //EmailtimerPROCESS = new System.Timers.Timer[no_of_timer];
                //for (int i = 1; i < no_of_timer; i++)
                //{
                //    System.Threading.Thread.Sleep(2000);
                //    EmailtimerPROCESS[i] = new System.Timers.Timer();
                //    EmailtimerPROCESS[i].Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]) + i * 3;
                //    EmailtimerPROCESS[i].Elapsed += new System.Timers.ElapsedEventHandler(this.EmailtimerPROCESS_Tick);
                //    EmailtimerPROCESS[i].Enabled = true;
                //}

                timerEXCEP = new System.Timers.Timer();
                timerEXCEP.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_EXCEP"]);
                this.timerEXCEP.Elapsed += new System.Timers.ElapsedEventHandler(this.timerEXCEP_Tick);
                timerEXCEP.Enabled = true;
                this.timerEXCEP.Start();

                timerapi_provider = new System.Timers.Timer();
                timerapi_provider.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["API_PROVIDER_INTERVAL"]);
                this.timerapi_provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerapi_provider_Tick);
                timerapi_provider.Enabled = true;
                this.timerapi_provider.Start();

            }
            catch (Exception ex)
            {

                throw;
            }
           

        }
        private void timerEXCEP_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessEXCEP();
            }
            catch (Exception ex)
            {
                _log.Info_Err2("ProcessEXCEP_" + ex.StackTrace + " - " + ex.Message, 9999);
            }
        }

        public void ProcessEXCEP()
        {
            
        }
        private void timerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
           // System.Threading.Thread.Sleep(1000);
            System.Timers.Timer t = (System.Timers.Timer)sender;
            string s = Convert.ToString(Convert.ToInt32(t.Interval));
            int i = Convert.ToInt16(s.Substring(s.Length - 2, 2));
            if (clsCheck.inprocess[i])
            {
                //obU.InfoTest("returned as inprocess true for " + i.ToString());
                return;
            }

            ProcessOBDsending(i);

        }
        private void WAtimerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            // System.Threading.Thread.Sleep(1000);
            System.Timers.Timer t = (System.Timers.Timer)sender;
            string s = Convert.ToString(Convert.ToInt32(t.Interval));
            int i = Convert.ToInt16(s.Substring(s.Length - 2, 2));
            if (clsCheck.inprocess[i])
            {
                //obU.InfoTest("returned as inprocess true for " + i.ToString());
                return;
            }
            ProcessWAsending(i);
        }
        private void RCStimerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            // System.Threading.Thread.Sleep(1000);
            System.Timers.Timer t = (System.Timers.Timer)sender;
            string s = Convert.ToString(Convert.ToInt32(t.Interval));
            int i = Convert.ToInt16(s.Substring(s.Length - 2, 2));
            if (clsCheck.inprocess[i])
            {
                //obU.InfoTest("returned as inprocess true for " + i.ToString());
                return;
            }
            ProcessRCSsending(i);
        }
        private void EmailtimerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            // System.Threading.Thread.Sleep(1000);
            System.Timers.Timer t = (System.Timers.Timer)sender;
            string s = Convert.ToString(Convert.ToInt32(t.Interval));
            int i = Convert.ToInt16(s.Substring(s.Length - 2, 2));
            if (clsCheck.inprocess[i])
            {
                //obU.InfoTest("returned as inprocess true for " + i.ToString());
                return;
            }
            ProcessEmailsending(i);
        }

        public void ProcessOBDsending(int i)
        {
            try
            {

                _log.InfoTest2(api_provider);

                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                clsCheck.inprocess[i] = true;

                //Fetch OBD Sending Data from Database

                DataTable dtOBD = obU.GetOBDRecords(no_of_record, sTime);

                if (dtOBD.Rows.Count > 0)
                {
                    clsCheck.inprocess[i] = true;
                    //This method calls obd Method 
                    SendOBD(dtOBD, i);
                }
                else
                {
                    clsCheck.inprocess[i] = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void ProcessWAsending(int i)
        {
            try
            {
                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                clsCheck.inprocess[i] = true;

                //Fetch OBD Sending Data from Database

                DataTable dtWA = obU.GetWARecords(no_of_record, sTime);

                if (dtWA.Rows.Count > 0)
                {
                    clsCheck.inprocess[i] = true;
                    //This method calls obd Method 
                    SendWA(dtWA, i);
                }
                else
                {
                    clsCheck.inprocess[i] = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void ProcessRCSsending(int i)
        {
            try
            {
                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                clsCheck.inprocess[i] = true;

                //Fetch OBD Sending Data from Database

                DataTable dtRCS = obU.GetRCSRecords(no_of_record, sTime);

                if (dtRCS.Rows.Count > 0)
                {
                    clsCheck.inprocess[i] = true;
                    //This method calls obd Method 
                    SendRCS(dtRCS, i);
                }
                else
                {
                    clsCheck.inprocess[i] = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void ProcessEmailsending(int i)
        {
            try
            {
                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                clsCheck.inprocess[i] = true;

                //Fetch OBD Sending Data from Database

                DataTable dtEmail = obU.GetEmailRecords(no_of_record, sTime);

                if (dtEmail.Rows.Count > 0)
                {
                    clsCheck.inprocess[i] = true;
                    //This method calls obd Method 
                    SendEmail(dtEmail, i);
                }
                else
                {
                    clsCheck.inprocess[i] = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task SendOBD(DataTable dtOBD, int i)
        {
            try
            {
                #region <commented for testing >
                forwardobd(dtOBD, i);
                #endregion
                //obU.InfoTest("frm SendMessage for " + i.ToString() + " set inprocess false");
                clsCheck.inprocess[i] = false;
            }
            catch (Exception ex)
            {
                throw;
                //clsCheck.inprocess[i] = false;
            }
        }
        #region < Sending Methods Session Wise with FILE UPLOAD / Normal OBD >

        public async Task SendWA(DataTable dtWA, int i)
        {
            try
            {
                #region <commented for testing >
                forwardWA(dtWA, i);
                #endregion
                //obU.InfoTest("frm SendMessage for " + i.ToString() + " set inprocess false");
                clsCheck.inprocess[i] = false;
            }
            catch (Exception ex)
            {
                throw;
                //clsCheck.inprocess[i] = false;
            }
        }
        public async Task SendRCS(DataTable dtRCS, int i)
        {
            try
            {
                #region <commented for testing >
                forwardRCS(dtRCS, i);
                #endregion
                //obU.InfoTest("frm SendMessage for " + i.ToString() + " set inprocess false");
                clsCheck.inprocess[i] = false;
            }
            catch (Exception ex)
            {
                throw;
                //clsCheck.inprocess[i] = false;
            }
        }
        public async Task SendEmail(DataTable dtEmail, int i)
        {
            try
            {
                #region <commented for testing >
                forwardemail(dtEmail, i);
                #endregion
                //obU.InfoTest("frm SendMessage for " + i.ToString() + " set inprocess false");
                clsCheck.inprocess[i] = false;
            }
            catch (Exception ex)
            {
                throw;
                //clsCheck.inprocess[i] = false;
            }
        }

        //FILE UPLOAD
        public async Task forwardobd(DataTable dtobdMAIN, int i)
        {
            try
            {
                foreach (DataRow dr in dtobdMAIN.Rows)
                {
                    int mobcnt = obU.GetOBDDuplicateCount(dr["TOMOBILE"].ToString(),  Convert.ToString(dr["FILEID"]), Convert.ToString(dr["PROFILEID"]));
                    if (mobcnt <= 0)
                    {
                        if (api_provider == "infobip")
                        {
                            obU.obd_infobip_api(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), apikey, infobip_authkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()), i, Infobip_req_body);
                        }
                        else if (api_provider == "knowlarity")
                        {
                            obU.obd_knowlarityapi(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), apikey, knowalarity_authkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()),i, knowalarity_req_body);
                        }
                    }
                }
                obU.RemoveFromOBDTran();
            }
            catch (Exception EX)
            {

                throw;
            }
        }


        public async Task forwardWA(DataTable dtwaMAIN, int i)
        {
            try
            {
                foreach (DataRow dr in dtwaMAIN.Rows)
                {
                  //  int mobcnt = obU.GetwaDuplicateCount(dr["TOMOBILE"].ToString(), Convert.ToString(dr["FILEID"]), Convert.ToString(dr["PROFILEID"]));
                    //if (mobcnt <= 0)
                    //{
                        DataRow[] dtr = Util.templateparam.Select("templatename='"+ dr["templatename"].ToString() + "'");
                        int cnt = 0;

                        List<string> fieldlist = new List<string>();
                        if (dtr.Length>0)
                        {


                            if (dtr[0]["Val1"].ToString().Trim()!="")
                            {
                                fieldlist.Add(dtr[0]["Val1"].ToString().Trim());
                                cnt = cnt + 1;
                            }
                            if (dtr[0]["Val2"].ToString().Trim() != "")
                            {
                                fieldlist.Add(dtr[0]["Val2"].ToString().Trim());
                                cnt = cnt + 1;

                            }
                            if (dtr[0]["Val3"].ToString().Trim() != "")
                            {
                                fieldlist.Add(dtr[0]["Val3"].ToString().Trim());
                                cnt = cnt + 1;

                            }
                            if (dtr[0]["Val4"].ToString().Trim() != "")
                            {
                                fieldlist.Add(dtr[0]["Val4"].ToString().Trim());
                                cnt = cnt + 1;

                            }
                            if (dtr[0]["Val5"].ToString().Trim() != "")
                            {
                                fieldlist.Add(dtr[0]["Val5"].ToString().Trim());
                                cnt = cnt + 1;

                            }
                        }
                        

                        obU.watemplateapi(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), WAapikey, WAauthkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()), i, dr["templatename"].ToString(), fieldlist);
                        //obU.waapi(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), WAapikey, WAauthkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()), i);
                    //}
                }
                obU.RemoveFromwaTran();
            }
            catch (Exception EX)
            {

                throw;
            }
        }

        public async Task forwardRCS(DataTable dtRCSMAIN, int i)
        {
            try
            {
                foreach (DataRow dr in dtRCSMAIN.Rows)
                {
                    //  int mobcnt = obU.GetwaDuplicateCount(dr["TOMOBILE"].ToString(), Convert.ToString(dr["FILEID"]), Convert.ToString(dr["PROFILEID"]));
                    //if (mobcnt <= 0)
                    //{
                    DataRow[] dtr = Util.templateparam.Select("templatename='" + dr["templatename"].ToString() + "'");
                    int cnt = 0;

                    List<string> fieldlist = new List<string>();
                    if (dtr.Length > 0)
                    {


                        if (dtr[0]["Val1"].ToString().Trim() != "")
                        {
                            fieldlist.Add(dtr[0]["Val1"].ToString().Trim());
                            cnt = cnt + 1;
                        }
                        if (dtr[0]["Val2"].ToString().Trim() != "")
                        {
                            fieldlist.Add(dtr[0]["Val2"].ToString().Trim());
                            cnt = cnt + 1;

                        }
                        if (dtr[0]["Val3"].ToString().Trim() != "")
                        {
                            fieldlist.Add(dtr[0]["Val3"].ToString().Trim());
                            cnt = cnt + 1;

                        }
                        if (dtr[0]["Val4"].ToString().Trim() != "")
                        {
                            fieldlist.Add(dtr[0]["Val4"].ToString().Trim());
                            cnt = cnt + 1;

                        }
                        if (dtr[0]["Val5"].ToString().Trim() != "")
                        {
                            fieldlist.Add(dtr[0]["Val5"].ToString().Trim());
                            cnt = cnt + 1;

                        }
                    }
                    
                    obU.RCSApi(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), "Hello to MIM", "Hello MIM", "https://www.example.com/rcs", "Hello MyInboxMedia", dr["MSGID2"].ToString(), RCSauthkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()), i,fieldlist, dtr[0]["imgurl"].ToString().Trim());
                    //obU.waapi(dr["tomobile"].ToString().Trim(), dr["msgtext"].ToString().Trim(), WAapikey, WAauthkey, dr["profileid"].ToString(), Convert.ToInt32(dr["fileid"].ToString()), i);
                    //}
                }
                obU.RemoveFromRCSTran();
            }
            catch (Exception EX)
            {

                throw;
            }
        }

        public async Task forwardemail(DataTable dtemailmain, int i)
        {
            try
            {
                foreach (DataRow dr in dtemailmain.Rows)
                {
                    int mobcnt = obU.GetemailDuplicateCount(dr["toemailid"].ToString(), Convert.ToString(dr["FILEID"]), Convert.ToString(dr["PROFILEID"]));
                    if (mobcnt <= 0)
                    {

                        DataTable dtsetting = Util.dtsettings;
                        string res =obU.SendEmailSVH(dtsetting.Rows[0]["MailFrom"].ToString(), dtsetting.Rows[0]["MailPWD"].ToString(), dtsetting.Rows[0]["Host"].ToString(), dr["toemailid"].ToString(), "", dr["msgtext"].ToString(), null);
                        if (res.Contains("Successfully"))
                        {

                            string sql = @"insert into [Emailsubmitted](toemail,mailsubject,mailbody,profileid,fileid,cnt,result)
                            select '" + dr["toemailid"].ToString() + "','',N'" + dr["msgtext"].ToString() + "','" + dr["PROFILEID"].ToString() + "','" + dr["FILEID"].ToString() + "' ,"+i+",'" + res + "'";
                            database.ExecuteNonQuery(sql);

                        }
                    }
                }
                obU.RemoveFromemailTran();
            }
            catch (Exception EX)
            {

                throw;
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //api_provider = Convert.ToString(database.GetScalarValue("select obd_api_provider from settings"));

            //Infobip_req_body = System.IO.File.ReadAllText(@"" + Convert.ToString(ConfigurationManager.AppSettings["REQBODY"]) + "infobip.txt");
            //knowalarity_req_body = System.IO.File.ReadAllText(@"" + Convert.ToString(ConfigurationManager.AppSettings["REQBODY"]) + "knowalarity.txt");

            //Util.dtsettings = database.GetDataTable("select * from settings");
            Util.templateparam = database.GetDataTable("select * from WATemplateVar");

            //DataTable dtsetting = Util.dtsettings;
            //obU.SendEmailSVH(dtsetting.Rows[0]["MailFrom"].ToString(), dtsetting.Rows[0]["MailPWD"].ToString(), dtsetting.Rows[0]["Host"].ToString(), "rabi@myinboxmedia.com", "", "Dear Customer Repair of your vehicle is almost done and will be ready in 45 min. Approximate amount of service is  917678936834 Please contact to Rabi Kumar for detail of repair.  Thanks, ACCENT", null);

            Initialize_Session();

            //obU.RCSApi("7678936834", "Hiesfd", "Hello to MIM", "Hello MIM", "https://www.example.com/rcs", "Hello MyInboxMedia", "test324e", RCSauthkey, "345rfs", 0, 0, null);

            //List<string> fieldslis = new List<string>();
            //fieldslis.Add("Rabi Kumar");
            //fieldslis.Add("1000");
            //fieldslis.Add("abc.in");
            //obU.watemplateapi("+917678936834", "hi", WAapikey, WAauthkey, "MiM2000099", 999, 0, "newaccount", fieldslis);

            //obU.obd_infobip_api("7678936834", "OTP Number is 917678936834 for dealer no ACCENT and VIN no Rabi Kumar  Regards,  Hyundai", apikey, infobip_authkey, "sdfc", 99901, 0, Infobip_req_body);
            // obU.obdapi("7678936834", "Hi", "GesxeTJGz52ReWg8UBb8w7fTtqaCy1107E6bNZmG", "a86d7c03-abb4-11e6-982f-066beb27a027", "sdfc", 9999, 0);
            //obU.waapi("917678936834", "Dear test 1 Registration done successfully Your RegNo is 123 thanks for choosing myinboxmedia.com", WAapikey,WAauthkey, "",0,0);
        }

        private void timerapi_provider_Tick(object sender, ElapsedEventArgs e)
        {

            try
            {
                //api_provider = Convert.ToString(database.GetScalarValue("select obd_api_provider from settings"));
                //Util.dtsettings = database.GetDataTable("select * from settings");
                Util.templateparam = database.GetDataTable("select * from WATemplateVar");
            }
            catch (Exception ex)
            {
                _log.Info_Err2("api_providerExecption_" + ex.StackTrace + " - " + ex.Message, 9999);
            }
        }
    }
}

