using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Web;

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/scr")]
    public class scrController : ApiController
    {

        public int expMin = 60;
        public string sms_provider = "";
        public string sms_ip = "";
        public int sms_port = 0;
        public string sms_acid = "";
        public string sms_systemid = "";
        public string sms_password = "";

        //------------PRIORITY ACCOUNT 
        public string sms_provider_PRIORITY = "INFOBIP";
        public string sms_ip_PRIORITY = "smpp3.infobip.com";
        public int sms_port_PRIORITY = 8888;
        public string sms_acid_PRIORITY = "1109";
        public string sms_systemid_PRIORITY = "OTP_Trans";
        public string sms_password_PRIORITY = "Shiva@1906";

        //------------PROMO ACCOUNT FOR NUMERIC SENDERID
        public string sms_provider_PROMO = "INFOBIP";
        public string sms_ip_PROMO = "smpp3.infobip.com";
        public int sms_port_PROMO = 8888;
        public string sms_acid_PROMO = "2006";
        public string sms_systemid_PROMO = "Promotestdlr";
        public string sms_password_PROMO = "Shiva@1906";

        //-----------------------------DUBAI -ETISALAT

        //public string sms_ip_DUB = "5.195.193.33";
        //public int sms_port_DUB = 9199;
        //public string sms_acid_DUB = "1508";
        //public string sms_systemid_DUB = "anirudh0706";
        //public string sms_password_DUB = "Seft@2308";

        //public string sms_ip_DUB = "103.205.64.220";
        //public int sms_port_DUB = 17225;
        //public string sms_acid_DUB = "1508";
        //public string sms_systemid_DUB = "MIM21484";
        //public string sms_password_DUB = "mim86731";

        public string sms_provider_DUB = "ETISALAT";
        public string sms_ip_DUB = "5.195.193.33";
        public int sms_port_DUB = 9199;
        public string sms_acid_DUB = "1008";
        public string sms_systemid_DUB = "dilip@2506";
        public string sms_password_DUB = "Shiva@1704";

        //-----------------------------INT

        public string sms_provider_INT = "MOBIS";
        public string sms_ip_INT = "180.179.210.40";
        public int sms_port_INT = 2345;
        public string sms_acid_INT = "1509";
        public string sms_systemid_INT = "my_inbx3";
        public string sms_password_INT = "inbx1921";

        //-------------------------VCON REPLACED WITH INFOBIP --05-05-2021--------------------->
        //public string sms_ip_VCON = "112.196.55.205";
        //public int sms_port_VCON = 10447;
        //public string sms_acid_VCON = "809";
        //public string sms_systemid_VCON = "MYINTR8";
        //public string sms_password_VCON = "MYIN_888";

        public string sms_provider_VCON = "INFOBIP";
        public string sms_ip_VCON = "smpp3.infobip.com";
        public int sms_port_VCON = 8888;
        public string sms_acid_VCON = "8201";
        public string sms_systemid_VCON = "Myinboxtrans10";
        public string sms_password_VCON = "Shiva@1906";

        public string sms_provider_API = "INFOBIP";
        public string sms_ip_API = "smpp3.infobip.com";
        public int sms_port_API = 8888;
        public string sms_acid_API = "1709";
        public string sms_systemid_API = "Myinboxtrans3";
        public string sms_password_API = "Vipinmim@9723";

        //-------------------------VCON REPLACED WITH INFOBIP ----------------------->

        public string sms_provider_INFBP = "INFOBIP";
        public string sms_ip_INFBP = "smpp3.infobip.com";
        public int sms_port_INFBP = 8888;
        public string sms_acid_INFBP = "1709";
        public string sms_systemid_INFBP = "Myinboxtrans3";
        public string sms_password_INFBP = "Vipinmim@9723";

        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        //----------VCON --------------//
        //public string sms_ip_OTP = "112.196.55.201";
        //public int sms_port_OTP = 16402;
        //public string sms_acid_OTP = "1409";
        //public string sms_systemid_OTP = "MYIOTP1";
        //public string sms_password_OTP = "MYIN_999";

        //----------INFOBIP --------------//

        public string sms_provider_OTP = "INFOBIP";
        public string sms_ip_OTP = "smpp3.infobip.com";
        public int sms_port_OTP = 8888;
        public string sms_acid_OTP = "1409";
        public string sms_systemid_OTP = "MyinboxOTP1";
        public string sms_password_OTP = "Vipinmim@9723";

        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        // ----- GSM ACCOUNT FOR LINKEXT API ------------------//

        public string sms_provider_SIM = "SIM";
        public string sms_ip_SIM = "167.114.0.183";
        public int sms_port_SIM = 5599;
        public string sms_acid_SIM = "2901";
        public string sms_systemid_SIM = "vipinmim1";
        public string sms_password_SIM = "Vipinmim@1";

        #region < not in use >
        [Route("Test")]
        [HttpGet]
        public string Test()
        {
            return "testing";
        }


        [Route("SendSMS2")]
        [HttpGet]
        public string SendSMS2(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
        {
            return "";
        }

        //with PEID & TemplateID
        [Route("SendSMSTest")]
        [HttpGet]
        public async Task<string> SendSMSTest(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid)
        {
            try
            {
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                if (templateid == null) return "Invalid Template ID";
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }

                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;

                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;



                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid);

                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        //WITHOUT PEID & TemplateID
        //[Route("MySendSMS")]
        //[HttpGet]
        //public async Task<string> MySendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
        //{
        //    try
        //    {

        //        //validations

        //        //sms types
        //        //premium sms   - 13
        //        //linktext sms  - 21
        //        //otp sms       - 33
        //        //campaign sms  - 47
        //        // INTERNATIONAL-15
        //        //DUBAI        -16

        //        //check user name and password
        //        if (userid == null) return "Invalid User ID";
        //        if (pwd == null) return "Invalid Password";
        //        if (mobile == null) return "Invalid Mobile Number";
        //        if (mobile == null) return "Invalid Mobile Number";
        //        if (sender == null) return "Invalid Sender ID";
        //        if (msg == null) return "Invalid Message Text";
        //        if (msgtype == null) return "Invalid Message Type";

        //        List<string> mobList1 = mobile.Split(',').ToList();
        //        List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
        //        //validation of list count
        //        if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

        //        Util ob = new Util();

        //        ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");
        //        //Util ob = new Util();
        //        DataTable dt = ob.GetUserParameter(userid);
        //        if (dt.Rows.Count <= 0) return "Invalid User ID";
        //        if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

        //        //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
        //        //double Num;
        //        //bool isNum = double.TryParse(mobile, out Num);
        //        //if (!isNum) return "Invalid Mobile Number.";

        //        //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

        //        if (msg.Trim() == "") return "Invalid Message Text";

        //        if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16")) return "Invalid Message Type";

        //        string peid = "";
        //        if (msgtype != "15" && msgtype != "16")
        //            peid = ob.getPEid(userid);

        //        if (msgtype != "15" && msgtype != "16")
        //            if (peid == "") return "Blocked by DLT";

        //        if (msgtype == "16")
        //        {
        //            if (mobile.Trim().Length < 12)
        //            {
        //                return "Invalid Mobile Number";
        //            }
        //        }

        //        //check balance
        //        double rate = 0;
        //        int noofsms = GetMsgCount(msg.Trim());
        //        bool ucs = false;
        //        if (msg.Trim().Any(c => c > 126)) ucs = true;

        //        if (msgtype == "13" || msgtype == "15" || msgtype == "16") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
        //        if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
        //        if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
        //        if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
        //        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
        //        //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
        //        { return "Insufficient Balance"; }
        //        //  if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

        //        //check valid sender id
        //        if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

        //        //get account for messagetype and bind ----
        //        if (msgtype == "47")
        //        {
        //            sms_ip = sms_ip_SIM;
        //            sms_port = sms_port_SIM;
        //            sms_acid = sms_acid_SIM;
        //            sms_systemid = sms_systemid_SIM;
        //            sms_password = sms_password_SIM;
        //        }
        //        else if (msgtype == "13")
        //        {
        //            sms_ip = sms_ip_VCON;
        //            sms_port = sms_port_VCON;
        //            sms_acid = sms_acid_VCON;
        //            sms_systemid = sms_systemid_VCON;
        //            sms_password = sms_password_VCON;
        //        }
        //        else if (msgtype == "33")
        //        {
        //            sms_ip = sms_ip_OTP;
        //            sms_port = sms_port_OTP;
        //            sms_acid = sms_acid_OTP;
        //            sms_systemid = sms_systemid_OTP;
        //            sms_password = sms_password_OTP;
        //        }
        //        else if (msgtype == "15")
        //        {
        //            sms_ip = sms_ip_INT;
        //            sms_port = sms_port_INT;
        //            sms_acid = sms_acid_INT;
        //            sms_systemid = sms_systemid_INT;
        //            sms_password = sms_password_INT;
        //        }
        //        else if (msgtype == "16")
        //        {
        //            sms_ip = sms_ip_DUB;
        //            sms_port = sms_port_DUB;
        //            sms_acid = sms_acid_DUB;
        //            sms_systemid = sms_systemid_DUB;
        //            sms_password = sms_password_DUB;
        //        }

        //        if (userid == "MIM2101450")
        //        {
        //            sms_ip = sms_ip_API;
        //            sms_port = sms_port_API;
        //            sms_acid = sms_acid_API;
        //            sms_systemid = sms_systemid_API;
        //            sms_password = sms_password_API;
        //        }

        //        bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
        //        if (isNumeric)
        //        {
        //            sms_ip = sms_ip_PROMO;
        //            sms_port = sms_port_PROMO;
        //            sms_acid = sms_acid_PROMO;
        //            sms_systemid = sms_systemid_PROMO;
        //            sms_password = sms_password_PROMO;
        //        }

        //        string templateid = "";
        //        if (msgtype != "15" && msgtype != "16")
        //        {
        //            if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
        //            {
        //                string sql = "";
        //                string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
        //                if (templID == "")
        //                {
        //                    // process REJECTION ....
        //                    //return "";
        //                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
        //                    string sRet = "";
        //                    string lastMsgID = "";
        //                    foreach (var m in mobList)
        //                    {
        //                        string nid = "";
        //                        for (int x = 0; x < noofsms; x++)
        //                        {
        //                            string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
        //                            nid = Guid.NewGuid().ToString();
        //                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
        //                            " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
        //                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
        //                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
        //                            " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
        //                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
        //                            " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
        //                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
        //                            " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
        //                            database.ExecuteNonQuery(sql);
        //                        }
        //                        sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
        //                        lastMsgID = nid;
        //                    }
        //                    sRet = sRet.Substring(0, sRet.Length - 2);
        //                    if (mobList.Count == 1)
        //                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
        //                    else
        //                        return "SMS Submitted Successfully. " + sRet;
        //                }
        //                if (templID != "")
        //                    templateid = templID;
        //            }
        //        }

        //        SmppClient client = smppClientContainer.GetClient;

        //        sms_ip = "smpp3.infobip.com";
        //        sms_port = 8888;
        //        sms_acid = "1709";
        //        sms_systemid = "Myinboxtrans9";
        //        sms_password = "Shiva@1906";

        //        bool b = false;

        //        if (client.Status != ConnectionStatus.Bound)
        //        {
        //            System.Threading.Thread.Sleep(1000);
        //            ob.Log("connecting 1");
        //            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { ob.Log("connecting Failed 1"); }
        //            BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

        //            if (client.Status != ConnectionStatus.Bound)
        //            {
        //                System.Threading.Thread.Sleep(1000);
        //                ob.Log("connecting 2");
        //                try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { ob.Log("connecting Failed 2"); }
        //                BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //                if (client.Status != ConnectionStatus.Bound)
        //                {
        //                    System.Threading.Thread.Sleep(1000);
        //                    ob.Log("connecting 3");
        //                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { ob.Log("connecting Failed 3"); }
        //                    BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //                    if (client.Status != ConnectionStatus.Bound) { }
        //                    else
        //                    {
        //                        ob.Log("Connected 3");
        //                    }
        //                }
        //                else
        //                {
        //                    ob.Log("Connected 2");
        //                }
        //            }
        //            else
        //            {
        //                ob.Log("Connected 1");
        //            }
        //        }
        //        //System.Threading.Thread.Sleep(200);
        //        var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));
        //        string smppaccountid = sms_acid;
        //        DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));
        //        List<SubmitSm> pduList = new List<SubmitSm>();
        //        int mobcnt = 0;

        //        if (peid != "")
        //        {
        //            foreach (var m in mobList)
        //            {
        //                //if (m.Length == 12 || m.Length == 10)
        //                //{
        //                mobcnt++;
        //                var pduBuilder = SMS.ForSubmit()
        //                     .From(sourceAddress)
        //                     .To(m)
        //                     .Coding(coding)
        //                     .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
        //                     .Text(msg)
        //                     .AddParameter(0x1400, peid)
        //                     .AddParameter(0x1401, templateid);
        //                pduList.AddRange(pduBuilder.Create(client));
        //                // }
        //            }
        //        }
        //        else
        //        {
        //            foreach (var m in mobList)
        //            {
        //                //if (m.Length == 12 || m.Length == 10)
        //                //{
        //                mobcnt++;
        //                var pduBuilder = SMS.ForSubmit()
        //                     .From(sourceAddress)
        //                     .To(m)
        //                     .Coding(coding)
        //                     .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
        //                     .Text(msg);
        //                pduList.AddRange(pduBuilder.Create(client));
        //                //}
        //            }

        //        }

        //        IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

        //        //UnBindResp Uresp = await client.UnbindAsync();
        //        //await client.DisconnectAsync();

        //        //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
        //        //{
        //        //deduct balance
        //        string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);

        //        //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");
        //        ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs);

        //        if (mobList.Count == 1)
        //            return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
        //        else
        //        {
        //            string ss = "";
        //            for (int i = 0; i < resp.Count; i++)
        //            {
        //                ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
        //            }
        //            ss = ss.Substring(0, ss.Length - 2);
        //            return "SMS Submitted Successfully. " + ss;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}


        //WITHOUT PEID & TemplateID

        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
        {
            try
            {

                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47
                // INTERNATIONAL-15
                //DUBAI        -16
                //PRIORITY OTP  - 17 
                //PRIORITY TRAN - 18 

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                string peid = "";
                if (msgtype != "15" && msgtype != "16")
                    peid = ob.getPEid(userid);

                if (msgtype != "15" && msgtype != "16")
                    if (peid == "") return "Blocked by DLT";

                if (msgtype == "16")
                {
                    if (mobile.Trim().Length < 12)
                    {
                        return "Invalid Mobile Number";
                    }
                }

                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                { return "Insufficient Balance"; }
                //  if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //get account for messagetype and bind ----

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "16")
                {
                    sms_provider = sms_provider_DUB;
                    sms_ip = sms_ip_DUB;
                    sms_port = sms_port_DUB;
                    sms_acid = sms_acid_DUB;
                    sms_systemid = sms_systemid_DUB;
                    sms_password = sms_password_DUB;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                string templateid = "";
                if (msgtype != "15" && msgtype != "16")
                {
                    if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                    {
                        string sql = "";
                        string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                        if (templID == "")
                        {
                            // process REJECTION ....
                            //return "";
                            //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                            string sRet = "";
                            string lastMsgID = "";
                            foreach (var m in mobList)
                            {
                                string nid = "";
                                for (int x = 0; x < noofsms; x++)
                                {
                                    string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                    " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                    database.ExecuteNonQuery(sql);
                                }
                                sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                lastMsgID = nid;
                            }
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
                        }
                        if (templID != "")
                            templateid = templID;
                    }
                }

                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;



                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }
                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));
                List<SubmitSm> pduList = new List<SubmitSm>();
                int mobcnt = 0;

                if (peid != "")
                {
                    foreach (var m in mobList)
                    {
                        //if (m.Length == 12 || m.Length == 10)
                        //{
                        mobcnt++;
                        var pduBuilder = SMS.ForSubmit()
                             .From(sourceAddress)
                             .To(m)
                             .Coding(coding)
                             .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                             .Text(msg)
                             .AddParameter(0x1400, peid)
                             .AddParameter(0x1401, templateid);
                        if (sms_provider.Contains("AIRTEL"))
                        {
                            pduBuilder = SMS.ForSubmit()
                            .From(sourceAddress)
                            .To(m)
                            .Coding(coding)
                            .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                            .Text(msg)
                            .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                            .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                            .AddParameter(0x1402, ob.getTMID(sms_provider));
                        }
                        pduList.AddRange(pduBuilder.Create(client));
                        // }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        //if (m.Length == 12 || m.Length == 10)
                        //{
                        mobcnt++;
                        var pduBuilder = SMS.ForSubmit()
                             .From(sourceAddress)
                             .To(m)
                             .Coding(coding)
                             .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                             .Text(msg);
                        pduList.AddRange(pduBuilder.Create(client));
                        //}
                    }
                }

                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);

                //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);

                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //with PEID   -- SMPP
        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid)
        {
            try
            {

                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                if (msgtype != "15" && msgtype != "16")
                    if (mobile.Length < 10) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                // if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms)) return "Insufficient Balance";
                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;


                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                if (userid == "MIM2101450")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
                        ss += "MobileNo: " + m.ToString() + " Message ID: " + msgid + ", ";
                    }
                    if (mobList.Count == 1)
                        return "SMS Submitted Successfully. Message ID: " + msgid;
                    else
                    {
                        ss = ss.Substring(0, ss.Length - 2);
                        return "SMS Submitted Successfully. " + ss;
                    }
                }
                else
                {

                    #region < COmmented >

                    string templateid = "";
                    if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                    {
                        string sql = "";
                        string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                        if (templID == "")
                        {
                            // process REJECTION ....
                            //return "";
                            //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                            string sRet = "";
                            string lastMsgID = "";
                            foreach (var m in mobList)
                            {
                                string nid = "";
                                for (int x = 0; x < noofsms; x++)
                                {
                                    string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'" + sms_provider + "','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'" + sms_provider + "','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                    " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                    database.ExecuteNonQuery(sql);
                                }
                                sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                lastMsgID = nid;
                            }
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
                        }
                        if (templID != "")
                            templateid = templID;
                    }

                    Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                    Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                    client.ResponseTimeout = TimeSpan.FromSeconds(60);
                    client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                    client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                    client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                    client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                    client.evUnBind += new UnBindEventHandler(client_evUnBind);
                    client.evDataSm += new DataSmEventHandler(client_evDataSm);
                    client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                    client.evServerCertificateValidation += OnCertificateValidation;


                    client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                    client.SystemType = ""; // tbSystemType.Text;
                    client.ConnectionRecovery = true; // cbReconnect.Checked;
                    client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                    client.EnabledSslProtocols = SslProtocols.None;



                    //get account for messagetype and bind ----
                    string smppaccountid = sms_acid;
                    bool b = await client.ConnectAsync(sms_ip, sms_port);
                    //System.Threading.Thread.Sleep(5000);
                    BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                        BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                            BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                            if (client.Status != ConnectionStatus.Bound)
                            {
                                System.Threading.Thread.Sleep(1000);
                                try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                                BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                            }
                        }
                    }
                    //System.Threading.Thread.Sleep(200);
                    var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                    DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));
                    List<SubmitSm> pduList = new List<SubmitSm>();

                    int mobcnt = 0;
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();

                    //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                    //{
                    //deduct balance
                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                    //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");
                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);

                    if (mobList.Count == 1)
                        return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                    else
                    {
                        string ss = "";
                        for (int i = 0; i < resp.Count; i++)
                        {
                            ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                        }
                        ss = ss.Substring(0, ss.Length - 2);
                        return "SMS Submitted Successfully. " + ss;
                    }

                    //}
                    //else
                    //{
                    //return resp;
                    //}
                    //    smpp obj = new smpp();
                    //obj.userid = userid;
                    //obj.sender = sender;
                    //obj.mobile = mobile;
                    //obj.msg = msg;
                    //obj.msgtype = msgtype;
                    //obj.connect();
                    //string resp = obj.msgid;
                    //return ""; // resp;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //with PEID AND SMSKEY    -- SMPP
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid)
        {
            try
            {
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";

                if (msgtype != "15" && msgtype != "16")
                    if (mobile.Length < 10) return "Invalid Mobile Number.";

                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                //string msgid = "";
                //string ss = "";
                //foreach (var m in mobList)
                //{
                //    msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                //    ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
                //    ss += "MobileNo: " + m.ToString() + " Message ID: " + msgid + ", ";
                //}
                //if (mobList.Count == 1)
                //    return "SMS Submitted Successfully. Message ID: " + msgid;
                //else
                //{
                //    ss = ss.Substring(0, ss.Length - 2);
                //    return "SMS Submitted Successfully. " + ss;
                //}
                #region <Commented >
                string templateid = "";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;



                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                //if(Bresp.)
                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }
                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                int mobcnt = 0;
                foreach (var m in mobList)
                {
                    if (m.Length == 12 || m.Length == 10)
                    {
                        mobcnt++;
                        var pduBuilder = SMS.ForSubmit()
                        .From(sourceAddress)
                        .To(m)
                        .Coding(coding)
                        .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                        .Text(msg)
                        .AddParameter(0x1400, peid)
                        .AddParameter(0x1401, templateid);
                        if (sms_provider.Contains("AIRTEL"))
                        {
                            pduBuilder = SMS.ForSubmit()
                            .From(sourceAddress)
                            .To(m)
                            .Coding(coding)
                            .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                            .Text(msg)
                            .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                            .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                            .AddParameter(0x1402, ob.getTMID(sms_provider));
                        }
                        pduList.AddRange(pduBuilder.Create(client));
                    }
                }

                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //with PEID & TemplateID  
        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid)
        {
            try
            {
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                if (templateid == null) return "Invalid Template ID";
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                if (msgtype != "15" && msgtype != "16")
                    if (mobile.Length < 10) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                //string msgid = "";
                //string ss = "";
                //foreach (var m in mobList)
                //{
                //    msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                //    ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs,templateid);
                //    ss += "MobileNo: " + m.ToString() + " Message ID: " + msgid + ", ";
                //}
                //if (mobList.Count == 1)
                //    return "SMS Submitted Successfully. Message ID: " + msgid;
                //else
                //{
                //    ss = ss.Substring(0, ss.Length - 2);
                //    return "SMS Submitted Successfully. " + ss;
                //}
                #region < Commented >
                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;

                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;



                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //with PEID & TemplateID AND APIKEY
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid)
        {
            try
            {
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                if (templateid == null) return "Invalid Template ID";
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["PWD"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect API key";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }
                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;



                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }

                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));


                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg)
                              .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //with PEID & TemplateID AND APIKEY & LongURL
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL)
        {
            try
            {
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                //if (templateid == null) return "Invalid Template ID";
                if (LongURL == null) return "Invalid Long URL";
                if (!LongURL.StartsWith("http://") && !LongURL.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";

                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["PWD"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect API key";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;
                string msg1 = msg.Trim() + " " + domain + "12345678";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                if (templateid == null || templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                //URL Shortning .....
                string lblShortURL = "";
                string segment = ob.GetShortURLofLongURL(userid, LongURL);
                if (segment == "")
                {
                    string sUrl = ob.NewShortURLfromSQL(domain);
                    ob.SaveShortURL(userid, LongURL, "", sUrl, "N", "Y", domain);
                    lblShortURL = domain + sUrl;
                }
                else
                    lblShortURL = segment;

                if (msg.Contains(LongURL))
                {
                    msg = msg.Trim().Replace(LongURL, lblShortURL);
                }
                else
                {
                    //URL Shortning .....
                    msg = msg.Trim() + " " + lblShortURL;
                }
                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }

                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));


                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg)
                              .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt()
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //with PEID & TemplateID AND APIKEY & LongURL
        [Route("SendSMSKWithLink")]
        [HttpGet]
        public async Task<string> SendSMSKWithLink(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL, string existingURL)
        {
            try
            {               
                //validations
                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                //if (templateid == null) return "Invalid Template ID";
                if (LongURL == null) return "Invalid Long URL";
                if (!LongURL.StartsWith("http://") && !LongURL.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";
                Util ob = new Util();
                if (existingURL.ToUpper()=="Y")
                {
                    if(!ob.CheckLongURL(userid, LongURL))
                    {
                        return "Long url does not exists";
                    }
                }
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["PWD"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect API key";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;
                string msg1 = msg.Trim() + " " + domain + "12345678";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                if (templateid == null || templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                if (existingURL.ToUpper() == "N")
                {
                    string sUrl = ob.NewShortURLfromSQL(domain);
                    ob.SaveShortURL(userid, LongURL, "", sUrl, "Y", "Y", domain);
                }
                string _shortUrlId = ob.GetShortUrlIdFromLongURL(LongURL, userid);

                DataTable dtMobTrackUrl = new DataTable();
                dtMobTrackUrl.Columns.Add("Mob");
                dtMobTrackUrl.Columns.Add("segment");
                //Add Short URL Id
                DataColumn newColumn = new DataColumn("shorturlid");
                newColumn.DefaultValue = _shortUrlId;
                dtMobTrackUrl.Columns.Add(newColumn);

                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }

                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                string msg_orig = msg;
                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            msg = msg_orig;
                            SetMultipleShortURL(ref msg, LongURL, existingURL, ob, domain, dtMobTrackUrl, m);

                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg)
                              .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            msg = msg_orig;
                            SetMultipleShortURL(ref msg, LongURL, existingURL, ob, domain, dtMobTrackUrl, m);

                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt()
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance               

                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid,dtMobTrackUrl);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("SendSMSscratch")]
        [HttpGet]
        public async Task<string> SendSMSscratch(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string URL, string couponNo)
        {
            try
            {
                string existingURL = "Y";
                //validations
                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
               
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                if(couponNo == null) return "Invalid Coupon Number";
                if (string.IsNullOrEmpty(templateid)) return "Invalid Template ID";
                if (URL == null) return "Invalid Long URL";
                if (!URL.StartsWith("http://") && !URL.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";
                Util ob = new Util();
                
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }


                //ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameterScrach(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["PWD"].ToString()) return "Incorrect Password";
               

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderIdScrach(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;
                string msg1 = msg.Trim() + " " + domain + "12345678";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                if (templateid == null || templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);
                    if (templID == "")
                    {
                        // process REJECTION ....
                        //return "";
                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                        string sRet = "";
                        string lastMsgID = "";
                        foreach (var m in mobList)
                        {
                            string nid = "";
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:5308 text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','5308',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                        else
                            return "SMS Submitted Successfully. " + sRet;
                    }
                    if (templID != "")
                        templateid = templID;
                }
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = "vcon";
                    sms_ip = "112.196.55.201";
                    sms_port = 17211;
                    sms_acid = "201";
                    sms_systemid = "MYINTR7";
                    sms_password = "VCON_123";
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }

                if (existingURL.ToUpper() == "N")
                {
                    string sUrl = ob.NewShortURLfromSQLScrach(domain);
                    ob.SaveShortURLScrach(userid, URL, "", sUrl, "Y", "Y", domain);
                }
                string _shortUrlId = ob.GetShortUrlIdFromLongURLScrach(URL, userid);

                DataTable dtMobTrackUrl = new DataTable();
                dtMobTrackUrl.Columns.Add("Mob");
                dtMobTrackUrl.Columns.Add("segment");                
                //Add Short URL Id
                DataColumn newColumn = new DataColumn("shorturlid");
                newColumn.DefaultValue = _shortUrlId;
                dtMobTrackUrl.Columns.Add(newColumn);

                DataColumn newColumn1 = new DataColumn("coupanNo");
                newColumn1.DefaultValue = couponNo;
                dtMobTrackUrl.Columns.Add(newColumn1);

                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }

                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                string msg_orig = msg;
                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            msg = msg_orig;
                            SetMultipleShortURL(ref msg, URL, existingURL, ob, domain, dtMobTrackUrl, m);

                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg)
                              .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            msg = msg_orig;
                            SetMultipleShortURL(ref msg, URL, existingURL, ob, domain, dtMobTrackUrl, m);

                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt()
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                string s = ob.UpdateAndGetBalanceScrach(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmittedScrach(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid, dtMobTrackUrl);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
        
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static void SetMultipleShortURL(ref string msg, string LongURL, string existingURL, Util ob, string domain, DataTable dtMobTrackUrl, string mobile)
        {
            string _segment = ob.NewShortURLforMobTrkSQL();
            string lblShortURL = "";
            if (existingURL.ToUpper() == "N")
            {
                lblShortURL = domain + _segment;
                if (msg.Contains(LongURL))
                {
                    msg = msg.Trim().Replace(LongURL, lblShortURL);
                }
                else
                {
                    msg = msg.Trim() + " " + lblShortURL;
                }
                DataRow row = dtMobTrackUrl.NewRow();
                row["Mob"] = mobile;
                row["segment"] = _segment;
                dtMobTrackUrl.Rows.Add(row);

            }
            else if (existingURL.ToUpper() == "Y")
            {
                lblShortURL = domain + _segment;
                DataRow row = dtMobTrackUrl.NewRow();
                row["Mob"] = mobile;
                row["segment"] = _segment;
                dtMobTrackUrl.Rows.Add(row);

                if (msg.Contains(LongURL))
                {
                    msg = msg.Trim().Replace(LongURL, lblShortURL);
                }
                else
                {
                    msg = msg.Trim() + " " + lblShortURL;
                }
            }
        }

        //with PEID & TemplateID AND APIKEY & LongURL &  activityid
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL, string activityId)
        {
            try
            {
                LongURL = string.Format("{0}&activityId={1}", LongURL, activityId);

                Random r = new Random();
                r.Next(111111, 999999);
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                if (peid == null) return "Invalid PE-ID";
                if (templateid == null) return "Invalid Template ID";
                if (LongURL == null) return "Invalid Long URL";
                if (!LongURL.StartsWith("http://") && !LongURL.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";

                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                Util ob = new Util();
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["PWD"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect API key";

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;
                string msg1 = msg.Trim() + " " + domain + "12345678";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;


                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;

                if (msgtype == "47")
                {
                    sms_provider = sms_provider_SIM;
                    sms_ip = sms_ip_SIM;
                    sms_port = sms_port_SIM;
                    sms_acid = sms_acid_SIM;
                    sms_systemid = sms_systemid_SIM;
                    sms_password = sms_password_SIM;
                }
                else if (msgtype == "13")
                {
                    sms_provider = sms_provider_VCON;
                    sms_ip = sms_ip_VCON;
                    sms_port = sms_port_VCON;
                    sms_acid = sms_acid_VCON;
                    sms_systemid = sms_systemid_VCON;
                    sms_password = sms_password_VCON;
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else if (msgtype == "15")
                {
                    sms_provider = sms_provider_INT;
                    sms_ip = sms_ip_INT;
                    sms_port = sms_port_INT;
                    sms_acid = sms_acid_INT;
                    sms_systemid = sms_systemid_INT;
                    sms_password = sms_password_INT;
                }
                else if (msgtype == "17" || msgtype == "18")
                {
                    sms_provider = sms_provider_PRIORITY;
                    sms_ip = sms_ip_PRIORITY;
                    sms_port = sms_port_PRIORITY;
                    sms_acid = sms_acid_PRIORITY;
                    sms_systemid = sms_systemid_PRIORITY;
                    sms_password = sms_password_PRIORITY;
                }
                if (userid == "MIM2101450")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }
                //URL Shortning .....
                string lblShortURL = "";
                string segment = ob.GetShortURLofLongURL(userid, LongURL);
                if (segment == "")
                {
                    string sUrl = ob.NewShortURLfromSQL(domain);
                    ob.SaveShortURL(userid, LongURL, "", sUrl, "N", "Y", domain);
                    lblShortURL = domain + sUrl;
                }
                else
                    lblShortURL = segment;
                //URL Shortning .....
                msg = msg.Trim() + " " + lblShortURL;

                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);

                if (client.Status != ConnectionStatus.Bound)
                {
                    System.Threading.Thread.Sleep(1000);
                    try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
                    BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                    if (client.Status != ConnectionStatus.Bound)
                    {
                        System.Threading.Thread.Sleep(1000);
                        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
                        BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        if (client.Status != ConnectionStatus.Bound)
                        {
                            System.Threading.Thread.Sleep(1000);
                            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
                            BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                        }
                    }
                }

                //System.Threading.Thread.Sleep(200);
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                List<SubmitSm> pduList = new List<SubmitSm>();
                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));


                int mobcnt = 0;
                if (templateid == "TEMPLATE-ID")
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg)
                              .AddParameter(0x1400, peid);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                }
                else
                {
                    foreach (var m in mobList)
                    {
                        if (m.Length == 12 || m.Length == 10)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                           .From(sourceAddress)
                           .To(m)
                           .Coding(coding)
                           .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                           .Text(msg)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                }
                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                UnBindResp Uresp = await client.UnbindAsync();
                await client.DisconnectAsync();

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (mobList.Count == 1)
                    return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                else
                {
                    string ss = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    ss = ss.Substring(0, ss.Length - 2);
                    return "SMS Submitted Successfully. " + ss;
                }
                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.sender = sender;
                //obj.mobile = mobile;
                //obj.msg = msg;
                //obj.msgtype = msgtype;
                //obj.connect();
                //string resp = obj.msgid;
                //return ""; // resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int GetMsgCount(string msg)
        {
            string q = msg.Trim();
            //if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
            int count_PIPE = q.Count(f => f == '|');
            int qlen = q.Length + count_PIPE;

            int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;

            int count1 = q.Count(f => f == '{'); qlen = qlen + count1;
            int count2 = q.Count(f => f == '}'); qlen = qlen + count2;
            int count3 = q.Count(f => f == '['); qlen = qlen + count3;
            int count4 = q.Count(f => f == ']'); qlen = qlen + count4;
            int count5 = q.Count(f => f == '^'); qlen = qlen + count5;
            int count6 = q.Count(f => f == '\\'); qlen = qlen + count6;

            int ln = qlen;
            int i = 0;
            if (ln >= 1) i = 1;
            if (ln > 160) i = 2;
            if (ln > 306) i = 3;
            if (ln > 459) i = 4;
            if (ln > 612) i = 5;
            if (ln > 765) i = 6;
            if (ln > 918) i = 7;
            if (ln > 1071) i = 8;
            if (ln > 1224) i = 9;
            if (ln > 1377) i = 10;
            if (ln > 1530) i = 11;
            if (ln > 1683) i = 12;

            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (qlen >= 1) i = 1;
                if (qlen > 70) i = 2;
                if (qlen > 134) i = 3;
                if (qlen > 201) i = 4;
                if (qlen > 268) i = 5;
                if (qlen > 335) i = 6;
                if (qlen > 402) i = 7;
                if (qlen > 469) i = 8;
                if (qlen > 536) i = 9;
                if (qlen > 603) i = 10;
            }

            return i;
        }
        //-------------------------------------------------

        //sendSMSMulti
        //with PEID 
        //        [Route("SendSMSMulti")]
        //        [HttpGet]
        //        public async Task<string> SendSMSMulti(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid)
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
        //                if (userid == null) return "Invalid User ID";
        //                if (pwd == null) return "Invalid Password";
        //                if (mobile == null) return "Invalid Mobile Number";
        //                if (mobile == null) return "Invalid Mobile Number";
        //                if (sender == null) return "Invalid Sender ID";
        //                if (msg == null) return "Invalid Message Text";
        //                if (msgtype == null) return "Invalid Message Type";
        //                if (peid == null) return "Invalid PE-ID";

        //                Util ob = new Util();
        //                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");
        //                //Util ob = new Util();
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
        //                int noofsms = GetMsgCount(msg.Trim());

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
        //                        .From(sourceAddress)
        //                        .To(mobile)
        //                        .DeliveryReceipt()
        //                        .Text(msg)
        //                        .AddParameter(0x1400, peid);
        //                pduList.AddRange(pduBuilder.Create(client));

        //                IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

        //                UnBindResp Uresp = await client.UnbindAsync();
        //                await client.DisconnectAsync();

        //                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
        //                //{
        //                //deduct balance
        //                string s = ob.UpdateAndGetBalance(userid, "", noofsms, rate);
        //                //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, "");
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


        //-----------------------------

        [Route("GetBalance")]
        [HttpGet]
        public string GetBalance(string userid, string pwd)
        {
            try
            {
                //validations
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                string s = ob.GetBalance(userid);
                return "Balance : " + s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("GetBalance")]
        [HttpGet]
        public string GetBalance(string userid, string pwd, string apikey)
        {
            try
            {
                //validations
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                string s = ob.GetBalance(userid);
                return "Balance : " + s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("GetDelivery")]
        [HttpGet]
        public string GetDelivery(string userid, string pwd, string msgid)
        {
            try
            {
                //validations
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (msgid == null) return "Invalid Message ID";
                Util ob = new Util();
                DataTable dt = ob.GetUserParameterScrach(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID.";
                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";

                DataTable dt1 = ob.GetDeliveryScratch(msgid);
                if (dt1.Rows.Count <= 0) return "Invalid Message ID";
                if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != userid.ToUpper()) return "Invalid Message ID.";
                string resp = "";
                if (dt1.Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
                else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
                {
                    resp = "Delivery Status : Delivered";
                    resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                }
                else resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                return resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("GetDelivery")]
        [HttpGet]
        public string GetDelivery(string userid, string pwd, string apikey, string msgid)
        {
            try
            {
                //validations
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (msgid == null) return "Invalid Message ID";
                Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID.";
                if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                DataTable dt1 = ob.GetDelivery(msgid);
                if (dt1.Rows.Count <= 0) return "Invalid Message ID";
                if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != userid.ToUpper()) return "Invalid Message ID.";
                string resp = "";
                if (dt1.Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
                else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
                {
                    resp = "Delivery Status : Delivered"; // Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                }
                else resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                return resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("Shortner2")]
        [HttpGet]
        public string Shortner2(string userid, string pwd, string longURL)
        {
            try
            {
                //validations
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (longURL == null) return "Invalid Long URL";
                if (!(longURL.ToLower().Contains("http"))) return "Invalid Long URL. It should start with http or https.";
                Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["apikey"].ToString()) return "Incorrect Password";

                string segment = ob.GetShortURLofLongURL(userid, longURL);
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("UrlShortner")]
        [HttpGet]
        public string UrlShortner(string userid, string pwd, string longURL)
        {
            if (string.IsNullOrEmpty(userid)) return "Invalid User ID";
            if (string.IsNullOrEmpty(pwd)) return "Invalid Password";
            if (string.IsNullOrEmpty(longURL)) return "Invalid Long URL";
            string shurl = longURL;

            if (!shurl.StartsWith("http://") && !shurl.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";

            string sql = string.Format("insert into APIURLLog (userId,longurl) values('{0}','{1}')", userid, shurl.Replace("'", "''"));
            database.ExecuteNonQuery(sql);

            Util ob = new Util();

            DataTable dt = ob.GetUserParameter(userid);

            if (dt.Rows.Count <= 0) return "Invalid User ID";
            string domain = dt.Rows[0]["domainname"].ToString();

            if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

            //double bal = Convert.ToDouble(dt.Rows[0]["balance"]);
            //double rate = Convert.ToDouble(dt.Rows[0]["urlrate"]);

            //if (bal - rate < 0)
            //{
            //    return "Insufficient Balance. Cannot create Short URL.";
            //}

            string lblShortURL = "";
            string segment = ob.GetShortURLofLongURL(userid, longURL);
            if (segment == "")
            {
                string sUrl = ob.NewShortURLfromSQL(domain);
                ob.SaveShortURL(userid, shurl, "", sUrl, "N", "Y", domain);
                lblShortURL = domain + sUrl;
            }
            else
                lblShortURL = segment;

            //string bal2 = ob.UdateAndGetURLbal1(userid, sUrl);
            //lblURLbal.Text = bal;
            // Session["SMSBAL"] = bal2;

            return "Short URL: " + lblShortURL;
        }


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
