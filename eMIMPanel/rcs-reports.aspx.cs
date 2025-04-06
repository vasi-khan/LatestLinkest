using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;
using eMIMPanel.rcscode;
namespace eMIMPanel
{
    public partial class rcs_reports : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                rcscode.Util ob = new rcscode.Util();
                DataTable dt = ob.GetSMSReport("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
                txtFrm.Text = hdntxtFrm.Value;
                txtTo.Text = hdntxtTo.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
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
            rcscode.Util ob = new rcscode.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserId"]);
            DataTable dt = ob.Getrcsreports(user, s1, s2, usertype, Convert.ToString(Session["DLT"]));
            grv.DataSource = null;
            grv.DataSource = dt;
            SetFooterValue(dt);
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
            //modalpopuppwd.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label l = (Label)gvro.FindControl("lblUserId");
                Label lbldate = (Label)gvro.FindControl("lblDate");
                string user = l.Text;
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
                rcscode.Util ob = new rcscode.Util();
                Response.Redirect("sms-reports_download.aspx?x=" + l.Text + "$" + lbldate.Text, true);
                //string un = h.Value;
                //ViewState["UN"] = un;
                //txtusername.Text = l.Text;
                //txtpwd.Text = h.Value;
                //modalpopuppwd.Show();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(Rejected)", string.Empty);

            grv.Columns[3].FooterText = "Total : ";
            grv.Columns[4].FooterText = sumSubmitted.ToString();
            grv.Columns[5].FooterText = sumDelivered.ToString();
            grv.Columns[6].FooterText = sumFailed.ToString();
            grv.Columns[7].FooterText = sumUnknown.ToString();

        }
    }
}