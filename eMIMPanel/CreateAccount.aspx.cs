using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class CreateAccount : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                FillValueCreateAccount("AllGetInformation_BDCustomerRequest", id);

            }
            else
            {
                usertype = Convert.ToString(Session["UserType"]);
                user = Convert.ToString(Session["User"]);
                if (user == "") Response.Redirect("login.aspx");
                if (usertype != "SYSADMIN" && usertype == "ADMIN")
                {
                    Response.Redirect("CreateAccountAdmin.aspx");
                }
                if (usertype == "BD")
                {
                    Response.Redirect("CreateAccountBD.aspx");
                }

                txtCreatedAt.Text = "Account Creation Date : " + DateTime.Now.ToString("dd/MMM/yyyy");
                if (!IsPostBack)
                {
                    Session["EDITUSER"] = "";
                    txtExpiry.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");
                    string PWD = ob.GeneratePWD();
                    // string PWD = ob.GeneratePWD().Replace('#', '0').Replace('|', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('+', '_').Replace('%', '_').Replace('&', '_').Replace('/', '_').Replace(@"\", "_");
                    txtPwd.Text = PWD;
                    fillemployee();
                    rbTrans.Checked = true;
                    ddlMiMReportGroupBind();
                }

            }
            
            //txtCreatedAt.Enabled = false;
        }

        public void FillValueCreateAccount(string proc, int id)
        {
            List<SqlParameter> pram = new List<SqlParameter>();
            Helper.Util db = new Helper.Util();
            DataTable dt = new DataTable();
            pram.Clear();
            pram.Add(new SqlParameter("@action", "GetBDCUSTOMERREQUESTDetail"));
            pram.Add(new SqlParameter("@id",id));
            dt = db.GetDataTableRecord(proc, pram);
            if (dt!= null)
            {
                if (dt.Rows.Count>0)
                {

                    ddlUserType.SelectedValue = dt.Rows[0]["USERTYPE"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ACCOUNTTYPE"].ToString()))
                    {
                        ddlActType.SelectedValue = Convert.ToString(dt.Rows[0]["ACCOUNTTYPE"]);
                    }
                    txtName.Text = dt.Rows[0]["FULLNAME"].ToString();
                    txtMob1.Text= dt.Rows[0]["MOBILE1"].ToString();
                    txtEmail.Text= dt.Rows[0]["EMAIL"].ToString();
                    txtDLT.Text= dt.Rows[0]["DLTNO"].ToString();
                    txtWebsite.Text = Convert.ToString(dt.Rows[0]["WEBSITE"]);
                    txtgroupname.Text= dt.Rows[0]["GROUPNAME"].ToString();
                    txtccemail.Text= dt.Rows[0]["CCEmail"].ToString();
                    txtPwd.Text=  dt.Rows[0]["PWD"].ToString();
                    txtCompName.Text = dt.Rows[0]["COMPNAME"].ToString();
                    
                    if (!String.IsNullOrEmpty(dt.Rows[0]["peid"].ToString()))
                    {
                        txtPEID.Text = Convert.ToString(dt.Rows[0]["peid"].ToString());
                    }
                 
                    txtCreatedAt.Text = "Account Creation Date : " + Convert.ToDateTime(dt.Rows[0]["ACCOUNTCREATEDON"]).ToString("dd/MMM/yyyy");
                    if (!String.IsNullOrEmpty(dt.Rows[0]["EXPIRY"].ToString()))
                    {
                        txtExpiry.Text = Convert.ToDateTime(dt.Rows[0]["EXPIRY"]).ToString("dd/MMM/yyyy");

                    }
                    else
                    {
                        //txtExpiry.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");
                    }
                    
                    txtSender.Text = Convert.ToString(dt.Rows[0]["SENDERID"]);
                   
                    

                    fillemployee();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["EmpCode"].ToString()))
                    {
                        ddlemployee.SelectedValue = dt.Rows[0]["EmpCode"].ToString();

                    }
                    



                }

            }


            

        }
        public void fillemployee()
        {
            DataTable dt = Helper.database.GetDataTable("select distinct name+' ('+employeecode+')' empname,employeecode from employee order by empname");
            ddlemployee.DataSource = dt;
            ddlemployee.DataTextField = "empname";
            ddlemployee.DataValueField = "employeecode";
            ddlemployee.DataBind();
            ddlemployee.Items.Insert(0, new ListItem("Select Executive", "0"));
        }

        public void ddlMiMReportGroupBind()
        {
            DataTable dt = ob.getMiMReportGroup();
            ddlMiMREportGroup.DataSource = dt;
            ddlMiMREportGroup.DataTextField = "client";
            ddlMiMREportGroup.DataValueField = "client";
            ddlMiMREportGroup.DataBind();
            ddlMiMREportGroup.Items.Insert(0, new ListItem("OTHERS", "0"));

        }
        protected void lnkShowAC_Click(object sender, EventArgs e)
        {
            if(txtAccountID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Account ID to EDIT.');", true);
                return;
            }
            
            DataTable dt = ob.GetUserParameter(txtAccountID.Text.Trim());

            if(dt.Rows.Count>0)
            {
                Session["EDITUSER"] = txtAccountID.Text.Trim();
                txtCompName.Text = Convert.ToString(dt.Rows[0]["COMPNAME"]);
                txtName.Text = Convert.ToString(dt.Rows[0]["FULLNAME"]);
                txtMob1.Text = Convert.ToString(dt.Rows[0]["MOBILE1"]);
                txtEmail.Text= Convert.ToString(dt.Rows[0]["EMAIL"]);
                txtWebsite.Text = Convert.ToString(dt.Rows[0]["WEBSITE"]);
                txtccemail.Text = Convert.ToString(dt.Rows[0]["CCEmail"]);
                txtDLT.Text = Convert.ToString(dt.Rows[0]["DLTNO"]);
                txtCreatedAt.Text = "Account Creation Date : " + Convert.ToDateTime(dt.Rows[0]["ACCOUNTCREATEDON"]).ToString("dd/MMM/yyyy");
                txtExpiry.Text = Convert.ToDateTime(dt.Rows[0]["EXPIRY"]).ToString("dd/MMM/yyyy");
                txtPEID.Text = Convert.ToString(dt.Rows[0]["PEID"]);
                txtSender.Text = Convert.ToString(dt.Rows[0]["SENDERID"]);
                ddlUserType.SelectedValue = Convert.ToString(dt.Rows[0]["USERTYPE"]).ToUpper();
                ddlSMSType.SelectedValue = Convert.ToString(dt.Rows[0]["SMSTYPE"]);
                ddlActType.SelectedValue = Convert.ToString(dt.Rows[0]["ACCOUNTTYPE"]);
                ddlPermission.SelectedValue = Convert.ToString(dt.Rows[0]["PERMISSION"]);
                txtPwd.Text= Convert.ToString(dt.Rows[0]["pwd"]);
                ddlCountryCode.SelectedValue = Convert.ToString(dt.Rows[0]["defaultCountry"]);
                chkIsShowcurrency.Checked= Convert.ToBoolean(dt.Rows[0]["Isshowcurrency"]);
                txtgroupname.Text = Convert.ToString(dt.Rows[0]["groupname"]);
                string AccountCreationType = Convert.ToString(dt.Rows[0]["AccountCreationType"]);//--Shishir
                if (AccountCreationType == "PostPaid")
                {
                    rbPrepaid.Checked = false;
                    rbPostpaid.Checked = true;
                }
                else
                {
                    rbPostpaid.Checked = false;
                    rbPrepaid.Checked = true;
                }

                string ReportGroup = ob.getMiMReportGroupBind(txtAccountID.Text.Trim());
                if (ReportGroup != "")
                {
                    ddlMiMREportGroup.SelectedValue = ReportGroup;
                }
                if (Convert.ToString(dt.Rows[0]["EmpCode"])!="")
                {
                    ddlemployee.SelectedValue = Convert.ToString(dt.Rows[0]["EmpCode"]);
                }
                if(Convert.ToString(dt.Rows[0]["TranOrPromo"]) != "")
                {
                    if (Convert.ToString(dt.Rows[0]["TranOrPromo"]) == "Promo")
                    {
                        rbPromo.Checked = true;
                        rbTrans.Checked = false;
                    }
                    else
                    {
                        rbPromo.Checked = false;
                        rbTrans.Checked = true;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Account ID.');", true);
                return;
            }
        }
        protected void lnkEditAC_Click(object sender, EventArgs e)
        {
            pwd.Visible = false;
            pnledit.Visible = true;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string mode = "ADD";
            if (pnledit.Visible && txtAccountID.Text != "" && Session["EDITUSER"] != null) mode = "EDIT";
            if (ddlUserType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select User Type.');", true);
                return;
            }
            if (txtSender.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID can not be blank.');", true);
                return;
            }
            //if (txtSender.Text.Trim().Length != 6)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sender ID must be of 6 Alphabets.');", true);
            //    return;
            //}
            if (ddlSMSType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select SMS Type.');", true);
                return;
            }
            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name can not be blank.');", true);
                return;
            }
            if (ddlActType.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Account Type.');", true);
                return;
            }
            if (ddlPermission.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Permission.');", true);
                return;
            }
            if (txtCompName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Company Name can not be blank.');", true);
                return;
            }
            //if (txtName.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name can not be blank.');", true);
            //    return;
            //}
            if (txtMob1.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile 1 can not be blank.');", true);
                return;
            }
            else
            {
                if (txtMob1.Text.Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile number must be minimum of 10 digits');", true);
                    return;
                }
                else if (txtMob1.Text.Length < 9 && ddlCountryCode.SelectedValue == "971")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile number must be minimum of 9 digits');", true);
                    return;
                }
                else
                {
                    if (txtMob1.Text.Length == 10 || (txtMob1.Text.Length == 9 && ddlCountryCode.SelectedValue == "971")) txtMob1.Text = ddlCountryCode.SelectedValue + txtMob1.Text;
                    // else txtMob1.Text = ddlCountryCode.SelectedValue + txtMob1.Text;

                }
            }


            //if (txtMob2.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile 2 can not be blank.');", true);
            //    return;
            //}
            if (txtEmail.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email can not be blank.');", true);
                return;
            }
            if (txtExpiry.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Expiry can not be blank.');", true);
                return;
            }
            if (txtDLT.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('DLT No can not be blank.');", true);
                return;
            }
            if (txtPEID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('PE-ID can not be blank.');", true);
                return;
            }
            if (txtPwd.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
                return;
            }
            //Shishir Region Start
            bool resPanel = ValidatePassword(txtPwd.Text.Trim());
            if (resPanel == false && mode == "ADD")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password Must Contain A-Za-z 0-9 and Special Character !');", true);
                return;
            }
            //Shishir Region End
            if (txtgroupname.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group name can not be blank.');", true);
                return;
            }
            if (ddlemployee.SelectedValue.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select executive.');", true);
                ddlemployee.Focus();
                return;
            }
            string AccountCreationType = "";
            if (rbPostpaid.Checked == true)
            {
                AccountCreationType = "PostPaid";
            }
            else
            {
                AccountCreationType = "Prepaid";
            }
            string GetAccountTypeinfo = Convert.ToString(Helper.database.GetScalarValue("select AccountCreationType from Customer where username='" + txtAccountID.Text + "'"));
            if (mode != "ADD")
            {
                if (GetAccountTypeinfo != AccountCreationType)
                {
                    String msg = ob.InsertLog(txtAccountID.Text, GetAccountTypeinfo, AccountCreationType, user);
                }
            }
            //if (mode == "ADD")
            //{
            //    //check duplicate mobile or email
            //    if (ob.CheckMobileEmailDuplicate(txtMob1.Text, "M"))
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Mobile Number has already been used for an existing Account.');", true);
            //        return;
            //    }
            //    if (ob.CheckMobileEmailDuplicate(txtEmail.Text, "E"))
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered Email ID has already been used for an existing Account.');", true);
            //        return;
            //    }
            //}

            bool IsShowcurrency = true;
            if (chkIsShowcurrency.Checked)
            {
                IsShowcurrency = true;
            }
            else
            {
                IsShowcurrency = false;
            }
            bool IsWABAAcive = chkWABA.Checked;
            if (txtDLT.Text == "HYUNDAISALES" || txtDLT.Text == "HYUNDAISERVICE" || txtDLT.Text == "HASC")
            {
                string msg = ob.InsertDealerMasterDltNoBased(txtName.Text, txtCompName.Text, Convert.ToInt64(txtMob1.Text.ToString()), txtAccountID.Text, "MiM@2021", "HMISVR", "1201158323518414595", "https://myinboxmedia.in/api/mim/SendSMS?", 0, "https://myinboxmedia.in/api/mim/GetBalance?");
            }
            string TranorPromo = "";
            if (rbTrans.Checked == true)
            {
                TranorPromo = "Trans";
            }
            else
            {
                TranorPromo = "Promo";
            }

            string res = ob.SaveCustomer(AccountCreationType, IsShowcurrency, txtSender.Text, txtName.Text, txtCompName.Text, txtWebsite.Text, txtMob1.Text, ddlUserType.Text, txtEmail.Text, (chkExpiry.Checked ? "31/Dec/2050" : txtExpiry.Text), txtDLT.Text, ddlSMSType.SelectedValue, ddlActType.SelectedValue, ddlPermission.SelectedValue, ddlCountryCode.SelectedValue, user, usertype, txtPEID.Text, mode, Convert.ToString(Session["EDITUSER"]), txtPwd.Text.Trim(), ddlemployee.SelectedValue, IsWABAAcive, txtgroupname.Text.Trim(), txtccemail.Text.Trim(), TranorPromo, ddlMiMREportGroup.SelectedValue);
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
            pnlMain.Enabled = false;
            Session["EDITUSER"] = "";
            //Response.Redirect("~/CreateAccount.aspx");

        }
        //Shishir Region Start
        static bool ValidatePassword(string passWord)
        {
            int validConditions = 0;
            if (passWord.Length < 8) return false;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions < 3) return false;
            if (validConditions == 3)
            {
                char[] special = { '!', '@', '#', '$', '%', '^', '*', '+', '=', '-', '_', '(', ')', '{', '}', '[', ']' }; // or whatever    
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }
        //Shishir Region End

        //string res = ob.SaveCustomer(IsShowcurrency, txtSender.Text, txtName.Text, txtCompName.Text, txtWebsite.Text, txtMob1.Text, ddlUserType.Text, txtEmail.Text, (chkExpiry.Checked ? "31/Dec/2050" : txtExpiry.Text), txtDLT.Text, ddlSMSType.SelectedValue, ddlActType.SelectedValue, ddlPermission.SelectedValue, ddlCountryCode.SelectedValue, user, usertype, txtPEID.Text, mode, Convert.ToString(Session["EDITUSER"]), txtPwd.Text.Trim(), ddlemployee.SelectedValue, IsWABAAcive, txtgroupname.Text.Trim(),txtccemail.Text.Trim());
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
        //    pnlMain.Enabled = false;
        //    Session["EDITUSER"] = "";
        //Response.Redirect("~/CreateAccount.aspx");
        //}
    }
}