using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class credit_debit_logs : System.Web.UI.Page
    {
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
        }
    }
}