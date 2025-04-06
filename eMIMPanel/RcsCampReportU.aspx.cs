using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace eMIMPanel
{
    public partial class RcsCampReportU : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        rcscode.UtilN ob = new rcscode.UtilN();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            Session["RCSUserID"] = Convert.ToString(rcscode.database.GetScalarValue("select top 1 RCSACCID  from MapSMSAcc where smsAccId='" + Convert.ToString(Session["UserID"]) + "'"));

            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["RCSUserID"]);
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
            if (hdntxtFrm.Value == "" || hdntxtTo.Value == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select From and To date');", true);
                return;
            }

            if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.997";
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
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.997";
            }

            //if (Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))if (Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))
            //if (Convert.ToDateTime(s1, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(s2, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From and To date should be less than Today date');", true);
            //    return;
            //}
            string user = Convert.ToString(Session["RCSUserID"]);
            string camp = "";
            if (ddlCamp.SelectedIndex > 0)
            {
                camp = ddlCamp.SelectedItem.Text;
            }


            if (Convert.ToString(Session["RCSUserID"]).ToUpper() == "MIM2002078")
            {
                if (Convert.ToDateTime(s1) < Convert.ToDateTime("2021-06-01"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Access Denied.');", true);
                    return;
                }
            }
            DataSet ds = new DataSet();
            ds = ob.GetCampaignWiseDatap(user, s1, s2, camp, usertype);

            SetValueInHtmlTable(ds);
            txtFrm.Text = s1;
            txtTo.Text = s2;
        }

        public static string ConvertDataTableToHTML_old(DataTable dt)
        {
            string html = "<table class=\"table table table - borderless table - striped\">";
            //add header row
            html += "<tbody>";
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            return html;
        }

        public void SetValueInHtmlTable_old(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table id=\"table\" class=\"table table - borderless table - striped\" data-locale=\"en - US\" data-toggle=\"table\" data - toolbar = \"#toolbar\" data - search = \"true\" data - filter - control = \"true\" data - show - columns = \"true\" data - click - to - select = \"true\" data - minimum - count - columns = \"2\" data - pagination = \"true\" data - id - field = \"id\" data - buttons -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");

                htmlTable.Append("<thead>");
                htmlTable.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\">" + dt.Columns[i].ColumnName + "</th>");
                }
                htmlTable.Append("</tr>");
                htmlTable.Append("</thead>");
                htmlTable.Append("<tbody>");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    htmlTable.Append("<tr class=\"tr-1\">");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["requestdate"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["FILENM"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["sender"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["credit"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["smscount"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent1\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg"] + "</td>");
                    htmlTable.Append("</tr>");


                    DataView dataView = dt1.DefaultView;
                    dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "'";
                    string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                    htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent1\" aria-expanded=\"true\">");
                    htmlTable.Append("<td class=\"inside-table\" colspan=\"12\">" + subHtml + "</td>");
                    // htmlTable.Append( subHtml );

                    htmlTable.Append("</tr>");

                }
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
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
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Msg Credit</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Submited</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
            //htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Awaited</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Rejected Count</th>");
            //htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:7%;\" rowspan=\"2\">Sender ID</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > Msg Text </th>");
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
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["CreatedDate"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "<br>" + dt.Rows[j]["FILENM"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["Sms_credit"] + "</td>");
                //htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["smscount"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DELIVERED"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["FAIL"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed_p"] + "</td>");
                //htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED"] + "</td>");
                //htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED_p"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["REJECTED"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["Rejected_p"] + "</td>");
                //htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["sender"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");
            return Convert.ToString(htmlTable);
        }
        public void SetValueInHtmlTable(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                // DataTable dt1 = ds.Tables[0];

                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
                htmlTable.Append("<thead>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Request <br>Date</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign /<br> File Name</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Msg Credit</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Submited</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Rejected</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Unknown</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Seen Count</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > Msg Text </th>");
                htmlTable.Append("</tr>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th> Numbers </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                //htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("</tr>");
                htmlTable.Append("</thead>");
                htmlTable.Append("<tbody>");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    htmlTable.Append("<tr class=\"tr-1\">");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + Convert.ToString(j + 1) + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["CreatedDate"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "<br>" + dt.Rows[j]["FILENM"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["Sms_credit"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["SUBMITTED"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DELIVERED"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["FAIL"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["REJECTED"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["Rejected_p"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["unknown"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["Seen"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                    htmlTable.Append("</tr>");

                    //DataView dataView = dt1.DefaultView;
                    //dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND CreatedDate='" + dt.Rows[j]["CreatedDate"] + "'";
                    //string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                    //htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                    //htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>");
                    //htmlTable.Append( subHtml );

                    htmlTable.Append("</tr>");

                }
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
        }

        protected void GridFormat(DataTable dt)
        {
            //grv.UseAccessibleHeader = true;
            //grv.HeaderRow.TableSection = TableRowSection.TableHeader;

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


        public void PopulateCampaign()
        {
            rcscode.UtilN ob = new rcscode.UtilN();
            DataTable dt = ob.GetCampaignAllUser(Convert.ToString(Session["RCSUserID"]));

            ddlCamp.DataSource = dt;
            ddlCamp.DataTextField = "campaign";
            ddlCamp.DataValueField = "campaign";
            ddlCamp.DataBind();
            ListItem objListItem = new ListItem("--Campaign Name--", "0");
            ddlCamp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlCamp.SelectedIndex = 1;
            else
                ddlCamp.SelectedIndex = 0;
        }

        private void DatatableToExcel(DataTable dt)
        {
            string attachment = "attachment; filename=RcsCampaignReportU.xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            string tab = "";

            dt.Columns["CreatedDate"].ColumnName = "Request Date";
            dt.Columns["FILENM"].ColumnName = "File";
            dt.Columns["Campaign"].ColumnName = "Campaign";
            dt.Columns["Sms_credit"].ColumnName = "Msg Credit";
            dt.Columns["SUBMITTED"].ColumnName = "Submitted";
            dt.Columns["DELIVERED"].ColumnName = "Delivered(No)";
            dt.Columns["delivered_p"].ColumnName = "Delivered(%)";
            dt.Columns["FAIL"].ColumnName = "Failed(No)";
            dt.Columns["failed_p"].ColumnName = "Failed(%)";
            dt.Columns["REJECTED"].ColumnName = "Rejected(No)";
            dt.Columns["Rejected_p"].ColumnName = "Rejected(%)";
            dt.Columns["unknown"].ColumnName = "Unknown";
            dt.Columns["Seen"].ColumnName = "Seen Count";
            dt.Columns["message"].ColumnName = "Msg Text";

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
            usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["RCSUserID"]);
            string camp = "";
            if (ddlCamp.SelectedIndex > 0)
            {
                camp = ddlCamp.SelectedItem.Text;
            }
            DataSet ds = ob.GetCampaignWiseDatap(user, s1, s2, camp, usertype);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DatatableToExcel(dt);
            }

        }
    }
}