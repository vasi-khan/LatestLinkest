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
    public partial class approve_sender_id : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            //if (usertype != "SYSADMIN")
            //    Response.Redirect("index.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            
            //s1 = h1.Value;
            //s2 = h2.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            HiddenField filename = (HiddenField)gvro.FindControl("hdnfilepath");
            // string ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/");
            if (string.IsNullOrEmpty(filename.Value))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('No Preview Available.');", true);
                return;
            }
            string url = ResolveUrl("~\\SenderIDDoc\\" + filename.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);
            GetData();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsenderid = (Label)gvro.FindControl("lblsenderid");
            Label lblccode = (Label)gvro.FindControl("lblccode");

            string user = Convert.ToString(Session["User"]);

            //string chkUAE = CheckUAEAccount(username.Value);
            //bool flag = false;
            //if (chkUAE == "true")
            //{
            //    //Getsmppaccountuserid
            //    if (Getsmppaccountuserid(lblsenderid.Text, username.Value) == "true")
            //    {
            //        flag = true;
            //    }
            //    else if (GetUAEAPIAccounts(lblsenderid.Text, username.Value) == "true")
            //    {
            //        flag = true;
            //    }
            //    else if (GetUAEAPIACCOUNTPROMO(lblsenderid.Text, username.Value) == "true")
            //    {
            //        flag = true;
            //    }
            //    if (flag == false)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender Id  is Not Mapped with Operator Sender Id Can Not Approved !!');", true);
            //        return;
            //    }
            //}


            Helper.Util ob = new Helper.Util();
            ob.ApproveRejectSenderId(lblsenderid.Text, username.Value, user, "APPROVE", lblccode.Text);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender ID Approved Successfully.');", true);
            GetData();
        }

        public string GetUAEAPIACCOUNTPROMO(string senderid, string user)
        {
            string sql = "if exists(select * from UAEAPIACCOUNTPROMO up inner join OperatorSenderId os on up.ACCOUNT=os.Provider where os.Active=1 and os.SID='" + senderid + "' and up.userid='" + user + "') begin select 'true' end";
            string res = Convert.ToString(Helper.database.GetScalarValue(sql));
            return res;
        }
        public string GetUAEAPIAccounts(string senderid, string user)
        {
            string sql = "if exists(select * from UAEAPIAccounts ua inner join OperatorSenderId os on ua.ACCOUNT=os.Provider where ua.Active=1 and os.Active=1 and os.SID='" + senderid + "' and ua.userid='" + user + "') begin select 'true' end";
            string res = Convert.ToString(Helper.database.GetScalarValue(sql));
            return res;
        }

        public string Getsmppaccountuserid(string senderid, string user)
        {
            string sql = @"if exists(select  su.Userid, s.PROVIDER, os.SID from smppaccountuserid su 
                inner join smppsetting s on s.smppaccountid = su.smppaccountid
                inner join OperatorSenderId os on os.Provider = s.PROVIDER where s.ACTIVE = 1 and os.Active=1 and os.SID='" + senderid + "' and su.userid='" + user + "') begin select 'true' end";
            string res = Convert.ToString(Helper.database.GetScalarValue(sql));
            return res;
        }

        public string CheckUAEAccount(string user)
        {
            string sql = "if exists (select * from customer where username='" + user + "' and  countrycode in ('971','966')) begin select 'true' end";
            string flag = Convert.ToString(Helper.database.GetScalarValue(sql));
            return flag;
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField username = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsenderid = (Label)gvro.FindControl("lblsenderid");
            Label lblccode = (Label)gvro.FindControl("lblccode");

            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            ob.ApproveRejectSenderId(lblsenderid.Text, username.Value, user, "REJECT", lblccode.Text);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender ID Rejected Successfully.');", true);
            GetData();
        }
        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                return;
            }
            if (Convert.ToDateTime(s2) < Convert.ToDateTime(s1))
            {
                hdntxtFrm.Value = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            DataTable dt = ob.GetSenderIdListForApproval(s1, s2, usertype, Convert.ToString(Session["DLT"]));

            grv.DataSource = dt;
            grv.DataBind();

            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv.TopPagerRow != null)
            {
                grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv.BottomPagerRow != null)
            {
                grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
}