using eMIMPanel.rcscode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class SendRcs : System.Web.UI.Page
    {
        string _user = "";
        string paisa = "";
        string UploadPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            UploadPath = Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]);
            paisa = Convert.ToString(Session["SUBCURRENCY"]);
            Session["RCSUserID"] = Convert.ToString(rcscode.database.GetScalarValue("select top 1 RCSACCID  from MapSMSAcc where smsAccId='" + Convert.ToString(Session["UserID"]) + "'"));

            _user = Convert.ToString(Session["RCSUserID"]);
            if (_user == "") Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                txtMsg.Attributes.Add("maxlength", txtMsg.MaxLength.ToString());
                Util ob = new Util();
                PopulateCountry();
                ddlRCSType_SelectedIndexChanged(sender, e);

                ob.DropUserTmpTable(_user);

                Helper.Global.templateErrorCode = "";
                Helper.Global.Istemplatetest = true;
                Helper.Global.openTempAc = ob.IsOpenTempAc(Convert.ToString(Session["RCSUserID"]));


                ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
                if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) != "971") lblOptOut.Text = "";
                ViewState["TemplateID"] = null;
                ViewState["TemplateFields"] = null;
                ViewState["dtMaxLen"] = null;
                ViewState["dtTopRec"] = null;
                Session["NOOFCHARINMSG"] = null;
                rdbEntry.Checked = true;
                ViewState["SMSRATE"] = Session["RATE_NORMALSMS"];
                //lblRate.Text = Convert.ToString(Session["RATE_NORMALSMS"]) + " " + paisa + " per SMS";              

                PopulateSender();
                PopulateTemplateID(1);
                StartProcess();
                divOptOut.Attributes.Add("Style", "display:none;");
                HideTemplateIdForeignAcc();



            }

            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (FileName != "")
                    {

                        rcscode.Util ob = new rcscode.Util();
                        //if (ob.FileUploadStopped())
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File upload is temporarily stopped.');", true);
                        //}
                        //else
                        //{
                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string en = Extension.ToUpper();

                        if (en.Contains("TXT"))
                            if (FileUpload1.PostedFile.ContentLength > (12 * 1024 * 1024))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text file size cannot be above of 6 MB');", true);
                                lblUploading.Text = "Upload rejected.";
                                return;
                            }
                        if (en.Contains("CSV"))
                            if (FileUpload1.PostedFile.ContentLength > (20971520))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CSV file size cannot be above of 20 MB');", true);
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


                        string FolderPath = "Uploads/Files/";
                        Session["UPLOADFILENM"] = FileName;
                        Session["UPLOADFILENMEXT"] = Extension;
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                        Session["FileNameOnly"] = FileNameOnly;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        FileUpload1.SaveAs(FilePath);
                        string namewithoutextenrion = Path.GetFileNameWithoutExtension(FileNameOnly);
                        // string fileUname = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff");
                        string fileUname = namewithoutextenrion;
                        Session["FileUName"] = fileUname;

                        string res = Import_To_Grid(FilePath, Extension, "Yes", FolderPathOnly, FileNameOnly, fileUname);


                        if (res.Contains("RECORDCOUNT"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                            lblUploading.Text = "" + FileName + " Uploaded successfully.";
                            divFileLoader.Style.Add("display", "none");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                            lblUploading.Text = "Upload Rejected.";
                            divFileLoader.Style.Add("display", "none");
                            File.Delete(FilePath);
                        }
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }

            try
            {
                if (IsPostBack && uImage.PostedFile != null)
                {

                    string FileName = Path.GetFileName(uImage.PostedFile.FileName);
                    if (FileName != "")
                    {
                        ddlRCSType_SelectedIndexChanged(sender, e);
                        rcscode.Util ob = new rcscode.Util();

                        string Extension = Path.GetExtension(uImage.PostedFile.FileName);
                        string en = Extension.ToUpper();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(uImage.PostedFile.InputStream);
                        int height = img.Height;
                        int width = img.Width;
                        // int size = uImage.PostedFile.ContentLength;
                        decimal size = Math.Round(((decimal)uImage.PostedFile.ContentLength / (decimal)1024), 2);
                        string dimension = width.ToString() + "*" + height.ToString();
                        if (size > 2048)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insert image with dimension 1440*720 and size less then 2 mb.');", true);

                            return;
                        }

                        string FolderPath = "Uploads/Images/";
                        Session["ImageName"] = FileName;
                        Session["imageExtension"] = Extension;
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                        Session["imageNameOnly"] = FileNameOnly;
                        Session["imageUrl"] = UploadPath + FolderPath + FileNameOnly;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        uImage.SaveAs(FilePath);
                        string namewithoutextenrion = Path.GetFileNameWithoutExtension(FileNameOnly);

                        string fileUname = namewithoutextenrion;
                        Session["imageUName"] = FilePath;

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                        lblImage.Text = "" + FileName + " Uploaded successfully.";

                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }

            try
            {
                if (IsPostBack && uVideo.PostedFile != null)
                {
                    string FileName = Path.GetFileName(uVideo.PostedFile.FileName);
                    if (FileName != "")
                    {
                        ddlRCSType_SelectedIndexChanged(sender, e);
                        rcscode.Util ob = new rcscode.Util();

                        string Extension = Path.GetExtension(uVideo.PostedFile.FileName);
                        string en = Extension.ToUpper();

                        decimal size = Math.Round(((decimal)uVideo.PostedFile.ContentLength / (decimal)1024), 2);

                        if (size > 10240)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Upload video less then 10 mb.');", true);

                            return;
                        }



                        string FolderPath = "Uploads/Videos/";
                        Session["VideoName"] = FileName;
                        Session["VideoExtension"] = Extension;
                        string FolderPathOnly = Server.MapPath(FolderPath);
                        string FileNameOnly = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff") + Extension;
                        Session["VideoNameOnly"] = FileNameOnly;
                        Session["VideoUrl"] = UploadPath + FolderPath + FileNameOnly;
                        string FilePath = Server.MapPath(FolderPath + FileNameOnly);
                        uVideo.SaveAs(FilePath);
                        string namewithoutextenrion = Path.GetFileNameWithoutExtension(FileNameOnly);
                        string fileUname = namewithoutextenrion;
                        Session["VideoUName"] = FilePath;

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + FileName + " Uploaded Successfully');", true);
                        lblVideo.Text = FileName;
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                new Util().LogError("Fail in File Uploading!  ", ex.Message + " - " + ex.StackTrace);
                throw ex;
            }


            if (txtMsg.Text.Trim() != "")
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "smscnt", "smscnt()", true);
            if (txtsms.Text.Trim() != "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "smscnt1", "smscnt1()", true);
                //ddlsmsTemplateId_SelectedIndexChanged(sender, e);
                txtsms.Attributes["readonly"] = "readonly";
            }
        }
        public string Import_To_Grid(string FilePath, string Extension, string isHDR, string folder, string filenm, string fileUname)
        {
            //if (ddlSender.SelectedValue == "0")   /* rabi 15 july 21*/
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);

            //    ddlSender.Focus();
            //    return "";
            //}
            string conStr = "";
            string SheetName = "";
            DataTable dt = new DataTable();
            if (Extension.ToLower().Contains(".xls"))
            {
                #region <Commented>

                #endregion
            }
            SheetName = "Sheet1$";
            #region < Commented 2 >
            //Read Data from First Sheet
            //connExcel.Open();
            //cmdExcel.CommandText = "SELECT * From [" + SheetName + "] ";
            //oda.SelectCommand = cmdExcel;
            //oda.Fill(dt);
            //connExcel.Close();
            //lblMobileCnt.Text = dt.Rows.Count.ToString();
            #endregion
            int MAXXLRECORD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXXLRECORD"]);

            rcscode.Util ob = new rcscode.Util();
            string mobLen = Convert.ToString(Session["mobLength"]);
            string res = ob.SaveTempTable(FilePath, SheetName, _user, Extension, folder, filenm, ddlSender.SelectedValue, mobLen, fileUname);

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
                ob.DropUserTmpTable(_user);
            }
            return res;
        }

        protected void ddlSender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971")
            {
                if (ddlSender.SelectedValue.Contains("AD-"))
                {
                    chkOptOut.Checked = true;
                    chkOptOut.Enabled = false;
                }
                else
                {
                    chkOptOut.Checked = false;
                    chkOptOut.Enabled = true;
                }
            }
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {

            rcscode.Util ob = new rcscode.Util();
            DataTable DTSMPPAC = ob.GetUserSMPPACCOUNTCountry(Convert.ToString(Session["RCSUserID"]), ddlCCode.SelectedValue);
            Session["DTSMPPAC"] = DTSMPPAC;

            bool res;

            //if (ddlTemplate.SelectedValue != "0" && txtPreview.Text != "")
            //    res = SendTemplateMSG("");
            //else
            res = MsgSend("");
        }
        public bool MsgSend(string sch)
        {
            txtMsg.Text = txtMsg.Text.Replace("\r\n", "\n");
            rcscode.Util ob = new rcscode.Util();
            string smstext = "";
            // Rachit 14 july 
            #region Country Block 
            //string _smstype = ddlSMSType.SelectedValue;
            DataTable dtCountry = ob.GetCountryTimeZone(Convert.ToString(Session["DEFAULTCOUNTRYCODE"]));
            string fCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsFromTime"]) : "";
            string tCtry = dtCountry.Rows.Count > 0 ? Convert.ToString(dtCountry.Rows[0]["smsToTime"]) : "";


            #endregion
            //---------------------------------END ----------------------------//
            string tmpfilenm = "";
            string filenm = "";
            string filenmext = "";
            int check;
            if (cbfailover.Checked == true)
            {
                check = 1;
            }
            else
            {
                check = 0;
            }

            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                filenm = Convert.ToString(Session["UPLOADFILENM"]);
                filenmext = Convert.ToString(Session["UPLOADFILENMEXT"]);
                tmpfilenm = Convert.ToString(Session["FileUName"]);

                txtMobNum.Text = "";

                if (rdbUpload.Checked == true)
                {
                    if (txtCampNm.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Campaign Name');", true);
                        return false;
                    }
                    if (ob.CampaignExistsForDay(_user, txtCampNm.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Campaign Name already exists for the day. Please Enter another campaign name.');", true);
                        return false;
                    }
                }

            }

            //if (ddlSender.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select sender id');", true);
            //    return false;
            //}


            if (chkAllowDuplicates.Checked == false && Convert.ToString(Session["XLUPLOADED"]) == "Y")
                ob.RemoveDuplicateRowsFromTempTable(_user, Convert.ToString(Session["FileUName"].ToString()));


            DataTable dt2 = ob.GetUserParameter(_user);
            string bal2 = dt2.Rows[0]["RCSbalance"].ToString();

            string country_code = ddlCCode.SelectedValue;
            string mobile = "";
            if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();


            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
            if (mobile.Trim() != "")
            {
                if (mobList.Count > 25000)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please use file upload option to send SMS to more than 25000 mobile numbers.');", true);
                    return false;
                }

                if (country_code == "91")
                {
                    int maxlen = mobList.Max(arr => arr.Length);
                    int minlen = mobList.Min(arr => arr.Length);
                    if (maxlen != minlen)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                    if (maxlen != 10 || minlen != 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ]');", true);
                        return false;
                    }
                }

                //  if (maxlen == 10) country_code = "91";
            }
            else
            {
                if (Session["MOBILECOUNT"] == null && Session["XLUPLOADED"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers first.');", true);
                    return false;
                }
            }
            if (cbfailover.Checked == true)
            {
                if (ddlsmsTemplateId.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please SMS Template ID !!!');", true);
                    return false;
                }
                if (txtsms.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill SMS Text !!!');", true);
                    return false;
                }


                int countS = txtsms.Text.IndexOf("{");
                if (countS > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter value of all variables !!!');", true);
                    return false;
                }
                string[] stringseprator = new string[] { "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}", "{11}", "{12}", "{13}", "{14}", "{15}" };
                string[] str1 = txtsms.Text.Split(stringseprator, StringSplitOptions.None);
                foreach (string str in str1)
                {
                    smstext += str;
                }
                if (smstext == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill SMS Text !!!');", true);
                    return false;
                }
            }
            string UserID = Convert.ToString(Session["RCSUserID"]);



            Int32 cnt = 0;
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
                cnt = Convert.ToInt32(Session["MOBILECOUNT"]);
            else
                cnt = mobList.Count;

            int noofsms = 0;
            int noofsms1S = 0;
            bool ucs2 = false;
            //txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            if (chkOptOut.Checked) txtMsg.Text = txtMsg.Text.Trim() + " " + lblOptOut.Text;
            txtMsg.Text = txtMsg.Text.Trim();
            string q = txtMsg.Text.Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtMsg.Text.Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~');
            qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{');
            qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}');
            qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '[');
            qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']');
            qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^');
            qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\');
            qlen = qlen + count_s6;



            ucs2 = false;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 1024) noofsms = 2;
            if (qlen > 2048) noofsms = 3;
            if (qlen > 3072) noofsms = 4;
            if (qlen > 4096) noofsms = 5;
            if (qlen > 5120) noofsms = 6;
            if (qlen > 6144) noofsms = 7;
            if (qlen > 7168) noofsms = 8;
            if (qlen > 8192) noofsms = 9;
            if (qlen > 9216) noofsms = 10;
            if (qlen > 10240) noofsms = 11;
            if (qlen > 11264) noofsms = 12;

            if (q.Any(c => c > 1024))
            {
                // unicode = y
                ucs2 = true;
                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 1024) noofsms = 2;
                if (qlen > 3072) noofsms = 3;
                if (qlen > 4096) noofsms = 4;
                if (qlen > 5120) noofsms = 5;
                if (qlen > 6144) noofsms = 6;
                if (qlen > 7168) noofsms = 7;
                if (qlen > 8192) noofsms = 8;
                if (qlen > 9216) noofsms = 9;
                if (qlen > 10240) noofsms = 10;
            }

            if (ddlRCSType.SelectedValue != "1")
                noofsms = 1;

            // fOR smS

            txtsms.Text = txtsms.Text.Trim();
            string qS = txtsms.Text.Trim();

            int countS_PIPE = q.Count(f => f == '|');
            int qlenS = txtsms.Text.Trim().Length + countS_PIPE;

            int countS_tild = qS.Count(f => f == '~'); qlenS = qlenS + countS_tild;
            int countS_s1 = qS.Count(f => f == '{'); qlenS = qlenS + countS_s1;
            int countS_s2 = qS.Count(f => f == '}'); qlenS = qlenS + countS_s2;
            int countS_s3 = qS.Count(f => f == '['); qlenS = qlenS + countS_s3;
            int countS_s4 = qS.Count(f => f == ']'); qlenS = qlenS + countS_s4;
            int countS_s5 = qS.Count(f => f == '^'); qlenS = qlenS + countS_s5;
            int countS_s6 = qS.Count(f => f == '\\'); qlenS = qlenS + countS_s6;


            ucs2 = false;
            if (qlenS >= 1) noofsms1S = 1;
            if (qlenS > 160) noofsms1S = 2;
            if (qlenS > 306) noofsms1S = 3;
            if (qlenS > 459) noofsms1S = 4;
            if (qlenS > 612) noofsms1S = 5;
            if (qlenS > 765) noofsms1S = 6;
            if (qlenS > 918) noofsms1S = 7;
            if (qlenS > 1071) noofsms1S = 8;
            if (qlenS > 1224) noofsms1S = 9;
            if (qlenS > 1377) noofsms1S = 10;
            if (qlenS > 1530) noofsms1S = 11;
            if (qlenS > 1683) noofsms1S = 12;

            if (qS.Any(c => c > 126))
            {
                // unicode = y
                ucs2 = true;
                qlenS = qS.Length;
                if (qlenS >= 1) noofsms1S = 1;
                if (qlenS > 70) noofsms1S = 2;
                if (qlenS > 134) noofsms1S = 3;
                if (qlenS > 201) noofsms1S = 4;
                if (qlenS > 268) noofsms1S = 5;
                if (qlenS > 335) noofsms1S = 6;
                if (qlenS > 402) noofsms1S = 7;
                if (qlenS > 469) noofsms1S = 8;
                if (qlenS > 536) noofsms1S = 9;
                if (qlenS > 603) noofsms1S = 10;
            }




            Int32 noofSMS = noofsms1S * cnt;
            Int32 noofmessages = noofsms * cnt;

            double rate = 0;
            string strd = "Select * from tblrcsratemaster where userid='" + _user + "'";
            DataTable dtrcs = database.GetDataTable(strd);
            if (dtrcs.Rows.Count > 0)
            {
                rate = (ddlRCSType.SelectedValue == "1" ? Convert.ToDouble(dtrcs.Rows[0]["TextRate"].ToString()) : rate);
                rate = (ddlRCSType.SelectedValue == "2" ? Convert.ToDouble(dtrcs.Rows[0]["ImageRate"].ToString()) : rate);
                rate = (ddlRCSType.SelectedValue == "3" ? Convert.ToDouble(dtrcs.Rows[0]["VideoRate"].ToString()) : rate);
                rate = (ddlRCSType.SelectedValue == "4" ? Convert.ToDouble(dtrcs.Rows[0]["CardRate"].ToString()) : rate);
                rate = (ddlRCSType.SelectedValue == "5" ? Convert.ToDouble(dtrcs.Rows[0]["CarouselRate"].ToString()) : rate);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Rate not Define. Contat Adminstrator !!!');", true);
                return false;
            }

            if (rate <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Rate not Define. Contat Adminstrator !!!');", true);
                return false;
            }
            double bal = Convert.ToDouble(bal2) * 1000;
            //if (bal - Convert.ToDouble(noofmessages * (rate)) < 0)
            if (Convert.ToDouble(noofmessages * (rate * 10)) > bal)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient RCS Balance.');", true);
                return false;
            }
            //bal = bal - Convert.ToDouble(cnt * (rate * 10));
            //bal = Math.Round((bal / 1000), 3);

            //this.Master.lblbalance = Convert.ToString(Session["SMSBAL"]);
            //   Session["SMSBAL"] = ob.UpdateAndGetBalance(UserID, ddlSMSType.SelectedValue, noofmessages, rate);

            ob.noof_message = noofmessages;
            ob.msg_rate = rate;
            double PrevBalance = Convert.ToDouble(bal2);

            Label lblbalance = Master.FindControl("lblrcsBal") as Label;
            lblbalance.Text = Convert.ToString(Session["RCSBAL"]);

            double AvailableBalance = ob.CalculateRCSAmount(UserID, cnt, rate, 1);
            lblbalance.Text = Convert.ToString(AvailableBalance);
            Session["RCSBAL"] = AvailableBalance;
            DataTable dtSMPPAC = new DataTable();


            string templID = "";
            if (ddlTempID.SelectedValue != "0") templID = ddlTempID.SelectedValue;
            //  if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["RCSUserID"]).ToUpper() == speceficAcc || Convert.ToString(Session["RCSUserID"]).ToUpper() == speceficAcc2)
            if (Helper.Global.openTempAc)
            {
                templID = ob.GetUniversalTemplateId();
                //180
                if (qlen > 180)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message has a maximum limit 180 characters');", true);
                    return false;
                }
            }


            string fileUname = _user + DateTime.Now.ToString("_yyyyMMddhhmmssfff");
            if (cnt > 10) // Test template before sending
            {
                // Helper.Global.Istemplatetest = ob.TestSmsbeforeSend(_user, ddlTempID.SelectedValue, txtMsg.Text, ddlSender.SelectedValue, Convert.ToString(Session["PEID"]));
            }
            if (Convert.ToString(Session["XLUPLOADED"]) == "Y")
            {
                if (ddlRCSType.SelectedValue == "1")
                {
                    if (txtMsg.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text first.');", true);
                        return false;
                    }
                    if (Convert.ToInt32(ddlTempID.SelectedValue) == 1)
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }
                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", "", "", "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }

                }
                else if (ddlRCSType.SelectedValue == "2")
                {
                    if (ddlTempID.SelectedIndex > 0)
                    {
                        if (ViewState["tImagePath"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Image Upload First !!!');", true);
                            return true;
                        }
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, ViewState["tImagePath"].ToString(), "", ddlTempID.SelectedValue, ViewState["tImageurl"].ToString(), check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }
                    else
                    {


                        if (Session["imageUName"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Image Upload First !!!');", true);
                            return true;
                        }
                        string imagepath = Session["imageUName"].ToString();
                        string imageUrl = Session["imageUrl"].ToString();
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, imagepath, "", "", imageUrl, check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                        Session.Remove("imageUName");
                    }
                }

                else if (ddlRCSType.SelectedValue == "3")
                {
                    if (ddlTempID.SelectedIndex > 0)
                    {
                        if (ViewState["tvideoPath"] == null && ViewState["tvideoUrl"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Video Upload First !!!');", true);
                            return true;
                        }
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", ViewState["tvideoPath"].ToString(), ddlTempID.SelectedValue, ViewState["tvideoUrl"].ToString(), check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);

                    }
                    else
                    {
                        if (Session["VideoUName"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Video Upload First !!!');", true);
                            return true;
                        }
                        string videopath = Session["VideoUName"].ToString();
                        string VideoUrl = Session["VideoUrl"].ToString();
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", videopath, "", VideoUrl, check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                        Session.Remove("VideoUName");
                    }
                }

                else if (ddlRCSType.SelectedValue == "4")
                {
                    if (ddlTempID.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Template ID First !!!');", true);
                        return true;
                    }

                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }

                }

                else if (ddlRCSType.SelectedValue == "5")
                {
                    if (ddlTempID.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Template ID First !!!');", true);
                        return true;


                    }
                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, txtCampNm.Text, ucs2, noofsms, rate, mobList, "", "", templID, country_code, PrevBalance, AvailableBalance, tmpfilenm, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }

                }
            }
            else
            {

                string campName = "Manual";

                if (ddlRCSType.SelectedValue == "1")
                {
                    if (txtMsg.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Message Text first.');", true);
                        return false;
                    }
                    if (ddlTempID.SelectedIndex > 0)
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }
                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", "", "", "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }


                }
                else if (ddlRCSType.SelectedValue == "2")
                {
                    if (ddlTempID.SelectedIndex > 0)
                    {
                        if (ViewState["tImagePath"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Image Upload First !!!');", true);
                            return true;
                        }
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, ViewState["tImagePath"].ToString(), "", ddlTempID.SelectedValue, ViewState["tImageurl"].ToString(), check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }
                    else
                    {
                        if (Session["imageUName"] == null && Session["imageUrl"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Image Upload First !!!');", true);
                            return true;
                        }
                        string imagepath = Session["imageUName"].ToString();
                        string imageurl = Session["imageUrl"].ToString();
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, imagepath, "", "", imageurl, check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                        Session.Remove("imageUName");
                    }
                }

                else if (ddlRCSType.SelectedValue == "3")
                {

                    if (ddlTempID.SelectedIndex > 0)
                    {
                        if (ViewState["tvideoPath"].ToString() == null && ViewState["tvideoUrl"].ToString() == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Video Upload First !!!');", true);
                            return true;
                        }
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", ViewState["tvideoPath"].ToString(), ddlTempID.SelectedValue, ViewState["tvideoUrl"].ToString(), check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                    }
                    else
                    {
                        if (Session["VideoUName"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Video Upload First !!!');", true);
                            return true;
                        }
                        string videopath = Session["VideoUName"].ToString();
                        string VideoUrl = Session["VideoUrl"].ToString();
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, "", filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", videopath, "", VideoUrl, check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);
                        Session.Remove("VideoUName");
                    }
                }
                else if (ddlRCSType.SelectedValue == "4")
                {
                    if (ddlTempID.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Template ID First !!!');", true);
                        return true;

                    }
                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);

                    }


                }

                else if (ddlRCSType.SelectedValue == "5")
                {
                    if (ddlTempID.SelectedIndex <= 0)
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Template ID First !!!');", true);
                        return true;



                    }
                    else
                    {
                        ob.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, ddlSender.Text, ddlRCSType.SelectedValue, txtMsg.Text.Trim(), filenm, filenmext, dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "MANUAL", "", templID, country_code, PrevBalance, AvailableBalance, fileUname, "", "", ddlTempID.SelectedValue, "", check, ddlsmsTemplateId.SelectedValue, smstext, noofSMS);

                    }


                }


            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Sent Successfully');window.location ='SendRcs.aspx';", true);

            return true;
        }
        protected void ddlTempID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTempID.SelectedIndex > 0)
            {


                rcscode.Util ob = new rcscode.Util();
                DataTable dtT = ob.GetTemplateRCSfromID(Convert.ToString(Session["RCSUserID"]), ddlTempID.SelectedValue);
                //lblTempSMS.Visible = false;
                //string smstxt = dtT.Rows[0]["template"].ToString();

                //if (ddlTempID.SelectedValue == "0") lblTempSMS.Text = "";
                // lblTempSMS.Text = dtT.Rows[0]["TemplateText"].ToString();


                if (dtT.Rows[0]["TemplateText"].ToString() == null)
                    txtMsg.Text = "";
                else
                    txtMsg.Text = dtT.Rows[0]["TemplateText"].ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "smscnt", "smscnt()", true);
                //ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "smscnt();",true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "smscnt()", true);

                Session.Remove("imageUName");
                lblImage.Text = "";
                ViewState["tImagePath"] = dtT.Rows[0]["filepath"].ToString();
                ViewState["tImageurl"] = dtT.Rows[0]["fileurl"].ToString();
                Session.Remove("VideoUName");
                lblVideo.Text = "";
                ViewState["tvideoPath"] = dtT.Rows[0]["filepath"].ToString();
                ViewState["tvideoUrl"] = dtT.Rows[0]["fileurl"].ToString();

                //if (ddlTempID.SelectedValue == "2")
                //{
                //    Session.Remove("imageUName");
                //    lblImage.Text = "";
                //    ViewState["tImagePath"] = dtT.Rows[0]["filepath"].ToString();
                //    ViewState["tImageurl"] = dtT.Rows[0]["fileurl"].ToString();
                //}
                //else if (ddlTempID.SelectedValue == "3")
                //{
                //    Session.Remove("VideoUName");
                //    lblVideo.Text = "";
                //    ViewState["tvideoPath"] = dtT.Rows[0]["filepath"].ToString();
                //    ViewState["tvideoUrl"] = dtT.Rows[0]["fileurl"].ToString();
                //}
            }
            else
            {
                txtMsg.Text = "";
            }

        }
        protected void ddlCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSender();

            Util ob = new Util();
            DataTable dt = ob.GetSMSRateAsPerCountry(Convert.ToString(Session["RCSUserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                //lblRate.Text = Convert.ToString(dt.Rows[0]["RATE_NORMALSMS"]) + " " + paisa + " per SMS";
                ViewState["SMSRATE"] = dt.Rows[0]["RATE_NORMALSMS"];
            }
            if (ddlCCode.SelectedValue != "91")
            {
                //ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Campaign"));
                //ddlSMSType.Items.Remove(ddlSMSType.Items.FindByText("Promotional"));
                HideTemplateIdForeignAcc();
            }
            else
            {
                //Response.Redirect(Request.RawUrl);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "window.location ='send-sms_u1.aspx';", true);
                // PopulateSMSType();
                divTempId.Attributes.Add("class", "form-group row d-block");
                // divTempsms.Attributes.Add("class", "form-group row d-block");
                divOptOut.Attributes.Add("Style", "display:none;");
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SendRcs.aspx");
        }
        protected void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            StartProcess();
            lblMobileCnt.Text = "";
            txtMobNum.Text = "";
            //txtMobNum.Enabled = false;
            divNum.Attributes.Add("style", "pointer-events:none;");

            divFileUpload.Attributes.Add("class", "form-group row d-none");

            if (rdbEntry.Checked)
                divNum.Attributes.Add("style", "pointer-events:all;");

            if (rdbUpload.Checked)
            // if (rdbPersonal.Checked )
            {
                lbln.Visible = false;
                divNum.Visible = false;
                divFileUpload.Attributes.Add("class", "form-group row d-block;");

                divCamp.Attributes.Add("class", "form-group row d-block;");
            }
            else
            {
                divCamp.Attributes.Add("class", "form-group row d-none");
                lbln.Visible = true;
                divNum.Visible = true;
            }

        }
        public void PopulateCountry()
        {
            Util ob = new Util();
            DataTable dt = ob.GetActiveCountry(Convert.ToString(Session["RCSUserID"]));
            ddlCCode.DataSource = dt;
            ddlCCode.DataTextField = "name";
            ddlCCode.DataValueField = "countrycode";
            ddlCCode.DataBind();
        }
        protected void ddlRCSType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Util ob = new Util();
            if (Convert.ToInt32(ddlRCSType.SelectedValue) < 4)
            {
                rbtnSelect.SelectedValue = "0";
                rbtnSelect_SelectedIndexChanged(sender, e);
                divselect.Visible = true;
            }

            else
            {
                divselect.Visible = false;
            }



            DataTable dt = ob.GetRCSRateAsPerCountry(Convert.ToString(Session["RCSUserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (ddlRCSType.SelectedValue == "0")
                {

                    lblRCSRate.Text = "";
                }
                if (ddlRCSType.SelectedValue == "1")
                {
                    PopulateTemplateID(1);
                    lblRCSRate.Text = Convert.ToString(dt.Rows[0]["TextRate"]) + " " + paisa + " per RCS"; ViewState["SMSRATE"] = dt.Rows[0]["TextRate"];
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = true;

                };
                if (ddlRCSType.SelectedValue == "2")
                {
                    PopulateTemplateID(2);
                    lblRCSRate.Text = Convert.ToString(dt.Rows[0]["ImageRate"]) + " " + paisa + " per RCS"; ViewState["SMSRATE"] = dt.Rows[0]["ImageRate"];
                    divuploadImage.Visible = true;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                }
                if (ddlRCSType.SelectedValue == "3")
                {
                    PopulateTemplateID(3);
                    lblRCSRate.Text = Convert.ToString(dt.Rows[0]["VideoRate"]) + " " + paisa + " per RCS"; ViewState["SMSRATE"] = dt.Rows[0]["VideoRate"];
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = true;
                    divsmstext.Visible = false;
                }

                if (ddlRCSType.SelectedValue == "4")
                {
                    PopulateTemplateID(4);
                    lblRCSRate.Text = Convert.ToString(dt.Rows[0]["CardRate"]) + " " + paisa + " per RCS"; ViewState["SMSRATE"] = dt.Rows[0]["CardRate"];
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                    divTempId.Attributes.Add("class", "form-group row");
                }
                if (ddlRCSType.SelectedValue == "5")
                {
                    PopulateTemplateID(5);
                    lblRCSRate.Text = Convert.ToString(dt.Rows[0]["CarouselRate"]) + " " + paisa + " per RCS"; ViewState["SMSRATE"] = dt.Rows[0]["CarouselRate"];
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                    divTempId.Attributes.Add("class", "form-group row");
                }

            }
            else
            {
                if (ddlRCSType.SelectedValue == "0")
                {

                    lblRCSRate.Text = "";
                }
                if (ddlRCSType.SelectedValue == "1")
                {
                    PopulateTemplateID(1);
                    lblRCSRate.Text = Convert.ToString("0");
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = true;

                };
                if (ddlRCSType.SelectedValue == "2")
                {
                    PopulateTemplateID(2);
                    lblRCSRate.Text = Convert.ToString("0");
                    divuploadImage.Visible = true;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                }
                if (ddlRCSType.SelectedValue == "3")
                {
                    PopulateTemplateID(3);
                    lblRCSRate.Text = Convert.ToString("0");
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = true;
                    divsmstext.Visible = false;
                }

                if (ddlRCSType.SelectedValue == "4")
                {
                    PopulateTemplateID(4);
                    lblRCSRate.Text = Convert.ToString("0");
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                }
                if (ddlRCSType.SelectedValue == "5")
                {
                    PopulateTemplateID(5);
                    lblRCSRate.Text = Convert.ToString("0");
                    divuploadImage.Visible = false;
                    divuploadVideo.Visible = false;
                    divsmstext.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Rate not define. Please Contact Admin.');", true);

                return;

            }
        }

        public void PopulateTemplateID(int id)
        {
            rcscode.Util ob = new rcscode.Util();
            DataTable dt = ob.GetRCSTemplateId(Convert.ToString(Session["RCSUserID"]), id);

            ddlTempID.DataSource = dt;
            ddlTempID.DataTextField = "name";
            ddlTempID.DataValueField = "TemplateID";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlTempID.SelectedIndex = 1;
            //else
            // ddlTempID.SelectedIndex = 0;

        }
        public void PopulateSender()
        {
            rcscode.Util ob = new rcscode.Util();
            DataTable dt = ob.GetSenderId(Convert.ToString(Session["RCSUserID"]), ddlCCode.SelectedValue);
            if (dt.Rows.Count == 0)
            {
                ddlSender.Items.Clear();
                return;
            }

            ddlSender.DataSource = dt;
            ddlSender.DataTextField = "senderid";
            ddlSender.DataValueField = "senderid";
            ddlSender.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlSender.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlSender.SelectedIndex = 1;
            //else
            //    ddlSender.SelectedIndex = 0;
        }
        public void StartProcess()
        {
            Session["XLUPLOADED"] = null;
            Session["MOBILECOUNT"] = null;
            Session["SHORTURL"] = null;
            rcscode.Util ob = new rcscode.Util();
            ob.DropUserTmpTable(_user);
            lblUploading.Text = "";
        }
        private void HideTemplateIdForeignAcc()
        {
            //   if (Convert.ToString(Session["DEFAULTCOUNTRYCODE"]) == "971" || Convert.ToString(Session["RCSUserID"]).ToUpper() == speceficAcc || Convert.ToString(Session["RCSUserID"]).ToUpper() == speceficAcc2)
            if (Convert.ToString(ddlCCode.SelectedValue) != "91")
            {
                divTempId.Attributes.Add("class", "form-group row d-none");

                if (Convert.ToString(ddlCCode.SelectedValue) == "971")
                    divOptOut.Attributes.Add("Style", "display:block;");

            }
            if (Helper.Global.openTempAc)
            {
                divTempId.Attributes.Add("class", "form-group row d-none");

            }

            if (string.IsNullOrEmpty(Convert.ToString(Session["Notice"])))
                divFooter.Attributes.Add("class", "form-group row d-none");
            else
            {
                divFooter.Attributes.Add("class", "form-group row d-block");
                lblNotice.Text = Convert.ToString(Session["Notice"]);
            }

        }

        protected void cbfailover_CheckedChanged(object sender, EventArgs e)
        {
            if (cbfailover.Checked == true)
            {
                smstempId.Attributes.Add("class", "form-group row");
                BindSmsTemplate();
            }
            if (cbfailover.Checked == false)
            {
                divTempsms.Visible = false;
                divsms.Visible = false;
                smstempId.Attributes.Add("class", "form-group row d-none");
                //lblTempSMS.Text = "";
            }
        }
        protected void BindSmsTemplate()
        {
            rcscode.Util ob = new rcscode.Util();
            DataTable dt = ob.GetTemplateId(Convert.ToString(Session["RCSUserID"]), 0);

            ddlsmsTemplateId.DataSource = dt;
            ddlsmsTemplateId.DataTextField = "TemplateIDS";
            ddlsmsTemplateId.DataValueField = "TemplateID";
            ddlsmsTemplateId.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlsmsTemplateId.Items.Insert(0, objListItem);
        }
        protected void txtfalse()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            //txtsms.Text="";
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            TextBox9.Visible = false;
            TextBox10.Visible = false;
        }
        protected void ddlsmsTemplateId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlsmsTemplateId.SelectedIndex > 0)
            {

                txtfalse();
                //if(txtsms.Text !="")
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(),"claamyfunction", "Confirm()",true);
                //   //ScriptManager.RegisterStartupScript(this, this.GetType(), "Confirm", "Confirm()", true);

                //    string confirmValue = Request.Form["confirm_value"];
                //    if (confirmValue == "N0")
                //        return;
                //}
                rcscode.Util ob = new rcscode.Util();
                DataTable dtT = ob.GetTemplateSMSfromID(Convert.ToString(Session["RCSUserID"]), ddlsmsTemplateId.SelectedValue);
                divTempsms.Visible = true;
                //divsms.Visible = true;
                lblTempSMS.Text = dtT.Rows[0]["temptextwithvarsrno"].ToString();
                txtsms.Text = dtT.Rows[0]["temptextwithvarsrno"].ToString();
                txtsms.Attributes["readonly"] = "readonly";
                string s = dtT.Rows[0]["temptextwithvarsrno"].ToString();
                hdnTemplateVarText.Value = s;

                string[] stringseprator = new string[] { "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}", "{11}", "{12}", "{13}", "{14}", "{15}" };
                string[] str1 = s.Split(stringseprator, StringSplitOptions.None);
                string[] s1 = s.Split(stringseprator, StringSplitOptions.None);

                vrCount.Value = Convert.ToString(s1.Length - 1);
                if (txtsms.Text != "")
                {
                    txtsms.Text = s;
                    System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "smscnt1", "smscnt1()", true);
                }

                // foreach(string entry in str1)
                //{
                //    i1++;
                //    txtStrtest.Text += entry;
                //    String divt = "div" + i1;
                //    TextBox txtd = new TextBox();
                //    txtd.ID = "txt" + i1;

                //    txtd.CssClass = "form-control";
                //    div11.Controls.Add(txtd);
                //}
                //string[] s1 = s.Split('#');
                int i = 0; if (s.Substring(0, 1) != Convert.ToString(stringseprator)) i = 1;
                int txtid = 0;
                for (; i < s1.Length; i++)
                {
                    //txtFilltext.Text += s1[i].ToString();
                    if (s1[i].Contains("\n"))
                    {
                        string[] st = s1[i].Split('\n');
                        s1[i] = st[0];
                    }
                    if (s1[i] != "")
                    {
                        string[] s2 = s1[i].Split(' ');
                        //LstTemplateFld.Items.Add("#" + s2[0].Replace(",", "").Replace(".", "").Replace("\n", "").Replace("\r", ""));

                        txtid++;
                        if (txtid <= 10)
                        {


                            if (txtid == 1)
                            {
                                TextBox1.Visible = true;

                            }
                            else if (txtid == 2)
                            {
                                TextBox2.Visible = true;
                            }
                            else if (txtid == 3)
                            {
                                TextBox3.Visible = true;
                            }
                            else if (txtid == 4)
                            {
                                TextBox4.Visible = true;
                            }
                            else if (txtid == 5)
                            {
                                TextBox5.Visible = true;
                            }
                            else if (txtid == 6)
                            {
                                TextBox6.Visible = true;
                            }
                            else if (txtid == 7)
                            {
                                TextBox7.Visible = true;
                            }
                            else if (txtid == 8)
                            {
                                TextBox8.Visible = true;
                            }
                            else if (txtid == 9)
                            {
                                TextBox9.Visible = true;
                            }
                            else if (txtid == 10)
                            {
                                TextBox10.Visible = true;
                            }
                        }




                    }
                }
                divtxt.Attributes.Add("Style", "overflow-y:auto;height:220px;");
            }
            else
            {
                divTempsms.Visible = false;
                divsms.Visible = false;
                //lblTempSMS.Text = "";
                divtxt.Attributes.Add("Style", "overflow-y:auto;height:0px;");
            }
        }

        protected void rbtnSelect_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlRCSType.SelectedValue == "1")
            {
                if (rbtnSelect.SelectedValue == "0")
                {

                    divsmstext.Attributes.Add("class", "form-group row");
                    divTempId.Attributes.Add("class", "form-group row d-none");
                    txtMsg.Text = "";
                }
                else
                {
                    ddlTempID.SelectedValue = "0";
                    divTempId.Attributes.Add("class", "form-group row");
                    txtMsg.Text = "";
                }
            }
            if (ddlRCSType.SelectedValue == "2")
            {
                if (rbtnSelect.SelectedValue == "0")
                {
                    divuploadImage.Attributes.Add("class", "form-group row");
                    divTempId.Attributes.Add("class", "form-group row d-none");
                }
                else
                {
                    divTempId.Attributes.Add("class", "form-group row");
                    divuploadImage.Attributes.Add("class", "form-group row  d-none");
                }
            }
            if (ddlRCSType.SelectedValue == "3")
            {
                if (rbtnSelect.SelectedValue == "0")
                {
                    divuploadVideo.Attributes.Add("class", "form-group row");
                    divTempId.Attributes.Add("class", "form-group row d-none");
                }
                else
                {
                    divuploadVideo.Attributes.Add("class", "form-group row  d-none");
                    divTempId.Attributes.Add("class", "form-group row");

                }
            }



        }
    }
}