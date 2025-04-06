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
    public partial class Hyundai_InsuranceSubClientMaster : System.Web.UI.Page
    {
        Helper.Util db = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {

            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                BindGrdiView();

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtInsuranceCompCode.Text=="")
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert","alert('Enter Insurance Company Code!! ');",true);
                return;

            }
            if (txtInsuranceCompName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Insurance Company Name!! ');", true);
                return;

            }
            string msg = "";
            if (btnSubmit.Text=="Submit")
            {
                msg = db.InsertHuundaiInsuranceSubClient(txtInsuranceCompCode.Text.Trim(), txtInsuranceCompName.Text.Trim(), "Insert");
            }
            else if (btnSubmit.Text== "Update")
            {
                msg = db.InsertHuundaiInsuranceSubClient(txtInsuranceCompCode.Text.Trim(), txtInsuranceCompName.Text.Trim(), "Update");

            }
            

            if (msg!="")
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('"+msg+ "'); window.open('Hyundai_InsuranceSubClientMaster.aspx');", true);
                BindGrdiView();
                btnSubmit.Text = "Submit";
                txtInsuranceCompCode.ReadOnly = false;
                txtInsuranceCompCode.Text = "";
                txtInsuranceCompName.Text = "";
                return;
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("Hyundai_InsuranceSubClientMaster.aspx");
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            GridViewRow gd = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int RowIndex = gd.RowIndex;
            Label CompCode = grd.Rows[RowIndex].FindControl("lblCompCode") as Label;
            Label CompName= grd.Rows[RowIndex].FindControl("lblCompName") as Label;
            txtInsuranceCompCode.Text = CompCode.Text;
            txtInsuranceCompName.Text = CompName.Text;
            btnSubmit.Text = "Update";
            txtInsuranceCompCode.ReadOnly = true;
        }

        public void BindGrdiView()
        {
            DataTable dt = new DataTable();
            dt = database.GetDataTable("Select * from mstSubClientCode");

            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    grd.DataSource = dt;
                    grd.DataBind();

                }
                else
                {
                    grd.DataSource = dt;
                    grd.DataBind();
                }
            }
        }

    }
}