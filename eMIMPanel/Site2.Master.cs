using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;
namespace eMIMPanel
{
    public partial class Site2 : System.Web.UI.MasterPage
    {
        string currency = "Rs. ";
        public string lblbalance
        {
            get
            {
                return lblBal.Text;
            }
            set
            {
                lblBal.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string _user = Convert.ToString(Session["UserID"]);
                if (_user == "")
                {
                    Response.Redirect("sessionout.aspx");
                }

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

                //if (Session["UserID"] == null) return;
                currency = Convert.ToString(Session["CURRENCY"]);

                Helper.Util ob = new Helper.Util();
                //check logindtl table and update lastactivitytime.
                //ob.UpdateLastActivity(Convert.ToString(Session["UserID"]));

                DataTable dt = ob.GetUserParameter(Convert.ToString(Session["UserID"]));

                Session["SMSBAL"] = dt.Rows[0]["balance"].ToString();
                string Extrabal = dt.Rows[0]["extrabal"].ToString();
                Session["NOOFHIT"] = dt.Rows[0]["NOOFHIT"].ToString();
                if (bool.Parse(dt.Rows[0]["Isshowcurrency"].ToString().Trim()) == false)
                {
                    currency = "";
                }
                //lblBal.Text = currency + " " + Convert.ToString(Session["SMSBAL"]);
                lblBal.Text = currency + " " + Convert.ToString(Convert.ToDecimal(Convert.ToString(Session["SMSBAL"])) + Convert.ToDecimal(Extrabal));
                lblClickBal.Text = Convert.ToString(Session["NOOFHIT"]);
                lblBal2.Text = currency + " " + Convert.ToString(Session["SMSBAL"]);
                lblClickBal2.Text = Convert.ToString(Session["NOOFHIT"]);
                lbluser.Text = Convert.ToString(Session["UserID"]);

                if (Convert.ToString(Session["WABARCS"]) == "TRUE")
                    divWARCS.Visible = true;
                else
                    divWARCS.Visible = false;

                if (dt.Rows[0]["IsRcsActive"].ToString().ToUpper() == "TRUE")
                {
                    divrcs.Visible = true;
                }
                else
                {
                    divrcs.Visible = false;
                }

                lblSMSCount.Text = Convert.ToString(database.GetScalarValue("select dbo.[GetSMSCount]('" + Convert.ToString(Session["UserID"]) + "')"));
                if (bool.Parse(dt.Rows[0]["IsShowSMSCount"].ToString().Trim()) == false)
                {
                    dvSMSCount.Visible = false;
                }
                else
                {
                    dvSMSCount.Visible = true;
                }

                divSMSbalance.Visible = bool.Parse(dt.Rows[0]["ISSHOWBALANCE"].ToString().Trim());
                if (Session["UserID"].ToString().ToUpper() == "MIM2201048" || Session["UserID"].ToString().ToUpper() == "MIM2201104" || Session["UserID"].ToString().ToUpper() == "MIM2400116")
                {
                    ancerSummary.Visible = true;
                }
                if (Session["UserID"].ToString().ToUpper() == "MIM2201185")
                {
                    ancerDLRReport.Visible = false;
                    ancercampaignRpt.Visible = false;
                }
                if (Session["UserID"].ToString().ToUpper() == "MIM2002097")
                {
                    dealerRpt.Visible = true;
                }
                if (Session["UserID"].ToString().ToUpper() == "MIM2201010" || Session["UserID"].ToString().ToUpper() == "MIM2201011" || Session["UserID"].ToString().ToUpper() == "MIM2201009")
                {
                    ancersmsreport2.Visible = true;
                }

                if (Session["UserID"].ToString() == "MIM2201104")
                {
                    voicereport_mim2201104.Visible = true;
                    OBDReport.Visible = true;
                }

                if (Convert.ToString(Session["Usertype"]).ToUpper() != "SYSADMIN")
                {
                    string Login_Session_Id = Convert.ToString(Session["Login_Session_Id"]);
                    if (Login_Session_Id != "")
                    {
                        int Exists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM LoginEntry WITH(NOLOCK) WHERE UserId='" + _user.Trim() + "' AND LoginSessionID='" + Login_Session_Id.Trim() + "'"));
                        if (Exists != 1)
                        {
                            Session.Abandon();
                            Session.RemoveAll();
                            Response.Redirect("login.aspx");
                        }
                    }
                }
                //DLT CHECK HERO
                string DLTNO = ConfigurationManager.AppSettings["DLTNO"].ToString();
                string DLTNO1 = ConfigurationManager.AppSettings["DLTNO1"].ToString();
                string DLTNO2 = ConfigurationManager.AppSettings["DLTNO2"].ToString();
                if (DLTNO == Session["DLT"].ToString() || DLTNO1 == Session["DLT"].ToString() || DLTNO2 == Session["DLT"].ToString())
                {
                    A17.Visible = false;
                    divnav.Visible = false;
                }
                else
                {
                    div11.Visible = false;
                }
            }
            catch(Exception EE)
            {
                Util ob = new Util();
                ob.Log("error - " + EE.Message + "  - " + EE.StackTrace);
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            string _user = Convert.ToString(Session["UserID"]);
            Helper.database.ExecuteNonQuery("UPDATE LoginEntry SET LoginStatus='0' WHERE UserId='" + _user.Trim() + "'");
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}