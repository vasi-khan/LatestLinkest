using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class click_reports_u : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                DataTable dt = ob.GetUserReport(user, ws, "02/JAN/1900", "02/JAN/1900", true);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            s1 = hdntxtFrm.Value;
           
            //s2 = h2.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
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
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            //DataTable dt = ob.GetSMSReport_user(s1, s2, usertype, user);
            //DataTable dt = ob.GetUserReport(user, Request.Url.Scheme + @"://" + Request.Url.Authority, s1, s2, true);
            DataTable dt = ob.GetUserReport(user, "", s1, s2, true);
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

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void btnDel_Click(object sender, EventArgs e)
        { }

        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblurlid = (Label)gvro.FindControl("lblurlid");
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetUserReportDetail(user, lblurlid.Text, true);
            Session["rptDetail"] = dt;
            if (dt.Rows.Count > 0)
            {
                string url = ResolveUrl("~\\click-reports-detail_u.aspx");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }
        protected void btnXL_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblurlid = (Label)gvro.FindControl("lblurlid");
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetUserReportDetail(user, lblurlid.Text, true);
            Session["MOBILEDATA"] = dt;
            
            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "ClickReportDetail.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }
        
    }
}