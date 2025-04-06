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
    public partial class SenderListA : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            Helper.Util ob = new Helper.Util();
            if (!string.IsNullOrEmpty(txtUserId.Text.Trim()))
                user = txtUserId.Text.Trim();
            DataTable dt = ob.GetSenderList(usertype, user);
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
            string attachment = "attachment; filename=AccountListReport.xls";
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

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string qy = "";
            foreach (GridViewRow gvrow in grv.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                string userid = ((Label)gvrow.FindControl("lblUserId")).Text;
                string senderid = ((Label)gvrow.FindControl("lblsender")).Text;
                string ccode = ((Label)gvrow.FindControl("lblccode")).Text;
                if (chk != null & chk.Checked)
                {
                    qy = qy + string.Format("Delete from senderidmast where userid='{0}' and senderid='{1}' AND countrycode='{2}' ;", userid, senderid, ccode);
                }
            }
            Helper.Util ob = new Helper.Util();
            if (string.IsNullOrEmpty(qy))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select SenderId. !!')", true);
                return;
            }
            else
                ob.DeleteSenderIdRequest(qy);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SenderId deleted successfully. !!')", true);

            GetData();

        }
    }
}