using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class HeroAccountMapping : System.Web.UI.Page
    {
        Helper.smpp ob = new Helper.smpp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string sql = "select * from categorymast";
                DataTable dt = ob.DataTable(sql);
                ddlgroup.DataSource = dt;
                ddlgroup.DataTextField = "CategoryName";
                ddlgroup.DataValueField = "CategoryId";
                ddlgroup.DataBind();


            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            grv.Visible = true;
            if (txtUserID.Text != null && txtUserID.Text.Trim() != "")
            {
                if (verify_user() == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Record doesnt exist for the user ');", true);
                    return;
                }
                string sql = "select * from HeroAccountMapping inner join categorymast on categoryid=grouplocationcode where userId='"+txtUserID.Text+"'";
                DataTable dt=ob.DataTable(sql);
                grv.DataSource = dt;
                grv.DataBind();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please enter the User Id ');", true);
                return;
            }


        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (txtUserID.Text != null && txtUserID.Text.Trim() != "")
            {
                grv.DataSource = null;
                grv.Visible = false;
                grv.DataBind();
                if (insertion_check() == 1)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Record already exists');", true);
                    return;
                }
                string sql = "insert into HeroAccountMapping (userId,GroupLocationCode) values('" + txtUserID.Text + "','" + ddlgroup.SelectedValue + "')";
                Helper.database.ExecuteNonQuery(sql);
                
           }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please enter the User Id ');", true);
                return;
            }
        }

        int verify_user()
        {
            string sql= "select Isnull((select COUNT(*) from HeroAccountMapping where userId='" + txtUserID.Text + "'),0)";
            return((int)Helper.database.GetScalarValue(sql));

        }

        int insertion_check()
        {
            string sql = "select Isnull((select 1 from HeroAccountMapping where userId='" + txtUserID.Text + "' and GroupLocationCode='"+ddlgroup.SelectedValue+"'),0)";
            return ((int)Helper.database.GetScalarValue(sql));
        }

    }
}