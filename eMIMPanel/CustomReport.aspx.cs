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
    public partial class CustomReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Convert.ToString(Session["User"]);
            if (user == "")
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                gridview();
            }

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (btnsave.Text=="Save")
            {

                try
                {

                    if (string.IsNullOrEmpty(txtprocedurename.Text.Trim()))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Procedure Name');", true);
                        return;

                    }
                    if (string.IsNullOrEmpty(txtreportname.Text.Trim()))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter Report Name');", true);
                        return;

                    }

                    if (chk1.Checked) {
                        if (String.IsNullOrEmpty(txtFN1.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName1');", true);
                            return;

                        }
                        if (ddltype1.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType1');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT1.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 1');", true);
                            return;

                        }
                    }

                    if (chk2.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN2.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName2');", true);
                            return;

                        }
                        if (ddltype2.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType2');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT2.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 2');", true);
                            return;

                        }
                    }
                    if (chk3.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN3.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName3');", true);
                            return;

                        }
                        if (ddltype3.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType3');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT3.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 3');", true);
                            return;

                        }
                    }
                    if (chk4.Checked)
                    {
                        if (String.IsNullOrEmpty(FN4.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName4');", true);
                            return;

                        }
                        if (ddltype4.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType4');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT4.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 4');", true);
                            return;

                        }
                    }
                    if (chk5.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN5.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName5');", true);
                            return;

                        }
                        if (ddltype5.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType5');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT5.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 5');", true);
                            return;

                        }
                    }
                    if (chk6.Checked)
                    {
                        if (String.IsNullOrEmpty(FN6.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName6');", true);
                            return;

                        }
                        if (ddltype6.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType6');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT6.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 6');", true);
                            return;

                        }

                    }
                    if (chk7.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN7.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName7');", true);
                            return;

                        }
                        if (ddltype7.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType7');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT7.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 7');", true);
                            return;

                        }
                    }
                    if (chk8.Checked)
                    {

                        if (String.IsNullOrEmpty(txtFN8.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName8');", true);
                            return;

                        }
                        if (ddltype8.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType8');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT8.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 8');", true);
                            return;

                        }
                    }
                    if (chk9.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN9.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName9');", true);
                            return;

                        }
                        if (ddltype9.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType9');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT9.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 9');", true);
                            return;

                        }
                    }
                    if (chk10.Checked)
                    {
                        if (String.IsNullOrEmpty(txtFN10.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterName10');", true);
                            return;

                        }
                        if (ddltype10.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter FilterType10');", true);
                            return;

                        }

                        if (string.IsNullOrEmpty(CLT10.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter ControlLabelText 10');", true);
                            return;

                        }
                    }
                    string sp_name = "SP_InsertInto_CustomReport";
                    List<SqlParameter> pram = new List<SqlParameter>();
                    pram.Add(new SqlParameter("@ReportName", txtreportname.Text.Trim()));
                    pram.Add(new SqlParameter("@ReportProcedureName", txtprocedurename.Text.Trim()));
                    int count = 0;


                    if (chk1.Checked)
                    {
                        

                        pram.Add(new SqlParameter("@Filter1active", 1));
                        pram.Add(new SqlParameter("@Filter1Name", "@" + txtFN1.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter1Type", ddltype1.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter1LabelText", CLT1.Text.Trim()));
                        count++;

                    }

                    if (chk2.Checked)
                    {

                        

                        pram.Add(new SqlParameter("@Filter2active", 1));
                        pram.Add(new SqlParameter("@Filter2Name", "@" + txtFN2.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter2Type", ddltype2.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter2LabelText", CLT2.Text.Trim()));
                        count++;


                    }


                    if (chk3.Checked)
                    {



                       

                        pram.Add(new SqlParameter("@Filter3active", 1));
                        pram.Add(new SqlParameter("@Filter3Name", "@" + txtFN3.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter3Type", ddltype3.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter3LabelText", CLT3.Text.Trim()));
                        count++;


                    }

                    if (chk4.Checked)
                    {

                        



                        pram.Add(new SqlParameter("@Filter4active", 1));
                        pram.Add(new SqlParameter("@Filter4Name", "@" + FN4.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter4Type", ddltype4.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter4LabelText", CLT4.Text.Trim()));
                        count++;


                    }

                    if (chk5.Checked)
                    {


                        


                        pram.Add(new SqlParameter("@Filter5active", 1));
                        pram.Add(new SqlParameter("@Filter5Name", "@" + txtFN5.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter5Type", ddltype5.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter5LabelText", CLT5.Text.Trim()));
                        count++;


                    }

                    if (chk6.Checked)
                    {

                        

                        pram.Add(new SqlParameter("@Filter6active", 1));
                        pram.Add(new SqlParameter("@Filter6Name", "@" + FN6.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter6Type", ddltype6.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter6LabelText", CLT6.Text.Trim()));
                        count++;


                    }
                    if (chk7.Checked)
                    {

                       

                        pram.Add(new SqlParameter("@Filter7active", 1));
                        pram.Add(new SqlParameter("@Filter7Name", "@" + txtFN7.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter7Type", ddltype7.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter7LabelText", CLT7.Text.Trim()));
                        count++;


                    }


                    if (chk8.Checked)
                    {


                        pram.Add(new SqlParameter("@Filter8active", 1));
                        pram.Add(new SqlParameter("@Filter8Name", "@" + txtFN8.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter8Type", ddltype8.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter8LabelText", CLT8.Text.Trim()));
                        count++;

                    }


                    if (chk9.Checked)
                    {


                        

                        pram.Add(new SqlParameter("@Filter9active", 1));
                        pram.Add(new SqlParameter("@Filter9Name", "@" + txtFN9.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter9Type", ddltype9.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter9LabelText", CLT9.Text.Trim()));
                        count++;


                    }

                    if (chk10.Checked)
                    {


                       

                        pram.Add(new SqlParameter("@Filter10active", 1));
                        pram.Add(new SqlParameter("@Filter10Name", "@" + txtFN10.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter10Type", ddltype10.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter10LabelText", CLT10.Text.Trim()));
                        count++;


                    }

                    pram.Add(new SqlParameter("@NoOfFilter", count));
                    pram.Add(new SqlParameter("action", "insert"));



                    Helper.Util db = new Helper.Util();
                    bool a = db.ShowMsgExecuteprocedure(sp_name, pram);

                    if (a == true)
                    {
                        gridview();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('SuccessFully InsertRecord')", true);
                        return;

                    }



                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
            else if (btnsave.Text=="Update")
            {

                try
                {
                    string sp_name = "SP_UpdateInto_CustomReport";
                    List<SqlParameter> pram = new List<SqlParameter>();
                    pram.Add(new SqlParameter("@ReportName", txtreportname.Text.Trim()));
                    pram.Add(new SqlParameter("@ReportProcedureName", txtprocedurename.Text.Trim()));
                    int count = 0;

                    if (chk1.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter1active", 1));
                        pram.Add(new SqlParameter("@Filter1Name", "@" + txtFN1.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter1Type", ddltype1.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter1LabelText", CLT1.Text.Trim()));
                        count++;

                    }

                    if (chk2.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter2active", 1));
                        pram.Add(new SqlParameter("@Filter2Name", "@" + txtFN2.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter2Type", ddltype2.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter2LabelText", CLT2.Text.Trim()));
                        count++;


                    }


                    if (chk3.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter3active", 1));
                        pram.Add(new SqlParameter("@Filter3Name", "@" + txtFN3.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter3Type", ddltype3.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter3LabelText", CLT3.Text.Trim()));
                        count++;


                    }

                    if (chk4.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter4active", 1));
                        pram.Add(new SqlParameter("@Filter4Name", "@" + FN4.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter4Type", ddltype4.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter4LabelText", CLT4.Text.Trim()));
                        count++;


                    }

                    if (chk5.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter5active", 1));
                        pram.Add(new SqlParameter("@Filter5Name", "@" + txtFN5.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter5Type", ddltype5.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter5LabelText", CLT5.Text.Trim()));
                        count++;


                    }

                    if (chk6.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter6active", 1));
                        pram.Add(new SqlParameter("@Filter6Name", "@" + FN6.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter6Type", ddltype6.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter6LabelText", CLT6.Text.Trim()));
                        count++;


                    }
                    if (chk7.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter7active", 1));
                        pram.Add(new SqlParameter("@Filter7Name", "@" + txtFN7.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter7Type", ddltype7.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter7LabelText", CLT7.Text.Trim()));
                        count++;


                    }


                    if (chk8.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter8active", 1));
                        pram.Add(new SqlParameter("@Filter8Name", "@" + txtFN8.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter8Type", ddltype8.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter8LabelText", CLT8.Text.Trim()));
                        count++;

                    }


                    if (chk9.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter9active", 1));
                        pram.Add(new SqlParameter("@Filter9Name", "@" + txtFN9.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter9Type", ddltype9.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter9LabelText", CLT9.Text.Trim()));
                        count++;


                    }

                    if (chk10.Checked)
                    {
                        pram.Add(new SqlParameter("@Filter10active", 1));
                        pram.Add(new SqlParameter("@Filter10Name", "@" + txtFN10.Text.Trim()));
                        pram.Add(new SqlParameter("@Filter10Type", ddltype10.SelectedItem.Text));
                        pram.Add(new SqlParameter("@Filter10LabelText", CLT10.Text.Trim()));
                        count++;


                    }

                    pram.Add(new SqlParameter("@NoOfFilter", count));
                    pram.Add(new SqlParameter("@id", ViewState["nID"]));



                    Helper.Util db = new Helper.Util();
                    bool a = db.ShowMsgExecuteprocedure(sp_name, pram);

                    if (a == true)
                    {
                        btnsave.Text = "Save";
                        gridview();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('SuccessFully Update Record')", true);
                        return;

                    }



                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            
            gridview();

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }

   
        public void gridview()
        {




            string sp_name = "SP_GetCustomReport";
            DataTable dt = Helper.database.GetDataTable(sp_name);

           

           


            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    grd_bind.DataSource = dt;
                    grd_bind.DataBind();
                }
                else
                {
                    grd_bind.DataSource = null;
                    grd_bind.DataBind();
                }

            }

            foreach (GridViewRow gr in grd_bind.Rows)
            {
                int index = gr.RowIndex;
                CheckBox chk = (grd_bind.Rows[index].FindControl("chkbox") as CheckBox);
                string Active = (grd_bind.Rows[index].FindControl("hd_Active") as HiddenField).Value;

                if (Active == "False")
                {
                    chk.Checked = false;

                }
                else if (Active == "True")
                {
                    chk.Checked = true;
                }


            }

        }

       

        protected void btnview_Click(object sender, EventArgs e)
        {

            GridViewRow gr = (GridViewRow)(sender as Button).Parent.Parent;

            int index = gr.RowIndex;
            int id = int.Parse((grd_bind.Rows[index].FindControl("hd_id") as HiddenField).Value);
            ViewState["nID"] = id;
            string SP_name = "SP_getCustomreporttgable";
            List<SqlParameter> pram = new List<SqlParameter>();
            pram.Add(new SqlParameter("@id",id));
            Helper.Util db = new Helper.Util();
            DataTable dt = db.ProcedureDatatable(SP_name, pram);

            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    txtreportname.Text = dt.Rows[0]["ReportName"].ToString();
                    txtprocedurename.Text = dt.Rows[0]["ReportProcedureName"].ToString();
                    txtprocedurename.Text = dt.Rows[0]["ReportProcedureName"].ToString();


                    if (dt.Rows[0]["Filter1active"].ToString()== "True")
                    {
                        chk1.Checked = true;
                        txtFN1.Text = dt.Rows[0]["Filter1Name"].ToString();
                        ddltype1.SelectedItem.Text= dt.Rows[0]["Filter1Type"].ToString();
                        CLT1.Text = dt.Rows[0]["Filter1LabelText"].ToString();


                    }
                    if (dt.Rows[0]["Filter2active"].ToString() == "True")
                    {
                        chk2.Checked = true;
                        txtFN2.Text = dt.Rows[0]["Filter2Name"].ToString();
                        ddltype2.SelectedItem.Text = dt.Rows[0]["Filter2Type"].ToString();
                        CLT2.Text = dt.Rows[0]["Filter2LabelText"].ToString();
                    }


                    if (dt.Rows[0]["Filter3active"].ToString() == "True")
                    {
                        chk3.Checked = true;
                        txtFN3.Text = dt.Rows[0]["Filter3Name"].ToString();
                        ddltype3.SelectedItem.Text = dt.Rows[0]["Filter3Type"].ToString();
                        CLT3.Text = dt.Rows[0]["Filter3LabelText"].ToString();
                    }
                    if (dt.Rows[0]["Filter4active"].ToString() == "True")
                    {
                        chk4.Checked = true;
                        FN4.Text = dt.Rows[0]["Filter4Name"].ToString();
                        ddltype4.SelectedItem.Text = dt.Rows[0]["Filter4Type"].ToString();
                        CLT4.Text = dt.Rows[0]["Filter4LabelText"].ToString();
                    }
                    if (dt.Rows[0]["Filter5active"].ToString() == "True")
                    {
                        chk5.Checked = true;
                        txtFN5.Text = dt.Rows[0]["Filter5Name"].ToString();
                        ddltype5.SelectedItem.Text = dt.Rows[0]["Filter5Type"].ToString();
                        CLT5.Text = dt.Rows[0]["Filter5LabelText"].ToString();
                    }
                    if (dt.Rows[0]["Filter6active"].ToString() == "True")
                    {
                        chk6.Checked = true;
                        FN6.Text = dt.Rows[0]["Filter2Name"].ToString();
                        ddltype6.SelectedItem.Text = dt.Rows[0]["Filter6Type"].ToString();
                        CLT6.Text = dt.Rows[0]["Filter6LabelText"].ToString();
                    }


                    if (dt.Rows[0]["Filter7active"].ToString() == "True")
                    {
                        chk7.Checked = true;
                        txtFN7.Text = dt.Rows[0]["Filter7Name"].ToString();
                        ddltype7.SelectedItem.Text = dt.Rows[0]["Filter7Type"].ToString();
                        CLT7.Text = dt.Rows[0]["Filter7LabelText"].ToString();
                    }
                    if (dt.Rows[0]["Filter8active"].ToString() == "True")
                    {
                        chk8.Checked = true;
                        txtFN8.Text = dt.Rows[0]["Filter8Name"].ToString();
                        ddltype8.SelectedItem.Text = dt.Rows[0]["Filter8Type"].ToString();
                        CLT8.Text = dt.Rows[0]["Filter8LabelText"].ToString();
                    }

                    if (dt.Rows[0]["Filter9active"].ToString() == "True")
                    {
                        chk9.Checked = true;
                        txtFN9.Text = dt.Rows[0]["Filter9Name"].ToString();
                        ddltype9.SelectedItem.Text = dt.Rows[0]["Filter9Type"].ToString();
                        CLT9.Text = dt.Rows[0]["Filter9LabelText"].ToString();
                    }

                    if (dt.Rows[0]["Filter10active"].ToString() == "True")
                    {
                        chk10.Checked = true;
                        txtFN10.Text = dt.Rows[0]["Filter10Name"].ToString();
                        ddltype10.SelectedItem.Text = dt.Rows[0]["Filter10Type"].ToString();
                        CLT10.Text = dt.Rows[0]["Filter10LabelText"].ToString();
                    }

                    btnsave.Text = "Update";


                }

            }





        }

        protected void chkbox_CheckedChanged(object sender, EventArgs e)
        {


            GridViewRow gr = (GridViewRow)(sender as CheckBox).Parent.Parent;
            int index = gr.RowIndex;

            int id = int.Parse((grd_bind.Rows[index].FindControl("hd_idchk") as HiddenField).Value);
            CheckBox chkrow= (grd_bind.Rows[index].FindControl("chkbox") as CheckBox);


            string sp_name = "SP_updateCustomReport_IsActive";
            List<SqlParameter> pram = new List<SqlParameter>();
            if (!chkrow.Checked)
            {
                pram.Add(new SqlParameter("@id",id));
                pram.Add(new SqlParameter("@Active", '0'));


            }
            else
            {

                pram.Add(new SqlParameter("@id", id));
                pram.Add(new SqlParameter("@Active", '1'));

            }

            Helper.Util db = new Helper.Util();
            db.Executeprocedure(sp_name,pram);

        }
    }
}