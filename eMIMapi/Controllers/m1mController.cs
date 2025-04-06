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
using System.Globalization;

namespace eMIMapi.Controllers
{
    //sms types
    //premium sms   - 13
    //linktext sms  - 21
    //otp sms       - 33
    //campaign sms  - 47
    //INTERNATIONAL - 15
    //DUBAI         - 16
    //PRIORITY OTP  - 17 
    //PRIORITY TRAN - 18 
    //KSA           - 19
    //QUTAR         - 22

    //HYUNDAI OTP     - 25
    //DIRECT OTP FDA  - 26

    //UAE WHITE PANEL TRANS- 20
    //UAE WHITE PANEL PROMO- 30  

    //ksa WHITE PANEL TRANS- 40
    //ksa WHITE PANEL PROMO- 50  

    [RoutePrefix("api/mim")]
    public class m1mController : ApiController
    {
        bool bSecure = ConfigurationManager.AppSettings["SECURITY"].ToString() == "Y" ? true : false;

        public bool HomeCR = false;   //   true;    //
        public bool WhitePanel = false;   //    true;    // 
        public int expMin = 60;
        public string sms_provider = "";
        public string sms_ip = "";
        public int sms_port = 0;
        public string sms_acid = "";
        public string sms_systemid = "";
        public string sms_password = "";


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

        //JIO PROMO  for 103.205.64.64
        public string sms_provider_PROMO2 = "JIO";
        public string sms_ip_PROMO2 = "185.255.8.176";
        public int sms_port_PROMO2 = 8888;
        public string sms_acid_PROMO2 = "2407";
        public string sms_systemid_PROMO2 = "MyinboxXPROMO"; // "Promotestdlr"; "Promotestdlr";
        public string sms_password_PROMO2 = "Shiva@1906";

        //PROMO  for AWS
        //public string sms_provider_PROMO2 = "AIRTEL_TELSP";
        //public string sms_ip_PROMO2 = "124.30.18.27";
        //public int sms_port_PROMO2 = 6161;
        //public string sms_acid_PROMO2 = "2409";
        //public string sms_systemid_PROMO2 = "myinboxpr"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO2 = "vKXh6YGy";

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

        public string sms_provider_FDAOTP = "AIRTEL_TELSP";
        public string sms_ip_FDAOTP = "103.188.78.11";
        public int sms_port_FDAOTP = 2776;
        public string sms_acid_FDAOTP = "2009";
        public string sms_systemid_FDAOTP = "myinbotp";
        public string sms_password_FDAOTP = "Myn12304";

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
        //========================================================================================================================================
        //========================================================================================================================================

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
        
            //124.30.18.27	6161	0	myinboxpr	vKXh6YGy
        public string sms_provider_PROMO = "AIRTEL_TELSP";
        public string sms_ip_PROMO = "124.30.18.27";
        public int sms_port_PROMO = 6161;
        public string sms_acid_PROMO = "2407";
        public string sms_systemid_PROMO = "myinboxpr"; // "Promotestdlr"; "Promotestdlr";
        public string sms_password_PROMO = "vKXh6YGy";

        //AIRTEL
        //public string sms_provider_PROMO = "AIRTEL";
        //public string sms_ip_PROMO = "125.19.17.115";
        //public int sms_port_PROMO = 2776;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "SKV_MIB_P3"; // "Promotestdlr"; "Promotestdlr";
        //public string sms_password_PROMO = "mib@1234";

        //------------PROMO ACCOUNT FOR NUMERIC SENDERID

        //INFOBIP
        //public string sms_provider_PROMO = "INFOBIP";
        //public string sms_ip_PROMO = "smpp3.infobip.com";
        //public int    sms_port_PROMO = 8888;
        //public string sms_acid_PROMO = "2007";
        //public string sms_systemid_PROMO = "Myinboxpromo5"; // "Promotestdlr";
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

        //UAE ACCOUNTS - -    msgtype = 16

        public string sms_provider_DUB = "ETISALAT";
        public string sms_ip_DUB = "86.96.241.54";
        public int sms_port_DUB = 2775;
        public string sms_acid_DUB = "1509";
        public string sms_systemid_DUB = "MiMDXB";
        public string sms_password_DUB = "Nzjocp6&";

        #region < white label panel API >

        //UAE ACCOUNTS WHITEPANEL - TRANS -    msgtype = 20
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

        //UAE ACCOUNTS WHITEPANEL - PROMO -    msgtype = 30
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

        #endregion

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

        //public string sms_provider_VCON = "INFOBIP";
        //public string sms_ip_VCON = "smpp3.infobip.com";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "8302";
        //public string sms_systemid_VCON = "Myinboxtrans16";
        //public string sms_password_VCON = "Shiva@1906";

        #endregion

        #region < JIO >
        //public string sms_provider_VCON = "JIO";
        //public string sms_ip_VCON = "185.255.8.176";
        //public int sms_port_VCON = 8888;
        //public string sms_acid_VCON = "8809";
        //public string sms_systemid_VCON = "MyinboxTRN4";
        //public string sms_password_VCON = "Shiva@1906";
        #endregion

        //HOME CREDIT
        public string sms_provider_HC = "INFOBIP";
        public string sms_ip_HC = "smpp3.infobip.com";
        public int sms_port_HC = 8888;
        public string sms_acid_HC = "8201";
        public string sms_systemid_HC = "Myinboxtrans14";
        public string sms_password_HC = "Shiva@1906";

        //API AIRTEL FOR =>  API TO SMPP
        public string sms_provider_API = "AIRTEL_TELSP";
        public string sms_ip_API = "124.30.18.27";
        public int sms_port_API = 6161;
        public string sms_acid_API = "8001";
        public string sms_systemid_API = "myinboxtrnew7";
        public string sms_password_API = "Ld5389KW";

        

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

        //public string sms_provider_OTP = "TELSP";
        //public string sms_ip_OTP = "smpp.telsp.io";
        //public int sms_port_OTP = 2776;
        //public string sms_acid_OTP = "1101";
        //public string sms_systemid_OTP = "myinboxotp2";
        //public string sms_password_OTP = "k3sote9v";



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


        //QATAR 2//
        public string sms_provider_QTR2 = "TOBEPRECISESMS";
        public string sms_ip_QTR2 = "smpp1.tobeprecisesms.com";
        public int sms_port_QTR2 = 2775;
        public string sms_acid_QTR2 = "1509";
        public string sms_systemid_QTR2 = "myinboxtrans";
        public string sms_password_QTR2 = "mynb6535";


        // FirstCry // 
        public string sms_acid_API_ForFirstCry = "2901";


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

        // Secured Encrypted N
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

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                //Shishir 07/08/2023 start
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);//For ApiKey
                    if (dt.Rows.Count <= 0) return "Incorrect Password";
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }
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

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion

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

                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
                        }
                    }
                }

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
        //                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
        //                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED 5308' ; " +
        //                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
        //                            " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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

        [Route("SendHybridSMS")]
        [HttpGet]
        public async Task<string> SendHybridSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
        {
            string apiurl = "https://lz6q85.api.infobip.com/ott/rcs/1/message";
            string acid = "myinbox";
            string authkey = "App dea4c1493b68dd1c6456557b488ddb0f-636a4a02-7b9c-4629-a812-092204fdb102";

            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

            Util ob = new Util();
            if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

            //validation of list count
            if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }
            string msgid = "";
            foreach (var m in mobList)
            {
                msgid = msgid + RCSApiAN(m.ToString(), authkey, msg, acid, apiurl, userid);
                msgid = msgid + ",";
            }
            return msgid;

        }

        public string RCSApiAN(string mob, string authkey, string msgtext, string acid, string apiurl, string pUserId)
        {
            string SessionId = "1";
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");

            var body = @"{
             ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n") + @""",
               ""suggestions"":[";
            body += @"],

                        ""type"": ""TEXT""
             }
           }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            RCSRoot res = new RCSRoot();
            string msgid = "";
            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '1','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    database.ExecuteNonQuery(sql + selectsql);
                    msgid = ms.messageId;
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '1','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                    //Info_Err(response.Content, 0);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return msgid;
        }

        #endregion

        public string CheckPromoTime(string account, string sender, List<string> mobList, int noofsms, double rate, string msg, bool ucs, string userid)
        {
            Util ob = new Util();
            if ((sender.ToUpper().StartsWith("AD-") || sender.ToUpper().EndsWith("-AD")) || (account == "INDIA"))
            {
                DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                if ((account == "ETISALAT" && CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25")) ||
                    (account == "KARIX" && CurrTime >= Convert.ToDateTime("08:35") && CurrTime <= Convert.ToDateTime("21:55")) ||
                    (account == "GUPSHUP" && CurrTime >= Convert.ToDateTime("10:30") && CurrTime <= Convert.ToDateTime("22:30")) ||
                    (account == "INDIA" && CurrTime >= Convert.ToDateTime("10:00") && CurrTime <= Convert.ToDateTime("21:00"))
                   )
                {
                    return "1";
                }
                else
                {
                    // process REJECTION ....
                    //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                    string errcd_ = "968";
                    string sql = "";
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
                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                    else
                        return "SMS Submitted Successfully. " + sRet;
                }
            }
            else
                return "1";
        }

        //1. Secured Encrypted
        //THIS METHOD IS THE MAIN METHOD FOR ALL OUTSIDE INDIA TRAFFIC ......... - WITH HYUNDAI TRAFFIC
        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string SubClientCode = "")
        {
            try
            {
                string FailOver = "";
                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47
                //INTERNATIONAL - 15
                //DUBAI         - 16
                //PRIORITY OTP  - 17 
                //PRIORITY TRAN - 18 
                //KSA           - 19
                //QUTAR         - 22

                //HYUNDAI OTP   - 25

                //uae white panel TRANS - 20
                //uae white panel PROMO - 30
                //KSA white panel TRANS - 40
                //KSA white panel PROMO - 50

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                mobile = mobile.Replace("+", "").Replace("-", "").Replace(" ", "");

                if (userid == "MIM2300165" || userid == "MIM2300167")
                {
                    if (pwd.ToUpper() == "FIRSTCRY321")
                    {
                        string msgid = GetMsgID();
                        string SubClienQry = "INSERT INTO MSGUAESubClientCode (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,msgidClient,SubClientCode) " +
                        " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + msgid + "','" + Convert.ToString(SubClientCode) + "')";
                        database.ExecuteNonQuery(SubClienQry);

                        string sql = "INSERT INTO MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,msgidClient) " +
                        " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + msgid + "')";
                        database.ExecuteNonQuery(sql);
                        return "SMS Submitted Successfully. Message ID: " + msgid;
                    }
                    else
                    {
                        return "Invalid Password";
                    }
                }
                if (msgtype == "16")
                {
                    if (userid.ToUpper() == "MIM2201196")
                    {
                        if (mobile.Length != 12)
                        {
                            int lnm = mobile.Length;
                            if (lnm >= 9)
                                mobile = "971" + mobile.Substring(mobile.Length - 9, 9);
                            else
                                mobile = "971" + mobile;
                        }
                    }
                    if (mobile.Trim().Length < 12)
                    {
                        return "Invalid Mobile Number";
                    }
                }

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                Util ob = new Util();

                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live with (nolock) where userid='" + userid + "'"));

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }
                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "22" || msgtype == "25" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "30" || msgtype == "40" || msgtype == "50")) return "Invalid Message Type";

                string peid = "";
                if (msgtype != "15" && msgtype != "16" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    peid = ob.getPEid(userid);

                if (msgtype != "15" && msgtype != "16" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    if (peid == "") return "Blocked by DLT";

                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "22" || msgtype == "30" || msgtype == "40" || msgtype == "50") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33" || msgtype == "25") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                    { return "Insufficient Balance"; }
                }
                #endregion
                //  if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //get account for messagetype and bind ----
                #region < MessageType check and account bind >
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
                else if (msgtype == "33" || msgtype == "25")
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
                    string retvl = CheckPromoTime(sms_provider_WP_UAE, sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                else if (msgtype == "30")
                {
                    sms_provider = sms_provider_WP_UAE_P;
                    sms_ip = sms_ip_WP_UAE_P;
                    sms_port = sms_port_WP_UAE_P;
                    sms_acid = sms_acid_WP_UAE_P;
                    sms_systemid = sms_systemid_WP_UAE_P;
                    sms_password = sms_password_WP_UAE_P;
                    string retvl = CheckPromoTime(sms_provider_WP_UAE_P, sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;

                                #region < added in Separate Method  - CheckPromoTime >
                                /*
                                if (sender.StartsWith("AD-") || sender.EndsWith("-AD"))
                                {
                                    DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                                    if (CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25"))
                                    {

                                    }
                                    else
                                    {
                                        // process REJECTION ....
                                        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                        string errcd_ = "991";
                                        string sql = "";
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
                                            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                        else
                                            return "SMS Submitted Successfully. " + sRet;
                                    }
                                }
                                */
                                #endregion
                            }
                        }
                    }

                    #region REJECTION20240219
                    string sRet = "";
                    string lastMsgID = "";
                    string m = mobile;
                    string nid = "";
                    int MobLen = Convert.ToInt32(m.Length);
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string errcd_ = "5308";
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
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
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;


                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
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

                if (userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2101450")
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

                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }


                if (msgtype == "47")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueueGSM(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                //Insert data in MSGQUEUEB4singleAPI where Usrid is blank at 11/04/2023
                else if (Convert.ToString(Usrid) == "")
                {
                    string msgid1 = "";
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33" || msgtype == "25") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;

                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", SubClientCode);
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
                else
                {
                    string templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    {

                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    else
                                        return "SMS Submitted Successfully. " + sRet;
                                }
                            }


                        }

                    }

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

                    string sTon = "0";
                    if (sms_provider.Contains("ETISALAT")) sTon = "5";

                    client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte(sTon), (AddressNPI)Convert.ToByte("0"));
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
                    sTon = "0";
                    if (sms_provider.Contains("ETISALAT")) sTon = "5";

                    var sourceAddress = new SmeAddress(sender, (AddressTON)byte.Parse(sTon), (AddressNPI)byte.Parse("0"));

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
                        }
                    }

                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();

                    //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                    //{ 

                    //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");


                    //deduct balance
                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);

                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);

                    if (msgtype == "16" && userid == "MIM2201270")
                    {
                        string msgid = "";
                        msgid = Convert.ToString(resp[0].MessageId);
                        if (Convert.ToString(FailOver) != "" && FailOver != null)
                        {
                            new Util().AddInFailOver(Convert.ToString(userid), mobile.ToString(), msg, msgid, Convert.ToString(resp[0].Header.Status), FailOver.ToString(), "");
                        }
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
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //2. Secured Encrypted
        //THIS METHOD IS THE MAIN METHOD FOR ALL OUTSIDE INDIA TRAFFIC ......... -
        [Route("SendSMSWithWABAFailOver")]
        [HttpGet]
        public async Task<string> SendSMSWithWABAFailOver(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string WABATemplateName, string WABAVariables, String FailOver = "")
        {
            try
            {

                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47
                //INTERNATIONAL - 15
                //DUBAI         - 16
                //PRIORITY OTP  - 17 
                //PRIORITY TRAN - 18 
                //KSA           - 19

                //uae white panel TRANS - 20
                //uae white panel PROMO - 30
                //KSA white panel TRANS - 40
                //KSA white panel PROMO - 50

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                mobile = mobile.Replace("+", "").Replace("-", "").Replace(" ", "");
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                Util ob = new Util();

                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }
                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }
                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "30" || msgtype == "40" || msgtype == "50")) return "Invalid Message Type";

                string peid = "";
                if (msgtype != "15" && msgtype != "16" && msgtype != "19" && msgtype != "20" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    peid = ob.getPEid(userid);

                if (msgtype != "15" && msgtype != "16" && msgtype != "19" && msgtype != "20" && msgtype != "30" && msgtype != "40" && msgtype != "50")
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

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "30" || msgtype == "40" || msgtype == "50") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                    { return "Insufficient Balance"; }
                }
                #endregion
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
                    string retvl = CheckPromoTime(sms_provider_WP_UAE, sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                else if (msgtype == "30")
                {
                    sms_provider = sms_provider_WP_UAE_P;
                    sms_ip = sms_ip_WP_UAE_P;
                    sms_port = sms_port_WP_UAE_P;
                    sms_acid = sms_acid_WP_UAE_P;
                    sms_systemid = sms_systemid_WP_UAE_P;
                    sms_password = sms_password_WP_UAE_P;
                    string retvl = CheckPromoTime(sms_provider_WP_UAE_P, sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
                                #region <Shifted to CheckPromoTime >
                                //if (sender.StartsWith("AD-") || sender.EndsWith("-AD"))
                                //{
                                //    DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                                //    if (CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25"))
                                //    {

                                //    }
                                //    else
                                //    {
                                //        // process REJECTION ....
                                //        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                //        string errcd_ = "991";
                                //        string sql = "";
                                //        string sRet = "";
                                //        string lastMsgID = "";
                                //        foreach (var m in mobList)
                                //        {
                                //            string nid = "";
                                //            for (int x = 0; x < noofsms; x++)
                                //            {
                                //                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
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
                                //        string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                //        sRet = sRet.Substring(0, sRet.Length - 2);
                                //        if (mobList.Count == 1)
                                //            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                //        else
                                //            return "SMS Submitted Successfully. " + sRet;
                                //    }
                                //}
                                #endregion
                            }
                        }
                    }

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
                if (userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2101450")
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
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }


                if (msgtype == "47")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueueGSM(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                //Insert data in MSGQUEUEB4singleAPI where Usrid is blank at 11/04/2023
                else if (Convert.ToString(Usrid) == "")
                {
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;
                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msgid1 = GetMsgID();
                        //string tablenm = "MSGQUEUEB4singleAPI";
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
                        //string sql = "insert into " + tablenm + " (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,FILEID,peid,templateid,smsrate,noofsms,datacode,msgidClient) " +
                        //" VALUES ('','" + sms_acid.Substring(0, 2) + "','" + userid + "',N'" + msg + "','" + m + "','" + sender + "','1','" + peid + "','','" + rate.ToString() + "','" + noofsms + "','" + (ucs ? "UCS2" : "Default") + "','" + msgid1 + "')";
                        //database.ExecuteNonQuery(sql);
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
                else
                {
                    string templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    {

                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    else
                                        return "SMS Submitted Successfully. " + sRet;
                                }
                            }


                        }

                    }

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
                        }
                    }

                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();

                    //if (resp[0].Header.Status == CommandStatus.ESME_ROK)
                    //{ 

                    //ob.AddInMsgSubmitted(userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid,peid,"");


                    //deduct balance
                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);

                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);

                    if (msgtype == "16" && userid == "MIM2201270")
                    {
                        string msgid = "";
                        msgid = Convert.ToString(resp[0].MessageId);
                        if (Convert.ToString(FailOver) != "" && FailOver != null)
                        {
                            new Util().AddInFailOver(Convert.ToString(userid), mobile.ToString(), msg, msgid, Convert.ToString(resp[0].Header.Status), FailOver.ToString(), Convert.ToString(WABATemplateName), Convert.ToString(WABAVariables));
                        }
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
                }


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 3. Secured Encrypted
        //with PEID   -- SMPP -
        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string SubClientCode = "")
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

                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (userid.ToUpper() == "MIM2201078")
                { if (mobList.Count > 500) { return "Mobile numbers cannot be more than 500"; } }
                else { if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; } }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

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

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }

                }
                #endregion
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

                    if (userid == "MIM2201143")
                    {
                        sms_provider = sms_provider_OTP3;
                        sms_ip = sms_ip_OTP3;
                        sms_port = sms_port_OTP3;
                        sms_acid = sms_acid_OTP3;
                        sms_systemid = sms_systemid_OTP3;
                        sms_password = sms_password_OTP3;
                    }

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

                if (userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101277" || userid == "MIM2101450" || userid == "MIM2101650"
                    || userid == "MIM2102201" || sender.ToUpper() == "HMISVR")
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
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }

                //if (msgtype == "33")
                //{
                //    string msgid = "";
                //    string ss = "";
                //    foreach (var m in mobList)
                //    {
                //        msgid = GetMsgID();
                //        ob.AddInMsgQueueOTP(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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

                if (userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101450"
                    || (msgtype == "33" && userid != "MIM2201143" && Convert.ToString(Usrid) == "") || (WhitePanel && msgtype == "13"))
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
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
                else if (userid == "MIM2101650")
                {
                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        //msgid1 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueue1(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "",SubClientCode);
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
                else if (userid == "MIM2102201" || userid == "MIM2201009" || userid == "MIM2101277")
                {
                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        //msgid1 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueue3(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                else if (sender.ToUpper() == "HMISVR")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        // msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                else if (msgtype == "47")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueueGSM(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                //Insert data in MSGQUEUEB4singleAPI where Usrid is blank
                else if (Convert.ToString(Usrid) == "")
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
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", SubClientCode);
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
                else
                {
                    #region < Commented >

                    string templateid = "";

                    // rabi for template DLT block 02/11/2022
                    string errcd_ = "5308";
                    if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                    " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
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
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                                string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                if (mobList.Count == 1)
                                    return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                else
                                    return "SMS Submitted Successfully. " + sRet;
                            }
                        }
                    }


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


        // 3. Secured Encrypted
        //with PEID   -- SMPP -
        [Route("SendSMSB")]
        [HttpGet]
        public async Task<string> SendSMSB(string userid, string pwd, string mobile, string sender, string msg, string peid, string SubClientCode = "")
        {
            try
            {
                string msgtype = "";

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
                if (peid == null) return "Invalid PE-ID";

                if (userid == "MIM2400063" || userid == "MIM2400064") msgtype = "13";
                if (userid == "MIM2400065") msgtype = "33";

                if (msgtype == "") return "Invalid User ID";

                mobile = mobile.Trim().Replace("+", "");

                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (userid.ToUpper() == "MIM2201078")
                { if (mobList.Count > 500) { return "Mobile numbers cannot be more than 500"; } }
                else { if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; } }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

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

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
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

                    if (userid == "MIM2201143")
                    {
                        sms_provider = sms_provider_OTP3;
                        sms_ip = sms_ip_OTP3;
                        sms_port = sms_port_OTP3;
                        sms_acid = sms_acid_OTP3;
                        sms_systemid = sms_systemid_OTP3;
                        sms_password = sms_password_OTP3;
                    }

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

                if (userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101277" || userid == "MIM2101450" || userid == "MIM2101650"
                    || userid == "MIM2102201" || sender.ToUpper() == "HMISVR")
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
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }

                //if (msgtype == "33")
                //{
                //    string msgid = "";
                //    string ss = "";
                //    foreach (var m in mobList)
                //    {
                //        msgid = GetMsgID();
                //        ob.AddInMsgQueueOTP(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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

                if (userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101450"
                    || (msgtype == "33" && userid != "MIM2201143" && Convert.ToString(Usrid) == "") || (WhitePanel && msgtype == "13"))
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
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
                else if (userid == "MIM2101650")
                {
                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        //msgid1 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueue1(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                else if (userid == "MIM2102201" || userid == "MIM2201009" || userid == "MIM2101277")
                {
                    string msgid1 = "";
                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        //msgid1 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueue3(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                else if (sender.ToUpper() == "HMISVR")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        // msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                else if (msgtype == "47")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueueGSM(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                //Insert data in MSGQUEUEB4singleAPI where Usrid is blank
                else if (Convert.ToString(Usrid) == "")
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
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", SubClientCode);
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
                else
                {
                    #region < Commented >

                    string templateid = "";

                    // rabi for template DLT block 02/11/2022
                    string errcd_ = "5308";
                    if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                    " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                    " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                    " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                    " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
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
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                                string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                if (mobList.Count == 1)
                                    return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                else
                                    return "SMS Submitted Successfully. " + sRet;
                            }
                        }
                    }


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

        // 4. Secured Encrypted
        //with PEID   -- SMPP -
        [Route("SendSMSMessage")]
        [HttpGet]
        public async Task<HttpResponseMessage> SendSMSMessage(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string SubClientCode = "")
        {
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            DataTable dtRes = new DataTable("dtRes");
            dtRes.Columns.Add("Mobile");
            dtRes.Columns.Add("MessageID");

            DataTable dtRes2 = new DataTable("dtRes2");
            dtRes2.Columns.Add("Response");

            try
            {

                //validations

                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) { yourJson = "Invalid User ID"; ret = true; }
                if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
                if (mobile == null) { yourJson = "Invalid Mobile Number"; ret = true; }
                if (mobile == null) { yourJson = "Invalid Mobile Number"; ret = true; }
                if (sender == null) { yourJson = "Invalid Sender ID"; ret = true; }
                if (msg == null) { yourJson = "Invalid Message Text"; ret = true; }
                if (msgtype == null) { yourJson = "Invalid Message Type"; ret = true; }
                if (peid == null) { yourJson = "Invalid PE-ID"; ret = true; }
                mobile = mobile.Trim().Replace("+", "");

                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { yourJson = "Invalid Mobile Number"; ret = true; }


                //validation of list count
                if (userid.ToUpper() == "MIM2201078")
                { if (mobList.Count > 500) { yourJson = "Mobile numbers cannot be more than 500"; ret = true; } }
                else { if (mobList.Count > 30) { yourJson = "Mobile numbers cannot be more than 30"; ret = true; } }
                if (!ret)
                {
                    ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");

                    DataTable dt = new DataTable();
                    if (bSecure)
                    {
                        dt = ob.GetUserParameterSecure(userid, pwd);
                        if (dt.Rows.Count <= 0) { yourJson = "Invalid Credentials"; ret = true; }
                    }
                    else
                    {
                        dt = ob.GetUserParameter(userid);
                        if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }
                        if (pwd != dt.Rows[0]["APIKEY"].ToString()) { yourJson = "Incorrect Password"; ret = true; }
                    }

                    if (msgtype != "15" && msgtype != "16")
                        if (mobile.Length < 10) { yourJson = "Invalid Mobile Number."; ret = true; }
                    //double Num;
                    //bool isNum = double.TryParse(mobile, out Num);
                    //if (!isNum) return "Invalid Mobile Number.";

                    //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                    if (msg.Trim() == "") { yourJson = "Invalid Message Text"; ret = true; }

                    if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18"))
                    { yourJson = "Invalid Message Type"; ret = true; }
                    //check balance
                    double rate = 0;
                    int noofsms = GetMsgCount(msg.Trim());
                    bool ucs = false;
                    if (msg.Trim().Any(c => c > 126)) ucs = true;

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
                    //check valid sender id
                    if (!ob.CheckSenderId(userid, sender)) { yourJson = "Invalid Sender ID"; ret = true; }

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

                        if (userid == "MIM2201143")
                        {
                            sms_provider = sms_provider_OTP3;
                            sms_ip = sms_ip_OTP3;
                            sms_port = sms_port_OTP3;
                            sms_acid = sms_acid_OTP3;
                            sms_systemid = sms_systemid_OTP3;
                            sms_password = sms_password_OTP3;
                        }

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

                    bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                    if (isNumeric)
                    {
                        sms_provider = sms_provider_PROMO;
                        sms_ip = sms_ip_PROMO;
                        sms_port = sms_port_PROMO;
                        sms_acid = sms_acid_PROMO;
                        sms_systemid = sms_systemid_PROMO;
                        sms_password = sms_password_PROMO;
                        string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                        if (retvl != "1") { yourJson = retvl; ret = true; }
                    }

                    if (!ret)
                    {
                        #region < Commented >

                        string templateid = "";

                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                string lastMob = "";
                                foreach (var m in mobList)
                                {
                                    string nid = "";
                                    for (int x = 0; x < noofsms; x++)
                                    {
                                        string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                        nid = Guid.NewGuid().ToString();
                                        sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                        " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                        " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                        " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                        " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                        " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                        "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd_ + " text:' AS DLVRTEXT," +
                                        " '" + nid + "', GETDATE(), 'Rejected','" + errcd_ + "',getdate() ; ";
                                        database.ExecuteNonQuery(sql);
                                    }
                                    sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                    lastMsgID = nid;
                                    lastMob = m;
                                }
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                if (mobList.Count == 1)
                                { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                else
                                { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
                                if (userid == "MIM2301076")
                                {
                                    DataRow dr1 = dtRes.NewRow();
                                    dr1["Mobile"] = lastMob;
                                    dr1["MessageID"] = lastMsgID;
                                    dtRes.Rows.Add(dr1);
                                    ret = true;
                                }
                            }
                            if (templID != "")
                                templateid = templID;
                        }
                        else
                        {
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
                                            " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                            " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd_ + "' ; " +
                                            " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                            " select '1' as id,'" + sms_provider + "','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
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
                                    string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                    if (mobList.Count == 1)
                                    { yourJson = "SMS Submitted Successfully. Message ID: " + lastMsgID; ret = true; }
                                    else
                                    { yourJson = "SMS Submitted Successfully. " + sRet; ret = true; }
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

                            //if (mobList.Count == 1)
                            //    //return "SMS Submitted Successfully. Message ID: " + Convert.ToString(resp[0].MessageId);
                            //else
                            //{
                            //    string ss = "";
                            //    for (int i = 0; i < resp.Count; i++)
                            //    {
                            //        ss += "MobileNo: " + resp[i].Request.DestinationAddress.Address.ToString() + " Message ID: " + resp[i].MessageId.ToString() + ", ";
                            //    }
                            //    ss = ss.Substring(0, ss.Length - 2);
                            //    //return "SMS Submitted Successfully. " + ss;
                            //}

                            DataRow dr1 = dtRes.NewRow();
                            dr1["Mobile"] = resp[0].Request.DestinationAddress.Address;
                            dr1["MessageID"] = resp[0].MessageId;
                            dtRes.Rows.Add(dr1);

                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                yourJson = ex.Message; ret = true;
            }
            if (dtRes.Rows.Count > 0)
            {
                yourJson = JsonConvert.SerializeObject(dtRes);
                try
                { if (dtRes2.Rows.Count > 0) yourJson = JsonConvert.SerializeObject(dtRes2); }
                catch (Exception e4) { }
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

        //with PEID & TemplateID  for FirstCryOnly  not in use
        [Route("SendSMSFc")]
        [HttpGet]
        public string SendSMSFc(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid)
        {
            try
            {
                if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                {
                    if (pwd.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }

                    string msgid = GetMsgID();

                    string sql = "insert into MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,peid,templateid,msgidClient) " +
                    " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + peid + "','" + templateid + "','" + msgid + "')";
                    database.ExecuteNonQuery(sql);

                    return "SMS Submitted Successfully. Message ID: " + msgid;
                }
                else return "Invalid User ID";
            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }

        // 5. Secured Encrypted
        //with PEID & TemplateID  --------------------------------------------   WITH HYUNDAI TRAFFIC
        [Route("SendSMS")]
        [HttpGet]
        public async Task<string> SendSMS(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string FailOver = "", string SubClientCode = "")
        {
            try
            {
                //validations
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;
                Util ob = new Util();
                if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                {
                    if (pwd.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }
                    sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (userid == "MIM2201009") sms_acid = sms_acid_PROMO2;
                    if (userid == "MIM2201194") sms_acid = sms_acid_API_ForFirstCry;

                    string msgid = GetMsgID();
                    //ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid, msgid);

                    if (userid == "MIM2201009")
                        ob.AddInMsgQueue3(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    //else if (userid == "MIM2201194")
                    //    ob.AddInMsgQueue(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    else
                        ob.AddInMsgQueue(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);

                    return "SMS Submitted Successfully. Message ID: " + msgid;
                }

                if (userid == null) return "Invalid User ID";
                if (mobile == null) return "Invalid Mobile Number";
                if (pwd == null) return "Invalid Password";
                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));
                mobile = mobile.Trim().Replace("+", "").Replace(" ","");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();


                if (userid.ToUpper() != "MIM2201011")            // SKIP VALIDATION 01/10/2022
                {

                    if (sender == null) return "Invalid Sender ID";
                    if (msg == null) return "Invalid Message Text";
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
                    ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                }

                //Util ob = new Util();
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

                if (userid.ToUpper() != "MIM2201011")                            // SKIP VALIDATION 01/10/2022   
                {
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    //if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                    //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                    if (msgtype != "15" && msgtype != "16")
                        if (mobile.Length < 10) return "Invalid Mobile Number.";
                    //double Num;
                    //bool isNum = double.TryParse(mobile, out Num);
                    //if (!isNum) return "Invalid Mobile Number.";

                    //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                    if (msg.Trim() == "") return "Invalid Message Text";
                }

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "25" || msgtype == "26" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;


                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33" || msgtype == "25" || msgtype == "26") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    {
                        return "Insufficient Balance";
                    }
                }
                #endregion
                //check valid sender id
                if (!(userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
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
                else if (msgtype == "33" || msgtype == "25")
                {
                    sms_provider = sms_provider_OTP;
                    sms_ip = sms_ip_OTP;
                    sms_port = sms_port_OTP;
                    sms_acid = sms_acid_OTP;
                    sms_systemid = sms_systemid_OTP;
                    sms_password = sms_password_OTP;

                    if (userid == "MIM2201143" || sender.ToUpper() == "HMISVR")
                    {
                        sms_provider = sms_provider_OTP3;
                        sms_ip = sms_ip_OTP3;
                        sms_port = sms_port_OTP3;
                        sms_acid = sms_acid_OTP3;
                        sms_systemid = sms_systemid_OTP3;
                        sms_password = sms_password_OTP3;
                    }

                }
                else if (msgtype == "26")
                {
                    sms_provider = sms_provider_FDAOTP;
                    sms_ip = sms_ip_FDAOTP;
                    sms_port = sms_port_FDAOTP;
                    sms_acid = sms_acid_FDAOTP;
                    sms_systemid = sms_systemid_FDAOTP;
                    sms_password = sms_password_FDAOTP;
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
                if (userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101277" || userid == "MIM2101450" || userid == "MIM2101650"
                    || userid == "MIM2102201" || (sender.ToUpper() == "HMISVR" && msgtype == "13"))
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
                    isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                //if (msgtype == "33")
                //{
                //    string msgid = "";
                //    string ss = "";
                //    foreach (var m in mobList)
                //    {
                //        msgid = GetMsgID();
                //        ob.AddInMsgQueueOTP(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                //if (userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101450" || userid == "MIM2101277" 
                //|| (msgtype == "33" && userid != "MIM2201143" && sender.ToUpper() != "HMISVR") || (WhitePanel && msgtype == "13"))

                if ((userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2002035" )
                   || ((msgtype == "33" || msgtype == "25" || msgtype == "26") && Convert.ToString(Usrid) == "")
                || (WhitePanel && msgtype == "13"))
                {
                    //if (userid != "MIM2101643")
                    //{
                    if (WhitePanel)
                    {
                        if (isNumeric)
                            sms_acid = "2001";
                        else if (msgtype == "13") sms_acid = "1601";
                        else if (msgtype == "33") sms_acid = "1101";
                        else if (msgtype == "25") sms_acid = "1101";
                    }
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                        ob.AddInMsgQueue3(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                if (sender.ToUpper() == "HMISVR" && msgtype == "13")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, SubClientCode);
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


                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (FailOver != "")
                {
                    ob.AddInFailOver(userid, mobile, msg.Replace("'", "''"), Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), FailOver, "");
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

        // 6. Secured Encrypted
        //with PEID & TemplateID  -
        [Route("SendSMSmsg")]
        [HttpPost]
        public async Task<string> SendSMSmsg(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string FailOver = "", string SubClientCode = "")
        {
            try
            {
                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                //if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                //{
                //    if (pwd.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }

                //    string msgid = GetMsgID();
                //    string sql = "insert into MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,peid,templateid,msgidClient) " +
                //     " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + peid + "','" + templateid + "','" + msgid + "')";
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

                if (msgtype == "16")
                {
                    if (userid.ToUpper() == "MIM2201196")
                    {
                        if (mobile.Length != 12)
                        {
                            int lnm = mobile.Length;
                            if (lnm >= 9)
                                mobile = "971" + mobile.Substring(mobile.Length - 9, 9);
                            else
                                mobile = "971" + mobile;
                        }
                    }
                    if (mobile.Trim().Length < 12)
                    {
                        return "Invalid Mobile Number";
                    }
                }

                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;
                Util ob = new Util();
                if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                {
                    if (pwd.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }
                    sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (userid == "MIM2201009") sms_acid = sms_acid_PROMO2;
                    if (userid == "MIM2201194") sms_acid = sms_acid_API_ForFirstCry;

                    string msgid = GetMsgID();
                    //ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid, msgid);

                    if (userid == "MIM2201009")
                        ob.AddInMsgQueue3(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    //else if (userid == "MIM2201194")
                    //    ob.AddInMsgQueue(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);
                    else
                        ob.AddInMsgQueue(userid, sender, mobile, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, 9.0, noofsms, ucs, templateid);

                    return "SMS Submitted Successfully. Message ID: " + msgid;
                }

                if (userid == null) return "Invalid User ID";
                if (mobile == null) return "Invalid Mobile Number";
                if (pwd == null) return "Invalid Password";

                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live where userid='" + userid + "'"));

                if (userid.ToUpper() != "MIM2201011")            // SKIP VALIDATION 01/10/2022
                {

                    if (sender == null) return "Invalid Sender ID";
                    if (msg == null) return "Invalid Message Text";
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
                    ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                }

                //Util ob = new Util();
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

                if (userid.ToUpper() != "MIM2201011")                            // SKIP VALIDATION 01/10/2022   
                {
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    //if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";

                    //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                    if (msgtype != "15" && msgtype != "16")
                        if (mobile.Length < 10) return "Invalid Mobile Number.";
                    //double Num;
                    //bool isNum = double.TryParse(mobile, out Num);
                    //if (!isNum) return "Invalid Mobile Number.";

                    //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                    if (msg.Trim() == "") return "Invalid Message Text";
                }

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;


                if (msgtype == "16" || msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                //check valid sender id
                if (!(userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011"))
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
                                #region < Shifted to CheckPromoTime >
                                //if (sender.StartsWith("AD-") || sender.EndsWith("-AD"))
                                //{
                                //    DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                                //    if (CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25"))
                                //    {

                                //    }
                                //    else
                                //    {
                                //        // process REJECTION ....
                                //        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                //        string errcd1_ = "991";
                                //        string sql = "";
                                //        string sRet = "";
                                //        string lastMsgID = "";
                                //        foreach (var m in mobList)
                                //        {
                                //            string nid = "";
                                //            for (int x = 0; x < noofsms; x++)
                                //            {
                                //                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                //                nid = Guid.NewGuid().ToString();
                                //                sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                //                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                //                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcd1_ + "' ; " +
                                //                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                //                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                //                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTD err:" + errcd1_ + " text:' AS DLVRTEXT," +
                                //                " '" + nid + "', GETDATE(), 'Rejected','" + errcd1_ + "',getdate() ; ";
                                //                database.ExecuteNonQuery(sql);
                                //            }
                                //            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                                //            lastMsgID = nid;
                                //        }
                                //        string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                //        sRet = sRet.Substring(0, sRet.Length - 2);
                                //        if (mobList.Count == 1)
                                //            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                //        else
                                //            return "SMS Submitted Successfully. " + sRet;
                                //    }
                                //}
                                #endregion
                            }
                        }
                    }

                    #region REJECTION20240219
                    string sRet = "";
                    string lastMsgID = "";
                    string m = mobile;
                    string nid = "";
                    int MobLen = Convert.ToInt32(m.Length);
                    string errcde_ = "5308";
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcde_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTED err:" + errcde_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcde_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;

                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
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
                if (userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201078" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101277" || userid == "MIM2101450" || userid == "MIM2101650"
                    || userid == "MIM2102201" || sender.ToUpper() == "HMISVR")
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
                    isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                //if (msgtype == "33")
                //{
                //    string msgid = "";
                //    string ss = "";
                //    foreach (var m in mobList)
                //    {
                //        msgid = GetMsgID();
                //        ob.AddInMsgQueueOTP(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                if (userid == "MIM2101228" || userid == "MIM2201354" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2002035" || userid == "MIM2101450" || userid == "MIM2101277" || (msgtype == "33" && userid != "MIM2201143") || (WhitePanel && msgtype == "13"))
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
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                        ob.AddInMsgQueue3(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                if (sender.ToUpper() == "HMISVR")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid,SubClientCode);
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


                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                //deduct balance
                string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                if (FailOver != "")
                {
                    ob.AddInFailOver(userid, mobile, msg.Replace("'", "''"), Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), FailOver, "");
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

        // 7. Secured Encrypted
        //with PEID   -- SMPP
        [Route("SendSMSUNI")]
        [HttpGet]
        public async Task<string> SendSMSUNI(string userid, string pwd, string mobile, string sender, string msg, string msgtype)
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
                // ILD - 24

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

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");
                //Util ob = new Util();
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "24")) return "Invalid Message Type";

                string peid = "";
                if (msgtype != "15" && msgtype != "16" && msgtype != "24")
                    peid = ob.getPEid(userid);

                if (msgtype != "15" && msgtype != "16" && msgtype != "24")
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

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "24") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);
                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    //if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms))
                    { return "Insufficient Balance"; }
                }
                #endregion
                //  if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                //check valid sender id
                // if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

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
                    if (userid == "MIM2200674" || userid == "MIM2200714" || userid == "MIM2200704" || userid == "MIM2200731" || userid == "MIM2200768" || userid == "MIM2200780" || userid == "MIM2200781")
                    {
                        sms_provider = sms_provider_DUB_2;
                        sms_ip = sms_ip_DUB_2;
                        sms_port = sms_port_DUB_2;
                        sms_acid = sms_acid_DUB_2;
                        sms_systemid = sms_systemid_DUB_2;
                        sms_password = sms_password_DUB_2;
                    }
                }
                else if (msgtype == "24")
                {
                    sms_provider = sms_provider_UIN;
                    sms_ip = sms_ip_UIN;
                    sms_port = sms_port_UIN;
                    sms_acid = sms_acid_UIN;
                    sms_systemid = sms_systemid_UIN;
                    sms_password = sms_password_UIN;
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

                //bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                //if (isNumeric)
                //{
                //    sms_provider = sms_provider_PROMO;
                //    sms_ip = sms_ip_PROMO;
                //    sms_port = sms_port_PROMO;
                //    sms_acid = sms_acid_PROMO;
                //    sms_systemid = sms_systemid_PROMO;
                //    sms_password = sms_password_PROMO;
                //}



                string templateid = "";
                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (msgtype != "15" && msgtype != "16" && msgtype != "24")
                {
                    if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                if (mobList.Count == 1)
                                    return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                else
                                    return "SMS Submitted Successfully. " + sRet;
                            }
                        }
                    }
                }

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

        // 8. Secured Encrypted
        // FOR HONDA   ========>  
        //with PEID AND SMSKEY    -- SMPP
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid="")
        {
            try
            {
                //return "succes";
                //validations

                //sms types
                //DUBAI         - 16  --Added by Naved
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                //if (peid == null) return "Invalid PE-ID";
                string SubClientCode = "";
                //mobile = mobile.Trim().Replace("+", "");
                mobile = mobile.Replace("+", "").Replace("-", "").Replace(" ", "");


                if (msgtype == "16")
                {
                    if (userid.ToUpper() == "MIM2201196")
                    {
                        if (mobile.Length != 12)
                        {
                            int lnm = mobile.Length;
                            if (lnm >= 9)
                                mobile = "971" + mobile.Substring(mobile.Length - 9, 9);
                            else
                                mobile = "971" + mobile;
                        }
                    }
                    if (mobile.Trim().Length < 12)
                    {
                        return "Invalid Mobile Number";
                    }
                }

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }
                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live with (nolock) where userid='" + userid + "'"));
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

                if (msgtype != "15" && msgtype != "16")
                    if (mobile.Length < 10) return "Invalid Mobile Number.";

                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";
                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
                            }
                        }
                    }

                    #region REJECTION20240219
                    string sRet = "";
                    string lastMsgID = "";
                    string m = mobile;
                    string nid = "";
                    int MobLen = Convert.ToInt32(m.Length);
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string errcode_ = "5308";
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcode_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTED err:" + errcode_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcode_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;


                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
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

                

                if (userid == "MIM2101450" || sender == "HMISVR" || sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                //bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                if (sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "");
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

                #region <Commented >

                if (Convert.ToString(Usrid) == "")
                {
                    string msgid1 = "";
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;

                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msgid1 = GetMsgID();
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", SubClientCode);
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
                else
                {

                    string templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    {
                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                                    sRet = sRet.Substring(0, sRet.Length - 2);
                                    if (mobList.Count == 1)
                                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    else
                                        return "SMS Submitted Successfully. " + sRet;
                                }
                            }
                        }
                    }



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
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 9. Secured Encrypted
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

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                if (templateid == "1407165994961713123" || templateid == "1407165994973390803" || templateid == "1407165994983355180")
                    msg = msg.Trim() + " HONDA";
                if (templateid == "1407165582774630555" || templateid == "1407165582788056635")
                    msg = msg.Trim() + " Honda Authorized Dealership.";
                if (templateid == "1107167454081595585")
                    msg = msg.Trim() + " -Honda Cars";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
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
                if (userid == "MIM2101450" || sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
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
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }

                if (sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
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
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
                        }
                    }
                }

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

        // 10. Secured Encrypted
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
                //if (peid == null) return "Invalid PE-ID";
                //if (templateid == null) return "Invalid Template ID";
                if (LongURL == null) return "Invalid Long URL";
                if (!LongURL.StartsWith("http://") && !LongURL.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";

                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;

                string msg1 = msg.Trim() + " " + domain + "12345678";
                if (templateid == "1407165994961713123" || templateid == "1407165994973390803" || templateid == "1407165994983355180")
                    msg1 = msg1.Trim() + " HONDA";
                if (templateid == "1407165582774630555" || templateid == "1407165582788056635")
                    msg1 = msg1.Trim() + " Honda Authorized Dealership.";
                int noofsms = GetMsgCount(msg1);

                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

                if (sender.ToUpper() != "HMISVR")
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
                                    sRet = sRet.Substring(0, sRet.Length - 2);
                                    if (mobList.Count == 1)
                                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    else
                                        return "SMS Submitted Successfully. " + sRet;
                                }
                            }
                        }
                    }
                }
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
                if (userid == "MIM2101450" || sender == "HMISVR" || sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
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
                //string segment = ob.GetShortURLofLongURL(userid, LongURL);
                //if (segment == "")
                //{
                string sUrl = ob.NewShortURLfromSQLHondaHmi(domain);
                ob.SaveShortURL(userid, LongURL, "", sUrl, "N", "Y", domain);
                lblShortURL = domain + sUrl;
                //}
                //else
                //    lblShortURL = segment;

                if (msg.Contains(LongURL))
                {
                    msg = msg.Trim().Replace(LongURL, lblShortURL);
                }
                else
                {
                    //URL Shortning .....
                    msg = msg.Trim() + " " + lblShortURL;
                }
                if (templateid == "1407165994961713123" || templateid == "1407165994973390803" || templateid == "1407165994983355180")
                    msg = msg.Trim() + " HONDA";
                if (templateid == "1407165582774630555" || templateid == "1407165582788056635")
                    msg = msg.Trim() + " Honda Authorized Dealership.";

                // SAVE SMS IN QUEUE
                if (sender.ToUpper() == "HMISVR")
                {
                    string msgid2 = "";
                    string ss2 = "";
                    foreach (var m in mobList)
                    {
                        //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid2 = GetMsgID();
                        ob.AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs);
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

                if (sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
                else if (peid != null && templateid != null)
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

        //Secured Encrypted
        //with PEID & TemplateID AND APIKEY & LongURL &  activityid
        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL, string activityId)
        {
            try
            {
                string[] act_id = activityId.Split(' ');
                if (activityId.Trim() != "")
                {
                    if (msg.ToLower().Contains(LongURL.ToLower()))
                        msg = msg + "&activityId=" + activityId;
                }

                LongURL = string.Format("{0}&activityId={1}", LongURL, act_id[0]);

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

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

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
                if (templateid == "1407165994961713123" || templateid == "1407165994973390803" || templateid == "1407165994983355180")
                    msg1 = msg1.Trim() + " HONDA";
                if (templateid == "1407165582774630555" || templateid == "1407165582788056635")
                    msg1 = msg1.Trim() + " Honda Authorized Dealership.";
                if (templateid == "1107167038925119549" || templateid == "1107166927064169464" || templateid == "1107166927048238140" || templateid == "1107167453839709431"
                    || templateid == "1107167453789869023" || templateid == "1107167453785889172")
                    msg1 = msg1.Trim() + " - Honda Cars";
                if (templateid == "1107166987077225851" || templateid == "1107166961965896811" || templateid == "1107166961959766103" || templateid == "1107167508637797121"
                    || templateid == "1107167508632181681" || templateid == "1107167508626561218" || templateid == "1407170891829087934")
                    msg1 = msg1.Trim() + " T&C Apply* -Honda Cars";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";
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
                if (userid == "MIM2101450" || sender == "HMISVR" || sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
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
                //string segment = ob.GetShortURLofLongURL(userid, LongURL);
                //if (segment == "")
                //{
                string sUrl = ob.NewShortURLfromSQLHondaHmi(domain);
                ob.SaveShortURL(userid, LongURL, "", sUrl, "N", "Y", domain);
                lblShortURL = domain + sUrl;
                //}
                //else
                //    lblShortURL = segment;

                //URL Shortning .....
                if (msg.Contains(LongURL))
                    msg = msg.Trim().Replace(LongURL, lblShortURL);
                else
                    msg = msg.Trim() + " " + lblShortURL;

                if (templateid == "1407165994961713123" || templateid == "1407165994973390803" || templateid == "1407165994983355180")
                    msg = msg.Trim() + " HONDA";
                if (templateid == "1407165582774630555" || templateid == "1407165582788056635")
                    msg = msg.Trim() + " Honda Authorized Dealership.";
                if (templateid == "1107167038925119549" || templateid == "1107166927064169464" || templateid == "1107166927048238140" || templateid == "1107167453839709431"
                   || templateid == "1107167453789869023" || templateid == "1107167453785889172")
                    msg = msg.Trim() + " - Honda Cars";
                if (templateid == "1107166987077225851" || templateid == "1107166961965896811" || templateid == "1107166961959766103" || templateid == "1107167508637797121"
                    || templateid == "1107167508632181681" || templateid == "1107167508626561218" || templateid == "1407170891829087934")
                    msg = msg.Trim() + " T&C Apply* -Honda Cars";

                if (sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        //msgid = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                        msgid = GetMsgID();
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid);
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
        /* FOR HONDA   <========   */

        //Secured Encrypted
        //with PEID & TemplateID AND APIKEY & LongURL -  also for Outside India Traffic for link shorten...
        [Route("SendSMSKWithLink")]
        [HttpGet]
        public async Task<string> SendSMSKWithLink(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL, string existingURL, string subclientcode = "", string xyg = "")
        {
            try
            {
                //validations
                //sms types
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47
                //QUTAR  - 22

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
                if (existingURL.ToUpper() == "Y")
                {
                    if (!ob.CheckLongURL(userid, LongURL))
                    {
                        return "Long url does not exists";
                    }
                }
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }

                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live with (nolock) where userid='" + userid + "'"));
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

                //if (!(mobile.Length == 12 || mobile.Length == 10)) return "Invalid Mobile Number.";
                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "22" || msgtype == "30" || msgtype == "40" || msgtype == "50")) return "Invalid Message Type";

                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";

                //check valid TEMPLATE ID for senderid
                //if (!ob.CheckTemplateIdSenderId(userid, sender, templateid)) return "Invalid Template ID";

                string domain = dt.Rows[0]["domainname"].ToString();
                //check balance
                double rate = 0;
                string msg1 = msg.Trim(); // + " " + domain + "12345678";
                if (msg1.Contains(LongURL))
                    msg1 = msg1.Replace(LongURL, domain + "12345678");
                else
                    msg1 = msg1 + " " + domain + "12345678";
                int noofsms = GetMsgCount(msg1);
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18" || msgtype == "19" || msgtype == "20" || msgtype == "22" || msgtype == "30" || msgtype == "40" || msgtype == "50") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                // rabi for template DLT block 02/11/2022
                string errcd_ = "5308";
                if (msgtype != "16" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                {
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
                                sRet = sRet.Substring(0, sRet.Length - 2);
                                if (mobList.Count == 1)
                                    return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                else
                                    return "SMS Submitted Successfully. " + sRet;
                            }
                        }
                    }
                }
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
                            }

                            //------------Add Code by Naved..27-06-2023
                            else if (account == "ETISALAT")
                            {
                                sms_provider = sms_provider_DUB;
                                sms_ip = sms_ip_DUB;
                                sms_port = sms_port_DUB;
                                sms_acid = sms_acid_DUB;
                                sms_systemid = sms_systemid_DUB;
                                sms_password = sms_password_DUB;
                                string retvl = CheckPromoTime(account, sender, mobList, noofsms, rate, msg, ucs, userid);
                                if (retvl != "1") return retvl;
                                #region <Shifted to CheckPromoTime >
                                //if (sender.StartsWith("AD-") || sender.EndsWith("-AD"))
                                //{
                                //    DateTime CurrTime = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                                //    if (CurrTime >= Convert.ToDateTime("09:30") && CurrTime <= Convert.ToDateTime("22:25"))
                                //    {

                                //    }
                                //    else
                                //    {
                                //        // process REJECTION ....
                                //        //insert into NOTSUBMITTED, MSGSUBMITTED & DELIVERY as REJECTED 
                                //        errcd_ = "991";
                                //        string sql = "";
                                //        string sRet = "";
                                //        string lastMsgID = "";
                                //        foreach (var m in mobList)
                                //        {
                                //            string nid = "";
                                //            for (int x = 0; x < noofsms; x++)
                                //            {
                                //                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
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
                                //        string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                                //        sRet = sRet.Substring(0, sRet.Length - 2);
                                //        if (mobList.Count == 1)
                                //            return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                //        else
                                //            return "SMS Submitted Successfully. " + sRet;
                                //    }
                                //}
                                #endregion
                            }
                            //-------------End Code-----------
                        }
                    }
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


                //Added by Naved On 29/09/2023
                //if ((sender.ToUpper() == "HIIBPL" || userid == "MIM2201048" || userid == "MIM2201104" ||
                //    userid == "MIM2300228" || userid == "MIM2300229") && Usrid == "")
                //{
                //    string msgid2 = "";
                //    string ss2 = "";
                //    foreach (var m in mobList)
                //    {
                //        //msgid2 = "S" + DateTime.Now.ToString("yyMMddHHmmssfffffff") + (new Random().Next(10000, 99999)).ToString();
                //        msgid2 = GetMsgID();
                //        new Util().AddInMsgQueue2(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid2, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, templateid, subclientcode);
                //        ss2 += "MobileNo: " + m.ToString() + " Message ID: " + msgid2 + ", ";
                //    }
                //    if (mobList.Count == 1)
                //        return "SMS Submitted Successfully. Message ID: " + msgid2;
                //    else

                //        ss2 = ss2.Substring(0, ss2.Length - 2);
                //    return "SMS Submitted Successfully. Message ID: " + ss2;
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
                        SetMultipleShortURL(ref msg, LongURL, existingURL, ob, domain, dtMobTrackUrl, m);

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
                        return "SMS Submitted Successfully. Message ID: " + msgid1;
                    else
                    {
                        ss1 = ss1.Substring(0, ss1.Length - 2);
                        return "SMS Submitted Successfully. " + ss1;
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
                    else if (peid == "0" && templateid == "0")
                    {
                        foreach (var m in mobList)
                        {
                            msg = msg_orig;
                            SetMultipleShortURL(ref msg, LongURL, existingURL, ob, domain, dtMobTrackUrl, m);

                            mobcnt++;
                            var pduBuilder = SMS.ForSubmit()
                              .From(sourceAddress)
                              .To(m)
                              .Coding(coding)
                              .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                              .Text(msg);
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }
                    else
                    {
                        foreach (var m in mobList)
                        {
                            if (m.Length == 12 || m.Length == 10 || m.Length == 11)
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

                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);

                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid, dtMobTrackUrl);
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
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted
        //with PEID & TemplateID AND APIKEY & LongURL & ActivityID  for HONDA..
        [Route("SendSMSKWithLink")]
        [HttpGet]
        public async Task<string> SendSMSKWithLink(string userid, string pwd, string apikey, string mobile, string sender, string msg, string msgtype, string peid, string templateid, string LongURL, string existingURL, string activityId)
        {
            try
            {
                string[] act_id = activityId.Split(' ');
                if (activityId.Trim() != "")
                {
                    msg = msg + "&activityId=" + activityId;
                }

                LongURL = string.Format("{0}&activityId={1}", LongURL, act_id[0]);

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
                if (existingURL.ToUpper() == "Y")
                {
                    if (!ob.CheckLongURL(userid, LongURL))
                    {
                        return "Long url does not exists";
                    }
                }
                mobile = mobile.Trim().Replace("+", "");

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }


                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, templateid);
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

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

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                // if (Convert.ToDouble(dt.Rows[0]["balance"]) <= (rate * noofsms)) return "Insufficient Balance";

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
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
                        }
                    }
                }
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

                ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid, dtMobTrackUrl);
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



        public string GetMsgID()
        {
            return 'S' + Convert.ToString(Guid.NewGuid());
        }

        public int GetMsgCount(string msg)
        {
            //Texting.SmsHelpers ob = new Texting.SmsHelpers();
            //int sc = ob.CountSmsParts(msg);
            //return sc;

            smsCounter smsOB = new smsCounter();
            int scnt = smsOB.CalculateSmsCount(msg);
            return scnt;

            //smsCounter_CS sOb = new smsCounter_CS();
            //int sc = sOb.CalculateSmsCount(msg);
            //return sc;

            string hc = ConfigurationManager.AppSettings["HOMECR"].ToString();
            if (hc == "1") HomeCR = true;
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

        //Secured Encrypted
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
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
                }
                string s = ob.GetBalance(userid);
                return "Balance : " + s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted
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
                DataTable dt = new DataTable();
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }
                string s = ob.GetBalance(userid);
                return "Balance : " + s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted N
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
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecurePWD(userid, pwd);//For Pwd
                    if (dt.Rows.Count <= 0)
                    {
                        dt = ob.GetUserParameterSecure(userid, pwd);//for Apikey
                        if (dt.Rows.Count <= 0) return "Invalid Credentials";
                    }
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID.";
                    if (pwd != dt.Rows[0]["pwd"].ToString())
                    {
                        if (pwd != dt.Rows[0]["apikey"].ToString())
                            return "Incorrect Password";
                    }
                }
                DataTable dt1 = ob.GetDelivery(msgid);
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
                string SubClientCode = string.Empty;
                DataTable dt2 = ob.GetDeliveryWithSubClientCode(msgid);
                if (dt2.Rows.Count > 0)
                {
                    SubClientCode = dt2.Rows[0]["SubClientCode"].ToString();
                    resp = resp + ", SubClientCode : " + SubClientCode;
                }
                return resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[Route("GetDeliveryWithWABA")]
        //[HttpGet]
        //public string GetSmsDeliveryWithWABA(string userid, string pwd, string msgid)
        //{

        //    string resp = "";
        //    try
        //    {
        //        //validations
        //        //check user name and password
        //        string respwaba = "";
        //        if (userid == null)
        //            return "Invalid User ID";
        //        if (pwd == null)
        //            return "Invalid Password";
        //        if (msgid == null)
        //            return "Invalid Message ID";
        //        Util ob = new Util();
        //        DataTable dt = ob.GetUserParameter(userid);
        //        if (dt.Rows.Count <= 0)
        //            return "Invalid User ID.";
        //        if (pwd != dt.Rows[0]["pwd"].ToString())
        //            return "Incorrect Password";


        //        DataSet dt1 = ob.GetDeliveryWithWABA(msgid);
        //        if (dt1.Tables[0].Rows.Count <= 0)
        //            return "Invalid Message ID";
        //        if (dt1.Tables[0].Rows[0]["PROFILEID"].ToString().ToUpper() != userid.ToUpper())
        //            return "Invalid Message ID.";
        //        //string resp = "";
        //        if (dt1.Tables[0].Rows[0]["dlvrstat"].ToString() == "")
        //            resp = "SMS Delivery Status : Unknown";
        //        if (dt1.Tables[0].Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
        //        {
        //            resp = "SMS Delivery Status : Delivered";
        //            resp += ", SMS Delivery Time : " + Convert.ToDateTime(dt1.Tables[0].Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");

        //            //if (dt1.Tables.Count > 1)
        //            //{
        //            //    if (dt1.Tables[1].Rows.Count > 0)
        //            //    {
        //            //        foreach (DataRow dr in dt1.Tables[1].Rows)
        //            //        {
        //            //            if (respwaba.ToString() == "")
        //            //            {
        //            //                respwaba = dr["Status"].ToString();
        //            //            }
        //            //            else
        //            //            {
        //            //                respwaba = respwaba + "," + dr["Status"].ToString();
        //            //            }

        //            //        }
        //            //    }
        //            //}

        //            //if (respwaba != "")
        //            //{
        //            //    resp = "Delivery Status : Delivered" + " and Waba Status : " + respwaba;
        //            //}
        //            //else
        //            //{
        //            //    resp = "Delivery Status : Delivered" + " and Waba not Sent"; //Delivery Status : Delivered and Waba not Sent
        //            //}

        //            //resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Tables[0].Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss"); //Delivery Status : Delivered and Waba not Sent, Delivery Time : 23 - Jan - 2023 10:01:07
        //        }



        //        if (dt1.Tables[1].Rows.Count > 0)
        //        {
        //            if (dt1.Tables.Count > 1)
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
        //            }
        //            if (respwaba != "")
        //            {
        //                resp += " and Waba Status : " + respwaba;
        //            }
        //            else
        //            {
        //                resp += " and Waba not Sent"; //Delivery Status : Delivered and Waba not Sent
        //            }

        //            resp += ", Waba Delivery Time : " + Convert.ToDateTime(dt1.Tables[1].Rows[0]["DeliveryTime"]).ToString("dd-MMM-yyyy HH:mm:ss"); //Delivery Status : Delivered and Waba not Sent, Delivery Time : 23 - Jan - 2023 10:01:07
        //        }
        //        else
        //            resp = "Delivery Status : Failed. Error Code : " + dt1.Tables[0].Rows[0]["errcd"].ToString();
        //        //return resp;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return resp;
        //}



        [Route("GetDeliveryWithWABA")]
        [HttpGet]
        public HttpResponseMessage GetWabaDeliveryWithWABA(string userid, string pwd, string msgid)
        {

            string WABADBO = ConfigurationManager.AppSettings["WABADBO"].ToString();
            string SMPPMAINDBO = ConfigurationManager.AppSettings["SMPPMAINDBO"].ToString();

            string resp = GetDelivery(userid, pwd, msgid);
            string WabaMSGID = Convert.ToString(database.GetScalarValue(@"select wabamsgid from SMSFailOverWABAsentResponse with(nolock) where MSGID='" + msgid + "'"));
            string WabaProfileId = Convert.ToString(database.GetScalarValue(@"select profileid from " + WABADBO + "tblWABASubmitted with(nolock) where messageId='" + WabaMSGID + "'"));
            string ApiKey = Convert.ToString(database.GetScalarValue(@"select APIKEY from " + WABADBO + "customer with(nolock) where username='" + WabaProfileId + "'"));


            string MsgID = WabaMSGID;
            string ProfileId = WabaProfileId;
            string APIKey = ApiKey;

            string deliverydetail = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            DataTable dt1 = new DataTable("dt");
            dt1.Columns.Add("WABA Response");


            string sql = "select username,pwd,APiKey_waba,WabaNo,ApiUrl_waba,providername from" + WABADBO + "customer with (nolock) where username='" + ProfileId + "'  AND PWD='" + APIKey + "'";
            DataTable dtcustomer = database.GetDataTable(sql);
            if (dtcustomer != null && dtcustomer.Rows.Count > 0)
            {

                var dt = Helper.database.GetDataTable(@"select distinct MSGID, isnull(e.mimErrDesc,'') Status,inserttime DeliveryTime,isnull(MiMErrCode,'') Code, 'C' Chargable
 from " + WABADBO + @" tblWABASubmitted s with (nolock)
  JOIN " + SMPPMAINDBO + @"DeliveryWABA d WITH (NOLOCK) ON s.messageid=d.msgid 
 JOIN " + WABADBO + @" CUSTOMER c WITH (NOLOCK) ON c.username=s.profileid 
 left JOIN " + WABADBO + @"tblWabaDlrErrCode e WITH (NOLOCK) ON e.providername=c.providername and e.dlrErrCode=d.status_groupId 
   WHERE MSGID='" + MsgID + "' order by d.inserttime desc");

                if (dt != null && dt.Rows.Count > 0)
                {

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;

                    row = new Dictionary<string, object>();
                    row.Add("SMS Delivery Status", resp);
                    rows.Add(row);

                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        row.Add("Communication Type", "WhatsApp Business");
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }

                    deliverydetail = JsonConvert.SerializeObject(rows);
                    response.Content = new StringContent(deliverydetail, Encoding.UTF8, "application/json");
                }
                else
                {
                    DataRow dr = dt1.NewRow();

                    dr[0] = resp;
                    dt1.Rows.Add(dr);


                    dr[0] = "WhatsApp not sent";
                    dt1.Rows.Add(dr);



                    deliverydetail = JsonConvert.SerializeObject(dt1);
                    response.Content = new StringContent(deliverydetail, Encoding.UTF8, "application/json");
                }
            }
            else if (Convert.ToString(MsgID) == "")
            {
                DataRow drow = dt1.NewRow();
                //DataRow drow1 = dt1.NewRow();
                drow[0] = "WhatsApp not sent";
                dt1.Columns.Add("SMS Delivery Status");
                drow[1] = resp;
                dt1.Rows.Add(drow);


                //dt1.Rows.Add(drow1);

                deliverydetail = JsonConvert.SerializeObject(dt1);
                response.Content = new StringContent(deliverydetail, Encoding.UTF8, "application/json");
            }
            else
            {
                DataRow dr = dt1.NewRow();
                dr[0] = "Unauthorized User";
                dt1.Rows.Add(dr);
                deliverydetail = JsonConvert.SerializeObject(dt1);
                response.Content = new StringContent(deliverydetail, Encoding.UTF8, "application/json");
            }
            return response;
        }

        //Secured Encrypted N
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
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID.";

                    if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }
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
                else
                    resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();

                string SubClientCode = string.Empty;
                DataTable dt2 = ob.GetDeliveryWithSubClientCode(msgid);
                if (dt2.Rows.Count > 0)
                {
                    SubClientCode = dt2.Rows[0]["SubClientCode"].ToString();
                    resp = resp + ", SubClientCode : " + SubClientCode;
                }
                return resp;
                //return resp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted N
        [Route("GetDeliveryWithCode")]
        [HttpGet]
        public string GetDeliveryWithCode(string userid, string pwd, string msgid)
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
                    dt = ob.GetUserParameterSecurePWD(userid, pwd);//For Pwd
                    if (dt.Rows.Count <= 0)
                    {
                        dt = ob.GetUserParameterSecure(userid, pwd);//for Apikey
                        if (dt.Rows.Count <= 0) return "Invalid Credentials";
                    }
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID.";
                    if (pwd != dt.Rows[0]["pwd"].ToString())
                    {
                        if (pwd != dt.Rows[0]["apikey"].ToString())
                            return "Incorrect Password";
                    }
                }
                DataTable dt1 = ob.GetDelivery(msgid);
                if (dt1.Rows.Count <= 0) return "Invalid Message ID";
                if (dt1.Rows[0]["PROFILEID"].ToString().ToUpper() != userid.ToUpper()) return "Invalid Message ID.";
                string resp = "";
                if (dt1.Rows[0]["dlvrstat"].ToString() == "")
                {
                    resp = "Delivery Status : Unknown";
                }
                else if (dt1.Rows[0]["dlvrstat"].ToString().ToUpper() == "DELIVERED")
                {
                    resp = "Delivery Status : Delivered";
                    resp += ", Delivery Time : " + Convert.ToDateTime(dt1.Rows[0]["DLVRTIME"]).ToString("dd-MMM-yyyy HH:mm:ss");
                }
                else
                {
                    resp = "Delivery Status : Failed. Error Code : " + dt1.Rows[0]["errcd"].ToString();
                }
                string SubClientCode = string.Empty;
                DataTable dt2 = ob.GetDeliveryWithCode(msgid);
                if (dt2.Rows.Count > 0)
                {
                    SubClientCode = dt2.Rows[0]["SubClientCode"].ToString();
                }
                return resp + ", Code : " + SubClientCode;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted N
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
                DataTable dt = new DataTable();
                //Shishir 07/08/2023 start
                if (bSecure)
                {
                    dt = ob.GetUserParameterSecure(userid, pwd);//for Apikey
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                //end
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    if (pwd != dt.Rows[0]["apikey"].ToString()) return "Incorrect Password";
                }
                string segment = ob.GetShortURLofLongURL(userid, longURL);
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Secured Encrypted N
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

            DataTable dt = new DataTable();
            //Shishir 07/08/2023 start
            if (bSecure)
            {
                dt = ob.GetUserParameterSecure(userid, pwd);//For ApiKey
                if (dt.Rows.Count <= 0) return "Invalid Credentials";
            }
            //end
            else
            {
                dt = ob.GetUserParameter(userid);
                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
            }

            string domain = dt.Rows[0]["domainname"].ToString();

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

        [Route("GetMobile")]
        [HttpGet]
        public HttpResponseMessage GetMobile(string userid)
        {
            string yourJson = "Testing JSON Output";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            DataTable dt1 = database.GetDataTable("Select * from MobileNumbers where userid='" + userid + "'");
            //dt1.Columns.Add("Response");
            //DataRow dr = dt1.NewRow();
            //dr[0] = yourJson;
            //dt1.Rows.Add(dr);
            yourJson = JsonConvert.SerializeObject(dt1);
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
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

        [Route("GetSMSSummary")]
        [HttpGet]
        public HttpResponseMessage GetSMSSummary(string userid, string pwd, string datefrom, string dateto)
        {

            //check user name and password
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            if (dateto == null) { yourJson = "Invalid dateto"; ret = true; }
            if (datefrom == null) { yourJson = "Invalid datefrom"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            DateTime dtm;
            string[] formats = { "yyyy-MM-dd" };
            if (!ret)
                if (!DateTime.TryParseExact(datefrom, formats,
                            System.Globalization.CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dtm))
                {
                    yourJson = "Invalid datefrom format. It should be yyyy-MM-dd"; ret = true;
                }

            if (!ret)
                if (!DateTime.TryParseExact(dateto, formats,
                           System.Globalization.CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out dtm))
                {
                    yourJson = "Invalid dateto format. It should be yyyy-MM-dd"; ret = true;
                }

            if (!ret)
                if (Convert.ToDateTime(dateto) < Convert.ToDateTime(datefrom))
                {
                    yourJson = "Invalid date. To Cannot be greater than from date"; ret = true;
                }
            Util ob = new Util();
            DataTable dt = new DataTable();
            if (!ret)
            {
                dt = ob.GetUserParameterSecurePWD(userid, pwd);
                if (dt.Rows.Count <= 0) { yourJson = "Invalid Credentials"; ret = true; }
            }
            if (!ret)
            {
                try
                {
                    DataTable dt1 = ob.SMSSummaryReport4API(userid, datefrom, dateto);
                    if (dt1.Rows.Count > 0 && dt1 != null)
                    {
                        SmsSummary obj = new SmsSummary();
                        obj.FromDate = Convert.ToString(dt1.Rows[0]["FromDate"]);
                        obj.ToDate = Convert.ToString(dt1.Rows[0]["ToDate"]);
                        obj.SUBMITTED = Convert.ToString(dt1.Rows[0]["submitted"]);
                        obj.DELIVERED = Convert.ToString(dt1.Rows[0]["delivered"]);
                        obj.FAILED = Convert.ToString(dt1.Rows[0]["failed"]);
                        obj.UNKNOWN = Convert.ToString(dt1.Rows[0]["unknown"]);
                        yourJson = JsonConvert.SerializeObject(obj);
                    }
                    else
                    { yourJson = "No Data Found"; ret = true; }
                }
                catch (Exception ex)
                {
                    ob.Log("GetSMSSummary : " + ex.ToString());
                }
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;

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

        [Route("SendSMSK")]
        [HttpGet]
        public async Task<string> SendSMSK(string userid, string apikey, string mobile, string sender, string msg, string msgtype)
        {
            try
            {
                //return "succes";
                //validations

                //sms types
                //DUBAI         - 16  --Added by Naved
                //premium sms   - 13
                //linktext sms  - 21
                //otp sms       - 33
                //campaign sms  - 47

                //check user name and password
                if (userid == null) return "Invalid User ID";
                //if (pwd == null) return "Invalid Password";
                if (apikey == null) return "Invalid API Key";
                if (mobile == null) return "Invalid Mobile Number";
                if (sender == null) return "Invalid Sender ID";
                if (msg == null) return "Invalid Message Text";
                if (msgtype == null) return "Invalid Message Type";
                //if (peid == null) return "Invalid PE-ID";
                string SubClientCode = "";
                mobile = mobile.Replace("+", "").Replace("-", "").Replace(" ", "");


                if (msgtype == "16")
                {
                    if (userid.ToUpper() == "MIM2201196")
                    {
                        if (mobile.Length != 12)
                        {
                            int lnm = mobile.Length;
                            if (lnm >= 9)
                                mobile = "971" + mobile.Substring(mobile.Length - 9, 9);
                            else
                                mobile = "971" + mobile;
                        }
                    }
                    if (mobile.Trim().Length < 12)
                    {
                        return "Invalid Mobile Number";
                    }
                }

                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

                Util ob = new Util();
                if (ob.invalidMobileCheck(mobList)) { return "Invalid Mobile Number"; }

                //validation of list count
                if (mobList.Count > 30) { return "Mobile numbers cannot be more than 30"; }
                string Usrid = Convert.ToString(database.GetScalarValue(@"select USERID from apiaccounts_live with (nolock) where userid='" + userid + "'"));
                //ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, peid, "");
                ob.InsertInAPiLog(userid, mobile, sender, msg, msgtype, "", "");

                DataTable dt = new DataTable();
                if (bSecure)
                {
                    //dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    dt = ob.GetUserParameterSecure(userid, apikey);
                    if (dt.Rows.Count <= 0) return "Invalid Credentials";
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) return "Invalid User ID";
                    //if (pwd != dt.Rows[0]["pwd"].ToString()) return "Incorrect Password";
                    if (apikey != dt.Rows[0]["apikey"].ToString()) return "Incorrect API Key";
                }

                if (msgtype != "15" && msgtype != "16")
                    if (mobile.Length < 10) return "Invalid Mobile Number.";

                //double Num;
                //bool isNum = double.TryParse(mobile, out Num);
                //if (!isNum) return "Invalid Mobile Number.";

                //if (mobile.Trim().Length == 10) mobile = "91" + mobile;

                if (msg.Trim() == "") return "Invalid Message Text";

                if (!(msgtype == "13" || msgtype == "21" || msgtype == "33" || msgtype == "47" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18")) return "Invalid Message Type";
                //check balance
                double rate = 0;
                int noofsms = GetMsgCount(msg.Trim());
                bool ucs = false;
                if (msg.Trim().Any(c => c > 126)) ucs = true;

                if (msgtype == "13" || msgtype == "15" || msgtype == "16" || msgtype == "17" || msgtype == "18") rate = Convert.ToDouble(dt.Rows[0]["rate_normalsms"]);
                if (msgtype == "21") rate = Convert.ToDouble(dt.Rows[0]["rate_smartsms"]);
                if (msgtype == "33") rate = Convert.ToDouble(dt.Rows[0]["rate_otp"]);
                if (msgtype == "47") rate = Convert.ToDouble(dt.Rows[0]["rate_campaign"]);

                #region Shishir 26/04/2024 Balance  Not be Checked For Postpaid
                if (Convert.ToString(dt.Rows[0]["AccountCreationType"]).ToUpper() != "POSTPAID")
                {
                    if ((Convert.ToDouble(dt.Rows[0]["balance"]) * 1000) <= ((rate * 10) * noofsms * mobList.Count))
                    { return "Insufficient Balance"; }
                }
                #endregion
                //check valid sender id
                if (!ob.CheckSenderId(userid, sender)) return "Invalid Sender ID";
                bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
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
                            if (retvl != "1") return retvl;
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
                            if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
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
                                if (retvl != "1") return retvl;
                            }
                        }
                    }

                    #region REJECTION20240219
                    string sRet = "";
                    string lastMsgID = "";
                    string m = mobile;
                    string nid = "";
                    int MobLen = Convert.ToInt32(m.Length);
                    if (MobLen == 12)
                    {
                        if (!(m.StartsWith("971")))
                        {
                            for (int x = 0; x < noofsms; x++)
                            {
                                string errcode_ = "5308";
                                string smsTex = ob.GetSMSText(msg, x + 1, noofsms, ucs);
                                nid = Guid.NewGuid().ToString();
                                string sql = " Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,INSERTDATE,FILEID,NSEND,smstext,smsrate,DELIVERY_STATUS) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1'," +
                                " N'" + msg.Replace("'", "''") + "','" + rate + "','REJECTED " + errcode_ + "' ; " +
                                " Insert into FAILsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,DELIVERY_DATETIME,MSGID,INSERTDATE,FILEID,NSEND) " +
                                " select '1' as id,'vcon','0','" + userid + "',N'" + smsTex + "','" + m + "','" + sender + "',GETDATE(),GETDATE(),GETDATE(),'" + nid + "',getdate(),'1','1' ; " +
                                " Insert into delivery (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,err_code,INSERTDATE) " +
                                " select 'id:" + nid + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                                "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:REJECTED err:" + errcode_ + " text:' AS DLVRTEXT," +
                                " '" + nid + "', GETDATE(), 'Rejected','" + errcode_ + "',getdate() ; ";
                                database.ExecuteNonQuery(sql);
                            }
                            sRet += "MobileNo: " + m + " Message ID: " + nid + ", ";
                            lastMsgID = nid;


                            string sx = ob.UpdateAndGetBalance(userid, "", noofsms * mobList.Count, rate);
                            sRet = sRet.Substring(0, sRet.Length - 2);
                            if (mobList.Count == 1)
                                return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                            else
                                return "SMS Submitted Successfully. " + sRet;
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



                if (userid == "MIM2101450" || sender == "HMISVR" || sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    sms_provider = sms_provider_API;
                    sms_ip = sms_ip_API;
                    sms_port = sms_port_API;
                    sms_acid = sms_acid_API;
                    sms_systemid = sms_systemid_API;
                    sms_password = sms_password_API;
                }

                //bool isNumeric = long.TryParse(Convert.ToString(sender).Trim(), out long n);
                if (isNumeric)
                {
                    sms_provider = sms_provider_PROMO;
                    sms_ip = sms_ip_PROMO;
                    sms_port = sms_port_PROMO;
                    sms_acid = sms_acid_PROMO;
                    sms_systemid = sms_systemid_PROMO;
                    sms_password = sms_password_PROMO;
                    string retvl = CheckPromoTime("INDIA", sender, mobList, noofsms, rate, msg, ucs, userid);
                    if (retvl != "1") return retvl;
                }
                if (sender == "HONDAE" || sender == "HONIND" || sender == "HNDASR")
                {
                    string msgid = "";
                    string ss = "";
                    foreach (var m in mobList)
                    {
                        msgid = GetMsgID();
                        //ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "");
                        ob.AddInMsgQueue(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid, sms_acid.Substring(0, 2), "", rate, noofsms, ucs, "");
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

                #region <Commented >

                if (Convert.ToString(Usrid) == "")
                {
                    string msgid1 = "";
                    if (msgtype == "13") sms_acid = sms_acid_API;
                    if (msgtype == "33") sms_acid = sms_acid_OTP;
                    if (isNumeric) sms_acid = sms_acid_PROMO;

                    string ss1 = "";
                    foreach (var m in mobList)
                    {
                        msgid1 = GetMsgID();
                        //ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), peid, rate, noofsms, ucs, "", SubClientCode);
                        ob.AddInMsgQueueAPI(userid, sender, m, msg.Replace("'", "''"), msgtype, msgid1, sms_acid.Substring(0, 2), "", rate, noofsms, ucs, "", SubClientCode);
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
                else
                {

                    string templateid = "";
                    if (msgtype != "15" && msgtype != "16" && msgtype != "47" && msgtype != "19" && msgtype != "20" && msgtype != "22" && msgtype != "30" && msgtype != "40" && msgtype != "50")
                    {
                        // rabi for template DLT block 02/11/2022
                        string errcd_ = "5308";
                        if (templateid == "1111111111111111111" || templateid == "" || templateid == "TEMPLATE-ID")
                        {
                            string sql = "";
                            string templID = ob.GetTemplateIDfromSMS(sender, msg, ucs);



                            if ((templID != "") && (msgtype == "13" || msgtype == "33" || msgtype == "17" || msgtype == "18"))
                            {
                                string[] ar1 = templID.Split(new string[] { "#$" }, StringSplitOptions.None);
                                string e_tempid = ar1[0];
                                //string e_peid = peid;
                                string e_sender = sender;

                                //errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + e_tempid + "' and peid='" + e_peid + "'"));
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + e_tempid + "'"));

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
                                //string e_peid = peid;
                                string e_sender = sender;
                                //errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + templateid + "' and peid='" + e_peid + "'"));
                                errcd_ = Convert.ToString(database.GetScalarValue("select top 1 isnull(errorcode,'')errorcode from errorlog where senderid='" + e_sender + "' and TemplateID='" + templateid + "'"));

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
                                    sRet = sRet.Substring(0, sRet.Length - 2);
                                    if (mobList.Count == 1)
                                        return "SMS Submitted Successfully. Message ID: " + lastMsgID;
                                    else
                                        return "SMS Submitted Successfully. " + sRet;
                                }
                            }
                        }
                    }



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
                            //.AddParameter(0x1400, peid)
                            .AddParameter(0x1401, templateid);
                            if (sms_provider.Contains("AIRTEL"))
                            {
                                pduBuilder = SMS.ForSubmit()
                                .From(sourceAddress)
                                .To(m)
                                .Coding(coding)
                                .DeliveryReceipt().ExpireIn(TimeSpan.FromMinutes(expMin))
                                .Text(msg)
                                //.AddParameter(0x1400, ob.getPEID(sms_provider, peid))
                                .AddParameter(0x1401, ob.getTEMPLATEID(sms_provider, templateid))
                                .AddParameter(0x1402, ob.getTMID(sms_provider));
                            }
                            pduList.AddRange(pduBuilder.Create(client));
                        }
                    }

                    IList<SubmitSmResp> resp = await client.SubmitAsync(pduList.ToArray());

                    UnBindResp Uresp = await client.UnbindAsync();
                    await client.DisconnectAsync();

                    string s = ob.UpdateAndGetBalance(userid, "", noofsms * mobcnt, rate);
                    //ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, peid, templateid, rate, ucs, sms_provider + '-' + sms_systemid);
                    ob.AddInMsgSubmitted(resp, userid, sender, mobile, msg.Replace("'", "''"), msgtype, Convert.ToString(resp[0].MessageId), Convert.ToString(resp[0].Header.Status), smppaccountid, "", templateid, rate, ucs, sms_provider + '-' + sms_systemid);
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
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }

    public class RCSRoot
    {
        public List<RCSMessage> messages { get; set; }
    }

    public class RCSMessage
    {
        public string to { get; set; }
        public int messageCount { get; set; }
        public string messageId { get; set; }
        public Status status { get; set; }
    }
    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    public class SmsSummary
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SUBMITTED { get; set; }
        public string DELIVERED { get; set; }
        public string FAILED { get; set; }
        public string UNKNOWN { get; set; }
    }
}


