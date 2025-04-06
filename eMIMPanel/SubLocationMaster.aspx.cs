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
    public partial class SubLocationMaster : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                DataTable dt1 = null;
                bindCategory();
                BidLocation(dt1);
            }
            DataTable dt = obj.GetSubLocationMast("");
            grd.DataSource = dt;
            grd.DataBind();
            if (dt.Rows.Count > 0)
            {
                GridFormat(dt);
            }
        }

        public void bindCategory()
        {
            DataTable dt = obj.GetCategoryMast("");
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ListItem objlst = new ListItem("SELECT", "0");
            ddlCategory.Items.Insert(0, objlst);
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

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = obj.getLocation(ddlCategory.SelectedValue);
            BidLocation(dt);
        }

        public void BidLocation(DataTable dt)
        {
            ddlLocation.DataSource = dt;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objlst = new ListItem("SELECT", "0");
            ddlLocation.Items.Insert(0, objlst);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmit.Text != "UPDATE")
            {
                if (ddlCategory.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Group Location !!');", true);
                    return;
                }
                if (ddlLocation.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Loaction !!');", true);
                    return;
                }
            }
            if (txtSubLocCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sub Location Code Can Not be Empty !!');", true);
                return;
            }
            if (txtSubLocName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sub Location Name Can Not be Empty !!');", true);
                return;
            }
            string rescode = obj.CheckDubliSubLocationMast(txtSubLocCode.Text.Trim(), "");
            string resname = obj.CheckDubliSubLocationMast("", txtSubLocName.Text.Trim());
            if (btnSubmit.Text != "UPDATE")
            {
                if (rescode != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sub Location Code Already Exists !!');", true);
                    return;
                }
            }
            if (resname != "")
            {
                if (btnSubmit.Text != "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sub Location Name Already Exists !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully !!');window.location ='SubLocationMaster.aspx';", true);
                    return;
                }
            }
            string Msg = "";
            if (btnSubmit.Text == "UPDATE")
            {
                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", ddlLocation.SelectedValue, "", txtSubLocCode.Text.Trim(), txtSubLocName.Text.Trim(), "", "", "UPDATE_SUBLOCATIONMASTER");
            }
            else
            {

                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", ddlLocation.SelectedValue, "", txtSubLocCode.Text.Trim(), txtSubLocName.Text.Trim(), "", "", "INSERT_SUBLOCATIONMASTER");
            }
            if (Msg != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='SubLocationMaster.aspx';", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("SubLocationMaster.aspx");
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string SubLocationID = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_SubLocationID") as HiddenField).Value);
            DataTable dt = obj.GetSubLocationMast(SubLocationID);
            if (dt.Rows.Count > 0)
            {
                ddlCategory.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                ddlLocation.SelectedItem.Text = dt.Rows[0]["LocationName"].ToString();
                txtSubLocCode.Text = dt.Rows[0]["SubLocationID"].ToString();
                txtSubLocName.Text = dt.Rows[0]["SubLocationName"].ToString();
                txtSubLocCode.ReadOnly = true;
                ddlCategory.Attributes.Add("Style", "pointer-events:none;");
                ddlLocation.Attributes.Add("Style", "pointer-events:none;");
                btnSubmit.Text = "UPDATE";
            }
        }
    }
}