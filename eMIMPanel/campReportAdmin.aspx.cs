using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class campReportAdmin : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        string UserL = "";

        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            usertype = Convert.ToString(Session["UserType"]);
            UserL = Convert.ToString(Session["User"]);
            if (UserL == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                // PopulateCampaign();

                dUser.Visible = false;
                dCamp.Visible = false;
                // dGo.Visible = false;
            }
        }


        protected void lnkShow_Click(object sender, EventArgs e)
        {

            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            h1.Value = hdntxtFrm.Value;
            h2.Value = hdntxtTo.Value;

        }

        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "") 
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be Empty.');", true);
                return;
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date can not be Empty.');", true);
                return;
            }
            else
            {
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.997";
            }
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

            //if (Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))if (Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))

            if (Convert.ToDateTime(s1) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)) || Convert.ToDateTime(s2) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From and To date should be less than Today date');", true);
                return;
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(s2))
            {
                hdntxtFrm.Value = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            if (Convert.ToInt32(rbUser.SelectedValue) > 0)
            {
                DataTable dtC = ob.GetUserParameter(txtUser.Text.Trim());

                if (string.IsNullOrEmpty(txtUser.Text) || dtC.Rows.Count < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter User Name.');", true);
                    return;
                } 

                string dlt = Convert.ToString(Session["DLT"]);

                if (Convert.ToString(Session["UserType"]).ToUpper() == "ADMIN")
                {
                    if (dlt.ToUpper() != Convert.ToString(dtC.Rows[0]["DLTNO"]).ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('User does not belong to the current login Admin.');", true);
                        return;
                    }
                }

                string user = dtC.Rows.Count > 0 ? Convert.ToString(dtC.Rows[0]["username"]) : "";
                ViewState["User"] = user;
                if (user == "" || user == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter a valid User ID.');", true);
                    return;
                }
                if (Convert.ToString(user).ToUpper() == "MIM2002078")
                {
                    if (Convert.ToDateTime(s1) < Convert.ToDateTime("2021-06-01"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Access Denied.');", true);
                        return;
                    }
                }
            }
            else
            {
                ViewState["User"] = "";
            }

            string camp = ddlCamp.SelectedValue;

            DataSet ds = new DataSet();

            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();
            if (hdntxtFrm.Value.Trim() == "")
            {
                FromDate = DateTime.Now;
            }
            else
            {
                FromDate = ob.Setdate(s1);
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ToDate = DateTime.Now;
            }
            else
            {
                ToDate = ob.Setdate(s2);
            }

            if (ViewState["User"].ToString() != null || ViewState["User"].ToString() != "")
            {
                user = ViewState["User"].ToString();
            }
            DataTable dt = new DataTable();

            string strpName = "SP_GetcampaigrptWorking";
            SqlParameter[] arrprm = new SqlParameter[5];

            arrprm[0] = new SqlParameter("@fdate", SqlDbType.VarChar, 50);
            arrprm[0].Value = s1;
            arrprm[1] = new SqlParameter("@tDate", SqlDbType.VarChar, 50);
            arrprm[1].Value = s2;
            arrprm[2] = new SqlParameter("@usr", SqlDbType.VarChar, 50);
            arrprm[2].Value = user;
            arrprm[3] = new SqlParameter("@camp", SqlDbType.VarChar, 50);
            arrprm[3].Value = ddlCamp.SelectedValue == "0" ? "" : ddlCamp.SelectedValue;
            if (Convert.ToString(Session["UserType"]).ToUpper() == "ADMIN")
            {
                arrprm[4] = new SqlParameter("@DLT", SqlDbType.VarChar, 50);
                arrprm[4].Value = Convert.ToString(Session["DLT"]);
            }
            else if(Convert.ToString(Session["UserType"]).ToUpper() == "BD")
            {
                arrprm[4] = new SqlParameter("@DLT", SqlDbType.VarChar, 50);
                arrprm[4].Value = string.Empty;
            }
            else
            {
                arrprm[4] = new SqlParameter("@DLT", SqlDbType.VarChar, 50);
                arrprm[4].Value = string.Empty;
            }

            dt = database.GetDataTableSp(arrprm, strpName);

            SetValueInHtmlTable(dt);
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
            double credit = 0;
            int smscount = 0;
            int delivered = 0;
            int delivered_p = 0;
            int failed = 0;
            int failed_p = 0;
            int AWAITED = 0;
            int AWAITED_p = 0;
            int openmsg = 0;
            int openmsg_p = 0;

            if (dt.Rows.Count > 0)
            {
                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
                htmlTable.Append("<thead>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%;\" rowspan=\"2\">Sr. No</th>");
                htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">User Id</th>");
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
                    htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["userid"] + "</td>");
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

                    credit= credit+ Convert.ToDouble(dt.Rows[j]["credit"].ToString());
                    smscount = smscount + Convert.ToInt32(dt.Rows[j]["smscount"].ToString());
                    delivered = delivered + Convert.ToInt32(dt.Rows[j]["delivered"].ToString());
                    delivered_p = delivered_p + Convert.ToInt32(dt.Rows[j]["delivered_p"].ToString());
                    failed = failed + Convert.ToInt32(dt.Rows[j]["failed"].ToString());
                    failed_p = failed_p + Convert.ToInt32(dt.Rows[j]["failed_p"].ToString());
                    AWAITED = AWAITED + Convert.ToInt32(dt.Rows[j]["AWAITED"].ToString());
                    AWAITED_p = AWAITED_p + Convert.ToInt32(dt.Rows[j]["AWAITED_p"].ToString());
                    openmsg = openmsg + Convert.ToInt32(dt.Rows[j]["openmsg"].ToString());
                    openmsg_p = openmsg_p + Convert.ToInt32(dt.Rows[j]["openmsg_p"].ToString());

                    //DataView dataView = dt.DefaultView;
                    //dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND requestdate='" + dt.Rows[j]["requestdate"] + "'";
                    //string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                    //htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                    //htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>"); 
                    // htmlTable.Append("</tr>");

                }
                // Total Show Start
                htmlTable.Append("<tr class=\"tr-1\">");
                //htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + Convert.ToString(j + 1) + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"> </td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"> </td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">Total</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">"+ credit + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + smscount + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + delivered + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + failed+"</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + AWAITED + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + openmsg + "</td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
                htmlTable.Append("<td  data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\"></td>");
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
                
                // Total Show Start


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
            dt.Columns["userid"].ColumnName = "User ID";
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
            if (h1.Value == "" || h2.Value == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select From and To date');", true);
                return;
            }

            if (h1.Value.Trim() == "" || h2.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }
            else if (h1.Value.Trim() == "" && h2.Value.Trim() != "")
            {
                s1 = Convert.ToDateTime(h1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = s1;
            }
            else if (h1.Value.Trim() != "" && h2.Value.Trim() == "")
            {
                s1 = Convert.ToDateTime(h1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); ;
            }
            else
            {
                s1 = Convert.ToDateTime(h1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(h2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }

            if (Convert.ToDateTime(h1.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now) || Convert.ToDateTime(h2.Value, CultureInfo.InvariantCulture) > Convert.ToDateTime(DateTime.Now))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From and To date should be less than Today date');", true);
                return;
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(s2))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date cannot be above To Date.');", true);
                return;
            }

            if (Convert.ToInt32(rbUser.SelectedValue) > 0)
            {
                DataTable dtC = ob.GetUserParameter(txtUser.Text.Trim());

                if (string.IsNullOrEmpty(txtUser.Text) || dtC.Rows.Count < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter User Name.');", true);
                    return;
                }

                string dlt = Convert.ToString(Session["DLT"]);

                if (Convert.ToString(Session["UserType"]).ToUpper() == "ADMIN")
                {
                    if (dlt.ToUpper() != Convert.ToString(dtC.Rows[0]["DLTNO"]).ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('User does not belong to the current login Admin.');", true);
                        return;
                    }
                }

                string user = dtC.Rows.Count > 0 ? Convert.ToString(dtC.Rows[0]["username"]) : "";
                ViewState["User"] = user;
                if (user == "" || user == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter a valid User ID.');", true);
                    return;
                }
                if (Convert.ToString(user).ToUpper() == "MIM2002078")
                {
                    if (Convert.ToDateTime(s1) < Convert.ToDateTime("2021-06-01"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Access Denied.');", true);
                        return;
                    }
                }
            }
            else
            {
                ViewState["User"] = "";
            }
            //string user = Convert.ToString(Session["UserID"]);
            string camp = ddlCamp.SelectedValue; // txtCamp.Text.Trim();

            //  DataSet ds = ob.GetCampaignWiseData_new(user, s1, s2, camp);


            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();
            if (h1.Value.Trim() == "")
            {
                FromDate = DateTime.Now;
            }
            else
            {
                FromDate = ob.Setdate(Convert.ToDateTime(h1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            if (h2.Value.Trim() == "")
            {
                ToDate = DateTime.Now;
            }
            else
            {
                ToDate = ob.Setdate(Convert.ToDateTime(h2.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["User"])))
            {
                user = Convert.ToString(ViewState["User"]);
            }
            else
            {
                user = "";
            }
            string strProc = "SP_GetcampaigrptWorking";
            SqlParameter[] arrparm = new SqlParameter[5];
            arrparm[0] = new SqlParameter("@usr", SqlDbType.VarChar, 50);
            arrparm[0].Value = user;
            arrparm[1] = new SqlParameter("@Fdate", SqlDbType.DateTime);
            arrparm[1].Value = Convert.ToDateTime(FromDate);
            arrparm[2] = new SqlParameter("@tdate", SqlDbType.DateTime);
            arrparm[2].Value = Convert.ToDateTime(ToDate);
            arrparm[3] = new SqlParameter("@camp", SqlDbType.VarChar, 50);
            arrparm[3].Value = camp == "0" ? "" : camp;
            if (Convert.ToString(Session["UserType"]).ToUpper() == "ADMIN")
            {
                arrparm[4] = new SqlParameter("@DLT", SqlDbType.VarChar, 50);
                arrparm[4].Value = Convert.ToString(Session["DLT"]);
            }
            else
            {
                arrparm[4] = new SqlParameter("@DLT", SqlDbType.VarChar, 50);
                arrparm[4].Value = string.Empty;
            }
            DataSet ds = Helper.database.GetDataSetSp(arrparm, strProc);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DatatableToCSV(dt,"CampaignReport.csv");
            }

        }
        private void DatatableToCSV(DataTable dt, string FileName)
        {
            try
            {
                //abhishek -09-02-2023 s 
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
                
                cusTm.Rows.Add("","","","", "Total", Session["credit"], Session["smscount"], Session["delivered"], "", Session["failed"], "", Session["AWAITED"],"" ,Session["openmsg"], "", "");
                cusTm.AcceptChanges();
                //abhishek -09-02-2023 E

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

                //abhishek -09-02-2023 s 
                foreach (DataRow dr in cusTm.Rows)
                {
                    for (int i = 0; i < cusTm.Columns.Count; i++)
                    {
                        columnbind.Append(dr[i].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');

                    }

                }
                //abhishek -09-02-2023 E



                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }
        protected void rbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(rbUser.SelectedValue) == 1)
            {
                dUser.Visible = true;
                //dGo.Visible = true;
            }
            else
            {
                dUser.Visible = false;
                // dGo.Visible = false;
                dCamp.Visible = false;
            }

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            dCamp.Visible = true;
        }
    }
}