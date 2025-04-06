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
    public partial class add_group : System.Web.UI.Page
    {
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
                GetData();
            }
            
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtGrNm.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter group name.');", true);
                return;
            }
            //check duplicate
            string user = Convert.ToString(Session["UserID"]);
            if (ob.GroupExists4User(user, txtGrNm.Text.Trim(), ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group name already exists.');", true);
                return;
            }
            ob.CreateGroup(user, txtGrNm.Text.Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group successfully created.');", true);
            txtGrNm.Text = "";
            GetData();
            btnUpdate.Visible = false;
            btnCreate.Visible = true;
            Session["LBLGROUPNAME"] = "";
        }

        public void GetData()
        {
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetGroup(user);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }
        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;

            Label lblgrpname = (Label)gvro.FindControl("lblgrpname");
            Session["LBLGROUPNAME"] = lblgrpname.Text;
            txtGrNm.Text = lblgrpname.Text;
            btnCreate.Visible = false;
            btnUpdate.Visible = true;
        }
        protected void btnDw_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblgrpname = (Label)gvro.FindControl("lblgrpname");
            DataTable dt = ob.GetMobileNumbersOfGroup(Convert.ToString(Session["UserID"]), lblgrpname.Text);
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "MobileNumbers_" + lblgrpname.Text + ".xls";
                Session["PageName"] = "add-page.aspx";
                //btn.Attributes.Add("class", "btn btn-primary text-secondary");
                Response.Redirect("sms-reports_u_download.aspx", false);
                //Response.Redirect("add-group.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
                
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtGrNm.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter group name.');", true);
                return;
            }
            //check duplicate
            string user = Convert.ToString(Session["UserID"]);
            if (ob.GroupExists4User(user, txtGrNm.Text.Trim(), Convert.ToString(Session["LBLGROUPNAME"])))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group name already exists.');", true);
                return;
            }
            ob.UpdateGroup(user, txtGrNm.Text.Trim(), Convert.ToString(Session["LBLGROUPNAME"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group successfully updated.');", true);
            txtGrNm.Text = "";
            btnUpdate.Visible = false;
            btnCreate.Visible = true;
            Session["LBLGROUPNAME"] = "";
            GetData();
        }

        protected void btndlt_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblgroupname = (Label)gvro.FindControl("lblgrpname");
            string user = Convert.ToString(Session["UserID"]);
            ob.DeleteGroup(user, lblgroupname.Text);
            GetData();
        }
    }
}