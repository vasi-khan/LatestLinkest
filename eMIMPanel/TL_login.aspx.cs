using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Reflection;

namespace eMIMPanel
{
    public partial class TL_login : System.Web.UI.Page
    {
        Helper.common obj = new Helper.common();
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Lang"]== null)
            {
                Session["Lang"] = ddLang.SelectedValue;

            }
            if (!IsPostBack)
            {
                Session["Time"] = DateTime.Now.ToString();
                Session["User"] = null;

            }


        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["Time"] = Session["Time"];
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert()","alert('Enter a Email');",true);
                return;

            }
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert()","alert('Enter a Password');",true);
                return;

            }
            if (Convert.ToString(ViewState["Time"])== Convert.ToString(Session["Time"]))
            {
                Helper.common.mlogin mlog = new Helper.common.mlogin();
                DataTable dlt = obj.EmployeeLogin(txtEmail.Text.Trim(), txtPassword.Text.Trim());
                if (dlt!= null)
                {
                    if (dlt.Rows.Count>0)
                    {
                        mlog.usernmae = dlt.Rows[0]["USER"].ToString().Trim();
                        mlog.role = dlt.Rows[0]["RoleCode"].ToString().Trim();
                        mlog.employeeid = int.Parse(dlt.Rows[0]["employeeid"].ToString().Trim());
                        mlog.name = dlt.Rows[0]["Name"].ToString().Trim();
                        Session["USER"] = mlog;
                        Session["Lang"] = dlt.Rows[0]["LangCode"].ToString().Trim();
                        Session["Role"] = dlt.Rows[0]["RoleCode"].ToString().Trim();

                        if (mlog.role.ToString()=="E")
                        {
                            Response.Redirect("Dashboard.aspx");

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                            return;
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Credentials.');", true);
                        return;
                    }
                    Session["Time"] = DateTime.Now.ToString();

                }

             



            }
            else
            {
                Response.Redirect("TL_Login.aspx");

            }

        }

        protected void ddLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Lang"] = ddLang.SelectedValue;
            LoadString();
        }
        private void LoadString()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("eMIMPanel.App_GlobalResources2.Lang", Assembly.GetExecutingAssembly()); //we configure resource manages for mapping with resource files in App_GlobalResources folder.  
            ci = Thread.CurrentThread.CurrentCulture;

            lblheading.InnerText = rm.GetString("SignIn", ci);
            txtEmail.Attributes.Add("placeholder", rm.GetString("UserId", ci));
            txtPassword.Attributes.Add("placeholder", rm.GetString("Password", ci));

            btnLogin.Text = rm.GetString("Login", ci);
        }
    }
}