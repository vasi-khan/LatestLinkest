using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerDataSYNC.Helper
{
    public class SyncBody
    {
        public class DealerMaster
        {
            public string DealerCode { get; set; }
            public string DealerName { get; set; }
            public string DealerMobile { get; set; }
            public DateTime LastSyncon { get; set; }
            public string SMSUserid { get; set; }
            public string SMSPassword { get; set; }
            public string SMSSender { get; set; }
            public string Peid { get; set; }
            public string SMSDomain { get; set; }
            public bool InActive { get; set; }
            public string BALURL { get; set; }
        }

        public class SMPPAccountUserId
        {
            public string User { get; set; }
            public string SMPPAccountid { get; set; }
            public string CountryCode { get; set; }
        }

        public class Customer
        {
            public string username { get; set; }
            public string SENDERID { get; set; }
            public string SMSTYPE { get; set; }
            public string FULLNAME { get; set; }
            public string ACCOUNTTYPE { get; set; }
            public string PERMISSION { get; set; }
            public string COMPNAME { get; set; }
            public string WEBSITE { get; set; }
            public string MOBILE1 { get; set; }
            public string MOBILE2 { get; set; }
            public string EMAIL { get; set; }
            public string COUNTRYCODE { get; set; }
            public DateTime? ACCOUNTCREATEDON { get; set; }
            public DateTime? EXPIRY { get; set; }
            public bool? ACTIVE { get; set; }
            public decimal? balance { get; set; }
            public decimal? rate_normalsms { get; set; }
            public decimal? rate_smartsms { get; set; }
            public decimal? rate_campaign { get; set; }
            public decimal? rate_otp { get; set; }
            public string DLTNO { get; set; }
            public string PWD { get; set; }
            public string MOBTRACK { get; set; }
            public string USERTYPE { get; set; }
            public string createdby { get; set; }
            public int? noofurl { get; set; }
            public int? noofhit { get; set; }
            public int? urlrate { get; set; }
            public string domainname { get; set; }
            public decimal? dltcharge { get; set; }
            public string peid { get; set; }
            public bool? showSMSDlr { get; set; }
            public bool? cansendsms { get; set; }
            public bool? showmobilexxxx { get; set; }
            public string APIKEY { get; set; }
            public bool? CAMPAIGN_APPLICABLE { get; set; }
            public string defaultCountry { get; set; }
            public string EmpCode { get; set; }
            public bool? SMSOnLowBalance { get; set; }
            public bool? EmailOnLowBalance { get; set; }
            public decimal? LowBalanceAmt { get; set; }
            public bool? WABARCS { get; set; }
            public int? WABARCSbal { get; set; }
            public bool? IsRCSActive { get; set; }
            public bool? IsRateShow { get; set; }
            public string DLRPushHookAPI { get; set; }
            public string ClickDataPushHookAPI { get; set; }
            public bool? showPEID { get; set; }
            public bool? Isshowcurrency { get; set; }
            public bool? IsInternalAcc { get; set; }
            public bool? IsShowSMSCount { get; set; }
            public string CCEmail { get; set; }
            public bool? ReportonEmail { get; set; }
            public string MIMSUMMARYUSERID { get; set; }
            public bool? ISSHOWBALANCE { get; set; }
            public string GROUPNAME { get; set; }
            public decimal? extrabal { get; set; }
            public int? FailOverWabaSecond { get; set; }
            public string wabaProfileId { get; set; }
            public string wabaPwd { get; set; }
            public bool? OTP_VERIFICATION_REQD { get; set; }
            public string Login_OTP_Template_ID { get; set; }
            public string Login_OTP_Sender_ID { get; set; }
            public char? Login_OTP_SMSWABA { get; set; }
            public string dlrHookApiHeader1 { get; set; }
            public string dlrHookApiHeader1val { get; set; }
            public string dlrHookApiHeader2 { get; set; }
            public string dlrHookApiHeader2val { get; set; }
            public string dlrHookApiHeader3 { get; set; }
            public string dlrHookApiHeader3val { get; set; }
            public string AccountCreationType { get; set; }
            public bool? ipwhitelisting { get; set; }
            public string OLDPWD { get; set; }
            public string OLDAPIKEY { get; set; }
            public string OLDPWD1 { get; set; }
            public string OLDAPIKEY1 { get; set; }
            public string TranOrPromo { get; set; }
            public string EmailUserId { get; set; }
            public int? FailOverOBDSecond { get; set; }
            public bool? Hidetemplateid { get; set; }
            public string WABAQrOtpTemplateId { get; set; }
            public bool? ISFLASHSMS { get; set; }
            public string MakerCheckerType { get; set; }
            public int? TestingCount { get; set; }
        }

        public class SenderIDMast
        {
            public string UserID { get; set; }
            public string SenderId { get; set; }
            public string CountryCode { get; set; }
        }

        public class Dashboard
        {
            public string Userid { get; set; }
        }

        public class SMSRateASPerCountry
        {
            public string UserName { get; set; }
            public string countrycode { get; set; }
            public string rate_normalsms { get; set; }
            public string rate_campaign { get; set; }
            public string rate_smartsms { get; set; }
            public string rate_otp { get; set; }
            public string urlrate { get; set; }
            public string dltcharge { get; set; }
        }

        public class MiMReportGroup
        {
            public string Client { get; set; }
            public string userid { get; set; }
            public string serverip { get; set; }
        }
    }
}