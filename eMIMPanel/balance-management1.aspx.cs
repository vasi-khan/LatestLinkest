using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class balance_management1 : System.Web.UI.Page
    {
        protected String LabelProperty
        {
            get
            {
                return hidden.Value;
            }
            set
            {
                hidden.Value = value;
            }
        }
        protected String LabelProperty2
        {
            get
            {
                return hidden2.Value;
            }
            set
            {
                hidden2.Value = value;
            }
        }
        protected String s1
        {
            get
            {
                return hs1.Value;
            }
            set
            {
                hs1.Value = value;
            }
        }
        protected String s2
        {
            get
            {
                return hs2.Value;
            }
            set
            {
                hs2.Value = value;
            }
        }
        protected String s3
        {
            get
            {
                return hs3.Value;
            }
            set
            {
                hs3.Value = value;
            }
        }
        protected String s4
        {
            get
            {
                return hs4.Value;
            }
            set
            {
                hs4.Value = value;
            }
        }
        protected String s5
        {
            get
            {
                return hdnUrlRate.Value;
            }
            set
            {
                hdnUrlRate.Value = value;
            }
        }
        protected String s6
        {
            get
            {
                return hdnDltRate.Value;
            }
            set
            {
                hdnDltRate.Value = value;
            }
        }
        string _user;
        string usertype;
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            _user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            //if(usertype == "ADMIN") Response.Redirect("balance-management1.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string un = LabelProperty;
                string bal = txtbal.Text;
                if (txtbal.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Balance should not be blank.');", true);
                    return;
                }

                DataTable dt = ob.GetUserParameter(_user);
                Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();

                if (rdbCredit.Checked && Convert.ToDouble(txtbal.Text.Trim()) > Convert.ToDouble(Session["SMSBAL"]))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient balance to credit.');", true);
                    return;
                }

                if (rdbCredit.Checked == false)
                {
                    DataTable dt1 = ob.GetUserParameter(un);
                    string bl = dt1.Rows[0]["balance"].ToString();
                    if (Convert.ToDouble(txtbal.Text.Trim()) > Convert.ToDouble(bl))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient balance of user to debit.');", true);
                        return;
                    }
                }

                string cd = (rdbCredit.Checked ? "C" : "D");
                string r = ob.SaveCrDrBalance(un, bal, cd, _user, usertype,"");

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + r + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Balance Updated Successfully');window.location ='balance-management1.aspx';", true);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
            }
        }
        protected void btnUpdateRate_Click(object sender, EventArgs e)
        {
            string un = LabelProperty2;
            string s1o = s1;
            string s2o = s2;
            string s3o = s3;
            string s4o = s4;
            string s5o = s5;
            string d1o = s6;
            string s1n = (txtns1.Text.Trim() == "" ? s1: txtns1.Text);
            string s2n = (txtns2.Text.Trim() == "" ? s2: txtns2.Text);
            string s3n = (txtns3.Text.Trim() == "" ? s3: txtns3.Text);
            string s4n = (txtns4.Text.Trim() == "" ? s4: txtns4.Text);
            string s5n = (txtUrlRateN.Text.Trim() == "" ? s5o : txtUrlRateN.Text);
            string d1n = (txtnd1.Text.Trim() == "" ? d1o : txtnd1.Text);
            //if (txtns1.Text.Trim() == "" && txtns2.Text.Trim() == "" && txtns3.Text.Trim() == "" && txtns4.Text.Trim() == "" && txtUrlRateN.Text.Trim() == "" && txtnd1.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter new price.');", true);
            //    return;
            //}
            string user = Convert.ToString(Session["User"]);
            string r = ob.UpdateSMSPrice(un, s1o, s2o, s3o, s4o, s1n, s2n, s3n, s4n, s5o, s5n, d1o, d1n, user);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + r + "');", true);

        }

    }
}