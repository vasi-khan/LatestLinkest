using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class profile : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                getDtl();
            }  
        }
        public void getDtl()
        {
            DataTable dt = ob.GetUserParameter(user);

            if (dt.Rows.Count > 0)
            {
                txtUserId.Text = user;
                txtComp.Text = Convert.ToString(dt.Rows[0]["COMPNAME"]);
                txtName.Text = Convert.ToString(dt.Rows[0]["FULLNAME"]);
                txtMobile.Text = Convert.ToString(dt.Rows[0]["MOBILE1"]);
                txtMail.Text = Convert.ToString(dt.Rows[0]["EMAIL"]);
                txtWebsite.Text = Convert.ToString(dt.Rows[0]["WEBSITE"]);
                txtDLT.Text = Convert.ToString(dt.Rows[0]["DLTNO"]);
                txtACcreateDt.Text = "Account Creation Date : " + Convert.ToDateTime(dt.Rows[0]["ACCOUNTCREATEDON"]).ToString("dd/MMM/yyyy");
                txtExpDt.Text = "Account Expiry Date : " + Convert.ToDateTime(dt.Rows[0]["EXPIRY"]).ToString("dd/MMM/yyyy");
                if(Convert.ToBoolean(dt.Rows[0]["showPEID"]))
                    txtPEID.Text = Convert.ToString(dt.Rows[0]["PEID"]);
                else
                    txtPEID.Text = "XXXXXXXXXXXXXXXXXXX";
                PopulateSender();
                ddlSender.SelectedValue = Convert.ToString(dt.Rows[0]["SENDERID"]);
                if (Convert.ToString(dt.Rows[0]["PERMISSION"]) == "1") rdbDnd.Checked = true;
                if (Convert.ToString(dt.Rows[0]["PERMISSION"]) == "2") rdbNonDnd.Checked = true;
            }
        }
        public void PopulateSender()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["User"]));

            ddlSender.DataSource = dt;
            ddlSender.DataTextField = "senderid";
            ddlSender.DataValueField = "senderid";
            ddlSender.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlSender.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlSender.SelectedIndex = 1;
            else
                ddlSender.SelectedIndex = 0;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ob.UpdateCustomer(Convert.ToString(Session["User"]), ddlSender.SelectedValue, txtWebsite.Text, rdbDnd.Checked);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account updated successfully.');", true);
            getDtl();
        }
    }
}