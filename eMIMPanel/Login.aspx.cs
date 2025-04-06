using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Login : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        Helper.common obj = new Helper.common();
        int Minute = Convert.ToInt32(ConfigurationManager.AppSettings["LogintoLinkextValidityMinute"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Request.QueryString["d"]) != null && Convert.ToString(Request.QueryString["d"]) != "")
            {
                string Segment = Convert.ToString(Request.QueryString["d"]);
                DataTable dt = database.GetDataTable(@"SELECT Id,Added, case when dateadd(minute," + Minute + ",Added) > getdate() then 1 else 0 end as validLogin FROM short_urls WITH(NOLOCK) WHERE segment='" + Segment + "'");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["validLogin"]) == 1)
                    {
                        string Mobile = Convert.ToString(database.GetScalarValue(@"select mobile from mobtrackurl with(nolock) where urlid='" + dt.Rows[0]["Id"] + "'"));
                        DataTable dtC = database.GetDataTable(@"SELECT top 1 username,pwd FROM Customer WITH(NOLOCK) WHERE MOBILE1='" + Mobile + "'");
                        int cntLogin = Convert.ToInt16(database.GetScalarValue("Select count(*) from LoginToLinkext_LOG where Segment='" + Segment + "'"));
                        if (cntLogin > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Your unique link for login to Linkext has been expired !!. Please login with userid and password.');window.location ='Login.aspx';", true);
                        }
                        else
                        {
                            database.ExecuteNonQuery("Insert into LoginToLinkext_LOG (userid, segment) values ('" + Convert.ToString(dtC.Rows[0]["username"]) + "','" + Segment + "')");
                            LoginUserDetails(Convert.ToString(dtC.Rows[0]["username"]), Convert.ToString(dtC.Rows[0]["pwd"]));
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Your unique link for login to Linkext has been expired !!. Please login with userid and password.');window.location ='Login.aspx';", true);
                    }
                }
            }


            string ws = ConfigurationManager.AppSettings["LoginWebSite"].ToString();
            string ws2 = ConfigurationManager.AppSettings["LoginWebSite2"].ToString();
            string ws3 = ConfigurationManager.AppSettings["LoginWebSite3"].ToString();
            string ws4 = ConfigurationManager.AppSettings["LoginWebSite4"].ToString();
            string ws5 = ConfigurationManager.AppSettings["LoginWebSite5"].ToString();

            //string url = string.Format("{0}://{1}}", Request.Url.Scheme, Request.Url.Authority, "/");
            if (ws.ToUpper() == Request.Url.Authority.ToUpper()) { }
            else if (ws2.ToUpper() == Request.Url.Authority.ToUpper()) { }
            else if (ws3.ToUpper() == Request.Url.Authority.ToUpper()) { }
            else if (ws4.ToUpper() == Request.Url.Authority.ToUpper()) { }
            else if (ws5.ToUpper() == Request.Url.Authority.ToUpper()) { }
            else { Response.Redirect("~/IncorrectURL.html"); }
            //Session["UserID"] = null;
            //Session["User"] = null;
            //Session.Abandon();
        }

        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

        private static string GetClientMAC(string strClientIP)
        {
            string mac_dest = "";
            try
            {
                Int32 ldest = inet_addr(strClientIP);
                Int32 lhost = inet_addr("");
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                string mac_src = macinfo.ToString("X");

                while (mac_src.Length < 12)
                {
                    mac_src = mac_src.Insert(0, "0");
                }

                for (int i = 0; i < 11; i++)
                {
                    if (0 == (i % 2))
                    {
                        if (i == 10)
                        {
                            mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                        else
                        {
                            mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("L?i " + err.Message);
            }
            return mac_dest;
        }

        private static string Fetch_UserIP()
        {
            string VisitorsIPAddress = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddress = HttpContext.Current.Request.UserHostAddress;
                }
            }
            catch (Exception ex)
            {

                //Handle Exceptions  
            }
            return VisitorsIPAddress;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            if (txtUserID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
                return;
            }
            if (txtPassword.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
                return;
            }

            if (txtCaptcha.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Captcha can not be blank.');", true);
                return;
            }

            bool IsValid = false;
            if (!string.IsNullOrEmpty(txtCaptcha.Text.Trim()))
            {
                captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());
                IsValid = captcha1.UserValidated;
                if (!IsValid)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please Enter Valid Captcha!');", true);
                    return;
                }
            }

            LoginUserDetails(txtUserID.Text.Trim(), txtPassword.Text.Trim());
            #region < Commented >
            // 2. User ID creation..
            //string type = ob.CheckValidUser(txtUserID.Text.Trim(), txtPassword.Text.Trim());
            //if (type != "")
            //{
            //    DataTable dt = ob.GetUserParameter(txtUserID.Text.Trim());
            //    if (Convert.ToBoolean(dt.Rows[0]["ACTIVE"]) == false)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account deactivated. Please contact administrator.');", true);
            //        return;
            //    }
            //    string OTPVerification = dt.Rows[0]["OTP_VERIFICATION_REQD"].ToString().ToUpper();
            //    if (OTPVerification.ToUpper() == "FALSE")
            //    {
            //        Session["WABARCS"] = dt.Rows[0]["WABARCS"].ToString().ToUpper();
            //        Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
            //        Session["URLBALANCE"] = dt.Rows[0]["noofurl"].ToString();
            //        Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();
            //        DataTable dtRate = database.GetDataTable("select * from smsrateaspercountry WHERE username='" + dt.Rows[0]["UserName"].ToString() + "' and countrycode='" + dt.Rows[0]["defaultCountry"].ToString() + "'");
            //        if (dtRate.Rows.Count > 0)
            //        {
            //            Session["URLRATE"] = dtRate.Rows[0]["urlrate"].ToString();
            //            Session["DLTCHARGE"] = dtRate.Rows[0]["dltcharge"].ToString();
            //            Session["RATE_NORMALSMS"] = dtRate.Rows[0]["rate_normalsms"].ToString();
            //            Session["RATE_SMARTSMS"] = dtRate.Rows[0]["rate_smartsms"].ToString();
            //            Session["RATE_CAMPAIGN"] = dtRate.Rows[0]["rate_campaign"].ToString();
            //            Session["RATE_OTP"] = dtRate.Rows[0]["rate_otp"].ToString();
            //        }
            //        else
            //        {
            //            Session["URLRATE"] = dt.Rows[0]["urlrate"].ToString();
            //            Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
            //            Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
            //            Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
            //            Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
            //            Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();
            //        }
            //        //Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
            //        //Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
            //        //Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
            //        //Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
            //        //Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();

            //        Session["DLT"] = dt.Rows[0]["DLTNO"].ToString();
            //        Session["EMPCODE"] = dt.Rows[0]["EMPCODE"].ToString();
            //        Session["PEID"] = dt.Rows[0]["PEID"].ToString();
            //        Session["MOBILE"] = dt.Rows[0]["MOBILE1"].ToString();
            //        Session["SENDERID"] = dt.Rows[0]["SENDERID"].ToString();
            //        Session["DOMAINNAME"] = dt.Rows[0]["DOMAINNAME"].ToString();
            //        Session["SHOWSMSDLR"] = dt.Rows[0]["showSMSDlr"].ToString();
            //        Session["CANSENDSMS"] = dt.Rows[0]["cansendsms"].ToString();
            //        Session["SHOWMOBILEXXXX"] = dt.Rows[0]["showmobilexxxx"].ToString();
            //        Session["DEFAULTCOUNTRYCODE"] = dt.Rows[0]["defaultCountry"].ToString();
            //        Session["FullName"] = dt.Rows[0]["FullName"].ToString();

            //        Helper.Global.SHOWMOBILEXXXX = dt.Rows[0]["showmobilexxxx"].ToString().ToUpper();
            //        DataTable dt1 = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            //        Global.addMinutes = dt1.Rows.Count == 0 ? 0 : Convert.ToInt16(dt1.Rows[0]["timedifferenceInMinute"]);
            //        Session["timedifferenceInMinute"] = Global.addMinutes;



            //        if (Session["DOMAINNAME"] == null || Convert.ToString(Session["DOMAINNAME"]) == "")
            //            Session["DOMAINNAME"] = ConfigurationManager.AppSettings["WebSite"].ToString();
            //        Session.Timeout = 30;
            //        Session["UserType"] = type;
            //        Session["CURRENCY"] = Convert.ToString(dt1.Rows[0]["CURRENCY"]);
            //        Session["mobLength"] = Convert.ToString(dt1.Rows[0]["mobLength"]);
            //        Session["SUBCURRENCY"] = Convert.ToString(dt1.Rows[0]["SUBCURRENCY"]);
            //        Session["maximgfilesize"] = Convert.ToString(dt1.Rows[0]["maximgfilesize"]);
            //        Session["maxvideofilesize"] = Convert.ToString(dt1.Rows[0]["maxvideofilesize"]);
            //        Session["Notice"] = Convert.ToString(dt1.Rows[0]["NoticeMsg"]);

            //        DataTable DTSMPPAC = ob.GetUserSMPPACCOUNT(dt.Rows[0]["DLTNO"].ToString(), txtUserID.Text.Trim());
            //        Session["DTSMPPAC"] = DTSMPPAC;

            //        Session["ISIntermediate"] = "NO";

            //        string uid = HttpUtility.UrlEncode(ob.Encrypt(txtUserID.Text.Trim()));
            //        string pwd = HttpUtility.UrlEncode(ob.Encrypt(txtPassword.Text.Trim()));
            //        string subDomain = Convert.ToString(dt1.Rows[0]["subDomain"]);

            //        //subDomain = "http://localhost:54751/";

            //        if (type == "USER")
            //        {
            //            ob.DropUserTmpTable(txtUserID.Text.Trim());

            //            Session["UserID"] = txtUserID.Text.Trim();
            //            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
            //                Response.Redirect("index_u2.aspx", false);
            //            else
            //                Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
            //            //Response.Redirect("index_u2.aspx");
            //        }
            //        else if (type == "BD")
            //        {
            //            Helper.common.mlogin mlog = new Helper.common.mlogin();
            //            DataTable empdt = obj.EmployeeDetails(Convert.ToString(Session["EMPCODE"]));
            //            DataTable dlt = obj.EmployeeLogin(Convert.ToString(Session["EMPCODE"]), Convert.ToString(empdt.Rows[0]["RequestFormPwd"]));
            //            if (dlt != null)
            //            {
            //                if (dlt.Rows.Count > 0)
            //                {
            //                    mlog.usernmae = dlt.Rows[0]["USER"].ToString().Trim();
            //                    mlog.role = dlt.Rows[0]["RoleCode"].ToString().Trim();
            //                    mlog.employeeid = int.Parse(dlt.Rows[0]["employeeid"].ToString().Trim());
            //                    mlog.name = dlt.Rows[0]["Name"].ToString().Trim();
            //                    Session["USER_BD"] = mlog;
            //                    Session["Lang"] = dlt.Rows[0]["LangCode"].ToString().Trim();
            //                    Session["Role"] = dlt.Rows[0]["RoleCode"].ToString().Trim();

            //                    if (mlog.role.ToString() == "E")
            //                    {
            //                        Session["User"] = txtUserID.Text.Trim();
            //                        Response.Redirect("index2.aspx");

            //                    }
            //                    else
            //                    {
            //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
            //                        return;
            //                    }

            //                }
            //                else
            //                {
            //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
            //                    return;
            //                }
            //                Session["Time"] = DateTime.Now.ToString();

            //            }
            //        }
            //        else
            //        {
            //            Session["User"] = txtUserID.Text.Trim();
            //            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
            //                Response.Redirect("index2.aspx");
            //            else
            //                Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
            //        }

            //        int Exists = Convert.ToInt32(database.GetScalarValue("select COUNT(1) from LoginEntry with(nolock) where UserId='" + txtUserID.Text.Trim() + "'"));
            //        if (Exists == 1)
            //        {
            //            ob.UpdateLastLoginDate(Convert.ToString(txtUserID.Text.Trim()));
            //        }
            //        else
            //        {
            //            ob.InsertCredentialsInLoginEntry(Convert.ToString(txtUserID.Text.Trim()));
            //        }
            //    }
            //    else
            //    {
            //        divOTP.Visible = true;
            //        Random rand = new Random();
            //        string vOTP = rand.Next(100000, 999999).ToString();
            //        Session["vOTP"] = vOTP;
            //        if (dt.Rows.Count > 0)
            //        {
            //            string Country_Code = dt.Rows[0]["COUNTRYCODE"].ToString();
            //            string MobileNo = dt.Rows[0]["MOBILE1"].ToString();
            //            string XXNo = "XXXXXX";
            //            string last4 = MobileNo.Substring(MobileNo.Length - 4, 4);
            //            lblSentOtpMobNo.Text = XXNo + last4;

            //            string LinkextUserId = dt.Rows[0]["username"].ToString();
            //            string pwd = dt.Rows[0]["APIKEY"].ToString();
            //            string APIKEY = dt.Rows[0]["APIKEY"].ToString();
            //            if(LinkextUserId.ToUpper() == "MIM2200812")
            //            {
            //                LinkextUserId = "MIM2200838";
            //                pwd = "AbcAcc123";
            //                APIKEY = "AbcAcc123";
            //            }
            //            string SENDERID = dt.Rows[0]["Login_OTP_Sender_ID"].ToString().Trim();
            //            string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
            //            string loginOTPType = dt.Rows[0]["Login_OTP_SMSWABA"].ToString();

            //            string peid = dt.Rows[0]["peid"].ToString();
            //            string TemplateId= dt.Rows[0]["Login_OTP_Template_ID"].ToString();
            //            if (loginOTPType=="S")
            //            {
            //                if (Country_Code == "91")
            //                {
            //                    DataTable dtSms = ob.GetDataLoginSMSFortemplate(LinkextUserId, TemplateId);
            //                    msg = dtSms.Rows[0]["template"].ToString();
            //                }
            //                msg = msg.ToString().Replace("{#var#}", vOTP);
            //                msg = msg.ToString().Replace("#var1", vOTP);
            //                ob.SendOTPthroughAPI(Country_Code, MobileNo, LinkextUserId, pwd, SENDERID, msg, peid, TemplateId, vOTP);
            //            }
            //            else
            //            {
            //                msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
            //                ob.WABA_MIM_Media_Template_LoginOTP_Message_Gupshup(MobileNo, LinkextUserId, APIKEY, msg, vOTP);
            //            }
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP sent successfully please check..');", true);
            //            ScriptManager.RegisterStartupScript(this,this.GetType(), "CallMyFunction", "MyFunction()", true);
            //        }
            //    }
            //}
            //else
            //{
            //    Session["User"] = null;
            //    Session["UserType"] = null;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential.');", true);
            //}
            #endregion
        }

        public void LoginUserDetails(string Userid, string Password)
        {
            Helper.Util hl = new Helper.Util();
            //Shishir Start
            DataTable dt = hl.GetLoginDetails(Userid, Password);
            if (dt == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                return;
            }
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                return;
            }
            //if user active under 45 days SHISHIR//
            //string LastLogin = ob.GetLastLoginTime(txtUserID.Text.Trim());
            Session["Username"] = txtUserID.Text.Trim();
            string LastLogin = ob.GetLastLoginDetails(txtUserID.Text.Trim());
            if (LastLogin != "")
            {
                string AlertMsg = ConfigurationManager.AppSettings["AlertMsg"].ToString();
                lblHeader.Text = AlertMsg;
                lblAlert.Text = "Do You Want to Activate Your Account ?";
                ViewState["username"] = dt.Rows[0]["username"].ToString();
                //ModalPopitm.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
            //else
            //{
            //    if (dt.Rows[0]["usertype"].ToString().ToUpper() == "USER")
            //    {
            //        DataTable dt8 = ob.GetLastLogindt(txtUserID.Text.Trim());
            //        if (dt8 != null && dt8.Rows.Count > 0)
            //        {
            //            DateTime LastLoginTime = Convert.ToDateTime(Convert.ToString(dt8.Rows[0]["SMSDATE"]));
            //            DateTime CurrentLoginTime = DateTime.Now;
            //            TimeSpan time = CurrentLoginTime - LastLoginTime;
            //            int ToDays = Convert.ToInt32(time.TotalDays);
            //            if (ToDays > 45)
            //            {
            //                string AlertMsg = ConfigurationManager.AppSettings["AlertMsg"].ToString();

            //                lblHeader.Text = AlertMsg;
            //                lblAlert.Text = "Do You Want to Activate Your Account ?";
            //                ViewState["username"] = dt.Rows[0]["username"].ToString();
            //                //ModalPopitm.Show();
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
            //                return;
            //            }
            //        }
            //    }
            //}
            //SHISHIR END
            //if (Convert.ToString(LastLogin) != "")
            //{
            //    DateTime LastLoginTime = Convert.ToDateTime(LastLogin);
            //    DateTime CurrentLoginTime = DateTime.Now;
            //    TimeSpan time = CurrentLoginTime - LastLoginTime;
            //    int ToDays = Convert.ToInt32(time.TotalDays); 
            //    if (ToDays > 45)
            //    {
            //        string AlertMsg = ConfigurationManager.AppSettings["AlertMsg"].ToString();

            //        lblHeader.Text = AlertMsg;
            //        lblAlert.Text = "Do You Want to Activate Your Account ?";
            //        ViewState["username"] = dt.Rows[0]["username"].ToString();
            //        //ModalPopitm.Show();
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
            //        return;
            //    }
            //}
            //user active end//
            //Previous Password Check Start
            //string type = ob.CheckValidUser(Userid.Trim(), Password.Trim());
            string type = Convert.ToString(dt.Rows[0]["usertype"]);
            if (type != "")
            {
                //DataTable dt = ob.GetUserParameter(Userid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["IsFlashSMS"] = Convert.ToBoolean(dt.Rows[0]["ISFLASHSMS"]);
                    Session["MacId"] = GetClientMAC(GetIPAddress());
                    Session["IpAddress"] = Fetch_UserIP();

                    // Insert Ip & Mac
                    hl.InsertIpMac(Userid, Convert.ToString(Session["MacId"]), Convert.ToString(Session["IpAddress"]));


                    bool IPWhiteListing = hl.GetIPWhiteListing(Userid);
                    Session["IPWhiteListing"] = IPWhiteListing;

                    DataTable dtgetIp = hl.GetIP(Userid);
                    Session["dtgetIp"] = dtgetIp;
                    type = dt.Rows[0]["usertype"].ToString();
                    //Shishir Over

                    Session["PWD"] = txtPassword.Text.Trim();//Shishir

                    //Previous Password Check End

                    if (Convert.ToBoolean(dt.Rows[0]["ACTIVE"]) == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account deactivated. Please contact administrator.');", true);
                        return;
                    }
                    string OTPVerification = dt.Rows[0]["OTP_VERIFICATION_REQD"].ToString().ToUpper();
                    if (OTPVerification.ToUpper() == "FALSE")
                    {
                        Session["WABARCS"] = dt.Rows[0]["WABARCS"].ToString().ToUpper();
                        Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
                        Session["URLBALANCE"] = dt.Rows[0]["noofurl"].ToString();
                        Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();

                        Session["MakerCheckerType"] = Convert.ToString(dt.Rows[0]["MakerCheckerType"]);  // Add By Vikas On 02_07_2024


                        DataTable dtRate = database.GetDataTable("select * from smsrateaspercountry WHERE username='" + dt.Rows[0]["UserName"].ToString() + "' and countrycode='" + dt.Rows[0]["defaultCountry"].ToString() + "'");
                        if (dtRate.Rows.Count > 0)
                        {
                            Session["URLRATE"] = dtRate.Rows[0]["urlrate"].ToString();
                            Session["DLTCHARGE"] = dtRate.Rows[0]["dltcharge"].ToString();
                            Session["RATE_NORMALSMS"] = dtRate.Rows[0]["rate_normalsms"].ToString();
                            Session["RATE_SMARTSMS"] = dtRate.Rows[0]["rate_smartsms"].ToString();
                            Session["RATE_CAMPAIGN"] = dtRate.Rows[0]["rate_campaign"].ToString();
                            Session["RATE_OTP"] = dtRate.Rows[0]["rate_otp"].ToString();
                        }
                        else
                        {
                            Session["URLRATE"] = dt.Rows[0]["urlrate"].ToString();
                            Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                            Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                            Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                            Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                            Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();
                        }
                        //Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                        //Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                        //Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                        //Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                        //Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();

                        Session["DLT"] = dt.Rows[0]["DLTNO"].ToString();
                        Session["EMPCODE"] = dt.Rows[0]["EMPCODE"].ToString();
                        Session["PEID"] = dt.Rows[0]["PEID"].ToString();
                        Session["MOBILE"] = dt.Rows[0]["MOBILE1"].ToString();
                        Session["SENDERID"] = dt.Rows[0]["SENDERID"].ToString();
                        Session["DOMAINNAME"] = dt.Rows[0]["DOMAINNAME"].ToString();
                        Session["SHOWSMSDLR"] = dt.Rows[0]["showSMSDlr"].ToString();
                        Session["CANSENDSMS"] = dt.Rows[0]["cansendsms"].ToString();
                        Session["SHOWMOBILEXXXX"] = dt.Rows[0]["showmobilexxxx"].ToString();
                        Session["DEFAULTCOUNTRYCODE"] = dt.Rows[0]["defaultCountry"].ToString();
                        Session["FullName"] = dt.Rows[0]["FullName"].ToString();
                        Session["Hidetemplateid"] = ""; // Convert.ToString(dt.Rows[0]["Hidetemplateid"]);
                        Session["WabaProfileId"] = dt.Rows[0]["WabaProfileId"].ToString();
                        Session["WabaPwd"] = dt.Rows[0]["WabaPwd"].ToString();
                        Session["ShowDNDInSummary"] = dt.Rows[0]["ShowDNDInSummary"].ToString();

                        Helper.Global.SHOWMOBILEXXXX = dt.Rows[0]["showmobilexxxx"].ToString().ToUpper();
                        DataTable dt1 = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
                        Global.addMinutes = dt1.Rows.Count == 0 ? 0 : Convert.ToInt16(dt1.Rows[0]["timedifferenceInMinute"]);
                        Session["timedifferenceInMinute"] = Global.addMinutes;



                        if (Session["DOMAINNAME"] == null || Convert.ToString(Session["DOMAINNAME"]) == "")
                            Session["DOMAINNAME"] = ConfigurationManager.AppSettings["WebSite"].ToString();
                        Session.Timeout = 30;
                        Session["UserType"] = type;
                        Session["CURRENCY"] = Convert.ToString(dt1.Rows[0]["CURRENCY"]);
                        Session["mobLength"] = Convert.ToString(dt1.Rows[0]["mobLength"]);
                        Session["MobMIN"] = Convert.ToString(dt1.Rows[0]["MobMIN"]);
                        Session["MobMAX"] = Convert.ToString(dt1.Rows[0]["MobMAX"]);
                        Session["SUBCURRENCY"] = Convert.ToString(dt1.Rows[0]["SUBCURRENCY"]);
                        Session["maximgfilesize"] = Convert.ToString(dt1.Rows[0]["maximgfilesize"]);
                        Session["maxvideofilesize"] = Convert.ToString(dt1.Rows[0]["maxvideofilesize"]);
                        Session["Notice"] = Convert.ToString(dt1.Rows[0]["NoticeMsg"]);


                        DataTable DTSMPPAC = ob.GetUserSMPPACCOUNT(dt.Rows[0]["DLTNO"].ToString(), Userid.Trim());
                        Session["DTSMPPAC"] = DTSMPPAC;

                        Session["ISIntermediate"] = "NO";

                        string uid = HttpUtility.UrlEncode(ob.Encrypt(Userid.Trim()));
                        string pwd = HttpUtility.UrlEncode(ob.Encrypt(Password.Trim()));
                        string subDomain = Convert.ToString(dt1.Rows[0]["subDomain"]);
#if DEBUG
                        subDomain = "http://localhost:54751/";
#endif

                        if (type.ToUpper() != "SYSADMIN")
                        {
                            Session["PassWord"] = txtPassword.Text.ToString();
                            int CheckExists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM LoginEntry with(nolock) WHERE UserId='" + Userid.Trim() + "'"));
                            if (CheckExists == 1)
                            {
                                CheckExists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM LoginEntry with(nolock) WHERE UserId='" + Userid.Trim() + "' AND LoginStatus=1"));
                                if (CheckExists == 1)
                                {
                                    string AfterYesLogin = Convert.ToString(Session["AfterYesLogin"]);
                                    if (AfterYesLogin == "Yes")
                                    {
                                        Session["Login_Session_Id"] = Guid.NewGuid().ToString();
                                        Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='1',LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE UserId='" + Userid.Trim() + "'");
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm()", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    Session["Login_Session_Id"] = Guid.NewGuid().ToString();
                                    Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='1',LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE UserId='" + Userid.Trim() + "'");
                                }
                            }
                        }
                        if (type == "USER")
                        {
                            ob.DropUserTmpTable(Userid.Trim());
                            Session["UserID"] = Userid.Trim();
                            ob.DropUserTmpTable(Userid.Trim());
                            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
                            {
                                LoginEntry(Userid);
                                Response.Redirect("index_u2.aspx", false);
                            }
                            else
                            {
#if DEBUG
                                LoginEntry(Userid);
                                Response.Redirect("index_u2.aspx");
#else
                                Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
#endif
                            }
                        }
                        else if (type == "BD")
                        {
                            Helper.common.mlogin mlog = new Helper.common.mlogin();
                            DataTable empdt = obj.EmployeeDetails(Convert.ToString(Session["EMPCODE"]));
                            DataTable dlt = obj.EmployeeLogin(Convert.ToString(Session["EMPCODE"]), Convert.ToString(empdt.Rows[0]["RequestFormPwd"]));
                            if (dlt != null)
                            {
                                if (dlt.Rows.Count > 0)
                                {
                                    mlog.usernmae = dlt.Rows[0]["USER"].ToString().Trim();
                                    mlog.role = dlt.Rows[0]["RoleCode"].ToString().Trim();
                                    mlog.employeeid = int.Parse(dlt.Rows[0]["employeeid"].ToString().Trim());
                                    mlog.name = dlt.Rows[0]["Name"].ToString().Trim();
                                    Session["USER_BD"] = mlog;
                                    Session["Lang"] = dlt.Rows[0]["LangCode"].ToString().Trim();
                                    Session["Role"] = dlt.Rows[0]["RoleCode"].ToString().Trim();

                                    if (mlog.role.ToString() == "E")
                                    {
                                        LoginEntry(Userid);
                                        Session["User"] = Userid.Trim();
                                        Response.Redirect("index2.aspx");
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                                    return;
                                }
                                Session["Time"] = DateTime.Now.ToString();
                            }
                        }
                        else if (type == "Operator")
                        {
                            Session["User"] = Userid.Trim();
                            LoginEntry(Userid);
                            Response.Redirect("HMILsmsSummary.aspx");
                        }
                        else
                        {
                            Session["User"] = Userid.Trim();
                            LoginEntry(Userid);
                            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
                                Response.Redirect("index2.aspx");
                            else
                                Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
                        }
                    }
                    else
                    {
                        divOTP.Visible = true;
                        Random rand = new Random();
                        string vOTP = rand.Next(100000, 999999).ToString();
                        Session["vOTP"] = vOTP;
                        if (dt.Rows.Count > 0)
                        {
                            string Country_Code = dt.Rows[0]["COUNTRYCODE"].ToString();
                            string MobileNo = dt.Rows[0]["MOBILE1"].ToString();
                            string XXNo = "XXXXXX";
                            string last4 = MobileNo.Substring(MobileNo.Length - 4, 4);
                            lblSentOtpMobNo.Text = XXNo + last4;

                            string LinkextUserId = dt.Rows[0]["username"].ToString();
                            string pwd = dt.Rows[0]["APIKEY"].ToString();
                            string APIKEY = dt.Rows[0]["APIKEY"].ToString();
                            if (LinkextUserId.ToUpper() == "MIM2200812")
                            {
                                LinkextUserId = "MIM2200838";
                                pwd = "AbcAcc123";
                                APIKEY = "AbcAcc123";
                            }
                            string SENDERID = dt.Rows[0]["Login_OTP_Sender_ID"].ToString().Trim();
                            string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                            string loginOTPType = dt.Rows[0]["Login_OTP_SMSWABA"].ToString();

                            string peid = dt.Rows[0]["peid"].ToString();
                            string TemplateId = dt.Rows[0]["Login_OTP_Template_ID"].ToString();
                            DataTable dtSms = ob.GetDataLoginSMSFortemplate(LinkextUserId, TemplateId);

                            if (loginOTPType == "S")
                            {
                                if (dtSms.Rows.Count > 0)
                                {
                                    msg = dtSms.Rows[0]["template"].ToString();
                                }
                                msg = msg.ToString().Replace("{#var#}", vOTP);
                                msg = msg.ToString().Replace("#var1", vOTP);
                                if (Country_Code == "91")
                                {
                                    ob.Insert_MSGTRAN_91M(Userid, msg, MobileNo, SENDERID, peid, TemplateId);
                                }
                                else if (Country_Code == "971")
                                {
                                    string defaultSMPPacID = ob.getDefaultSMPPAccountId(Country_Code);//from tblcontry
                                    string trantable = ob.GetTranTableName(defaultSMPPacID);// SmppSetting
                                    defaultSMPPacID = defaultSMPPacID + "01";
                                    ob.InsertintoTranTable(trantable, defaultSMPPacID, Userid, msg, MobileNo, SENDERID, peid, TemplateId);
                                }
                                //ob.SendOTPthroughAPI(Country_Code, MobileNo, LinkextUserId, pwd, SENDERID, msg, peid, TemplateId, vOTP);
                            }
                            else
                            {
                                msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                                ob.WABA_MIM_Media_Template_LoginOTP_Message_Gupshup(MobileNo, LinkextUserId, APIKEY, msg, vOTP);
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP sent successfully please check..');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                        }
                    }
                }
                else
                {
                    Session["User"] = null;
                    Session["UserType"] = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential.');", true);
                }
            }
            else
            {
                Session["User"] = null;
                Session["UserType"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential.');", true);
            }
        }

        protected void btnOTPSubmit_Click(object sender, EventArgs e)
        {
            string SentOTP = Session["vOTP"].ToString();
            string EnterOTP = txtOTPEnter.Text.ToString().Trim();
            if (EnterOTP == SentOTP)
            {
                if (txtUserID.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
                    return;
                }
                if (txtPassword.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
                    return;
                }

                Helper.Util h1 = new Helper.Util();
                DataTable dt = h1.GetLoginDetails(txtUserID.Text.Trim(), txtPassword.Text.Trim());
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                    return;
                }
                if (dt.Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                    return;
                }
                //Previous Password Check Start
                //string type = ob.CheckValidUser(Userid.Trim(), Password.Trim());
                string type = Convert.ToString(dt.Rows[0]["usertype"]);

                // 2. User ID creation..
                //string type = ob.CheckValidUser(txtUserID.Text.Trim(), txtPassword.Text.Trim());

                if (type != "")
                {
                    //dt = ob.GetUserParameter(txtUserID.Text.Trim());
                    if (Convert.ToBoolean(dt.Rows[0]["ACTIVE"]) == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account deactivated. Please contact administrator.');", true);
                        return;
                    }
                    database.ExecuteNonQuery("UPDATE LoginEntry SET SENT_OTP_VERIFY=CURRENT_TIMESTAMP WHERE UserId='" + txtUserID.Text.ToString().Trim() + "'");
                    Session["WABARCS"] = dt.Rows[0]["WABARCS"].ToString().ToUpper();
                    Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
                    Session["URLBALANCE"] = dt.Rows[0]["noofurl"].ToString();
                    Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();
                    Session["MakerCheckerType"] = Convert.ToString(dt.Rows[0]["MakerCheckerType"]);  // Add By Vikas On 02_07_2024

                    DataTable dtRate = database.GetDataTable("SELECT * FROM smsrateaspercountry WHERE username='" + dt.Rows[0]["UserName"].ToString() + "' and countrycode='" + dt.Rows[0]["defaultCountry"].ToString() + "'");
                    if (dtRate.Rows.Count > 0)
                    {
                        Session["URLRATE"] = dtRate.Rows[0]["urlrate"].ToString();
                        Session["DLTCHARGE"] = dtRate.Rows[0]["dltcharge"].ToString();
                        Session["RATE_NORMALSMS"] = dtRate.Rows[0]["rate_normalsms"].ToString();
                        Session["RATE_SMARTSMS"] = dtRate.Rows[0]["rate_smartsms"].ToString();
                        Session["RATE_CAMPAIGN"] = dtRate.Rows[0]["rate_campaign"].ToString();
                        Session["RATE_OTP"] = dtRate.Rows[0]["rate_otp"].ToString();
                    }
                    else
                    {
                        Session["URLRATE"] = dt.Rows[0]["urlrate"].ToString();
                        Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                        Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                        Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                        Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                        Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();
                    }
                    //Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                    //Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                    //Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                    //Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                    //Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();

                    Session["DLT"] = dt.Rows[0]["DLTNO"].ToString();
                    Session["EMPCODE"] = dt.Rows[0]["EMPCODE"].ToString();
                    Session["PEID"] = dt.Rows[0]["PEID"].ToString();
                    Session["MOBILE"] = dt.Rows[0]["MOBILE1"].ToString();
                    Session["SENDERID"] = dt.Rows[0]["SENDERID"].ToString();
                    Session["DOMAINNAME"] = dt.Rows[0]["DOMAINNAME"].ToString();
                    Session["SHOWSMSDLR"] = dt.Rows[0]["showSMSDlr"].ToString();
                    Session["CANSENDSMS"] = dt.Rows[0]["cansendsms"].ToString();
                    Session["SHOWMOBILEXXXX"] = dt.Rows[0]["showmobilexxxx"].ToString();
                    Session["DEFAULTCOUNTRYCODE"] = dt.Rows[0]["defaultCountry"].ToString();
                    Session["FullName"] = dt.Rows[0]["FullName"].ToString();
                    Session["WabaProfileId"] = dt.Rows[0]["WabaProfileId"].ToString();
                    Session["WabaPwd"] = dt.Rows[0]["WabaPwd"].ToString();
                    Session["ShowDNDInSummary"] = dt.Rows[0]["ShowDNDInSummary"].ToString();

                    Helper.Global.SHOWMOBILEXXXX = dt.Rows[0]["showmobilexxxx"].ToString().ToUpper();
                    DataTable dt1 = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
                    Global.addMinutes = dt1.Rows.Count == 0 ? 0 : Convert.ToInt16(dt1.Rows[0]["timedifferenceInMinute"]);
                    Session["timedifferenceInMinute"] = Global.addMinutes;

                    if (Session["DOMAINNAME"] == null || Convert.ToString(Session["DOMAINNAME"]) == "")
                        Session["DOMAINNAME"] = ConfigurationManager.AppSettings["WebSite"].ToString();
                    Session.Timeout = 30;
                    Session["UserType"] = type;
                    Session["CURRENCY"] = Convert.ToString(dt1.Rows[0]["CURRENCY"]);
                    Session["mobLength"] = Convert.ToString(dt1.Rows[0]["mobLength"]);
                    Session["MobMIN"] = Convert.ToString(dt1.Rows[0]["MobMIN"]);
                    Session["MobMAX"] = Convert.ToString(dt1.Rows[0]["MobMAX"]);
                    Session["SUBCURRENCY"] = Convert.ToString(dt1.Rows[0]["SUBCURRENCY"]);
                    Session["maximgfilesize"] = Convert.ToString(dt1.Rows[0]["maximgfilesize"]);
                    Session["maxvideofilesize"] = Convert.ToString(dt1.Rows[0]["maxvideofilesize"]);
                    Session["Notice"] = Convert.ToString(dt1.Rows[0]["NoticeMsg"]);

                    DataTable DTSMPPAC = ob.GetUserSMPPACCOUNT(dt.Rows[0]["DLTNO"].ToString(), txtUserID.Text.Trim());
                    Session["DTSMPPAC"] = DTSMPPAC;

                    Session["ISIntermediate"] = "NO";

                    string uid = HttpUtility.UrlEncode(ob.Encrypt(txtUserID.Text.Trim()));
                    string pwd = HttpUtility.UrlEncode(ob.Encrypt(txtPassword.Text.Trim()));
                    string subDomain = Convert.ToString(dt1.Rows[0]["subDomain"]);
#if DEBUG
                    subDomain = "http://localhost:54751/";
#endif
                    if (type == "USER")
                    {
                        ob.DropUserTmpTable(txtUserID.Text.Trim());

                        Session["UserID"] = txtUserID.Text.Trim();
                        LoginEntry(txtUserID.Text.Trim());
                        if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
                            Response.Redirect("index_u2.aspx", false);
                        else
#if DEBUG
                            Response.Redirect("index_u2.aspx");
#else
                            Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
#endif
                    }
                    else if (type == "BD")
                    {
                        Helper.common.mlogin mlog = new Helper.common.mlogin();
                        DataTable empdt = obj.EmployeeDetails(Convert.ToString(Session["EMPCODE"]));
                        DataTable dlt = obj.EmployeeLogin(Convert.ToString(Session["EMPCODE"]), Convert.ToString(empdt.Rows[0]["RequestFormPwd"]));
                        if (dlt != null)
                        {
                            if (dlt.Rows.Count > 0)
                            {
                                mlog.usernmae = dlt.Rows[0]["USER"].ToString().Trim();
                                mlog.role = dlt.Rows[0]["RoleCode"].ToString().Trim();
                                mlog.employeeid = int.Parse(dlt.Rows[0]["employeeid"].ToString().Trim());
                                mlog.name = dlt.Rows[0]["Name"].ToString().Trim();
                                Session["USER_BD"] = mlog;
                                Session["Lang"] = dlt.Rows[0]["LangCode"].ToString().Trim();
                                Session["Role"] = dlt.Rows[0]["RoleCode"].ToString().Trim();

                                if (mlog.role.ToString() == "E")
                                {
                                    Session["User"] = txtUserID.Text.Trim();
                                    LoginEntry(txtUserID.Text.Trim());
                                    Response.Redirect("index2.aspx");
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                                return;
                            }
                            Session["Time"] = DateTime.Now.ToString();
                        }
                    }
                    else
                    {
                        Session["User"] = txtUserID.Text.Trim();
                        LoginEntry(txtUserID.Text.Trim());
                        if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91")
                            Response.Redirect("index2.aspx");
                        else
                            Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
                    }
                }
                else
                {
                    Session["User"] = null;
                    Session["UserType"] = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('otp is invalid please enter valid otp.');", true);
            }
        }

        protected void btnOTPResent_Click(object sender, EventArgs e)
        {
            divOTP.Visible = true;
            Random rand = new Random();
            string vOTP = rand.Next(100000, 999999).ToString();
            Session["vOTP"] = vOTP;
            DataTable dt = ob.GetUserParameter(txtUserID.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                string Country_Code = dt.Rows[0]["COUNTRYCODE"].ToString();
                string MobileNo = dt.Rows[0]["MOBILE1"].ToString();
                string XXNo = "XXXXXX";
                string last4 = MobileNo.Substring(MobileNo.Length - 4, 4);
                lblSentOtpMobNo.Text = XXNo + last4;
                string LinkextUserId = dt.Rows[0]["username"].ToString();
                string pwd = dt.Rows[0]["OLDAPIKEY"].ToString();
                string SENDERID = dt.Rows[0]["SENDERID"].ToString();
                string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                string loginOTPType = dt.Rows[0]["Login_OTP_SMSWABA"].ToString();
                string APIKEY = dt.Rows[0]["APIKEY"].ToString();
                string peid = dt.Rows[0]["peid"].ToString();
                string TemplateId = dt.Rows[0]["Login_OTP_Template_ID"].ToString();
                if (loginOTPType == "S")
                {
                    if (Country_Code == "91")
                    {
                        DataTable dtSms = ob.GetDataLoginSMSFortemplate(LinkextUserId, TemplateId);
                        msg = dtSms.Rows[0]["template"].ToString();
                    }
                    msg = msg.ToString().Replace("{#var#}", vOTP);
                    msg = msg.ToString().Replace("#var1", vOTP);
                    ob.SendOTPthroughAPI(Country_Code, MobileNo, LinkextUserId, pwd, SENDERID, msg, peid, TemplateId, vOTP);
                }
                else
                {
                    msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                    ob.WABA_MIM_Media_Template_LoginOTP_Message_Gupshup(MobileNo, LinkextUserId, APIKEY, msg, vOTP);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP RESent successfully please check..');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            string password = "";
            Session["AfterYesLogin"] = "Yes";
            if (txtUserID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
                return;
            }
            if (Convert.ToString(Session["PassWord"]) != "")
            {
                password = Convert.ToString(Session["PassWord"]);
            }
            else if (txtPassword.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
                return;
            }
            else
            {
                password = txtPassword.Text.ToString().Trim();
            }

            LoginUserDetails(txtUserID.Text.Trim(), password);
        }

        public void LoginEntry(string Userid)
        {
            Session["Login_Session_Id"] = Guid.NewGuid().ToString();
            int Exists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) from LoginEntry with(nolock) where UserId='" + Userid.Trim() + "'"));
            if (Exists == 1)
            {
                ob.UpdateLastLoginDate(Convert.ToString(Userid.Trim()));
                Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='1',LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE UserId='" + Userid.Trim() + "'");
            }
            else
            {
                ob.InsertCredentialsInLoginEntry(Convert.ToString(Userid.Trim()));
                Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='1',LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE UserId='" + Userid.Trim() + "'");
            }
        }

        //SHISHIR START
        protected void btnYes_Click(object sender, EventArgs e)
        {
            string GetEmailSent = ob.GetEmailSent(Convert.ToString(Session["Username"]));
            if (GetEmailSent == "")
            {
                GetEmailSent = "1900-01-01";
            }
            var EmailSent = Convert.ToDateTime(GetEmailSent);
            var CurrentLoginTime = DateTime.Now;
            var EmailSentTimediff = CurrentLoginTime - EmailSent;
            int EmailSentTdiffDays = Convert.ToInt32(EmailSentTimediff.TotalDays);

            if (EmailSentTdiffDays > 0)
            {
                string Host = ConfigurationManager.AppSettings["Host"].ToString();
                string MailFrom = ConfigurationManager.AppSettings["FromMail"].ToString();
                string PWD = ConfigurationManager.AppSettings["PWD"].ToString();
                string Subj = ConfigurationManager.AppSettings["Subject"].ToString();
                string Body = ConfigurationManager.AppSettings["Body"].ToString();
                string ToAdress = ConfigurationManager.AppSettings["ToMailId"].ToString();
                string CCMailId = ConfigurationManager.AppSettings["CCMailId"].ToString();

                ob.SendEmaillogin(ToAdress, Subj, Body + Convert.ToString(Session["Username"]), MailFrom, PWD, Host, CCMailId);
                ob.UpdateEmailSent(Convert.ToString(Session["Username"]));

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Thanks, One of Our Executive will Contact you to enable your Account. In case of any Urgency, you May reach us on Email : support@myinboxmedia.com or you can Contact us : 011-46714074.');", true); /*One of MyInboxMedia Executive will Contact you to enable your Account.*/
            return;
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "HideModal1()", true);
            return;
        }
        //SHISHIR END


    }
}