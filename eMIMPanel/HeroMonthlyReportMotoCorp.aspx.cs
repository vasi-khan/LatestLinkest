using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using Ionic.Zip;
using ZipFile = System.IO.Compression.ZipFile;
using System.Globalization;
using System.Text;

namespace eMIMPanel
{
    public partial class HeroMonthlyReportMotoCorp : System.Web.UI.Page
    {
        string d1 = "";
        string d2 = "";
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                BindData();  //Bind Data For DropDown
                SetDropDownListItemColor();
            }
        }

        public void BindData()
        {
            try
            {

                //Bind Category(dtCategory) DropDown Data
                DataTable dtCategory = ob.GetCategory((string)Session["userId"]);
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ListItem objListItemCategory = new ListItem("--All--", "0");
                ddlCategory.Items.Insert(0, objListItemCategory);

                //Bind Location(ddlLocation) DropDown Data
                DataTable dtLocation = ob.GetLocation((string)Session["userId"]);
                ddlLocation.DataSource = dtLocation;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationID";
                ddlLocation.DataBind();
                ListItem objListItemLocation = new ListItem("--All--", "0");
                ddlLocation.Items.Insert(0, objListItemLocation);


                //Bind SubLocation(ddlSubLocation) DropDown Data
                DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],(string)Session["userId"]);
                ddlSubLocation.DataSource = dtSubLocation;
                ddlSubLocation.DataTextField = "SubLocationName";
                ddlSubLocation.DataValueField = "SubLocationID";
                ddlSubLocation.DataBind();
                ListItem objListItemSubLocation = new ListItem("--All--", "0");
                ddlSubLocation.Items.Insert(0, objListItemSubLocation);


                //Bind DealerCode(ddlDealerCode) DropDown Data
                DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"]);
                ddlDealerCode.DataSource = dtDealerCode;
                ddlDealerCode.DataTextField = "DLRName";
                ddlDealerCode.DataValueField = "DLRCODE";
                ddlDealerCode.DataBind();
                ListItem objListItemDealerCode = new ListItem("--All--", "0");
                ddlDealerCode.Items.Insert(0, objListItemDealerCode);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        //Bind Location(ddlLocation) DropDown Data
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            DataTable dtLocation = ob.GetLocation((string)Session["userId"],CategoryId);
            if (dtLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlLocation.Items.Insert(0, objListItem);

            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItemSubLocation = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItemSubLocation);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind SubLocation(ddlSubLocation) DropDown Data
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            DataTable dtSubLocation = ob.GetSubLocation((string)Session["userId"],CategoryId, LocationId);
            if (dtSubLocation.Rows.Count == 0)
            {
                ddlLocation.Items.Clear();
                return;
            }
            ddlSubLocation.DataSource = dtSubLocation;
            ddlSubLocation.DataTextField = "SubLocationName";
            ddlSubLocation.DataValueField = "SubLocationID";
            ddlSubLocation.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlSubLocation.Items.Insert(0, objListItem);

            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItemDealerCode = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItemDealerCode);
        }

        //Bind DealerCode(ddlDealerCode) DropDown Data
        protected void ddlSubLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryId = ddlCategory.SelectedValue.ToString().Trim();
            string LocationId = ddlLocation.SelectedValue.ToString().Trim();
            string SubLocationId = ddlSubLocation.SelectedValue.ToString().Trim();
            DataTable dtDealerCode = ob.GetDealerCode((string)Session["userId"],CategoryId, LocationId, SubLocationId);
            if (dtDealerCode.Rows.Count == 0)
            {
                ddlDealerCode.Items.Clear();
                return;
            }
            ddlDealerCode.DataSource = dtDealerCode;
            ddlDealerCode.DataTextField = "DLRName";
            ddlDealerCode.DataValueField = "DLRCODE";
            ddlDealerCode.DataBind();
            ListItem objListItem = new ListItem("--All--", "0");
            ddlDealerCode.Items.Insert(0, objListItem);
        }

        public void SetDropDownListItemColor()
        {
            foreach (ListItem item in ddlDealerCode.Items)
            {
                item.Attributes.CssStyle.Add("font-family", "Consolas");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        public void GetData()
        {
            Helper.Util ob = new Helper.Util();
            string UserName = Convert.ToString(Session["UserID"]);
            string Year = ddlYear.SelectedValue.ToString();
            string Month = ddlMonth.SelectedValue.ToString();
            string CategoryID = ddlCategory.SelectedValue.ToString();
            string LocationID = ddlLocation.SelectedValue.ToString();
            string SubLocationID = ddlSubLocation.SelectedValue.ToString();
            string Dealer = ddlDealerCode.SelectedValue.ToString();
            DataTable dt = ob.GetHeroMonthlyReportMotoCorp(UserName, Year, Month, CategoryID, LocationID, SubLocationID, Dealer);
            if (dt.Rows.Count > 0)
            {
                csv(dt);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('No Data Found.');", true);
                return;
            }
        }

        public void csv(DataTable dt)
        {
            try
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=HeroMonthlyReport.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                StringBuilder columnbind = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    if (DateTime.TryParse(dt.Columns[k].ColumnName, out DateTime date))
                    {
                        // Check if the column name can be parsed as a DateTime
                        columnbind.Append(date.Day + ",");
                    }
                    else
                    {
                        columnbind.Append(dt.Columns[k].ColumnName + ",");
                    }
                }


                columnbind.Append("\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                    }
                    columnbind.Append("\r\n");
                }
                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }
    }
}