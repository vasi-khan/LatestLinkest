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

namespace eMIMPanel
{
    public partial class sms_reports_usrDLR : System.Web.UI.Page
    {
        string d1 = "";
        string d2 = "";
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            //if (user == "MIM2000002") Response.Redirect("sms-reports_usrDLR29.aspx");
            if (user == "MIM2000029") Response.Redirect("sms-reports_usrDLR29.aspx");
            if (user == "MIM2002097") Response.Redirect("sms-reports_usrDLR29.aspx");
            if (user == "") Response.Redirect("login.aspx");

            //SMSFields();
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                //DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                DataTable dt = ob.GetSMSReport_user("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);

                dt = ob.GetSMSReport_user_newConsolidated("02/JAN/1900", "02/JAN/1900", txtmob.Text, user, "", "");
                grv2.DataSource = null;
                grv2.DataSource = dt;
                grv2.DataBind();
                GridFormat2(dt);

            }
            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
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
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //s1 = h1.Value;
                //s2 = h2.Value;
                GetData();
                //txtFrm.Text = hdntxtFrm.Value;
                //txtTo.Text = hdntxtTo.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            //if (txtFrm.Text.Trim() == "")
            //{
            //    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            //else
            //{
            //    s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}

            s1 = "2020-10-05";
            s2 = "2020-10-05 23:59:59.999";

            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetSMSReport_user_new(s1, s2, usertype, user);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }
        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            //if (grv.TopPagerRow != null)
            //{
            //    grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            //}
            //if (grv.BottomPagerRow != null)
            //{
            //    grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            //}
            //if (dt.Rows.Count > 0)
            //    grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void GridFormat2(DataTable dt)
        {
            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            //if (grv2.TopPagerRow != null)
            //{
            //    grv2.TopPagerRow.TableSection = TableRowSection.TableHeader;
            //}
            //if (grv2.BottomPagerRow != null)
            //{
            //    grv2.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            //}
            //if (dt.Rows.Count > 0)
            //    grv2.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            //modalpopuppwd.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField fileid = (HiddenField)gvro.FindControl("hdnFileId");
            HiddenField userid = (HiddenField)gvro.FindControl("hdnUserId");
            Label lblsender = (Label)gvro.FindControl("lblsender");
            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            Session["rptFILEID"] = fileid.Value;
            Session["rptSENDER"] = lblsender.Text;
            Session["rptUSERID"] = userid.Value;
            Session["rptS1"] = s1;
            Session["rptS2"] = s2;

            string url = ResolveUrl("~\\sms-reports-detail_usr.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);


            return;
            /*
            DataTable dt = ob.GetSMSReportDetail_user_new(userid.Value, fileid.Value, lblsender.Text, s1, s2);

            Session["rptDetail"] = dt;
            if (dt.Rows.Count > 0)
            {
                string url = ResolveUrl("~\\sms-reports-detail_usr.aspx");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
            */
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
            s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            DataTable dt = ob.GetSMSReportDetail_user_new(userid.Value, fileid.Value, lblsender.Text, s1, s2);
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "SMSReportDetail.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }

        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtmob.Text.Trim() != "")
                {
                    if (txtmob.Text.Trim().Length < 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter 10 digit mobile number');", true);
                        return;
                    }
                }
                if (hdntxtFrm1.Value.Trim() == "")
                {
                    d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                }
                else
                {
                    d1 = Convert.ToDateTime(hdntxtFrm1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    d2 = Convert.ToDateTime(hdntxtTo1.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                }
                Helper.Util ob = new Helper.Util();
                string usertype = Convert.ToString(Session["UserType"]);
                string user = Convert.ToString(Session["UserID"]);
                //DataTable dt = ob.GetSMSReport4MOBILE_user(d1, d2, txtmob.Text, user);
                DataTable dt = ob.GetSMSReport_user_newConsolidated(d1, d2, txtmob.Text, user, "", "");
                if (dt.Rows.Count > 0)
                {
                    grv2.DataSource = null;
                    grv2.DataSource = dt;
                    grv2.DataBind();
                    GridFormat2(dt);
                    //Session["MOBILEDATA"] = dt;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data fetched successfully. Click buttons to download.');", true);
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data found.');", true);
                    grv2.DataSource = null;
                    grv2.DataSource = dt;
                    grv2.DataBind();
                    GridFormat2(dt);
                }
                txtFrm1.Text = hdntxtFrm1.Value;
                txtTo1.Text = hdntxtTo1.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }
        protected void btnXLdw_Click(object sender, EventArgs e)
        {
            if (Session["MOBILEDATA"] != null)
            {
                Session["FILENAME"] = "SMS_Mobile_Seaarch_Report.xls";
                Response.Redirect("sms-reports_u_download.aspx");
            }
        }
        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblsender = (Label)gvro.FindControl("lblsender");
            HiddenField hdnfrm = (HiddenField)gvro.FindControl("hdnFrmDW");
            HiddenField hdnto = (HiddenField)gvro.FindControl("hdnToDW");
            //if (hdntxtFrm1.Value.Trim() == "")
            //{
            //    d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            //else
            //{
            //    d1 = Convert.ToDateTime(hdntxtFrm1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    d2 = Convert.ToDateTime(hdntxtTo1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //} 

            d1 = hdnfrm.Value; d2 = hdnto.Value + " 23:59:59.999";
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string senderid = lblsender.Text;

            string mainPath = "DWReports";
            string subPath = "DWReports/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = Server.MapPath(subPath); 

            for (var day = Convert.ToDateTime(hdnfrm.Value).Date; day.Date <= Convert.ToDateTime(hdnto.Value).Date; day = day.AddDays(1))
            {
                DateTime fromDate = day;
                string date = fromDate.ToString("yyyy-MM-dd");
                DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(date, date + " 23:59:59.999", senderid, user);

                DataView dv = new DataView(dt);
                DataTable dtDates = dv.ToTable(true, "SMSdate");

                bool exists = System.IO.Directory.Exists(mappath);
                if (!exists) System.IO.Directory.CreateDirectory(mappath);
                 
                int takeCount = 100000;
                int fn = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable data = dt.AsEnumerable().Skip(i).Take(takeCount).CopyToDataTable();
                    i = i + takeCount;

                    string mydate = dtDates.Rows[0]["SMSdate"].ToString();

                    //DataView dv1 = dt.DefaultView;
                    //dv1.RowFilter = "SMSdate = '" + mydate + "'";
                    //DataTable myDataTable = dv1.ToTable();

                    //string myTableAsString =
                    //String.Join(Environment.NewLine, myDataTable.Rows.Cast<DataRow>().
                    //Select(r => r.ItemArray).ToArray().
                    //Select(x => String.Join("\t", x.Cast<string>())));

                    StreamWriter sw = new StreamWriter(mappath + @"\" + mydate.Replace(".", "-") +"_"+ fn.ToString() +".txt");fn++;
                    sw.WriteLine("SMS Date" + "\t" + "Message ID" + "\t" + "Mobile No" + "\t" + "Sender ID" + "\t" + "Sent Date" + "\t" + "Delivered Date" + "\t" + "Message" + "\t" + "Status" + "\t" + "Delivery Response");

                    foreach (DataRow dr in data.Rows)
                    {
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                            if (!Convert.IsDBNull(dr[j]))
                            {
                                sw.Write(dr[j].ToString() + "\t");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    sw.Close();
                }
            }

            string startPath = mappath;//folder to add
            string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);
            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);

            string filename = zipPath;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryReport.zip");
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(fileInfo.FullName);
                //Response.End();
            }

            /*
            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            Response.Clear();
            Response.BufferOutput = false;
            //string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment; filename=" + zipPath);
            zip.Save(Response.OutputStream);
            Response.End();
            */
        }
        //public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        //{
        //    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
        //        yield return day;
        //}

        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["analyticsdata"];
            //grv.DataSource = null;
            //grv.DataSource = dt;
            //grv.DataBind();
            //GridFormat(dt);
            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
        }
    }
}