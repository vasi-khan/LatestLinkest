using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using eMIMPanel.rcscode;
using System.Text.RegularExpressions;

namespace eMIMPanel
{
    public partial class send_rcs_user_template : System.Web.UI.Page
    {
        rcscode.UtilN ob = new rcscode.UtilN();
        rcscode.Util obU = new rcscode.Util();
        string user;
        string usertype;
        string UserID;
        private int PageSize = 6;
        string UploadPath;

        protected void Page_Load(object sender, EventArgs e)
        {

            string sql = "";
            Session["RCSUserID"] = Convert.ToString(rcscode.database.GetScalarValue("select top 1 RCSACCID  from MapSMSAcc where smsAccId='" + Convert.ToString(Session["UserID"]) + "'"));

            UploadPath = Convert.ToString(ConfigurationManager.AppSettings["LOGPATH"]);
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (usertype == "") Response.Redirect("login.aspx");
            UserID = Convert.ToString(Session["RCSUserID"]);
            //if (txtTempText.Text.Trim() != "")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this.upPnl, upPnl.GetType(), "smscnt", "smscnt()", true);
            //    //ScriptManager.RegisterStartupScript(this.upPnl, upPnl.GetType(), "smscnt", "smscnt", true);
            //}

            Hidden();
            ChooseTypes();
            carouselTypes();
            CardTypes();
            ShowMsgCharCnt();

            if (!Page.IsPostBack)
            {
                txtdesc.Attributes.Add("maxlength", txtdesc.MaxLength.ToString());
                txtdescription.Attributes.Add("maxlength", txtdescription.MaxLength.ToString());
                txtTempText.Attributes.Add("maxlength", txtTempText.MaxLength.ToString());
                btnSave.Visible = false;
                //btnsubmit.Visible = false;
                //btnsubmit.CssClass = "btn btn-primary btn-icon-split  mt-2";
                ddlRCSType.SelectedValue = "0";
                ddlRCSType_SelectedIndexChanged1(sender, e);
                string user1 = "Tmp_DeleteCrousel_" + UserID;
                sql = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @"";
                database.ExecuteNonQuery(sql);
                //if (Request.QueryString.Count > 0)
                //{
                usertype = Convert.ToString(Session["UserType"]);
                user = Convert.ToString(Session["User"]);
                if (usertype == "") Response.Redirect("login.aspx");
                UserID = Convert.ToString(Session["RCSUserID"]);
                //string TempId = Request.QueryString[0].ToString();
                //string Rid = Request.QueryString[1].ToString();
                //ddlSelectrcstype.SelectedValue = Rid;
                //ddlRCSType.SelectedValue = Rid;
                //lnkShow_Click(sender, e);

                btnSave.Visible = false;
                btnSave.Attributes.Add("class", "btn btn-primary btn-icon-split  mt-2 mb-3");
                //Hid(Rid, TempId);

                //}
            }
            btnSave.CssClass = "btn btn-primary btn-icon-split  mt-2";

        }
        public void Hid(string id, string TempId)
        {
            divSearch.Attributes.Add("class", "row");
            divmain.Visible = false;
            //if (ddlRCSType.SelectedValue == id)
            if (id == "1" || id == "2" || id == "3" || id == "4" || id == "5")
            {
                divSearch.Attributes.Add("class", "row");
                CommandEventArgs commandArgs = new CommandEventArgs("View", TempId.ToString());

                GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grv.Rows[0], grv, commandArgs);

                grv_RowCommand(grv, eventArgs);
                //divSearch.Style.Add("display", "show");
            }
            //else if(id == "1" || id == "2")
            //{
            //    divSearch.Attributes.Add("class", "row");
            //    CommandEventArgs commandArgs = new CommandEventArgs("View", TempId.ToString());

            //    GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grv.Rows[0], grv, commandArgs);

            //    grv_RowCommand(grv, eventArgs);
            //}
        }
        private void Hidden()
        {
            // ddlRCSType.Items[0].Attributes.CssStyle.Add("margin-right", "30px;");
            if (ddlRCSType.SelectedValue == "1")
            {
                divTempTxt.Style.Add("display", "show");
                divimg.Style.Add("display", "none");
                divVedio.Style.Add("display", "none");
                divcarousel.Style.Add("display", "none");
                divcard.Style.Add("display", "none");
            }
            if (ddlRCSType.SelectedValue == "2")
            {
                divimg.Style.Add("display", "show");
                divTempTxt.Style.Add("display", "none");
                divVedio.Style.Add("display", "none");
                divcarousel.Style.Add("display", "none");
                divcard.Style.Add("display", "none");

            }
            if (ddlRCSType.SelectedValue == "3")
            {
                divTempTxt.Style.Add("display", "none");
                divimg.Style.Add("display", "none");
                divVedio.Style.Add("display", "show");
                divcarousel.Style.Add("display", "none");
                divcard.Style.Add("display", "none");

            }
            if (ddlRCSType.SelectedValue == "4")
            {
                divTempTxt.Style.Add("display", "none");
                divimg.Style.Add("display", "none");
                divVedio.Style.Add("display", "none");
                divcarousel.Style.Add("display", "none");
                divcard.Style.Add("display", "show");
            }
            if (ddlRCSType.SelectedValue == "5")
            {
                divTempTxt.Style.Add("display", "none");
                divimg.Style.Add("display", "none");
                divVedio.Style.Add("display", "none");
                divcarousel.Style.Add("display", "show");
                divcard.Style.Add("display", "none");
            }


        }

        public void ShowMsgCharCnt()
        {
            string q = txtTempText.Text.Replace("'", "''").Trim();

            int count_PIPE = q.Count(f => f == '|');
            int qlen = txtTempText.Text.Replace("'", "''").Trim().Length + count_PIPE;

            int count_tild = q.Count(f => f == '~');
            qlen = qlen + count_tild;
            int count_s1 = q.Count(f => f == '{'); qlen = qlen + count_s1;
            int count_s2 = q.Count(f => f == '}'); qlen = qlen + count_s2;
            int count_s3 = q.Count(f => f == '['); qlen = qlen + count_s3;
            int count_s4 = q.Count(f => f == ']'); qlen = qlen + count_s4;
            int count_s5 = q.Count(f => f == '^'); qlen = qlen + count_s5;
            int count_s6 = q.Count(f => f == '\\'); qlen = qlen + count_s6;

            int noofsms = 0;
            if (qlen >= 1) noofsms = 1;
            if (qlen > 160) noofsms = 2;
            if (qlen > 306) noofsms = 3;
            if (qlen > 459) noofsms = 4;
            if (qlen > 612) noofsms = 5;
            if (qlen > 765) noofsms = 6;
            if (qlen > 918) noofsms = 7;
            if (qlen > 1071) noofsms = 8;
            if (qlen > 1224) noofsms = 9;
            if (qlen > 1377) noofsms = 10;
            if (qlen > 1530) noofsms = 11;
            if (qlen > 1683) noofsms = 12;
            // lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();
            lblsmscnt.Text = noofsms.ToString();
            if (q.Any(c => c > 126))
            {
                // unicode = y

                qlen = q.Length;
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
                //lblsmscnt.Text = "No. of Char: " + qlen + ". No. of SMS: " + noofsms.ToString();
                lblsmscnt.Text = noofsms.ToString();
                lblUniCode.Text = "UNICODE : YES";
            }
        }

        protected void ddlRCSType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            btnsubmit.Visible = false;
            Hidden();

            if (ddlRCSType.SelectedValue == "1")
            {
                addnewhide.Visible = false;
                txttempname.ReadOnly = false;
                txttempname.Text = "";
                btngrid.Visible = false;
                if (txtTempText.Text.Replace("'", "''").Trim() != "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.upPnl, upPnl.GetType(), "smscnt", "smscnt()", true);
                    //ScriptManager.RegisterStartupScript(this.upPnl, upPnl.GetType(), "smscnt", "smscnt", true);
                }
            }
            if (ddlRCSType.SelectedValue == "2")
            {
                addnewhide.Visible = false;
                txttempname.ReadOnly = false;
                txttempname.Text = "";
                btngrid.Visible = false;
            }
            if (ddlRCSType.SelectedValue == "3")
            {
                addnewhide.Visible = false;
                txttempname.ReadOnly = false;
                txttempname.Text = "";
                btngrid.Visible = false;
            }
            if (ddlRCSType.SelectedValue == "4")
            {
                if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
                {
                    BindcardSuggestion();


                }
                addnewhide.Visible = false;
                txttempname.ReadOnly = false;
                txttempname.Text = "";
                btngrid.Visible = false;
            }
            if (ddlRCSType.SelectedValue == "5")
            {
                if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
                {
                    BindcardSuggestion();
                    DataTable dtCC1 = database.GetDataTable("select * from CarouselSuggetion where CarouselId='" + ViewState["Tid"] + "' and active=1 ");
                    if (dtCC1.Rows.Count > 0)
                    {

                        gvcrauselS.DataSource = dtCC1;
                        gvcrauselS.DataBind();

                        //gvSType.Columns[7].Visible = true;

                    }
                    ViewState["SgoverallTable"] = dtCC1;
                    divcarouselsuggsction.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
                    suggectionCrasoul.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
                    suggectionCardCrasoul.Style.Add("display", "show");
                    suggectionCrasoul.Style.Add("display", "show");
                }
                else
                {
                    btnsubmit.Visible = true;
                    string user1 = "Tmp_Template_" + UserID;
                    string query = "";
                    query = @"if exists (select * from sys.tables where name='" + user1 + @"') Select * from " + user1 + @"";
                    DataTable dttemp = database.GetDataTable(query);
                    if (dttemp.Rows.Count > 0)
                    {
                        lbltcount.Text = Convert.ToString(dttemp.Rows.Count);
                        pnlPopUp_Detail_ModalPopupExtender.Show();

                    }
                    //bindcardDetails();
                    addnewhide.Visible = true;
                    btngrid.Visible = true;
                    string Carouselcard = "Tmp_Carouselcard_" + UserID;
                    database.ExecuteNonQuery("if exists (select * from sys.tables where name='" + Carouselcard + @"') BEGIN drop table  " + Carouselcard + "  end");
                    //    DataTable dtCC = database.GetDataTable("if exists (select * from sys.tables where name='" + Carouselcard + @"') BEGIN select * from  " + Carouselcard + "  end ");
                    //if (dtCC.Rows.Count > 0)
                    //{
                    //    gvSType.DataSource = null;
                    //    gvSType.DataSource = dtCC;
                    //    gvSType.DataBind();
                    //    // GridFormat(gvSType);
                    //}
                }
                //btnSave.Visible = false;

                //divmain.Attributes.Add("Class","col-xl-12 col-lg-12");
                //rcscard( Convert.ToInt32(ddlRCSType.SelectedValue)); 
                // divmain.Style.Add("Class","col-xl-12 col-lg-12");

            }
        }
        protected void bindcardDetails()
        {
            string user1 = "Tmp_Template_" + UserID;
            string query = "";
            query = @"if exists (select * from sys.tables where name='" + user1 + @"') Select * from " + user1 + @"";
            DataTable dttemp = database.GetDataTable(query);
            if (dttemp.Rows.Count > 0)
            {
                txttempname.Text = dttemp.Rows[0]["templatetext"].ToString().Replace("''", "'");
                ddlwidth.SelectedValue = dttemp.Rows[0]["CardWidth"].ToString();
                // ddlwidth.Attributes.Add("disabled", "disabled");

                ddlheight.SelectedValue = dttemp.Rows[0]["CardHeight"].ToString();
                txttempname.ReadOnly = true;
                ddlwidth.Style.Add("pointer-events", "none");
                ddlheight.Style.Add("pointer-events", "none");
                lblcreatedcard.Text = Convert.ToString(dttemp.Rows.Count);

                lblcreatedcard.Visible = true;
                divTc.Visible = true;
                divTc.Style.Add("display", "show");


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (txttempname.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Template Name !!!');", true);
            //    return;
            //}
            string fileurl = "";
            string filepath = "";

            if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
            {


                if (ViewState["Did"].ToString() == "" && ViewState["Did"].ToString() == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Card id !!!');", true);
                    return;
                }
                if (ViewState["Tid"].ToString() == "" && ViewState["Tid"].ToString() == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Card id !!!');", true);
                    return;
                }
                if (ddlRCSType.SelectedValue == "1")
                {
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (txtTempText.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Text');", true);
                        return;
                    }
                    try
                    {
                        ob.UpdateCard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), fileurl, filepath, ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                           ddlAlignment.SelectedValue, txtcardtitle.Text.Replace("'", "''").Trim(), txtdesc.Text.Replace("'", "''").Trim(), ddlcardheight.SelectedValue, txtcardtext.Text.Trim(), ddlcardtype.SelectedValue,
                           txtcardurl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["Did"].ToString(), ViewState["Tid"].ToString());

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Updated Successfully !!');", true);
                        //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["Tid"].ToString() + "&cstype="+ ddlSelectrcstype.SelectedValue + "", false);
                        //Response.Redirect("send_rcs_user_template.aspx", false);
                        Hid(ddlSelectrcstype.SelectedValue, ViewState["Tid"].ToString());
                        ViewState["IsEdit"] = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Edit Sucessfully !!');", true);
                        lbtnFind.Visible = true;
                        btnSave.Visible = false;
                        lbtnAdd.Visible = true;
                        span.InnerText = "Save Template";
                        ispan.Attributes.Add("class", "fas fa-save");
                        span1.Attributes.Add("class", "text-success");
                        span.Attributes.Add("class", "text-success font-weight-bold");

                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }
                if (ddlRCSType.SelectedValue == "2")
                {
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }

                    if (Session["UPLOADMEDIA2"] == null && Session["filepath2"] == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select First Image');", true);

                        return;
                    }
                    try
                    {
                        ob.UpdateCard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), Session["UPLOADMEDIA2"].ToString(), Session["filepath2"].ToString(), ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                           ddlAlignment.SelectedValue, txtcardtitle.Text.Replace("'", "''").Trim(), txtdesc.Text.Replace("'", "''").Trim(), ddlcardheight.SelectedValue, txtcardtext.Text.Trim(), ddlcardtype.SelectedValue,
                           txtcardurl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["Did"].ToString(), ViewState["Tid"].ToString());

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Updated Successfully !!');", true);
                        //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["Tid"].ToString() + "&cstype="+ ddlSelectrcstype.SelectedValue + "", false);
                        //Response.Redirect("send_rcs_user_template.aspx", false);
                        Hid(ddlSelectrcstype.SelectedValue, ViewState["Tid"].ToString());
                        ViewState["IsEdit"] = false;
                        lbtnFind.Visible = true;
                        lbtnAdd.Visible = true;
                        btnSave.Visible = false;
                        Session.Remove("UPLOADMEDIA2");
                        Session.Remove("filepath2");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Edit Sucessfully !!');", true);
                        span.InnerText = "Save Template";
                        ispan.Attributes.Add("class", "fas fa-save");
                        span1.Attributes.Add("class", "text-success");

                        span.Attributes.Add("class", "text-success font-weight-bold");

                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }
                if (ddlRCSType.SelectedValue == "3")
                {
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (Session["UPLOADMEDIAvdo"] == null && Session["filepathvdo"] == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Video');", true);
                        return;
                    }

                    try
                    {
                        ob.UpdateCard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), Session["UPLOADMEDIAvdo"].ToString(), Session["filepathvdo"].ToString(), ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                           ddlAlignment.SelectedValue, txtcardtitle.Text.Replace("'", "''").Trim(), txtdesc.Text.Replace("'", "''").Trim(), ddlcardheight.SelectedValue, txtcardtext.Text.Trim(), ddlcardtype.SelectedValue,
                           txtcardurl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["Did"].ToString(), ViewState["Tid"].ToString());

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Updated Successfully !!');", true);
                        //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["Tid"].ToString() + "&cstype="+ ddlSelectrcstype.SelectedValue + "", false);
                        //Response.Redirect("send_rcs_user_template.aspx", false);
                        Hid(ddlSelectrcstype.SelectedValue, ViewState["Tid"].ToString());
                        ViewState["IsEdit"] = false;
                        lbtnFind.Visible = true;
                        lbtnAdd.Visible = true;
                        btnSave.Visible = false;
                        Session.Remove("UPLOADMEDIAvdo");
                        Session.Remove("filepathvdo");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Edit Sucessfully !!');", true);
                        span.InnerText = "Save Template";
                        ispan.Attributes.Add("class", "fas fa-save");
                        span1.Attributes.Add("class", "text-success");
                        span.Attributes.Add("class", "text-success font-weight-bold");

                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }
                if (ddlRCSType.SelectedValue == "4")
                {
                    if (ddlOrientation.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Orientation');", true);
                        return;
                    }
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (ddlAlignment.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Alignment');", true);
                        return;
                    }
                    if (txtcardtitle.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Title');", true);
                        return;
                    }
                    if (txtdesc.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Description');", true);
                        return;
                    }
                    if (ddlcardheight.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Card Height');", true);
                        return;
                    }

                    if (Session["UPLOADMEDIACARD"].ToString() == null && Session["Filepathcard"].ToString() == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Type');", true);
                        return;
                    }
                    else
                    {
                        fileurl = Session["UPLOADMEDIACARD"].ToString();
                        filepath = Session["Filepathcard"].ToString();
                    }
                    if (fileurl == "" && filepath == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Image');", true);
                        return;
                    }
                    try
                    {


                        ob.UpdateCard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), fileurl, filepath, ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                        ddlAlignment.SelectedValue, txtcardtitle.Text.Replace("'", "''").Trim(), txtdesc.Text.Replace("'", "''").Trim(), ddlcardheight.SelectedValue, txtcardtext.Text.Trim(), ddlcardtype.SelectedValue,
                        txtcardurl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["Did"].ToString(), ViewState["Tid"].ToString());
                        DataTable dtup = ViewState["CurrentTable"] as DataTable;

                        string strs = "";
                        strs = "update CardSuggetion set active = 0 where CardId = '" + ViewState["Did"] + "'";
                        database.ExecuteNonQuery(strs);
                        if (dtup != null)
                        {
                            foreach (DataRow row in dtup.Rows)
                            {
                                string str = "";
                                str = @"insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId,active)" +
                                    "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString().Replace("'", "''") + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', '" + ViewState["Did"] + "',1)";
                                database.ExecuteNonQuery(str);
                            }
                        }

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Updated Successfully !!');", true);
                        //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["Tid"].ToString() + "&cstype="+ ddlSelectrcstype.SelectedValue + "", false);
                        //Response.Redirect("send_rcs_user_template.aspx", false);
                        Hid(ddlSelectrcstype.SelectedValue, ViewState["Tid"].ToString());
                        ViewState["IsEdit"] = false;
                        lbtnFind.Visible = true;
                        lbtnAdd.Visible = true;
                        btnSave.Visible = false;
                        ViewState["CurrentTable"] = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Edit Sucessfully !!');", true);
                        span.InnerText = "Save Template";
                        ispan.Attributes.Add("class", "fas fa-save");
                        span1.Attributes.Add("class", "text-success");
                        span.Attributes.Add("class", "text-success font-weight-bold");
                        Session.Remove("Filepathcard");
                        Session.Remove("UPLOADMEDIACARD");
                        suggestionCard.Style.Add("display", "none");


                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }

                }

                if (ddlRCSType.SelectedValue == "5")
                {
                    if (ddlwidth.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Card Width');", true);
                        return;
                    }
                    if (txttitle.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Title');", true);
                        return;
                    }
                    if (txtdescription.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Description');", true);
                        return;
                    }
                    if (ddlheight.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Height');", true);
                        return;
                    }


                    if (Session["UPLOADMEDIACAROUSEL"].ToString() == null && Session["filepathcarosel"].ToString() == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Image');", true);
                        return;
                    }
                    else
                    {
                        fileurl = Session["UPLOADMEDIACAROUSEL"].ToString();
                        filepath = Session["filepathcarosel"].ToString();
                    }
                    if (fileurl == "" && filepath == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Image');", true);
                        return;
                    }
                    try
                    {


                        ob.UpdateCard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), fileurl, filepath, ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                        ddlAlignment.SelectedValue, txttitle.Text.Replace("'", "''").Trim(), txtdescription.Text.Replace("'", "''").Trim(), ddlheight.SelectedValue, txttext.Text.Trim(), ddltypes.SelectedValue,
                        txturl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["Did"].ToString(), ViewState["Tid"].ToString());
                        DataTable dtup = ViewState["CurrentTable"] as DataTable;
                        string strs = "";
                        strs = "update CardSuggetion set active = 0 where CardId = '" + ViewState["Did"] + "'";
                        database.ExecuteNonQuery(strs);

                        foreach (DataRow row in dtup.Rows)
                        {
                            string str = "";
                            str = @"insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId,active)" +
                                "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString() + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', '" + ViewState["Did"] + "',1)";
                            database.ExecuteNonQuery(str);
                        }
                        DataTable dtsgOVC = ViewState["SgoverallTable"] as DataTable;
                        string str1 = "";
                        str1 = "update CarouselSuggetion set active = 0 where CarouselId = '" + ViewState["Tid"] + "'";
                        database.ExecuteNonQuery(str1);

                        foreach (DataRow row in dtsgOVC.Rows)
                        {
                            string str2 = "";
                            str2 = @"insert into CarouselSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CarouselId,active)" +
                                "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString() + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', '" + ViewState["Tid"] + "',1)";
                            database.ExecuteNonQuery(str2);
                        }

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Updated Successfully !!');", true);
                        //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["Tid"].ToString() + "&cstype="+ ddlSelectrcstype.SelectedValue + "", false);
                        //Response.Redirect("send_rcs_user_template.aspx", false);
                        Hid(ddlSelectrcstype.SelectedValue, ViewState["Tid"].ToString());
                        ViewState["IsEdit"] = false;
                        lbtnFind.Visible = true;
                        lbtnAdd.Visible = true;
                        btnSave.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Edit Sucessfully !!');", true);
                        span.InnerText = "Save Template";
                        ispan.Attributes.Add("class", "fas fa-save");
                        span1.Attributes.Add("class", "text-success");
                        span.Attributes.Add("class", "text-success font-weight-bold");

                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }




            }
            else if (Convert.ToBoolean(ViewState["IsAddcard"]) == true)
            {
                if (ddlwidth.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Card Width');", true);
                    return;
                }
                if (txttitle.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Title');", true);
                    return;
                }
                if (txtdescription.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Description');", true);
                    return;
                }
                if (ddlheight.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Height');", true);
                    return;
                }


                if (Session["UPLOADMEDIACAROUSEL"] == null && Session["filepathcarosel"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload Image');", true);
                    return;
                }
                //if (txttext.Text.Trim() == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Text');", true);
                //    return;
                //}
                DataTable dtc = new DataTable();

                dtc = database.GetDataTable("select * from RcsTemplateDetail where userid='" + UserID + "' and templateid='" + ViewState["templateid"].ToString() + "' and active=1");
                if (dtc.Rows.Count >= 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You have already added 10 cards. You can not add more than 10 cards. !!!');", true);
                    return;

                }
                DataTable dtcard = ob.Addcard(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), Session["UPLOADMEDIACAROUSEL"].ToString(), Session["filepathcarosel"].ToString(), ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                ddlAlignment.SelectedValue, txttitle.Text.Replace("'", "''").Trim(), txtdescription.Text.Replace("'", "''").Trim(), ddlheight.SelectedValue, txttext.Text.Trim(), ddltypes.SelectedValue,
                txturl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text, ViewState["templateid"].ToString());
                if (dtcard.Rows.Count >= 2)
                {
                    btnsubmit.Visible = true;
                }

                string Card = "";

                Card = "Tmp_Carouselcard_" + UserID;
                string sql2 = "";


                DataTable dtup = ViewState["CurrentTable"] as DataTable;

                string strs = "";
                //strs = "update CardSuggetion set active = 0 where CardId = '" + ViewState["Did"] + "'";
                //database.ExecuteNonQuery(strs);
                if (dtup != null)
                {
                    foreach (DataRow row in dtup.Rows)
                    {
                        string str = "";
                        str = @"declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId,active)" +
                            "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString() + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', @id,1)";
                        database.ExecuteNonQuery(str);
                    }
                }

                //sql2 = @"declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId)" +
                //    "Select suggetiontype, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, @id from " + Card + "";
                //database.ExecuteNonQuery(sql2);
                //database.ExecuteNonQuery("drop table " + Card + "");
                suggestionCard.Style.Add("display", "none");
                Session.Remove("Filepathcard");
                Session.Remove("UPLOADMEDIACARD");
                //ViewState["carocount"] = Convert.ToString (dttmp.Rows.Count);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Added Successfully !!');", true);
                //Response.Redirect("send_rcs_user_template.aspx?ID=" + ViewState["templateid"].ToString() + "&cstype='" + ddlSelectrcstype.SelectedValue + "'", false);
                Hid(ddlSelectrcstype.SelectedValue, ViewState["templateid"].ToString());
                //CommandEventArgs commandArgs = new CommandEventArgs("Add", ViewState["CId"].ToString());
                ViewState["IsAddcard"] = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Added Sucessfully !!!');", true);

                lbtnFind.Visible = true;
                lbtnAdd.Visible = true;
                btnSave.Visible = false;
                span.InnerText = "Save Template";
                ispan.Attributes.Add("class", "fas fa-save");
                span1.Attributes.Add("class", "text-success");
                span.Attributes.Add("class", "text-success font-weight-bold");
                Session.Remove("UPLOADMEDIACAROUSEL");
                Session.Remove("filepathcarosel");
                //GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grv.Rows[0], grv, commandArgs);

                //grv_RowCommand(grv, eventArgs);
            }
            else if (Convert.ToBoolean(ViewState["IsDelete"]) == true)
            {
                try
                {


                    string sql = "";
                    string sql1 = "";


                    string user1 = "Tmp_DeleteCrousel_" + UserID;
                    sql = "declare @id bigint update RcsTemplateDetail set active=0 where id in (Select id from " + user1 + " ); ";


                    string tid = Convert.ToString(database.GetScalarValue("Select distinct templateid from " + user1 + ""));
                    database.ExecuteNonQuery(sql);
                    sql1 = @"Drop table " + user1 + "";
                    database.ExecuteNonQuery(sql1);
                    //CommandEventArgs commandArgs = new CommandEventArgs("View", tid);
                    Hid(ddlSelectrcstype.SelectedValue, ViewState["IsDeleteTempId"].ToString());
                    //CommandEventArgs commandArgs = new CommandEventArgs("Add", ViewState["CId"].ToString());
                    ViewState["IsDelete"] = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Deleted Sucessfully !!!');", true);
                    span.InnerText = "Save Template";
                    ispan.Attributes.Add("class", "fas fa-save");
                    span1.Attributes.Add("class", "text-success");
                    span.Attributes.Add("class", "text-success font-weight-bold");
                    //GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grv.Rows[0], grv, commandArgs);

                    //grv_RowCommand(grv, eventArgs);

                }
                catch (Exception ex)
                {

                }
            }
            else
            {


                //---------------For Text-----------
                if (ddlRCSType.SelectedValue == "1")
                {
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (txtTempText.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Text');", true);
                        return;
                    }
                    try
                    {
                        //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Text1');", true);
                        string strQuery = "sp_RcsTempDetails";
                        SqlParameter[] objTempinsert = new SqlParameter[5];

                        objTempinsert[0] = new SqlParameter("@RCSType", SqlDbType.VarChar, 1);
                        objTempinsert[0].Value = ddlRCSType.SelectedValue;

                        objTempinsert[1] = new SqlParameter("@TemplateName", SqlDbType.NVarChar, 100);
                        objTempinsert[1].Value = txttempname.Text.Replace("'", "''").Trim();

                        objTempinsert[2] = new SqlParameter("@TemplateText", SqlDbType.NVarChar);
                        objTempinsert[2].Value = txtTempText.Text.Replace("'", "''").Trim();

                        objTempinsert[3] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                        objTempinsert[3].Value = UserID;

                        objTempinsert[4] = new SqlParameter("@intresult", SqlDbType.Int);
                        objTempinsert[4].Direction = ParameterDirection.Output;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Text2');", true);
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name1');", true);
                        int intresult = rcscode.database.ExecuteSqlSP(objTempinsert, strQuery);
                        if (intresult > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Text Template Created Successfully !!');", true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Created Successfully !!');", true);

                        }

                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }

                //---------------For Image----------

                if (ddlRCSType.SelectedValue == "2")
                {
                    //if (FileUpload2.HasFile == false)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Image');", true);
                    //    return;
                    //}
                    //if (FileUpload2.PostedFile.ContentLength > (1024 * 1024))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Image file size cannot be above of 2 MB');", true);
                    //    lblUploading.Text = "Upload rejected.";
                    //    return;
                    //}
                    try
                    {
                        if (txttempname.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                            return;
                        }
                        if (Session["UPLOADMEDIA2"] == null && Session["filepath2"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select First Image');", true);

                            return;
                        }

                        string strQuery = "sp_RcsTempDetails";
                        SqlParameter[] objTempinsert = new SqlParameter[6];

                        objTempinsert[0] = new SqlParameter("@RCSType", SqlDbType.VarChar, 1);
                        objTempinsert[0].Value = ddlRCSType.SelectedValue;

                        objTempinsert[1] = new SqlParameter("@FileUrl", SqlDbType.VarChar);
                        objTempinsert[1].Value = Session["UPLOADMEDIA2"];

                        objTempinsert[2] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                        objTempinsert[2].Value = Session["filepath2"];

                        objTempinsert[3] = new SqlParameter("@TemplateName", SqlDbType.NVarChar, 100);
                        objTempinsert[3].Value = txttempname.Text.Replace("'", "''").Trim();

                        objTempinsert[4] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                        objTempinsert[4].Value = UserID;

                        objTempinsert[5] = new SqlParameter("@intresult", SqlDbType.Int);
                        objTempinsert[5].Direction = ParameterDirection.Output;

                        int intresult = rcscode.database.ExecuteSqlSP(objTempinsert, strQuery);
                        if (intresult > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Created Successfully');", true);
                            Session.Remove("UPLOADMEDIA2");
                            Session.Remove("filepath2");
                        }

                        //if (!(en.Contains("MP4") || en.Contains("MOV")))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                        //    lblUploading.Text = "Upload rejected.";
                        //    return;
                        //}

                        //string FolderPath = "MEDIAUpload/";

                        //Session["UPLOADFILENMEXT"] = Extension;
                        //string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                        //string FilePath = Server.MapPath(FolderPath + FN + Extension);
                        //FileUpload1.SaveAs(FilePath);
                        //Session["UPLOADMEDIA"] = FolderPath + FN + Extension;
                        //Session["UPLOADMEDIAFN"] = FN + Extension;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                        //lblUploading.Text = "Uploaded successfully.";


                    }
                    catch (Exception EX)
                    {
                        throw EX;
                    }
                }

                //---------------For Video----------

                if (ddlRCSType.SelectedValue == "3")
                {
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (Session["UPLOADMEDIAvdo"] == null && Session["filepathvdo"] == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Video');", true);
                        return;
                    }
                    try
                    {

                        string strQuery = "sp_RcsTempDetails";
                        SqlParameter[] objTempinsert = new SqlParameter[6];

                        objTempinsert[0] = new SqlParameter("@RCSType", SqlDbType.VarChar, 1);
                        objTempinsert[0].Value = ddlRCSType.SelectedValue;

                        objTempinsert[1] = new SqlParameter("@FileUrl", SqlDbType.VarChar);
                        objTempinsert[1].Value = Session["UPLOADMEDIAvdo"];

                        objTempinsert[2] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                        objTempinsert[2].Value = Session["filepathvdo"];

                        objTempinsert[3] = new SqlParameter("@TemplateName", SqlDbType.NVarChar, 100);
                        objTempinsert[3].Value = txttempname.Text.Replace("'", "''").Trim();

                        objTempinsert[4] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                        objTempinsert[4].Value = UserID;

                        objTempinsert[5] = new SqlParameter("@intresult", SqlDbType.Int);
                        objTempinsert[5].Direction = ParameterDirection.Output;

                        int intresult = rcscode.database.ExecuteSqlSP(objTempinsert, strQuery);
                        if (intresult > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Created Successfully');", true);
                            Session.Remove("UPLOADMEDIAvdo");
                            Session.Remove("filepathvdo");

                        }

                    }
                    catch (Exception EX)
                    {
                        throw EX;
                    }
                }

                //---------------For Card-----------

                if (ddlRCSType.SelectedValue == "4")
                {
                    string Card = "";
                    if (ddlOrientation.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Orientation');", true);
                        return;
                    }
                    if (txttempname.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                        return;
                    }
                    if (ddlAlignment.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Alignment');", true);
                        return;
                    }
                    if (txtcardtitle.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Title');", true);
                        return;
                    }
                    if (txtdesc.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Description');", true);
                        return;
                    }
                    if (ddlcardheight.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Card Height');", true);
                        return;
                    }
                    if (Session["Filepathcard"] == null && Session["UPLOADMEDIACARD"] == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload first Image !!!');", true);
                        return;
                    }
                    Card = "Tmp_CardS_" + UserID;

                    int count = Convert.ToInt32(database.GetScalarValue("if exists (select * from sys.tables where name='" + Card + @"') BEGIN select count(*) from  " + Card + "  end "));
                    // database.ExecuteNonQuery(Templateid);                   

                    //if (count <= 0)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select card suggestion');", true);
                    //    return;
                    //}
                    try
                    {

                        string strQuery = "sp_RcsTempDetails";
                        SqlParameter[] objTempinsert = new SqlParameter[15];

                        objTempinsert[0] = new SqlParameter("@RCSType", SqlDbType.VarChar, 1);
                        objTempinsert[0].Value = ddlRCSType.SelectedValue;

                        objTempinsert[1] = new SqlParameter("@CardOrientation", SqlDbType.VarChar, 1);
                        objTempinsert[1].Value = ddlOrientation.SelectedValue;

                        objTempinsert[2] = new SqlParameter("@CardAlignment", SqlDbType.VarChar, 1);
                        objTempinsert[2].Value = ddlAlignment.SelectedValue;

                        objTempinsert[3] = new SqlParameter("@CardTitle", SqlDbType.NVarChar, 200);
                        objTempinsert[3].Value = txtcardtitle.Text.Replace("'", "''").Trim();

                        objTempinsert[4] = new SqlParameter("@CardDesc", SqlDbType.NVarChar, 2000);
                        objTempinsert[4].Value = txtdesc.Text.Replace("'", "''").Trim();

                        objTempinsert[5] = new SqlParameter("@FileUrl", SqlDbType.VarChar);
                        objTempinsert[5].Value = Session["UPLOADMEDIACARD"];

                        objTempinsert[6] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                        objTempinsert[6].Value = Session["Filepathcard"];

                        objTempinsert[7] = new SqlParameter("@CardHeight", SqlDbType.VarChar, 1);
                        objTempinsert[7].Value = ddlcardheight.SelectedValue;

                        objTempinsert[8] = new SqlParameter("@SuggestionText", SqlDbType.NVarChar, 25);
                        objTempinsert[8].Value = txtcardtext.Text.Trim();

                        objTempinsert[9] = new SqlParameter("@SuggestionType", SqlDbType.VarChar, 1);
                        objTempinsert[9].Value = ddlcardtype.SelectedValue;

                        if (ddlcardtype.SelectedValue == "2")
                        {

                            objTempinsert[10] = new SqlParameter("@SuggestionUrl", SqlDbType.VarChar);
                            objTempinsert[10].Value = txtcardurl.Text.Trim();

                            objTempinsert[11] = new SqlParameter("@Active", SqlDbType.VarChar);
                            objTempinsert[11].Value = 1;
                        }
                        else if (ddlcardtype.SelectedValue == "3")
                        {

                            objTempinsert[10] = new SqlParameter("@SuggestionPhone", SqlDbType.VarChar, 15);
                            objTempinsert[10].Value = txtcardphone.Text.Trim();

                            objTempinsert[11] = new SqlParameter("@Active", SqlDbType.VarChar);
                            objTempinsert[11].Value = 1;

                        }
                        else if (ddlcardtype.SelectedValue == "4")
                        {

                            objTempinsert[10] = new SqlParameter("@SuggestionLatitude", SqlDbType.VarChar, 100);
                            objTempinsert[10].Value = txtcrdlatitude.Text.Trim();

                            objTempinsert[11] = new SqlParameter("@SuggestionLongitude", SqlDbType.VarChar, 100);
                            objTempinsert[11].Value = txtcrdlongitude.Text.Trim();

                        }
                        else
                        {
                            objTempinsert[10] = new SqlParameter("@SuggestionLatitude", SqlDbType.VarChar, 100);
                            objTempinsert[10].Value = "";

                            objTempinsert[11] = new SqlParameter("@SuggestionLongitude", SqlDbType.VarChar, 100);
                            objTempinsert[11].Value = "";
                        }

                        objTempinsert[12] = new SqlParameter("@TemplateName", SqlDbType.NVarChar, 100);
                        objTempinsert[12].Value = txttempname.Text.Replace("'", "''").Trim();

                        objTempinsert[13] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                        objTempinsert[13].Value = UserID;

                        objTempinsert[14] = new SqlParameter("@intresult", SqlDbType.Int);
                        objTempinsert[14].Direction = ParameterDirection.Output;

                        int intresult = rcscode.database.ExecuteSqlSP(objTempinsert, strQuery);
                        if (intresult > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Created Successfully !!');", true);
                            Session.Remove("UPLOADMEDIACARD");
                            Session.Remove("Filepathcard");


                            DataTable dtcardsg = ViewState["cardSgTable"] as DataTable;
                            if (dtcardsg != null)
                            {
                                foreach (DataRow row in dtcardsg.Rows)
                                {
                                    string str = "";
                                    str = @"declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId,active)" +
                                        "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString() + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', @id,1)";
                                    database.ExecuteNonQuery(str);
                                }
                            }
                            //Card = "Tmp_CardS_" + UserID;
                            //string sql2 = "";
                            //sql2 = @"declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId)" +
                            //    "Select suggetiontype, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, @id from " + Card + "";
                            //database.ExecuteNonQuery(sql2);
                            //database.ExecuteNonQuery("drop table "+ Card + "");

                            suggestionCard.Style.Add("display", "none");
                            Session.Remove("Filepathcard");
                            Session.Remove("UPLOADMEDIACARD");

                        }


                    }
                    catch (Exception EX)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                    }
                }


                //---------------For Carousel-------

                if (ddlRCSType.SelectedValue == "5")
                {

                    string sql = "", sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "";
                    string Carouselcard1 = "Tmp_Carouselcard1_" + UserID;
                    string user1 = "Tmp_Template_" + UserID;
                    string user2 = "Tmp_Templateheader_" + UserID;
                    string Carouselsuggection = "Tmp_Carouselsuggection_" + UserID;


                    DataTable dtCC = database.GetDataTable("if exists (select * from sys.tables where name='" + user1 + "') select * from " + user1 + "");
                    // database.ExecuteNonQuery(Templateid);
                    if (dtCC.Rows.Count < 2)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please add atleast 2 cards to save carousel Template !!!');", true);
                        return;
                    }
                    DataTable dtcarousel = database.GetDataTable("if exists (select * from sys.tables where name='" + Carouselsuggection + "') select * from " + Carouselsuggection + "");
                    // database.ExecuteNonQuery(Templateid);
                    //if (dtcarousel.Rows.Count <=0)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select First Carousel Suggection !!!');", true);
                    //    return;
                    //}
                    sql = @"insert into RcsTemplateHeader(RCSType,TemplateName, Active, Userid, CreatedDate) " +
                   "select RCSType,TemplateName, Active, Userid, CreatedDate from " + user2 + "";
                    //sql += @" declare @templateid int Select  @templateid=max(templateid) from RcsTemplateHeader where userid='" + UserID + "' insert into CarouselSuggetion (SuggetionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude,CarouselId) " +
                    //"select SuggestionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude,@templateid from " + Carouselsuggection + " ";
                    database.ExecuteNonQuery(sql);
                    DataTable dtcardsg = ViewState["SgoverallTable"] as DataTable;
                    if (dtcardsg != null)
                    {
                        foreach (DataRow row in dtcardsg.Rows)
                        {
                            string str = "";
                            str = @" declare @templateid int Select  @templateid=max(templateid) from RcsTemplateHeader where userid='" + UserID + "' declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CarouselSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CarouselId,active)" +
                                "values('" + row["suggetiontype"].ToString() + "', N'" + row["SuggestionText"].ToString() + "', '" + row["SuggestionUrl"].ToString() + "', '" + row["SuggestionPhone"].ToString() + "', '" + row["SuggestionLatitude"].ToString() + "', '" + row["SuggestionLongitude"].ToString() + "', @templateid,1)";
                            database.ExecuteNonQuery(str);
                        }
                    }



                    foreach (DataRow row in dtCC.Rows)
                    {


                        sql1 = @"declare @templateid int Select  @templateid=max(templateid) from RcsTemplateHeader where userid='" + UserID + "' insert into RcsTemplateDetail (RCSType, TemplateText, FileUrl, FilePath, CardWidth, CardTitle, CardDesc, CardHeight, SuggestionText, SuggestionType, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID,TemplateID ,CreatedDate,Active) " +
                           "select RCSType, TemplateText, FileUrl, FilePath, CardWidth, CardTitle, CardDesc, CardHeight, SuggestionText, SuggetionType, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID,id ,'" + DateTime.Now.ToString("yyyy - MM - dd", CultureInfo.InvariantCulture) + "',1 from " + user1 + " where id= " + row["ID"] + "" +
                           "declare @id int Select  @id=max(id) from RcsTemplateDetail update RcsTemplateDetail set TemplateID=@templateid where ID=@id ";
                        database.ExecuteNonQuery(sql1);

                        sql2 = @"declare @id int Select  @id=max(id) from RcsTemplateDetail where userid='" + UserID + "' insert into CardSuggetion (SuggetionType,SuggestionText,SuggestionUrl,SuggestionPhone,SuggestionLatitude,SuggestionLongitude,CardId)" +
                            "Select suggetiontype, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, @id from " + Carouselcard1 + " where CtemplateID =" + row["ID"] + " ";
                        database.ExecuteNonQuery(sql2);


                    }


                    sql3 = "drop table " + user1 + "";
                    sql4 = "drop table " + user2 + "";
                    sql5 = "drop table " + Carouselcard1 + "";
                    //sql6 = "drop table " + Carouselsuggection +"";

                    database.ExecuteNonQuery(sql3);
                    database.ExecuteNonQuery(sql4);
                    database.ExecuteNonQuery(sql5);




                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('carousel Template Save Successfully');", true);


            }

            Reset();
            carouselTypes();
            CardTypes();
            ChooseTypes();
            suggectionCardCrasoul.Style.Add("display", "none");
            suggectionCrasoul.Style.Add("display", "none");
        }

        protected void UploadImg_Click(object sender, EventArgs e)
        {
            if (FileUpload2.HasFile)
            {
                if (FileUpload2.FileName != "")
                {
                    string Extension = Path.GetExtension(FileUpload2.PostedFile.FileName);
                    string en = Extension.ToUpper();

                    if (!(en.Contains("JPG") || en.Contains("JPEG") || en.Contains("PNG") || en.Contains("GIF") || en.Contains("TIFF") || en.Contains("BMP") || en.Contains("JFIF")))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload image file of type ( JPG / JPEG / PNG / GIF / TIFF / BMP / JFIF )');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload2.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    //int size = FileUpload2.PostedFile.ContentLength;
                    decimal size = Math.Round(((decimal)FileUpload2.PostedFile.ContentLength / (decimal)1024), 2);
                    string dimension = width.ToString() + "*" + height.ToString();
                    if (size > 2048)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insert image with dimension 1440*720 and size less then 2 mb.');", true);

                        return;
                    }

                    //if (!(en.Contains("MP4") || en.Contains("MOV")))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                    //    lblUploading.Text = "Upload rejected.";
                    //    return;
                    //}
                    string FolderPath = "MEDIAUpload/";

                    Session["UPLOADFILENMEXT"] = Extension;
                    string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    string FilePath = Server.MapPath(FolderPath + FN + Extension);
                    Session["filepath2"] = FilePath;
                    FileUpload2.SaveAs(FilePath);
                    Label1.Text = FileUpload2.PostedFile.FileName;
                    Session["UPLOADMEDIA2"] = UploadPath + FolderPath + FN + Extension;
                    Session["UPLOADMEDIAFN"] = FN + Extension;
                    lblImagepath.Text = FileUpload2.PostedFile.FileName;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                    lblUploading.Text = "Uploaded successfully.";
                    Label6.Text = "Uploaded successfully.";
                    lblupImg.Text = "Uploaded successfully.";
                    Label6.ForeColor = System.Drawing.Color.Green;
                    Label6.Font.Bold = true;

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Please Select File !!');", true);
            }
        }

        private void ChooseTypes()
        {
            ddltypes.Items[0].Attributes.CssStyle.Add("margin-right", "30px;");
            if (ddltypes.SelectedValue == "0")
            {
                openurl.Style.Add("display", "none");
                dialphone.Style.Add("display", "none");
                divshowlocation.Style.Add("display", "none");
                dicccs.Style.Add("display", "none");
            }
            if (ddltypes.SelectedValue == "1")
            {
                openurl.Style.Add("display", "none");
                dialphone.Style.Add("display", "none");
                divshowlocation.Style.Add("display", "none");
                dicccs.Style.Add("display", "show");
            }
            if (ddltypes.SelectedValue == "2")
            {
                openurl.Style.Add("display", "show");
                dialphone.Style.Add("display", "none");
                divshowlocation.Style.Add("display", "none");
                //divcarousel.Style.Add("display", "none");
                dicccs.Style.Add("display", "show");

            }
            if (ddltypes.SelectedValue == "3")
            {
                openurl.Style.Add("display", "none");
                dialphone.Style.Add("display", "show");
                divshowlocation.Style.Add("display", "none");
                //divcarousel.Style.Add("display", "none");
                dicccs.Style.Add("display", "show");

            }
            if (ddltypes.SelectedValue == "4")
            {
                openurl.Style.Add("display", "none");
                dialphone.Style.Add("display", "none");
                divshowlocation.Style.Add("display", "true");
                //divcarousel.Style.Add("display", "none");
                dicccs.Style.Add("display", "show");
            }
            if (ddltypes.SelectedValue == "5")
            {
                openurl.Style.Add("display", "none");
                dialphone.Style.Add("display", "none");
                divshowlocation.Style.Add("display", "none");
                //divcarousel.Style.Add("display", "show");
                dicccs.Style.Add("display", "show");
            }
        }

        protected void ddltypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChooseTypes();
        }

        private void CardTypes()
        {
            ddlcardtype.Items[0].Attributes.CssStyle.Add("margin-right", "30px;");
            if (ddlcardtype.SelectedValue == "0")
            {
                crdopenurl.Style.Add("display", "none");
                crddialphone.Style.Add("display", "none");
                crdshowlocation.Style.Add("display", "none");
                DIVTEXT.Style.Add("display", "none");
            }
            if (ddlcardtype.SelectedValue == "1")
            {
                crdopenurl.Style.Add("display", "none");
                crddialphone.Style.Add("display", "none");
                crdshowlocation.Style.Add("display", "none");
                DIVTEXT.Style.Add("display", "show");
            }
            if (ddlcardtype.SelectedValue == "2")
            {
                crdopenurl.Style.Add("display", "show");
                crddialphone.Style.Add("display", "none");
                crdshowlocation.Style.Add("display", "none");
                DIVTEXT.Style.Add("display", "show");
            }
            if (ddlcardtype.SelectedValue == "3")
            {
                crdopenurl.Style.Add("display", "none");
                crddialphone.Style.Add("display", "show");
                crdshowlocation.Style.Add("display", "none");
                DIVTEXT.Style.Add("display", "show");
            }
            if (ddlcardtype.SelectedValue == "4")
            {

                openurl.Style.Add("display", "none");
                crddialphone.Style.Add("display", "none");
                crdopenurl.Style.Add("display", "none");
                crdshowlocation.Style.Add("display", "true");
                DIVTEXT.Style.Add("display", "show");
            }
            if (ddlcardtype.SelectedValue == "5")
            {
                crdopenurl.Style.Add("display", "none");
                crddialphone.Style.Add("display", "none");
                crdshowlocation.Style.Add("display", "none");
                DIVTEXT.Style.Add("display", "none");
            }
        }

        protected void ddlcardtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            CardTypes();
        }


        protected void Lkbtnvideo_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.FileName != "")
                {
                    string Extension = Path.GetExtension(FileUpload1.FileName);
                    string en = Extension.ToUpper();

                    if (!(en.Contains("MP4") || en.Contains("MOV")))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    decimal size = Math.Round(((decimal)FileUpload1.PostedFile.ContentLength / (decimal)1024), 2);
                    //int size = FileUpload1.PostedFile.ContentLength;

                    if (size > 10240)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insert Video size less then 10 mb.');", true);
                        Reset();
                        return;
                    }
                    string FolderPath = "MEDIAUpload/";

                    Session["UPLOADFILENMEXT"] = Extension;
                    string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    string FilePath = Server.MapPath(FolderPath + FN + Extension);
                    Session["filepathvdo"] = FilePath;
                    FileUpload1.SaveAs(FilePath);
                    Session["UPLOADMEDIAvdo"] = UploadPath + FolderPath + FN + Extension;
                    Session["UPLOADMEDIAFN"] = FN + Extension;
                    lblvideoname.Text = FileUpload1.FileName;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                    lblUploading.Text = "Uploaded successfully.";
                    lblvideoS.Text = "Uploaded successfully.";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Please Upload File !!');", true);
            }
        }

        protected void carofile_Click(object sender, EventArgs e)
        {
            if (FileUpload3.HasFile)
            {
                if (FileUpload3.FileName != "")
                {
                    string Extension = Path.GetExtension(FileUpload3.PostedFile.FileName);
                    string en = Extension.ToUpper();

                    if (!(en.Contains("JPG") || en.Contains("JPEG") || en.Contains("PNG") || en.Contains("GIF") || en.Contains("TIFF") || en.Contains("BMP") || en.Contains("JFIF")))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload image file of type ( JPG / JPEG / PNG / GIF / TIFF / BMP / JFIF )');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload3.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round(((decimal)FileUpload3.PostedFile.ContentLength / (decimal)1024), 2);
                    //int size = FileUpload3.PostedFile.ContentLength;

                    string dimension = width.ToString() + "*" + height.ToString();
                    if (size > 2048)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insert image with dimension 1440*720 and size less then 2 mb.');", true);

                        return;
                    }
                    //if (!(en.Contains("MP4") || en.Contains("MOV")))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                    //    lblUploading.Text = "Upload rejected.";
                    //    return;
                    //}
                    string FolderPath = "MEDIAUpload/";

                    Session["UPLOADFILENMEXT"] = Extension;
                    string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    string FilePath = Server.MapPath(FolderPath + FN + Extension);
                    Session["filepathcarosel"] = FilePath;
                    FileUpload3.SaveAs(FilePath);
                    Session["UPLOADMEDIACAROUSEL"] = UploadPath + FolderPath + FN + Extension;
                    Session["UPLOADMEDIAFN"] = FN + Extension;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                    lblUploading.Text = "Uploaded successfully.";
                    Label1.Text = FileUpload3.PostedFile.FileName;
                    Label6.Text = "Uploaded successfully.";
                    Label6.ForeColor = System.Drawing.Color.Green;
                    Label6.Font.Bold = true;
                    //bindcardDetails();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Please Select Image!!');", true);
                Label1.Text = "";
                Label6.Text = "";

            }
        }

        protected void cardbtnfile_Click(object sender, EventArgs e)
        {
            if (FileUpload5.HasFile)
            {
                if (FileUpload5.FileName != "")
                {
                    string Extension = Path.GetExtension(FileUpload5.PostedFile.FileName);
                    string en = Extension.ToUpper();

                    if (!(en.Contains("JPG") || en.Contains("JPEG") || en.Contains("PNG") || en.Contains("GIF") || en.Contains("TIFF") || en.Contains("BMP") || en.Contains("JFIF")))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload image file of type ( JPG / JPEG / PNG / GIF / TIFF / BMP / JFIF )');", true);
                        lblUploading.Text = "Upload rejected.";
                        return;
                    }
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload5.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    //int size = FileUpload5.PostedFile.ContentLength;
                    decimal size = Math.Round(((decimal)FileUpload5.PostedFile.ContentLength / (decimal)1024), 2);
                    string dimension = width.ToString() + "*" + height.ToString();
                    if (size > 2048)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insert image with dimension 1440*720 and size less then 2 mb.');", true);

                        return;
                    }
                    //if (!(en.Contains("MP4") || en.Contains("MOV")))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please upload video file of type ( MP4 / MOV )');", true);
                    //    lblUploading.Text = "Upload rejected.";
                    //    return;
                    //}
                    string FolderPath = "MEDIAUpload/";

                    Session["UPLOADFILENMEXT"] = Extension;
                    string FN = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    string FilePath = Server.MapPath(FolderPath + FN + Extension);
                    Session["Filepathcard"] = FilePath;
                    FileUpload5.SaveAs(FilePath);

                    Session["UPLOADMEDIACARD"] = UploadPath + FolderPath + FN + Extension;
                    lblcardimage.Text = FileUpload5.PostedFile.FileName;
                    Session["UPLOADMEDIAFN"] = FN + Extension;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File Uploaded Successfully');", true);
                    lblcardimageSucess.Text = "Uploaded successfully.";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Please Upload File !!');", true);
            }
        }



        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string user1 = "Tmp_Template_" + UserID;
            string query = "";
            string Carouselsuggection = "Tmp_Carouselsuggection_" + UserID;
            query = @"if exists (select * from sys.tables where name='" + user1 + @"') Select * from " + user1 + @"";
            DataTable dttemp = database.GetDataTable(query);
            if (dttemp.Rows.Count < 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please add atleast 2 cards to save carousel Template.');", true);
                return;
            }
            DataTable dtCC1 = database.GetDataTable("if exists(select * from sys.tables where name = '" + Carouselsuggection + @"') BEGIN select *from  " + Carouselsuggection + "  end ");
            if (dtCC1.Rows.Count > 0)
            {
                gvcrauselS.DataSource = null;
                gvcrauselS.DataSource = dtCC1;
                gvcrauselS.DataBind();
                // GridFormat(gvSType);
            }
            //addnewhide.Disabled = true;
            btnnewcard.Visible = false;
            btnnewcard.CssClass = "btn btn-primary btn-icon-split  mt-2";
            //btnnewcard.Attributes.Add("Style", "btn btn-primary btn-icon-split  mt-2");
            //btnnewcard.Attributes.Add("class", "btn btn-primary btn-icon-split  mt-2");
            //btnnewcard.CssClass = "btn btn-primary btn-icon-split  mt-2";
            btnSave.Visible = true;
            divcarouselsuggsction.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
            divcarouselsuggsction.Style.Add("display", "show");

            // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please note you will not be able to add more cards after clicking submit  Are you sure that you want to proceed with sumbit !!');", true);
        }

        protected void btnnewcard_Click(object sender, EventArgs e)
        {

            if (ddlRCSType.SelectedValue == "5")
            {
                if (ddlwidth.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Card Width');", true);
                    return;
                }
                if (txttempname.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Template Name');", true);
                    return;
                }
                if (txttitle.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Title');", true);
                    return;
                }
                if (txtdescription.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Description');", true);
                    return;
                }
                if (ddlheight.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Height');", true);
                    return;
                }


                if (Session["UPLOADMEDIACAROUSEL"] == null && Session["filepathcarosel"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload Image');", true);
                    return;
                }
                //if (txttext.Text.Trim() == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Text');", true);
                //    return;
                //}

                try
                {
                    DataTable dttmp = ob.Inserttempdata(UserID, ddlRCSType.SelectedValue, txttempname.Text.Replace("'", "''"), txtTempText.Text.Replace("'", "''"), Session["UPLOADMEDIACAROUSEL"].ToString(), Session["filepathcarosel"].ToString(), ddlwidth.SelectedValue, ddlOrientation.SelectedValue,
                    ddlAlignment.SelectedValue, txttitle.Text.Replace("'", "''").Trim(), txtdescription.Text.Replace("'", "''").Trim(), ddlheight.SelectedValue, txttext.Text.Trim(), ddltypes.SelectedValue,
                    txturl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text);



                    if (dttmp.Rows.Count >= 2)
                    {
                        btnsubmit.Visible = true;
                    }
                    if (dttmp.Rows.Count >= 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can create only 10 card');", true);
                        return;
                    }
                    Reset();
                    bindcardDetails();
                    //ViewState["carocount"] = Convert.ToString (dttmp.Rows.Count);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Card Added Successfully !!');", true);
                    Session.Remove("UPLOADMEDIACAROUSEL");
                    Session.Remove("filepathcarosel");

                    string Carouselcard = "";
                    string sql1 = "";
                    Carouselcard = "Tmp_Carouselcard_" + UserID;

                    sql1 = @"if exists (select * from sys.tables where name='" + Carouselcard + @"') BEGIN select * from  " + Carouselcard + "  end ";
                    DataTable dtCC = database.GetDataTable(sql1);

                    if (dtCC.Rows.Count > 0)
                    {
                        gvSType.DataSource = null;
                        gvSType.DataSource = dtCC;
                        gvSType.DataBind();
                        // GridFormat(gvSType);
                    }
                    else
                    {
                        suggectionCardCrasoul.Style.Add("display", "none");
                    }

                }
                catch (Exception EX)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Fill Valid Details !!');", true);
                }
            }

        }

        protected void Reset()
        {
            txttempname.Text = "";
            txtTempText.Text = "";
            Label1.Text = "";
            lblupImg.Text = "";
            Label6.Text = "";
            lblImagepath.Text = "";
            FileUpload1.Dispose();
            FileUpload2.Dispose();
            FileUpload3.Dispose();
            FileUpload4.Dispose();
            FileUpload5.Dispose();
            ddlOrientation.SelectedValue = "0";
            ddlAlignment.SelectedValue = "0";
            txtcardtitle.Text = "";
            txtdesc.Text = "";
            ddlcardheight.SelectedValue = "0";
            ddlcardtype.SelectedValue = "0";
            txtcardurl.Text = "";
            txtcardphone.Text = "";
            txtcrdlatitude.Text = "";
            txtcrdlongitude.Text = "";
            //ddlwidth.SelectedValue = "0";
            txttitle.Text = "";
            txtdescription.Text = "";
            //ddlheight.SelectedValue = "0";
            ddltypes.SelectedValue = "0";
            ddlCarouselSuggetion.SelectedValue = "0";
            txturl.Text = "";
            txtphone.Text = "";
            txtlatitude.Text = "";
            txtlongitude.Text = "";
            txttext.Text = "";
            lblcreatedcard.Text = "0";
            Label1.Text = "";
            Label6.Text = "";
            lblcardimage.Text = "";
            lblcardimageSucess.Text = "";
            lblUploading.Text = "";
            lblvideoS.Text = "";
            lblvideoname.Text = "";
            txttempname.ReadOnly = false;


        }

        protected void lnkReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {



            btnSave.Visible = true;
            lbtnFind.Visible = true;
            lbtnAdd.Visible = false;

            span.InnerText = "Save Template";
            ispan.Attributes.Add("class", "fas fa-save");
            span1.Attributes.Add("class", "text-success");
            ddlRCSType.Style.Add("pointer-events", "show");
            span.Attributes.Add("class", "text-success font-weight-bold");
            divTemplate.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
            divSearch.Attributes.Add("class", "d-none");
            divcarouselsuggsction.Style.Add("display", "none");
            divmain.Visible = true;
            Reset();
            ddlheight.Style.Add("pointer-events", "show");
            ddlwidth.Style.Add("pointer-events", "show");


            ddlRCSType.SelectedValue = "1";
            ddlRCSType_SelectedIndexChanged1(sender, e);
            // Response.Redirect("send_rcs_user_template.aspx");
        }

        protected void lbtnFind_Click(object sender, EventArgs e)
        {
            btnSave.Visible = false;

            btnSave.Visible = false;
            lbtnFind.Visible = false;
            lbtnAdd.Visible = true;

            span.InnerText = "Save Template";
            ispan.Attributes.Add("class", "fas fa-save");
            span1.Attributes.Add("class", "text-success");

            span.Attributes.Add("class", "text-success font-weight-bold");
            divTemplate.Attributes.Add("class", "d-none");
            divcardetail.Attributes.Add("class", "d-none");
            divSearch.Attributes.Add("class", "card card-body mb-4 bg-primary border-light shadow-soft");
            grv.DataSource = null;
            grv.DataBind();
            lvCrousel.Items.Clear();
            lvCrousel.DataSource = null;
            lvCrousel.DataBind();
            ddlSelectrcstype.SelectedValue = "1";
        }

        protected void lnkShow_Click(object sender, EventArgs e)
        {

            GetData(Convert.ToInt32(ddlSelectrcstype.SelectedValue));
            divcardetail.Attributes.Add("class", "d-none");
        }
        public void GetData(int Id)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["RCSUserID"]);

            DataTable dt = ob.GetRcsSearch(user, Id);

            lvCrousel.DataSource = null;
            //lvCrousel.DataSource = dt;
            lvCrousel.DataBind();

            grv.DataSource = null;
            grv.DataSource = dt;
            grv.DataBind();


            //GridFormat(dt);

            Session["analyticsdata"] = dt;
        }

        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv.TopPagerRow != null)
            {
                grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv.BottomPagerRow != null)
            {
                grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void grv_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "View")
            {
                grv.EditIndex = -1;
                ViewState["CId"] = e.CommandArgument;
                divcardetail.Attributes.Add("class", "col-xl-12 col-lg-12");
                rcscard(Convert.ToInt32(e.CommandArgument));
                SetFocus("divcardetail");
                //lnkShow_Click(sender, e);
                string usertype = Convert.ToString(Session["UserType"]);
                string user = Convert.ToString(Session["RCSUserID"]);

                DataTable dt = ob.GetRcsGridS(user, Convert.ToInt32(e.CommandArgument));
                //lvCrousel.DataSource = null;
                //lvCrousel.DataBind();
                grv.DataSource = null;

                grv.DataSource = dt;
                grv.DataBind();
                //GridFormat(dt);
            }
            if (e.CommandName == "delete")
            {
                grv.EditIndex = -1;
                string sql = "update RcsTemplateHeader set active=0 from RcsTemplateHeader rh,RcsTemplateDetail rd  where rh.templateid=rd.templateid and rh.templateid='" + e.CommandArgument + "' update RcsTemplateDetail set  active=0 from RcsTemplateHeader rh,RcsTemplateDetail rd  where rh.templateid=rd.templateid and rh.templateid='" + e.CommandArgument + "'";
                database.ExecuteNonQuery(sql);
                lnkShow_Click(sender, e);


            }
            if (e.CommandName == "popup")
            {
                ViewState["sendtemplateid"] = e.CommandArgument;
                mpetestpopup.Show();
            }
        }
        protected void rcscard(int id)
        {
            string sql1 = "";

            sql1 = @"select th.templatename,(th.templateid) as tempid,td.* from RcsTemplateHeader th,RcsTemplateDetail td where th.templateid=td.templateid and th.userid='" + UserID + "' and th.templateid='" + id + "' and th.active=1 and td.active=1 order by td.id asc;";


            DataTable dtc = new DataTable();

            dtc = database.GetDataTable(sql1);
            if (dtc.Rows.Count > 0)
            {
                txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                lvCrousel.DataSource = dtc;

                lvCrousel.DataBind();
                if (ddlSelectrcstype.SelectedValue == "5")
                {
                    addCard.Visible = true;

                }
                else
                {
                    addCard.Visible = false;



                }


            }

        }

        protected void lvCrousel_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            if (e.CommandName == "Edit")
            {



                string[] commandsrgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                ViewState["Did"] = commandsrgs[0];
                ViewState["Tid"] = commandsrgs[1];
                divTemplate.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
                divSearch.Attributes.Add("class", "d-none");
                BindTemplateid(Convert.ToInt32(ViewState["Did"]));
                divSearch.Attributes.Add("class", "d-none");
                ViewState["IsEdit"] = true;
                string sql1 = "";

                sql1 = @"select th.templatename,td.* from RcsTemplateHeader th,RcsTemplateDetail td where th.templateid=td.templateid and th.userid='" + UserID + "' and td.id='" + ViewState["Did"].ToString() + "' and td.active=1;";


                DataTable dtc = new DataTable();
                dtc = database.GetDataTable(sql1);
                if (dtc.Rows.Count > 0)
                {
                    btnSave.Visible = true;
                    lbtnFind.Visible = true;
                    lbtnAdd.Visible = true;


                    ddlRCSType.SelectedValue = dtc.Rows[0]["RCStype"].ToString();
                    ddlRCSType_SelectedIndexChanged1(sender, e);
                    txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["RCStype"].ToString() == "") ddlRCSType.SelectedValue = "1"; else ddlRCSType.SelectedValue = dtc.Rows[0]["RCStype"].ToString();
                    ddlRCSType_SelectedIndexChanged1(sender, e);
                    //if (dtc.Rows[0]["suggestiontype"].ToString() == "") ddltypes.SelectedValue = "0"; else ddltypes.SelectedValue = dtc.Rows[0]["suggestiontype"].ToString();
                    // ddltypes_SelectedIndexChanged(sender, e);
                    if (dtc.Rows[0]["cardwidth"].ToString() == "") ddlwidth.SelectedValue = "0"; else ddlwidth.SelectedValue = dtc.Rows[0]["cardwidth"].ToString();
                    if (dtc.Rows[0]["templatetext"].ToString() == null) txtTempText.Text = ""; else txtTempText.Text = dtc.Rows[0]["templatetext"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["cardtitle"].ToString() == null) txttitle.Text = ""; else txttitle.Text = dtc.Rows[0]["cardtitle"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["cardtitle"].ToString() == null) txtcardtitle.Text = ""; else txtcardtitle.Text = dtc.Rows[0]["cardtitle"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["carddesc"].ToString() == null) txtdescription.Text = ""; else txtdescription.Text = dtc.Rows[0]["carddesc"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["carddesc"].ToString() == null) txtdesc.Text = ""; else txtdesc.Text = dtc.Rows[0]["carddesc"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["cardheight"].ToString() == "") ddlheight.SelectedValue = "0"; else ddlheight.SelectedValue = dtc.Rows[0]["cardheight"].ToString();
                    if (dtc.Rows[0]["cardheight"].ToString() == "") ddlcardheight.SelectedValue = "0"; else ddlcardheight.SelectedValue = dtc.Rows[0]["cardheight"].ToString();
                    //if (dtc.Rows[0]["suggestiontype"].ToString() == "") ddlcardtype.SelectedValue = "0"; else ddlcardtype.SelectedValue = dtc.Rows[0]["suggestiontype"].ToString();
                    //ddlcardtype_SelectedIndexChanged(sender, e);

                    if (dtc.Rows[0]["suggestiontext"].ToString() == null) txttext.Text = ""; else txttext.Text = dtc.Rows[0]["suggestiontext"].ToString();
                    if (dtc.Rows[0]["suggestiontext"].ToString() == null) txtcardtext.Text = ""; else txtcardtext.Text = dtc.Rows[0]["suggestiontext"].ToString();


                    if (dtc.Rows[0]["templatename"].ToString() == null) txttempname.Text = ""; else txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                    if (dtc.Rows[0]["cardwidth"].ToString() == "") ddlwidth.SelectedValue = "1"; else ddlwidth.SelectedValue = dtc.Rows[0]["cardwidth"].ToString();
                    if (dtc.Rows[0]["cardOrientation"].ToString() == "") ddlOrientation.SelectedValue = "1"; else ddlOrientation.SelectedValue = dtc.Rows[0]["cardOrientation"].ToString();
                    if (dtc.Rows[0]["cardAlignment"].ToString() == "") ddlAlignment.SelectedValue = "1"; else ddlAlignment.SelectedValue = dtc.Rows[0]["cardAlignment"].ToString();
                    //ddltypes_SelectedIndexChanged(sender, e);

                    if (dtc.Rows[0]["suggestionphone"].ToString() == null) txtphone.Text = ""; else txtphone.Text = dtc.Rows[0]["suggestionphone"].ToString();
                    if (dtc.Rows[0]["SuggestionLatitude"].ToString() == null) txtlatitude.Text = ""; else txtlatitude.Text = dtc.Rows[0]["SuggestionLatitude"].ToString();
                    if (dtc.Rows[0]["SuggestionLongitude"].ToString() == null) txtlongitude.Text = ""; else txtlongitude.Text = dtc.Rows[0]["SuggestionLongitude"].ToString();
                    if (dtc.Rows[0]["Suggestionurl"].ToString() == null) txturl.Text = ""; else txturl.Text = dtc.Rows[0]["Suggestionurl"].ToString();
                    if (dtc.Rows[0]["Suggestionurl"].ToString() == null) txtcardurl.Text = ""; else txtcardurl.Text = dtc.Rows[0]["Suggestionurl"].ToString();
                    if (dtc.Rows[0]["FileUrl"].ToString() != null)
                    {


                        Session["UPLOADMEDIACAROUSEL"] = dtc.Rows[0]["FileUrl"].ToString();
                        Session["UPLOADMEDIACARD"] = dtc.Rows[0]["FileUrl"].ToString();
                    }
                    if (dtc.Rows[0]["FilePath"].ToString() != null)
                    {
                        Session["filepathcarosel"] = dtc.Rows[0]["FilePath"].ToString();
                        Session["Filepathcard"] = dtc.Rows[0]["FilePath"].ToString();
                        Label1.Text = dtc.Rows[0]["FilePath"].ToString();
                        lblcardimage.Text = dtc.Rows[0]["FilePath"].ToString();
                    }
                    txttempname.ReadOnly = true;
                    ddlRCSType.Style.Add("pointer-events", "none");
                    ddlwidth.Style.Add("pointer-events", "none");
                    ddlheight.Style.Add("pointer-events", "none");

                    divmain.Visible = true;
                    btngrid.Visible = false;

                    //btnSave.Visible = true;
                    //lbtnAdd.Visible = false;
                    //lbtnFind.Visible = false;

                    ViewState["IsDelete"] = false;
                    ViewState["IsAddcard"] = false;
                    span.InnerText = "Update Template";
                    ispan.Attributes.Add("class", "fas fa-save");
                    span1.Attributes.Add("class", "text-success");
                    lbtnAdd.CssClass = "btn btn-primary btn-icon-split  mt-2 mb-3";
                    lbtnFind.CssClass = "btn btn-primary btn-icon-split  mt-2 mb-3";
                    span.Attributes.Add("class", "text-success font-weight-bold");

                }
            }

            if (e.CommandName == "Delete")
            {
                Util obu = new Util();
                string[] commandsrgs = e.CommandArgument.ToString().Split(new char[] { ',' });

                try
                {
                    string user1 = "Tmp_DeleteCrousel_" + UserID;
                    string sql = "";
                    string sql1 = "";
                    //string sql2 = "";
                    sql = @"select * from " + user1 + "";
                    //sql2 = @"IF EXISTS( SELECT ID FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo." + user1 + "')) BEGIN select count(*) from " + user1 + " END;"; ;
                    sql1 = @"IF EXISTS( SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo." + user1 + "')) BEGIN select count(*) from " + user1 + " END;";

                    int count = Convert.ToInt32(database.GetScalarValue("Select Count(*) from RcsTemplateDetail where userid='" + UserID + "' and templateid='" + commandsrgs[1] + "'and active=1"));
                    int count1 = Convert.ToInt32(database.GetScalarValue(sql1));
                    string rcstype = database.GetScalarValue("Select rcstype from RcsTemplateDetail where userid='" + UserID + "' and templateid='" + commandsrgs[1] + "'and active=1 ").ToString();
                    if (rcstype == "5")
                    {
                        if ((count - count1) <= 2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not delete more then 2 !!!');", true);
                            return;
                        }
                    }


                    obu.Updattempdata(UserID, Convert.ToInt64(commandsrgs[0]), Convert.ToInt64(commandsrgs[1]));
                    span.InnerText = "Delete";
                    ispan.Attributes.Add("class", "fas fa-save");
                    span1.Attributes.Add("class", "text-success");

                    span.Attributes.Add("class", "text-success font-weight-bold");
                }
                catch (Exception EX)
                {

                }
                updatercscard(Convert.ToInt32(commandsrgs[1]));
                DataTable dt = ob.GetRcsGridS(UserID, Convert.ToInt32(commandsrgs[1]));
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                //GridFormat(dt);
                ViewState["IsDeleteTempId"] = commandsrgs[1];
                ViewState["IsDelete"] = true;
                ViewState["IsEdit"] = false;
                ViewState["IsAddcard"] = false;
                btnSave.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Deleted !!!');", true);

                //pnlPopUp_Detail_ModalPopupExtender.Show();
            }

        }
        protected void updatercscard(int id)
        {
            string sql1 = "";
            string user1 = "Tmp_DeleteCrousel_" + UserID;
            sql1 = @"IF EXISTS( SELECT  count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo." + user1 + "')) BEGIN select  count(*) from " + user1 + " END;";
            int count = Convert.ToInt32(database.GetScalarValue(sql1));
            if (count > 0)
            {
                sql1 = @"select th.templatename,(th.templateid) as tempid,td.* from RcsTemplateHeader th,RcsTemplateDetail td where th.templateid=td.templateid and th.userid='" + UserID + "' and th.templateid='" + id + "' and th.active=1 and td.active=1 and td.id not in(Select id from " + user1 + ");";
            }
            else
            {
                sql1 = @"select th.templatename,(th.templateid) as tempid,td.* from RcsTemplateHeader th,RcsTemplateDetail td where th.templateid=td.templateid and th.userid='" + UserID + "' and th.templateid='" + id + "' and th.active=1 and td.active=1 ;";
            }

            DataTable dtc = new DataTable();

            dtc = database.GetDataTable(sql1);
            if (dtc.Rows.Count > 0)
            {
                txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                lvCrousel.DataSource = dtc;
                lvCrousel.DataBind();

                //dlCustomers.DataSource = dtc;
                //dlCustomers.DataBind();


            }

        }
        protected void BindTemplateid(int Id)
        {
            string sql1 = "";

            sql1 = @"select th.templatename,td.* from RcsTemplateHeader th,RcsTemplateDetail td where th.templateid=td.templateid and th.userid='" + UserID + "' and td.id='" + Id + "' and td.active=1;";


            DataTable dtc = database.GetDataTable(sql1);
            if (dtc.Rows.Count > 0)
            {
                ddlRCSType.SelectedValue = dtc.Rows[0]["RCStype"].ToString();


            }
        }



        protected void lvCrousel_ItemEditing(object sender, ListViewEditEventArgs e)
        {

        }

        protected void lvCrousel_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LinkButton lkbtnEdit = e.Item.FindControl("lkbtnEdit") as LinkButton;
                LinkButton lbtnDelete = e.Item.FindControl("lbtnDelete") as LinkButton;
                //Literal ifram = (Literal)lvCrousel.Controls[0].FindControl("ifVideo");
                Control ifram = e.Item.FindControl("ifVideo") as Control;

                Image Img = e.Item.FindControl("Img") as Image;
                Label lblid = e.Item.FindControl("lblid") as Label;
                Label lblTitle = e.Item.FindControl("lblTitle") as Label;
                Label lbltname = e.Item.FindControl("lbltname") as Label;
                Label Label11 = e.Item.FindControl("Label11") as Label;
                Label Label1 = e.Item.FindControl("Label1") as Label;
                Label lblrcstype = e.Item.FindControl("lblrcstype") as Label;
                Label lbltempText = e.Item.FindControl("lbltempText") as Label;

                string path = database.GetScalarValue("Select fileurl from RcsTemplateDetail where id='" + lblid.Text + "'").ToString();
                Img.ImageUrl = path;
                if (lblrcstype.Text == "3")
                {
                    ifram.Visible = true;
                    lblTitle.Visible = false;
                    Label11.Visible = false;
                    Label1.Visible = false;
                    lbltname.Visible = true;
                    lbltempText.Visible = true;
                    Img.Visible = false;
                }
                if (lblrcstype.Text == "1")
                {
                    lblTitle.Visible = false;
                    Label11.Visible = false;
                    Label1.Visible = false;
                    lbltname.Visible = true;
                    lbltempText.Visible = true;
                }
                if (lblrcstype.Text != "5")
                {
                    lbtnDelete.Visible = false;
                }
                else
                {
                    lbtnDelete.Visible = true;

                }
                //if (lblrcstype.Text == "1")
                //{
                //    lbltname.Visible = true;
                //    Img.Visible = false;
                //    lblTitle.Visible = false;
                //    Label11.Visible = false;
                //    Label1.Visible = false;
                //    ifram.Visible = true;
                //}
            }
        }

        protected void grv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void lvCrousel_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }



        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            if (ViewState["CId"].ToString() == null && ViewState["CId"].ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Crousel id !!!');", true);
                return;
            }
            string sql1 = "";
            sql1 = @"Select rh.templateid, rd.*,rh.* from RcsTemplateDetail rh,RcsTemplateHeader rd where rh.templateid=rd.templateid and rh.templateid='" + ViewState["CId"] + "' and rh.userid='" + UserID + "' and rh.active=1;";
            DataTable dtc = new DataTable();
            dtc = database.GetDataTable(sql1);
            if (dtc.Rows.Count >= 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add card. Already 10 card created !!!');", true);
                return;
            }


            divTemplate.Attributes.Add("class", "card bg-primary border-light shadow-soft mb-4");
            divSearch.Attributes.Add("class", "d-none");
            //BindTemplateid( Convert.ToInt32(e.CommandArgument));
            divSearch.Attributes.Add("class", "d-none");




            if (dtc.Rows.Count > 0)
            {
                Reset();
                txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                ddlRCSType.SelectedValue = dtc.Rows[0]["rcstype"].ToString();

                //ddlRCSType_SelectedIndexChanged1(sender, e);
                if (ddlRCSType.SelectedValue == "5")
                {
                    txttempname.Text = dtc.Rows[0]["templatename"].ToString().Replace("''", "'");
                    ddlwidth.SelectedValue = dtc.Rows[0]["cardwidth"].ToString();
                    ddlheight.SelectedValue = dtc.Rows[0]["cardheight"].ToString();

                    txttempname.ReadOnly = true;
                    ddlwidth.Style.Add("pointer-events", "none");
                    ddlheight.Style.Add("pointer-events", "none");
                    ddlRCSType.Style.Add("pointer-events", "none");
                }
                ddlRCSType_SelectedIndexChanged1(sender, e);
                ViewState["templateid"] = dtc.Rows[0]["templateid"].ToString();
                ViewState["IsAddcard"] = true;
                ViewState["IsDelete"] = false;
                ViewState["IsEdit"] = false;
                btngrid.Visible = false;
                btnSave.Visible = true;
                divmain.Visible = true;
                lbtnFind.Visible = false;
                lbtnAdd.Visible = false;
                lbtnAdd.CssClass = "btn btn-primary btn-icon-split  mt-2 mb-3";
                lbtnFind.CssClass = "btn btn-primary btn-icon-split  mt-2 mb-3";
                divcarouselsuggsction.Style.Add("display", "none");
                span.InnerText = "Add Card";
                ispan.Attributes.Add("class", "fas fa-save");
                span1.Attributes.Add("class", "text-success");


            }

        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LinkButton lkbtnEdit = e.Item.FindControl("lkbtnEdit") as LinkButton;
                Image Img = e.Item.FindControl("Img") as Image;
                Label lblid = e.Item.FindControl("lblid1") as Label;
                string path = database.GetScalarValue("Select fileurl from RcsTemplateDetail where id='" + lblid.Text + "'").ToString();
                Img.ImageUrl = path;
            }
        }

        protected void txttempname_TextChanged(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(database.GetScalarValue("Select count(*) from RcsTemplateHeader where templatename=N'" + txttempname.Text.Replace("'", "''") + "' and USERID='" + UserID + "' and active=1"));
            if (count > 0)
            {
                txttempname.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('template Name already exist !!!');", true);
                return;

            }

        }

        protected void btnCarouselcaedrdadd_Click(object sender, EventArgs e)
        {
            if (ddltypes.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Carousel Card Suggestions Type !!!');", true);
                return;
            }
            if (ddltypes.SelectedValue == "1")
            {

                if (txttext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddltypes.SelectedValue == "2")
            {
                if (txturl.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions URL !!!');", true);
                    return;
                }
                if (txttext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddltypes.SelectedValue == "3")
            {
                if (txtphone.Text != "")
                {
                    if (txtphone.Text.Length < 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone no must be 10 digits !!!');", true);
                        return;
                    }
                }
                if (txtphone.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions PHONE !!!');", true);
                    return;
                }
                if (txttext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddltypes.SelectedValue == "4")
            {
                if (txtlatitude.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Latitude !!!');", true);
                    return;
                }
                if (txtlongitude.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Longitude !!!');", true);
                    return;
                }
                if (txttext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card Suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddltypes.SelectedValue == "5")
            {

                if (txttext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Carousel Card suggestions Text !!!');", true);
                    return;
                }

            }
            try
            {
                if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
                {

                    DataTable dt2 = (DataTable)ViewState["CurrentTable"];
                    if (dt2.Rows.Count >= 4)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Carousel Card suggestions more then 4 !!!');", true);
                        return;
                    }
                    int count = dt2.Rows.Count;
                    DataRow dr = dt2.NewRow();
                    //dr["Id"] = Convert.ToInt32(id);
                    dr["SuggetionType"] = ddltypes.SelectedValue;
                    dr["SuggestionText"] = txttext.Text;
                    dr["SuggestionUrl"] = txturl.Text;
                    dr["SuggestionPhone"] = txtphone.Text;
                    dr["SuggestionLatitude"] = txtlatitude.Text;
                    dr["SuggestionLongitude"] = txtlongitude.Text;
                    //dr["USERID"] = UserID;
                    dt2.Rows.Add(dr);

                    gvSType.DataSource = dt2;
                    gvSType.DataBind();
                    ViewState["CurrentTable"] = dt2;
                    cardclear();
                }
                else
                {
                    string Carouselcard = "Tmp_Carouselcard_" + UserID;
                    string sql = "";
                    string sql1 = "";

                    sql1 = @"if exists (select * from sys.tables where name='" + Carouselcard + @"') BEGIN select * from  " + Carouselcard + "  end ";
                    DataTable dtCC = database.GetDataTable(sql1);
                    // database.ExecuteNonQuery(Templateid);                   

                    if (dtCC.Rows.Count >= 4)
                    {
                        cardclear();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Carousel Card suggestions more then 4 !!!');", true);
                        return;
                    }


                    sql = @"if exists (select * from sys.tables where name='" + Carouselcard + @"')  " +
                            " begin " +
                           "Insert into " + Carouselcard + @"(RCSType, SuggetionType,SuggestionText,  SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                     Values('" + ddlRCSType.SelectedValue + "', '" + ddltypes.SelectedValue + "', N'" + txttext.Text + "', '" + txturl.Text + "', '" + txtphone.Text + "', '" + txtlatitude.Text + "', '" + txtlongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "')" +
                           " end else begin Create table " + Carouselcard + @" (RCSType VARCHAR(1),SuggetionType VARCHAR(1), ID INT IDENTITY(10001,1),SuggestionText NVARCHAR(MAX),SuggestionUrl VARCHAR(MAX),SuggestionPhone VARCHAR(15),SuggestionLatitude VARCHAR(100),SuggestionLongitude VARCHAR(100),USERID VARCHAR(100),CreatedDate DateTime,sessioncard int default 0)" +
                           "Insert into " + Carouselcard + @"(RCSType, SuggetionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                Values('" + ddlRCSType.SelectedValue + "', '" + ddltypes.SelectedValue + "', N'" + txttext.Text + "', '" + txturl.Text + "', '" + txtphone.Text + "', '" + txtlatitude.Text + "', '" + txtlongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "') end";
                    database.ExecuteNonQuery(sql);
                    //ob.CarouselCard(UserID, ddlRCSType.SelectedValue, ddltypes.SelectedValue, txttext.Text,   
                    //txturl.Text, txtphone.Text, txtlatitude.Text, txtlongitude.Text);
                    DataTable dtCC1 = database.GetDataTable(sql1);
                    if (dtCC1.Rows.Count > 0)
                    {
                        gvSType.DataSource = null;
                        gvSType.DataSource = dtCC1;
                        gvSType.DataBind();
                        suggectionCardCrasoul.Style.Add("display", "show");
                        ViewState["CurrentTable"] = dtCC1;
                        // GridFormat(gvSType);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Suggestion Added');", true);
                    cardclear();
                }
            }
            catch (Exception EX)
            {

            }


        }
        protected void cardclear()
        {
            txttext.Text = "";
            txturl.Text = "";
            txtphone.Text = "";
            txtlatitude.Text = "";
            txtlongitude.Text = "";
            txtcctext.Text = "";
            txtCcurl.Text = "";
            tctccnumber.Text = "";
            txtcLatitude.Text = "";
            txtcLongitude.Text = "";
            txtcardtext.Text = "";
            txtcrdlongitude.Text = "";
            txtcrdlatitude.Text = "";
            txtcardphone.Text = "";
            txtcardurl.Text = "";

        }

        protected void gvSType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (e.Row.FindControl("lbldtype") as Label);
                if (lbl.Text == "1")
                {
                    lbl.Text = "REPLY";
                }
                if (lbl.Text == "2")
                {
                    lbl.Text = "OPEN_URL";
                }
                else if (lbl.Text == "3")
                {
                    lbl.Text = "DIAL_PHONE";
                }
                else if (lbl.Text == "4")
                {
                    lbl.Text = "SHOW_LOCATION";
                }
                else if (lbl.Text == "5")
                {
                    lbl.Text = "REQUEST_LOCATION";
                }

            }

        }

        protected void ddlCarouselSuggetion_SelectedIndexChanged(object sender, EventArgs e)
        {
            carouselTypes();
        }

        private void carouselTypes()
        {
            ddlCarouselSuggetion.Items[0].Attributes.CssStyle.Add("margin-right", "30px;");
            if (ddlCarouselSuggetion.SelectedValue == "0")
            {
                sugurl.Style.Add("display", "none");
                sgdialphone.Style.Add("display", "none");
                sgdivshowlocation.Style.Add("display", "none");
                divcarsS.Style.Add("display", "none");
            }
            if (ddlCarouselSuggetion.SelectedValue == "1")
            {
                sugurl.Style.Add("display", "none");
                sgdialphone.Style.Add("display", "none");
                sgdivshowlocation.Style.Add("display", "none");
                divcarsS.Style.Add("display", "show");
            }
            if (ddlCarouselSuggetion.SelectedValue == "2")
            {
                sugurl.Style.Add("display", "show");
                sgdialphone.Style.Add("display", "none");
                sgdivshowlocation.Style.Add("display", "none");
                divcarsS.Style.Add("display", "show");

            }
            if (ddlCarouselSuggetion.SelectedValue == "3")
            {
                sugurl.Style.Add("display", "none");
                sgdialphone.Style.Add("display", "show");
                sgdivshowlocation.Style.Add("display", "none");
                divcarsS.Style.Add("display", "show");

            }
            if (ddlCarouselSuggetion.SelectedValue == "4")
            {
                sugurl.Style.Add("display", "none");
                sgdialphone.Style.Add("display", "none");
                sgdivshowlocation.Style.Add("display", "true");
                divcarsS.Style.Add("display", "show");
            }
            if (ddlCarouselSuggetion.SelectedValue == "5")
            {
                sugurl.Style.Add("display", "none");
                sgdialphone.Style.Add("display", "none");
                sgdivshowlocation.Style.Add("display", "none");
                divcarsS.Style.Add("display", "show");
            }
        }

        protected void btncarouslsgtion_Click(object sender, EventArgs e)
        {
            string Carouselsuggection = "Tmp_Carouselsuggection_" + UserID;
            string sql = "";
            string sql1 = "";
            if (ddlCarouselSuggetion.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Over All Carousel suggestions Type !!!');", true);
                return;
            }
            if (ddlCarouselSuggetion.SelectedValue == "1")
            {

                if (txtcctext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over All Carousel suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddlCarouselSuggetion.SelectedValue == "2")
            {
                if (txtCcurl.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over All Carousel suggestions URL !!!');", true);
                    return;
                }
                if (txtcctext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over All Carousel suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddlCarouselSuggetion.SelectedValue == "3")
            {
                if (tctccnumber.Text != "")
                {
                    if (tctccnumber.Text.Length < 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone no must be 10 digits !!!');", true);
                        return;
                    }
                }
                if (tctccnumber.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions PHONE !!!');", true);
                    return;
                }
                if (txtcctext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddlCarouselSuggetion.SelectedValue == "4")
            {
                if (txtcLatitude.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions Latitude !!!');", true);
                    return;
                }
                if (txtcLongitude.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions Longitude !!!');", true);
                    return;
                }
                if (txtcctext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions Text !!!');", true);
                    return;
                }

            }
            if (ddlCarouselSuggetion.SelectedValue == "5")
            {

                if (txtcctext.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Over all Carousel suggestions Text !!!');", true);
                    return;
                }

            }
            //sql1 = @"if exists (select * from sys.tables where name='" + Carouselsuggection + @"') BEGIN select * from  " + Carouselsuggection + "  end ";
            //DataTable dtCC = database.GetDataTable(sql1);
            //// database.ExecuteNonQuery(Templateid);                   

            //if (dtCC.Rows.Count >= 4)
            //{
            //    cardclear();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add carousel Suggestion more then 4 !!!');", true);
            //    return;
            //}


            if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
            {
                DataTable dtCC = ViewState["SgoverallTable"] as DataTable;
                // database.ExecuteNonQuery(Templateid);                   


                DataTable dt2 = (DataTable)ViewState["SgoverallTable"];
                if (dt2.Rows.Count >= 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Over all suggestions more then 10 !!!');", true);
                    return;
                }
                int count = dt2.Rows.Count;
                DataRow dr = dt2.NewRow();
                //dr["Id"] = Convert.ToInt32(id);

                dr["SuggetionType"] = ddlCarouselSuggetion.SelectedValue;
                dr["SuggestionText"] = txtcctext.Text.Replace("'", "''");
                dr["SuggestionUrl"] = txtCcurl.Text;
                dr["SuggestionPhone"] = tctccnumber.Text;
                dr["SuggestionLatitude"] = txtcLatitude.Text;
                dr["SuggestionLongitude"] = txtcLongitude.Text;
                //dr["USERID"] = UserID;
                dt2.Rows.Add(dr);

                gvcrauselS.DataSource = dt2;
                gvcrauselS.DataBind();
                ViewState["SgoverallTable"] = dt2;
                cardclear();
            }
            else
            {

                DataTable dtCC = ViewState["SgoverallTable"] as DataTable;
                // database.ExecuteNonQuery(Templateid);                   


                DataTable dt2 = (DataTable)ViewState["SgoverallTable"];
                if (dt2 != null)
                {


                    if (dt2.Rows.Count >= 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Over all suggestions more then 10 !!!');", true);
                        return;
                    }
                }

                if (ViewState["SgoverallTable"] == null)
                {

                    DataTable overallSg = new DataTable();
                    overallSg.Columns.Add("RCSType", typeof(string));
                    overallSg.Columns.Add("SuggetionType", typeof(string));
                    overallSg.Columns.Add("SuggestionText", typeof(string));
                    overallSg.Columns.Add("SuggestionUrl", typeof(string));
                    overallSg.Columns.Add("SuggestionPhone", typeof(string));
                    overallSg.Columns.Add("SuggestionLatitude", typeof(string));
                    overallSg.Columns.Add("SuggestionLongitude", typeof(string));
                    overallSg.Columns.Add("USERID", typeof(string));
                    overallSg.Columns.Add("CreatedDate", typeof(string));

                    DataRow dr = overallSg.NewRow();
                    dr["RCSType"] = ddlRCSType.SelectedValue;
                    dr["SuggetionType"] = ddlCarouselSuggetion.SelectedValue;
                    dr["SuggestionText"] = txtcctext.Text;
                    dr["SuggestionUrl"] = txtCcurl.Text;
                    dr["SuggestionPhone"] = tctccnumber.Text;
                    dr["SuggestionLatitude"] = txtcLatitude.Text;
                    dr["SuggestionLongitude"] = txtcLongitude.Text;
                    dr["USERID"] = UserID;
                    dr["CreatedDate"] = System.DateTime.Now;

                    overallSg.Rows.Add(dr);

                    ViewState["SgoverallTable"] = overallSg;
                    this.gvcrauselS.DataSource = overallSg;
                    this.gvcrauselS.DataBind();
                    cardclear();
                }
                else
                {

                    DataTable overallSg1 = (DataTable)ViewState["SgoverallTable"];
                    int count = overallSg1.Rows.Count;
                    DataRow dr = overallSg1.NewRow();
                    dr["RCSType"] = ddlRCSType.SelectedValue;
                    dr["SuggetionType"] = ddlCarouselSuggetion.SelectedValue;
                    dr["SuggestionText"] = txtcctext.Text.Replace("'", "''");
                    dr["SuggestionUrl"] = txtCcurl.Text;
                    dr["SuggestionPhone"] = tctccnumber.Text;
                    dr["SuggestionLatitude"] = txtcLatitude.Text;
                    dr["SuggestionLongitude"] = txtcLongitude.Text;
                    dr["USERID"] = UserID;
                    dr["CreatedDate"] = System.DateTime.Now;

                    overallSg1.Rows.Add(dr);
                    this.gvcrauselS.DataSource = overallSg1;
                    this.gvcrauselS.DataBind();
                    ViewState["SgoverallTable"] = overallSg1;
                    cardclear();
                }


                //sql = @"if exists (select * from sys.tables where name='" + Carouselsuggection + @"')  " +
                //        " begin " +
                //       "Insert into " + Carouselsuggection + @"(RCSType, SuggestionType,SuggestionText,  SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                //         Values('" + ddlRCSType.SelectedValue + "', '" + ddlCarouselSuggetion.SelectedValue + "', N'" + txtcctext.Text + "', '" + txtCcurl.Text + "', '" + tctccnumber.Text + "', '" + txtcLatitude.Text + "', '" + txtcLongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "')" +
                //       " end else begin Create table " + Carouselsuggection + @" (RCSType VARCHAR(1),SuggestionType VARCHAR(1), ID INT IDENTITY(10001,1),SuggestionText NVARCHAR(MAX),SuggestionUrl VARCHAR(MAX),SuggestionPhone VARCHAR(15),SuggestionLatitude VARCHAR(100),SuggestionLongitude VARCHAR(100),USERID VARCHAR(100),CreatedDate DateTime,sessioncard int default 0)" +
                //       "Insert into " + Carouselsuggection + @"(RCSType, SuggestionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                //    Values('" + ddlRCSType.SelectedValue + "', '" + ddlCarouselSuggetion.SelectedValue + "', N'" + txtcctext.Text + "', '" + txtCcurl.Text + "', '" + tctccnumber.Text + "', '" + txtcLatitude.Text + "', '" + txtcLongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "') end";
                //database.ExecuteNonQuery(sql);
                //DataTable dtCC1 = database.GetDataTable("if exists(select * from sys.tables where name = '" + Carouselsuggection + @"') BEGIN select *from  " + Carouselsuggection + "  end ");
                //if (dtCC1.Rows.Count > 0)
                //{
                //    gvcrauselS.DataSource = null;
                //    gvcrauselS.DataSource = dtCC1;
                //    gvcrauselS.DataBind();
                //    // GridFormat(gvSType);
                //}
            }
            cardclear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('suggestions Added');", true);

        }

        protected void btnCarouselcardadd_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
            {
                if (ddlcardtype.SelectedValue == "0")
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Card suggestions Type !!!');", true);
                    return;
                }
                if (ddlcardtype.SelectedValue == "1")
                {

                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }
                }

                if (ddlcardtype.SelectedValue == "2")
                {
                    if (txtcardurl.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions URL !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "3")
                {
                    if (txtcardphone.Text != "")
                    {
                        if (txtcardphone.Text.Length < 10)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone no must be 10 digits !!!');", true);
                            return;
                        }

                    }
                    if (txtcardphone.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions PHONE !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "4")
                {
                    if (txtcrdlatitude.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Latitude !!!');", true);
                        return;
                    }
                    if (txtcrdlongitude.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Longitude !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "5")
                {

                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                DataTable dt2 = (DataTable)ViewState["CurrentTable"];
                if (dt2.Rows.Count >= 4)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Card suggestions more then 4 !!!');", true);
                    return;
                }
                int count = dt2.Rows.Count;
                DataRow dr = dt2.NewRow();
                //dr["Id"] = Convert.ToInt32(id);
                dr["SuggetionType"] = ddlcardtype.SelectedValue;
                dr["SuggestionText"] = txtcardtext.Text.Replace("'", "''");
                dr["SuggestionUrl"] = txtcardurl.Text;
                dr["SuggestionPhone"] = txtcardphone.Text;
                dr["SuggestionLatitude"] = txtcrdlatitude.Text;
                dr["SuggestionLongitude"] = txtcrdlongitude.Text;
                //dr["USERID"] = UserID;
                dt2.Rows.Add(dr);

                gvCardsg.DataSource = dt2;
                gvCardsg.DataBind();
                ViewState["CurrentTable"] = dt2;
                suggestionCard.Style.Add("display", "show");
                cardclear();
            }
            else
            {


                //string Card = "Tmp_CardS_" + UserID;
                //string sql = "";
                //string sql1 = "";

                if (ddlcardtype.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Card suggestions Type !!!');", true);
                    return;
                }
                if (ddlcardtype.SelectedValue == "1")
                {

                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "2")
                {
                    if (txtcardurl.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions URL !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "3")
                {
                    if (txtcardphone.Text != "")
                    {
                        if (txtcardphone.Text.Length < 10)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone no must be 10 digits !!!');", true);
                            return;
                        }

                    }
                    if (txtcardphone.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions PHONE !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "4")
                {
                    if (txtcrdlatitude.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Latitude !!!');", true);
                        return;
                    }
                    if (txtcrdlongitude.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter  Card suggestions Longitude !!!');", true);
                        return;
                    }
                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                if (ddlcardtype.SelectedValue == "5")
                {

                    if (txtcardtext.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Card suggestions Text !!!');", true);
                        return;
                    }

                }
                //sql1 = @"if exists (select * from sys.tables where name='" + Card + @"') BEGIN select * from  " + Card + "  end ";
                if (ViewState["cardSgTable"] != null)
                {
                    DataTable dtCC = ViewState["cardSgTable"] as DataTable;
                    // database.ExecuteNonQuery(Templateid);                   

                    if (dtCC.Rows.Count >= 4)
                    {
                        cardclear();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You can not add Card suggestions more then 4 !!!');", true);
                        return;
                    }

                }

                if (ViewState["cardSgTable"] == null)
                {

                    DataTable dtcardSg = new DataTable();
                    dtcardSg.Columns.Add("RCSType", typeof(string));
                    dtcardSg.Columns.Add("SuggetionType", typeof(string));
                    dtcardSg.Columns.Add("SuggestionText", typeof(string));
                    dtcardSg.Columns.Add("SuggestionUrl", typeof(string));
                    dtcardSg.Columns.Add("SuggestionPhone", typeof(string));
                    dtcardSg.Columns.Add("SuggestionLatitude", typeof(string));
                    dtcardSg.Columns.Add("SuggestionLongitude", typeof(string));
                    dtcardSg.Columns.Add("USERID", typeof(string));
                    dtcardSg.Columns.Add("CreatedDate", typeof(string));

                    DataRow dr = dtcardSg.NewRow();
                    dr["RCSType"] = ddlRCSType.SelectedValue;
                    dr["SuggetionType"] = ddlcardtype.SelectedValue;
                    dr["SuggestionText"] = txtcardtext.Text.Replace("'", "''");
                    dr["SuggestionUrl"] = txtcardurl.Text;
                    dr["SuggestionPhone"] = txtcardphone.Text;
                    dr["SuggestionLatitude"] = txtcrdlatitude.Text;
                    dr["SuggestionLongitude"] = txtcrdlongitude.Text;
                    dr["USERID"] = UserID;
                    dr["CreatedDate"] = System.DateTime.Now;

                    dtcardSg.Rows.Add(dr);

                    ViewState["cardSgTable"] = dtcardSg;
                    this.gvCardsg.DataSource = dtcardSg;
                    this.gvCardsg.DataBind();
                    suggestionCard.Style.Add("display", "show");
                    cardclear();
                }
                else
                {

                    DataTable dtcardSg1 = (DataTable)ViewState["cardSgTable"];
                    int count = dtcardSg1.Rows.Count;
                    DataRow dr = dtcardSg1.NewRow();
                    dr["RCSType"] = ddlRCSType.SelectedValue;
                    dr["SuggetionType"] = ddlcardtype.SelectedValue;
                    dr["SuggestionText"] = txtcardtext.Text.Replace("'", "''");
                    dr["SuggestionUrl"] = txtcardurl.Text;
                    dr["SuggestionPhone"] = txtcardphone.Text;
                    dr["SuggestionLatitude"] = txtcrdlatitude.Text;
                    dr["SuggestionLongitude"] = txtcrdlongitude.Text;
                    dr["USERID"] = UserID;
                    dr["CreatedDate"] = System.DateTime.Now;

                    dtcardSg1.Rows.Add(dr);

                    this.gvCardsg.DataSource = dtcardSg1;
                    this.gvCardsg.DataBind();
                    ViewState["cardSgTable"] = dtcardSg1;
                    suggestionCard.Style.Add("display", "show");
                    cardclear();
                }

                //sql = @"if exists (select * from sys.tables where name='" + Card + @"')  " +
                //        " begin " +
                //       "Insert into " + Card + @"(RCSType, SuggetionType,SuggestionText,  SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                //     Values('" + ddlRCSType.SelectedValue + "', '" + ddlcardtype.SelectedValue + "', N'" + txtcardtext.Text + "', '" + txtcardurl.Text + "', '" + txtcardphone.Text + "', '" + txtcrdlatitude.Text + "', '" + txtcrdlongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "')" +
                //       " end else begin Create table " + Card + @" (RCSType VARCHAR(1),SuggetionType VARCHAR(1), ID INT IDENTITY(10001,1),SuggestionText NVARCHAR(MAX),SuggestionUrl VARCHAR(MAX),SuggestionPhone VARCHAR(15),SuggestionLatitude VARCHAR(100),SuggestionLongitude VARCHAR(100),USERID VARCHAR(100),CreatedDate DateTime,sessioncard int default 0)" +
                //       "Insert into " + Card + @"(RCSType, SuggetionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude, USERID, CreatedDate) 
                //Values('" + ddlRCSType.SelectedValue + "', '" + ddlcardtype.SelectedValue + "', N'" + txtcardtext.Text + "', '" + txtcardurl.Text + "', '" + txtcardphone.Text + "', '" + txtcrdlatitude.Text + "', '" + txtcrdlongitude.Text + "', '" + UserID + "', '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "') end";
                //database.ExecuteNonQuery(sql);
                //DataTable dtCC1 = database.GetDataTable("if exists(select * from sys.tables where name = '" + Card + @"') BEGIN select *from  " + Card + "  end ");
                //if (dtCC1.Rows.Count > 0)
                //{
                //    gvCardsg.DataSource = null;
                //    gvCardsg.DataSource = dtCC1;
                //    gvCardsg.DataBind();
                //    // GridFormat(gvSType);
                //}
                cardclear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Suggestion Added');", true);
            }
        }



        protected void gvCardsg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (e.Row.FindControl("lbldtype") as Label);
                if (lbl.Text == "1")
                {
                    lbl.Text = "REPLY";
                }
                if (lbl.Text == "2")
                {
                    lbl.Text = "OPEN_URL";
                }
                else if (lbl.Text == "3")
                {
                    lbl.Text = "DIAL_PHONE";
                }
                else if (lbl.Text == "4")
                {
                    lbl.Text = "SHOW_LOCATION";
                }
                else if (lbl.Text == "5")
                {
                    lbl.Text = "REQUEST_LOCATION";
                }

            }
        }

        protected void gvcrauselS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (e.Row.FindControl("lbldtype") as Label);
                if (lbl.Text == "1")
                {
                    lbl.Text = "REPLY";
                }
                if (lbl.Text == "2")
                {
                    lbl.Text = "OPEN_URL";
                }
                else if (lbl.Text == "3")
                {
                    lbl.Text = "DIAL_PHONE";
                }
                else if (lbl.Text == "4")
                {
                    lbl.Text = "SHOW_LOCATION";
                }
                else if (lbl.Text == "5")
                {
                    lbl.Text = "REQUEST_LOCATION";
                }
            }
        }

        protected void txtcardurl_TextChanged(object sender, EventArgs e)
        {
            string Url = default(string);
            Url = "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\\\\\/\\?\\.\\:\\;\\\'\\,]*)?";
            if (Regex.IsMatch(txtcardurl.Text, Url))
            {

            }
            else
            {
                txtcardurl.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Url');", true);
                return;
            }
        }

        protected void txtCcurl_TextChanged(object sender, EventArgs e)
        {
            string Url = default(string);
            Url = "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\\\\\/\\?\\.\\:\\;\\\'\\,]*)?";
            if (Regex.IsMatch(txtCcurl.Text, Url))
            {

            }
            else
            {
                txtCcurl.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Url');", true);
                return;
            }
        }

        protected void txturl_TextChanged(object sender, EventArgs e)
        {
            string Url = default(string);
            Url = "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\\\\\/\\?\\.\\:\\;\\\'\\,]*)?";
            if (Regex.IsMatch(txturl.Text, Url))
            {

            }
            else
            {
                txturl.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Url');", true);
                return;
            }
        }

        protected void gvCardsg_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void BindcardSuggestion()
        {
            if (IsPostBack)
            {
                DataTable dtCC1 = database.GetDataTable("select * from CardSuggetion where cardid='" + ViewState["Did"] + "' and active=1 ");
                if (dtCC1.Rows.Count > 0)
                {
                    if (ddlRCSType.SelectedValue == "4")
                    {


                        gvCardsg.DataSource = dtCC1;
                        gvCardsg.DataBind();
                    }
                    if (ddlRCSType.SelectedValue == "5")
                    {
                        gvSType.DataSource = dtCC1;
                        gvSType.DataBind();

                        //gvSType.Columns[7].Visible = true;

                    }
                    suggectionCardCrasoul.Style.Add("display", "show");
                    suggectionCrasoul.Style.Add("display", "show");
                    suggestionCard.Style.Add("display", "show");
                }
                ViewState["CurrentTable"] = dtCC1;

            }
        }

        protected void gvCardsg_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
            {

                int index = Convert.ToInt32(e.RowIndex);
                DataTable dt2 = ViewState["CurrentTable"] as DataTable;
                dt2.Rows[index].Delete();
                dt2.AcceptChanges();
                ViewState["CurrentTable"] = dt2;
                gvCardsg.DataSource = dt2;
                gvCardsg.DataBind();
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);
                DataTable dt2 = ViewState["cardSgTable"] as DataTable;
                dt2.Rows[index].Delete();
                dt2.AcceptChanges();
                ViewState["cardSgTable"] = dt2;
                gvCardsg.DataSource = dt2;
                gvCardsg.DataBind();
            }

        }

        protected void gvSType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt2 = ViewState["CurrentTable"] as DataTable;
            dt2.Rows[index].Delete();
            dt2.AcceptChanges();
            ViewState["CurrentTable"] = dt2;
            gvSType.DataSource = dt2;
            gvSType.DataBind();
        }

        protected void gvcrauselS_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Convert.ToBoolean(ViewState["IsEdit"]) == true)
            {

                int index = Convert.ToInt32(e.RowIndex);
                DataTable dt2 = ViewState["SgoverallTable"] as DataTable;
                dt2.Rows[index].Delete();
                dt2.AcceptChanges();
                ViewState["SgoverallTable"] = dt2;
                gvcrauselS.DataSource = dt2;
                gvcrauselS.DataBind();
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);
                DataTable dt2 = ViewState["SgoverallTable"] as DataTable;
                dt2.Rows[index].Delete();
                dt2.AcceptChanges();
                ViewState["SgoverallTable"] = dt2;
                gvcrauselS.DataSource = dt2;
                gvcrauselS.DataBind();
            }
        }

        protected void btnTempcrauselsave_Click(object sender, EventArgs e)
        {
            bindcardDetails();
        }

        protected void btnTempcrauseldel_Click(object sender, EventArgs e)
        {
            string user1 = "Tmp_Template_" + UserID;
            string
            query = @"if exists (select * from sys.tables where name='" + user1 + @"') drop table " + user1 + @"";
            database.ExecuteNonQuery(query);
        }

        protected void txtcrdlatitude_TextChanged(object sender, EventArgs e)
        {

            if (IsValidlatitude(txtcrdlatitude.Text))
            {

            }
            else
            {
                txtcrdlatitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Latitude');", true);
                return;
            }
        }


        protected void txtlatitude_TextChanged(object sender, EventArgs e)
        {
            if (IsValidlatitude(txtlatitude.Text))
            {

            }
            else
            {
                txtlatitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Latitude');", true);
                return;
            }
        }

        protected void txtcLatitude_TextChanged(object sender, EventArgs e)
        {
            if (IsValidlatitude(txtcLatitude.Text))
            {

            }
            else
            {
                txtcLatitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Latitude');", true);
                return;
            }
        }
        public bool IsValidlatitude(string latitude)
        {
            string Url = default(string);
            Url = "^(\\+|-)?(?:90(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\\.[0-9]{1,6})?))$";
            if (Regex.IsMatch(latitude, Url))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void txtcrdlongitude_TextChanged(object sender, EventArgs e)
        {
            if (IsValidLongitude(txtcrdlongitude.Text))
            {

            }
            else
            {
                txtcrdlongitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Longitude');", true);
                return;
            }
        }

        protected void txtlongitude_TextChanged(object sender, EventArgs e)
        {
            if (IsValidLongitude(txtlongitude.Text))
            {

            }
            else
            {
                txtlongitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Longitude');", true);
                return;
            }
        }

        protected void txtcLongitude_TextChanged(object sender, EventArgs e)
        {
            if (IsValidLongitude(txtcLongitude.Text))
            {

            }
            else
            {
                txtcLongitude.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Valid Longitude');", true);
                return;
            }
        }
        public bool IsValidLongitude(string Longitude)
        {
            string Url = default(string);
            Url = "^(\\+|-)?(?:180(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\\.[0-9]{1,6})?))$";
            if (Regex.IsMatch(Longitude, Url))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void lbtntest_Click(object sender, EventArgs e)
        {

        }

        protected void btntest_Click(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Label msgtext = new Label();
            if (txtsendmobile.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Mobile No');", true);
                return;
            }
            if (Convert.ToInt32(ddlSelectrcstype.SelectedValue) > 0)
            {



                DataTable dtT = obU.GetTemplateRCSfromID(Convert.ToString(Session["RCSUserID"]), ViewState["sendtemplateid"].ToString());

                if (dtT.Rows[0]["TemplateText"].ToString() == null)
                    msgtext.Text = "";
                else
                    msgtext.Text = dtT.Rows[0]["TemplateText"].ToString();
            }
            string mobile = "";
            if (txtsendmobile.Text != "")
                mobile = txtsendmobile.Text;
            DataTable dtSMPPAC = new DataTable();
            string fileUname = UserID + DateTime.Now.ToString("_yyyyMMddhhmmssfff");
            string country_code = Session["DEFAULTCOUNTRYCODE"].ToString();

            if (txtsendmobile.Text != "") mobile = txtsendmobile.Text.Replace('\n', ',');
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();


            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
            if (mobile.Trim() != "")
            {
                if (country_code == "91")
                {
                    int maxlen = mobList.Max(arr => arr.Length);
                    int minlen = mobList.Min(arr => arr.Length);
                    if (maxlen != minlen)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ]');", true);
                        return;
                    }
                    if (maxlen != 10 || minlen != 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ]');", true);
                        return;
                    }
                }

                //  if (maxlen == 10) country_code = "91";
            }
            Int32 cnt = 0;

            cnt = mobList.Count;
            int noofsms = 0;
            int noofsms1S = 0;


            msgtext.Text = msgtext.Text.Trim();
            string q = msgtext.Text.Trim();
            bool ucs2 = false;
            int count_PIPE = q.Count(f => f == '|');
            int qlen = msgtext.Text.Trim().Length + count_PIPE;

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

            if (ddlSelectrcstype.SelectedValue != "1")
                noofsms = 1;


            Int32 noofSMS = noofsms1S * cnt;
            Int32 noofmessages = noofsms * cnt;

            double rate = 0;
            string strd = "Select * from tblrcsratemaster where userid='" + UserID + "'";
            DataTable dtrcs = database.GetDataTable(strd);
            if (dtrcs.Rows.Count > 0)
            {
                rate = (ddlSelectrcstype.SelectedValue == "1" ? Convert.ToDouble(dtrcs.Rows[0]["TextRate"].ToString()) : rate);
                rate = (ddlSelectrcstype.SelectedValue == "2" ? Convert.ToDouble(dtrcs.Rows[0]["ImageRate"].ToString()) : rate);
                rate = (ddlSelectrcstype.SelectedValue == "3" ? Convert.ToDouble(dtrcs.Rows[0]["VideoRate"].ToString()) : rate);
                rate = (ddlSelectrcstype.SelectedValue == "4" ? Convert.ToDouble(dtrcs.Rows[0]["CardRate"].ToString()) : rate);
                rate = (ddlSelectrcstype.SelectedValue == "5" ? Convert.ToDouble(dtrcs.Rows[0]["CarouselRate"].ToString()) : rate);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Rate not Define. Contat Adminstrator !!!');", true);
                return;
            }

            if (rate <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('RCS Rate not Define. Contat Adminstrator !!!');", true);
                return;
            }
            DataTable dt2 = ob.GetUserParameter(UserID);
            string bal2 = dt2.Rows[0]["RCSbalance"].ToString();
            double bal = Convert.ToDouble(bal2) * 1000;
            //if (bal - Convert.ToDouble(noofmessages * (rate)) < 0)
            if (Convert.ToDouble(noofmessages * (rate * 10)) > bal)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient RCS Balance.');", true);
                return;
            }


            ob.noof_message = noofmessages;
            ob.msg_rate = rate;
            double PrevBalance = Convert.ToDouble(bal2);

            Label lblbalance = Master.FindControl("lblrcsBal") as Label;
            lblbalance.Text = Convert.ToString(Session["RCSBAL"]);

            double AvailableBalance = obU.CalculateRCSAmount(UserID, cnt, rate, 1);
            lblbalance.Text = Convert.ToString(AvailableBalance);
            Session["RCSBAL"] = AvailableBalance;



            string campName = "Test";
            // obU.InsertRCSrecordsFromUSERTMP(UserID, 0, 0, "", ddlSelectrcstype.SelectedValue, "", "", "", dtSMPPAC, campName, false, 0, 0, mobList, "Test", "", "", country_code, 0, 0, fileUname, "", "", ViewState["sendtemplateid"].ToString(), "", 0, "", "", 0);
            // obU.InsertRCSrecordsFromUSERTMP(UserID, 0, noofmessages, "", ddlSelectrcstype.SelectedValue,"", "", "", dtSMPPAC, campName, false, noofsms, rate, mobList, "Test", "", "", country_code, PrevBalance, AvailableBalance, fileUname, "", "", ViewState["sendtemplateid"].ToString(), "", 0, "", "", noofSMS);
            obU.InsertRCSrecordsFromUSERTMP(UserID, qlen, noofmessages, "", ddlSelectrcstype.SelectedValue, msgtext.Text.Trim(), "", "", dtSMPPAC, campName, ucs2, noofsms, rate, mobList, "Test", "", "", country_code, PrevBalance, AvailableBalance, fileUname, "", "", ViewState["sendtemplateid"].ToString(), "", 0, "", "", noofSMS);
            txtsendmobile.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Send Sucessfully.');", true);
        }


    }
}
