using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Inetlab.SMPP;
using Inetlab.SMPP.Builders;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using System.Timers;
using System.Configuration;

namespace API_DLR_RECEIPT
{
    public partial class Form1 : Form
    {
        int no_of_sms = Convert.ToInt32(ConfigurationManager.AppSettings["NO_OF_SMS"]);
        string gsmSysID= Convert.ToString(ConfigurationManager.AppSettings["GSMSYSID"]).ToUpper();
        public string dlrCallBack = Convert.ToString(ConfigurationManager.AppSettings["DLR_CALLBACK_APPLICABLE"]);
        //private readonly SmppClient[] _client;
        private SmppClient[] _client;
        private System.Timers.Timer timerPROCESS;
        private System.Timers.Timer timerDLRReceivedCheck;
        private MessageComposer[] _messageComposer;

        bool bProcessDeliveryQ = false;
        bool bDLRReceivedCheck = false;
        internal BackgroundWorker[] worker;

        internal BackgroundWorker workerSMSSend;

        internal bool[] m_IsConnected;
        internal bool[] m_ConnectedSucceed;
        internal bool[] m_ConnectionInProcess;
        DataTable dt;
        Util _log = new Util();
        Util obU = new Util();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _log.Info("Service started.","0");

            // C#
            #region <Licence >
            string licenseContent = @"
-----BEGIN INETLAB LICENSE------
O4TP3GKNGFSNQCAMNIBCIVDFMNUG433I
NF3GKICTN5WHK5DJN5XHGICQOJUXMYLU
MUQEY2LNNF2GKZADDV2GK23ON5UGS5TF
FZZW63DVORUW63TTIBTW2YLJNQXGG33N
AQIXG2DBOJSWS5BNGY4DANJSGMZDSMYF
QDQDZI7NMPMAQBUAUABRZP4C3EEIAALP
Y6C7AKTR64LUPXOB5LYEJ55W5SCUZJHP
YRDJGALKWBY5IZHZKGGPM5V23FXWXKJ4
KHE4QNIKJGZU76DSPS6KM6TKB56B3R4E
BKXEC6OB3WZDKQUT2TAEQA75VD2EDAZB
D4BVOXGUFK2JUVVCAXV2FGLNO5XYM25Z
EAXSNVCRTU7Z6HQLK4KS5JIX2QWH3MZI
25PHPAIB7MCQ====
-----END INETLAB LICENSE--------";
            #endregion

            #region < Licence 2.9.9 >
            licenseContent = @"-----BEGIN INETLAB LICENSE------
O5VYVIFW3YUNSCAMPAFAYSLOMV2GYYLC
FZJU2UCQAISFIZLDNBXG62DJOZSSAU3P
NR2XI2LPNZZSAUDSNF3GC5DFEBGGS3LJ
ORSWIAY5ORSWW3TPNBUXMZJOONXWY5LU
NFXW442AM5WWC2LMFZRW63IECFZWQYLS
MVUXILJWHAYDKMRTGI4TGBMA4A6KH3LD
3AEANAFAAMOL7AWZBCAACRRPS3OPJDRU
NBGQBR64NSQ5HTVM3O6NYMPAKDHJQZ6J
UPDOQ2WAQAG7LLASKNSFTAMZ4OLOFBXE
JDRT7YOGQYGTAZFJRWZZ3MUTLR67XPFK
OT3HBKF6NSCNAZ4JQUUGBVRDCOXGKRTS
BKLQ2HFP24WCNNNLGMK6JHFAK4NYRDBO
XU67JUWNEAD3HDEDUZOO7LZUJPH3GPLO
SE======
-----END INETLAB LICENSE--------";
            #endregion
            Inetlab.SMPP.LicenseManager.SetLicense(licenseContent);
            //Inetlab.SMPP.LicenseManager.SetLicense(this.GetType().Assembly.GetManifestResourceStream(this.GetType(), "Inetlab.SMPP.license" ));

            Util.tblerror = database.GetDataTable("select * from errorcodeTemplate with (nolock)");
            
            //Get DLRCallBackCustomers
            Util.dtDLRCallBackCust = obU.GetDLRCallBackCustomers();

            Initialize_Client();

            timerPROCESS = new System.Timers.Timer();
            timerPROCESS.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
            timerPROCESS.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_Tick);
            timerPROCESS.Enabled = true;

            try
            {
                timerDLRReceivedCheck = new System.Timers.Timer();
                timerDLRReceivedCheck.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL_DLRReceivedCheck"]);
                timerDLRReceivedCheck.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDLRReceivedCheck_Tick);
                timerDLRReceivedCheck.Enabled = true;
            }
            catch (Exception ex) { }
            //ProcessSMSsending();
            //timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
            //timer1.Enabled = true;
        }

        public void ProcessSMSsending(int i)
        {
          
        }

        //private void workerSMSSend_DoWork(object sender, DoWorkEventArgs workEventArgs)
        //{
        //    DataTable dtSMS = new DataTable();
        //    dtSMS = (DataTable)(workEventArgs.Argument);
        //    int i = Convert.ToInt16(dtSMS.TableName);
        //    SendMessage(dtSMS, i);
        //}
        //private void workerSMSSend_ProgressChanged(object sender, ProgressChangedEventArgs progressEventArgs)
        //{

        //}

        //private void workerSMSSend_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs connectEventArgs)
        //{

        //}

        public async void SendMessage(DataTable dtSMS, int i)
        {
            
        }

        public void AddInMsgSubmitted2(DataTable dtSMS, DateTime sendTime)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
            {
                Random a = new Random();
                int x = a.Next(1, 32000);
                string s = Convert.ToString(new Guid());
                obU.AddInMsgSubmitted(DateTime.Now.ToString("ddMMyyyyHHmmssfffffff") + x.ToString() + s, dtSMS.Rows[i], sendTime);
            }
            obU.RemoveFromMsgTran(dtSMS.Rows[0]["SMPPACCOUNTID"].ToString());
        }

       
        public void AddInMsgSubmittedInvalidSender(DataTable dtSMS, DateTime sendTime)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
            {
                string segment = Guid.NewGuid().ToString();
                obU.AddInMsgSubmittedInvalidSender(segment, dtSMS.Rows[i], sendTime);
            }
            //obU.RemoveFromMsgTran(dtSMS.Rows[0]["SMPPACCOUNTID"].ToString());
        }

        public void AddInMsgSubmitted(string[] strResp, DataTable dtSMS, DateTime sendTime)
        {
            //System.Threading.Thread t = new System.Threading.Thread(() =>
            //{
            InsertInMsgSubmitted(strResp, dtSMS, sendTime);
            //});
            //t.Start();

            //obU.RemoveFromMsgTran(dtSMS.Rows[0]["SMPPACCOUNTID"].ToString());
        }

        public void InsertInMsgSubmitted(string[] strResp, DataTable dtSMS, DateTime sendTime)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
                obU.AddInMsgSubmitted(strResp[i], dtSMS.Rows[i], sendTime);
        }

        public void Initialize_Client()
        {

            try { if (Convert.ToString(ConfigurationManager.AppSettings["DLR_CALLBACK_APPLICABLE"])=="Y") clsCheck.dlrCallBackApplicable = true; } catch (Exception e2) { }
            clsCheck.dtDLRPushAPI = obU.getCallBackAPICustomers();
            dt = obU.GetSMPPAccountsAPI();
            if (dt.Rows.Count > 0)
            {
                _client = new SmppClient[dt.Rows.Count];
                _messageComposer = new MessageComposer[dt.Rows.Count];
                worker = new BackgroundWorker[dt.Rows.Count];

                m_IsConnected = new bool[dt.Rows.Count];
                m_ConnectedSucceed = new bool[dt.Rows.Count];
                m_ConnectionInProcess = new bool[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    clsCheck.prodDt[i] = DateTime.Now;
                    worker[i] = new BackgroundWorker();
                    worker[i].DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker[i].ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                    worker[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

                    _client[i] = new SmppClient();
                    _client[i].ResponseTimeout = TimeSpan.FromSeconds(60);
                    _client[i].EnquireLinkInterval = TimeSpan.FromSeconds(20);

                    _client[i].evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                    _client[i].evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                    _client[i].evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                    _client[i].evUnBind += new UnBindEventHandler(client_evUnBind);
                    _client[i].evDataSm += new DataSmEventHandler(client_evDataSm);
                    _client[i].evRecoverySucceeded += ClientOnRecoverySucceeded;

                    _client[i].evServerCertificateValidation += OnCertificateValidation;

                    _messageComposer[i] = new MessageComposer();
                    _messageComposer[i].evFullMessageReceived += OnFullMessageReceived;
                    _messageComposer[i].evFullMessageTimeout += OnFullMessageTimeout;

                    DataRow dr = dt.Rows[i];

                    _client[i].Name = Convert.ToString(dr["SMPPACCOUNTID"]);

                    worker[i].RunWorkerAsync(i + "$" + "CONNECT" + "$" +     // 0, 1
                        Convert.ToString(dr["SMPPACCOUNTID"]) + "$" +   //2
                        Convert.ToString(dr["PROVIDER"]) + "$" +        //3
                        Convert.ToString(dr["ACCOUNTTYPE"]) + "$" +     //4
                        Convert.ToString(dr["HOSTNAME"]) + "$" +        //5
                        Convert.ToString(dr["PORT"]) + "$" +            //6
                        Convert.ToString(dr["USESSL"]) + "$" +          //7
                        Convert.ToString(dr["SYSTEMID"]) + "$" +        //8
                        Convert.ToString(dr["PASSWORD"]) + "$" +        //9
                        Convert.ToString(dr["BINDINGMODE"]) + "$" +     //10
                        Convert.ToString(dr["SYSTEMTYPE"]) + "$" +      //11
                        Convert.ToString(dr["ADDRESS_TON"]) + "$" +     //12
                        Convert.ToString(dr["ADDRESS_NPI"]) + "$" +     //13
                        Convert.ToString(dr["SOURCE_ADDRESS"]) + "$" +  //14
                        Convert.ToString(dr["TON_S"]) + "$" +           //15
                        Convert.ToString(dr["NPI_S"]) + "$" +           //16
                        Convert.ToString(dr["SERVICE"]) + "$" +         //17
                        Convert.ToString(dr["DESTNATION_ADDRESS"]) + "$" + //18
                        Convert.ToString(dr["TON_D"]) + "$" +           //19
                        Convert.ToString(dr["NPI_D"]) + "$" +           //20
                        Convert.ToString(dr["DATACODING"]) + "$" +      //21
                        Convert.ToString(dr["MODE"])                    //22
                        );

                    DataGridViewRow dr2 = (DataGridViewRow)grid.Rows[0].Clone();
                    dr2.Cells[0].Value = (i + 1).ToString();
                    dr2.Cells[1].Value = Convert.ToString(dr["PROVIDER"]) + " - " + Convert.ToString(dr["SMPPACCOUNTID"]);
                    dr2.Cells[2].Value = Convert.ToString(dr["SYSTEMID"]) + " - " + Convert.ToString(dr["HOSTNAME"]);
                    dr2.Cells[3].Value = "Disconnected";
                    grid.Rows.Insert(0, dr2);

                    System.Threading.Thread.Sleep(4000);
                    
                }
            }
        }

        private void timerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (bProcessDeliveryQ) return;
                bProcessDeliveryQ = true;
                ProcessDLRQ();
                bProcessDeliveryQ = false;
            }
            catch (Exception ex)
            {
                bProcessDeliveryQ = false;
                new Util().LogDlvError("Failed to process DLRQ. " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void ProcessDLRQ()
        {
            int DND2DLRPER = Convert.ToInt32(ConfigurationManager.AppSettings["DND2DLRPER"]);
            string curtime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            string sql = @"With cte As
                    ( SELECT * from DeliveryQueue where PickedDate is null )
                    UPDATE cte set PickedDate='" + curtime + @"' where PickedDate is null; 
                    Select count(*) from DeliveryQueue where PickedDate ='" + curtime + @"' ";

            Int16 cnt = Convert.ToInt16( database.GetScalarValue(sql));
            int totdlr = (DND2DLRPER * cnt) / 100;

            sql = @"Update top (" + totdlr.ToString() + ") DeliveryQueue set DLR=1 where PickedDate ='" + curtime + @"' ;
                Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,delme) 
               select 'id:' + t.msgid + ' sub:001 dlvrd:001 submit date:' + RIGHT(cONVERT(VARCHAR,max(t.dlvrtime),112),6) + REPLACE(cONVERT(VARCHAR,max(t.dlvrtime),108),':','') + 
 ' done date:' + RIGHT(cONVERT(VARCHAR, dateadd(MINUTE,0,max(t.dlvrtime)), 112), 6) + REPLACE(cONVERT(VARCHAR, dateadd(MINUTE,0,max(t.dlvrtime)), 108), ':', '') + 
 ' stat:DELIVRD err:000 text:' AS DLVRTEXT, T.MSGID, dateadd(MINUTE,0,max(t.dlvrtime)),dateadd(MINUTE,0,max(t.dlvrtime)), 'Delivered','000','1' 
FROM DeliveryQueue t where PickedDate ='" + curtime + @"' and DLR=1 group by t.msgid ;

 Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) 
                select DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code FROM DeliveryQueue t where PickedDate ='" + curtime + @"' and DLR=0 ; 
Delete from DeliveryQueue where PickedDate ='" + curtime + @"' ";

            database.ExecuteNonQuery(sql);
        }

        private string[] GetArg(int i)
        {
            DataRow dr = dt.Rows[i];
            string s = i.ToString() + "$" + "CONNECT" + "$" +     // 0, 1
                Convert.ToString(dr["SMPPACCOUNTID"]) + "$" +   //2
                Convert.ToString(dr["PROVIDER"]) + "$" +        //3
                Convert.ToString(dr["ACCOUNTTYPE"]) + "$" +     //4
                Convert.ToString(dr["HOSTNAME"]) + "$" +        //5
                Convert.ToString(dr["PORT"]) + "$" +            //6
                Convert.ToString(dr["USESSL"]) + "$" +          //7
                Convert.ToString(dr["SYSTEMID"]) + "$" +        //8
                Convert.ToString(dr["PASSWORD"]) + "$" +        //9
                Convert.ToString(dr["BINDINGMODE"]) + "$" +     //10
                Convert.ToString(dr["SYSTEMTYPE"]) + "$" +      //11
                Convert.ToString(dr["ADDRESS_TON"]) + "$" +     //12
                Convert.ToString(dr["ADDRESS_NPI"]) + "$" +     //13
                Convert.ToString(dr["SOURCE_ADDRESS"]) + "$" +  //14
                Convert.ToString(dr["TON_S"]) + "$" +           //15
                Convert.ToString(dr["NPI_S"]) + "$" +           //16
                Convert.ToString(dr["SERVICE"]) + "$" +         //17
                Convert.ToString(dr["DESTNATION_ADDRESS"]) + "$" + //18
                Convert.ToString(dr["TON_D"]) + "$" +           //19
                Convert.ToString(dr["NPI_D"]) + "$" +           //20
                Convert.ToString(dr["DATACODING"]) + "$" +      //21
                Convert.ToString(dr["MODE"]);
            string[] arg = s.Split('$');
            return arg;
        }

        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private async void worker_DoWork(object sender, DoWorkEventArgs workEventArgs)
        {
            string[] arg = Convert.ToString(workEventArgs.Argument).Split('$');
            int i = Convert.ToInt16(arg[0]);
            if (arg[1] == "CONNECT")
            {
                await Connect(arg);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs progressEventArgs)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs connectEventArgs)
        {

        }

        private async Task Connect(string[] arg)
        {
            int i = Convert.ToInt16(arg[0]);
            try
            {
                

                if (_client[i].Status == ConnectionStatus.Closed)
                {
                    _log.Info("Connecting to " + arg[5], i.ToString());

                    //bConnect.Enabled = false;
                    //bDisconnect.Enabled = false;
                    //cbReconnect.Enabled = false;

                    //_client[i].EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte(tbAddrTon.Text), (AddressNPI)Convert.ToByte(tbAddrNpi.Text));
                    _client[i].EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte(arg[12]), (AddressNPI)Convert.ToByte(arg[13]));
                    _client[i].SystemType = arg[11]; // tbSystemType.Text;

                    _client[i].ConnectionRecovery = true; // cbReconnect.Checked;
                    _client[i].ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);


                    if (arg[7] == "1" || arg[7].ToUpper() == "TRUE") // cbSSL.Checked)
                    {
                        _client[i].EnabledSslProtocols = SslProtocols.Default;
                        _client[i].ClientCertificates.Clear();
                        _client[i].ClientCertificates.Add(new X509Certificate2("client.p12", "12345"));
                    }
                    else
                    {
                        _client[i].EnabledSslProtocols = SslProtocols.None;
                    }

                    bool bSuccess = await _client[i].ConnectAsync(arg[5], Convert.ToInt32(arg[6]));

                    if (bSuccess)
                    {
                        _log.Info("SmppClient connected " + arg[8], i.ToString());

                        await Bind(arg);
                        for (int m = 0; m < grid.Rows.Count; m++)
                            if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                                grid.Rows[m].Cells[3].Value = "Connected";
                    }
                    else
                    {
                        _log.Info("Not Connected .. " + arg[8], i.ToString());
                        for (int m = 0; m < grid.Rows.Count; m++)
                            if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                                grid.Rows[m].Cells[3].Value = "DisConnected";
                        //bConnect.Enabled = true;
                        //cbReconnect.Enabled = true;
                        //bDisconnect.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info("err on connect " + arg[5] + ". " + ex.Message + ". - " + ex.StackTrace, i.ToString());

            }

        }

        private async Task Bind(string[] arg)
        {
            
            int i = Convert.ToInt16(arg[0]);
            _log.Info("Bind client with SystemId: " + arg[8], i.ToString());
            ConnectionMode mode = ConnectionMode.Receiver;


            //bDisconnect.Enabled = true;
            //mode =  (ConnectionMode)cbBindingMode.SelectedItem;
            object o = arg[10];
            mode = ConnectionMode.Receiver;

            BindResp resp = await _client[i].BindAsync(arg[8], arg[9], mode);

            switch (resp.Header.Status)
            {
                case CommandStatus.ESME_ROK:
                    _log.Info("Bind succeeded: Status: " + resp.Header.Status + ", SystemId: " + resp.SystemId, i.ToString());
                    //bSubmit.Enabled = true;
                    break;
                default:
                    _log.Info("Bind failed: Status: " + resp.Header.Status, i.ToString());
                    //await Disconnect(arg);
                    break;
            }
        }

        private async Task Disconnect(string[] arg)
        {

            int i = Convert.ToInt16(arg[0]);
            _log.Info("Disconnect from SMPP server " + arg[5], i.ToString());

            if (_client[i].Status == ConnectionStatus.Bound)
            {
                await UnBind(arg);
            }

            if (_client[i].Status == ConnectionStatus.Open)
            {
                //await _client[i].DisconnectAsync();
            }
        }

        private async void client_evDisconnected(object sender)
        {

            _log.Info("SmppClient disconnected","0");

            SmppClient client = (SmppClient)sender;
            
            string s = client.SystemID;
            int i = -1;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["SYSTEMID"].ToString() == s && client.Name == dt.Rows[j]["SMPPACCOUNTID"].ToString())
                { i = j; break; }
            }
            if (i >= 0)
            {
                DataRow[] dr = dt.Select("SYSTEMID = '" + s + "' AND SMPPACCOUNTID='" + client.Name + "'");
                string[] arg = (
                i.ToString() + "$" + "CONNECT" +      // 0, 1
                        Convert.ToString(dr[0]["SMPPACCOUNTID"]) + "$" +   //2
                        Convert.ToString(dr[0]["PROVIDER"]) + "$" +        //3
                        Convert.ToString(dr[0]["ACCOUNTTYPE"]) + "$" +     //4
                        Convert.ToString(dr[0]["HOSTNAME"]) + "$" +        //5
                        Convert.ToString(dr[0]["PORT"]) + "$" +            //6
                        Convert.ToString(dr[0]["USESSL"]) + "$" +          //7
                        Convert.ToString(dr[0]["SYSTEMID"]) + "$" +        //8
                        Convert.ToString(dr[0]["PASSWORD"]) + "$" +        //9
                        Convert.ToString(dr[0]["BINDINGMODE"]) + "$" +     //10
                        Convert.ToString(dr[0]["SYSTEMTYPE"]) + "$" +      //11
                        Convert.ToString(dr[0]["ADDRESS_TON"]) + "$" +     //12
                        Convert.ToString(dr[0]["ADDRESS_NPI"]) + "$" +     //13
                        Convert.ToString(dr[0]["SOURCE_ADDRESS"]) + "$" +  //14
                        Convert.ToString(dr[0]["TON_S"]) + "$" +           //15
                        Convert.ToString(dr[0]["NPI_S"]) + "$" +           //16
                        Convert.ToString(dr[0]["SERVICE"]) + "$" +         //17
                        Convert.ToString(dr[0]["DESTNATION_ADDRESS"]) + "$" + //18
                        Convert.ToString(dr[0]["TON_D"]) + "$" +           //19
                        Convert.ToString(dr[0]["NPI_D"]) + "$" +           //20
                        Convert.ToString(dr[0]["DATACODING"]) + "$" +      //21
                        Convert.ToString(dr[0]["MODE"])).Split('$');

                await Disconnect(arg);

                for (int m = 0; m < grid.Rows.Count; m++)
                    if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                        grid.Rows[m].Cells[3].Value = "DisConnected";

                await Connect(arg);
            }


            //Sync(this, () =>
            //{
            //    bConnect.Enabled = true;
            //    bDisconnect.Enabled = false;
            //    bSubmit.Enabled = false;
            //    cbReconnect.Enabled = true;
            //});

        }

        
        private void ClientOnRecoverySucceeded(object sender, BindResp data)
        {
            try
            {
                SmppClient client = (SmppClient)sender;
                string s = client.SystemID;
                string n = client.Name;
                _log.Info("Connection has been recovered. " + n + " - " + s,"0");
            }
            catch (Exception ex) { }
        }


        private async Task UnBind(string[] arg)
        {
            int i = Convert.ToInt16(arg[0]);
            _log.Info("Unbind SmppClient " + arg[5], i.ToString());
            UnBindResp resp = await _client[i].UnbindAsync();

            switch (resp.Header.Status)
            {
                case CommandStatus.ESME_ROK:
                    _log.Info("UnBind succeeded: Status: " + resp.Header.Status, i.ToString());
                    break;
                default:
                    _log.Info("UnBind failed: Status: " + resp.Header.Status, i.ToString());
                    //await _client[i].DisconnectAsync();
                    break;
            }

        }


        private void client_evDeliverSm(object sender, DeliverSm data)
        {
            try
            {
                //Check if we received Delivery Receipt
                if (data.MessageType == MessageTypes.SMSCDeliveryReceipt)
                {
                    //Get MessageId of delivered message
                    string messageId = data.Receipt.MessageId;
                    DateTime donedate = data.Receipt.DoneDate;
                    string deliveryStatus = Convert.ToString(data.Receipt.State);
                    string destno = Convert.ToString(data.SourceAddress.Address);
                    string sid = Convert.ToString(data.DestinationAddress.Address);
                    string errcd = Convert.ToString(data.Receipt.ErrorCode);
                    string param1_Tag;
                    string param1_val;
                    string param1_val2;
                    string MCCMNC = "";
                    if (data.Parameters.Count > 0)
                        for (int q = 0; q < data.Parameters.Count; q++)
                        {
                            param1_Tag = "";
                            param1_val = "";
                            param1_Tag = Convert.ToString(data.Parameters[q].TagValue);
                            param1_val = Encoding.UTF8.GetString(data.Parameters[q].Value, 0, data.Parameters[q].Value.Length);
                            param1_val2 = Encoding.UTF8.GetString(data.Parameters[q].Value, 0, data.Parameters[q].Value.Length-1);
                            if (param1_Tag.ToUpper().Trim() == "RECEIPTEDMESSAGEID" ) messageId = param1_val2;
                            if (param1_Tag == "5142") { MCCMNC = param1_val; break; }
                        }
                    Util ob = new Util();

                    if (data.Client.SystemID.ToUpper().Contains(gsmSysID) && errcd == "002")
                    {
                        System.Threading.Thread t1 = new System.Threading.Thread(() =>
                        {
                            ob.UpdateDeliveryQueue(messageId, donedate, deliveryStatus, data.Receipt.ToString(), errcd, MCCMNC, sid, destno);
                        });
                        t1.Start();
                    }
                    else
                    {
                        System.Threading.Thread t = new System.Threading.Thread(() =>
                        {
                            ob.UpdateDelivery(messageId, donedate, deliveryStatus, data.Receipt.ToString(), errcd, MCCMNC, sid, destno, dlrCallBack);
                        });
                        t.Start();
                    }

                    //_log.Info("Delivery Receipt received: " + data.Receipt.ToString());

                }
                else
                {
                    #region < Commented
                    /*
                    // Receive incoming message and try to concatenate all parts
                    if (data.Concatenation != null)
                    {
                        _messageComposer.AddMessage(data);

                        _log.Info("DeliverSm part received: Sequence: {0}, SourceAddress: {1}, Concatenation ( {2} )" +
                                " Coding: {3}, Text: {4}",
                                data.Header.Sequence, data.SourceAddress, data.Concatenation, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                    }
                    else
                    {
                        _log.Info("DeliverSm received : Sequence: {0}, SourceAddress: {1}, Coding: {2}, Text: {3}",
                            data.Header.Sequence, data.SourceAddress, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                    }

                    // Check if an ESME acknowledgement is required
                    if (data.Acknowledgement != SMEAcknowledgement.NotRequested)
                    {
                        // You have to clarify with SMSC support what kind of information they request in ESME acknowledgement.

                        string messageText = data.GetMessageText(_client.EncodingMapper);

                        var smBuilder = SMS.ForSubmit()
                            .From(data.DestinationAddress)
                            .To(data.SourceAddress)
                            .Coding(data.DataCoding)
                            .Concatenation(ConcatenationType.UDH8bit, _client.SequenceGenerator.NextReferenceNumber())
                            .Set(m => m.MessageType = MessageTypes.SMEDeliveryAcknowledgement)
                            .Text(new Receipt
                            {
                                DoneDate = DateTime.Now,
                                State = MessageState.Delivered,
                                //  MessageId = data.Response.MessageId,
                                ErrorCode = "0",
                                SubmitDate = DateTime.Now,
                                Text = messageText.Substring(0, Math.Min(20, messageText.Length))
                            }.ToString()
                            );



                        _client.SubmitAsync(smBuilder).ConfigureAwait(false);
                    }
                    */
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util ob = new Util();
                data.Response.Header.Status = CommandStatus.ESME_RX_T_APPN;
                ob.LogDlvError("Failed to process DeliverSm. " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void client_evDataSm(object sender, DataSm data)
        {
            //_log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Incomplete message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", Text: " + args.Text,"0");
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Full message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", To: " + args.GetFirst<DeliverSm>().DestinationAddress + ", Text: " + args.Text);
        }

        private void client_evEnquireLink(object sender, EnquireLink data)
        {
            //_log.Info("EnquireLink received","0");
        }

        private void client_evUnBind(object sender, UnBind data)
        {
           // _log.Info("UnBind request received");
        }

        public delegate void SyncAction();

        public static void Sync(Control control, SyncAction action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, new object[] { });
                return;
            }
            action();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    if (clsCheck.inProcess) return;
            //    ProcessSMSsending();
            //}
            //catch (Exception ex)
            //{
            //    _log.Info("PROCESSTimer_" + ex.Message + " - " + ex.StackTrace);
            //}
        }

        private void timerDLRReceivedCheck_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (bDLRReceivedCheck) return;
                bDLRReceivedCheck = true;
                //ProcessDLRReceivedCheck();
                bDLRReceivedCheck = false;
            }
            catch (Exception ex)
            {
                bDLRReceivedCheck = false;
                new Util().LogDlvError("Failed to DLRReceivedCheck. " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void ProcessDLRReceivedCheck()
        {
            string BodyMailSMPP = "";
            int Notification_NoOfSMPPKey = Convert.ToInt32(ConfigurationManager.AppSettings["Notification_NoOfSMPPKey"]);
            int CheckDLRTime = Convert.ToInt32(ConfigurationManager.AppSettings["Notification_CheckDLRTime"]);
            string MailSubject = ConfigurationManager.AppSettings["Notification_MailSubject"].ToString();
            string MailFrom = ConfigurationManager.AppSettings["Notification_MailFrom"].ToString();
            string Password = ConfigurationManager.AppSettings["Notification_Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Notification_Host"].ToString();
            string TO = ConfigurationManager.AppSettings["Notification_TO"].ToString();
            string CC = ConfigurationManager.AppSettings["Notification_CC"].ToString();
            string Body = ConfigurationManager.AppSettings["Notification_Body"].ToString();
            List<string> ListCC = new List<string>();
            ListCC = Convert.ToString(CC).Split(';', ',').ToList();

            string curtime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            //database.ExecuteNonQuery("UPDATE tblFileProcess SET PickedTime = '" + curtime + "' WHERE PickedTime IS NULL");

            for (int i = 1; i <= Notification_NoOfSMPPKey; i++)
            {
                string Notification_SMPPACCOUNTID = ConfigurationManager.AppSettings["Notification_SMPPACCOUNTID" + i + ""].ToString();
                string Notification_SYSTEMID = ConfigurationManager.AppSettings["Notification_SYSTEMID" + i + ""].ToString();
                string sql = @"SELECT m.PROFILEID,m.SMPPACCOUNTID FROM MSGSUBMITTED m with (nolock) LEFT JOIN DELIVERY d with (nolock) ON m.msgid = d.msgid 
                           WHERE d.msgid IS NULL AND DATEDIFF(MINUTE, m.sentdatetime, GETDATE()) > " + CheckDLRTime + @" 
                                 AND m.sentdatetime>'" + curtime + @"' AND m.SMPPACCOUNTID='" + Notification_SMPPACCOUNTID + @"'
                           GROUP BY m.PROFILEID,m.SMPPACCOUNTID,m.sentdatetime
                           ORDER BY m.sentdatetime desc;";

                DataTable dtDLR = database.GetDataTable(sql);
                if (dtDLR.Rows.Count > 0)
                {
                    BodyMailSMPP = BodyMailSMPP + Notification_SYSTEMID + ",";
                }
            }
            Body = Body + "\n" + BodyMailSMPP;
            obU.SendEmail(MailFrom, Password, Host, TO, MailSubject, Body, ListCC);
            string SqlInst = @"INSERT INTO NotifactionEmailLog (BodyMailSMPP,LastCheckDatetime,CreatedAt) values ('" + BodyMailSMPP + "','" + curtime + "',getdate())";
            database.ExecuteNonQuery(SqlInst);
        }
    }
}