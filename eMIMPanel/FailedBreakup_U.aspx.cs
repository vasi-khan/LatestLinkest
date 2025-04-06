using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class FailedBreakup_U : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {

            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("Login.aspx");
            if (!IsPostBack)
            {
                PopulateCampaign();

            }
        }

        public void PopulateCampaign()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetCampaignAll(Convert.ToString(Session["UserID"]));

            ddlCamp.DataSource = dt;
            ddlCamp.DataTextField = "campaignname";
            ddlCamp.DataValueField = "campaignname";
            ddlCamp.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlCamp.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlCamp.SelectedIndex = 1;
            else
                ddlCamp.SelectedIndex = 0;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            s1 = hdntxtFrm.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
        }

        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.Getfailbreakup(s1, s2, user, Convert.ToString(ddlCamp.SelectedValue));

            grv.DataSource = null;
            System.Data.DataColumn newColumn = new System.Data.DataColumn("From");
            newColumn.DefaultValue = s1;
            dt.Columns.Add(newColumn);
            System.Data.DataColumn newColumn1 = new System.Data.DataColumn("To");
            newColumn1.DefaultValue = s2;
            dt.Columns.Add(newColumn1);

            ViewState["From"] = s1;
            ViewState["To"] = s2;
            Session["ClickReportMaindt"] = dt;
            grv.DataSource = dt;
            grv.DataBind();

            GridFormat(dt);
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

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
                ExportToExcel();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
        }

        private void ExportToExcel()
        {
            string attachment = "attachment; filename=SMSFailureBreakup.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grv.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}