using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class ChangePWD_ADMIN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                Response.Redirect("Login.aspx");

            string usr = Session["User"].ToString();

            if (!IsPostBack)
            {
                divVerify.Attributes.Add("Style", "display:block;");
                divPwd.Attributes.Add("Style", "display:none;");
                divProcess.Attributes.Add("Style", "display:none;");
            }
                Timer1.Interval = 5000;
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            //generate a short url with (v_ + 6) char and send to registered mobile number of account
            //insert into VerifyshortURL with mobile no, sendtime, shorturl, validhitrecdtime, hitrecdtime
            //hide the verify panel and open the waiting panel.
            //enable the timer and check the hitrecetime in the VerifyshortURL in every 5 seconds. If found, then 
            //hide the waiting panel and open the new password panel and disable the timer.
            Helper.Util ob = new Helper.Util();
            string usr = Session["User"].ToString();
            string sURL = "v-" + Guid.NewGuid().ToString().Substring(0, 6);
            Session["VERIFYLINK"] = sURL;

            ob.SaveAndSendVerificationLink(usr, Convert.ToString(Session["MOBILE"]), sURL, Convert.ToString(Session["DOMAINNAME"]), Session["PWD"].ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS containg Link to reset the password has been sent to your registered mobile number.');", true);

            divVerify.Attributes.Add("Style", "display:none;");
            divPwd.Attributes.Add("Style", "display:none;");
            divProcess.Attributes.Add("Style", "display:block;");
            Timer1.Interval = 5000;
            Session["STARTTIMER"] = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
            Timer1.Enabled = true;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["STARTTIMER"] == null)
                {
                    Timer1.Enabled = false;
                    divVerify.Attributes.Add("Style", "display:block;");
                    divPwd.Attributes.Add("Style", "display:none;");
                    divProcess.Attributes.Add("Style", "display:none;");
                    return;
                }
                else
                {
                    string st = Convert.ToString(Session["STARTTIMER"]);
                    DateTime dt = Convert.ToDateTime(st);
                    if(DateTime.Now > dt.AddMinutes(5))
                    {
                        Timer1.Enabled = false;
                        divVerify.Attributes.Add("Style", "display:block;");
                        divPwd.Attributes.Add("Style", "display:none;");
                        divProcess.Attributes.Add("Style", "display:none;");
                        return;
                    }
                }
                //check on the link that 
                Helper.Util ob = new Helper.Util();
                string usr = Session["User"].ToString();
                string seg = Convert.ToString(Session["VERIFYLINK"]);
                bool b = ob.GetVerifyStatus(usr, seg);
                if(b)
                {
                    Timer1.Enabled = false;
                    divVerify.Attributes.Add("Style", "display:none;");
                    divPwd.Attributes.Add("Style", "display:block;");
                    divProcess.Attributes.Add("Style", "display:none;");
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //Shishir Region Start
            bool resPanel = ValidatePassword(txtPwd.Text.Trim());
            if (resPanel == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password Must Contain A-Z,a-z,0-9 and _!@#$^*() !');", true);
                return;
            }
            //Shishir Region End

            if (txtConfPwd.Text.Trim() != txtPwd.Text.Trim() )
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password and Confirm Password must be same.');", true);
                Timer1.Enabled = false;
                divVerify.Attributes.Add("Style", "display:none;");
                divPwd.Attributes.Add("Style", "display:block;");
                divProcess.Attributes.Add("Style", "display:none;");
                return;
            }
            
            Helper.Util ob = new Helper.Util();
            string usr = Session["User"].ToString();
            ob.ChangeUserPassword(usr, txtPwd.Text.Trim());
            ob.UpdatePWD(usr, txtPwd.Text.Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password changed successfully.'); window.location = 'Login.aspx';", true);
        }
        //Shishir Region Start
        static bool ValidatePassword(string passWord)
        {
            int validConditions = 0;
            if (passWord.Length < 8) return false;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions < 3) return false;
            if (validConditions == 3)
            {
                char[] special = { '!', '@', '#', '$', '%', '^', '*', '+', '=', '-', '_', '(', ')', '{', '}', '[', ']' }; // or whatever    
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }
        //Shishir Region End
    }
}