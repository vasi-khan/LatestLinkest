using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Intermediate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Helper.Util hl = new Helper.Util();
            Helper.Util ob = new Helper.Util();

            string username = ob.Decrypt(HttpUtility.UrlDecode(Request.QueryString["abc"]));
            string pwd = ob.Decrypt(HttpUtility.UrlDecode(Request.QueryString["pqr"]));

            DataTable dt = hl.GetLoginDetails(username, pwd);
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

            //string type = ob.CheckValidUser(username, pwd);

            if (type != "")
            {
                //DataTable dt = ob.GetUserParameter(username);
                Session["WABARCS"] = dt.Rows[0]["WABARCS"].ToString().ToUpper();
                Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
                Session["URLBALANCE"] = dt.Rows[0]["noofurl"].ToString();
                Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();

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
                //Session["URLRATE"] = dt.Rows[0]["urlrate"].ToString();
                //Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                //Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                //Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                //Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                //Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();

                Session["DLT"] = dt.Rows[0]["DLTNO"].ToString();
                Session["PEID"] = dt.Rows[0]["PEID"].ToString();
                Session["MOBILE"] = dt.Rows[0]["MOBILE1"].ToString();
                Session["SENDERID"] = dt.Rows[0]["SENDERID"].ToString();
                Session["DOMAINNAME"] = dt.Rows[0]["DOMAINNAME"].ToString();
                Session["SHOWSMSDLR"] = dt.Rows[0]["showSMSDlr"].ToString();
                Session["CANSENDSMS"] = dt.Rows[0]["cansendsms"].ToString();
                Session["SHOWMOBILEXXXX"] = dt.Rows[0]["showmobilexxxx"].ToString();
                Session["DEFAULTCOUNTRYCODE"] = dt.Rows[0]["defaultCountry"].ToString();

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
                Session["SUBCURRENCY"] = Convert.ToString(dt1.Rows[0]["SUBCURRENCY"]);
                Session["ISIntermediate"] = "YES";
                Session["maximgfilesize"] = Convert.ToString(dt1.Rows[0]["maximgfilesize"]);
                Session["maxvideofilesize"] = Convert.ToString(dt1.Rows[0]["maxvideofilesize"]);
                Session["Notice"] = Convert.ToString(dt1.Rows[0]["NoticeMsg"]);

                DataTable DTSMPPAC = ob.GetUserSMPPACCOUNT(dt.Rows[0]["DLTNO"].ToString(), username);
                Session["DTSMPPAC"] = DTSMPPAC;
                string subDomain = Convert.ToString(dt1.Rows[0]["subDomain"]);
                //subDomain = "http://localhost:54751/";
                if (type == "USER")
                {
                    Session["UserID"] = username;
                    Response.Redirect(string.Format("{0}/index_u2.aspx", subDomain));
                }
                else
                {
                    Session["User"] = username;
                    Response.Redirect(string.Format("{0}/index2.aspx", subDomain));                     
                }
            }
            else
            {
                Session["User"] = null;
                Session["UserType"] = null;
            }
        }
    }
}