using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace eMIMPanel
{
    public partial class AddTemplateAdminNew : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (usertype != "SYSADMIN") Response.Redirect("index2.aspx");
        }
        private bool ValidateTemplate()
        {
            if (txtUser.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter User Name');", true);
                return false;
            }
            if (txtSenderId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter SenderId');", true);
                return false;
            }

            if (txtTempId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template ID');", true);
                return false;
            }

            if (txtTempName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Name');", true);
                return false;
            }

            if (txtTemplateContent.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Content Text');", true);
                return false;
            }

            DataTable dt = ob.ValidateTemplateIdforAPI(txtTempId.Text.Trim(), txtTempName.Text, txtSenderId.Text.Trim());
            if (Convert.ToInt16(dt.Rows[0]["TemplateId"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template ID already exist for API');", true);
                return false;
            }

            DataSet ds = ob.ValidateTemplateRequest(txtTempId.Text.Trim(), txtTempName.Text.Trim(), txtUser.Text.Trim());

            if (Convert.ToInt16(ds.Tables[0].Rows[0]["TemplateId"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template ID already exist for Panel');", true);
                return false;
            }
            if (Convert.ToInt16(ds.Tables[1].Rows[0]["Templatename"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Name already exist for Panel');", true);
                return false;
            }

            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateTemplate())
            {
                string senderId = txtSenderId.Text.Trim();
                string tempMsg = txtTemplateContent.Text;
                string tempName = txtTempName.Text;
                string tempId = txtTempId.Text.Trim();

                string tempWord = txtTemplateContent.Text.Replace("{#var#}", ";").Replace(" ;", ";").Replace("; ", ";").Replace(";;", ";").Replace(Environment.NewLine, ";").Replace(";;", ";").TrimEnd(';');

                ob.SaveTemplateInTemplateId(senderId, tempMsg, tempWord, tempName, tempId);

                string _user = txtUser.Text.Trim();
                string msg = txtTemplateContent.Text.Trim();
                string fileName = "TemplateRequest.txt";
                ob.SaveTemplateRequest(_user, msg, fileName, tempName, tempId, true);
                ClearControl();

                string respmsg = "Template Request generated successfully. for API & Panel";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + respmsg + "')", true);

                BindData();
            }

        }

        private void ClearControl()
        {
            txtTemplateContent.Text = "";
            txtTempName.Text = "";
            txtTempId.Text = "";

        }

        private void BindData()
        {
            grvTemplate.DataSource = null;
            if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                DataTable dt = ob.GetTemplateList(txtUser.Text);
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
            else if (!string.IsNullOrEmpty(txtSenderId.Text.Trim()))
            {
                DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }

        }

        protected void lnkShow_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;

            GridViewRow gvro = (GridViewRow)btn.NamingContainer;

            Label lblTemplateId = (Label)gvro.FindControl("lblTemplateId");

            if (!string.IsNullOrEmpty(txtSenderId.Text.Trim()))
            {
                string senderId = txtSenderId.Text.Trim();
                ob.DeleteTemplateInTemplateId(senderId, lblTemplateId.Text);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Request deleted successfully. for API');", true);

                grvTemplate.DataSource = ob.GetTemplateListOfAPI(senderId);
                grvTemplate.DataBind();
            }
            else if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                string userName = txtUser.Text.Trim();
                ob.DeleteTemplateInRequest(userName, lblTemplateId.Text);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Request deleted successfully.');", true);
                grvTemplate.DataSource = ob.GetTemplateList(userName);
                grvTemplate.DataBind();
            }
        }

        private void BindSenderId(string userId)
        {
            DataTable dt = ob.GetSenderId(userId);
            //ddlLinkrSender.Items.Clear();
            ddlLinkrSender.DataSource = dt;
            ddlLinkrSender.DataTextField = "senderid";
            ddlLinkrSender.DataValueField = "senderid";
            ddlLinkrSender.DataBind();
            //ListItem objListItem = new ListItem("--Select--", "0");
            //ddlLinkrSender.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlLinkrSender.SelectedIndex = 1;
            //else
            //    ddlLinkrSender.SelectedIndex = 0;
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSenderId.Text.Trim()))
            {
                if (txtTestUser.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Username');", true);
                    return;
                }
                DataTable dt = ob.GetUserParameter(txtTestUser.Text);
                string password = Convert.ToString(dt.Rows[0]["APIKEY"]);

                string message = txtTestMessage.Text;
                message = message.Replace("&", "%26").Replace("+", "%2B");

                string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + txtTestUser.Text + "&pwd=" + password + "&mobile=" + txtTestMobile.Text + "&sender=" + txtTestSender.Text + "&msg=" + message + "&msgtype=13";
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
                int submitted = ob.GetCountForSubmittedTemplate(txtTestUser.Text, txtSenderId.Text.Trim(), currentTime);
                int msgLength = message.Length;

                lblTotalMessage.Text = "Total Message : " + submitted.ToString();
                lblMessageLength.Text = "Message Length :" + msgLength.ToString() + " character";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Send successfully. for API');", true);
                pnlPopUp_NUMBER_ModalPopupExtender.Show();
            }
        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            GridViewRow gvro = (GridViewRow)btn.NamingContainer;

            TextBox txtTemplate = (TextBox)gvro.FindControl("txtTemplate");

            lblTotalMessage.Text = "";
            lblMessageLength.Text = "";

            if (!string.IsNullOrEmpty(txtSenderId.Text.Trim()))
            {
                txtTestMessage.Text = txtTemplate.Text;
                txtTestSender.Text = Convert.ToString(ViewState["SenderId"]);
                pnlPopUp_NUMBER_ModalPopupExtender.Show();
            }
            else if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                Label lblTemplateId = (Label)gvro.FindControl("lblTemplateId");
                ViewState["TemplateId"] = lblTemplateId.Text;
                txtLinkMessage.Text = txtTemplate.Text;
                string userName = txtUser.Text.Trim();
                txtLinkUser.Text = userName;
                BindSenderId(userName);
                pnlPopUp_Linkext_ModalPopupExtender.Show();
            }

        }

        protected void btnLinkSend_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text.Trim();
            DataTable dt = ob.GetUserParameter(userName);
            string smppAcountId = ob.GetTemplateTestAccounts();
            string profileId = userName;
            string msg = txtLinkMessage.Text;
            string mobile = txtLinkMobile.Text;
            string senderId = ddlLinkrSender.SelectedValue;
            string fileId = "108";
            string peid = Convert.ToString(dt.Rows[0]["peid"]);

            string templId = Convert.ToString(ViewState["TemplateId"]);

            string dataCode = "Default";
            string q = msg;
            if (q.Any(c => c > 126))
            {
                dataCode = "UCS2";
            }

            ob.InsertMsgTomsgtranForTemplateTest(smppAcountId, profileId, msg, mobile, senderId, fileId, peid, templId, dataCode);

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSenderId.Text.Trim()))
            {
                if (txtSenderId.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Sender Id');", true);
                    return;
                }
                DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                DatatableToExcel(dt);
            }
            else if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                if (txtUser.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter User Name');", true);
                    return;
                }

                DataTable dt = ob.GetTemplateList(txtUser.Text);
                DatatableToExcel(dt);
            }
        }

        private void ExportToExcel()
        {
            string attachment = "attachment; filename=TemplateReport.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grvTemplate.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void DatatableToExcel(DataTable dt)
        {

            string attachment = "attachment; filename=TemplateReport.xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string tab = "";

            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    string leadingChar = i == 0 ? "'" : "";
                    HttpContext.Current.Response.Write(tab + leadingChar + dr[i].ToString().Replace('\n', ' ').Replace(Convert.ToChar(10), ' ').Replace(Convert.ToChar(13), ' '));
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            // Response.End();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void GridFormat(DataTable dt)
        {
            grvTemplate.UseAccessibleHeader = true;
            grvTemplate.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grvTemplate.TopPagerRow != null)
            {
                grvTemplate.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvTemplate.BottomPagerRow != null)
            {
                grvTemplate.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grvTemplate.FooterRow.TableSection = TableRowSection.TableFooter;
        }


    }
}