using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using eMIMapi.Helper;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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

namespace eMIMapi
{
    public partial class SendSMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid; string pwd; string mobile; string senderid; string msg; string msgtype;
            userid = Request.QueryString["userid"];
            pwd = Request.QueryString["pwd"];
            mobile = Request.QueryString["mobile"];
            senderid = Request.QueryString["sender"];
            msg = Request.QueryString["msg"];
            msgtype = Request.QueryString["msgtype"];

            //check user name and password
            if (userid == null) Response.Write("Invalid User ID");
            if (pwd == null) Response.Write("Invalid Password");
            if (mobile == null) Response.Write("Invalid Mobile Number");
            if (mobile == null) Response.Write("Invalid Mobile Number");
            if (senderid == null) Response.Write("Invalid Sender ID");
            if (msg == null) Response.Write("Invalid Message Text");
            if (msgtype == null) Response.Write("Invalid Message Type");

            //Response.Write(sendSMS(userid, pwd, mobile, senderid, msg, msgtype));
        }

//        public async Task<string> sendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
//        {
//            try
//            {

//                //validations

//                //sms types
//                //premium sms   - 13
//                //linktext sms  - 21
//                //otp sms       - 33
//                //campaign sms  - 47

//                //check user name and password
//                //if (userid == null) return "Invalid User ID";
//                //if (pwd == null) return "Invalid Password";
//                //if (mobile == null) return "Invalid Mobile Number";
//                //if (mobile == null) return "Invalid Mobile Number";
//                //if (sender == null) return "Invalid Sender ID";
//                //if (msg == null) return "Invalid Message Text";
//                //if (msgtype == null) return "Invalid Message Type";

//                Util ob = new Util();
//                DataTable dt = ob.GetUserParameter(userid);
//                if (dt.Rows.Count <= 0) return "Invalid User ID";
//                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";

//                if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
//                double Num;
//                bool isNum = double.TryParse(mobile, out Num);
//                if (!isNum) return "Invalid Mobile Number.";

//                if (mobile.Trim().Length == 10) mobile = "91" + mobile;

//                if (msg.Trim() == "") return "Invalid Message Text";

//                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47")) return "Invalid Message Type";
//                //check balance
//                double rate = 0;
//                int noofsms = (msg.Trim().Length / 160);
//                if (msg.Trim().Length % 160 > 0) noofsms++;

//                if (msgtype == "13") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
//                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
//                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
//                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
//                if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

//                //check valid sender id
//                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

//                Inetlab.SMPP.LicenseManager.SetLicense(licenseContent);
//                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

//                client.ResponseTimeout = TimeSpan.FromSeconds(60);
//                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

//                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
//                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
//                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
//                client.evUnBind += new UnBindEventHandler(client_evUnBind);
//                client.evDataSm += new DataSmEventHandler(client_evDataSm);
//                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

//                client.evServerCertificateValidation += OnCertificateValidation;


//                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
//                client.SystemType = ""; // tbSystemType.Text;
//                client.ConnectionRecovery = true; // cbReconnect.Checked;
//                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
//                client.EnabledSslProtocols = SslProtocols.None;

//                //get account for messagetype and bind ----
//                string smppaccountid = sms_acid;
//                bool b = await client.ConnectAsync(sms_ip, sms_port);
//                //System.Threading.Thread.Sleep(5000);
//                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
//                //System.Threading.Thread.Sleep(200);
//                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

//                List<SubmitSm> pduList = new List<SubmitSm>();

//                var pduBuilder = SMS.ForSubmit()
//                    .From(sourceAddress)
//                    .To(mobile)
//                    .DeliveryReceipt()
//                    .Text(msg);

//                pduList.AddRange(pduBuilder.Create(client));

//                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

//                UnBindResp Uresp = await client.UnbindAsync();
//                await client.DisconnectAsync();

//                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
//                //{
//                //deduct balance
//                string s = ob.UpdateAndGetBalance(userid, "", noofsms, rate);
//                ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,"","");
//                return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
//                //}
//                //else
//                //{
//                //return resp;
//                //}
//                //    smpp obj = new smpp();
//                //obj.userid = userid;
//                //obj.sender = sender;
//                //obj.mobile = mobile;
//                //obj.msg = msg;
//                //obj.msgtype = msgtype;
//                //obj.connect();
//                //string resp = obj.msgid;
//                //return ""; // resp;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }

        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private async void client_evDisconnected(object sender)
        {

            //_log.Info("SmppClient disconnected");

            //SmppClient client = (SmppClient)sender;
            //string s = client.SystemID;
            //int i = -1;
            //for (int j = 0; j < dt.Rows.Count; j++)
            //{
            //    if (dt.Rows[j]["SYSTEMID"].ToString() == s)
            //    { i = j; break; }
            //}
            //if (i >= 0)
            //{
            //    DataRow[] dr = dt.Select("SYSTEMID = '" + s + "'");
            //    string[] arg = (
            //    i.ToString() + "$" + "CONNECT" +      // 0, 1
            //            Convert.ToString(dr[0]["SMPPACCOUNTID"]) + "$" +   //2
            //            Convert.ToString(dr[0]["PROVIDER"]) + "$" +        //3
            //            Convert.ToString(dr[0]["ACCOUNTTYPE"]) + "$" +     //4
            //            Convert.ToString(dr[0]["HOSTNAME"]) + "$" +        //5
            //            Convert.ToString(dr[0]["PORT"]) + "$" +            //6
            //            Convert.ToString(dr[0]["USESSL"]) + "$" +          //7
            //            Convert.ToString(dr[0]["SYSTEMID"]) + "$" +        //8
            //            Convert.ToString(dr[0]["PASSWORD"]) + "$" +        //9
            //            Convert.ToString(dr[0]["BINDINGMODE"]) + "$" +     //10
            //            Convert.ToString(dr[0]["SYSTEMTYPE"]) + "$" +      //11
            //            Convert.ToString(dr[0]["ADDRESS_TON"]) + "$" +     //12
            //            Convert.ToString(dr[0]["ADDRESS_NPI"]) + "$" +     //13
            //            Convert.ToString(dr[0]["SOURCE_ADDRESS"]) + "$" +  //14
            //            Convert.ToString(dr[0]["TON_S"]) + "$" +           //15
            //            Convert.ToString(dr[0]["NPI_S"]) + "$" +           //16
            //            Convert.ToString(dr[0]["SERVICE"]) + "$" +         //17
            //            Convert.ToString(dr[0]["DESTNATION_ADDRESS"]) + "$" + //18
            //            Convert.ToString(dr[0]["TON_D"]) + "$" +           //19
            //            Convert.ToString(dr[0]["NPI_D"]) + "$" +           //20
            //            Convert.ToString(dr[0]["DATACODING"]) + "$" +      //21
            //            Convert.ToString(dr[0]["MODE"])).Split('$');

            //    await Disconnect(arg);

            //    for (int m = 0; m < grid.Rows.Count; m++)
            //        if (Convert.ToInt16(grid.Rows[m].Cells[0].Value) == Convert.ToInt16(i + 1))
            //            grid.Rows[m].Cells[3].Value = "DisConnected";

            //    await Connect(arg);
            //}


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
            //_log.Info("Connection has been recovered.");

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
            //int i = Convert.ToInt16(arg[0]);
            //_log.Info("Unbind SmppClient " + arg[5]);
            //UnBindResp resp = await client.UnbindAsync();

            //switch (resp.Header.Status)
            //{
            //    case CommandStatus.ESME_ROK:
            //        //_log.Info("UnBind succeeded: Status: " + resp.Header.Status);
            //        break;
            //    default:
            //        //_log.Info("UnBind failed: Status: " + resp.Header.Status);
            //        await client.DisconnectAsync();
            //        break;
            //}

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
                    //System.Threading.Thread t = new System.Threading.Thread(() =>
                    //{
                    //    ob.UpdateDelivery(messageId, donedate, deliveryStatus, data.Receipt.ToString(), errcd);
                    //});
                    //t.Start();


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
                data.Response.Header.Status = CommandStatus.ESME_RX_T_APPN;
                //_log.Info("Failed to process DeliverSm. " + ex.Message + " - " + ex.StackTrace);
            }
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