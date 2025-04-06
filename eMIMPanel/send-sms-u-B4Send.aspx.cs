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
using System.Configuration;
using System.Text;

namespace eMIMPanel
{
    public partial class send_sms_u_B4Send : System.Web.UI.Page
    {
        string tempVar = "{#var#}";
        string _user = "";
        string paisa = "";
        string dbName = "";
        int moblen = 0;
        string FileId = "";
        string FId = "";
        int IsScratched = 0;
        string EventsCode = "";
        int IsDLRData = 0;
        string DNDUAEUSERID = System.Configuration.ConfigurationManager.AppSettings["DNDUAEUSERID"].ToString();
        string DebitedBalSendSMS = System.Configuration.ConfigurationManager.AppSettings["DebitedBalSendSMS"].ToString();
        string DebitedBalScheduleCreation = System.Configuration.ConfigurationManager.AppSettings["DebitedBalScheduleCreation"].ToString();

        //For Next Process Alert Msg---------
        string WithLastFileNameAlertMsg = System.Configuration.ConfigurationManager.AppSettings["WithLastFileNameAlertMsg"].ToString();
        string WithOutLastFileNameAlertMsg = System.Configuration.ConfigurationManager.AppSettings["WithOutLastFileNameAlertMsg"].ToString();

        //Added by naved at 29/09/2023
        string DLTNO = "";
        string DLTNO1 = System.Configuration.ConfigurationManager.AppSettings["DLTNO"].ToString();
        string DLTNO2 = System.Configuration.ConfigurationManager.AppSettings["DLTNO1"].ToString();
        string DLTNO3 = System.Configuration.ConfigurationManager.AppSettings["DLTNO2"].ToString();

        string insurance = System.Configuration.ConfigurationManager.AppSettings["InsuranceCompUserId"].ToString(); //DW

        string FileProcessTableName = "fileprocess";
        string MobileDependency = "MobileDependency";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != string.Empty)
            {
                FileId = Request.QueryString["Id"];
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
                divFileUpload3.Attributes.Add("class", "form-group row d-block;");
                divCamp.Attributes.Add("class", "form-group row d-block;");
            }

            if (Request.QueryString["FId"] != null && Request.QueryString["FId"] != string.Empty)
            {
                FId = Request.QueryString["FId"].ToString();
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
                divFileUpload3.Attributes.Add("class", "form-group row d-block;");
                divCamp.Attributes.Add("class", "form-group row d-block;");
            }
            if (Convert.ToString(Session["MakerCheckerType"]).ToUpper() == "MAKER")
            {
                LinkButton5.Visible = false;
                LinkButton2.Text = "Create Campaign";
            }
            else
            {
                LinkButton5.Visible = true;
                LinkButton2.Text = "Send";
            }
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            paisa = Convert.ToString(Session["SUBCURRENCY"]);
            dbName = System.Configuration.ConfigurationManager.AppSettings["dbname"].ToString();

            _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");
            if (_user == "MIM2300128" || _user == "MIM2300129")
            {
                Response.Redirect("send-sms-u-B4SendDemo.aspx", true);
            }

            if (DLTNO1 == Session["DLT"].ToString())
            {
                DLTNO = DLTNO1;
            }
            else if (DLTNO2 == Session["DLT"].ToString())
            {
                DLTNO = DLTNO2;
            }
            else if (DLTNO3 == Session["DLT"].ToString())
            {
                DLTNO = DLTNO3;
            }

            if (DLTNO == Session["DLT"].ToString())
            {
                lblRate.Attributes.Add("style", "display:none;");
            }

            bool IsRateShow = Convert.ToBoolean(Convert.ToString(database.GetScalarValue("select IsRateShow from customer where username='" + _user + "'")));
            if (!IsRateShow) { lblRate.Visible = false; }

            string ss = "";
            Session["Domain"] = ss;
            if (Convert.ToString(Session["CANSENDSMS"]).ToUpper() == "FALSE") Response.Redirect("index_u.aspx");
            if (!IsPostBack)
            {
                Util ob = new Util();
                PopulateCountry();
                TotalGroups();
                //ob.DropUserTmpTable(_user);

                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    lblEvents.Visible = true;
                    DivEvents.Visible = true;
                    DivDealerName.Visible = true;
                    PopulateEvents();
                    PopulateDealerCode();
                }

                //DW
                if (Convert.ToString(Session["userId"]).ToLower() == Convert.ToString(insurance).ToLower())
                {
                    divcompName.Visible = true;
                    PopulateIsurancecompname();
                }


                Global.templateErrorCode = "";
                Global.Istemplatetest = true;
                Global.openTempAc = ob.IsOpenTempAc(Convert.ToString(Session["UserID"]));
                ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) != "971") lblOptOut.Text = "";
                ViewState["TemplateID"] = null;
                ViewState["TemplateFields"] = null;
                ViewState["dtMaxLen"] = null;
                ViewState["dtTopRec"] = null;
                Session["NOOFCHARINMSG"] = null;
                rdbEntry.Checked = true;
                ViewState["SMSRATE"] = Session["RATE_NORMALSMS"];
                lblRate.Text = Convert.ToString(Session["RATE_NORMALSMS"]) + " " + paisa + " per SMS";
                PopulateSMSType();
                PopulateSender();
                PopulateTemplateID();
                StartProcess();
                divOptOut.Attributes.Add("Style", "display:none;");
                HideTemplateIdForeignAcc();
                PopulateTemplate();
                ResendSMSProcess();
                ResendSMSDuplicateProcess();

                divscratchcard.Visible = false;

                string Type = Request.QueryString["Type"];
                string GroupID = Request.QueryString["GroupID"];
                string MobileNo = Request.QueryString["MobileNo"];

                if (!string.IsNullOrEmpty(Type))
                {
                    if (Type == "GroupWise")
                    {
                        if (!string.IsNullOrEmpty(GroupID))
                        {
                            rdbImport.Checked = true;
                            rdbUpload_CheckedChanged(sender, e);
                            pnlPopUp_GROUP_ModalPopupExtender.Hide();
                            string[] groupIDArray = GroupID.Split(',');
                            foreach (string groupID in groupIDArray)
                            {
                                for (int i = 0; i < chkbxgroup.Items.Count; i++)
                                {
                                    if (chkbxgroup.Items[i].Value == groupID.Trim())
                                    {
                                        chkbxgroup.Items[i].Selected = true;
                                    }
                                }
                            }
                            btnAddGroup_Click(sender, e);
                        }
                    }
                    else if (Type == "ManuelWise")
                    {
                        if (!string.IsNullOrEmpty(MobileNo))
                        {
                            txtMobNum.Text = MobileNo;
                            rdbEntry.Checked = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "mobnumbcnt()", true);
                        }
                    }
                }
            }

            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName).Replace("'", "");
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

                            int sizeTxt = Convert.ToInt16(ConfigurationManager.AppSettings["SIZETXT"]);
                            int sizeCSV = Convert.ToInt16(ConfigurationManager.AppSettings["SIZECSV"]);
                            int sizeXLS = Convert.ToInt16(ConfigurationManager.AppSettings["SIZEXLS"]);

                            if (en.Contains("TXT"))
                                if (FileUpload1.PostedFile.ContentLength > (sizeTxt * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of " + Convert.ToString(sizeTxt) + " MB');", true);
                                    lblUploading.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("CSV"))
                                if (FileUpload1.PostedFile.ContentLength > (sizeCSV * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of " + Convert.ToString(sizeCSV) + " MB');", true);
                                    lblUploading.Text = "Upload rejected.";
                                    return;
                                }
                            if (en.Contains("XLS"))
                                if (FileUpload1.PostedFile.ContentLength > (sizeXLS * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of " + Convert.ToString(sizeXLS) + " MB');", true);
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
                            string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly, DLTNO);
                            if (res == "DLRCODE MANDATORY")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Upload File With DLRCode !!');", true);
                                lblUploading.Text = "Upload Rejected.";
                                divFileLoader.Style.Add("display", "none");
                                File.Delete(FilePath);
                                return;
                            }
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
                        if (rdbPersonal.Checked)
                        {
                            BindFileMapFld();
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
                    string FileName = Path.GetFileName(FileUpload3.PostedFile.FileName).Replace("'", "");
                    if (FileName != "")
                    {
                        if (Convert.ToString(Session["XLUPLOADED"]) != "Y")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Upload Mobile Number File.');", true);
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
                    Session["SHORTURL"] = ws + s;
                    ShowMsgCharCnt();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('2 more character will be added in the link text for individual mobile numbers. Length of Message characters will be increased by 2.');", true);
                    if (s == "") pnlPopUp_URL_ModalPopupExtender.Show();
                }
            }
        }


        protected void ResendSMSProcess()
        {
            Util ob = new Util();
            if (FileId != null && FileId != "")
            {
                DataTable dt = ob.GetValueForResendSMS(Convert.ToString(FileId), Convert.ToString(Session["UserID"]));
                if (dt.Rows.Count > 0)
                {
                    rdbUpload.Checked = true;
                    ddlSMSType.SelectedValue = dt.Rows[0]["smstype"].ToString();
                    ddlSender.SelectedValue = dt.Rows[0]["sender"].ToString();
                    lblUploading.Text = "" + dt.Rows[0]["FileName"].ToString() + " Uploaded successfully.";
                    txtCampNm.Text = Convert.ToString(dt.Rows[0]["Campaign"]);
                    lblMobileCnt.Text = dt.Rows[0]["smscount"].ToString();
                    txtMsg.Text = dt.Rows[0]["Message"].ToString();
                    DataTable dttemp = ob.GetSApprovedTemplateOfUserResend(Convert.ToString(Session["UserID"]), Convert.ToString(dt.Rows[0]["TemplateId"]));
                    if (dttemp.Rows.Count > 0)
                    {
                        txtPreview.Text = MessageVaribleColorChange(Convert.ToString(dttemp.Rows[0]["template"]));

                        ddlTempID.SelectedValue = Convert.ToString(dttemp.Rows[0]["onlyTemplateID"]);
                        lblTempSMS.Text = MessageVaribleColorChange(Convert.ToString(dttemp.Rows[0]["template"]));
                    }
                    string res = ob.SaveTempTableResendSMS(Convert.ToString(Session["UserID"]), Convert.ToString(FileId), Convert.ToString(dt.Rows[0]["sender"].ToString()), "10");
                    Session["XLUPLOADED"] = "Y";
                    Session["DTXL"] = dt;
                    Session["MOBILECOUNT"] = lblMobileCnt.Text;


                    Session["SHORTURL"] = dt.Rows[0]["shorturl"].ToString();
                    Session["DOMAINNAME"] = dt.Rows[0]["domainname"].ToString();
                    Session["UPLOADFILENM"] = dt.Rows[0]["FILENM"].ToString();
                    Session["UPLOADFILENMEXT"] = dt.Rows[0]["EXTENSION"].ToString();
                }
            }
        }

        //DW
        public void PopulateIsurancecompname()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = database.GetDataTable(@"select * from mstSubClientCode ");

            if (dt.Rows.Count == 0)
            {
                ddlcompname.Items.Clear();
                return;
            }

            ddlcompname.DataSource = dt;
            ddlcompname.DataTextField = "SMS_IC_Name";
            ddlcompname.DataValueField = "SMS_IC_Code";
            ddlcompname.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlcompname.Items.Insert(0, objListItem);
        }

        protected void ResendSMSDuplicateProcess()
        {
            Util ob = new Util();
            if (FId != null && FId != "")
            {
                DataTable dt = ob.GetValueForResendSMS(Convert.ToString(FId), Convert.ToString(Session["UserID"]));
                if (dt.Rows.Count > 0)
                {
                    rdbUpload.Checked = true;
                    ddlSMSType.SelectedValue = dt.Rows[0]["smstype"].ToString();
                    ddlSender.SelectedValue = dt.Rows[0]["sender"].ToString();
                    txtCampNm.Text = Convert.ToString(dt.Rows[0]["Campaign"]);
                    txtMsg.Text = dt.Rows[0]["Message"].ToString();
                    DataTable dttemp = ob.GetSApprovedTemplateOfUserResend(Convert.ToString(Session["UserID"]), Convert.ToString(dt.Rows[0]["TemplateId"]));
                    if (dttemp.Rows.Count > 0)
                    {
                        txtPreview.Text = dttemp.Rows[0]["template"].ToString();
                        ddlTempID.SelectedValue = dttemp.Rows[0]["onlyTemplateID"].ToString();
                        lblTempSMS.Text = dttemp.Rows[0]["template"].ToString();
                    }
                    Session["DTXL"] = dt;
                    Session["SHORTURL"] = dt.Rows[0]["shorturl"].ToString();
                    Session["DOMAINNAME"] = dt.Rows[0]["domainname"].ToString();
                }
            }
        }

        private void HideTemplateIdForeignAcc()
        {
            ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Google RCS"));
            if (Convert.ToString(ddlCCode.SelectedValue) != "91")
            {
                divTempId.Attributes.Add("class", "form-group row d-none");
                divTempsms.Attributes.Add("class", "form-group row d-none");
                if (Convert.ToString(ddlCCode.SelectedValue) == "971")
                    divOptOut.Attributes.Add("Style", "display:block;");
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Campaign"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Promotional"));
            }
            if (Global.openTempAc)
            {
                divTempId.Attributes.Add("class", "form-group row d-none");
                divTempsms.Attributes.Add("class", "form-group row d-none");
            }
            if (Convert.ToBoolean(Session["IsFlashSMS"]) == false)
            {
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Flash SMS"));
            }
            if (_user == "MIM2102290")
            {
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Premium"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Link Text"));
                ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Promotional"));
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
            ResetAllClear();
            ViewState["TemplateID"] = null;
            ViewState["TemplateFields"] = null;
            ViewState["dtMaxLen"] = null;
            ViewState["dtTopRec"] = null;
            Session["NOOFCHARINMSG"] = null;
            PopulateTemplate();
            btnInsTemplate.Visible = false;
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
        }

        public void PopulateEvents()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = database.GetDataTable(@"SELECT EventName + ' (' + EventID + ')' as Events,EventID FROM EventMast WITH(NOLOCK) ORDER BY InsertDate DESC");
            if (dt.Rows.Count == 0)
            {
                ddlEvents.Items.Clear();
                return;
            }

            ddlEvents.DataSource = dt;
            ddlEvents.DataTextField = "Events";
            ddlEvents.DataValueField = "EventID";
            ddlEvents.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlEvents.Items.Insert(0, objListItem);
        }

        public void PopulateDealerCode()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = database.GetDataTable(@"SELECT DLRCODE + ' (' + DLRName + ')' as DLRName,DLRCODE FROM DealerMast WITH(NOLOCK) ORDER BY InsertDate DESC");
            if (dt.Rows.Count == 0)
            {
                ddldlrname.Items.Clear();
                return;
            }

            ddldlrname.DataSource = dt;
            ddldlrname.DataTextField = "DLRName";
            ddldlrname.DataValueField = "DLRCODE";
            ddldlrname.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddldlrname.Items.Insert(0, objListItem);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Valid Captcha!');", true);
                    return;
                }
            }

            if (Convert.ToString(Session["MakerCheckerType"]).ToUpper() == "MAKER")
            {
                FileProcessTableName = "FileProcessMaker";
                MobileDependency = "MobileDependencyMaker";
            }

            moblen = Convert.ToInt32(Convert.ToString(database.GetScalarValue("SELECT moblength FROM tblcountry WHERE counryCode='" + ddlCCode.SelectedValue + "'")));
            Helper.Util ob = new Helper.Util();
            DataTable DTSMPPAC = ob.GetUserSMPPACCOUNTCountry(Convert.ToString(Session["UserID"]), ddlCCode.SelectedValue);
            Session["DTSMPPAC"] = DTSMPPAC;

            bool res;
            if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                res = SendTemplateMSG("");
            else
                res = MsgSend("");
        }

        public string chkSenderMappedOperator(string sender)
        {
            string res = "";

            res = Getsmppaccountuserid(sender);
            if (res == "")
            {
                res = GetUAEAPIAccounts(sender);
            }
            else if (res == "")
            {
                res = GetUAEAPIACCOUNTPROMO(sender);
            }
            return res;
        }

        public string GetUAEAPIACCOUNTPROMO(string senderid)
        {
            string sql = "if exists(select * from UAEAPIACCOUNTPROMO up inner join OperatorSenderId os on up.ACCOUNT=os.Provider where os.Active=1 and os.SID='" + senderid + "' ) begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string GetUAEAPIAccounts(string senderid)
        {
            string sql = "if exists(select * from UAEAPIAccounts ua inner join OperatorSenderId os on ua.ACCOUNT=os.Provider where ua.Active=1 and os.Active=1 and os.SID='" + senderid + "' ) begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string Getsmppaccountuserid(string senderid)
        {
            string sql = @"if exists(select  su.Userid, s.PROVIDER, os.SID from smppaccountuserid su 
                inner join smppsetting s on s.smppaccountid = su.smppaccountid
                inner join OperatorSenderId os on os.Provider = s.PROVIDER where s.ACTIVE = 1 and os.Active=1 and os.SID='" + senderid + "' ) begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string CheckUAEAccount()
        {
            string sql = "if exists (select * from customer where username='" + Session["UserID"].ToString() + "' and  countrycode in ('971','966')) begin select 'true' end";
            string flag = Convert.ToString(database.GetScalarValue(sql));
            return flag;
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
            string MobMIN = Convert.ToString(Session["MobMIN"]);
            string MobMAX = Convert.ToString(Session["MobMAX"]);

            // Check if MobMIN is null, empty, or "0"
            string mobMinCondition = "";
            if (!string.IsNullOrEmpty(MobMIN) && MobMIN != "0")
            {
                mobMinCondition = "OR LEN(MobNo) < " + MobMIN + ""; // Include the condition
            }

            // Check if MobMAX is null, empty, or "0"
            string mobMaxCondition = "";
            if (!string.IsNullOrEmpty(MobMAX) && MobMAX != "0")
            {
                mobMaxCondition = "OR LEN(MobNo) > " + MobMAX + "";
            }

            #region DW
            string dlrcode = "";
            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                dlrcode = "";
                if (rdbImport.Checked == true)
                {
                    IsDLRData = 0;
                }
                else
                {
                    IsDLRData = 1;
                }
            }

            if (Convert.ToString(Session["userId"]).ToLower() == Convert.ToString(insurance).ToLower())
            {
                if (ddlcompname.SelectedValue == "0" || ddlcompname.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Insurance Name !!');", true);
                    return false;
                }

                dlrcode = ddlcompname.SelectedValue;
                IsDLRData = 0;
            }
            #endregion

            if (chkScratchCard.Checked)
            {
                IsScratched = 1;
            }
            if (txtMsg.Text.Length > 3000)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages length exceeds the limit !');", true);
                return false;
            }
            if (ddlEvents.SelectedValue != "" && ddlEvents.SelectedValue != "0")
            {
                IsDLRData = 1;
                EventsCode = ddlEvents.SelectedValue;
            }
            if (txtMsg.Text.ToUpper().Contains("SECURITY-SAFETY.ORG"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SECURITY-SAFETY.ORG link not allowed !');", true);
                return false;
            }
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
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                    {
                        if (ddlEvents.SelectedValue == "0" || ddlEvents.SelectedValue == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                            return false;
                        }
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
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                    {
                        if (ddlEvents.SelectedValue == "0" || ddlEvents.SelectedValue == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                            return false;
                        }
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
                filenm = Convert.ToString(Session["UPLOADFILENM"]); //20
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                tmpfilenm = Convert.ToString(Session["FileNameOnly"]); //""
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

            if (ddlSender.SelectedValue == "0" || ddlSender.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
                return false;
            }
            string Username = Convert.ToString(Session["UserID"]);
            if (Username.ToUpper() == DNDUAEUSERID)
            {
                if (chkOptOut.Checked)
                {
                    if (ddlSender.SelectedValue.Contains("AD-"))
                    {
                        string msg = txtMsg.Text.ToString();
                        if (!msg.Contains("DND7726"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Plases Enter DND !!');", true);
                            return false;
                        }
                    }
                }
            }
            if (rdbEntry.Checked == true)
            {
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (ddldlrname.SelectedValue == "0" || ddldlrname.SelectedValue == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Dealer Name !!');", true);
                        return false;
                    }
                }
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
                    if (ddlCCode.SelectedValue == "91" && (!Global.openTempAc))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Template id');", true);
                        return false;
                    }
                }
            }
            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                {
                    if (ddlEvents.SelectedValue == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
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
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            mobList = mobList.Distinct().ToList();// Remove Duplicate

            mobList.RemoveAll(x => x.Length < moblen);
            mobList = mobList.Select(x => x.Substring(x.Length - moblen)).ToList();

            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);

            if (mobile.Trim() != "")
            {
                if (mobList.Count > 25000)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please use file upload option to send SMS to more than 25000 mobile numbers.');", true);
                    return false;
                }
                if (mobList.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ " + moblen + " or " + Convert.ToInt32(moblen + country_code.Length) + " digits ]');", true);
                    return false;
                }
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers.');", true);
                    return false;
                }
            }

            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text.');", true);
                return false;
            }
            string UserID = Convert.ToString(Session["UserID"]);

            if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must present in Message Text for SMS Type - Smart SMS .');", true);
                return false;
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if ((ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must present in Message Text as you have inserted URL .');", true);
                        return false;
                    }
                }

            string shortURL = "";
            int shortURLId = 0;
            string ws = "";
            if (ddlSMSType.SelectedValue == "2")
            {
                shortURL = Session["SHORTURL"].ToString();
                ws = Convert.ToString(Session["DOMAINNAME"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
                    {
                        shortURL = Session["SHORTURL"].ToString();
                        ws = Convert.ToString(Session["DOMAINNAME"]);
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
            if (UserID.ToUpper() != DNDUAEUSERID)
            {
                if (chkOptOut.Checked) txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            }
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

            if (ddlSMSType.SelectedValue == "2") qlen += 2;
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
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
            rate = (ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
            if (rate <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                return false;
            }
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
            }
            else
            {
                ob.smscount = 1;
                //Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            }

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            double AvailableBalance = ob.CalculateAmount(UserID, cnt, rate, 1);

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

            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    string SaveFileName = Convert.ToString(Session["FileNameOnly"]);
                    string scheduleDate = liScheduleDates.FirstOrDefault();
                    // ob.Schedule_SMS_BULK(UserID, txtMsg.Text.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, mobList, "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm);
                    ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);

                    //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                    string CheckLast = ob.CheckLastProcessTime(UserID, filenm);
                    if (CheckLast == "FileName")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    else if (CheckLast == "TimeDiff")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    //----------END-------------

                    string sq = @"declare @maxid INT 
                    SET @maxid=isnull((SELECT max(id) FROM " + FileProcessTableName + @"),0)+1
                    INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,isschedule,
                    scheduletime,ccode,fileext,rate,smstype,methodname,shortURLId,shortURL,domainname,ucs2,noofsms,campname,scratchcard,EventCode,
                    IsDLRData,dlrcode,SaveFileName)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"'" +
                    ",'" + ddlSender.SelectedValue + @"',0,Null,1,'" + scheduleDate + @"','" + country_code + @"','" + filenmext + @"',
                    " + rate + ",'" + ddlSMSType.SelectedValue + @"','Schedule_SMS_BULK', '" + shortURLId + @"','" + shortURL + @"','" + ws + @"',
                    '" + ucs2 + @"'," + noofsms + ",'" + txtCampNm.Text.Trim() + @"'," + IsScratched + ",'" + EventsCode + @"',
                    " + IsDLRData + ",'" + dlrcode + "','" + SaveFileName + "' " +
                     " SELECT * INTO " + tblname + @" from " + dbName + ".dbo." + user + @";
                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                     SELECT @maxid, MOBNO, 'I', GETDATE() 
                     FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                     WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			         SELECT @maxid, MOBNO, 'D', GETDATE()
                     FROM " + dbName + "..tblIncorrect_" + UserID + @"
			         GROUP BY MOBNO
			         HAVING COUNT(*) > 1; ";

                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                    {
                        ob.InsertIntoHERODLRDATA(dbName, user);
                    }

                    if (lstMappedFields != null)
                    {
                        if (lstMappedFields.Items.Count > 0)
                        {
                            sq = sq + @" INSERT INTO MapFields (id,mapfieldname) ";
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

                    // 05/07/2022
                    double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                    string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                               "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalScheduleCreation.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                    database.ExecuteNonQuery(sql1);

                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @"; Create table " + user + @" (MobileNo numeric) ;  ");
                        foreach (var m in mobList)
                        {
                            database.ExecuteNonQuery(" INSERT into " + user + @" values ('" + m + "')");
                        }
                        database.ExecuteNonQuery("delete d FROM " + user + @" d inner join globalBlackListNo b on b.mobile=d.MobileNo ");

                        // foreach (string scheduleDate in liScheduleDates)
                        {
                            string campName = "Manual";
                            string scheduleDate = liScheduleDates.FirstOrDefault();
                            // ob.Schedule_SMS_BULK(UserID, txtMsg.Text.Trim(), ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, mobList, "MANUAL", templID, country_code, PrevBalance, AvailableBalance);
                            ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);

                            //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                            string CheckLast = ob.CheckLastProcessTime(UserID);
                            if (CheckLast == "TimeDiff")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                                return false;
                            }
                            //----------END-------------

                            string sq = @"declare @maxid int 
                    set @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1
                    INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,isschedule,scheduletime,fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"'," +
                    "'" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",1,'" + scheduleDate + "','" + filenmext + "'," + rate + ",'Schedule_SMS_BULK'," + IsScratched + ",'" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + "'" +
                    " SELECT * INTO " + tblname + @" FROM " + dbName + ".dbo." + user + @";
                    INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                    SELECT @maxid, MOBNO, 'I', GETDATE() 
                     FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                     WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			        INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			        SELECT @maxid, MOBNO, 'D', GETDATE()
                    FROM " + dbName + "..tblIncorrect_" + UserID + @"
			        GROUP BY MOBNO
			        HAVING COUNT(*) > 1; ";

                            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                            {
                                ob.InsertIntoHERODLRDATA(dbName, user);
                            }

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

                            // 05/07/2022
                            double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                            string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalScheduleCreation.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                            database.ExecuteNonQuery(sql1);
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

                        #region DW
                        string schdate = liScheduleDates.FirstOrDefault();
                        string sql = " Insert into SMSFILEUPLOAD (USERID,RECCOUNT,senderid,schedule,campaignname,SMSRATE,shortURLId,COUNTRYCODE,dlrcode) values ('" + UserID + "','" + mobList.Count.ToString() + "', '" + ddlSender.Text + "','" + schdate + "','Manual','" + rate + "','" + Convert.ToString(shortURLId) + "','" + country_code + "','" + dlrcode + "')" +
                                        " declare @id numeric(10) select @id = max(id) from SMSFILEUPLOAD where userid='" + UserID + "'  select @id ;";
                        string fileId = Convert.ToString(database.GetScalarValue(sql));
                        #endregion

                        foreach (var m in mobList)
                        {
                            string msg = txtMsg.Text.Trim();
                            string shurl = "";
                            string mseg = "";
                            if (ddlSMSType.SelectedValue == "2")
                            {
                                //if (UserID == "MIM2002132") mseg = "Oncquest" + ob.NewSegment8();
                                //else 
                                mseg = ob.NewSegment8();

                                shurl = ws + mseg;
                                msg = msg.Replace(shortURL, shurl);
                            }
                            //insert record to blocksmswhitelist ADD by VIkas On 13-09-2023
                            try
                            {
                                database.ExecuteNonQuery("INSERT INTO blocksmswhitelist (userid,mobile,insertdate,isAuto) VALUES('" + UserID + "','" + m + "',GETDATE(),1)");
                            }
                            catch (Exception e2) { }
                            // check for blaclist no mob entry 15/09/2021
                            int chk = int.Parse(Convert.ToString(database.GetScalarValue("select count(1) from globalBlackListNo where mobile='" + m + "'")));
                            if (chk == 0)
                            {
                                //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                                string resp = "";
                                //ob.SendURL_SMS(UserID, country_code + m, msg, ddlSender.Text);
                                string scheduleDate = liScheduleDates.FirstOrDefault();

                                ob.B4SEND_Schedule_SMS(UserID, m, msg, ddlSender.Text, scheduleDate, shortURLId, shortURL, ws, mseg, rate, ddlSMSType.SelectedValue, dtSMPPAC, ucs2, templID, country_code, fileId, dlrcode);
                                ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
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
                    string SaveFileName = Convert.ToString(Session["FileNameOnly"]);
                    //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                    string CheckLast = ob.CheckLastProcessTime(UserID, filenm);
                    if (CheckLast == "FileName")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    else if (CheckLast == "TimeDiff")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    //----------END-------------

                    if (ddlSMSType.SelectedValue == "2")
                    {
                        string sql = "SELECT * into " + tblname + @" from " + dbName + ".dbo." + user;
                        dbB4Link.ExecuteNonQuery(sql);

                        string sq = @"declare @maxid int 
                                      SET @maxid=isnull((SELECT max(id) From " + FileProcessTableName + @"),0)+1 ";

                        if (lstMappedFields != null)
                        {
                            if (lstMappedFields.Items.Count > 0)
                            {
                                sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                    else
                                    {
                                        sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                }
                            }
                        }

                        sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,
                                     ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,
                                     methodname,scratchcard,EventCode,isdlrdata,dlrcode,SaveFileName)
                                     SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',
                                     N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"',
                                     '" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"',
                                     '" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                                     " + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"',
                                     " + IsDLRData + ",'" + dlrcode + "','" + SaveFileName + @"';
                                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                                     SELECT @maxid, MOBNO, 'I', GETDATE() 
                                     FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                     WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                         INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                         SELECT @maxid, MOBNO, 'D', GETDATE()
                                     FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                         GROUP BY MOBNO
			                         HAVING COUNT(*) > 1; ";

                        dbB4Link.ExecuteNonQuery(sq);

                        if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                        {
                            ob.InsertIntoHERODLRDATA(dbName, user);
                        }

                        // 05/07/2022
                        double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                        string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                            "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                        database.ExecuteNonQuery(sql1);

                    }
                    else
                    {
                        if (Session["SHORTURL"] != null)
                        {
                            if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                            {
                                if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
                                {
                                    string sql = "SELECT * into " + tblname + @" from " + dbName + ".dbo." + user;
                                    dbB4Link.ExecuteNonQuery(sql);

                                    string sq = @"declare @maxid int 
                                                  SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";

                                    if (lstMappedFields != null)
                                    {
                                        if (lstMappedFields.Items.Count > 0)
                                        {
                                            sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                                else
                                                {
                                                    sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                                }
                                            }
                                        }
                                    }

                                    sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,
                                    processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,
                                    fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode,SaveFileName)
                                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',
                                    N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"',
                                    '" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"',
                                    '" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                                    " + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"',
                                    " + IsDLRData + ",'" + dlrcode + "','" + SaveFileName + @"';
                                    INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                                    SELECT @maxid, MOBNO, 'I', GETDATE() 
                                    FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                    WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                        INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                        SELECT @maxid, MOBNO, 'D', GETDATE()
                                    FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                        GROUP BY MOBNO
			                        HAVING COUNT(*) > 1; ";

                                    dbB4Link.ExecuteNonQuery(sq);

                                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                                    {
                                        ob.InsertIntoHERODLRDATA(dbName, user);
                                    }

                                    //05/07/2022
                                    double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                                    string sql1 = "UPDATE customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                        "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                        "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                                    database.ExecuteNonQuery(sql1);
                                }
                            }
                            else
                            {
                                string sql = "SELECT * INTO " + tblname + @" FROM " + dbName + ".dbo." + user;
                                dbB4Link.ExecuteNonQuery(sql);

                                string sq = @"declare @maxid int 
                                              SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";

                                if (lstMappedFields != null)
                                {
                                    if (lstMappedFields.Items.Count > 0)
                                    {
                                        sq = sq + @" INSERT into MapFields (id,mapfieldname) ";
                                        for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                            else
                                            {
                                                sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                        }
                                    }
                                }

                                sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,
                                     processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,
                                     fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode,SaveFileName)
                                     SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',
                                     N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"',
                                     '" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"',
                                     '" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                                     " + rate + ",'InsertSMSrecordsFromUSERTMP'," + IsScratched + ",'" + EventsCode + @"',
                                     " + IsDLRData + ",'" + dlrcode + "','" + SaveFileName + @"';
                                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                                     SELECT @maxid, MOBNO, 'I', GETDATE() 
                                     FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                     WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                         INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                         SELECT @maxid, MOBNO, 'D', GETDATE()
                                     FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                         GROUP BY MOBNO
			                         HAVING COUNT(*) > 1; ";

                                dbB4Link.ExecuteNonQuery(sq);
                                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                                {
                                    ob.InsertIntoHERODLRDATA(dbName, user);
                                }

                                // 05/07/2022
                                double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                                string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                    "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                     "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                                database.ExecuteNonQuery(sql1);
                            }
                        }
                        else
                        {
                            string sql = "SELECT * INTO " + tblname + @" FROM " + dbName + ".dbo." + user;
                            dbB4Link.ExecuteNonQuery(sql);

                            string sq = @" DECLARE @maxid INT 
                                           SET @maxid=isnull((SELECT max(id) FROM " + FileProcessTableName + @"),0)+ 1";

                            if (lstMappedFields != null)
                            {
                                if (lstMappedFields.Items.Count > 0)
                                {
                                    sq = sq + @" INSERT INTO MapFields (id,mapfieldname) ";
                                    for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                        else
                                        {
                                            sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                        }
                                    }
                                }
                            }

                            sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,
                                         processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,
                                         availablebalance,fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode,SaveFileName)
                            SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"',
                            '" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"',
                            '" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                            " + rate + ",'InsertSMSrecordsFromUSERTMP'," + IsScratched + ",'" + EventsCode + @"',
                            " + IsDLRData + ",'" + dlrcode + "','" + SaveFileName + @"';
                            INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                            SELECT @maxid, MOBNO, 'I', GETDATE() 
                            FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                            WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                SELECT @maxid, MOBNO, 'D', GETDATE()
                            FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                GROUP BY MOBNO
			                HAVING COUNT(*) > 1; ";
                            dbB4Link.ExecuteNonQuery(sq);

                            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                            {
                                ob.InsertIntoHERODLRDATA(dbName, user);
                            }

                            // 05/07/2022
                            double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                            string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                            database.ExecuteNonQuery(sql1);

                        }
                    }
                }
                else
                {
                    //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                    string CheckLast = ob.CheckLastProcessTime(UserID);
                    if (CheckLast == "TimeDiff")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    //----------END-------------

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
                        if (ddlSMSType.SelectedValue == "2")
                        {
                            string sql = "select* into " + tblname + @" from " + dbName + ".dbo." + user;
                            dbB4Link.ExecuteNonQuery(sql);

                            string sq = @"declare @maxid INT 
                            SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";

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

                            sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,
                            smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,scratchcard,EventCode,
                            isdlrdata,dlrcode)
                            SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"',
                            '" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"',
                            '" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                            " + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + "' ";

                            dbB4Link.ExecuteNonQuery(sq);

                            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                            {
                                ob.InsertIntoHERODLRDATA(dbName, user);
                            }
                            //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text.Trim(), ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, mobList, "MANUAL", "", templID, country_code);

                            // 05/07/2022
                            double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                            string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                            database.ExecuteNonQuery(sql1);
                        }
                        else
                        {
                            if (Session["SHORTURL"] != null)
                            {
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
                                    {
                                        string sql = "select* into " + tblname + @" from " + dbName + ".dbo." + user;
                                        dbB4Link.ExecuteNonQuery(sql);

                                        string sq = @"declare @maxid int 
                                        set @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";

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

                                        sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,
                                        ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,
                                        scratchcard,EventCode,isdlrdata,dlrcode)
                                        SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                                        "'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + "'";

                                        dbB4Link.ExecuteNonQuery(sq);

                                        if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                                        {
                                            ob.InsertIntoHERODLRDATA(dbName, user);
                                        }

                                        // 05/07/2022
                                        double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                                        string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                           "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                                        database.ExecuteNonQuery(sql1);

                                    }
                                }
                                else
                                {
                                    string sql = "select * into " + tblname + @" from " + dbName + ".dbo." + user;
                                    dbB4Link.ExecuteNonQuery(sql);

                                    string sq = @"declare @maxid INT 
                                    SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";
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

                                    sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode)
                                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0 " +
                                    ",Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + "'";

                                    dbB4Link.ExecuteNonQuery(sq);

                                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                                    {
                                        ob.InsertIntoHERODLRDATA(dbName, user);
                                    }
                                    // 05/07/2022
                                    double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                                    string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                       "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                       "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                                    database.ExecuteNonQuery(sql1);
                                }
                            }
                            else
                            {
                                string sql = "select* into " + tblname + @" from " + dbName + ".dbo." + user;
                                dbB4Link.ExecuteNonQuery(sql);

                                string sq = @"declare @maxid INT 
                                SET @maxid=ISNULL((SELECT max(id) FROM " + FileProcessTableName + @"),0)+1 ";

                                if (lstMappedFields != null)
                                {
                                    if (lstMappedFields.Items.Count > 0)
                                    {
                                        sq = sq + @" insert into MapFields (id,mapfieldname) ";
                                        for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                            else
                                            {
                                                sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                            }
                                        }
                                    }
                                }

                                sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,scratchcard,EventCode,isdlrdata,dlrcode)
                    SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',0,Null," +
                    "'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + campName + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + "'," + rate + ",'Insert_SMS_BULK_4url'," + IsScratched + ",'" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + "' ";

                                dbB4Link.ExecuteNonQuery(sq);

                                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                                {
                                    ob.InsertIntoHERODLRDATA(dbName, user);
                                }

                                // 05/07/2022
                                double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                                string sql1 = "update customer set balance = balance - '" + bal1 + "' where username = '" + UserID + "' ; " +
                                    "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                                "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                                database.ExecuteNonQuery(sql1);
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


                        string sql = @" Insert into SMSFILEUPLOAD (USERID,RECCOUNT,senderid,campaignname,smsrate,shortURLId,countrycode,dlrcode)
                                   values ('" + UserID + "','" + mobList.Count.ToString() + "','" + ddlSender.SelectedValue + "','Manual','" + rate + "','" + Convert.ToString(shortURLId) + "','" + country_code + "','" + dlrcode + "') " +
                                "  select max(id) from SMSFILEUPLOAD where userid='" + UserID + "' ; ";
                        string fileId = Convert.ToString(database.GetScalarValue(sql));
                        string HeroDLRCode = "";
                        if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                        {
                            if (Convert.ToString(ddldlrname.SelectedValue) != "0" && Convert.ToString(ddldlrname.SelectedValue) != "")
                            {
                                for (int i = 0; i < mobList.Count; i++)
                                {
                                    HeroDLRCode = ddldlrname.SelectedValue;
                                    string MobNo = database.Right(Convert.ToString(mobList[i]), 10);
                                    string SqlDLR = @"insert into HERODLRDATA(FileProcessID,MobileNo,DLRCode,ProfileId) values('0','" + ddlCCode.SelectedValue + MobNo + "','" + ddldlrname.SelectedValue + "','" + _user + "')";
                                    database.ExecuteNonQuery(SqlDLR);
                                }
                            }
                        }

                        foreach (var m in mobList)
                        {
                            string msg = txtMsg.Text;
                            string shurl = "";
                            string mseg = "";
                            if (ddlSMSType.SelectedValue == "2")
                            {
                                mseg = ob.NewSegment8();
                                shurl = ws + mseg;
                                msg = msg.Replace(shortURL, shurl);
                            }
                            if (Session["SHORTURL"] != null)
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
                                    {
                                        mseg = ob.NewSegment8();
                                        shurl = ws + mseg;
                                        msg = msg.Replace(shortURL, shurl);
                                    }
                                }
                            //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                            string resp = "";
                            //insert record to blocksmswhitelist ADD by VIkas On 13-09-2023
                            try
                            {
                                database.ExecuteNonQuery("INSERT INTO blocksmswhitelist (userid,mobile,insertdate,isAuto) VALUES('" + UserID + "','" + m + "',GETDATE(),1)");
                            }
                            catch (Exception e2) { }
                            // check for mob entry blaclistno rabi 15/09/2021
                            int chk = int.Parse(Convert.ToString(database.GetScalarValue("select count(1) from globalBlackListNo where mobile='" + m + "'")));
                            if (chk == 0)
                            {
                                ob.B4SEND_SendURL_SMS(UserID, m, msg.Trim(), ddlSender.Text, dtSMPPAC, ucs2, bulk, rate, noofsms, templID, country_code, ddlSMSType.SelectedValue, fileId, dlrcode);
                                if (ddlSMSType.SelectedValue == "2") ob.SaveURL_MOBILE(UserID, shortURLId, m, mseg, resp, country_code, fileId, templID);
                                if (Session["SHORTURL"] != null)
                                    if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                    {
                                        if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8") ob.SaveURL_MOBILE(UserID, shortURLId, m, mseg, resp, country_code, fileId, templID);
                                    }
                            }
                        }
                    }
                }
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "maker")
                {
                   ob.SendEmailInCaseOfMaker(_user, Convert.ToString(Session["DLT"]),Convert.ToString(txtCampNm.Text.Trim()), filenm.Replace("'", "''"),cnt, templID, txtMsg.Text.Trim().Replace("'", "''"));
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send-sms-u-B4Send.aspx';", true);
            }
            return true;
        }

        public bool SendTemplateMSG(string sch)
        {
            string MobMIN = Convert.ToString(Session["MobMIN"]);
            string MobMAX = Convert.ToString(Session["MobMAX"]);

            // Check if MobMIN is null, empty, or "0"
            string mobMinCondition = "";
            if (!string.IsNullOrEmpty(MobMIN) && MobMIN != "0")
            {
                mobMinCondition = "OR LEN(MobNo) < " + MobMIN + ""; // Include the condition
            }

            // Check if MobMAX is null, empty, or "0"
            string mobMaxCondition = "";
            if (!string.IsNullOrEmpty(MobMAX) && MobMAX != "0")
            {
                mobMaxCondition = "AND LEN(MobNo) > " + MobMAX + "";
            }

            Helper.Util ob = new Helper.Util();
            txtMsg.Text = txtMsg.Text.Replace("\r\n", "\n");

            #region DW
            string dlrcode = "";
            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                dlrcode = "";
                if (rdbImport.Checked == true)
                {
                    IsDLRData = 0;
                }
                else
                {
                    IsDLRData = 1;
                }
            }
            if (Convert.ToString(Session["userId"]).ToLower() == Convert.ToString(insurance).ToLower())
            {
                if (ddlcompname.SelectedValue == "0" || ddlcompname.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Insurance Name !!');", true);
                    return false;
                }
                dlrcode = ddlcompname.SelectedValue;
                IsDLRData = 0;
            }
            #endregion

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
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                    {
                        if (ddlEvents.SelectedValue == "0" || ddlEvents.SelectedValue == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                            return false;
                        }
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
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                    {
                        if (ddlEvents.SelectedValue == "0" || ddlEvents.SelectedValue == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                            return false;
                        }
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
            string Username = Convert.ToString(Session["UserID"]);
            if (Username.ToUpper() == DNDUAEUSERID)
            {
                if (chkOptOut.Checked)
                {
                    if (ddlSender.SelectedValue.Contains("AD-"))
                    {
                        string msg = txtMsg.Text.ToString();
                        if (!msg.Contains("DND7726"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Plases Enter DND !!');", true);
                            return false;
                        }
                    }
                }
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


            if (Convert.ToString(Session["DLT"]).ToLower() != Convert.ToString(DLTNO).ToLower())
            {
                if (_user.ToUpper() == "MIM2002097" && Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    string SqlIns = @"IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DlrCode' AND Object_ID = Object_ID(N'dbo.tmp_" + _user + @"'))
                                    BEGIN
                                      insert into HondaDLRMobile(MobileNo,DlrCode,CampaignName) 
                                      SELECT  MobNo,DlrCode,'" + txtCampNm.Text.Trim() + "' from  tmp_" + _user + @"
                                    END";
                    try
                    {
                        database.ExecuteNonQuery(SqlIns);
                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('DlrCode Field must be present in the uploaded file.');", true);
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
            //if (txtMobNum.Text != "") mobile = txtMobNum.Text;
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            mobList = mobList.Distinct().ToList();

            mobList.RemoveAll(x => x.Length < moblen);
            // mobList.RemoveAll(x => x.Length > Convert.ToInt32(moblen + country_code.Length));
            mobList = mobList.Select(x => x.Substring(x.Length - moblen)).ToList();

            if (mobile.Trim() != "")
            {
                if (mobList.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ " + moblen + " or " + Convert.ToInt32(moblen) + Convert.ToInt32(country_code.Length) + " digits ]');", true);
                    return false;
                }
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers.');", true);
                    return false;
                }
            }

            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text.');", true);
                return false;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must present in Message Text for SMS Type - Smart SMS .');", true);
                return false;
            }

            string shortURL = "";
            int shortURLId = 0;
            string ws = "";
            if (ddlSMSType.SelectedValue == "2")
            {
                shortURL = Session["SHORTURL"].ToString();
                ws = Convert.ToString(Session["DOMAINNAME"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }

            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                    {
                        shortURL = Session["SHORTURL"].ToString();
                        ws = Convert.ToString(Session["DOMAINNAME"]);
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
            if (UserID.ToUpper() != DNDUAEUSERID)
            {
                if (chkOptOut.Checked) txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            }
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

            if (ddlSMSType.SelectedValue == "2") qlen += 2;

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
            //check whether balance is available or not ---

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
                ob.smscount = liScheduleDates.Count;
            }
            else
            {
                ob.smscount = 1;
            }

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            double AvailableBalance = ob.CalculateAmount(UserID, cnt, rate, 1);

            DataTable dtSMPPAC = new DataTable();

            DataTable dtCols = (DataTable)ViewState["dtMaxLen"];
            List<string> tempFields = (List<string>)ViewState["TemplateFields"];
            string templateID = Convert.ToString(ViewState["TemplateID"]);

            if (Global.openTempAc)
            {
                if (qlen > 180)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message has a maximum limit 180 characters');", true);
                    return false;
                }
            }
            string tblname = Convert.ToString(Session["UserId"]) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            if (ddlEvents.SelectedValue != "" && ddlEvents.SelectedValue != "0")
            {
                IsDLRData = 1;
                EventsCode = ddlEvents.SelectedValue;
            }

            int AutoMapping = 0;
            if (chkAutoMapping.Checked)
            {
                AutoMapping = 1;
            }

            string tblname_Ac = tblname + "Ac";
            string user = "tmp_" + UserID;
            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    string SaveFileName = Convert.ToString(Session["FileNameOnly"]);
                    //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                    string CheckLast = ob.CheckLastProcessTime(UserID, filenm);
                    if (CheckLast == "FileName")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    else if (CheckLast == "TimeDiff")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    //----------END-------------

                    // foreach (string scheduleDate in liScheduleDates)
                    {
                        string scheduleDate = liScheduleDates.FirstOrDefault();
                        string sql = "SELECT * INTO " + tblname + @" FROM " + dbName + ".dbo." + user;
                        dbB4Link.ExecuteNonQuery(sql);

                        string sq = @"declare @maxid int 
                                     SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1
                                     INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,
                                                 shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,IsSchedule,
                                                 ScheduleTime,EventCode,IsDLRData,dlrcode,IsAutoMapping,SaveFileName)
                                     SELECT @maxid,'" + UserID + @"' ,'" + filenm + @"' ,'" + tblname + @"','" + cnt + @"','" + templateID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"',
                                     '" + ddlSender.SelectedValue + @"',0,Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"',
                                     '" + shortURL + @"','" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext + @"',
                                     " + rate + ",'InsertTemplateSMSrecordsFromUSERTMP',1,'" + scheduleDate + "','" + EventsCode + @"',
                                     '" + IsDLRData + "','" + dlrcode + "'," + AutoMapping + ",'" + SaveFileName + @"';
                                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                                     SELECT @maxid, MOBNO, 'I', GETDATE() 
                                     FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                     WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                         INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                         SELECT @maxid, MOBNO, 'D', GETDATE()
                                     FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                         GROUP BY MOBNO
			                         HAVING COUNT(*) > 1; ";

                        if (lstMappedFields != null)
                        {
                            if (lstMappedFields.Items.Count > 0)
                            {
                                sq = sq + @" INSERT INTO MapFields (id,mapfieldname) ";
                                for (int i = 0; i < lstMappedFields.Items.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                    else
                                    {
                                        sq = sq + @" UNION SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                    }
                                }
                            }
                        }

                        dbB4Link.ExecuteNonQuery(sq);

                        // 05/07/2022
                        double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                        string sql1 = "UPDATE customer SET balance = balance - '" + bal1 + "' WHERE username = '" + UserID + "' ; " +
                               "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalScheduleCreation.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                        database.ExecuteNonQuery(sql1);

                        ob.GetSchedule_SMS(liScheduleDates, UserID, country_code);
                    }
                }
            }
            else
            {
                if (cnt > 10)
                {
                    //Global.Istemplatetest = ob.TestSmsbeforeSend(_user, templateID, txtMsg.Text.Trim(), ddlSender.Text, Convert.ToString(Session["PEID"]));
                }
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    string SaveFileName = Convert.ToString(Session["FileNameOnly"]);
                    //---- Add Method for check Last Insert record fill process table ---- Add by Vikas ON 09-08-2023
                    string CheckLast = ob.CheckLastProcessTime(UserID, filenm);
                    if (CheckLast == "FileName")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    else if (CheckLast == "TimeDiff")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                        return false;
                    }
                    //----------END-------------

                    string sql = "SELECT * INTO " + tblname + @" FROM " + dbName + ".dbo." + user;
                    dbB4Link.ExecuteNonQuery(sql);

                    string sq = @"declare @maxid INT 
                    SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1 ";

                    if (lstMappedFields != null)
                    {
                        if (lstMappedFields.Items.Count > 0)
                        {
                            sq = sq + @" INSERT INTO MapFields (id,mapfieldname) ";
                            for (int i = 0; i < lstMappedFields.Items.Count; i++)
                            {
                                if (i == 0)
                                {
                                    sq = sq + @" SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                                else
                                {
                                    sq = sq + @" Union SELECT @maxid,'" + lstMappedFields.Items[i].Text.Trim() + "' ";
                                }
                            }
                        }
                    }
                    sq = sq + @" INSERT INTO " + FileProcessTableName + @"(id,profileid,fileName,tblname,noofrecord,templateid,msg,sender,isprocessed,processedtime,ccode,smstype,
                                 shorturlid,shorturl,domainname,ucs2,noofsms,campname,prevbalance,availablebalance,fileext,rate,methodname,EventCode,
                                 isdlrdata,dlrcode,IsAutoMapping,SaveFileName)
                                 SELECT @maxid,'" + UserID + @"' ,'" + filenm.Replace("'", "''") + @"' ,'" + tblname + @"','" + cnt + @"',
                                 '" + templateID + @"',N'" + txtMsg.Text.Trim().Replace("'", "''") + @"','" + ddlSender.SelectedValue + @"',
                                 0,Null,'" + country_code + @"','" + ddlSMSType.SelectedValue + @"','" + shortURLId.ToString() + @"','" + shortURL + @"',
                                 '" + ws + @"','" + ucs2 + @"','" + noofsms + @"','" + txtCampNm.Text + @"'," + PrevBalance + "," + PrevBalance + ",'" + filenmext.Replace("'", "''") + @"',
                                 " + rate + ",'InsertTemplateSMSrecordsFromUSERTMP','" + EventsCode + @"'," + IsDLRData + ",'" + dlrcode + @"',
                                 " + AutoMapping + ",'" + SaveFileName + @"';
                                 INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
                                 SELECT @maxid, MOBNO, 'I', GETDATE() 
                                 FROM " + dbName + "..tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                 WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + @"; 
			                     INSERT INTO " + MobileDependency + @" (FileProcessId, MoblieNo, [TYPE], InsertDateTime)
			                     SELECT @maxid, MOBNO, 'D', GETDATE()
                                 FROM " + dbName + "..tblIncorrect_" + UserID + @"
			                     GROUP BY MOBNO
			                     HAVING COUNT(*) > 1; ";

                    dbB4Link.ExecuteNonQuery(sq);

                    if (Convert.ToString(Session["DLT"]).ToLower() != Convert.ToString(DLTNO).ToLower())
                    {
                        string sqlhondafile = @"declare @maxid INT 
                             SET @maxid=isnull((select max(id) From " + FileProcessTableName + @"),0)+1
                                UPDATE HondaDLRMobile SET ProcessId =@maxid  WHERE CampaignName='" + txtCampNm.Text.Trim() + "'";
                        database.ExecuteNonQuery(sqlhondafile);
                    }

                    // 05/07/2022
                    double bal1 = new Util().CalculateSMSCost(noofsms * cnt, Convert.ToDouble(rate));
                    string sql1 = "UPDATE customer SET balance = balance - '" + bal1 + "' WHERE username = '" + UserID + "' ; " +
                            "INSERT INTO userBalCrDr (username,trantype,balance,trandate,tranby,REMARKS,clickrecharge,SMSrate) " +
                         "VALUES('" + UserID + "','D','" + bal1 + "',GETDATE(),'" + UserID + "','" + DebitedBalSendSMS.Replace("{1}", txtCampNm.Text.Trim()).Replace("{2}", DateTime.Now.ToString()) + "','0','" + rate + "')";
                    database.ExecuteNonQuery(sql1);
                }
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "maker")
                {
                    ob.SendEmailInCaseOfMaker(_user, Convert.ToString(Session["DLT"]), Convert.ToString(txtCampNm.Text.Trim()), filenm.Replace("'", "''"), cnt, templateID, txtMsg.Text.Trim().Replace("'", "''"));
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send-sms-u-B4Send.aspx';", true);
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
                TimeSpan start = TimeSpan.Parse(Convert.ToString(dt.Rows[0]["smsFromTime"]));
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
            Response.Redirect("send-sms-u-B4Send.aspx");
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            #region ForScheduleValidation
            Helper.Util ob = new Helper.Util();
            if (txtMsg.Text.Length > 3000)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages length exceeds the limit !');", true);
                return;
            }
            if (txtMsg.Text.ToUpper().Contains("SECURITY-SAFETY.ORG"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SECURITY-SAFETY.ORG link not allowed !');", true);
                return;
            }
            string _smstype = ddlSMSType.SelectedValue;
            DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            string fCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsFromTime"]) : "";
            string tCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsToTime"]) : "";

            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                {
                    if (ddlEvents.SelectedValue == "0" || ddlEvents.SelectedValue == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                        return;
                    }
                }
            }
            string tmpfilenm = "";
            string filenm = "";
            string filenmext = "";
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]); //20
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                tmpfilenm = Convert.ToString(Session["FileNameOnly"]); //""
                txtMobNum.Text = "";

                if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                {
                    if (txtCampNm.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Campaign Name');", true);
                        return;
                    }
                    if (ob.CampaignExistsForDay(_user, txtCampNm.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Name already exists for the day. Please Enter another campaign name.');", true);
                        return;
                    }
                }
            }

            if (ddlSender.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
                return;
            }

            if (rdbEntry.Checked == true)
            {
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    if (ddldlrname.SelectedValue == "0" || ddldlrname.SelectedValue == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Dealer Name !!');", true);
                        return;
                    }
                }
            }

            if (ddlSMSType.SelectedValue == "1" || ddlSMSType.SelectedValue == "2")
            {
                int senderValue = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out senderValue);
                if (isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender can not be numeric for Premium or Link SMS');", true);
                    return;
                }
            }

            if (ddlSMSType.SelectedValue == "6")
            {
                int senderValue = 0;
                bool isNumeric = int.TryParse(ddlSender.SelectedValue, out senderValue);
                if (!isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender should be numeric for Promotional SMS');", true);
                    return;
                }
            }

            if (ddlSMSType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select SMS Type');", true);
                return;
            }
            if (ddlSMSType.SelectedValue != "3")
            {
                if (ddlTempID.SelectedValue == "0" && Helper.database.TemplateIDMandatory() == "Y")
                {

                }
            }
            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                if (rdbUpload.Checked == true || rdbPersonal.Checked == true || rdbImport.Checked == true)
                {
                    if (ddlEvents.SelectedValue == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Event !!');", true);
                        return;
                    }
                }
            }

            string country_code = ddlCCode.SelectedValue;
            string mobile = "";
            if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            mobList = mobList.Distinct().ToList();
            mobList.RemoveAll(x => x.Length < moblen);
            mobList = mobList.Select(x => x.Substring(x.Length - moblen)).ToList();
            if (mobile.Trim() != "")
            {
                if (mobList.Count > 25000)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please use file upload option to send SMS to more than 25000 mobile numbers.');", true);
                    return;
                }
                if (mobList.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ " + moblen + " or " + Convert.ToInt32(moblen + country_code.Length) + " digits ]');", true);
                    return;
                }
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers.');", true);
                    return;
                }
            }
            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text.');", true);
                return;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must present in Message Text for SMS Type - Smart SMS .');", true);
                return;
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if ((ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") && (!(txtMsg.Text.ToLower().Contains(Convert.ToString(Session["DOMAINNAME"]).ToLower()))))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must present in Message Text as you have inserted URL .');", true);
                        return;
                    }
                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        string CheckLast = ob.CheckLastProcessTime(UserID);
                        if (CheckLast == "TimeDiff")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + WithOutLastFileNameAlertMsg + "');", true);
                            return;
                        }
                    }
                }
            #endregion

            ClearScheduleDateTime();
            pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.ID != "btnUploadMedia")
            {

            }
            else
            {
                if (FileUpload2.HasFile)
                {

                }
            }
        }

        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm, string Dltno = "")
        {
            string SheetName = "";
            DataTable dt = new DataTable();
            if (Extension.ToLower().Contains(".xls"))
            {

            }
            SheetName = "Sheet1$";
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            
            string mobLen = Convert.ToString(Session["mobLength"]);
            string MobMIN = Convert.ToString(Session["MobMIN"]);
            string MobMAX = Convert.ToString(Session["MobMAX"]);
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]).Trim() == "971")
            { MobMIN = "9"; MobMAX = "9"; }
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]).Trim() == "91")
            { MobMIN = "10"; MobMAX = "10"; }

            string res = ob.SaveTempTable(FilePath, SheetName, _user, Extension, folder, filenm, ddlSender.SelectedValue, mobLen, MobMIN, MobMAX); // Change BY Vikas on 16-10-2023 add MObMIn And MObMAX Prameter
            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
            {
                if (res.Contains("RECORDCOUNT"))
                {
                    string TempTable = "tmp_" + _user;
                    string CheckDlrCode = Convert.ToString(database.GetScalarValue(@"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + TempTable + "' and COLUMN_NAME='DLRCODE'"));
                    if (CheckDlrCode == "")
                    {
                        return res = "DLRCODE MANDATORY";
                    }
                    else
                    {
                        database.ExecuteNonQuery("UPDATE " + TempTable + " SET DLRCode = 'Common' WHERE ISNULL(DLRCode,'')=''");
                    }
                }
            }
            txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text;
                lblMobTotalCount.Text = Convert.ToString(Session["UploadedCount"]);
                lblMobDuplicateCount.Text = Convert.ToString(Session["DuplicateCount"]);

                // Check if MobMIN is null, empty, or "0"
                string mobMinCondition = "";
                if (!string.IsNullOrEmpty(MobMIN) && MobMIN != "0")
                {
                    mobMinCondition = "OR LEN(MobNo) < " + MobMIN + ""; // Include the condition
                }

                // Check if MobMAX is null, empty, or "0"
                string mobMaxCondition = "";
                if (!string.IsNullOrEmpty(MobMAX) && MobMAX != "0")
                {
                    mobMaxCondition = "OR LEN(MobNo) > " + MobMAX + "";
                }

                string MobIncorrectCount = Convert.ToString(database.GetScalarValue(@"SELECT COUNT(MOBNO) MOBNO FROM tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                           WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + ""));

                //lblMobIncorrectCount.Text = Convert.ToString(Convert.ToInt32(lblMobTotalCount.Text) - Convert.ToInt32(lblMobileCnt.Text) - Convert.ToInt32(lblMobDuplicateCount.Text));
                lblMobIncorrectCount.Text = MobIncorrectCount;
                lblMobTotalCountNoti.Text = Convert.ToString(Session["UploadedCount"]);
                lblMobDuplicateCountNoti.Text = Convert.ToString(Session["DuplicateCount"]);
                lblMobIncorrectCountNoti.Text = Convert.ToString(MobIncorrectCount);
                lblMobileCntNotI.Text = res.Replace("RECORDCOUNT", "").Trim();
                notificationAlert.Style["display"] = "block";
            }
            else
            {
                lblMobileCnt.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(_user);
                lblMobTotalCount.Text = "";
                lblMobTotalCount.Text = "";
                lblMobDuplicateCount.Text = "";
                lblMobIncorrectCount.Text = "";

                lblMobTotalCountNoti.Text = "";
                lblMobDuplicateCountNoti.Text = "";
                lblMobIncorrectCountNoti.Text = "";
                lblMobileCntNotI.Text = "";
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
            string MobMIN = Convert.ToString(Session["MobMIN"]);
            string MobMAX = Convert.ToString(Session["MobMAX"]);
            string res = ob.SaveTempTable3(FilePath, SheetName, _user, Extension, folder, filenm, mobLen, MobMIN, MobMAX);  // change by vikas on 16-10-2023 add two new prameter MObMin and MObMax

            //txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                string[] arr = res.Split(',');
                lblExcludeCnt.Text = arr[1];
                lblMobileCnt.Text = arr[3];
                lblMobileCntNotI.Text = arr[3];
                //lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text; // lblMobileCnt.Text;
            }
            else
            {
                lblMobileCnt.Text = "";
                lblMobileCntNotI.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(_user);
            }
            return res;
        }

        protected void btnInsURL_Click(object sender, EventArgs e)
        {
            //mpeAddUpdateEmployee.Show();

            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8")
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
            Session["Domain"] = ws;
            if (pnlExisting.Visible)
            {
                if (ddlURL.SelectedIndex > 0)
                {
                    txtMsg.Text = txtMsg.Text.Trim() + " " + ddlURL.SelectedValue + " ";
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

            string UserID = Convert.ToString(Session["UserID"]);
            //abbhishek 11-01-2023 Start
            if (UserID.ToUpper() != DNDUAEUSERID)
            {
                if (chkOptOut.Checked)
                {
                    string lbText = " " + lblOptOut.Text.ToString();
                    qlen = qlen + lbText.Length;
                }
            }
            //else
            //{
            //    string lbText = " " ;
            //    qlen = qlen - lbText.Length;

            //}

            //abhishek End
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

            //if (chkOptOut.Checked)
            //{
            //    string lbText = " " +lblOptOut.Text.ToString();
            //    qlen = qlen + lbText.Length;
            //    lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();

            //}
            //else
            //{
            //    lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();
            //}

            lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();


            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (UserID.ToUpper() != DNDUAEUSERID)
                {
                    if (chkOptOut.Checked)
                    {
                        string lbText = " " + lblOptOut.Text.ToString();
                        qlen = qlen + lbText.Length;
                    }
                }

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
            Helper.Util ob = new Helper.Util();
            if (ddlURL.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Short URL');", true);
                pnlPopUp_URL_ModalPopupExtender.Show();
                return;
            }
            string ws = Convert.ToString(Session["DOMAINNAME"]);
            DataTable dt = ob.GetURLSofUser(Convert.ToString(Session["UserID"]), ddlURL.SelectedValue, ws);
            if (dt != null && dt.Rows.Count > 0)
            {
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
                if (!txtLongURL.Contains(Convert.ToString(Session["DOMAINNAME"])))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Long URL.');", true);
                    return "";
                }
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
                            ob.SaveShortURLRichMedia(UserID, txtLongURL, Request.UserHostAddress, sUrl, "Y", "N", Convert.ToString(Session["DOMAINNAME"]), name, "N");

                    }
                }
                else
                    // End
                    ob.SaveShortURL(UserID, txtLongURL, Request.UserHostAddress, sUrl, "Y", "N", Convert.ToString(Session["DOMAINNAME"]), name);

                lblShortURL = sUrl;
                return lblShortURL;
            }
        }

        protected void ddlSMSType_SelectedIndexChanged(object sender, EventArgs e)
        {

            Boolean CheckScratch = Convert.ToBoolean(database.GetScalarValue("select Active from scratchcardCustomer where username='" + Convert.ToString(Session["UserID"]) + "'"));
            if (CheckScratch == true && ddlSMSType.SelectedItem.Text == "Link Text")
            {
                divscratchcard.Visible = true;
            }
            else
            {
                divscratchcard.Visible = false;
            }

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
                if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6" || ddlSMSType.SelectedValue == "8") LinkButton1.Visible = true; else LinkButton1.Visible = false;
            }
        }

        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            StartProcess();
            lblMobileCnt.Text = "";
            txtMobNum.Text = "";
            divFileUpload.Visible = true;
            DivDealerName.Visible = false;
            divFileUpload.Attributes.Add("class", "form-group row d-none");
            divFileUpload3.Attributes.Add("class", "form-group row d-none");
            btnInsTemplate.Visible = false;
            if (rdbEntry.Checked)
            {
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(DLTNO).ToLower())
                {
                    DivDealerName.Visible = true;
                }
                divNum.Attributes.Add("style", "pointer-events:all;");
                divEnterMobNum.Attributes.Add("style", "form-group row");
            }
            else
            {
                divNum.Attributes.Add("style", "pointer-events:none;");
                divEnterMobNum.Attributes.Add("style", "form-group row d-none");
            }

            if (rdbUpload.Checked || rdbPersonal.Checked || rdbImport.Checked)
            {
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
                divFileUpload3.Attributes.Add("class", "form-group row d-block;");
                divCamp.Attributes.Add("class", "form-group row d-block;");
                lnkbtnimport.Enabled = false;
            }
            else
            {
                divCamp.Attributes.Add("class", "form-group row d-none");
            }
            if (rdbImport.Checked)
            {
                lnkbtnimport.Enabled = true;
                divFileUpload.Visible = false;
                populateGroup();
                pnlPopUp_GROUP_ModalPopupExtender.Show();
            }
            if (rdbPersonal.Checked)
            {
                btnInsTemplate.Visible = true;
            }
        }

        public void populateGroup()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetGroup(Convert.ToString(Session["UserID"]));

            chkbxgroup.DataSource = dt;
            chkbxgroup.DataTextField = "grpname";
            chkbxgroup.DataValueField = "id";
            chkbxgroup.DataBind();
        }

        public void TotalGroups()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.TotalGroups(Convert.ToString(Session["UserID"]));
            lbltotgroups.Text = Convert.ToString(dt.Rows[0]["TotalGroups"]);
            ViewState["TotalGroups"] = Convert.ToString(lbltotgroups.Text);
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            List<string> lstitems = new List<string>();
            for (int i = 0; i < chkbxgroup.Items.Count; i++)
            {
                if (chkbxgroup.Items[i].Selected)
                    lstitems.Add(chkbxgroup.Items[i].Value);
            }

            string res = ob.SaveTempTableGroup(_user, lstitems);
            DataTable dt = new DataTable();
            txtMobNum.Text = "";

            if (res.Contains("RECORDCOUNT"))
            {
                lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text;
                Session["UPLOADFILENM"] = chkbxgroup.SelectedValue;
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
            // 26 jul 2022
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + ddlCCode.SelectedValue + ")"));


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

            // if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) <= DateTime.Now)  // 26 jul 2022
            if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime + " cannot be below of current date time');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            string schMin = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);

            //DateTime t = DateTime.Now.AddMinutes(Convert.ToDouble(schMin)); // 26 jul 2022
            DateTime t = DateTime.Now.AddMinutes(Convert.ToDouble(schMin) + timedifferenceInMinute);
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

                // 26 jul 2022
                string datetime1 = Convert.ToDateTime(hdnScheduleDate1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime1.Text;
                //if (Convert.ToDateTime(datetime1, CultureInfo.InvariantCulture) <= DateTime.Now)
                if (Convert.ToDateTime(datetime1, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute)) // 26 jul 2022
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime1 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin1 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                // DateTime t1 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin1));
                DateTime t1 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin1) + timedifferenceInMinute); // 26 jul 2022
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

                // if (Convert.ToDateTime(datetime2, CultureInfo.InvariantCulture) <= DateTime.Now)
                if (Convert.ToDateTime(datetime2, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime2 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin2 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                //DateTime t2 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin2)); // 26 jul 2022
                DateTime t2 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin2) + timedifferenceInMinute);
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

                if (Convert.ToDateTime(datetime3, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))//26 jul
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime3 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin3 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t3 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin3) + timedifferenceInMinute);//26 jul
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

                if (Convert.ToDateTime(datetime4, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime4 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin4 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t4 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin4) + timedifferenceInMinute);
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

                if (Convert.ToDateTime(datetime5, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime5 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin5 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t5 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin5) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime6, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime6 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin6 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t6 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin6) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime7, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime7 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin7 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t7 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin7) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime8, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime8 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin8 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t8 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin8) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime9, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime9 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin9 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t9 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin9) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime10, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime10 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin10 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t10 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin10) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime11, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime11 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin11 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t11 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin11) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime12, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime12 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin12 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t12 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin12) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime13, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime13 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin13 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t13 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin13) + timedifferenceInMinute);

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

                if (Convert.ToDateTime(datetime14, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime14 + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return false;
                }
                string schMin14 = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t14 = DateTime.Now.AddMinutes(Convert.ToDouble(schMin14) + timedifferenceInMinute);

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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Valid Captcha!');", true);
                        return;
                    }
                }

                moblen = Convert.ToInt32(Convert.ToString(database.GetScalarValue("SELECT moblength FROM tblcountry where counryCode='" + ddlCCode.SelectedValue + "'")));
                liScheduleDates = new List<string>();
                ValidateScheduleTime();

                #region ForScheduleValidation
                Util ob = new Util();
                string _smstype = ddlSMSType.SelectedValue;
                DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
                string fCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsFromTime"]) : "";
                string tCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsToTime"]) : "";

                bool flagLevel1 = ValidateForCountryLevel1Schedule(dtCountry);
                if (flagLevel1 == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Messages should be scheduled during (" + fCtry + " - " + tCtry + ") as per country !');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    return;
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
                        pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                        return;
                    }
                }
                else
                {
                    // for Premimum & Link
                    DataTable dtAcc = ob.GetSMPPAccountIdTimeZone(_user);
                    string fAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsFromTime"]) : "";
                    string tAcc = dtAcc.Rows.Count > 0 ? Convert.ToString(dtAcc.Rows[0]["smsToTime"]) : "";

                    bool flagLevel3 = ValidateForAccountIdLevel3Schedule(dtAcc);
                    if (flagLevel3 == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message should be scheduled during (" + fAcc + " - " + tAcc + " As Per Account Setting !)');", true);
                        pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                        return;
                    }
                }

                if ((Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "966"))
                {
                    string _senderId = ddlSender.SelectedValue;
                    if (_senderId.ToUpper().StartsWith("AD-") || _senderId.ToUpper().EndsWith("-AD"))
                    {
                        bool IsProm = IsPromotionalSMSTimeOver("YES");
                        if (IsProm == false)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('promotional sms can not be sent at current time or can not be scheduled for entered time!');", true);
                            pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                            return;
                        }
                    }
                }
                #endregion

                SetAttribueScheduleRows(hdnScheduleCount.Value);

                if (hdnScheduleCount.Value != liScheduleDates.Count.ToString())
                {
                    return;
                }

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload CSV File');", true);
                return;
            }
            LstTemplateFld.Items.Clear();
            divTemplateList.Attributes.Add("Style", "display:block;");
            divTempId.Attributes.Add("class", "form-group row d-none");
            divTempsms.Attributes.Add("class", "form-group row d-none");
            PopulateTemplate();

            BindFileMapFld(); //Add By Vikas On 22-05-2024

            IsPersalisedSMS.Value = "1";

            divBindHeader.Attributes.Add("class", "form-group row");
            divBindBody.Attributes.Add("class", "form-group row");
            btnUnMap.Visible = true;
            ddlTemplate.Focus();
        }

        public void PopulateTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSApprovedTemplateOfUser(Convert.ToString(Session["UserID"]), Convert.ToString(Session["Hidetemplateid"]));

            ddlTemplate.DataSource = dt;
            ddlTemplate.DataTextField = "TemplateID";
            ddlTemplate.DataValueField = "onlyTemplateID";
            ddlTemplate.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTemplate.Items.Insert(0, objListItem);
            ddlTemplate.SelectedIndex = 0;
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstTemplateFld.Items.Clear();
            lstMappedFields.Items.Clear();
            Helper.Util ob = new Helper.Util();
            DataTable dtT = ob.GetTemplateSMS(Convert.ToString(Session["UserID"]), ddlTemplate.SelectedValue);
            if (dtT.Rows.Count > 0)
            {
                string smstxt = dtT.Rows[0]["template"].ToString();
                ViewState["TemplateID"] = dtT.Rows[0]["templateid"].ToString();
                ViewState["TemplateFields"] = null;
                txtMsg.Text = smstxt;
                string str = smstxt;
                BindFileMapFld(); // Add By Vikas On 22-05-2024
                string s = smstxt;

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
                    }
                }
                List<string> sa = new List<string>();
                foreach (ListItem li in LstTemplateFld.Items) sa.Add(li.Text);
                ViewState["TemplateFields"] = sa;
                SetPreview();
                divBindHeader.Attributes.Add("class", "form-group row");
                divBindBody.Attributes.Add("class", "form-group row");
                btnUnMap.Visible = true;
                chkAutoMapping.Enabled = true;
                chkAutoMapping.Checked = false;
            }
            else
            {
                divBindHeader.Attributes.Add("class", "form-group row");
                divBindBody.Attributes.Add("class", "form-group row");
                btnUnMap.Visible = true;
                chkAutoMapping.Enabled = false;
                chkAutoMapping.Checked = false;
            }
        }

        protected void btnMap_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(LstTemplateFld.SelectedItem) == "" || Convert.ToString(LstXLSFld.SelectedItem) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template Field and CSV Field for Mapping');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template Field and CSV Field for Mapping');", true);
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
            txtPreview.Text = MessageVaribleColorChange(Convert.ToString(txtMsg.Text));
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
                {
                    msg = msg.Replace(s1[0], "<span style='color: blue;'>" + Convert.ToDateTime(dtTopRec.Rows[0][s1[1]]).ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + "</span>");
                }
                else
                {
                    msg = msg.Replace(s1[0], "<span style='color: blue;'>" + Convert.ToString(dtTopRec.Rows[0][s1[1]]) + "</span>");
                }
            }

            txtPreview.Text = MessageVaribleColorChange(msg);
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
            DataTable dt = ob.GetTemplateId(Convert.ToString(Session["UserID"]), Convert.ToString(Session["Hidetemplateid"]));

            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "TemplateID";
            ddlTempID.DataValueField = "Template";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
            ddlTempID.SelectedIndex = 0;
        }

        protected void ddlTempID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            if (ddlTempID.SelectedValue == "0")
            {
                lblTempSMS.Text = "";
            }
            else
            {
                DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["UserID"]), ddlTempID.SelectedValue);
                if (dtT.Rows.Count > 0)
                {
                    lblTempSMS.Text = MessageVaribleColorChange(Convert.ToString(dtT.Rows[0]["template"]));
                }
                else
                {
                    lblTempSMS.Text = "";
                }
            }
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
                    if (Convert.ToString(Session["userid"]).ToUpper() == "MIM2300275" || Convert.ToString(Session["userid"]).ToUpper() == "MIM2201203" || Convert.ToString(Session["userid"]).ToUpper() == "MIM2201216" || Convert.ToString(Session["userid"]).ToUpper() == "MIM2300184")
                    {
                        chkOptOut.Enabled = true;
                        chkOptOut.Checked = false;
                    }
                    else
                    {
                        chkOptOut.Checked = true;
                        chkOptOut.Enabled = false;
                    }
                    if (Convert.ToString(Session["userid"]).ToUpper() == DNDUAEUSERID)
                    {
                        divUAEDNDUser.Style.Add("display", "block");
                        divOptOut.Style.Add("display", "none");
                    }
                    else
                    {
                        divOptOut.Style.Add("display", "block");
                        divUAEDNDUser.Style.Add("display", "none");
                    }
                }
                else
                {
                    chkOptOut.Checked = false;
                    chkOptOut.Enabled = true;
                    if (Convert.ToString(Session["userid"]).ToUpper() == DNDUAEUSERID)
                    {
                        chkOptOut.Checked = true;
                        chkOptOut.Enabled = false;
                        divOptOut.Style.Add("display", "none");
                        divUAEDNDUser.Style.Add("display", "block");
                    }
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
                PopulateSMSType();
                divTempId.Attributes.Add("class", "form-group row d-block");
                divTempsms.Attributes.Add("class", "form-group row d-block");
                divOptOut.Attributes.Add("Style", "display:none;");
            }

        }

        protected void chkbxgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectcnt = 0;
            foreach (ListItem li in chkbxgroup.Items)
            {
                if (li.Selected)
                {
                    selectcnt = selectcnt + 1;
                    pnlPopUp_GROUP_ModalPopupExtender.Show();
                }
            }
        }

        protected void lnkbtnimport_Click(object sender, EventArgs e)
        {
            pnlPopUp_GROUP_ModalPopupExtender.Show();
        }

        protected void btnCleargroup_Click(object sender, EventArgs e)
        {
            chkbxgroup.SelectedIndex = -1;
            lblMobileCnt.Text = "";
            pnlPopUp_GROUP_ModalPopupExtender.Show();
        }

        protected void chkOptOut_CheckedChanged(object sender, EventArgs e)
        {
            ShowMsgCharCnt();
        }

        #region For Auto Mapping Add By Vikas On 22-05-2024
        protected void AutoMapping()
        {
            lstMappedFields.Items.Clear();
            int templateCount = LstTemplateFld.Items.Count;
            int xlsCount = LstXLSFld.Items.Count;

            // Determine the minimum count of items between LstTemplateFld and LstXLSFld
            int minCount = Math.Min(templateCount, xlsCount - 1);

            for (int i = 0; i < minCount; i++)
            {
                string templateField = Convert.ToString(LstTemplateFld.Items[i]);

                // Ensure that the index i + 1 is within the bounds of LstXLSFld
                if (i + 1 < xlsCount)
                {
                    string xlsField = Convert.ToString(LstXLSFld.Items[i + 1]);

                    if (!string.IsNullOrEmpty(xlsField))
                    {
                        lstMappedFields.Items.Add(templateField + " ---->> " + xlsField);
                    }
                }
            }
            SetPreview();
        }

        protected void BindFileMapFld()
        {
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
        }

        protected void chkAutoMapping_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoMapping.Checked)
            {
                int templateCount = LstTemplateFld.Items.Count;
                int xlsCount = LstXLSFld.Items.Count;
                if ((templateCount + 1) > xlsCount)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Number of CSV File Columns does not match with the number of Template Variables.');", true);
                    chkAutoMapping.Checked = false;
                    return;
                }
                divBindHeader.Attributes.Add("class", "form-group row d-none");
                divBindBody.Attributes.Add("class", "form-group row d-none");
                btnUnMap.Visible = false;
                AutoMapping();
            }
            else
            {
                lstMappedFields.Items.Clear();
                divBindHeader.Attributes.Add("class", "form-group row");
                divBindBody.Attributes.Add("class", "form-group row");
                btnUnMap.Visible = true;
                SetPreview();
            }
        }
        #endregion

        private void ResetAllClear()
        {
            divBindHeader.Attributes.Add("class", "form-group row d-none");
            divBindBody.Attributes.Add("class", "form-group row d-none");
            divTemplateList.Attributes.Add("Style", "display:none;");
            divTempId.Attributes.Add("class", "form-group row");
            divTempsms.Attributes.Add("class", "form-group row");

            LstTemplateFld.Items.Clear();
            LstXLSFld.Items.Clear();
            lstMappedFields.Items.Clear();
            IsPersalisedSMS.Value = "0";

            chkAutoMapping.Checked = false;
            chkAutoMapping.Enabled = false;

            Session.Remove("UPLOADFILENM");
            Session.Remove("UPLOADFILENMEXT");
            Session.Remove("FileNameOnly");
            Session.Remove("XLUPLOADED");
            Session.Remove("UPLOADFILENMXCLUDE");
            Session.Remove("UPLOADFILENMEXTEXCLUDE");
            Session.Remove("SHORTURL");
            Session.Remove("DTXL");
            Session.Remove("MOBILECOUNT");

            txtMsg.Text = "";
        }

        #region For Message Varible Color Change Add By Vikas On 29-05-2024
        private string MessageVaribleColorChange(string TemplateText)
        {
            string blueText;

            string[] variables = { "#var1", "#var2", "#var3", "#var4", "#var5", "#var6", "#var7", "#var8", "#var9", "#var10" };

            foreach (string variable in variables)
            {
                int index = TemplateText.IndexOf(variable);

                while (index != -1)
                {
                    blueText = "<span style='color: blue;'>" + variable + "</span>";
                    TemplateText = TemplateText.Remove(index, variable.Length).Insert(index, blueText);
                    index = TemplateText.IndexOf(variable, index + blueText.Length);
                }
            }

            return TemplateText;
        }
        #endregion

        protected void lblMobDuplicateCountNoti_Click(object sender, EventArgs e)
        {
            DataTable dt = database.GetDataTable(@"SELECT MOBNO FROM tblIncorrect_" + Convert.ToString(Session["UserID"]) + " GROUP BY MOBNO HAVING COUNT(*) > 1;");
            if (dt.Rows.Count > 0)
            {
                string CSVData = ConvertDataTableToCsv(dt);
                DownloandCSV(CSVData, "Duplicate");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No Data Found.');", true);
                return;
            }
        }

        protected void lblMobIncorrectCountNoti_Click(object sender, EventArgs e)
        {
            string MobMIN = Convert.ToString(Session["MobMIN"]);
            string MobMAX = Convert.ToString(Session["MobMAX"]);

            // Check if MobMIN is null, empty, or "0"
            string mobMinCondition = "";
            if (!string.IsNullOrEmpty(MobMIN) && MobMIN != "0")
            {
                mobMinCondition = "OR LEN(MobNo) < " + MobMIN + ""; // Include the condition
            }

            // Check if MobMAX is null, empty, or "0"
            string mobMaxCondition = "";
            if (!string.IsNullOrEmpty(MobMAX) && MobMAX != "0")
            {
                mobMaxCondition = "OR LEN(MobNo) > " + MobMAX + "";
            }

            DataTable dt = database.GetDataTable(@"SELECT MOBNO FROM tblIncorrect_" + Convert.ToString(Session["UserID"]) + @" AS Incorrect 
                                           WHERE MobNo LIKE '%[^0-9]%' " + mobMinCondition + " " + mobMaxCondition + "");
            if (dt.Rows.Count > 0)
            {
                string CSVData = ConvertDataTableToCsv(dt);
                DownloandCSV(CSVData, "Incorrect");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No Data Found.');", true);
                return;
            }
        }

        private string ConvertDataTableToCsv(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            // Write the column headers
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append(column.ColumnName);
            }
            sb.AppendLine();

            // Write the data rows
            foreach (DataRow row in dt.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    sb.Append(item.ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void DownloandCSV(string CSVData, string FileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "text/csv";
            Response.Output.Write(CSVData);
            Response.Flush();
            Response.End();
        }

        protected void btnNotificationAlert_Click(object sender, EventArgs e)
        {
            notificationAlert.Style["display"] = "none";
        }
    }
}