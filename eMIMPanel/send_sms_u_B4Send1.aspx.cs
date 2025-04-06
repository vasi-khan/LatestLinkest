using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Mvc;
using System.Web.Hosting;
using System.ComponentModel;
using Shortnr.Web.Business;
using System.Threading;
using System.Globalization;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class send_sms_u_B4Send1 : System.Web.UI.Page
    {
        string tempVar = "{#var#}";
        string _user = "";
        string paisa = "";
        // private readonly string speceficAcc = "MIM2101495";
        // private readonly string speceficAcc2 = "MIM2002006";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            //paisa = "Paisa";
            //if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971") paisa = "fills";
            //if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "965") paisa = "Fils";
            //FilteredTextBoxExtender1.ValidChars = FilteredTextBoxExtender1.ValidChars + "\r\n";
            paisa = Convert.ToString(Session["SUBCURRENCY"]);

            _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");

            // if (_user == "MIM2000002") Response.Redirect("send-sms_u30.aspx");
            // if (_user == "MIM2000029") Response.Redirect("send-sms_u30.aspx");
            // if (_user == "MIM2002097") Response.Redirect("send-sms_u30.aspx");

            if (Convert.ToString(Session["CANSENDSMS"]).ToUpper() == "FALSE") Response.Redirect("index_u.aspx");
            if (!IsPostBack)
            {

                Util ob = new Util();
                PopulateCountry();

                ob.DropUserTmpTable(_user);

                Global.templateErrorCode = "";
                Global.Istemplatetest = true;
                Global.openTempAc = ob.IsOpenTempAc(Convert.ToString(Session["UserID"]));

                //List<string> countryList = CountryList();
                //string duplicateItem = "";
                //foreach (var items in countryList)
                //{
                //    string ccode = GetCode(items.ToString());
                //    if (duplicateItem == "")
                //    {
                //        String text = String.Format("{0}-{1}\n",
                //                        items.ToString(), ccode);
                //        ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                //        //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                //        duplicateItem = items;
                //    }
                //    if (duplicateItem != items.ToString())
                //    {
                //        String text = String.Format("{0}-{1}\n",
                //                        items.ToString(), ccode);
                //        //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                //        ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                //        duplicateItem = items;
                //    }
                //}

                //Imagetitlebind();
                //  ddlCCode.Items.Insert(0, new ListItem("Select", "0"));
                ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) != "971") lblOptOut.Text = "";
                ViewState["TemplateID"] = null;
                ViewState["TemplateFields"] = null;
                ViewState["dtMaxLen"] = null;
                ViewState["dtTopRec"] = null;
                Session["NOOFCHARINMSG"] = null;
                rdbEntry.Checked = true;
                ViewState["SMSRATE"] = Session["RATE_SMARTSMS"];
                lblRate.Text = Convert.ToString(Session["RATE_SMARTSMS"]) + " " + paisa + " per SMS";
                PopulateSMSType();
                PopulateSender();
                PopulateTemplateID();
                StartProcess();
                divOptOut.Attributes.Add("Style", "display:none;");
                HideTemplateIdForeignAcc();

            }

            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (FileName != "")
                    {
                        Helper.Util ob = new Helper.Util();
                        if (ob.FileUploadStopped())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File upload is temporarily stopped.');", true);
                        }
                        else
                        {
                            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                            string en = Extension.ToUpper();

                            if (en.Contains("TXT"))
                                if (FileUpload1.PostedFile.ContentLength > (12 * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 6 MB');", true);
                                    lblUploading.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("CSV"))
                                if (FileUpload1.PostedFile.ContentLength > (20971520))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of 20 MB');", true);
                                    lblUploading.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("XLS"))
                                if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of 6 MB');", true);
                                    lblUploading.Text = "Upload rejected.";
                                    return;
                                }

                            string FolderPath = "XLSUpload/";
                            Session["UPLOADFILENM"] = FileName;
                            Session["UPLOADFILENMEXT"] = Extension;
                            string FolderPathOnly = Server.MapPath(FolderPath);
                            string FileNameOnly = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                            Session["FileNameOnly"] = FileNameOnly;
                            string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                            FileUpload1.SaveAs(FilePath);
                            string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);

                            if (res.Contains("RECORDCOUNT"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                                lblUploading.Text = "" + FileName + " Uploaded successfully.";
                                divFileLoader.Style.Add("display", "none");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                                lblUploading.Text = "Upload Rejected.";
                                divFileLoader.Style.Add("display", "none");
                                File.Delete(FilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }

            try
            {
                if (IsPostBack && FileUpload3.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload3.PostedFile.FileName);
                    if (FileName != "")
                    {
                        if (Convert.ToString(Session["XLUPLOADED"]) != "Y")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('First Upload Mobile Number File.');", true);
                            return;
                        }
                        Helper.Util ob = new Helper.Util();
                        if (ob.FileUploadStopped())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File upload is temporarily stopped.');", true);
                        }
                        else
                        {
                            string Extension = Path.GetExtension(FileUpload3.PostedFile.FileName);
                            string en = Extension.ToUpper();

                            if (en.Contains("TXT"))
                                if (FileUpload3.PostedFile.ContentLength > (10 * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 10 MB');", true);
                                    lblUploading3.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("CSV"))
                                if (FileUpload3.PostedFile.ContentLength > (10 * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of 10 MB');", true);
                                    lblUploading3.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("XLS"))
                                if (FileUpload3.PostedFile.ContentLength > (10 * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of 10 MB');", true);
                                    lblUploading3.Text = "Upload rejected.";
                                    return;
                                }

                            string FolderPath = "XLSUploadExlcude/";
                            Session["UPLOADFILENMXCLUDE"] = FileName;
                            Session["UPLOADFILENMEXTEXCLUDE"] = Extension;
                            string FolderPathOnly = Server.MapPath(FolderPath);
                            string FileNameOnly = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                            string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                            FileUpload3.SaveAs(FilePath);
                            string res = Import_To_Grid3(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);
                            if (res.Contains("RECORDCOUNT"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                                lblUploading3.Text = "" + FileName + " Uploaded successfully.";
                                divFileLoader.Style.Add("display", "none");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                                lblUploading3.Text = "Upload Rejected.";
                                divFileLoader.Style.Add("display", "none");
                                File.Delete(FilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in Exclude File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
            // For Media File
            if (IsPostBack && FileUpload2.PostedFile != null)
            {
                string FileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                if (FileName != "")
                {
                    //string FileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    string Extension = Path.GetExtension(FileUpload2.PostedFile.FileName);
                    string FolderPath = "MEDIAUpload/";
                    string fn = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                    string FilePath = Server.MapPath(FolderPath + fn);
                    FileUpload2.SaveAs(FilePath);
                    string ws = Convert.ToString(Session["DOMAINNAME"]);
                    //string s = create_ShortURL(string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", FolderPath + fn));
                    string s = create_ShortURL(ws + FolderPath + fn, "");
                    //if (s != "") txtMsg.Text = txtMsg.Text.Trim() + " " + string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", s);
                    if (s != "") txtMsg.Text = txtMsg.Text.Trim() + " " + ws + s;
                    //Session["SHORTURL"] = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", s);
                    if (chkMobTrack.Checked)
                        Session["SHORTURL"] = ws + s;
                    ShowMsgCharCnt();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('2 more character will be added in the link text for individual mobile numbers. Length of Message characters will be increased by 2.');", true);
                    if (s == "") pnlPopUp_URL_ModalPopupExtender.Show();
                }
            }

        }

        private void HideTemplateIdForeignAcc()
        {
            //   if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc2)
            if (Convert.ToString(ddlCCode.SelectedValue) != "91")
            {
                divTempId.Attributes.Add("class", "form-group row d-none");
                divTempsms.Attributes.Add("class", "form-group row d-none");
                if (Convert.ToString(ddlCCode.SelectedValue) == "971")
                    divOptOut.Attributes.Add("Style", "display:block;");
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Campaign"));
            }
            if (Global.openTempAc)
            {
                divTempId.Attributes.Add("class", "form-group row d-none");
                divTempsms.Attributes.Add("class", "form-group row d-none");
            }
            if (_user != "MIM2200055")
            {
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Google RCS"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Flash SMS"));
            }
            if (_user == "MIM2102290")
            {
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Premium"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Link Text"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Promotional"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Google RCS"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Flash SMS"));
            }

            if (string.IsNullOrEmpty(Convert.ToString(Session["Notice"])))
                divFooter.Attributes.Add("class", "form-group row d-none");
            else
            {
                divFooter.Attributes.Add("class", "form-group row d-block");
                lblNotice.Text = Convert.ToString(Session["Notice"]);
            }

        }
        public static List<string> CountryList()
        {
            List<string> CultureList = new List<string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(CultureList.Contains(getRegionInfo.Name)))
                {
                    CultureList.Add(getRegionInfo.EnglishName);
                }
            }
            CultureList.Sort();
            return CultureList;
        }

        protected void Imagetitlebind()
        {
            if (ddlCCode != null)
            {
                foreach (ListItem item in ddlCCode.Items)
                {
                    item.Attributes["title"] = item.Value;
                }
            }
        }

        public static string GetCountryCode(string country)
        {
            string countryCode = "";
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);
                if (region.EnglishName.ToUpper().Contains(country.ToUpper()))
                {
                    countryCode = region.TwoLetterISORegionName;
                }
            }
            return countryCode;
        }

        public string GetCode(string country)
        {
            string code = "";
            JObject weatherInfo = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Server.MapPath("~/TextFile.json")));
            code = Convert.ToString(((JToken)weatherInfo[GetCountryCode(country)])).Replace("\"", string.Empty);
            return code;
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

        public void StartProcess()
        {
            Session["XLUPLOADED"] = null;
            Session["MOBILECOUNT"] = null;
            Session["SHORTURL"] = null;
            Helper.Util ob = new Helper.Util();
            ob.DropUserTmpTable(_user);
            lblUploading.Text = "";
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
            //if (dt.Rows.Count == 1)
            //    ddlSender.SelectedIndex = 1;
            //else
            //    ddlSender.SelectedIndex = 0;
        }
        public void PopulateSMSType()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSMSType(Convert.ToString(Session["UserID"]));
            ddlSMSType.DataSource = dt;
            ddlSMSType.DataTextField = "SMSTYPE";
            ddlSMSType.DataValueField = "SMSVAL";
            ddlSMSType.DataBind();
            ddlSMSType.SelectedIndex = 0;
        }
        protected void btnAddLink_Click(object sender, EventArgs e)
        {

        }
        protected void btnExisting_Click(object sender, EventArgs e)
        {
            pnlExisting.Visible = true;
            pnlMedia.Visible = false;
            pnlNew.Visible = false;
            pnlPopUp_URL_ModalPopupExtender.Show();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            pnlExisting.Visible = false;
            pnlMedia.Visible = false;
            pnlNew.Visible = true;
            pnlPopUp_URL_ModalPopupExtender.Show();
        }
        protected void btnMedia_Click(object sender, EventArgs e)
        {
            pnlExisting.Visible = false;
            pnlMedia.Visible = true;
            pnlNew.Visible = false;
            pnlPopUp_URL_ModalPopupExtender.Show();
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {

            Helper.Util ob = new Helper.Util();
            DataTable DTSMPPAC = ob.GetUserSMPPACCOUNTCountry(Convert.ToString(Session["UserID"]), ddlCCode.SelectedValue);
            Session["DTSMPPAC"] = DTSMPPAC;

            bool res;
            if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                res = SendTemplateMSG("");
            else
                res = MsgSend("");
        }


        private bool IsPromotionalSMSTimeOver(string sch)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));

            if (dtCountry.Rows.Count > 0)
            {
                int timeDiff = Convert.ToInt32(dtCountry.Rows[0]["timedifferenceInMinute"]);
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dtCountry.Rows[0]["promoStartTime"])); //"11:59"
                TimeSpan end = TimeSpan.Parse(Convert.ToString(dtCountry.Rows[0]["promoEndTime"]));

                if (sch == "")
                {
                    TimeSpan now = DateTime.Now.AddMinutes(timeDiff).TimeOfDay;
                    if (!((now > start) && (now < end)))
                        return false;
                }
                else
                {
                    // for scheduel
                    for (int x = 0; x < liScheduleDates.Count; x++)
                    {
                        string _time = liScheduleDates[x].Substring(liScheduleDates[x].Length - 5);

                        DateTime ddddd = Convert.ToDateTime(_time).AddMinutes(timeDiff);
                        TimeSpan now = TimeSpan.Parse(ddddd.ToString("HH:mm:ss"));
                        if (!((now > start) && (now < end)))
                            return false;
                    }
                }
            }
            return true;
        }

        public bool MsgSend(string sch)
        {
            txtMsg.Text = txtMsg.Text.Replace("\r\n", "\n");
            Helper.Util ob = new Helper.Util();
            // Rachit 14 july 
            #region Country Block 
            string _smstype = ddlSMSType.SelectedValue;
            DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            string fCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsFromTime"]) : "";
            string tCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsToTime"]) : "";

            if (sch != "")
            {
                bool flagLevel1 = ValidateForCountryLevel1Schedule(dtCountry);
                if (flagLevel1 == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages should be scheduled during (" + fCtry + " - " + tCtry + ") as per country !');", true);
                    return false;
                }
                // for Campaign & Promotional
                if (_smstype == "3" || _smstype == "6")
                {
                    DataTable dtSMPPSetting = ob.GetSMPPSettingTimeZone(_smstype);
                    string fSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsFromTime"]) : "";
                    string tSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsToTime"]) : "";

                    bool flagLevel2 = ValidateForSMPPLevel2Schedule(dtSMPPSetting);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages should be scheduled during (" + fSMPP + " - " + tSMPP + ") as per Account settings !');", true);
                        return false;
                    }
                }
                else
                { // for Premimum & Link
                  // bool flagLevel1 = ValidateForCampaignCountryLevel1Schedule(dtCountry);
                    DataTable dtAcc = ob.GetSMPPAccountIdTimeZone(_user);
                    string fAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsFromTime"]) : "";
                    string tAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsToTime"]) : "";

                    bool flagLevel3 = ValidateForAccountIdLevel3Schedule(dtAcc);
                    if (flagLevel3 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message should be scheduled during (" + fAcc + " - " + tAcc + " As Per Account Setting !)');", true);
                        return false;
                    }
                }
            }
            else
            {
                bool flagLevel1 = ValidateForCountryLevel1(dtCountry);
                if (flagLevel1 == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message can be sent during (" + fCtry + " - " + tCtry + ") hours as per country !)');", true);
                    return false;
                }
                if (_smstype == "3" || _smstype == "6")
                {
                    DataTable dtSMPPSetting = ob.GetSMPPSettingTimeZone(_smstype);
                    string fSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsFromTime"]) : "";
                    string tSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsToTime"]) : "";
                    bool flagLevel2 = ValidateForSMPPLevel2(dtSMPPSetting);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message can be sent during (" + fSMPP + " - " + tSMPP + ") as per Account setting !)');", true);
                        return false;
                    }
                }
                else
                {// for Premimum & Link
                    //bool flagLevel1 = ValidateForCountryLevel1(dtCountry);
                    DataTable dtAcc = ob.GetSMPPAccountIdTimeZone(_user);
                    string fAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsFromTime"]) : "";
                    string tAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsToTime"]) : "";
                    bool flagLevel2 = ValidateForAccountIdLevel3(dtAcc);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Sent during (" + fAcc + " - " + tAcc + " As Per Account Setting !)');", true);
                        return false;
                    }
                }
            }

            #endregion
            //---------------------------------END ----------------------------//

            // Rachit -08-Apr-2022 Validate time for UAE & KSA

            if ((Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "966"))
            {
                string _senderId = ddlSender.SelectedValue;
                if (_senderId.ToUpper().StartsWith("AD-") || _senderId.ToUpper().EndsWith("-AD"))
                {
                    bool IsProm = IsPromotionalSMSTimeOver(sch);
                    if (IsProm == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('promotional sms can not be sent at current time or can not be scheduled for entered time!');", true);
                        return false;
                    }
                }
            }
            //---------------------------------END ----------------------------//


            string tmpfilenm = "";
            string filenm = "";
            string filenmext = "";
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]);
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                tmpfilenm = Convert.ToString(Session["FileNameOnly"]);
                txtMobNum.Text = "";

                if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                {
                    if (txtCampNm.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Campaign Name');", true);
                        return false;
                    }
                    if (ob.CampaignExistsForDay(_user, txtCampNm.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Name already exists for the day. Please Enter another campaign name.');", true);
                        return false;
                    }
                }

            }

            if (ddlSender.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
                return false;
            }
            if (ddlSMSType.SelectedValue == "1" || ddlSMSType.SelectedValue == "2")
            {
                int sender = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out sender);
                if (isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender can not be numeric for Premium or Link SMS');", true);
                    return false;
                }
            }

            if (ddlSMSType.SelectedValue == "6")
            {
                int sender = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out sender);
                if (!isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender should be numeric for Promotional SMS');", true);
                    return false;
                }
            }

            if (ddlSMSType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select SMS Type');", true);
                return false;
            }
            if (ddlSMSType.SelectedValue != "3")
            {
                if (ddlTempID.SelectedValue == "0" && Helper.database.TemplateIDMandatory() == "Y")
                {
                    // if (ddlCCode.SelectedValue == "91" && Convert.ToString(Session["UserID"]).ToUpper() != speceficAcc && Convert.ToString(Session["UserID"]).ToUpper() != speceficAcc2)

                    if (ddlCCode.SelectedValue == "91" && (!Global.openTempAc))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Template id');", true);
                        return false;
                    }
                }
            }


            if (chkAllowDuplicates.Checked == false && Convert.ToString(Session["XLUPLOADED"]) == "Y" && rdbImport.Checked == false)
                ob.RemoveDuplicateRowsFromTempTable(_user, filenmext);

            DataTable dt2 = ob.GetUserParameter(_user);
            string bal2 = dt2.Rows[0]["balance"].ToString();

            string country_code = ddlCCode.SelectedValue;
            string mobile = "";
            if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
            if (mobile.Trim() != "")
            {
                if (mobList.Count > 25000)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please use file upload option to send SMS to more than 25000 mobile numbers.');", true);
                    return false;
                }

                if (country_code == "91")
                {
                    int maxlen = mobList.Max(arr => arr.Length);
                    int minlen = mobList.Min(arr => arr.Length);
                    if (maxlen != minlen)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                    if (maxlen != 10 || minlen != 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                }
                else if (country_code == "971")
                {
                    int maxlen = mobList.Max(arr => arr.Length);
                    int minlen = mobList.Min(arr => arr.Length);
                    if (maxlen != minlen)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 9 digits ]');", true);
                        return false;
                    }
                    if (maxlen != 9 || minlen != 9)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 9 digits ]');", true);
                        return false;
                    }
                    bool result = mobList.All(o => o.StartsWith("5"));
                    if (result == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be starts with 5');", true);
                        return false;
                    }
                }
                //  if (maxlen == 10) country_code = "91";
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers first.');", true);
                    return false;
                }
            }



            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text first.');", true);
                return false;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            //if specific user has already the long url, then don't reduce balance -
            //if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])) && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must be present in Message Text for SMS Type - Smart SMS .');", true);
                return false;
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if ((ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must be present in Message Text as you have inserted URL .');", true);
                        return false;
                    }
                }
            //if (Convert.ToInt64(Session["URLBALANCE"]) <= 0)
            //{
            //    //mobtrk.bal = "0";
            //    Session["URLBALANCE"] = "0";
            //    lblURLbal.Text = "0";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL Balance Expired.');", true);
            //    return;
            //}

            string shortURL = "";
            int shortURLId = 0;
            string ws = "";
            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
            {
                shortURL = Convert.ToString(Session["SHORTURL"]);
                //ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"); 
                ws = Convert.ToString(Session["DOMAINNAME"]);
                //Convert.ToString(ConfigurationManager.AppSettings["WebSite"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"]) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                    {
                        shortURL = Convert.ToString(Session["SHORTURL"]);
                        //ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"); 
                        ws = Convert.ToString(Session["DOMAINNAME"]);
                        //Convert.ToString(ConfigurationManager.AppSettings["WebSite"]);
                        shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
                    }
                }

            Int32 cnt = 0;
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                cnt = Convert.ToInt32(Session["MOBILECOUNT"]);
            else
                cnt = mobList.Count;

            int noofsms = 0;
            bool ucs2 = false;
            //txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            if (chkOptOut.Checked) txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            txtMsg.Text = txtMsg.Text.Trim();
            string q = txtMsg.Text.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtMsg.Text.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;

            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"]))) qlen += 2;
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                        qlen += 2;
                }

            ucs2 = false;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 160) noofsms = 2;
            if (qlen > 306) noofsms = 3;
            if (qlen > 459) noofsms = 4;
            if (qlen > 612) noofsms = 5;
            if (qlen > 765) noofsms = 6;
            if (qlen > 918) noofsms = 7;
            if (qlen > 1071) noofsms = 8;
            if (qlen > 1224) noofsms = 9;
            if (qlen > 1377) noofsms = 10;
            if (qlen > 1530) noofsms = 11;
            if (qlen > 1683) noofsms = 12;

            if (q.Any(c => c > 126))
            {
                // unicode = y
                ucs2 = true;
                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
            }

            //}
            //check whether balance is available or not ---
            //int noofsms = (txtMsg.Text.Trim().Length / 160);
            //if (txtMsg.Text.Trim().Length % 160 > 0) noofsms++;

            Int32 noofmessages = noofsms * cnt;

            //update balance
            double rate = 0;
            rate = (ddlSMSType.SelectedValue == "1" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
            rate = (ddlSMSType.SelectedValue == "2" ? Convert.ToDouble(Session["RATE_SMARTSMS"]) : rate);
            rate = (ddlSMSType.SelectedValue == "3" ? Convert.ToDouble(Session["RATE_CAMPAIGN"]) : rate);
            rate = (ddlSMSType.SelectedValue == "4" ? Convert.ToDouble(Session["RATE_OTP"]) : rate);
            rate = (ddlSMSType.SelectedValue == "6" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);

            double bal = Convert.ToDouble(bal2) * 1000;
            if (bal - Convert.ToDouble(noofmessages * (rate * 10)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                return false;
            }
            //bal = bal - Convert.ToDouble(cnt * (rate * 10));
            //bal = Math.Round((bal / 1000), 3);

            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);
            //   Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);

            ob.noof_message = noofmessages;
            ob.msg_rate = rate;
            double PrevBalance = Convert.ToDouble(bal2);
            if (sch != "")
            {
                if (bal - Convert.ToDouble(noofmessages * (liScheduleDates.Count) * (rate * 10)) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                    return false;
                }

                string schBal = "";
                ob.smscount = liScheduleDates.Count;

                //for (int x = 1; x <= liScheduleDates.Count; x++)
                //{
                //    schBal = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
                //}
                //Session["SMSBAL"] = schBal;

            }
            else
            {
                ob.smscount = 1;
                //Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            }

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            double AvailableBalance = ob.CalculateAmount(UserID, cnt, rate, 1);

            //txtMsg.Text = txtMsg.Text.Replace("@", "|");
            //Get SMS Accounts with Load and accordingly bifurcate no. of sms
            //DataTable dt = ob.GetSMPP_Account_Load();
            //dt.Columns.Add("nos", typeof(Int32));
            DataTable dtSMPPAC = new DataTable();


            string templID = "";
            if (ddlTempID.SelectedValue != "0") templID = ddlTempID.SelectedValue;
            //  if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc2)
            if (Global.openTempAc)
            {
                templID = ob.GetUniversalTemplateId();
                //180
                if (qlen > 180)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message has a maximum limit 180 characters');", true);
                    return false;
                }
            }
            string tblname = Convert.ToString(Session["UserId"]) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string tblname_Ac = tblname + "_AC";
            string user = "tmp_" + UserID;

            string smsType = ddlSMSType.SelectedValue;
            if (string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])) && smsType == "2")
                smsType = "1";

            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {

                    string scheduleDate = liScheduleDates.FirstOrDefault();
                    // ob.Schedule_SMS_BULK(UserID, txtMsg.Text.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, mobList, "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);
                    ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
                    string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,isschedule,scheduletime,ccode,fileext,rate,smstype,methodname,shortURLId,shortURL,domainname,ucs2,noofsms,campname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"'" +
                    ",'" + ddlSender.SelectedValue + @"',0,Null,1,'" + scheduleDate + @"','" + country_code + @"','" + filenmext + @"'," + rate + ",'" + smsType + @"','Schedule_SMS_BULK', '" + shortURLId + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"'," + noofsms + ",'" + txtCampNm.Text.Trim() + @"' " +
                     " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                    if (lstMappedFields != null)
                    {
                        if (lstMappedFields.Items.Count > 0)
                        {
                            sq = sq + @" insert into MapFields (id,mapfieldname) ";
                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                            {
                                if (i == 0)
                                {
                                    sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                                else
                                {
                                    sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                            }
                        }
                    }

                    dbB4Link.ExecuteNonQuery(sq);
                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                        foreach (var m in mobList)
                        {
                            database.ExecuteNonQuery(" Insert into " + user + @" values ('" + m + "')");
                        }
                        database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");

                        // foreach (string scheduleDate in liScheduleDates)
                        {


                            string campName = "Manual";
                            string scheduleDate = liScheduleDates.FirstOrDefault();
                            // ob.Schedule_SMS_BULK(UserID, txtMsg.Text.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, mobList, "MANUAL", templID, country_code, PrevBalance, AvailableBalance);
                            ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
                            string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,isschedule,scheduletime,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"'," +
                    "'" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",1,'" + scheduleDate + "','" + filenmext + "'," + rate + ",'Schedule_SMS_BULK'" +
                    " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";
                            if (lstMappedFields != null)
                            {
                                if (lstMappedFields.Items.Count > 0)
                                {
                                    sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                    for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                        else
                                        {
                                            sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                    }
                                }
                            }

                            dbB4Link.ExecuteNonQuery(sq);
                        }
                        //ob.Schedule_SMS_BULK(UserID, txtMsg.Text, ddlSender.Text, sch, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, "", ucs2, mobList, "MANUAL", templID, country_code);
                    }
                    else
                    {

                        if (ddlSMSType.SelectedValue == "6") //03 / 03 / 2022
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtCm = ob.GetPromotionAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtCm.Rows.Count; j++)
                                smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }

                        else if (ddlSMSType.SelectedValue == "3")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "Y";
                            DataTable dtCm = ob.GetCampaignAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtCm.Rows.Count; j++)
                                smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else if (ddlSMSType.SelectedValue == "7")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtRCS = ob.GetGoogleRCSAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtRCS.Rows.Count; j++)
                                smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else if (ddlSMSType.SelectedValue == "8")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtRCS = ob.GetFlashSMSAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtRCS.Rows.Count; j++)
                                smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else
                            dtSMPPAC = (DataTable)Session["DTSMPPAC"];

                        foreach (var m in mobList)
                        {

                            string msg = txtMsg.Text.Trim();
                            string shurl = "";
                            string mseg = "";
                            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                            {
                                mseg = ob.NewSegment8();
                                shurl = ws + mseg;
                                msg = msg.Replace(shortURL, shurl);
                            }
                            // check for blaclist no mob entry 15/09/2021
                            int chk = int.Parse(Convert.ToString(database.GetScalarValue("select count(1) from globalBlackListNo where mobile='" + m + "'")));
                            if (chk == 0)
                            {
                                //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                                string resp = "";
                                //ob.SendURL_SMS(UserID, country_code + m, msg, ddlSender.Text);
                                string scheduleDate = liScheduleDates.FirstOrDefault();
                                //ob.Schedule_SMS(UserID, m, msg.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, mseg, rate, ddlSMSType.SelectedValue, dtSMPPAC, ucs2, templID, country_code);
                                ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);

                                ob.B4SEND_Schedule_SMS(UserID, m, msg, ddlSender.Text, sch, shortURLId, shortURL, ws, mseg, rate, smsType, dtSMPPAC, ucs2, templID, country_code);
                                //if (ddlSMSType.SelectedValue == "2")
                                //    ob.SaveURL_MOBILE(UserID, shortURLId, country_code + m, mseg, resp);
                            }
                        }
                    }
                }
            }
            else
            {
                if (cnt > 10) // Test template before sending
                {
                    // Global.Istemplatetest = ob.TestSmsbeforeSend(_user, ddlTempID.SelectedValue, txtMsg.Text, ddlSender.SelectedValue, Convert.ToString(Session["PEID"]));
                }
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                    {
                        //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text.Trim(), ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);

                        string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'" +
                    " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";
                        if (lstMappedFields != null)
                        {
                            if (lstMappedFields.Items.Count > 0)
                            {
                                sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                    else
                                    {
                                        sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                }
                            }
                        }

                        dbB4Link.ExecuteNonQuery(sq);
                    }
                    else
                    {
                        if (Session["SHORTURL"] != null)
                        {
                            if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                            {
                                if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                                {
                                    string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'" +
                    " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";
                                    if (lstMappedFields != null)
                                    {
                                        if (lstMappedFields.Items.Count > 0)
                                        {
                                            sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                                else
                                                {
                                                    sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                            }
                                        }
                                    }

                                    dbB4Link.ExecuteNonQuery(sq);
                                    //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text.Trim(), ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);

                                }
                            }
                            else
                            {
                                string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0," +
                    "Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'InsertSMSrecordsFromUSERTMP'" +
                    " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";
                                if (lstMappedFields != null)
                                {
                                    if (lstMappedFields.Items.Count > 0)
                                    {
                                        sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                        for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                            else
                                            {
                                                sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                        }
                                    }
                                }

                                dbB4Link.ExecuteNonQuery(sq);
                                //ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);

                            }
                        }
                        else
                        {

                            string sq = @"DECLARE @maxid int 
                    SET @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,
                    '" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'InsertSMSrecordsFromUSERTMP' " +
                    " select * into " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                            if (lstMappedFields != null)
                            {
                                if (lstMappedFields.Items.Count > 0)
                                {
                                    sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                    for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                        else
                                        {
                                            sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                    }
                                }
                            }

                            dbB4Link.ExecuteNonQuery(sq);
                            //ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);

                        }
                    }
                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                        foreach (var m in mobList)
                        {
                            database.ExecuteNonQuery(" Insert into " + user + @" values ('" + country_code + m + "')");
                        }
                        database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");


                        string campName = "Manual";
                        if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                        {

                            string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url' " +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + @" ";
                            if (lstMappedFields != null)
                            {
                                if (lstMappedFields.Items.Count > 0)
                                {
                                    sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                    for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                        else
                                        {
                                            sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                    }
                                }
                            }
                            dbB4Link.ExecuteNonQuery(sq);
                            //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text.Trim(), ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, mobList, "MANUAL", "", templID, country_code);
                        }
                        else
                        {
                            if (Session["SHORTURL"] != null)
                            {
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                                    {
                                        string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'" +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                                        if (lstMappedFields != null)
                                        {
                                            if (lstMappedFields.Items.Count > 0)
                                            {
                                                sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                                for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                                {
                                                    if (i == 0)
                                                    {
                                                        sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                    }
                                                    else
                                                    {
                                                        sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                    }
                                                }
                                            }
                                        }

                                        dbB4Link.ExecuteNonQuery(sq);
                                        //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text.Trim(), ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, mobList, "MANUAL", "", templID, country_code);

                                    }
                                }
                                else
                                {
                                    string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0 " +
                    ",Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'" +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                                    if (lstMappedFields != null)
                                    {
                                        if (lstMappedFields.Items.Count > 0)
                                        {
                                            sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                                else
                                                {
                                                    sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                            }
                                        }
                                    }

                                    dbB4Link.ExecuteNonQuery(sq);
                                    //ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code);

                                }
                            }
                            else
                            {

                                string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url' " +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                                if (lstMappedFields != null)
                                {
                                    if (lstMappedFields.Items.Count > 0)
                                    {
                                        sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                        for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                            else
                                            {
                                                sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                        }
                                    }
                                }

                                dbB4Link.ExecuteNonQuery(sq);
                                //ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code);

                            }
                        }
                    }
                    else
                    {

                        if (ddlSMSType.SelectedValue == "6") //03 / 03 / 2022
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtCm = ob.GetPromotionAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtCm.Rows.Count; j++)
                                smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else if (ddlSMSType.SelectedValue == "3")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "Y";
                            DataTable dtCm = ob.GetCampaignAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtCm.Rows.Count; j++)
                                smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else if (ddlSMSType.SelectedValue == "7")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtRCS = ob.GetGoogleRCSAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtRCS.Rows.Count; j++)
                                smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else if (ddlSMSType.SelectedValue == "8")
                        {
                            dtSMPPAC.Columns.Add("DLTNO");
                            dtSMPPAC.Columns.Add("GSM");
                            dtSMPPAC.Columns.Add("smppaccountid");
                            dtSMPPAC.Columns.Add("smppaccountidall");
                            DataRow drn = dtSMPPAC.NewRow();
                            drn["DLTNO"] = "";
                            drn["GSM"] = "N";
                            DataTable dtRCS = ob.GetFlashSMSAccounts();
                            string smppac = "'0'";
                            for (int j = 0; j < dtRCS.Rows.Count; j++)
                                smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                            drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                            drn["smppaccountidall"] = smppac;
                            dtSMPPAC.Rows.Add(drn);
                        }
                        else
                            dtSMPPAC = (DataTable)Session["DTSMPPAC"];


                        string sql = @" Insert into SMSFILEUPLOAD (USERID,RECCOUNT,senderid,campaignname,smsrate,shortURLId,countrycode)
                                   values ('" + UserID + "','" + mobList.Count.ToString() + "','" + ddlSender.SelectedValue + "','Manual','" + rate + "','" + Convert.ToString(shortURLId) + "','" + country_code + "') " +
                                "  select max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; ";
                        string fileId = Convert.ToString(database.GetScalarValue(sql));

                        foreach (var m in mobList)
                        {
                            string msg = txtMsg.Text;
                            string shurl = "";
                            string mseg = "";
                            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                            {
                                mseg = ob.NewSegment8();
                                shurl = ws + mseg;
                                msg = msg.Replace(shortURL, shurl);
                            }
                            if (Session["SHORTURL"] != null)
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                                    {
                                        mseg = ob.NewSegment8();
                                        shurl = ws + mseg;
                                        msg = msg.Replace(shortURL, shurl);
                                    }
                                }
                            //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                            string resp = "";
                            // check for mob entry blaclistno rabi 15/09/2021
                            int chk = int.Parse(Convert.ToString(database.GetScalarValue("select count(1) from globalBlackListNo where mobile='" + m + "'")));
                            if (chk == 0)
                            {

                                ob.B4SEND_SendURL_SMS(UserID, m, msg.Trim(), ddlSender.Text, dtSMPPAC, ucs2, bulk, rate, noofsms, templID, country_code, smsType, fileId);
                                if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                                    ob.SaveURL_MOBILE(UserID, shortURLId, m, mseg, resp, country_code, fileId, templID);
                                if (Session["SHORTURL"] != null)
                                    if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                    {
                                        if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") ob.SaveURL_MOBILE(UserID, shortURLId, m, mseg, resp, country_code, fileId, templID);
                                    }

                            }

                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send_sms_u_B4Send1.aspx';", true);
            }
            return true;
        }

        public bool SendTemplateMSG(string sch)
        {
            //if (ddlSMSType.SelectedValue == "2")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Link Text type SMS not allowed for Template Message');", true);
            //    return false;
            //}
            Helper.Util ob = new Helper.Util();
            txtMsg.Text = txtMsg.Text.Replace("\r\n", "\n");

            // Rachit 14 july 
            #region Country Block 
            string _smstype = ddlSMSType.SelectedValue;
            DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            string fCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsFromTime"]) : "";
            string tCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsToTime"]) : "";

            if (sch != "")
            {
                bool flagLevel1 = ValidateForCountryLevel1Schedule(dtCountry);
                if (flagLevel1 == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages should be scheduled during (" + fCtry + " - " + tCtry + ") as per country !');", true);
                    return false;
                }
                // for Campaign & Promotional
                if (_smstype == "3" || _smstype == "6")
                {
                    DataTable dtSMPPSetting = ob.GetSMPPSettingTimeZone(_smstype);
                    string fSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsFromTime"]) : "";
                    string tSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsToTime"]) : "";

                    bool flagLevel2 = ValidateForSMPPLevel2Schedule(dtSMPPSetting);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages should be scheduled during (" + fSMPP + " - " + tSMPP + ") as per Account settings !');", true);
                        return false;
                    }
                }
                else
                { // for Premimum & Link
                  // bool flagLevel1 = ValidateForCampaignCountryLevel1Schedule(dtCountry);
                    DataTable dtAcc = ob.GetSMPPAccountIdTimeZone(_user);
                    string fAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsFromTime"]) : "";
                    string tAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsToTime"]) : "";

                    bool flagLevel3 = ValidateForAccountIdLevel3Schedule(dtAcc);
                    if (flagLevel3 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Schedule between working hours Only (" + fAcc + " - " + tAcc + " As Per Setting !)');", true);
                        return false;
                    }
                }
            }
            else
            {
                bool flagLevel1 = ValidateForCountryLevel1(dtCountry);
                if (flagLevel1 == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message can be sent during (" + fCtry + " - " + tCtry + ") hours as per country !)');", true);
                    return false;
                }
                if (_smstype == "3" || _smstype == "6")
                {
                    DataTable dtSMPPSetting = ob.GetSMPPSettingTimeZone(_smstype);
                    string fSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsFromTime"]) : "";
                    string tSMPP = dtSMPPSetting.Rows.Count > 0 ? Convert.ToString(dtSMPPSetting.Rows[0]["smsToTime"]) : "";
                    bool flagLevel2 = ValidateForSMPPLevel2(dtSMPPSetting);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message can be sent during (" + fSMPP + " - " + tSMPP + ") as per Account setting !)');", true);
                        return false;
                    }
                }
                else
                {// for Premimum & Link
                    //bool flagLevel1 = ValidateForCountryLevel1(dtCountry);
                    DataTable dtAcc = ob.GetSMPPAccountIdTimeZone(_user);
                    string fAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsFromTime"]) : "";
                    string tAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsToTime"]) : "";
                    bool flagLevel2 = ValidateForAccountIdLevel3(dtAcc);
                    if (flagLevel2 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Sent during (" + fAcc + " - " + tAcc + " As Per Account Setting !)');", true);
                        return false;
                    }
                }
            }

            #endregion
            //---------------------------------END ----------------------------//


            // Rachit -08-Apr-2022 Validate time for UAE & KSA

            if ((Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "966"))
            {
                string _senderId = ddlSender.SelectedValue;
                if (_senderId.ToUpper().StartsWith("AD-") || _senderId.ToUpper().EndsWith("-AD"))
                {
                    bool IsProm = IsPromotionalSMSTimeOver(sch);
                    if (IsProm == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('promotional sms time is over !');", true);
                        return false; ;
                    }
                }
            }

            //---------------------------------END ----------------------------//

            string filenm = "";
            string filenmext = "";
            string tmpfilenm = "";
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]);
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                tmpfilenm = Convert.ToString(Session["FileNameOnly"]);
                txtMobNum.Text = "";

                if (rdbUpload.Checked == true || rdbPersonal.Checked == true)
                {
                    if (txtCampNm.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Campaign Name');", true);
                        return false;
                    }
                    if (ob.CampaignExistsForDay(_user, txtCampNm.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Name already exists for the day. Please Enter another campaign name.');", true);
                        return false;
                    }
                }
            }

            if (ddlSender.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
                return false;
            }
            if (ddlSMSType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select SMS Type');", true);
                return false;
            }
            if (ddlSMSType.SelectedValue == "1" || ddlSMSType.SelectedValue == "2")
            {
                int sender = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out sender);
                if (isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender can not be numeric for Premium or Link SMS');", true);
                    return false;
                }
            }
            if (ddlSMSType.SelectedValue == "6")
            {
                int sender = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out sender);
                if (!isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender should be numeric for Promotional SMS');", true);
                    return false;
                }
            }


            //if (ddlSMSType.SelectedValue != "3")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Template id');", true);
            //    return false;
            //}

            if (chkAllowDuplicates.Checked == false && Convert.ToString(Session["XLUPLOADED"]) == "Y" && rdbImport.Checked == false)
                ob.RemoveDuplicateRowsFromTempTable(_user, filenmext);

            DataTable dt2 = ob.GetUserParameter(_user);
            string bal2 = dt2.Rows[0]["balance"].ToString();

            string country_code = ddlCCode.SelectedValue;
            string mobile = "";
            //if (txtMobNum.Text != "") mobile = txtMobNum.Text;
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            if (mobile.Trim() != "")
            {
                if (country_code == "91")
                {
                    int maxlen = mobList.Max(arr => arr.Length);
                    int minlen = mobList.Min(arr => arr.Length);
                    if (maxlen != minlen)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                    if (maxlen != 10 || minlen != 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                }
                //if (maxlen == 10) country_code = "91";
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers first.');", true);
                    return false;
                }
            }

            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text first.');", true);
                return false;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            //if specific user has already the long url, then don't reduce balance -
            //if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])) && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
            {
                //if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must be present in Message Text for SMS Type - Smart SMS .');", true);
                return false;
            }

            //if (Convert.ToInt64(Session["URLBALANCE"]) <= 0)
            //{
            //    //mobtrk.bal = "0";
            //    Session["URLBALANCE"] = "0";
            //    lblURLbal.Text = "0";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL Balance Expired.');", true);
            //    return;
            //}

            string shortURL = "";
            int shortURLId = 0;
            string ws = "";
            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])))
            {
                shortURL = Session["SHORTURL"].ToString();
                //ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"); 
                ws = Convert.ToString(Session["DOMAINNAME"]);
                //Convert.ToString(ConfigurationManager.AppSettings["WebSite"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                    {
                        shortURL = Session["SHORTURL"].ToString();
                        //ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"); 
                        ws = Convert.ToString(Session["DOMAINNAME"]);
                        //Convert.ToString(ConfigurationManager.AppSettings["WebSite"]);
                        shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
                    }
                }
            Int32 cnt = 0;
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                cnt = Convert.ToInt32(Session["MOBILECOUNT"]);
            else
                cnt = mobList.Count;

            int noofsms = 0;
            bool ucs2 = false;
            //txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            if (chkOptOut.Checked) txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            txtMsg.Text = txtMsg.Text.Trim();

            string q = txtMsg.Text.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtMsg.Text.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;

            if (ddlSMSType.SelectedValue == "2" && !string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"]))) qlen += 2;

            //if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "6") qlen += 2;

            ucs2 = false;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 160) noofsms = 2;
            if (qlen > 306) noofsms = 3;
            if (qlen > 459) noofsms = 4;
            if (qlen > 612) noofsms = 5;
            if (qlen > 765) noofsms = 6;
            if (qlen > 918) noofsms = 7;
            if (qlen > 1071) noofsms = 8;
            if (qlen > 1224) noofsms = 9;
            if (qlen > 1377) noofsms = 10;
            if (qlen > 1530) noofsms = 11;
            if (qlen > 1683) noofsms = 12;
            //}

            if (q.Any(c => c > 126))
            {
                // unicode = y
                ucs2 = true;
                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
            }
            //check whether balance is available or not ---
            //int noofsms = (txtMsg.Text.Trim().Length / 160);
            //if (txtMsg.Text.Trim().Length % 160 > 0) noofsms++;

            Int32 noofmessages = noofsms * cnt;

            //update balance
            double rate = 0;
            rate = (ddlSMSType.SelectedValue == "1" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
            rate = (ddlSMSType.SelectedValue == "2" ? Convert.ToDouble(Session["RATE_SMARTSMS"]) : rate);
            rate = (ddlSMSType.SelectedValue == "3" ? Convert.ToDouble(Session["RATE_CAMPAIGN"]) : rate);
            rate = (ddlSMSType.SelectedValue == "4" ? Convert.ToDouble(Session["RATE_OTP"]) : rate);
            rate = (ddlSMSType.SelectedValue == "6" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);

            double bal = Convert.ToDouble(bal2) * 1000;
            if (bal - Convert.ToDouble(noofmessages * (rate * 10)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                return false;
            }
            //bal = bal - Convert.ToDouble(cnt * (rate * 10));
            //bal = Math.Round((bal / 1000), 3);

            // Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);
            double PrevBalance = Convert.ToDouble(bal2);
            ob.noof_message = noofmessages;
            ob.msg_rate = rate;
            if (sch != "")
            {
                if (bal - Convert.ToDouble(noofmessages * (liScheduleDates.Count) * (rate * 10)) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                    return false;
                }
                string schBal = "";
                //for (int x = 1; x <= liScheduleDates.Count; x++)
                //{
                //    schBal = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
                //}
                //Session["SMSBAL"] = schBal;
                ob.smscount = liScheduleDates.Count;
            }
            else
            {
                ob.smscount = 1;
                //  Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            }

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            double AvailableBalance = ob.CalculateAmount(UserID, cnt, rate, 1);

            //txtMsg.Text = txtMsg.Text.Replace("@", "|");
            //Get SMS Accounts with Load and accordingly bifurcate no. of sms
            //DataTable dt = ob.GetSMPP_Account_Load();
            //dt.Columns.Add("nos", typeof(Int32));
            //DataTable dtSMPPAC = (DataTable)Session["DTSMPPAC"];
            DataTable dtSMPPAC = new DataTable();

            //if (ddlSMSType.SelectedValue == "6")
            //{
            //    dtSMPPAC.Columns.Add("DLTNO");
            //    dtSMPPAC.Columns.Add("GSM");
            //    dtSMPPAC.Columns.Add("smppaccountid");
            //    dtSMPPAC.Columns.Add("smppaccountidall");
            //    DataRow drn = dtSMPPAC.NewRow();
            //    drn["DLTNO"] = "";
            //    drn["GSM"] = "N";
            //    DataTable dtCm = ob.GetPromotionAccounts();
            //    string smppac = "'0'";
            //    for (int j = 0; j < dtCm.Rows.Count; j++)
            //        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
            //    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
            //    drn["smppaccountidall"] = smppac;
            //    dtSMPPAC.Rows.Add(drn);
            //}  // at service

            //else if (ddlSMSType.SelectedValue == "3")
            //{
            //    dtSMPPAC.Columns.Add("DLTNO");
            //    dtSMPPAC.Columns.Add("GSM");
            //    dtSMPPAC.Columns.Add("smppaccountid");
            //    dtSMPPAC.Columns.Add("smppaccountidall");
            //    DataRow drn = dtSMPPAC.NewRow();
            //    drn["DLTNO"] = "";
            //    drn["GSM"] = "Y";
            //    DataTable dtCm = ob.GetCampaignAccounts();
            //    string smppac = "'0'";
            //    for (int j = 0; j < dtCm.Rows.Count; j++)
            //        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
            //    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
            //    drn["smppaccountidall"] = smppac;
            //    dtSMPPAC.Rows.Add(drn);
            //}
            //else if (ddlSMSType.SelectedValue == "7")
            //{
            //    dtSMPPAC.Columns.Add("DLTNO");
            //    dtSMPPAC.Columns.Add("GSM");
            //    dtSMPPAC.Columns.Add("smppaccountid");
            //    dtSMPPAC.Columns.Add("smppaccountidall");
            //    DataRow drn = dtSMPPAC.NewRow();
            //    drn["DLTNO"] = "";
            //    drn["GSM"] = "N";
            //    DataTable dtRCS = ob.GetGoogleRCSAccounts();
            //    string smppac = "'0'";
            //    for (int j = 0; j < dtRCS.Rows.Count; j++)
            //        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
            //    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
            //    drn["smppaccountidall"] = smppac;
            //    dtSMPPAC.Rows.Add(drn);
            //}
            //else if (ddlSMSType.SelectedValue == "8")
            //{
            //    dtSMPPAC.Columns.Add("DLTNO");
            //    dtSMPPAC.Columns.Add("GSM");
            //    dtSMPPAC.Columns.Add("smppaccountid");
            //    dtSMPPAC.Columns.Add("smppaccountidall");
            //    DataRow drn = dtSMPPAC.NewRow();
            //    drn["DLTNO"] = "";
            //    drn["GSM"] = "N";
            //    DataTable dtRCS = ob.GetFlashSMSAccounts();
            //    string smppac = "'0'";
            //    for (int j = 0; j < dtRCS.Rows.Count; j++)
            //        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
            //    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
            //    drn["smppaccountidall"] = smppac;
            //    dtSMPPAC.Rows.Add(drn);
            //}
            //else
            //    dtSMPPAC = (DataTable)Session["DTSMPPAC"];

            DataTable dtCols = (DataTable)ViewState["dtMaxLen"];
            List<string> tempFields = (List<string>)ViewState["TemplateFields"];
            string templateID = Convert.ToString(ViewState["TemplateID"]);

            // if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc || Convert.ToString(Session["UserID"]).ToUpper() == speceficAcc2)
            if (Global.openTempAc)
            {
                //    templateID = ob.GetUniversalTemplateId();
                if (qlen > 180)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message has a maximum limit 180 characters');", true);
                    return false;
                }
            }

            string smsType = ddlSMSType.SelectedValue;
            if (string.IsNullOrEmpty(Convert.ToString(Session["SHORTURL"])) && smsType == "2")
                smsType = "1";

            string tblname = Convert.ToString(Session["UserId"]) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string tblname_Ac = tblname + "Ac";
            string user = "tmp_" + UserID;
            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    // foreach (string scheduleDate in liScheduleDates)
                    {
                        string scheduleDate = liScheduleDates.FirstOrDefault();
                        // ob.InsertTemplateSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, dtCols, lstMappedFields, txtCampNm.Text, ucs2, rate, noofsms, scheduleDate, tempFields, templateID, country_code, shortURL, ws, shortURLId, PrevBalance, AvailableBalance, tmpfilenm);

                        string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,IsSchedule,ScheduleTime)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templateID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,
                    '" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'InsertTemplateSMSrecordsFromUSERTMP',1,'" + scheduleDate + "' " +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + @" ";

                        if (lstMappedFields != null)
                        {
                            if (lstMappedFields.Items.Count > 0)
                            {
                                sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                    else
                                    {
                                        sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                }
                            }
                        }

                        dbB4Link.ExecuteNonQuery(sq);
                        ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
                    }
                    //  ob.InsertTemplateSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, dtCols, lstMappedFields, "", ucs2, rate, noofsms, sch, tempFields, templateID, country_code, shortURL, ws, shortURLId);

                }
            }
            else
            {
                if (cnt > 10) // Test- Template befor sending 
                {
                    //Global.Istemplatetest = ob.TestSmsbeforeSend(_user, templateID, txtMsg.Text.Trim(), ddlSender.Text, Convert.ToString(Session["PEID"]));
                }
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    //if (ddlSMSType.SelectedValue == "2")
                    //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC,"",ucs2,noofsms, mobList, "");
                    //else
                    // ob.InsertTemplateSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, dtCols, lstMappedFields, txtCampNm.Text, ucs2, rate, noofsms, "", tempFields, templateID, country_code, shortURL, ws, shortURLId, PrevBalance, AvailableBalance, tmpfilenm);
                    string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From FileProcess),0)+1
                    INSERT INTO FileProcess(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm.Replace("'", "''") + @"' ,'" + tblname + @"','" + cnt + @"','" + templateID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,
                    '" + country_code + @"','" + smsType + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext.Replace("'", "''") + "'," + rate + ",'InsertTemplateSMSrecordsFromUSERTMP' " +
                    " SELECT * INTO " + tblname + @" from EMIMPANEL.dbo." + user + " ";
                    if (lstMappedFields != null)
                    {
                        if (lstMappedFields.Items.Count > 0)
                        {
                            sq = sq + @" insert into MapFields (id,mapfieldname) ";
                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                            {
                                if (i == 0)
                                {
                                    sq = sq + @" select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                                else
                                {
                                    sq = sq + @" Union select @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                            }
                        }
                    }

                    dbB4Link.ExecuteNonQuery(sq);

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send_sms_u_B4Send1.aspx';", true);
            }
            return true;
        }

        private bool ValidateForCountryLevel1(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsFromTime"])); //"11:59"
                TimeSpan end = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsToTime"]));
                TimeSpan now = DateTime.Now.TimeOfDay;
                if ((now > start) && (now < end)) return true;
            }
            return false;
        }
        private bool ValidateForCountryLevel1Schedule(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsFromTime"])); //"11:59"
                TimeSpan end = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsToTime"]));
                // TimeSpan now = DateTime.Now.TimeOfDay;
                for (int x = 0; x < liScheduleDates.Count; x++)
                {
                    string _time = liScheduleDates[x].Substring(liScheduleDates[x].Length - 5);
                    TimeSpan now = TimeSpan.Parse(_time);
                    if (!((now > start) && (now < end)))
                        return false;
                }
            }
            return true;
        }

        private bool ValidateForSMPPLevel2(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsFromTime"])); //"11:59"
                TimeSpan end = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsToTime"]));
                TimeSpan now = DateTime.Now.TimeOfDay;
                if ((now > start) && (now < end)) return true;
            }
            return false;
        }
        private bool ValidateForSMPPLevel2Schedule(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsFromTime"])); //"11:59"
                TimeSpan end = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsToTime"]));

                for (int x = 0; x < liScheduleDates.Count; x++)
                {
                    string _time = liScheduleDates[x].Substring(liScheduleDates[x].Length - 5);
                    TimeSpan now = TimeSpan.Parse(_time);
                    if (!((now > start) && (now < end)))
                        return false;
                }
            }
            return true;
        }

        private bool ValidateForAccountIdLevel3(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TimeSpan start = TimeSpan.Parse(Convert.ToString(dr["smsFromTime"])); //"11:59"
                    TimeSpan end = TimeSpan.Parse(Convert.ToString(dr["smsToTime"]));
                    TimeSpan now = DateTime.Now.TimeOfDay;
                    if (!((now > start) && (now < end))) return false;
                }
            }
            return true;
        }
        private bool ValidateForAccountIdLevel3Schedule(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TimeSpan start = TimeSpan.Parse(Convert.ToString(dr["smsFromTime"])); //"11:59"
                    TimeSpan end = TimeSpan.Parse(Convert.ToString(dr["smsToTime"]));
                    for (int x = 0; x < liScheduleDates.Count; x++)
                    {
                        string _time = liScheduleDates[x].Substring(liScheduleDates[x].Length - 5);
                        TimeSpan now = TimeSpan.Parse(_time);
                        if (!((now > start) && (now < end)))
                            return false;
                    }
                }
            }
            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("send_sms_u_B4Send1.aspx");
        }
        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            ClearScheduleDateTime();
            pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.ID != "btnUploadMedia")
            {
                //if (FileUpload1.HasFile)
                //{
                //    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                //    string FolderPath = "XLSUpload/";

                //    string FilePath = Server.MapPath(FolderPath + DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension);
                //    FileUpload1.SaveAs(FilePath);
                //    Import_To_Grid(FilePath, Extension, "Yes");
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                //}
            }
            else
            {
                if (FileUpload2.HasFile)
                {

                }
            }

        }
        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            if (ddlSender.SelectedValue == "0")   /* rabi 15 july 21*/
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);

                ddlSender.Focus();
                return "";
            }
            string conStr = "";
            string SheetName = "";
            DataTable dt = new DataTable();
            if (Extension.ToLower().Contains(".xls"))
            {
                #region <Commented>
                /*
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();

                cmdExcel.Connection = connExcel;
                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();
                */

                /*
                OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
                String strExtendedProperties = String.Empty;
                sbConnection.DataSource = FilePath;
                if (Path.GetExtension(FilePath).Equals(".xls"))//for 97-03 Excel file
                {
                    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                    strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
                }
                else if (Path.GetExtension(FilePath).Equals(".xlsx"))  //for 2007 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
                }
                sbConnection.Add("Extended Properties", strExtendedProperties);
                List<string> listSheet = new List<string>();
                using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
                {
                    conn.Open();
                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    SheetName = dtSheet.Rows[0]["TABLE_NAME"].ToString();
                }
                */
                #endregion
            }
            SheetName = "Sheet1$";
            #region < Commented 2 >
            //Read Data from First Sheet
            //connExcel.Open();
            //cmdExcel.CommandText = "SELECT * From [" + SheetName + "] ";
            //oda.SelectCommand = cmdExcel;
            //oda.Fill(dt);
            //connExcel.Close();
            //lblMobileCnt.Text = dt.Rows.Count.ToString();
            #endregion
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string res = ob.SaveTempTable(FilePath, SheetName, _user, Extension, folder, filenm, ddlSender.SelectedValue, mobLen);

            txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text;
            }
            else
            {
                lblMobileCnt.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(_user);
            }
            return res;
        }
        public string Import_To_Grid3(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            string SheetName = "";
            DataTable dt = new DataTable();
            SheetName = "Sheet1$";
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string res = ob.SaveTempTable3(FilePath, SheetName, _user, Extension, folder, filenm, mobLen);

            //txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                string[] arr = res.Split(',');
                lblExcludeCnt.Text = arr[1];
                lblActualMobCnt.Text = arr[3];
                //lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblActualMobCnt.Text; // lblMobileCnt.Text;
            }
            else
            {
                lblMobileCnt.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(_user);
            }
            return res;
        }

        protected void btnInsURL_Click(object sender, EventArgs e)
        {
            //mpeAddUpdateEmployee.Show();

            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
            {
                lblLongUrl.Text = "";
                Helper.Util ob = new Helper.Util();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                //DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), "", string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"));
                //DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), "", ws);// comment by Rachit 11-04-2022
                DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), ws);

                ddlURL.DataSource = dt;
                ddlURL.DataTextField = "shorturlDISP";
                ddlURL.DataValueField = "shorturl";
                ddlURL.DataBind();
                ListItem objlistitem = new ListItem("--select--", "0");
                ddlURL.Items.Insert(0, objlistitem);
                ddlURL.SelectedIndex = 0;
                pnlExisting.Visible = true;
                pnlNew.Visible = false;
                pnlMedia.Visible = false;
                pnlPopUp_URL_ModalPopupExtender.Show();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('please select smart sms in sms type to insert url');", true);
                return;
            }

        }
        protected void btnInsertURLinMsg_Click(object sender, EventArgs e)
        {
            string ws = Convert.ToString(Session["DOMAINNAME"]);
            if (pnlExisting.Visible)
            {
                if (ddlURL.SelectedIndex > 0)
                {
                    txtMsg.Text = txtMsg.Text.Trim() + " " + ddlURL.SelectedValue + " ";
                    if (chkMobTrack.Checked)
                        Session["SHORTURL"] = ddlURL.SelectedValue;
                    ShowMsgCharCnt();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('2 more character will be added in the link text for individual mobile numbers. Length of Message characters will be increased by 2.');", true);
                    pnlPopUp_URL_ModalPopupExtender.Hide();
                }
                else
                {
                    pnlPopUp_URL_ModalPopupExtender.Show();
                }
            }
            if (pnlNew.Visible)
            {
                if (txtAddName.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter name for the URL');", true);
                    pnlPopUp_URL_ModalPopupExtender.Show();
                }

                if (txtLongURL.Text != "")
                {
                    string s = create_ShortURL(txtLongURL.Text, txtAddName.Text);
                    //if (s != "") txtMsg.Text = txtMsg.Text.Trim() + " " + string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", s);
                    if (s != "") txtMsg.Text = txtMsg.Text.Trim() + " " + ws + s;
                    //Session["SHORTURL"] = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", s);

                    if (chkMobTrack.Checked)
                        Session["SHORTURL"] = ws + s;
                    ShowMsgCharCnt();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('2 more character will be added in the link text for individual mobile numbers. Length of Message characters will be increased by 2.');", true);
                    if (s == "") pnlPopUp_URL_ModalPopupExtender.Show();
                }
                else
                {
                    pnlPopUp_URL_ModalPopupExtender.Show();
                }
            }
        }
        public void ShowMsgCharCnt()
        {
            string q = txtMsg.Text.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtMsg.Text.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~');
            qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;

            int noofsms = 0;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 160) noofsms = 2;
            if (qlen > 306) noofsms = 3;
            if (qlen > 459) noofsms = 4;
            if (qlen > 612) noofsms = 5;
            if (qlen > 765) noofsms = 6;
            if (qlen > 918) noofsms = 7;
            if (qlen > 1071) noofsms = 8;
            if (qlen > 1224) noofsms = 9;
            if (qlen > 1377) noofsms = 10;
            if (qlen > 1530) noofsms = 11;
            if (qlen > 1683) noofsms = 12;
            lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();

            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
                lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();
                lblUniCode.Text = "UNICODE : YES";
            }
        }
        protected void btnClose_URL_Click(object sender, EventArgs e)
        {
            pnlPopUp_URL_ModalPopupExtender.Hide();
        }
        protected void ddlURL_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnlPopUp_URL_ModalPopupExtender.Show();
        }
        protected void btnShowLongURL_Click(object sender, EventArgs e)
        {
            if (ddlURL.SelectedIndex >= 0)
            {
                Helper.Util ob = new Helper.Util();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                //DataTable dt = ob.GetURLSofUser(Convert.ToString(Session["UserID"]), ddlURL.SelectedValue, string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"));
                DataTable dt = ob.GetURLSofUser(Convert.ToString(Session["UserID"]), ddlURL.SelectedValue, ws);
                lblLongUrl.Text = dt.Rows[0][0].ToString();
            }
            pnlPopUp_URL_ModalPopupExtender.Show();
        }

        public string create_ShortURL(string txtLongURL, string name)
        {
            Helper.Util ob = new Helper.Util();

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return "";
            }

            //if (Convert.ToInt64(Session["URLBALANCE"]) <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL Balance expired.');", true);
            //    return "";
            //}

            if (txtLongURL.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Long URL.');", true);
                return "";
            }

            if (!txtLongURL.StartsWith("http://") && !txtLongURL.StartsWith("https://"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Long URL.');", true);
                return "";
            }

            string UserID = Convert.ToString(Session["UserID"]);
            string txtShortURL = "";
            string lblShortURL = "";

            //if specific user has already the long url, then don't reduce balance -
            using (var ctx = new ShortnrContext())
            {
                //lblShortURL = ob.GetExistingShortURL(txtLongURL, UserID);
                //if (lblShortURL != "")
                //{
                //    //lblShortURL = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", lblShortURL);
                //    return lblShortURL;
                //}
                //else
                //{
                if (txtShortURL.Trim() != "")
                {
                    if (txtShortURL.Length == 8)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL Word cannot be of 8 characters. It can be more or less than 8.');", true);
                        return "";
                    }
                }
                UrlManager ob2 = new UrlManager();
                string mobTrk = "N"; // ob.GetMobTrkOfUser(UserID);
                if (ddlSMSType.SelectedValue == "2") mobTrk = "Y";
                string sUrl = "";

                //sUrl = ob2.ShortenURL1(UserID, txtLongURL, Request.UserHostAddress, txtShortURL, mobTrk);
                sUrl = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString()); // ob2.NewSegment();

                //Rachit : 5-4-2022        
                string segment = "";
                bool IsRichMedia = false;
                if (txtLongURL.Contains(Convert.ToString(Session["DOMAINNAME"])))
                {
                    segment = txtLongURL.Replace("//", "/").Split('/').Last();
                    IsRichMedia = ob.IsRichMediaURL(UserID, segment);
                }

                if (IsRichMedia)
                {
                    string Expiry = Convert.ToString(database.GetScalarValue("select top 1 cast(Expiry as date) from short_urls where segment='" + segment + "' AND userid='" + UserID + "' order by added desc "));
                    if (!string.IsNullOrEmpty(Expiry))
                    {
                        if (Convert.ToDateTime(Expiry) < Convert.ToDateTime(DateTime.Now))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Expire Long URL !');", true);
                            return "";
                        }
                        else
                            ob.SaveShortURLRichMedia(UserID, txtLongURL, Request.UserHostAddress, sUrl, "Y", "N", Convert.ToString(Session["DOMAINNAME"]), name, "Y");

                    }
                }
                else
                    // End
                    ob.SaveShortURL(UserID, txtLongURL, Request.UserHostAddress, sUrl, "Y", "N", Convert.ToString(Session["DOMAINNAME"]), name);

                lblShortURL = sUrl;  //string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", sUrl);
                                     //ob.STOREURL(UserID, txtLongURL, Request.UserHostAddress, lblShortURL, "N");
                                     //ShortUrl shortUrl1 = await this._urlManager.ShortenUrl(UserID, url.LongURL, Request.UserHostAddress, shortUrl.Segment + "_Q", "N");
                                     //string bal = ob.UdateAndGetURLbal(Convert.ToString(Session["UserID"]));
                                     //lblURLbal.Text = bal;
                                     //Session["URLBALANCE"] = bal;
                return lblShortURL;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Short URL created Successfully.');", true);
                //}
            }
        }

        protected void ddlSMSType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Util ob = new Util();
            DataTable dt = ob.GetSMSRateAsPerCountry(Convert.ToString(Session["UserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (ddlSMSType.SelectedValue == "0") lblRate.Text = "";
                if (ddlSMSType.SelectedValue == "1") { lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_NORMALSMS"]) + " " + paisa + " per SMS"; ViewState["SMSRATE"] = dt.Rows[0]["RATE_NORMALSMS"]; };
                if (ddlSMSType.SelectedValue == "2") { lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_SMARTSMS"]) + " " + paisa + " per SMS"; ViewState["SMSRATE"] = dt.Rows[0]["RATE_SMARTSMS"]; }
                // if (ddlSMSType.SelectedValue == "3") lblRate.Text = Convert.ToString(Session["RATE_OTP"]) + " Paisa per SMS";
                if (ddlSMSType.SelectedValue == "3") { lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_CAMPAIGN"]) + " " + paisa + " per SMS"; ViewState["SMSRATE"] = dt.Rows[0]["RATE_CAMPAIGN"]; }
                if (ddlSMSType.SelectedValue == "6") { lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_NORMALSMS"]) + " " + paisa + " per SMS"; ViewState["SMSRATE"] = dt.Rows[0]["RATE_NORMALSMS"]; }
                if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") LinkButton1.Visible = true; else LinkButton1.Visible = false;

            }
        }

        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            StartProcess();
            chkMobTrack.Checked = false;
            lblMobileCnt.Text = "";
            txtMobNum.Text = "";
            //txtMobNum.Enabled = false;
            divNum.Attributes.Add("style", "pointer-events:none;");

            divFileUpload.Attributes.Add("class", "form-group row d-none");
            divFileUpload3.Attributes.Add("class", "form-group row d-none");
            if (rdbEntry.Checked)
                divNum.Attributes.Add("style", "pointer-events:all;");

            if (rdbUpload.Checked || rdbPersonal.Checked || rdbImport.Checked)
            {
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
                divFileUpload3.Attributes.Add("class", "form-group row d-block;");
                divCamp.Attributes.Add("class", "form-group row d-block;");
            }
            else
            {
                divCamp.Attributes.Add("class", "form-group row d-none");
            }
            if (rdbImport.Checked)
            {
                //open popup group selection.
                populateGroup();
                pnlPopUp_GROUP_ModalPopupExtender.Show();
            }
        }
        public void populateGroup()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetGroup(Convert.ToString(Session["UserID"]));

            ddlGrp.DataSource = dt;
            ddlGrp.DataTextField = "grpname";
            ddlGrp.DataValueField = "grpname";
            ddlGrp.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlGrp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlGrp.SelectedIndex = 1;
            else
                ddlGrp.SelectedIndex = 0;
        }
        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            string res = ob.SaveTempTable4Group(_user, ddlGrp.SelectedValue);
            DataTable dt = new DataTable();
            txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text;
                Session["UPLOADFILENM"] = ddlGrp.SelectedValue;
                Session["UPLOADFILENMEXT"] = "GROUP";
                pnlPopUp_GROUP_ModalPopupExtender.Hide();
            }
            else
            {
                lblMobileCnt.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(_user);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
            }
        }

        List<string> liScheduleDates;
        protected bool ValidateScheduleTime()
        {
            #region ScheduleTime 0
            if (txtTime.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            if (txtTime.Text.Trim().Length != 5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }

            string HH = txtTime.Text.Split(':')[0];
            string MM = txtTime.Text.Split(':')[1];

            if (Convert.ToInt16(HH) > 23)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }

            if (Convert.ToInt16(MM) > 59)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            if (hdnScheduleDate.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            string datetime = Convert.ToDateTime(hdnScheduleDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime.Text;
            //string datetime = Convert.ToDateTime(hdnScheduleDate.Value.Trim()).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime.Text;

            if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) <= DateTime.Now)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime + " cannot be below of current date time');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            string schMin = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
            DateTime t = DateTime.Now.AddMinutes(Convert.ToDouble(schMin));
            if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) < t)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin + " minutes of current date time');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }

            #endregion

            liScheduleDates.Add(datetime);

            if (hdnSchedule1.Value == "1")
            {
                #region ScheduleTime 1          
                if (txtTime1.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime1.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH1 = txtTime1.Text.Split(':')[0];
                string MM1 = txtTime1.Text.Split(':')[1];

                if (Convert.ToInt16(HH1) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM1) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate1.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime1 = Convert.ToDateTime(hdnScheduleDate1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime1.Text;

                if (Convert.ToDateTime(datetime1, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime1 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin1 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t1 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin1));
                if (Convert.ToDateTime(datetime1, CultureInfo.InvariantCulture) < t1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin1 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime1))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime1);
                #endregion

            }
            if (hdnSchedule2.Value == "1")
            {
                #region ScheduleTime 2          
                if (txtTime2.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime2.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH2 = txtTime2.Text.Split(':')[0];
                string MM2 = txtTime2.Text.Split(':')[1];

                if (Convert.ToInt16(HH2) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM2) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate2.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime2 = Convert.ToDateTime(hdnScheduleDate2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime2.Text;

                if (Convert.ToDateTime(datetime2, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime2 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin2 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t2 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin2));
                if (Convert.ToDateTime(datetime2, CultureInfo.InvariantCulture) < t2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin2 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime2))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime2);
                #endregion

            }
            if (hdnSchedule3.Value == "1")
            {
                #region ScheduleTime 3        
                if (txtTime3.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime3.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH3 = txtTime3.Text.Split(':')[0];
                string MM3 = txtTime3.Text.Split(':')[1];

                if (Convert.ToInt16(HH3) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM3) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate3.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime3 = Convert.ToDateTime(hdnScheduleDate3.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime3.Text;

                if (Convert.ToDateTime(datetime3, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime3 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin3 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t3 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin3));
                if (Convert.ToDateTime(datetime3, CultureInfo.InvariantCulture) < t3)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin3 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime3))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime3);
                #endregion

            }
            if (hdnSchedule4.Value == "1")
            {
                #region ScheduleTime 4
                if (txtTime4.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime4.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH4 = txtTime4.Text.Split(':')[0];
                string MM4 = txtTime4.Text.Split(':')[1];

                if (Convert.ToInt16(HH4) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM4) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate4.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime4 = Convert.ToDateTime(hdnScheduleDate4.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime4.Text;

                if (Convert.ToDateTime(datetime4, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime4 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin4 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t4 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin4));
                if (Convert.ToDateTime(datetime4, CultureInfo.InvariantCulture) < t4)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin4 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime4))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime4);
                #endregion

            }
            if (hdnSchedule5.Value == "1")
            {
                #region ScheduleTime 5          
                if (txtTime5.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime5.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH5 = txtTime5.Text.Split(':')[0];
                string MM5 = txtTime5.Text.Split(':')[1];

                if (Convert.ToInt16(HH5) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM5) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate5.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime5 = Convert.ToDateTime(hdnScheduleDate5.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime5.Text;

                if (Convert.ToDateTime(datetime5, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime5 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin5 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t5 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin5));
                if (Convert.ToDateTime(datetime5, CultureInfo.InvariantCulture) < t5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin5 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime5))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime5);
                #endregion

            }
            if (hdnSchedule6.Value == "1")
            {
                #region ScheduleTime 6          
                if (txtTime6.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime6.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH6 = txtTime6.Text.Split(':')[0];
                string MM6 = txtTime6.Text.Split(':')[1];

                if (Convert.ToInt16(HH6) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM6) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate6.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime6 = Convert.ToDateTime(hdnScheduleDate6.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime6.Text;

                if (Convert.ToDateTime(datetime6, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime6 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin6 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t6 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin6));
                if (Convert.ToDateTime(datetime6, CultureInfo.InvariantCulture) < t6)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin6 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime6);
                #endregion
            }
            if (hdnSchedule7.Value == "1")
            {
                #region ScheduleTime 7          
                if (txtTime7.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime7.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH7 = txtTime7.Text.Split(':')[0];
                string MM7 = txtTime7.Text.Split(':')[1];

                if (Convert.ToInt16(HH7) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM7) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate7.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime7 = Convert.ToDateTime(hdnScheduleDate7.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime7.Text;

                if (Convert.ToDateTime(datetime7, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime7 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin7 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t7 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin7));
                if (Convert.ToDateTime(datetime7, CultureInfo.InvariantCulture) < t7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin7 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime7))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime7);
                #endregion
            }
            if (hdnSchedule8.Value == "1")
            {
                #region ScheduleTime 8          
                if (txtTime8.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime8.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH8 = txtTime8.Text.Split(':')[0];
                string MM8 = txtTime8.Text.Split(':')[1];

                if (Convert.ToInt16(HH8) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM8) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (hdnScheduleDate8.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime8 = Convert.ToDateTime(hdnScheduleDate8.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime8.Text;

                if (Convert.ToDateTime(datetime8, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime8 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin8 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t8 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin8));
                if (Convert.ToDateTime(datetime8, CultureInfo.InvariantCulture) < t8)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin8 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime8))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime8);
                #endregion
            }
            if (hdnSchedule9.Value == "1")
            {
                #region ScheduleTime 9          
                if (txtTime9.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime9.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH9 = txtTime9.Text.Split(':')[0];
                string MM9 = txtTime9.Text.Split(':')[1];

                if (Convert.ToInt16(HH9) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM9) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate9.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime9 = Convert.ToDateTime(hdnScheduleDate9.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime9.Text;

                if (Convert.ToDateTime(datetime9, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime9 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin9 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t9 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin9));
                if (Convert.ToDateTime(datetime9, CultureInfo.InvariantCulture) < t9)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin9 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime9))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime9);
                #endregion
            }
            if (hdnSchedule10.Value == "1")
            {
                #region ScheduleTime 10          
                if (txtTime10.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime10.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH10 = txtTime10.Text.Split(':')[0];
                string MM10 = txtTime10.Text.Split(':')[1];

                if (Convert.ToInt16(HH10) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM10) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate10.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime10 = Convert.ToDateTime(hdnScheduleDate10.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime10.Text;

                if (Convert.ToDateTime(datetime10, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime10 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin10 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t10 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin10));
                if (Convert.ToDateTime(datetime10, CultureInfo.InvariantCulture) < t10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin10 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime10))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime10);
                #endregion
            }
            if (hdnSchedule11.Value == "1")
            {
                #region ScheduleTime 11          
                if (txtTime11.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime11.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH11 = txtTime11.Text.Split(':')[0];
                string MM11 = txtTime11.Text.Split(':')[1];

                if (Convert.ToInt16(HH11) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM11) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate11.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime11 = Convert.ToDateTime(hdnScheduleDate11.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime11.Text;

                if (Convert.ToDateTime(datetime11, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime11 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin11 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t11 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin11));
                if (Convert.ToDateTime(datetime11, CultureInfo.InvariantCulture) < t11)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin11 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime11))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime11);
                #endregion
            }
            if (hdnSchedule12.Value == "1")
            {
                #region ScheduleTime 12          
                if (txtTime12.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime12.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH12 = txtTime12.Text.Split(':')[0];
                string MM12 = txtTime12.Text.Split(':')[1];

                if (Convert.ToInt16(HH12) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM12) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate12.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime12 = Convert.ToDateTime(hdnScheduleDate12.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime12.Text;

                if (Convert.ToDateTime(datetime12, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime12 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin12 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t12 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin12));
                if (Convert.ToDateTime(datetime12, CultureInfo.InvariantCulture) < t12)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin12 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime12))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime12);
                #endregion
            }
            if (hdnSchedule13.Value == "1")
            {
                #region ScheduleTime 13          
                if (txtTime13.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime13.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH13 = txtTime13.Text.Split(':')[0];
                string MM13 = txtTime13.Text.Split(':')[1];

                if (Convert.ToInt16(HH13) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM13) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate13.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime13 = Convert.ToDateTime(hdnScheduleDate13.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime13.Text;

                if (Convert.ToDateTime(datetime13, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime13 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin13 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t13 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin13));
                if (Convert.ToDateTime(datetime13, CultureInfo.InvariantCulture) < t13)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin13 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime13))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime13);
                #endregion
            }
            if (hdnSchedule14.Value == "1")
            {
                #region ScheduleTime 14          
                if (txtTime14.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (txtTime14.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                string HH14 = txtTime14.Text.Split(':')[0];
                string MM14 = txtTime14.Text.Split(':')[1];

                if (Convert.ToInt16(HH14) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }

                if (Convert.ToInt16(MM14) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (hdnScheduleDate14.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string datetime14 = Convert.ToDateTime(hdnScheduleDate14.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime14.Text;

                if (Convert.ToDateTime(datetime14, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime14 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin14 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t14 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin14));
                if (Convert.ToDateTime(datetime14, CultureInfo.InvariantCulture) < t14)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin14 + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                if (liScheduleDates.Contains(datetime14))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule Datetime already exist! ');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                else
                    liScheduleDates.Add(datetime14);
                #endregion
            }


            return true;
        }
        protected void SetAttribueScheduleRows(string rowCount)
        {
            if (rowCount == "1" || rowCount == "")
            {
                lblScheduleDate.Text = hdnScheduleDate.Value;
            }
            else if (rowCount == "2")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
            }
            else if (rowCount == "3")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
            }
            else if (rowCount == "4")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
            }
            else if (rowCount == "5")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
            }
            else if (rowCount == "6")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
            }
            else if (rowCount == "7")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
            }
            else if (rowCount == "8")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
            }
            else if (rowCount == "9")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
            }
            else if (rowCount == "10")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
            }
            else if (rowCount == "11")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
                divSchedule10.Style["display"] = ""; lblScheduleDate10.Text = hdnScheduleDate10.Value;
            }
            else if (rowCount == "12")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
                divSchedule10.Style["display"] = ""; lblScheduleDate10.Text = hdnScheduleDate10.Value;
                divSchedule11.Style["display"] = ""; lblScheduleDate11.Text = hdnScheduleDate11.Value;
            }
            else if (rowCount == "13")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
                divSchedule10.Style["display"] = ""; lblScheduleDate10.Text = hdnScheduleDate10.Value;
                divSchedule11.Style["display"] = ""; lblScheduleDate11.Text = hdnScheduleDate11.Value;
                divSchedule12.Style["display"] = ""; lblScheduleDate12.Text = hdnScheduleDate12.Value;
            }
            else if (rowCount == "14")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
                divSchedule10.Style["display"] = ""; lblScheduleDate10.Text = hdnScheduleDate10.Value;
                divSchedule11.Style["display"] = ""; lblScheduleDate11.Text = hdnScheduleDate11.Value;
                divSchedule12.Style["display"] = ""; lblScheduleDate12.Text = hdnScheduleDate12.Value;
                divSchedule13.Style["display"] = ""; lblScheduleDate13.Text = hdnScheduleDate13.Value;
            }
            else if (rowCount == "15")
            {
                divSchedule1.Style["display"] = ""; lblScheduleDate1.Text = hdnScheduleDate1.Value;
                divSchedule2.Style["display"] = ""; lblScheduleDate2.Text = hdnScheduleDate2.Value;
                divSchedule3.Style["display"] = ""; lblScheduleDate3.Text = hdnScheduleDate3.Value;
                divSchedule4.Style["display"] = ""; lblScheduleDate4.Text = hdnScheduleDate4.Value;
                divSchedule5.Style["display"] = ""; lblScheduleDate5.Text = hdnScheduleDate5.Value;
                divSchedule6.Style["display"] = ""; lblScheduleDate6.Text = hdnScheduleDate6.Value;
                divSchedule7.Style["display"] = ""; lblScheduleDate7.Text = hdnScheduleDate7.Value;
                divSchedule8.Style["display"] = ""; lblScheduleDate8.Text = hdnScheduleDate8.Value;
                divSchedule9.Style["display"] = ""; lblScheduleDate9.Text = hdnScheduleDate9.Value;
                divSchedule10.Style["display"] = ""; lblScheduleDate10.Text = hdnScheduleDate10.Value;
                divSchedule11.Style["display"] = ""; lblScheduleDate11.Text = hdnScheduleDate11.Value;
                divSchedule12.Style["display"] = ""; lblScheduleDate12.Text = hdnScheduleDate12.Value;
                divSchedule13.Style["display"] = ""; lblScheduleDate13.Text = hdnScheduleDate13.Value;
                divSchedule14.Style["display"] = ""; lblScheduleDate14.Text = hdnScheduleDate14.Value;
            }

        }
        protected void ClearScheduleDateTime()
        {
            hdnScheduleCount.Value = "1"; // hdnScheduleCount.Value = "";
            hdnSchedule1.Value = "";
            hdnSchedule2.Value = "";
            hdnSchedule3.Value = "";
            hdnSchedule4.Value = "";
            hdnSchedule5.Value = "";
            hdnSchedule6.Value = "";
            hdnSchedule7.Value = "";
            hdnSchedule8.Value = "";
            hdnSchedule9.Value = "";
            hdnSchedule10.Value = "";
            hdnSchedule11.Value = "";
            hdnSchedule12.Value = "";
            hdnSchedule13.Value = "";
            hdnSchedule14.Value = "";
            txtScheduleDate.Text = ""; txtTime.Text = "";
            txtScheduleDate1.Text = ""; txtTime1.Text = "";
            txtScheduleDate2.Text = ""; txtTime2.Text = "";
            txtScheduleDate3.Text = ""; txtTime3.Text = "";
            txtScheduleDate4.Text = ""; txtTime4.Text = "";
            txtScheduleDate5.Text = ""; txtTime5.Text = "";
            txtScheduleDate6.Text = ""; txtTime6.Text = "";
            txtScheduleDate7.Text = ""; txtTime7.Text = "";
            txtScheduleDate8.Text = ""; txtTime8.Text = "";
            txtScheduleDate9.Text = ""; txtTime9.Text = "";
            txtScheduleDate10.Text = ""; txtTime10.Text = "";
            txtScheduleDate11.Text = ""; txtTime11.Text = "";
            txtScheduleDate12.Text = ""; txtTime12.Text = "";
            txtScheduleDate13.Text = ""; txtTime13.Text = "";
            txtScheduleDate14.Text = ""; txtTime14.Text = "";
            divSchedule1.Attributes.Add("style", "display:none");
            divSchedule2.Attributes.Add("style", "display:none");
            divSchedule3.Attributes.Add("style", "display:none");
            divSchedule4.Attributes.Add("style", "display:none");
            divSchedule5.Attributes.Add("style", "display:none");
            divSchedule6.Attributes.Add("style", "display:none");
            divSchedule7.Attributes.Add("style", "display:none");
            divSchedule8.Attributes.Add("style", "display:none");
            divSchedule9.Attributes.Add("style", "display:none");
            divSchedule10.Attributes.Add("style", "display:none");
            divSchedule11.Attributes.Add("style", "display:none");
            divSchedule12.Attributes.Add("style", "display:none");
            divSchedule13.Attributes.Add("style", "display:none");
            divSchedule14.Attributes.Add("style", "display:none");
        }

        protected void btnScheduleSMS_Click(object sender, EventArgs e)
        {
            try
            {
                // string s1 = Convert.ToDateTime(hdnScheduleDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                //string s1 = Convert.ToDateTime(hdnScheduleDate.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                //if (txtScheduleDate.Text.Trim() == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be left blank');", true);
                //    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                //    return;
                //}

                liScheduleDates = new List<string>();

                ValidateScheduleTime();

                SetAttribueScheduleRows(hdnScheduleCount.Value);

                if (hdnScheduleCount.Value != liScheduleDates.Count.ToString())
                {
                    return;
                }


                //if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                //    res = SendTemplateMSG(datetime);
                //else
                //    res = MsgSend(datetime);

                bool res = false;
                if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                    res = SendTemplateMSG("YES");
                else
                    res = MsgSend("YES");

                if (res == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Scheduled Successfully');window.location ='send-sms-u-B4Send.aspx';", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Hide();
                    divSchedule.Style.Add("display", "none");
                }
                else
                {
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                divSchedule.Style.Add("display", "none");
            }
        }

        protected void btnClose_Schedule_Click(object sender, EventArgs e)
        {
            pnlPopUp_SCHEDULE_ModalPopupExtender.Hide();
            divSchedule.Style.Add("display", "none");
        }

        protected void btnInsTemplate_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["XLUPLOADED"]) != "Y" || Convert.ToString(Session["UPLOADFILENMEXT"]) == ".txt")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload Xls File');", true);
                return;
            }
            //bind dropdown
            divTemplateList.Attributes.Add("Style", "display:block;");

            divTempId.Attributes.Add("class", "form-group row d-none");

            //LinkButton5.Visible = false;
            PopulateTemplate();
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetFieldsOfUserUploadedXLS(Convert.ToString(Session["UserID"]));

            DataTable dtMax = ob.GetMaxLenFieldsOfUserUploadedXLS(Convert.ToString(Session["UserID"]), dt);
            ViewState["dtMaxLen"] = dtMax;
            DataTable dtTopRec = ob.GetTopRecordOfUserUploadedXLS(Convert.ToString(Session["UserID"]));
            ViewState["dtTopRec"] = dtTopRec;

            LstXLSFld.Items.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LstXLSFld.Items.Add(dt.Rows[i][0].ToString());
            }

            IsPersalisedSMS.Value = "1";

        }
        public void PopulateTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSApprovedTemplateOfUser(Convert.ToString(Session["UserID"]));

            ddlTemplate.DataSource = dt;
            ddlTemplate.DataTextField = "TemplateID";
            ddlTemplate.DataValueField = "onlyTemplateID";
            ddlTemplate.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTemplate.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlTemplate.SelectedIndex = 1;
            //else
            ddlTemplate.SelectedIndex = 0;
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dtT = ob.GetTemplateSMS(Convert.ToString(Session["UserID"]), ddlTemplate.SelectedValue);
            string smstxt = dtT.Rows[0]["template"].ToString();
            ViewState["TemplateID"] = dtT.Rows[0]["templateid"].ToString();
            ViewState["TemplateFields"] = null;
            txtMsg.Text = smstxt; // ddlTemplate.SelectedValue; //.Replace('\n',' ');
                                  //     divMsg.Attributes.Add("style", "pointer-events:none;");
            string str = smstxt; // ddlTemplate.SelectedValue; //.Replace('\n', ' ');
            LstTemplateFld.Items.Clear();
            string s = smstxt; // ddlTemplate.SelectedValue; //.Replace('\n', ' ');

            //string variableText = tempVar;
            //int variableTextCount = 0;
            //foreach (Match m in Regex.Matches(s, variableText))
            //{
            //    variableTextCount++;
            //}

            //for (int v = 1; v <= variableTextCount; v++)
            //{
            //    LstTemplateFld.Items.Add(v.ToString() + tempVar);
            //}


            string[] s1 = s.Split('#');
            int i = 0; if (s.Substring(0, 1) != "#") i = 1;

            for (; i < s1.Length; i++)
            {
                if (s1[i].Contains("\n"))
                {
                    string[] st = s1[i].Split('\n');
                    s1[i] = st[0];
                }
                if (s1[i] != "")
                {
                    string[] s2 = s1[i].Split(' ');
                    LstTemplateFld.Items.Add("#" + s2[0].Replace(",", "").Replace(".", "").Replace("\n", "").Replace("\r", ""));
                    // LstTemplateFld.Items.Add("1");
                }
            }
            SetPreview();
            List<string> sa = new List<string>();
            foreach (ListItem li in LstTemplateFld.Items) sa.Add(li.Text);
            ViewState["TemplateFields"] = sa;
        }

        protected void btnMap_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(LstTemplateFld.SelectedItem) == "" || Convert.ToString(LstXLSFld.SelectedItem) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template Field and XLS Field for Mapping');", true);
                return;
            }
            lstMappedFields.Items.Add(LstTemplateFld.SelectedItem.ToString() + " ---->> " + LstXLSFld.SelectedItem.ToString());
            LstTemplateFld.Items.Remove(LstTemplateFld.SelectedItem.ToString());
            LstXLSFld.Items.Remove(LstXLSFld.SelectedItem.ToString());

            SetPreview();
        }

        protected void btnUnMap_Click(object sender, EventArgs e)
        {
            if (lstMappedFields.SelectedItem.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template Field and XLS Field for Mapping');", true);
                return;
            }
            string s = lstMappedFields.SelectedItem.ToString();
            string[] s1 = s.Replace(" ---->> ", "$").Split('$');
            LstTemplateFld.Items.Add(s1[0].Trim());
            LstXLSFld.Items.Add(s1[1].Trim());
            lstMappedFields.Items.Remove(lstMappedFields.SelectedItem.ToString());

            SetPreview();
        }

        public void SetPreview()
        {
            Helper.Util obU = new Helper.Util();
            txtPreview.Text = txtMsg.Text;
            lblsmscnt.Text = "";
            int len = txtMsg.Text.Length;
            int i = 0;
            if (len >= 1) i = 1;
            if (len > 160) i = 2;
            if (len > 306) i = 3;
            if (len > 459) i = 4;
            if (len > 612) i = 5;
            if (len > 765) i = 6;
            if (len > 918) i = 7;
            if (len > 1071) i = 8;
            if (len > 1224) i = 9;
            if (len > 1377) i = 10;
            if (len > 1530) i = 11;
            if (len > 1683) i = 12;
            lblsmscnt.Text = "No. of Char: " + len + ". No. of SMS:" + i.ToString();
            Session["NOOFCHARINMSG"] = len;
            if (lstMappedFields.Items.Count <= 0) return;

            string msg = txtMsg.Text;
            len = msg.Length;
            DataTable dtTopRec = (DataTable)ViewState["dtTopRec"];
            DataTable dtMaxLen = (DataTable)ViewState["dtMaxLen"];

            foreach (ListItem li in lstMappedFields.Items)
            {
                string st = li.Text;

                string[] s1 = st.Replace(" ---->> ", "$").Split('$');
                int l = s1[0].Length;
                len = len - l; len = len + Convert.ToInt16(dtMaxLen.Rows[0][s1[1]]);
                string datatype = obU.GetDataType(Convert.ToString(Session["UserID"]), s1[1]);
                if (datatype == "DATE")
                    msg = msg.Replace(s1[0], Convert.ToDateTime(dtTopRec.Rows[0][s1[1]]).ToString("dd MMM yyyy", CultureInfo.InvariantCulture));
                else
                {
                    msg = msg.Replace(s1[0], dtTopRec.Rows[0][s1[1]].ToString());
                }
            }

            txtPreview.Text = msg;
            i = 0;
            if (len >= 1) i = 1;
            if (len > 160) i = 2;
            if (len > 306) i = 3;
            if (len > 459) i = 4;
            if (len > 612) i = 5;
            if (len > 765) i = 6;
            if (len > 918) i = 7;
            if (len > 1071) i = 8;
            if (len > 1224) i = 9;
            if (len > 1377) i = 10;
            if (len > 1530) i = 11;
            if (len > 1683) i = 12;
            lblsmscnt.Text = "No. of Char: " + len + ". No. of SMS:" + i.ToString();
            Session["NOOFCHARINMSG"] = len;
        }

        public int IndexOfNth(string str, string value, int nth = 0)
        {
            //if (nth < 0)
            //    throw new ArgumentException("Can not find a negative index of substring in string. Must start with 0");

            int offset = str.IndexOf(value);
            for (int i = 0; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }

            return offset;
        }

        public void PopulateTemplateID()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTemplateId(Convert.ToString(Session["UserID"]));

            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "TemplateID";
            ddlTempID.DataValueField = "Template";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlTempID.SelectedIndex = 1;
            //else
            ddlTempID.SelectedIndex = 0;

        }
        protected void ddlTempID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["UserID"]), ddlTempID.SelectedValue);
            //string smstxt = dtT.Rows[0]["template"].ToString();

            if (ddlTempID.SelectedValue == "0") lblTempSMS.Text = "";
            else lblTempSMS.Text = dtT.Rows[0]["template"].ToString();
        }

        protected void btnCancelSch_Click(object sender, EventArgs e)
        {
            ClearScheduleDateTime();
            pnlPopUp_SCHEDULE_ModalPopupExtender.Hide();
        }

        protected void ddlSender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971")
            {
                if (ddlSender.SelectedValue.Contains("AD-"))
                {
                    chkOptOut.Checked = true;
                    chkOptOut.Enabled = false;
                }
                else
                {
                    chkOptOut.Checked = false;
                    chkOptOut.Enabled = true;
                }
            }
        }

        protected void ddlCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSender();
            lblRate.Text = "";
            Util ob = new Util();
            DataTable dt = ob.GetSMSRateAsPerCountry(Convert.ToString(Session["UserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count > 0 && ddlSMSType.SelectedValue == "1")
            {
                lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_NORMALSMS"]) + " " + paisa + " per SMS";
                ViewState["SMSRATE"] = dt.Rows[0]["RATE_NORMALSMS"];
            }
            if (ddlCCode.SelectedValue != "91")
            {
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Campaign"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Promotional"));
                HideTemplateIdForeignAcc();
            }
            else
            {
                //Response.Redirect(Request.RawUrl);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "window.location ='send-sms-u-B4Send.aspx';", true);
                PopulateSMSType();
                divTempId.Attributes.Add("class", "form-group row d-block");
                divTempsms.Attributes.Add("class", "form-group row d-block");
                divOptOut.Attributes.Add("Style", "display:none;");
            }

        }
    }
}