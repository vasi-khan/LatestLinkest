using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;
using RestSharp;
using System.Data.SqlClient;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class Activate_Rcs : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        rcscode.Util rcob = new rcscode.Util();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            //if (usertype == "SYSADMIN")
            if (usertype != "SYSADMIN" ) Response.Redirect("Login.aspx");
            //txtCreatedAt.Text = "Account Creation Date : " + DateTime.Now.ToString("dd/MMM/yyyy");
                if (!IsPostBack)
            {
                //btnActivate.Attributes.Add("class", "btn btn-primary btn-icon-split");
                btnActivate.Visible = false;
                btnActivate.CssClass = "btn btn-primary btn-icon-split";
            }
               
        }

        protected void lnkverify_Click(object sender, EventArgs e)
        {
           int cnt=Convert.ToInt32(Convert.ToString(Helper.database.GetScalarValue("select count(1) username  from customer with (nolock) where username='" + txtuser.Text.Trim() + "'")));
            if (cnt<=0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter valid User ID');", true);
                txtuser.Focus();
                return;
            }
            int exists = Convert.ToInt32(Convert.ToString(rcscode.database.GetScalarValue("select count(1) from MapSMSAcc with (nolock) where SMSAccId='" + txtuser.Text.Trim() + "'")));
            if (exists > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Already exists');", true);
                txtuser.Focus();
                return;
            }
            if (txtuser.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
                    return;
                }
                if (txtAccountID.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account ID can not be blank.');", true);
                    return;
                }
                if (txtapikey.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('API Key can not be blank.');", true);
                    return;
                }
                if (txtapiurl.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('API URL can not be blank.');", true);
                    return;
                }
                if (txtmobile.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Number can not be blank.');", true);
                    return;
                }
                string userid = Convert.ToString(txtuser.Text.Trim());
                string accid = Convert.ToString(txtAccountID.Text.Trim());
                string apikey = Convert.ToString(txtapikey.Text.Trim());
                string apiurl = Convert.ToString(txtapiurl.Text.Trim());
                string mobile = Convert.ToString(txtmobile.Text.Trim());
                int msg = ob.RCSApiAN(mobile, apikey, "Hello", userid, accid, apiurl);
                Session["msg"] = Convert.ToString(msg);
                if (Convert.ToInt32(msg) > 0)
                {
                    btnActivate.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Verify Successfully !!');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential !!');", true);
                    return;
                }
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            int cnt = Convert.ToInt32(Convert.ToString(Helper.database.GetScalarValue("select count(1) username  from customer with (nolock) where username='" + txtuser.Text.Trim() + "'")));
            if (cnt <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter valid User ID');", true);
                txtuser.Focus();
                return;
            }
            int exists = Convert.ToInt32(Convert.ToString(rcscode.database.GetScalarValue("select count(1) from MapSMSAcc with (nolock) where SMSAccId='" + txtuser.Text.Trim() + "'")));
            if (exists > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Already exists');", true);
                txtuser.Focus();
                return;
            }
            if (Convert.ToInt32(Session["msg"]) > 0)
            {
                DataTable dt = new DataTable();
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_CreateRCSAccount";

                    cmd.Parameters.AddWithValue("Userid", txtuser.Text.Trim());
                    cmd.Parameters.AddWithValue("ApiAccId", txtAccountID.Text.Trim());
                    cmd.Parameters.AddWithValue("ApiKey", txtapikey.Text.Trim());
                    cmd.Parameters.AddWithValue("apiUrl", txtapiurl.Text.Trim());
                    cmd.Parameters.AddWithValue("Mobile", txtmobile.Text.Trim());
                    cmd.Parameters.AddWithValue("Msg", "");
                    cmd.Parameters["Msg"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["Msg"].Size = 0x100;
                    cmd.ExecuteNonQuery();
                    string result = Convert.ToString(cmd.Parameters["Msg"].Value.ToString());
                    if (result.Contains("Succesfully"))
                    {
                        
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Activated Successfully !!');", true);
                        Reset();
                        return;
                        

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential !!');", true);
                        return;
                    }
                    
                }
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User Credential !!');", true);
                return;
            }
            
        }

        public void Reset()
        {
            txtuser.Text = "";
            txtAccountID.Text = "";
            txtapikey.Text = "";
            txtapiurl.Text = "";
            txtmobile.Text = "";
        }

        protected void lnkEditAC_Click(object sender, EventArgs e)
        {
            pnlMain.Visible = false;
            pnledit.Visible = true;
            lnkEditAC.Visible = false;
        }

        protected void lnkShowAC_Click(object sender, EventArgs e)
        {
            if (txtAccId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter User ID to EDIT.');", true);
                return;
            }
            DataTable dt = rcob.GetRCSUserParameterToEdit(txtAccId.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                Session["EDITUSER"] = txtAccId.Text.Trim();
                txtrcsaccid.Text = Convert.ToString(dt.Rows[0]["RCSACCID"]);
                txtrcsapikey.Text = Convert.ToString(dt.Rows[0]["RCSAUTHKEY"]);
                txtrcsapiurl.Text = Convert.ToString(dt.Rows[0]["RCSURL"]);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid User ID.');", true);
                return;
            }
        }

        protected void lnkbtntoedit_Click(object sender, EventArgs e)
        {
            if (txtrcsaccid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID can not be blank.');", true);
                return;
            }
            if (txtrcsapikey.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('API Key can not be blank.');", true);
                return;
            }
            if (txtrcsapiurl.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('API URL can not be blank.');", true);
                return;
            }
            try
            {
                rcob.GetRCSUserToEdit(Convert.ToString(Session["EDITUSER"]), txtrcsaccid.Text.Trim(), txtrcsapikey.Text.Trim(), txtrcsapiurl.Text.Trim());
                ResetEditDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Updated Successfully !!');", true);
                return;
            }
            catch (Exception ex)
            {

            }
            
        }
        public void ResetEditDetails()
        {
            txtAccId.Text = "";
            txtrcsaccid.Text = "";
            txtrcsapikey.Text = "";
            txtrcsapiurl.Text = "";
        }
    }
}