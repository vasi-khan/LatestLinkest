using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;

namespace eMIMPanel
{
    public partial class SignUp1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillCountry();
            }
        }
        public void fillCountry()
        {
            DataTable dt = new Util().GetCountryForSignUp();

            ddlCountryCode.DataSource = dt;
            ddlCountryCode.DataValueField = "counryCode";
            ddlCountryCode.DataTextField = "country";
            ddlCountryCode.DataBind();

            //ListItem objListItem = new ListItem("--Select--", "0");
            //ddlCountryCode.Items.Insert(0, objListItem);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";
                msg.InnerText = "Please enter name.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtName.Focus();
                return;
            }
            if (ddlCountryCode.SelectedValue.Trim() == "0")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please select country.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                ddlCountryCode.Focus();
                return;
            }
            if (txtMobile.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter mobile no.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtMobile.Focus();
                return;
            }
            int moblen = Convert.ToInt32(database.GetScalarValue("select  mobLength from tblCountry where counryCode='" + ddlCountryCode.SelectedValue + "'"));

            if (txtMobile.Text.Trim().Length != moblen)
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter valid mobile no.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtMobile.Focus();
                return;
            }
            if (txtEmailId.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email id.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("@"))
            { }
            else
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email address in xxxxx@yyyy.zzz format.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtEmailId.Focus();
                return;
            }
            if (txtEmailId.Text.Contains("."))
            { }
            else
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter email address in xxxxx@yyyy.zzz format.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);

                txtEmailId.Focus();
                return;
            }

            if (txtCompany.Text.Trim() == "")
            {
                sptitle.InnerText = "Warning";

                msg.InnerText = "Please enter company name.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
                txtCompany.Focus();
                return;
            }

            //if (txtDesignation.Text.Trim() == "")
            //{
            //    sptitle.InnerText = "Warning";

            //    msg.InnerText = "Please enter designation.";

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);
            //    txtDesignation.Focus();
            //    return;
            //}

            DataTable dt = new DataTable();
            dt.Columns.Add("TOMOBILE");
            DataRow  myDataRow = dt.NewRow();
            myDataRow["TOMOBILE"] = txtMobile.Text.Trim();
            dt.Rows.Add(myDataRow);
            
            string str = new Helper.Util().InsertSignUp(txtName.Text.Trim(), txtMobile.Text.Trim(), txtEmailId.Text.Trim(), txtDesignation.Text.Trim(), txtCompany.Text.Trim(), ddlCountryCode.SelectedValue);


            msg.InnerText = str;


            if (str.Contains("successfully"))
            {
                sptitle.InnerText = "Success";

                txtName.Text = ""; txtMobile.Text = ""; txtCompany.Text = ""; txtEmailId.Text = ""; txtDesignation.Text = "";//ddlCountryCode.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg1();", true);

                //DataTable dtOBD = database.GetDataTable("select n.* from [10.10.33.252].OBD.DBO.USEROBDNAME U JOIN [10.10.33.252].OBD.DBO.OBDName N ON U.SCENARIOKEY=N.ScenarioKey WHERE USERID='MIMO00006' and Name='MiM_Signup_Form'");

                //  string authKey = System.Configuration.ConfigurationManager.AppSettings["obdAuthkey"].ToString();

                //  new Helper.Util().obdpostapi_Bulk(dt,Convert.ToString(dtOBD.Rows[0]["ScenarioKey"]), authKey, Convert.ToString(dtOBD.Rows[0]["DIDNUMBER"]));

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showpopmsg();", true);

            }



        }
    }
}