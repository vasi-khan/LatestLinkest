using eMIMPanel.Helper;
using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class sms_whatsapp_u : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx");

            string usr = Session["UserID"].ToString();

            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();

                PopulateCountry();
                ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                ddlCCodeSelected();
                string b = ob.GetURLbal(Session["UserID"].ToString());

                Session["URLBALANCE"] = b;
                //lblSMS.Text = "This is ASPX Page";
            }
        }

        public void PopulateCountry()
        {
            Util ob = new Util();
            DataTable dt = ob.GetActiveCountry(Convert.ToString(Session["UserID"]));
            ddlCCode.DataSource = dt;
            ddlCCode.DataTextField = "name";
            ddlCCode.DataValueField = "countrycode";
            ddlCCode.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("sms-whatsapp_u.aspx");
        }
        protected void btnOTPSubmit_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }

            if (txtOTP.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter the received OTP.');", true);
                return;
            }
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91" && txtMobile.Text.Trim().Length < 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter 10 Digit Mobile Number.');", true);
                return;
            }
            string UserID = Convert.ToString(Session["UserID"]);

            string mob = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) + txtMobile.Text;
            string otp = txtOTP.Text;
            //string mob = txtMobile.Text;

            string s = ob.OTPVerifyAndUpdate(mob, otp);
            if (s == "InvalidOTP")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Incorrect OTP Entered.');", true);
                return;
            }
            if (s == "Already" || s == "Y")
            {
                divVerified.Attributes.Add("Style", "display:block;");
                divOTP.Attributes.Add("Style", "display:none;");
                //show below div
                divBelow.Attributes.Add("Style", "display:block;");
            }

        }

        protected void btnResendOTP_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91" && txtMobile.Text.Trim().Length < 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter 10 Digit Mobile Number.');", true);
                return;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            string cc = ddlCCode.SelectedValue;
            string mob = cc + txtMobile.Text;
            string senderid = "";
            if (cc != "91") senderid = ddlSender.SelectedValue;
            Helper.Util ob = new Helper.Util();

            ob.ReSendOTP(mob, cc, senderid, UserID);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP sent successfully.');", true);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            //if (Convert.ToInt64(Session["URLBALANCE"]) <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL Balance expired.');", true);
            //    return;
            //}

            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            //if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "91" && txtMobile.Text.Trim().Length < 10)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter 10 Digit Mobile Number.');", true);
            //    return;
            //}
            string UserID = Convert.ToString(Session["UserID"]);
            string cc = ddlCCode.SelectedValue;
            string mob = cc + txtMobile.Text;
            string senderid = "";
            if (cc != "91") senderid = ddlSender.SelectedValue;
            string s = ob.CheckAndSendOTP(mob, UserID, cc,senderid);
            if (s == "Already")
            {
                divVerified.Attributes.Add("Style", "display:block;");
                //show below div.
                divBelow.Attributes.Add("Style", "display:block;");
            }
            if (s == "OTPAlready")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP Already Sent To Entered Mobile Number.');", true);
                divOTP.Attributes.Add("Style", "display:block;");
                divReSendOTP.Attributes.Add("Style", "display:block;");

                return;
            }
            if (s == "Start")
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter the received OTP.');", true);
                divOTP.Attributes.Add("Style", "display:block;");
            }


        }
        protected void btnSMSUrl_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message.');", true);
                return;
            }
            string mob = txtMobile.Text;
            string msg = txtMsg.Text.Trim();
            //string m = "SMSTO:" + mob + "&body=" + msg;
            string m = "sms:" + mob + "?body=" + msg;
            SMSWSURL(m);
        }

        protected void SMSWSURL(string m)
        {
            Helper.Util ob = new Helper.Util();

            string bal = "";// ob.GetURLbal(Convert.ToString(Session["UserID"]));
                            //Session["URLBALANCE"] = bal;
                            //if (Convert.ToInt64(Session["URLBALANCE"]) <= 0)
                            //{
                            //    Session["URLBALANCE"] = "0";
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Small URL balance expired.');", true);
                            //    return;
                            //}
                            //else
                            //{
            string UserID = Convert.ToString(Session["UserID"]);
            //if specific user has already the long url, then don't reduce balance -
            using (var ctx = new ShortnrContext())
            {
                //var recordCount = ctx.ShortUrls.Count(u => u.LongUrl == m && u.UserID == UserID);
                int recordCount = ob.ShortUrls_Count(m, UserID);
                if (Convert.ToInt16(recordCount) >= 1)
                {
                    string surl = ob.GetExistingShortURL(m, UserID);
                    // ctx.ShortUrls.Where(u => u.LongUrl == m && u.UserID == UserID).FirstOrDefault();
                    if (surl != null)
                    {
                        //qr.ShortURL =    string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"), surl.Segment);
                        //lblShortURL.Text = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", surl.Segment);
                        lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + surl;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);
                    }
                }
                else
                {
                    if (txtShortURL.Text.Trim() != "")
                    {
                        if (txtShortURL.Text.Length == 8)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL Word cannot be of 8 characters. It can be more or less than 8.');", true);
                            return;
                        }
                    }
                    //ShortUrl shortUrl = await this._urlManager.ShortenUrl(UserID, m, Request.UserHostAddress, qr.CustomSegment, "N");
                    //qr.ShortURL = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"), shortUrl.Segment);

                    //UrlManager ob2 = new UrlManager();
                    //string mobTrk = "Y"; // ob.GetMobTrkOfUser(UserID);

                    //string sUrl = ob2.ShortenURL1(UserID, m, Request.UserHostAddress, txtShortURL.Text, mobTrk);
                    string sUrl = "";
                    if (txtShortURL.Text.Trim() != "")
                        sUrl = txtShortURL.Text.Trim();
                    else
                        sUrl = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString());
                    ob.SaveShortURL(UserID, m, Request.UserHostAddress, sUrl, "N", "N", Session["DOMAINNAME"].ToString());

                    lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + sUrl;

                    //bal = ob.UdateAndGetURLbal(Convert.ToString(Session["UserID"]));
                    //Session["URLBALANCE"] = bal;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);
                }
            }
            // }

        }
        protected void btnWAUrl_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message.');", true);
                return;
            }
            string cc = ddlCCode.SelectedValue;
            string mob = cc + txtMobile.Text;
            string msg = txtMsg.Text.Trim();

            // string m = "https://api.whatsapp.com/send?phone=+91" + mob + "&text=" + msg;
            string m = "https://api.whatsapp.com/send?phone=" + mob + "&text=" + msg;// + "&lang= en";

            SMSWSURL(m);
        }
        protected void btnSMSQR_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message.');", true);
                return;
            }
            string mob = txtMobile.Text;
            string msg = txtMsg.Text.Trim();
            //string m = "SMSTO:" + mob + "&body=" + msg;
            string m = "sms:" + mob + "?body=" + msg;
            SMSWSQR(m);
            lblQR4.Text = "SMS";
        }
        protected void SMSWSQR(string url)
        {
            Helper.Util ob = new Helper.Util();
            Bitmap qrcd = ob.convertStringtoBitMap(Convert.ToString(url), 2);
            byte[] qc;
            using (MemoryStream stream2 = new MemoryStream())
            {
                //Bitmap resized = new Bitmap(QRCodeImage, new Size(QRCodeImage.Width * 2, QRCodeImage.Height * 2));
                qrcd.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                qc = stream2.ToArray();
            }

            byte[] Qcode = qc;
            string Qstring = Convert.ToBase64String(qc, 0, qc.Length);
            string imgDataURL = string.Format("data:image/png;base64,{0}", Qstring);
            imgQR.ImageUrl = imgDataURL;
        }
        protected void btnWAQR_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number.');", true);
                return;
            }
            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message.');", true);
                return;
            }
            string cc = ddlCCode.SelectedValue;
            string mob = cc + txtMobile.Text;
            string msg = txtMsg.Text.Trim();

            // string m = "https://api.whatsapp.com/send?phone=+91" + mob + "&text=" + msg;
            string m = "https://api.whatsapp.com/send?phone=" + mob + "&text=" + msg;

            SMSWSQR(m);
            lblQR4.Text = "WhastApp";
        }

        protected void btnDwQR_Click(object sender, EventArgs e)
        {
            //UpdateProgress1.Visible = false;
            string base64 = imgQR.ImageUrl.Split(',')[1];
            Session["IMGDW"] = base64;
            Response.Redirect("download-img.aspx");
            //Server.Transfer("download-img.aspx");
            return;
            //string filePath = "~/QRTemp/QRCode.png" ;
            //if (File.Exists(Server.MapPath(filePath))) File.Delete(Server.MapPath(filePath));
            //File.WriteAllBytes(Server.MapPath(filePath), bytes);

            //string url = ResolveUrl("~\\QRTemp\\QRCode.png");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);

            /*
            Response.Clear();
            Response.ContentType = "image/png";
            Response.AddHeader("Content-Disposition", "attachment; filename=QRCode.png");
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            Response.End();
            */
        }

        protected void ddlCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCCodeSelected();
        }

        public void ddlCCodeSelected()
        {
            if (Convert.ToString(ddlCCode.SelectedValue) == "91")
            {
                divSender.Visible = false;
            }
            else
            {
                divSender.Visible = true;
                PopulateSender();
            }
        }
        public void PopulateSender()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["UserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count == 0)
            {
                ddlSender.Items.Clear();
                return;
            }

            ddlSender.DataSource = dt;
            ddlSender.DataTextField = "senderid";
            ddlSender.DataValueField = "senderid";
            ddlSender.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlSender.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlSender.SelectedIndex = 1;
            else
                ddlSender.SelectedIndex = 0;
        }
    }
}