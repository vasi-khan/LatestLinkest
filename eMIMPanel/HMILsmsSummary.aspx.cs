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
    public partial class HMILsmsSummary : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string s3 = "";
        string usertype = "";
        string user = "";
        string DLTNO = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                Helper.Util ob = new Helper.Util();
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
                s3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
                s3 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            string usertype = Convert.ToString(Session["UserType"]);
            DLTNO = "";
            foreach (ListItem item in lstHMIL.Items)
            {
                if (item.Selected)
                {
                    DLTNO += "'" + item.Value + "',";
                }
            }
            if (DLTNO != "")
            {
                DLTNO = DLTNO.Substring(0, DLTNO.Length - 1);
            }
            string senderID = ddlSenderID.SelectedValue.ToString().Trim();
            string TemplateID = ddlTemplate.SelectedValue.ToString().Trim();
            DataTable dt = new DataTable();
            dt = ob.GetHMILSMSSummary(s1, s2, s3, DLTNO, senderID, TemplateID);
            ViewState["dt"] = dt;
            grv.DataSource = null;
            grv.DataSource = dt;
            SetFooterValue(dt);
            grv.DataBind();
            GridFormat(dt);
            if (senderID != "" && senderID != "0")
            {
                grv.Columns[10].Visible = true;
            }
            if (TemplateID != "" && TemplateID != "0")
            {
                grv.Columns[11].Visible = true;
            }
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[9] {
                new DataColumn("1"),
                new DataColumn("2"),
                new DataColumn("3"),
                new DataColumn("4"),
                new DataColumn("5"),
                new DataColumn("6"),
                new DataColumn("7"),
                new DataColumn("8"),
                new DataColumn("9")
            });
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(Unknown)", string.Empty);

            grv.Columns[5].FooterText = "Total : ";
            grv.Columns[6].FooterText = sumSubmitted.ToString();
            grv.Columns[7].FooterText = sumDelivered.ToString();
            grv.Columns[8].FooterText = sumFailed.ToString();
            grv.Columns[9].FooterText = sumUnknown.ToString();
            dt.Rows.Add("", "", "", "", "Total ", sumSubmitted.ToString(), sumDelivered.ToString(), sumFailed.ToString(), sumUnknown.ToString());
            dt.AcceptChanges();
            Session["FottorValue"] = dt;
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
            DataTable dt = ob.GetDayWiseSMSSummaryDetail(lblUserId.Text, lbldate.Text);
            grv2.DataSource = null;
            grv2.DataSource = dt;
            grv2.DataBind();
            GridFormat2(dt);
            pnlPopUp_Detail_ModalPopupExtender.Show();
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
            Helper.Util ob = new Helper.Util();

            int size = 3;
            string UserID = Convert.ToString(Session["UserID"]);
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
                    string bal = ob.UdateAndGetURLbal(Convert.ToString(Session["UserID"]));
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
            byte[] qc;
            using (MemoryStream stream2 = new MemoryStream())
            {
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
                        string rowtext = dt.Rows[i][k].ToString().Replace(",", "");
                        columnbind.Append(rowtext.ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                    }
                    columnbind.Append("\r\n");
                }

                DataTable FDT=Session["FottorValue"] as DataTable;
                if (FDT!=null && FDT.Rows.Count>0)
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

        private void ExportToExcel()
        {
            string attachment = "attachment; filename=SMSSummaryReport.xls";
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
            {
                string senderID = ddlSenderID.SelectedValue.ToString().Trim();
                string TemplateID = ddlTemplate.SelectedValue.ToString().Trim();
                DataView view = new DataView((ViewState["dt"] as DataTable));
                DataTable distinctValues = new DataTable();
                distinctValues = view.ToTable(true, "userid", "FULLNAME", "COMPNAME", "FromDATE", "ToDATE", "Submitted", "Delivered", "Failed", "Unknown");
                if (senderID != "" && senderID != "0")
                {
                    distinctValues = view.ToTable(true, "userid", "FULLNAME", "COMPNAME", "FromDATE", "ToDATE", "Submitted", "Delivered", "Failed", "Unknown", "SenderId");
                }
                if (TemplateID != "" && TemplateID != "0")
                {
                    distinctValues = view.ToTable(true, "userid", "FULLNAME", "COMPNAME", "FromDATE", "ToDATE", "Submitted", "Delivered", "Failed", "Unknown", "SenderId", "TemplateID");
                }
                DatatableToCSV(distinctValues, "HMILSMSSummary.csv");  //ExportToExcel();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
            }
        }

        protected void lstHMIL_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in lstHMIL.Items)
            {
                if (item.Selected)
                {
                    DLTNO += "'" + item.Value + "',";
                }
            }
            if (DLTNO != "")
            {
                DLTNO = DLTNO.Substring(0, DLTNO.Length - 1);
                SenderID(DLTNO);
            }
        }

        private void SenderID(string message)
        {
            DataTable dt = ob.GetSenderIdDLTNoWise(message);
            ddlSenderID.DataSource = dt;
            ddlSenderID.DataTextField = "SENDERID";
            ddlSenderID.DataValueField = "SENDERID";
            ddlSenderID.DataBind();
            ListItem objListItem = new ListItem("--Select Sender ID--", "0");
            ddlSenderID.Items.Insert(0, objListItem);
            ddlSenderID.SelectedIndex = 0;
        }

        protected void ddlSenderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SenderID = ddlSenderID.SelectedValue.ToString();
            DataTable dt = ob.GetTemplateIdSenderWise(SenderID);
            ddlTemplate.DataSource = dt;
            ddlTemplate.DataTextField = "TemplateName";
            ddlTemplate.DataValueField = "template";
            ddlTemplate.DataBind();
            ListItem objListItem = new ListItem("--Select Sender ID--", "0");
            ddlTemplate.Items.Insert(0, objListItem);
            ddlTemplate.SelectedIndex = 0;
        }
    }
}