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
    public partial class EventMaster : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            DataTable dt = obj.GetEventMast("");
            grd.DataSource = dt;
            grd.DataBind();
            if (dt.Rows.Count > 0)
            {
                GridFormat(dt);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtEvntCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event Code Can Not be Empty !!');", true);
                return;
            }
            if (txtEvntName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event Name Can Not be Empty !!');", true);
                return;
            }
            string rescode = obj.CheckDubliEventMast(txtEvntCode.Text.Trim(), "");
            string resname = obj.CheckDubliEventMast("",txtEvntName.Text.Trim());
            if (btnSubmit.Text != "UPDATE")
            {
                if (rescode != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event Code Already Exists !!');", true);
                    return;
                }
            }

            if (resname != "")
            {
                if (btnSubmit.Text != "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event Name Already Exists !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully !!');window.location ='EventMaster.aspx';", true);//If Dublicate Found 
                    return;
                }
            }
            string Msg = "";
            if (btnSubmit.Text == "UPDATE")
            {
                Msg = obj.INSERTHEROMAST(txtEvntCode.Text.Trim(), txtEvntName.Text.Trim(), "", "", "", "", "", "", "", "", "UPDATE_EVENTMAST");
            }
            else
            {

                Msg = obj.INSERTHEROMAST(txtEvntCode.Text.Trim(), txtEvntName.Text.Trim(), "", "", "", "", "", "", "", "", "INSERT_EVENTMAST");
            }
            if (Msg != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='EventMaster.aspx';", true);
            }

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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("EventMaster.aspx");
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string EventID = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_EventID") as HiddenField).Value);
            DataTable dt = obj.GetEventMast(EventID);
            if (dt.Rows.Count > 0)
            {
                txtEvntCode.Text = dt.Rows[0]["EventID"].ToString();
                txtEvntName.Text = dt.Rows[0]["EventName"].ToString();
                txtEvntCode.ReadOnly = true;
                btnSubmit.Text = "UPDATE";
            }
        }
    }
}