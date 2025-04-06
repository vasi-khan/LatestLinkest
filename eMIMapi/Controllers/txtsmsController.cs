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
using RestSharp;
using Newtonsoft.Json;

namespace eMIMapi.Controllers
{
    [RoutePrefix("txtsms")]
    public class txtsmsController : ApiController
    {
        public bool HomeCR = false;   //   true;    //
        public bool WhitePanel = false;   //    true;    // 
        public int expMin = 60;
        public string sms_provider = "";
        public string sms_ip = "";
        public int sms_port = 0;
        public string sms_acid = "";
        public string sms_systemid = "";
        public string sms_password = "";

        //------------UINVERSAL ACCOUNT 
        //public string sms_provider_UIN = "INFOBIP";
        //public string sms_ip_UIN = "smpp3.infobip.com";
        //public int sms_port_UIN = 8888;
        //public string sms_acid_UIN = "6101";
        //public string sms_systemid_UIN = "MyinboxILDO";
        //public string sms_password_UIN = "Shiva@1906";

        public string sms_provider_UIN = "TUBE";
        public string sms_ip_UIN = "103.205.64.220";
        public int sms_port_UIN = 17220;
        public string sms_acid_UIN = "6102";
        public string sms_systemid_UIN = "MIM21483";
        public string sms_password_UIN = "mim86731";

        //------------PRIORITY ACCOUNT 
        public string sms_provider_PRIORITY = "INFOBIP";
        public string sms_ip_PRIORITY = "smpp3.infobip.com";
        public int sms_port_PRIORITY = 8888;
        public string sms_acid_PRIORITY = "1109";
        public string sms_systemid_PRIORITY = "OTP_Trans";
        public string sms_password_PRIORITY = "Shiva@1906";

        //------------PROMO ACCOUNT FOR NUMERIC SENDERID

        //INFOBIP
        //public string sms_provider_PROMO = "INFOBIP";
        //public string sms_ip_PROMO = "smpp3.infobip.com";
        //public int    sms_port_PROMO = 8888;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "Myinboxpromo5"; // "Promotestdlr";
        //public string sms_password_PROMO = "Shiva@1906";

        //AIRTEL
        public string sms_provider_PROMO = "AIRTEL";
        public string sms_ip_PROMO = "125.19.17.115";
        public int sms_port_PROMO = 2776;
        public string sms_acid_PROMO = "2007";
        public string sms_systemid_PROMO = "SKV_MIB_P3"; // "Promotestdlr"; "Promotestdlr";
        public string sms_password_PROMO = "mib@1234";

        //------------PROMO ACCOUNT FOR NUMERIC SENDERID

        //INFOBIP
        //public string sms_provider_PROMO = "INFOBIP";
        //public string sms_ip_PROMO = "smpp3.infobip.com";
        //public int    sms_port_PROMO = 8888;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "Myinboxpromo5"; // "Promotestdlr";
        //public string sms_password_PROMO = "Shiva@1906";

        //JIO PROMO
        public string sms_provider_PROMO2 = "JIO";
        public string sms_ip_PROMO2 = "185.255.8.176";
        public int sms_port_PROMO2 = 8888;
        public string sms_acid_PROMO2 = "2407";
        public string sms_systemid_PROMO2 = "MyinboxXPROMO"; // "Promotestdlr"; "Promotestdlr";
        public string sms_password_PROMO2 = "Shiva@1906";

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

        //UAE ACCOUNTS - -    msgtype = 16

        public string sms_provider_DUB = "ETISALAT";
        public string sms_ip_DUB = "5.195.193.33";
        public int sms_port_DUB = 9199;
        public string sms_acid_DUB = "1008";
        public string sms_systemid_DUB = "dilip@2506";
        public string sms_password_DUB = "Shiva@1704";

        //--------------------------------------------------------------------------------------------
        //UAE API FOR KARIX TRANS
        public string sms_provider_DUB_2 = "KARIX";
        public string sms_ip_DUB_2 = "smpp.instaalerts.zone";
        public int sms_port_DUB_2 = 14612;
        public string sms_acid_DUB_2 = "1009";
        public string sms_systemid_DUB_2 = "MIMTRANS";
        public string sms_password_DUB_2 = "Shiva@1906";

        //UAE API FOR KARIX PROMO
        public string sms_provider_DUB_3 = "KARIX";
        public string sms_ip_DUB_3 = "smpp.instaalerts.zone";
        public int sms_port_DUB_3 = 14612;
        public string sms_acid_DUB_3 = "2704";
        public string sms_systemid_DUB_3 = "MIMUAEPROMO";
        public string sms_password_DUB_3 = "Shiva@1906";

        //UAE API FOR GUPSHUP TRANS
        public string sms_provider_DUB_4 = "GUPSHUP";
        public string sms_ip_DUB_4 = "202.87.33.182";
        public int sms_port_DUB_4 = 9099;
        public string sms_acid_DUB_4 = "1008";
        public string sms_systemid_DUB_4 = "2000218106";
        public string sms_password_DUB_4 = "9haP!hgf";

        //UAE API FOR GUPSHUP PROMO
        public string sms_provider_DUB_5 = "GUPSHUP";
        public string sms_ip_DUB_5 = "202.87.33.182";
        public int sms_port_DUB_5 = 9099;
        public string sms_acid_DUB_5 = "1007";
        public string sms_systemid_DUB_5 = "2000213142";
        public string sms_password_DUB_5 = "pTv8nFyp";

        //--------------------------------------------------------------------------------------------
        //UAE ACCOUNTS WHITEPANEL - TRANS -    msgtype = 20
        public string sms_provider_WP_UAE = "KARIX";
        public string sms_ip_WP_UAE = "smpp.instaalerts.zone";
        public int sms_port_WP_UAE = 14612;
        public string sms_acid_WP_UAE = "1001";
        public string sms_systemid_WP_UAE = "sunilmimtrans1";
        public string sms_password_WP_UAE = "Shiva@1906";

        //UAE ACCOUNTS WHITEPANEL - PROMO -    msgtype = 30
        public string sms_provider_WP_UAE_P = "KARIX";
        public string sms_ip_WP_UAE_P = "smpp.instaalerts.zone";
        public int sms_port_WP_UAE_P = 14612;
        public string sms_acid_WP_UAE_P = "1001";
        public string sms_systemid_WP_UAE_P = "sunilmimpromo3";
        public string sms_password_WP_UAE_P = "Shiva@1906";


        //KSA ACCOUNTS WHITEPANEL - PROMO -    msgtype = 50
        public string sms_provider_WP_KSA_P = "BROADNET";
        public string sms_ip_WP_KSA_P = "141.94.101.39";
        public int sms_port_WP_KSA_P = 8899;
        public string sms_acid_WP_KSA_P = "301";
        public string sms_systemid_WP_KSA_P = "KSAPROMO";
        public string sms_password_WP_KSA_P = "KSAPROMO";

        //KSA ACCOUNTS WHITEPANEL - TRANS -    msgtype = 40  
        public string sms_provider_WP_KSA_T = "BROADNET";
        public string sms_ip_WP_KSA_T = "141.94.101.39";
        public int sms_port_WP_KSA_T = 8899;
        public string sms_acid_WP_KSA_T = "601";
        public string sms_systemid_WP_KSA_T = "KSATRANS";
        public string sms_password_WP_KSA_T = "ksatrans";
        //--------------------------------------------------------------------------------------------


        // ksa account  -    msgtype = 19
        public string sms_provider_KSA1 = "SMSCOUNTRY";
        public string sms_ip_KSA1 = "182.18.132.7";
        public int sms_port_KSA1 = 2775;
        public string sms_acid_KSA1 = "1909";
        public string sms_systemid_KSA1 = "myinboxmedia2nd";
        public string sms_password_KSA1 = "38940017";

        /* ---------------MONTI ACCOUNT FOR INTERNATIONAL ROUTE ---------- */


        // for dubai account - SM5
        //public string sms_provider_DUB = "ETISALAT";
        //public string sms_ip_DUB = "5.195.193.33";
        //public int sms_port_DUB = 9199;
        //public string sms_acid_DUB = "0305";
        //public string sms_systemid_DUB = "Pixl2906";
        //public string sms_password_DUB = "Pixl@290621";

        //-----------------------------INT

        //public string sms_provider_INT = "MOBIS";
        //public string sms_ip_INT = "180.179.210.40";
        //public int sms_port_INT = 2345;
        //public string sms_acid_INT = "1509";
        //public string sms_systemid_INT = "my_inbx3";
        //public string sms_password_INT = "inbx1921";

        public string sms_provider_INT = "MONTY";
        public string sms_ip_INT = "185.135.128.114";
        public int sms_port_INT = 9003;
        public string sms_acid_INT = "1608";
        public string sms_systemid_INT = "InboxMty";
        public string sms_password_INT = "Medi@mty";


        //public string sms_provider_INT = "MOBIS";
        //public string sms_ip_INT = "180.179.210.40";
        //public int sms_port_INT = 2345;
        //public string sms_acid_INT = "1509";
        //public string sms_systemid_INT = "sgsc";
        //public string sms_password_INT = "Sghbt@qd";

        //-------------------------VCON REPLACED WITH INFOBIP --05-05-2021--------------------->
        //public string sms_ip_VCON = "112.196.55.205";
        //public int sms_port_VCON = 10447;
        //public string sms_acid_VCON = "809";
        //public string sms_systemid_VCON = "MYINTR8";
        //public string sms_password_VCON = "MYIN_888";

        #region <INFOBIP>
        public string sms_provider_info_trans = "INFOBIP";
        public string sms_ip_info_trans = "smpp3.infobip.com";
        public int sms_port_info_trans = 8888;
        public string sms_acid_info_trans = "8301";
        public string sms_systemid_info_trans = "Myinboxtrans16";
        public string sms_password_info_trans = "Shiva@1906";
        #endregion

        #region <airtel>   SE5 IS FOR HONDA SE11 FOR OTHERS
        //public string sms_provider_VCON = "AIRTEL";
        //public string sms_ip_VCON = "125.17.6.26";
        //public int sms_port_VCON = 2790;
        //public string sms_acid_VCON = "8304";
        //public string sms_systemid_VCON = "SKV_MIB_SE5";
        //public string sms_password_VCON = "mib@1234";

        //public string sms_provider_VCON = "AIRTEL";
        //public string sms_ip_VCON = "125.17.6.26";
        //public int sms_port_VCON = 2875;
        //public string sms_acid_VCON = "8302";
        //public string sms_systemid_VCON = "SKV_MIB_SE11";
        //public string sms_password_VCON = "mib@1234";

        public string sms_provider_VCON = "INFOBIP";
        public string sms_ip_VCON = "smpp3.infobip.com";
        public int sms_port_VCON = 8888;
        public string sms_acid_VCON = "8302";
        public string sms_systemid_VCON = "Myinboxtrans16";
        public string sms_password_VCON = "Shiva@1906";

        #endregion

        //HOME CREDIT
        public string sms_provider_HC = "INFOBIP";
        public string sms_ip_HC = "smpp3.infobip.com";
        public int sms_port_HC = 8888;
        public string sms_acid_HC = "8201";
        public string sms_systemid_HC = "Myinboxtrans14";
        public string sms_password_HC = "Shiva@1906";

        //API AIRTEL FOR =>  API TO SMPP
        public string sms_provider_API = "AIRTEL";
        public string sms_ip_API = "125.17.6.26";
        public int sms_port_API = 2875;
        public string sms_acid_API = "1609";
        public string sms_systemid_API = "SKV_MIB_SE9";
        public string sms_password_API = "mib@1234";


        //public string sms_acid_API_2 = "2801";

        //API INFOBIP FOR =>  API TO SMPP
        //public string sms_provider_API = "INFOBIP";
        //public string sms_ip_API = "smpp3.infobip.com";
        //public int sms_port_API = 8888;
        //public string sms_acid_API = "1709";
        //public string sms_systemid_API = "Myinboxtrans3";
        //public string sms_password_API = "Vipinmim@9723";


        //-------------------------VCON REPLACED WITH INFOBIP ----------------------->

        public string sms_provider_INFBP = "INFOBIP";
        public string sms_ip_INFBP = "smpp3.infobip.com";
        public int sms_port_INFBP = 8888;
        public string sms_acid_INFBP = "1709";
        public string sms_systemid_INFBP = "Myinboxtrans3";
        public string sms_password_INFBP = "Vipinmim@9723";

        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        //----------VCON --------------//		
        public string sms_provider_OTP3 = "VCONNEW";
        public string sms_ip_OTP3 = "103.132.145.137";
        public int sms_port_OTP3 = 16375;
        public string sms_acid_OTP3 = "1110";
        public string sms_systemid_OTP3 = "MYINOTP";
        public string sms_password_OTP3 = "MYIN_333";

        //----------INFOBIP --------------//


        //----------INFOBIP OTP --------------//
        public string sms_provider_info_OTP = "INFOBIP";
        public string sms_ip_info_OTP = "smpp3.infobip.com";
        public int sms_port_info_OTP = 8888;
        public string sms_acid_info_OTP = "1409";
        public string sms_systemid_info_OTP = "MyinboxOTP1";
        public string sms_password_info_OTP = "Vipinmim@9723";

        public string sms_provider_OTP = "TELSP";
        public string sms_ip_OTP = "smpp.telsp.io";
        public int sms_port_OTP = 2776;
        public string sms_acid_OTP = "1101";
        public string sms_systemid_OTP = "myinboxotp2";
        public string sms_password_OTP = "k3sote9v";

        public string sms_acid_OTP_2 = "1101";
        //public string sms_provider_OTP = "AIRTEL";
        //public string sms_ip_OTP = "125.19.17.115";
        //public int sms_port_OTP = 2790;
        //public string sms_acid_OTP = "1409";
        //public string sms_systemid_OTP = "SKV_MIB_SE4";
        //public string sms_password_OTP = "mib@1234";

        public string sms_provider_OTP_HC = "INFOBIP";
        public string sms_ip_OTP_HC = "smpp3.infobip.com";
        public int sms_port_OTP_HC = 8888;
        public string sms_acid_OTP_HC = "1409";
        public string sms_systemid_OTP_HC = "MyinboxOTP2";
        public string sms_password_OTP_HC = "Shiva@1906";

        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        // ----- GSM ACCOUNT FOR LINKEXT API ------------------//

        public string sms_provider_SIM = "SIM";
        public string sms_ip_SIM = "167.114.0.183";
        public int sms_port_SIM = 5599;
        public string sms_acid_SIM = "2901";
        public string sms_systemid_SIM = "vipinmim1";
        public string sms_password_SIM = "Vipinmim@1";

        // FirstCry // 
        public string sms_acid_API_ForFirstCry = "2901";

        //with PEID & TemplateID  
        [Route("SMS")]
        [HttpGet]
        public async Task<string> TSMS(string userid, string password, string peid, string templateid, string senderid, string mobile, string message, string msgtype="13", string FailOver = "")
        {
            try
            {
                //if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                //{
                //    if (password.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }

                //    string msgid = GetMsgID();
                //    string sql = "insert into MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,peid,templateid,msgidClient) " +
                //     " VALUES ('" + userid + "',N'" + message.Replace("'", "''") + "','" + mobile + "','" + senderid + "','" + peid + "','" + templateid + "','" + msgid + "')";
                //    database.ExecuteNonQuery(sql);

                //    return "SMS Submitted Successfully. Message ID: " + msgid;
                //}

                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password


                int noofsms = GetMsgCount(message.Trim());
                bool ucs = false;
                if (message.Trim().Any(c => c > 126)) ucs = true;
                Util ob = new Util();
                if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                {
                    if (password.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }
                    sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (userid == "MIM2201009") sms_acid = sms_acid_PROMO2;
                    if (userid == "MIM2201194") sms_acid = sms_acid_API_ForFirstCry;

                    string msgid = GetMsgID();
                    //ob.InsertInAPiLog(userid, mobile, senderid, message, msgtype, peid, templateid, msgid);

                    if (userid == "MIM2201009")
                        ob.AddInMsgQueue3(userid, senderid, mobile, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    //else if (userid == "MIM2201194")
                    //    ob.AddInMsgQueue(userid, senderid, mobile, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    else
                        ob.AddInMsgQueue(userid, senderid, mobile, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);

                    return "SMS Submitted Successfully. Message ID: " + msgid;
                }

                if (userid == null) return "Invalid User ID";
                if (mobile == null) return "Invalid Mobile Number";
                if (password == null) return "Invalid Password";

                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                if (userid.ToUpper() != "MIM2201011")            // SKIP VALIDATION 01/10/2022
                {
                    if (senderid == null) return "Invalid senderid ID";
                    if (message == null) return "Invalid Message Text";
                    if (msgtype == null) return "Invalid Message Type";
                    if (peid == null) return "Invalid PE-ID";
                    if (templateid == null) return "Invalid Template ID";

                    if (!(userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
                        if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                    //validation of list count
                    if (userid.ToUpper() == "MIM2201078")
                    { if (mobList.Count > 500) { return "Mobile numbers cannot be more than 500"; } }
                    else
                    { if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; } }

                    //if (!(userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
                    ob.InsertInAPiLog(userid, mobile, senderid, message, msgtype, peid, templateid);
                }

                //Util ob = new Util();
                DataTable dt = ob.GetUserParameter(userid);

                if (userid.ToUpper() != "MIM2201011")                            // SKIP VALIDATION 01/10/2022   
                {
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (password != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                    //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                    if (msgtype != "15" && msgtype != "16")
                        if (mobile.Length < 10) return "Invalid Mobile Number.";
                    //double Num;
                    //bool isNum = double.TryParse(mobile, out Num);
                    //if (!isNum) return "Invalid Mobile Number.";

                    //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                    if (message.Trim() == "") return "Invalid Message Text";
                }

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;


                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                { return "Insufficient Balance"; }

                //check valid senderid id
                if (!(userid.ToUpper() == "DEMO4852" || userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
                    if (!ob.CheckSenderId(userid, senderid)) return "Invalid senderid ID";

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


                    //if (userid == "MIM2101643")
                    //{
                    //    sms_provider = sms_provider_info_trans;
                    //    sms_ip = sms_ip_info_trans;
                    //    sms_port = sms_port_info_trans;
                    //    sms_acid = sms_acid_info_trans;
                    //    sms_systemid = sms_systemid_info_trans;
                    //    sms_password = sms_password_info_trans;
                    //}
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;

                    if (userid == "MIM2201143")
                    {
                        sms_provider = sms_provider_OTP3;
                        sms_ip = sms_ip_OTP3;
                        sms_port = sms_port_OTP3;
                        sms_acid = sms_acid_OTP3;
                        sms_systemid = sms_systemid_OTP3;
                        sms_password = sms_password_OTP3;
                    }

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
                if (userid.ToUpper() == "DEMO4852" || userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101277" || userid == "MIM2101450" || userid == "MIM2101650"
                    || userid == "MIM2102201" || senderid.ToUpper() == "HMISVR")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }
                bool isNumeric = false;
                if (!(userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
                    isNumeric = long.TryParse(Convert.ToString(senderid).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }
                //if (msgtype == "33")
                //{
                //    string msgid = "";
                //    string ss = "";
                //    foreach (var m in mobList)
                //    {
                //        msgid = GetMsgID();
                //        ob.AddInMsgQueueOTP(userid, senderid, m, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
                //        ss += "MobileNo: " + m.ToString() + " Message ID: " + msgid + ", ";
                //    }
                //    if (mobList.Count == 1)
                //        return "SMS Submitted Successfully. Message ID: " + msgid;
                //    else
                //    {
                //        ss = ss.Substring(0, ss.Length - 2);
                //        return "SMS Submitted Successfully. " + ss;
                //    }
                //}
                if (userid.ToUpper() == "DEMO4852" || userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101450" || userid == "MIM2101277" || (msgtype == "33" && userid != "MIM2201143") || (WhitePanel && msgtype == "13"))
                {
                    //if (userid != "MIM2101643")
                    //{
                    if (WhitePanel)
                    {
                        if (isNumeric)
                            sms_acid = "2001";
                        else if (msgtype == "13") sms_acid = "1601";
                        else if (msgtype == "33") sms_acid = "1101";
                    }
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, senderid, m, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
                        ss += "MobileNo: " + m.ToString() + " Message ID: " + msgid + ", ";
                    }
                    if (mobList.Count == 1)
                        return "SMS Submitted Successfully. Message ID: " + msgid;
                    else
                    {
                        ss = ss.Substring(0, ss.Length - 2);
                        return "SMS Submitted Successfully. " + ss;
                    }
                    // }
                }
                else if (userid == "MIM2201009")
                {
                    sms_provider = sms_provider_PROMO2;
                    sms_ip = sms_ip_PROMO2;
                    sms_port = sms_port_PROMO2;
                    sms_acid = sms_acid_PROMO2;
                    sms_systemid = sms_systemid_PROMO2;
                    sms_password = sms_password_PROMO2;

                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        //msgid1 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueue3(userid, senderid, m, message.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
                        ss1 += "MobileNo: " + m.ToString() + " Message ID: " + msgid1 + ", ";
                    }
                    if (mobList.Count == 1)
                        return "SMS Submitted Successfully. Message ID: " + msgid1;
                    else
                    {
                        ss1 = ss1.Substring(0, ss1.Length - 2);
                        return "SMS Submitted Successfully. " + ss1;
                    }
                }
                if (senderid.ToUpper() == "HMISVR")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, senderid, m, message.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
                        ss2 += "MobileNo: " + m.ToString() + " Message ID: " + msgid2 + ", ";
                    }
                    if (mobList.Count == 1)
                        return "SMS Submitted Successfully. Message ID: " + msgid2;
                    else
                    {
                        ss2 = ss2.Substring(0, ss2.Length - 2);
                        return "SMS Submitted Successfully. " + ss2;
                    }
                }

                //string msgid = "";
                //string ss = "";
                //foreach (var m in mobList)
                //{
                //    msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                //    ob.AddInMsgQueue(userid, senderid, m, message.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs,templateid);
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
                //if (!ob.CheckTemplateIdSenderId(userid, senderid, templateid)) return "Invalid Template ID";


                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(senderid, message, ucs);


                    if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                        string e_tempid = ar1[0];
                        string e_peid = peid;
                        string e_sender = senderid;

                        errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + e_tempid + "' and peid='" + e_peid + "'"));

                        if (errcd_ != "")
                        {
                            templID = "";
                        }
                    }


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
                                string smsTex = ob.GetSMSText(message, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + senderid + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + message.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + senderid + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
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
                else
                {
                    if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string e_peid = peid;
                        string e_sender = senderid;
                        errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + templateid + "' and peid='" + e_peid + "'"));

                        if (errcd_ != "")
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
                                    string smsTex = ob.GetSMSText(message, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + senderid + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + message.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'vcon','" + sms_acid + "','" + userid + "',N'" + smsTex + "','" + m + "','" + senderid + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                    " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
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
                    }
                }
                // SAVE SMS IN QUEUE

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
                var sourceAddress = new SmeAddress(senderid, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

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
                           .Text(message)
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
                           .Text(message)
                           .AddParameter(0x1400, peid)
                           .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(message)
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
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, senderid, mobile, message.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (FailOver != "")
                {
                    ob.AddInFailOver(userid, mobile, message.Replace("'", "''"), Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), FailOver,"");
                }
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

                //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                //{

                //}
                //else
                //{
                //return resp;
                //}
                //    smpp obj = new smpp();
                //obj.userid = userid;
                //obj.senderid = senderid;
                //obj.mobile = mobile;
                //obj.message = message;
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

        public string GetMsgID()
        {
            return 'S' + Convert.ToString(Guid.NewGuid());
        }

        public int GetMsgCount(string message)
        {
            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") HomeCR = true;
            string q = message.Trim();
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
            if (ln > 1836) i = 13;
            if (ln > 1989) i = 14;
            if (ln > 2142) i = 15;
            if (ln > 2295) i = 16;
            if (ln > 2448) i = 17;
            if (ln > 2601) i = 18;
            if (ln > 2754) i = 19;
            if (ln > 2907) i = 20;
            if (ln > 3060) i = 21;
            if (ln > 3213) i = 22;
            if (ln > 3366) i = 23;
            if (ln > 3519) i = 24;
            if (ln > 3672) i = 25;
            if (ln > 3825) i = 26;
            if (ln > 3978) i = 27;
            if (ln > 4131) i = 28;

            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (qlen >= 1) i = 1;
                if (qlen > 70) i = 2;
                if (qlen > 133) i = 3;
                if (qlen > 196) i = 4;
                if (qlen > 259) i = 5;
                if (qlen > 322) i = 6;
                if (qlen > 385) i = 7;
                if (qlen > 448) i = 8;
                if (qlen > 536) i = 9;
                if (qlen > 511) i = 10;
                if (qlen > 574) i = 11;
                if (qlen > 637) i = 12;
                if (qlen > 700) i = 13;
                if (qlen > 763) i = 14;
                if (qlen > 826) i = 15;
                if (qlen > 889) i = 16;
                if (qlen > 952) i = 17;
                if (qlen > 1015) i = 18;
                if (qlen > 1078) i = 19;
                if (qlen > 1141) i = 20;
                if (qlen > 1204) i = 21;
                if (qlen > 1267) i = 22;
                if (qlen > 1330) i = 23;
                if (qlen > 1393) i = 24;
                if (qlen > 1456) i = 25;
                if (qlen > 1519) i = 26;
                if (qlen > 1582) i = 27;
                if (qlen > 1645) i = 28;
                if (qlen > 1708) i = 29;
                if (qlen > 1771) i = 30;
                if (qlen > 1834) i = 31;
                if (qlen > 1897) i = 32;
                if (qlen > 1960) i = 33;
                if (qlen > 2023) i = 34;
                if (qlen > 2086) i = 35;
                if (qlen > 2149) i = 36;
                if (qlen > 2212) i = 37;
                if (qlen > 2275) i = 38;
                if (qlen > 2338) i = 39;
                if (qlen > 2401) i = 40;
                if (qlen > 2464) i = 41;
                if (qlen > 2527) i = 42;
                if (qlen > 2590) i = 43;
                if (qlen > 2653) i = 44;
                if (qlen > 2716) i = 45;
                if (qlen > 2779) i = 46;
                if (qlen > 2842) i = 47;
                if (qlen > 2905) i = 48;
                if (qlen > 2968) i = 49;
                if (qlen > 3031) i = 50;
            }

            return i;
        }

        private void OnCertificateValidation(object senderid, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private async void client_evDisconnected(object senderid)
        {

            //_log.Info("SmppClient disconnected");

            //SmppClient client = (SmppClient)senderid;
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

        private void ClientOnRecoverySucceeded(object senderid, BindResp data)
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

        private void client_evDeliverSm(object senderid, DeliverSm data)
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

        private void client_evDataSm(object senderid, DataSm data)
        {
            //_log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object senderid, MessageEventHandlerArgs args)
        {
            //_log.Info("Incomplete message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", Text: " + args.Text);
        }

        private void OnFullMessageReceived(object senderid, MessageEventHandlerArgs args)
        {
            //_log.Info("Full message received From: " + args.GetFirst<DeliverSm>().SourceAddress + ", To: " + args.GetFirst<DeliverSm>().DestinationAddress + ", Text: " + args.Text);
        }

        private void client_evEnquireLink(object senderid, EnquireLink data)
        {
            // _log.Info("EnquireLink received");
        }

        private void client_evUnBind(object senderid, UnBind data)
        {
            //_log.Info("UnBind request received");
        }

    }
}
