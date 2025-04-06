using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace eMIMPanel
{
    public partial class add_template_u : System.Web.UI.Page
    {
        string _user = "";
        string senderId = "";
        Helper.Util obj = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                Session["FILENAME"] = "";


                if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"].ToString())=="91")
                {
                    idsenderidIND.Visible = true;
                    txtTempId.MaxLength = 19;
                }
                else
                {
                    idsenderidALL.Visible = true;
                }
            }

           
            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                lblfn.Text = FileName;
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = "~/TemplateDoc/";
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Session["FILENAME"] = FileName;
            }
        }


       
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string s = txtName.Text;
            string d = txtmsg.Text;
            string tempid = txtTempId.Text;
            
            if (idsenderidALL.Visible==true)
            {
                senderId=txtSenderIdall.Text.Trim();
            }
            else
            {
                senderId = tstsenderind.Text.Trim();
            }
            
            if (senderId.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter senderId');", true);
                return;
            }
            if (s.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Name');", true);
                return;
            }

            if (d.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Message Text');", true);
                return;
            }

            if (tempid.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template ID');", true);
                return;
            }
            string f = Convert.ToString(Session["FILENAME"]);
            if (f == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload file.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();

            DataSet ds = ob.ValidateTemplateRequest(tempid, s, _user);

            if (Convert.ToInt16(ds.Tables[0].Rows[0]["TemplateId"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template ID already exist');", true);
                return;
            }
            if (Convert.ToInt16(ds.Tables[1].Rows[0]["Templatename"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Name already exist');", true);
                return;
            }


            ob.SaveTemplateRequest(_user, d, f, s, tempid, false, senderId);
            Session["FILENAME"] = "";
            lblfn.Text = "";
            SendEmail();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Request generated successfully.');window.location ='add-template_u.aspx';", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("add-template_u.aspx");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                lblfn.Text = FileName;
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                //if (Extension.ToUpper().Trim() == ".RAR" || Extension.ToUpper().Trim() == ".ZIP" || Extension.ToUpper().Trim() == ".PDF" || Extension.ToUpper().Trim() == ".BMP" || Extension.ToUpper().Trim() == ".JPG" || Extension.ToUpper().Trim() == ".JPEG" || Extension.ToUpper().Trim() == "PNG" || Extension.ToUpper().Trim() == "GIF")
                //{
                string FolderPath = "~/TemplateDoc/";
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Session["FILENAME"] = FileName;
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

            Helper.Util ob = new Helper.Util();
            TemplateField templateFieldTemplateId = (TemplateField)grvTemplate.Columns[1];
            if (Convert.ToString(Session["Hidetemplateid"]) == "True")
            {
                templateFieldTemplateId.Visible = false;
            }
            else
            {
                templateFieldTemplateId.Visible = true;
            }
            grvTemplate.DataSource = ob.GetTemplateList(_user);
            grvTemplate.DataBind();
            pnlPopUp_NUMBER_ModalPopupExtender.Show();
        }
        public void Email(string To, string CC, string Subject, string body)
        {
            string result = "";

            string ToEmailId = To;
            string Host = "";
            string Port = "";
            string UserId = "";
            string Password = "";
            // ToEmailId = "rachit@myinboxmedia.com";

            //string Subject = "Low Balance Report for " + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Host = "smtpout.secureserver.net";
            Port = "25";
            UserId = "support@myinboxmedia.io";
            Password = "MiM#987654321";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(UserId, Password),
                    Timeout = 30000
                };
                //smtp.UseDefaultCredentials = false;

                //smtp.Credentials = new System.Net.NetworkCredential(UserId, Password);
                MailMessage message = new MailMessage(UserId, ToEmailId, Subject, body);
                message.CC.Add(CC);
                message.IsBodyHtml = true;

                smtp.EnableSsl = true;
                smtp.Send(message);
                result = "Sent Successfully..!!";
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
            }

        }


        public void SendEmail()
        {
           DataTable dt = obj.GetRecordDataTable("SP_GetSettingR");
            string to = "";
            string cc = "";
            string subject = "";
            string body = "";

            if (dt!=null && dt.Rows.Count>0)
            {
                
                 to = Convert.ToString(dt.Rows[0]["SettingRequestTemplateTO"]);
                 cc = Convert.ToString(dt.Rows[0]["SettingRequestTemplateCC"]);
                 subject = Convert.ToString(dt.Rows[0]["SettingRequestTemplateSUB"]);
                subject = subject.Replace("{#USERID#}", _user);
                 body = Convert.ToString(dt.Rows[0]["SettingRequestTemplateBODY"]);
                 body = body.Replace("{#USERID#}", _user);
                 body = body.Replace("{#TEMPLATEID#}", txtTempId.Text.Trim());
                 body = body.Replace("{#TEMPLATETEXT#}", txtmsg.Text.Trim());


                //Dear Team, {#USERID#} has added Template for approval.    Template ID - {#TEMPLATEID#}    Template Text - {#TEMPLATETEXT#}      Please Approve


                Email(to,cc, subject, body);

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTemplateListDownload(_user);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("INSERTDATETIME");
                Session["TemplateData"] = dt;
                Session["FILENAME2"] = "TemplateReport.xls";
                Response.Redirect("DownloadTemplateUser.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data Found.');", true);
            }
        }
    }
}