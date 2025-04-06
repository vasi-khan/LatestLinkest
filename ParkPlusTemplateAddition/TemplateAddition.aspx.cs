using ParkPlusTemplateAddition.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestSharp;
using System.Globalization;

namespace ParkPlusTemplateAddition
{
    public partial class TemplateAddition : System.Web.UI.Page
    {
        Util ob = new Util();
        string user;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("Login.aspx");
            lblAcc.Text = user;
            if (!IsPostBack)
            {
                rblType.SelectedValue = "API";
                DataTable dt = ob.GetTemplteAPI(user);
                grvAPI.DataSource = dt;
                grvAPI.DataBind();
                GridFormat(dt);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtTempId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Id Can not be Empty !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                return;
            }
            if (txtTempName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Name Can not be Empty !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                return;
            }
            if (txtTemplateContent.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Text Can not be Empty !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                return;
            }
            List<string> LstUserId = new List<string>();
            List<string> LstSenderId = new List<string>();
            for (int i = 1; i < 7; i++)
            {
                string txtuserid = "txtuserid" + i;
                string txtSenderId = "txtSenderId" + i;
                TextBox txtuseridID = (TextBox)divuser.FindControl(txtuserid);
                TextBox txtSenderID = (TextBox)divSender.FindControl(txtSenderId);
                if (txtuseridID.Text.Trim() != "")
                {
                    DataTable dt = ob.GetUserInfo(txtuseridID.Text.ToString());
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        LstUserId.Add(txtuseridID.Text.ToString());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('UserId Does Not exists " + txtuseridID.Text.ToString() + " !')", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                        return;
                    }
                }
                if (txtSenderID.Text.Trim() != "")
                {
                    LstSenderId.Add(txtSenderID.Text.ToString());
                }
            }
            if (LstUserId.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('user Id Can not be Empty !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                return;
            }
            if (LstSenderId.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender Id Can not be Empty !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
                return;
            }
            string TempCntnt = txtTemplateContent.Text;
            string TempContent = "";
            if (txtTemplateContent.Text.ToUpper().Contains("{#VAR#}"))
            {
                TempContent = TempCntnt.ToUpper().Replace("{#VAR#}", ";");
            }
            else
            {
                TempContent = txtTemplateContent.Text.Trim();
            }
            if (txtTemplateContent.Text.ToUpper().Contains(" ; "))
            {
                TempContent = TempContent.Replace(" ; ", ";");
            }
            string res = "", res1 = "";

            for (int i = 0; i < LstSenderId.Count; i++)
            {
                DataTable dt = ob.ValidateTemplateIdforAPI(txtTempId.Text.Trim(), txtTempName.Text, LstSenderId[i].ToString());
                if (Convert.ToInt32(dt.Rows[0]["TemplateId"]) == 0)
                {
                    ob.InsertTempAPI(txtTempId.Text.Trim(), txtTempName.Text.Trim(), txtTemplateContent.Text.Trim(), LstSenderId[i].ToString(), TempContent, user, Convert.ToString(Session["COUNTRYCODE"]));
                    res = "API";
                }
            }

            for (int i = 0; i < LstUserId.Count; i++)
            {
                DataSet ds = ob.ValidateTemplateRequest(txtTempId.Text.Trim(), txtTempName.Text.Trim(), LstUserId[i].ToString());
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["TemplateId"]) == 0)
                {
                    ob.InsertTempPanel(txtTempId.Text.Trim(), txtTempName.Text.Trim(), txtTemplateContent.Text.Trim(), "", LstUserId[i].ToString());
                    res1 = " PANEL";
                }
            }
            if (res == "" && res1 == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Already Exist !')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "MyFunction()", true);
            }
            else
            {
                string n = res + " " + res1 + " " + "!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Created Successfully for " + n + "');window.location='TemplateAddition.aspx';", true);
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("TemplateAddition.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void rbAPI_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            LinkButton lnksms = (LinkButton)sender;
            GridViewRow grvrow = (GridViewRow)lnksms.NamingContainer;
            HiddenField TemplateId = (HiddenField)grvrow.FindControl("hfTemplateId");
            HiddenField TemplateName = (HiddenField)grvrow.FindControl("hftemplatename");

            if (TemplateId.Value != "" && TemplateName.Value != "")
            {
                if (rblType.SelectedValue == "API")
                {
                    DataTable dt = ob.GetTemplteAPI(user, TemplateId.Value);
                    DataTable dt1 = database.GetDataTable(@"select distinct t.senderid from TemplateID t with(nolock) 
                                    inner join senderidmast s with(nolock) on  t.senderid = s.senderid where userid = '" + user + "' " +
                                    "and TemplateID = '" + TemplateId.Value + "'");
                    BindSenderId(dt1);
                    lbltempid.Text = Convert.ToString(dt.Rows[0]["templateID"]);
                    lbltempName.Text = Convert.ToString(dt.Rows[0]["templateName"]);
                    lbltempText.Text = Convert.ToString(dt.Rows[0]["msgtext"]);
                    txtUserId.Text = user;
                    txtUserId.ReadOnly = true;
                    lblHeader.Text = "API";
                }
                else
                {
                    DataTable dt = ob.GetSender(user);
                    BindSenderId(dt);
                    DataTable dt1 = ob.GetTemplatePanel(user, TemplateId.Value, TemplateName.Value);
                    lbltempid.Text = Convert.ToString(dt1.Rows[0]["templateid"]);
                    lbltempName.Text = Convert.ToString(dt1.Rows[0]["templateName"]);
                    lbltempText.Text = Convert.ToString(dt1.Rows[0]["msgtext"]);
                    lblHeader.Text = "PANEL";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Name or TemplateId Can not be Empty !');window.location='TemplateAddition.aspx';", true);
                return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);

        }

        private void BindSenderId(DataTable dt)
        {
            ddlSenderId.DataSource = dt;
            ddlSenderId.DataTextField = "senderid";
            ddlSenderId.DataValueField = "senderid";
            ddlSenderId.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlSenderId.Items.Insert(0, objListItem);
        }

        protected void GridFormat(DataTable dt)
        {
            grvAPI.UseAccessibleHeader = true;
            grvAPI.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grvAPI.TopPagerRow != null)
            {
                grvAPI.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvAPI.BottomPagerRow != null)
            {
                grvAPI.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grvAPI.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Username !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
            if (txtMobNo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Mob No !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
            if (txtMobNo.Text.Length < 10 || txtMobNo.Text.Length > 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mob No Must be 10 digit !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
            if (ddlSenderId.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Sender ID !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
            DataTable dt = ob.GetUserInfo(txtUserId.Text.Trim());
            if (dt.Rows.Count > 0 && dt != null)
            {
                string res = ob.Insert_MSGTRAN_91M(txtUserId.Text.Trim(), lbltempText.Text.Trim(), txtMobNo.Text, ddlSenderId.SelectedItem.Text, Convert.ToString(dt.Rows[0]["peid"]), lbltempid.Text.Trim());
                if (res != "")
                {
                    res = lblHeader.Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SMS Sent Successfully for "+ res + " !');window.location='TemplateAddition.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Something Went Wrong !');window.location='TemplateAddition.aspx';", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid UserID !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "Confirm1()", true);
                return;
            }
        }



        public void SMSAPI(DataTable dt)
        {
            string password = Convert.ToString(dt.Rows[0]["APIKEY"]);

            string message = lbltempText.Text;
            message = message.Replace("&", "%26").Replace("+", "%2B");

            string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + txtUserId.Text + "&pwd=" + password + "&mobile=" + txtMobNo.Text + "&sender=" + ddlSenderId.SelectedItem.Text + "&msg=" + message + "&msgtype=13";
            string getResponseTxt = "";
            string getStatus = "";
            WinHttp.WinHttpRequest objWinRq;
            objWinRq = new WinHttp.WinHttpRequest();
            try
            {
                objWinRq.Open("GET", url, false);
                objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                objWinRq.Send(null);

                while (!(getStatus != "" && getResponseTxt != ""))
                {
                    getStatus = objWinRq.Status + objWinRq.StatusText;
                    getResponseTxt = objWinRq.ResponseText;
                }
                getResponseTxt = "[" + getResponseTxt + "]";
            }
            catch (Exception EX)
            {
                throw EX;
            }

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            System.Threading.Thread.Sleep(2000);
            int submitted = ob.GetCountForSubmittedTemplate(txtUserId.Text, ddlSenderId.SelectedItem.Text, currentTime);
            int msgLength = message.Length;
            //lblTotalMessage.Text = "Total Message : " + submitted.ToString();
            //lblMessageLength.Text = "Message Length :" + msgLength.ToString() + " character";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("TemplateAddition.aspx");
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "API")
            {

                DataTable dt = ob.GetTemplteAPI(user);
                grvAPI.DataSource = dt;
                grvAPI.DataBind();
                GridFormat(dt);
            }
            else
            {
                DataTable dt = ob.GetTemplatePanel(user);
                grvAPI.DataSource = dt;
                grvAPI.DataBind();
                GridFormat(dt);
            }
        }

    }
}