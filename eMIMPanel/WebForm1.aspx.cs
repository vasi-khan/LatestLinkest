using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateTemplateID();

        }
        public void PopulateTemplateID()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTemplateId("MIM2000002");

            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "TemplateID";
            ddlTempID.DataValueField = "Template";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlTempID.SelectedIndex = 1;
            //else
            ddlTempID.SelectedIndex = 0;

        }

    }
}