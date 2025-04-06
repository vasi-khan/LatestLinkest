using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class m1m2 : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            string shorturlid = Convert.ToString(Request.QueryString["s"]);
            string lurl = ob.GetLongURLfromURLID(shorturlid);
            Response.Redirect(lurl);
        }
    }
}