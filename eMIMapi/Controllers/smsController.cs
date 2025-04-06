using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using eMIMapi.Helper;
using System.Data;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;
using System.Security.Authentication;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.PDU;

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/sms")]
    public class smsController : ApiController
    {
        bool bSecure = ConfigurationManager.AppSettings["SECURITY"].ToString() == "Y" ? true : false;
        public bool HomeCR = false; //   true;   // 
        public bool WhitePanel = false;   //true;    // 
        public int expMin = 60;

        public string sms_provider = "";
        public string sms_ip = "";
        public int sms_port = 0;
        public string sms_acid = "";
        public string sms_systemid = "";
        public string sms_password = "";


        //========================================================================================================================================

        //JIO PROMO  for 103.205.64.64
        public string sms_provider_PROMO = "JIO";
        public string sms_ip_PROMO = "185.255.8.176";
        public int sms_port_PROMO = 8888;
        public string sms_acid_PROMO = "2407";
        public string sms_systemid_PROMO = "MyinboxXPROMO"; // "Promotestdlr"; "Promotestdlr";
        public string sms_password_PROMO = "Shiva@1906";

        //PROMO  for AWS
        //public string sms_provider_PROMO = "AIRTEL_TELSP";
        //public string sms_ip_PROMO = "124.30.18.27";
        //public int sms_port_PROMO = 6161;
        //public string sms_acid_PROMO = "2409";
        //public string sms_systemid_PROMO = "myinboxpr"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO = "vKXh6YGy";

        //========================================================================================================================================

        //UAE API FOR KARIX TRANS for 103.205.64.64
        public string sms_provider_DUB_2 = "KARIX";
        public string sms_ip_DUB_2 = "smpp.instaalerts.zone";
        public int sms_port_DUB_2 = 14612;
        public string sms_acid_DUB_2 = "3501";
        public string sms_systemid_DUB_2 = "MIMTRANS";
        public string sms_password_DUB_2 = "Shiva@1906";

        //UAE API FOR KARIX TRANS FOR AWS
        //public string sms_provider_DUB_2 = "KARIX";
        //public string sms_ip_DUB_2 = "smpp.instaalerts.zone";
        //public int sms_port_DUB_2 = 14612;
        //public string sms_acid_DUB_2 = "3509";
        //public string sms_systemid_DUB_2 = "sunilmimtrans3";
        //public string sms_password_DUB_2 = "Shiva@1906";

        //UAE API FOR KARIX PROMO for 103.205.64.64
        public string sms_provider_DUB_3 = "KARIX";
        public string sms_ip_DUB_3 = "smpp.instaalerts.zone";
        public int sms_port_DUB_3 = 14612;
        public string sms_acid_DUB_3 = "2801";
        public string sms_systemid_DUB_3 = "MIMUAEPROMO";
        public string sms_password_DUB_3 = "Shiva@1906";

        //UAE API FOR KARIX PROMO for AWS
        //public string sms_provider_DUB_3 = "KARIX";
        //public string sms_ip_DUB_3 = "smpp.instaalerts.zone";
        //public int sms_port_DUB_3 = 14612;
        //public string sms_acid_DUB_3 = "2809";
        //public string sms_systemid_DUB_3 = "mimuaeawpromo";
        //public string sms_password_DUB_3 = "Mim@1234";

        //UAE API FOR GUPSHUP TRANS for 103.205.64.64
        public string sms_provider_DUB_4 = "GUPSHUP";
        public string sms_ip_DUB_4 = "202.87.33.182";
        public int sms_port_DUB_4 = 9099;
        public string sms_acid_DUB_4 = "1201";
        public string sms_systemid_DUB_4 = "2000218106";
        public string sms_password_DUB_4 = "yGXx6ebw";

        //UAE API FOR GUPSHUP TRANS for AWS
        //public string sms_provider_DUB_4 = "GUPSHUP";
        //public string sms_ip_DUB_4 = "202.87.33.182";
        //public int sms_port_DUB_4 = 9099;
        //public string sms_acid_DUB_4 = "1209";
        //public string sms_systemid_DUB_4 = "2000220450";
        //public string sms_password_DUB_4 = "X7NcF2zR";

        //UAE API FOR GUPSHUP PROMO for 103.205.64.64
        public string sms_provider_DUB_5 = "GUPSHUP";
        public string sms_ip_DUB_5 = "202.87.33.182";
        public int sms_port_DUB_5 = 9099;
        public string sms_acid_DUB_5 = "0501";
        public string sms_systemid_DUB_5 = "2000213142";
        public string sms_password_DUB_5 = "pTv8nFyp";

        //UAE API FOR GUPSHUP PROMO for AWS
        //public string sms_provider_DUB_5 = "GUPSHUP";
        //public string sms_ip_DUB_5 = "202.87.33.182";
        //public int sms_port_DUB_5 = 9099;
        //public string sms_acid_DUB_5 = "0509";
        //public string sms_systemid_DUB_5 = "2000220451";
        //public string sms_password_DUB_5 = "6h*2z9P6";

        //========================================================================================================================================

        #region < TELSP - for 103.205.64.64 > 
        public string sms_provider_VCON = "AIRTEL_TELSP";
        public string sms_ip_VCON = "124.30.18.27";
        public int sms_port_VCON = 6161;
        public string sms_acid_VCON = "8001";
        public string sms_systemid_VCON = "myinboxtrnew7";
        public string sms_password_VCON = "Ld5389KW";

        //public string sms_provider_VCON = "AIRTEL_JIO";
        //public string sms_ip_VCON = "185.255.8.176";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "1609";
        //public string sms_systemid_VCON = "MyinboxTRN2";
        //public string sms_password_VCON = "Shiva@1906";
        #endregion

        #region < TELSP - for AWS > 
        //public string sms_provider_VCON = "AIRTEL_TELSP";
        //public string sms_ip_VCON = "124.30.18.27";
        //public int sms_port_VCON = 6161;
        //public string sms_acid_VCON = "1609";
        //public string sms_systemid_VCON = "myinboxtr11";
        //public string sms_password_VCON = "Nktwj8dW";

        //public string sms_provider_VCON = "JIO";
        //public string sms_ip_VCON = "185.255.8.176";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "1609";
        //public string sms_systemid_VCON = "MYINBOXTRN12";
        //public string sms_password_VCON = "Myi@#987";
        #endregion

        //========================================================================================================================================

        #region < TELSP OTP for 103.205.64.64 >
        public string sms_provider_OTP = "AIRTEL_TELSP";
        public string sms_ip_OTP = "124.30.18.27";
        public int sms_port_OTP = 6161;
        public string sms_acid_OTP = "1109";
        public string sms_systemid_OTP = "myinboxotp8";
        public string sms_password_OTP = "gcp1tttF";

        //public string sms_provider_OTP = "AIRTEL_JIO";
        //public string sms_ip_OTP = "185.255.8.176";
        //public int sms_port_OTP = 8888;
        //public string sms_acid_OTP = "1109";
        //public string sms_systemid_OTP = "MyinboxXOTP1";
        //public string sms_password_OTP = "Shiva@1906";
        #endregion

        #region < TELSP OTP for AWS >
        //public string sms_provider_OTP = "AIRTEL_TELSP";
        //public string sms_ip_OTP = "124.30.18.27";
        //public int sms_port_OTP = 6161;
        //public string sms_acid_OTP = "1109";
        //public string sms_systemid_OTP = "myinboxotp10";
        //public string sms_password_OTP = "JJausxdL";

        //public string sms_provider_OTP = "JIO";
        //public string sms_ip_OTP = "185.255.8.176";
        //public int sms_port_OTP = 8888;
        //public string sms_acid_OTP = "1109";
        //public string sms_systemid_OTP = "MyinboxOTP6";
        //public string sms_password_OTP = "Shiva@1906";
        #endregion

        //========================================================================================================================================

        //QATAR START
        //QATAR for 103.205.64.64
        public string sms_provider_QTR = "Broadnet";
        public string sms_ip_QTR = "141.94.101.39";
        public int sms_port_QTR = 8899;
        public string sms_acid_QTR = "1301";
        public string sms_systemid_QTR = "MyInBhRn";
        public string sms_password_QTR = "My9748";

        //QATAR for AWS
        //public string sms_provider_QTR = "Broadnet";
        //public string sms_ip_QTR = "51.210.118.154";
        //public int sms_port_QTR = 8899;
        //public string sms_acid_QTR = "1309";
        //public string sms_systemid_QTR = "MIMQatrTR";
        //public string sms_password_QTR = "Shiva@1906";

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================

        //========================================================================================================================================
        //------------UINVERSAL ACCOUNT 
        public string sms_provider_UIN = "INFOBIP";
        public string sms_ip_UIN = "smpp3.infobip.com";
        public int sms_port_UIN = 8888;
        public string sms_acid_UIN = "1109";
        public string sms_systemid_UIN = "OTP_Trans";
        public string sms_password_UIN = "Shiva@1906";

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
        //public int sms_port_PROMO = 8888;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "Myinboxpromo5"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO = "Shiva@1906";

        //AIRTEL
        //public string sms_provider_PROMO = "AIRTEL";
        //public string sms_ip_PROMO = "125.19.17.115";
        //public int sms_port_PROMO = 2776;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "SKV_MIB_P3"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO = "mib@1234";

        //public string sms_provider_PROMO = "JIO";
        //public string sms_ip_PROMO = "185.255.8.176";
        //public int sms_port_PROMO = 8888;
        //public string sms_acid_PROMO = "2407";
        //public string sms_systemid_PROMO = "MyinboxXPROMO"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO = "Shiva@1906";



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

        //public string sms_provider_DUB = "ETISALAT";
        //public string sms_ip_DUB = "5.195.193.33";
        //public int sms_port_DUB = 9199;
        //public string sms_acid_DUB = "1008";
        //public string sms_systemid_DUB = "dilip@2506";
        //public string sms_password_DUB = "Shiva@1704";

        public string sms_provider_DUB = "ETISALAT";
        public string sms_ip_DUB = "86.96.241.54";
        public int sms_port_DUB = 2775;
        public string sms_acid_DUB = "1509";
        public string sms_systemid_DUB = "MiMDXB";
        public string sms_password_DUB = "Nzjocp6&";

        //public string sms_provider_DUB = "ETISALAT";
        //public string sms_ip_DUB = "86.96.241.54";
        //public int sms_port_DUB = 2775;
        //public string sms_acid_DUB = "2309";
        //public string sms_systemid_DUB = "mim";
        //public string sms_password_DUB = "Nzjocp6&";

        //--------------------------------------------------------------------------------------------




        //--------------------------------------------------------------------------------------------
        // ksa account

        // ksa account  -    msgtype = 19
        //public string sms_provider_KSA1 = "SMSCOUNTRY";
        //public string sms_ip_KSA1 = "182.18.132.7";
        //public int sms_port_KSA1 = 2775;
        //public string sms_acid_KSA1 = "1909";
        //public string sms_systemid_KSA1 = "myinboxmedia2nd";
        //public string sms_password_KSA1 = "38940017";

        // ksa account  -    msgtype = 19
        public string sms_provider_KSA1 = "BROADNTKSA";
        public string sms_ip_KSA1 = "141.94.101.39";
        public int sms_port_KSA1 = 8899;
        public string sms_acid_KSA1 = "1510";
        public string sms_systemid_KSA1 = "UAEPROMO";
        public string sms_password_KSA1 = "uaepromo";


        //-------------------------VCON REPLACED WITH INFOBIP --05-05-2021--------------------->
        //public string sms_ip_VCON = "112.196.55.205";
        //public int sms_port_VCON = 10447;
        //public string sms_acid_VCON = "809";
        //public string sms_systemid_VCON = "MYINTR8";
        //public string sms_password_VCON = "MYIN_888";



        #region < JIO >
        //public string sms_provider_VCON = "JIO";
        //public string sms_ip_VCON = "185.255.8.176";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "8809";
        //public string sms_systemid_VCON = "MyinboxTRN4";
        //public string sms_password_VCON = "Shiva@1906";
        #endregion


        #region < INFOBIP >
        //public string sms_provider_VCON = "INFOBIP";
        //public string sms_ip_VCON = "smpp3.infobip.com";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "8301";
        //public string sms_systemid_VCON = "Myinboxtrans10";
        //public string sms_password_VCON = "Shiva@1906";
        #endregion

        #region < AIRTEL >
        //public string sms_provider_VCON = "AIRTEL";
        //public string sms_ip_VCON = "125.17.6.26";
        //public int sms_port_VCON = 2875;
        //public string sms_acid_VCON = "8601";
        //public string sms_systemid_VCON = "SKV_MIB_SE11";
        //public string sms_password_VCON = "mib@1234";
        #endregion

        //public string sms_provider_VCON = "AIRTEL";
        //public string sms_ip_VCON = "125.19.17.115";
        //public int sms_port_VCON = 2790;
        //public string sms_acid_VCON = "8201";
        //public string sms_systemid_VCON = "SKV_MIB_SE4";
        //public string sms_password_VCON = "mib@1234";

        #region <  HOME CREDIT >
        public string sms_provider_HC = "INFOBIP";
        public string sms_ip_HC = "smpp3.infobip.com";
        public int sms_port_HC = 8888;
        public string sms_acid_HC = "8201";
        public string sms_systemid_HC = "Myinboxtrans14";
        public string sms_password_HC = "Shiva@1906";

        public string sms_provider_HC2 = "INFOBIP";
        public string sms_ip_HC2 = "smpp3.infobip.com";
        public int sms_port_HC2 = 8888;
        public string sms_acid_HC2 = "8301";
        public string sms_systemid_HC2 = "Myinboxtrans11";
        public string sms_password_HC2 = "Shiva@1906";
        #endregion

        //API AIRTEL FOR =>  API TO SMPP
        public string sms_provider_API = "AIRTEL";
        public string sms_ip_API = "125.17.6.26";
        public int sms_port_API = 2875;
        public string sms_acid_API = "1609";
        public string sms_systemid_API = "SKV_MIB_SE9";
        public string sms_password_API = "mib@1234";

        //API INFOBIP FOR =>  API TO SMPP
        //public string sms_provider_API = "INFOBIP";
        //public string sms_ip_API = "smpp3.infobip.com";
        //public int sms_port_API = 8888;
        //public string sms_acid_API = "1709";
        //public string sms_systemid_API = "Myinboxtrans3";
        //public string sms_password_API = "Vipinmim@9723";

        //-------------------------VCON REPLACED WITH INFOBIP ----------------------->

        //-----------------------------INT

        public string sms_provider_INT = "MOBIS";
        public string sms_ip_INT = "180.179.210.40";
        public int sms_port_INT = 2345;
        public string sms_acid_INT = "1509";
        public string sms_systemid_INT = "my_inbx3";
        public string sms_password_INT = "inbx1921";


        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        //----------VCON --------------//
        //public string sms_ip_OTP = "112.196.55.201";
        //public int sms_port_OTP = 16402;
        //public string sms_acid_OTP = "1409";
        //public string sms_systemid_OTP = "MYIOTP1";
        //public string sms_password_OTP = "MYIN_999";

        //----------INFOBIP --------------//

        //public string sms_provider_OTP = "INFOBIP";
        //public string sms_ip_OTP = "smpp3.infobip.com";
        //public int sms_port_OTP = 8888;
        //public string sms_acid_OTP = "1109";
        //public string sms_systemid_OTP = "MyinboxOTP1";
        //public string sms_password_OTP = "Vipinmim@9723";



        //public string sms_provider_OTP = "AIRTEL";
        //public string sms_ip_OTP = "125.19.17.115";
        //public int sms_port_OTP = 2790;
        //public string sms_acid_OTP = "1409";
        //public string sms_systemid_OTP = "SKV_MIB_SE4";
        //public string sms_password_OTP = "mib@1234";

        #region < Home Credit >
        //public string sms_provider_OTP_HC = "INFOBIP";
        //public string sms_ip_OTP_HC = "smpp3.infobip.com";
        //public int sms_port_OTP_HC = 8888;
        //public string sms_acid_OTP_HC = "1409";
        //public string sms_systemid_OTP_HC = "MyinboxOTP2";
        //public string sms_password_OTP_HC = "Shiva@1906";

        public string sms_provider_OTP_HC = "INFOBIP";
        public string sms_ip_OTP_HC = "smpp3.infobip.com";
        public int sms_port_OTP_HC = 8888;
        public string sms_acid_OTP_HC = "1407";
        public string sms_systemid_OTP_HC = "OTPTrans3";
        public string sms_password_OTP_HC = "Shiva@1906";
        #endregion

        // ----- OTP ACCOUNT FOR LINKEXT API ------------------//

        public string sms_provider_SIM = "SIM";
        public string sms_ip_SIM = "167.114.0.183";
        public int sms_port_SIM = 5599;
        public string sms_acid_SIM = "2901";
        public string sms_systemid_SIM = "vipinmim1";
        public string sms_password_SIM = "Vipinmim@1";

        //UAE ACCOUNTS WHITEPANEL - -    msgtype = 20
        //public string sms_provider_WP_UAE = "KARIX";
        //public string sms_ip_WP_UAE = "smpp.instaalerts.zone";
        //public int sms_port_WP_UAE = 14612;
        //public string sms_acid_WP_UAE = "1001";
        //public string sms_systemid_WP_UAE = "sunilmimtrans1";
        //public string sms_password_WP_UAE = "Shiva@1906";
        public string sms_provider_WP_UAE = "MIM";
        public string sms_ip_WP_UAE = "103.205.64.220";
        public int sms_port_WP_UAE = 17225;
        public string sms_acid_WP_UAE = "1001";
        public string sms_systemid_WP_UAE = "MIM21484";
        public string sms_password_WP_UAE = "mim86731";

        //UAE ACCOUNTS WHITEPANEL - -    msgtype = 30
        //public string sms_provider_WP_UAE_P = "KARIX";
        //public string sms_ip_WP_UAE_P = "smpp.instaalerts.zone";
        //public int sms_port_WP_UAE_P = 14612;
        //public string sms_acid_WP_UAE_P = "1001";
        //public string sms_systemid_WP_UAE_P = "sunilmimpromo3";
        //public string sms_password_WP_UAE_P = "Shiva@1906";
        public string sms_provider_WP_UAE_P = "MIM";
        public string sms_ip_WP_UAE_P = "103.205.64.220";
        public int sms_port_WP_UAE_P = 17225;
        public string sms_acid_WP_UAE_P = "1001";
        public string sms_systemid_WP_UAE_P = "MIM21484";
        public string sms_password_WP_UAE_P = "mim86731";


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


        public string sms_provider_QTR2 = "TOBEPRECISESMS";
        public string sms_ip_QTR2 = "smpp1.tobeprecisesms.com";
        public int sms_port_QTR2 = 2775;
        public string sms_acid_QTR2 = "1509";
        public string sms_systemid_QTR2 = "myinboxtrans";
        public string sms_password_QTR2 = "mynb6535";
        //QATAR END

        // GET: api/sms
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetTest")]
        [HttpGet]
        public HttpResponseMessage GetTest()
        {
            string yourJson = "Testing JSON Output";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            DataTable dt1 = new DataTable("dt");
            dt1.Columns.Add("Response");
            DataRow dr = dt1.NewRow();
            dr[0] = yourJson;
            dt1.Rows.Add(dr);
            yourJson = JsonConvert.SerializeObject(dt1);
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted   india and UAE
        [Route("SendSMSMulti_NEW")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMSMulti_NEW([FromBody] clsSMS ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            //clsSMS ob = JsonConvert.DeserializeObject<clsSMS>(param);

            string userid;
            string pwd;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string peid;
            userid = ob.userid;
            pwd = ob.pwd;
            mobile = ob.mobile;
            sender = ob.sender;
            msg = ob.msg;
            msgtype = ob.msgtype;
            peid = ob.peid;


            if (peid == null) { yourJson = "Invalid PEID"; ret = true; }
            if (msgtype == null) { yourJson = "Invalid Message Type"; ret = true; }
            if (msg == null) { yourJson = "Invalid Message Text"; ret = true; }
            if (sender == null) { yourJson = "Invalid senderid"; ret = true; }
            if (mobile == null) { yourJson = "Invalid mobile no"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            if (Convert.ToString(peid).Trim() == "") { yourJson = "Invalid PEID"; ret = true; }
            if (Convert.ToString(msgtype).Trim() == "") { yourJson = "Invalid Message Type"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (Convert.ToString(sender).Trim() == "") { yourJson = "Invalid senderid"; ret = true; }
            if (Convert.ToString(mobile).Trim() == "") { yourJson = "Invalid mobile no"; ret = true; }
            if (Convert.ToString(pwd).Trim() == "") { yourJson = "Invalid Password"; ret = true; }
            if (Convert.ToString(userid).Trim() == "") { yourJson = "Invalid User ID"; ret = true; }

            if (msgtype != "15" && msgtype != "16")
                if (mobile.Length < 10) { yourJson = "Invalid mobile no"; ret = true; }

            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

            Util obU = new Util();
            if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

            //validation of list count
            if (mobList.Count > 50) { yourJson = "Mobile numbers cannot be more than 50"; ret = true; }

            string templateid = "";

            obU.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
            DataTable dt = new DataTable();
            double rate = 0;
            int noofsms = 0;
            bool ucs = false;
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

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

            if (!ret)
            {
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = obU.GetUserParameterSecure(userid, pwd);//For ApiKey
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson = "Invalid Credentials"; ret = true;
                        }
                }
                // end
                else
                {
                    dt = obU.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }


                    if (!ret)
                        if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                }
                if (!ret)
                    if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18"))
                    { yourJson = "Invalid Message Type"; ret = true; }

                if (!ret)
                {
                    //check balance
                    noofsms = GetMsgCount(msg.Trim());
                    if (msg.Trim().Any(c => c > 126)) ucs = true;

                    if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                    if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                    if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                    if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                    //if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms * mobList.Count))
                    #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                    if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                    {
                        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                        { yourJson = "Insufficient Balance"; ret = true; }
                    }
                    #endregion

                }

                //check valid sender id
                if (!ret)
                {
                    if (!obU.CheckSenderId(userid, sender))
                    { yourJson = "Invalid Sender ID"; ret = true; }
                }

                if (!ret)
                {
                    string msgid = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        obU.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);

                        DataRow dr1 = dtRes.NewRow();
                        dr1["Mobile"] = m;
                        dr1["MessageID"] = msgid;
                        dtRes.Rows.Add(dr1);
                    }
                }
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted
        [Route("SendSMSMulti")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMSMulti([FromBody] clsSMS ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") HomeCR = true;
            //clsSMS ob = JsonConvert.DeserializeObject<clsSMS>(param);

            string userid;
            string pwd;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string peid;
            userid = ob.userid;
            pwd = ob.pwd;
            mobile = ob.mobile;
            sender = ob.sender;
            msg = ob.msg;
            msgtype = ob.msgtype;
            peid = ob.peid;

            if (peid == null) { yourJson = "Invalid PEID"; ret = true; }
            if (msgtype == null) { yourJson = "Invalid Message Type"; ret = true; }
            if (msg == null) { yourJson = "Invalid Message Text"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (sender == null) { yourJson = "Invalid senderid"; ret = true; }
            if (mobile == null) { yourJson = "Invalid mobile no"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));

            Util obU = new Util();
            if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

            //validation of list count
            if (mobList.Count > 50) { yourJson = "Mobile numbers cannot be more than 50"; ret = true; }

            string templateid = "";

            obU.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
            DataTable dt = new DataTable();
            double rate = 0;
            int noofsms = 0;
            bool ucs = false;
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

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

                if (HomeCR)
                {
                    sms_provider = sms_provider_HC;
                    sms_ip = sms_ip_HC;
                    sms_port = sms_port_HC;
                    sms_acid = sms_acid_HC;
                    sms_systemid = sms_systemid_HC;
                    sms_password = sms_password_HC;
                }
            }
            else if (msgtype == "33")
            {
                sms_provider = sms_provider_OTP;
                sms_ip = sms_ip_OTP;
                sms_port = sms_port_OTP;
                sms_acid = sms_acid_OTP;
                sms_systemid = sms_systemid_OTP;
                sms_password = sms_password_OTP;
                if (HomeCR)
                {
                    sms_provider = sms_provider_OTP_HC;
                    sms_ip = sms_ip_OTP_HC;
                    sms_port = sms_port_OTP_HC;
                    sms_acid = sms_acid_OTP_HC;
                    sms_systemid = sms_systemid_OTP_HC;
                    sms_password = sms_password_OTP_HC;
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

            if (!ret)
            {
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = obU.GetUserParameterSecure(userid, pwd);
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson = "Invalid Credentials"; ret = true;
                        }
                }
                // end
                else
                {
                    dt = obU.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }

                    if (!ret)
                        if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                }
                if (!ret)
                    if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18"))
                    { yourJson = "Invalid Message Type"; ret = true; }

                if (!ret)
                {
                    //check balance
                    noofsms = GetMsgCount(msg.Trim());
                    if (msg.Trim().Any(c => c > 126)) ucs = true;

                    if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                    if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                    if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                    if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                    //if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms * mobList.Count))
                    #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                    if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                    {
                        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                        { yourJson = "Insufficient Balance"; ret = true; }
                    }
                    #endregion
                }

                //check valid sender id
                if (!ret)
                {
                    if (!obU.CheckSenderId(userid, sender))
                    { yourJson = "Invalid Sender ID"; ret = true; }
                }
            }

            //Insert data in MSGQUEUEB4singleAPI where Usrid is blank at 11/04/2023
            if (!ret)
            {
                if (Convert.ToString(Usrid) == "")
                {
                    //sms_acid = sms_acid_API;
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;

                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msgid1 = GetMsgID();
                        obU.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
                        ss1 += "MobileNo: " + m.ToString() + " Message ID: " + msgid1 + ", ";
                    }
                    if (mobList.Count == 1)
                    {
                        yourJson = "SMS Submitted Successfully. Message ID: " + msgid1; ret = true;
                    }
                    else
                    {
                        ss1 = ss1.Substring(0, ss1.Length - 2);
                        yourJson = "SMS Submitted Successfully. Message ID: " + ss1; ret = true;
                    }
                }
            }

            if (!ret)
            {
                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = obU.GetTemplateIDfromSMS(sender, msg, ucs);


                    if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                        string e_tempid = ar1[0];
                        string e_peid = peid;
                        string e_sender = sender;

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
                                string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;
                        }
                        string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                        sRet = sRet.Substring(0, sRet.Length - 2);
                        if (mobList.Count == 1)
                        { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                        else
                        { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                    }
                    if (templID != "")
                        templateid = templID;
                }
                else
                {
                    // rabi for template DLT block 02/11/2022
                    if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string e_peid = peid;
                        string e_sender = sender;
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
                                    string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                    " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                    database.ExecuteNonQuery(sql);
                                }
                                sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                lastMsgID = nid;
                            }
                            string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                            { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                            else
                            { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                        }
                    }


                }
            }

            if (!ret)
            {
                Inetlab.SMPP.LicenseManager.SetLicense(obU.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);
                client.EncodingMapper.AddressEncoding = Encoding.ASCII;
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
                //string smppaccountid = "1401";
                //connect(client);
                //System.Threading.Thread.Sleep(5000);
                //bind(client);

                //bool b = await client.ConnectAsync("ucc-bsnl1.tanla.net", 5620);
                //System.Threading.Thread.Sleep(5000);
                //BindResp Bresp = await client.BindAsync("myinboxt2", "1OkLiNzH", ConnectionMode.Transmitter);
                //System.Threading.Thread.Sleep(200);

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
                foreach (var m in mobList)
                {
                    string m1 = m;
                    if (userid == "MIM2101322")
                    {
                        if (m.Length == 10) m1 = "91" + m;
                    }
                    if (m.Length == 12 || m.Length == 10)
                    {
                        var pduBuilder = SMS.ForSubmit()
                             .From(sourceAddress)
                             .To(m1)
                             .Coding(coding)
                             .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                             .Text(msg)
                             .AddParameter(0x1400, peid)
                            .AddParameter(0x1401, templateid);
                        if (sms_provider.Contains("AIRTEL"))
                        {
                            pduBuilder = SMS.ForSubmit()
                            .From(sourceAddress)
                            .To(m1)
                            .Coding(coding)
                            .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                            .Text(msg)
                            .AddParameter(0x1400, obU.getPEID(sms_provider, peid))
                            .AddParameter(0x1401, obU.getTEMPLATEID(sms_provider, templateid))
                            .AddParameter(0x1402, obU.getTMID(sms_provider));
                        }
                        pduList.AddRange(pduBuilder.Create(client));
                    }
                }
                string result = "";
                if (pduList.Count > 0)
                {
                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());
                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();
                    string s = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                    for (int i = 0; i < resp.Count; i++)
                    {
                        DataRow dr1 = dtRes.NewRow();
                        dr1["Mobile"] = resp[i].Request.DestinationAddress.Address;
                        dr1["MessageID"] = resp[i].MessageId;
                        dtRes.Rows.Add(dr1);
                        string msgt = resp[i].Request.MessageText.ToString();
                        result = result + resp[i].Request.DestinationAddress.Address + "-" + resp[i].MessageId + ", ";
                        obU.AddInMsgSubmittedMulti(userid, sender, resp[i].Request.DestinationAddress.Address, msgt, msgtype, Convert.ToString(resp[i].MessageId), Convert.ToString(resp[i].Header.Status), smppaccountid, peid, templateid, rate, msg, ucs, sms_provider + '-' + sms_systemid);
                    }
                    if (result != "") result = result.Substring(0, result.Length - 2);

                }
                else
                {
                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();
                }
                yourJson = (result == "" ? "Invalid Parameters" : result);
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted
        [Route("ScheduleSMS")]
        [HttpPost]
        public async Task<HttpResponseMessage> ScheduleSMS([FromBody] clsSchedule ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") HomeCR = true; else HomeCR = false;
            //clsSMS ob = JsonConvert.DeserializeObject<clsSMS>(param);

            string userid;
            string pwd;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string scheduleDateTime;
            string peid;
            string templateid;

            userid = ob.userid;
            pwd = ob.pwd;
            List<mobile> mob = ob.mobile;
            sender = ob.sender;
            msg = ob.msg;
            msgtype = ob.msgtype;
            scheduleDateTime = ob.scheduleDateTime;
            peid = ob.peid;
            templateid = ob.templateid;

            //if (templateid == null) { yourJson = "Invalid Template ID"; ret = true; }
            //if (peid == null) { yourJson = "Invalid PEID"; ret = true; }
            if (scheduleDateTime == null) { yourJson = "Invalid Schedule Date Time"; ret = true; }
            if (msgtype == null) { yourJson = "Invalid Message Type"; ret = true; }
            if (msg == null) { yourJson = "Invalid Message Text"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (sender == null) { yourJson = "Invalid senderid"; ret = true; }
            //            if (mobile == null) { yourJson = "Invalid mobile no"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            //List<string> mobList1 = mob.ToList(string);
            //List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

            Util obU = new Util();
            //if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

            //validation of list count
            if (mob.Count > 100000) { yourJson = "Mobile numbers cannot be more than 100000"; ret = true; }

            obU.InsertInAPiLog(userid, "3333333333", sender, msg, msgtype, peid, templateid);

            DataTable dt = new DataTable();
            double rate = 0;
            int noofsms = 0;
            bool ucs = false;
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");
            //Shishir 07/08/2023 start
            if (bSecure)
            {
                if (!ret)
                {
                    dt = obU.GetUserParameterSecure(userid, pwd);//For ApiKey
                    if (dt.Rows.Count <= 0)
                    {
                        yourJson = "Invalid Credentials"; ret = true;
                    }
                }
            }
            //end
            else
            {
                dt = obU.GetUserParameter(userid);

                if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }

                if (!ret)
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
            }
            if (!ret)
            {
                if (!ret)
                    if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "30"))
                    { yourJson = "Invalid Message Type"; ret = true; }

                if (!ret)
                {
                    //check balance
                    noofsms = GetMsgCount(msg.Trim());
                    if (msg.Trim().Any(c => c > 126)) ucs = true;

                    if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "30") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                    if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                    if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                    if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                    //if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms * mobList.Count))
                    #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                    if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                    {
                        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mob.Count))
                        //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                        { yourJson = "Insufficient Balance"; ret = true; }
                    }
                    #endregion
                }

                //check valid sender id
                if (!ret)
                {
                    if (!obU.CheckSenderId(userid, sender))
                    { yourJson = "Invalid Sender ID"; ret = true; }
                }

                //check valid TEMPLATE ID for senderid
                if (!ret)
                {
                    //if (!obU.CheckTemplateIdSenderId(userid, sender, templateid))
                    //{ yourJson = "Invalid Template ID"; ret = true; }
                    templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "30")
                    {
                        //string templateid = "";

                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                        {
                            string sql = "";
                            string templID = obU.GetTemplateIDfromSMS(sender, msg, ucs);


                            if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                            {
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                string e_tempid = ar1[0];
                                string e_peid = peid;
                                string e_sender = sender;

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
                                foreach (var m in mob)
                                {
                                    string nid = "";
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                        nid = Guid.NewGuid().ToString();
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                    sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                    lastMsgID = nid;
                                }
                                string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mob.Count, rate);
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                if (mob.Count == 1)
                                { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                else
                                { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                            }
                            if (templID != "")
                                templateid = templID;
                        }
                        else
                        {
                            // rabi for template DLT block 02/11/2022
                            if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                            {
                                string e_peid = peid;
                                string e_sender = sender;
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + templateid + "' and peid='" + e_peid + "'"));
                                if (errcd_ != "")
                                {
                                    // process REJECTION ....
                                    //return "";
                                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                    string sRet = "";
                                    string lastMsgID = "";
                                    foreach (var m in mob)
                                    {
                                        string nid = "";
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                            nid = Guid.NewGuid().ToString();
                                            string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                            database.ExecuteNonQuery(sql);
                                        }
                                        sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                        lastMsgID = nid;
                                    }
                                    string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mob.Count, rate);
                                    sRet = sRet.Substring(0, sRet.Length - 2);
                                    if (mob.Count == 1)
                                    { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                    else
                                    { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                                }
                            }


                        }
                    }
                }
            }


            if (!ret)
            {
                //put mobile list on temp table ---
                string user = "tmp_" + userid + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                string country_code = dt.Rows[0]["COUNTRYCODE"].ToString();
                dbB4Link.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobNo varchar(15)) ;  ");
                foreach (var m in mob)
                {
                    try { dbB4Link.ExecuteNonQuery(" Insert into " + user + @" values ('" + m.to + "')"); }
                    catch (Exception E4) { }
                }
                //database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");

                string campName = "APISchd" + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                string scheduleDate = scheduleDateTime;
                // ob.Schedule_SMS_BULK(UserID, txtMsg.Text.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, mobList, "MANUAL", templID, country_code, PrevBalance, AvailableBalance);
                //ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
                string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,
                    ucs2,noofsms,campname,prevbalance,availablebalance,isschedule,scheduletime,fileext,rate,methodname)
                    SELECT @maxid,'" + userid + @"' ,'APISchd' ,'" + user + @"','" + mob.Count.ToString() + @"','" + templateid + @"',N'" + msg.Trim().Replace("'", "''") + @"','" + sender +
                @"',0,Null,'" + country_code + @"','1','0','','','" + (ucs ? "1" : "0") + @"','" + noofsms + @"','" + campName + @"',0,0,1,'" + scheduleDate + "','api'," + rate + ",'Schedule_SMS_BULK'";
                //" select * into " + tblname + @" from " + dbName + ".dbo." + user + @" ";

                dbB4Link.ExecuteNonQuery(sq);

                double bal1 = CalculateSMSCost(noofsms * mob.Count, Convert.ToDouble(rate));
                string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + userid + "'";
                database.ExecuteNonQuery(sql1);

                yourJson = "SMS Scheduled Successfully";
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted N
        [Route("CancelSchedule")]
        [HttpPost]
        public async Task<HttpResponseMessage> CancelSchedule([FromBody] clsCancelSchedule ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string userid;
            string pwd;
            string scheduleDateTimeFrom;
            string scheduleDateTimeTo;

            userid = ob.userid;
            pwd = ob.pwd;
            scheduleDateTimeFrom = ob.scheduleDateTimeFrom;
            scheduleDateTimeTo = ob.scheduleDateTimeTo;

            if (scheduleDateTimeTo == null) { yourJson = "Invalid Schedule Date Time"; ret = true; }
            if (scheduleDateTimeFrom == null) { yourJson = "Invalid Schedule Date Time"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            Util obU = new Util();
            DataTable dt = new DataTable();

            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");
            //Shishir 07/08/2023 start
            if (bSecure)
            {
                dt = obU.GetUserParameterSecure(userid, pwd);//For ApiKey
                if (!ret)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        yourJson = "Invalid Credentials"; ret = true;
                    }
                }
            }
            //end
            else
            {
                dt = obU.GetUserParameter(userid);

                if (!ret)
                {
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }
                    if (!ret)
                        if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                }
            }
            if (!ret)
            {
                DataTable dt1 = obU.GetScheduleRecords(userid, scheduleDateTimeFrom, scheduleDateTimeTo);
                if (dt1.Rows.Count > 0)
                {
                    Int32 tot = 0;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        string SMSRate = dr["SMSRate"].ToString();
                        string NoOfSMS = dr["NoOfSMS"].ToString();
                        tot += Convert.ToInt32(dr["mobiles"].ToString());
                        Int32 cnt = Convert.ToInt32(dr["mobiles"].ToString()) * Convert.ToInt32(NoOfSMS) * (-1);
                        obU.UpdateAndGetBalanceSch(userid, "", cnt, Convert.ToDouble(SMSRate));
                        obU.RemoveFromSchedule(userid, dr["FileID"].ToString(), dr["Schedule"].ToString());
                    }
                    yourJson = "No of Schedules: " + dt1.Rows.Count.ToString() + " and SMS Count: " + tot.ToString() + " Cancelled Successfully.";
                }
                else
                {
                    yourJson = "Schedule not available for the period.";
                }
            }
            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;

        }

        public double CalculateSMSCost(long cnt, double rate)
        {
            double bal = 0;
            bal = (cnt * rate) / 100;
            return Convert.ToDouble(Math.Round(bal, 3));
        }

        [Route("SendSMShmil")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMShmil([FromBody] Hmil[] ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            Util obU = new Util();
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");
            dtRes.Columns.Add("ID");

            for (int i = 0; i < ob.Length; i++)
            {
                Hmil obH = ob[i];
                string userid;
                string pwd;
                string mobile;
                string sender;
                string msg;
                string msgtype;
                string peid;
                string templateid;
                string id;

                userid = obH.usr;
                pwd = obH.pwd;
                mobile = obH.mob;
                sender = obH.snd;
                msg = obH.msg;
                msgtype = obH.mtp;
                peid = obH.pe;
                templateid = obH.tm;
                id = obH.id;

                obU.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                }
                else
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                string msgid2 = "";
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;
                foreach (string m in mobList)
                {
                    if (m.Trim().Length == 10 || m.Trim().Length == 12)
                    {
                        double bal = Convert.ToDouble(database.GetScalarValue("Select balance from customer where username='" + userid + "'"));
                        if (bal > 0)
                        {
                            msgid2 = GetMsgID();
                            new Util().AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, 12, noofsms, ucs, templateid, "");
                        }
                        else
                        {
                            msgid2 = "Insufficient Balance";
                        }
                        //new Util().AddInMsgQueueTEST(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, 12, noofsms, ucs, templateid, "");
                    }
                    else
                    {
                        msgid2 = "Invalid Mobile No";
                    }
                    DataRow dr1 = dtRes.NewRow();
                    dr1["Mobile"] = m;
                    dr1["MessageID"] = msgid2;
                    dr1["ID"] = id;
                    dtRes.Rows.Add(dr1);
                }
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        public string CheckPromoTime(string account, string sender, List<string> mobList, int noofsms, double rate, string msg, bool ucs, string userid)
        {
            Util obU = new Util();
            if (sender.ToUpper().StartsWith("AD-") || sender.ToUpper().EndsWith("-AD"))
            {
                DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                if ((account == "ETISALAT" && CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25")) ||
                    (account == "KARIX" && CurrTime >= Convert.ToDateTime("08:35") && CurrTime <= Convert.ToDateTime("21:55")) ||
                    (account == "GUPSHUP" && CurrTime >= Convert.ToDateTime("10:30") && CurrTime <= Convert.ToDateTime("22:30"))
                   )
                {
                    return "1";
                }
                else
                {
                    noofsms = GetMsgCount(msg.Trim());
                    // process REJECTION ....
                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                    string sRet = "";
                    string sql = "";
                    string errcd_ = "968";
                    string lastMsgID = "";
                    foreach (var m in mobList)
                    {
                        string nid = "";
                        for (int x = 0; x < noofsms; x++)
                        {
                            string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                            nid = Guid.NewGuid().ToString();
                            sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                            " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                            " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                            database.ExecuteNonQuery(sql);
                        }
                        sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                        lastMsgID = nid;
                    }
                    string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                    sRet = sRet.Substring(0, sRet.Length - 2);
                    if (mobList.Count == 1)
                    { return  "SMS Submitted Successfully. Message ID: " + lastMsgID ; }
                    else
                    { return "SMS Submitted Successfully. " + sRet;  }
                }
            }
            else
                return "1";
        }

        //Secured Encrypted    india and uae
        [Route("SendSMS")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMS([FromBody] clsSMS2 ob)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string hc = "0"; // ConfigurationManager.AppSettings["HOMECR"].ToString();
            //if (hc == "1")
            HomeCR = false;
            //clsSMS ob = JsonConvert.DeserializeObject<clsSMS>(param);

            
            string userid;
            string pwd;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string peid;
            string templateid;
            string subclientcode;
            string FailOver;
            string WABATemplateName;
            string WABAVariables;
            string emailFrom;
            string emailTo;
            string emailCC;
            string emailSubject;
            string sendEmail;    //Add by Vikas
            string FailOver2;    //Add by Vikas

            userid = ob.userid;
            pwd = ob.pwd;
            mobile = ob.mobile;
            sender = ob.sender;
            msg = ob.msg;


            msgtype = ob.msgtype;
            peid = ob.peid;
            templateid = ob.templateid;
            FailOver = ob.failover;
            subclientcode = ob.subclientcode;
            WABATemplateName = ob.WABATemplateName;
            WABAVariables = ob.WABAVariables;
            emailFrom = ob.emailFrom;
            emailTo = ob.emailTo;
            emailCC = ob.emailCC;
            emailSubject = ob.emailSubject;
            sendEmail = ob.sendEmail;   //Add by Vikas
            FailOver2 = ob.failOver2;   //Add by Vikas

            //if (templateid == null) { yourJson = "Invalid Template ID"; ret = true; }
            if (templateid == null) templateid = "";
            if (FailOver == null) FailOver = "";
            if (subclientcode == null) subclientcode = "";
            if (WABATemplateName == null) WABATemplateName = "";
            if (WABAVariables == null) WABAVariables = "";
            if (emailFrom == null) emailFrom = "";
            if (emailTo == null) emailTo = "";
            if (emailCC == null) emailCC = "";
            if (emailSubject == null) emailSubject = "";
            if (sendEmail == null) sendEmail = "";
            if (FailOver2 == null) FailOver2 = "";


            //if (peid == null) { yourJson = "Invalid PEID"; ret = true; }
            if (msgtype == null) { yourJson = "Invalid Message Type"; ret = true; }
            if (msg == null) { yourJson = "Invalid Message Text"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (sender == null) { yourJson = "Invalid senderid"; ret = true; }
            if (mobile == null) { yourJson = "Invalid mobile no"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            mobile = mobile.Replace("+", "").Replace("-", "").Replace(" ", "");

            if (ob.param_1 != null) msg = msg.Replace(ob.param_1, ob.param_1v);
            if (ob.param_2 != null) msg = msg.Replace(ob.param_2, ob.param_2v);
            if (ob.param_3 != null) msg = msg.Replace(ob.param_3, ob.param_3v);
            if (ob.param_4 != null) msg = msg.Replace(ob.param_4, ob.param_4v);

            Util obU = new Util();
            bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);

            userid = userid.ToUpper();
            bool sendRspJSON = false;
            if (userid == "MIM2301076" || userid == "MIM2400108" || userid == "MIM2400179") sendRspJSON = true;

            if (userid == "MIM2300228" )
            {
                if (pwd == "ParkOne")
                {
                    if (msgtype == "13") sms_acid = "2201"; // sms_acid_API;
                    if (msgtype == "33") sms_acid = "2201"; // sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;
                    string msgid1 = "";
                    string ss1 = "";
                    var m = mobile.Replace("+", "").Replace(" ", "");

                    msgid1 = GetMsgID();
                    //obU.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, 1, 1, false, templateid, subclientcode, "", "", "", "", "", "", "", "");
                    obU.AddInMsgQueueAPIRabbitMQ(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid, peid, 1, 1, false, templateid, subclientcode, "", "", "", "", "", "", "", "");

                    yourJson = "SMS Submitted Successfully. Message ID: " + msgid1; ret = true;
                }
                else
                {
                    yourJson = "Invalid Password";
                    ret = true;
                }
            }

            DataTable dt = new DataTable();
            double rate = 0;
            int noofsms = 0;
            bool ucs = false;
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

            DataTable dtRes2 = new DataTable("dtRes2");
            dtRes2.Columns.Add("Response");
            List<string> mobList1;
            List<string> mobList = new List<string>();
            string Usrid = "";

            if (!ret)
            {
                if (userid == "MIM2101541")
                {
                    if(subclientcode == "")
                    {
                        yourJson = "Invalid SubClientCode";
                        ret = true;
                    }
                }
            }

            if (!ret)
            {
                mobList1 = mobile.Split(',').ToList();
                mobList = mobList1.Select(item => item.Trim()).ToList();

                if (userid == "MIM2300165" || userid == "MIM2300167")
                {
                    if (pwd.ToUpper() == "FIRSTCRY321")
                    {
                        string msgid = GetMsgID();
                        string SubClienQry = "INSERT INTO MSGUAESubClientCode (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,msgidClient,SubClientCode) " +
                        " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + msgid + "','" + Convert.ToString(subclientcode) + "')";
                        database.ExecuteNonQuery(SubClienQry);

                        string sql = "INSERT INTO MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,msgidClient) " +
                        " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + msgid + "')";
                        database.ExecuteNonQuery(sql);

                        yourJson = "SMS Submitted Successfully. Message ID: " + msgid;
                        ret = true;
                    }
                    else
                    {
                        yourJson = "Invalid Password";
                        ret = true;
                    }
                }

                
                if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

                //validation of list count
                if (mobList.Count > 500) { yourJson = "Mobile numbers cannot be more than 500"; ret = true; }
                Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
                obU.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                #region <Message Type wise account bind >
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

                    if (HomeCR)
                    {
                        sms_provider = sms_provider_HC;
                        sms_ip = sms_ip_HC;
                        sms_port = sms_port_HC;
                        sms_acid = sms_acid_HC;
                        sms_systemid = sms_systemid_HC;
                        sms_password = sms_password_HC;
                    }
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                    if (HomeCR)
                    {
                        sms_provider = sms_provider_OTP_HC;
                        sms_ip = sms_ip_OTP_HC;
                        sms_port = sms_port_OTP_HC;
                        sms_acid = sms_acid_OTP_HC;
                        sms_systemid = sms_systemid_OTP_HC;
                        sms_password = sms_password_OTP_HC;
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
                else if (msgtype == "19")
                {
                    sms_provider = sms_provider_KSA1;
                    sms_ip = sms_ip_KSA1;
                    sms_port = sms_port_KSA1;
                    sms_acid = sms_acid_KSA1;
                    sms_systemid = sms_systemid_KSA1;
                    sms_password = sms_password_KSA1;
                }
                else if (msgtype == "20")
                {
                    sms_provider = sms_provider_WP_UAE;
                    sms_ip = sms_ip_WP_UAE;
                    sms_port = sms_port_WP_UAE;
                    sms_acid = sms_acid_WP_UAE;
                    sms_systemid = sms_systemid_WP_UAE;
                    sms_password = sms_password_WP_UAE;
                }
                else if (msgtype == "30")
                {
                    sms_provider = sms_provider_WP_UAE_P;
                    sms_ip = sms_ip_WP_UAE_P;
                    sms_port = sms_port_WP_UAE_P;
                    sms_acid = sms_acid_WP_UAE_P;
                    sms_systemid = sms_systemid_WP_UAE_P;
                    sms_password = sms_password_WP_UAE_P;
                }
                else if (msgtype == "40")
                {
                    sms_provider = sms_provider_WP_KSA_T;
                    sms_ip = sms_ip_WP_KSA_T;
                    sms_port = sms_port_WP_KSA_T;
                    sms_acid = sms_acid_WP_KSA_T;
                    sms_systemid = sms_systemid_WP_KSA_T;
                    sms_password = sms_password_WP_KSA_T;
                }
                else if (msgtype == "50")
                {
                    sms_provider = sms_provider_WP_KSA_P;
                    sms_ip = sms_ip_WP_KSA_P;
                    sms_port = sms_port_WP_KSA_P;
                    sms_acid = sms_acid_WP_KSA_P;
                    sms_systemid = sms_systemid_WP_KSA_P;
                    sms_password = sms_password_WP_KSA_P;
                }
                else if (msgtype == "22")
                {
                    sms_provider = sms_provider_QTR;
                    sms_ip = sms_ip_QTR;
                    sms_port = sms_port_QTR;
                    sms_acid = sms_acid_QTR;
                    sms_systemid = sms_systemid_QTR;
                    sms_password = sms_password_QTR;

                    DataTable dtQ = database.GetDataTable("Select account from QatarAPIACCOUNTPROMO with (nolock) where userid = '" + userid + "' and senderid = '" + sender + "'");
                    if (dtQ.Rows.Count > 0)
                    {
                        string account = Convert.ToString(dtQ.Rows[0]["account"]);
                        if (account.ToUpper() == "BROADNET")
                        {
                            sms_provider = sms_provider_QTR;
                            sms_ip = sms_ip_QTR;
                            sms_port = sms_port_QTR;
                            sms_acid = sms_acid_QTR;
                            sms_systemid = sms_systemid_QTR;
                            sms_password = sms_password_QTR;
                        }
                        else
                        {
                            sms_provider = sms_provider_QTR2;
                            sms_ip = sms_ip_QTR2;
                            sms_port = sms_port_QTR2;
                            sms_acid = sms_acid_QTR2;
                            sms_systemid = sms_systemid_QTR2;
                            sms_password = sms_password_QTR2;
                        }
                    }
                    else
                    {
                        DataTable dtQ2 = database.GetDataTable("Select account from QatarAPIAccounts with (nolock) where userid='" + userid + "'");
                        if (dtQ2.Rows.Count > 0)
                        {
                            string account = Convert.ToString(dtQ2.Rows[0]["account"]);
                            if (account.ToUpper() == "BROADNET")
                            {
                                sms_provider = sms_provider_QTR;
                                sms_ip = sms_ip_QTR;
                                sms_port = sms_port_QTR;
                                sms_acid = sms_acid_QTR;
                                sms_systemid = sms_systemid_QTR;
                                sms_password = sms_password_QTR;
                            }
                            else
                            {
                                sms_provider = sms_provider_QTR2;
                                sms_ip = sms_ip_QTR2;
                                sms_port = sms_port_QTR2;
                                sms_acid = sms_acid_QTR2;
                                sms_systemid = sms_systemid_QTR2;
                                sms_password = sms_password_QTR2;
                            }
                        }
                    }
                }
                else if (msgtype == "16")
                {

                    sms_provider = sms_provider_DUB_3;
                    sms_ip = sms_ip_DUB_3;
                    sms_port = sms_port_DUB_3;
                    sms_acid = sms_acid_DUB_3;
                    sms_systemid = sms_systemid_DUB_3;
                    sms_password = sms_password_DUB_3;

                    DataTable dtA = database.GetDataTable("Select account from UAEAPIACCOUNTPROMO with (nolock) where userid = '" + userid + "' and senderid = '" + sender + "'");
                    if (dtA.Rows.Count > 0)
                    {
                        string account = Convert.ToString(dtA.Rows[0]["account"]);
                        if (account == "KARIX")
                        {
                            sms_provider = sms_provider_DUB_3;
                            sms_ip = sms_ip_DUB_3;
                            sms_port = sms_port_DUB_3;
                            sms_acid = sms_acid_DUB_3;
                            sms_systemid = sms_systemid_DUB_3;
                            sms_password = sms_password_DUB_3;
                            string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                            if (retvl != "1") { yourJson = retvl; ret = true; }
                        }
                        else if (account == "GUPSHUP")
                        {
                            sms_provider = sms_provider_DUB_5;
                            sms_ip = sms_ip_DUB_5;
                            sms_port = sms_port_DUB_5;
                            sms_acid = sms_acid_DUB_5;
                            sms_systemid = sms_systemid_DUB_5;
                            sms_password = sms_password_DUB_5;
                            string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                            if (retvl != "1") { yourJson = retvl; ret = true; }
                        }
                    }
                    else
                    {
                        DataTable dtB = database.GetDataTable("Select account from UAEAPIAccounts with (nolock) where userid='" + userid + "'");
                        if (dtB.Rows.Count > 0)
                        {
                            string account = Convert.ToString(dtB.Rows[0]["account"]);
                            if (account == "KARIX")
                            {
                                sms_provider = sms_provider_DUB_2;
                                sms_ip = sms_ip_DUB_2;
                                sms_port = sms_port_DUB_2;
                                sms_acid = sms_acid_DUB_2;
                                sms_systemid = sms_systemid_DUB_2;
                                sms_password = sms_password_DUB_2;
                                string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                                if (retvl != "1") { yourJson = retvl; ret = true; }
                            }
                            else if (account == "GUPSHUP")
                            {
                                sms_provider = sms_provider_DUB_4;
                                sms_ip = sms_ip_DUB_4;
                                sms_port = sms_port_DUB_4;
                                sms_acid = sms_acid_DUB_4;
                                sms_systemid = sms_systemid_DUB_4;
                                sms_password = sms_password_DUB_4;
                                string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                                if (retvl != "1") { yourJson = retvl; ret = true; }
                            }
                            else if (account == "ETISALAT")
                            {
                                sms_provider = sms_provider_DUB;
                                sms_ip = sms_ip_DUB;
                                sms_port = sms_port_DUB;
                                sms_acid = sms_acid_DUB;
                                sms_systemid = sms_systemid_DUB;
                                sms_password = sms_password_DUB;
                                string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                                if (retvl != "1") { yourJson = retvl; ret = true; }
                                #region <Shifted to CheckPromoTime function >
                                //if (sender.StartsWith("AD-") || sender.EndsWith("-AD"))
                                //{
                                //    DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                                //    if (CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25"))
                                //    {

                                //    }
                                //    else
                                //    {
                                //        noofsms = GetMsgCount(msg.Trim());
                                //        // process REJECTION ....
                                //        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                //        string sRet = "";
                                //        string sql = "";
                                //        string errcd_ = "991";
                                //        string lastMsgID = "";
                                //        foreach (var m in mobList)
                                //        {
                                //            string nid = "";
                                //            for (int x = 0; x < noofsms; x++)
                                //            {
                                //                string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                //                nid = Guid.NewGuid().ToString();
                                //                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                //                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                //                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                //                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                //                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                //                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                //                " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                //                database.ExecuteNonQuery(sql);
                                //            }
                                //            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                //            lastMsgID = nid;
                                //        }
                                //        string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                //        sRet = sRet.Substring(0, sRet.Length - 2);
                                //        if (mobList.Count == 1)
                                //        { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                //        else
                                //        { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                                //    }
                                //}
                                #endregion
                            }
                        }
                    }

                    #region REJECTION20240219
                    string nid = "";
                    string ss2 = "";
                    string m = mobile;

                    int MobLen = Convert.ToInt32(m.Length);
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            noofsms = GetMsgCount(msg.Trim());
                            for (int x = 0; x < noofsms; x++)
                            {
                                string errcd_ = "5308";
                                string smsTex = msg.Replace("'", "''");
                                nid = Guid.NewGuid().ToString();
                                string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTED err:" + errcd_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            ss2 += "MobileNo: " + m.ToString() + " Message ID: " + nid + ", ";

                            if (mobList.Count == 1)
                            {
                                yourJson = "SMS Submitted Successfully. Message ID: " + nid;
                                if(sendRspJSON) yourJson = "SMS Submitted Successfully. Message ID: " + nid;
                                ret = true;
                            }
                            else
                            {
                                ss2 = ss2.Substring(0, ss2.Length - 2);
                                { yourJson = "SMS Submitted Successfully. Message ID: " + ss2; ret = true; }
                            }
                        }
                    }
                    #endregion
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
                #endregion
                if (userid == "MIM2101450" || sender.ToUpper() == "HIIBPL" || userid == "MIM2201048" || userid == "MIM2201104"
                    || userid == "MIM2300228" || userid == "MIM2300229")   //hyundai insurance , park plus
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;

                    if ((userid == "MIM2300228" || userid == "MIM2300229") && msgtype == "33")
                    {
                        sms_provider = sms_provider_OTP;
                        sms_ip = sms_ip_OTP;
                        sms_port = sms_port_OTP;
                        sms_acid = sms_acid_OTP;
                        sms_systemid = sms_systemid_OTP;
                        sms_password = sms_password_OTP;
                    }
                }
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                }
            }

            if (!ret)
            {
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = obU.GetUserParameterSecure(userid, pwd);//For ApiKey
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson = "Invalid Credentials"; ret = true;
                        }
                }
                //end
                else
                {
                    dt = obU.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }

                    if (!ret)
                        if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                }
                if (!ret)
                    if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "22" || msgtype == "30" || msgtype == "40" || msgtype == "50"))
                    { yourJson = "Invalid Message Type"; ret = true; }

                if (!ret)
                    if (msgtype == "16")
                    {
                        if (mobile.Trim().Length < 12)
                        { yourJson = "Invalid Mobile Number"; ret = true; }
                    }
                if (!ret)
                {
                    //check balance
                    noofsms = GetMsgCount(msg.Trim());
                    if (msg.Trim().Any(c => c > 126)) ucs = true;

                    if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "22" || msgtype == "30" || msgtype == "40" || msgtype == "50") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                    if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                    if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                    if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                    //if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms * mobList.Count))
                    #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                    if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                    {
                        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                        //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                        { yourJson = "Insufficient Balance"; ret = true; }
                    }
                    #endregion
                }

                //check valid sender id
                if (!ret)
                {
                    if (!obU.CheckSenderId(userid, sender))
                    { yourJson = "Invalid Sender ID"; ret = true; }
                }

                if (WhitePanel)
                {
                    if (!ret)
                    {
                        if (isNumeric)
                            sms_acid = "2001";
                        else if (msgtype == "13") sms_acid = "1601";
                        else if (msgtype == "33") sms_acid = "1101";

                        string msgid2 = "";
                        string ss2 = "";
                        foreach (var m in mobList)
                        {
                            msgid2 = GetMsgID();
                            new Util().AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
                            ss2 += "MobileNo: " + m.ToString() + " Message ID: " + msgid2 + ", ";
                        }
                        if (mobList.Count == 1)
                        { yourJson = "SMS Submitted Successfully. Message ID: " + msgid2; ret = true; }
                        else
                        {
                            ss2 = ss2.Substring(0, ss2.Length - 2);
                            { yourJson = "SMS Submitted Successfully. Message ID: " + ss2; ret = true; }
                        }
                    }
                }

                if ((sender.ToUpper() == "HIIBPL" || userid == "MIM2201048" || userid == "MIM2201104" ||
                    userid == "MIM2300228" || userid == "MIM2300229") && Usrid == "")
                {
                    //if (Convert.ToString(ob.subclientcode) == "") { yourJson = "Invalid Sub Client Code"; ret = true; }

                    // SAVE SMS IN QUEUE

                    if (!ret)
                    {
                        string msgid2 = "";
                        string ss2 = "";
                        foreach (var m in mobList)
                        {
                            //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                            msgid2 = GetMsgID();
                            new Util().AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, ob.subclientcode, FailOver);
                            ss2 += "MobileNo: " + m.ToString() + " Message ID: " + msgid2 + ", ";
                        }
                        if (mobList.Count == 1)
                        { yourJson = "SMS Submitted Successfully. Message ID: " + msgid2; ret = true; }
                        else
                        {
                            ss2 = ss2.Substring(0, ss2.Length - 2);
                            { yourJson = "SMS Submitted Successfully. Message ID: " + ss2; ret = true; }
                        }
                    }
                }

                if (!ret)
                {
                    //Insert data in MSGQUEUEB4singleAPI where Usrid is blank at 11/04/2023
                    if (Convert.ToString(Usrid) == "")
                    {
                        //sms_acid = sms_acid_API;
                        if (msgtype == "13") sms_acid = sms_acid_API;
                        if (msgtype == "33") sms_acid = sms_acid_OTP;
                        if (isNumeric) sms_acid = sms_acid_PROMO;
                        string msgid1 = "";
                        string ss1 = "";
                        foreach (var m in mobList)
                        {
                            msgid1 = GetMsgID();
                            obU.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode, FailOver, WABATemplateName, WABAVariables, emailFrom, emailTo, emailCC, emailSubject, FailOver2);
                            ss1 += "MobileNo: " + m.ToString() + " Message ID: " + msgid1 + ", ";
                            if (sendRspJSON)
                            {
                                DataRow dr1 = dtRes.NewRow();
                                dr1["Mobile"] = m.ToString();
                                dr1["MessageID"] = msgid1;
                                dtRes.Rows.Add(dr1);
                            }
                        }
                        if (mobList.Count == 1)
                        {       //CWC - MIM2002191, MIM2002196, MIM2201083
                            if ((userid.ToUpper() == "MIM2002191" || userid.ToUpper() == "MIM2002196" || userid.ToUpper() == "MIM2201083") && subclientcode != "")
                            { yourJson = "SMS Submitted Successfully. Message ID: " + msgid1 + " . Client Message ID:" + subclientcode; ret = true; }
                            else
                            {
                                yourJson = "SMS Submitted Successfully. Message ID: " + msgid1; 
                                ret = true;
                            }

                        }
                        else
                        {
                            ss1 = ss1.Substring(0, ss1.Length - 2);
                            yourJson = "SMS Submitted Successfully. Message ID: " + ss1; ret = true;
                        }
                    }
                }
                //check valid TEMPLATE ID for senderid
                if (!ret)
                {
                    //if (!obU.CheckTemplateIdSenderId(userid, sender, templateid))
                    //{ yourJson = "Invalid Template ID"; ret = true; }
                    //templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    {
                        //string templateid = "";

                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";

                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                        {
                            string sql = "";
                            string templID = obU.GetTemplateIDfromSMS(sender, msg, ucs);

                            if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                            {
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                string e_tempid = ar1[0];
                                string e_peid = peid;
                                string e_sender = sender;

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
                                string lastMob = "";
                                foreach (var m in mobList)
                                {
                                    string nid = "";
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                        nid = Guid.NewGuid().ToString();
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);

                                        try
                                        {
                                            if (subclientcode != "")
                                            {
                                                obU.addInMSGSubClientCode(userid, sender, m, msg.Replace("'", "''"), nid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", subclientcode);
                                                sql = " Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno) " +
                                                "values ('" + userid + "','id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' ," +
                                                "'" + nid + "',GETDATE(),GETDATE(),'Rejected','" + errcd_ + "','" + sender + "','" + m + "')";
                                                database.ExecuteNonQuery(sql);
                                            }
                                        }
                                        catch (Exception e5) { }

                                    }
                                    sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                    if (sendRspJSON)
                                    {
                                        DataRow dr1 = dtRes.NewRow();
                                        dr1["Mobile"] = m.ToString();
                                        dr1["MessageID"] = nid;
                                        dtRes.Rows.Add(dr1);
                                    }
                                    lastMsgID = nid;
                                    lastMob = m;
                                }
                                string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                if (mobList.Count == 1)
                                {
                                    yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    ret = true;
                                }
                                else
                                { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                                //if (userid == "MIM2301076")
                                //{
                                //    DataRow dr1 = dtRes.NewRow();
                                //    dr1["Mobile"] = lastMob;
                                //    dr1["MessageID"] = lastMsgID;
                                //    dtRes.Rows.Add(dr1);
                                //    ret = true;
                                //}
                            }
                            if (templID != "")
                                templateid = templID;
                        }
                        else
                        {
                            // rabi for template DLT block 02/11/2022
                            if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                            {
                                string e_peid = peid;
                                string e_sender = sender;
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + templateid + "' and peid='" + e_peid + "'"));
                                if (errcd_ != "")
                                {
                                    string sRet = "";
                                    string lastMsgID = "";
                                    foreach (var m in mobList)
                                    {
                                        string nid = "";
                                        for (int x = 0; x < noofsms; x++)
                                        {
                                            string smsTex = obU.GetSMSText(msg, x + 1, noofsms, ucs);
                                            nid = Guid.NewGuid().ToString();
                                            string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                            " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                            " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                            "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                            " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                            database.ExecuteNonQuery(sql);
                                        }
                                        sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                        lastMsgID = nid;
                                        if (sendRspJSON)
                                        {
                                            DataRow dr1 = dtRes.NewRow();
                                            dr1["Mobile"] = m.ToString();
                                            dr1["MessageID"] = nid;
                                            dtRes.Rows.Add(dr1);
                                        }
                                    }
                                    string sx = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                    sRet = sRet.Substring(0, sRet.Length - 2);
                                    if (mobList.Count == 1)
                                    { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                    else
                                    { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                                }
                            }
                        }

                    }
                }
            }

            string yj = "";

            if (!ret)
            {
                Inetlab.SMPP.LicenseManager.SetLicense(obU.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);
                client.EncodingMapper.AddressEncoding = Encoding.ASCII;
                client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
                client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
                client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
                client.evUnBind += new UnBindEventHandler(client_evUnBind);
                client.evDataSm += new DataSmEventHandler(client_evDataSm);
                client.evRecoverySucceeded += ClientOnRecoverySucceeded;

                client.evServerCertificateValidation += OnCertificateValidation;

                string sTon = "0";
                if (sms_provider.Contains("ETISALAT")) sTon = "5";

                client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte(sTon), (AddressNPI)Convert.ToByte("0"));
                
                client.SystemType = ""; // tbSystemType.Text;
                client.ConnectionRecovery = true; // cbReconnect.Checked;
                client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
                client.EnabledSslProtocols = SslProtocols.None;

                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                bool b = await client.ConnectAsync(sms_ip, sms_port);
                //System.Threading.Thread.Sleep(5000);
                BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
                //System.Threading.Thread.Sleep(200);

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

                DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));
                sTon = "0";
                if (sms_provider.Contains("ETISALAT")) sTon = "5";
                var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse(sTon), (AddressNPI)byte.Parse("0"));
                expMin = 60 * 60;
                if (msgtype == "33") { expMin = 10; }

                List<SubmitSm> pduList = new List<SubmitSm>();
                foreach (var m in mobList)
                {
                    if (m.Length == 12 || m.Length == 10 || (msgtype == "16" && (m.Length == 12 || m.Length == 9) || (msgtype == "22" && m.Length == 11)))
                    {
                        string m1 = m;
                        if (userid == "MIM2101322")
                        {
                            if (m.Length == 10) m1 = "91" + m;
                        }

                        var pduBuilder = SMS.ForSubmit()
                         .From(sourceAddress)
                         .To(m1)
                         .Coding(coding)
                         .DeliveryReceipt().ExpireIn(TimeSpan.FromSeconds(expMin))
                         .Text(msg)
                         .AddParameter(0x1400, (peid == null ? "" : peid))
                         .AddParameter(0x1401, (templateid == null ? "" : templateid));
                        if (sms_provider.Contains("AIRTEL"))
                        {
                            pduBuilder = SMS.ForSubmit()
                            .From(sourceAddress)
                            .To(m1)
                            .Coding(coding)
                            .DeliveryReceipt().ExpireIn(TimeSpan.FromSeconds(expMin))
                            .Text(msg)
                            .AddParameter(0x1400, obU.getPEID(sms_provider, (peid == null ? "" : peid)))
                            .AddParameter(0x1401, obU.getTEMPLATEID(sms_provider, (templateid == null ? "" : templateid)))
                            .AddParameter(0x1402, obU.getTMID(sms_provider));
                        }
                        if (sms_provider.Contains("ETISALAT"))
                        {
                            pduBuilder = SMS.ForSubmit()
                            .From(sourceAddress)
                            .To(m1)
                            .Coding(coding)
                            .DeliveryReceipt().ExpireIn(TimeSpan.FromSeconds(expMin))
                            .Text(msg)
                            .AddParameter(0x1400, System.Text.Encoding.UTF8.GetBytes(""))
                            .AddParameter(0x1401, System.Text.Encoding.UTF8.GetBytes((sender.ToUpper().StartsWith("AD-") || sender.ToUpper().EndsWith("-AD") ? "" : "txn")));
                        }
                        pduList.AddRange(pduBuilder.Create(client));
                    }
                }
                string result = "";
                if (pduList.Count > 0)
                {
                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());
                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();
                    string s = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                    for (int i = 0; i < resp.Count; i++)
                    {
                        DataRow dr1 = dtRes.NewRow();
                        dr1["Mobile"] = resp[i].Request.DestinationAddress.Address;
                        dr1["MessageID"] = resp[i].MessageId;

                        if (userid.ToUpper() == "MIM2400145" || userid.ToUpper() == "MIM2300213")
                        {
                            yj = "SMS Submitted Successfully. Message ID: " + resp[0].MessageId;
                            if (subclientcode != "" && i==0)
                                obU.addInMSGSubClientCode(userid, sender, mobile, msg, resp[0].MessageId, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode);
                        }
                        else
                        {
                            if (subclientcode != "")
                                obU.addInMSGSubClientCode(userid, sender, mobile, msg, resp[i].MessageId, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode);
                        }

                        dtRes.Rows.Add(dr1);

                        if (userid == "MIM2300228" || userid == "MIM2300229" || userid == "MIM2000014")
                        {
                            DataRow dr2 = dtRes2.NewRow();
                            dr2["Response"] = "SMS Submitted Successfully. Message ID: " + resp[i].MessageId;
                            dtRes2.Rows.Add(dr2);
                        }

                        
                        string msgt = resp[i].Request.MessageText.ToString();
                        result = result + resp[i].Request.DestinationAddress.Address + "-" + resp[i].MessageId + ", ";
                        if (msgtype == "47")
                            obU.AddInMsgQueueGSM(userid, sender, Convert.ToString(dr1["Mobile"]), msg.Replace("'", "''"), msgtype, resp[i].MessageId, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
                        else
                        {
                            //if (mobList.Count==1 && (Convert.ToString(resp[i].Header.Status).ToUpper() == "ESME_RTHROTTLED" || Convert.ToString(resp[i].Header.Status).ToUpper() == "SMPPCLIENT_UNBOUND" || Convert.ToString(resp[i].Header.Status).ToUpper() == "SMPPCLIENT_NOCONN"))
                            //{
                            //    //dtRes.Clear();
                            //    //dtRes2.Clear();
                            //    //await SendFailOverSmsRoute(userid, msgtype, ucs, sender, peid, templateid, msg, mobList[0], ref dtRes, ref dtRes2);
                            //}
                            //else
                            //{
                            obU.AddInMsgSubmittedMulti(userid, sender, resp[i].Request.DestinationAddress.Address, msgt, msgtype, Convert.ToString(resp[i].MessageId), Convert.ToString(resp[i].Header.Status), smppaccountid, peid, templateid, rate, msg, ucs, sms_provider + '-' + sms_systemid);
                            //}
                        }

                        if (Convert.ToString(FailOver) != "" && FailOver != null)
                        {
                            obU.AddInFailOver(userid, mobile, msg.Replace("'", "''"), Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), FailOver, WABATemplateName, WABAVariables, emailFrom, emailTo, emailCC, emailSubject, FailOver2);
                        }
                    }
                    if (result != "") result = result.Substring(0, result.Length - 2);
                }
                else
                {
                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();
                }
                yourJson = (result == "" ? "Invalid Parameters" : result);
            }

            if (dtRes.Rows.Count > 0)
            {
                if (yj != "")
                {
                    DataTable dt1 = new DataTable("dt");
                    dt1.Columns.Add("Response");
                    DataRow dr = dt1.NewRow();
                    dr[0] = yj;
                    dt1.Rows.Add(dr);
                    yourJson = JsonConvert.SerializeObject(dt1);
                }
                else
                {
                    yourJson = JsonConvert.SerializeObject(dtRes);
                    try
                    {
                        if (dtRes2.Rows.Count > 0)
                        { yourJson = JsonConvert.SerializeObject(dtRes2); }

                        if (userid == "MIM2400108" && dtRes.Rows.Count == 1)
                            yourJson = yourJson.Replace("[", "").Replace("]", "").Trim();
                    }
                    catch (Exception e4) { }
                }
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }

            try
            {
                //Email Send Method By Vikas On 25-10-2023
                if (sendEmail.ToUpper() == "Y")
                {
                    string EmailDBName = System.Configuration.ConfigurationManager.AppSettings["EmailDBO"].ToString();
                    string EmailUserId = Convert.ToString(database.GetScalarValue("SELECT EmailUserId FROM CUSTOMER WITH(NOLOCK) WHERE username='" + userid + "'"));
                    if (EmailUserId != "")
                    {
                        string ApiKey = Convert.ToString(database.GetScalarValue("SELECT UM_ApiKEY FROM " + EmailDBName + @"USER_MASTER WITH(NOLOCK) WHERE UM_Profile_Id='" + EmailUserId + "'"));
                        obU.SendEMAILthroughAPI(EmailUserId, ApiKey, emailFrom, emailTo, emailCC, emailSubject, msg.Replace("'", "''"));
                    }
                }
            }
            catch (Exception e3) { }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //public async Task SendFailOverSmsRoute(string userid, string msgtype, bool ucs, string sender,string peid, string templateid, string msg,string m, ref DataTable dtRes, ref DataTable dtRes2)
        //{
        //    Util obU = new Util();
        //    Inetlab.SMPP.LicenseManager.SetLicense(obU.licenseContent);
        //    Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

        //    client.ResponseTimeout = TimeSpan.FromSeconds(60);
        //    client.EnquireLinkInterval = TimeSpan.FromSeconds(20);
        //    client.EncodingMapper.AddressEncoding = Encoding.ASCII;
        //    client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
        //    client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
        //    client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
        //    client.evUnBind += new UnBindEventHandler(client_evUnBind);
        //    client.evDataSm += new DataSmEventHandler(client_evDataSm);
        //    client.evRecoverySucceeded += ClientOnRecoverySucceeded;

        //    client.evServerCertificateValidation += OnCertificateValidation;

        //    client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte("0"), (AddressNPI)Convert.ToByte("0"));
        //    client.SystemType = ""; // tbSystemType.Text;
        //    client.ConnectionRecovery = true; // cbReconnect.Checked;
        //    client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
        //    client.EnabledSslProtocols = SslProtocols.None;

        //    //get account for messagetype and bind ----
        //    string smppaccountid = sms_acid;
        //    bool b = await client.ConnectAsync(sms_ip, sms_port);
        //    //System.Threading.Thread.Sleep(5000);
        //    BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //    //System.Threading.Thread.Sleep(200);

        //    if (client.Status != ConnectionStatus.Bound)
        //    {
        //        System.Threading.Thread.Sleep(1000);
        //        try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex1) { }
        //        BindResp Bresp2 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //        if (client.Status != ConnectionStatus.Bound)
        //        {
        //            System.Threading.Thread.Sleep(1000);
        //            try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex2) { }
        //            BindResp Bresp3 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //            if (client.Status != ConnectionStatus.Bound)
        //            {
        //                System.Threading.Thread.Sleep(1000);
        //                try { b = await client.ConnectAsync(sms_ip, sms_port); } catch (Exception Ex3) { }
        //                BindResp Bresp4 = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        //            }
        //        }
        //    }

        //    DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

        //    var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));
        //    expMin = 60 * 60;
        //    if (msgtype == "33") { expMin = 10; }

        //    List<SubmitSm> pduList = new List<SubmitSm>();
        //    if(1==1)
        //    {
        //        if (m.Length == 12 || m.Length == 10 || (msgtype == "16" && (m.Length == 12 || m.Length == 9) || (msgtype == "22" && m.Length == 11)))
        //        {
        //            string m1 = m;
        //            if (userid == "MIM2101322")
        //            {
        //                if (m.Length == 10) m1 = "91" + m;
        //            }

        //            var pduBuilder = SMS.ForSubmit()
        //             .From(sourceAddress)
        //             .To(m1)
        //             .Coding(coding)
        //             .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
        //             .Text(msg)
        //             .AddParameter(0x1400, (peid == null ? "" : peid))
        //             .AddParameter(0x1401, (templateid == null ? "" : templateid));
        //            if (sms_provider.Contains("AIRTEL"))
        //            {
        //                pduBuilder = SMS.ForSubmit()
        //                .From(sourceAddress)
        //                .To(m1)
        //                .Coding(coding)
        //                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
        //                .Text(msg)
        //                .AddParameter(0x1400, obU.getPEID(sms_provider, (peid == null ? "" : peid)))
        //                .AddParameter(0x1401, obU.getTEMPLATEID(sms_provider, (templateid == null ? "" : templateid)))
        //                .AddParameter(0x1402, obU.getTMID(sms_provider));
        //            }
        //            if (sms_provider.Contains("ETISALAT"))
        //            {
        //                pduBuilder = SMS.ForSubmit()
        //                .From(sourceAddress)
        //                .To(m1)
        //                .Coding(coding)
        //                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
        //                .Text(msg)
        //                .AddParameter(0x1400, System.Text.Encoding.UTF8.GetBytes(""))
        //                .AddParameter(0x1401, System.Text.Encoding.UTF8.GetBytes((sender.ToUpper().StartsWith("AD-") || sender.ToUpper().EndsWith("-AD") ? "" : "txn")));
        //            }
        //            pduList.AddRange(pduBuilder.Create(client));
        //        }
        //    }
        //    string result = "";
        //    if (pduList.Count > 0)
        //    {
        //        IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());
        //        UnBindResp Uresp = await client.UnbindAsync();
        //        await client.DisconnectAsync();
        //        //string s = obU.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
        //        for (int i = 0; i < resp.Count; i++)
        //        {
        //            DataRow dr1 = dtRes.NewRow();
        //            dr1["Mobile"] = resp[i].Request.DestinationAddress.Address;
        //            dr1["MessageID"] = resp[i].MessageId;
        //            dtRes.Rows.Add(dr1);

        //            if (userid == "MIM2300228" || userid == "MIM2300229" || userid == "MIM2000014")
        //            {
        //                DataRow dr2 = dtRes2.NewRow();
        //                dr2["Response"] = "SMS Submitted Successfully. Message ID: " + resp[i].MessageId;
        //                dtRes2.Rows.Add(dr2);

        //            }

        //            string msgt = resp[i].Request.MessageText.ToString();
        //            result = result + resp[i].Request.DestinationAddress.Address + "-" + resp[i].MessageId + ", ";
        //            if (msgtype == "47")
        //                obU.AddInMsgQueueGSM(userid, sender, Convert.ToString(dr1["Mobile"]), msg.Replace("'", "''"), msgtype, resp[i].MessageId, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
        //            else
        //            {

        //                obU.AddInMsgSubmittedMulti(userid, sender, resp[i].Request.DestinationAddress.Address, msgt, msgtype, Convert.ToString(resp[i].MessageId), Convert.ToString(resp[i].Header.Status), smppaccountid, peid, templateid, rate, msg, ucs, sms_provider + '-' + sms_systemid);

        //            }

        //        }
        //        if (result != "") result = result.Substring(0, result.Length - 2);

        //    }
        //    else
        //    {
        //        UnBindResp Uresp = await client.UnbindAsync();
        //        await client.DisconnectAsync();
        //    }
        //    yourJson = (result == "" ? "Invalid Parameters" : result);
        //}

        //Secured Encrypted
        [Route("UrlShortner")]
        [HttpPost]
        public async Task<HttpResponseMessage> UrlShortner([FromBody] shortner sms)
        {
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Response");

            string yourJson = "";
            bool ret = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            //validations
            //check user name and password
            if (string.IsNullOrEmpty(sms.userId)) { yourJson = "Invalid User ID"; ret = true; }
            if (string.IsNullOrEmpty(sms.pwd)) { yourJson = "Invalid Password"; ret = true; }
            if (string.IsNullOrEmpty(sms.longUrl)) { yourJson = "Invalid Long URL"; ret = true; }
            Util ob = new Util();
            DataTable dt = new DataTable();
            //Shishir 07/08/2023 start
            if (bSecure)
            {
                dt = ob.GetUserParameterSecurePWD(sms.userId, sms.pwd);
                if (!ret)
                    if (dt.Rows.Count <= 0)
                    {
                        yourJson += "Invalid Credentials"; ret = true;
                    }
            }
            //end
            else
            {
                dt = ob.GetUserParameter(sms.userId);
                if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID."; ret = true; }
                if (!ret)
                    if (sms.pwd != dt.Rows[0]["pwd"].ToString()) { yourJson += "Incorrect Password"; ret = true; }
            }
            string shurl = sms.longUrl;

            if (!shurl.StartsWith("http://") && !shurl.StartsWith("https://"))
            { yourJson = "Invalid Long URL. It should start with http:// or https://"; ret = true; }

            if (!ret)

            {
                string sql = string.Format("insert into APIURLLog (userId,longurl) values('{0}','{1}')", sms.userId, shurl.Replace("'", "''"));
                database.ExecuteNonQuery(sql);

                string domain = dt.Rows[0]["domainname"].ToString();
                string lblShortURL = "";
                string segment = ob.GetShortURLofLongURL(sms.userId, sms.longUrl);
                if (segment == "")
                {
                    string sUrl = ob.NewShortURLfromSQL(domain);
                    ob.SaveShortURL(sms.userId, shurl, "", sUrl, "N", "Y", domain);
                    lblShortURL = domain + sUrl;
                }
                else
                    lblShortURL = segment;
                string resp = "Short URL: " + lblShortURL;
                DataRow dr1 = dtRes.NewRow();
                dr1["Response"] = resp;
                dtRes.Rows.Add(dr1);
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dterr = new DataTable("dt");
                dterr.Columns.Add("Response");
                DataRow dr = dterr.NewRow();
                dr[0] = yourJson;
                dterr.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dterr);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted
        [Route("GetBalance")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetBalance([FromBody] smsbalance sms)
        {
            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Response");

            string yourJson = "";
            bool ret = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            //validations
            //check user name and password
            if (string.IsNullOrEmpty(sms.userId)) { yourJson = "Invalid User ID"; ret = true; }
            if (string.IsNullOrEmpty(sms.pwd)) { yourJson = "Invalid Password"; ret = true; }

            Util ob = new Util();
            DataTable dt = new DataTable();
            //Shishir 07/08/2023 start
            if (bSecure)
            {
                dt = ob.GetUserParameterSecurePWD(sms.userId, sms.pwd);//For PWD
                if (!ret)
                    if (dt.Rows.Count <= 0)
                    {
                        yourJson = "Invalid Credentials"; ret = true;
                    }
            }
            //end
            else
            {
                dt = ob.GetUserParameter(sms.userId);
                if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID."; ret = true; }
                if (!ret)
                    if (sms.pwd != dt.Rows[0]["pwd"].ToString()) { yourJson += "Incorrect Password"; ret = true; }
            }
            if (!ret)
            {
                string s = ob.GetBalance(sms.userId);
                string resp = "Balance : " + s;
                DataRow dr1 = dtRes.NewRow();
                dr1["Response"] = resp;
                dtRes.Rows.Add(dr1);
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dterr = new DataTable("dt");
                dterr.Columns.Add("Response");
                DataRow dr = dterr.NewRow();
                dr[0] = yourJson;
                dterr.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dterr);
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted
        [Route("SendSMSKWithLink")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMSKWithLink([FromBody] SMSLink sms)
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") HomeCR = true;

            string userid;
            string pwd;
            string apiKey;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string peid;
            string templateid;
            string longUrl;
            string existingUrl;
            string subclientcode = ""; //Added by Naved

            userid = sms.userid;
            pwd = sms.pwd;
            apiKey = sms.apiKey;
            mobile = sms.mobile;
            sender = sms.sender;
            msg = sms.msg;
            msgtype = sms.msgtype;
            peid = sms.peid;
            templateid = sms.templateid;
            longUrl = sms.longUrl;
            existingUrl = sms.existingUrl;
            subclientcode = Convert.ToString(sms.subclientcode); //Added by Naved

            if (string.IsNullOrEmpty(existingUrl)) { yourJson = "Invalid existingUrl"; ret = true; }
            if (string.IsNullOrEmpty(longUrl)) { yourJson = "Invalid longUrl"; ret = true; }
            //    if (string.IsNullOrEmpty(templateid)) { yourJson = "Invalid Template ID"; ret = true; }
            // if (string.IsNullOrEmpty(peid)) { yourJson = "Invalid PEID"; ret = true; }
            if (string.IsNullOrEmpty(msgtype)) { yourJson = "Invalid Message Type"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (string.IsNullOrEmpty(sender)) { yourJson = "Invalid senderid"; ret = true; }
            if (string.IsNullOrEmpty(mobile)) { yourJson = "Invalid mobile no"; ret = true; }
            if (string.IsNullOrEmpty(apiKey)) { yourJson = "Invalid apiKey"; ret = true; }
            if (string.IsNullOrEmpty(pwd)) { yourJson = "Invalid Password"; ret = true; }
            if (string.IsNullOrEmpty(userid)) { yourJson = "Invalid User ID"; ret = true; }
            if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
            { yourJson = "Invalid Long URL. It should start with http:// or https://"; ret = true; }

            mobile = mobile.Trim().Replace("+", "");
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
            Util obU = new Util();
            if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

            //validation of list count          
            //  if (mobList.Count > 30) { yourJson = "Mobile numbers cannot be more than 30"; ret = true; }

            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

            Util ob = new Util();
            DataTable dt = new DataTable();
            string domain = "";
            ob.InsertInAPiLog(Convert.ToString(userid), Convert.ToString(mobile), Convert.ToString(sender), Convert.ToString(msg), Convert.ToString(msgtype), Convert.ToString(peid), Convert.ToString(templateid));
            if (!ret)
            {
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apiKey);
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson = "Invalid Credentials"; ret = true;
                        }
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }
                    if (!ret)
                        if (pwd != dt.Rows[0]["PWD"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                    if (!ret)
                        if (apiKey != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect API key"; ret = true; }
                }
            }
            if (!ret)
                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18"))
                { yourJson = "Invalid Message Type"; ret = true; }

            //check valid sender id
            if (!ret)
                if (!ob.CheckSenderId(userid, sender)) { yourJson = "Invalid Sender ID"; ret = true; }
            if (!ret)
                domain = dt.Rows[0]["domainname"].ToString();

            //check balance
            double rate = 0;
            //string msg1 = msg.Trim() + " " + domain + "12345678";
            string msg1 = msg.Trim(); // + " " + domain + "12345678";
            if (msg1.Contains(longUrl))
                msg1 = msg1.Replace(longUrl, domain + "12345678");
            else
                msg1 = msg1 + " " + domain + "12345678";
            int noofsms = GetMsgCount(msg1);
            bool ucs = false;
            if (msg.Trim().Any(c => c > 126)) ucs = true;
            if (!ret)
            {
                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { yourJson = "Insufficient Balance"; ret = true; }
                }
                #endregion
            }

            //Insert data in MSGQUEUEB4singleAPI where Usrid is blank at 11/04/2023
            //if (Convert.ToString(Usrid) == "")
            //{
            //    string msgid1 = "";
            //    string ss1 = "";
            //    foreach (var m in mobList)
            //    {
            //        msgid1 = GetMsgID();
            //        obU.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs,templateid);
            //        ss1 += "MobileNo: " + m.ToString() + " Message ID: " + msgid1 + ", ";
            //    }
            //    if (mobList.Count == 1)
            //    {
            //        yourJson = "SMS Submitted Successfully. Message ID: " + msgid1; ret = true;
            //    }
            //    else
            //    {
            //        ss1 = ss1.Substring(0, ss1.Length - 2);
            //        yourJson = "SMS Submitted Successfully. Message ID: " + ss1; ret = true;
            //    }
            //}

            if (!ret)
            {
                if (msgtype != "16")
                {
                    // rabi for template DLT block 02/11/2022
                    string errcd_ = "5308";
                    if (templateid == null || templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                    {
                        string sql = "";
                        string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);

                        if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                        {
                            string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                            string e_tempid = ar1[0];
                            string e_peid = peid;
                            string e_sender = sender;

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
                                    string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                    " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                    " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                    " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                    " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                    database.ExecuteNonQuery(sql);
                                }
                                sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                lastMsgID = nid;
                            }
                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                            { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                            else
                            { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                        }
                        if (templID != "")
                            templateid = templID;
                    }
                    else
                    {
                        // rabi for template DLT block 02/11/2022
                        if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                        {
                            string e_peid = peid;
                            string e_sender = sender;
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
                                        string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                        nid = Guid.NewGuid().ToString();
                                        string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                    sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                    lastMsgID = nid;
                                }
                                string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                if (mobList.Count == 1)
                                { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                else
                                { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                            }
                        }

                    }
                }
            }

            if (!ret)
            {
                Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                client.ResponseTimeout = TimeSpan.FromSeconds(60);
                client.EnquireLinkInterval = TimeSpan.FromSeconds(20);
                client.EncodingMapper.AddressEncoding = Encoding.ASCII;
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

                    if (HomeCR)
                    {
                        sms_provider = sms_provider_HC;
                        sms_ip = sms_ip_HC;
                        sms_port = sms_port_HC;
                        sms_acid = sms_acid_HC;
                        sms_systemid = sms_systemid_HC;
                        sms_password = sms_password_HC;
                    }
                }
                else if (msgtype == "33")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;
                    if (HomeCR)
                    {
                        sms_provider = sms_provider_OTP_HC;
                        sms_ip = sms_ip_OTP_HC;
                        sms_port = sms_port_OTP_HC;
                        sms_acid = sms_acid_OTP_HC;
                        sms_systemid = sms_systemid_OTP_HC;
                        sms_password = sms_password_OTP_HC;
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
                else if (msgtype == "16")
                {
                    sms_provider = sms_provider_DUB;
                    sms_ip = sms_ip_DUB;
                    sms_port = sms_port_DUB;
                    sms_acid = sms_acid_DUB;
                    sms_systemid = sms_systemid_DUB;
                    sms_password = sms_password_DUB;

                    //if (userid == "MIM2200803" || userid == "MIM2200793" || userid =="MIM2200674" || userid == "MIM2200714" || userid == "MIM2200704" || userid == "MIM2200731" || userid == "MIM2200768" || userid == "MIM2200780" || userid == "MIM2200781")

                    int cnt1 = 0;
                    cnt1 = Convert.ToInt16(database.GetScalarValue("Select count(*) from UAEAPIACCOUNTPROMO with (nolock) where userid='" + userid + "' and senderid='" + sender + "' "));
                    if (cnt1 > 0)
                    {
                        sms_provider = sms_provider_DUB_3;
                        sms_ip = sms_ip_DUB_3;
                        sms_port = sms_port_DUB_3;
                        sms_acid = sms_acid_DUB_3;
                        sms_systemid = sms_systemid_DUB_3;
                        sms_password = sms_password_DUB_3;
                    }

                    int cnt = 0;
                    cnt = Convert.ToInt16(database.GetScalarValue("Select count(*) from UAEAPIAccounts with (nolock) where userid='" + userid + "'"));
                    if (cnt > 0)
                    {
                        sms_provider = sms_provider_DUB_2;
                        sms_ip = sms_ip_DUB_2;
                        sms_port = sms_port_DUB_2;
                        sms_acid = sms_acid_DUB_2;
                        sms_systemid = sms_systemid_DUB_2;
                        sms_password = sms_password_DUB_2;
                    }


                    #region REJECTION20240219
                    string ss1 = "";
                    string nid = "";
                    string m = mobile;
                    
                    int MobLen = Convert.ToInt32(m.Length);
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string errcd_ = "5308";
                                string smsTex = msg.Replace("'", "''");
                                nid = Guid.NewGuid().ToString();
                                string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTED err:" + errcd_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            ss1 += "MobileNo: " + m.ToString() + " Message ID: " + nid + ", ";

                            if (mobList.Count == 1)
                                yourJson = "SMS Submitted Successfully. Message ID: " + nid;
                            else
                            {
                                ss1 = ss1.Substring(0, ss1.Length - 2);
                                yourJson = "SMS Submitted Successfully. " + ss1;
                            }
                        }
                    }
                    #endregion
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

                else if (msgtype == "22")
                {
                    sms_provider = sms_provider_QTR;
                    sms_ip = sms_ip_QTR;
                    sms_port = sms_port_QTR;
                    sms_acid = sms_acid_QTR;
                    sms_systemid = sms_systemid_QTR;
                    sms_password = sms_password_QTR;

                    DataTable dtQ = database.GetDataTable("Select account from QatarAPIACCOUNTPROMO with (nolock) where userid = '" + userid + "' and senderid = '" + sender + "'");
                    if (dtQ.Rows.Count > 0)
                    {
                        string account = Convert.ToString(dtQ.Rows[0]["account"]);
                        if (account.ToUpper() == "BROADNET")
                        {
                            sms_provider = sms_provider_QTR;
                            sms_ip = sms_ip_QTR;
                            sms_port = sms_port_QTR;
                            sms_acid = sms_acid_QTR;
                            sms_systemid = sms_systemid_QTR;
                            sms_password = sms_password_QTR;
                        }
                        else
                        {
                            sms_provider = sms_provider_QTR2;
                            sms_ip = sms_ip_QTR2;
                            sms_port = sms_port_QTR2;
                            sms_acid = sms_acid_QTR2;
                            sms_systemid = sms_systemid_QTR2;
                            sms_password = sms_password_QTR2;
                        }
                    }
                    else
                    {
                        DataTable dtQ2 = database.GetDataTable("Select account from QatarAPIAccounts with (nolock) where userid='" + userid + "'");
                        if (dtQ2.Rows.Count > 0)
                        {
                            string account = Convert.ToString(dtQ2.Rows[0]["account"]);
                            if (account.ToUpper() == "BROADNET")
                            {
                                sms_provider = sms_provider_QTR;
                                sms_ip = sms_ip_QTR;
                                sms_port = sms_port_QTR;
                                sms_acid = sms_acid_QTR;
                                sms_systemid = sms_systemid_QTR;
                                sms_password = sms_password_QTR;
                            }
                            else
                            {
                                sms_provider = sms_provider_QTR2;
                                sms_ip = sms_ip_QTR2;
                                sms_port = sms_port_QTR2;
                                sms_acid = sms_acid_QTR2;
                                sms_systemid = sms_systemid_QTR2;
                                sms_password = sms_password_QTR2;
                            }
                        }
                    }
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

                //Added by Naved On 29/09/2023
                //if ((sender.ToUpper() == "HIIBPL" || userid == "MIM2201048" || userid == "MIM2201104" ||
                //    userid == "MIM2300228" || userid == "MIM2300229") && Usrid == "" && (msgtype == "13" || msgtype =="33")
                //{
                //    if (!ret)
                //    {
                //        string msgid2 = "";
                //        string ss2 = "";
                //        foreach (var m in mobList)
                //        {
                //            //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                //            msgid2 = GetMsgID();
                //            new Util().AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode);
                //            ss2 += "MobileNo: " + m.ToString() + " Message ID: " + msgid2 + ", ";
                //        }
                //        if (mobList.Count == 1)
                //        { yourJson = "SMS Submitted Successfully. Message ID: " + msgid2; ret = true; }
                //        else
                //        {
                //            ss2 = ss2.Substring(0, ss2.Length - 2);
                //            { yourJson = "SMS Submitted Successfully. Message ID: " + ss2; ret = true; }
                //        }
                //    }
                //}



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

                if (existingUrl.ToUpper() == "N")
                {
                    string sUrl = ob.NewShortURLfromSQL(domain);
                    ob.SaveShortURL(userid, longUrl, "", sUrl, "Y", "Y", domain);
                }
                string _shortUrlId = ob.GetShortUrlIdFromLongURL(longUrl, userid);

                DataTable dtMobTrackUrl = new DataTable();
                dtMobTrackUrl.Columns.Add("Mob");
                dtMobTrackUrl.Columns.Add("segment");
                //Add Short URL Id
                DataColumn newColumn = new DataColumn("shorturlid");
                newColumn.DefaultValue = _shortUrlId;
                dtMobTrackUrl.Columns.Add(newColumn);

                //get account for messagetype and bind ----
                string smppaccountid = sms_acid;
                string msg_orig = msg;
                int mobcnt = 0;

                if (Convert.ToString(Usrid) == "")
                {
                    string msgid1 = "";
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msg = msg_orig;
                        SetMultipleShortURL(ref msg, longUrl, existingUrl, ob, domain, dtMobTrackUrl, m, msg_orig);

                        if (dtMobTrackUrl != null)
                        {
                            string sql = "";
                            foreach (DataRow dr in dtMobTrackUrl.Rows)
                            {
                                sql = string.Format("insert into mobtrackurl(urlid, mobile, segment, sentdate,templateId) values ('{0}','{1}','{2}',GETDATE(),'{3}')", dr["shorturlid"], dr["Mob"], dr["segment"], templateid);
                                database.ExecuteNonQuery(sql);
                            }
                        }

                        msgid1 = GetMsgID();
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode);
                        ss1 += "MobileNo: " + m.ToString() + " Message ID: " + msgid1 + ", ";
                    }
                    if (mobList.Count == 1)
                        yourJson = "SMS Submitted Successfully. Message ID: " + msgid1;
                    else
                    {
                        ss1 = ss1.Substring(0, ss1.Length - 2);
                        yourJson = "SMS Submitted Successfully. " + ss1;
                    }
                }
                else
                {
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

                    if (templateid == "TEMPLATE-ID")
                    {
                        foreach (var m in mobList)
                        {
                            if (m.Length == 12 || m.Length == 10)
                            {
                                string msg2 = SetMultipleShortURL(ref msg, longUrl, existingUrl, ob, domain, dtMobTrackUrl, m, sms.msg);

                                mobcnt++;
                                var pduBuilder = SMS.ForSubmit()
                                  .From(sourceAddress)
                                  .To(m)
                                  .Coding(coding)
                                  .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                  .Text(msg2)
                                  .AddParameter(0x1400, peid);
                                pduList.AddRange(pduBuilder.Create(client));
                            }
                        }
                    }
                    else if (peid != null && templateid != null)
                    {
                        foreach (var m in mobList)
                        {
                            if (m.Length == 12 || m.Length == 10)
                            {
                                string msg2 = SetMultipleShortURL(ref msg, longUrl, existingUrl, ob, domain, dtMobTrackUrl, m, sms.msg);

                                mobcnt++;
                                var pduBuilder = SMS.ForSubmit()
                               .From(sourceAddress)
                               .To(m)
                               .Coding(coding)
                               .DeliveryReceipt()
                               .Text(msg2)
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
                    else
                    {
                        foreach (var m in mobList)
                        {
                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                                 .From(sourceAddress)
                                 .To(m)
                                 .Coding(coding)
                                 .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                 .Text(msg);
                            if (sms_provider.Contains("ETISALAT"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                .AddParameter(0x1400, System.Text.Encoding.UTF8.GetBytes(""))
                                .AddParameter(0x1401, System.Text.Encoding.UTF8.GetBytes((sender.ToUpper().StartsWith("AD-") || sender.ToUpper().EndsWith("-AD") ? "" : "txn")));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                            //}
                        }
                    }
                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();

                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid, dtMobTrackUrl);

                    string result = "";
                    for (int i = 0; i < resp.Count; i++)
                    {
                        DataRow dr1 = dtRes.NewRow();
                        dr1["Mobile"] = resp[i].Request.DestinationAddress.Address.ToString();
                        dr1["MessageID"] = resp[i].MessageId.ToString();
                        dtRes.Rows.Add(dr1);

                        result += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                    }
                    if (result != "") result = result.Substring(0, result.Length - 2);
                    yourJson = (result == "" ? "Invalid Parameters" : result);
                }
            }
            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }

            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        //Secured Encrypted N
        [Route("SendSMSKURLShortner")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSMSKURLShortner([FromBody] SMSLink sms)
        {
            string createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") { HomeCR = true; bSecure = false; }

            string userid;
            string pwd;
            string apiKey;
            string mobile;
            string sender;
            string msg;
            string msgtype;
            string peid;
            string templateid;
            string longUrl;
            string existingUrl;

            userid = sms.userid;
            pwd = sms.pwd;
            apiKey = sms.apiKey;
            mobile = sms.mobile;
            sender = sms.sender;
            msg = sms.msg;
            msgtype = sms.msgtype;
            peid = sms.peid;
            templateid = ""; // sms.templateid;
            longUrl = ""; // sms.longUrl;
            existingUrl = "N"; // sms.existingUrl;

            //if (string.IsNullOrEmpty(existingUrl)) { yourJson = "Invalid existingUrl"; ret = true; }
            //if (string.IsNullOrEmpty(longUrl)) { yourJson = "Invalid longUrl"; ret = true; }
            //    if (string.IsNullOrEmpty(templateid)) { yourJson = "Invalid Template ID"; ret = true; }
            if (string.IsNullOrEmpty(peid)) { yourJson = "Invalid PEID"; ret = true; }
            if (string.IsNullOrEmpty(msgtype)) { yourJson = "Invalid Message Type"; ret = true; }
            if (Convert.ToString(msg).Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }
            if (string.IsNullOrEmpty(sender)) { yourJson = "Invalid senderid"; ret = true; }
            if (string.IsNullOrEmpty(mobile)) { yourJson = "Invalid mobile no"; ret = true; }
            if (string.IsNullOrEmpty(apiKey)) { yourJson = "Invalid apiKey"; ret = true; }
            if (string.IsNullOrEmpty(pwd)) { yourJson = "Invalid Password"; ret = true; }
            if (string.IsNullOrEmpty(userid)) { yourJson = "Invalid User ID"; ret = true; }
            //if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
            //{ yourJson = "Invalid Long URL. It should start with http:// or https://"; ret = true; }
            msg = msg.Replace(Convert.ToChar(160), Convert.ToChar(32));
            mobile = mobile.Trim().Replace("+", "");
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

            Util obU = new Util();
            if (obU.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }

            //validation of list count          
            //  if (mobList.Count > 30) { yourJson = "Mobile numbers cannot be more than 30"; ret = true; }

            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

            string result = "";
            Util ob = new Util();
            DataTable dt = new DataTable();
            string domain = "";
            ob.InsertInAPiLog(Convert.ToString(userid), Convert.ToString(mobile), Convert.ToString(sender), Convert.ToString(msg), Convert.ToString(msgtype), Convert.ToString(peid), Convert.ToString(templateid));
            if (!ret)
            {
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apiKey);
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson = "Invalid Credentials"; ret = true;
                        }
                }
                //Shishir 07/08/2023
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }
                    if (!ret)
                        if (pwd != dt.Rows[0]["PWD"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                    if (!ret)
                        if (apiKey != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect API key"; ret = true; }
                }
            }
            if (!ret)
                //if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18"))
                if (!(msgtype == "13" || msgtype == "33"))
                { yourJson = "Invalid Message Type"; ret = true; }

            //check valid sender id
            if (!ret)
                if (!ob.CheckSenderId(userid, sender)) { yourJson = "Invalid Sender ID"; ret = true; }
            if (!ret)
                domain = dt.Rows[0]["domainname"].ToString();

            //check balance
            double rate = 0;
            //string msg1 = msg.Trim() + " " + domain + "12345678";
            string msg1 = msg.Trim();
            int noofsms = GetMsgCount(msg1);

            if (!ret)
            {
                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { yourJson = "Insufficient Balance"; ret = true; }
                }
                #endregion
            }
            #region <SMPP Account Setting >
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

                if (HomeCR)
                {
                    //string mn = Convert.ToString(DateTime.Now.Minute);
                    //if (mn == "1" || mn == "2" || mn == "3" || mn == "7" || mn == "8" || mn == "9" || mn == "13" || mn == "14" || mn == "15" || mn == "19" || mn == "20" || mn == "21" || mn == "25" || mn == "26" || mn == "27" ||
                    //     mn == "31" || mn == "32" || mn == "33" || mn == "37" || mn == "38" || mn == "39" || mn == "43" || mn == "44" || mn == "45" ||
                    //      mn == "49" || mn == "50" || mn == "51" || mn == "55" || mn == "56" || mn == "57" )
                    //{
                    sms_provider = sms_provider_HC;
                    sms_ip = sms_ip_HC;
                    sms_port = sms_port_HC;
                    sms_acid = sms_acid_HC;
                    sms_systemid = sms_systemid_HC;
                    sms_password = sms_password_HC;
                    //}
                    //else
                    //{
                    //    sms_provider = sms_provider_HC2;
                    //    sms_ip = sms_ip_HC2;
                    //    sms_port = sms_port_HC2;
                    //    sms_acid = sms_acid_HC2;
                    //    sms_systemid = sms_systemid_HC2;
                    //    sms_password = sms_password_HC2;
                    //}
                }
            }
            else if (msgtype == "33")
            {
                sms_provider = sms_provider_OTP;
                sms_ip = sms_ip_OTP;
                sms_port = sms_port_OTP;
                sms_acid = sms_acid_OTP;
                sms_systemid = sms_systemid_OTP;
                sms_password = sms_password_OTP;
                if (HomeCR)
                {
                    sms_provider = sms_provider_OTP_HC;
                    sms_ip = sms_ip_OTP_HC;
                    sms_port = sms_port_OTP_HC;
                    sms_acid = sms_acid_OTP_HC;
                    sms_systemid = sms_systemid_OTP_HC;
                    sms_password = sms_password_OTP_HC;
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
            #endregion
            string[] link = new string[10];
            int totlinks = 0;
            int lnk = 0;
            if (msg1.ToLower().Contains("http"))
            {
                string[] ar1 = msg1.Split(' ');
                for (int w = 0; w < ar1.Length; w++)
                    if (ar1[w].ToLower().Contains("http"))
                    {
                        link[lnk] = ar1[w].Trim();
                        lnk++;
                    }
            }
            totlinks = lnk;

            bool ucs = false;
            if (msg.Trim().Any(c => c > 126)) ucs = true;
            if (!ret && (msgtype == "13" || msgtype == "33"))
            {
                foreach (var m in mobList)
                {
                    string msgid = "";
                    msgid = GetMsgID();
                    DataTable dtx = new DataTable();
                    ob.AddInMsgQueue_HC(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, dtx, createdAt);
                    //string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                    DataRow dr1 = dtRes.NewRow();
                    dr1["Mobile"] = m;
                    dr1["MessageID"] = msgid;
                    dtRes.Rows.Add(dr1);
                    result += "MobileNo: " + m + " Message ID: " + msgid + ", ";
                }
            }
            else if (!ret && msgtype != "13")
            {
                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == null || templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                {
                    string sql = "";
                    string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs, "Y");

                    if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                        string e_tempid = ar1[0];
                        string e_peid = peid;
                        string e_sender = sender;

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
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "','" + createdAt + "',GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "','" + createdAt + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                        { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                        else
                        { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                    }
                    if (templID != "")
                        templateid = templID;
                }
                else
                {
                    // rabi for template DLT block 02/11/2022
                    if ((msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                    {
                        string e_peid = peid;
                        string e_sender = sender;
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
                                    string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                    nid = Guid.NewGuid().ToString();
                                    string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                     " select '1' as id,'','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "','" + createdAt + "',GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                     " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                     " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                     " select '1' as id,'','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "','" + createdAt + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                            { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                            else
                            { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                        }
                    }


                }

                if (templateid != "")
                {
                    string[] ar1 = templateid.Split(new string[] { "#$" }, StringSplitOptions.None);
                    templateid = ar1[0];

                    if (totlinks > 0)
                    {
                        string msTxt = ar1[1].ToLower();
                        for (int x = 0; x < totlinks; x++)
                            if (msTxt.Contains(link[x].ToLower()))
                                link[x] = "";

                        for (int x = 0; x < totlinks; x++)
                            if (link[x] != "")
                                msg1 = msg1.Replace(link[x], domain + "123456");
                        noofsms = GetMsgCount(msg1);
                    }
                }
                /* balance check and return */
                if (!ret)
                {
                    if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                    if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                    if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                    if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                    #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                    if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                    {
                        if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                        { yourJson = "Insufficient Balance"; ret = true; }
                    }
                    #endregion
                }

                if (!ret)
                {
                    #region < SMPP CLIENT >
                    Inetlab.SMPP.LicenseManager.SetLicense(ob.licenseContent);
                    Inetlab.SMPP.SmppClient client = new Inetlab.SMPP.SmppClient();

                    client.ResponseTimeout = TimeSpan.FromSeconds(60);
                    client.EnquireLinkInterval = TimeSpan.FromSeconds(20);
                    client.EncodingMapper.AddressEncoding = Encoding.ASCII;
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
                    #endregion

                    int total_lnk = 0;
                    if (totlinks > 0)
                        for (int x = 0; x < totlinks; x++)
                            if (link[x] != "") total_lnk++;

                    string _shortUrlId = "";
                    string[] shortUrlIdAr = new string[totlinks];
                    if (total_lnk > 0)
                    {
                        for (int x = 0; x < totlinks; x++)
                        {
                            if (link[x] != "")
                            {
                                string sUrl = ob.NewShortURLfromSQL4HC(domain);
                                _shortUrlId = ob.SaveShortURL4HC(userid, link[x], "", sUrl, "Y", "Y", domain);
                                shortUrlIdAr[x] = _shortUrlId;
                            }
                        }
                    }

                    DataTable dtMobTrackUrl = new DataTable();
                    dtMobTrackUrl.Columns.Add("Mob");
                    dtMobTrackUrl.Columns.Add("segment");
                    //Add Short URL Id
                    DataColumn newColumn = new DataColumn("shorturlid");
                    newColumn.DefaultValue = _shortUrlId;
                    dtMobTrackUrl.Columns.Add(newColumn);

                    //get account for messagetype and bind ----
                    string smppaccountid = sms_acid;

                    if (msgtype != "13")
                    {
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
                    }

                    //System.Threading.Thread.Sleep(200);
                    var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                    List<SubmitSm> pduList = new List<SubmitSm>();
                    DataCodings coding = (DataCodings)Enum.Parse(typeof(DataCodings), (ucs ? "UCS2" : "Default"));

                    int mobcnt = 0;
                    if (templateid == "TEMPLATE-ID")
                    {
                        //foreach (var m in mobList)
                        //{
                        //    if (m.Length == 12 || m.Length == 10)
                        //    {
                        //        string msg2 = SetMultipleShortURL(ref msg, longUrl, existingUrl, ob, domain, dtMobTrackUrl, m, sms.msg);

                        //        mobcnt++;
                        //        var pduBuilder = SMS.ForSubmit()
                        //          .From(sourceAddress)
                        //          .To(m)
                        //          .Coding(coding)
                        //          .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                        //          .Text(msg2)
                        //          .AddParameter(0x1400, peid);
                        //        pduList.AddRange(pduBuilder.Create(client));
                        //    }
                        //}
                    }
                    else
                    {
                        foreach (var m in mobList)
                        {
                            if (m.Length == 12 || m.Length == 10)
                            {
                                string msg2 = msg;
                                if (total_lnk > 0)
                                    msg2 = SetMultipleShortURLAll(ref msg, link, existingUrl, ob, domain, dtMobTrackUrl, m, sms.msg, shortUrlIdAr, totlinks);
                                mobcnt++;
                                var pduBuilder = SMS.ForSubmit()
                               .From(sourceAddress)
                               .To(m)
                               .Coding(coding)
                               .DeliveryReceipt()
                               .Text(msg2)
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

                                if (msgtype == "13")
                                {
                                    string msgid = "";
                                    msgid = GetMsgID();
                                    ob.AddInMsgQueue_HC(userid, sender, m, msg2.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, dtMobTrackUrl, createdAt);
                                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                                    DataRow dr1 = dtRes.NewRow();
                                    dr1["Mobile"] = m;
                                    dr1["MessageID"] = msgid;
                                    dtRes.Rows.Add(dr1);
                                    result += "MobileNo: " + m + " Message ID: " + msgid + ", ";
                                }
                            }
                        }
                    }

                    if (msgtype != "13")
                    {
                        IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                        UnBindResp Uresp = await client.UnbindAsync();
                        await client.DisconnectAsync();

                        string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                        ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid, dtMobTrackUrl, createdAt);

                        for (int i = 0; i < resp.Count; i++)
                        {
                            DataRow dr1 = dtRes.NewRow();
                            dr1["Mobile"] = resp[i].Request.DestinationAddress.Address.ToString();
                            dr1["MessageID"] = resp[i].MessageId.ToString();
                            dtRes.Rows.Add(dr1);

                            result += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                        }
                    }

                    if (result != "") result = result.Substring(0, result.Length - 2);
                    yourJson = (result == "" ? "Invalid Parameters" : result);
                }
            }

            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
            }
            else
            {
                DataTable dt1 = new DataTable("dt");
                dt1.Columns.Add("Response");
                DataRow dr = dt1.NewRow();
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }

            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

        private static string SetMultipleShortURLAll(ref string msg, string[] link, string existingURL, Util ob, string domain, DataTable dtMobTrackUrl, string mobile, string originalmsg, string[] shortUrlIdAr, int totlinks)
        {
            for (int x = 0; x < totlinks; x++)
            {
                if (link[x] != "")
                {
                    string _segment = ob.NewShortURLforMobTrkSQL_6char();
                    string lblShortURL = "";

                    lblShortURL = domain + _segment;
                    DataRow row = dtMobTrackUrl.NewRow();
                    row["Mob"] = mobile;
                    row["segment"] = _segment;
                    dtMobTrackUrl.Rows.Add(row);

                    if (originalmsg.ToLower().Contains(link[x].ToLower()))
                    {
                        originalmsg = originalmsg.Trim().Replace(link[x], lblShortURL);
                    }
                    else
                    {
                        msg = originalmsg.Trim() + " " + lblShortURL;
                    }
                }
            }
            msg = originalmsg;
            return msg;
        }

        private static string SetMultipleShortURL(ref string msg, string LongURL, string existingURL, Util ob, string domain, DataTable dtMobTrackUrl, string mobile, string originalmsg)
        {
            string _segment = ob.NewShortURLforMobTrkSQL();
            string lblShortURL = "";
            if (existingURL.ToUpper() == "N")
            {
                lblShortURL = domain + _segment;
                DataRow row = dtMobTrackUrl.NewRow();
                row["Mob"] = mobile;
                row["segment"] = _segment;
                dtMobTrackUrl.Rows.Add(row);
                if (originalmsg.Contains(LongURL))
                {
                    originalmsg = originalmsg.Trim().Replace(LongURL, lblShortURL);
                    msg = originalmsg;
                }
                else
                {
                    msg = originalmsg.Trim() + " " + lblShortURL;
                }
            }
            else if (existingURL.ToUpper() == "Y")
            {
                lblShortURL = domain + _segment;
                DataRow row = dtMobTrackUrl.NewRow();
                row["Mob"] = mobile;
                row["segment"] = _segment;
                dtMobTrackUrl.Rows.Add(row);

                if (originalmsg.Contains(LongURL))
                {
                    originalmsg = originalmsg.Trim().Replace(LongURL, lblShortURL);
                    msg = originalmsg;
                }
                else
                {
                    msg = originalmsg.Trim() + " " + lblShortURL;
                }
            }
            return msg;
        }

        public async Task bind(Inetlab.SMPP.SmppClient client)
        {
            BindResp Bresp = await client.BindAsync(sms_systemid, sms_password, ConnectionMode.Transmitter);
        }
        public async Task connect(Inetlab.SMPP.SmppClient client)
        {
            await client.ConnectAsync(sms_ip, sms_port);
        }

        public string GetMsgID()
        {
            return "S" + Convert.ToString(Guid.NewGuid());
        }
        public int GetMsgCount(string msg)
        {
            smsCounter smsOB = new smsCounter();
            int scnt = smsOB.CalculateSmsCount(msg);
            return scnt;

            //smsCounter_CS sOb = new smsCounter_CS();
            //int sc = sOb.CalculateSmsCount(msg);
            //return sc;

            string q = msg.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = q.Length + count_PIPE;

            int count_tild = q.Count(f => f == '~');
            qlen = qlen + count_tild;

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

        // GET: api/sms/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/sms
        public void Post([FromBody]string value)
        {
        }

        //Secured Encrypted
        [Route("GetDelivery")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDelivery(SMSDelivery sms)
        {
            try
            {
                DataTable dtRes = new DataTable("dtRes");
                dtRes.Columns.Add("Response");


                string yourJson = "";
                bool ret = false;
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                //validations
                //check user name and password
                if (string.IsNullOrEmpty(sms.userId)) { yourJson = "Invalid User ID"; ret = true; }
                if (string.IsNullOrEmpty(sms.pwd)) { yourJson = "Invalid Password"; ret = true; }
                if (string.IsNullOrEmpty(sms.msgId)) { yourJson = "Invalid Message ID"; ret = true; }
                Util ob = new Util();
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecurePWD(sms.userId, sms.pwd);//For Pwd
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            dt = ob.GetUserParameterSecure(sms.userId, sms.pwd);//for Apikey
                            if (dt.Rows.Count <= 0)
                            {
                                yourJson += "Invalid Credentials"; ret = true;
                            }
                        }
                }
                //Shishir 07/08/2023 end
                else
                {
                    dt = ob.GetUserParameter(sms.userId);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID."; ret = true; }

                    if (!ret)
                        if (sms.pwd != dt.Rows[0]["pwd"].ToString())
                        {
                            if (sms.pwd != dt.Rows[0]["apikey"].ToString())
                                yourJson += "Incorrect Password"; ret = true;
                        }
                }
                DataTable dt1 = new DataTable();
                if (!ret)
                {
                    dt1 = ob.GetDelivery(sms.msgId);
                    if (dt1.Rows.Count <= 0) { yourJson = "Invalid Message ID"; ret = true; }
                    if (!ret)
                        if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != sms.userId.ToUpper()) { yourJson = "Invalid Message ID."; ret = true; }
                }

                if (!ret)
                {
                    string resp = "";
                    if (dt1.Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
                    else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
                    {
                        resp = "Delivery Status : Delivered";
                        resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    else resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                    //added by naved
                    string SubClientCode = string.Empty;
                    if (!ret)
                    {
                        DataTable dt2 = ob.GetDeliveryWithSubClientCode(sms.msgId);
                        if (dt2.Rows.Count > 0)
                        {
                            SubClientCode = dt2.Rows[0]["SubClientCode"].ToString();
                            resp = resp + ", SubClientCode : " + SubClientCode;
                        }
                    }
                    //resp = resp;
                    DataRow dr1 = dtRes.NewRow();
                    dr1["Response"] = resp;

                    dtRes.Rows.Add(dr1);
                }
                if (dtRes.Rows.Count > 0)
                {
                    yourJson = JsonConvert.SerializeObject(dtRes);
                }
                else
                {
                    DataTable dterr = new DataTable("dt");
                    dterr.Columns.Add("Response");
                    DataRow dr = dterr.NewRow();
                    dr[0] = yourJson;
                    dterr.Rows.Add(dr);
                    yourJson = JsonConvert.SerializeObject(dterr);
                }
                response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                DataTable dterr = new DataTable("dt");
                dterr.Columns.Add("Response");
                DataRow dr = dterr.NewRow();
                dr[0] = ex.Message;
                dterr.Rows.Add(dr);
                string yourJson1 = JsonConvert.SerializeObject(dterr);

                var response = this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(yourJson1, Encoding.UTF8, "application/json");
                return response;
            }

        }

        //Secured Encrypted N
        [Route("GetDeliveryWithCode")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeliveryWithCode(SMSDelivery sms)
        {
            try
            {
                DataTable dtRes = new DataTable("dtRes");
                dtRes.Columns.Add("Response");

                string yourJson = "";
                bool ret = false;
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                //validations
                //check user name and password
                if (string.IsNullOrEmpty(sms.userId)) { yourJson = "Invalid User ID"; ret = true; }
                if (string.IsNullOrEmpty(sms.pwd)) { yourJson = "Invalid Password"; ret = true; }
                if (string.IsNullOrEmpty(sms.msgId)) { yourJson = "Invalid Message ID"; ret = true; }
                Util ob = new Util();
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecurePWD(sms.userId, sms.pwd);//FOR PWD
                    if (!ret)
                        if (dt.Rows.Count <= 0)
                        {
                            yourJson += "Incorrect Password"; ret = true;
                        }
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(sms.userId);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID."; ret = true; }
                    if (!ret)
                        if (sms.pwd != dt.Rows[0]["pwd"].ToString()) { yourJson += "Incorrect Password"; ret = true; }
                }
                DataTable dt1 = ob.GetDelivery(sms.msgId);
                if (dt1.Rows.Count <= 0) { yourJson = "Invalid Message ID"; ret = true; }
                if (!ret)
                    if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != sms.userId.ToUpper()) { yourJson = "Invalid Message ID."; ret = true; }

                if (!ret)
                {
                    string resp = "";
                    if (dt1.Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
                    else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
                    {
                        resp = "Delivery Status : Delivered";
                        resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    else resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                    string SubClientCode = string.Empty;
                    if (!ret)
                    {
                        DataTable dt2 = ob.GetDeliveryWithCode(sms.msgId);
                        if (dt2.Rows.Count > 0)
                        {
                            SubClientCode = dt2.Rows[0]["SubClientCode"].ToString();
                        }
                    }
                    resp = resp + ", Code : " + SubClientCode;
                    DataRow dr1 = dtRes.NewRow();
                    dr1["Response"] = resp;

                    dtRes.Rows.Add(dr1);
                }
                if (dtRes.Rows.Count > 0)
                {
                    yourJson = JsonConvert.SerializeObject(dtRes);
                }
                else
                {
                    DataTable dterr = new DataTable("dt");
                    dterr.Columns.Add("Response");
                    DataRow dr = dterr.NewRow();
                    dr[0] = yourJson;
                    dterr.Rows.Add(dr);
                    yourJson = JsonConvert.SerializeObject(dterr);
                }

                response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                DataTable dterr = new DataTable("dt");
                dterr.Columns.Add("Response");
                DataRow dr = dterr.NewRow();
                dr[0] = ex.Message;
                dterr.Rows.Add(dr);
                string yourJson1 = JsonConvert.SerializeObject(dterr);

                var response = this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(yourJson1, Encoding.UTF8, "application/json");
                return response;
            }

        }

        //[Route("GetDeliveryWithWaba")]
        //[HttpPost]
        //public async Task<HttpResponseMessage> GetSmsDeliveryWithWaba(SMSDelivery sms)
        //{
        //    try
        //    {
        //        string respwaba = "";
        //        DataTable dtRes = new DataTable("dtRes");
        //        dtRes.Columns.Add("Response");


        //        string yourJson = "";
        //        bool ret = false;
        //        var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //        //validations
        //        //check user name and password
        //        if (string.IsNullOrEmpty(sms.userId)) { yourJson = "Invalid User ID"; ret = true; }
        //        if (string.IsNullOrEmpty(sms.pwd)) { yourJson = "Invalid Password"; ret = true; }
        //        if (string.IsNullOrEmpty(sms.msgId)) { yourJson = "Invalid Message ID"; ret = true; }
        //        Util ob = new Util();
        //        DataTable dt = ob.GetUserParameter(sms.userId);
        //        if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID."; ret = true; }
        //        if (!ret)
        //            if (sms.pwd != dt.Rows[0]["pwd"].ToString()) { yourJson += "Incorrect Password"; ret = true; }

        //        DataSet dt1 = ob.GetDeliveryWithWABA(sms.msgId);
        //        if (dt1.Tables[0].Rows.Count <= 0) { yourJson = "Invalid Message ID"; ret = true; }
        //        if (!ret)
        //            if (dt1.Tables[0].Rows[0]["PROFILEID"].ToString().ToUpper() != sms.userId.ToUpper()) { yourJson = "Invalid Message ID."; ret = true; }

        //        if (!ret)
        //        {
        //            string resp = "";
        //            if (dt1.Tables[0].Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
        //            else if (dt1.Tables[0].Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
        //            {

        //                if (dt1.Tables[1].Rows.Count > 0)
        //                {
        //                    foreach (DataRow dr in dt1.Tables[1].Rows)
        //                    {
        //                        if (respwaba.ToString() == "")
        //                        {
        //                            respwaba = dr["Status"].ToString();
        //                        }
        //                        else
        //                        {
        //                            respwaba = respwaba + "," + dr["Status"].ToString();
        //                        }

        //                    }
        //                }
        //                if (respwaba != "")
        //                {
        //                    resp = "Delivery Status : Delivered" + " and Waba Status : " + respwaba;
        //                }
        //                else
        //                {
        //                    resp = "Delivery Status : Delivered" + " and Waba not Sent";
        //                }

        //                //resp = "Delivery Status : Delivered";
        //                resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Tables[0].Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
        //            }
        //            else
        //                resp = "Delivery Status : Failed. Error Code : " + dt1.Tables[0].Rows[0]["errcd"].ToString();

        //            DataRow dr1 = dtRes.NewRow();
        //            dr1["Response"] = resp;

        //            dtRes.Rows.Add(dr1);
        //        }
        //        if (dtRes.Rows.Count > 0)
        //        {
        //            yourJson = JsonConvert.SerializeObject(dtRes);
        //        }
        //        else
        //        {
        //            DataTable dterr = new DataTable("dt");
        //            dterr.Columns.Add("Response");
        //            DataRow dr = dterr.NewRow();
        //            dr[0] = yourJson;
        //            dterr.Rows.Add(dr);
        //            yourJson = JsonConvert.SerializeObject(dterr);
        //        }
        //        response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        DataTable dterr = new DataTable("dt");
        //        dterr.Columns.Add("Response");
        //        DataRow dr = dterr.NewRow();
        //        dr[0] = ex.Message;
        //        dterr.Rows.Add(dr);
        //        string yourJson1 = JsonConvert.SerializeObject(dterr);

        //        var response = this.Request.CreateResponse(HttpStatusCode.InternalServerError);
        //        response.Content = new StringContent(yourJson1, Encoding.UTF8, "application/json");
        //        return response;
        //    }

        //}

        //        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid)
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
        //                ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, "");
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



        //--------------------------------------------------------------------------

        //Secured Encrypted N

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
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecurePWD(userid, pwd);//For PWD
                    if (dt.Rows.Count <= 0)
                    {
                        return "Invalid Credentials";
                    }
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID.";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                }
                DataTable dt1 = ob.GetDelivery(msgid);
                if (dt1.Rows.Count <= 0) return "Invalid Message ID";
                if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != userid.ToUpper()) return "Invalid Message ID.";
                string resp = "";
                if (dt1.Rows[0]["dlvrstat"].ToString() == "") resp = "Delivery Status : Unknown";
                else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED") resp = "Delivery Status : Delivered"; // Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                else resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                return resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region < SMPP EVENTS >
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
        #endregion
    }

    public class clsHmil
    {
        public Hmil[] hmil { get; set; }
    }
    public class Hmil
    {
        public string usr { get; set; }
        public string pwd { get; set; }
        public string mob { get; set; }
        public string snd { get; set; }
        public string msg { get; set; }
        public string mtp { get; set; }
        public string pe { get; set; }
        public string tm { get; set; }
        public string id { get; set; }
    }
}
