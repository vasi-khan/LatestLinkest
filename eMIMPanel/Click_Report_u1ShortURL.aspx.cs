using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class Click_Report_u1ShortURL : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string s3 = "";
        string usertype = "";
        string user = "";
        string f = "";
        string t = "";
        DateTime F2;
        DateTime T2;
        DateTime S1;
        DateTime S2;
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");
            if (!IsPostBack)
            {
                BindUrlDDl();
                SetDropDownListItemColor();
                BindTemplate();
            }
        }

        public void BindTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTempIdAndName(Convert.ToString(Session["UserID"]));
            if (dt.Rows.Count == 0)
            {
                ddlTempIdName.Items.Clear();
                return;
            }
            ddlTempIdName.DataSource = dt;
            ddlTempIdName.DataTextField = "tempname";
            ddlTempIdName.DataValueField = "templateid";
            ddlTempIdName.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempIdName.Items.Insert(0, objListItem);
        }

        public void BindUrlDDl()
        {
            string sql = @"SELECT left(domainname+segment+replicate('_',50),31) + left('{' +  ISNULL(urlname,'') + replicate('_',50),21) + '}   [ ' + CONVERT(varchar,added,102) + ' ]' as shorturl,id from short_urls with(nolock) where userid='" + user + "' order by added desc";
            DataTable dt = ob.GetRecordDataTableSql(sql);
            if (dt != null)
            {
                ddlShortUrl.ClearSelection();
                ddlShortUrl.DataSource = dt;
                ddlShortUrl.DataValueField = "id";
                ddlShortUrl.DataTextField = "shorturl";
                ddlShortUrl.DataBind();
                ddlShortUrl.Items.Insert(0, new ListItem("--ALL--", "-1"));
            }
        }

        public void SetDropDownListItemColor()
        {
            foreach (ListItem item in ddlShortUrl.Items)
            {
                item.Attributes.CssStyle.Add("font-family", "Consolas");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Util obj = new Util();
            s1 = hdntxtFrm2.Value;
            s2 = hdntxtTo2.Value;
            if (rbOther.Checked) {
                GetData();
            }
            else
            {
                if (ddlTempIdName.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template ID.');", true);
                    return;
                }
                if (txtdatefromCLickDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Click Date From.');", true);
                    return;
                }
                if (txtdatetoToCLickDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Click Date To.');", true);
                    return;
                }
                s1 = Convert.ToDateTime(hdntxtFrm2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                string TempId = ddlTempIdName.SelectedValue;
                string user = Convert.ToString(Session["UserID"]);
                DataTable dt = obj.GetDataTempWise(user, TempId, s1, s2);
                grv.Columns[11].Visible = true;
                grv.DataSource = null;
                System.Data.DataColumn newColumn = new System.Data.DataColumn("From");
                newColumn.DefaultValue = s1;
                dt.Columns.Add(newColumn);
                System.Data.DataColumn newColumn1 = new System.Data.DataColumn("To");
                newColumn1.DefaultValue = s2;
                dt.Columns.Add(newColumn1);
                ViewState["From"] = s1;
                ViewState["To"] = s2;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
            txtdatefromCLickDate.Text = hdntxtFrm2.Value;
            txtdatetoToCLickDate.Text = hdntxtTo2.Value;
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            f = hdntxtFrm.Value;
            t = hdntxtTo.Value;
            DataTable dt = new DataTable();
            string sql = "";

            if (f != "" && t != "")
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                sql = @"SELECT left(domainname+segment+replicate('_',50),31) + left('{' +  ISNULL(urlname,'') + replicate('_',50),21) + '}   [ ' + CONVERT(varchar,added,102) + ' ]' as shorturl,id from short_urls  where userid='" + user + "' and CONVERT(date,added) between CONVERT(date,'" + s1 + "') and CONVERT(date,'" + s2 + "') order by added desc";
                dt = ob.GetRecordDataTableSql(sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlShortUrl.ClearSelection();
                        ddlShortUrl.DataSource = dt;
                        ddlShortUrl.DataValueField = "id";
                        ddlShortUrl.DataTextField = "shorturl";
                        ddlShortUrl.DataBind();
                        ddlShortUrl.Items.Insert(0, new ListItem("--ALL--", "-1"));
                    }
                    else
                    {
                        ddlShortUrl.DataSource = dt;
                        ddlShortUrl.DataValueField = "id";
                        ddlShortUrl.DataTextField = "shorturl";
                        ddlShortUrl.DataBind();
                    }
                    SetDropDownListItemColor();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select Data');", true);
                return;
            }
        }

        public void GetData()
        {
            DataTable dt = null;
            s1 = hdntxtFrm2.Value;
            s2 = hdntxtTo2.Value;

            Helper.Util obj = new Helper.Util();

            if (s1 != "" && s2 != "")
            {
                s1 = Convert.ToDateTime(hdntxtFrm2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            if (Chkdatewise.Checked)
            {
                dt = ob.GetUserReportShortURlWise(user, ddlShortUrl.SelectedValue, s1, s2, "Checked");
                grv.Columns[11].Visible = true;
            }
            else
            {
                dt = ob.GetUserReportShortURlWise(user, ddlShortUrl.SelectedValue, s1, s2, "Unchecked");
                dt.Columns.Add("clkdate");
                grv.Columns[11].Visible = false;
            }
            grv.DataSource = null;
            System.Data.DataColumn newColumn = new System.Data.DataColumn("From");
            newColumn.DefaultValue = s1;
            dt.Columns.Add(newColumn);
            System.Data.DataColumn newColumn1 = new System.Data.DataColumn("To");
            newColumn1.DefaultValue = s2;
            dt.Columns.Add(newColumn1);

            ViewState["From"] = s1;
            ViewState["To"] = s2;

            grv.DataSource = dt;
            grv.DataBind();

            GridFormat(dt);
        }

        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv.TopPagerRow != null)
            {
                grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv.BottomPagerRow != null)
            {
                grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void btnDel_Click(object sender, EventArgs e)
        { }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label lblurlid = (Label)gvro.FindControl("lblurlid");
                Label lblcountry = (Label)gvro.FindControl("lblCountryCode");
                Label lblDATE = (Label)gvro.FindControl("lblClickDate");
                string user = Convert.ToString(Session["UserID"]);

                if (hdntxtFrm.Value.Trim() == "")
                {
                    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                }
                else
                {
                    s1 = Convert.ToString(lblDATE.Text);
                    s2 = Convert.ToString(ViewState["To"]);
                }
                DataTable dt = ob.GetUserReportDetail(user, lblurlid.Text, true, s1, s2, lblcountry.Text);
                Session["rptDetail"] = dt;
                if (dt.Rows.Count > 0)
                {
                    grvDtl.DataSource = null;
                    grvDtl.DataSource = dt;
                    grvDtl.DataBind();

                    grvDtl.UseAccessibleHeader = true;
                    grvDtl.HeaderRow.TableSection = TableRowSection.TableHeader;

                    if (grvDtl.TopPagerRow != null)
                    {
                        grvDtl.TopPagerRow.TableSection = TableRowSection.TableHeader;
                    }
                    if (grvDtl.BottomPagerRow != null)
                    {
                        grvDtl.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                    }
                    if (dt.Rows.Count > 0)
                        grvDtl.FooterRow.TableSection = TableRowSection.TableFooter;

                    pnlPopUp_Detail_ModalPopupExtender.Show();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnXL_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            DataTable dt = null;
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblurlid = (Label)gvro.FindControl("lblurlid");
            Label lblcountry = (Label)gvro.FindControl("lblCountryCode");
            Label lblDATE = (Label)gvro.FindControl("lblClickDate");
            string user = Convert.ToString(Session["UserID"]);
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToString(lblDATE.Text);
                s2 = Convert.ToDateTime(((HiddenField)gvro.FindControl("hdnTo")).Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
                s3 = Convert.ToDateTime(hdntxtFrm2.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            if (Chkdatewise.Checked)
            {
                dt = ob.GetUserReportDetail(user, lblurlid.Text, true, s1, s2, lblcountry.Text, "Checked");
            }
            else
            {
                dt = ob.GetUserReportDetail(user, lblurlid.Text, true, s3, s2, lblcountry.Text, "Unchecked");
            }
            dt.Columns.Remove("Browser");
            dt.Columns.Remove("Platform");
            dt.Columns.Remove("IsMobileDevice");
            dt.Columns.Remove("MobileDeviceManufacturer");
            dt.Columns.Remove("MobileDeviceModel");
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "ClickReportDetail.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlPopUp_Detail_ModalPopupExtender.Hide();
            s1 = hdntxtFrm.Value;

            //s2 = h2.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
        }

        protected void btnReTarget_Click(object sender, EventArgs e)
        {
            // Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblurlid = (Label)gvro.FindControl("lblurlid");

            Label lblcountry = (Label)gvro.FindControl("lblCountryCode");
            Label lblDATE = (Label)gvro.FindControl("lblClickDate");

            Label lblNoOfHits = (Label)gvro.FindControl("lblNoOfHits");

            string s1 = Convert.ToString(lblDATE.Text);
            string s2 = Convert.ToString(ViewState["To"]);

            ViewState["CCode"] = lblcountry.Text.Trim();

            if (lblcountry.Text.Trim() != "91")
            {
                divTempsms.Attributes.Add("class", "form-group row d-none");
            }
            else
            {
                divTempsms.Attributes.Add("class", "form-group row d-block");
            }
            if (lblNoOfHits.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to ReTarget.');", true);
                return;
            }

            ViewState["URLID"] = lblurlid.Text;
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetClickMobileNumbers(user, lblurlid.Text, s1, s2, lblcountry.Text);
            Session["dtRETARGET"] = dt;
            PopulateSender();
            PopulateTemplateID();
            PopulateShortURL();
            txtMsg.Text = "";
            pnlPopUp_Detail1_ModalPopupExtender.Show();
        }

        public void PopulateShortURL()
        {
            Helper.Util ob = new Helper.Util();
            string ws = Convert.ToString(Session["DOMAINNAME"]);
            DataTable dt = ob.GetURLSofUser_4SMSSEND(Convert.ToString(Session["userid"]), "", ws);

            ddlURL.DataSource = dt;
            ddlURL.DataTextField = "shorturlDISP";
            ddlURL.DataValueField = "shorturl";
            ddlURL.DataBind();
            ListItem objlistitem = new ListItem("--Select Short URL--", "0");
            ddlURL.Items.Insert(0, objlistitem);
            ddlURL.SelectedIndex = 0;
        }

        public void PopulateSender()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["UserID"]));

            ddlSender.DataSource = dt;
            ddlSender.DataTextField = "senderid";
            ddlSender.DataValueField = "senderid";
            ddlSender.DataBind();
            ListItem objListItem = new ListItem("--Select SenderID--", "0");
            ddlSender.Items.Insert(0, objListItem);
            ddlSender.SelectedIndex = 0;
        }

        protected void btnInsertURL_Click(object sender, EventArgs e)
        {
            if (ddlURL.SelectedIndex > 0)
            {
                txtMsg.Text = txtMsg.Text.Trim() + " " + ddlURL.SelectedValue + " ";
                Session["SHORTURL"] = ddlURL.SelectedValue;
                ShowMsgCharCnt();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('2 more character will be added in the link text for individual mobile numbers. Length of Message characters will be increased by 2.');", true);
            }
            pnlPopUp_Detail1_ModalPopupExtender.Show();
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

        protected void btnSend_Click(object sender, EventArgs e)
        {
            bool b = MsgSend("");
            if (!b)
            {
                pnlPopUp_Detail1_ModalPopupExtender.Show();
            }
        }

        public bool MsgSend(string sch)
        {
            string filenm = "";
            string filenmext = "";
            if (ddlSender.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
                return false;
            }
            Helper.Util ob = new Helper.Util();

            DataTable dt2 = ob.GetUserParameter(user);
            string bal2 = dt2.Rows[0]["balance"].ToString();

            string country_code = "";
            string mobile = "";

            country_code = Convert.ToString(ViewState["CCode"]);
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();

            if (txtMsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text first.');", true);
                return false;
            }
            string UserID = Convert.ToString(Session["UserID"]);
            if (ddlURL.SelectedValue != "0" && (!(txtMsg.Text.ToLower().Contains("http://") || txtMsg.Text.ToLower().Contains("https://"))))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('URL must be present in Message Text for as you have selected a Short URL.');", true);
                return false;
            }

            string shortURL = "";
            int shortURLId = 0;
            string ws = "";
            if (ddlURL.SelectedValue != "0")
            {
                shortURL = Session["SHORTURL"].ToString();
                ws = Convert.ToString(Session["DOMAINNAME"]);
                shortURLId = ob.GetUrlID(UserID, shortURL.Replace(ws, ""));
            }

            Int32 cnt = 0;
            DataTable dtRec = (DataTable)Session["dtRETARGET"];
            cnt = dtRec.Rows.Count;
            int noofsms = 0;
            bool ucs2 = false;
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

            if (ddlURL.SelectedValue != "0") qlen += 2;

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

            Int32 noofmessages = noofsms * cnt;

            //update balance
            double rate = 0;
            rate = (ddlURL.SelectedValue == "0" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
            rate = (ddlURL.SelectedValue != "0" ? Convert.ToDouble(Session["RATE_SMARTSMS"]) : rate);

            double bal = Convert.ToDouble(bal2) * 1000;
            if (bal - Convert.ToDouble(noofmessages * (rate * 10)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                return false;
            }

            Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, "", noofmessages, rate);

            Label lblbalance = Master.FindControl("lblBal") as Label;
            lblbalance.Text = Convert.ToString(Session["SMSBAL"]);

            DataTable dtSMPPAC = new DataTable();

            dtSMPPAC = ob.GetUserSMPPACCOUNTCountry(Convert.ToString(Session["UserId"]), country_code);
            
            string templID = "";
            if (ddlTempID.SelectedValue != "0") templID = ddlTempID.SelectedValue;

            if (sch != "")
            {

            }
            else
            {
                if (ddlURL.SelectedValue != "0")
                {
                    ob.Insert_SMS_BULK_4url(UserID, txtMsg.Text, ddlSender.Text, "", shortURLId, shortURL, ws, rate, "2", filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, mobList, "", "RETARGET", templID, country_code);
                }
                else
                    ob.InsertSMSrecordsFromUSERTMP(UserID, ddlSender.Text, "1", txtMsg.Text, filenm, filenmext, dtSMPPAC, "", ucs2, noofsms, rate, mobList, "", "RETARGET", templID, country_code);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');", true);
            }
            return true;
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
            ddlTempID.SelectedIndex = 0;
        }

        protected void ddlTempID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["UserID"]), ddlTempID.SelectedValue);
            if (ddlTempID.SelectedValue == "0") txtTempSMS.Text = "";
            else txtTempSMS.Text = dtT.Rows[0]["template"].ToString();

            pnlPopUp_Detail1_ModalPopupExtender.Show();
        }

        protected void rbTempWise_CheckedChanged(object sender, EventArgs e)
        {
            ditempwise.Visible = true;
            didatewise.Visible = false;
            txtdatefromCLickDate.Text = "";
            txtdatetoToCLickDate.Text = "";
            DataTable dt = null;
            grv.DataSource = dt;
            grv.DataBind();
        }

        protected void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            ditempwise.Visible = false;
            didatewise.Visible = true;
            txtdatefromCLickDate.Text = "";
            txtdatetoToCLickDate.Text = "";
            DataTable dt = null;
            grv.DataSource = dt;
            grv.DataBind();
        }
    }
}