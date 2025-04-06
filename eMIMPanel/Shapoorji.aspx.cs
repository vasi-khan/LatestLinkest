using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Shapoorji : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        public string _client = "Shapoorji";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter name.');", true);
                return;
            }
            if (txtPhone.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter mobile number.');", true);
                return;
            }
            if (txtPhone.Text.Trim().Length != 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter mobile number of 10 digit.');", true);
                return;
            }
            if (txtOTP.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter received OTP.');", true);
                return;
            }

            bool b = ob.SVHSubmit(txtName.Text, txtPhone.Text.Trim(), txtOTP.Text, _client);
            if (b)
            {
                divOTP.Style.Add("display", "none");
                divMain.Style.Add("display", "none");
                divSuccess.Style.Add("display", "block");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid OTP.');", true);
            }
        }

        protected void btnOTP_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter name.');", true);
                return;
            }
            if (txtPhone.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter mobile number.');", true);
                return;
            }
            if (txtPhone.Text.Trim().Length != 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter mobile number of 10 digit.');", true);
                return;
            }
            ob.SendSVH_OTP(txtPhone.Text.Trim(), txtName.Text.Trim(), _client);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP sent on entered mobile number.');", true);
            divOTP.Style.Add("display", "block");
            divMain.Style.Add("display", "none");
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            ob.ResendOldOTP(_client, txtPhone.Text.Trim(), txtName.Text.Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP Resend on entered mobile number.');", true);
            divOTP.Style.Add("display", "block");
            divMain.Style.Add("display", "none");
        }
    }
}