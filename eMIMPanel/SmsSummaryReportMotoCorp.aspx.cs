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
using System.Text;

namespace eMIMPanel
{
    public partial class SmsSummaryReportMotoCorp : System.Web.UI.Page
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
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                BindData();  //Bind Data For DropDown
                SetDropDownListItemColor();
                DataTable dt = ob.GetSMSReport_user_new2("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public void BindData()
        {
            try
            {
                //--Bind Camaign DropDown Data
                DataTable dt = ob.GetCampaignToday(Convert.ToString(Session["UserID"]));
                ddlCamp.DataSource = dt;
                ddlCamp.DataTextField = "campaignname";
                ddlCamp.DataValueField = "campaignname";
                ddlCamp.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddlCamp.Items.Insert(0, objListItem);
                if (dt.Rows.Count == 1)
                    ddlCamp.SelectedIndex = 1;
                else
                    ddlCamp.SelectedIndex = 0;

                //--Bind TempIDAndName DropDown Data
                DataTable dtTempIdAndName = ob.GetTempIdAndName(Convert.ToString(Session["UserID"]));
                ddlTempIdAndName.DataSource = dtTempIdAndName;
                ddlTempIdAndName.DataTextField = "tempname";
                ddlTempIdAndName.DataValueField = "templateid";
                ddlTempIdAndName.DataBind();
                ListItem objListItemTemp = new ListItem("--Select--", "0");
                ddlTempIdAndName.Items.Insert(0, objListItemTemp);

                //Bind Category(dtCategory) DropDown Data
                DataTable dtCategory = ob.GetCategory((string)Session["userId"]);
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ListItem objListItemCategory = new ListItem("--All--", "0");
                ddlCategory.Items.Insert(0, objListItemCategory);

                //Bind Location(ddlLocation) DropDown Data
                DataTable dtLocation = ob.GetLocation((string)Session["userId"]);
                ddlLocation.DataSource = dtLocation;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationID";
                ddlLocation.DataBind();
                ListItem objListItemLocation = new ListItem("--All--", "0");
                ddlLocation.Items.Insert(0, objListItemLocation);


                //Bind SubLocation(ddlSubLocation) DropDown Data
                DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"]);
                ddlSubLocation.DataSource = dtSubLocation;
                ddlSubLocation.DataTextField = "SubLocationName";
                ddlSubLocation.DataValueField = "SubLocationID";
                ddlSubLocation.DataBind();
                ListItem objListItemSubLocation = new ListItem("--All--", "0");
                ddlSubLocation.Items.Insert(0, objListItemSubLocation);


                //Bind DealerCode(ddlDealerCode) DropDown Data
                DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"]);
                ddlDealerCode.DataSource = dtDealerCode;
                ddlDealerCode.DataTextField = "DLRName";
                ddlDealerCode.DataValueField = "DLRCODE";
                ddlDealerCode.DataBind();
                ListItem objListItemDealerCode = new ListItem("--All--", "0");
                ddlDealerCode.Items.Insert(0, objListItemDealerCode);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        public void SetDropDownListItemColor()
        {
            foreach (ListItem item in ddlDealerCode.Items)
            {
                item.Attributes.CssStyle.Add("font-family", "Consolas");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                s1 = hdntxtFrm.Value;
                s2 = hdntxtTo.Value;
                GetData();
                txtFrm.Text = hdntxtFrm.Value;
                txtTo.Text = hdntxtTo.Value;
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
            if (txtFrm.Text.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            string CampName = ddlCamp.SelectedValue.ToString();
            string TempIdAndName = ddlTempIdAndName.SelectedValue.ToString();
            if (TempIdAndName == "0")
            {
                TempIdAndName = "";
            }
            Helper.Util ob = new Helper.Util();
            string user = Convert.ToString(Session["UserID"]);
            string CategoryID = ddlCategory.SelectedValue.ToString();
            string LocationID = ddlLocation.SelectedValue.ToString();
            string SubLocationID = ddlSubLocation.SelectedValue.ToString();
            string Dealer = ddlDealerCode.SelectedValue.ToString();
            DataTable dt = ob.GetSMSReportuserMotoCorp(s1, s2, usertype, user, CampName, txtMobileNo.Text.Trim(), TempIdAndName, CategoryID, LocationID, SubLocationID, Dealer);
            DataTable dt1 = new DataTable();
            dt1 = dt.Copy();
            ViewState["Finaldt"] = dt1;
            grv.DataSource = null;
            grv.DataSource = dt;
            SetFooterValue(dt);
            grv.DataBind();
            GridFormat(dt);
        }

        private void ExportToExcel()
        {
            try
            {

                string attachment = "attachment; filename=report_obd.xls";
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";

                StringWriter oStringWriter = new StringWriter();
                HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                grv.RenderControl(oHtmlTextWriter);
                Response.Write(oStringWriter.ToString());
                Response.Flush();
                Response.End();
                //Thread.ResetAbort();
            }
            catch (Exception ex)
            {

            }

        }

        public void csv(DataTable dt)
        {
            try
            {
                Response.Clear();
                Response.ClearHeaders(); //use by me
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Report.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                //    Response.ContentEncoding = Encoding.UTF8;
                //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
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



                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
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
            {
                try
                {
                    grv.FooterRow.TableSection = TableRowSection.TableFooter;
                }
                catch (Exception ex) { }
            }
        }

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
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
        }

        protected void btnXL_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField fileid = (HiddenField)gvro.FindControl("hdnFileId");
            HiddenField userid = (HiddenField)gvro.FindControl("hdnUserId");
            string reqsrc = ((Label)gvro.FindControl("lblSource")).Text;

            Label lblsender = (Label)gvro.FindControl("lblSenderID");
            Label lblDealerCode = (Label)gvro.FindControl("lblDealerCode");
            Label lblSubmitDate = (Label)gvro.FindControl("lblSubmitDate");
            s1 = Convert.ToDateTime(lblSubmitDate.Text.ToString(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = Convert.ToDateTime(lblSubmitDate.Text.ToString(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";

            DataTable dt = ob.GetSMSReportDetailUserMotoCorp(userid.Value, fileid.Value, Convert.ToString(lblsender.Text), s1, s2, reqsrc, txtMobileNo.Text.ToString().Trim(), lblDealerCode.Text.ToString());
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

            if (hdntxtFrm1.Value.Trim() == "")
            {
                d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                d1 = Convert.ToDateTime(hdntxtFrm1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                d2 = Convert.ToDateTime(hdntxtTo1.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string senderid = lblsender.Text;
            DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL_new2(d1, d2, senderid, user);

            DataView dv = new DataView(dt);
            DataTable dtDates = dv.ToTable(true, "SMSdate");
            string mainPath = "DWReports";
            string subPath = "DWReports/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = Server.MapPath(subPath);
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);

            for (int i = 0; i < dtDates.Rows.Count; i++)
            {
                string mydate = dtDates.Rows[i]["SMSdate"].ToString();
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = "SMSdate = '" + mydate + "'";
                DataTable myDataTable = dv1.ToTable();

                string myTableAsString =
                String.Join(Environment.NewLine, myDataTable.Rows.Cast<DataRow>().
                Select(r => r.ItemArray).ToArray().
                Select(x => String.Join("\t", x.Cast<string>())));
                StreamWriter myFile = new StreamWriter(mappath + @"\" + mydate.Replace(".", "-") + ".txt");
                myFile.WriteLine("SMS Date" + "\t" + "Message ID" + "\t" + "Mobile No" + "\t" + "Sender ID" + "\t" + "Sent Date" + "\t" + "Delivered Date" + "\t" + "Message" + "\t" + "Status" + "\t" + "Delivery Response");
                myFile.WriteLine(myTableAsString);
                myFile.Close();
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
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(Unknown)", string.Empty);

            grv.Columns[9].FooterText = "Total : ";
            grv.Columns[10].FooterText = sumSubmitted.ToString();
            grv.Columns[11].FooterText = sumDelivered.ToString();
            grv.Columns[12].FooterText = sumFailed.ToString();
            grv.Columns[13].FooterText = sumUnknown.ToString();
        }

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

        protected void btnSumm_Click(object sender, EventArgs e)
        {
            Response.Redirect("usmsSummery.aspx");
        }

        //Bind Location(ddlLocation) DropDown Data
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            DataTable dtLocation = ob.GetLocation((string)Session["userId"],CategoryId);
            if (dtLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlLocation.Items.Insert(0, objListItem);

            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItemSubLocation = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItemSubLocation);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind SubLocation(ddlSubLocation) DropDown Data
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItem);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind DealerCode(ddlDealerCode) DropDown Data
        protected void ddlSubLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItem);
        }

        protected void btnDownload_Click1(object sender, EventArgs e)
        {

            DataTable dt = ViewState["Finaldt"] as DataTable; 
            if (dt.Rows.Count > 0 && dt != null)
            {
                dt.Columns.Remove("sr");
                dt.Columns.Remove("SL");
                //dt.Columns.Remove("reqsrc");
                //dt.Columns.Remove("sender");
                //dt.Columns.Remove("submitted");
                //dt.Columns.Remove("delivered");
                //dt.Columns.Remove("failed");
                //dt.Columns.Remove("unknown");
                //dt.Columns.Remove("filenm");
                //dt.Columns.Remove("msg");
                //dt.Columns.Remove("fileid");
                //dt.Columns.Remove("userid");
                dt.AcceptChanges();
                csv(dt);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No Data Found !!');", true);
            }
        }
    }
}