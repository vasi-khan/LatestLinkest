using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class AddDistributionList : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Util ob = new Util();
        bool isUpload = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                PopulateCountry();
                GetData();
            }

            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (FileName != "")
                {
                    Helper.Util ob = new Helper.Util();
                    if (ob.FileUploadStopped())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File upload is temporarily stopped.');", true);
                    }
                    else
                    {
                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string en = Extension.ToUpper();
                        if (en.Contains("XLS"))
                            if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of 6 MB');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }

                        string FolderPath = "XLSUpload/";
                        Session["UPLOADFILENM"] = FileName;
                        Session["UPLOADFILENMEXT"] = Extension;
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        FileUpload1.SaveAs(FilePath);
                        string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);
                        if (res.Contains("RECORDCOUNT"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                            lblUploading.Text = FileUpload1.FileName;
                            lblUploading.ForeColor = System.Drawing.Color.Green;
                            divFileLoader.Style.Add("display", "none");
                            pnlPopUp_UploadNumber_Modal.Show();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                            lblUploading.Text = "Upload Rejected.";
                            divFileLoader.Style.Add("display", "none");
                            File.Delete(FilePath);
                            pnlPopUp_UploadNumber_Modal.Show();
                        }
                    }
                }
            }
        }

        public void PopulateCountry()
        {
            DataTable dt = ob.GetActiveCountry(Convert.ToString(Session["UserID"]));
            ddlCountry.DataSource = dt;
            ddlCountry.DataTextField = "name";
            ddlCountry.DataValueField = "countrycode";
            ddlCountry.DataBind();
            ddlCountry.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
        }

        public void GetData()
        {
            string UserName = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetGroupDetailsByUserId(UserName);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }

        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string user = Convert.ToString(Session["UserID"]);
            if (txtGroupName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Group Name.');", true);
                return;
            }
            if (ob.GroupExists4User(user, txtGroupName.Text.Trim(), ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group name already exists.');", true);
                return;
            }
            if (btnCreate.Text.Trim() == "ADD")
            {
                ob.CreateGroup(user, txtGroupName.Text.Trim());
            }
            else
            {
                ob.UpdateGroupName(user, txtGroupName.Text.Trim(), Convert.ToString(Session["GroupID"]));
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Group successfully created.');", true);
            txtGroupName.Text = "";
            GetData();
            btnCreate.Text = "ADD";
            Session["GroupID"] = "";
        }

        protected void lnkGroupNameWithCount_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label lblGroupID = (Label)row.FindControl("lblGroupID");
            string GroupID = Convert.ToString(lblGroupID.Text);
            string UserName = Convert.ToString(Session["UserID"]);
            Session["GroupID"] = GroupID;
            DataTable dt = ob.GetGroupDetailsByUserId(UserName, GroupID, "1");
            if (dt.Rows.Count > 0)
            {
                grvDistributionDetails.DataSource = null;
                grvDistributionDetails.DataSource = dt;
                grvDistributionDetails.DataBind();
                grvDistributionDetails.UseAccessibleHeader = true;
                grvDistributionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

                PanelShowDetailsGroup.Visible = true;
                ModalPopupExtenderShowDetails.Show();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No Mobile Number Found.');", true);
                return;
            }
        }

        protected void btnADD_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label lblGroupID = (Label)row.FindControl("lblGroupID");
            int rowIndex = row.RowIndex;
            string CommandName = btn.CommandName;
            string GroupID = Convert.ToString(lblGroupID.Text);
            Session["GroupID"] = GroupID;
            if (CommandName == "Add")
            {
                isUpload = false;
                divMannuel.Attributes.Add("class", "form-group row d-block;");
                divFileUpload.Attributes.Add("class", "form-group row d-none");
            }
            else if (CommandName == "Text")
            {
                isUpload = true;
                divMannuel.Attributes.Add("class", "form-group row d-none");
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
            }
            else if (CommandName == "Excel")
            {
                isUpload = true;
                divMannuel.Attributes.Add("class", "form-group row d-none");
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
            }

            // Show the modal popup
            pnlPopUp_UploadNumber.Visible = true;
            pnlPopUp_UploadNumber_Modal.Show();
        }

        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            string SheetName = "";
            DataTable dt = new DataTable();
            SheetName = "Sheet1$";
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);
            string mobLen = Convert.ToString(Session["mobLength"]);
            string minlen = Convert.ToString(Session["MobMIN"]);
            string maxlen = Convert.ToString(Session["MobMAX"]);
            string res = ob.SaveTempTable(FilePath, SheetName, user, Extension, folder, filenm, "", mobLen, minlen, maxlen);
            txtMobNum.Text = "";
            if (res.Contains("RECORDCOUNT"))
            {
                lblMobileCnt.Text = res.Replace("RECORDCOUNT", "").Trim();
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt;
                Session["MOBILECOUNT"] = lblMobileCnt.Text;
            }
            else
            {
                lblMobileCnt.Text = "";
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(user);
            }
            return res;
        }

        protected void btnUploadMobileNo_Click(object sender, EventArgs e)
        {
            string GroupID = Convert.ToString(Session["GroupID"]);
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                isUpload = true;
            }
            if (isUpload == false && txtMobNum.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Mobile Numbers.');", true);
                return;
            }
            string country_code = "";
            country_code = ddlCountry.SelectedValue;
            List<string> mobList = null;
            if (isUpload == false)
            {
                string mobile = "";
                if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
                List<string> mobList1 = mobile.Split(',').ToList();
                mobList = mobList1.Select(item => item.Trim()).ToList();
                int clength = int.Parse(Convert.ToString(database.GetScalarValue("SELECT moblength FROM tblcountry WITH(NOLOCK) WHERE counrycode=" + country_code + "")));

                mobList.RemoveAll(x => x.Length != clength);
            }
            ob.AddMobileInGroup(mobList, country_code, Convert.ToString(Session["UserID"]), "", isUpload, GroupID);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers added in Group.');", true);
            Session["XLUPLOADED"] = "";
            lblUploading.Text = "";
            txtMobNum.Text = "";
            GetData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtMobNum.Text = "";
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblGroupID = (Label)gvro.FindControl("lblGroupID");
            Label lblGroupName = (Label)gvro.FindControl("lblGroupName");
            Session["GroupID"] = lblGroupID.Text;
            txtGroupName.Text = lblGroupName.Text;
            btnCreate.Text = "Update";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblGroupID = (Label)gvro.FindControl("lblGroupID");
            string user = Convert.ToString(Session["UserID"]);
            ob.DeleteGroupByID(user, lblGroupID.Text);
            GetData();
        }

        protected void btnDw_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblgrpname = (Label)gvro.FindControl("lblgrpname");
            DataTable dt = ob.GetMobileNumbersOfGroup(Convert.ToString(Session["UserID"]), lblgrpname.Text);
            Session["MOBILEDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                Session["FILENAME"] = "MobileNumbers_" + lblgrpname.Text + ".xls";
                Session["PageName"] = "add-page.aspx";
                Response.Redirect("sms-reports_u_download.aspx", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
            }
        }

        protected void btnSMSSend_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblGroupID = (Label)gvro.FindControl("lblGroupID");
            LinkButton lnkGroupNameWithCount=(LinkButton)gvro.FindControl("lnkGroupNameWithCount");
            string GroupID = Convert.ToString(lblGroupID.Text);
            if (Convert.ToString(lnkGroupNameWithCount.Text).Contains("(0)"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No Mobile Numbers found in the GROUP.');", true);
                return;
            }
            else
            {
                Response.Redirect("send-sms-u-B4Send.aspx?Type=GroupWise&GroupID=" + GroupID);
            }
        }

        protected void btnSMSSendManuel_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblMobile = (Label)gvro.FindControl("lblMobile");
            Response.Redirect("send-sms-u-B4Send.aspx?Type=ManuelWise&MobileNo=" + Convert.ToString(lblMobile.Text));
        }

        protected void btnEditManuel_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            LinkButton btnEditManuel = (LinkButton)gvro.FindControl("btnEditManuel");
            LinkButton lnkUpdateManuel = (LinkButton)gvro.FindControl("lnkUpdateManuel");
            Label lblMobile = (Label)gvro.FindControl("lblMobile");
            TextBox txtMobile = (TextBox)gvro.FindControl("txtMobile");
            string GroupID = Convert.ToString(Session["GroupID"]);
            lblMobile.Visible = false;
            txtMobile.Visible = true;
            btnEditManuel.Visible = false;
            lnkUpdateManuel.Visible = true;
            PanelShowDetailsGroup.Visible = true;
            ModalPopupExtenderShowDetails.Show();
        }

        protected void lnkUpdateManuel_Click(object sender, EventArgs e)
        {
            string UserName = Convert.ToString(Session["UserID"]);
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            LinkButton btnEditManuel = (LinkButton)gvro.FindControl("btnEditManuel");
            LinkButton lnkUpdateManuel = (LinkButton)gvro.FindControl("lnkUpdateManuel");
            Label lblMobile = (Label)gvro.FindControl("lblMobile");
            TextBox txtMobile = (TextBox)gvro.FindControl("txtMobile");
            string GroupID = Convert.ToString(Session["GroupID"]);
            lblMobile.Visible = true;
            txtMobile.Visible = false;
            btnEditManuel.Visible = true;
            lnkUpdateManuel.Visible = false;
            database.ExecuteNonQuery("UPDATE groupdtl SET mob='" + Convert.ToString(txtMobile.Text) + "' WHERE id='" + GroupID + "' AND mob='" + Convert.ToString(lblMobile.Text) + "'");

            DataTable dt = ob.GetGroupDetailsByUserId(UserName, GroupID, "1");
            grvDistributionDetails.DataSource = null;
            grvDistributionDetails.DataSource = dt;
            grvDistributionDetails.DataBind();
            grvDistributionDetails.UseAccessibleHeader = true;
            grvDistributionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            PanelShowDetailsGroup.Visible = true;
            ModalPopupExtenderShowDetails.Show();

        }

        protected void btnDeleteManuel_Click(object sender, EventArgs e)
        {
            string UserName = Convert.ToString(Session["UserID"]);
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblMobile = (Label)gvro.FindControl("lblMobile");
            string GroupID = Convert.ToString(Session["GroupID"]);
            database.ExecuteNonQuery("DELETE FROM groupdtl WHERE id='" + GroupID + "' AND mob='" + Convert.ToString(lblMobile.Text) + "'");

            DataTable dt = ob.GetGroupDetailsByUserId(UserName, GroupID, "1");
            grvDistributionDetails.DataSource = null;
            grvDistributionDetails.DataSource = dt;
            grvDistributionDetails.DataBind();
            grvDistributionDetails.UseAccessibleHeader = true;
            grvDistributionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            PanelShowDetailsGroup.Visible = true;
            ModalPopupExtenderShowDetails.Show();

            GetData();
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            string UserName = Convert.ToString(Session["UserID"]);
            string SerachType = Convert.ToString(ddlSerachType.SelectedValue);
            string SearchText = Convert.ToString(txtSearchText.Text);
            string ActionType = "0";
            if (SerachType == "Distribution")
                ActionType = "2";
            else
                ActionType = "3";

            DataTable dt = ob.GetGroupDetailsByUserId(UserName, "0", ActionType, SearchText);
            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }

        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            List<string> selectedGroupIDs = new List<string>();
            foreach (GridViewRow row in grv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkItem = (CheckBox)row.FindControl("chkItem");
                    if (chkItem.Checked)
                    {
                        Label lblGroupID = (Label)row.FindControl("lblGroupID");
                        if (lblGroupID != null)
                        {
                            selectedGroupIDs.Add(lblGroupID.Text.Trim());
                        }
                    }
                }
            }
            if (selectedGroupIDs.Count > 0)
            {
                string groupIdsString = string.Join(",", selectedGroupIDs);
                Response.Redirect("send-sms-u-B4Send.aspx?Type=GroupWise&GroupID=" + groupIdsString);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No group selected.');", true);
                return;
            }
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            string user = Convert.ToString(Session["UserID"]);
            foreach (GridViewRow row in grv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkItem = (CheckBox)row.FindControl("chkItem");
                    if (chkItem.Checked)
                    {
                        Label lblGroupID = (Label)row.FindControl("lblGroupID");
                        if (lblGroupID != null)
                        {
                            ob.DeleteGroupByID(user, Convert.ToString(lblGroupID.Text));
                        }
                    }
                }
            }
            GetData();
        }

        protected void lnkSendAllDelete_Click(object sender, EventArgs e)
        {
            string UserName = Convert.ToString(Session["UserID"]);
            string GroupID = Convert.ToString(Session["GroupID"]);
            foreach (GridViewRow row in grvDistributionDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkItem = (CheckBox)row.FindControl("chkItem");
                    if (chkItem.Checked)
                    {
                        Label lblMobile = (Label)row.FindControl("lblMobile");
                        if (lblMobile != null)
                        {
                            database.ExecuteNonQuery("DELETE FROM groupdtl WHERE id='" + GroupID + "' AND mob='" + Convert.ToString(lblMobile.Text) + "'");
                        }
                    }
                }
            }
            
            DataTable dt = ob.GetGroupDetailsByUserId(UserName, GroupID, "1");
            grvDistributionDetails.DataSource = null;
            grvDistributionDetails.DataSource = dt;
            grvDistributionDetails.DataBind();
            grvDistributionDetails.UseAccessibleHeader = true;
            grvDistributionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            PanelShowDetailsGroup.Visible = true;
            ModalPopupExtenderShowDetails.Show();

            GetData();
        }

        protected void lnlSendAllDetails_Click(object sender, EventArgs e)
        {
            List<string> selectedMobileNos = new List<string>();
            foreach (GridViewRow row in grvDistributionDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkItem = (CheckBox)row.FindControl("chkItem");
                    if (chkItem.Checked)
                    {
                        Label lblMobile = (Label)row.FindControl("lblMobile");
                        if (lblMobile != null)
                        {
                            selectedMobileNos.Add(lblMobile.Text.Trim());
                        }
                    }
                }
            }
            if (selectedMobileNos.Count > 0)
            {
                string MobileNosString = string.Join(",", selectedMobileNos);
                Response.Redirect("send-sms-u-B4Send.aspx?Type=ManuelWise&MobileNo=" + Convert.ToString(MobileNosString));
            }
        }
    }
}
