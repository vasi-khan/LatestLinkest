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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class RichMediaButtonClickReport : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {

            }
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            string f = hdntxtFrm.Value;
            string t = hdntxtTo.Value;
            DataTable dt = new DataTable();
            string sql = "";

            if (f != "" && t != "")
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                sql = @"SELECT left(domainname+segment+replicate('_',50),31) + left('{' +  ISNULL(urlname,'') + replicate('_',50),21) + '}  
                         [ ' + CONVERT(varchar,added,102) + ' ]' AS shorturl,RichMediaGroupRows AS id 
                       FROM short_urls WITH(NOLOCK) 
                       WHERE userid='" + user + "' AND CONVERT(date,added) between CONVERT(date,'" + s1 + "') AND CONVERT(date,'" + s2 + @"') AND richmediaurl = 1 
                       AND ISNULL(RichMediaGroupRows,'')<>'' ORDER BY added DESC";
                dt = ob.GetRecordDataTableSql(sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlShortUrl.ClearSelection();
                        ddlShortUrl.DataSource = dt;
                        ddlShortUrl.DataValueField = "id";
                        ddlShortUrl.DataTextField = "shorturl";
                        ddlShortUrl.DataBind();
                        ddlShortUrl.Items.Insert(0, new ListItem("--ALL--", "-1"));
                    }
                    else
                    {
                        ddlShortUrl.DataSource = dt;
                        ddlShortUrl.DataValueField = "id";
                        ddlShortUrl.DataTextField = "shorturl";
                        ddlShortUrl.DataBind();
                    }
                    SetDropDownListItemColor();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select Data');", true);
                return;
            }
        }

        public void SetDropDownListItemColor()
        {
            foreach (ListItem item in ddlShortUrl.Items)
            {
                item.Attributes.CssStyle.Add("font-family", "Consolas");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            collapseOne.Attributes.Add("class", "collapse show");
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

            string RichMediaShortURL = Convert.ToString(ddlShortUrl.SelectedValue);
            if (RichMediaShortURL == "0" || RichMediaShortURL == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select RichMedia Shorts Url ID !!')", true);
                return;
            }

            Helper.Util ob = new Helper.Util();
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetRichMediaButtonDetails(user, RichMediaShortURL);
            HttpContext.Current.Session["ShowSummary"] = dt;
            SetValueInHtmlTable(dt);
        }


        public void SetValueInHtmlTable(DataTable dt)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr>");
            htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Short URL</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Long URl</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">URL Send</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">URL Hits</th>");

            for (int i = 1; i < dt.Rows.Count; i++)
            {
                string ButtonName = Convert.ToString(dt.Rows[i]["ButtonName"]);
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">" + ButtonName + " (Button)</th>");
            }

            htmlTable.Append("</tr>");
            htmlTable.Append("</thead>");
            htmlTable.Append("<tbody>");

            htmlTable.Append("<tr class=\"tr-1\">");
            htmlTable.Append("<td>" + Convert.ToString(1) + "</td>");

            htmlTable.Append("<td>" + Convert.ToString(dt.Rows[0]["ShortsURL"]) + "</td>");
            htmlTable.Append("<td>" + Convert.ToString(dt.Rows[0]["LongURL"]) + "</td>");
            htmlTable.Append("<td>" + Convert.ToString(dt.Rows[0]["NoOfCount"]) + "</td>");
            htmlTable.Append("<td><a onclick=\"ShowDetails('" + Convert.ToString(dt.Rows[0]["ButtonName"]) + "','" + Convert.ToString(dt.Rows[0]["RichMediaGroupRows"]) + "')\" style='color: blue; '>" + Convert.ToString(dt.Rows[0]["NoOfHits"]) + "</a></td>");
            for (int j = 1; j < dt.Rows.Count; j++)
            {
                htmlTable.Append("<td><a onclick=\"ShowDetails('" + Convert.ToString(dt.Rows[j]["ButtonName"]) + "','" + Convert.ToString(dt.Rows[j]["ID"]) + "')\" style='color: blue; '>" + Convert.ToString(dt.Rows[j]["NoOfHits"]) + "</a></td>");
            }
            htmlTable.Append("</tr>");
            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");
            divResult.InnerHtml = htmlTable.ToString();
        }

        [WebMethod]
        public static string ShowDetails(string ButtonName, string UId)
        {
            try
            {
                Helper.Util ob = new Helper.Util();
                string user = Convert.ToString(HttpContext.Current.Session["UserID"]);
                DataTable dt = ob.GetRichMediaButtonDetails(user, "", ButtonName, UId, 1);
                HttpContext.Current.Session["ShowDetails"] = dt;
                return RichMediaButtonDetailsHtmlTable(dt);
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public static string RichMediaButtonDetailsHtmlTable(DataTable dt)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr>");

            // Generate headers based on DataTable columns
            foreach (DataColumn column in dt.Columns)
            {
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:10%;\">" + column.ColumnName + "</th>");
            }

            htmlTable.Append("</tr>");
            htmlTable.Append("</thead>");
            htmlTable.Append("<tbody>");

            // Generate data rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                htmlTable.Append("<tr>");

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    htmlTable.Append("<td>" + Convert.ToString(dt.Rows[i][j]) + "</td>");
                }

                htmlTable.Append("</tr>");
            }

            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");

            return htmlTable.ToString();
        }

        protected void lnkbtnDownloadDetails_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = HttpContext.Current.Session["ShowDetails"] as DataTable;

                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }

                    // Prepare the response for CSV download
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=RichMediaButtonDetails.csv");
                    Response.Charset = "";
                    Response.ContentType = "text/csv";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    // Handle the case where dt is null or empty
                    Response.Write("No data available to download.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                //Response.Write("Error: " + ex.Message);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = HttpContext.Current.Session["ShowSummary"] as DataTable;

                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }

                    // Prepare the response for CSV download
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=RichMediaButtonDetails.csv");
                    Response.Charset = "";
                    Response.ContentType = "text/csv";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    // Handle the case where dt is null or empty
                    Response.Write("No data available to download.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                //Response.Write("Error: " + ex.Message);
            }
        }
    }
}