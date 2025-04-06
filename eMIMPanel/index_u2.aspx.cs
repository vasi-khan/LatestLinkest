using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class index_u2 : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            //get from dashboard
            GetValueForDashboard();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            lblLastUpd.Text = Convert.ToString(Session["lblLastUpd"]);
            lblNotice.Text = Convert.ToString(Session["Notice"]);
            grv2.DataSource = ob.GetValueForURL(user);
            grv2.DataBind();
            if (!Page.IsPostBack)
            {
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "maker")
                {
                    HeadingCheckerMaker.InnerText = "Makers Campaign";
                }
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "checker")
                {
                    HeadingCheckerMaker.InnerText = "Checkers Campaign for Approval";
                }
            }

            if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "maker")
            {
                dt = ob.GetMakerData(user);
            }
            if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "checker")
            {
                dt = ob.GetCheckerData(user);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "maker")
                {
                    grdMakerCampaign.Columns[9].Visible = true;
                    grdMakerCampaign.Columns[10].Visible = true;
                    grdMakerCampaign.Columns[11].Visible = false;
                    grdMakerCampaign.Columns[12].Visible = false;
                }
                if (Convert.ToString(Session["MakerCheckerType"]).ToLower() == "checker")
                {
                    grdMakerCampaign.Columns[9].Visible = false;
                    grdMakerCampaign.Columns[10].Visible = false;
                    grdMakerCampaign.Columns[11].Visible = true;
                    grdMakerCampaign.Columns[12].Visible = true;
                }
                grdMakerCampaign.DataSource = dt;
                grdMakerCampaign.DataBind();
                GridFormat(dt);
            }

            //SMSFields();
            //URLFields();
            ////AccountFields();
        }

        protected void GridFormat(DataTable dt)
        {
            grdMakerCampaign.UseAccessibleHeader = true;
            grdMakerCampaign.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grdMakerCampaign.TopPagerRow != null)
            {
                grdMakerCampaign.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grdMakerCampaign.BottomPagerRow != null)
            {
                grdMakerCampaign.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
            {
                try
                {
                    grdMakerCampaign.FooterRow.TableSection = TableRowSection.TableFooter;
                }
                catch (Exception ex) { }
            }
        }

        public void GetValueForDashboard()
        {
            DataTable dt = ob.GetDashboardSummary(user);
            if (dt.Rows.Count > 0)
            {
                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["smssubmitted"]);
                Session["lblTodaySubmitted"] = lblTodaySubmitted.Text;
                lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["smsdelivered"]);
                Session["lblTodayDelivered"] = lblTodayDelivered.Text;
                lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["smsfailed"]);
                Session["lblTodayFailed"] = lblTodayFailed.Text;
                lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["links"]);
                Session["lblMonthLinkCreated"] = lblMonthLinkCreated.Text;
                lblMonthClick.Text = Convert.ToString(dt.Rows[0]["clicks"]);
                Session["lblMonthClick"] = lblMonthClick.Text;
                lblMonthSmsClick.Text = Convert.ToString(dt.Rows[0]["smsclicks"]);
                Session["lblMonthSmsClick"] = lblMonthSmsClick.Text;
                lblLastUpd.Text = Convert.ToString(dt.Rows[0]["updtime"]);
                Session["lblLastUpd"] = lblLastUpd.Text;
            }
            else
            {
                lblTodayFailed.Text = "0"; Session["lblTodayFailed"] = "0";
                lblTodayDelivered.Text = "0"; Session["lblTodayDelivered"] = "0";
                lblTodaySubmitted.Text = "0"; Session["lblTodaySubmitted"] = "0";
                lblMonthLinkCreated.Text = "0"; Session["lblMonthLinkCreated"] = "0";
                lblMonthClick.Text = "0"; Session["lblMonthClick"] = "0";
                lblMonthSmsClick.Text = "0"; Session["lblMonthSmsClick"] = "0";
                lblLastUpd.Text = ""; Session["lblLastUpd"] = "";
            }
        }

        public void SMSFields()
        {
            string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetSMSSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["Submitted"]);
                lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["Delivered"]);
                lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["Failed"]);
            }
            else
            {
                lblTodayFailed.Text = "0";
                lblTodayDelivered.Text = "0";
                lblTodaySubmitted.Text = "0";
            }

        }

        public void URLFields()
        {
            string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetURLSummary(s1, s2, usertype, user);
            DataTable dt2 = ob.GetSMSClickSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["URLS"]);
                lblMonthClick.Text = Convert.ToString(dt.Rows[0]["CLICKED"]);
            }
            else
            {
                lblMonthLinkCreated.Text = "0";
                lblMonthClick.Text = "0";
            }
            if (dt2.Rows.Count > 0) lblMonthSmsClick.Text = dt2.Rows[0][0].ToString();
            else lblMonthSmsClick.Text = "0";
        }

        public void AccountFields()
        {
            //string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            //string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //lblAccountCreated.Text = ob.GetAccountSummary(s1, s2, usertype, user);
            //lblLastMonthAccountCreated.Text = ob.GetAccountSummaryLastMonth(s1, s2, usertype, user);
            //lblCreditAllotted.Text = ob.GetCreditSummary(s1, s2, usertype, user);
            //lblLastMonthCreditAlloted.Text = ob.GetCreditSummaryLastMonth(s1, s2, usertype, user);
            //lblActiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "1");
            //lblInactiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "0");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SMSFields();
            URLFields();

            string s = lblTodaySubmitted.Text;
            string d = lblTodayDelivered.Text;
            string f = lblTodayFailed.Text;
            string l = lblMonthLinkCreated.Text;
            string c = lblMonthClick.Text;
            string m = lblMonthSmsClick.Text;
            ob.UpdateDashboard(user, s == "" ? "0" : s, d == "" ? "0" : d, f == "" ? "0" : f, l == "" ? "0" : l, c == "" ? "0" : c, m == "" ? "0" : m);
            GetValueForDashboard();
            Response.Redirect("index_u2.aspx");
        }

        protected void btnRenew_Click(object sender, EventArgs e)
        {

            LinkButton btndetails = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btndetails.NamingContainer;
            string id = ((HiddenField)row.FindControl("hdnId")).Value;
            string sUrl = ((Label)row.FindControl("lbl2")).Text;
            string user = Convert.ToString(Session["UserID"]);
            ob.RenewShortURL(user, id);

            string bal2 = ob.UdateAndGetURLbal1(Convert.ToString(Session["UserID"]), sUrl);

            Session["SMSBAL"] = bal2;

            string msg = "URL Expires On: " + DateTime.Now.AddDays(7).ToString("dd-MM-yyyy");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + msg + "');window.location ='index_u2.aspx';", true);

            //grv2.DataSource = ob.GetValueForURL(user);
            //grv2.DataBind();
        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)lnkbtn.NamingContainer;
            HiddenField hId = (HiddenField)gvrow.FindControl("hdnMId");
            ViewState["ID"] = hId.Value;
            string sql = "select * from fileprocessMaker with(nolock) where profileid='" + user + "' and id=" + hId.Value + "";
            DataTable dt = database.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblCampaignName.Text = Convert.ToString(dt.Rows[0]["campname"]);
                lblSenderID.Text = Convert.ToString(dt.Rows[0]["sender"]);
                lblFileName.Text = Convert.ToString(dt.Rows[0]["filename"]);
                lblTemplateID.Text = Convert.ToString(dt.Rows[0]["templateid"]);
                lblMobileCount.Text = Convert.ToString(dt.Rows[0]["noofrecord"]);
                lblTemplateText.Text = Convert.ToString(dt.Rows[0]["msg"]);
            }
            pnlPopUp_Detail_ModalPopupExtender.Show();
        }

        protected void lnkbtnTest_Click(object sender, EventArgs e)
        {
            string sql = "select * from fileprocessMaker with(nolock) where profileid='" + user + "' and id=" + Convert.ToString(ViewState["ID"]) + "";
            DataTable dt = database.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(txtMobNum.Text.Trim()) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Number !!');", true);
                    pnlPopUp_Detail_ModalPopupExtender.Show();
                    return;
                }
                string mobile = "";
                int moblen = 0; Int32 cnt = 0;
                moblen = Convert.ToInt32(Convert.ToString(database.GetScalarValue("SELECT moblength FROM tblcountry WHERE counryCode='" + Convert.ToString(dt.Rows[0]["ccode"]) + "'")));
                if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                mobList = mobList.Distinct().ToList();// Remove Duplicate
                mobList.RemoveAll(x => x.Length < moblen);
                mobList = mobList.Select(x => x.Substring(x.Length - moblen)).ToList();
                int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
                cnt = mobList.Count;
                Int32 TestingCount = Convert.ToInt32(database.GetScalarValue(@"select TestingCount from customer where username='" + user + "'"));
                if (cnt > TestingCount)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can send SMS only '" + TestingCount + "' Mobile Numbers !!');", true);
                    pnlPopUp_Detail_ModalPopupExtender.Show();
                    return;
                }
                string shortURL = "";
                int shortURLId = 0;
                string ws = "";
                if (dt.Rows[0]["smstype"].ToString() == "2")
                {
                    shortURL = dt.Rows[0]["shorturl"].ToString();
                    ws = Convert.ToString(Session["DOMAINNAME"]);
                    shortURLId = ob.GetUrlID(user, shortURL.Replace(ws, ""));
                }
                if (dt.Rows[0]["shorturl"].ToString() != null)
                    if (Convert.ToString(dt.Rows[0]["shorturl"]) != "")
                    {
                        if (dt.Rows[0]["smstype"].ToString() == "3" || dt.Rows[0]["smstype"].ToString() == "6" || dt.Rows[0]["smstype"].ToString() == "8")
                        {
                            shortURL = dt.Rows[0]["shorturl"].ToString();
                            ws = Convert.ToString(Session["DOMAINNAME"]);
                            shortURLId = ob.GetUrlID(user, shortURL.Replace(ws, ""));
                        }
                    }


                string q = lblTemplateText.Text.Trim();
                int count_PIPE = q.Count(f => f == '|');
                int qlen = lblTemplateText.Text.Trim().Length + count_PIPE;

                int count_tild = q.Count(f => f == '~'); qlen = qlen + count_tild;
                int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
                int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
                int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
                int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
                int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
                int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;

                if (dt.Rows[0]["smstype"].ToString() == "2") qlen += 2;
                if (dt.Rows[0]["shorturl"].ToString() != null)
                    if (Convert.ToString(dt.Rows[0]["shorturl"]) != "")
                    {
                        if (dt.Rows[0]["smstype"].ToString() == "3" || dt.Rows[0]["smstype"].ToString() == "6" || dt.Rows[0]["smstype"].ToString() == "8")
                            qlen += 2;
                    }
                int noofsms = 0;
                bool ucs2 = false;
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
                double rate = 0;
                rate = (dt.Rows[0]["smstype"].ToString() == "1" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
                rate = (dt.Rows[0]["smstype"].ToString() == "2" ? Convert.ToDouble(Session["RATE_SMARTSMS"]) : rate);
                rate = (dt.Rows[0]["smstype"].ToString() == "3" ? Convert.ToDouble(Session["RATE_CAMPAIGN"]) : rate);
                rate = (dt.Rows[0]["smstype"].ToString() == "4" ? Convert.ToDouble(Session["RATE_OTP"]) : rate);
                rate = (dt.Rows[0]["smstype"].ToString() == "6" || dt.Rows[0]["smstype"].ToString() == "8" ? Convert.ToDouble(Session["RATE_NORMALSMS"]) : rate);
                if (rate <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                    pnlPopUp_Detail_ModalPopupExtender.Show();
                    return;
                }

                DataTable dt2 = ob.GetUserParameter(user);
                string bal2 = dt2.Rows[0]["balance"].ToString();

                double bal = Convert.ToDouble(bal2) * 1000;
                if (bal - Convert.ToDouble(noofmessages * (rate * 10)) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient SMS Balance.');", true);
                    pnlPopUp_Detail_ModalPopupExtender.Show();
                    return;
                }
                bool bulk = mobList.Count > 25 ? true : false;
                string sql1 = @" Insert into SMSFILEUPLOAD (USERID,RECCOUNT,senderid,campaignname,smsrate,shortURLId,countrycode,dlrcode)
                                   values ('" + user + "','" + mobList.Count.ToString() + "','" + lblSenderID.Text + "','Manual','" + rate + "','" + Convert.ToString(dt.Rows[0]["shorturlid"]) + "','" + Convert.ToString(dt.Rows[0]["ccode"]) + "','" + Convert.ToString(dt.Rows[0]["DlrCode"]) + "') " +
                        "  select max(id) from SMSFILEUPLOAD where userid='" + user + "' ; ";
                string fileId = Convert.ToString(database.GetScalarValue(sql1));

                DataTable dtSMPPAC = new DataTable();
                if (Convert.ToString(dt.Rows[0]["smstype"]) == "6") //03 / 03 / 2022
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
                else if (Convert.ToString(dt.Rows[0]["smstype"]) == "3")
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
                else if (Convert.ToString(dt.Rows[0]["smstype"]) == "7")
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
                else if (Convert.ToString(dt.Rows[0]["smstype"]) == "8")
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
                    string msg = lblTemplateText.Text.Trim();
                    string shurl = "";
                    string mseg = "";
                    if (dt.Rows[0]["smstype"].ToString() == "2")
                    {
                        mseg = ob.NewSegment8();
                        shurl = ws + mseg;
                        msg = msg.Replace(shortURL, shurl);
                    }
                    if (dt.Rows[0]["shorturl"].ToString() != null)
                        if (Convert.ToString(dt.Rows[0]["shorturl"].ToString()) != "")
                        {
                            if (dt.Rows[0]["smstype"].ToString() == "3" || dt.Rows[0]["smstype"].ToString() == "6" || dt.Rows[0]["smstype"].ToString() == "8")
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
                        database.ExecuteNonQuery("INSERT INTO blocksmswhitelist (userid,mobile,insertdate,isAuto) VALUES('" + user + "','" + m + "',GETDATE(),1)");
                    }
                    catch (Exception e2) { }
                    int chk = int.Parse(Convert.ToString(database.GetScalarValue("select count(1) from globalBlackListNo where mobile='" + m + "'")));
                    if (chk == 0)
                    {
                        ob.B4SEND_SendURL_SMS(user, m, msg.Trim(), lblSenderID.Text, dtSMPPAC, ucs2, bulk, rate, noofsms, lblTemplateID.Text.Trim(), Convert.ToString(dt.Rows[0]["ccode"]), Convert.ToString(dt.Rows[0]["smstype"]), fileId, Convert.ToString(dt.Rows[0]["dlrcode"]));
                        if (Convert.ToString(dt.Rows[0]["smstype"]) == "2") ob.SaveURL_MOBILE(user, shortURLId, m, mseg, resp, Convert.ToString(dt.Rows[0]["ccode"]), fileId, lblTemplateID.Text.Trim());
                        if (Convert.ToString(dt.Rows[0]["shorturl"]) != null)
                            if (Convert.ToString(dt.Rows[0]["shorturl"]) != "")
                            {
                                if (Convert.ToString(dt.Rows[0]["smstype"]) == "3" || Convert.ToString(dt.Rows[0]["smstype"]) == "6" || Convert.ToString(dt.Rows[0]["smstype"]) == "8") ob.SaveURL_MOBILE(user, shortURLId, m, mseg, resp, Convert.ToString(dt.Rows[0]["ccode"]), fileId, lblTemplateID.Text.Trim());
                            }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully');window.location ='index_u2.aspx';", true);
                        ViewState["ID"] = "";
                    }
                }
            }
        }

        protected void lnkbtnExecute_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)lnkbtn.NamingContainer;
            HiddenField hId = (HiddenField)gvrow.FindControl("hdnMId");
            if (Convert.ToString(hId.Value) != "")
            {
                ViewState["Execute"] = hId.Value;
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
            }
        }

        protected void btnCancelSch_Click(object sender, EventArgs e)
        {
            ClearScheduleDateTime();
            pnlPopUp_SCHEDULE_ModalPopupExtender.Hide();
        }
        protected void ClearScheduleDateTime()
        {
            hdnScheduleCount.Value = "1";
        }
        protected void btnScheduleSMS_Click(object sender, EventArgs e)
        {
            try
            {
                liScheduleDates = new List<string>();
                ValidateScheduleTime();
                SetAttribueScheduleRows(hdnScheduleCount.Value);
                if (hdnScheduleCount.Value != liScheduleDates.Count.ToString())
                {
                    return;
                }

                if (liScheduleDates.Count > 0 && liScheduleDates != null)
                {
                    string sql = @"declare @maxid int 
                                     SET @maxid=isnull((select max(id) From FileProcess),0)+1 
                        INSERT INTO fileprocess (id,profileid, fileName, tblname, noofrecord, templateid, msg, sender, isschedule, scheduletime, ccode, smstype, shorturlid, shorturl, " +
                        "domainname, ucs2, noofsms, campname, prevbalance, availablebalance, methodname, InsertTime, fileext, rate, scheduleDeletedTime, IsFromRcsFailOver, scratchcard, EventCode, IsDLRData, DlrCode, IsAutoMapping, SaveFileName, MakerFileProcessID) " +
                         "select @maxid,profileid,filename,tblname,noofrecord,templateid,msg,sender,1,'" + Convert.ToString(liScheduleDates[0]) + "',ccode,smstype, shorturlid, shorturl," +
                         "domainname, ucs2, noofsms, campname, prevbalance, availablebalance, methodname, InsertTime, fileext, rate, scheduleDeletedTime, IsFromRcsFailOver, scratchcard, EventCode, IsDLRData, DlrCode, IsAutoMapping, " +
                         "SaveFileName," + Convert.ToString(ViewState["Execute"]) + " from fileprocessMaker where profileid='" + user + "' and id=" + Convert.ToString(ViewState["Execute"]) + "; " +
                         "INSERT INTO MobileDependencyMaker (FileProcessid, MoblieNo, Type, InsertDateTime) select FileProcessid, MoblieNo, Type, InsertDateTime from MobileDependency where FileProcessId=@maxid ; " + 
                         "update fileprocessMaker set isprocessed=1,processedtime=getdate() where profileid='" + user + "' and id=" + Convert.ToString(ViewState["Execute"]) + "";
                    try
                    {
                        database.ExecuteNonQuery(sql);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Scheduled Successfully');window.location ='index_u2.aspx';", true);
                        ViewState["Execute"] = "";
                        return;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "')", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        List<string> liScheduleDates;
        protected bool ValidateScheduleTime()
        {
            #region ScheduleTime 0
            // 26 jul 2022
            string ccode = Convert.ToString(database.GetScalarValue(@"select ccode from fileprocessMaker with(nolock) where profileid='" + user + "' and id=" + Convert.ToString(ViewState["Execute"]) + ""));
            int timedifferenceInMinute = Convert.ToInt32(database.GetScalarValue("select timedifferenceInMinute from tblCountry where counryCode in(" + ccode + ")"));
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
            if (Convert.ToDateTime(datetime, CultureInfo.InvariantCulture) <= DateTime.Now.AddMinutes(timedifferenceInMinute))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule date " + datetime + " cannot be below of current date time');", true);
                pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
                divSchedule.Style.Add("display", "none");
                return false;
            }
            string schMin = Convert.ToString(ConfigurationManager.AppSettings["SCHEDULEMINUTE"]);
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
            return true;
        }
        protected void SetAttribueScheduleRows(string rowCount)
        {
            if (rowCount == "1" || rowCount == "")
            {
                lblScheduleDate.Text = hdnScheduleDate.Value;
            }
        }
        protected void rdbScheduleSMS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbScheduleSMS.Checked)
            {
                lnkbtnSendInstantly.Visible = false;
                btnScheduleSMS.Visible = true;
            }
            if (rdbSendInstantly.Checked)
            {
                btnScheduleSMS.Visible = false;
                lnkbtnSendInstantly.Visible = true;
            }
            pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
        }

        protected void lnkbtnSendInstantly_Click(object sender, EventArgs e)
        {
            string sql = @"declare @maxid int 
                                     SET @maxid=isnull((select max(id) From FileProcess),0)+1 
                        INSERT INTO fileprocess (id,profileid, fileName, tblname, noofrecord, templateid, msg, sender, ccode, smstype, shorturlid, shorturl, " +
                        "domainname, ucs2, noofsms, campname, prevbalance, availablebalance, methodname, InsertTime, fileext, rate, IsFromRcsFailOver, scratchcard, EventCode, IsDLRData, DlrCode, IsAutoMapping, SaveFileName, MakerFileProcessID) " +
                         "select @maxid,profileid,filename,tblname,noofrecord,templateid,msg,sender,ccode,smstype, shorturlid, shorturl," +
                         "domainname, ucs2, noofsms, campname, prevbalance, availablebalance, methodname, InsertTime, fileext, rate, IsFromRcsFailOver, scratchcard, EventCode, IsDLRData, DlrCode, IsAutoMapping, " +
                         "SaveFileName," + Convert.ToString(ViewState["Execute"]) + " from fileprocessMaker where profileid='" + user + "' and id=" + Convert.ToString(ViewState["Execute"]) + "; " +
                         "INSERT INTO MobileDependencyMaker (FileProcessid, MoblieNo, Type, InsertDateTime) select FileProcessid, MoblieNo, Type, InsertDateTime from MobileDependency where FileProcessId=@maxid ; " +
                         "update fileprocessMaker set isprocessed=1,processedtime=getdate() where profileid='" + user + "' and id=" + Convert.ToString(ViewState["Execute"]) + "";
            try
            {
                database.ExecuteNonQuery(sql);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Send Successfully');window.location ='index_u2.aspx';", true);
                ViewState["Execute"] = "";
                return;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "')", true);
                return;
            }
        }
        protected void lnkbtnApprovePopup_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = (LinkButton)sender;
            GridViewRow grdrow = (GridViewRow)lnkbtn.NamingContainer;
            HiddenField hfID = (HiddenField)grdrow.FindControl("hdnMId");
            ViewState["hfID"] = hfID.Value;
            lblHeadingReason.Text = "Approve Reason";
            pnlPopUp_AppRej_ModalPopupExtender.Show();
        }

        protected void lnkbtnRejectPopup_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = (LinkButton)sender;
            GridViewRow grdrow = (GridViewRow)lnkbtn.NamingContainer;
            HiddenField hfID = (HiddenField)grdrow.FindControl("hdnMId");
            ViewState["hfID"] = hfID.Value;
            lblHeadingReason.Text = "Reject Reason";
            pnlPopUp_AppRej_ModalPopupExtender.Show();
        }

        protected void lnkbtnAppRej_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(ViewState["hfID"]) != "")
            {
                string Status = "";
                if (Convert.ToString(txtreason.Text) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Reason !!')", true);
                    return;
                }
                if (lblHeadingReason.Text.Contains("Approve"))
                {
                    Status = "Approved";
                }
                if (lblHeadingReason.Text.Contains("Reject"))
                {
                    Status = "Rejected";
                }
                ob.UpdateCheckerMakerStatus(user,Status,Convert.ToString(txtreason.Text.Trim()), Convert.ToString(ViewState["hfID"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Status Updated Successfully !!');window.location ='index_u2.aspx';", true);
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlPopUp_AppRej_ModalPopupExtender.Hide();
        }
    }
}