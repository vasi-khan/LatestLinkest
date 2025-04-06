using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class campReportU : System.Web.UI.Page
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

            string user = Convert.ToString(Session["UserID"]);
            string camp = ddlCamp.SelectedValue;

            if (Convert.ToString(Session["UserID"]).ToUpper() == "MIM2002078")
            {
                if (Convert.ToDateTime(s1) < Convert.ToDateTime("2021-06-01"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Access Denied.');", true);
                    return;
                }
            }
            DataSet ds = new DataSet();
            ds = ob.SP_GetCampaignWiseDatap(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp);
            SetValueInHtmlTable(ds);
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
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");
            return Convert.ToString(htmlTable);
        }

        public void SetValueInHtmlTable(DataSet ds)
        {
            //-----Start Add By Vikas For Bajaj On 23-05-2024---
            string BajajAllianzDLTNO = Convert.ToString(ConfigurationManager.AppSettings["BajajAllianzDLTNO"]);
            //-----END-------

            double credit = 0;
            int NoOfCount = 0;
            int smscount = 0;
            int delivered = 0;
            int delivered_p = 0;
            int failed = 0;
            int failed_p = 0;
            int AWAITED = 0;
            int AWAITED_p = 0;
            int openmsg = 0;
            int openmsg_p = 0;

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[0];

                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
                htmlTable.Append("<thead>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
                if (user == "MIM2201216" || user == "MIM2300184")
                {
                    htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2;\" > Action</th>");
                }
                else
                {
                    htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;display:none\" rowspan=\"2;\" > Action</th>");
                }
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Campaign <br>Date</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Campaign <br>Time</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign Name</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">File Name</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SMS Credit</th> ");

                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Mobile Number Count</th> ");

                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">SMS Submited</th> ");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Awaited</th>");
                htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Hit Count</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:7%;\" rowspan=\"2\">Sender ID</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > SMS Text </th>");

                //Add By Vikas For Bajaj On 23-05-2024
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                {
                    htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2;\" >Campaign File <br> Download</th>");
                    htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" colspan=\"2;\" >Uploaded File <br> Details</th>");
                }

                htmlTable.Append("</tr>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th> Numbers </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                htmlTable.Append("<th> Number </th><th>%</th>");
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    htmlTable.Append("<th> Duplicate </th><th>InCorrect</th>");

                htmlTable.Append("</tr>");
                htmlTable.Append("</thead>");
                htmlTable.Append("<tbody>");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    htmlTable.Append("<tr class=\"tr-1\">");
                    htmlTable.Append("<td>" + Convert.ToString(j + 1) + "</td>");

                    htmlTable.Append("<td>" + dt.Rows[j]["requestdate"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["requesttime"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["campaign"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["FILENM"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["credit"] + "</td>");

                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                        htmlTable.Append("<td>" + dt.Rows[j]["NoOfCount"] + "</td>");

                    htmlTable.Append("<td>" + dt.Rows[j]["smscount"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["delivered"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["delivered_p"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["failed"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["failed_p"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["AWAITED"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["AWAITED_p"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["openmsg"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["openmsg_p"] + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[j]["sender"] + "</td>");
                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                        htmlTable.Append("<td>" + dt.Rows[j]["TemplateText"] + "</td>");
                    else
                        htmlTable.Append("<td>" + dt.Rows[j]["message"] + "</td>");

                    //Add By Vikas On 23-05-2024
                    if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    {
                        DataTable dtSaveFileName = Helper.database.GetDataTable("SELECT SaveFileName,FP.ID FROM SMSFILEUPLOAD SU INNER JOIN fileprocess FP ON SU.fileprocessid=FP.ID WHERE USERID='" + user + "' AND SaveFileName IS NOT NULL AND SU.ID='" + dt.Rows[j]["fileid"] + "'");
                        if (dtSaveFileName.Rows.Count > 0)
                        {
                            string SaveFileName = Convert.ToString(dtSaveFileName.Rows[0]["SaveFileName"]);
                            string FilePorcessid = Convert.ToString(dtSaveFileName.Rows[0]["ID"]);
                            if (SaveFileName != "")
                            {
                                htmlTable.Append("<td><a href='XLSUpload/" + SaveFileName + "' title='Download'><i class='fa fa-download'></i></a></td>");
                                if (FilePorcessid != "")
                                {
                                    string Duplicate = Convert.ToString(Helper.database.GetScalarValue("SELECT COUNT(*) AS Duplicate FROM MobileDependency WITH(NOLOCK) WHERE FileProcessId='" + FilePorcessid + "' AND TYPE='D'"));
                                    string InCorrect = Convert.ToString(Helper.database.GetScalarValue("SELECT COUNT(*) AS InCorrect FROM MobileDependency WITH(NOLOCK) WHERE FileProcessId='" + FilePorcessid + "' AND TYPE='I'"));
                                    if (Duplicate == "0")
                                    {
                                        htmlTable.Append("<td>" + Duplicate + "</td>");
                                    }
                                    else
                                    {
                                        htmlTable.Append("<td><a onclick=\"DownloadCSV('" + FilePorcessid + "','D')\" style='color: blue; '>" + Duplicate + "</a></td>");
                                    }
                                    if (InCorrect == "0")
                                    {
                                        htmlTable.Append("<td>" + InCorrect + "</td>");
                                    }
                                    else
                                    {
                                        htmlTable.Append("<td><a onclick=\"DownloadCSV('" + FilePorcessid + "','I')\" style='color: blue; '>" + InCorrect + "</a> </td>");
                                    }
                                }
                            }
                            else
                            {
                                htmlTable.Append("<td></td>");
                                htmlTable.Append("<td></td>");
                                htmlTable.Append("<td></td>");
                            }
                        }
                        else
                        {
                            htmlTable.Append("<td></td>");
                            htmlTable.Append("<td></td>");
                            htmlTable.Append("<td></td>");
                        }
                    }
                    htmlTable.Append("</tr>");

                    DataView dataView = dt1.DefaultView;
                    dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND requestdate='" + dt.Rows[j]["requestdate"] + "'";
                    string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                    htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                    htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>");
                    // htmlTable.Append( subHtml );

                    htmlTable.Append("</tr>");

                    //abhishek 09-02-2023 S
                    NoOfCount = NoOfCount + Convert.ToInt32(dt.Rows[j]["NoOfCount"].ToString());
                    smscount = smscount + Convert.ToInt32(dt.Rows[j]["smscount"].ToString());
                    delivered = delivered + Convert.ToInt32(dt.Rows[j]["delivered"].ToString());
                    delivered_p = delivered_p + Convert.ToInt32(dt.Rows[j]["delivered_p"].ToString());
                    failed = failed + Convert.ToInt32(dt.Rows[j]["failed"].ToString());
                    failed_p = failed_p + Convert.ToInt32(dt.Rows[j]["failed_p"].ToString());
                    AWAITED = AWAITED + Convert.ToInt32(dt.Rows[j]["AWAITED"].ToString());
                    AWAITED_p = AWAITED_p + Convert.ToInt32(dt.Rows[j]["AWAITED_p"].ToString());
                    openmsg = openmsg + Convert.ToInt32(dt.Rows[j]["openmsg"].ToString());
                    openmsg_p = openmsg_p + Convert.ToInt32(dt.Rows[j]["openmsg_p"].ToString());
                    credit = credit + Convert.ToDouble(dt.Rows[j]["credit"].ToString());

                    //abhishek 09-02-2023 E

                }
                // abhishek 09-02-2023 S

                htmlTable.Append("<tr class=\"tr-1\">");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">Total</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + credit + "</td>");

                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + NoOfCount + "</td>");

                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + smscount + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + delivered + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + failed + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + AWAITED + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + openmsg + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");

                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                {
                    htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                    htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                    htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                }
                htmlTable.Append("</tr>");


                htmlTable.Append("<tr  aria-expanded=\"true\">");
                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    htmlTable.Append("<td class=\"inside-table\" colspan=\"21\"></td>");
                else
                    htmlTable.Append("<td class=\"inside-table\" colspan=\"17\"></td>");

                htmlTable.Append("</tr>");

                Session["NoOfCount"] = NoOfCount;
                Session["smscount"] = smscount;
                Session["delivered"] = delivered;
                Session["delivered_p"] = delivered_p;
                Session["failed"] = failed;
                Session["failed_p"] = failed_p;
                Session["AWAITED"] = AWAITED;
                Session["AWAITED_p"] = AWAITED_p;
                Session["openmsg"] = openmsg;
                Session["openmsg_p"] = openmsg_p;
                Session["credit"] = credit;

                // abhishek 09-02-2023 E
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
        }

        protected void myFunction()
        {

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
            string BajajAllianzDLTNO = Convert.ToString(ConfigurationManager.AppSettings["BajajAllianzDLTNO"]);

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
            dt.Columns["NoOfCount"].ColumnName = "Mobile No Count";
            dt.Columns["smscount"].ColumnName = "SMS Submitted";
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

            if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                dt.Columns.Remove("message");

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
                    HttpContext.Current.Response.Write(tab + dr[i].ToString().Replace('\n', ' ').Replace(Convert.ToChar(10), ' ').Replace(Convert.ToChar(13), ' '));
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
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
            string camp = ddlCamp.SelectedValue;
            DataSet ds = ob.SP_GetCampaignWiseDatap(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DatatableToCSV(dt, "CampaignReport.csv");
                }
            }
        }

        private void DatatableToCSV(DataTable dt, string FileName)
        {
            try
            {
                string BajajAllianzDLTNO = Convert.ToString(ConfigurationManager.AppSettings["BajajAllianzDLTNO"]);

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName.Replace(".xls", ".csv"));
                Response.Charset = "";
                Response.ContentType = "application/text";

                dt.Columns["RequestDate"].ColumnName = "Campaign Date";
                dt.Columns["RequestTime"].ColumnName = "Campaign Time";
                dt.Columns["Campaign"].ColumnName = "Campaign Name";
                dt.Columns["FILENM"].ColumnName = "File Name";
                dt.Columns["NoOfCount"].ColumnName = "Mobile No Count";
                dt.Columns["smscount"].ColumnName = "SMS Submitted";
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

                if (Convert.ToString(Session["DLT"]).ToLower() == Convert.ToString(BajajAllianzDLTNO).ToLower())
                    dt.Columns.Remove("message");

                DataTable dtCloned = dt.Clone();
                foreach (DataColumn column in dtCloned.Columns)
                {
                    column.DataType = typeof(string);
                }
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                dtCloned.Rows.Add("", "", "", "", "", "Total", Session["credit"], Session["NoOfCount"], Session["smscount"], Session["delivered"], "", Session["failed"], "", Session["AWAITED"], "", Session["openmsg"], "", "");
                dtCloned.AcceptChanges();

                string CSVData = ConvertDataTableToCsv(dtCloned);

                Response.Output.Write(Convert.ToString(CSVData));
                Response.Flush();
                Response.End();
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }

        #region For Download File Add By Vikas On 01_06_2024
        [WebMethod]
        public static object UploadedDownloadCSV(string FileProcessId, string TYPE)
        {
            try
            {
                string qry = "";
                if (TYPE == "D")
                {
                    qry = "SELECT MoblieNo FROM MobileDependency WHERE TYPE='D' AND FileProcessId='" + FileProcessId + "' ";
                }
                else
                {
                    qry = "SELECT MoblieNo FROM MobileDependency WHERE TYPE='I' AND FileProcessId='" + FileProcessId + "' ";
                }
                DataTable dt = Helper.database.GetDataTable(qry);
                string CSVData = ConvertDataTableToCsv(dt);
                string FileName = "";
                if (TYPE == "D")
                {
                    FileName = "Duplicate";
                }
                else
                {
                    FileName = "Incorrect";
                }

                return new { fileName = FileName, csvData = CSVData };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        private static string ConvertDataTableToCsv(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in dt.Columns)
            {
                sb.Append(column.ColumnName);
                if (column != dt.Columns[dt.Columns.Count - 1])
                    sb.Append(",");
            }
            sb.AppendLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    sb.Append(QuoteValue(item.ToString()));
                    if (Array.IndexOf(row.ItemArray, item) != row.ItemArray.Length - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string QuoteValue(string value)
        {
            return string.Concat("\"", value.Replace("\"", "\"\""), "\"");
        }
        #endregion
    }
}