using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace eMIMPanel
{
    public partial class credit_debit_logs_Uold : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetCRDRLog("02/JAN/1900", "02/JAN/1900", user, usertype, Convert.ToString(Session["DLT"]) ,Convert.ToString(Session["EMPCODE"]));
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
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);// + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // + " 23:59:59.999";
            }
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetCRDRLog(s1, s2, user, usertype, Convert.ToString(Session["DLT"]),Convert.ToString(Session["EMPCODE"]));
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
                HiddenField hf =(HiddenField)gvro.FindControl("hdnfrmdt");
                HiddenField ht = (HiddenField)gvro.FindControl("hdntodt");
                s1 = hf.Value;
                s2 = ht.Value;
                
                Helper.Util ob = new Helper.Util();
                DataTable dt = ob.GetCRDRLogDetail(s1, s2, l.Text);
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
    }
}