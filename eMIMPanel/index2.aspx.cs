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

namespace eMIMPanel
{
    public partial class index2 : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        string EmpCode = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            EmpCode = Convert.ToString(Session["EmpCode"]);
            if (user == "") Response.Redirect("login.aspx");
            //get from dashboard
            GetValueForDashboard();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            //get from dashboard
            //GetValueForDashboard();

            //SMSFields();
            //URLFields();
            

            if (!IsPostBack)
            {
                AccountFields();
                BindTopActiveUser();
                if (usertype.ToUpper()=="SYSADMIN")
                {
                    BindTopBD();
                }
                else
                {

                    divDBTraffic.Visible = false;
                 
                }
               
            }

        }

        public DataTable GetMostActiveUserEmpCode(int noofrecord, string userType, string empcode, string dltno)
        {
            //string sql = @"SELECT top " + noofrecord + @" c.groupname as groupname,count(s.id) submitted,
            //            sum(case when isnull(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
            //            sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
            //            sum(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown               
            //                            from MSGSUBMITTED s with(nolock) 
            //                            inner join customer c with(nolock) ON c.username = s.profileid
            //                            left join DELIVERY d with(nolock) on s.msgid = d.msgid 
            //                            where s.SENTDATETIME >= CONVERT(varchar,GETDATE(),102) ";


            ViewState["NoOfRecord"] = noofrecord;
            string sql = @"SELECT top " + noofrecord + @" c.GROUPNAME as groupname,SUM(s.smssubmitted) as submitted,
SUM(s.smsdelivered) as delivered,SUM(s.smsfailed) as failed,(sum(s.smssubmitted) - sum(s.smsdelivered) - sum(s.smsfailed)) as unknown
from dashboard s with(nolock)
inner join customer c with(nolock) ON c.username = s.userid  where s.updtime >= CONVERT(varchar, GETDATE(), 102) and c.USERTYPE = 'User' ";
            if (userType.ToUpper() == "BD")
            {
                sql = sql + " AND c.EmpCode = '" + empcode + "' ";
            }

            if (userType.ToUpper() == "SYSADMIN" || userType.ToUpper() == "ADMIN")
            {
                sql = sql + " AND c.dltno = '" + dltno + "' ";
            }
            //sql = sql + "group by c.groupname order by submitted desc";
            sql = sql + "AND GROUPNAME is not null AND smssubmitted<>0 GROUP BY GROUPNAME  --order by submitted desc";
            return database.GetDataTable(sql);

        }
        public void BindTopActiveUser()
        {
            DataTable dt = new DataTable();
            if (usertype.ToUpper() == "BD")
            {
                this.grv.Columns[2].Visible = false;
                this.grv.Columns[3].Visible = false;
                grv.Columns[4].Visible = true;
                grv.Columns[6].Visible = false;
                dt = GetMostActiveUserEmpCode(Convert.ToInt32(ddlRecords.SelectedValue), usertype, EmpCode, Convert.ToString(Session["DLT"]));
                dt.Columns.Add("UserId");
                dt.Columns.Add("COMPNAME");
            }
            else
            {
                grv.Columns[0].Visible = false;
                grv.Columns[2].Visible = true;
                grv.Columns[3].Visible = true;
                grv.Columns[4].Visible = false;
                dt = ob.GetMostActiveUserr(Convert.ToInt32(ddlRecords.SelectedValue), usertype, Convert.ToString(Session["DLT"]));
                dt.Columns.Add("groupname");
            }

          

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
            DataTable dt = ob.GetDashboardSummary(user);
            if (dt.Rows.Count > 0)
            {
              
                lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["smssubmitted"]);
                Session["lblTodaySubmitted"] = lblTodaySubmitted.Text; //388792
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

             dt = ob.GetMonthSummary(user,Convert.ToString(Session["UserType"]));

            if (dt.Rows.Count>0)
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
                //lblTodaySubmitted.Text = "0";
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

        //public void URLFields()
        //{
        //    string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
        //    string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
        //    DataTable dt = ob.GetURLSummary(s1, s2, usertype, user);
        //    DataTable dt2 = ob.GetSMSClickSummary(s1, s2, usertype, user);
        //    if (dt.Rows.Count > 0)
        //    {
        //        lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["URLS"]);
        //        lblMonthClick.Text = Convert.ToString(dt.Rows[0]["CLICKED"]);
        //    }
        //    else
        //    {
        //        lblMonthLinkCreated.Text = "0";
        //        lblMonthClick.Text = "0";
        //    }
        //    if (dt2.Rows.Count > 0) lblMonthSmsClick.Text = dt2.Rows[0][0].ToString();
        //    else lblMonthSmsClick.Text = "0";
        //}

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
                string GroupName = this.grv.DataKeys[e.Row.RowIndex].Value.ToString();
                ViewState["grpname"] = GroupName;
                GridView nestedgv = e.Row.FindControl("nestedgv") as GridView;
                //nestedgv.DataSource = database.GetDataTable("select top 10 * from customer where groupname='"+ GroupName + "'");
                DataTable dt = ob.GetValueGroupName(Convert.ToInt32(ViewState["NoOfRecord"]), Convert.ToString(usertype), Convert.ToString(Session["DLT"]), Convert.ToString(Session["EMPCODE"]), Convert.ToString(GroupName));
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

        protected void nestedgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string username = "";
            //foreach (GridViewRow rowPeople in grv.Rows)
            //{
            //    GridView gvDocuments = (GridView)rowPeople.FindControl("nestedgv");
            //    //this will get you the outer/parent gridview datakeys
            //   string p1 = gvDocuments.DataKeys[rowPeople.RowIndex].Values[0].ToString();
            //    foreach (GridViewRow rowDocuments in gvDocuments.Rows)
            //    {
            //        //this will get you the inner/child gridview datakeys
            //       username =  gvDocuments.DataKeys[rowDocuments.RowIndex].Values[0].ToString();
            //    }
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //GridView twonestedgv = e.Row.FindControl("twonestedgv") as GridView;
                //GridView nestedgv = e.Row.FindControl("nestedgv") as GridView;
                //Accessing GridView from Sender object
                GridView childGrid = (GridView)sender;
                GridView grv = (GridView)e.Row.FindControl("grv");
                username = childGrid.DataKeys[e.Row.RowIndex].Value.ToString();

                GridView twonestedgv = (GridView)e.Row.FindControl("twonestedgv");
                DataTable dt = ob.GetValueForNestedGridTwo(Convert.ToInt32(ViewState["NoOfRecord"]), Convert.ToString(usertype), Convert.ToString(Session["DLT"]), Convert.ToString(Session["EMPCODE"]),Convert.ToString(ViewState["grpname"]), Convert.ToString(username));
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

                twonestedgv.DataSource = null;
                twonestedgv.DataSource = dt;
                twonestedgv.DataBind();
            }
        }
        public DataTable GetMostBDEmpCode(int noofrecord, string userType, string empcode, string dltno)
        {
            string sql = @"SELECT DISTINCT top " + noofrecord + @" userid ,COMPNAME,EmpCode,smssubmitted submitted,
                        smsdelivered as delivered,
                        smsfailed as failed,
                        (s.smssubmitted-s.smsdelivered-s.smsfailed) as unknown               
                                        from dashboard s with(nolock) 
                            join customer r on s.userid=r.username
                                      -- where s.updtime >= CONVERT(varchar,GETDATE(),102)
WHERE r.USERTYPE='BD' ";

            return database.GetDataTable(sql);

        }
        public void BindTopBD()
        {
            DataTable dt = new DataTable();

            dt = GetMostBDEmpCode(Convert.ToInt32(ddlBDRecords.SelectedValue), usertype, EmpCode, Convert.ToString(Session["DLT"]));
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

            GvBDWise.DataSource = dt;
            GvBDWise.DataBind();
            GridFormat(dt);
        }

        protected void lnkBDShow_Click(object sender, EventArgs e)
        {
            BindTopBD();
        }

        protected void GvBDWise_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string GroupName = grv.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView Grouptedgv = e.Row.FindControl("Grouptedgv") as GridView;

                DataTable dt = ob.GetValueGroupWise(int.Parse(ddlBDRecords.SelectedValue), "User", (e.Row.FindControl("hfEmpID") as HiddenField).Value);
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

                Grouptedgv.DataSource = null;
                Grouptedgv.DataSource = dt;
                Grouptedgv.DataBind();

            }
        }



        protected void Grouptedgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string GroupName = grv.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView Adminnestedgv = e.Row.FindControl("Adminnestedgv") as GridView;

                DataTable dt = ob.GetValueAdminWise(int.Parse(ddlBDRecords.SelectedValue), "Admin", e.Row.Cells[2].Text.Trim());
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

                Adminnestedgv.DataSource = null;
                Adminnestedgv.DataSource = dt;
                Adminnestedgv.DataBind();

            }
        }

        protected void Adminnestedgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string GroupName = grv.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView Usernestedgv = e.Row.FindControl("Usernestedgv") as GridView;

                DataTable dt = ob.GetValueUserWise(int.Parse(ddlBDRecords.SelectedValue), "User", e.Row.Cells[3].Text.Trim());
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

                Usernestedgv.DataSource = null;
                Usernestedgv.DataSource = dt;
                Usernestedgv.DataBind();

            }
        }
    }
}
