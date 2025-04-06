using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smsSummary
{
    public partial class smsSummary : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["User"]) == "")
            {
                Response.Redirect("Login.aspx");
            }
            lblUserid.Text = Convert.ToString(Session["User"]);
            if (!IsPostBack)
            {
                BindData();
            }
        }
        public void BindData()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetTempIdAndName("MIM2000025");
            if (dt.Rows.Count == 0)
            {
                ddlTempIdAndName.Items.Clear();
                return;
            }
            ddlTempIdAndName.DataSource = dt;
            ddlTempIdAndName.DataTextField = "tempname";
            ddlTempIdAndName.DataValueField = "templateid";
            ddlTempIdAndName.DataBind();
            ListItem objListItem = new ListItem("--Select--", "0");
            ddlTempIdAndName.Items.Insert(0, objListItem);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ddlTempIdAndName.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Please Select Template !');", true);
                ddlTempIdAndName.Focus();
                return;
            }
            if (txtFrm.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date Can not be Empty !');", true);
                txtFrm.Focus();
                return;
            }
            if (txtTo.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('To Date Can not be Empty !');", true);
                txtTo.Focus();
                return;
            }
            
            s1 = Convert.ToDateTime(txtFrm.Text, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            s2 = Convert.ToDateTime(txtTo.Text, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            
            if (Convert.ToDateTime(s1) > Convert.ToDateTime(s2))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('From Date can not be above To Date.');", true);
                return;
            }
            string TempIdAndName = ddlTempIdAndName.SelectedValue.ToString();
            if (TempIdAndName == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Template ID');", true);
                return;
            }
            string _user = "MIM2000025";
            string Action = string.Empty;
            string checkdate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            string checkdateform = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (s1 == checkdateform && s2 == checkdate)
            {
                Action = "1";
            }
            else if (s1 != checkdateform && s2 == checkdate)
            {
                Action = "2";
            }
            else if (s1 != checkdateform && s2 != checkdate)
            {
                Action = "3";
            }
            DataTable dt = new DataTable();
            dt = ob.GetTemplateWiseSMSSummary(_user, s1, s2, TempIdAndName, Action);
            ViewState["dt"] = dt;
            grv.DataSource = null;
            grv.DataSource = dt;
            //grv.Columns[1].Visible = false;
            //grv.Columns[2].Visible = true;
            SetFooterValue(dt);
            grv.DataBind();
            GridFormat(dt);
        }
        private void SetFooterValue(DataTable copyDataTable)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] {
                new DataColumn("1"),
                new DataColumn("2"),
                new DataColumn("3"),
                new DataColumn("4"),
                new DataColumn("5"),
                new DataColumn("6"),
                new DataColumn("7"),
                new DataColumn("8"),

            });
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(Submitted)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Delivered)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(Failed)", string.Empty);
            object sumUnknown;
            sumUnknown = copyDataTable.Compute("Sum(Unknown)", string.Empty);

            grv.Columns[4].FooterText = "Total : ";
            grv.Columns[5].FooterText = sumSubmitted.ToString();
            grv.Columns[6].FooterText = sumDelivered.ToString();
            grv.Columns[7].FooterText = sumFailed.ToString();
            grv.Columns[8].FooterText = sumUnknown.ToString();
            dt.Rows.Add("", "", "Total ", sumSubmitted.ToString(), sumDelivered.ToString(), sumFailed.ToString(), sumUnknown.ToString());
            dt.AcceptChanges();
            Session["FottorValue"] = dt;
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("smsSummary.aspx");
        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (grv.Rows.Count > 0)
            {
                DataView view = new DataView(ViewState["dt"] as DataTable);
                DataTable distinctValues = view.ToTable(true, "SMSDATE", "userid", "senderid", "Submitted", "Delivered", "Failed", "Unknown");

                DatatableToCSV(distinctValues, "SMSSummary.csv");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found !!')", true);
            }
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

                DataTable FDT = Session["FottorValue"] as DataTable;
                if (FDT != null && FDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in FDT.Rows)
                    {
                        for (int k = 0; k < FDT.Columns.Count; k++)
                        {
                            columnbind.Append(dr[k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}