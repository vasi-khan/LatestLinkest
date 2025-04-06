using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using Ionic.Zip;
using ZipFile = System.IO.Compression.ZipFile;
using System.Globalization;
using System.Data.SqlClient;
using eMIMPanel.Helper;
using System.Text;
namespace eMIMPanel
{
    public partial class BlacklistNoEntry_A : System.Web.UI.Page
    {
        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            DataTable dt = ob.GetBlackListNumbersUsingDLTNO(Convert.ToString(Session["DLT"]));
            if (dt != null && dt.Rows.Count > 0)
            {
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
            else
            {
                grv.DataSource = null;
                grv.DataBind();
            }
        }

        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnXL_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField fileid = (HiddenField)gvro.FindControl("hdnFileId");
            HiddenField userid = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsender = (Label)gvro.FindControl("lblsender");
            //s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            //DataTable dt = ob.GetSMSReportDetail_user_new2(userid.Value, fileid.Value, lblsender.Text, s1, s2);
            //Session["MOBILEDATA"] = dt;

            //if (dt.Rows.Count > 0)
            //{
            //    Session["FILENAME"] = "SMSReportDetail.xls";
            //    Response.Redirect("sms-reports_u_download.aspx");
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            //}
        }

        protected void btnXLdw_Click(object sender, EventArgs e)
        {
            if (Session["MOBILEDATA"] != null)
            {
                Session["FILENAME"] = "SMS_Mobile_Seaarch_Report.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
        }

        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                GetData();
            }
            grv.UseAccessibleHeader = true;
            //grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((Session["analyticsdata"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "SMSDATE", "Messageid", "MobileNo", "Sender", "SentDate", "DeliveredDate", "Message", "MessageState", "RESPONSE");
                DatatableToCSV(distinctValues, "ReportsMobileNoWise.csv");

            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        private void DatatableToCSV(DataTable dt, string FileName)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName.Replace(".xls", ".csv"));
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder columnbind = new StringBuilder();
                for (int k = 0; k < (dt).Columns.Count; k++)
                {
                    columnbind.Append(dt.Columns[k].ColumnName + ',');
                }

                columnbind.Append("\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                    }
                    columnbind.Append("\r\n");
                }

                DataTable FDT = Session["FottorValue"] as DataTable;
                if (FDT != null && FDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in FDT.Rows)
                    {
                        for (int k = 0; k < FDT.Columns.Count; k++)
                        {
                            columnbind.Append(dr[k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }
                    }
                    columnbind.Append("\r\n");

                }
                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }

        protected void LnkbtnInsert_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            string dltno = Convert.ToString(Session["DLT"]);
            if (txtmob.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Numbers !!');", true);
                return;
            }
            int mobLen = Convert.ToInt32(Session["mobLength"]);
            int CountryCode = Convert.ToInt32(Session["DEFAULTCOUNTRYCODE"]);
            string mobile = "";
            if (txtmob.Text != "") mobile = txtmob.Text.Replace('\n', ',');
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            mobList = mobList.Distinct().ToList();
            mobList.RemoveAll(x => x.Length < mobLen);
            mobList = mobList.Select(x => x.Substring(x.Length - mobLen)).ToList();
            if (mobList.Count > 0)
            {
                try
                {
                    ob.AddBlackListNumbersUsingDLT(mobList, dltno);
                }
                catch (Exception ex)
                {

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers added in blacklist !!');", true);
                lblMobileCnt.Text = "0";
                txtmob.Text = "";
                GetData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Correct Mobile Numbers !!');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtmob.Text = "";
            lblMobileCnt.Text = "0";
        }
    }
}