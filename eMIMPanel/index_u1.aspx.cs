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
    public partial class index_u1 : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            if (usertype == "SYSADMIN")
                this.MasterPageFile = "~/Site1.Master";
        }


        protected void Page_PreLoad(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            if (usertype == "SYSADMIN")
                user = Convert.ToString(Session["User"]);
            else
                user = Convert.ToString(Session["UserID"]);

            if (user == "") Response.Redirect("login.aspx");

            if (usertype == "SYSADMIN")
            {
                user = Convert.ToString(Session["User"]);
                GetGraphData_A();
                GetValueForDashboard_U("SYSADMIN");
            }
            else
            {
                GetGraphData_U();
                GetValueForDashboard_U("user");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            if (usertype == "SYSADMIN")
                user = Convert.ToString(Session["User"]);
            else
                user = Convert.ToString(Session["UserID"]);

            if (user == "") Response.Redirect("login.aspx");

            //lblLastUpd.Text = Convert.ToString(Session["lblLastUpd"]);

            //SMSFields();
            //URLFields();
            ////AccountFields();
        }

        public void GetGraphData_A()
        {
            // DataTable dt = database.GetDataTable("Select convert(varchar(6),smsdate,106) as smsdate,TOTALSUBMITED,smsdate as ord_date from DATEDAYANALYSIS where smsdate >= dateadd(D,-10,getdate()) and smsdate<=getdate() order by ord_date ");

            string sqlTemp = "Create table #dummyDate(tempDate datetime,cnt int);";

            DateTime from = DateTime.Now.AddDays(-9);
            DateTime to = DateTime.Now;
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                int cnt = 0;
                string date = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                sqlTemp += string.Format("Insert into #dummyDate(tempDate,cnt) values('{0}',{1});", date, cnt);

            }
            string sql = "";
            sql = sqlTemp;
            sql += @"Select convert(varchar(6),dd.tempDate,106) as smsdate,SUM(ISNULL(SUBMITTED, 0))[TOTALSUBMITED] from #dummyDate dd
             LEFT JOIN DAYSUMMARY  ON dd.tempDate = smsdate             
             GROUP BY dd.tempDate order by dd.tempDate";
           
            DataTable dt = database.GetDataTable(sql);
            dt.Rows[dt.Rows.Count - 1]["TOTALSUBMITED"] = database.GetScalarValue("Select isnull(smssubmitted,0) from dashboard where userid='" + user + "'");
            dt.AcceptChanges();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Session["Date" + (i + 1).ToString()] = dt.Rows[i]["smsdate"].ToString();
                decimal n = Convert.ToDecimal(dt.Rows[i]["TOTALSUBMITED"]) / 1000000;
                // decimal n = Convert.ToDecimal(formatnum.FormatNumber(Convert.ToInt32(dt.Rows[i]["TOTALSUBMITED"])));
                Session["Value" + (i + 1).ToString()] = Math.Round(n, 5).ToString();
            }
        }

        public void GetGraphData_U()
        {
            string sqlTemp = "Create table #dummyDate(tempDate datetime,cnt int);";

            DateTime from = DateTime.Now.AddDays(-9);
            DateTime to = DateTime.Now;
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                int cnt = 0;
                string date = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                sqlTemp += string.Format("Insert into #dummyDate(tempDate,cnt) values('{0}',{1});", date, cnt);

            }
            string sql = "";
            sql = sqlTemp;
            sql += @"Select convert(varchar(6),dd.tempDate,106) as smsdate,SUM(ISNULL(SUBMITTED, 0))[TOTALSUBMITED] from #dummyDate dd
             LEFT JOIN DAYSUMMARY  ON dd.tempDate = smsdate and userid = '" + user + "' GROUP BY dd.tempDate order by dd.tempDate";
            DataTable dt = database.GetDataTable(sql);
            dt.Rows[dt.Rows.Count - 1]["TOTALSUBMITED"] = database.GetScalarValue("Select isnull(smssubmitted,0) from dashboard where userid='" + user + "'"); 
            dt.AcceptChanges();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Session["Date" + (i + 1).ToString()] = dt.Rows[i]["smsdate"].ToString();
                decimal n = Convert.ToDecimal(dt.Rows[i]["TOTALSUBMITED"]) / 1000000;
                // decimal n = Convert.ToDecimal(formatnum.FormatNumber(Convert.ToInt32(dt.Rows[i]["TOTALSUBMITED"])));
                Session["Value" + (i + 1).ToString()] = Math.Round(n, 5).ToString();
            }
        }
        public void GetValueForDashboard_U(string usertype)
        {
            string f = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string t = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            string sql = ""; string sqlFailBrek = "";
            if (usertype == "SYSADMIN")
            {
                sql = @"with ReportCTE as ( select count(s.id) as submitted,
sum(IIF(isnull(d.dlvrstatus, '') = 'Delivered',1,0)) as delivered,
sum(IIF(isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL,1,0)) as failed
from MSGSUBMITTED s with(nolock)
left join delivery d with(nolock) on s.msgid = d.msgid 
where  s.sentdatetime between '" + f + "' and '" + t + "')" +
" select submitted, isnull(delivered,0) delivered,isnull((delivered * 100 / submitted),0) [delivered_p],isnull(failed,0) failed,  isnull((failed* 100/submitted),0) [failed_p] from ReportCTE ";

                sqlFailBrek = @"SELECT top 8 LEFT(cast(isnull(e.code,'') as nvarchar) +'  '+ isnull(e.descr,'Others'),30) AS Descreption ,COUNT(*) AS SUBMITTED,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
FROM MSGSUBMITTED M WITH(NOLOCK)
left join smppsession s with (nolock) on m.smppaccountid=s.sessionid
left join smppsetting t with (nolock) on s.smppaccountid=t.smppaccountid
left JOIN delivery d with(nolock) on M.MSGID = d.msgid
left join errorcode e on d.err_code=e.code and t.provider=e.provider
WHERE M.SENTDATETIME between '" + f + "' and '" + t + "' and d.dlvrstatus<>'Delivered' " +
    " GROUP BY e.code,e.descr order by SUBMITTED desc";

            }
            else
            {
                sql = @"with ReportCTE as ( select count(s.id) as submitted,
sum(IIF(isnull(d.dlvrstatus, '') = 'Delivered',1,0)) as delivered,
sum(IIF(isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL,1,0)) as failed
from MSGSUBMITTED s with(nolock)
left join delivery d with(nolock) on s.msgid = d.msgid 
where s.profileid = '" + user + "' and s.sentdatetime between '" + f + "' and '" + t + "')" +
 " select submitted, isnull(delivered,0) delivered,isnull((delivered * 100 / submitted),0) [delivered_p],isnull(failed,0) failed,  isnull((failed* 100/submitted),0) [failed_p] from ReportCTE ";

                sqlFailBrek = @"SELECT top 8 LEFT(cast(isnull(e.code,'') as nvarchar) +'  '+ isnull(e.descr,'Others'),30) AS Descreption ,COUNT(*) AS SUBMITTED,
sum(case when isnull(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed
FROM MSGSUBMITTED M WITH(NOLOCK)
left join smppsession s with (nolock) on m.smppaccountid=s.sessionid
left join smppsetting t with (nolock) on s.smppaccountid=t.smppaccountid
left JOIN delivery d with(nolock) on M.MSGID = d.msgid
left join errorcode e on d.err_code=e.code and t.provider=e.provider
WHERE M.SENTDATETIME between '" + f + "' and '" + t + "' and d.dlvrstatus<>'Delivered' and m.profileid='" + user + "' " +
    " GROUP BY e.code,e.descr order by SUBMITTED desc";
            }

            DataTable dt = database.GetDataTable(sql);
            DataTable dtBrek = database.GetDataTable(sqlFailBrek);
            int totalFail = 1;
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lblLastUpd.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);
                lblSubmit.Text = formatnum.FormatNumber(Convert.ToInt32(dr["submitted"]));
                lblDlr.Text = formatnum.FormatNumber(Convert.ToInt32(dr["delivered"]));
                lblDlrPer.Text = dr["delivered_p"].ToString();
                lblFail.Text = formatnum.FormatNumber(Convert.ToInt32(dr["failed"].ToString()));
                lblFailPer.Text = dr["failed_p"].ToString();
                totalFail = Convert.ToInt32(dr["failed"]);

            }
            else
            {
                lblLastUpd.Text = "";
                lblSubmit.Text = "";
                lblDlr.Text = "";
                lblDlrPer.Text = "";
                lblFail.Text = "";
            }

            if (dtBrek.Rows.Count > 0)
            {
                for (int i = 0; i < dtBrek.Rows.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            lblFailPer1.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc1"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 1:
                            lblFailPer2.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc2"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 2:
                            lblFailPer3.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc3"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 3:
                            lblFailPer4.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc4"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 4:
                            lblFailPer5.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc5"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 5:
                            lblFailPer6.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc6"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 6:
                            lblFailPer7.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc7"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;
                        case 7:
                            lblFailPer8.Text = dtBrek.Rows[i]["SUBMITTED"] + " (" + (Convert.ToDouble(dtBrek.Rows[i]["SUBMITTED"]) * 100 / totalFail).ToString("N2") + " %)";
                            Session["Desc8"] = Convert.ToString(dtBrek.Rows[i]["Descreption"]);
                            break;

                    }
                }

            }
            else
            {
                lblFailPer1.Text = "";
                lblFailPer2.Text = "";
                lblFailPer3.Text = "";
                lblFailPer4.Text = "";
                lblFailPer5.Text = "";
                lblFailPer6.Text = "";
                lblFailPer7.Text = "";
                lblFailPer8.Text = "";
            }


        }

        public void SMSFields()
        {
            string s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetSMSSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                //lblTodaySubmitted.Text = Convert.ToString(dt.Rows[0]["Submitted"]);
                //lblTodayDelivered.Text = Convert.ToString(dt.Rows[0]["Delivered"]);
                //lblTodayFailed.Text = Convert.ToString(dt.Rows[0]["Failed"]);
            }
            else
            {
                //lblTodayFailed.Text = "0";
                //lblTodayDelivered.Text = "0";
                //lblTodaySubmitted.Text = "0";
            }

        }

        public void URLFields()
        {
            string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            DataTable dt = ob.GetURLSummary(s1, s2, usertype, user);
            DataTable dt2 = ob.GetSMSClickSummary(s1, s2, usertype, user);
            if (dt.Rows.Count > 0)
            {
                //lblMonthLinkCreated.Text = Convert.ToString(dt.Rows[0]["URLS"]);
                //lblMonthClick.Text = Convert.ToString(dt.Rows[0]["CLICKED"]);
            }
            else
            {
                //lblMonthLinkCreated.Text = "0";
                //lblMonthClick.Text = "0";
            }
            //if (dt2.Rows.Count > 0) lblMonthSmsClick.Text = dt2.Rows[0][0].ToString();
            //else lblMonthSmsClick.Text = "0";
        }

        public void AccountFields()
        {
            //string s1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";
            //string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //lblAccountCreated.Text = ob.GetAccountSummary(s1, s2, usertype, user);
            //lblLastMonthAccountCreated.Text = ob.GetAccountSummaryLastMonth(s1, s2, usertype, user);
            //lblCreditAllotted.Text = ob.GetCreditSummary(s1, s2, usertype, user);
            //lblLastMonthCreditAlloted.Text = ob.GetCreditSummaryLastMonth(s1, s2, usertype, user);
            //lblActiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "1");
            //lblInactiveUsers.Text = ob.GetActiveAccountSummary(s1, s2, usertype, user, "0");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //ob.CalculateSKYLINE(s2);
            //GetValueForDashboard();

            Response.Redirect("index_u1.aspx");
        }
    }
}