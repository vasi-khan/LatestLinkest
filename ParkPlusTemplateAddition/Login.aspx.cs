using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ParkPlusTemplateAddition
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
            Helper.Util hl = new Helper.Util();
            DataTable dt = hl.GetLoginDetails(txtuserid.Text.Trim(), txtPwd.Text.Trim());
            if (dt == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                return;
            }
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                return;
            }
            Session["User"] = dt.Rows[0]["username"].ToString();
            Session["COUNTRYCODE"] = Convert.ToString(dt.Rows[0]["COUNTRYCODE"]);
            Response.Redirect("TemplateAddition.aspx");
        }
    }
}