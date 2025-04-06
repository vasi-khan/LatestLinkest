using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class SMSReport2 : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            if (grv.DataSource != null)
            {
                lblTotal.Text = "Total Page: " + Math.Ceiling(Convert.ToDecimal((grv.DataSource as DataTable).Rows.Count) / grv.PageSize) + " Total Records: " + (grv.DataSource as DataTable).Rows.Count;

            }
        }
        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            grv.DataBind();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            fillgrid();
        }

        public void fillgrid()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter From date');", true);
                return;
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter To date');", true);
                return;
            }

            if (txtMobileNo.Text.Trim()=="")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Mobile No');", true);
                return;
            }

                DataTable dt = new Util().GetSMSReport2(user, txtMobileNo.Text.Trim(),(hdntxtFrm.Value.Trim()), (hdntxtTo.Value.Trim()));
                if (dt != null && dt.Rows.Count > 0)
                {
                    grv.DataSource = dt;
                    grv.DataBind();
                }
            else
            {
                grv.DataSource = null;
                grv.DataBind();
            }
            

        }
    }
}