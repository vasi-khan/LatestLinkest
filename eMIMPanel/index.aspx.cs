using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace eMIMPanel
{
    public partial class index : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            SMSFields();
            URLFields();
            AccountFields();
        }
        public void SMSFields()
        {
           string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
           string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
           DataTable dt = ob.GetSMSSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["Submitted"]);
                lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["Delivered"]);
                lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["Failed"]);
            }
            else
            {
                lblTodayFailed.Text = "0";
                lblTodayDelivered.Text = "0";
                lblTodaySubmitted.Text = "0";
            }

        }

        public void URLFields()
        {
            string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01" ;
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetURLSummary(s1, s2, usertype, user);
            DataTable dt2 = ob.GetSMSClickSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["URLS"]);
                lblMonthClick.Text = Convert.ToString(dt.Rows[0]["CLICKED"]);
            }
            else
            {
                lblMonthLinkCreated.Text = "0";
                lblMonthClick.Text = "0";
            }
            if (dt2.Rows.Count > 0) lblMonthSmsClick.Text = dt2.Rows[0][0].ToString();
            else lblMonthSmsClick.Text = "0";        }

        public void AccountFields()
        {
            string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            lblAccountCreated.Text = ob.GetAccountSummary(s1, s2, usertype, user);
            lblLastMonthAccountCreated.Text = ob.GetAccountSummaryLastMonth(s1, s2, usertype, user);
            lblCreditAllotted.Text = ob.GetCreditSummary(s1, s2, usertype, user);
            lblLastMonthCreditAlloted.Text = ob.GetCreditSummaryLastMonth(s1, s2, usertype, user);
            lblActiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "1");
            lblInactiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "0");
        }

    }
}
