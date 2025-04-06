using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class AgentMast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void ClearControl()
        {
            txtName.Text = "";
            txtAEmail.Text = "";
            txtMob1.Text = "";
            txtMob2.Text = "";
            rbtnlist.SelectedValue = "Agent";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Agent Name can not be blank.');", true);
                return;
            }
            if (txtMob1.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile1 can not be blank.');", true);
                return;
            }

            //if (rbtnlist.SelectedValue == "Agent")
                //ob.SaveAgentDtl(txtName.Text.Trim(), txtAEmail.Text.Trim(), txtMob1.Text.Trim(), txtMob2.Text.Trim(), "", rbtnlist.SelectedValue);
            //else
                //ob.SaveAgentDtl(txtName.Text.Trim(), txtAEmail.Text.Trim(), txtMob1.Text.Trim(), txtMob2.Text.Trim(), "", rbtnlist.SelectedValue);

            ClearControl();
        }
    }
}