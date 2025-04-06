using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class BDMaster : System.Web.UI.MasterPage
    {

        Helper.common.mlogin mobj = new common.mlogin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER"]== null)
            {
                Response.Redirect("TL_Login.aspx");

            }
           

            lbluser.Text = (Session["USER"] as Helper.common.mlogin).usernmae.ToString();
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("TL_login.aspx");
            // Response.Redirect("Lognic.aspx");

        }
    }
}