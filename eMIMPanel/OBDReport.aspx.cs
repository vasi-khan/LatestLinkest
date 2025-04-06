using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using eMIMPanel.Helper;
using System.IO;
using System.Drawing;

namespace eMIMPanel
{
    public partial class OBDReport : System.Web.UI.Page
    {
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

        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            if ((hdntxtFrm1.Value) == "" && (hdntxtTo1.Value) == "")
            {
                d1 = Convert.ToDateTime(DateTime.Now.Date, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                d2 = Convert.ToDateTime(DateTime.Now.Date, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            else if ((hdntxtFrm1.Value) != "" && (hdntxtTo1.Value) == "")
            {
                d1 = Convert.ToDateTime(hdntxtFrm1.Value).ToString("yyyy-MM-dd");
                d2 = Convert.ToDateTime(DateTime.Now.Date, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            else if ((hdntxtFrm1.Value) == "" && (hdntxtTo1.Value) != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select Date From')", true);
                return;
            }
            else
            {
                d1 = Convert.ToDateTime(hdntxtFrm1.Value).ToString("yyyy-MM-dd");
                d2 = Convert.ToDateTime(hdntxtTo1.Value).ToString("yyyy-MM-dd");
            }
            string proc = "SP_GetOBDReportData";
            List<SqlParameter> pram = new List<SqlParameter>();
            pram.Clear();
            pram.Add(new SqlParameter("@ProfileId", user));
            pram.Add(new SqlParameter("@fromdate", Convert.ToDateTime(d1)));
            pram.Add(new SqlParameter("@todate", Convert.ToDateTime(d2)));
            DataTable dt = ob.GetRecord(proc, pram);
            ViewState["grvDLT"] = dt;
            SetFooterValue(dt);
            GridViewbind(dt);

        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumOBDCount;
            sumOBDCount = copyDataTable.Compute("Sum(OBDCount)", string.Empty);

            grv2.Columns[1].FooterText = "Total : ";
            grv2.Columns[2].FooterText = sumOBDCount.ToString();
        }

        public void GridViewbind(DataTable dlt)
        {
            if (dlt != null && dlt.Rows.Count > 0)
            {
                grv2.DataSource = dlt;
                grv2.DataBind();
            }
            else
            {
                grv2.DataSource = null;
                grv2.DataBind();
            }
            GridFormat(dlt);
        }

        private void ExportToExcel()
        {

            string attachment = "attachment; filename=OBDReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.ContentType.ToString();
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                grv2.HeaderRow.BackColor = Color.White;
                for (int i = 0; i < grv2.HeaderRow.Cells.Count; i++)
                {
                    grv2.HeaderRow.Cells[i].Style.Add("background-color", "#eab012");
                    grv2.HeaderRow.Cells[i].Style.Add(" ForeColor", "#ffffff");

                }
                for (int i = 0; i < grv2.Rows.Count; i++)
                {

                    grv2.Rows[i].Cells[1].Style.Add("mso-number-format", "0");
                    grv2.Rows[i].Cells[2].Style.Add("mso-number-format", "0");

                }

                grv2.RenderControl(hw);

                Response.ContentType = "application/text";

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                Response.Clear();
            }


        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (grv2.Rows.Count > 0)
            {
                ExportToExcel();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);

        }

        protected void GridFormat(DataTable dt)
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
    }
}