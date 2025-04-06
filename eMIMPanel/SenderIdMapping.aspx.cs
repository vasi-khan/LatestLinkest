using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class SenderIdMapping : System.Web.UI.Page
    {
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                BindCountryCode();
                BindRoute();
                BindGrid(null);
            }

        }

        public void BindCountryCode()
        {
            DataTable dt = ob.SP_GetCountry();
            ddlChangeCountry.DataSource = dt;
            ddlChangeCountry.DataTextField = "Country";
            ddlChangeCountry.DataValueField = "Country";
            ddlChangeCountry.DataBind();
            ListItem objListItem = new ListItem("--Select Country--", "0");
            ddlChangeCountry.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlChangeCountry.SelectedIndex = 1;
            else
                ddlChangeCountry.SelectedIndex = 0;
        }

        public void BindRoute()
        {
            DataTable dt = ob.SP_GetRoute();
            ddlChangeRoute.DataSource = dt;
            ddlChangeRoute.DataTextField = "provider";
            ddlChangeRoute.DataValueField = "smppaccountid";
            ddlChangeRoute.DataBind();
            ListItem objListItem = new ListItem("--Select Route--", "0");
            ddlChangeRoute.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlChangeRoute.SelectedIndex = 1;
            else
                ddlChangeRoute.SelectedIndex = 0;
        }

        protected void BindGrid(DataTable dt)
        {
            grv.DataSource = dt;
            grv.DataBind();
        }

        protected void lnkbtnCr_Click(object sender, EventArgs e)
        {
            if (ddlChangeCountry.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Country Code !!');", true);
                return;
            }
            if (ddlChangeRoute.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Route !!');", true);
                return;
            }
            string input = ddlChangeRoute.SelectedItem.Text;
            string SmppAccountId = input.Split('-').Last().Trim();
            string input1 = ddlChangeCountry.SelectedItem.Text;
            string CountryCode = input1.Split('-').Last().Trim();
            DataTable dt = ob.GetValueForGrid(CountryCode, SmppAccountId);
            if (dt.Rows.Count > 0)
            {
                BindGrid(dt);
            }
        }

        protected void btnaddsenderid_Click(object sender, EventArgs e)
        {
            if (ddlChangeCountry.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Country Code !!');", true);
                return;
            }
            if (ddlChangeRoute.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Route !!');", true);
                return;
            }
            if (txtsenderid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter SenderId !!');", true);
                return;
            }
            string input = ddlChangeRoute.SelectedItem.Text;
            string SmppAccountId = input.Split('-').Last().Trim();
            string Systemid = input.Split('-')[1].Trim();
            string input1 = ddlChangeCountry.SelectedItem.Text;
            string CountryCode = input1.Split('-').Last().Trim();
            DataTable dt = ob.SP_InsertValueForSenderMapping(SmppAccountId, Systemid,txtsenderid.Text.Trim(),CountryCode);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successfully Inserted !!');", true);
                Reset();
                BindGrid(dt);
                return;
            }
        }

        protected void Reset()
        {
            ddlChangeCountry.SelectedIndex = 0;
            ddlChangeRoute.SelectedIndex = 0;
            txtsenderid.Text = "";
        }

        protected void lnkbtndlt_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtndlt = (LinkButton)sender;
            GridViewRow grvrow = (GridViewRow)lnkbtndlt.NamingContainer;
            Label Country = (Label)grvrow.FindControl("ddlcountry");
            Label Smppaccounid = (Label)grvrow.FindControl("lblsmppaccid");
            Label SenderId = (Label)grvrow.FindControl("ddlSenderId");
            Label lblsysid = (Label)grvrow.FindControl("ddlRoute");
            //string input = ddlChangeRoute.SelectedItem.Text;
            string input = lblsysid.Text;
            string SysId = input.Split('-')[0].Trim();
            DataTable dt = ob.DeleteFromSenderMapping(Smppaccounid.Text, SysId, SenderId.Text, Country.Text);
            BindGrid(dt);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successfully Deleted !!');", true);
            return;
        }
    }
}