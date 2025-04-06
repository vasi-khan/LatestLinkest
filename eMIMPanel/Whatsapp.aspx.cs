using Shortnr.Web.Business.Implementations;
using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Mvc;
using System.Web.Hosting;
using System.ComponentModel;
using Shortnr.Web.Business;
using System.Threading;
using System.Globalization;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using eMIMPanel.Helper;
using System.Web.Services;
using System.Text;

namespace eMIMPanel
{
    public partial class Whatsapp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            string _user = Convert.ToString(Session["UserID"]);
            if (_user == "")
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                LoadWAtemplate();
                setWABABal();
            }
        }
        public void setWABABal()
        {

            string sql = @" select WABARCSbal from CUSTOMER where username='"+Convert.ToString(Session["UserId"])+"'";
            lblbal.InnerText = Convert.ToString(database.GetScalarValue(sql));
        }
        public void LoadWAtemplate()
        {
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlwatempate.Items.Clear();
            string sql = @"select distinct  t.template,tempname,t.TemplateID as onlyTemplateID 
from templaterequest t  where username = '" + Convert.ToString(Session["UserID"]) + "' and  IsWhatsapp=1 and isnull(allotted,0)=1";
           DataTable dt = database.GetDataTable(sql); ;

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlwatempate.DataSource = dt;
                ddlwatempate.DataTextField = "tempname";
                ddlwatempate.DataValueField = "onlyTemplateID";
                ddlwatempate.DataBind();
            }
            ddlwatempate.Items.Insert(0, objListItem);
        }
        public void SendWA()
        {
            string country_code = Convert.ToString(Session["DEFAULTCOUNTRYCODE"]);
            string mobile = "";
            if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
            if (mobile.Trim() != "")
            {

              

                //if (country_code == "91")
                //{
                //    int maxlen = mobList.Max(arr => arr.Length);
                //    int minlen = mobList.Min(arr => arr.Length);
                //    if (maxlen != minlen)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 10 digits ]');", true);
                //        return;
                //    }
                //    if (maxlen != 10 || minlen != 10)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 10 digits ]');", true);
                //        return;
                //    }
                //}
                //else if (country_code == "971")
                //{
                //    int maxlen = mobList.Max(arr => arr.Length);
                //    int minlen = mobList.Min(arr => arr.Length);
                //    if (maxlen != minlen)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('All Mobile Numbers must be of [ 9 digits ]');", true);
                //        return;
                //    }
                //    if (maxlen != 9 || minlen != 9)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be of [ 9 digits ]');", true);
                //        return;
                //    }
                //    bool result = mobList.All(o => o.StartsWith("5"));
                //    if (result == false)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile Numbers must be starts with 5');", true);
                //        return;
                //    }
                //}
                //  if (maxlen == 10) country_code = "91";
            }
            else
            {
                if (Session["MOBILECOUNT"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter / Insert Mobile Numbers first.');", true);
                    return;
                }
            }
            if (mobList.Count>5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Only 5 mobile number allowed for demo.');", true);
                txtMobNum.Focus();
                return;
            }



            bool IsBalAvail = new Util().isBalanceAvailable_WABARCS(Session["UserId"].ToString(), mobList.Count);
            if (!IsBalAvail)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Insufficient Balance');window.location ='Whatsapp.aspx';", true);
                return;
            }

            if (rdbRCS.Checked)
            {
                new Util().sendRCS(txtpreview.Text.Trim(), mobList, Convert.ToString(Session["UserId"]), "", ddlwatempate.SelectedValue, ddlwatempate.SelectedItem.Text.Trim(), txtCampNm.Text.Trim());
            }
            else
            {
                new Util().sendWhatsapp(txtpreview.Text.Trim(), mobList, Convert.ToString(Session["UserId"]), "", ddlwatempate.SelectedValue, ddlwatempate.SelectedItem.Text.Trim(), txtCampNm.Text.Trim());
            }

             new Util().WABARCSBalUpdate(Session["UserId"].ToString(), mobList.Count);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Sent Successfully');window.location ='Whatsapp.aspx';", true);

        }
        protected void lnkbtnContinue_Click(object sender, EventArgs e)
        {

            if (rdbWhatsapp.Checked)
            {
                lblHeading.Text = "Whatsapp Preview";
                previewWA.Visible = true;
                previewRCS.Visible = false;
            }
            else
            {
                lblHeading.Text = "RCS Preview";

                previewWA.Visible = false;
                previewRCS.Visible = true;
            }
            if (ddlwatempate.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Pleas select templates');", true);
                ddlwatempate.Focus();
                return;
            }
            if (txtCampNm.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter campaign name');", true);
                txtCampNm.Focus();
                return;
            }
            if (txtpreview.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter enter message');", true);
                txtpreview.Focus();
                return;
            }
            string mobile = "";
            if (txtMobNum.Text != "") mobile = txtMobNum.Text.Replace('\n', ',');
            //string[] mo;
            //List<Mobile> mobList = new List<Mobile>();
            List<string> mobList1 = mobile.Split(',').ToList();
            List<string> mobList = mobList1.Select(item => item.Trim()).ToList();
            int z1 = mobList.RemoveAll(string.IsNullOrWhiteSpace);
            if (mobList.Count > 5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Only 5 mobile number allowed for demo.');", true);
                txtMobNum.Focus();
                return;
            }
            pnlPopUp_WA_ModalPopupExtender.Show();
            lblRecipientcnt.Text = mobList.Count.ToString();
            lblMessagecnt.Text = mobList.Count.ToString();

            txtpreview.Text = txtTemplatePreview.Text;
            Session["WAMsgText"]= txtTemplatePreview.Text;
        }
        protected void ddlwatempate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlwatempate.SelectedValue != "0")
            {
                DataTable dtT = new Util().GetWATemplateText(Convert.ToString(Session["UserID"]), ddlwatempate.SelectedValue);
                string msgtxt = dtT.Rows[0]["template"].ToString();
                txtpreview.Text = msgtxt;
                txtTemplatePreview.Text = msgtxt;
            }
                
        }
        protected void lnkbtnSend_Click(object sender, EventArgs e)
        {
            SendWA();
            setWABABal();
        }

        protected void lnkbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Whatsapp.aspx");
        }
    }
}