using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class add_phone_number : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            //FilteredTextBoxExtender1.ValidChars = FilteredTextBoxExtender1.ValidChars + "\r\n";
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                //  if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971")
                //txtMobNum.Attributes.Add("maxlength", txtMobNum.MaxLength.ToString());
                PopulateCountry();
                ddlCountry.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                populateGroup();
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
                            //lblUploading.Text = "Uploaded successfully.";
                            lblUploading.Text = FileUpload1.FileName;
                            lblUploading.ForeColor = System.Drawing.Color.Green;
                            divFileLoader.Style.Add("display", "none");
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

        }

        public void StartProcess()
        {
            Session["XLUPLOADED"] = null;
            Session["MOBILECOUNT"] = null;
            Helper.Util ob = new Helper.Util();
            ob.DropUserTmpTable(user);
            lblUploading.Text = "";
        }
        public void populateGroup()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetGroup(Convert.ToString(Session["UserID"]));

            ddlGrp.DataSource = dt;
            ddlGrp.DataTextField = "grpname";
            ddlGrp.DataValueField = "grpname";
            ddlGrp.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlGrp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlGrp.SelectedIndex = 1;
            else
                ddlGrp.SelectedIndex = 0;
        }

        public void PopulateCountry()
        {
            DataTable dt = ob.GetActiveCountry(Convert.ToString(Session["UserID"]));
            ddlCountry.DataSource = dt;
            ddlCountry.DataTextField = "name";
            ddlCountry.DataValueField = "countrycode";
            ddlCountry.DataBind();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isUpload = rdbUpload.Checked;

            if (isUpload == false && txtMobNum.Text.Trim().Length < 10)
            {
              //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ] or [ 12 digits (with 91) ].');", true);
               // return;
            }
            if (ddlGrp.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a Group.');", true);
                return;
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
                //string[] mo;
                //List<Mobile> mobList = new List<Mobile>();
                List<string> mobList1 = mobile.Split(',').ToList();
                mobList = mobList1.Select(item => item.Trim()).ToList();
                int clength = int.Parse(Convert.ToString(database.GetScalarValue("select moblength from tblcountry where counrycode=" + country_code + "")));

                mobList.RemoveAll(x => x.Length != clength);

                //if (mobile.Trim() != "")
                //{                   
                    //int maxlen = mobList.Max(arr => arr.Length);
                    //int minlen = mobList.Min(arr => arr.Length);
                    //if (maxlen != minlen)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ] or [ 12 digits (with 91) ].');", true);
                    //    return;
                    //}
                    //if (maxlen == 11)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ] or [ 12 digits (with 91) ].');", true);
                    //    return;
                    //}
                    //if (maxlen == 10) country_code = "";
                //}
            }
            ob.AddMobileInGroup(mobList, country_code, Convert.ToString(Session["UserID"]), ddlGrp.SelectedValue, isUpload);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers added in Group.');", true);
            lblUploading.Text = "";
            ddlGrp.SelectedValue = "0";
            txtMobNum.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlGrp.SelectedValue = "0";
            txtMobNum.Text = "";
        }

        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            // string conStr = "";
            string SheetName = "";
            DataTable dt = new DataTable();

            SheetName = "Sheet1$";

            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string minlen = Convert.ToString(Session["MobMIN"]);
            string maxlen = Convert.ToString(Session["MobMAX"]);
            string res = ob.SaveTempTable(FilePath, SheetName, user, Extension, folder, filenm, "", mobLen,minlen,maxlen); 
            txtMobNum.Text = "";
            //string input = res;
            //string Output = res.Split().Last().Trim();
            //lblcount.Text = Convert.ToString(Output);
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


        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            StartProcess();
            lblMobileCnt.Text = "";
            txtMobNum.Text = "";
            // divNum.Attributes.Add("style", "pointer-events:none;");
            divFileUpload.Attributes.Add("class", "form-group row d-none");
            //if (rdbEntry.Checked)
            //    divNum.Attributes.Add("style", "pointer-events:all;");

            if (rdbUpload.Checked)
            {
                divFileUpload.Attributes.Add("class", "form-group row d-block;");
            }

        }
    }
}