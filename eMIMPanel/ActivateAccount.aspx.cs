using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace eMIMPanel
{
    public partial class ActivateAccount : System.Web.UI.Page
    {
        DataTable dt;
        string proc = "AllGetInformation_BDCustomerRequest";
        List<SqlParameter> pram = new List<SqlParameter>();
        Helper.Util obj = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["NOOFHIT"] == null)
            {
                Response.Redirect("login.aspx");

            }
            if (!IsPostBack)
            {
                gridviewBind();
            }
        }

        public void gridviewBind()
        {
            pram.Clear();
            pram.Add(new SqlParameter("@action", "GetActiveAccountInfro"));
            dt = obj.GetDataTableRecord(proc,pram);
            if (dt!= null)
            {
                if (dt.Rows.Count>0)
                {
                    grv2.DataSource = dt;
                    grv2.DataBind();
                    grv2.Visible = true;

                }
                else
                {
                    grv2.DataSource = null;
                    grv2.DataBind();
                }


            }


        }

        

        protected void btnReject_Click(object sender, EventArgs e)
        {
            pnlPopUp_SCHEDULE_ModalPopupExtender.Show();
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField nid = (HiddenField)gvr.FindControl("hdnid");
            //string id = nid.Value.ToString();
            lblid.Text = Convert.ToString(nid.Value);

        }






        protected void btnAccept_Click(object sender , EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hid = (HiddenField)gvr.FindControl("hdnid");
            lblid.Text = Convert.ToString(hid.Value);
            string EID= Convert.ToString(hid.Value);
            pram.Clear();
            pram.Add(new SqlParameter("@action", "UpdateRejectAcceptInfro"));
            pram.Add(new SqlParameter("@id", lblid.Text.Trim()));
            
            pram.Add(new SqlParameter("@approve_Reject_Date", DateTime.Now.ToString()));
            pram.Add(new SqlParameter("@approve_Reject", "A"));
            string msg = obj.ExecuteRecord(proc, pram);

            if (msg.Contains("SuccessFully"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('SuccessFully Approve ');", true);
                Response.Redirect("CreateAccount.aspx?id=" + EID);
                //return;

            }


        }

        

        protected void btnCancelSch_Click(object sender, EventArgs e)
        {
            pnlPopUp_SCHEDULE_ModalPopupExtender.Hide();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //LinkButton btn = (LinkButton)sender;
            //GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //HiddenField nid = (HiddenField)gvr.FindControl("hdnid");
            //string id = nid.Value.ToString();
            

            pram.Clear();
            pram.Add(new SqlParameter("@action", "UpdateRejectAcceptInfro"));
            pram.Add(new SqlParameter("@id", lblid.Text.Trim()));
            pram.Add(new SqlParameter("@Reject_resion", txtRejReason.Text.Trim()));
            pram.Add(new SqlParameter("@approve_Reject_Date", DateTime.Now.ToString()));
            pram.Add(new SqlParameter("@approve_Reject", "R"));
            string msg = obj.ExecuteRecord(proc, pram);
            if (msg.Contains("SuccessFully"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('SuccessFully Reject ');", true);
                return;

            }
        }
    }
}