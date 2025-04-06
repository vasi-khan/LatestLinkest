using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class CampaignEntry : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "")
            { 
                Response.Redirect("login.aspx");
            }


            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName).Replace("'", "");
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

                            int sizeTxt = Convert.ToInt16(ConfigurationManager.AppSettings["SIZETXT"]);
                            int sizeCSV = Convert.ToInt16(ConfigurationManager.AppSettings["SIZECSV"]);
                            int sizeXLS = Convert.ToInt16(ConfigurationManager.AppSettings["SIZEXLS"]);

                            //if (en.Contains("TXT"))
                            //    if (FileUpload1.PostedFile.ContentLength > (sizeTxt * 1024 * 1024))
                            //    {
                            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of " + Convert.ToString(sizeTxt) + " MB');", true);
                            //        lblfn.Text = "Upload rejected.";
                            //        return;
                            //    }
                            if (en.Contains("CSV"))
                                if (FileUpload1.PostedFile.ContentLength > (sizeCSV * 1024 * 1024))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of " + Convert.ToString(sizeCSV) + " MB');", true);
                                    lblfn.Text = "Upload rejected.";
                                    return;
                                }
                            //if (en.Contains("XLS"))
                            //    if (FileUpload1.PostedFile.ContentLength > (sizeXLS * 1024 * 1024))
                            //    {
                            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file size cannot be above of " + Convert.ToString(sizeXLS) + " MB');", true);
                            //        lblfn.Text = "Upload rejected.";
                            //        return;
                            //    }

                            string FolderPath = "XLSUpload/";
                            Session["UPLOADFILENM"] = FileName;
                            Session["UPLOADFILENMEXT"] = Extension;
                            string FolderPathOnly = Server.MapPath(FolderPath);
                            string FileNameOnly = user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                            Session["FileNameOnly"] = FileNameOnly;
                            string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                            FileUpload1.SaveAs(FilePath);
                            string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly);

                            if (res.Contains("OK"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                                lblfn.Text = "" + FileName + " Uploaded successfully.";
                                divFileLoader.Style.Add("display", "none");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                                lblfn.Text = "Upload Rejected.";
                                divFileLoader.Style.Add("display", "none");
                                File.Delete(FilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Helper.Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }
        }

        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm)
        {
            string conStr = "";
            string res = "";
            string SheetName = "";
            DataTable dt = new DataTable();
            if (Extension.ToLower().Contains(".xls"))
            {
                #region <Commented>
                /*
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();

                cmdExcel.Connection = connExcel;
                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();
                */

                /*
                OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
                String strExtendedProperties = String.Empty;
                sbConnection.DataSource = FilePath;
                if (Path.GetExtension(FilePath).Equals(".xls"))//for 97-03 Excel file
                {
                    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                    strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
                }
                else if (Path.GetExtension(FilePath).Equals(".xlsx"))  //for 2007 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
                }
                sbConnection.Add("Extended Properties", strExtendedProperties);
                List<string> listSheet = new List<string>();
                using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
                {
                    conn.Open();
                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    SheetName = dtSheet.Rows[0]["TABLE_NAME"].ToString();
                }
                */
                #endregion
            }
            SheetName = "Sheet1$";
            #region < Commented 2 >
            #endregion
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            Helper.Util ob = new Helper.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string minlen = Convert.ToString(Session["MobMIN"]);
            string maxlen = Convert.ToString(Session["MobMAX"]);
            DataTable dt1 = SaveTempTable(FilePath, SheetName, user, Extension, folder, filenm, "", "");

            //txtMobNum.Text = "";
            if (dt1.Rows.Count > 0)
            {
                res = "OK";
                Session["XLUPLOADED"] = "Y";
                Session["DTXL"] = dt1;
                DataTable tempdt = (DataTable)Session["DTXL"];
                SetValueInHtmlTable(tempdt);
            }
            else
            {
                dt1 = null;
                SetValueInHtmlTable(dt1);
            }
            return res;
            //ob.DropUserTmpTable(user);
        }








        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if ((Session["DTXL"]) != null)
            {
                DataTable dt = Session["DTXL"] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    string result = ob.SP_InsertFileUCampDetails();
                    if (result.Contains("Succesfully"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Details successfully Inserted !!.');", true);
                        Clear();
                        Session["DTXL"] = null;
                        SetValueInHtmlTable(null);
                        lblfn.Text = "";
                        return;
                    }
                }
               
            }



            string s1 = "";
            txtFrm.Text = hdntxtFrm.Value;
            if (txtUserid.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Userid ID !!');", true);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "loadscrq();", true);
            if (hdntxtFrm.Value == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please select Request Date !!');", true);
                return;
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (txtcampname.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter campaign name !!');", true);
                return;
            }
            if (txtsndrid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter sender Id !!');", true);
                return;
            }
            
           string res = ob.SP_InsertCampaignEntryDetails(Convert.ToString(txtUserid.Text.Trim()),Convert.ToDateTime(s1),Convert.ToString(txtcampname.Text.Trim()), Convert.ToString(txtsmscredit.Text.Trim()), Convert.ToString(txtsubmitted.Text.Trim()), Convert.ToString(txtdelivered.Text.Trim()), 
               Convert.ToString(txtdlvrper.Text.Trim()), Convert.ToString(txtfailed.Text.Trim()), Convert.ToString(txtfldper.Text.Trim()), Convert.ToString(txtawaited.Text.Trim()), Convert.ToString(txtawtdper.Text.Trim()), Convert.ToString(txthitcount.Text.Trim()),Convert.ToString(txthitcntper.Text.Trim()), Convert.ToString(txtfname.Text.Trim()), Convert.ToString(txtsndrid.Text.Trim()), Convert.ToString(txtmsg.Text.Trim()));

            if (res.Contains("Succesfully"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Details successfully Inserted !!.');", true);
                Clear();
                return;
            }
        }

        protected void Lnkbtnreset_Click(object sender, EventArgs e)
        {
            Response.Redirect("CampaignEntry.aspx");
        }

        protected void Clear()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "loadscrq();", true);
            txtcampname.Text = "";
            txtsmscredit.Text = "";
            txtsubmitted.Text = "";
            txtdelivered.Text = "";
            txtdlvrper.Text = "";
            txtfailed.Text = "";
            txtfldper.Text = "";
            txtawaited.Text = "";
            txtawtdper.Text = "";
            txthitcount.Text = "";
            txthitcntper.Text = "";
            txtfname.Text = "";
            txtsndrid.Text = "";
            txtmsg.Text = "";
            txtUserid.Text = "";
        }


        public DataTable SaveTempTable(string FilePath, string SheetName, string user, string extension, string folder, string filenm, string s = "", string moblen = "")
        {
            // bool chkUnq = isAllowDuplicate == true ? false : true;
            bool chkUnq = user.ToUpper() == "MIM2101371" ? false : false;
            string username = user;
            //if (!File.Exists(FilePath))
            //{
            //    return "There was some error on file upload. Please upload again.";
            //}
            new Util().Log("FU-user-" + user + ". File-" + FilePath);
            string user1 = "tmp1_" + user;
            user = "tmp_" + user;
            string colnm = "";
            string datatype = "";
            string sql = "";

            string ccode = Convert.ToString(database.GetScalarValue("Select defaultCountry from customer where username='" + username + "'"));
            // string moblen = (ccode == "91" ? "10" : (ccode == "971" ? "9" : "9"));

            //if (extension.ToLower().Contains("xls"))
            //{
            //    sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @" ;
            //    SELECT " + (chkUnq ? "DISTINCT" : "") + " * INTO " + user1 + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
            //    try
            //    {
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex1)
            //    {
            //        return "Invalid file format.";
            //    }
            //    colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
            //    datatype = Convert.ToString(database.GetScalarValue("select Data_Type From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
            //    if (datatype.ToLower() != "float")
            //    {
            //        Int64 cn1 = Convert.ToInt64(database.GetScalarValue("Select count(*) from " + user1 + @" where [" + colnm + "] like '%.%e+%' "));
            //        if (cn1 > 0) return "Error in file. Please check the file. ";
            //    }

            //    if (datatype.ToLower() != "float")
            //    {
            //        //sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ; SELECT distinct right([" + colnm + "],10) AS [" + colnm + "] INTO " + user + @" FROM  " + user1 + " ; ";
            //        sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
            //        SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(dbo.udf_GetNumeric([" + colnm + "]),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
            //    }
            //    else
            //    {
            //        sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + @" ;
            //        SELECT " + (chkUnq ? "DISTINCT" : "") + " CONVERT(nvarchar(255),LTRIM(RTRIM(str(isnull([" + colnm + "],0),20,0)))) AS [" + colnm + "] INTO " + user + @" FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=" + FilePath + ";HDR=Yes;IMEX=1','Select * from [" + SheetName + "]') ; ";
            //    }

            //    try
            //    {
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2)
            //    {
            //        return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            //    }

            //}



            //else if (extension.ToLower().Contains("txt"))
            //{
            //    #endregion
            //    try
            //    {
            //        DataTable dt = ReadTextFile(FilePath, moblen);

            //        if (dt.Rows.Count == 0)
            //            return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            //        else
            //        {
            //            sql = string.Format("if exists (select * from sys.tables where name = '{0}') drop table {1}", user, user);
            //            sql += string.Format(" Create table {0} (MobNo varchar(15) )", user);

            //            database.ExecuteNonQuery(sql);
            //            database.BulkInsertData(dt, user); colnm = "MobNo";

            //        }

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}


            //rabi 29/07/21
            DataTable dt = new DataTable();
            if (extension.ToLower().Contains("csv"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();

                    dt = ReadCSVNew(FilePath, moblen);
                    if (dt.Rows.Count > 0) 
                    {
                        sql = @"if exists (select * from sys.tables where name='" + user + @"') drop table " + user + "";
                        string sql1 = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + "";

                        sb.Append(@"Create table " + user + "  (");
                        sb1.Append(@"Create table " + user1 + "  (");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            
                            if (i != dt.Columns.Count - 1)
                            {
                                if (dt.Columns[i].ColumnName.Contains("requestdate"))
                                {
                                    sb.Append("[" + dt.Columns[i] + "] Date,");
                                    sb1.Append("[" + dt.Columns[i] + "] Date,");
                                }
                                else
                                {
                                    sb.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                                    sb1.Append("[" + dt.Columns[i] + "] nvarchar (max),");
                                }
                                
                            }
                            else
                            {
                                if (dt.Columns[i].ColumnName.Contains("requestdate"))
                                {
                                    sb.Append("[" + dt.Columns[i] + "] Date,");
                                    sb1.Append("[" + dt.Columns[i] + "] Date,");
                                }
                                else
                                {
                                sb.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                                sb1.Append("[" + dt.Columns[i] + "] nvarchar(max) ");
                                }
                            }
                        }
                        sb.Append(")");
                        sb1.Append(")");

                        database.ExecuteNonQuery(sql + "  " + sb.ToString() + " " + sql1 + " " + sb1.ToString());
                        database.BulkInsertDataDynamic(dt, user1);
                        database.BulkInsertDataDynamic(dt, user);
                        colnm = Convert.ToString(database.GetScalarValue("select column_name From information_schema.columns where table_name = '" + user1 + @"' and ordinal_position = 1 "));
                        
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex.Message + ");", true);
                }
            }

            return dt;
            //database.ExecuteNonQuery("delete from " + user + @"   where len([" + colnm + "]) < " + moblen);
            //database.ExecuteNonQuery("update " + user + @" set [" + colnm + "] = right([" + colnm + "], " + moblen + ") where len([" + colnm + "]) > " + moblen);

            //database.ExecuteNonQuery("delete d from " + user + @" d inner join globalBlackListNo b on b.mobile=d.[" + colnm + "] ");
            ///* rabi 15 july 21*/
            //database.ExecuteNonQuery(" if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where userid='" + username + "' AND TYPE='U') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE UserId='" + username + "' AND TYPE='U' if exists(select * from smsrestrictmobile srm join [smsrestriction] sr on srm.smsrestrictionid=sr.id where SenderId='" + s + "' AND TYPE='S') delete d from " + user + @" d inner join SMSRestrictmobile SRM on SRM.MobileNo='91'+d.[" + colnm + "]  join [SMSRestriction] SR on SRM.SMSRestrictioniD=SR.Id  WHERE SenderId='" + s + "' AND TYPE='S'");

            ////check column name is numeric or not
            //sql = "select ISNUMERIC(column_name) From information_schema.columns where table_name = '" + user + "' and ordinal_position = 1";
            //Int32 x = Convert.ToInt16(database.GetScalarValue(sql));
            //if (x == 1) return "Column name is numeric. Cannot upload file.";

            //try
            //{
            //    sql = "select convert(numeric,[" + colnm + "]) from " + user;
            //    DataTable dt = database.GetDataTable(sql);
            //}
            //catch (Exception ex)
            //{
            //    return "Mobile Numbers in the file are not Numeric. Please check the file. ";
            //}

            //// CHECK FOR ALL NULL VALUES
            //sql = "select count(distinct [" + colnm + "]) from " + user + " where [" + colnm + "] is not null ";
            //Int32 Y = Convert.ToInt32(database.GetScalarValue(sql));
            //if (Y <= 0) return "No Mobile Numbers found in the file";
            //return "RECORDCOUNT " + Y.ToString();
        }


        public DataTable ReadCSVNew(string path, string moblen)
        {
            try
            {
                DataTable dt = new DataTable();

                StreamReader sr = new StreamReader(path);

                string[] Headers = sr.ReadLine().Split(',');
                foreach (string header in Headers)
                {
                    
                        dt.Columns.Add(header);
                   
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < Headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                sr.Dispose();
                sr.Close();

                
dt.Columns[0].ColumnName = "Userid";
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public void SetValueInHtmlTable(DataTable dt)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table id=\"table\" class=\"table table-striped border\" data-locale=\"en-US\" data-toggle=\"table\" data=\"\" -=\"\" toolbar=\"#toolbar\" search=\"true\" filter=\"\" control=\"true\" show=\"\" columns=\"true\" click=\"\" to=\"\" select=\"true\" minimum=\"\" count=\"\" pagination=\"true\" field=\"id\" buttons=\"\" -class=\"light\" data-buttons=\"btn btn-sm\" data-pagination-pre-text=\"Previous\" data-pagination-next-text=\"Next\" data-page-list=\"[10, 25, 50, 100, all]\" data-show-footer=\"false\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr>");
            htmlTable.Append("<th data-filter-control =\"input\" data-sortable=\"true\" style=\"width:4%; \" rowspan=\"2\">Sr. No</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">User Id</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8% !important;\" rowspan=\"2\">Request <br>Date</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:10%;\" rowspan=\"2\">Campaign /<br> File Name</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:8%;\" rowspan=\"2\">SMS Credit</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:8%;\" rowspan=\"2\">Submited</th> ");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Delivered</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Failed</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Awaited</th>");
            htmlTable.Append("<th data-filter-control=\"input\" data-sortable=\"true\" style=\"width:7%;\" colspan=\"2\">Hit Count</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" style = \"width:7%;\" rowspan=\"2\">Sender ID</th>");
            htmlTable.Append("<th data-filter-control = \"input\" data-sortable = \"true\" rowspan = \"2\" > SMS Text </th>");
            htmlTable.Append("</tr>");
            htmlTable.Append("<tr>");
            htmlTable.Append("<th> Numbers </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("<th> Number </th><th>%</th>");
            htmlTable.Append("</tr>");
            htmlTable.Append("</thead>");
            htmlTable.Append("<tbody>");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {


                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        htmlTable.Append("<tr class=\"tr-1\">");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + Convert.ToString(j + 1) + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["userid"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["requestdate"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["campaign"] + "<br>" + dt.Rows[j]["FileNm"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["credit"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["smscount"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["delivered_p"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["failed_p"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["AWAITED_p"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["openmsg_p"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["sender"] + "</td>");
                        htmlTable.Append("<td data-target=\"#collapseContent" + Convert.ToString(j + 1) + "\" data-toggle=\"collapse\" data-group-id=\"grandparent\" data-role=\"expander\">" + dt.Rows[j]["message"] + "</td>");
                        htmlTable.Append("</tr>");

                        //DataView dataView = dt.DefaultView;
                        //dataView.RowFilter = "campaign = '" + dt.Rows[j]["campaign"] + "' AND requestdate='" + dt.Rows[j]["requestdate"] + "'";
                        //string subHtml = ConvertDataTableToHTML(dataView.ToTable());
                        //htmlTable.Append("<tr class=\"collapse\" id=\"collapseContent" + Convert.ToString(j + 1) + "\" aria-expanded=\"true\">");
                        //htmlTable.Append("<td class=\"inside-table\" colspan=\"15\">" + subHtml + "</td>"); 
                        // htmlTable.Append("</tr>");

                    }
                    htmlTable.Append("</tbody>");
                    htmlTable.Append("</table>");
                    divResult.InnerHtml = htmlTable.ToString();
                }
            }
            else
            {
                htmlTable.Append("<tr class=\"tr-1\"><td colspan='16'>No Record found</td></tr>");
                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                divResult.InnerHtml = htmlTable.ToString();
            }
        }
    }
}