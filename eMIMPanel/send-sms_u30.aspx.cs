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

namespace eMIMPanel
{
    public partial class send_sms_u30 : System.Web.UI.Page
    {
        string _user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            //FilteredTextBoxExtender1.ValidChars = FilteredTextBoxExtender1.ValidChars + "\r\n";

            _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");

            //if (_user == "MIM2000002") Response.Redirect("send-sms_u29.aspx");
            //if (_user == "MIM2000029") Response.Redirect("send-sms_u29.aspx");
            //if (_user == "MIM2002097") Response.Redirect("send-sms_u29.aspx");

            if (Convert.ToString(Session["CANSENDSMS"]).ToUpper() == "FALSE") Response.Redirect("index_u.aspx");
            if (!IsPostBack)
            {
                ViewState["TemplateID"] = null;
                ViewState["TemplateFields"] = null;
                ViewState["dtMaxLen"] = null;
                ViewState["dtTopRec"] = null;
                Session["NOOFCHARINMSG"] = null;
                rdbEntry.Checked = true;
                lblRate.Text = Convert.ToString(Session["RATE_NORMALSMS"]) + " Paisa per SMS";
                PopulateSMSType();
                PopulateSender();
                PopulateTemplateID();
                StartProcess();
            }

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
                            if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 6 MB');", true);
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
                        string FileNameOnly = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        FileUpload1.SaveAs(FilePath);
                        string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);
                        if (res.Contains("RECORDCOUNT"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                            lblUploading.Text = "Uploaded successfully.";
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
            if (IsPostBack && FileUpload3.PostedFile != null)
            {

                string FileName = Path.GetFileName(FileUpload3.PostedFile.FileName);
                if (FileName != "")
                {
                    if (Convert.ToString(Session["XLUPLOADED"]) != "Y")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('First Upload Mobile Number XLS File.');", true);
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
                            if (FileUpload3.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 6 MB');", true);
                                lblUploading3.Text = "Upload rejected.";
                                return;
                            }
                        if (en.Contains("XLS"))
                            if (FileUpload3.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of 6 MB');", true);
                                lblUploading3.Text = "Upload rejected.";
                                return;
                            }

                        string FolderPath = "XLSUploadExlcude/";
                        Session["UPLOADFILENMXCLUDE"] = FileName;
                        Session["UPLOADFILENMEXTEXCLUDE"] = Extension;
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        FileUpload3.SaveAs(FilePath);
                        string res = Import_To_Grid3(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);
                        if (res.Contains("RECORDCOUNT"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                            lblUploading3.Text = "Uploaded successfully.";
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
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["UserID"]));

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
            bool res;
            if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                res = SendTemplateMSG("");
            else
                res = MsgSend("");
        }
        public bool MsgSend(string sch)
        {
            Helper.Util ob = new Helper.Util();
            string filenm = "";
            string filenmext = "";
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]);
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                txtMobNum.Text = "";

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
            if (ddlSMSType.SelectedValue != "3")
            {
                if (ddlTempID.SelectedValue == "0" && Helper.database.TemplateIDMandatory() == "Y")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Template id');", true);
                    return false;
                }
            }

            DataTable dt2 = ob.GetUserParameter(_user);
            string bal2 = dt2.Rows[0]["balance"].ToString();

            string country_code = "91";
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
            if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must be present in Message Text for SMS Type - Smart SMS .');", true);
                return false;
            }
            if (Session["SHORTURL"] != null)
                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                {
                    if ((ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
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
            if (ddlSMSType.SelectedValue == "2")
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
            string q = txtMsg.Text.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtMsg.Text.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~');
            qlen = qlen + count_tild;

            if (ddlSMSType.SelectedValue == "2") qlen += 2;
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

            if (q.Any(c => c > 255))
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

            Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            //txtMsg.Text = txtMsg.Text.Replace("@", "|");
            //Get SMS Accounts with Load and accordingly bifurcate no. of sms
            //DataTable dt = ob.GetSMPP_Account_Load();
            //dt.Columns.Add("nos", typeof(Int32));
            DataTable dtSMPPAC = new DataTable();

            if (ddlSMSType.SelectedValue == "6")
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
            else
                dtSMPPAC = (DataTable)Session["DTSMPPAC"];

            string templID = "";
            if (ddlTempID.SelectedValue != "0") templID = ddlTempID.SelectedValue;

            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    ob.Schedule_SMS_BULK(UserID, txtMsg.Text, ddlSender.Text, sch, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, mobList, "", templID,country_code);
                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        ob.Schedule_SMS_BULK(UserID, txtMsg.Text, ddlSender.Text, sch, shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, "", ucs2, mobList, "MANUAL", templID, country_code);
                    }
                    else
                    {
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
                            //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                            string resp = "";
                            //ob.SendURL_SMS(UserID, country_code + m, msg, ddlSender.Text);
                            ob.Schedule_SMS(UserID, country_code + m, msg, ddlSender.Text, sch, shortURLId, shortURL, ws, mseg, rate, ddlSMSType.SelectedValue, dtSMPPAC, ucs2, templID, country_code);
                            //if (ddlSMSType.SelectedValue == "2")
                            //    ob.SaveURL_MOBILE(UserID, shortURLId, country_code + m, mseg, resp);
                        }
                    }
                }
            }
            else
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    if (ddlSMSType.SelectedValue == "2")
                    {
                        ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, mobList, "", "", templID, country_code);
                    }
                    else
                    {
                        if (Session["SHORTURL"] != null)
                        {
                            if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                            {
                                if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, mobList, "", "", templID, country_code);
                            }
                            else
                                ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code);
                        }
                        else
                            ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code);
                    }
                }
                else
                {
                    bool bulk = mobList.Count > 25 ? true : false;
                    if (bulk)
                    {
                        if (ddlSMSType.SelectedValue == "2")
                        {
                            ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, mobList, "MANUAL", "", templID, country_code);
                        }
                        else
                        {
                            if (Session["SHORTURL"] != null)
                            {
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, mobList, "MANUAL", "", templID, country_code);
                                }
                                else
                                    ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code);
                            }
                            else
                                ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code);
                        }
                    }
                    else
                    {
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
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")
                                    {
                                        mseg = ob.NewSegment8();
                                        shurl = ws + mseg;
                                        msg = msg.Replace(shortURL, shurl);
                                    }
                                }
                            //SaveURL_MOBILE(string UserID, int urlid, string mobile, string mseg, string resp)
                            string resp = "";
                            ob.SendURL_SMS(UserID, m, msg, ddlSender.Text, dtSMPPAC, ucs2, bulk, rate, noofsms, templID, country_code,ddlSMSType.SelectedValue);
                            if (ddlSMSType.SelectedValue == "2") ob.SaveURL_MOBILE(UserID, shortURLId, m, mseg, resp, country_code,"","");
                            if (Session["SHORTURL"] != null)
                                if (Convert.ToString(Session["SHORTURL"].ToString()) != "")
                                {
                                    if (ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6") ob.SaveURL_MOBILE(UserID, shortURLId,  m, mseg, resp, country_code, "", "");
                                }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send-sms_u30.aspx';", true);
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
            string filenm = "";
            string filenmext = "";
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]);
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                txtMobNum.Text = "";

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
            Helper.Util ob = new Helper.Util();

            DataTable dt2 = ob.GetUserParameter(_user);
            string bal2 = dt2.Rows[0]["balance"].ToString();

            string country_code = "91";
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
            if (ddlSMSType.SelectedValue == "2" && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
            {
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
            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "6")
            {
                shortURL = Session["SHORTURL"].ToString();
                //ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"); 
                ws = Convert.ToString(Session["DOMAINNAME"]);
                //Convert.ToString(ConfigurationManager.AppSettings["WebSite"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }

            Int32 cnt = 0;
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                cnt = Convert.ToInt32(Session["MOBILECOUNT"]);
            else
                cnt = mobList.Count;

            int noofsms = 0;
            bool ucs2 = false;
            string q = txtMsg.Text.Trim();
            int count_AtTheRate = q.Count(f => f == '@');
            int qlen = Convert.ToInt16(Session["NOOFCHARINMSG"]);

            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "6") qlen += 2;

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

            if (q.Any(c => c > 255))
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

            Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);
            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            //txtMsg.Text = txtMsg.Text.Replace("@", "|");
            //Get SMS Accounts with Load and accordingly bifurcate no. of sms
            //DataTable dt = ob.GetSMPP_Account_Load();
            //dt.Columns.Add("nos", typeof(Int32));
            //DataTable dtSMPPAC = (DataTable)Session["DTSMPPAC"];
            DataTable dtSMPPAC = new DataTable();

            if (ddlSMSType.SelectedValue == "6")
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

           else  if (ddlSMSType.SelectedValue == "3")
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
            else
                dtSMPPAC = (DataTable)Session["DTSMPPAC"];

            DataTable dtCols = (DataTable)ViewState["dtMaxLen"];
            List<string> tempFields = (List<string>)ViewState["TemplateFields"];
            string templateID = Convert.ToString(ViewState["TemplateID"]);
            if (sch != "")
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    ob.InsertTemplateSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, dtCols, lstMappedFields, txtCampNm.Text, ucs2, rate, noofsms, sch, tempFields, templateID, country_code , shortURL, ws, shortURLId);
                }
            }
            else
            {
                if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                {
                    //if (ddlSMSType.SelectedValue == "2")
                    //ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, ddlSMSType.SelectedValue, filenm, filenmext, dtSMPPAC,"",ucs2,noofsms, mobList, "");
                    //else
                    ob.InsertTemplateSMSrecordsFromUSERTMP(UserID, ddlSender.Text, ddlSMSType.SelectedValue, txtMsg.Text, filenm, filenmext, dtSMPPAC, dtCols, lstMappedFields, txtCampNm.Text, ucs2, rate, noofsms, "", tempFields, templateID, country_code, shortURL, ws, shortURLId);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='send-sms_u30.aspx';", true);
            }
            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("send-sms_u30.aspx");
        }
        protected void btnSchedule_Click(object sender, EventArgs e)
        {
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
            string minlen = Convert.ToString(Session["MobMIN"]);
            string maxlen = Convert.ToString(Session["MobMAX"]);
            string res = ob.SaveTempTable(FilePath, SheetName, _user, Extension, folder, filenm, ddlSender.SelectedValue, mobLen, minlen, maxlen);

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
            string res = ob.SaveTempTable3(FilePath, SheetName, _user, Extension, folder, filenm);

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

            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3")
            {
                lblLongUrl.Text = "";
                Helper.Util ob = new Helper.Util();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                //DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), "", string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"));
                DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), "", ws);

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

            if (q.Any(c => c > 255))
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
            if (ddlSMSType.SelectedValue == "0") lblRate.Text = "";
            if (ddlSMSType.SelectedValue == "1") lblRate.Text = Convert.ToString(Session["RATE_NORMALSMS"]) + " Paisa per SMS";
            if (ddlSMSType.SelectedValue == "2") lblRate.Text = Convert.ToString(Session["RATE_SMARTSMS"]) + " Paisa per SMS";
            //if (ddlSMSType.SelectedValue == "3") lblRate.Text = Convert.ToString(Session["RATE_OTP"]) + " Paisa per SMS";
            if (ddlSMSType.SelectedValue == "3") lblRate.Text = Convert.ToString(Session["RATE_CAMPAIGN"]) + " Paisa per SMS";

            if (ddlSMSType.SelectedValue == "2" || ddlSMSType.SelectedValue == "3" || ddlSMSType.SelectedValue == "6")  LinkButton1.Visible = true; else LinkButton1.Visible = false;

        }

        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            StartProcess();
            lblMobileCnt.Text = "";
            txtMobNum.Text = "";
            //txtMobNum.Enabled = false;
            divNum.Attributes.Add("style", "pointer-events:none;");

            divFileUpload.Attributes.Add("class", "form-group row d-none");
            divFileUpload3.Attributes.Add("class", "form-group row d-none");
            if (rdbEntry.Checked)
                divNum.Attributes.Add("style", "pointer-events:all;");

            if (rdbUpload.Checked || rdbPersonal.Checked)
            {
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
                divFileUpload3.Attributes.Add("class", "form-group row d-block;");
                divCamp.Attributes.Add("class", "form-group row d-block;");
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
        protected void btnScheduleSMS_Click(object sender, EventArgs e)
        {
            try
            {
                string s1 = Convert.ToDateTime(hdnScheduleDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                //string s1 = Convert.ToDateTime(hdnScheduleDate.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                //if (txtScheduleDate.Text.Trim() == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date cannot be left blank');", true);
                //    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                //    return;
                //}
                if (txtTime.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time cannot be left blank');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }
                if (txtTime.Text.Trim().Length != 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Schedule time in correct format');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }

                string HH = txtTime.Text.Split(':')[0];
                string MM = txtTime.Text.Split(':')[1];

                if (Convert.ToInt16(HH) > 23)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter HH less than 24');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }

                if (Convert.ToInt16(MM) > 59)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter MM less than 60');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }

                string datetime = Convert.ToDateTime(hdnScheduleDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime.Text;
                //string datetime = Convert.ToDateTime(hdnScheduleDate.Value.Trim()).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " " + txtTime.Text;

                if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime + " cannot be below of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }
                string schMin = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
                DateTime t = DateTime.Now.AddMinutes(Convert.ToDouble(schMin));
                if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) < t)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule time must be above of " + schMin + " minutes of current date time');", true);
                    pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                    divSchedule.Style.Add("display", "none");
                    return;
                }

                bool res;
                if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
                    res = SendTemplateMSG(datetime);
                else
                    res = MsgSend(datetime);

                if (res == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Scheduled Successfully');window.location ='send-sms_u30.aspx';", true);
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
            if (Convert.ToString(Session["XLUPLOADED"]) != "Y")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload Xls File');", true);
                return;
            }
            //bind dropdown
            divTemplateList.Attributes.Add("Style", "display:block;");
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

        }
        public void PopulateTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSApprovedTemplateOfUser(Convert.ToString(Session["UserID"]));

            ddlTemplate.DataSource = dt;
            ddlTemplate.DataTextField = "tempname";
            ddlTemplate.DataValueField = "tempname";
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
            divMsg.Attributes.Add("style", "pointer-events:none;");
            string str = smstxt; // ddlTemplate.SelectedValue; //.Replace('\n', ' ');
            LstTemplateFld.Items.Clear();
            string s = smstxt; // ddlTemplate.SelectedValue; //.Replace('\n', ' ');
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
                    LstTemplateFld.Items.Add("#" + s2[0].Replace(",", "").Replace(".", "").Replace("\n", ""));

                }
                //if (s1[i].Substring(0, 1) == "#")
                //{
                //   LstTemplateFld.Items.Add(s1[i].Replace(",", "").Replace(".", ""));
                //}
            }
            SetPreview();
            List<string> sa = new List<string>();
            foreach (ListItem li in LstTemplateFld.Items) sa.Add(li.Text);
            ViewState["TemplateFields"] = sa;
        }

        protected void btnMap_Click(object sender, EventArgs e)
        {
            if (LstTemplateFld.SelectedItem.ToString() == "" || LstXLSFld.SelectedItem.ToString() == "")
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
                    msg = msg.Replace(s1[0], dtTopRec.Rows[0][s1[1]].ToString());
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
    }
}