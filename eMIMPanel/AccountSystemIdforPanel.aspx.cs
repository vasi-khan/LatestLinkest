using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class AccountSystemIdforPanel : System.Web.UI.Page
    {
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (rbTran.Checked == true)
            {
                BindApiTranGrid();
            }
            else if (rbPromo.Checked == true)
            {
                BindApipromoGrid();
            }
            if (rbPanel.Checked == true)
            {
                BindPanelGrid();
            }
            if (!IsPostBack)
            {
                rbAPI.Checked = true;
                rbTran.Checked = true;
                ddlAPIProviderbind();
                ddlManualACIDbind();
                ddlPanelProviderbind();
                BindApiTranGrid();
            }
        }

        public void BindApiTranGrid()
        {
            divpanelgrd.Visible = false;
            divgrdpromo.Visible = false;
            divgrdtran.Visible = true;
            DataTable dt = obj.GetAPITran();
            grdAPITran.DataSource = dt;
            grdAPITran.DataBind();
            GridFormat2(dt);
        }
        public void BindApipromoGrid()
        {
            divpanelgrd.Visible = false;
            divgrdtran.Visible = false;
            divgrdpromo.Visible = true;
            DataTable dt = obj.GetAPIPromo();
            GridAPIPROMO.DataSource = dt;
            GridAPIPROMO.DataBind();
            GridFormat1(dt);
        }
        public void BindPanelGrid()
        {
            divgrdtran.Visible = false;
            divgrdpromo.Visible = false;
            divpanelgrd.Visible = true;
            DataTable dt = obj.GetPaneldata();
            grdPanel.DataSource = dt;
            grdPanel.DataBind();
            GridFormat(dt);
        }

        protected void rbAPI_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) return;
            divAPI.Visible = rb.ID == rbAPI.ID;
            divPanel.Visible = rb.ID == rbPanel.ID;
        }

        public void ddlAPIProviderbind()
        {
            DataTable dt = obj.GetProvider("API");
            ddlProviderAPI.DataSource = dt;
            ddlProviderAPI.DataTextField = "PROVIDER";
            ddlProviderAPI.DataValueField = "PROVIDER";
            ddlProviderAPI.DataBind();
            ListItem objlst = new ListItem("--Select--", "0");
            ddlProviderAPI.Items.Insert(0, objlst);
        }

        public void ddlManualACIDbind()
        {

            DataTable dt = obj.GetProvider("Provider");
            ddlManualAcid.DataSource = dt;
            ddlManualAcid.DataTextField = "Provider";
            ddlManualAcid.DataValueField = "smppaccountid";
            ddlManualAcid.DataBind();
            ListItem objlst = new ListItem("--Select--", "0");
            ddlManualAcid.Items.Insert(0, objlst);
        }
        public void ddlPanelProviderbind()
        {
            DataTable dt = obj.GetProvider("Provider");
            ddlProviderPanel.DataSource = dt;
            ddlProviderPanel.DataTextField = "Provider";
            ddlProviderPanel.DataValueField = "smppaccountid";
            ddlProviderPanel.DataBind();
            ListItem objlst = new ListItem("--Select--", "0");
            ddlProviderPanel.Items.Insert(0, objlst);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            mp1.Show();
            //if (ViewState["PWDCHECK"] == null || ViewState["PWDCHECK"].ToString() == "false")
            //{
            //    mp1.Show();
            //    return;
            //}

        }

        protected void btnSuccess_Click(object sender, EventArgs e)
        {
            Util obj = new Util();
            string check = obj.PWDCHECK(user, txtPWD.Text.Trim());
            if (check == "")
            {
                lblPWDCheck.Text = "Incorrect Password !!";
                ViewState["PWDCHECK"] = "false";
            }
            else
            {
                mp1.Hide();
                //lblPWDCheck.Text = "Password Verified Successfully you can Now Close the Window.";
                ViewState["PWDCHECK"] = "true";
                if (rbAPI.Checked == true)
                {
                    uservalidcheckAPI();
                    if (txtUserIdAPI.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User Id Can not be Empty !!');", true);
                        txtUserIdAPI.Focus();
                        return;
                    }
                    if (ddlProviderAPI.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Provider !!');", true);
                        ddlProviderAPI.Focus();
                        return;
                    }
                    string chkmpdoperator = "";
                    if (rbTran.Checked == true)
                    {
                        chkmpdoperator = CheckMappedOperatorTran();
                    }
                    else
                    {
                        chkmpdoperator = CheckMappedOperatorPromo();
                    }
                    if (chkmpdoperator != "")
                    {
                        string OPEARTOR = "";
                        if (rbTran.Checked == true)
                        {
                            string sql = "select ACCOUNT from UAEAPIAccounts ua inner join OperatorSenderId os on ua.ACCOUNT=os.Provider where ua.Active=1 and userid='" + txtUserIdAPI.Text + "'";
                            OPEARTOR = Convert.ToString(database.GetScalarValue(sql));
                        }
                        else
                        {
                            string sql = "select ACCOUNT from UAEAPIACCOUNTPROMO up inner join OperatorSenderId os on up.ACCOUNT=os.Provider where  up.userid='" + txtUserIdAPI.Text + "'";
                            OPEARTOR = Convert.ToString(database.GetScalarValue(sql));
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender Id for user " + txtUserIdAPI.Text + " is already Mapped with "+ OPEARTOR + " Operator !!');", true);
                        return;
                    }
                    if (rbTran.Checked == true)
                    {
                        if (Convert.ToString(ViewState["CheckTranUserTran"]) == "false")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Already Exists Please Delete This To insert New One !!');", true);
                            txtUserIdAPI.Focus();
                            return;
                        }
                        if (Convert.ToString(ViewState["CheckTranUserUAETran"]) == "false")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('USER does Not Belong to UAE, please Enter UAE USERID to proceed !!');", true);
                            txtUserIdAPI.Focus();
                            return;
                        }
                        string Msg = obj.INSERTUAEAPIAccounts(txtUserIdAPI.Text.Trim(), "", ddlProviderAPI.SelectedValue, true, "TRAN");
                        if (Msg == "Saved Successfully !!")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='AccountSystemIdforPanel.aspx';", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not saved !!');", true);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ViewState["CheckTranUserPromo"]) == "false")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Already Exists Please Delete This To insert New One !!');", true);
                            return;
                        }
                        if (Convert.ToString(ViewState["CheckTranUserUAEPromo"]) == "false")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('USER does Not Belong to UAE, please Enter UAE USERID to proceed !!');", true);
                            return;
                        }

                        string Msg = obj.INSERTUAEAPIAccounts(txtUserIdAPI.Text.Trim(), "", ddlProviderAPI.SelectedValue, true, "PROMO");
                        if (Msg == "Saved Successfully !!")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='AccountSystemIdforPanel.aspx';", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not saved !!');", true);
                            return;
                        }
                    }

                }
                else
                {
                    userpanel();
                    if (txtUserIdPanel.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter User ID !!');", true);
                        txtUserIdPanel.Focus();
                        return;
                    }
                    if (ddlProviderPanel.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Plaese Select Provider  !!');", true);
                        ddlProviderPanel.Focus();
                        return;
                    }
                    string chkmpdoperator = CheckMappedOperatorPanel();
                    if (chkmpdoperator != "")
                    {
                        string sql = @"select  s.PROVIDER from smppaccountuserid su 
                        inner join smppsetting s on Convert(varchar(20),s.smppaccountid) = Convert(varchar(20), su.smppaccountid)
                        inner join OperatorSenderId os on os.Provider = s.PROVIDER
                        where s.ACTIVE = 1 and os.Active = 1 and su.userid = '" + txtUserIdPanel.Text + "'";
                        string getOperator = Convert.ToString(database.GetScalarValue(sql));

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender Id for user " + txtUserIdPanel.Text + " is already Mapped with " + getOperator + " Operator !!');", true);
                        return;
                    }
                    if (Convert.ToString(ViewState["CheckPanelUser"]) == "false")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Already Exists Please Delete this To insert New One !!');", true);
                        txtUserIdPanel.Focus();
                        return;
                    }
                    if (Convert.ToString(ViewState["CheckValidUser"]) == "false")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account does Not Exists !!');", true);
                        txtUserIdPanel.Focus();
                        return;
                    }
                    
                    string CountryCode = obj.CountryCode(txtUserIdPanel.Text);
                    string ManualACID = "";
                    if (ddlManualAcid.SelectedValue == "0")
                    {
                        ManualACID = ddlProviderPanel.SelectedValue;
                    }
                    else
                    {
                        ManualACID = ddlManualAcid.SelectedValue;
                    }
                    string Msg = obj.Insertsmppaccountuserid(txtUserIdPanel.Text.Trim(), ddlProviderPanel.SelectedValue, CountryCode, true, ManualACID);
                    if (Msg == "Saved Successfully !!")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Msg + "');window.location ='AccountSystemIdforPanel.aspx';", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not saved !!');", true);
                        return;
                    }
                }
            }
            mp1.Show();
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            mp1.Hide();
        }

        protected void txtUserIdAPI_TextChanged(object sender, EventArgs e)
        {
            uservalidcheckAPI();
            if (rbTran.Checked == true)
            {
                BindApiTranGrid();
            }
            else 
            {
                BindApipromoGrid();
            }
        }

        public string CheckMappedOperatorTran()
        {
            string sql = "if exists (select * from UAEAPIAccounts ua inner join OperatorSenderId os on ua.ACCOUNT=os.Provider where ua.Active=1 and userid='" + txtUserIdAPI.Text + "' ) begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }
        public string CheckMappedOperatorPromo()
        {
            string sql = "if exists (select * from UAEAPIACCOUNTPROMO up inner join OperatorSenderId os on up.ACCOUNT=os.Provider where  up.userid='" + txtUserIdAPI.Text + "' ) begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }
        public string CheckMappedOperatorPanel()
        {
            string sql = @"if exists(select  su.Userid, s.PROVIDER, os.SID from smppaccountuserid su 
                            inner join smppsetting s on Convert(varchar(20),s.smppaccountid) = Convert(varchar(20),su.smppaccountid)  
                            inner join OperatorSenderId os on os.Provider = s.PROVIDER 
                            where s.ACTIVE = 1 and os.Active = 1 and su.userid = '" + txtUserIdPanel.Text.Trim() + "')begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }
        public void uservalidcheckAPI()
        {
            if (rbTran.Checked == true)
            {
                string sql = "if exists (select * from UAEAPIAccounts where userid='" + txtUserIdAPI.Text.Trim() + "') begin select 'true' end";
                string CheckTranUser = Convert.ToString(database.GetScalarValue(sql));
                if (CheckTranUser == "true")
                {
                    lblUserIdAPICheck.Text = "Account Already Exists Please Delete this To insert New One !!";
                    ViewState["CheckTranUserTran"] = "false";
                    return;
                }
                else
                {
                    lblUserIdAPICheck.Text = "";
                    ViewState["CheckTranUserTran"] = null;
                }
                string sql1 = "if exists (select * from Customer where COUNTRYCODE=971 and username='" + txtUserIdAPI.Text.Trim() + "') begin select 'true' end";
                string CheckTranUserUAE = Convert.ToString(database.GetScalarValue(sql1));
                if (CheckTranUserUAE == "")
                {
                    lblUserIdAPICheck.Text = "USER does Not Belong to UAE, please Enter UAE USERID to proceed !!";
                    ViewState["CheckTranUserUAETran"] = "false";
                    return;
                }
                else
                {
                    lblUserIdAPICheck.Text = "";
                    ViewState["CheckTranUserUAETran"] = null;
                }
            }
            if (rbPromo.Checked == true)
            {
                string sql = "if exists (select * from UAEAPIAccountPromo where userid='" + txtUserIdAPI.Text.Trim() + "') begin select 'true' end";
                string CheckTranUser = Convert.ToString(database.GetScalarValue(sql));
                if (CheckTranUser == "true")
                {
                    lblUserIdAPICheck.Text = "Account Already Exists Please Delete this To insert New One !!";
                    ViewState["CheckTranUserPromo"] = "false";
                    return;
                }
                else
                {
                    lblUserIdAPICheck.Text = "";
                    ViewState["CheckTranUserPromo"] = null;
                }
                string sql1 = "if exists (select * from Customer where COUNTRYCODE=971 and username='" + txtUserIdAPI.Text.Trim() + "') begin select 'true' end";
                string CheckTranUserUAE = Convert.ToString(database.GetScalarValue(sql1));
                if (CheckTranUserUAE == "")
                {
                    lblUserIdAPICheck.Text = "USER does Not Belong to UAE, please Enter UAE USERID to proceed !!";
                    ViewState["CheckTranUserUAEPromo"] = "false";
                    return;
                }
                else
                {
                    lblUserIdAPICheck.Text = "";
                    ViewState["CheckTranUserUAEPromo"] = null;
                }
            }
        }

        protected void txtUserIdPanel_TextChanged(object sender, EventArgs e)
        {
            userpanel();
            BindPanelGrid();
        }


        public void userpanel()
        {
            string sql = "if exists (select * from smppaccountuserid where userid='" + txtUserIdPanel.Text.Trim() + "') begin select 'true' end";
            string CheckPanelUser = Convert.ToString(database.GetScalarValue(sql));
            string sql1 = "if exists (select * from customer where username='" + txtUserIdPanel.Text.Trim() + "') begin select 'true' end";
            string CheckvalidUser = Convert.ToString(database.GetScalarValue(sql1));
            if (CheckPanelUser == "true")
            {
                lblUserIdPanel.Text = "Account Already Exists Please Delete this To insert New One !!";
                ViewState["CheckPanelUser"] = "false";
                return;
            }
            else
            {
                lblUserIdPanel.Text = "";
                ViewState["CheckPanelUser"] = null;
            }
            if (CheckvalidUser == "true")
            {
                lblUserIdPanel.Text = "";
                ViewState["CheckValidUser"] = null;
            }
            else
            {
                lblUserIdPanel.Text = "User Account Does Not Exist !!";
                ViewState["CheckValidUser"] = "false";
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string userid = Convert.ToString((grdAPITran.Rows[IndexNo].FindControl("HD_usid") as HiddenField).Value);
            string DateTime = Convert.ToString((grdAPITran.Rows[IndexNo].FindControl("HD_dtime") as HiddenField).Value);
            DateTime date = Convert.ToDateTime(DateTime);
            string ndate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string Provider = Convert.ToString((grdAPITran.Rows[IndexNo].FindControl("HD_provider") as HiddenField).Value);
            string Msg = obj.INSERTUAEAPIAccountslog(userid, "", ndate, Provider, false, "TRAN");
            obj.deleteUAEAPIAccountsTRAN(userid, ndate, Provider);
            BindApiTranGrid();
        }

        protected void lnkAPIPromoDelete_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string userid = Convert.ToString((GridAPIPROMO.Rows[IndexNo].FindControl("HD_usid") as HiddenField).Value);
            string DateTime = Convert.ToString((GridAPIPROMO.Rows[IndexNo].FindControl("HD_dtime") as HiddenField).Value);
            DateTime date = Convert.ToDateTime(DateTime);
            string ndate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string Provider = Convert.ToString((GridAPIPROMO.Rows[IndexNo].FindControl("HD_provider") as HiddenField).Value);
            string Msg = obj.INSERTUAEAPIAccountslog(userid, "", ndate, Provider, true, "Promo");
            obj.deleteUAEAPIAccountsPROMO(userid, ndate, Provider);
            BindApipromoGrid();
        }

        protected void lnkpanelDelete_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string userid = Convert.ToString((grdPanel.Rows[IndexNo].FindControl("HD_usid") as HiddenField).Value);
            string smppaccountid = Convert.ToString((grdPanel.Rows[IndexNo].FindControl("HD_smppaccountid") as HiddenField).Value);
            string countrycode = Convert.ToString((grdPanel.Rows[IndexNo].FindControl("HD_countrycode") as HiddenField).Value);
            string ManualAcId = Convert.ToString((grdPanel.Rows[IndexNo].FindControl("HD_ManualAcId") as HiddenField).Value);
            string msg = obj.Insertsmppaccountuseridlog(userid, smppaccountid, countrycode, true, ManualAcId);
            obj.deletePanelData(userid, smppaccountid, countrycode, ManualAcId);
            BindPanelGrid();
        }
        protected void GridFormat(DataTable dt)
        {
            grdPanel.UseAccessibleHeader = true;
            grdPanel.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grdPanel.TopPagerRow != null)
            {
                grdPanel.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grdPanel.BottomPagerRow != null)
            {
                grdPanel.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grdPanel.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void GridFormat1(DataTable dt)
        {
            GridAPIPROMO.UseAccessibleHeader = true;
            GridAPIPROMO.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (GridAPIPROMO.TopPagerRow != null)
            {
                GridAPIPROMO.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (GridAPIPROMO.BottomPagerRow != null)
            {
                GridAPIPROMO.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                GridAPIPROMO.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void GridFormat2(DataTable dt)
        {
            grdAPITran.UseAccessibleHeader = true;
            grdAPITran.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grdAPITran.TopPagerRow != null)
            {
                grdAPITran.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grdAPITran.BottomPagerRow != null)
            {
                grdAPITran.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grdAPITran.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountSystemIdforPanel.aspx");
        }
    }
}