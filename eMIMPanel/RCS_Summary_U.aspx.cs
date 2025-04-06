using Shortnr.Web.Business.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class RCS_Summary_U : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        rcscode.UtilN ob = new rcscode.UtilN();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            Session["RCSUserID"] = Convert.ToString(rcscode.database.GetScalarValue("select top 1 RCSACCID  from MapSMSAcc where smsAccId='" + Convert.ToString(Session["UserID"]) + "'"));

            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["RCSUserID"]);
            if (user == "") Response.Redirect("login.aspx");
            //if (usertype != "SYSADMIN") Response.Redirect("INDEX2.aspx");

            if (!IsPostBack)
            {
                PopulateCampaign();

                string ws = Convert.ToString(Session["DOMAINNAME"]);
                rcscode.UtilN ob = new rcscode.UtilN();
                DataTable dt = ob.GetUserReport(user, ws, "02/JAN/1900", "02/JAN/1900", false);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);

                dt = ob.GetUserReportDetail(user, "-22", false);
                grv2.DataSource = null;
                grv2.DataSource = dt;
                grv2.DataBind();
                GridFormat2(dt);

            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;


            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
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
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            rcscode.UtilN ob = new rcscode.UtilN();
            string campname = "";
            if (ddlCamp.SelectedIndex > 0)
            {
                campname = ddlCamp.SelectedItem.Text;
            }
            string usertype = Convert.ToString(Session["UserType"]);
            //if (username != "")
            //{
            //    if (usertype.ToUpper() == "ADMIN" && Convert.ToString(Session["DLT"]) != ob.ValidateDLT(username))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('User does not belong to this admin. !!')", true);
            //        return;
            //    }
            //}

            string user = Convert.ToString(Session["RCSUserID"]);
            //DataTable dt = ob.GetSMSReport_user(s1, s2, usertype, user);
            //DataTable dt = ob.GetUserReport(user, Request.Url.Scheme + @"://" + Request.Url.Authority, s1, s2, false);
            DataTable dt = ob.RcsGetDayWiseSummaryUser(user, s1, s2, usertype, campname);
            grv.DataSource = null;
            grv.DataSource = dt;
            SetFooterValue(dt);
            grv.DataBind();

            GridFormat(dt);
            //Session["analyticsdata"] = dt;
        }

        public void PopulateCampaign()
        {
            rcscode.UtilN ob = new rcscode.UtilN();
            DataTable dt = ob.GetCampaignAllUser(user);

            ddlCamp.DataSource = dt;
            ddlCamp.DataTextField = "campaign";
            ddlCamp.DataValueField = "campaign";
            ddlCamp.DataBind();
            ListItem objListItem = new ListItem("--Campaign Name--", "0");
            ddlCamp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlCamp.SelectedIndex = 1;
            else
                ddlCamp.SelectedIndex = 0;
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Fail)", string.Empty);
            object sumRejected;
            sumRejected = copyDataTable.Compute("Sum(REJECTED)", string.Empty);

            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(unknown)", string.Empty);
            object sumSeenCount;
            sumSeenCount = copyDataTable.Compute("Sum(seen)", string.Empty);

            grv.Columns[3].FooterText = "Total : ";
            grv.Columns[4].FooterText = sumSubmitted.ToString();
            grv.Columns[5].FooterText = sumDelivered.ToString();
            grv.Columns[6].FooterText = sumFailed.ToString();
            grv.Columns[7].FooterText = sumRejected.ToString();
            grv.Columns[8].FooterText = sumUnknown.ToString();
            grv.Columns[9].FooterText = sumSeenCount.ToString();

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
        protected void GridFormat2(DataTable dt)
        {
            grv2.UseAccessibleHeader = true;
            grv2.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv2.TopPagerRow != null)
            {
                grv2.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv2.BottomPagerRow != null)
            {
                grv2.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv2.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            //modalpopuppwd.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblUserId = (Label)gvro.FindControl("lblUserId");
            Label lbldate = (Label)gvro.FindControl("lblDate");
            HiddenField hdndate = (HiddenField)gvro.FindControl("hdndate");

            DataTable dt = ob.RcsGetDayWiseSummaryDetails(lblUserId.Text, lbldate.Text);
            //Session["rptDetail"] = dt;

            grv2.DataSource = null;
            grv2.DataSource = dt;
            grv2.DataBind();
            GridFormat2(dt);

            pnlPopUp_Detail_ModalPopupExtender.Show();

            //< div class="collapse show" id="panel-4" runat="server">
            //collapseOne.Attributes.Add("class", "collapse");
            //collapseTwo.Attributes.Add("class", "collapse show");

            //string url = ResolveUrl("~\\Analytics-detail_u.aspx");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SHOW_REPORT", "window.open('" + url + "');", true);

        }

        protected void QR_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblUserId = (Label)gvro.FindControl("lblUserId");
            HiddenField hdnSegment = (HiddenField)gvro.FindControl("hdnSegment");

            GenerateQR(hdnSegment.Value);
        }
        protected void GenerateQR(string segment1)
        {
            rcscode.UtilN ob = new rcscode.UtilN();

            int size = 3;
            string UserID = Convert.ToString(Session["RCSUserID"]);
            //string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/") + segment1;
            string ws1 = Convert.ToString(Session["DOMAINNAME"]);
            string url = ws1 + segment1;
            if (url.Substring(url.Length - 2, 2) != "_Q")
            {
                string[] segment = url.Split('/');
                string seg = segment[segment.Length - 1];
                bool exists = ob.SegmentExists(seg + "_Q");
                if (!exists)
                {
                    string LongUrl = ob.GetLongURLforQR(seg);
                    UrlManager ob2 = new UrlManager();
                    string sUrl = ob2.ShortenURL1(UserID, LongUrl, Request.UserHostAddress, seg + "_Q", "N");
                    //lblShortURL.Text = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", sUrl);
                    string bal = ob.UdateAndGetURLbal(Convert.ToString(Session["RCSUserID"]));
                    //lblURLbal.Text = bal;
                    Session["URLBALANCE"] = bal;
                    url = url + "_Q";
                }
                else
                {
                    url = url + "_Q";
                }
            }

            Bitmap qrcd = ob.convertStringtoBitMap(Convert.ToString(url), size);

            //if (rdbQRColor.SelectedValue == "R") qrcd = ob.ChangeColor(qrcd, Color.Black, Color.Red);
            //if (rdbQRColor.SelectedValue == "G") qrcd = ob.ChangeColor(qrcd, Color.Black, Color.Green);
            //if (rdbQRColor.SelectedValue == "B") qrcd = ob.ChangeColor(qrcd, Color.Black, Color.Blue);

            //string hex = "#000000" ;
            //Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

            //qrcd = ob.ChangeColor(qrcd, Color.Black, _color);

            byte[] qc;
            using (MemoryStream stream2 = new MemoryStream())
            {
                //Bitmap resized = new Bitmap(QRCodeImage, new Size(QRCodeImage.Width * 2, QRCodeImage.Height * 2));
                qrcd.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                qc = stream2.ToArray();
            }

            byte[] Qcode = qc;
            string Qstring = Convert.ToBase64String(qc, 0, qc.Length);
            string imgDataURL = string.Format("data:image/png;base64,{0}", Qstring);
            System.Web.UI.WebControls.Image imgQR = new System.Web.UI.WebControls.Image();
            imgQR.ImageUrl = imgDataURL;

            string base64 = imgQR.ImageUrl.Split(',')[1];
            Session["IMGDW"] = base64;
            Response.Redirect("download-img.aspx");

            return;

            //primaryStyle.BorderColor = System.Drawing.Color.Red;
            //if (txtFramColor.Text == "") txtFramColor.Text = "000000";
            //divQR.Style.Add("Border", "10px solid #" + txtFramColor.Text);
            //if (size == 3) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-10 p-2");
            //if (size == 2) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-6 p-2");
            //if (size == 1) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-4 p-2");

        }

        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["analyticsdata"];
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();

            GridFormat(dt);


            collapseOne.Attributes.Add("class", "collapse show");
            collapseTwo.Attributes.Add("class", "collapse");
        }
        private void ExportToExcel()
        {
            grv.Columns[1].Visible = false;
            string attachment = "attachment; filename=RcsSummaryReportUser.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grv.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
                ExportToExcel();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        private void ExportToExcelDetails()
        {
            string attachment = "attachment; filename=RcsSummaryDetailsUser.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grv2.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());

            Response.End();

        }

        protected void Lnkdownload_Click1(object sender, EventArgs e)
        {
            if (grv2.Rows.Count > 0)
                ExportToExcelDetails();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }
    }
}