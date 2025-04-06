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
    public partial class CategoryMaster : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            DataTable dt = obj.GetCategoryMast("");
            grd.DataSource = dt;
            grd.DataBind();
            if (dt.Rows.Count > 0)
            {
                GridFormat(dt);
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

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string CategoryID = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_CategoryID") as HiddenField).Value);
            DataTable dt = obj.GetCategoryMast(CategoryID);
            if (dt.Rows.Count > 0)
            {
                txtCatCode.Text = dt.Rows[0]["CategoryID"].ToString();
                txtCatName.Text = dt.Rows[0]["CategoryName"].ToString();
                txtCatCode.ReadOnly = true;
                btnSubmit.Text = "UPDATE";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtCatCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group Location Code Can Not be Empty !!');", true);
                return;
            }
            if (txtCatName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group Location Name Can Not be Empty !!');", true);
                return;
            }
            string rescode = obj.CheckDubliCategoryMast(txtCatCode.Text.Trim(), "");
            string resname = obj.CheckDubliCategoryMast("", txtCatName.Text.Trim());
            if (btnSubmit.Text != "UPDATE")
            {
                if (rescode != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group Location Code Already Exists !!');", true);
                    return;
                }
            }
            if (resname != "")
            {
                if (btnSubmit.Text != "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group Location Name Already Exists !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully !!');window.location =' CategoryMaster.aspx';", true);
                    return;
                }
            }
            string Msg = "";
            if (btnSubmit.Text == "UPDATE")
            {
                Msg = obj.INSERTHEROMAST("", "", txtCatCode.Text.Trim(), txtCatName.Text.Trim(), "", "", "", "", "", "", "UPDATE_CATEGORYMAST");
            }
            else
            {

                Msg = obj.INSERTHEROMAST("", "", txtCatCode.Text.Trim(), txtCatName.Text.Trim(), "", "", "", "", "", "", "INSERT_CATEGORYMAST");
            }
            if (Msg != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location =' CategoryMaster.aspx';", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("CategoryMaster.aspx");
        }
    }
}