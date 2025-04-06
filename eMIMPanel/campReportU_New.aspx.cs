using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class campReportU_New : System.Web.UI.Page
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
                PopulateCampaign();
            }
        }
        protected void lnkShow_Click(object sender, EventArgs e)
        {
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "loadscrq();", true);

        }

        public void GetData()
        { 
            string user = Convert.ToString(Session["UserID"]);

            if (rbTdy.Checked)
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            if (rbHis.Checked)
            {
                if (hdntxtFrm.Value == "" || hdntxtTo.Value == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select From and To date');", true);
                    return;
                }

                //if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
                //{
                //    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.997";
                //}
                //else if (hdntxtFrm.Value.Trim() == "" && hdntxtTo.Value.Trim() != "")
                //{
                //    s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //    s2 = s1;
                //}
                //else if (hdntxtFrm.Value.Trim() != "" && hdntxtTo.Value.Trim() == "")
                //{
                //    s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); ;
                //}
                //else
                //{
                //    s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //    s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.997";
                //}

                if (Convert.ToString(Session["UserID"]).ToUpper() == "MIM2002078")
                {
                    if (Convert.ToDateTime(s1) < Convert.ToDateTime("2021-06-01"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Access Denied.');", true);
                        return;
                    }
                }

                if (Convert.ToDateTime(DateTime.Now.Date) == Convert.ToDateTime(hdntxtFrm.Value) || Convert.ToDateTime(DateTime.Now.Date) == Convert.ToDateTime(hdntxtTo.Value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Today Option');", true);
                    return;
                }

                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }

            DataTable dt = new DataTable();

            string strpName = "SP_GetcampaigrptWorking";
            SqlParameter[] arrprm = new SqlParameter[4];

            arrprm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
            arrprm[0].Value = s1;
            arrprm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
            arrprm[1].Value = s2;
            arrprm[2] = new SqlParameter("@usr", SqlDbType.VarChar, 50);
            arrprm[2].Value = user;
            arrprm[3] = new SqlParameter("@camp", SqlDbType.VarChar, 50);
            arrprm[3].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
            dt = database.GetDataTableSp(arrprm, strpName);  

            SetValueInHtmlTable(dt);
            txtFrm.Text = s1;
            txtTo.Text = s2;
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {


            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr>");
            htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Request <br>Date</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign /<br> File Name</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SMS Credit</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Submited</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Awaited</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Hit Count</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:7%;\" rowspan=\"2\">Sender ID</th>");
            //htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > SMS Text </th>");
            htmlTable.Append("</tr>");
            htmlTable.Append("<tr>");
            htmlTable.Append("<th> Numbers </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("</tr>");
            htmlTable.Append("</thead>");
            htmlTable.Append("<tbody>");

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                htmlTable.Append("<tr class=\"tr-1\">");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + Convert.ToString(j + 1) + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["requestdate"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "<br>" + dt.Rows[j]["FILENM"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["credit"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["smscount"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["sender"] + "</td>");
                // htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");
            return Convert.ToString(htmlTable);
        }
        public void SetValueInHtmlTable(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
                htmlTable.Append("<thead>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Request <br>Date</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign /<br> File Name</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SMS Credit</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Submited</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Awaited</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Hit Count</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:7%;\" rowspan=\"2\">Sender ID</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > SMS Text </th>");
                htmlTable.Append("</tr>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th> Numbers </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("</tr>");
                htmlTable.Append("</thead>");
                htmlTable.Append("<tbody>");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    htmlTable.Append("<tr class=\"tr-1\">");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + Convert.ToString(j + 1) + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["requestdate"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "<br>" + dt.Rows[j]["FILENM"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["credit"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["smscount"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["sender"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                    htmlTable.Append("</tr>");

                    //DataView dataView = dt.DefaultView;
                    //dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND requestdate='" + dt.Rows[j]["requestdate"] + "'";
                    //string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                    //htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                    //htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>"); 
                   // htmlTable.Append("</tr>");

                }
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
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

        private void DatatableToExcel(DataTable dt)
        {
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt, "Customers");

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=CampaignReport.xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            string attachment = "attachment; filename=CampaignReport.xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            string tab = "";

            dt.Columns["RequestDate"].ColumnName = "Request Date";
            dt.Columns["FILENM"].ColumnName = "File";
            dt.Columns["Campaign"].ColumnName = "Campaign";
            dt.Columns["smscount"].ColumnName = "Submitted";
            dt.Columns["credit"].ColumnName = "SMS Credit";
            dt.Columns["delivered"].ColumnName = "Delivered(No)";
            dt.Columns["delivered_p"].ColumnName = "Delivered(%)";
            dt.Columns["failed"].ColumnName = "Failed(No)";
            dt.Columns["failed_p"].ColumnName = "Failed(%)";
            dt.Columns["AWAITED"].ColumnName = "Awaited(No)";
            dt.Columns["AWAITED_p"].ColumnName = "Awaited(%)";
            dt.Columns["openmsg"].ColumnName = "Hit Count(No)";
            dt.Columns["openmsg_p"].ColumnName = "Hit Count(%)";
            dt.Columns["sender"].ColumnName = "Sender ID";
            //dt.Columns["message"].ColumnName = "SMS Text";

            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    //HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    HttpContext.Current.Response.Write(tab + dr[i].ToString().Replace('\n', ' ').Replace(Convert.ToChar(10), ' ').Replace(Convert.ToChar(13), ' '));

                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            // Response.End();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (hdntxtFrm.Value == "" || hdntxtTo.Value == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select From and To date');", true);
                return;
            }

            if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            else if (hdntxtFrm.Value.Trim() == "" && hdntxtTo.Value.Trim() != "")
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = s1;
            }
            else if (hdntxtFrm.Value.Trim() != "" && hdntxtTo.Value.Trim() == "")
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); ;
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }

            if (Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From and To date should be less than Today date');", true);
                return;
            }

            string user = Convert.ToString(Session["UserID"]);
            string camp = ddlCamp.SelectedValue; // txtCamp.Text.Trim();

            //  DataSet ds = ob.GetCampaignWiseData_new(user, s1, s2, camp);


            //DateTime FromDate = new DateTime();
            //DateTime ToDate = new DateTime();
            //if (hdntxtFrm.Value.Trim() == "")
            //{
            //    FromDate = DateTime.Now;
            //}
            //else
            //{
            //    FromDate = ob.Setdate(hdntxtFrm.Value.Trim());
            //}
            //if (hdntxtTo.Value.Trim() == "")
            //{
            //    ToDate = DateTime.Now;
            //}
            //else
            //{
            //    ToDate = ob.Setdate(hdntxtTo.Value.Trim());
            //}

            //DataSet ds = ob.SP_GetCampaignWiseDatap(user, FromDate, ToDate, camp);
            DataSet ds = ob.SP_GetCampaignWiseDatap(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DatatableToExcel(dt);
            }

        }

        protected void rbTdy_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-none");
        }

        protected void rbHis_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-block");
        }



    }
}