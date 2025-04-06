using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class allot_sender_id : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            //string s = Convert.ToString( Page.Request.Form["txtTo"].ToString());
            if (!IsPostBack)
            {
                //GetData();
                // BindCountryCode();


            }
        }


        public void PopulateCountry(string uname)
        {
            Util ob = new Util();
            DataTable dt = ob.GetActiveCountry(uname);
            ddlCCode.DataSource = dt;
            ddlCCode.DataTextField = "name";
            ddlCCode.DataValueField = "countrycode";
            ddlCCode.DataBind();

            ddlCCode.SelectedValue = Convert.ToString(database.GetScalarValue("select defaultCountry from CUSTOMER where username='" + uname + "' "));
        }

        private void BindCountryCode()
        {
            List<string> countryList = CountryList();
            string duplicateItem = "";
            foreach (var items in countryList)
            {
                string ccode = GetCode(items.ToString());
                if (duplicateItem == "")
                {
                    String text = String.Format("{0}-{1}\n",
                                    items.ToString(), ccode);
                    ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                    //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                    duplicateItem = items;
                }
                if (duplicateItem != items.ToString())
                {
                    String text = String.Format("{0}-{1}\n",
                                    items.ToString(), ccode);
                    //ddlCCode.Items.Add(new ListItem(text.ToString(), "http://www.geognos.com/api/en/countries/flag/" + GetCountryCode(items.ToString()) + ".png"));
                    ddlCCode.Items.Add(new ListItem(text.ToString(), ccode));
                    duplicateItem = items;
                }
            }
            //Imagetitlebind();
            ddlCCode.Items.Insert(0, new ListItem("Select", "0"));
            ddlCCode.SelectedValue = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
        }


        public static string GetCountryCode(string country)
        {
            string countryCode = "";
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);
                if (region.EnglishName.ToUpper().Contains(country.ToUpper()))
                {
                    countryCode = region.TwoLetterISORegionName;
                }
            }
            return countryCode;
        }

        public string GetCode(string country)
        {
            string code = "";
            JObject weatherInfo = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Server.MapPath("~/TextFile.json")));
            code = Convert.ToString(((JToken)weatherInfo[GetCountryCode(country)])).Replace("\"", string.Empty);
            return code;
        }


        public static List<string> CountryList()
        {
            List<string> CultureList = new List<string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(CultureList.Contains(getRegionInfo.Name)))
                {
                    CultureList.Add(getRegionInfo.EnglishName);
                }
            }
            CultureList.Sort();
            return CultureList;
        }



        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            //s1 = h1.Value;
            //s2 = h2.Value;

            //if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
            //{
            //    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            //else
            //{
            //    s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}


            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            DataTable dt = ob.GetSenderIdList(s1, s2, usertype, user, txtUser.Text.Trim(), txtcompnm.Text.Trim());

            grv.DataSource = dt;
            grv.DataBind();

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
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;
            GetData();
            //txtFrm.Text = hdntxtFrm.Value;
            //txtTo.Text = hdntxtTo.Value;
        }

        protected void btnAllot_Click(object sender, EventArgs e)
        {
            Clear();
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField h = (HiddenField)gvro.FindControl("hdnUserId");
            string un = h.Value;
            ViewState["UN"] = un;

            PopulateCountry(un);

            divAllot1.Style["display"] = "none;";
            divAllot2.Style["display"] = "none;";
            divAllot3.Style["display"] = "none;";
            divAllot4.Style["display"] = "none;";
            divAllot5.Style["display"] = "none;";
            divAllot6.Style["display"] = "none;";
            divAllot7.Style["display"] = "none;";
            divAllot8.Style["display"] = "none;";
            divAllot9.Style["display"] = "none;";

            modalpopupallot.Show();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);

        }

        protected void SetAttribueScheduleRows(string rowCount)
        {

            if (rowCount == "2")
            {
                divAllot1.Style["display"] = "";
            }
            else if (rowCount == "3")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
            }
            else if (rowCount == "4")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
            }
            else if (rowCount == "5")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
            }
            else if (rowCount == "6")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
                divAllot5.Style["display"] = "";
            }
            else if (rowCount == "7")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
                divAllot5.Style["display"] = "";
                divAllot6.Style["display"] = "";
            }
            else if (rowCount == "8")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
                divAllot5.Style["display"] = "";
                divAllot6.Style["display"] = "";
                divAllot7.Style["display"] = "";
            }
            else if (rowCount == "9")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
                divAllot5.Style["display"] = "";
                divAllot6.Style["display"] = "";
                divAllot7.Style["display"] = "";
                divAllot8.Style["display"] = "";
            }
            else if (rowCount == "10")
            {
                divAllot1.Style["display"] = "";
                divAllot2.Style["display"] = "";
                divAllot3.Style["display"] = "";
                divAllot4.Style["display"] = "";
                divAllot5.Style["display"] = "";
                divAllot6.Style["display"] = "";
                divAllot7.Style["display"] = "";
                divAllot8.Style["display"] = "";
                divAllot9.Style["display"] = "";
            }
        }

        private void Clear()
        {
            txtSender.Text = ""; hdnAllotCount.Value = "";
            txtSender1.Text = ""; hdnAllot1.Value = "";
            txtSender2.Text = ""; hdnAllot2.Value = "";
            txtSender3.Text = ""; hdnAllot3.Value = "";
            txtSender4.Text = ""; hdnAllot4.Value = "";
            txtSender5.Text = ""; hdnAllot5.Value = "";
            txtSender6.Text = ""; hdnAllot6.Value = "";
            txtSender7.Text = ""; hdnAllot7.Value = "";
            txtSender8.Text = ""; hdnAllot8.Value = "";
            txtSender9.Text = ""; hdnAllot9.Value = "";
        }
        protected void ValidateAllotSender(List<string> liSenderId)
        {
            if (txtSender.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            liSenderId.Add(txtSender.Text.Trim());
            if (hdnAllot1.Value == "1" && txtSender1.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender1.Text.Trim() != "")
                if (liSenderId.Contains(txtSender1.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender1.Text.Trim());
            if (hdnAllot2.Value == "1" && txtSender2.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender2.Text.Trim() != "")
                if (liSenderId.Contains(txtSender2.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender2.Text.Trim());
            if (hdnAllot3.Value == "1" && txtSender3.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender3.Text.Trim() != "")
                if (liSenderId.Contains(txtSender3.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender3.Text.Trim());
            if (hdnAllot4.Value == "1" && txtSender4.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender4.Text.Trim() != "")
                if (liSenderId.Contains(txtSender4.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender4.Text.Trim());
            if (hdnAllot5.Value == "1" && txtSender5.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender5.Text.Trim() != "")
                if (liSenderId.Contains(txtSender5.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender5.Text.Trim());
            if (hdnAllot6.Value == "1" && txtSender6.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender6.Text.Trim() != "")
                if (liSenderId.Contains(txtSender6.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender6.Text.Trim());
            if (hdnAllot7.Value == "1" && txtSender7.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender7.Text.Trim() != "")
                if (liSenderId.Contains(txtSender7.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender7.Text.Trim());
            if (hdnAllot8.Value == "1" && txtSender8.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender8.Text.Trim() != "")
                if (liSenderId.Contains(txtSender8.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender8.Text.Trim());
            if (hdnAllot9.Value == "1" && txtSender9.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Enter Sender Id');", true);
                return;
            }
            if (txtSender9.Text.Trim() != "")
                if (liSenderId.Contains(txtSender9.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Sender Id Already Exist !!');", true);
                    return;
                }
                else liSenderId.Add(txtSender9.Text.Trim());


            //string chkUAE = CheckUAEAccount();
            //bool flag = false;
            //if (chkUAE == "true")
            //{
            //    //Getsmppaccountuserid
            //    if (Getsmppaccountuserid(txtSender.Text) == "true")
            //    {
            //        flag = true;
            //    }
            //    if (Getsmppaccountuserid(txtSender.Text) == "")
            //    {
            //        ViewState["Sender"] = txtSender.Text;
            //        txtSender.Text = "";
            //    }
            //    if (hdnAllot1.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender1.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender1"] = txtSender1.Text;
            //            txtSender1.Text = "";
            //        }
            //    }
            //    if (hdnAllot2.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender2.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender2"] = txtSender2.Text;
            //            txtSender2.Text = "";
            //        }
            //    }
            //    if (hdnAllot3.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender3.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender3"] = txtSender3.Text;
            //            txtSender3.Text = "";
            //        }
            //    }
            //    if (hdnAllot4.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender4.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender4"] = txtSender4.Text;
            //            txtSender4.Text = "";
            //        }
            //    }
            //    if (hdnAllot5.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender5.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender5"] = txtSender5.Text;
            //            txtSender5.Text = "";
            //        }
            //    }
            //    if (hdnAllot6.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender6.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender6"] = txtSender6.Text;
            //            txtSender6.Text = "";
            //        }
            //    }
            //    if (hdnAllot7.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender7.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender7"] = txtSender7.Text;
            //            txtSender7.Text = "";
            //        }
            //    }
            //    if (hdnAllot8.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender8.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender8"] = txtSender8.Text;
            //            txtSender8.Text = "";
            //        }
            //    }
            //    if (hdnAllot9.Value == "1")
            //    {
            //        if (Getsmppaccountuserid(txtSender9.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender9"] = txtSender9.Text;
            //            txtSender9.Text = "";
            //        }
            //    }
            //end 

            //start UAEAPIAccounts
            //if (flag == false)
            //{
            //    if (GetUAEAPIAccounts(txtSender.Text) == "true")
            //    {
            //        flag = true;
            //    }
            //    if (GetUAEAPIAccounts(txtSender.Text) == "")
            //    {
            //        ViewState["Sender"] = txtSender.Text;
            //        txtSender.Text = "";
            //    }
            //    if (hdnAllot1.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender1.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender1"] = txtSender1.Text;
            //            txtSender1.Text = "";
            //        }
            //    }
            //    if (hdnAllot2.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender2.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender2"] = txtSender2.Text;
            //            txtSender2.Text = "";
            //        }
            //    }
            //    if (hdnAllot3.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender3.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender3"] = txtSender3.Text;
            //            txtSender3.Text = "";
            //        }
            //    }
            //    if (hdnAllot4.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender4.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender4"] = txtSender4.Text;
            //            txtSender4.Text = "";
            //        }
            //    }
            //    if (hdnAllot5.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender5.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender5"] = txtSender5.Text;
            //            txtSender5.Text = "";
            //        }
            //    }
            //    if (hdnAllot6.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender6.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender6"] = txtSender6.Text;
            //            txtSender6.Text = "";
            //        }
            //    }
            //    if (hdnAllot7.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender7.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender7"] = txtSender7.Text;
            //            txtSender7.Text = "";
            //        }
            //    }
            //    if (hdnAllot8.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender8.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender8"] = txtSender8.Text;
            //            txtSender8.Text = "";
            //        }
            //    }
            //    if (hdnAllot9.Value == "1")
            //    {
            //        if (GetUAEAPIAccounts(txtSender9.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender9"] = txtSender9.Text;
            //            txtSender9.Text = "";
            //        }
            //    }
            //}
            //End UAEAPIAccounts

            //start GetUAEAPIACCOUNTPROMO
            //if (flag == false)
            //{
            //    if (GetUAEAPIACCOUNTPROMO(txtSender.Text) == "true")
            //    {
            //        flag = true;
            //    }
            //    if (GetUAEAPIACCOUNTPROMO(txtSender.Text) == "")
            //    {
            //        ViewState["Sender"] = txtSender.Text;
            //        txtSender.Text = "";
            //    }
            //    if (hdnAllot1.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender1.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender1"] = txtSender1.Text;
            //            txtSender1.Text = "";
            //        }
            //    }
            //    if (hdnAllot2.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender2.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender2"] = txtSender2.Text;
            //            txtSender2.Text = "";
            //        }
            //    }
            //    if (hdnAllot3.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender3.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender3"] = txtSender3.Text;
            //            txtSender3.Text = "";
            //        }
            //    }
            //    if (hdnAllot4.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender4.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender4"] = txtSender4.Text;
            //            txtSender4.Text = "";
            //        }
            //    }
            //    if (hdnAllot5.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender5.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender5"] = txtSender5.Text;
            //            txtSender5.Text = "";
            //        }
            //    }
            //    if (hdnAllot6.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender6.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender6"] = txtSender6.Text;
            //            txtSender6.Text = "";
            //        }
            //    }
            //    if (hdnAllot7.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender7.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender7"] = txtSender7.Text;
            //            txtSender7.Text = "";
            //        }
            //    }
            //    if (hdnAllot8.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender8.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender8"] = txtSender8.Text;
            //            txtSender8.Text = "";
            //        }
            //    }
            //    if (hdnAllot9.Value == "1")
            //    {
            //        if (GetUAEAPIACCOUNTPROMO(txtSender9.Text) == "true")
            //        {
            //            flag = true;
            //        }
            //        else
            //        {
            //            ViewState["Sender9"] = txtSender9.Text;
            //            txtSender9.Text = "";
            //        }
            //    }
            //}
            //End UAEAPIAccounts


            //if (flag == false)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender Id  is Not Mapped with Operator !!');", true);
            //    return;
            //}
            //else
            //{
            //    if(Convert.ToString(ViewState["Sender"])!="" || Convert.ToString(ViewState["Sender1"])!="" || Convert.ToString(ViewState["Sender2"])!=""|| Convert.ToString(ViewState["Sender3"])!="" || Convert.ToString(ViewState["Sender4"])!="" || Convert.ToString(ViewState["Sender5"])!="" || Convert.ToString(ViewState["Sender6"])!=""|| Convert.ToString(ViewState["Sender7"])!=""|| Convert.ToString(ViewState["Sender8"])!=""|| Convert.ToString(ViewState["Sender9"]) != "")
            //    {
            //        string ExSenderid = ViewState["Sender"].ToString()+" "+ViewState["Sender1"].ToString() + " " + ViewState["Sender2"].ToString() + " " + ViewState["Sender3"].ToString() + " " + ViewState["Sender4"].ToString() + " " + ViewState["Sender5"].ToString() + " " + ViewState["Sender6"].ToString() + " " + ViewState["Sender7"].ToString() + " " + ViewState["Sender8"].ToString() + " " + ViewState["Sender9"].ToString();
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Excluded Sender ID " + ExSenderid + " ');", true);
            //        return;
            //    }   
            //}
        }

    
        protected void btnAllotUpdate_Click(object sender, EventArgs e)
        {
            if (hdnAllotCount.Value == "") hdnAllotCount.Value = "1";
            List<string> liSenderId = new List<string>();
            ValidateAllotSender(liSenderId);
            if (Convert.ToString(ViewState["Sender"]) != "")
            {
                liSenderId.Remove(ViewState["Sender"].ToString());
            }
            if (Convert.ToString(ViewState["Sender1"]) != "")
            {
                liSenderId.Remove(ViewState["Sender1"].ToString());
            }
            if (Convert.ToString(ViewState["Sender2"]) != "")
            {
                liSenderId.Remove(ViewState["Sender2"].ToString());
            }
            if (Convert.ToString(ViewState["Sender3"]) != "")
            {
                liSenderId.Remove(ViewState["Sender3"].ToString());
            }
            if (Convert.ToString(ViewState["Sender4"]) != "")
            {
                liSenderId.Remove(ViewState["Sender4"].ToString());
            }
            if (Convert.ToString(ViewState["Sender5"]) != "")
            {
                liSenderId.Remove(ViewState["Sender5"].ToString());
            }
            if (Convert.ToString(ViewState["Sender6"]) != "")
            {
                liSenderId.Remove(ViewState["Sender6"].ToString());
            }
            if (Convert.ToString(ViewState["Sender7"]) != "")
            {
                liSenderId.Remove(ViewState["Sender7"].ToString());
            }
            if (Convert.ToString(ViewState["Sender8"]) != "")
            {
                liSenderId.Remove(ViewState["Sender8"].ToString());
            }
            if (Convert.ToString(ViewState["Sender9"]) != "")
            {
                liSenderId.Remove(ViewState["Sender9"].ToString());
            }
            liSenderId = liSenderId.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            SetAttribueScheduleRows(hdnAllotCount.Value);
            if (hdnAllotCount.Value != liSenderId.Count.ToString())
            {
                modalpopupallot.Show();
                return;
            }

            Helper.Util ob = new Helper.Util();

            if (Convert.ToString(Session["UserType"]) == "ADMIN")
            {
                foreach (string senderId in liSenderId)
                {
                    bool exist = ob.CheckSenderIDforAdmin(senderId, Convert.ToString(Session["User"]));
                    if (!exist)
                    {
                        string msg = string.Format("Entered Sender ID ({0}) is not available.", senderId);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                        return;
                    }
                }
            }

            string ccode = ddlCCode.SelectedValue;

            foreach (string senderId in liSenderId)
            {
                string s = senderId; // txtSender.Text;
                string un = Convert.ToString(ViewState["UN"]);
                string user = Convert.ToString(Session["User"]);

                if (Convert.ToString(Session["UserType"]) == "ADMIN")
                {
                    ob.UpdateSender(un, s, user, "UPDATEONLY", ccode);
                }
                else if (Convert.ToString(Session["UserType"]) == "SYSADMIN")
                {
                    bool exist = ob.CheckSenderID(s);

                    if (exist == false)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Entered Sender ID is not available.');", true);
                        //udate senderid on username and insert into sernderid master
                        ob.UpdateSender(un, s, user, "UPDATEANDINSERT", ccode);
                    }
                    else
                    {
                        ob.UpdateSender(un, s, user, "UPDATEONLY", ccode);
                    }
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender id successfully updated.');", true);
        }

        public string GetUAEAPIACCOUNTPROMO(string senderid)
        {
            string sql = "if exists(select * from UAEAPIACCOUNTPROMO up inner join OperatorSenderId os on up.ACCOUNT=os.Provider where os.Active=1 and os.SID='" + senderid + "' and up.userid='" + ViewState["UN"].ToString() + "') begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }
        public string GetUAEAPIAccounts(string senderid)
        {
            string sql = "if exists(select * from UAEAPIAccounts ua inner join OperatorSenderId os on ua.ACCOUNT=os.Provider where ua.Active=1 and os.Active=1 and os.SID='" + senderid + "' and ua.userid='" + ViewState["UN"].ToString() + "') begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string Getsmppaccountuserid(string senderid)
        {
            string sql = @"if exists(select  su.Userid, s.PROVIDER, os.SID from smppaccountuserid su 
                inner join smppsetting s on s.smppaccountid = su.smppaccountid
                inner join OperatorSenderId os on os.Provider = s.PROVIDER where s.ACTIVE = 1 and os.Active=1 and os.SID='" + senderid + "' and su.userid='" + ViewState["UN"].ToString() + "') begin select 'true' end";
            string res = Convert.ToString(database.GetScalarValue(sql));
            return res;
        }

        public string CheckUAEAccount()
        {
            string sql = "if exists (select * from customer where username='" + ViewState["UN"].ToString() + "' and  countrycode in ('971','966')) begin select 'true' end";
            string flag = Convert.ToString(database.GetScalarValue(sql));
            return flag;
        }
        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            modalpopupallot.Hide();
        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            //ViewState["ToDate"] = txtTo.Text;
        }

        protected void lnkbtnRemove_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField h = (HiddenField)gvro.FindControl("hdnUserId");
            string un = h.Value;
            ViewState["UN"] = un;
            ModalPopuprm.Show();

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            string s = txtSenderrm.Text;
            string un = Convert.ToString(ViewState["UN"]);
            string user = Convert.ToString(Session["User"]);
            if (Convert.ToString(Session["UserType"]) == "ADMIN")
            {
                bool exist = ob.CheckSenderID(s);
                if (!exist)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Entered Sender ID is not available.');", true);
                    return;
                }
                else
                {
                    int count = ob.CountSenderId(un);
                    if (count < 2)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Only one Sender ID exists of this user. Can not remove.');", true);
                        return;
                    }
                    ob.RemoveSender(s, un);

                }

            }
            else if (Convert.ToString(Session["UserType"]) == "SYSADMIN")
            {
                bool exist = ob.CheckSenderID(s);

                if (!exist)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Entered Sender ID is not available.');", true);
                    //udate senderid on username and insert into sernderid master
                    // ob.UpdateSender(un, s, user, "UPDATEANDINSERT");
                }
                else
                {
                    int count = ob.CountSenderId(un);
                    if (count < 2)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Entered Sender ID can not remove.');", true);
                        return;
                    }
                    ob.RemoveSender(s, un);
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Sender id successfully removed.');", true);
        }
    }
}