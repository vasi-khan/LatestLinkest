using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace eMIMPanel
{
    public partial class balance_management : System.Web.UI.Page
    {
        //string s1 = "";
        //string s2 = "";
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (usertype != "SYSADMIN")
                Response.Redirect("index.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //s1 = h1.Value;
            //s2 = h2.Value;
            GetData();
        }



        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            //if (txtFrm.Text.Trim() == "")
            //{
            //    s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            //else
            //{
            //    s1 = Convert.ToDateTime(txtFrm.Text).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    s2 = Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            //}
            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            DataTable dt = ob.GetCustomersWithBalance();
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
            modalpopupCRDR.Hide();
            GetData();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label l = (Label)gvro.FindControl("lblUserId");
            string un = l.Text;
            ViewState["UN"] = un;
            
            modalpopupCRDR.Show();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "ShowModal('DivPopUp','',550,400)", true);

        }
        protected void btnUpdateCRDR_Click(object sender, EventArgs e)
        {
            string un = Convert.ToString(ViewState["UN"]);
            string bal = txtbal.Text;
            if (txtbal.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Balance should not be blank.');", true);
                modalpopupCRDR.Show();
                return;
            }
            Helper.Util ob = new Helper.Util();
            string user = Convert.ToString(Session["User"]);
            string cd = (rdbCredit.Checked ? "C" : "D");
            string r = ob.SaveCrDrBalance(un, bal, cd, user, usertype,"");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + r + "');", true);
            GetData();
        }

        protected void btnUpdateRate_Click(object sender, EventArgs e)
        {
            string un = Convert.ToString(ViewState["UN"]);
            string s1o = Convert.ToString(ViewState["S1"]);
            string s2o = Convert.ToString(ViewState["S2"]);
            string s3o = Convert.ToString(ViewState["S3"]);
            string s4o = Convert.ToString(ViewState["S4"]);
            string s5o = Convert.ToString(ViewState["S5"]);
            string d1o = Convert.ToString(ViewState["D1"]);
            string s1n = (txtns1.Text.Trim() == "" ? s1o : txtns1.Text);
            string s2n = (txtns2.Text.Trim() == "" ? s2o : txtns2.Text);
            string s3n = (txtns3.Text.Trim() == "" ? s3o : txtns3.Text);
            string s4n = (txtns4.Text.Trim() == "" ? s4o : txtns4.Text);
            string s5n = (txtUrlRateN.Text.Trim() == "" ? s5o : txtUrlRateN.Text);
            string d1n = (txtnd1.Text.Trim() == "" ? d1o : txtnd1.Text);

            Helper.Util ob = new Helper.Util();
            string user = Convert.ToString(Session["User"]);

            if (txtns1.Text.Trim() == "" && txtns2.Text.Trim() == "" && txtns3.Text.Trim() == "" && txtns4.Text.Trim() == "" && txtUrlRateN.Text.Trim()=="" && txtnd1.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter new price.');", true);
                modalpopupRATE.Show();
                return;
            }
            
            string r = ob.UpdateSMSPrice(un, s1o, s2o, s3o, s4o, s1n, s2n, s3n, s4n, s5o, s5n, d1o,d1n, user);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + r + "');", true);
            GetData();
        }
        protected void btnSMSRate_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label l = (Label)gvro.FindControl("lblUserId");
            HiddenField h1 = (HiddenField)gvro.FindControl("hdnrate_normalsms");
            HiddenField h2 = (HiddenField)gvro.FindControl("hdnrate_smartsms");
            HiddenField h3 = (HiddenField)gvro.FindControl("hdnrate_campaign");
            HiddenField h4 = (HiddenField)gvro.FindControl("hdnrate_otp");
            HiddenField h5 = (HiddenField)gvro.FindControl("hdnUrlrate");
            HiddenField d1 = (HiddenField)gvro.FindControl("hdn_dlt");
            string un = l.Text;
            ViewState["UN"] = un;
            txts1.Text = h1.Value;
            txts2.Text = h2.Value;
            txts3.Text = h3.Value;
            txts4.Text = h4.Value;
            txtUrlRate.Text = h5.Value;
            txtd1.Text = d1.Value;
            
            ViewState["S1"] = h1.Value;
            ViewState["S2"] = h2.Value;
            ViewState["S3"] = h3.Value;
            ViewState["S4"] = h4.Value;
            ViewState["S5"] = h5.Value;
            ViewState["D1"] = d1.Value;
            txtns1.Text = "";
            txtns2.Text = "";
            txtns3.Text = "";
            txtns4.Text = "";
            txtUrlRateN.Text = "";
            txtnd1.Text = "";
            modalpopupRATE.Show();
        }
    }
}