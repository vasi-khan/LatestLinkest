using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class UploadSIDforOperatorAccount : System.Web.UI.Page
    {
        string user = "";
        Util obj = new Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                ddlPanelProviderbind();
                ddlSystemIdBind();
                DataTable dt = obj.GetOperatorSenderId();
                grd.DataSource = dt;
                grd.DataBind();
                GridFormat(dt);
            }
        }

        public void ddlPanelProviderbind()
        {
            DataTable dt = obj.GetProvider("API");
            ddlProvider.DataSource = dt;
            ddlProvider.DataTextField = "Provider";
            ddlProvider.DataValueField = "Provider";
            ddlProvider.DataBind();
            ListItem objlst = new ListItem("--Select--", "0");
            ddlProvider.Items.Insert(0, objlst);
        }

        public void ddlSystemIdBind()
        {
            DataTable dt = obj.GetSystemId();
            ddlSystemId.DataSource = dt;
            ddlSystemId.DataTextField = "SystemId";
            ddlSystemId.DataValueField = "smppaccountid";
            ddlSystemId.DataBind();
            ListItem objlst = new ListItem("--Select--", "0");
            ddlSystemId.Items.Insert(0, objlst);
        }

        protected void btnUploadSenderId_Click(object sender, EventArgs e)
        {
            if (IsPostBack && FileUploadSenderID.PostedFile != null)
            {
                string Extension = Path.GetExtension(FileUploadSenderID.PostedFile.FileName);
                string en = Extension.ToUpper();
                string FileName = Path.GetFileName(FileUploadSenderID.PostedFile.FileName).Replace("'", "");
                if (FileName != "")
                {
                    if (en.Contains("TXT"))
                        if (FileUploadSenderID.PostedFile.ContentLength > (10 * 1024 * 1024))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of " + Convert.ToString("10") + " MB');", true);
                            lblUploading.Text = "Upload rejected.";
                            return;
                        }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload File !! ');", true);
                    lblUploading.Text = "Upload rejected.";
                    return;
                }
                Random rn = new Random();
                int no = rn.Next(1, 999);
                string txtname = "tmp_" + no.ToString() + DateTime.Now.ToString("_yyyyMMddhhmmssfff");
                string FolderPath = "FileUpload/";
                string FolderPathOnly = Server.MapPath(FolderPath);
                string FileNameOnly = txtname + Extension;
                string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                FileUploadSenderID.SaveAs(FilePath);
                DataTable dt = ReadTextFile(FilePath);
                Session["SID"] = dt;
                lblUploading.Text = FileName;

            }
        }
        public DataTable ReadTextFile(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SID");
            List<string> lines = File.ReadAllLines(path).ToList();
            lines.ForEach((item) => dt.Rows.Add(item));
            return dt;
        }

        protected void lnkinfo_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string Provider = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_Provider") as HiddenField).Value);
            //string SystemId = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_SystemId") as HiddenField).Value);
            //string DateTime = Convert.ToString((grd.Rows[IndexNo].FindControl("HD_InsertDateTime") as HiddenField).Value);
            //DateTime date = Convert.ToDateTime(DateTime);
            //string ndate = date.ToString("yyyy-MM-dd HH:mm");
            Session["Provider"] = Provider;
            //Session["SystemId"] = SystemId;
            //Session["ndate"] = ndate;
            bindPopup(Provider);//, ndate
        }
        public void bindPopup(string Provider, string SenderID="")//string ndate, 
        {
            DataTable dt = new DataTable();
            if (SenderID == "")
            {
                dt = obj.GetOperatorSenderID(Provider);
            }
            else
            {
                dt = obj.GetOperatorSenderIDFilter(Provider, SenderID);
            }
            if (dt.Rows.Count > 0)
            {
                lblProvider.Text = dt.Rows[0]["Provider"].ToString();
                lblSystemId.Text = dt.Rows[0]["SystemId"].ToString();
            }
            GridV.DataSource = dt;
            GridV.DataBind();
            GridFormat1(dt);
            ModalPopitm.Show();
        }
       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblUploading.Text == "Upload rejected.")
            {
                Session["SID"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload Sender Id !!');", true);
                return;
            }
            if (ddlProvider.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Provider !!');", true);
                ddlProvider.Focus();
                return;
            }
            if (ddlSystemId.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select SystemId !!');", true);
                ddlSystemId.Focus();
                return;
            }
            if (Session["SID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('please Upload Sender Id !!');", true);
                FileUploadSenderID.Focus();
                return;
            }
            DataTable dt = Session["SID"] as DataTable;
            string Msg = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Msg = obj.InsertOperatorSenderId(ddlProvider.SelectedItem.Text, ddlSystemId.SelectedItem.Text, dt.Rows[i]["SID"].ToString());
            }
            if(Msg=="")
            {
                Session["SID"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error Data Not Saved !!');", true);
            }
            else
            {
                Session["SID"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('"+Msg+ "');window.location ='UploadSIDforOperatorAccount.aspx';", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadSIDforOperatorAccount.aspx");
        }
        protected void GridFormat(DataTable dt)
        {
            grd.UseAccessibleHeader = true;
            grd.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grd.TopPagerRow != null)
            {
                grd.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grd.BottomPagerRow != null)
            {
                grd.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grd.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void GridFormat1(DataTable dt)
        {
            GridV.UseAccessibleHeader = true;
            GridV.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (GridV.TopPagerRow != null)
            {   
                GridV.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }   
            if (GridV.BottomPagerRow != null)
            {   
                GridV.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                GridV.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as CheckBox).Parent.Parent;
            int IndexNo = gvr.RowIndex;
            string ID = Convert.ToString((GridV.Rows[IndexNo].FindControl("HD_ID") as HiddenField).Value);
            string Active = obj.CheckActive(ID);
            Session["ID"] = ID;
            Session["Active"] = Active;
            if (Active == "False")
            {
                lblStatus.Text = "Are You Sure You Want to Active this SenderId !";
            }
            else
            {
                lblStatus.Text = "Are You Sure You Want to InActive this SenderId !";
            }
            mp1.Show();
        }
        public void ChangeSenderIdStatus(string Active)
        {
            string ID = Session["ID"].ToString();
            if (Active == "False")
            {
                obj.Active(ID);
            }
            else
            {
                obj.Inactive(ID);
            }
            DataTable dt = obj.GetOperatorSenderId();
            grd.DataSource = dt;
            grd.DataBind();
            GridFormat(dt);
            bindPopup(Session["Provider"].ToString());
            ModalPopitm.Show();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ModalPopitm.Hide();
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            string Active= Session["Active"].ToString();
            ChangeSenderIdStatus(Active);
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            mp1.Hide();
            bindPopup(Session["Provider"].ToString());
            ModalPopitm.Show();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            bindPopup(Session["Provider"].ToString(), txtSender.Text.Trim().ToString());
            txtSender.Text = "";
            ModalPopitm.Show();
        }
    }
}