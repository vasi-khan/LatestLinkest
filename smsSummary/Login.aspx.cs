using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smsSummary
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtuserid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
                return;
            }
            if (txtPwd.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
                return;
            }
            string UserId = System.Configuration.ConfigurationManager.AppSettings["UserId"].ToString();
            string Pwd = System.Configuration.ConfigurationManager.AppSettings["Pwd"].ToString();

            if (txtuserid.Text.Trim()== UserId && txtPwd.Text.Trim()== Pwd)
            {
                Session["User"] = UserId;
                Response.Redirect("smsSummary.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                return;
            }
        }
    }
}