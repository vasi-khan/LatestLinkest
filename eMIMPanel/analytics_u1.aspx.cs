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
    public partial class analytics_u1 : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                string ws = Convert.ToString(Session["DOMAINNAME"]);
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetUserReport(user,ws, "02/JAN/1900", "02/JAN/1900", false);
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
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            //DataTable dt = ob.GetSMSReport_user(s1, s2, usertype, user);
            //DataTable dt = ob.GetUserReport(user, Request.Url.Scheme + @"://" + Request.Url.Authority, s1, s2, false);
            DataTable dt = ob.GetUserReport(user, "", s1, s2, false);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();

            GridFormat(dt);
            Session["analyticsdata"] = dt;
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
            Label lblurlid = (Label)gvro.FindControl("lblurlid");

            DataTable dt = ob.GetUserReportDetail(lblUserId.Text, lblurlid.Text, false);
            Session["rptDetail"] = dt;

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

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((Session["analyticsdata"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "userid", "pagename", "btn", "LongURL", "smallURL", "No_Of_Hits", "CreationDate", "URLID");
                DatatableToCSV(distinctValues, "Analytics.csv");  //ExportToExcel();

            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        protected void lnkDownloadDetails_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((Session["rptDetail"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "ClickDate", "ip", "Platform", "Browser", "IsMobileDevice", "MobileDeviceManufacturer", "MobileDeviceModel");
                DatatableToCSV(distinctValues, "AnalyticsDetails.csv");  //ExportToExcel();

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
    }
}