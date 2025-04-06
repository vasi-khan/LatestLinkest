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
using System.Text;

namespace eMIMPanel
{
    public partial class sms_reports_usr_mobileno_wise : System.Web.UI.Page
    {
        public void SMSFields()
        {
            string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetSMSSummary(s1, s2, usertype, user);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
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
        }


        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            //modalpopuppwd.Hide();
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

            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");

            //SMSFields();
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);

            }
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        protected void rbTdy_CheckedChanged(object sender, EventArgs e)
        {
            DivDateForm.Visible = false;
            DivDateTo.Visible = false;
            hdntxtFrm1.Value = "";
            hdntxtTo1.Value = "";
        }

        protected void rbHis_CheckedChanged(object sender, EventArgs e)
        {
            DivDateForm.Visible = true;
            DivDateTo.Visible = true;
            hdntxtFrm1.Value = "";
            hdntxtTo1.Value = "";
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string mobno = txtmob.Text.ToString().Trim();
            String STR = "Sp_GetSMSReport_user_Details_MobNo_Wise";
            if (rbTdy.Checked)
            {
                hdntxtFrm1.Value = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                hdntxtTo1.Value = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            DateTime fromDate = Convert.ToDateTime(hdntxtFrm1.Value).Date;
            DateTime ToDate = Convert.ToDateTime(hdntxtTo1.Value).Date;
            string FrmDate = fromDate.ToString("yyyy-MM-dd");
            string To_Date = ToDate.ToString("yyyy-MM-dd");
            if (rbHis.Checked)
            {
                if (hdntxtFrm1.Value == "" && hdntxtTo1.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Date');", true);
                    return;
                }
                else if (hdntxtFrm1.Value != "" && hdntxtTo1.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select To Date');", true);
                    return;
                }
                else if (hdntxtFrm1.Value == "" && hdntxtTo1.Value != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select From Date');", true);
                    return;
                }
            }
            if (mobno == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile No');", true);
                return;
            }
            int moblenght = mobno.Trim().Length;
            string CountryCode = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
            if (CountryCode == "91")
            {
                if (moblenght < 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile No Minimum 10 Digits');", true);
                    return;
                }
            }
            else
            {
                if (moblenght < 7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile No Minimum 7 Digits');", true);
                    return;
                }
            }
            SqlParameter[] aarPrm = new SqlParameter[4];
            aarPrm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
            aarPrm[0].Value = FrmDate;
            aarPrm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
            aarPrm[1].Value = To_Date + " 23:59:59.997";
            aarPrm[2] = new SqlParameter("@user", SqlDbType.VarChar, 50);
            aarPrm[2].Value = user;
            aarPrm[3] = new SqlParameter("@mobno", SqlDbType.VarChar, 15);
            aarPrm[3].Value = mobno;
            DataTable dt = null;
            if (rbTdy.Checked)
                dt = database.GetDataTableSp(aarPrm, STR);
            if (rbHis.Checked)
                dt = dbHistorical.GetDataTableSp(aarPrm, STR);

            if (dt != null && dt.Rows.Count > 0)
            {
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat3(dt);
                Session["analyticsdata"] = dt;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Data Not Found')", true);
                return;
            }
        }

        protected void GridFormat3(DataTable dt)
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


        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((Session["analyticsdata"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "SMSDATE", "Messageid", "MobileNo", "Sender", "SentDate", "DeliveredDate", "Message", "MessageState", "RESPONSE");
                DatatableToCSV(distinctValues, "ReportsMobileNoWise.csv");

            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        private void DatatableToCSV(DataTable dt, string FileName)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName.Replace(".xls", ".csv"));
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder columnbind = new StringBuilder();
                for (int k = 0; k < (dt).Columns.Count; k++)
                {
                    columnbind.Append(dt.Columns[k].ColumnName + ',');
                }

                columnbind.Append("\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                    }
                    columnbind.Append("\r\n");
                }

                DataTable FDT = Session["FottorValue"] as DataTable;
                if (FDT != null && FDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in FDT.Rows)
                    {
                        for (int k = 0; k < FDT.Columns.Count; k++)
                        {
                            columnbind.Append(dr[k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }
                    }
                    columnbind.Append("\r\n");

                }

                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }
        //--------------------------END Working Code -------------------------------------------------//
    }
}