using CustomerDataSYNC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CustomerDataSYNC.Controllers
{
    [RoutePrefix("ApiSync")]
    public class DataSyncController : ApiController
    {
        Util ob = new Util();
        [Route("DealerMaster")]
        [HttpPost]
        public HttpResponseMessage DealerMaster(SyncBody.DealerMaster DM)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                request = ob.InsertDealerMasterDltNoBased(Convert.ToString(DM.DealerCode), Convert.ToString(DM.DealerName), Convert.ToString(DM.DealerMobile),
                    Convert.ToString(DM.SMSUserid), Convert.ToString(DM.SMSPassword), Convert.ToString(DM.SMSSender),
                    Convert.ToString(DM.Peid), Convert.ToString(DM.SMSDomain), Convert.ToBoolean(DM.InActive), Convert.ToString(DM.BALURL));
                ob.Log("DealerMaster " + request);
            }
            catch (Exception ex)
            {
                ob.Log("DealerMasterError " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("SMPPAccountUserId")]
        [HttpPost]
        public HttpResponseMessage SMPPAccountUserId(SyncBody.SMPPAccountUserId SMPP)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ob.IsCountryCodeNotExistSMPPForUser(Convert.ToString(SMPP.User), Convert.ToString(SMPP.SMPPAccountid), Convert.ToString(SMPP.CountryCode));
                request = "Insert Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("SMPPAccountUserIdError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("Customer")]
        [HttpPost]
        public async Task<HttpResponseMessage> Customer(SyncBody.Customer Cus)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                int i = await ob.InsertCustomerAsync(Convert.ToString(Cus.SENDERID), Convert.ToString(Cus.SMSTYPE),Cus.FULLNAME, Cus.ACCOUNTTYPE, Cus.PERMISSION, Cus.COMPNAME,
                Cus.WEBSITE,Cus.MOBILE1,Cus.MOBILE2,Cus.EMAIL, Cus.COUNTRYCODE, Cus.ACCOUNTCREATEDON ?? DateTime.Now, Cus.EXPIRY ?? DateTime.Now, 
                Cus.ACTIVE ?? false, Cus.balance ?? 0, Cus.rate_normalsms ?? 0, Cus.rate_smartsms ?? 0, Cus.rate_campaign ?? 0, Cus.rate_otp ?? 0, 
                Cus.username,Cus.DLTNO, Cus.PWD, Cus.MOBTRACK, Cus.USERTYPE, Cus.createdby, Cus.noofurl ?? 0, Cus.noofhit ?? 0, Cus.domainname, 
                Cus.peid, Cus.APIKEY, Cus.CAMPAIGN_APPLICABLE ?? false, Cus.defaultCountry, Cus.EmpCode,Cus.SMSOnLowBalance ?? false,
                Cus.EmailOnLowBalance ?? false,Cus.LowBalanceAmt ?? 0,Cus.WABARCS ?? false,Cus.WABARCSbal ?? 0,Cus.IsRCSActive ?? false,
                Cus.IsRateShow ?? false,Cus.DLRPushHookAPI,Cus.ClickDataPushHookAPI,Cus.showPEID ?? false,Cus.Isshowcurrency ?? false,Cus.IsInternalAcc ?? false,
                Cus.IsShowSMSCount ?? false,Cus.CCEmail,Cus.ReportonEmail ?? false,Cus.MIMSUMMARYUSERID,Cus.ISSHOWBALANCE ?? false,Cus.GROUPNAME,
                Cus.extrabal ?? 0,Cus.FailOverWabaSecond ?? 0,Cus.wabaProfileId, Cus.wabaPwd, Cus.OTP_VERIFICATION_REQD ?? false, Cus.Login_OTP_Template_ID, Cus.Login_OTP_Sender_ID, Cus.Login_OTP_SMSWABA ?? '\0',
                Cus.dlrHookApiHeader1, Cus.dlrHookApiHeader1val, Cus.dlrHookApiHeader2, Cus.dlrHookApiHeader2val, Cus.dlrHookApiHeader3, Cus.dlrHookApiHeader3val, Cus.AccountCreationType, Cus.ipwhitelisting ?? false, Cus.OLDPWD,
                Cus.OLDAPIKEY, Cus.OLDPWD1, Cus.OLDAPIKEY1, Cus.TranOrPromo, Cus.EmailUserId, Cus.FailOverOBDSecond ?? 0, Cus.Hidetemplateid ?? false, 
                Cus.WABAQrOtpTemplateId, Cus.ISFLASHSMS ?? false, Cus.MakerCheckerType, Cus.TestingCount ?? 0);
                request = "Insert Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("CustomerError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("SenderIDMast")]
        [HttpPost]
        public HttpResponseMessage SenderIDMast(SyncBody.SenderIDMast SIM)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ob.SenderIDMast(Convert.ToString(SIM.UserID), Convert.ToString(SIM.SenderId), Convert.ToString(SIM.CountryCode));
                request = "Insert SenderIDMast Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("SenderIDMastError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("Dashboard")]
        [HttpPost]
        public HttpResponseMessage Dashboard(SyncBody.Dashboard Das)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ob.Dashboard(Convert.ToString(Das.Userid));
                request = "Insert Dashboard Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("DashboardError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("SMSRateASPerCountry")]
        [HttpPost]
        public HttpResponseMessage SMSRateASPerCountry(SyncBody.SMSRateASPerCountry SMSRate)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ob.SMSRateASPerCountry(Convert.ToString(SMSRate.UserName), Convert.ToString(SMSRate.countrycode), Convert.ToString(SMSRate.rate_normalsms),
                    Convert.ToString(SMSRate.rate_campaign), Convert.ToString(SMSRate.rate_smartsms), Convert.ToString(SMSRate.rate_otp),
                    Convert.ToString(SMSRate.urlrate), Convert.ToString(SMSRate.dltcharge));
                request = "Insert SMSRateASPerCountry Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("SMSRateASPerCountryError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("MiMReportGroup")]
        [HttpPost]
        public HttpResponseMessage MiMReportGroup(SyncBody.MiMReportGroup Rep)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ob.InsertMiMReportGroup(Convert.ToString(Rep.Client), Convert.ToString(Rep.serverip), Convert.ToString(Rep.userid));
                request = "Insert MiMReportGroup Sucessfully";
            }
            catch (Exception ex)
            {
                ob.Log("MiMReportGroupError_ " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(request, Encoding.UTF8, "application/json");
            return response;
        }
    }
}
