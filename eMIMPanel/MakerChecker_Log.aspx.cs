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
    public partial class MakerChecker_Log : System.Web.UI.Page
    {
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
            if (!IsPostBack)
            {

            }
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
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            Helper.Util ob = new Helper.Util();
            string _user = Convert.ToString(Session["UserID"]); 
            DataTable dt = ob.GetMakerCheckerDetails(_user, s1, s2);
            ViewState["dt"] = dt;
            grv.DataSource = null;
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
                dt.Columns["campname"].ColumnName = "Campaign Name";
                dt.Columns["CampStartDate"].ColumnName = "Campaign Start Date";
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

                //Abhishek 09-02-2023 S
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
                //Abhishek 09-02-2023 E
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
            try
            {
                DataTable dt = ViewState["dt"] as DataTable;
                if (ViewState["dt"] != null)
                {
                    DatatableToCSV(dt, "MakerCheckerReport.csv");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data Not Found !');", true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            //abhishek 09-02-2023 S
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
            //abhishek 09-02-2023 E
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(Unknown)", string.Empty);

            grv.Columns[4].FooterText = "Total : ";
            grv.Columns[5].FooterText = sumSubmitted.ToString();
            grv.Columns[6].FooterText = sumDelivered.ToString();
            grv.Columns[7].FooterText = sumFailed.ToString();
            grv.Columns[8].FooterText = sumUnknown.ToString();
            dt.Rows.Add("", "", "Total ", sumSubmitted.ToString(), sumDelivered.ToString(), sumFailed.ToString(), sumUnknown.ToString());
            dt.AcceptChanges();
            Session["FottorValue"] = dt;
        }
    }
}