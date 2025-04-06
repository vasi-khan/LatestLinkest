using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class TemplateWiseSubmission : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            PopulateTemplate();
        }

        public void PopulateTemplate()
        {
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetTemplateId(Convert.ToString(Session["UserID"]), Convert.ToString(Session["Hidetemplateid"]));

                ddlTempID.DataSource = dt;
                ddlTempID.DataTextField = "TemplateID";
                ddlTempID.DataValueField = "Template";
                ddlTempID.DataBind();
                ListItem objListItem = new ListItem("--Select--", "0");
                ddlTempID.Items.Insert(0, objListItem);
                ddlTempID.SelectedIndex = 0;
        }

        public DataTable GetData()
        {
            string s1 = "", s2 = "";
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }

            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Sp_TemplateWiseSubmission";
                    //s1 = "2022-01-01";
                    cmd.Parameters.AddWithValue("@fromdate", s1);
                    
                    cmd.Parameters.AddWithValue("@todate", s2);
                    cmd.Parameters.AddWithValue("@userid", Convert.ToString(Session["UserId"]));

                    cmd.Parameters.AddWithValue("@templateid", (ddlTempID.SelectedValue=="0")? "": ddlTempID.SelectedValue);
                     
                    da.Fill(dt);
                    cmd.ExecuteNonQuery();
                }
                return dt;
            
            }
            catch (Exception ex)
            {
                throw ex;
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

        protected void lnkShow_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData();
            if (dt != null && dt.Rows.Count>0)
            {
                grv.Visible = true;
                object submitted;
                submitted = dt.Compute("Sum(SUBMITTED)", string.Empty);
                object delivered;
                delivered = dt.Compute("Sum(DELIVERED)", string.Empty);
                object failed;
                failed = dt.Compute("Sum(FAILED)", string.Empty);
                object unknown;
                unknown = dt.Compute("Sum(UNKNOWN)", string.Empty);

                grv.Columns[1].FooterText = "Total : ";
                grv.Columns[2].FooterText = submitted.ToString();
                grv.Columns[3].FooterText = delivered.ToString();
                grv.Columns[4].FooterText = failed.ToString();
                grv.Columns[5].FooterText = unknown.ToString();

                grv.DataSource = null;
                grv.DataSource = dt;
                grv.DataBind();
                GridFormat(dt);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data found !');", true);
                grv.DataSource = null;
                grv.Visible = false;
                return;
            }
        }
    }
}