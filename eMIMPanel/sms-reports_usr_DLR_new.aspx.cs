using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using Ionic.Zip;
using ZipFile = System.IO.Compression.ZipFile;
using System.Globalization;
using System.Data.SqlClient;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class sms_reports_usr_DLR_new : System.Web.UI.Page
    {

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
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //s1 = h1.Value;
                //s2 = h2.Value;
                GetData();
                //txtFrm.Text = hdntxtFrm.Value;
                //txtTo.Text = hdntxtTo.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            s1 = "2020-10-05";
            s2 = "2020-10-05 23:59:59.999";

            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetSMSReport_user_new2(s1, s2, usertype, user);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }

        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            //if (grv.TopPagerRow != null)
            //{
            //    grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            //}
            //if (grv.BottomPagerRow != null)
            //{
            //    grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            //}
            //if (dt.Rows.Count > 0)
            //    grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void GridFormat2(DataTable dt)
        {
            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            //if (grv2.TopPagerRow != null)
            //{
            //    grv2.TopPagerRow.TableSection = TableRowSection.TableHeader;
            //}
            //if (grv2.BottomPagerRow != null)
            //{
            //    grv2.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            //}
            //if (dt.Rows.Count > 0)
            //    grv2.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField fileid = (HiddenField)gvro.FindControl("hdnFileId");
            HiddenField userid = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsender = (Label)gvro.FindControl("lblsender");
            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            Session["rptFILEID"] = fileid.Value;
            Session["rptSENDER"] = lblsender.Text;
            Session["rptUSERID"] = userid.Value;
            Session["rptS1"] = s1;
            Session["rptS2"] = s2;

            string url = ResolveUrl("~\\sms-reports-detail_usr.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);


            return;
        }

        protected void btnXL_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField fileid = (HiddenField)gvro.FindControl("hdnFileId");
            HiddenField userid = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsender = (Label)gvro.FindControl("lblsender");
            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            DataTable dt = ob.GetSMSReportDetail_user_new2(userid.Value, fileid.Value, lblsender.Text, s1, s2);
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "SMSReportDetail.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }

        protected void btnXLdw_Click(object sender, EventArgs e)
        {
            if (Session["MOBILEDATA"] != null)
            {
                Session["FILENAME"] = "SMS_Mobile_Seaarch_Report.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
        }

        //--------------------------Working Code -------------------------------------------------//
        string d1 = "";
        string d2 = "";
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtFrm1.Text = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");

            //txtTo1.Text = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1800;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");

            //SMSFields();
            if (!IsPostBack)
            {
                BindTemplate();

                PopulateSender();
                PopulateCampaign();
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);

                dt = ob.GetSMSReport_user_newConsolidated("02/JAN/1900", "02/JAN/1900", txtmob.Text, user, "", "");
                grv2.DataSource = null;
                grv2.DataSource = dt;
                grv2.DataBind();
                GridFormat2(dt);
            }

            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
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

        public void PopulateCampaign()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetCampaignAll(Convert.ToString(Session["UserID"]));

            ddlCamp.DataSource = dt;
            ddlCamp.DataTextField = "campaignname";
            ddlCamp.DataValueField = "campaignname";
            ddlCamp.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlCamp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlCamp.SelectedIndex = 1;
            else
                ddlCamp.SelectedIndex = 0;
        }


        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
        }

        protected void rbTdy_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-none");
        }

        protected void rbHis_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-block");
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            string strpName = "sp_GetSMSReport_test";
            try
            {
                Helper.Util ob = new Helper.Util();
                string usertype = Convert.ToString(Session["UserType"]);
                string user = Convert.ToString(Session["UserID"]);
                string mobno = txtmob.Text.ToString().Trim();
                DataTable dt = new DataTable();

                if (rbTdy.Checked)
                {
                    d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
                }
                if (rbHis.Checked)
                {
                    if (hdntxtFrm1.Value == "" && hdntxtTo1.Value == "")
                    {
                        d1 = "";
                        d2 = "";
                    }
                    else if (hdntxtFrm1.Value != "" && hdntxtTo1.Value == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter To Date');", true);
                        return;
                    }
                    else if (hdntxtFrm1.Value == "" && hdntxtTo1.Value != "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter From Date');", true);
                        return;
                    }
                    else
                    {
                        if (Convert.ToDateTime(DateTime.Now.Date) == Convert.ToDateTime(hdntxtFrm1.Value) || Convert.ToDateTime(DateTime.Now.Date) == Convert.ToDateTime(hdntxtTo1.Value))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delivery Report can be fetched upto Last Date or for Today only. Select Today Option to fetch Delivery Report for Today or Select Date Range upto Yesterday for Old Reports.');", true);
                            return;
                        }
                        d1 = Convert.ToDateTime(hdntxtFrm1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        d2 = Convert.ToDateTime(hdntxtTo1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
                    }
                }

                SqlParameter[] arrprm = new SqlParameter[8];

                arrprm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
                arrprm[0].Value = d1;
                arrprm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
                arrprm[1].Value = d2;
                arrprm[2] = new SqlParameter("@user", SqlDbType.VarChar, 50);
                arrprm[2].Value = user.ToString();
                arrprm[3] = new SqlParameter("@campnm", SqlDbType.VarChar, 50);
                if (rdbCamp.Checked)
                {
                    arrprm[3].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
                }
                else
                {
                    arrprm[3].Value = "";
                }
                arrprm[4] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
                if (rdbSender.Checked)
                {
                    arrprm[4].Value = ddlSender.SelectedValue == "0" ? "" : ddlSender.SelectedValue;
                }
                else
                {
                    arrprm[4].Value = "";
                }
                arrprm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
                arrprm[5].Value = mobno.ToString();
                arrprm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 50);

                if (rbDlvr.Checked == true)
                {
                    arrprm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                    arrprm[7].Value = "DELIVERED";
                }
                else if (rbFailed.Checked == true)
                {
                    arrprm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                    arrprm[7].Value = "FAILED";
                }
                else if (rbSbmtd.Checked == true)
                {
                    arrprm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                    arrprm[7].Value = "";
                }

                if (rbTdy.Checked)
                {
                    arrprm[4].Value = "";
                    arrprm[3].Value = "";
                    arrprm[6].Value = "";
                    dt = database.GetDataTableSp(arrprm, strpName);

                }
                if (rbHis.Checked)
                {
                    if (rdblistselect.SelectedValue == "S")
                    {
                        strpName = "sp_GetSMSReport_testSingle";
                    }
                    arrprm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                    dt = dbHistorical.GetDataTableSp(arrprm, strpName);
                }

                if (dt.Rows.Count > 0)
                {
                    grv2.DataSource = null;
                    grv2.DataSource = dt;
                    grv2.DataBind();
                    GridFormat2(dt);

                    bool a = ob.CheckUserName(user);
                    if (a)
                    {
                        foreach (GridViewRow row in grv2.Rows)
                        {
                            LinkButton lb = row.FindControl("Downloadxlx") as LinkButton;
                            lb.Visible = true;
                        }
                    }
                }
                else
                {
                    grv2.DataSource = null;
                    grv2.DataSource = dt;
                    grv2.DataBind();
                    GridFormat2(dt);
                }
                txtFrm1.Text = hdntxtFrm1.Value;
                txtTo1.Text = hdntxtTo1.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        public void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        // download inside gridview click
        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label lblsender = (Label)gvro.FindControl("lblsender");
                HiddenField hdnfrm = (HiddenField)gvro.FindControl("hdnFrmDW");
                HiddenField hdnto = (HiddenField)gvro.FindControl("hdnToDW");

                d1 = hdnfrm.Value; d2 = hdnto.Value + " 23:59:59.997";
                Helper.Util ob = new Helper.Util();
                string usertype = Convert.ToString(Session["UserType"]);
                string user = Convert.ToString(Session["UserID"]);
                string senderid = lblsender.Text.Trim();
                string mobno = txtmob.Text.ToString().Trim();
                String STR = "Sp_GetSMSReport_User_ConsolidatedDETAIL";

                string mainPath = "DWReports";
                string subPath = "DWReports/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                string mappath = Server.MapPath(subPath);
                if (rdblistselect.SelectedValue == "D")
                {
                    for (var day = Convert.ToDateTime(hdnfrm.Value).Date; day.Date <= Convert.ToDateTime(hdnto.Value).Date; day = day.AddDays(1))
                    {
                        DateTime fromDate = day;
                        string date = fromDate.ToString("yyyy-MM-dd");

                        SqlParameter[] aarPrm = new SqlParameter[8];

                        aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
                        aarPrm[0].Value = date;

                        aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
                        aarPrm[1].Value = date + " 23:59:59.997";

                        aarPrm[2] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
                        aarPrm[2].Value = senderid;
                        aarPrm[3] = new SqlParameter("@user", SqlDbType.VarChar, 50);
                        aarPrm[3].Value = user;

                        aarPrm[4] = new SqlParameter("@campnm", SqlDbType.VarChar, 500);

                        if (rdbCamp.Checked)
                        {
                            aarPrm[4].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
                        }
                        else
                        {
                            aarPrm[4].Value = "";
                        }
                        aarPrm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
                        aarPrm[5].Value = mobno;
                        aarPrm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 50);
                        aarPrm[6].Value = "";
                        if (rbDlvr.Checked == true)
                        {
                            aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                            aarPrm[7].Value = "DELIVERED";
                        }
                        else if (rbFailed.Checked == true)
                        {
                            aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                            aarPrm[7].Value = "FAILED";
                        }
                        else if (rbSbmtd.Checked == true)
                        {
                            aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                            aarPrm[7].Value = "";
                        }
                        DataTable dt = null;
                        if (rbTdy.Checked)
                        {
                            STR = "Sp_GetSMSReport_user_ConsolidatedDownloadDETAIL";
                            dt = database.GetDataTableSp(aarPrm, STR);
                        }
                        if (rbHis.Checked)
                        {
                            aarPrm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                            dt = dbHistorical.GetDataTableSp(aarPrm, STR);
                        }

                        DataView dv = new DataView(dt);
                        //if(dv.Ro)
                        DataTable dtDates = dv.ToTable(true, "SMSdate");

                        bool exists = System.IO.Directory.Exists(mappath);
                        if (!exists) System.IO.Directory.CreateDirectory(mappath);

                        int takeCount = 100000;
                        int fn = 1;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataTable data = dt.AsEnumerable().Skip(i).Take(takeCount).CopyToDataTable();
                            i = i + takeCount - 1;

                            string mydate = dtDates.Rows[0]["SMSdate"].ToString();

                            ToCSV(data, mappath + @"\" + mydate.Replace(".", "-") + "_" + fn.ToString() + ".csv"); fn++;
                        }
                    }
                }
                else
                {
                    bool exists = System.IO.Directory.Exists(mappath);
                    if (!exists) System.IO.Directory.CreateDirectory(mappath);
                    DateTime fromDate = Convert.ToDateTime(hdnfrm.Value).Date;
                    DateTime ToDate = Convert.ToDateTime(hdnto.Value).Date;
                    string FrmDate = fromDate.ToString("yyyy-MM-dd");
                    string To_Date = ToDate.ToString("yyyy-MM-dd");

                    SqlParameter[] aarPrm = new SqlParameter[8];

                    aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
                    aarPrm[0].Value = FrmDate;

                    aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
                    aarPrm[1].Value = To_Date + " 23:59:59.997";
                    aarPrm[2] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
                    if (senderid == "NA")
                    {
                        aarPrm[2].Value = ddlSender.SelectedValue;
                    }
                    else
                    {
                        aarPrm[2].Value = senderid;
                    }
                    aarPrm[3] = new SqlParameter("@user", SqlDbType.VarChar, 50);
                    aarPrm[3].Value = user;
                    aarPrm[4] = new SqlParameter("@campnm", SqlDbType.VarChar, 500);

                    if (rdbCamp.Checked)
                    {
                        aarPrm[4].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
                    }
                    else
                    {
                        aarPrm[4].Value = "";
                    }
                    aarPrm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
                    aarPrm[5].Value = mobno;
                    aarPrm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 50);
                    aarPrm[6].Value = "";
                    //ADD BY NAVED KHAN
                    if (rbDlvr.Checked == true)
                    {
                        aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                        aarPrm[7].Value = "DELIVERED";
                    }
                    else if (rbFailed.Checked == true)
                    {
                        aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                        aarPrm[7].Value = "FAILED";
                    }
                    else if (rbSbmtd.Checked == true)
                    {
                        aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                        aarPrm[7].Value = "";
                    }
                    DataTable dt = null;
                    if (rbTdy.Checked)
                    {
                        dt = database.GetDataTableSp(aarPrm, STR);
                    }
                    if (rbHis.Checked)
                    {
                        aarPrm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                        dt = dbHistorical.GetDataTableSp(aarPrm, STR);
                    }
                    int fn = 1;
                    DataView dv = new DataView(dt);
                    DataTable dtDates = dv.ToTable(true, "SMSdate");

                    string mydate = dtDates.Rows[0]["SMSdate"].ToString();

                    ToCSV(dt, mappath + @"\" + mydate.Replace(".", "-") + "_" + fn.ToString() + ".csv"); fn++;
                }

                string startPath = mappath;//folder to add
                string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
                if (File.Exists(zipPath)) File.Delete(zipPath);
                System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                System.IO.Directory.Delete(mappath, true);

                string filename = zipPath;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

                if (fileInfo.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryReport.zip");
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.Flush();
                    Response.TransmitFile(fileInfo.FullName);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void Downloadxlx_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblsender = (Label)gvro.FindControl("lblsender");
            HiddenField hdnfrm = (HiddenField)gvro.FindControl("hdnFrmDW");
            HiddenField hdnto = (HiddenField)gvro.FindControl("hdnToDW");

            d1 = hdnfrm.Value; d2 = hdnto.Value + " 23:59:59.997";
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string senderid = lblsender.Text;
            string mobno = txtmob.Text.ToString().Trim();
            String STR = "Sp_GetSMSReport_User_ConsolidatedDETAIL";

            if (rdblistselect.SelectedValue == "D")
            {
                for (var day = Convert.ToDateTime(hdnfrm.Value).Date; day.Date <= Convert.ToDateTime(hdnto.Value).Date; day = day.AddDays(1))
                {
                    DateTime fromDate = day;
                    string date = fromDate.ToString("yyyy-MM-dd");

                    SqlParameter[] aarPrm = new SqlParameter[7];

                    aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
                    aarPrm[0].Value = date;

                    aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
                    aarPrm[1].Value = date + " 23:59:59.997";
                    aarPrm[2] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
                    aarPrm[2].Value = senderid;
                    aarPrm[3] = new SqlParameter("@user", SqlDbType.VarChar, 50);
                    aarPrm[3].Value = user;

                    aarPrm[4] = new SqlParameter("@campnm", SqlDbType.VarChar, 500);

                    if (rdbCamp.Checked)
                    {
                        aarPrm[4].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
                    }
                    else
                    {
                        aarPrm[4].Value = "";
                    }
                    aarPrm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
                    aarPrm[5].Value = mobno;

                    aarPrm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 15);
                    aarPrm[6].Value = "";
                    DataTable dt = null;
                    if (rbTdy.Checked)
                        dt = database.GetDataTableSp(aarPrm, STR);
                    if (rbHis.Checked)
                    {
                        aarPrm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                        dt = dbHistorical.GetDataTableSp(aarPrm, STR);
                    }

                    int takeCount = 100000;
                    int fn = 1;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Session["MOBILEDATA"] = dt;
                    }
                }
            }
            else
            {
                DateTime fromDate = Convert.ToDateTime(hdnfrm.Value).Date;
                DateTime ToDate = Convert.ToDateTime(hdnto.Value).Date;
                string FrmDate = fromDate.ToString("yyyy-MM-dd");
                string To_Date = ToDate.ToString("yyyy-MM-dd");

                SqlParameter[] aarPrm = new SqlParameter[7];

                aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
                aarPrm[0].Value = FrmDate;

                aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
                aarPrm[1].Value = To_Date + " 23:59:59.997";
                aarPrm[2] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
                aarPrm[2].Value = senderid;
                aarPrm[3] = new SqlParameter("@user", SqlDbType.VarChar, 50);
                aarPrm[3].Value = user;
                aarPrm[4] = new SqlParameter("@campnm", SqlDbType.VarChar, 500);

                if (rdbCamp.Checked)
                {
                    aarPrm[4].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
                }
                else
                {
                    aarPrm[4].Value = "";
                }
                aarPrm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
                aarPrm[5].Value = mobno;
                aarPrm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 50);
                aarPrm[6].Value = "";
                DataTable dt = null;
                if (rbTdy.Checked)
                    dt = database.GetDataTableSp(aarPrm, STR);
                if (rbHis.Checked)
                {
                    aarPrm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                    dt = dbHistorical.GetDataTableSp(aarPrm, STR);
                }
                int takeCount = 100000;
                int fn = 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Session["MOBILEDATA"] = dt;
                }
            }

            DataTable dt2 = Session["MOBILEDATA"] as DataTable;

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                Session["FILENAME"] = "DeliveryReportDownload.xls";
                Response.Redirect("sms-report_u_download2.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Data Not Found')", true);
                return;
            }
        }

        protected void btnViewFile_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblsender = (Label)gvro.FindControl("lblsender");
            HiddenField hdnfrm = (HiddenField)gvro.FindControl("hdnFrmDW");
            HiddenField hdnto = (HiddenField)gvro.FindControl("hdnToDW");
            d1 = hdnfrm.Value;
            d2 = hdnto.Value + " 23:59:59.997";
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string senderid = lblsender.Text;
            string mobno = txtmob.Text.ToString().Trim();
            String STR = "Sp_GetSMSReport_User_ConsolidatedDETAIL";

            DateTime fromDate = Convert.ToDateTime(hdnfrm.Value).Date;
            DateTime ToDate = Convert.ToDateTime(hdnto.Value).Date;
            string FrmDate = fromDate.ToString("yyyy-MM-dd");
            string To_Date = ToDate.ToString("yyyy-MM-dd");

            SqlParameter[] aarPrm = new SqlParameter[8];

            aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
            aarPrm[0].Value = FrmDate;

            aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
            aarPrm[1].Value = To_Date + " 23:59:59.997";
            aarPrm[2] = new SqlParameter("@sender", SqlDbType.VarChar, 50);
            if (senderid == "NA")
            {
                aarPrm[2].Value = ddlSender.SelectedValue;
            }
            else
            {
                aarPrm[2].Value = senderid;
            }
            aarPrm[3] = new SqlParameter("@user", SqlDbType.VarChar, 50);
            aarPrm[3].Value = user;
            aarPrm[4] = new SqlParameter("@campnm", SqlDbType.VarChar, 500);

            if (rdbCamp.Checked)
            {
                aarPrm[4].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
            }
            else
            {
                aarPrm[4].Value = "";
            }
            aarPrm[5] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
            aarPrm[5].Value = mobno;
            aarPrm[6] = new SqlParameter("@templateid", SqlDbType.VarChar, 50);
            aarPrm[6].Value = "";
            //ADD BY NAVED KHAN
            if (rbDlvr.Checked == true)
            {
                aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                aarPrm[7].Value = "DELIVERED";
            }
            else if (rbFailed.Checked == true)
            {
                aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                aarPrm[7].Value = "FAILED";
            }
            else if (rbSbmtd.Checked == true)
            {
                aarPrm[7] = new SqlParameter("@ReportType", SqlDbType.VarChar, 12);
                aarPrm[7].Value = "";
            }
            DataTable dt = null;
            if (rbTdy.Checked)
            {
                dt = database.GetDataTableSp(aarPrm, STR);
            }
            if (rbHis.Checked)
            {
                aarPrm[6].Value = ddltemplate.SelectedValue == "-1" ? "" : ddltemplate.SelectedValue;
                dt = dbHistorical.GetDataTableSp(aarPrm, STR);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Session["MOBILEDATA"] = dt;
            }

            DataTable dt2 = Session["MOBILEDATA"] as DataTable;

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                grvModal.DataSource = null;
                grvModal.DataSource = dt2;
                grvModal.DataBind();
                GridFormat3(dt2);
                pnlPopUp_Detail_ModalPopupExtender.Show();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Data Not Found')", true);
                return;

            }
        }

        protected void GridFormat3(DataTable dt)
        {
            grvModal.UseAccessibleHeader = true;
            grvModal.HeaderRow.TableSection = TableRowSection.TableHeader;
            if (grvModal.TopPagerRow != null)
            {
                grvModal.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvModal.BottomPagerRow != null)
            {
                grvModal.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grvModal.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void grv2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnViewFile = (LinkButton)e.Row.FindControl("btnViewFile");
                string mobno = txtmob.Text.ToString().Trim();
                if (mobno == "")
                    btnViewFile.Visible = false;
                else
                    btnViewFile.Visible = true;
            }
        }

        //--------------------------END Working Code -------------------------------------------------//
        //Create Function to Bind a Template in DropDownlist//Abhishek Bhaiya G 13-06-2023
        public void BindTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTempIdAndName(Convert.ToString(Session["UserID"]));
            if (dt.Rows.Count == 0)
            {
                ddltemplate.Items.Clear();
                return;
            }

            ddltemplate.DataSource = dt;
            ddltemplate.DataTextField = "tempname";
            ddltemplate.DataValueField = "templateid";
            ddltemplate.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddltemplate.Items.Insert(0, objListItem);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.filterTemplatepopup(Convert.ToString(Session["UserID"]), "", ""); ;
            if (dt.Rows.Count > 0)
            {
                TEMPLATE_FIND.DataSource = dt;
                TEMPLATE_FIND.DataBind();
            }
            else
            {
                TEMPLATE_FIND.DataSource = null;
                TEMPLATE_FIND.DataBind();
            }
            modal_Find_TEMPLATE.Show();
        }

        protected void Filtertemp_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.filterTemplatepopup(Convert.ToString(Session["UserID"]), txtTemplateId.Text.ToString(), txtTempName.Text.ToString()); ;
            if (dt.Rows.Count > 0)
            {
                TEMPLATE_FIND.DataSource = dt;
                TEMPLATE_FIND.DataBind();
            }
            else
            {
                TEMPLATE_FIND.DataSource = null;
                TEMPLATE_FIND.DataBind();
            }
            modal_Find_TEMPLATE.Show();
            txtTemplateId.Text = "";
            txtTempName.Text = "";
        }

        protected void btn_Viewrecord_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int indexrow = gvr.RowIndex;
            string tempid = Convert.ToString((TEMPLATE_FIND.Rows[indexrow].FindControl("HD_templateid") as HiddenField).Value);
            ddltemplate.SelectedValue = tempid;
            modal_Find_TEMPLATE.Hide();
        }
    }
}