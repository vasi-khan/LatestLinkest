using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel.wa_
{
    public partial class WAPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");
            if (Session["WAMsgText"]!=null)
            {
                random.InnerHtml = Session["WAMsgText"].ToString().Replace(Convert.ToString(Convert.ToChar(13))+ Convert.ToString(Convert.ToChar(10)),"<br>");
                //txtprv.Text = Session["WAMsgText"].ToString();
            }
        }
    }
}