using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace eMIMPanel
{
    public partial class APITrafficRouteSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindItemtable();
            BindItemtable1();
            if (!Page.IsPostBack)
            {
                ddlSenderID.SelectedValue = "-1";
                BindCountryCode();
                BindRoute();
                BindSMMPAccountList();
            }
        }

        Helper.Util ob = new Helper.Util();
        string res = "";
        

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (txtproid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Profile ID !!');", true);
                Reset();
                return;
            }
            DataTable dt = ob.SP_GetProfileDetails(txtproid.Text.Trim(), "", "");
            if (dt.Rows.Count > 0)
            {
                txtfullname.Text = dt.Rows[0]["FULLNAME"].ToString();
                txtdefaultcountry.Text = dt.Rows[0]["DefaultCountry"].ToString();
                txtsender.Text = dt.Rows[0]["SENDERID"].ToString();
            }
            DataTable dt1 = ob.SP_GetProfileDetailsForGrid_API(txtproid.Text.Trim());
            
            if (dt1.Rows.Count > 0)
            {
                Session["AccountExit"] = dt1.Rows[0]["account"].ToString();


                ViewState["dtItems"] = dt1;


                BindGrid();
            }

            //DataTable dt2 = ob.SP_GetProfileDetailsForSenderGrid(txtproid.Text.Trim());
            //if (dt2.Rows.Count > 0)
            //{
            //    ViewState["dt1Items"] = dt2;
            //    ViewState["OldItems"] = dt2;
            //    BindSenderGrid();
            //}
            txtfullname.ReadOnly = true;
            txtdefaultcountry.ReadOnly = true;
            txtsender.ReadOnly = true;

        }

        public void BindGridView()
        {
            DataTable dt1 = ob.SP_GetProfileDetailsForGrid_API(txtproid.Text.Trim());
            if (dt1.Rows.Count > 0)
            {
                //dt1.Columns.Add("DeleteValue");

                ViewState["dtItems"] = dt1;
                Session["AccountExit"] = dt1.Rows[0]["account"].ToString();
                BindGrid();
            }

        }

        protected void lnkbtnshow_Click(object sender, EventArgs e)
        {
            if (txtprofile.Text.Trim() == "" && txtname.Text.Trim() == "" && txtcompname.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Any Field !!');", true);
                pnlPopUp_Detail_ModalPopupExtender.Show();
                return;
            }
            DataTable dt = ob.SP_GetProfileDetails(txtprofile.Text.Trim(), txtname.Text.Trim(), txtcompname.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                rpItems.DataSource = null;
                rpItems.DataSource = dt;
                rpItems.DataBind();
            }
            else
            {
                rpItems.DataSource = null;
                rpItems.DataBind();
            }
            pnlPopUp_Detail_ModalPopupExtender.Show();

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            pnlPopUp_Detail_ModalPopupExtender.Show();
        }

        protected void lnkselect_Click(object sender, EventArgs e)
        {
            pnlPopUp_Detail_ModalPopupExtender.Dispose();
            LinkButton lnkselect = sender as LinkButton;
            RepeaterItem Rptitem = (RepeaterItem)lnkselect.NamingContainer;
            Label lblprofileid = (Label)Rptitem.FindControl("lblprofileid1");
            string Userid = lblprofileid.Text;
            DataTable dt = ob.SP_GetProfileDetails_API(Userid, "", "");
            if (dt.Rows.Count > 0)
            {
                txtfullname.Text = dt.Rows[0]["FULLNAME"].ToString();
                txtdefaultcountry.Text = dt.Rows[0]["DefaultCountry"].ToString();
                txtsender.Text = dt.Rows[0]["SENDERID"].ToString();
                txtfullname.ReadOnly = true;
                txtdefaultcountry.ReadOnly = true;
                txtsender.ReadOnly = true;
            }
        }

        public void BindSenderID(string AccountID)
        {
            DataTable dt = ob.SP_GetAllSenderID(AccountID);
            ddlSenderID.DataSource = dt;
            ddlSenderID.DataTextField = "SenderId";
            ddlSenderID.DataValueField = "SenderId";
            ddlSenderID.DataBind();
            ListItem objListItem = new ListItem("--Select Sender ID--", "0");
            ddlSenderID.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlSenderID.SelectedIndex = 1;
            else
                ddlSenderID.SelectedIndex = 0;
        }

        public void BindCountryCode()
        {
            DataTable dt = ob.SP_GetCountry();
            ddlChangeCountry.DataSource = dt;
            ddlChangeCountry.DataTextField = "Country";
            ddlChangeCountry.DataValueField = "Country";
            ddlChangeCountry.DataBind();
            ListItem objListItem = new ListItem("--Select Country--", "0");
            ddlChangeCountry.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlChangeCountry.SelectedIndex = 1;
            else
                ddlChangeCountry.SelectedIndex = 0;

            ddlSenderCC.DataSource = dt;
            ddlSenderCC.DataTextField = "Country";
            ddlSenderCC.DataValueField = "Country";
            ddlSenderCC.DataBind();
            ddlSenderCC.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlSenderCC.SelectedIndex = 1;
            else
                ddlSenderCC.SelectedIndex = 0;
        }

        public void BindRoute()
        {
            DataTable dt = ob.SP_GetRoute();
            ddlSenderRoute.DataSource = dt;
            ddlSenderRoute.DataTextField = "provider";
            ddlSenderRoute.DataValueField = "smppaccountid";
            ddlSenderRoute.DataBind();
            ListItem objListItem = new ListItem("--Select Provider--", "0");
            ddlSenderRoute.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlSenderRoute.SelectedIndex = 1;
            else
                ddlSenderRoute.SelectedIndex = 0;


            ddlChangeRoute.DataSource = dt;
            ddlChangeRoute.DataTextField = "provider";
            ddlChangeRoute.DataValueField = "smppaccountid";
            ddlChangeRoute.DataBind();
            ddlChangeRoute.Items.Insert(0, objListItem);
            if (dt.Rows.Count == 1)
                ddlChangeRoute.SelectedIndex = 1;
            else
                ddlChangeRoute.SelectedIndex = 0;
        }

        protected void lnkbtnCr_Click(object sender, EventArgs e)
        {
            if (ddlChangeCountry.SelectedItem.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Select Country Code')", true);
                return;
            }
            if (ddlChangeRoute.SelectedItem.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Select Route')", true);
                return;
            }


            GetTableRow(ddlChangeCountry.SelectedValue, ddlChangeRoute.SelectedItem.Text, ddlChangeRoute.SelectedValue);
            BindGrid();

        }

        public void GetTableRow(string CcCode, string Route, string smppaccountid)
        {
            try
            {
                string input = CcCode;
                string CountryCode = input.Split('-').Last();

                if (Convert.ToString(Session["AccountExit"]) == "NotExitRecordSmmp")
                {
                    ViewState["dtItems"] = null;
                }

                if (ViewState["dtItems"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Route"),
                    new DataColumn("CountryCode"),
                    new DataColumn("smppaccountid")
                    //new DataColumn("DeleteValue")
                    });

                    dt.Rows.Add(Route, CountryCode, smppaccountid);
                    dt.AcceptChanges();
                    ViewState["dtItems"] = dt;
                    //ViewState["Newdata"] = dt;
                    BindGrid();
                }
                else
                {
                    bool a = true;
                    DataTable dtviewstate = (DataTable)ViewState["dtItems"];

                   
                    if (a)
                    {
                        dtviewstate.Rows.Add(Route, CountryCode, smppaccountid);
                        dtviewstate.AcceptChanges();
                        ViewState["dtItems"] = dtviewstate;
                        //ViewState["Newdata"] = dtviewstate;
                        BindGrid();

                    }
                    
                }

                lnkbtnsave_Click(null, null);
                //FinalSubmit();
            }
            catch (Exception EX)
            {

            }

        }

        protected void BindGrid()
        {
            DataTable dt= ViewState["dtItems"] as DataTable;
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    RptChangeRoute.DataSource = dt;
                  RptChangeRoute.DataBind();
                }
                else
                {
                    RptChangeRoute.DataSource = null;
                    RptChangeRoute.DataBind();
                }

            }
            
        }

        protected void RptChangeRoute_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dt = ViewState["dtItems"] as DataTable;
                string msg = ob.SP_Deletesmppaccountuserid(txtproid.Text.Trim(), dt.Rows[index]["CountryCode"].ToString(), dt.Rows[index]["smppaccountid"].ToString(), "SP_Deletesmppaccountuserid_API");
                BindGridView();
                BindSMMPAccountList();
                
            }
        }

        protected void BindSenderGrid()
        {
            RptSenderId.DataSource = ViewState["dt1Items"] as DataTable;
            RptSenderId.DataBind();
        }

        public void GetSenderIDTableRow(string Route, string CcCode1, string smppaccountid, string Senderid)
        {
            try
            {
                string input = CcCode1;
                string CountryCode = input.Split('-').Last();
                if (ViewState["dt1Items"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[4] {
                    new DataColumn("Route"),
                    new DataColumn("CountryCode"),
                    new DataColumn("smppaccountid"),
                    new DataColumn("Senderid")
                    });
                    dt.Rows.Add(Route, CountryCode, smppaccountid, Senderid);
                    dt.AcceptChanges();
                    ViewState["dt1Items"] = dt;
                    BindSenderGrid();
                }
                else
                {

                    DataTable dtviewstate = (DataTable)ViewState["dt1Items"];
                    dtviewstate.Rows.Add(Route, CountryCode, smppaccountid, Senderid);
                    //ViewState["NewRow"] = dtviewstate;
                    dtviewstate.AcceptChanges();
                    ViewState["dt1Items"] = dtviewstate;
                    BindSenderGrid();
                }
            }
            catch (Exception EX)
            {

            }

        }

        protected void RptSenderId_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dt = ViewState["dt1Items"] as DataTable;
                dt.Rows[index].Delete();
                //currentRow = dt.NewRow();
                dt.AcceptChanges();
                ViewState["dt1Items"] = dt;
                BindSenderGrid();
            }
        }

        protected void lnkbtnsender_Click(object sender, EventArgs e)
        {
            GetSenderIDTableRow(ddlSenderRoute.SelectedItem.Text, ddlSenderCC.SelectedValue, ddlSenderRoute.SelectedValue, ddlSenderID.SelectedValue);
            BindSenderGrid();
        }

        protected void Reset()
        {
            txtproid.Text = "";
            txtfullname.Text = "";
            txtdefaultcountry.Text = "";
            txtsender.Text = "";
            ddlChangeCountry.SelectedIndex = 0;
            ddlChangeRoute.SelectedIndex = 0;
            RptChangeRoute.DataSource = null;
            RptChangeRoute.DataBind();
            ddlSenderCC.SelectedIndex = 0;
            ddlSenderRoute.SelectedIndex = 0;
            if (divsenderid.Visible == true)
            {
                ddlSenderID.SelectedIndex = 0;
            }

            RptSenderId.DataSource = null;
            RptSenderId.DataBind();
            ViewState["dtItems"] = null;
            ViewState["dt1Items"] = null;
            BindItemtable();
            BindItemtable1();
        }

        protected void lnkbtnreset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void BindItemtable()
        {
            if (ViewState["dtItems"] != null)
            {
                DataTable dtitem = ViewState["dtItems"] as DataTable;
                RptChangeRoute.DataSource = dtitem;
                RptChangeRoute.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
             new DataColumn("Route"),
             new DataColumn("CountryCode"),
             new DataColumn("smppaccountid")

             });
                ViewState["dtItems"] = dt;
                DataTable dtitem = ViewState["dtItems"] as DataTable;
                RptChangeRoute.DataSource = dtitem;
                RptChangeRoute.DataBind();
            }
        }

        protected void BindItemtable1()
        {
            if (ViewState["dt1Items"] != null)
            {
                DataTable dtitem = ViewState["dt1Items"] as DataTable;
                RptSenderId.DataSource = dtitem;
                RptSenderId.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] {
             new DataColumn("Route"),
             new DataColumn("CountryCode"),
             new DataColumn("smppaccountid"),
             new DataColumn("Senderid")
             });
                ViewState["dt1Items"] = dt;
                DataTable dtitem = ViewState["dt1Items"] as DataTable;
                RptSenderId.DataSource = dtitem;
                RptSenderId.DataBind();
            }
        }


       
        protected void lnkbtnsave_Click(object sender, EventArgs e)
        {
            if (txtproid.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter Profile ID !!');", true);
                return;
            }

            DataTable dt = ViewState["dtItems"] as DataTable;
           res = ob.SP_InsertValueForSmppAccId_API(txtproid.Text.Trim(), dt);
            if (res == "Succesfully Inserted" || res == "Country Code Already Exist")
            {
                BindGridView();
                BindSMMPAccountList();
                //Session["AccountExit"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + res + "');", true);
                return;
            }

            //DataTable dt1 = ViewState["dt1Items"] as DataTable;
            //res = ob.SP_InsertValueForSenderIdMast(txtproid.Text.Trim(), dt1);
            //if (res == "Succesfully Inserted")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Inserted Successfully !!');", true);
            //    Reset();
            //    return;
            //}
        }

        protected void ddlSenderRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSenderRoute.SelectedIndex > 0)
            {
                string input = Convert.ToString(ddlSenderRoute.SelectedItem.Text);
                string smppaccountid = input.Split('-').Last().Trim();
                BindSenderID(smppaccountid);
            }

        }

        public void CheckDataTable()
        {
            //string sql = "";
            //DataTable dt = ViewState["dtItems"] as DataTable;
            //var rows = dt.Select("submitted = 0");
            //foreach (var row in rows)
            //    row.Delete();
            //dt.AcceptChanges();
            //return dt;
        }


        public void BindSMMPAccountList()
        {
            DataTable dt = ob.GetRecordDataTable("SP_GetSMMPAccountList_APIDetail");
           

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();

                }
                else
                {
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                }

            }
        }
    }
}