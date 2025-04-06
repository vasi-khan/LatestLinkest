using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;
namespace eMIMPanel
{
    public partial class DeliveryDistinctCount : System.Web.UI.Page
    {
        //complete by ayaz//
        string s1 = "";
        string s2 = "";
        Util obj = new Util();
        string usertype = "";
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            //EmpCode = Convert.ToString(Session["EmpCode"]);
            if (user == "") Response.Redirect("login.aspx");
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            //string Fdate = FromDate.ToString("dd-mm-yyyy");
            if (txtuserid.Text.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User Id Can Not be Empty !!!');", true);
                return;
            }
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            DataTable dt = obj.ShowGird(Convert.ToString(txtuserid.Text), s1, s2, txtdistinct.Text.Trim());
            lbldistinct.Text = dt.Rows[0]["COUNT"].ToString();
        }

        protected void btnkdownload_Click(object sender, EventArgs e)
        {
            s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            //DateTime s = Convert.ToDateTime(hdntxtFrm.Value);
            //DateTime s3 = Convert.ToDateTime(hdntxtTo.Value);
            //s1 = s.ToString("yyyy-MM-dd");
            //s2 = s.ToString("yyyy-MM-dd");
            DataTable dt = obj.ShowGird(Convert.ToString(txtuserid.Text), s1, s2, txtdistinct.Text.Trim(), 1);
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not download empty Data !!!');", true);
                return;
            }
            Random rn = new Random();
            int no = rn.Next(1, 999);
            string FileName = "CountDownload_" + no.ToString() + DateTime.Now.ToString("_yyyyMMddhhmmssfff")+".csv";
            Session["FILENAME"] = FileName;
            Session["MOBILEDATA"] = dt;
            Response.Redirect("sms-reports_u_download.aspx");

        }
    }
}