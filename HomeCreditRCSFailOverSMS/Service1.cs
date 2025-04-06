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
        bool IsRunning = false;
        bool IsRunningDL = false;
        int no_of_timer = 0;
        int no_of_record = Convert.ToInt32(ConfigurationManager.AppSettings["no_of_record"]);

        private System.Timers.Timer[] timerPROCESSRCS;

        private Timer timerRcsFailOver = null;
        private Timer timerDelevery = null;
        private Timer timerPROCESS = null;
        private Timer timerDaySummary = null;
        private Timer timerDashboard = null;

        string RCSauthkey = Convert.ToString(ConfigurationManager.AppSettings["RCSauthkey"]);
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            LogError("Serv Start", "");
            //dtSession = database.GetDataTable("select sessionid,0 cnt from rcssessions");

            // no_of_timer = dtSession.Rows.Count;

            //timerPROCESSRCS = new System.Timers.Timer[(no_of_timer+1)];
            //for (int i = 1; i <= no_of_timer; i++)
            //{
            //    System.Threading.Thread.Sleep(100);
            //    timerPROCESSRCS[i] = new System.Timers.Timer();
            //    timerPROCESSRCS[i].Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_RCSINTERVAL"]) + i ;
            //    timerPROCESSRCS[i].Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESSRCS_Tick);
            //    timerPROCESSRCS[i].Enabled = true;
            //}

           

            //timerPROCESS = new Timer();
            //this.timerPROCESS.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
            //this.timerPROCESS.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_Tick);
            //timerPROCESS.Enabled = true;
            //this.timerPROCESS.Start();

            //timerDelevery = new Timer();
            //this.timerDelevery.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["DELIVERY_INTERVAL"]);
            //this.timerDelevery.Elapsed += new System.Timers.ElapsedEventHandler(this.timerdelivery_Tick);
            //timerDelevery.Enabled = true;
            //this.timerDelevery.Start();

            //timerDaySummary = new Timer();
            //this.timerDaySummary.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["DAYSUMMERY_INTERVAL"]);
            //this.timerDaySummary.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDaySummary_Tick);
            //timerDaySummary.Enabled = true;
            //this.timerDaySummary.Start();

            //timerDashboard = new Timer();
            //this.timerDashboard.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["DASHBOARD_INTERVAL"]);
            //this.timerDashboard.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDashboard_Tick);
            //timerDashboard.Enabled = true;
            //this.timerDashboard.Start();

            timerRcsFailOver = new Timer();
            this.timerRcsFailOver.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FAILOVER_INTERVAL"]);
            this.timerRcsFailOver.Elapsed += new System.Timers.ElapsedEventHandler(this.timerRcsFailOver_Tick);
            timerRcsFailOver.Enabled = true;
            this.timerRcsFailOver.Start();


        }

        protected override void OnStop()
        {
            this.timerPROCESS.Stop();
            this.timerDelevery.Stop();
            this.timerDaySummary.Stop();
            this.timerDashboard.Stop();

            LogError("Serv Stop.", "");
        }

        private void timerPROCESSRCS_Tick(object sender, ElapsedEventArgs e)
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
            //obU.Info_Err3("Session - " + i.ToString());
            ProcessRCSsending(i);
        }

        public void ProcessRCSsending(int i)
        {
            try
            {
               // _log.InfoTest2(api_provider);

                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                clsCheck.inprocess[i] = true;

                //Fetch RCS Sending Data from Database

                DataTable dtRCS = obU.GetRCSRecords(no_of_record, sTime,i);

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
                //throw;
            }
        }
        public async Task SendRCS(DataTable dtRCS, int i)
        {
            try
            {
                #region <commented for testing >
                forwardrcs(dtRCS, i);
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
        public async Task forwardrcs(DataTable dtrcsMAIN, int i)
        {
            try
            {
                foreach (DataRow dr in dtrcsMAIN.Rows)
                {
                    int mobcnt = obU.GetRCSDuplicateCount(dr["TOMOBILE"].ToString(), Convert.ToString(dr["rcsmsgrcvdid"]));
                    if (mobcnt <= 0)
                    {
                        string pUrl = "";

                        string apiurl = dr["rcsurl"].ToString();
                        string authkey = dr["rcsauthkey"].ToString();
                        string acid = dr["rcsaccid"].ToString();

                        string SessionId = dr["SessionId"].ToString().ToUpper();
                        string Msg = dr["MsgText"].ToString();
                        string RId = dr["rcsmsgrcvdid"].ToString();
                        string pUserId = dr["UserId"].ToString();
                        string pCounCode = dr["CountryCode"].ToString();
                        string pMsgType = dr["msgtype"].ToString();
                        string pTempletId = dr["TemplateID"].ToString();
                        string Mob = dr["CountryCode"] + dr["TOMOBILE"].ToString();

                        if (pMsgType == "2")
                        {
                            pUrl = dr["ImageFileURL"].ToString();
                        }
                        if (pMsgType == "3")
                        {
                            pUrl = dr["VedioFileURL"].ToString();
                        }

                        // 1:Text,2:Image,3:video,4:Card,5:Carousal

                            if (pMsgType == "1")
                            {
                                obU.RCSApiAN(RId, Mob, authkey, Msg, pUserId, SessionId, acid, apiurl, pTempletId);//Text
                            }

                            if (pMsgType == "2" || pMsgType == "3") //Image/Video
                            {
                                obU.rcsimgAN(RId, Mob, authkey, Msg, pUrl, pUserId,SessionId, acid, apiurl);
                            }

                            if (pMsgType == "4")
                            {
                                obU.RCSCard(RId, Mob, authkey, Msg, pTempletId, pUserId, SessionId, acid, apiurl);//Card
                            }

                            if (pMsgType == "5")
                            {
                                obU.RCSCarousalAN(RId, Mob, authkey, Msg, pTempletId, pUserId,SessionId, acid, apiurl);//Carousal
                            }
                       
                    }
                }
                obU.RemoveFromRCSTran(i);
            }
            catch (Exception EX)
            {

                LogError("PROCESSTimer_"+i.ToString() + EX.StackTrace, EX.Message);
            }
        }
       
        private void timerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                SubmitToTran();
               // ProcessRCS();
            }
            catch (Exception ex)
            {
                LogError("SubmitToTranTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerdelivery_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessDelivery();
            }
            catch (Exception ex)
            {
                LogError("PROCESSTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerDaySummary_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                string datetime = DateTime.Now.ToString("hh:mm tt");
                string Sdatetime = ConfigurationManager.AppSettings["DaySimmeryTime"].ToString();
                if (datetime==Sdatetime)
                { 
                    ProcessDaySummary();
                    LogError("", "DaySummary_Called");

                }
            }
            catch (Exception ex)
            {
                LogError("PROCESSTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerDashboard_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessDashboard();
            }
            catch (Exception ex)
            {
                LogError("PROCESSTimer_" + ex.StackTrace, ex.Message);
            }
        }

        private void timerRcsFailOver_Tick(object sender, ElapsedEventArgs e)
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

        private void ProcessDelivery()
        {

            if (!IsRunningDL)
            {
                IsRunningDL = true;
                string sql = "";
                string Mob = "";
                Util obU = new Util();

                //Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from tblRCSMSGRCVD WHERE  RCSSend is NULL; "));

                string strsq = "select d.msgid,s.messageId from tblRCSMSGSUBMITTED S with(nolock) left outer join[dbo].[tblRCSMSGDELIVERY] D with(nolock) on rtrim(ltrim(s.messageId)) = rtrim(ltrim(D.msgid)) where d.msgid is null and s.messageId is not null ; ";
                DataTable dt1 = database.GetDataTable(strsq);

                if (dt1.Rows.Count > 0)
                {

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string sqlstr = "insert into tblRCSMSGDELIVERY(bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime) " +
                                         " select bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime from  [10.10.31.35].[smppmain_tx].dbo.DELIVERYRCS SD where SD.MSGID = '" + dt1.Rows[i]["messageId"].ToString() + "';";
                        database.ExecuteNonQuery(sqlstr);
                    }

                }
                
                string sqlseen = @"insert into  RCSSeenRpt(MSGID,fromMob,toMob,sentAt,seenAt,inserttime)
                                    select s1.MSGID,s1.fromMob,s1.toMob,s1.sentAt,s1.seenAt,s1.inserttime 
                                    from tblRCSMSGDELIVERY d WITH (NOLOCK)
                                    join [10.10.31.35].[smppmain_tx].dbo.RCSSeenRpt s1 WITH (NOLOCK) on s1.msgid=d.MSGID
                                    left join [dbo].[RCSSeenRpt] s WITH (NOLOCK) on d.MSGID=s.MSGID where s.msgid is null";
                database.ExecuteNonQuery(sqlseen);

                IsRunningDL = false;
            }

        }

        private void ProcessDaySummary()
        {
            string sql = "";
            string Mob = "";
            Util obU = new Util();

            //Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from tblRCSMSGRCVD  with(nolock) WHERE  RCSSend is NULL; "));

            string strsq = @"select * from tblRCSMSGDAYSUMMARY s where  convert(date, s.rcsdate)= convert(date, GETDATE()-1) ";
            DataTable dt1 = database.GetDataTable(strsq);
            if (dt1.Rows.Count <= 0)
            {

 string  sqlstr = @"insert into tblRCSMSGDAYSUMMARY(RCSDATE,USERID,SUBMITTED,REJECTED,DELIVERED,FAILED,UNKNOWN,SEEN)
select  convert(date,r.CreatedDate) CreatedDate , c.username,   
  count(*) as SUBMITTED,  
 sum(case when isnull(s.sentstatus, '') = 'OK' then 0 else 1 end) as REJECTED,  
 sum(case when isnull(d.status_groupName, '') = 'DELIVERED' then 1 else 0 end) as DELIVERED,  
 sum(case when isnull(d.status_groupName, '') <> 'DELIVERED'  AND d.status_groupName IS NOT NULL then 1 else 0 end) as FAILED,
 sum(case when isnull(s.sentstatus, '') = 'OK' and d.status_groupName IS NULL then 1 else 0 end) as UNKNOWN ,
 sum(case when seen.seenAt IS NOT NULL then 1 else 0 end) as SEEN 
 from CUSTOMER c   
  inner join tblRCSMSGRCVD r with (nolock) on c.username=r.UserId  
  inner join tblRCSMSGSUBMITTED s with (nolock) on r.Id=s.RcsMsgRcvdId  
  left outer join dbo.tblRCSMSGDELIVERY d with (nolock) on s.messageId=d.MSGID  
  left	join dbo.RCSSeenRpt seen with (nolock) on seen.MSGID=s.messageId   
  where convert(date, r.CreatedDate )=convert(date, DATEADD(day,-1,getdate())) group by  convert(date,r.CreatedDate)  , c.username ";

                database.ExecuteNonQuery(sqlstr);
                
            }
        }

        private void ProcessDashboard()
        {
            
            Util obU = new Util();
            obU.UpdateDashboardData();
           
        }

        private void SubmitToTran()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                try
                {
                    string sql = @"select datatablename,noofrecds,id,rcsaccid,rcsauthkey,RCSURL from tblrcsmsgrcvd r with(nolock) join customer c on c.username=r.userid where pickedtime is null order by createddate";
                    DataTable dt = database.GetDataTable(sql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        sql = " Update tblRCSMSGRCVD set PickedTime= getdate() where id='" + dr["id"].ToString() + "';  ";
                        database.ExecuteNonQuery(sql);

                        DataTable dtc = dtSession;
                        int NoOfRecds = Convert.ToInt32(Convert.ToString(dr["NoOfRecds"]));
                        int noofsession = dtc.Rows.Count;

                        if (NoOfRecds > noofsession)
                        {
                            int cnt = (NoOfRecds / noofsession);
                            int rem = (NoOfRecds % noofsession);

                            for (int i = 0; i < dtc.Rows.Count; i++)
                            {
                                dtc.Rows[i]["cnt"] = cnt.ToString();
                            }
                            for (int j = 0; j < rem; j++)
                            {
                                dtc.Rows[j]["cnt"] = Convert.ToString(Convert.ToInt32(dtc.Rows[j]["cnt"]) + 1);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < NoOfRecds; j++)
                            {
                                dtc.Rows[j]["cnt"] = 1;
                            }
                        }
                        for (int i = 0; i < dtc.Rows.Count; i++)
                        {
                            database.ExecuteNonQuery("IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = N'SESSIONID' AND Object_ID = Object_ID(N'" + dr["DataTableName"] + "')) " +
                            " ALTER TABLE " + dr["DataTableName"] + @" add SessionId numeric(10) ; "); //add new colum
                            sql = " update top (" + dtc.Rows[i]["cnt"].ToString() + ") " + dr["DataTableName"] + " set SessionId = '" + dtc.Rows[i]["SessionId"].ToString() + "' where isnull(SessionId,0)=0  ";
                            database.ExecuteNonQuery(sql);
                        }

                        sql = @"INSERT INTO rcstran(UserId,rcsmsgrcvdid,CountryCode,ToMobile,MsgType,MsgText,ImageFileURL,VedioFileURL,TemplateID,CreatedDate,SessionId,rcsaccid,rcsauthkey,RCSURL)
                        select UserId,tbl.id,CountryCode,tmp.MobNo,MsgType,MsgText,ImageFileURL,VedioFileURL,TemplateID,CreatedDate,SessionId,'" + dr["rcsaccid"] + "','" + dr["rcsauthkey"] + @"','" + dr["RCSURL"] + @"'
                        from " + dr["DataTableName"] + " tmp join tblRCSMSGRCVD tbl on '" + dr["DataTableName"] + "'=tbl.DataTableName ";
                        database.ExecuteNonQuery(sql);
                    }
                }
                catch (Exception EX)
                {
                    LogError("PROCESSTimer_" + EX.StackTrace, EX.Message);
                }
                IsRunning = false;
            }
        }
        
        public void Debug()
        {
            obU.FetchfailOverRecord();
            //string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //Fetch RCS Sending Data from Database

            //DataTable dtRCS = obU.GetRCSRecords(10000, sTime, 19);

            //forwardrcs(dtRCS, 19);


            //obU.FetchfailOverRecord();
            //string RCSauthkey = Convert.ToString(ConfigurationManager.AppSettings["RCSauthkey"]);

            //DataTable dt = new DataTable();
            //dt = database.GetDataTable("select * from CUSTOMER where username='MIM2200034'");
            //DataRow dr = dt.Rows[0];
            //string apiurl = dr["rcsurl"].ToString();
            //string authkey = dr["rcsauthkey"].ToString();
            //string acid = dr["rcsaccid"].ToString();

            //string msgtext = Convert.ToString(database.GetScalarValue("select MsgText from tblRCSMSGRCVD with (nolock) where id=586 order by CreatedDate desc"));

            //    string datetime = DateTime.Now.ToString("hh:mm tt");
            //    string Sdatetime = ConfigurationManager.AppSettings["DaySimmeryTime"].ToString();
            //obU.RCSCarousalAN_test("0", "917678936834", RCSauthkey, "AOV", "10258", "test", "0", "myinbox", "https://lz6q85.api.infobip.com/ott/rcs/1/message");

            //    string txt = Convert.ToString(database.GetScalarValue("select MsgText from tblRCSMSGRCVD  with (nolock) where id=258"));
            //obU.rcsimgAN("0", "917678936834", authkey, "efddsf", "http://103.205.64.220:17250/rcs/MEDIAUpload/20220510014445463.mp4", "", "0", acid,apiurl, "10512");
            // obU.RCSCard("0", "917678936834", RCSauthkey, "Hi test Card", "10276", "MIM2200034", "0", "myinbox", "https://lz6q85.api.infobip.com/ott/rcs/1/message");
            //string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //Fetch RCS Sending Data from Database

            //DataTable dtRCS = obU.GetRCSRecords(20, sTime, 1);
            //dtSession = database.GetDataTable("select sessionid,0 cnt from rcssessions");
            //SubmitToTran();
            //forwardrcs(dtRCS, 0);
            //ProcessSchedule();
            //ProcessDelivery();
            //ProcessDaySummary();
            //ProcessDashboard();
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

    }
}
