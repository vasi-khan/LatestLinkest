using eMIMPanel.Helper;
using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class SMSLinkToWABA : System.Web.UI.Page
    {
        Util obj = new Util();
        string User = "", usertype = "", WabaProfileId = "", WabaPwd = "";
        string dbNameWABA = System.Configuration.ConfigurationManager.AppSettings["dbnameWABA"].ToString();
        string UploadPath = Convert.ToString(ConfigurationManager.AppSettings["FILEURL"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            User = Convert.ToString(Session["UserID"]);
            if (User == "") Response.Redirect("login.aspx");

            if (!Page.IsPostBack)
            {
                BindShortURL();
                PopulateTemplateID("");
                divuploadImage.Visible = false;

                WabaProfileId = Convert.ToString(Session["WabaProfileId"]);
                WabaPwd = Convert.ToString(Session["WabaPwd"]);
                if (WabaProfileId != "" && WabaPwd != "")
                {
                    lblMessage.Visible = false;
                    divShortURL.Visible = true;
                    divWABAtemplate.Visible = true;

                    DataTable dt = obj.GetShortURL(User);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        grdshorturl.DataSource = dt;
                        grdshorturl.DataBind();
                        GridFormat(dt);
                    }
                    else
                    {
                        dt = null;
                        grdshorturl.DataSource = dt;
                        grdshorturl.DataBind();
                    }
                    divgridviewdetails.Visible = true;
                }
                else
                {
                    divgridviewdetails.Visible = false;
                    lblMessage.Visible = true;
                    divShortURL.Visible = false;
                    divWABAtemplate.Visible = false;
                }
            }

            try
            {
                if (IsPostBack && uImage.PostedFile != null)
                {
                    string FileName = Path.GetFileName(uImage.PostedFile.FileName);
                    if (FileName != "")
                    {
                        Helper.Util ob = new Helper.Util();
                        string Extension = Path.GetExtension(uImage.PostedFile.FileName);
                        string en = Extension.ToUpper();
                        ViewState["Extension"] = en.ToString();
                        decimal size = Math.Round(((decimal)uImage.PostedFile.ContentLength / (decimal)1024), 2);
                        if (Extension.ToUpper() == ".MP4" || Extension.ToUpper() == ".MP3" || Extension.ToUpper() == ".OPUS" || Extension.ToUpper() == ".ACC" || Extension.ToUpper() == ".AMR")
                        {
                            if (size > 16384)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Maximum audio/video size should be of 16 MB.');", true);
                                lblImage.Text = "Upload rejected.";
                                return;
                            }
                        }

                        if (Extension.ToUpper() == ".JPG" || Extension.ToUpper() == ".JPEG" || Extension.ToUpper() == ".PNG")//.jpg,.jpeg,.png,
                        {
                            if (size > 5120)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Maximum allowed Image size 5 MB.');", true);
                                lblImage.Text = "Upload rejected.";
                                return;
                            }
                        }

                        if (Extension.ToUpper() == ".PDF")
                        {
                            if (size > 102400)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Maximum allowed File size 100 MB.');", true);
                                lblImage.Text = "Upload rejected.";
                                return;
                            }
                        }

                        string FolderPath = "Uploads/Images/";
                        Session["ImageName"] = FileName;
                        Session["imageExtension"] = Extension;
                        Session["MediaFileName"] = FileName.Replace(Extension, "").ToString();

                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = User + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                        Session["imageNameOnly"] = FileNameOnly;
                        Session["MediaUrl"] = UploadPath + FolderPath + FileNameOnly;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        uImage.SaveAs(FilePath);
                        string namewithoutextenrion = Path.GetFileNameWithoutExtension(FileNameOnly);
                        string fileUname = namewithoutextenrion;
                        Session["MediaUName"] = FilePath;

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                        divmediafile.Visible = true;
                        lblImage.Text = "" + FileName + " Uploaded successfully.";
                        string fileType = "";
                        if (Extension.ToUpper() == ".JPG" || Extension.ToUpper() == ".PNG" || Extension.ToUpper() == ".JPEG")
                        {
                            fileType = "I";
                            txtPreview.Attributes.Add("Style", "pointer-events:none;");
                        }
                        else if (Extension.ToUpper() == ".MP4")
                        {
                            fileType = "V";
                            txtPreview.Attributes.Add("Style", "pointer-events:none;");
                        }
                        else if (Extension.ToUpper() == ".PDF")
                        {
                            fileType = "D";
                            txtPreview.Attributes.Add("Style", "pointer-events:none;");
                        }
                        divMsg.Attributes.Add("Style", "pointer-events:none;");
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        protected void GridFormat(DataTable dt)
        {
            grdshorturl.UseAccessibleHeader = true;
            grdshorturl.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grdshorturl.TopPagerRow != null)
            {
                grdshorturl.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grdshorturl.BottomPagerRow != null)
            {
                grdshorturl.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
            {
                try
                {
                    grdshorturl.FooterRow.TableSection = TableRowSection.TableFooter;
                }
                catch (Exception) { }
            }
        }

        protected void lnkCreateShortURL_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            if (rdbCreateShortURL.Checked)
            {
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
                if (txtShortURLname.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Short URL Name.');", true);
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
                using (var ctx = new ShortnrContext())
                {
                    UrlManager ob2 = new UrlManager();
                    string sUrl = "";
                    sUrl = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString());
                    string segment = "";
                    bool IsRichMedia = false;
                    if (txtLongURL.Text.Trim().Contains(Convert.ToString(Session["DOMAINNAME"])))
                    {
                        segment = txtLongURL.Text.Trim().Replace("//", "/").Split('/').Last();
                        IsRichMedia = ob.IsRichMediaURL(UserID, segment);
                    }
                    if (IsRichMedia)
                        ob.SaveShortURLRichMedia(UserID, txtLongURL.Text, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), txtShortURLname.Text.Trim());
                    else
                        ob.SaveShortURL(UserID, txtLongURL.Text, Request.UserHostAddress, sUrl, "N", "Y", Convert.ToString(Session["DOMAINNAME"]), txtShortURLname.Text.Trim());

                    lblShortURL.Text = Convert.ToString(Session["DOMAINNAME"]) + sUrl;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);
                }
            }
        }

        protected void lnkbtnLink_Click(object sender, EventArgs e)
        {
            int ChkEmailIsExistOrNot = Convert.ToInt32(database.GetScalarValue(@"SELECT count(1) FROM ReminderDetails WITH(NOLOCK) where UserId='" + Convert.ToString(Session["userid"]) + "'"));
            if (ChkEmailIsExistOrNot == 0)
            {
                List<string> ccs = new List<string>();
                DataTable dt = database.GetDataTable("SELECT * FROM settingmast WITH(NOLOCK)");
                if (dt.Rows.Count > 0)
                {
                    string ToMail = Convert.ToString(dt.Rows[0]["ToEmailId"]).Trim();
                    string cc1 = Convert.ToString(dt.Rows[0]["cc1"]);
                    string cc2 = Convert.ToString(dt.Rows[0]["cc2"]);
                    string cc3 = Convert.ToString(dt.Rows[0]["cc3"]);
                    string cc4 = Convert.ToString(dt.Rows[0]["cc4"]);
                    string cc5 = Convert.ToString(dt.Rows[0]["cc5"]);

                    if (!string.IsNullOrEmpty(cc1.Trim()))
                    {
                        ccs.Add(cc1);
                    }
                    if (!string.IsNullOrEmpty(cc2.Trim()))
                    {
                        ccs.Add(cc2);
                    }
                    if (!string.IsNullOrEmpty(cc3.Trim()))
                    {
                        ccs.Add(cc3);
                    }
                    if (!string.IsNullOrEmpty(cc4.Trim()))
                    {
                        ccs.Add(cc4);
                    }
                    if (!string.IsNullOrEmpty(cc5.Trim()))
                    {
                        ccs.Add(cc5);
                    }
                    DateTime dateTime = DateTime.Now;
                    string CCEmail = string.Join(",", ccs);
                    List<string> CC = CCEmail.Split(',').Select(t => Convert.ToString(t)).ToList();
                    string EmailSubject = Convert.ToString(dt.Rows[0]["WabaLinkEmailSubject"]);
                    string EmailBody = Convert.ToString(dt.Rows[0]["WabaLinkEmailBody"]);

                    EmailBody = EmailBody.Replace("#USERID#", Convert.ToString(Session["userid"])).Replace("#CLIENTNAME#", Convert.ToString(Session["FullName"])).Replace("#REQUESTDATE#", dateTime.ToString("dd/MM/yyyy")).Replace("#REQUESTTIME#", dateTime.ToString("hh:mm tt"));
                    string result = obj.SendEmail(ToMail, EmailSubject, EmailBody, Convert.ToString(dt.Rows[0]["userid"]), Convert.ToString(dt.Rows[0]["password"]), Convert.ToString(dt.Rows[0]["Host"]), CC);
                    if (Convert.ToBoolean(dt.Rows[0]["ReminderReq4SMSLinkToWABA"]) == true)
                    {
                        obj.InsertWhenIsReminderTrue(Convert.ToString(Session["userid"]));
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('We have informed MyInboxMedia Team for this, you will receive a communication shortly. !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Not Found !!');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('We have already informed MyInboxMedia Team for this, you will receive a communication shortly.');", true);
                return;
            }
        }

        protected void BindShortURL()
        {
            string ws = Convert.ToString(Session["DOMAINNAME"]);
            DataTable dt = obj.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), ws);
            ddlURL.DataSource = dt;
            ddlURL.DataTextField = "shorturlDISP";
            ddlURL.DataValueField = "shorturl";
            ddlURL.DataBind();
            ListItem objlistitem = new ListItem("--select--", "0");
            ddlURL.Items.Insert(0, objlistitem);
            ddlURL.SelectedIndex = 0;
        }


        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSelectShortURL.Checked)
            {
                divSelectShortURL.Visible = true;
                divCreateShortURL.Visible = false;
            }

            if (rdbCreateShortURL.Checked)
            {
                divSelectShortURL.Visible = false;
                divCreateShortURL.Visible = true;
            }
        }

        public void PopulateTemplateID(string id)
        {
            DataTable dt = obj.GetwaTemplateId(Convert.ToString(Session["WabaProfileId"]), id);
            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "name1";
            ddlTempID.DataValueField = "TemplateID";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
        }

        private void DefaultAllAfterChnage()
        {
            divTimeout.Visible = false;
            divoofrcode.Visible = false;
            divexp.Visible = false;
            divtxt.Visible = false;
            divemptydiv.Visible = false;
            divtxt.Visible = false;
            divemptydiv.Visible = false;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            TextBox9.Visible = false;
            TextBox10.Visible = false;
            divCardMain.Visible = false;
            divCard1.Visible = false;
            divCard2.Visible = false;
            divCard3.Visible = false;
            divCard4.Visible = false;
            divCard5.Visible = false;
            divCard6.Visible = false;
            divCard7.Visible = false;
            divCard8.Visible = false;
            divCard9.Visible = false;
            divCard10.Visible = false;
            for (int l = 1; l <= 10; l++)
            {
                TextBox txtVarCard1 = divtxt.FindControl("txtVarCard" + l + "1") as TextBox;
                TextBox txtVarCard2 = divtxt.FindControl("txtVarCard" + l + "2") as TextBox;
                TextBox txtVarCard3 = divtxt.FindControl("txtVarCard" + l + "3") as TextBox;
                TextBox txtVarCard4 = divtxt.FindControl("txtVarCard" + l + "4") as TextBox;
                TextBox txtVarCard5 = divtxt.FindControl("txtVarCard" + l + "5") as TextBox;
                TextBox txtVarCard6 = divtxt.FindControl("txtVarCard" + l + "6") as TextBox;
                TextBox txtVarCard7 = divtxt.FindControl("txtVarCard" + l + "7") as TextBox;
                TextBox txtVarCard8 = divtxt.FindControl("txtVarCard" + l + "8") as TextBox;
                TextBox txtVarCard9 = divtxt.FindControl("txtVarCard" + l + "9") as TextBox;
                TextBox txtVarCard10 = divtxt.FindControl("txtVarCard" + l + "10") as TextBox;
                txtVarCard1.Visible = false;
                txtVarCard2.Visible = false;
                txtVarCard3.Visible = false;
                txtVarCard4.Visible = false;
                txtVarCard5.Visible = false;
                txtVarCard6.Visible = false;
                txtVarCard7.Visible = false;
                txtVarCard8.Visible = false;
                txtVarCard9.Visible = false;
                txtVarCard10.Visible = false;
            }
            divCardMain.Visible = false;
            divCard1.Visible = false;
            divCard2.Visible = false;
            divCard3.Visible = false;
            divCard4.Visible = false;
            divCard5.Visible = false;
            divCard6.Visible = false;
            divCard7.Visible = false;
            divCard8.Visible = false;
            divCard9.Visible = false;
            divCard10.Visible = false;
            divemptydiv.Visible = false;
            divtxt.Visible = false;
            divtxt.Visible = false;
            divsmstext.Visible = false;
            divCard1.Visible = false;
            divCard2.Visible = false;
            divCard3.Visible = false;
            divCard4.Visible = false;
            divCard5.Visible = false;
            divCard6.Visible = false;
            divCard7.Visible = false;
            divCard8.Visible = false;
            divCard9.Visible = false;
            divCard10.Visible = false;
            divemptydiv.Visible = false;
            divtxt.Visible = false;
        }

        protected void txtfalse()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            TextBox9.Visible = false;
            TextBox10.Visible = false;
        }

        protected void LnkbtnSubmitTemplate_Click(object sender, EventArgs e)
        {
            string ShortURL = "";
            string WABAParamWithDelimeter = "";
            if (rdbCreateShortURL.Checked)
            {
                if (lblShortURL.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Create Short URL.');", true);
                    return;
                }
                else
                {
                    ShortURL = Convert.ToString(lblShortURL.Text.Trim()).Trim();
                }
            }
            if (rdbSelectShortURL.Checked)
            {
                if (ddlURL.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Short URL.');", true);
                    return;
                }
                else
                {
                    ShortURL = Convert.ToString(ddlURL.SelectedItem.Text).Trim();
                }
            }
            int checkshortrul = Convert.ToInt32(database.GetScalarValue(@"select count(*) from SendWABAOnLinkClick with(nolock) where ShortUrl='" + ShortURL + "' and LinkextUserId='" + User + "'"));
            if (checkshortrul > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL already exists !!');", true);
                return;
            }
            if (ddlTempID.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template.');", true);
                return;
            }

            if (TextBox1.Visible == true)
            {
                if (Convert.ToString(TextBox1.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var1.');", true);
                    return;
                }
                WABAParamWithDelimeter = Convert.ToString(TextBox1.Text.Trim());
            }
            if (TextBox2.Visible == true)
            {
                if (Convert.ToString(TextBox2.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var2.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox2.Text.Trim());
            }
            if (TextBox3.Visible == true)
            {
                if (Convert.ToString(TextBox3.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var3.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox3.Text.Trim());
            }
            if (TextBox4.Visible == true)
            {
                if (Convert.ToString(TextBox4.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var4.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox4.Text.Trim());
            }
            if (TextBox5.Visible == true)
            {
                if (Convert.ToString(TextBox5.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var5.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox5.Text.Trim());
            }
            if (TextBox6.Visible == true)
            {
                if (Convert.ToString(TextBox6.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var6.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox6.Text.Trim());
            }
            if (TextBox7.Visible == true)
            {
                if (Convert.ToString(TextBox7.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var7.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox7.Text.Trim());
            }
            if (TextBox8.Visible == true)
            {
                if (Convert.ToString(TextBox8.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var8.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox8.Text.Trim());
            }
            if (TextBox9.Visible == true)
            {
                if (Convert.ToString(TextBox9.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var9.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox9.Text.Trim());
            }
            if (TextBox10.Visible == true)
            {
                if (Convert.ToString(TextBox10.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Var10.');", true);
                    return;
                }
                WABAParamWithDelimeter = WABAParamWithDelimeter + "$," + Convert.ToString(TextBox10.Text.Trim());
            }

            string segment = ShortURL.Split('/').Last();
            DataTable dt = database.GetDataTable(@"SELECT * FROM short_urls WITH(NOLOCK) WHERE userid='" + User + "' AND Segment='" + segment + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtTemp = database.GetDataTable(@"SELECT * FROM " + dbNameWABA + ".dbo.Template with(nolock) where userid='" + Convert.ToString(Session["WabaProfileId"]) + "' and nid=" + ddlTempID.SelectedValue + "");
                string tHeadType = Convert.ToString(dtTemp.Rows[0]["tHeadType"]);
                if (tHeadType.ToUpper() == "I" || tHeadType.ToUpper() == "V" || tHeadType.ToUpper() == "D" || tHeadType.ToUpper() == "A")
                {
                    if (Session["MediaUName"] == null && Session["MediaUrl"] == null && rbFileOrURL.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Upload Media File !!');", true);
                        return;
                    }
                    if (rbFileOrURL.SelectedValue == "1" && txtFileUrl.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Media File URL !!');", true);
                        return;
                    }
                    if (rbFileOrURL.SelectedValue == "1" && (!txtFileUrl.Text.Trim().ToLower().Contains("http")))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Media File URL !!');", true);
                        return;
                    }
                }

                string path = "", URL = "";
                if (rbFileOrURL.SelectedValue == "1")
                {
                    URL = txtFileUrl.Text.Trim();
                }
                else
                {
                    path = Convert.ToString(Session["MediaUName"]);
                    URL = Convert.ToString(Session["MediaUrl"]);
                }


                string tHeadTypeConverted;
                switch (tHeadType)
                {
                    case "T":
                        tHeadTypeConverted = "TEXT";
                        break;
                    case "I":
                        tHeadTypeConverted = "IMAGE";
                        break;
                    case "V":
                        tHeadTypeConverted = "VIDEO";
                        break;
                    case "A":
                        tHeadTypeConverted = "AUDIO";
                        break;
                    case "D":
                        tHeadTypeConverted = "DOCUMENT";
                        break;
                    default:
                        tHeadTypeConverted = tHeadType; // or handle as needed
                        break;
                }
                obj.SaveSMSLinkToWABADetails(User, Convert.ToString(dt.Rows[0]["id"]), ShortURL.ToString().Trim(), Convert.ToString(dt.Rows[0]["segment"]),
                    Convert.ToString(Session["WabaProfileId"]), Convert.ToString(Session["WabaPwd"]), Convert.ToString(dtTemp.Rows[0]["tName"]),
                    tHeadTypeConverted, URL, WABAParamWithDelimeter);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Link Saved Successfully');  window.location ='SMSLinkToWABA.aspx';", true);
                Session["MediaUName"] = null; Session["MediaUrl"] = null;
                return;
            }
        }

        protected void grdshorturl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Unlink")
            {
                int recordID = Convert.ToInt32(e.CommandArgument);
                database.ExecuteNonQuery("insert into SendWABAOnLinkClick_log (LinkextUserId,ShortUrlID,ShortUrl,Segment,WABAUserID,WABAUserAPIKey,WABATemplateName,WABATemplateTempType,Url,InsertDateTime,Active,WabaParamWithDelimiter) " +
                    " Select LinkextUserId,ShortUrlID,ShortUrl,Segment,WABAUserID,WABAUserAPIKey,WABATemplateName,WABATemplateTempType,Url,InsertDateTime,Active,WabaParamWithDelimiter " +
                    " FROM SendWABAOnLinkClick WHERE LinkextUserId='" + Convert.ToString(Session["UserID"]) + "' AND ID='" + Convert.ToString(recordID) + "' ; " +
                    " DELETE FROM SendWABAOnLinkClick WHERE LinkextUserId='" + Convert.ToString(Session["UserID"]) + "' AND ID='" + Convert.ToString(recordID) + "'");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('UnLink Successfully');  window.location ='SMSLinkToWABA.aspx';", true);
                return;
            }
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefaultAllAfterChnage();
            txtfalse();
            Helper.Util ob = new Helper.Util();
            string sql = @"SELECT cast(a.nid as varchar) +'('+a.tName+')' AS name,a.nid TemplateID,VartBodyText tbodytext,* FROM " + dbNameWABA + ".dbo.Template a WITH(NOLOCK) WHERE nid ='" + ddlTempID.SelectedValue + "' ";
            DataTable dtT = database.GetDataTable(sql);
            DataTable dt1 = new DataTable();

            if (dtT.Rows.Count > 0)
            {
                Session["btnType"] = Convert.ToString(dtT.Rows[0]["btnType"]);
                Session["LTOType"] = Convert.ToString(dtT.Rows[0]["LTOType"]);
                Session["LTOOfferText"] = Convert.ToString(dtT.Rows[0]["LTOOfferText"]);
                Session["tButton2Title"] = Convert.ToString(dtT.Rows[0]["tButton2Title"]);

                if (Convert.ToString(dtT.Rows[0]["tHeadType"]).ToUpper() == "CAROUSEL")
                {
                    string sql1 = "SELECT * FROM " + dbNameWABA + ".dbo.TemplateCarouselCards WITH(NOLOCK) WHERE TemplateID='" + ddlTempID.SelectedValue + "'";
                    dt1 = database.GetDataTable(sql1);
                }

                if (Convert.ToString(dtT.Rows[0]["tHeadType"]).ToUpper() == "CATALOG")
                {
                    divCatalogProductId.Visible = true;
                }
                else
                {
                    divCatalogProductId.Visible = false;
                }

                if (Convert.ToString(dtT.Rows[0]["btnType"]) == "LTO")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "limitofrbind('" + Convert.ToString(dtT.Rows[0]["LTOOfferText"]) + "','" + Convert.ToString(dtT.Rows[0]["tButton2Title"]) + "');", true);
                    divTimeout.Visible = true;
                    hdnLto.Value = "LTO";
                    if (Convert.ToString(Session["XLUPLOADED"]) != "Y")
                    {
                        divoofrcode.Visible = true;
                    }
                    else
                    {
                        divoofrcode.Visible = false;
                    }
                    if (Convert.ToString(Session["LTOType"]).ToUpper() == "TRUE")
                    {
                        divexp.Visible = true;
                    }
                    else
                    {
                        divexp.Visible = false;
                    }
                }
                else
                {
                    divTimeout.Visible = false;
                    divexp.Visible = false;
                }
                string smstxt = "";

                DataTable dtVarNAme = database.GetDataTable(@" SELECT REPLACE( REPLACE(REPLACE(varname,'{{',''),'}}',''),'-->','#') varname,CarouselCardsNo 
                                      FROM " + dbNameWABA + ".dbo.TemplateVarName WITH(NOLOCK) WHERE TemplateId='" + ddlTempID.SelectedValue + "'");

                hfVar1.Value = "{Var1}";
                hfVar2.Value = "{Var2}";
                hfVar3.Value = "{Var3}";
                hfVar4.Value = "{Var4}";
                hfVar5.Value = "{Var5}";
                hfVar6.Value = "{Var6}";
                hfVar7.Value = "{Var7}";
                hfVar8.Value = "{Var8}";
                hfVar9.Value = "{Var9}";
                hfVar10.Value = "{Var10}";

                TextBox1.Attributes["placeholder"] = "{Var1}";
                TextBox2.Attributes["placeholder"] = "{Var2}";
                TextBox3.Attributes["placeholder"] = "{Var3}";
                TextBox4.Attributes["placeholder"] = "{Var4}";
                TextBox5.Attributes["placeholder"] = "{Var5}";
                TextBox6.Attributes["placeholder"] = "{Var6}";
                TextBox7.Attributes["placeholder"] = "{Var7}";
                TextBox8.Attributes["placeholder"] = "{Var8}";
                TextBox9.Attributes["placeholder"] = "{Var9}";
                TextBox10.Attributes["placeholder"] = "{Var10}";

                if (dtVarNAme != null && dtVarNAme.Rows.Count > 0)
                {
                    for (int j = 0; j < dtVarNAme.Rows.Count; j++)
                    {
                        string CarouselCardsNo = Convert.ToString(dtVarNAme.Rows[j]["CarouselCardsNo"]);
                        string varname = Convert.ToString(dtVarNAme.Rows[j]["varname"]).Split('#')[1];
                        string varSeq = Convert.ToString(dtVarNAme.Rows[j]["varname"]).Split('#')[0];
                        if (CarouselCardsNo == "0")
                        {
                            if (!(varname == "1" || varname == "2" || varname == "3" || varname == "4" || varname == "5" || varname == "6" || varname == "7" || varname == "8" || varname == "9" || varname == "10"))
                            {
                                if (varSeq.Trim() == "1")
                                {
                                    TextBox1.Attributes["placeholder"] = varname;
                                    hfVar1.Value = varname;
                                }
                                else if (varSeq.Trim() == "2")
                                {
                                    TextBox2.Attributes["placeholder"] = varname;
                                    hfVar2.Value = varname;
                                }
                                else if (varSeq.Trim() == "3")
                                {
                                    TextBox3.Attributes["placeholder"] = varname;
                                    hfVar3.Value = varname;
                                }
                                else if (varSeq.Trim() == "3")
                                {
                                    TextBox3.Attributes["placeholder"] = varname;
                                    hfVar3.Value = varname;
                                }
                                else if (varSeq.Trim() == "4")
                                {
                                    TextBox4.Attributes["placeholder"] = varname;
                                    hfVar4.Value = varname;
                                }
                                else if (varSeq.Trim() == "5")
                                {
                                    TextBox5.Attributes["placeholder"] = varname;
                                    hfVar5.Value = varname;
                                }
                                else if (varSeq.Trim() == "6")
                                {
                                    TextBox6.Attributes["placeholder"] = varname;
                                    hfVar6.Value = varname;
                                }
                                else if (varSeq.Trim() == "7")
                                {
                                    TextBox7.Attributes["placeholder"] = varname;
                                    hfVar7.Value = varname;

                                }
                                else if (varSeq.Trim() == "8")
                                {
                                    TextBox8.Attributes["placeholder"] = varname;
                                    hfVar8.Value = varname;

                                }
                                else if (varSeq.Trim() == "9")
                                {
                                    TextBox9.Attributes["placeholder"] = varname;
                                    hfVar9.Value = varname;

                                }
                                else if (varSeq.Trim() == "10")
                                {
                                    TextBox10.Attributes["placeholder"] = varname;
                                    hfVar10.Value = varname;
                                }
                            }
                        }
                        else if (CarouselCardsNo == "1" || CarouselCardsNo == "2" || CarouselCardsNo == "3" || CarouselCardsNo == "4" || CarouselCardsNo == "5" || CarouselCardsNo == "6" || CarouselCardsNo == "7" || CarouselCardsNo == "8" || CarouselCardsNo == "9" || CarouselCardsNo == "10")
                        {
                            if (!(varname == "1" || varname == "2" || varname == "3" || varname == "4" || varname == "5" || varname == "6" || varname == "7" || varname == "8" || varname == "9" || varname == "10"))
                            {
                                HiddenField HiddenFieldCard1 = divtxt.FindControl("HiddenFieldCard" + j + "1") as HiddenField;
                                HiddenField HiddenFieldCard2 = divtxt.FindControl("HiddenFieldCard" + j + "2") as HiddenField;
                                HiddenField HiddenFieldCard3 = divtxt.FindControl("HiddenFieldCard" + j + "3") as HiddenField;
                                HiddenField HiddenFieldCard4 = divtxt.FindControl("HiddenFieldCard" + j + "4") as HiddenField;
                                HiddenField HiddenFieldCard5 = divtxt.FindControl("HiddenFieldCard" + j + "5") as HiddenField;
                                HiddenField HiddenFieldCard6 = divtxt.FindControl("HiddenFieldCard" + j + "6") as HiddenField;
                                HiddenField HiddenFieldCard7 = divtxt.FindControl("HiddenFieldCard" + j + "7") as HiddenField;
                                HiddenField HiddenFieldCard8 = divtxt.FindControl("HiddenFieldCard" + j + "8") as HiddenField;
                                HiddenField HiddenFieldCard9 = divtxt.FindControl("HiddenFieldCard" + j + "9") as HiddenField;
                                HiddenField HiddenFieldCard10 = divtxt.FindControl("HiddenFieldCard" + j + "10") as HiddenField;

                                TextBox txtVarCard1 = divtxt.FindControl("txtVarCard" + j + "1") as TextBox;
                                TextBox txtVarCard2 = divtxt.FindControl("txtVarCard" + j + "2") as TextBox;
                                TextBox txtVarCard3 = divtxt.FindControl("txtVarCard" + j + "3") as TextBox;
                                TextBox txtVarCard4 = divtxt.FindControl("txtVarCard" + j + "4") as TextBox;
                                TextBox txtVarCard5 = divtxt.FindControl("txtVarCard" + j + "5") as TextBox;
                                TextBox txtVarCard6 = divtxt.FindControl("txtVarCard" + j + "6") as TextBox;
                                TextBox txtVarCard7 = divtxt.FindControl("txtVarCard" + j + "7") as TextBox;
                                TextBox txtVarCard8 = divtxt.FindControl("txtVarCard" + j + "8") as TextBox;
                                TextBox txtVarCard9 = divtxt.FindControl("txtVarCard" + j + "9") as TextBox;
                                TextBox txtVarCard10 = divtxt.FindControl("txtVarCard" + j + "10") as TextBox;

                                if (varSeq.Trim() == "1")
                                {
                                    txtVarCard1.Attributes["placeholder"] = varname;
                                    HiddenFieldCard1.Value = varname;
                                }
                                else if (varSeq.Trim() == "2")
                                {
                                    txtVarCard2.Attributes["placeholder"] = varname;
                                    HiddenFieldCard2.Value = varname;
                                }
                                else if (varSeq.Trim() == "3")
                                {
                                    txtVarCard3.Attributes["placeholder"] = varname;
                                    HiddenFieldCard3.Value = varname;
                                }
                                else if (varSeq.Trim() == "4")
                                {
                                    txtVarCard4.Attributes["placeholder"] = varname;
                                    HiddenFieldCard4.Value = varname;
                                }
                                else if (varSeq.Trim() == "5")
                                {
                                    txtVarCard5.Attributes["placeholder"] = varname;
                                    HiddenFieldCard5.Value = varname;
                                }
                                else if (varSeq.Trim() == "6")
                                {
                                    txtVarCard6.Attributes["placeholder"] = varname;
                                    HiddenFieldCard6.Value = varname;
                                }
                                else if (varSeq.Trim() == "7")
                                {
                                    txtVarCard7.Attributes["placeholder"] = varname;
                                    HiddenFieldCard7.Value = varname;
                                }
                                else if (varSeq.Trim() == "8")
                                {
                                    txtVarCard8.Attributes["placeholder"] = varname;
                                    HiddenFieldCard8.Value = varname;
                                }
                                else if (varSeq.Trim() == "9")
                                {
                                    txtVarCard9.Attributes["placeholder"] = varname;
                                    HiddenFieldCard9.Value = varname;
                                }
                                else if (varSeq.Trim() == "10")
                                {
                                    txtVarCard10.Attributes["placeholder"] = varname;
                                    HiddenFieldCard10.Value = varname;
                                }
                            }
                        }
                    }
                }

                if (dtT.Rows.Count > 0)
                {
                    smstxt = dtT.Rows[0]["tbodytext"].ToString();
                }
                txtMsg.Text = smstxt;
                txtPreview.Text = smstxt;

                string s = dtT.Rows[0]["tbodytext"].ToString();
                string[] stringseprator = new string[] { "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}", "{11}", "{12}", "{13}", "{14}", "{15}" };
                string[] str1 = s.Split(stringseprator, StringSplitOptions.None);
                string[] s1 = s.Split(stringseprator, StringSplitOptions.None);
                hdnTemplateVarText.Value = s;

                int count = Regex.Matches(s, "}}").Count;
                vrCount.Value = Convert.ToString(count);

                int i = 0; if (s != "") { if (s.Substring(0, 1) != Convert.ToString(stringseprator)) i = 1; }

                for (int txtid = 1; txtid <= count; txtid++)
                {
                    if (txtid <= 10)
                    {
                        if (txtid == 1)
                        {
                            TextBox1.Visible = true;
                        }
                        else if (txtid == 2)
                        {
                            TextBox2.Visible = true;
                        }
                        else if (txtid == 3)
                        {
                            TextBox3.Visible = true;
                        }
                        else if (txtid == 4)
                        {
                            TextBox4.Visible = true;
                        }
                        else if (txtid == 5)
                        {
                            TextBox5.Visible = true;
                        }
                        else if (txtid == 6)
                        {
                            TextBox6.Visible = true;
                        }
                        else if (txtid == 7)
                        {
                            TextBox7.Visible = true;
                        }
                        else if (txtid == 8)
                        {
                            TextBox8.Visible = true;
                        }
                        else if (txtid == 9)
                        {
                            TextBox9.Visible = true;
                        }
                        else if (txtid == 10)
                        {
                            TextBox10.Visible = true;
                        }
                    }
                }

                DataTable DTCarouselCardNo = database.GetDataTable(@"SELECT CarouselCardBody FROM " + dbNameWABA + @".dbo.TemplateCarouselCards WITH(NOLOCK) 
                                                 WHERE TemplateID ='" + ddlTempID.SelectedValue + "'");
                if (DTCarouselCardNo != null && DTCarouselCardNo.Rows.Count > 0)
                {
                    for (int CardNo = 0; CardNo < DTCarouselCardNo.Rows.Count; CardNo++)
                    {
                        string CardBody = DTCarouselCardNo.Rows[CardNo]["CarouselCardBody"].ToString();
                        if (CardBody != "")
                        {
                            divCardMain.Visible = true;
                            if (CardNo == 0)
                            {
                                divCard1.Visible = true;
                            }
                            else if (CardNo == 1)
                            {
                                divCard2.Visible = true;
                            }
                            else if (CardNo == 2)
                            {
                                divCard3.Visible = true;
                            }
                            else if (CardNo == 3)
                            {
                                divCard4.Visible = true;
                            }
                            else if (CardNo == 4)
                            {
                                divCard5.Visible = true;
                            }
                            else if (CardNo == 5)
                            {
                                divCard6.Visible = true;
                            }
                            else if (CardNo == 6)
                            {
                                divCard7.Visible = true;
                            }
                            else if (CardNo == 7)
                            {
                                divCard8.Visible = true;
                            }
                            else if (CardNo == 8)
                            {
                                divCard9.Visible = true;
                            }
                            else if (CardNo == 9)
                            {
                                divCard10.Visible = true;
                            }
                        }
                        int CarouselVarCount = Regex.Matches(CardBody, "}}").Count;
                        for (int txtid = 1; txtid <= CarouselVarCount; txtid++)
                        {
                            int l = CardNo + 1;
                            TextBox txtVarCard1 = divtxt.FindControl("txtVarCard" + l + "1") as TextBox;
                            TextBox txtVarCard2 = divtxt.FindControl("txtVarCard" + l + "2") as TextBox;
                            TextBox txtVarCard3 = divtxt.FindControl("txtVarCard" + l + "3") as TextBox;
                            TextBox txtVarCard4 = divtxt.FindControl("txtVarCard" + l + "4") as TextBox;
                            TextBox txtVarCard5 = divtxt.FindControl("txtVarCard" + l + "5") as TextBox;
                            TextBox txtVarCard6 = divtxt.FindControl("txtVarCard" + l + "6") as TextBox;
                            TextBox txtVarCard7 = divtxt.FindControl("txtVarCard" + l + "7") as TextBox;
                            TextBox txtVarCard8 = divtxt.FindControl("txtVarCard" + l + "8") as TextBox;
                            TextBox txtVarCard9 = divtxt.FindControl("txtVarCard" + l + "9") as TextBox;
                            TextBox txtVarCard10 = divtxt.FindControl("txtVarCard" + l + "10") as TextBox;
                            if (txtid <= 10)
                            {
                                if (txtid == 1)
                                {
                                    txtVarCard1.Visible = true;
                                }
                                else if (txtid == 2)
                                {
                                    txtVarCard2.Visible = true;
                                }
                                else if (txtid == 3)
                                {
                                    txtVarCard3.Visible = true;
                                }
                                else if (txtid == 4)
                                {
                                    txtVarCard4.Visible = true;
                                }
                                else if (txtid == 5)
                                {
                                    txtVarCard5.Visible = true;
                                }
                                else if (txtid == 6)
                                {
                                    txtVarCard6.Visible = true;
                                }
                                else if (txtid == 7)
                                {
                                    txtVarCard7.Visible = true;
                                }
                                else if (txtid == 8)
                                {
                                    txtVarCard8.Visible = true;
                                }
                                else if (txtid == 9)
                                {
                                    txtVarCard9.Visible = true;
                                }
                                else if (txtid == 10)
                                {
                                    txtVarCard10.Visible = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    divCardMain.Visible = false;
                    divCard1.Visible = false;
                    divCard2.Visible = false;
                    divCard3.Visible = false;
                    divCard4.Visible = false;
                    divCard5.Visible = false;
                    divCard6.Visible = false;
                    divCard7.Visible = false;
                    divCard8.Visible = false;
                    divCard9.Visible = false;
                    divCard10.Visible = false;
                }
                divtxt.Attributes.Add("Style", "overflow-y:auto;");
                divemptydiv.Visible = true;
                divtxt.Visible = true;
                divtxt.Visible = true;

                setDiv(Convert.ToString(dtT.Rows[0]["tHeadType"]));
                ViewState["HeaderType"] = dtT.Rows[0]["tHeadType"].ToString();
                string file = "";
                if (dtT.Rows[0]["fileuploadorURL"].ToString() == "U")
                {
                    file = dtT.Rows[0]["URL"].ToString();
                }
                else
                {
                    file = "/uploads/" + dtT.Rows[0]["fileupload"].ToString();
                }
            }
        }

        public void setDiv(string HeaderType)
        {
            if (HeaderType.Trim().ToUpper() == "I")
            {
                divuploadImage.Visible = true;
                divuploadImage.Attributes.Add("class", "row");
                divsmstext.Visible = false;
                lblfilesize.InnerText = "5 MB";
                uImage.Attributes.Remove("accept");
                uImage.Attributes.Add("accept", ".jpg,.jpeg,.png");
                divemptydiv.Visible = false;
                divtxt.Visible = true;
                //}
                divMsg.Visible = true;
                divsmstext.Visible = true;
            }
            else if (HeaderType.Trim().ToUpper() == "V")
            {
                divuploadImage.Visible = true;
                divuploadImage.Attributes.Add("class", "row");
                divsmstext.Visible = false;
                lblfilesize.InnerText = "16 MB";
                uImage.Attributes.Remove("accept");
                uImage.Attributes.Add("accept", ".mp4,.3gpp");
                dvcaption.Visible = true;
                divMsg.Visible = true;
                divsmstext.Visible = true;
            }
            else if (HeaderType.Trim().ToUpper() == "D")
            {
                divuploadImage.Visible = true;
                divuploadImage.Attributes.Add("class", "row");
                divsmstext.Visible = false;
                lblfilesize.InnerText = "100 MB";
                uImage.Attributes.Remove("accept");
                uImage.Attributes.Add("accept", ".pdf");
                divMsg.Visible = true;
                divsmstext.Visible = true;
                divMsg.Visible = true;
                divsmstext.Visible = true;
            }
            else
            {
                divuploadImage.Visible = false;
                divuploadImage.Attributes.Add("class", "row d-none");
                divsmstext.Visible = true;
                lblfilesize.InnerText = "";
                uImage.Attributes.Remove("accept");
                uImage.Attributes.Add("accept", ".mp4,.3gpp,.pdf,.jpg,.jpeg,.png,.aac,.amr,.mp3,.mp4,.opus");
                dvcaption.Visible = false;
            }
        }

        protected void rbFileOrURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbFileOrURL.SelectedValue == "0")
            {
                dvMediafile.Visible = true;
                divmediafile.Visible = true;
                dventerurl.Visible = false;
            }
            else
            {
                dvMediafile.Visible = false;
                divmediafile.Visible = false;
                dventerurl.Visible = true;
            }
        }
    }
}