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
    public partial class MiMReportGroupandAccounts : System.Web.UI.Page
    {
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            DataTable dt = obj.GetMiMreportGroup();
            grd.DataSource = dt;
            grd.DataBind();
            GridFormat(dt);

        }
        protected void GridFormat(DataTable dt)
        {
            grd.UseAccessibleHeader = true;
            grd.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grd.TopPagerRow != null)
            {
                grd.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grd.BottomPagerRow != null)
            {
                grd.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grd.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtGroupName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GROUP NAME CAN NOT BE EMPTY !');", true);
                txtGroupName.Focus();
                return;
            }
            string res = checkexist();
            if (res == "true")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Already Exists .');", true);
            }
            string Msg = obj.InsertMiMReportGroup(txtGroupName.Text.Trim(), ddlServer.SelectedValue);
            if (Msg == "Saved Successfully !!")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Saved Successfully !!');window.location ='MiMReportGroupandAccounts.aspx';", true);
            }
            DataTable dt = obj.GetMiMreportGroup();
            grd.DataSource = dt;
            grd.DataBind();
            GridFormat(dt);
        }

        public string checkexist()
        {
            string sql = "if exists (select * from MIMREPORTGROUP where Client='" + ddlServer.SelectedValue + "' and userid='" + txtGroupName.Text + "' ) begin select 'true' end";
            string Msg = Convert.ToString(database.GetScalarValue(sql));
            return Msg;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("MiMReportGroupandAccounts.aspx");
        }
    }
}