using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

namespace eMIMPanel
{
    public partial class ScheduleLogs_u_Admin : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                BindUsers();
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                DataTable dt = ob.GetScheduleLogReport(user, "02/JAN/1900", "02/JAN/1900");
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }

        public void BindUsers()
        {
            string sql = @"SELECT username FROM CUSTOMER WITH(NOLOCK) WHERE createdby='" + user + "' ORDER BY username";
            DataTable dt = ob.GetRecordDataTableSql(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ddlUserId.ClearSelection();
                    ddlUserId.DataSource = dt;
                    ddlUserId.DataValueField = "username";
                    ddlUserId.DataTextField = "username";
                    ddlUserId.DataBind();
                    ddlUserId.Items.Insert(0, new ListItem("--ALL--", ""));
                }
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be Empty.');", true);
                return;
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                
            }
            if (hdntxtTo.Value.Trim()=="")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date can not be Empty.');", true);
                return;
            }
            else
            {
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                return;
            }
            if (Convert.ToDateTime(s2) < Convert.ToDateTime(s1))
            {
                hdntxtFrm.Value = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            string usertype = Convert.ToString(Session["UserType"]);
            string ProfileId = ddlUserId.SelectedValue.ToString();
            DataTable dt = ob.GetScheduleLogsData(ProfileId, s1, s2);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
            Session["Scheduledt"] = dt;
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
            Label lblUsername = (Label)gvro.FindControl("lblUsername");
            Label lblSch = (Label)gvro.FindControl("lblSch");
            DateTime date = Convert.ToDateTime(lblSch.Text);
            string Format = date.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dt = ob.GetMobileNumbersOfSchedule(lblUsername.Text.ToString().Trim(), Format);
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
            Label lblEncryUsername = (Label)gvro.FindControl("lblEncryUsername");
            Label lblUsername = (Label)gvro.FindControl("lblUsername");
            Label lblSch = (Label)gvro.FindControl("lblSch");
            Label lblMobiles = (Label)gvro.FindControl("lblMobile");
            HiddenField hdnFileID = (HiddenField)gvro.FindControl("hdnFileID");
            Label lblcampname = (Label)gvro.FindControl("lblcampname");

            DateTime date = Convert.ToDateTime(lblSch.Text);
            string Format = date.ToString("yyyy-MM-dd HH:mm:ss");
            Int64 c = ob.GetNoOfMobilesForFileID(lblUsername.Text.ToString().Trim(), hdnFileID.Value.ToString().Trim());
            if (c > 0)
            {
                DataTable dtschdule = (Session["MOBILEDATA"] as DataTable);
                DataRow[] dr = dtschdule.Select("EncryUserName='" + lblEncryUsername.Text.ToString().Trim() + "' AND profileid = '" + lblUsername.Text.ToString().Trim() + "'");
                if (dr.Length > 0)
                {
                    DataTable dt = ob.GetScheduleLogReportFileId(user, Format);
                    if (dt.Rows.Count > 0)
                    {
                        string SMSRate = dt.Rows[0]["smsrate"].ToString().Trim();
                        string NoOfSMS = dt.Rows[0]["noofsms"].ToString().Trim();
                        Int32 cnt = Convert.ToInt32(lblMobiles.Text) * Convert.ToInt32(NoOfSMS) * (-1);
                        ob.UpdateAndGetBalanceSchedule(lblUsername.Text.ToString().Trim(), "", cnt, Convert.ToDouble(SMSRate), lblcampname.Text.ToString());
                        ob.RemoveFromSchedule(lblUsername.Text.ToString().Trim(), Format);
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Scheduled messages not found for delete.');", true);
            }
        }
    }
}