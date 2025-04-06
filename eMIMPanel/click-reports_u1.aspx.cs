using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class click_reports_u1 : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {

            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                DataTable dt = ob.GetUserReport(user, ws, "02/JAN/1900", "02/JAN/1900", true);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            s1 = hdntxtFrm.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
        }

        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetUserReport(user, "", s1, s2, true, "", rblFilter.SelectedValue);
            grv.DataSource = null;
            System.Data.DataColumn newColumn = new System.Data.DataColumn("From");
            newColumn.DefaultValue = s1;
            dt.Columns.Add(newColumn);
            System.Data.DataColumn newColumn1 = new System.Data.DataColumn("To");
            newColumn1.DefaultValue = s2;
            dt.Columns.Add(newColumn1);

            ViewState["From"] = s1;
            ViewState["To"] = s2;
            Session["ClickReportMaindt"] = dt;
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
                    GridFormat((DataTable)Session["ClickReportMaindt"]);
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
            }
            DataTable dt = ob.GetUserReportDetail(user, lblurlid.Text, true, s1, s2, lblcountry.Text);
            DataView view = new DataView(dt);
            dt = view.ToTable(true, "Slno", "mobile", "smsDate", "ClickDate", "operator", "Circle", "Platform", "Browser", "IsMobileDevice", "MobileDeviceManufacturer", "MobileDeviceModel");
            dt.Columns["IsMobileDevice"].ColumnName = "FromMobileDevice";

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
            //if specific user has already the long url, then don't reduce balance -
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

            ob.noof_message = noofmessages;
            ob.msg_rate = rate;

            Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, "", noofmessages, rate);
            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);

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
    }
}