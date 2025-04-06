using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Web.UI;
using StopSMS.Helper;

namespace StopSms
{
    public partial class STOPSMSMIM : System.Web.UI.Page
    {
        //string Segment = "", ProfileId = "";
        //string CC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Convert.ToString(Request.QueryString["id"]) != "" && Convert.ToString(Request.QueryString["id"]) != null)
            //{
            //    Segment = Convert.ToString(Request.QueryString["id"]);
            //    ProfileId = Convert.ToString(database.GetScalarValue(@"SELECT userid FROM short_urls with(nolock) WHERE segment='" + Segment + "'"));
            //}
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            SendOtpFunction();
        }

        public void SendOtpFunction()
        {
            string vOTP = "";
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number !!');", true);
                return;
            }
            string Mobileno = txtMobile.Text.ToString().Trim();
            string strfist = Mobileno.Substring(0, 1);
            if (strfist == "0")
            {
                strfist = Mobileno.Remove(0, 1);
                Mobileno = strfist;
            }
            Random rand = new Random();
            vOTP = rand.Next(100000, 999999).ToString();

            //string sql = "SELECT SENDERID,APIKEY,COUNTRYCODE FROM CUSTOMER with(nolock) WHERE username='" + ProfileId + "'";
            //DataTable dt = database.GetDataTable(sql);

            string sql2 = "SELECT StopSMS_Userid,StopSMS_Peid,StopSMS_Templateid,StopSMS_Senderid,StopSMS_SMSText FROM Settings with(nolock)";
            DataTable dt1 = database.GetDataTable(sql2);
            string Pwd = Convert.ToString(database.GetScalarValue(@"select pwd from customer with(nolock) where username='" + dt1.Rows[0]["StopSMS_Userid"] + "'"));


            //string APIKEY = dt.Rows[0]["APIKEY"].ToString().Trim();
            //string SENDERID = dt.Rows[0]["SENDERID"].ToString().Trim();
            //CC = dt.Rows[0]["COUNTRYCODE"].ToString().Trim();

            string ccmobileno = "91" + Mobileno;
            string ExistsCheck = "SELECT MOBILENO FROM MIMOPTOUTRCM with(nolock) WHERE MOBILENO='" + ccmobileno.Trim() + "'";
            DataTable dtExist = database.GetDataTableRCS(ExistsCheck);
            if (dtExist.Rows.Count > 0)
            {
                string ExistsMob = dtExist.Rows[0]["MOBILENO"].ToString().Trim();
                if (ExistsMob != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Number is Already UnSubscribed !!');", true);
                    return;
                }
            }
            //Session["COUNTRYCODE"] = CC;
            //if (Convert.ToString(CC) == "971")
            //{
            //    SendOTPthroughAPI(CC, Mobileno.Trim(), ProfileId, APIKEY, SENDERID, "Your OTP to stop sms is #var#", "", "", vOTP);
            //}
            //if (Convert.ToString(CC) == "91")
            //{
                SendOTPthroughAPI("91", Mobileno.Trim(), dt1.Rows[0]["StopSMS_Userid"].ToString(), Pwd, dt1.Rows[0]["StopSMS_Senderid"].ToString(), dt1.Rows[0]["StopSMS_SMSText"].ToString(), dt1.Rows[0]["StopSMS_Peid"].ToString(), dt1.Rows[0]["StopSMS_Templateid"].ToString(), vOTP);
            //}
            Session["SentOtp"] = vOTP;
            string sql1 = @"INSERT INTO LoginEntryStopSMS (MOBILENO,EntryDate,SENT_OTP) Values('" + ccmobileno.Trim() + "',CURRENT_TIMESTAMP,'" + vOTP + "')";
            database.ExecuteNonQueryRCS(sql1);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP sent Successfully');", true);
            return;
        }

        protected void btnSubmitOTP_Click(object sender, EventArgs e)
        {
            VerifiedOTP();
        }

        public void VerifiedOTP()
        {
            string Mobileno = txtMobile.Text.ToString().Trim();
            string strfist = Mobileno.Substring(0, 1);
            if (strfist == "0")
            {
                strfist = Mobileno.Remove(0, 1);
                Mobileno = strfist;
            }
            if (txtotp.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter OTP !!');", true);
                return;
            }
            //CC = Session["COUNTRYCODE"].ToString().Trim();
            string ccmobileno = "91" + Mobileno;
            string otp = Convert.ToString(database.GetScalarValueRCS(@"SELECT SENT_OTP FROM LoginEntryStopSMS with(nolock) WHERE MOBILENO='" + ccmobileno.Trim() + "'"));
            if (otp == Convert.ToString(txtotp.Text.Trim()))
            {
                string sql1 = @"UPDATE LoginEntryStopSMS SET SENT_OTP_VERIFY=CURRENT_TIMESTAMP WHERE MOBILENO='" + ccmobileno.Trim() + "'";
                database.ExecuteNonQueryRCS(sql1);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP Verify Successfully !!');", true);
                btnUnSubscribe.Visible = true;
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP not matched. Please try again.');", true);
                return;
            }
        }

        public void Reset()
        {
            txtMobile.Text = "";
            txtotp.Text = "";
        }

        public static void SendOTPthroughAPI(string CountryCode, string mob, string Userid, string pwd, string SenderId, string OtpSms, string Peid, string TemplateId, string Otp)
        {
            string url = "";
            if (CountryCode == "91")
            {
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + CountryCode + mob + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("#var#", Otp) + "&msgtype=13&PEID=" + Peid.ToString() + "&Templateid=" + TemplateId.ToString();
            }
            else if (CountryCode == "971")
            {
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + CountryCode + mob + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("#var#", Otp) + "&msgtype=16";
            }
            else
            {
                url = "https://myinboxmedia.in/api/mim/SendSMS?UserID=" + Userid.ToString() + "&pwd=" + pwd.ToString() + "&mobile=" + CountryCode + mob + "&sender=" + SenderId.ToString() + "&msg=" + OtpSms.ToString().Replace("#var#", Otp) + "&msgtype=15";

            }
            Log(url);
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                Log(objWinRq.ResponseText);
                getResponseTxt = "[" + getResponseTxt + "]";
            }
            catch (Exception EX)
            {
                Log(EX.Message);
            }
        }

        protected void btnUnSubscribe_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Number is invalid !!');", true);
                return;
            }
            string Mobileno = txtMobile.Text.ToString().Trim();
            string strfist = Mobileno.Substring(0, 1);
            if (strfist == "0")
            {
                strfist = Mobileno.Remove(0, 1);
                Mobileno = strfist;
            }
            //CC = Session["COUNTRYCODE"].ToString().Trim();
            string ccmobileno = "91" + Mobileno;
            box2.Attributes.Remove("style");
            string sql = @"INSERT INTO MIMOPTOUTRCM (MOBILENO,INSERTTIME) Values('" + ccmobileno.Trim() + "',CURRENT_TIMESTAMP)";
            database.ExecuteNonQueryRCS(sql);
            Reset();
        }

        public static void Log(string msg)
        {
            try
            {
                string fn = ConfigurationManager.AppSettings["LOGPATH"].ToString();
                StreamWriter writer1 = new StreamWriter(new FileStream(fn + "LogErr_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write));
                writer1.BaseStream.Seek(0L, SeekOrigin.End);
                writer1.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                writer1.Flush();
                writer1.Close();
            }
            catch (Exception)
            {
                string fn = ConfigurationManager.AppSettings["LOGPATH"].ToString();
                StreamWriter writer1 = new StreamWriter(new FileStream(fn + "LogErrCtch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write));
                writer1.BaseStream.Seek(0L, SeekOrigin.End);
                writer1.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                writer1.Flush();
                writer1.Close();
            }
        }
    }
}