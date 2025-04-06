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
    public partial class rich_media_url_u : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    lblFileSize.Text = Convert.ToString(Session["maxvideofilesize"]);

                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    int sizeImg = Convert.ToInt16(Session["maximgfilesize"]);
                    int sizeVideo = Convert.ToInt16(Session["maxvideofilesize"]);

                    if (rdbVideo.Checked)
                        if (FileUpload1.PostedFile.ContentLength > (sizeVideo * 1024 * 1024))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Video file size cannot be more than " + sizeVideo + " MB');", true);
                            lblUploading.Text = "Upload rejected.";
                            return;
                        }
                    if (rdbImg.Checked)
                        if (FileUpload1.PostedFile.ContentLength > (sizeImg * 1024 * 1024))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Image file size cannot be more than " + sizeImg + " MB');", true);
                            lblUploading.Text = "Upload rejected.";
                            return;
                        }
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (FileName != "")
                    {
                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string en = Extension.ToUpper();

                        if (rdbImg.Checked)
                        {
                            if (!(en.Contains("JPG") || en.Contains("JPEG") || en.Contains("PNG") || en.Contains("GIF") || en.Contains("TIFF") || en.Contains("BMP") || en.Contains("JFIF")))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload image file of type ( JPG / JPEG / PNG / GIF / TIFF / BMP / JFIF )');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }
                        }

                        if (rdbVideo.Checked)
                        {
                            if (!(en.Contains("MP4") || en.Contains("MOV")))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }
                        }
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
            try
            {
                int x = 0;
                if (chkCall.Checked) x++;
                if (chkVisit.Checked) x++;
                if (chkWA.Checked) x++;
                if (chkFB.Checked) x++;
                if (x > 2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can choose maximum of any two buttons. Please uncheck the extra button.');", true);
                    return;
                }

                if (txtPageName.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Reference Page Name.');", true);
                    return;
                }

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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Keyword already exists. Please enter a new keyword and try again.');", true);

                        txtShortURL.Focus();
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

                if (chkWA.Checked)
                {
                    if (txtWAButtonName.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Whatsapp button name cannot be blank.');", true);
                        return;
                    }
                    if (txtWA.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Whatsapp number cannot be blank.');", true);
                        return;
                    }
                }

                if (chkFB.Checked)
                {
                    if (txtFBButtonName.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Facebook button name cannot be blank.');", true);
                        return;
                    }
                    if (txtFB.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Facebook page name cannot be blank.');", true);
                        return;
                    }
                }

                if (Session["UPLOADMEDIA"] == null)
                {
                    if (rdbImg.Checked)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload image file.');", true);

                    if (rdbVideo.Checked)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file.');", true);
                    return;
                }
                if (chkLogo.Checked && Session["UPLOADMEDIALOGO"] == null)
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

                if (rdbVideo.Checked)
                {
                    strFile = strFile.Replace("###MEIDAFILE###", "../MediaUpload/" + Convert.ToString(Session["UPLOADMEDIAFN"]));
                    strFile = strFile.Replace("###VIDEODISPLAY###", "block");
                }
                else
                    strFile = strFile.Replace("###VIDEODISPLAY###", "none");

                if (rdbImg.Checked)
                {
                    strFile = strFile.Replace("###IMAGEFILE###", "../MediaUpload/" + Convert.ToString(Session["UPLOADMEDIAFN"]));
                    strFile = strFile.Replace("###IMAGEDISPLAY###", "block");
                }
                else
                    strFile = strFile.Replace("###IMAGEDISPLAY###", "none");

                if (chkLogo.Checked)
                {
                    strFile = strFile.Replace("###LOGODISPLAY###", "block");
                    strFile = strFile.Replace("###LOGOFILE###", "../MediaUpload/" + Convert.ToString(Session["UPLOADMEDIAFNLOGO"]));
                }
                else
                    strFile = strFile.Replace("###LOGODISPLAY###", "none");

                string protocol = GetProtocol(Convert.ToString(Session["DOMAINNAME"]));
                string RichMediaGroupRows = Convert.ToString(Guid.NewGuid());
                if (chkVisit.Checked)
                {
                    strFile = strFile.Replace("###VISITUSDISPLAY###", "block");
                    string shUrl = Guid.NewGuid().ToString().Replace("-", ""); shUrl = shUrl.Substring(shUrl.Length - 10, 9);
                    ob.SaveShortURL(UserID, txtVisitUs.Text.Trim(), Request.UserHostAddress, shUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), "", "N", txtVisitButtonName.Text.Trim(), txtPageName.Text.Trim(), RichMediaGroupRows);
                    string visit = protocol + Convert.ToString(Session["DOMAINNAME"]) + shUrl;
                    strFile = strFile.Replace("###VISITUS###", visit);
                    strFile = strFile.Replace("###VISITUSBUTTONTEXT###", txtVisitButtonName.Text.Trim());
                }
                else
                    strFile = strFile.Replace("###VISITUSDISPLAY###", "none");

                if (chkCall.Checked)
                {
                    strFile = strFile.Replace("###CALLUSDISPLAY###", "block");
                    string shUrl = Guid.NewGuid().ToString().Replace("-", ""); shUrl = shUrl.Substring(shUrl.Length - 10, 9);
                    string longURL = "tel:" + txtCallUs.Text.Trim();
                    ob.SaveShortURL(UserID, longURL, Request.UserHostAddress, shUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), "", "N", txtCallButtonName.Text.Trim(), txtPageName.Text.Trim(), RichMediaGroupRows);
                    string call = protocol + Convert.ToString(Session["DOMAINNAME"]) + shUrl;
                    strFile = strFile.Replace("###CALLUS###", call);
                    strFile = strFile.Replace("###CALLUSBUTTONTEXT###", txtCallButtonName.Text.Trim());
                }
                else
                    strFile = strFile.Replace("###CALLUSDISPLAY###", "none");

                if (chkWA.Checked)
                {
                    string cc = "+" + Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                    string mobileNo = txtWA.Text.Trim();
                    string shUrl = Guid.NewGuid().ToString().Replace("-", ""); shUrl = shUrl.Substring(shUrl.Length - 10, 9);
                    string longURL = "https://api.whatsapp.com/send?phone=" + mobileNo;
                    ob.SaveShortURL(UserID, longURL, Request.UserHostAddress, shUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), "", "N", txtWAButtonName.Text.Trim(), txtPageName.Text.Trim(), RichMediaGroupRows);
                    string wa = protocol + Convert.ToString(Session["DOMAINNAME"]) + shUrl;
                    strFile = strFile.Replace("###WHATSAPPDISPLAY###", "block");
                    strFile = strFile.Replace("###WHATSAPP###", wa);
                    strFile = strFile.Replace("###WHATSAPPBUTTONTEXT###", txtWAButtonName.Text.Trim());
                }
                else
                    strFile = strFile.Replace("###WHATSAPPDISPLAY###", "none");

                if (chkFB.Checked)
                {
                    strFile = strFile.Replace("###FACEBOOKDISPLAY###", "block");
                    string shUrl = Guid.NewGuid().ToString().Replace("-", ""); shUrl = shUrl.Substring(shUrl.Length - 10, 9);
                    string longURL = "https://www.facebook.com/" + txtFB.Text.Trim();
                    ob.SaveShortURL(UserID, longURL, Request.UserHostAddress, shUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), "", "N", txtFBButtonName.Text.Trim(), txtPageName.Text.Trim(), RichMediaGroupRows);
                    string fb = protocol + Convert.ToString(Session["DOMAINNAME"]) + shUrl;
                    strFile = strFile.Replace("###FBPAGE###", fb);
                    strFile = strFile.Replace("###FBBUTTONTEXT###", txtFBButtonName.Text.Trim());
                }
                else
                    strFile = strFile.Replace("###FACEBOOKDISPLAY###", "none");

                File.WriteAllText(destFilePath, strFile);
                string scheme = "";
                string domn = Session["DOMAINNAME"].ToString();
                if (!domn.ToLower().Contains("http")) scheme = "http://";
                string longurl = scheme + Session["DOMAINNAME"].ToString() + "MEDIAhtml/" + destfn;
                ob.SaveShortURL(UserID, longurl, Request.UserHostAddress, sUrl, "N", "N", Convert.ToString(Session["DOMAINNAME"]), "", "Y", "", "", RichMediaGroupRows);
                lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + sUrl;
                string bal2 = ob.UdateAndGetURLbal1(Convert.ToString(Session["UserID"]), sUrl);
                lblExp.Text = "The generated URL will expire on : " + DateTime.Now.AddDays(7).ToString("dd-MM-yyyy");
                Session["SMSBAL"] = bal2;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }
        public string GetProtocol(string domainname)
        {
            string retval = "";
            if (domainname.Contains("http") || domainname.Contains("https") || domainname.Contains("HTTP") || domainname.Contains("HTTPS"))
                retval = "";
            else
                retval = "https://";
            return retval;
        }
        protected void rdbVideo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbVideo.Checked)
                {
                    lblVI.Text = "Video";
                    lblFileSize.Text = Convert.ToString(Session["maxvideofilesize"]);
                }
                else
                {
                    lblVI.Text = "Image";
                    lblFileSize.Text = Convert.ToString(Session["maximgfilesize"]);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }
        protected void chkLogo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLogo.Checked)
                { divUploadLogo.Attributes.Add("style", "pointer-events:all;"); }
                else
                { divUploadLogo.Attributes.Add("style", "pointer-events:none;"); }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void chkButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int x = 0;
                if (chkCall.Checked) x++;
                if (chkVisit.Checked) x++;
                if (chkWA.Checked) x++;
                if (chkFB.Checked) x++;
                if (x > 2)
                {
                    ((CheckBox)sender).Checked = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can choose maximum of any two buttons. Two Buttons Already Selected !');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }
    }
}