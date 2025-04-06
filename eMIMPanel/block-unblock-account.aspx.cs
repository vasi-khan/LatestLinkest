using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace eMIMPanel
{
    public partial class block_unblock_account : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if(user == "") Response.Redirect("login.aspx");
            if (usertype != "SYSADMIN") Response.Redirect("index.aspx");

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            GetData();
        }
        
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label username = (Label)gvro.FindControl("lbluserid");

            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            ob.BlockUnblockUser(username.Text, user, "UNBLOCK");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('User ID Unblocked Successfully.');", true);
            GetData();
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label username = (Label)gvro.FindControl("lbluserid");

            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            ob.BlockUnblockUser( username.Text, user, "BLOCK");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('User ID Blocked Successfully.');", true);
            GetData();
        }
        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            //if (txtFrm.Text.Trim() == "")
            //{
            //    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            //else
            //{
            //    s1 = Convert.ToDateTime(txtFrm.Text).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            string status = (rdbBlocked.Checked ? "BLOCKED" : "UNBLOCKED");
            DataTable dt = ob.GetUserListForBlockUnBlock(usertype, user, status);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();

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

    }
}