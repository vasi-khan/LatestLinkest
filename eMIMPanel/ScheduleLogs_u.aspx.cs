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
    public partial class ScheduleLogs_u : System.Web.UI.Page
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
                DataTable dt = ob.GetScheduleLogReport(user, "02/JAN/1900", "02/JAN/1900");
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
            DataTable dt = ob.GetScheduleLogReport(user, s1, s2);
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

        protected void btnDw_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblSch = (Label)gvro.FindControl("lblSch");
            DateTime date = Convert.ToDateTime(lblSch.Text);
            string Format = date.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dt = ob.GetMobileNumbersOfSchedule(Convert.ToString(Session["UserID"]), Format);
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "Scheduled_" + lblSch.Text + ".xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblSch = (Label)gvro.FindControl("lblSch");
            Label lblMobiles = (Label)gvro.FindControl("lblMobile");
            HiddenField hdnFileID = (HiddenField)gvro.FindControl("hdnFileID");
            Label lblcampname = (Label)gvro.FindControl("lblcampname");

            DateTime date = Convert.ToDateTime(lblSch.Text);
            string Format = date.ToString("yyyy-MM-dd HH:mm:ss");
            Int64 c = ob.GetNoOfMobilesForFileID(Convert.ToString(Session["UserID"]), hdnFileID.Value.ToString().Trim());
            if (c > 0)
            {
                DataTable dt = ob.GetScheduleLogReportFileId(user, Format);
                if (dt.Rows.Count > 0)
                {
                    string SMSRate = dt.Rows[0]["smsrate"].ToString().Trim();
                    string NoOfSMS = dt.Rows[0]["noofsms"].ToString().Trim();
                    Int32 cnt = Convert.ToInt32(lblMobiles.Text) * Convert.ToInt32(NoOfSMS) * (-1);
                    ob.UpdateAndGetBalanceSchedule(Convert.ToString(Session["UserID"]), "", cnt, Convert.ToDouble(SMSRate), lblcampname.Text.ToString());
                    //ob.RemoveFromSchedule(Convert.ToString(Session["UserID"]), hdnFileID.Value, lblSch.Text);
                    ob.RemoveFromSchedule(Convert.ToString(Session["UserID"]), Format);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Schedule deleted successfully.');window.location ='ScheduleLogs_u.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Scheduled messages not found for delete.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Scheduled messages not found for delete.');", true);
            }
        }
    }
}