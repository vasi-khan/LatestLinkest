using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.SqlClient;
using eMIMPanel.Helper;
using System.IO;

namespace eMIMPanel
{
    public partial class DaySummaryReport : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        int IsDND = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            string empcode = Convert.ToString(Session["EMPCODE"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                BindYear();
                //Helper.Util ob = new Helper.Util();
                //DataTable dt = ob.GetSMSReport("02/JAN/1900", "02/JAN/1900", usertype, user, empcode);
                //grv.DataSource = null;
                //grv.DataSource = dt;
                //grv.DataBind();
                //GridFormat(dt);
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;
            try
            {
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

        protected void BindYear()
        {
            ListItem objListItem = new ListItem("---Select Year--", "0");
            ddlYear.Items.Insert(0, objListItem);

            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                ddlYear.Items.Add(i.ToString());
            }
        }

        public void GetData()
        {
            DataTable dt = null;
            if (ddlFromMonth.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select From Month !!');", true);
                return;
            }
            if (ddlToMonth.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select To Month !!');", true);
                return;
            }

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            string empcode = Convert.ToString(Session["EMPCODE"]);
            IsDND = 0;
            if (ChkDND.Checked)
            {
                IsDND = 1;
                grv.Columns[7].Visible = true;
            }
            if (ChkGroupOnDlt.Checked)
            {
                dt = ob.GetGroupOnDLTNO(Convert.ToString(ddlFromMonth.SelectedValue.Trim()), Convert.ToString(ddlToMonth.SelectedValue.Trim()), Convert.ToString(ddlYear.SelectedValue.Trim()), Convert.ToString(txtUserID.Text.Trim()), Convert.ToString(txtDltno.Text.Trim()), IsDND);
            }
            else
            {
                dt = GetDaysummaryreport(Convert.ToString(ddlFromMonth.SelectedValue.Trim()), Convert.ToString(ddlToMonth.SelectedValue.Trim()), Convert.ToString(ddlYear.SelectedValue.Trim()), Convert.ToString(txtUserID.Text.Trim()), Convert.ToString(txtDltno.Text.Trim()), IsDND);
            }
            //DataTable dt = ob.GetDaysummaryreport(Convert.ToString(ddlFromMonth.SelectedValue.Trim()),Convert.ToString(ddlFromMonth.SelectedValue.Trim()),Convert.ToString(ddlYear.SelectedValue.Trim()),Convert.ToString(txtUserID.Text.Trim()), Convert.ToString(txtDltno.Text.Trim()));
            //DataTable dt = GetDaysummaryreport(Convert.ToString(ddlFromMonth.SelectedValue.Trim()), Convert.ToString(ddlToMonth.SelectedValue.Trim()), Convert.ToString(ddlYear.SelectedValue.Trim()), Convert.ToString(txtUserID.Text.Trim()),Convert.ToString(txtDltno.Text.Trim()),IsDND);
            if (dt != null && dt.Rows.Count > 0)
            {
                grv.DataSource = null;
                grv.DataSource = dt;
                SetFooterValue(dt);
                grv.DataBind();
                GridFormat(dt);
                lnkDownload.Visible = true;
            }
            else
            {

                grv.DataSource = null;
                grv.DataBind();
                lnkDownload.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Data Not Found !!');", true);
                return;
            }
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
            //modalpopuppwd.Hide();
            GetData();
        }
        //protected void btnView_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Get the button that raised the event
        //        LinkButton btn = (LinkButton)sender;

        //        //Get the row that contains this button
        //        GridViewRow gvro = (GridViewRow)btn.NamingContainer;
        //        Label l = (Label)gvro.FindControl("lblUserId");
        //        Label l2 = (Label)gvro.FindControl("lblsender");

        //        if (hdntxtFrm.Value.Trim() == "")
        //        {
        //            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
        //        }
        //        else
        //        {
        //            s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
        //        }
        //        Helper.Util ob = new Helper.Util();
        //        Response.Redirect("sms-reports_download.aspx?x=" + l.Text + "$" + l2.Text + "$" + s1 + "$" + s2);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
        //        return;
        //    }
        //}

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            //object sumUnknown;
            //sumUnknown = copyDataTable.Compute("Sum(Unknown)", string.Empty);

            grv.Columns[3].FooterText = "Total : ";
            grv.Columns[4].FooterText = sumSubmitted.ToString();
            grv.Columns[5].FooterText = sumDelivered.ToString();
            grv.Columns[6].FooterText = sumFailed.ToString();
            //grv.Columns[7].FooterText = sumUnknown.ToString();

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        private void ExportToExcel()
        {
            string attachment = "attachment; filename=DaySummaryReport.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grv.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public DataTable GetDaysummaryreport(string FromMonth, string ToMonth, string Year, string Userid, string dltno, int DNDorNot = 0)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GenerateFilterWiseReports1";
                cmd.Parameters.AddWithValue("FromMonth", FromMonth);
                cmd.Parameters.AddWithValue("ToMonth", ToMonth);
                cmd.Parameters.AddWithValue("Year", Year);
                cmd.Parameters.AddWithValue("UserId", Userid);
                cmd.Parameters.AddWithValue("DltNo", dltno);
                cmd.Parameters.AddWithValue("@DNDorNOT", DNDorNot);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
            }
            return dt;
        }
    }
}