using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace eMIMPanel
{
    public partial class account_list : System.Web.UI.Page
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
            //s1 = h1.Value;
            //s2 = h2.Value;
            GetData();
            txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;

        }



        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be Empty !');", true);
                return;
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date can not be Empty !');", true);
                return;
            }
            if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
            {
                s1 = "";
                s2 = "";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above From Today Date.');", true);
                return;
            }
            if (Convert.ToDateTime(s2) < Convert.ToDateTime(s1))
            {
                hdntxtFrm.Value = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            DataTable dt = ob.GetCustomers_DEMO2(s1, s2, usertype, user, txtname.Text.Trim(), txtmobile.Text.Trim(), txtemailid.Text.Trim(), TemplateID.Text.Trim(), rblFilter.SelectedValue);
            ViewState["dt"] = dt;
            grv.DataSource = null;
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
        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            modalpopuppwd.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            HiddenField h = (HiddenField)gvro.FindControl("hdnpwd");
            Label l = (Label)gvro.FindControl("lblUserId");
            string un = h.Value;
            ViewState["UN"] = un;
            txtusername.Text = l.Text;
            txtpwd.Text = h.Value;
            myinput.Text = "User ID : " + l.Text + " Password : " + h.Value;
            //myinput.Visible = false;
            modalpopuppwd.Show();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);

        }

        protected void lnkCopy_Click(object sender, EventArgs e)
        {

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
        private void DatatableToCSV(DataTable dt, string FileName)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName.Replace(".xls", ".csv"));
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder columnbind = new StringBuilder();
                for (int k = 0; k < (dt).Columns.Count; k++)
                {

                    columnbind.Append(dt.Columns[k].ColumnName + ',');
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
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView((ViewState["dt"] as DataTable));
                DataTable distinctValues = view.ToTable(true, "compname", "username", "fullname", "senderid", "mobile", "Email", "balance", "status", "createdby");

                distinctValues.Columns["compname"].ColumnName = "Company Name";
                distinctValues.Columns["username"].ColumnName = "User Id";
                distinctValues.Columns["fullname"].ColumnName = "Name";


                DatatableToCSV(ViewState["dt"] as DataTable, "AccountList.csv");//ExportToExcel();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);

        }




    }
}