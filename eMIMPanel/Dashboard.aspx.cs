using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;
using eMIMPanel.Helper;
using System.Configuration;
using System.Data.SqlClient;

namespace eMIMPanel
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        string EmpCode = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");

            }
           
                usertype = Convert.ToString(Session["UserType"]);
                user = Convert.ToString(Session["User"]);
            //user = (Session["UserID"] as Helper.common.mlogin).usernmae.ToString();
            //EmpCode = (Session["UserID"] as Helper.common.mlogin).usernmae.ToString();
            EmpCode = Convert.ToString(Session["EmpCode"]);
            if (user == "") 
                //get from dashboard
                GetValueForDashboard();

            
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {

                Response.Redirect("Login.aspx");
            }
       
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            //user = (Session["USER"] as Helper.common.mlogin).usernmae.ToString();
            if (user == "") Response.Redirect("Login.aspx");
            GetValueForDashboard();
            //get from dashboard
            //GetValueForDashboard();

            //SMSFields();
            //URLFields();
            AccountFields();

            if (!IsPostBack)
            {
                BindTopActiveUser();
                gvCustomers.DataSource = GetData("select top 10 * from Customer");
                gvCustomers.DataBind();
            }
        }

        private static DataTable GetData(string query)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string customerId = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
                gvOrders.DataSource = GetData(string.Format("select top 3 * from customer where Username='{0}'", customerId));
                gvOrders.DataBind();
            }
        }

        public void BindTopActiveUser()
        {
            //DataTable dt = ob.GetMostActiveUserEmpCode(Convert.ToInt32(ddlRecords.SelectedValue), usertype, EmpCode, Convert.ToString(Session["DLT"]));
            //DataTable dt = ob.GetMostActiveUserEmpCode(Convert.ToInt32(ddlRecords.SelectedValue), usertype, EmpCode);
            DataTable dt = GetMostActiveUserEmpCode(Convert.ToInt32(ddlRecords.SelectedValue), usertype, EmpCode,Convert.ToString(Session["DLT"]));
            dt.Columns.Add("SubmittedPer");
            dt.Columns["SubmittedPer"].DefaultValue = 0;
            Int64 tdyCount = Convert.ToInt64(Session["lblTodaySubmitted"]);
            foreach (DataRow row in dt.Rows)
            {
                Int64 sub = Convert.ToInt64(row["Submitted"]);
                if (tdyCount > 0)
                {
                    Int64 subPer = sub * 100 / tdyCount;
                    row["SubmittedPer"] = Convert.ToString(subPer);
                }
            }
            dt.AcceptChanges();
            //dt.Columns.Add("SubmittedPer");

            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }


        public void GetValueForDashboard()
        {
            //Today
            DataTable dt = ob.GetDashboardSummaryBD(EmpCode);
            if (dt.Rows.Count > 0)
            {

                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["smssubmitted"]);
                Session["lblTodaySubmitted"] = lblTodaySubmitted.Text;
                lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["smsdelivered"]);
                Session["lblTodayDelivered"] = lblTodayDelivered.Text;
                lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["smsfailed"]);
                Session["lblTodayFailed"] = lblTodayFailed.Text;
                //lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["links"]);
                //Session["lblMonthLinkCreated"] = lblMonthLinkCreated.Text;
                //lblMonthClick.Text = Convert.ToString(dt.Rows[0]["clicks"]);
                // Session["lblMonthClick"] = lblMonthClick.Text;
                //lblMonthSmsClick.Text = Convert.ToString(dt.Rows[0]["smsclicks"]);
                //Session["lblMonthSmsClick"] = lblMonthSmsClick.Text;
                lblLastUpd.Text = Convert.ToString(dt.Rows[0]["updtime"]);
                Session["lblLastUpd"] = lblLastUpd.Text;
            }
            else
            {
                lblTodayFailed.Text = "0"; Session["lblTodayFailed"] = "0";
                lblTodayDelivered.Text = "0"; Session["lblTodayDelivered"] = "0";
                lblTodaySubmitted.Text = "0"; Session["lblTodaySubmitted"] = "0";
                //lblMonthLinkCreated.Text = "0"; Session["lblMonthLinkCreated"] = "0";
                //lblMonthClick.Text = "0"; Session["lblMonthClick"] = "0";
                //lblMonthSmsClick.Text = "0"; Session["lblMonthSmsClick"] = "0";
                lblLastUpd.Text = ""; Session["lblLastUpd"] = "";
            }

            //dt = ob.GetMonthSummary(user, Convert.ToString(Session["UserType"]));
            //monthly
            dt = ob.GetMonthSummaryBD(EmpCode, usertype);

            if (dt.Rows.Count > 0)
            {
                lblMonthSubmitted.Text = Convert.ToString(dt.Rows[0]["SUBMITTED"]);
                Session["lblMonthSubmitted"] = lblMonthSubmitted.Text;
                lblMonthdelivered.Text = Convert.ToString(dt.Rows[0]["DELIVERED"]);
                Session["lblMonthDelivered"] = lblMonthdelivered.Text;
                lblMonthFailed.Text = Convert.ToString(dt.Rows[0]["FAILED"]);
                Session["lblMonthFailed"] = lblMonthFailed.Text;

            }
            else
            {
                lblTodaySubmitted.Text = "0";
                lblMonthdelivered.Text = "0";
                lblMonthFailed.Text = "0";
                Session["lblMonthSubmitted"] = "0";
                Session["lblMonthDelivered"] = "0";
                Session["lblMonthFailed"] = "0";
            }



        }

        public void SMSFields()
        {
            string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetSMSSummaryBD(s1, s2, usertype, user);
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

        public void AccountFields()
        {
            string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            lblAccountCreated.Text = ob.GetAccountSummaryBD(s1, s2, usertype, EmpCode);
            lblLastMonthAccountCreated.Text = ob.GetAccountSummaryLastMonth(s1, s2, usertype, EmpCode);
            lblCreditAllotted.Text = ob.GetCreditSummary(s1, s2, usertype, EmpCode);
            lblLastMonthCreditAlloted.Text = ob.GetCreditSummaryLastMonth(s1, s2, usertype, EmpCode);
            lblActiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, EmpCode, "1");
            lblInactiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, EmpCode, "0");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SMSFields();
            //URLFields();

            string s = lblTodaySubmitted.Text;
            string d = lblTodayDelivered.Text;
            string f = lblTodayFailed.Text;
            //string l = lblMonthLinkCreated.Text;
            //string c = lblMonthClick.Text;
            //string m = lblMonthSmsClick.Text;

            string l = "";
            string c = "";
            string m = "";

            ob.UpdateDashboard(user, s == "" ? "0" : s, d == "" ? "0" : d, f == "" ? "0" : f, l == "" ? "0" : l, c == "" ? "0" : c, m == "" ? "0" : m);
            GetValueForDashboard();
            Response.Redirect("index2.aspx");
        }
        protected void lnkShow_Click(object sender, EventArgs e)
        {
            BindTopActiveUser();
        }
        protected void GridFormat(DataTable dt)
        {
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

        protected void grv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string GroupName = grv.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView nestedgv = e.Row.FindControl("nestedgv") as GridView;
                //nestedgv.DataSource = database.GetDataTable("select top 10 * from customer where groupname='"+ GroupName + "'");
                DataTable dt = GetValueGroupName(10, Convert.ToString(usertype), Convert.ToString(Session["DLT"]), Convert.ToString(Session["EMPCODE"]), Convert.ToString(GroupName));
                dt.Columns.Add("SubmittedPer");
                dt.Columns["SubmittedPer"].DefaultValue = 0;
                Int64 tdyCount = Convert.ToInt64(Session["lblTodaySubmitted"]);
                foreach (DataRow row in dt.Rows)
                {
                    Int64 sub = Convert.ToInt64(row["Submitted"]);
                    if (tdyCount > 0)
                    {
                        Int64 subPer = sub * 100 / tdyCount;
                        row["SubmittedPer"] = Convert.ToString(subPer);
                    }
                }
                dt.AcceptChanges();
                //dt.Columns.Add("SubmittedPer");

                nestedgv.DataSource = null;
                nestedgv.DataSource = dt;
                nestedgv.DataBind();
                //GridFormat(dt);

                //nestedgv.DataSource = GetValueGroupName(10,Convert.ToString(usertype),Convert.ToString(Session["DLT"]),Convert.ToString(Session["EMPCODE"]),Convert.ToString(GroupName));
                //nestedgv.DataBind();
            }
        }


        public DataTable GetMostActiveUserEmpCode(int noofrecord, string userType, string empcode,string dltno)
        {
            string sql = @"SELECT top " + noofrecord + @" c.groupname as groupname,count(s.id) submitted,
                        sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                        sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                        sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
                                        from MSGSUBMITTED s with(nolock) 
                                        inner join customer c with(nolock) ON c.username = s.profileid
                                        left join DELIVERY d with(nolock) on s.msgid = d.msgid 
                                        where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";
            if (userType.ToUpper() == "BD")
            {
                sql = sql + " AND EmpCode = '" + empcode + "' ";
            }

            if (userType.ToUpper() == "SYSADMIN" || userType.ToUpper() == "ADMIN")
            {
                sql = sql + " AND dltno = '" + dltno + "' ";
            }
            sql = sql + "group by c.groupname order by submitted desc";
            return database.GetDataTable(sql);

        }

        public DataTable GetValueGroupName(int noofrecord, string userType, string dltNO,string empcode, string grpname)
        {
            string sql = @"SELECT top " + noofrecord + @" c.username as Username,count(s.id) submitted,
                        sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
                        sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
                        sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
                                        from MSGSUBMITTED s with(nolock) 
                                        inner join customer c with(nolock) ON c.username = s.profileid
                                        left join DELIVERY d with(nolock) on s.msgid = d.msgid 
                                        where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";
            if (userType.ToUpper() == "BD")
            {
                sql = sql + " AND EmpCode = '" + empcode + "' and groupname='" + grpname + "' ";
            }

            if (userType.ToUpper() == "SYSADMIN" || userType.ToUpper() == "ADMIN")
            {
                sql = sql + " AND dltno = '" + dltNO + "' ";
            }
            sql = sql + "group by c.username order by submitted desc";
            return database.GetDataTable(sql);
        }
    }
}