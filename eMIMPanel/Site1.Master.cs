using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _user = Convert.ToString(Session["UserID"]);

            //Shishir Start
            bool IPWhiteListing = Convert.ToBoolean(Session["IPWhiteListing"]);
            DataTable dtgetIp = Session["dtgetIp"] as DataTable;
            string IpAddress = Convert.ToString(Session["IpAddress"]);

            if (IPWhiteListing == true)
            {
                DataRow[] dr = dtgetIp.Select("IpAddress ='" + IpAddress + "'");
                if (dr.Length == 0) Response.Redirect("login.aspx");
            }
            //Shishir End
            
            Helper.Util ob = new Helper.Util();

            DataTable dt = ob.GetUserParameter(Convert.ToString(Session["User"]));

            Session["SMSBAL"] = Convert.ToString(dt.Rows[0]["balance"]);
            Session["Usertype"] = Convert.ToString(dt.Rows[0]["UserType"]);

            //DLT CHECK HERO
            if (Session["Usertype"].ToString() != "SYSADMIN")
            {
                string DLTNO = ConfigurationManager.AppSettings["DLTNO"].ToString();
                string DLTNO1 = ConfigurationManager.AppSettings["DLTNO1"].ToString();
                string DLTNO2 = ConfigurationManager.AppSettings["DLTNO2"].ToString();
                if (DLTNO != Session["DLT"].ToString() && DLTNO1 != Session["DLT"].ToString() && DLTNO2 != Session["DLT"].ToString())
                {
                    A6.Visible = false;
                }
                
            }
            //DLT CHECK HERO


            A1.Visible = false;
            A2.Visible = false;
            if (Convert.ToString(Session["Usertype"]).ToUpper() == "SYSADMIN")
            {
                divActivateAccount.Visible = true;
                campEntry.Visible = true; 
                divroute.Visible = true;
                A1.Visible = true;
                A2.Visible = true;
                Anuncdiv.Visible = false;
                Actrcsdiv.Visible = false;
            }
            if (Convert.ToString(Session["Usertype"]).ToUpper() == "ADMIN")
            {
                divroute.Visible = false;
                CustomReportDiv.Visible = false;
                campEntry.Visible = false;
                Actrcsdiv.Visible = false;
                blocknunblock.Visible = false;
                Anuncdiv.Visible = false;
                blacklistdiv.Visible = false;
                apptempdiv.Visible = true;
            }
            if (Convert.ToString(Session["Usertype"]).ToUpper() == "BD")
            {
                balmngmntdiv.Visible = false;
                sndriddiv.Visible = false;
                apptempdiv.Visible = false;
                addtempdiv.Visible = false;
                Anuncdiv.Visible = false;
                blacklistdiv.Visible = false;
                Actrcsdiv.Visible = false;
                divroute.Visible = false;
                //RouteSetting.Visible = false;
                CustomReportDiv.Visible = false;
                CustomReport.Visible = false;
                divActivateAccount.Visible = false;
                accountlistas.Visible = false;
                blocknunblock.Visible = false;
                camprpt.Visible = false;
                divreqfrm.Visible = true;
                bdacclist.Visible = true;
                AdAndSysDiv.Visible = false;
                BDCreAccDiv.Visible = true;
                campEntry.Visible = false;
            }

            if (Convert.ToString(Session["Usertype"]).ToUpper() == "OPERATOR")
            {
                divnav1.Visible = false;
                divnav2.Visible = true;
            }
            lblBal.Text = Convert.ToString(Session["SMSBAL"]);
            lblBal3.Text = Convert.ToString(Session["SMSBAL"]);
            lbluser.Text = Convert.ToString(Session["User"]);

            if (Convert.ToString(Session["Usertype"]).ToUpper() != "SYSADMIN")
            {
                string Login_Session_Id = Convert.ToString(Session["Login_Session_Id"]);
                if (Login_Session_Id != "")
                {
                    int Exists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM LoginEntry WITH(NOLOCK) WHERE UserId='" + Convert.ToString(Session["User"]) + "' AND LoginSessionID='" + Login_Session_Id.Trim() + "'"));
                    if (Exists != 1)
                    {
                        Session.Abandon();
                        Session.RemoveAll();
                        Response.Redirect("login.aspx");
                    }
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            string _user = Convert.ToString(Session["UserID"]);
            Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='0' WHERE UserId='" + Convert.ToString(Session["User"]) + "'");
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }

}