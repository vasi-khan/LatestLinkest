using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class sms_reports_u1 : System.Web.UI.Page
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
             usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");

            SMSFields();
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                //DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }
        public void SMSFields()
        {
            string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetSMSSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["Submitted"]);
                lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["Delivered"]);
                lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["Failed"]);
            }
            else
            {
                lblTodayFailed.Text = "0";
                lblTodayDelivered.Text = "0";
                lblTodaySubmitted.Text = "0";
            }

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
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
            if (txtFrm.Text.Trim() == "")
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
            DataTable dt = ob.GetSMSReport_user(s1, s2, usertype, user);

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
            //grv.DataSource = null;
            //grv.DataSource = dt;
            //grv.DataBind();
            //GridFormat(dt);
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
            //modalpopuppwd.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label l = (Label)gvro.FindControl("lblUserId");
            //string un = h.Value;
            //ViewState["UN"] = un;
            //txtusername.Text = l.Text;
            //txtpwd.Text = h.Value;
            //modalpopuppwd.Show();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtmob.Text.Trim() != "")
            {
                if (txtmob.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter 10 digit mobile number');", true);
                    return;
                }
            }
            if (hdntxtFrm1.Value.Trim() == "")
            {
                d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                d1 = Convert.ToDateTime(hdntxtFrm1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                d2 = Convert.ToDateTime(hdntxtTo1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetSMSReport4MOBILE_user(d1, d2, txtmob.Text, user);
            if (dt.Rows.Count > 0)
            {
                Session["MOBILEDATA"] = dt;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data fetched successfully. Click buttons to download.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data found for the mobile number.');", true);
            }
        }
        protected void btnXLdw_Click(object sender, EventArgs e)
        {
            if (Session["MOBILEDATA"] != null)
            {
                Session["FILENAME"] = "SMS_Mobile_Seaarch_Report.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
        }
    }
}