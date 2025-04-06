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
    public partial class credit_debit_logs1 : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetCRDRLog("02/JAN/1900", "02/JAN/1900", user, usertype, Convert.ToString(Session["DLT"]),Convert.ToString(Session["EMPCODE"]));
                //DataTable dt = ob.GetCRDRReport("02/JAN/1900", "02/JAN/1900", usertype, user);
                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;
            try
            {
                GetData();
                txtFrm.Text = hdntxtFrm.Value;
                txtTo.Text = hdntxtTo.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
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
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date can not be Empty !');", true);
                return;
            }
            else
            {
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.999";
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
            int IntoCreditDebit = 0;
            if (chkIntoCreditDebit.Checked)
            {
                IntoCreditDebit = 1;
            }
            
            //if()
            Helper.Util ob = new Helper.Util();
            if (txtUserId.Text.Trim() != "")
            {
                string chkusr = ob.CheckUser(txtUserId.Text.Trim());
                if (chkusr == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('UserID not Found !');", true);
                    return;
                }
            }
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);

            DataTable dt = ob.GetCRDRLog(s1, s2, user, usertype, Convert.ToString(Session["DLT"]), Convert.ToString(Session["EMPCODE"]), IntoCreditDebit, txtUserId.Text.Trim());
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

        protected void btnClosePopup_Click(object sender, EventArgs e)
        {
            //modalpopuppwd.Hide();
            GetData();
        }

        protected void btnViewSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;
                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label l = (Label)gvro.FindControl("lblUserId");
                Label l2 = (Label)gvro.FindControl("lblsender");
                HiddenField hf = (HiddenField)gvro.FindControl("hdnfrmdt");
                HiddenField ht = (HiddenField)gvro.FindControl("hdntodt");
                s1 = hf.Value;
                s2 = ht.Value;

                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetCRDRLogDetailSMS(s1, s2, l.Text);
                Session["rptDetail"] = dt;
                if (dt.Rows.Count > 0)
                {
                    grvDtl1.DataSource = null;
                    grvDtl1.DataSource = dt;
                    grvDtl1.DataBind();

                    grvDtl1.UseAccessibleHeader = true;
                    grvDtl1.HeaderRow.TableSection = TableRowSection.TableHeader;

                    if (grvDtl1.TopPagerRow != null)
                    {
                        grvDtl1.TopPagerRow.TableSection = TableRowSection.TableHeader;
                    }
                    if (grvDtl1.BottomPagerRow != null)
                    {
                        grvDtl1.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                    }
                    if (dt.Rows.Count > 0)
                        grvDtl1.FooterRow.TableSection = TableRowSection.TableFooter;

                    pnlPopUp_Detail_ModalPopupExtender1.Show();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvro = (GridViewRow)btn.NamingContainer;
                Label l = (Label)gvro.FindControl("lblUserId");
                Label l2 = (Label)gvro.FindControl("lblsender");
                HiddenField hf = (HiddenField)gvro.FindControl("hdnfrmdt");
                HiddenField ht = (HiddenField)gvro.FindControl("hdntodt");
                s1 = hf.Value;
                s2 = ht.Value;

                int IntoCreditDebit = 0;
                if (chkIntoCreditDebit.Checked)
                {
                    IntoCreditDebit = 1;
                }
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetCRDRLogDetail(s1, s2, l.Text, IntoCreditDebit);
                Session["rptDetail"] = dt;
                if (dt.Rows.Count > 0)
                {
                    grvDtl.DataSource = null;
                    grvDtl.DataSource = dt;
                    grvDtl.DataBind();

                    grvDtl.UseAccessibleHeader = true;
                    grvDtl.HeaderRow.TableSection = TableRowSection.TableHeader;

                    if (grvDtl.TopPagerRow != null)
                    {
                        grvDtl.TopPagerRow.TableSection = TableRowSection.TableHeader;
                    }
                    if (grvDtl.BottomPagerRow != null)
                    {
                        grvDtl.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                    }
                    if (dt.Rows.Count > 0)
                        grvDtl.FooterRow.TableSection = TableRowSection.TableFooter;

                    pnlPopUp_Detail_ModalPopupExtender.Show();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        private void ExportToExcel()
        {
            string attachment = "attachment; filename=CR_DRReport.xls";
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

        private void DatatableToExcel(DataTable dt)
        {

            string attachment = "attachment; filename=CR_DRReport.xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            string tab = "";

            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    //HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    HttpContext.Current.Response.Write(tab + dr[i].ToString().Replace('\n', ' ').Replace(Convert.ToChar(10), ' ').Replace(Convert.ToChar(13), ' '));

                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            // Response.End();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
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

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (hdntxtFrm.Value.Trim() == "" || hdntxtTo.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);// + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.997";
            }

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);             
            DataTable dt = new DataTable();

            if (usertype.ToUpper() == "ADMIN")
            {                
                dt = ob.GetCRDRLogDetailSMS_Admin(s1, s2, usertype, Convert.ToString(Session["DLT"]));
            }
            else
            {
                dt = ob.GetCRDRLogDetailSMS(s1, s2);
            }

            if (dt.Rows.Count > 0)
                DatatableToCSV(dt, "CR_DRReport.csv");
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);

        }


    }
}