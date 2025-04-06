using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Web.Hosting;
using System.ComponentModel;
using Shortnr.Web.Business;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using eMIMPanel.Helper;
using System.Configuration;

namespace eMIMPanel
{
    public partial class Users_Update_Setting : System.Web.UI.Page
    {
        string _user = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            _user = Convert.ToString(Session["UserID"]);
            if (_user == "")
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                Util ob = new Util();
                PopulateSender();
                PopulateTemplateID();
                BindData();
            }
        }

        public void PopulateSender()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["UserID"]), Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            if (dt.Rows.Count == 0)
            {
                ddlSender.Items.Clear();
                return;
            }
            ddlSender.DataSource = dt;
            ddlSender.DataTextField = "senderid";
            ddlSender.DataValueField = "senderid";
            ddlSender.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlSender.Items.Insert(0, objListItem);
        }

        public void PopulateTemplateID()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTemplateId(Convert.ToString(Session["UserID"]));

            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "TemplateID";
            ddlTempID.DataValueField = "Template";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
            ddlTempID.SelectedIndex = 0;
        }

        public void BindData()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetDataUserSetting(Convert.ToString(Session["UserID"]));
            if (dt.Rows.Count > 0)
            {
                _user = Convert.ToString(Session["UserID"]);
                string EnableOTP = dt.Rows[0]["OTP_VERIFICATION_REQD"].ToString();
                string EnableEmailSend = dt.Rows[0]["ReportonEmail"].ToString();
                if (EnableOTP.ToUpper() == "FALSE")
                {
                    divSenderId.Visible = false;
                    divTempId.Visible = false;
                    divTempsms.Visible = false;
                    divPreview.Visible = false;
                    divlogintype.Visible = false;
                }
                else
                {
                    chkEnableOTP.Checked = true;
                    string CountryCode = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                    string otptype = dt.Rows[0]["Login_OTP_SMSWABA"].ToString();
                    if (otptype == "S")
                    {
                        rdbOtpSMS.Checked = true;
                        rdbOtpWABA.Checked = false;
                    }
                    else
                    {
                        rdbOtpWABA.Checked = true;
                        rdbOtpSMS.Checked = false;
                    }
                    divlogintype.Visible = true;
                    if (rdbOtpSMS.Checked)
                    {
                        if (CountryCode == "91")
                        {
                            divSenderId.Visible = true;
                            divTempId.Visible = true;
                            divTempsms.Visible = true;
                            ddlTempID.SelectedValue = dt.Rows[0]["Login_OTP_Template_ID"].ToString();
                            DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["UserID"]), ddlTempID.SelectedValue);

                            if (ddlTempID.SelectedValue == "0")
                            {
                                lblTempSMS.Text = "";
                                divPreview.Visible = true;
                                string TempSMS = lblTempSMS.Text.ToString();
                                TempSMS = TempSMS.ToString().Replace("{#var#}", "123456");
                                TempSMS = TempSMS.ToString().Replace("#var1", "123456");
                                txtpreview.Text = TempSMS;
                            }
                            else
                            {
                                lblTempSMS.Text = dtT.Rows[0]["template"].ToString();
                                divPreview.Visible = true;
                                string TempSMS = lblTempSMS.Text.ToString();
                                TempSMS = TempSMS.ToString().Replace("{#var#}", "123456");
                                TempSMS = TempSMS.ToString().Replace("#var1", "123456");
                                txtpreview.Text = TempSMS;
                            }
                        }
                        else
                        {
                            divSenderId.Visible = false;
                            divTempId.Visible = false;
                            divTempsms.Visible = true;
                            divPreview.Visible = true;
                            string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                            lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                            msg = msg.ToString().Replace("{#var#}", "123456");
                            txtpreview.Text = msg;
                        }
                        ddlSender.SelectedValue = dt.Rows[0]["Login_OTP_Sender_ID"].ToString();
                    }
                    else
                    {
                        divSenderId.Visible = false;
                        divTempId.Visible = false;
                        divTempsms.Visible = true;
                        divPreview.Visible = true;
                        string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        msg = msg.ToString().Replace("{#var#}", "123456");
                        txtpreview.Text = msg;
                    }
                }
                if (EnableEmailSend.ToUpper() == "FALSE")
                {
                    chkEnableSendEmail.Checked = false;
                    txtEmailTo.Text = dt.Rows[0]["Email"].ToString();
                    txtEmailCC.Text = dt.Rows[0]["CCEmail"].ToString();
                }
                else
                {
                    chkEnableSendEmail.Checked = true;
                    txtEmailTo.Text = dt.Rows[0]["Email"].ToString();
                    txtEmailCC.Text = dt.Rows[0]["CCEmail"].ToString();
                }
            }
        }

        protected void chkEnableOTP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableOTP.Checked)
            {
                divlogintype.Visible = true;
                string CountryCode = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                if (rdbOtpSMS.Checked)
                {
                    if (CountryCode == "91")
                    {
                        divSenderId.Visible = true;
                        divTempId.Visible = true;
                        divTempsms.Visible = true;
                        divPreview.Visible = false;
                    }
                    else
                    {
                        divSenderId.Visible = true;
                        divTempId.Visible = false;
                        divTempsms.Visible = true;
                        divPreview.Visible = true;
                        string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        msg = msg.ToString().Replace("{#var#}", "123456");
                        txtpreview.Text = msg;
                    }
                }
                else
                {
                    if (CountryCode == "91")
                    {
                        divSenderId.Visible = true;
                        divTempId.Visible = true;
                        divTempsms.Visible = true;
                        divPreview.Visible = false;
                    }
                    else
                    {
                        divSenderId.Visible = true;
                        divTempId.Visible = false;
                        divTempsms.Visible = true;
                        divPreview.Visible = true;
                        string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                        msg = msg.ToString().Replace("{#var#}", "123456");
                        txtpreview.Text = msg;
                    }
                }
            }
            else
            {
                divlogintype.Visible = false;
                divSenderId.Visible = false;
                divTempId.Visible = false;
                divTempsms.Visible = false;
                divPreview.Visible = false;
            }
        }

        protected void rdbOtpSMS_CheckedChanged(object sender, EventArgs e)
        {
            string CountryCode = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
            if (rdbOtpSMS.Checked)
            {
                if (CountryCode == "91")
                {
                    divSenderId.Visible = true;
                    divTempId.Visible = true;
                    divTempsms.Visible = true;
                    divPreview.Visible = false;
                }
                else
                {
                    divSenderId.Visible = true;
                    divTempId.Visible = false;
                    divTempsms.Visible = true;
                    divPreview.Visible = true;
                    string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                    lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                    msg = msg.ToString().Replace("{#var#}", "123456");
                    txtpreview.Text = msg;
                }
            }
        }

        protected void rdbOtpWABA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOtpWABA.Checked)
            {
                divSenderId.Visible = false;
                divTempId.Visible = false;
                divTempsms.Visible = true;
                divPreview.Visible = true;
                string msg = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                lblTempSMS.Text = ConfigurationManager.AppSettings["SENT_OTP_SMS_TEXT"].ToString();
                msg = msg.ToString().Replace("{#var#}", "123456");
                txtpreview.Text = msg;
            }
        }

        protected void ddlTempID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["UserID"]), ddlTempID.SelectedValue);

            if (ddlTempID.SelectedValue == "0")
            {
                lblTempSMS.Text = "";
                divPreview.Visible = true;
                string TempSMS = lblTempSMS.Text.ToString();
                TempSMS = TempSMS.ToString().Replace("{#var#}", "123456");
                TempSMS = TempSMS.ToString().Replace("#var1", "123456");
                txtpreview.Text = TempSMS;
            }
            else
            {
                lblTempSMS.Text = dtT.Rows[0]["template"].ToString();
                divPreview.Visible = true;
                string TempSMS = lblTempSMS.Text.ToString();
                TempSMS = TempSMS.ToString().Replace("{#var#}", "123456");
                TempSMS = TempSMS.ToString().Replace("#var1", "123456");
                txtpreview.Text = TempSMS;
            }
        }

        protected void lnkUpdateDate_Click(object sender, EventArgs e)
        {
            _user = Convert.ToString(Session["UserID"]);
            string SenderID = ddlSender.SelectedValue.ToString();
            string TempId = ddlTempID.SelectedValue.ToString();
            string CountryCode = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
            string OtpEnable = string.Empty;
            string OtpType = "0";
            if(chkEnableOTP.Checked)
            {
                OtpEnable = "1";
            }
            if(rdbOtpSMS.Checked)
            {
                OtpType = "S";
                if (SenderID == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Sender ID.');", true);
                    return;
                }
                if (CountryCode == "91")
                {
                    if (TempId == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Template ID.');", true);
                        return;
                    }
                }
                else
                {
                    TempId = "";
                }
            }
            else
            {
                OtpType = "W";
                SenderID = "";
                TempId = "";
            }
            database.ExecuteNonQuery("UPDATE customer SET OTP_VERIFICATION_REQD = '" + OtpEnable + "',Login_OTP_Template_ID = '" + TempId + "',Login_OTP_Sender_ID = '" + SenderID + "',Login_OTP_SMSWABA = '" + OtpType + "' WHERE username='" + _user + "'");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Update Successfully.');", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Users_Update_Setting.aspx");
        }

        protected void lnkbtnEmailUpdate_Click(object sender, EventArgs e)
        {
            if(chkEnableSendEmail.Checked)
            {
                database.ExecuteNonQuery("UPDATE customer SET ReportonEmail = '1' WHERE username='" + _user + "'");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Update Successfully.');", true);
            }
            else
            {
                database.ExecuteNonQuery("UPDATE customer SET ReportonEmail = '0' WHERE username='" + _user + "'");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Update Successfully.');", true);
            }
        }
    }
}