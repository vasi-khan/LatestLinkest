using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace eMIMPanel
{
    public partial class approve_template : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            
            //if (usertype != "SYSADMIN")
            //{ 
            //    Response.Redirect("index.aspx");
            //}
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
        }


        public void btnViewLink_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            HiddenField filename = (HiddenField)gvro.FindControl("hdnfilepath");
            if (string.IsNullOrEmpty(filename.Value))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('No Preview Available.');", true);
                return;
            }

            //string ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/");
            string url = ResolveUrl("~\\TemplateDoc\\" + filename.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            HiddenField filename = (HiddenField)gvro.FindControl("hdnfilepath");

            string url = ResolveUrl("~\\TemplateDoc\\" + filename.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);
            GetData();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");

            string templateId = ((Label)gvro.FindControl("lbltemplateID")).Text;
            Label lbltemplte = (Label)gvro.FindControl("lbltemplate");
            string senderId = ((HiddenField)gvro.FindControl("hdnSenderId")).Value;
            string templateName = ((HiddenField)gvro.FindControl("hdnTemplateName")).Value;

            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            ob.ApproveRejectTemplate(lbltemplte.Text, username.Value, user, "APPROVE", senderId, templateName, templateId);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Template Approved Successfully.');", true);
            GetData();
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            string templateId = ((Label)gvro.FindControl("lbltemplateID")).Text;
            Label lbltemplte = (Label)gvro.FindControl("lbltemplate");
            string senderId = ((HiddenField)gvro.FindControl("hdnSenderId")).Value;
            string templateName = ((HiddenField)gvro.FindControl("hdnTemplateName")).Value;

            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            ob.ApproveRejectTemplate(lbltemplte.Text, username.Value, user, "REJECT", senderId, templateName, templateId);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Template Rejected Successfully.');", true);
            GetData();
        }
        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            
            if (hdntxtFrm.Value.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be Empty !');", true);
                return;
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                
            }
            if (hdntxtTo.Value.Trim()=="")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date can not be Empty !');", true);
                return;
            }
            else
            {
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                return;
            }
            if (Convert.ToDateTime(s2) < Convert.ToDateTime(s1))
            {
                hdntxtFrm.Value = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            string dltno = Convert.ToString(Session["DLT"]);
            DataTable dt = ob.GetTemplateListForApproval(s1, s2,usertype,user, dltno);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();

            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv.TopPagerRow != null)
            {
                grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv.BottomPagerRow != null)
            {
                grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
}