using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CustomerDataSYNC.Helper
{
    public class Util
    {
        public string fn = ConfigurationManager.AppSettings["LogPATH"].ToString();
        public void Log(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.FileStream filestrm = new FileStream(fn + @"catch_Log_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception)
                { }
            }
        }

        public string InsertDealerMasterDltNoBased(string DealerCode, string DealerName, string DealerMobile, string SMSUserID, string smspwd, string smssender, 
            string PEID, string SMSDomain, bool Inactive, string BalUrl)
        {
            string msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(database.GetConnectstring_HyundaiDb()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "Sp_DEALERMASTERDLTNOINSERT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerCode", DealerCode);
                    cmd.Parameters.AddWithValue("@DealerName", DealerName);
                    cmd.Parameters.AddWithValue("@DealerMobile", DealerMobile);
                    cmd.Parameters.AddWithValue("@SMSuserId", SMSUserID);
                    cmd.Parameters.AddWithValue("@SMSPwd", smspwd);
                    cmd.Parameters.AddWithValue("@Smssender", smssender);
                    cmd.Parameters.AddWithValue("@PEID", PEID);
                    cmd.Parameters.AddWithValue("@SmsDomain", SMSDomain);
                    cmd.Parameters.AddWithValue("@Inactive", Inactive);
                    cmd.Parameters.AddWithValue("@Balurl", BalUrl);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 50);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    msg = Convert.ToString(cmd.Parameters["@Result"].Value);
                    con.Close();
                }
            }
            catch (Exception ex) {
                Log("InsertDealerMasterDltNoBasedError_ " + ex);
                msg = Convert.ToString(ex);
            }
            return msg;
        }

        public void IsCountryCodeNotExistSMPPForUser(string user, string ForeignAccountId, string CountryCode)
        {
            string sql = string.Format("IF NOT EXISTS(SELECT * FROM smppaccountuserid WHERE userid ='{0}') Insert into smppaccountuserid (userid,smppaccountid,countrycode,active) Values('{1}','{2}','{3}',1)", user, user, ForeignAccountId, CountryCode);
            database.ExecuteNonQuery(sql);
        }

        public async Task<int> InsertCustomerAsync(
        string senderId, string smsType, string fullName, string accountType, string permission,
        string companyName, string website, string mobile1, string mobile2, string email,
        string countryCode, DateTime accountCreatedOn, DateTime expiry, bool active,
        decimal balance, decimal rateNormalSMS, decimal rateSmartSMS, decimal rateCampaign, decimal rateOTP,
        string username, string dltNo, string pwd, string mobTrack, string userType,
        string createdBy, int noOfUrl, int noOfHit, string domainName, string peid,
        string apiKey, bool campaignApplicable, string defaultCountry, string empCode, bool smsOnLowBalance,
        bool emailOnLowBalance, decimal lowBalanceAmt, bool wabARCS, int wabARCSBal,
        bool isRCSActive, bool isRateShow, string dlrPushHookApi, string clickDataPushHookApi, bool showPEID,
        bool isShowCurrency, bool isInternalAcc, bool isShowSMSCount, string ccEmail, bool reportOnEmail,
        string mimSummaryUserId, bool isShowBalance, string groupName, decimal extraBal, int failOverWabaSecond,
        string wabaProfileId, string wabaPwd, bool otpVerificationReqd, string loginOTPTemplateId, string loginOTPSenderId,
        char loginOTPSMSWABA, string dlrHookApiHeader1, string dlrHookApiHeader1Val, string dlrHookApiHeader2, string dlrHookApiHeader2Val,
        string dlrHookApiHeader3, string dlrHookApiHeader3Val, string accountCreationType, bool ipWhitelisting, string oldPwd,
        string oldApiKey, string oldPwd1, string oldApiKey1, string tranOrPromo, string emailUserId,
        int failOverOBDSecond, bool hideTemplateId, string wabaQrOtpTemplateId, bool isFlashSMS, string makerCheckerType,
        int testingCount)
        {
            try
            {
                int i = 0;
                using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "Sp_InsertCustomer";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SENDERID", senderId);
                    cmd.Parameters.AddWithValue("@SMSTYPE", smsType);
                    cmd.Parameters.AddWithValue("@FULLNAME", fullName);
                    cmd.Parameters.AddWithValue("@ACCOUNTTYPE", accountType);
                    cmd.Parameters.AddWithValue("@PERMISSION", permission);
                    cmd.Parameters.AddWithValue("@COMPNAME", companyName);
                    cmd.Parameters.AddWithValue("@WEBSITE", website);
                    cmd.Parameters.AddWithValue("@MOBILE1", mobile1);
                    cmd.Parameters.AddWithValue("@MOBILE2", mobile2);
                    cmd.Parameters.AddWithValue("@EMAIL", email);
                    cmd.Parameters.AddWithValue("@COUNTRYCODE", countryCode);
                    cmd.Parameters.AddWithValue("@ACCOUNTCREATEDON", accountCreatedOn);
                    cmd.Parameters.AddWithValue("@EXPIRY", expiry);
                    cmd.Parameters.AddWithValue("@ACTIVE", active);
                    cmd.Parameters.AddWithValue("@balance", balance);
                    cmd.Parameters.AddWithValue("@rate_normalsms", rateNormalSMS);
                    cmd.Parameters.AddWithValue("@rate_smartsms", rateSmartSMS);
                    cmd.Parameters.AddWithValue("@rate_campaign", rateCampaign);
                    cmd.Parameters.AddWithValue("@rate_otp", rateOTP);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@DLTNO", dltNo);
                    cmd.Parameters.AddWithValue("@PWD", pwd);
                    cmd.Parameters.AddWithValue("@MOBTRACK", mobTrack);
                    cmd.Parameters.AddWithValue("@USERTYPE", userType);
                    cmd.Parameters.AddWithValue("@createdby", createdBy);
                    cmd.Parameters.AddWithValue("@noofurl", noOfUrl);
                    cmd.Parameters.AddWithValue("@noofhit", noOfHit);
                    cmd.Parameters.AddWithValue("@domainname", domainName);
                    cmd.Parameters.AddWithValue("@peid", peid);
                    cmd.Parameters.AddWithValue("@APIKEY", apiKey);
                    cmd.Parameters.AddWithValue("@CAMPAIGN_APPLICABLE", campaignApplicable);
                    cmd.Parameters.AddWithValue("@defaultCountry", defaultCountry);
                    cmd.Parameters.AddWithValue("@EmpCode", empCode);
                    cmd.Parameters.AddWithValue("@SMSOnLowBalance", smsOnLowBalance);
                    cmd.Parameters.AddWithValue("@EmailOnLowBalance", emailOnLowBalance);
                    cmd.Parameters.AddWithValue("@LowBalanceAmt", lowBalanceAmt);
                    cmd.Parameters.AddWithValue("@WABARCS", wabARCS);
                    cmd.Parameters.AddWithValue("@WABARCSbal", wabARCSBal);
                    cmd.Parameters.AddWithValue("@IsRCSActive", isRCSActive);
                    cmd.Parameters.AddWithValue("@IsRateShow", isRateShow);
                    cmd.Parameters.AddWithValue("@DLRPushHookAPI", dlrPushHookApi);
                    cmd.Parameters.AddWithValue("@ClickDataPushHookAPI", clickDataPushHookApi);
                    cmd.Parameters.AddWithValue("@showPEID", showPEID);
                    cmd.Parameters.AddWithValue("@Isshowcurrency", isShowCurrency);
                    cmd.Parameters.AddWithValue("@IsInternalAcc", isInternalAcc);
                    cmd.Parameters.AddWithValue("@IsShowSMSCount", isShowSMSCount);
                    cmd.Parameters.AddWithValue("@CCEmail", ccEmail);
                    cmd.Parameters.AddWithValue("@ReportonEmail", reportOnEmail);
                    cmd.Parameters.AddWithValue("@MIMSUMMARYUSERID", mimSummaryUserId);
                    cmd.Parameters.AddWithValue("@ISSHOWBALANCE", isShowBalance);
                    cmd.Parameters.AddWithValue("@GROUPNAME", groupName);
                    cmd.Parameters.AddWithValue("@extrabal", extraBal);
                    cmd.Parameters.AddWithValue("@FailOverWabaSecond", failOverWabaSecond);
                    cmd.Parameters.AddWithValue("@wabaProfileId", wabaProfileId);
                    cmd.Parameters.AddWithValue("@wabaPwd", wabaPwd);
                    cmd.Parameters.AddWithValue("@OTP_VERIFICATION_REQD", otpVerificationReqd);
                    cmd.Parameters.AddWithValue("@Login_OTP_Template_ID", loginOTPTemplateId);
                    cmd.Parameters.AddWithValue("@Login_OTP_Sender_ID", loginOTPSenderId);
                    cmd.Parameters.AddWithValue("@Login_OTP_SMSWABA", loginOTPSMSWABA);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader1", dlrHookApiHeader1);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader1val", dlrHookApiHeader1Val);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader2", dlrHookApiHeader2);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader2val", dlrHookApiHeader2Val);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader3", dlrHookApiHeader3);
                    cmd.Parameters.AddWithValue("@dlrHookApiHeader3val", dlrHookApiHeader3Val);
                    cmd.Parameters.AddWithValue("@AccountCreationType", accountCreationType);
                    cmd.Parameters.AddWithValue("@ipwhitelisting", ipWhitelisting);
                    cmd.Parameters.AddWithValue("@OLDPWD", oldPwd);
                    cmd.Parameters.AddWithValue("@OLDAPIKEY", oldApiKey);
                    cmd.Parameters.AddWithValue("@OLDPWD1", oldPwd1);
                    cmd.Parameters.AddWithValue("@OLDAPIKEY1", oldApiKey1);
                    cmd.Parameters.AddWithValue("@TranOrPromo", tranOrPromo);
                    cmd.Parameters.AddWithValue("@EmailUserId", emailUserId);
                    cmd.Parameters.AddWithValue("@FailOverOBDSecond", failOverOBDSecond);
                    cmd.Parameters.AddWithValue("@Hidetemplateid", hideTemplateId);
                    cmd.Parameters.AddWithValue("@WABAQrOtpTemplateId", wabaQrOtpTemplateId);
                    cmd.Parameters.AddWithValue("@ISFLASHSMS", isFlashSMS);
                    cmd.Parameters.AddWithValue("@MakerCheckerType", makerCheckerType);
                    cmd.Parameters.AddWithValue("@TestingCount", testingCount);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                i = 1;
                return i;
            }
            catch (Exception ex)
            {
                Log("InsertCustomerError_ " + ex);
                throw;
            }
        }

        public void SenderIDMast(string UserID, string Sender, string CountryCode)
        {
            string sql = "INSERT INTO senderidmast (userid,senderid,countrycode) VALUES ('" + UserID + "','" + Sender + "','" + CountryCode + "')";
            database.ExecuteNonQuery(sql);
        }

        public void Dashboard(string UserID)
        {
            string sql = "INSERT INTO Dashboard (userid) values ('" + UserID + "')";
            database.ExecuteNonQuery(sql);
        }

        public void SMSRateASPerCountry(string UserID,string CountryCode,string rate_normalsms,string rate_campaign,string rate_smartsms, 
                                        string rate_otp, string urlrate, string dltcharge)
        {
            string sql = @"INSERT INTO smsrateaspercountry (USERNAME,countrycode, rate_normalsms,rate_campaign, rate_smartsms, rate_otp, urlrate,
                           dltcharge,insertdate) 
                           VALUES ('" + UserID + "','" + CountryCode + "','" + rate_normalsms + "','" + rate_campaign + "','" + rate_smartsms + @"',
                           '" + rate_otp + "','" + urlrate + "','" + dltcharge + "',GETDATE())";
            database.ExecuteNonQuery(sql);
        }

        public string InsertMiMReportGroup(string Client, string Serverip, string userid)
        {
            string Msg = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertMiMReportGroup";
                    cmd.Parameters.AddWithValue("@Client", Client);
                    cmd.Parameters.AddWithValue("@serverip", Serverip);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Msg"].Size = 56;
                    cmd.ExecuteNonQuery();
                    Msg = cmd.Parameters["@Msg"].Value.ToString().Trim();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Msg;
        }
    }
}