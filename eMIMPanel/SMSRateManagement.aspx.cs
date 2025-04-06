using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace eMIMPanel
{
    public partial class SMSRateManagement : System.Web.UI.Page
    {
        string _user;
        string usertype;
        Helper.Util ob = new Helper.Util();

        protected void Page_Load(object sender, EventArgs e)
        {

            usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            _user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");

            if (!IsPostBack)
            {
                DataTable dtDtl = new DataTable();
                //dtDtl.Columns.Add("countrycode");
                //dtDtl.Columns.Add("RATE_NORMALSMS");
                //dtDtl.Columns.Add("RATE_SMARTSMS");
                //dtDtl.Columns.Add("RATE_CAMPAIGN");
                //dtDtl.Columns.Add("RATE_OTP");
                //dtDtl.Columns.Add("urlrate");
                //dtDtl.Columns.Add("DLTCHARGE");

                string username = Request.QueryString["A"];
                ViewState["username"] = username;

                DataTable dt = ob.GetUserParameterAsPerCountry(username);
                if (dt.Rows.Count > 0)
                {                    
                    txts1.Text = dt.Rows[0]["RATE_NORMALSMS"].ToString();
                    txts2.Text = dt.Rows[0]["RATE_SMARTSMS"].ToString();
                    txts3.Text = dt.Rows[0]["RATE_CAMPAIGN"].ToString();
                    txts4.Text = dt.Rows[0]["RATE_OTP"].ToString();
                    txtUrlRate.Text = dt.Rows[0]["urlrate"].ToString();
                    txtd1.Text = dt.Rows[0]["DLTCHARGE"].ToString();
                    lblUserId.Text = username;
                    ddlCountryCode.SelectedValue = dt.Rows[0]["countrycode"].ToString();

                    //DataRow dr = dtDtl.NewRow();
                    //dr[0] = ddlCountryCode.SelectedValue;
                    //dr[1] = txts1.Text;
                    //dr[2] = txts2.Text;
                    //dr[3] = txts3.Text;
                    //dr[4] = txts4.Text;
                    //dr[5] = txtUrlRate.Text;
                    //dr[6] = txtd1.Text;
                    //dtDtl.Rows.Add(dr);

                    grv.DataSource = dt;
                    grv.DataBind();

                    ViewState["Data"] = dt;

                }
            }
            //  Response.Redirect(string.Format("{0}/Intermediate.aspx?abc={1}&pqr={2}", subDomain, uid, pwd));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string un = Convert.ToString(ViewState["username"]);
            DataTable dtDtl = (DataTable)ViewState["Data"];

            if (Convert.ToBoolean(ViewState["IsEdit"]) == false)
            {
                bool exists = dtDtl.Select().ToList().Exists(row => row["countrycode"].ToString() == ddlCountryCode.SelectedValue);
                if (exists)
                {                   
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Rate Already Define for selected country');window.location='SMSRateManagement.aspx?A=" + un + "';", true);
                    return;
                }

                DataRow dr = dtDtl.NewRow();
                dr[0] = ddlCountryCode.SelectedValue;
                dr[1] = txtns1.Text == "" ? "0" : txtns1.Text;
                dr[2] = txtns2.Text == "" ? "0" : txtns2.Text;
                dr[3] = txtns3.Text == "" ? "0" : txtns3.Text;
                dr[4] = txtns4.Text == "" ? "0" : txtns4.Text;
                dr[5] = txtUrlRateN.Text == "" ? "0" : txtUrlRateN.Text;
                dr[6] = txtnd1.Text == "" ? "0" : txtnd1.Text;
                dr["countryName"] = ob.GetCountryName(ddlCountryCode.SelectedValue);
                dtDtl.Rows.Add(dr);

                grv.DataSource = null;
                grv.DataSource = dtDtl;
                grv.DataBind();

                ViewState["Data"] = dtDtl;
            }
            else
            {
                string ccode = Convert.ToString(ViewState["countryCode"]);
                var query = dtDtl.AsEnumerable().Where(r => r.Field<string>("countrycode") == ccode);
                foreach (var row in query.ToList())
                    row.Delete();

                dtDtl.AcceptChanges();

                DataRow dr = dtDtl.NewRow();
                dr[0] = ccode;
                dr[1] = txtns1.Text;
                dr[2] = txtns2.Text;
                dr[3] = txtns3.Text;
                dr[4] = txtns4.Text;
                dr[5] = txtUrlRateN.Text;
                dr[6] = txtnd1.Text;
                dtDtl.Rows.Add(dr);

                grv.DataSource = null;
                grv.DataSource = dtDtl;
                grv.DataBind();

                ViewState["Data"] = dtDtl;

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Rate Define for selected country');", true);

            ViewState["IsEdit"] = false;

        }

        private void SetEditableMode()
        {
            txtns1.ReadOnly = false;
            txtns2.ReadOnly = false;
            txtns3.ReadOnly = false;
            txtns4.ReadOnly = false;
            txtUrlRateN.ReadOnly = false;
            txtnd1.ReadOnly = false;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = (DataTable)ViewState["Data"];

            LinkButton lb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lb.NamingContainer;
            string ccode = ((Label)row.FindControl("lblCountry")).Text;

            var query = dtDtl.AsEnumerable().Where(r => r.Field<string>("countrycode") == ccode);
            foreach (var r in query.ToList())
                r.Delete();

            dtDtl.AcceptChanges();

            grv.DataSource = null;
            grv.DataSource = dtDtl;
            grv.DataBind();

            ViewState["Data"] = dtDtl;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

            LinkButton lb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lb.NamingContainer;
            string ccode = ((Label)row.FindControl("lblCountry")).Text;

            DataTable dt = (DataTable)ViewState["Data"];
            DataRow row1 = dt.AsEnumerable().FirstOrDefault(r => r.Field<string>("countrycode") == ccode);
            if (row1 != null)
            {
                ViewState["IsEdit"] = true;

                txts1.Text = row1["RATE_NORMALSMS"].ToString();
                txts2.Text = row1["RATE_SMARTSMS"].ToString();
                txts3.Text = row1["RATE_CAMPAIGN"].ToString();
                txts4.Text = row1["RATE_OTP"].ToString();
                txtUrlRate.Text = row1["urlrate"].ToString();
                txtd1.Text = row1["DLTCHARGE"].ToString();
                ddlCountryCode.SelectedValue = ccode;

                SetEditableMode();
            }

            ViewState["countryCode"] = ccode;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SetEditableMode();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dtDtl = (DataTable)ViewState["Data"];
            string sql = "";
            if (dtDtl.Rows.Count > 0)
            {
                sql = @"UPDATE CUSTOMER  SET rate_normalsms ='" + dtDtl.Rows[0]["rate_normalsms"] + @"',rate_campaign='" + dtDtl.Rows[0]["rate_campaign"] + @"',
                         rate_smartsms='" + dtDtl.Rows[0]["rate_smartsms"] + @"',rate_otp='" + dtDtl.Rows[0]["rate_otp"] + @"',urlrate='" + dtDtl.Rows[0]["urlrate"] + @"',dltcharge='" + dtDtl.Rows[0]["dltcharge"] + @"'
                            where username='" + lblUserId.Text + @"'";
                sql += " DELETE FROM smsrateaspercountry where username='" + lblUserId.Text + "'";
                foreach (DataRow row in dtDtl.Rows)
                {
                    sql += @" insert into smsrateaspercountry (username,countrycode,rate_normalsms,rate_campaign,rate_smartsms,rate_otp,urlrate,dltcharge)
                         Values('" + lblUserId.Text + @"', '" + row["countrycode"] + @"', '" + row["rate_normalsms"] + @"','" + row["rate_campaign"] + @"',
                            '" + row["rate_smartsms"] + @"','" + row["rate_otp"] + @"','" + row["urlrate"] + @"','" + row["dltcharge"] + @"') ";

                }
                ob.UpdateRateAspercountry(sql);
                string un = Convert.ToString(ViewState["username"]);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Rate updated successfuly');", true);

                // ELSE
                //BEGIN
                //     UPDATE smsrateaspercountry SET rate_normalsms = '" + row["rate_normalsms"] + @"', rate_campaign = '" + row["rate_campaign"] + @"',
                //      rate_smartsms = '" + row["rate_smartsms"] + @"', rate_otp = '" + row["rate_otp"] + @"', urlrate = '" + row["urlrate"] + @"', dltcharge = '" + row["dltcharge"] + @"'
                //         where username = '" + lblUserId.Text + @"' and countrycode = '" + row["countrycode"] + @"'
                // END ";
            }
        }


    }
}