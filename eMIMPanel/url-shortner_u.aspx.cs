using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class url_shortner_u : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx");

            string usr = Session["UserID"].ToString();
            //lblUser.Text = usr;
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();

                string b = ob.GetURLbal(Session["UserID"].ToString());
                //lblURLbal.Text = b;
                Session["URLBALANCE"] = b;
                //lblSMS.Text = "This is ASPX Page";
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("urls.aspx");
        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(lblShortURL.Text);
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (txtLongURL.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Long URL.');", true);
                return;
            }

            if (!txtLongURL.Text.StartsWith("http://") && !txtLongURL.Text.StartsWith("https://"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Long URL.');", true);
                return;
            }
            if (txtShortURL.Text.Trim() != "")
            {
                if (txtShortURL.Text.Length == 8)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL Word cannot be of 8 characters. It can be more or less than 8.');", true);
                    return;
                }
                if (txtShortURL.Text.Length > 20 || !Regex.IsMatch(txtShortURL.Text, @"^[A-Za-z\d_-]+$"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Short URL is invalid.');", true);
                    return;
                }
                if (ob.CheckShortURLDuplicate(txtShortURL.Text, Session["DOMAINNAME"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Short URL is already in use.');", true);
                    return;
                }
            }


            string UserID = Convert.ToString(Session["UserID"]);
            double bal = Convert.ToDouble(Session["SMSBAL"]);
            double rate = Convert.ToDouble(Session["URLRATE"]);

            if (bal - rate <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient Balance. Cannot create Short URL.');", true);
                return;
            }

            using (var ctx = new ShortnrContext())
            {
                UrlManager ob2 = new UrlManager();
                //string mobTrk = ob.GetMobTrkOfUser(UserID);
                string sUrl = "";
                if (txtShortURL.Text.Trim() != "")
                    sUrl = txtShortURL.Text.Trim();
                else
                    sUrl = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString()); // ob2.NewSegment();


                // ---------------------ADDD-------------------------------//

                string segment = "";
                bool IsRichMedia = false;
                if (txtLongURL.Text.Trim().Contains(Convert.ToString(Session["DOMAINNAME"])))
                {
                    segment = txtLongURL.Text.Trim().Replace("//", "/").Split('/').Last();
                    IsRichMedia = ob.IsRichMediaURL(UserID, segment);
                }

                if (IsRichMedia)
                    ob.SaveShortURLRichMedia(UserID, txtLongURL.Text, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]));
                else
                    ob.SaveShortURL(UserID, txtLongURL.Text, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]));
                //------------------------END ------------------------------------//

                // ob.SaveShortURL(UserID, txtLongURL.Text, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"])); // comment by Rachit

                //ob2.ShortenURL1(UserID, txtLongURL.Text, Request.UserHostAddress, txtShortURL.Text, mobTrk);

                //lblShortURL.Text = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", sUrl);
                lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + sUrl;

                //ShortUrl shortUrl1 = await this._urlManager.ShortenUrl(UserID, url.LongURL, Request.UserHostAddress, shortUrl.Segment + "_Q", "N");
                string bal2 = ob.UdateAndGetURLbal1(Convert.ToString(Session["UserID"]), sUrl);
                //lblURLbal.Text = bal;
                Session["SMSBAL"] = bal2;

                lblExp.Text = DateTime.Now.AddDays(7).ToShortDateString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);

            }
        }
    }
}