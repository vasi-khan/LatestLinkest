using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Web;

namespace eMIMapi.Helper
{
    public class smpp
    {
        public string userid;
        public string sender;
        public string mobile;
        public string msg;
        public string msgtype;
        public string msgid = "";
        public string msgstat = "";
        public const int i = 0;
        private SmppClient[] _client;
        private MessageComposer _messageComposer;
        Util _log = new Util();
        Util obU = new Util();
        DataTable dt;
        public void sendsmpp()
        {
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
            Inetlab.SMPP.LicenseManager.SetLicense(licenseContent);
            _client = new SmppClient[1];
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
        }

        public async void connect()
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

            _messageComposer = new MessageComposer();
            _messageComposer.evFullMessageReceived += OnFullMessageReceived;
            _messageComposer.evFullMessageTimeout += OnFullMessageTimeout;
            dt = new DataTable();


            dt = GetSMPPAccounts();

            DataRow dr = dt.Rows[i];
            string argm = i.ToString() + "$" + "CONNECT" + "$" +     // 0, 1
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
                Convert.ToString(dr["MODE"]);                    //22;

            string[] arg = Convert.ToString(argm).Split('$');

            await Connect(arg);
        }

        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }
        public DataTable GetSMPPAccounts()
        {
            string sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.active=1 and n.active=1 and n.sessionid='101'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        private async Task Connect(string[] arg)
        {
            try
            {

                int i = Convert.ToInt16(arg[0]);

                if (_client[i].Status == ConnectionStatus.Closed)
                {
                    _log.Info("Connecting to " + arg[5]);

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
                        _log.Info("SmppClient connected " + arg[8]);

                        await Bind(arg);
                        //for (int m = 0; m < grid.Rows.Count; m++)
                        //    if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                        //        grid.Rows[m].Cells[3].Value = "Connected";
                    }
                    else
                    {
                        _log.Info("Not Connected .. " + arg[8]);
                        //for (int m = 0; m < grid.Rows.Count; m++)
                        //    if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                        //        grid.Rows[m].Cells[3].Value = "DisConnected";
                        //bConnect.Enabled = true;
                        //cbReconnect.Enabled = true;
                        //bDisconnect.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info("err on connect " + arg[5] + ". " + ex.Message + ". - " + ex.StackTrace);
            }

        }

        private async Task Bind(string[] arg)
        {

            _log.Info("Bind client with SystemId: " + arg[8]);
            int i = Convert.ToInt16(arg[0]);
            ConnectionMode mode = ConnectionMode.Transceiver;


            //bDisconnect.Enabled = true;
            //mode =  (ConnectionMode)cbBindingMode.SelectedItem;
            object o = arg[10];
            mode = ConnectionMode.Transceiver;

            BindResp resp = await _client[i].BindAsync(arg[8], arg[9], mode);

            switch (resp.Header.Status)
            {
                case CommandStatus.ESME_ROK:
                    await SendMessage();
                    _log.Info("Bind succeeded: Status: " + resp.Header.Status + ", SystemId: " + resp.SystemId);
                    //bSubmit.Enabled = true;
                    break;
                default:
                    _log.Info("Bind failed: Status: " + resp.Header.Status);
                    await Disconnect(arg);
                    break;
            }
        }

        private async Task Disconnect(string[] arg)
        {

            int i = Convert.ToInt16(arg[0]);
            _log.Info("Disconnect from SMPP server " + arg[5]);

            if (_client[i].Status == ConnectionStatus.Bound)
            {
                await UnBind(arg);
            }

            if (_client[i].Status == ConnectionStatus.Open)
            {
                await _client[i].DisconnectAsync();
            }
        }

        private async void client_evDisconnected(object sender)
        {

            _log.Info("SmppClient disconnected");

            SmppClient client = (SmppClient)sender;
            string s = client.SystemID;
            int i = -1;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["SYSTEMID"].ToString() == s)
                { i = j; break; }
            }
            if (i >= 0)
            {
                DataRow[] dr = dt.Select("SYSTEMID = '" + s + "'");
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

                //for (int m = 0; m < grid.Rows.Count; m++)
                //    if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
                //        grid.Rows[m].Cells[3].Value = "DisConnected";

                //await Connect(arg);
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
            _log.Info("Connection has been recovered.");

            //Sync(this, () =>
            //{
            //    bConnect.Enabled = false;
            //    bDisconnect.Enabled = true;
            //    bSubmit.Enabled = true;
            //    cbReconnect.Enabled = false;
            //});

        }

        private async Task UnBind(string[] arg)
        {
            int i = Convert.ToInt16(arg[0]);
            _log.Info("Unbind SmppClient " + arg[5]);
            UnBindResp resp = await _client[i].UnbindAsync();

            switch (resp.Header.Status)
            {
                case CommandStatus.ESME_ROK:
                    _log.Info("UnBind succeeded: Status: " + resp.Header.Status);
                    break;
                default:
                    _log.Info("UnBind failed: Status: " + resp.Header.Status);
                    await _client[i].DisconnectAsync();
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
                    string errcd = Convert.ToString(data.Receipt.ErrorCode);
                    Util ob = new Util();
                    System.Threading.Thread t = new System.Threading.Thread(() =>
                    {
                        UpdateDelivery(messageId, donedate, deliveryStatus, data.Receipt.ToString(), errcd);
                    });
                    t.Start();


                    _log.Info("Delivery Receipt received: " + data.Receipt.ToString());

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
                data.Response.Header.Status = CommandStatus.ESME_RX_T_APPN;
                _log.Info("Failed to process DeliverSm. " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt, string errcd)
        {
            string sql = "";
            string d = "";
            try
            {
                if (txt.Contains(" done date:"))
                {
                    int p = txt.IndexOf(" done date:") + 11;
                    d = txt.Substring(p, 12);
                    if (d.Substring(10, 2).ToUpper() == " S") d.ToUpper().Replace(" S", "00");
                    d = "20" + d.Substring(0, 2) + "-" + d.Substring(2, 2) + "-" + d.Substring(4, 2) + " " + d.Substring(6, 2) + ":" + d.Substring(8, 2) + ":" + d.Substring(10, 2);
                }
            }
            catch (Exception ex)
            {
                d = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            }

            try
            {
                sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) values ('" + txt + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex2)
            {
                sql = "Insert into DELIVERY (DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code) values ('" + txt + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                database.ExecuteNonQuery(sql);
            }
        }

        private void client_evDataSm(object sender, DataSm data)
        {
            //_log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            _log.Info("Incomplete message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", Text: " + args.Text);
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            _log.Info("Full message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", To: " + args.GetFirst<DeliverSm>().DestinationAddress + ", Text: " + args.Text);
        }

        private void client_evEnquireLink(object sender, EnquireLink data)
        {
            _log.Info("EnquireLink received");
        }

        private void client_evUnBind(object sender, UnBind data)
        {
            _log.Info("UnBind request received");
        }

        public async Task<string> SendMessage()
        {
            string strResp = "";
            try
            {
                //var sourceAddress = new SmeAddress(dt.Rows[i]["SOURCE_ADDRESS"].ToString(), (AddressTON)byte.Parse(dt.Rows[i]["TON_S"].ToString()), (AddressNPI)byte.Parse(dt.Rows[i]["NPI_S"].ToString()));
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse(dt.Rows[i]["TON_S"].ToString()), (AddressNPI)byte.Parse(dt.Rows[i]["NPI_S"].ToString()));

                List<SubmitSm> pduList = new List<SubmitSm>();

                var pduBuilder = SMS.ForSubmit()
                    .From(sourceAddress)
                    .To(mobile)
                    .DeliveryReceipt()
                    .Text(msg);

                pduList.AddRange(pduBuilder.Create(_client[i]));

                IList<SubmitSmResp> resp = await _client[i].SubmitAsync(pduList.ToArray());
                DateTime sendTime = DateTime.Now;

                if (resp.All(x => x.Header.Status == CommandStatus.ESME_RINVSRCADR))
                {
                    //INVALID SENDER ID
                    strResp = string.Join(",", resp.Select(x => x.MessageId));
                    _log.Info("Submit Failed, Invalid Sender ID. MessageIds: " + strResp);
                    msgid = strResp;
                    msgstat = "ESME_RINVSRCADR";
                    //string[] strRsp = strResp.Split(',');
                    //AddInMsgSubmittedInvalidSender(dtSMS, sendTime);
                }
                else
                {
                    if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                    {
                        strResp = string.Join(",", resp.Select(x => x.MessageId));
                        _log.Info("Submit succeeded. MessageIds: " + strResp);
                        string[] strRsp = strResp.Split(',');
                        msgid = strResp;
                        msgstat = "SUCCESS";
                        //AddInMsgSubmitted(strRsp, dtSMS, sendTime);

                    }
                    else
                    {
                        _log.Info("Submit failed. Status: " + string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                        strResp = string.Join(",", resp.Select(x => x.MessageId));
                        _log.Info("Submit failed. MessageIds: " + strResp);
                        msgid = strResp;
                        msgstat = "SUCCESS";
                        //if ((strResp + " " + txtLog.Text).Length > 2000)
                        //    txtLog.Text = (strResp + " " + txtLog.Text).Substring(0, 2000);
                        //else
                        //    txtLog.Text = strResp + " " + txtLog.Text;
                        string[] strRsp = strResp.Split(',');
                        //AddInMsgSubmitted(strRsp, dtSMS, sendTime);
                    }
                }
                //System.Threading.Thread.Sleep(200);

                //obU.RemoveFromMsgTran(dtSMS.Rows[0]["SMPPACCOUNTID"].ToString());
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
            }
            return strResp;
        }
    }
}