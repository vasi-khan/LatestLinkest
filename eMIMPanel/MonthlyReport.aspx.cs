using Shortnr.Web.Business.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class MonthlyReport : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string s3 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            if (!IsPostBack)
            {
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                DataTable dt = ob.GetUserReport(user, ws, "02/JAN/1900", "02/JAN/1900", false);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);

                dt = ob.GetUserReportDetail(user, "-22", false);
                grv2.DataSource = null;
                grv2.DataSource = dt;
                grv2.DataBind();
                GridFormat2(dt);
                divOther.Visible = false;
            }
        }

        protected void rbHonda_CheckedChanged(object sender, EventArgs e)
        {
            txtDltNO.Text = "";
            divOther.Visible = false;
            grv.DataSource = null;
            DataTable dt = ob.GetUserReport(user, "", "02/JAN/1900", "02/JAN/1900", false);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }

        protected void rbOthers_CheckedChanged(object sender, EventArgs e)
        {
            txtDltNO.Text = "";
            divOther.Visible = true;
            grv.DataSource = null;
            DataTable dt = ob.GetUserReport(user, "", "02/JAN/1900", "02/JAN/1900", false);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
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
                s3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
                s3 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            string _user = Convert.ToString(Session["UserID"]);
            DataTable dt = new DataTable();
            if (rbHonda.Checked)
            {
                dt = ob.GetDayWiseMonthlySummary(s1, s2, s3, "", 1);
            }
            else
            {
                dt = ob.GetDayWiseMonthlySummary(s1, s2, s3, "", 2);
            }
            ViewState["dt"] = dt;
            grv.DataSource = null;
            grv.DataSource = dt;
            SetFooterValue(dt);
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

        protected void GridFormat2(DataTable dt)
        {
            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv2.TopPagerRow != null)
            {
                grv2.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv2.BottomPagerRow != null)
            {
                grv2.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv2.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblUserId = (Label)gvro.FindControl("lblUserId");
            Label lbldate = (Label)gvro.FindControl("lblDate");
            HiddenField hdndate = (HiddenField)gvro.FindControl("hdndate");

            DataTable dt = ob.GetDayWiseSMSSummaryDetail(lblUserId.Text, lbldate.Text);
            //Session["rptDetail"] = dt;
            grv2.DataSource = null;
            grv2.DataSource = dt;
            grv2.DataBind();
            GridFormat2(dt);
            pnlPopUp_Detail_ModalPopupExtender.Show();
        }

        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["analyticsdata"];
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
        }
     
        private void ExportToExcel()
        {
            string attachment = "attachment; filename=SMSSummaryReport.xls";
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
                        string RowText = dt.Rows[i][k].ToString().Replace(",", " ");
                        columnbind.Append(RowText.ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
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

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((ViewState["dt"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "userid", "COMPNAME", "FULLNAME", "FromDATE", "ToDATE", "Submitted", "Delivered", "Failed");
                DatatableToCSV(distinctValues, "MonthlyReport.csv");
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] {
                new DataColumn("1"),
                new DataColumn("2"),
                new DataColumn("3"),
                new DataColumn("4"),
                new DataColumn("5"),
                new DataColumn("6"),
                new DataColumn("7"),
                new DataColumn("8"),
            });
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            grv.Columns[5].FooterText = "Total : ";
            grv.Columns[6].FooterText = sumSubmitted.ToString();
            grv.Columns[7].FooterText = sumDelivered.ToString();
            grv.Columns[8].FooterText = sumFailed.ToString();
            dt.Rows.Add("", "", "", "", "Total ", sumSubmitted.ToString(), sumDelivered.ToString(), sumFailed.ToString());
            dt.AcceptChanges();
            Session["FottorValue"] = dt;
        }
    }
}