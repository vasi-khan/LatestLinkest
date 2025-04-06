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
    public partial class BulkUrlShortner : System.Web.UI.Page
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
            if (!IsPostBack)
            {

            }

            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (FileName != "")
                {
                    Helper.Util ob = new Helper.Util();
                    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string en = Extension.ToUpper();

                    if (en.Contains("CSV"))
                        if (FileUpload1.PostedFile.ContentLength > (6 * 1024 * 1024))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of 6 MB');", true);
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
                    if (res == "LONGURL MANDATORY")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Upload File With LongURL !!');", true);
                        lblUploading.Text = "Upload Rejected.";
                        divFileLoader.Style.Add("display", "none");
                        File.Delete(FilePath);
                        return;
                    }
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


        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            string SheetName = "";
            DataTable dt = new DataTable();
            SheetName = "Sheet1$";
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string minlen = Convert.ToString(Session["MobMIN"]);
            string maxlen = Convert.ToString(Session["MobMAX"]);
            string res = ob.SaveTempTable(FilePath, SheetName, user, Extension, folder, filenm, "", mobLen, "LongURLCheck");
            if (res.Contains("RECORDCOUNT"))
            {
                string TempTable = "tmp_" + user;
                string CheckLongUrl = Convert.ToString(database.GetScalarValue(@"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + TempTable + "' and COLUMN_NAME='LongUrl'"));
                if (CheckLongUrl == "")
                {
                    res = "LONGURL MANDATORY";
                }
                else
                {
                    Session["XLUPLOADED"] = "Y";
                    Session["DTXL"] = dt;
                }
            }
            else
            {
                Session["XLUPLOADED"] = "";
                Session["DTXL"] = null;
                ob.DropUserTmpTable(user);
            }
            return res;
        }

        protected void btnShortUrl_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txtUserId.Text.Trim()) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter UserID !!');", true);
                return;
            }
            else
            {
                string UserID = Convert.ToString(database.GetScalarValue("select Username from customer with(nolock) where UserType='USER' and Username='" + txtUserId.Text.Trim() + "'"));
                if (Convert.ToString(UserID) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Valid UserID !!');", true);
                    return;
                }
            }

            if (Convert.ToString(Session["XLUPLOADED"]) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload CSV File With LongURL !!');", true);
                return;
            }
            else
            {
                try
                {

                    string tmpname = "tmp_" + user;
                    string DomainName = Convert.ToString(database.GetScalarValue("select DomainName from customer with(nolock) where UserType='USER' and Username='" + txtUserId.Text.Trim() + "'"));
                    string AlterColumn = @"alter table " + tmpname + " add ShortUrl varchar(7)";
                    //AlterColumn += "alter table " + tmpname + " drop column Column1";
                    database.ExecuteNonQuery(AlterColumn);

                    string Execute = @"UPDATE " + tmpname + " SET ShortUrl = (select RIGHT(NEWID(),7))";
                    database.ExecuteNonQuery(Execute);

                    string InsQry = @"insert into short_urls (long_url,segment,added,ip,num_of_clicks,userid,mobtrack,mainurl,domainname,richmediaurl)
                                  select LongUrl,ShortUrl,getdate(),'1.1.1.1',0,'" + txtUserId.Text.Trim() + "','N',0,'" + DomainName + "',0 from " + tmpname + "";
                    database.ExecuteNonQuery(InsQry);

                    DataTable dt = database.GetDataTable("select *,('" + DomainName + "' + shortUrl) as ShortUrls from " + tmpname + "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("ShortUrl");
                        Reset();
                        Session["MOBILEDATA"] = dt;
                        Session["FILENAME"] = "ShortUrl.xls";
                        Response.Redirect("sms-reports_u_download.aspx");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] {
                   new DataColumn("LongUrl")
                    });
                dt.Rows.Add("https://abc.com/");
                dt.Rows.Add("https://abcd.com/");
                dt.Rows.Add("https://abcde.com/");
                dt.Rows.Add("https://abcdef.com/");
                dt.Rows.Add("https://abcdefg.com/");

                Session["FILENAME"] = "SampleData.csv";
                Session["MOBILEDATA"] = dt;
                Response.Redirect("sms-reports_u_download.aspx");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected void Reset()
        {
            txtUserId.Text = "";
            lblUploading.Text = "";
        }
    }
}