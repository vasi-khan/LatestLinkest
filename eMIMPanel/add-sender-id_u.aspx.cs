using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json.Linq;
using eMIMPanel.Helper;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace eMIMPanel
{
    public partial class add_sender_id_u : System.Web.UI.Page
    {
        string _user = "";
        Helper.Util obj = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = Convert.ToString(Session["UserID"]);
            if (_user == "") Response.Redirect("Login.aspx");
            txtDLT.Text = Convert.ToString(Session["DLT"]);

           
            if (!IsPostBack)
            {
                PopulateCountry();

                ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);

                if (Convert.ToString(ddlCCode.SelectedValue) != "91")
                    divTempsms.Attributes.Add("class", "form-group row d-none");
                else
                    divTempsms.Attributes.Add("class", "form-group row d-block");


                //List<string> countryList = CountryList();
                //string duplicateItem = "";
                //foreach (var items in countryList)
                //{
                //    string ccode = GetCode(items.ToString());
                //    if (duplicateItem == "")
                //    {
                //        String text = String.Format("{0}-{1}\n",
                //                        items.ToString(), ccode);
                //        ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                //        //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                //        duplicateItem = items;
                //    }
                //    if (duplicateItem != items.ToString())
                //    {
                //        String text = String.Format("{0}-{1}\n",
                //                        items.ToString(), ccode);
                //        //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                //        ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                //        duplicateItem = items;
                //    }
                //}
                ////Imagetitlebind();
                //ddlCCode.Items.Insert(0, new ListItem("Select", "0"));

                Session["FILENAME"] = "";

            }

        }


        public void PopulateCountry()
        {
            Util ob = new Util();
            DataTable dt = ob.GetActiveCountry(Convert.ToString(Session["UserID"]));
            ddlCCode.DataSource = dt;
            ddlCCode.DataTextField = "name";
            ddlCCode.DataValueField = "countrycode";
            ddlCCode.DataBind();
        }

        public static string GetCountryCode(string country)
        {
            string countryCode = "";
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);
                if (region.EnglishName.ToUpper().Contains(country.ToUpper()))
                {
                    countryCode = region.TwoLetterISORegionName;
                }
            }
            return countryCode;
        }



        public string GetCode(string country)
        {
            string code = "";
            JObject weatherInfo = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Server.MapPath("~/TextFile.json")));
            code = Convert.ToString(((JToken)weatherInfo[GetCountryCode(country)])).Replace("\"", string.Empty);
            return code;
        }


        public static List<string> CountryList()
        {
            List<string> CultureList = new List<string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(CultureList.Contains(getRegionInfo.Name)))
                {
                    CultureList.Add(getRegionInfo.EnglishName);
                }
            }
            CultureList.Sort();
            return CultureList;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            List<string> liSender = new List<string>();
            string d = txtDLT.Text;
            string fn = Convert.ToString(Session["FILENAME"]);
            string s1 = txtSender1.Text;
            //string s2 = txtSender2.Text;
            //string s3 = txtSender3.Text;
            //string s4 = txtSender4.Text;
            //string s5 = txtSender5.Text;
            //string s6 = txtSender6.Text;
            //string s7 = txtSender7.Text;
            //string s8 = txtSender8.Text;
            //string s9 = txtSender9.Text;
            //string s10 = txtSender10.Text;
            //string s11 = txtSender11.Text;
            //string s12 = txtSender12.Text;
            //string s13 = txtSender13.Text;
            //string s14 = txtSender14.Text;
            //string s15 = txtSender15.Text;
            //string s16 = txtSender15.Text;

            if (s1.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Sender ID');", true);
                return;
            }

            if (d.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter DLT Registration Number');", true);
                return;
            }

            for (int i = 1; i < 17; i++)
            {
                TextBox tb = updFormArea.FindControl("txtSender" + i.ToString()) as TextBox;
                if (tb != null)
                {
                    if (!string.IsNullOrEmpty(tb.Text))
                    {
                        liSender.Add(tb.Text);
                    }
                }
            }

            string ccode = ddlCCode.SelectedValue;
            string query = "";
            if (liSender.Count > 0)
            {
                foreach (string s in liSender)
                {
                    query = query + string.Format("IF NOT EXISTS (SELECT * FROM senderidrequeset WHERE USERNAME='" + _user + "' AND SENDERID='" + s + "' )  insert into senderidrequeset (username,senderid,createdat,filepath,countrycode) values('{0}','{1}',GETDATE(),'{2}','{3}'); ", _user, s, fn, ccode);
                }
            }
            Helper.Util ob = new Helper.Util();
            ob.SaveMultipleSenderIdRequest(query);

            // string f = Convert.ToString(Session["FILENAME"]);
            //if (f == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload file.');", true);
            //    return;
            //}


            //string dlt = ob.GetDLTofUser(_user);
            //if(dlt != d)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered DLT Registration Number is not associated with User ID');", true);
            //    return;
            //}
            //if (txtSender2.Text == "")
            //{
            //    ob.SaveSenderIDRequest1(_user, s1, fn);
            //}
            //else if (txtSender3.Text == "")
            //{
            //    ob.SaveSenderIDRequest2(_user, s1, s2, fn);
            //}
            //else if (txtSender4.Text == "")
            //{
            //    ob.SaveSenderIDRequest3(_user, s1, s2, s3, fn);
            //}
            //else if (txtSender5.Text == "")
            //{
            //    ob.SaveSenderIDRequest4(_user, s1, s2, s3, s4, fn);
            //}
            //else if (txtSender6.Text == "")
            //{
            //    ob.SaveSenderIDRequest5(_user, s1, s2, s3, s4, s5, fn);
            //}
            //else if (txtSender7.Text == "")
            //{
            //    ob.SaveSenderIDRequest6(_user, s1, s2, s3, s4, s5, s6, fn);
            //}
            //else if (txtSender8.Text == "")
            //{
            //    ob.SaveSenderIDRequest7(_user, s1, s2, s3, s4, s5, s6, s7, fn);
            //}
            //else if (txtSender9.Text == "")
            //{
            //    ob.SaveSenderIDRequest8(_user, s1, s2, s3, s4, s5, s6, s7, s8, fn);
            //}
            //else if (txtSender10.Text == "")
            //{
            //    ob.SaveSenderIDRequest9(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, fn);
            //}
            //else if (txtSender11.Text == "")
            //{
            //    ob.SaveSenderIDRequest10(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, fn);
            //}
            //else if (txtSender12.Text == "")
            //{
            //    ob.SaveSenderIDRequest11(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, fn);
            //}
            //else if (txtSender13.Text == "")
            //{
            //    ob.SaveSenderIDRequest12(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, fn);
            //}
            //else if (txtSender14.Text == "")
            //{
            //    ob.SaveSenderIDRequest13(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, fn);
            //}
            //else if (txtSender15.Text == "")
            //{
            //    ob.SaveSenderIDRequest15(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, fn);
            //}
            //else
            //{
            //    ob.SaveSenderIDRequest16(_user, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, fn);
            //}

            //   ob.SaveSenderIDRequest(_user, s, f);
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID Reequest generated successfully.');", true);
            SendEmail();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID Reequest generated successfully.');window.location ='add-sender-id_u.aspx';", true);


            //txtDLT.Text = "";
            txtSender1.Text = "";
            Session["FILENAME"] = "";
            lblfn.Text = "";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("add-sender-id_u.aspx");
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
                string FolderPath = "~/SenderIDdoc/";
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Session["FILENAME"] = FileName;

                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Image / PDF / WinRar file');", true);
                //    return;
                //}
            }

        }

        protected void button1_Click(object sender, EventArgs e)
        {
            Sen2.Attributes.Add("style", "display:block;");
        }

        protected void button2_Click(object sender, EventArgs e)
        {
            Sen3.Attributes.Add("style", "display:block;");
        }

        protected void button3_Click(object sender, EventArgs e)
        {
            Sen4.Attributes.Add("style", "display:block;");
        }

        protected void button4_Click(object sender, EventArgs e)
        {
            Sen5.Attributes.Add("style", "display:block;");
        }
        protected void button5_Click(object sender, EventArgs e)
        {
            Sen6.Attributes.Add("style", "display:block;");
        }
        protected void button6_Click(object sender, EventArgs e)
        {
            Sen7.Attributes.Add("style", "display:block;");

        }
        protected void button7_Click(object sender, EventArgs e)
        {
            Sen8.Attributes.Add("style", "display:block;");
        }
        protected void button8_Click(object sender, EventArgs e)
        {
            Sen9.Attributes.Add("style", "display:block;");
        }
        protected void button9_Click(object sender, EventArgs e)
        {
            Sen10.Attributes.Add("style", "display:block;");
        }
        protected void button10_Click(object sender, EventArgs e)
        {
            Sen11.Attributes.Add("style", "display:block;");
        }
        protected void button11_Click(object sender, EventArgs e)
        {
            Sen12.Attributes.Add("style", "display:block;");
        }
        protected void button12_Click(object sender, EventArgs e)
        {
            Sen13.Attributes.Add("style", "display:block;");
        }
        protected void button13_Click(object sender, EventArgs e)
        {
            Sen14.Attributes.Add("style", "display:block;");
        }
        protected void button14_Click(object sender, EventArgs e)
        {
            Sen15.Attributes.Add("style", "display:block;");
        }

        protected void ddlCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(ddlCCode.SelectedValue) != "91")
                divTempsms.Attributes.Add("class", "form-group row d-none");
            else
                divTempsms.Attributes.Add("class", "form-group row d-block");
        }

        //Abhishek 25-01-2023
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
            string senderid = "";

            if (dt != null && dt.Rows.Count > 0)
            {
                to = Convert.ToString(dt.Rows[0]["SettingRequestSIDTO"]);
                cc = Convert.ToString(dt.Rows[0]["SettingRequestSIDCC"]);
                subject = Convert.ToString(dt.Rows[0]["SettingRequestSIDSUB"]);
                subject = subject.Replace("{#USERID#}", _user);
                body = Convert.ToString(dt.Rows[0]["SettingRequestSIDBODY"]);
                body = body.Replace("{#USERID#}", _user);

                if (Sen2.Style["display"] == "block")
                {
                    senderid = senderid = senderid+txtSender2.Text.Trim();
                }
                if (Sen3.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender3.Text.Trim();
                }
                if (Sen4.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender4.Text.Trim();
                }
                if (Sen5.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender5.Text.Trim();
                }


                if (Sen6.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender6.Text.Trim();
                }
                if (Sen7.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender7.Text.Trim();
                }
                if (Sen8.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender8.Text.Trim();
                }
                if (Sen9.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender9.Text.Trim();
                }
                if (Sen10.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender10.Text.Trim();
                }
                if (Sen11.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender11.Text.Trim();
                }
                if (Sen12.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender12.Text.Trim();
                }
                if (Sen13.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender13.Text.Trim();
                }
                if (Sen14.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender14.Text.Trim();
                }
                if (Sen15.Style["display"] == "block")
                {
                    senderid = senderid = senderid + txtSender15.Text.Trim();
                }
                body = body.Replace("{#SENDERID#}", senderid);

                //Dear Team, {#USERID#} has added Sender ID - {#SENDERID#} for approval.     Please Approve
                Email(to, cc, subject, body);

            }
        }
    }
}