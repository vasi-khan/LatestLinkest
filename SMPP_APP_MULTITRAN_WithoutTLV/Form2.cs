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

namespace SMPP_APP
{
    public partial class Form2 : Form
    {
        private SmppClient[] _client;
        private MessageComposer[] _messageComposer;
        DataTable dt;
        Util _log = new Util();
        Util obU = new Util();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
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
            Inetlab.SMPP.LicenseManager.SetLicense(licenseContent);
            
            dt = obU.GetAllSMPPAccounts();

            if (dt.Rows.Count > 0)
            {
                _client = new SmppClient[dt.Rows.Count];
                _messageComposer = new MessageComposer[dt.Rows.Count];
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
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
                    _client[i].EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                    _client[i].SystemType = ""; // tbSystemType.Text;
                    _client[i].ConnectionRecovery = true; // cbReconnect.Checked;
                    _client[i].ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                    _client[i].EnabledSslProtocols = SslProtocols.None;

                    _messageComposer[i] = new MessageComposer();
                    _messageComposer[i].evFullMessageReceived += OnFullMessageReceived;
                    _messageComposer[i].evFullMessageTimeout += OnFullMessageTimeout;

                    DataRow dr = dt.Rows[i];
                    _client[i].Name = Convert.ToString(dr["SMPPACCOUNTID"]);
                }
            }

            timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //Fetch SMS Sending Data from Database
                DataTable dtSMSMain = obU.GetAllSMSRecords(sTime);
                if (dtSMSMain.Rows.Count > 0) ProcessSMSRecords(dtSMSMain, sTime);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void ProcessSMSRecords(DataTable dtSMSMain,string picktime)
        {
            try
            {
                timer1.Enabled = false;
                DataTable dtSender = new DataView(dtSMSMain).ToTable(true, "SENDERID");

                

                foreach (DataRow drN in dtSender.Rows)
                {
                    var sourceAddress = new SmeAddress(drN["SENDERID"].ToString(), (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));
                    DataTable dtMAIN = dtSMSMain.Select("SENDERID='" + drN["SENDERID"].ToString() + "'").CopyToDataTable();
                    Int32 i = 0;
                    while (i < dtMAIN.Rows.Count)
                    {
                        DataTable dtSMPP = obU.GetSMPPAccounts4TX();
                        foreach (DataRow drS in dtSMPP.Rows)
                        {
                            DataRow[] cl = dt.Select("SMPPACCOUNTID = '" + drS["SMPPACCOUNTID"].ToString() + "'");
                            int clk = Convert.ToInt16(cl[0]["slno"]) - 1;
                            int pdulimit = Convert.ToInt16(drS["PDUSIZE"]);
                            if (i == dtMAIN.Rows.Count) break;

                            DataTable dtSMS = new DataTable();
                            dtSMS.Columns.Add("ID", typeof(string));
                            dtSMS.Columns.Add("PROVIDER", typeof(string));
                            dtSMS.Columns.Add("SMPPACCOUNTID", typeof(int));
                            dtSMS.Columns.Add("PROFILEID", typeof(string));
                            dtSMS.Columns.Add("MSGTEXT", typeof(string));
                            dtSMS.Columns.Add("TOMOBILE", typeof(string));
                            dtSMS.Columns.Add("SENDERID", typeof(string));
                            dtSMS.Columns.Add("CREATEDAT", typeof(DateTime));
                            dtSMS.Columns.Add("FILEID", typeof(int));

                            for (int j = 0; j < pdulimit; j++)
                            {
                                if (i < dtMAIN.Rows.Count)
                                {
                                    DataRow dr = dtSMS.NewRow();
                                    dr["ID"] = Convert.ToString(dtMAIN.Rows[i]["ID"]);
                                    dr["PROVIDER"] = Convert.ToString(dtMAIN.Rows[i]["PROVIDER"]);
                                    dr["SMPPACCOUNTID"] = drS["SMPPACCOUNTID"].ToString(); // Convert.ToString(dtMAIN.Rows[i]["SMPPACCOUNTID"]);
                                    dr["PROFILEID"] = Convert.ToString(dtMAIN.Rows[i]["PROFILEID"]);
                                    dr["MSGTEXT"] = Convert.ToString(dtMAIN.Rows[i]["MSGTEXT"]);
                                    dr["TOMOBILE"] = Convert.ToString(dtMAIN.Rows[i]["TOMOBILE"]);
                                    dr["SENDERID"] = Convert.ToString(dtMAIN.Rows[i]["SENDERID"]);
                                    dr["CREATEDAT"] = Convert.ToDateTime(dtMAIN.Rows[i]["CREATEDAT"]);
                                    dr["FILEID"] = dtMAIN.Rows[i]["FILEID"];
                                    dtSMS.Rows.Add(dr);
                                    i++;
                                }
                            }
                            int stopFileUpload = obU.getFileUploadStop();
                            if (stopFileUpload == 0)
                            {
                                List<SubmitSm> pduList = new List<SubmitSm>();
                                for (int w = dtSMS.Rows.Count - 1; w >= 0; w--)
                                {
                                    DataRow dr = dtSMS.Rows[w];
                                    int mobcnt = obU.GetDuplicateCount(dr["TOMOBILE"].ToString(), dr["MSGTEXT"].ToString(),"0","");
                                    if (mobcnt <= 0)
                                    {
                                        var pduBuilder = SMS.ForSubmit()
                                            .From(sourceAddress)
                                            .To(dr["TOMOBILE"].ToString())
                                            .DeliveryReceipt()
                                            .Text(dr["MSGTEXT"].ToString());
                                        pduList.AddRange(pduBuilder.Create(_client[clk]));
                                    }
                                    else
                                        dr.Delete();
                                }
                                dtSMS.AcceptChanges();
                                if (pduList.Count > 0)
                                     sendMessage(_client[clk], dtSMS, pduList);
                            }
                            else
                            {
                                AddInMsgStopped(dtSMS);
                            }
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                obU.RemoveAllFromMsgTran(picktime);
                timer1.Enabled = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //public void MakePDU()
        //{

        //    List<SubmitSm> pduList = new List<SubmitSm>();

        //    var pduBuilder = SMS.ForSubmit()
        //        .From(sourceAddress)
        //        .To(mobile)
        //        .DeliveryReceipt()
        //        .Text(msg);
        //    pduList.AddRange(pduBuilder.Create(client));
        //}

        public async void sendMessage(SmppClient client, DataTable dtSMS, List<SubmitSm> pduList)
        {
            try
            {
                //System.Threading.Thread.Sleep(200);
                
                DataRow[] dr = dt.Select("SMPPACCOUNTID = '" + client.Name + "'");
                string smppaccountid = client.Name;
                if (client.Status != ConnectionStatus.Bound)
                {
                    bool b = await client.ConnectAsync(Convert.ToString(dr[0]["HOSTNAME"]), Convert.ToInt16(dr[0]["PORT"]));
                    BindResp Bresp = await client.BindAsync(Convert.ToString(dr[0]["SYSTEMID"]), Convert.ToString(dr[0]["PASSWORD"]), ConnectionMode.Transmitter);
                }

                if (client.Status == ConnectionStatus.Bound)
                {
                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());
                    processResponse(resp, dtSMS, DateTime.Now, Convert.ToInt16(client.Name));
                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();
                }
                else
                {
                    obU.UpdateMsgTran(dtSMS);
                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void processResponse(IList<SubmitSmResp> resp, DataTable dtSMS, DateTime sendTime, int i )
        {
            try
            {
                if (resp.All(x => x.Header.Status == CommandStatus.ESME_RINVSRCADR))
                {
                    //INVALID SENDER ID
                    string strResp = string.Join(",", resp.Select(x => x.MessageId));
                    _log.Info_Failed("Submit Failed, Invalid Sender ID. MessageIds: " + strResp, i);
                    AddInMsgSubmittedInvalidSender(dtSMS, sendTime);
                }
                else
                {
                    if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                    {
                        string strResp = string.Join(",", resp.Select(x => x.MessageId));
                        _log.Info_Submit("Submit succeeded. MessageIds: " + strResp, i);
                        string[] strRsp = strResp.Split(',');
                        AddInMsgSubmitted(strRsp, dtSMS, sendTime);
                    }
                    else
                    {
                        _log.Info_Failed("Submit failed. Status: " + string.Join(",", resp.Select(x => x.Header.Status.ToString())), i);
                        string strResp = string.Join(",", resp.Select(x => x.MessageId));
                        _log.Info_Failed("Submit failed. MessageIds: " + strResp, i);
                        string[] strRsp = strResp.Split(',');
                        AddInMsgSubmitted(strRsp, dtSMS, sendTime);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void AddInMsgSubmittedInvalidSender(DataTable dtSMS, DateTime sendTime)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
            {
                string segment = Guid.NewGuid().ToString();
                obU.AddInMsgSubmittedInvalidSender(segment, dtSMS.Rows[i], sendTime);
            }
        }
        public void AddInMsgSubmitted(string[] strResp, DataTable dtSMS, DateTime sendTime)
        {
            InsertInMsgSubmitted(strResp, dtSMS, sendTime);
        }
        public void InsertInMsgSubmitted(string[] strResp, DataTable dtSMS, DateTime sendTime)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
                obU.AddInMsgSubmitted(strResp[i], dtSMS.Rows[i], sendTime);
        }
        public void AddInMsgStopped(DataTable dtSMS)
        {
            for (int i = 0; i < dtSMS.Rows.Count; i++)
                obU.AddInMsgStopped(dtSMS.Rows[i]);
        }

                     
        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private void client_evDisconnected(object sender)
        {
            //_log.Info("SmppClient disconnected");
        }

        private void ClientOnRecoverySucceeded(object sender, BindResp data)
        {
            //_log.Info("Connection has been recovered.");
        }

        private void client_evDeliverSm(object sender, DeliverSm data)
        {

        }

        private void client_evDataSm(object sender, DataSm data)
        {
            //_log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Incomplete message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", Text: " + args.Text);
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            //_log.Info("Full message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", To: " + args.GetFirst<DeliverSm>().DestinationAddress + ", Text: " + args.Text);
        }

        private void client_evEnquireLink(object sender, EnquireLink data)
        {
            // _log.Info("EnquireLink received");
        }

        private void client_evUnBind(object sender, UnBind data)
        {
            //_log.Info("UnBind request received");
        }

        
    }
}
