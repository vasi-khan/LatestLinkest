using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class CreateAccountAdmin : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            //if (usertype != "SYSADMIN" && usertype != "ADMIN") Response.Redirect("CreateAccountAdmin.aspx");
            txtCreatedAt.Text = "Account Creation Date : " + DateTime.Now.ToString("dd/MMM/yyyy");
            if (!IsPostBack)
            {
                txtExpiry.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");
                ddlMiMReportGroupBind();
            }
            //txtCreatedAt.Enabled = false;
        }
        public void ddlMiMReportGroupBind()
        {
            DataTable dt = ob.getMiMReportGroup();
            ddlMiMReportGroup.DataSource = dt;
            ddlMiMReportGroup.DataTextField = "client";
            ddlMiMReportGroup.DataValueField = "client";
            ddlMiMReportGroup.DataBind();
            ddlMiMReportGroup.Items.Insert(0, new ListItem("OTHERS", "0"));

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ddlUserType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select User Type.');", true);
                return;
            }
            if (txtSender.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID can not be blank.');", true);
                return;
            }
            //if (txtSender.Text.Trim().Length != 6)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID must be of 6 Alphabets.');", true);
            //    return;
            //}
            if (ddlSMSType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select SMS Type.');", true);
                return;
            }
            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name can not be blank.');", true);
                return;
            }
            if (ddlActType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Account Type.');", true);
                return;
            }
            if (ddlPermission.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Permission.');", true);
                return;
            }
            if (txtCompName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Company Name can not be blank.');", true);
                return;
            }

            if (txtPEID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('PE-ID can not be blank.');", true);
                return;
            }
            //if (txtName.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name can not be blank.');", true);
            //    return;
            //}
            if (txtMob1.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile 1 can not be blank.');", true);
                return;
            }
            else
            {
                if (txtMob1.Text.Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile number must be minimum of 10 digits');", true);
                    return;
                }
                else if (txtMob1.Text.Length < 9 && ddlCountryCode.SelectedValue == "971")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile number must be minimum of 9 digits');", true);
                    return;
                }
                else
                {
                    if (txtMob1.Text.Length == 10 || (txtMob1.Text.Length == 9 && ddlCountryCode.SelectedValue == "971")) txtMob1.Text = ddlCountryCode.SelectedValue + txtMob1.Text;
                    // else txtMob1.Text = ddlCountryCode.SelectedValue + txtMob1.Text;

                }
            }


            //if (txtMob2.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile 2 can not be blank.');", true);
            //    return;
            //}
            if (txtEmail.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email can not be blank.');", true);
                return;
            }
            if (txtExpiry.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Expiry can not be blank.');", true);
                return;
            }

            if (txtgroupname.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group name can not be blank.');", true);
                return;
            }
            //if (txtDLT.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('DLT No can not be blank.');", true);
            //    return;
            //}
            //check duplicate mobile or email
            //if (ob.CheckMobileEmailDuplicate(txtMob1.Text,"M"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Mobile Number has already been used for an existing Account.');", true);
            //    return;
            //}
            //if (ob.CheckMobileEmailDuplicate(txtEmail.Text,"E"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Email ID has already been used for an existing Account.');", true);
            //    return;
            //}
            bool IsShowcurrency = true;
            if (chkIsShowcurrency.Checked)
            {
                IsShowcurrency = true;
            }
            else
            {
                IsShowcurrency = false;
            }
            string AccountCreationType = "";
            if (rbPostpaid.Checked == true)
            {
                AccountCreationType = "PostPaid";
            }
            else
            {
                AccountCreationType = "Prepaid";
            }
            string TranorPromo = "";
            if (rbTrans.Checked == true)
            {
                TranorPromo = "Trans";
            }
            else
            {
                TranorPromo = "Promo";
            }
            string Client = ddlMiMReportGroup.SelectedItem.Text;
            string res = ob.SaveCustomer(AccountCreationType, txtSender.Text, txtName.Text, txtCompName.Text, txtWebsite.Text, txtMob1.Text, ddlUserType.Text, txtEmail.Text, (chkExpiry.Checked ? "31/Dec/2050" : txtExpiry.Text), txtDLT.Text, ddlSMSType.SelectedValue, ddlActType.SelectedValue, ddlPermission.SelectedValue, ddlCountryCode.SelectedValue, user, usertype, IsShowcurrency,txtgroupname.Text.Trim(),txtPEID.Text.Trim(),txtccemail.Text.Trim(), TranorPromo, ddlMiMReportGroup.SelectedValue.Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
            pnlMain.Enabled = false;
            //Response.Redirect("~/CreateAccount.aspx");
        }
    }
}