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
    public partial class ViewDetailsSMSReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnShow_Click(null, null);
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            fillgrid();
        }
        public void fillgrid()
        {
            if (Request.QueryString["A"] != null)
            {
                string dater = Convert.ToString(Request.QueryString["A"]);
                string[] dtr = dater.Split('$');

                DataTable dt = new Util().GetSMSReportMobileWise(dtr[0], dtr[1], dtr[2]);
                if (dt != null && dt.Rows.Count > 0)
                {
                    grv.DataSource = dt;
                    grv.DataBind();
                }
            }

        }

        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            grv.DataBind();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {

        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            if (grv.DataSource != null)
            {
                lblTotal.Text = "Total Page: " + Math.Ceiling(Convert.ToDecimal((grv.DataSource as DataTable).Rows.Count) / grv.PageSize) + " Total Records: " + (grv.DataSource as DataTable).Rows.Count;

            }
        }
    }
}