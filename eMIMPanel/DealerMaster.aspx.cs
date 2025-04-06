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
    public partial class DealerMaster : System.Web.UI.Page
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
                BindLocation(dt1);
                subLocation(dt1);
            }
            DataTable dt = obj.GetDealerMast("");
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

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string DLRCODE = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_DLRCODE") as HiddenField).Value);
            DataTable dt = obj.GetDealerMast(DLRCODE);
            if (dt.Rows.Count > 0)
            {
                ddlCategory.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                ddlLocation.SelectedItem.Text = dt.Rows[0]["LocationName"].ToString();
                ddlSubLocation.SelectedItem.Text = dt.Rows[0]["SubLocationName"].ToString();
                txtDelCode.Text = dt.Rows[0]["DLRCODE"].ToString();
                txtDelName.Text = dt.Rows[0]["DLRName"].ToString();
                txtDelCode.ReadOnly = true;
                ddlCategory.Attributes.Add("Style", "pointer-events:none;");
                ddlLocation.Attributes.Add("Style", "pointer-events:none;");
                ddlSubLocation.Attributes.Add("Style", "pointer-events:none;");
                btnSubmit.Text = "UPDATE";
            }
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = obj.getSubLocation(ddlLocation.SelectedValue);
            subLocation(dt);
        }

        public void subLocation(DataTable dt)
        {
            ddlSubLocation.DataSource = dt;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objlst = new ListItem("SELECT", "0");
            ddlSubLocation.Items.Insert(0, objlst);
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
                if (ddlSubLocation.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Sub Loaction !!');", true);
                    return;
                }
            }
            if (txtDelCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Dealer Code Can Not be Empty !!');", true);
                return;
            }
            if (txtDelName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Dealer Name Can Not be Empty !!');", true);
                return;
            }
            string rescode = obj.CheckDubliDealerMast(txtDelCode.Text.Trim(), "");
            string resname = obj.CheckDubliDealerMast("", txtDelName.Text.Trim());
            if (btnSubmit.Text != "UPDATE")
            {
                if (rescode != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Dealer Code Already Exists !!');", true);
                    return;
                }
            }
            if (resname != "")
            {
                if (btnSubmit.Text != "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Dealer Name Already Exists !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated Successfully !!');window.location ='DealerMaster.aspx';", true);
                    return;
                }
            }
            string Msg = "";
            if (btnSubmit.Text == "UPDATE")
            {
                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", ddlLocation.SelectedValue, "", ddlSubLocation.SelectedValue, "", txtDelCode.Text.Trim(), txtDelName.Text.Trim(), "UPDATE_DEALERMAST");
            }
            else
            {

                Msg = obj.INSERTHEROMAST("", "", ddlCategory.SelectedValue, "", ddlLocation.SelectedValue, "", ddlSubLocation.SelectedValue, "", txtDelCode.Text.Trim(), txtDelName.Text.Trim(), "INSERT_DEALERMAST");
            }
            if (Msg != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='DealerMaster.aspx';", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("DealerMaster.aspx");
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = obj.getLocation(ddlCategory.SelectedValue);
            BindLocation(dt);
        }
        public void BindLocation(DataTable dt)
        {
            ddlLocation.DataSource = dt;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objlst = new ListItem("SELECT", "0");
            ddlLocation.Items.Insert(0, objlst);
        }
    }
}