using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class sms_reports_sysadmin_DLR_new : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            string empcode = Convert.ToString(Session["EMPCODE"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                DataTable dt = null;
                grv.DataSource = dt;
                grv.DataBind();
                //GridFormat(dt);
                PopulateSender();
                PopulateCampaign();
                BindTemplate();
            }
        }
        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            //if (grv.TopPagerRow != null)
            //{
            //    grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            //}
            //if (grv.BottomPagerRow != null)
            //{
            //    grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            //}
            //if (dt.Rows.Count > 0)
            //    grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        public void BindTemplate()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTempIdAndName(Convert.ToString(Session["User"]));
            if (dt.Rows.Count == 0)
            {
                DataTable dt1 = ob.GetTempIdAndNamenull();
                ddltemplate.DataSource = dt1;
                ddltemplate.DataTextField = "tempname";
                ddltemplate.DataValueField = "templateid";
                ddltemplate.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddltemplate.Items.Insert(0, objListItem);
            }
            else
            {
                ddltemplate.DataSource = dt;
                ddltemplate.DataTextField = "tempname";
                ddltemplate.DataValueField = "templateid";
                ddltemplate.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddltemplate.Items.Insert(0, objListItem);
            }
        }
        public void PopulateCampaign()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetCampaignAll(Convert.ToString(Session["User"]));
            if (dt.Rows.Count == 0)
            {
                DataTable dt1 = ob.GetCampaignAllNull();
                ddlCamp.DataSource = dt1;
                ddlCamp.DataTextField = "campaignname";
                ddlCamp.DataValueField = "campaignname";
                ddlCamp.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddlCamp.Items.Insert(0, objListItem);
                if (dt.Rows.Count == 1)
                    ddlCamp.SelectedIndex = 1;
                else
                    ddlCamp.SelectedIndex = 0;
            }
            else
            {
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
            }

        }
        public void PopulateSender()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["User"]));
            if (dt.Rows.Count == 0)
            {
                DataTable dt1 = ob.GetSenderIduservalnull();
                ddlSender.DataSource = dt1;
                ddlSender.DataTextField = "senderid";
                ddlSender.DataValueField = "senderid";
                ddlSender.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddlSender.Items.Insert(0, objListItem);
                if (dt.Rows.Count == 1)
                    ddlSender.SelectedIndex = 1;
                else
                    ddlSender.SelectedIndex = 0;
            }
            else
            {
                ddlSender.DataSource = dt;
                ddlSender.DataTextField = "senderid";
                ddlSender.DataValueField = "senderid";
                ddlSender.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddlSender.Items.Insert(0, objListItem);
                if (dt.Rows.Count == 1)
                    ddlSender.SelectedIndex = 1;
                else
                    ddlSender.SelectedIndex = 0;
            }

        }
        protected void rbTdy_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-none");
            DataTable dt = null;
            grv.DataSource = dt;
            grv.DataBind();
        }

        protected void rbHis_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-block");
            DataTable dt = null;
            grv.DataSource = dt;
            grv.DataBind();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbTdy.Checked)
                {
                    Session["Today"] = "True";
                }
                if(rdblistselect.SelectedValue=="D")
                {
                    Session["DATEWISE"] = "DATEWISE";
                }
                else
                {
                    Session["DATEWISE"] = "SINGLE";
                }
                string s1 = "";
                string s2 = "";
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label l = (Label)gvro.FindControl("lblUserId");
                Label l2 = (Label)gvro.FindControl("lblsender");

                if (rbTdy.Checked == true)
                {
                    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                }
                else
                {
                    s1 = Convert.ToDateTime(txtFrm1.Text.Trim(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    s2 = Convert.ToDateTime(txtTo1.Text.Trim(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
                }
                if (Convert.ToDateTime(s1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                    return;
                }
                if (Convert.ToDateTime(s2) < Convert.ToDateTime(s1))
                {
                    hdntxtFrm1.Value = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date Can not be greater than To date !');", true);
                    return;
                }
                if (rbSbmtd.Checked == true)
                {
                    ViewState["ReportType"] = "";
                }
                else if (rbDlvr.Checked == true)
                {
                    ViewState["ReportType"] = "Delivered";
                }
                else if (rbFailed.Checked == true)
                {
                    ViewState["ReportType"] = "Rejected";
                }
                string ReportType = Convert.ToString(ViewState["ReportType"]);
                string Uid = txtUserWise.Text.Trim();
                if (Uid == "")
                {
                    Uid = l.Text;
                }
                string Sid = ddlSender.SelectedValue;
                string templateid = ddltemplate.SelectedValue;
                string CampaingId = ddlCamp.SelectedValue;
                string Mobno = txtmob.Text.Trim();
                Helper.Util ob = new Helper.Util();
                string concat = l.Text + "$" + l2.Text + "$" + s1 + "$" + s2 + "$" + Uid + "$" + Sid + "$" + ReportType + "$" + templateid + "$" + CampaingId + "$" + Mobno + "$" + Convert.ToString(Session["UserType"]) + "$" + Convert.ToString(Session["DLT"]);
                Session["concat"] = concat;
                Response.Redirect("sms-reports_download.aspx");
                //Response.Redirect("sms-reports_download.aspx?x=" );
                //Session["DATEWISE"] = "DATEWISE";
                //Response.Redirect("sms-reports_download.aspx");
                //string un = h.Value;
                //ViewState["UN"] = un;
                //txtusername.Text = l.Text;
                //txtpwd.Text = h.Value;
                //modalpopuppwd.Show();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.Util ob = new Helper.Util();
                if (rbTdy.Checked)
                {
                    string d1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string d2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
                    
                    if (rbSbmtd.Checked == true)
                    {
                        ViewState["ReportType"] = "";
                    }
                    else if (rbDlvr.Checked == true)
                    {
                        ViewState["ReportType"] = "Delivered";
                    }
                    else if (rbFailed.Checked == true)
                    {
                        ViewState["ReportType"] = "Rejected";
                    }
                    DataTable dt = ob.GetDELIVERYREPORTTODAYSYSADMIN(Convert.ToString(Session["UserType"]), Convert.ToString(Session["User"]), Convert.ToString(Session["EMPCODE"]), d1, d2, Convert.ToString(ViewState["ReportType"]));
                    grv.DataSource = dt;
                    grv.DataBind();
                    GridFormat(dt);
                }
                else
                {
                    if (txtFrm1.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select From Date');", true);
                        return;
                    }
                    if (txtTo1.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select To Date');", true);
                        return;
                    }
                    string d1 = txtFrm1.Text.ToString();
                    string d2 = txtTo1.Text.ToString();
                    if (Convert.ToDateTime(d1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                        return;
                    }
                    if (Convert.ToDateTime(d2) < Convert.ToDateTime(d1))
                    {
                        hdntxtFrm1.Value = "";
                        txtFrm1.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date Can not be greater than To date !');", true);
                        return;
                    }
                    string UserId = txtUserWise.Text.ToString();
                    string SenderId = ddlSender.SelectedValue;
                    string CampaignWise = ddlCamp.SelectedValue;
                    string TempId = Convert.ToString(ddltemplate.SelectedValue);
                    if (rbSbmtd.Checked == true)
                    {
                        ViewState["ReportType"] = "";
                    }
                    else if (rbDlvr.Checked == true)
                    {
                        ViewState["ReportType"] = "Delivered";
                    }
                    else if (rbFailed.Checked == true)
                    {
                        ViewState["ReportType"] = "Rejected";
                    }

                    if (rdblistselect.SelectedValue == "D")
                    {
                        Session["DATEWISE"] = "DATEWISE";
                        DataTable dt = ob.GetDELIVERYREPORTTODAYSYSADMINFILTER(d1, d2, SenderId, CampaignWise, TempId, UserId, "NOTCAMPIAGNWISE", Convert.ToString(ViewState["ReportType"]), Convert.ToString(Session["UserType"]), Convert.ToString(Session["EMPCODE"]), txtmob.Text.Trim(), Convert.ToString(Session["DLT"]));
                        grv.DataSource = dt;
                        grv.DataBind();
                        GridFormat(dt);
                    }
                    else
                    {

                        DataTable dt = ob.GetDELIVERYREPORTTODAYSYSADMINSingle(d1, d2, SenderId, CampaignWise, TempId, UserId, "NOTCAMPIAGNWISE", Convert.ToString(ViewState["ReportType"]), Convert.ToString(Session["UserType"]), Convert.ToString(Session["EMPCODE"]), txtmob.Text.Trim(), Convert.ToString(Session["DLT"]));
                        grv.DataSource = dt;
                        grv.DataBind();
                        GridFormat(dt);
                    }
                    //txtFrm1.Text = "";
                    //txtTo1.Text = "";
                    hdntxtFrm1.Value = "";
                    hdntxtTo1.Value = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}