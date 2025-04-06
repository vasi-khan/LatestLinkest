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
using System.Configuration;

namespace eMIMPanel
{
    public partial class SendWABAOnLinkClick : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string _user = Convert.ToString(Session["User"]);
            if (_user == "")
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSendWabaLinkClick();
            if (dt.Rows.Count > 0)
            {
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
            else
            {
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
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
        protected void btnLinkID_Click(object sender, EventArgs e)
        {
            string LinkUserId = txtLinkUserId.Text.ToString().Trim();
            if (LinkUserId == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Linkext User ID.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();
            DataTable dtShortUrl = ob.GetShortUrl(LinkUserId);
            if (dtShortUrl.Rows.Count == 0)
            {
                ddlShortUrl.Items.Clear();
                return;
            }
            ddlShortUrl.DataSource = dtShortUrl;
            ddlShortUrl.DataTextField = "shorturl";
            ddlShortUrl.DataValueField = "id";
            ddlShortUrl.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlShortUrl.Items.Insert(0, objListItem);
        }

        protected void btnWABA_Click(object sender, EventArgs e)
        {
            string WABAUserID = txtWABAUserID.Text.ToString().Trim();
            if (WABAUserID == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter WABA User ID.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();
            DataTable dtRegiTempl = ob.GetwaTemplateId(WABAUserID);
            if (dtRegiTempl.Rows.Count == 0)
            {
                ddlShortUrl.Items.Clear();
                return;
            }
            ddlTempID.DataSource = dtRegiTempl;
            ddlTempID.DataTextField = "name1";
            ddlTempID.DataValueField = "TemplateID";
            ddlTempID.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempID.Items.Insert(0, objListItem);
        }

        protected void ddlTempID_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string WABALINKDB = ConfigurationManager.AppSettings["WABALINKDB"].ToString();
            string sql = @"select VartBodyText tbodytext,(case 
                           when ISNULL(tHeadType,'')='t' then 'Text'
                           when ISNULL(tHeadType,'')='i' then 'Image'
                           when ISNULL(tHeadType,'')='v' then 'Video'
                           when ISNULL(tHeadType,'')='d' then 'Document'
                           when ISNULL(tHeadType,'')='ca' then 'Call_To_Action'
                           when ISNULL(tHeadType,'')='qb' then 'Quick_Reply_Button'
                           when ISNULL(tHeadType,'')='ub' then 'URL_Button'
                           else '' end ) TempType,url from " + WABALINKDB + "template a with (nolock) where nid ='" + ddlTempID.SelectedValue + "' ";
            DataTable dtT = database.GetDataTable(sql);
            if (dtT.Rows.Count > 0)
            {
                txtWhatsappText.Text = dtT.Rows[0]["tbodytext"].ToString();
                Session["TempType"] = dtT.Rows[0]["TempType"].ToString();
                Session["Url"] = dtT.Rows[0]["url"].ToString();
            }
        }

        protected void lnkUpdateDate_Click(object sender, EventArgs e)
        {
            string WABALINKDB = ConfigurationManager.AppSettings["WABALINKDB"].ToString();
            string LinkUserId = txtLinkUserId.Text.ToString().Trim();
            string ShortsUrlID = ddlShortUrl.SelectedValue.ToString();
            string ShortsUrl = ddlShortUrl.SelectedItem.Text.ToString();
            string WABAUserID = txtWABAUserID.Text.ToString().Trim();
            string TempId = ddlTempID.SelectedValue.ToString();
            string TempIdName = ddlTempID.SelectedItem.Text.ToString();
            if (LinkUserId == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Linkext User ID.');", true);
                return;
            }
            if (ShortsUrlID == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Linkext ShortsUrlID.');", true);
                return;
            }
            if (WABAUserID == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter WABA User ID.');", true);
                return;
            }
            if (TempId == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Waba Templete.');", true);
                return;
            }
            string Segment = Convert.ToString(database.GetScalarValue(@"SELECT segment FROM short_urls WITH(NOLOCK) WHERE id='" + ShortsUrlID + "'"));
            string WabaApiKey= Convert.ToString(database.GetScalarValue(@"SELECT APIKEY FROM " + WABALINKDB + "CUSTOMER WITH(NOLOCK) WHERE username='" + WABAUserID + "'"));
            Helper.Util ob = new Helper.Util();
            ob.SaveSMSLinktoWABA(LinkUserId, ShortsUrlID.Trim(), ShortsUrl.Trim(), Segment.Trim(), WABAUserID.Trim(), WabaApiKey.Trim(), TempIdName.Trim(), Session["TempType"].ToString().Trim(), Session["Url"].ToString().Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Update Successfully.');", true);
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SendWABAOnLinkClick.aspx");
        }

        protected void chkAction_CheckedChanged(object sender, EventArgs e)
        {

            string sql = string.Empty;
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            string id = (row.FindControl("lblid") as Label).Text;
            if (chk.Checked)
            {
                sql = @"UPDATE SendWABAOnLinkClick SET Active=1 WHERE ID='" + id + "'";
            }
            else
            {
                sql = @"UPDATE SendWABAOnLinkClick SET Active=0 WHERE ID='" + id + "'";
            }
            database.ExecuteNonQuery(sql);
        }
    }
}