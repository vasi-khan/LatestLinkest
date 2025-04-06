using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class SenderListU : System.Web.UI.Page
    {

        string user = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                if (rblFilter.SelectedValue == "1")
                { 
                    DataTable dt = ob.GetPendingSenderIdList(user);
                    grv.DataSource = dt; 
                    grv.DataBind();
                    GridFormat(dt);
                }
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            GetData();
        }
        public void GetData()
        {
            DataTable dt = new DataTable();

            Helper.Util ob = new Helper.Util();
            user = Convert.ToString(Session["UserID"]);
            if (rblFilter.SelectedValue == "1")
                dt = ob.GetPendingSenderIdList(user);
            else if (rblFilter.SelectedValue == "2")
                dt = ob.GetRejectedSenderIdList(user);
            else
                dt = ob.GetApprovedSenderIdList(user);

            grv.DataSource = null;
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


        private void ExportToExcel()
        {
            string attachment = "attachment; filename=SenderIdListReport.xls";
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

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
                ExportToExcel();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);

        }


    }
}