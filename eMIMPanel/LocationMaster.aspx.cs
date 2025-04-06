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
    public partial class LocationMaster : System.Web.UI.Page
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
                bindCategory();
            }
            DataTable dt = obj.GetLocationMast("");
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

        public void bindLocation()
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Group Location !!');", true);
                return;
            }
            if (txtLocCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Location Code Can Not be Empty !!');", true);
                return;
            }
            if (txtLocName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Location Name Can Not be Empty !!');", true);
                return;
            }
            string rescode = obj.CheckDubliLocationMast(txtLocCode.Text.Trim(),"");
            string resname = obj.CheckDubliLocationMast("", txtLocName.Text.Trim());
            if (btnSubmit.Text != "UPDATE")
            {
                if (rescode != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Location Code Already Exists !!');", true);
                    return;
                }
            }
            if (resname != "")
            {
                if (btnSubmit.Text != "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Location Name Already Exists !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully !!');window.location ='LocationMaster.aspx';", true);
                    return;
                }
            }
            string Msg = "";
            if (btnSubmit.Text == "UPDATE")
            {
                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", txtLocCode.Text.Trim(), txtLocName.Text.Trim(), "", "", "", "", "UPDATE_LOCATIONMASTER");
            }
            else
            {

                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", txtLocCode.Text.Trim(), txtLocName.Text.Trim(), "", "", "", "", "INSERT_LOCATIONMASTER");
            }
            if (Msg != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='LocationMaster.aspx';", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("LocationMaster.aspx");
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string LocationID = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_LocationID") as HiddenField).Value);
            DataTable dt = obj.GetLocationMast(LocationID);
            if (dt.Rows.Count > 0)
            {
                ddlCategory.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                txtLocCode.Text = dt.Rows[0]["LocationID"].ToString();
                txtLocName.Text = dt.Rows[0]["LocationName"].ToString();
                txtLocCode.ReadOnly = true;
                ddlCategory.Attributes.Add("Style","pointer-events:none;");
                btnSubmit.Text = "UPDATE";
            }
        }
    }
}