using Shortnr.Web.Business.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class qr_code_u : System.Web.UI.Page
    {
        private Style primaryStyle = new Style();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnDownload);

            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx");

            string usr = Session["UserID"].ToString();
            //lblUser.Text = usr;
            if (!IsPostBack)
            {

                Helper.Util ob = new Helper.Util();

                //string b = ob.GetURLbal(Session["UserID"].ToString());
                //lblURLbal.Text = b;
                //Session["URLBALANCE"] = b;
                //lblSMS.Text = "This is ASPX Page";
                LoadURLs();
                txtQRCodeColor.Text = "000000"; txtQRCodeColor.BackColor = Color.Black;
                txtFramColor.Text = "000000"; txtFramColor.BackColor = Color.Black;
            }

            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                int size = 3;
                size = rdbQRsize.SelectedValue == "L" ? 3 : (rdbQRsize.SelectedValue == "M" ? 2 : 1);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = "QRHeadImg/";
                string filename = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                string FilePath = Server.MapPath(FolderPath + filename);

                FileUpload1.SaveAs(FilePath);
                imgPicture.ImageUrl = "~/QRHeadImg/" + filename;
                if (size == 3) { imgPicture.Width = 150; imgPicture.Height = 150; }
                if (size == 2) { imgPicture.Width = 100; imgPicture.Height = 100; }
                if (size == 1) { imgPicture.Width = 50; imgPicture.Height = 50; }
            }
        }

        public void LoadURLs()
        {
            Helper.Util ob = new Helper.Util();
            string ws = Convert.ToString(Session["DOMAINNAME"]);
            //DataTable dt = ob.GetURLSofUser_4QR(Convert.ToString(Session["UserID"]), "", string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/"));
            DataTable dt = ob.GetURLSofUser_4QR(Convert.ToString(Session["UserID"]), "", ws);

            ddlURL.DataSource = dt;
            ddlURL.DataTextField = "shorturl";
            ddlURL.DataValueField = "shorturl";
            ddlURL.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlURL.Items.Insert(0, objListItem);
            ddlURL.SelectedIndex = 0;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("qr-code_u.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (ddlURL.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Short URL first.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();

            int size = 3;
            size = rdbQRsize.SelectedValue == "L" ? 3 : (rdbQRsize.SelectedValue == "M" ? 2 : 1);
            lblTitle.Text = txtText.Text;
            //if (FileUpload1.HasFile)
            //{

            //}
            string UserID = Convert.ToString(Session["UserID"]);
            string url = ddlURL.Text;

            if (url.Substring(url.Length - 2, 2) != "_Q")
            {
                string[] segment = url.Split('/');
                string seg = segment[segment.Length - 1];
                bool exists = ob.SegmentExists(seg + "_Q");
                if (!exists)
                {
                    string LongUrl = ob.GetLongURLforQR(seg);
                    UrlManager ob2 = new UrlManager();

                    //string sUrl= = ob.NewShortURLfromSQL(Session["DOMAINNAME"].ToString());
                    string sUrl= seg + "_Q";

                    ob.SaveShortURL(UserID, LongUrl, Request.UserHostAddress, sUrl, "N", "N", Session["DOMAINNAME"].ToString());
                    //string sUrl = ob2.ShortenURL1(UserID, LongUrl, Request.UserHostAddress, seg + "_Q", "N");

                    //lblShortURL.Text = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, "/", sUrl);
                    //string bal = ob.UdateAndGetURLbal(Convert.ToString(Session["UserID"]));
                    //lblURLbal.Text = bal;
                    //Session["URLBALANCE"] = bal;
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
            if (txtQRCodeColor.Text == "") txtQRCodeColor.Text = "000000";
            string hex = "#" + txtQRCodeColor.Text;
            Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

            qrcd = ob.ChangeColor(qrcd, Color.Black, _color);
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
            imgQR.ImageUrl = imgDataURL;
            btnDownload.Visible = true;
            //primaryStyle.BorderColor = System.Drawing.Color.Red;
            if (txtFramColor.Text == "") txtFramColor.Text = "000000";
            divQR.Style.Add("Border", "10px solid #" + txtFramColor.Text);
            if (size == 3) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-10 p-2");
            if (size == 2) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-6 p-2");
            if (size == 1) divQROutside.Attributes.Add("class", "card card-body shadow border-0 col-lg-4 p-2");

            //if (rdbFrameColor.SelectedValue=="G") divQR.Style.Add("Border","10px solid Green");
            //if (rdbFrameColor.SelectedValue == "R") divQR.Style.Add("Border", "10px solid Red");
            //if (rdbFrameColor.SelectedValue == "B") divQR.Style.Add("Border", "10px solid Blue");

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {


        }
        protected void btnExport_Click(object sender, EventArgs e)
        {

            string base64 = Request.Form[hfImageData.UniqueID].Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64);

            //Response.Clear();
            //Response.ContentType = "image/png";
            //Response.AddHeader("Content-Disposition", "attachment; filename=QRCode.png");
            //Response.Buffer = true;
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(bytes);
            //Response.End();

            Response.AddHeader("Cache-Control", "no-cache, must-revalidate, post-check=0, pre-check=0");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Content-Description", "File Download");
            Response.AddHeader("Content-Type", "application/force-download");
            Response.AddHeader("Content-Transfer-Encoding", "binary\n");
            Response.AddHeader("content-disposition", "attachment;filename=QRCode.png");
            Response.BinaryWrite(bytes);
            Response.End();
        }
        protected void Btndownload_click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imgQR.ImageUrl))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please generate QR Code first.');", true);
                return;
            }
            string base64 = imgQR.ImageUrl.Split(',')[1];
            Session["IMGDW"] = base64;
            Response.Redirect("download-img.aspx");
            //Server.Transfer("download-img.aspx");
            return;
        }

    }
}