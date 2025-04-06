using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillCountry();
            }
        }
        public void fillCountry()
        {
            DataTable dt = new Util().GetCountryForSignUp();

            ddlCountryCode.DataSource = dt;
            ddlCountryCode.DataValueField = "counryCode";
            ddlCountryCode.DataTextField = "country";
            ddlCountryCode.DataBind();

            //ListItem objListItem = new ListItem("--Select--", "0");
            //ddlCountryCode.Items.Insert(0, objListItem);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";
                msg.InnerText = "Please enter name.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtName.Focus();
                return;
            }
            if (ddlCountryCode.SelectedValue.Trim() == "0")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please select country.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                ddlCountryCode.Focus();
                return;
            }
            if (txtMobile.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter mobile no.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtMobile.Focus();
                return;
            }
            int moblen =Convert.ToInt32(database.GetScalarValue("select  mobLength from tblCountry where counryCode='"+ddlCountryCode.SelectedValue+"'"));

            if (txtMobile.Text.Trim().Length!= moblen)
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter valid mobile no.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtMobile.Focus();
                return;
            }
            if (txtEmailId.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email id.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("@"))
            { }
            else
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email address in xxxxx@yyyy.zzz format.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("."))
            { }
            else
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email address in xxxxx@yyyy.zzz format.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);

                txtEmailId.Focus();
                return;
            }

            if (txtCompany.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter company name.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtCompany.Focus();
                return;
            }

            //if (txtDesignation.Text.Trim() == "")
            //{
            //    sptitle.InnerText = "Warning";

            //    msg.InnerText = "Please enter designation.";

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
            //    txtDesignation.Focus();
            //    return;
            //}
            string str = new Helper.Util().InsertSignUp(txtName.Text.Trim(), txtMobile.Text.Trim(), txtEmailId.Text.Trim(), txtDesignation.Text.Trim(), txtCompany.Text.Trim(),ddlCountryCode.SelectedValue);


            msg.InnerText = str;


            if (str.Contains("successfully"))
            {
                sptitle.InnerText = "Success";

                txtName.Text = ""; txtMobile.Text = ""; txtCompany.Text = "";  txtEmailId.Text = ""; txtDesignation.Text = "";//ddlCountryCode.SelectedValue = "0";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);


        }

    }
}