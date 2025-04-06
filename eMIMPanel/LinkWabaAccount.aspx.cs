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
    public partial class LinkWabaAccount : System.Web.UI.Page
    {
        string user = "";
        Util obj = new Util();
       string dbNameWABA = System.Configuration.ConfigurationManager.AppSettings["dbnameWABA"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                Response.Redirect("Login.aspx");
            user = Session["User"].ToString();
        }

        protected void btnVerifyAC_Click(object sender, EventArgs e)
        {
            if (ChklinkWabaAC.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Link WABA Account');", true);
                return;
            }
            if (Convert.ToString(txtUserId.Text) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Linkext Account ID !!');", true);
                return;
            }
            string query = "SELECT COUNT(*) FROM customer WHERE username = '" + txtUserId.Text.Trim() + "' AND Active = 1";
            int ExistOrNot = Convert.ToInt32(database.GetScalarValue(query));
            if (ExistOrNot > 0)
            {
                string sql = "SELECT COUNT(*) FROM customer WHERE username = '" + txtUserId.Text.Trim() + "' AND Active = 1 " +
                    "and (WabaProfileId <> '' AND WabaProfileId IS NOT NULL) AND (WabaPwd <> '' AND WabaPwd IS NOT NULL)";
                int IsVerifiedOrNot = Convert.ToInt32(database.GetScalarValue(sql));
                if (IsVerifiedOrNot > 0)
                {
                    Maindiv.Visible = true;
                    divVerified.Visible = true;
                    divVerify.Visible = false;
                }
                else
                {
                    divVerify.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Correct Linkext Account ID !!');", true);
                return;
            }
        }

        protected void LnkbtnSentOTP_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txtWabaAccountID.Text) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter WABA Account ID !!');", true);
                return;
            }
            string query = "SELECT * FROM " + dbNameWABA + ".dbo.customer WHERE username = '" + txtWabaAccountID.Text.Trim() + "' AND Active = 1";
            DataTable ExistOrNot = database.GetDataTable(query);
            if (ExistOrNot != null && ExistOrNot.Rows.Count > 0)
            {
                string sql = "SELECT COUNT(*) FROM customer WHERE username = '" + txtUserId.Text.Trim() + "' AND Active = 1 " +
                    "and (WabaProfileId <> '' AND	WabaProfileId IS NOT NULL) AND (WabaPwd <> '' AND WabaPwd IS NOT NULL)";
                int IsVerifiedOrNot = Convert.ToInt32(database.GetScalarValue(sql));
                if (IsVerifiedOrNot > 0)
                {
                    Maindiv.Visible = true;
                    divVerified.Visible = true;
                }
                else
                {
                    Random rand = new Random();
                    string vOTP = rand.Next(100000, 999999).ToString();
                    ViewState["OTP"] = vOTP;
                    string result = obj.SendOTPOnWABA(Convert.ToString(ExistOrNot.Rows[0]["MOBILE1"]),vOTP);
                    if (result.Contains("Success"))
                    {
                        Maindiv.Visible = true;
                        divVerifyOTP.Visible = true;
                        divVerified.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP Sent Successfully !!');", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Correct WABA Account ID !!');", true);
                return;
            }
        }

        protected void LnkbtnSubmit_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txtOTP.Text) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter OTP !!');", true);
                return;
            }

            if (Convert.ToString(txtOTP.Text.Trim()) == Convert.ToString(ViewState["OTP"]))
            {
                string WabaProfileId = "";
                string WabaAPIKEY = Convert.ToString(database.GetScalarValue(@"SELECT APIKEY FROM " + dbNameWABA + ".dbo.customer WHERE username='" + txtWabaAccountID.Text.Trim() + "' and Active=1"));
                string sql = @"update customer set WabaProfileId='" + txtWabaAccountID.Text.Trim() + "',WabaPwd='" + WabaAPIKEY + "' WHERE username='" + txtUserId.Text.Trim() + "'";
                try
                {
                    database.ExecuteNonQuery(sql);
                    divVerified.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP Verified Successfully !!');", true);
                    return;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Valid OTP !!');", true);
                return;
            }
        }
    }
}