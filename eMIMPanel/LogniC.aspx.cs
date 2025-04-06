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
    public partial class LogniC : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUrl.Text = Request.Url.Authority.ToUpper();
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
            else { //Response.Redirect("~/IncorrectURL.html"); 
            }
            //Session["UserID"] = null;
            //Session["User"] = null;
            //Session.Abandon();
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

            // 2. User ID creation..
            string type = ob.CheckValidUser(txtUserID.Text.Trim(), txtPassword.Text.Trim());
            if (type != "")
            {
                DataTable dt = ob.GetUserParameter(txtUserID.Text.Trim());
                if(Convert.ToBoolean(dt.Rows[0]["ACTIVE"])==false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account deactivated. Please contact administrator.');", true);
                    return;
                }

                Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
                Session["URLBALANCE"] = dt.Rows[0]["noofurl"].ToString();
                Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();
                Session["URLRATE"] = dt.Rows[0]["urlrate"].ToString();
                Session["DLTCHARGE"] = dt.Rows[0]["DLTCHARGE"].ToString();
                Session["RATE_NORMALSMS"] = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                Session["RATE_SMARTSMS"] = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                Session["RATE_CAMPAIGN"] = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                Session["RATE_OTP"] = dt.Rows[0]["RATE_OTP"].ToString();
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

                if (Session["DOMAINNAME"] == null || Convert.ToString(Session["DOMAINNAME"])=="")
                    Session["DOMAINNAME"] = ConfigurationManager.AppSettings["WebSite"].ToString();
                Session.Timeout = 30;
                Session["UserType"] = type;

                DataTable DTSMPPAC = ob.GetUserSMPPACCOUNT(dt.Rows[0]["DLTNO"].ToString(), txtUserID.Text.Trim());
                Session["DTSMPPAC"] = DTSMPPAC;

                if (type == "USER")
                {
                    Session["UserID"] = txtUserID.Text.Trim();
                    Response.Redirect("index_u2.aspx");
                }
                else
                {
                    Session["User"] = txtUserID.Text.Trim();
                    Response.Redirect("index2.aspx");
                }
            }
            else
            {
                Session["User"] = null;
                Session["UserType"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential.');", true);
            }
        }
    }
}