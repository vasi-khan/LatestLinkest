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
    public partial class CampaignReportMotoCorp : System.Web.UI.Page
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
                Bind();
                SetDropDownListItemColor();
            }
        }

        public void Bind()
        {

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

            //--Bind Events(ddlEvents) DropDown Data
            DataTable dtEvents = ob.GetEvents();
            ddlEvents.DataSource = dtEvents;
            ddlEvents.DataTextField = "EventName";
            ddlEvents.DataValueField = "EventID";
            ddlEvents.DataBind();
            ListItem objListItemTemp = new ListItem("--All--", "0");
            ddlEvents.Items.Insert(0, objListItemTemp);

            //Bind Category(dtCategory) DropDown Data
            DataTable dtCategory = ob.GetCategory((string)Session["userId"]);
            ddlCategory.DataSource = dtCategory;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ListItem objListItemCategory = new ListItem("--All--", "0");
            ddlCategory.Items.Insert(0, objListItemCategory);

            //Bind Location(ddlLocation) DropDown Data
            DataTable dtLocation = ob.GetLocation((string)Session["userId"]);
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objListItemLocation = new ListItem("--All--", "0");
            ddlLocation.Items.Insert(0, objListItemLocation);


            //Bind SubLocation(ddlSubLocation) DropDown Data
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"]);
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItemSubLocation = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItemSubLocation);


            //Bind DealerCode(ddlDealerCode) DropDown Data
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"]);
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        public void SetDropDownListItemColor()
        {
            foreach (ListItem item in ddlDealerCode.Items)
            {
                item.Attributes.CssStyle.Add("font-family", "Consolas");
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
            string Events = ddlEvents.SelectedValue.ToString();
            string GroupLocation = ddlCategory.SelectedValue.ToString();
            string Location = ddlLocation.SelectedValue.ToString();
            string SubLocation = ddlSubLocation.SelectedValue.ToString();
            string Dealer = ddlDealerCode.SelectedValue.ToString();
            DataSet ds = new DataSet();
            ds = ob.GetCampaignReportMotoCorp(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp, Events, GroupLocation, Location, SubLocation, Dealer);
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
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Events</th> ");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Group Location</th> ");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Location</th> ");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SubLocation</th> ");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Dealer Coed</th> ");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Dealer Name</th> ");
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
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["eventnm"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["CategoryName"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["LocationName"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["SubLocationName"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DlrCode"] + "</td>");
                htmlTable.Append("<td data-target=\"#collapseContent_" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DlrName"] + "</td>");
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

        public void SetValueInHtmlTable(DataSet ds)
        {
            double credit=0;
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
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Request <br>Date</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign /<br> File Name</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Events</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Group Location</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Location</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SubLocation</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Dealer Code</th> ");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">Dealer Name</th> ");
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
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["EventName"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["CategoryName"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["LocationName"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["SubLocationName"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DLRCODE"] + "</td>");
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["DLRName"] + "</td>");
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
                    
                    DataView dataView = dt1.DefaultView;
                    dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND requestdate='" + dt.Rows[j]["requestdate"] + "'";
                    string subHtml = "";// ConvertDataTableToHTML(dataView.ToTable());
                    htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                    htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>");
                    htmlTable.Append("</tr>");
                    smscount = smscount + Convert.ToInt32(dt.Rows[j]["smscount"].ToString());
                    delivered = delivered + Convert.ToInt32(dt.Rows[j]["delivered"].ToString());
                    delivered_p = delivered_p + Convert.ToInt32(dt.Rows[j]["delivered_p"].ToString());
                    failed = failed + Convert.ToInt32(dt.Rows[j]["failed"].ToString());
                    failed_p = failed_p + Convert.ToInt32(dt.Rows[j]["failed_p"].ToString());
                    AWAITED = AWAITED + Convert.ToInt32(dt.Rows[j]["AWAITED"].ToString());
                    AWAITED_p = AWAITED_p + Convert.ToInt32(dt.Rows[j]["AWAITED_p"].ToString());
                    openmsg = openmsg + Convert.ToInt32(dt.Rows[j]["openmsg"].ToString());
                    openmsg_p = openmsg_p + Convert.ToInt32(dt.Rows[j]["openmsg_p"].ToString());
                    //credit = credit + Convert.ToDouble(dt.Rows[j]["credit"].ToString()); 
                }
                htmlTable.Append("<tr class=\"tr-1\">");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">Total</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">"+ "" + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + smscount + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + delivered + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" +failed + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + AWAITED + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + openmsg + "</td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("</tr>");
                htmlTable.Append("<tr  aria-expanded=\"true\">");
                htmlTable.Append("<td class=\"inside-table\" colspan=\"20\"></td>");
                htmlTable.Append("</tr>");
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
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
        }

        protected void myFunction()
        {

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

        private void DatatableToCSV(DataTable dt ,string FileName)
        {
            try
            {
                DataTable cusTm = new DataTable();
                cusTm.Columns.AddRange(new DataColumn[16]
                    {
                    new DataColumn("1"),
                    new DataColumn("2"),
                    new DataColumn("3"),
                    new DataColumn("4"),
                    new DataColumn("5"),
                     new DataColumn("smscount"),
                    new DataColumn("delivered"),
                    new DataColumn("delivered_p"),
                    new DataColumn("failed"),
                    new DataColumn("failed_p"),
                    new DataColumn("AWAITED"),
                    new DataColumn("AWAITED_p"),
                    new DataColumn("openmsg"),
                    new DataColumn("openmsg_p"),
                    new DataColumn("6"),
                    new DataColumn("7")
                });

                cusTm.Rows.Add( "", "", "", "Total", Session["credit"], Session["smscount"], Session["delivered"], "", Session["failed"], "", Session["AWAITED"], "", Session["openmsg"], "", "","");
                cusTm.AcceptChanges();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName.Replace(".xls", ".csv"));
                Response.Charset = "";
                Response.ContentType = "application/text";

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
                foreach (DataRow dr in cusTm.Rows)
                {
                    for (int i = 0; i < cusTm.Columns.Count; i++)
                    {
                        columnbind.Append(dr[i].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                    }
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

        private void DatatableToExcel(DataTable dt)
        {
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
            string Events = ddlEvents.SelectedValue.ToString();
            string GroupLocation = ddlCategory.SelectedValue.ToString();
            string Location = ddlLocation.SelectedValue.ToString();
            string SubLocation = ddlSubLocation.SelectedValue.ToString();
            string Dealer = ddlDealerCode.SelectedValue.ToString();
            //DataSet ds = ob.GetCampaignWiseDetailsMotoCorp(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp, Events, GroupLocation, Location, SubLocation, Dealer);
            DataSet ds = ob.GetCampaignReportMotoCorp(user, Convert.ToDateTime(s1), Convert.ToDateTime(s2), camp, Events, GroupLocation, Location, SubLocation, Dealer);
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DatatableToCSV(dt, "CampaignReport.csv");
            }
        }

        //Bind Location(ddlLocation) DropDown Data
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            DataTable dtLocation = ob.GetLocation((string)Session["userId"],CategoryId);
            if (dtLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlLocation.Items.Insert(0, objListItem);

            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItemSubLocation = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItemSubLocation);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind SubLocation(ddlSubLocation) DropDown Data
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItem);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind DealerCode(ddlDealerCode) DropDown Data
        protected void ddlSubLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItem);
        }
    }
}