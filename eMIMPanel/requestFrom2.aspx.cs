using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;


namespace eMIMPanel
{
    public partial class requestFrom2 : System.Web.UI.Page
    {
        Helper.common obj = new Helper.common();
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                ddLang.SelectedValue = Convert.ToString(Session["Lang"]); //Request.UserLanguages[0];

                LoadString();
                fillEmployee();
                FillProductGroup();
                FillProductSubGroup(1);
                FillTransType();
                OrderType();

                //Comment by naved
                ddlEmployee.SelectedValue = (Session["USER_BD"] as Helper.common.mlogin).employeeid.ToString();
                //ddlEmployee.SelectedItem.Text = Convert.ToString(Session["FullName"]);

            }

        }
        public void OrderType()
        {
            Helper.common obj = new Helper.common();
            DataTable dt = obj.GetDataTable("SELECT ID,NAME FROM VW_PaymentMode");
            if (dt.Rows.Count > 0)
            {
                ddlOrderType.DataSource = dt;
                ddlOrderType.DataTextField = "Name";
                ddlOrderType.DataValueField = "ID";
                ddlOrderType.DataBind();
                ddlOrderType.Items.Insert(0, new ListItem("-- select --", "0"));
            }
        }
        public void FillTransType()
        {
            Helper.common obj = new Helper.common();
            DataTable dt = obj.GetDataTable("SELECT ID,NAME FROM VW_TransactionType");
            if (dt.Rows.Count > 0)
            {
                ddltranstype.DataSource = dt;
                ddltranstype.DataTextField = "Name";
                ddltranstype.DataValueField = "ID";
                ddltranstype.DataBind();
                ddltranstype.Items.Insert(0, new ListItem("-- select --", "0"));
            }
        }
        public void FillProductGroup()
        {
            Helper.common obj = new Helper.common();
            DataTable dt = obj.GetDataTable("SELECT ProductId,ProductName FROM PRODUCT WHERE ParentId =-1");
            if (dt.Rows.Count > 0)
            {
                ddlProduct.DataSource = dt;
                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductId";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-- select --", "0"));
            }
        }

        public void fillEmployee()
        {
            Helper.common obj = new Helper.common();
            DataTable dt = obj.GetDataTable("SELECT EMPLOYEEID,NAME,EMAILID FROM [EMPLOYEE] ORDER BY NAME");
            ViewState["Emp"] = dt;
            if (dt.Rows.Count > 0)
            {
                ddlEmployee.DataSource = dt;
                ddlEmployee.DataTextField = "Name";
                ddlEmployee.DataValueField = "EmployeeId";
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("-- select --", "0"));
            }
        }



        protected void ddLang_SelectedIndexChanged(object sender, EventArgs e) //this event for showing selected language.
        {
            Session["Lang"] = ddLang.SelectedValue;
            LoadString();

        }
        private void LoadString()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Convert.ToString(Session["Lang"]));
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(Convert.ToString("en-US"));
            rm = new ResourceManager("eMIMPanel.App_GlobalResources2.Lang", Assembly.GetExecutingAssembly()); //we configure resource manages for mapping with resource files in App_GlobalResources folder.  
            ci = Thread.CurrentThread.CurrentCulture;

            Campaign_Request_Form.InnerText = rm.GetString("Campaign_Request_Form", ci);
            Sales_Executive.InnerText = rm.GetString("Sales_Executive", ci);
            Company_Name.InnerText = rm.GetString("Company_Name", ci);
            Client_Name.InnerText = rm.GetString("Client_Name", ci);
            //txtPhone2.Name.pla = rm.GetString("Mobile_Number", ci); 
            txtPhone2.Attributes["placeholder"] = rm.GetString("Mobile_Number", ci);
            Email_Address.InnerText = rm.GetString("Email_Address", ci);
            Product_Group.InnerText = rm.GetString("Product_Group", ci);
            Product_Sub_Group.InnerText = rm.GetString("Product_Sub_Group", ci);
            Transaction_Type.InnerText = rm.GetString("Transaction_Type", ci);
            Order_Type.InnerText = rm.GetString("Order_Type", ci);
            Credentials_to_be_used.InnerText = rm.GetString("Credentials_to_be_used", ci);
            //Client.InnerText = rm.GetString("Client", ci); 
            Sender_Id.InnerText = rm.GetString("Sender_ID", ci);
            SMS_Text.InnerText = rm.GetString("SMS_Content", ci);
            //  Template_Id.InnerText = rm.GetString("Template_Id", ci);
            Quantity.InnerText = rm.GetString("Quantity", ci);
            Rate.InnerText = rm.GetString("Rate", ci);
            //Upload_File.InnerText = rm.GetString("Upload_File", ci);
            filesize.InnerText = rm.GetString("filesize", ci);
            //txtPhone2.Attributes.Add("placeholder", rm.GetString("Mobile_Number", ci));


            rbcredentials.Items[0].Text = rm.GetString("Client", ci);
            rbcredentials.Items[1].Text = rm.GetString("MiM", ci);


            //Choose_File.InnerText = rm.GetString("Choose_File", ci);
            //No_File_Chosen.InnerText = rm.GetString("No_File_Chosen", ci);
            if (ddLang.SelectedValue == "ar-EG")
            {
                dvPEID.Visible = false;
                dvTemplateId.Visible = false;
            }
            else
            {
                dvPEID.Visible = true;
                dvTemplateId.Visible = true;
            }
            btnSave.Text = rm.GetString("Submit", ci);
        }


        public void FillProductSubGroup(int ProductId)
        {


            DataTable dt = obj.GetDataTable("SELECT ProductId,ProductName FROM PRODUCT WHERE ParentId= " + ProductId);
            Helper.common.FillDropDown(ddlproductsubgroup, dt, "ProductName", "ProductId", 'S');
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = new Helper.common().GetDataTable("select 'XXXXXXXXXXXXXXX'+SUBSTRING(PEID,len(peid)-3,4)PEID,* from customer WHERE USERNAME='" + txtUserId.Text.Trim() + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                txtPEID.Text = dt.Rows[0]["PEID"].ToString();
                //txtsenderid.Text = dt.Rows[0]["SENDERID"].ToString();
            }
            else
            {
                txtPEID.Text = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
        }


        public void Save()
        {
            if (ddlEmployee.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select any sales executive.');", true);
                ddlEmployee.Focus();
                return;
            }
            if (txtCompany.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter company name.');", true);
                txtCompany.Focus();
                return;
            }
            if (txtcustomername.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter client name.');", true);
                txtcustomername.Focus();
                return;
            }

            if (txtPhone2.Value.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter mobile no.');", true);
                txtPhone2.Focus();
                return;
            }

            if (txtPhone2.Value.Trim().Length + hfCountryCode.Value.Length != 12)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile no should be of 12 digit .');", true);
                txtPhone2.Focus();
                return;
            }
            if (txtEmailId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter email id.');", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("@"))
            { }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter email address in xxxxx@yyyy.zzz format.');", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("."))
            { }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter email address in xxxxx@yyyy.zzz format.');", true);
                txtEmailId.Focus();
                return;
            }
            if (ddlProduct.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select any product group.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                ddlProduct.Focus();
                return;
            }
            if (ddlproductsubgroup.SelectedValue == "0" && ddlProduct.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select product sub group.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                ddlproductsubgroup.Focus();
                return;
            }
            if (ddlproductsubgroup.SelectedValue == "0" && ddlProduct.SelectedValue == "8")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select product sub group.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                ddlproductsubgroup.Focus();
                return;
            }
            if (ddltranstype.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select any transaction type.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                ddltranstype.Focus();
                return;
            }
            if (ddlOrderType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Order type.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                ddlOrderType.Focus();
                return;
            }
            if (ddlProduct.SelectedValue == "1" && txtUserId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter userid.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtUserId.Focus();
                return;
            }
            if (rbcredentials.SelectedValue == "Client" && txtPEID.Text.Trim() == "" && ddLang.SelectedValue != "ar-EG")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter peid.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtPEID.Focus();
                return;
            }
            if (rbcredentials.SelectedValue == "Client" && txtsenderid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter sender id.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtsenderid.Focus();
                return;
            }
            if (rbcredentials.SelectedValue == "Client" && txtTemplateId.Text.Trim() == "" && ddLang.SelectedValue != "ar-EG")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter template id.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtTemplateId.Focus();
                return;
            }
            if (txtmsg.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter sms text.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtmsg.Focus();
                return;
            }
            txtQuantity.Text = txtQuantity.Text.Replace(",", "");
            if (txtQuantity.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter quantity.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtQuantity.Focus();
                return;
            }
            if (int.Parse(txtQuantity.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter quantity.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtQuantity.Focus();
                return;
            }
            try
            {
                if (txtRate.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter rate.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                    txtRate.Focus();
                    return;
                }
                if (decimal.Parse(txtRate.Text.Trim()) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter rate.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                    txtRate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter rate in correct format');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                txtRate.Focus();
                return;
            }


            if (FU1.HasFile)
            {
                string Ext = "";

                if (FU1.PostedFile.ContentLength > (5 * 1024 * 1024))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File size can not be above of 5 mb.');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                    FU1.Focus();
                    return;
                }
                string FolderPath = "~/Uploads/Images";
                string FileName = Path.GetFileName(FU1.PostedFile.FileName);
                string Ex = Path.GetExtension(FU1.PostedFile.FileName);

                //if ((Ex.ToUpper()!=".JPEG") && (Ex.ToUpper() != ".JPG") && (Ex.ToUpper() != ".PNG") && (Ex.ToUpper() != ".PDF"))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Only .jpeg / .jpg / .png / .pdf');setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
                //    FU1.Focus();
                //    return;
                //}

                FileName = DateTime.Now.ToString("yyyyddMMhhmmssfff") + Ex;
                string FilePath = Server.MapPath(FolderPath +"//"+ FileName);
                ViewState["Attachment"] = FilePath;
                FU1.SaveAs(FilePath);
            }
            Helper.common.request obj = new Helper.common.request();

            obj.EmployeeId = int.Parse((Session["USER_BD"] as Helper.common.mlogin).employeeid.ToString());

            obj.CompanyName = txtCompany.Text.Trim();
            obj.ClientName = txtcustomername.Text.Trim();
            obj.MobileNo = hfCountryCode.Value + txtPhone2.Value.Trim();
            obj.EmailId = txtEmailId.Text.Trim();

            obj.ProductId = int.Parse(ddlProduct.SelectedValue);
            obj.TransTypeId = int.Parse(ddltranstype.SelectedValue);
            obj.PaymodeId = int.Parse(ddlOrderType.SelectedValue);
            obj.AccountTypeId = int.Parse(ddlproductsubgroup.SelectedValue);
            obj.QTY = int.Parse(txtQuantity.Text.Trim());
            obj.Rate = decimal.Parse(txtRate.Text.Trim());
            obj.UserId = txtUserId.Text.Trim();

            obj.Credentialtobeused = rbcredentials.SelectedValue.Trim();
            obj.senderid = txtsenderid.Text.Trim();
            obj.templateid = txtTemplateId.Text.Trim();
            obj.SMSText = txtmsg.Text.Trim();
            obj.PEID = txtPEID.Text.Trim();


            if (ViewState["Attachment"] == null)
            {
                ViewState["Attachment"] = "";
            }

            obj.Attachment = ViewState["Attachment"].ToString();

            string msg = new Helper.common().SaveRequest(obj);

            if (msg.Contains("successfully"))
            {
                string[] arr = msg.Split(':');
                obj.RequestNo = arr[1].ToString().Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                //sendmail(obj);
                Reset();
            }




        }
        public void sendmail(Helper.common.request rq)
        {
            StringBuilder sb = new StringBuilder();
            Helper.common obj = new Helper.common();

            List<string> CC = new List<string>();

            DataTable dt = obj.GetDataTable("SELECT * FROM [setting]");
            DataTable dtemp = (ViewState["Emp"] as DataTable);

            dtemp.DefaultView.RowFilter = "EmployeeId=" + ddlEmployee.SelectedValue.Trim() + "";
            dtemp = dtemp.DefaultView.ToTable();
            string ToEmailId = dtemp.Rows[0]["EmailId"].ToString().Trim();
            string Subject = "";
            sb.Append("<b>Executive Name :</b>" + ddlEmployee.SelectedItem.Text.Trim() + "<br>");
            sb.Append("<b>Company Name :</b>" + txtCompany.Text.Trim() + "<br>");
            sb.Append("<b>Client Name :</b>" + txtcustomername.Text.Trim() + "<br>");
            sb.Append("<b>Mobile No :</b>" + txtPhone2.Value.Trim() + "<br>");
            sb.Append("<b>Email Id :</b>" + txtEmailId.Text.Trim() + "<br>");
            sb.Append("<b>Product Group:</b>" + ddlProduct.SelectedItem.Text.Trim() + "<br>");

            if (ddlProduct.SelectedValue == "1" || ddlProduct.SelectedValue == "8")
            {
                sb.Append("<b>Product Sub Group :</b>" + ddlproductsubgroup.SelectedItem.Text.Trim() + "<br>");
            }
            sb.Append("<b>Transaction Type:</b>" + ddltranstype.SelectedItem.Text.Trim() + "<br>");
            sb.Append("<b>Order Type:</b>" + ddlOrderType.SelectedItem.Text.Trim() + "<br>");
            sb.Append("<b>Quantity :</b>" + txtQuantity.Text.Trim() + "<br>");
            sb.Append("<b>Rate :</b>" + txtRate.Text.Trim() + "<br>");
            sb.Append("<b>Credentials to be used :</b>" + rbcredentials.SelectedValue.Trim() + "<br>");
            if (rbcredentials.SelectedValue.Trim() != "MIM")
            {
                sb.Append("<b>Sender Id :</b>" + txtsenderid.Text.Trim() + "<br>");
                sb.Append("<b>Template Id :</b>" + txtTemplateId.Text.Trim() + "<br>");
                sb.Append("<b>PEID :</b>" + txtPEID.Text.Trim() + "<br>");
            }
            sb.Append("<b>SMS Text :</b>" + txtmsg.Text.Trim() + "<br>");
            sb.Append("This is a system generated email.<br><br>");
            sb.Append("Generated at : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));

            if (dt.Rows.Count > 0)
            {
                Subject = dt.Rows[0]["Subject"].ToString().Trim() + " " + rq.RequestNo + " Dated " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                if (dt.Rows[0]["CC1"] != null && dt.Rows[0]["CC1"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC1"].ToString().Trim());
                }
                if (dt.Rows[0]["CC2"] != null && dt.Rows[0]["CC2"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC2"].ToString().Trim());
                }
                if (dt.Rows[0]["CC3"] != null && dt.Rows[0]["CC3"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC3"].ToString().Trim());
                }
                if (dt.Rows[0]["CC4"] != null && dt.Rows[0]["CC4"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC4"].ToString().Trim());
                }
                if (dt.Rows[0]["CC5"] != null && dt.Rows[0]["CC5"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC5"].ToString().Trim());
                }
                if (dt.Rows[0]["CC6"] != null && dt.Rows[0]["CC6"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC6"].ToString().Trim());
                }
                if (dt.Rows[0]["CC7"] != null && dt.Rows[0]["CC7"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC7"].ToString().Trim());
                }
                if (dt.Rows[0]["CC8"] != null && dt.Rows[0]["CC8"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC8"].ToString().Trim());
                }
                if (dt.Rows[0]["CC9"] != null && dt.Rows[0]["CC9"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC9"].ToString().Trim());
                }
                if (dt.Rows[0]["CC10"] != null && dt.Rows[0]["CC10"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC10"].ToString().Trim());
                }
            }
            if (ToEmailId.Trim() == "madhur@myinboxmedia.com")
            {
                CC.Add("sangram@myinboxmedia.com");
            }

            string str = new Helper.common().SendEmail(ToEmailId, Subject, sb.ToString(), dt.Rows[0]["UserId"].ToString().Trim(), dt.Rows[0]["Password"].ToString().Trim(), dt.Rows[0]["host"].ToString().Trim(), dt.Rows[0]["PortNo"].ToString().Trim(), CC, ViewState["Attachment"].ToString());
        }
        public void Reset()
        {
            txtUserId.Text = "";
            txtCompany.Text = "";
            txtcustomername.Text = "";
            txtPhone2.Value = "";
            txtEmailId.Text = "";
            // ddlEmployee.SelectedValue = "0";
            ddlProduct.SelectedValue = "0";
            ddltranstype.SelectedValue = "0";
            ddlOrderType.SelectedValue = "0";
            ddlproductsubgroup.SelectedValue = "0";
            txtQuantity.Text = "";
            txtRate.Text = "";

            txtsenderid.Text = "";
            txtTemplateId.Text = "";
            txtPEID.Text = "";
            txtmsg.Text = "";
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProductSubGroup(int.Parse(ddlProduct.SelectedValue));
            if (ddlProduct.SelectedValue == "1")
            {
                dvuserid.Visible = true;
            }
            else
            {
                dvuserid.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "setTimeout(loadscr('" + hfiso2.Value.ToUpper() + "'), 500);", true);
            rbcredentials.SelectedValue = "Client";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}