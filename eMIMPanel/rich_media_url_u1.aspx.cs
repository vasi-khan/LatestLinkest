using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class rich_media_url_u1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    if(FileUpload1.PostedFile.ContentLength > (25 * 1024 * 1024))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Media file size cannot be above of 25 MB');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (FileName != "")
                    {
                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string FolderPath = "MEDIAUpload/";

                        Session["UPLOADFILENMEXT"] = Extension;
                        string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                        string FilePath = Server.MapPath(FolderPath + FN + Extension);
                        FileUpload1.SaveAs(FilePath);
                        Session["UPLOADMEDIA"] = FolderPath + FN + Extension;
                        Session["UPLOADMEDIAFN"] = FN + Extension;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                        lblUploading.Text = "Uploaded successfully.";
                    }
                }
                if (IsPostBack && FileUpload2.PostedFile != null)
                {
                    if (FileUpload2.PostedFile.ContentLength > (200 * 1024))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Brand Logo file size cannot be above of 200 KB');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    string FileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    if (FileName != "")
                    {
                        string Extension = Path.GetExtension(FileUpload2.PostedFile.FileName);
                        string FolderPath = "MEDIAUpload/";

                        Session["UPLOADFILENMEXTLOGO"] = Extension;
                        string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                        string FilePath = Server.MapPath(FolderPath + FN + Extension);
                        FileUpload2.SaveAs(FilePath);
                        Session["UPLOADMEDIALOGO"] = FolderPath + FN + Extension;
                        Session["UPLOADMEDIAFNLOGO"] = FN + Extension;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                        lblUploading2.Text = "Uploaded successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
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
            if (chkVisit.Checked)
            {
                if (txtVisitButtonName.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Visit us button name cannot be blank.');", true);
                    return;
                }
                if (txtVisitUs.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Visit us link cannot be blank.');", true);
                    return;
                }
            }

            if (chkCall.Checked)
            {
                if (txtCallButtonName.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Call us button name cannot be blank.');", true);
                    return;
                }
                if (txtCallUs.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Call us mobile number cannot be blank.');", true);
                    return;
                }
            }
            if (Session["UPLOADMEDIA"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload media.');", true);
                return;
            }
            if (Session["UPLOADMEDIALOGO"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload brand logo.');", true);
                return;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            double bal = Convert.ToDouble(Session["SMSBAL"]);
            double rate = Convert.ToDouble(Session["URLRATE"]);

            if (bal - rate <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient Balance. Cannot create Short URL.');", true);
                return;
            }

            string sUrl = "";
            if (txtShortURL.Text.Trim() != "")
                sUrl = txtShortURL.Text.Trim();
            else
                sUrl = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString()); // ob2.NewSegment();

            string destfn = Guid.NewGuid().ToString().Substring(0, 10) + ".html";
            string srcfn = "MediaHTML.html";
            string FolderPath = "MEDIAhtml/" + destfn;

            string srcFilePath = Server.MapPath(srcfn);
            string destFilePath = Server.MapPath(FolderPath);

            File.Copy(srcFilePath, destFilePath);
            string mediafn = Convert.ToString(Session["UPLOADMEDIA"]);
            String strFile = File.ReadAllText(destFilePath);
            strFile = strFile.Replace("###MEIDAFILE###", "../MediaUpload/" + Convert.ToString(Session["UPLOADMEDIAFN"]));
            strFile = strFile.Replace("###LOGOFILE###", "../MediaUpload/" + Convert.ToString(Session["UPLOADMEDIAFNLOGO"]));


            if (chkVisit.Checked)
            {
                strFile = strFile.Replace("###VISITUSDISPLAY###", "block");
                strFile = strFile.Replace("###VISITUS###", txtVisitUs.Text.Trim());
                strFile = strFile.Replace("###VISITUSBUTTONTEXT###", txtVisitButtonName.Text.Trim());
            }
            else
                strFile = strFile.Replace("###VISITUSDISPLAY###", "none");

            if (chkCall.Checked)
            {
                strFile = strFile.Replace("###CALLUSDISPLAY###", "block");
                strFile = strFile.Replace("###CALLUS###", txtCallUs.Text.Trim());
                strFile = strFile.Replace("###CALLUSBUTTONTEXT###", txtCallButtonName.Text.Trim());
            }
            else
                strFile = strFile.Replace("###CALLUSDISPLAY###", "none");


            File.WriteAllText(destFilePath, strFile);


            string longurl = Session["DOMAINNAME"].ToString() + "MEDIAhtml/" + destfn;
            ob.SaveShortURL(UserID, longurl, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]),"","Y");
            //ob2.ShortenURL1(UserID, txtLongURL.Text, Request.UserHostAddress, txtShortURL.Text, mobTrk);

            //lblShortURL.Text = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", sUrl);
            lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + sUrl;

            //ShortUrl shortUrl1 = await this._urlManager.ShortenUrl(UserID, url.LongURL, Request.UserHostAddress, shortUrl.Segment + "_Q", "N");
            //string bal2 = ob.UdateAndGetURLbal1(Convert.ToString(Session["UserID"]), sUrl);
            //lblURLbal.Text = bal;
            //Session["SMSBAL"] = bal2;

            string bal2 = ob.UdateAndGetURLbal1(Convert.ToString(Session["UserID"]), sUrl);
            //lblURLbal.Text = bal;
            Session["SMSBAL"] = bal2;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);

        }
    }
}