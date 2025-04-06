using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class BlackListNo : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;

            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            if (usertype == "SYSADMIN")
            {
                divFileUpload.Attributes.Add("class", "form-group row d-block");
            }

            if (!IsPostBack )
            {
                filloperator();
            }

        }
        public void filloperator()
        {
           
            ddlOperator.DataSource = ob.GetOperator();
            ddlOperator.DataTextField = "provider";
            ddlOperator.DataValueField = "provider";
            ddlOperator.DataBind();
            ddlOperator.Items.Insert(0, new ListItem("-- select --", "0"));
        }

        public void Save()
        {
            string Date = "", Oprerator = "";
            //if (ddlOperator.SelectedValue=="0")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select any operator');", true);
            //    ddlOperator.Focus();
            //    return;
            //}
            //if (txtDate.Text.Trim()=="")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter date');", true);
            //    txtDate.Focus();
            //    return;
            //}
            if (rdbUserIdBase.Checked && txtUser.Text.Trim()=="")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter user id');", true);
                txtUser.Focus();
                return;
            }
            if (rdbUpload.Checked)
            {
                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        Oprerator = ddlOperator.SelectedValue;
                        Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //Date = Convert.ToDateTime(hdntxtDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string en = Extension.ToUpper();

                        if (en.Contains("TXT"))
                            if (FileUpload1.PostedFile.ContentLength > (12 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 6 MB');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }
                        if (en.Contains("XLS"))
                            if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of 6 MB');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }
                        if (en.Contains("CSV"))
                            if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of 6 MB');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }

                        // System.Threading.Thread.Sleep(99999999);
                        string FolderPath = "XLSUpload/";
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = DateTime.Now.ToString("yyyyMMddhhmmssfff") + Extension;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        FileUpload1.SaveAs(FilePath);
                        string res = "";
                        if (rdbUserIdBase.Checked)
                        {
                            res = ob.SaveBlackListMobileNo(FilePath, Oprerator, Date, txtUser.Text.Trim(), en); //As Per User
                        }
                        else
                        {
                            res = ob.SaveBlackListMobileNo(FilePath, Oprerator, Date, en);
                        }
                        if (res.Contains("RECORDCOUNT"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                            lblUploading.Text = "Uploaded successfully.";
                            divFileLoader.Style.Add("display", "none");
                            reset();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                            lblUploading.Text = "Upload Rejected.";
                            divFileLoader.Style.Add("display", "none");
                            File.Delete(FilePath);
                        }
                    }
                }
            }
            else
            {
                Oprerator = ddlOperator.SelectedValue;
                Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //Date = Convert.ToDateTime(hdntxtDate.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string mobile = "";
                if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
                List<string> mobList1 = mobile.Split(',').ToList();
                List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
                int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
                if (mobile.Trim() != "")
                {
                    if (mobList.Count > 25000)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please use file upload option to send SMS to more than 25000 mobile numbers.');", true);
                        return;
                    }
                    string res = "";
                    if (rdbUserIdBase.Checked)
                    {
                        res = ob.SaveBlackListMobileNoEntry(mobList, Oprerator, Date, txtUser.Text.Trim());
                    }
                    else
                    {
                        res = ob.SaveBlackListMobileNoEntry(mobList, Oprerator, Date);
                    }
                    if (res.Contains("RECORDCOUNT"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Saved Successfully');", true);
                        reset();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                    }
                }
                else
                {
                    if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers first.');", true);
                        return;
                    }
                }
            }
      
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        public void reset()
        {
            txtUser.Text = "";
            txtDate.Text = "";
            ddlOperator.SelectedValue = "0";
            txtMobNum.Text = "";
            chkAsperUser.Checked = false;
        }

        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEntry.Checked)
            {
                divFileUpload.Visible = false;
                divNum.Visible = true;
            }
            else
            {
                divFileUpload.Visible = true;
                divNum.Visible = false;
            }
        }

        protected void rdbEntry_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEntry.Checked)
            {
                divFileUpload.Visible = false;
                divNum.Visible = true;
            }
            else
            {
                divFileUpload.Visible = true;
                divNum.Visible = false;
            }
        }

        protected void rdbUserIdBase_CheckedChanged(object sender, EventArgs e)
        {
            userdiv.Visible = true;
            spantextnum.Visible = true;
            spantextglobal.Visible = false;
            ptextnum.Visible = true;
            ptextglobal.Visible = false;
        }

        protected void rdbGlobal_CheckedChanged(object sender, EventArgs e)
        {
            userdiv.Visible = false;
            spantextnum.Visible = false;
            spantextglobal.Visible = true;
            ptextnum.Visible = false;
            ptextglobal.Visible = true;
        }
    }
}