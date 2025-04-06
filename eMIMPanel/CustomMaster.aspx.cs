using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class CustomMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtnooffield_TextChanged(object sender, EventArgs e)
        {

            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
            if (txtnooffield.Text.Trim()=="1")
            {
                div1.Visible = true;

            }
            else if(txtnooffield.Text.Trim() == "2")
            {

                div1.Visible = true;
                div2.Visible = true;
            }
            else if (txtnooffield.Text.Trim() == "3")
            {
                div1.Visible = true;
                div2.Visible = true;
                div3.Visible = true;
            }
            else if (txtnooffield.Text.Trim() == "4")
            {
                div1.Visible = true;
                div2.Visible = true;
                div3.Visible = true;
                div4.Visible = true;

            }
            else if (txtnooffield.Text.Trim() == "5")
            {
                div1.Visible = true;
                div2.Visible = true;
                div3.Visible = true;
                div4.Visible = true;
                div5.Visible=true;
                
            }
            else if (txtnooffield.Text.Trim() == "6")
            {
                div1.Visible = true;
                div2.Visible = true;
                div3.Visible = true;
                div4.Visible = true;
                div5.Visible = true;
                div6.Visible = true;
            }

        }

        protected void ddlcontroltype1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef1.SelectedValue=="2")
            {
                divradiobutton1.Visible = false;
                ddlshow1.Visible = true;
            }
            else if(ddlcontroltypef1.SelectedValue == "4")
            {
                ddlshow1.Visible = false;
                divradiobutton1.Visible = true;

            }
        }

        protected void ddlcontroltypef2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef2.SelectedValue == "2")
            {
                divradiobutton2.Visible = false;
                ddlshow2.Visible = true;
            }
            else if (ddlcontroltypef2.SelectedValue == "4")
            {
                ddlshow2.Visible = false;
                divradiobutton2.Visible = true;

            }

        }

        protected void ddlcontroltypef3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef3.SelectedValue == "2")
            {
                divradiobutton3.Visible = false;
                ddlshow3.Visible = true;
            }
            else if (ddlcontroltypef3.SelectedValue == "4")
            {
                ddlshow3.Visible = false;
                divradiobutton3.Visible = true;

            }

        }

        protected void ddlcontroltypef4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef4.SelectedValue == "2")
            {
                divradiobutton4.Visible = false;
                ddlshow4.Visible = true;
            }
            else if (ddlcontroltypef4.SelectedValue == "4")
            {
                ddlshow4.Visible = false;
                divradiobutton4.Visible = true;

            }

        }

        protected void ddlcontroltypef5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef5.SelectedValue == "2")
            {
                divradiobutton5.Visible = false;
                ddlshow5.Visible = true;
            }
            else if (ddlcontroltypef5.SelectedValue == "4")
            {
                ddlshow5.Visible = false;
                divradiobutton5.Visible = true;

            }

        }

        protected void ddlcontroltypef6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcontroltypef6.SelectedValue == "2")
            {
                divradiobutton6.Visible = false;
                ddlshow6.Visible = true;
            }
            else if (ddlcontroltypef6.SelectedValue == "4")
            {
                ddlshow6.Visible = false;
                divradiobutton6.Visible = true;

            }

        }
        //filed 1 start
        protected void grdf1_rblb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F1rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F1rdlb"] = dt;
                    grdf1_rdlb();

                }
            }
            catch (Exception ex)
            {

            }

        }

       
        protected void btnf1_rdla_Click(object sender, EventArgs e)
        {
            
            if (ViewState["dtf1_rdla"]==null)
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2]
                {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                    

                }
                );
                dt.Rows.Add(conditonf1_varName_rbla.Text.Trim(), conditionf1_fieldname_rbla.Text.Trim());
                dt.AcceptChanges();
                ViewState["dtf1_rdla"] = dt;
                function_grdf1_rdla();

            }
            else
            {
                DataTable dt2 = ViewState["dtf1_rdla"] as DataTable;
                dt2.Rows.Add(conditonf1_varName_rbla.Text.Trim(), conditionf1_fieldname_rbla.Text.Trim());
                dt2.AcceptChanges();
                ViewState["dtf1_rdla"] = dt2;

                function_grdf1_rdla();

            }
          



        }
        public  void function_grdf1_rdla()
        {
            DataTable dt = ViewState["dtf1_rdla"] as DataTable;
            if (dt!=null)
            {
                if (dt.Rows.Count>0|| dt.Rows!=null)
                {
                    grdf1_rbla.DataSource = null;
                    grdf1_rbla.DataSource = dt;
                    grdf1_rbla.DataBind();
                }
                else
                {
                    grdf1_rbla.DataSource = null;
                    grdf1_rbla.DataBind();
                }

            }



        }
        public void grdf1_rdlb() {
            DataTable dt = ViewState["F1rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdf1_rblb.DataSource = null;
                    grdf1_rblb.DataSource = dt;
                    grdf1_rblb.DataBind();
                }
                else
                {
                    grdf1_rblb.DataSource = null;
                    grdf1_rblb.DataBind();
                }

            }
        }

        protected void grdf1_rbla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["dtf1_rdla"] as DataTable;

                    dt.Rows[s_no-1].Delete();
                    dt.AcceptChanges();
                    ViewState["dtf1_rdla"] = dt;
                    function_grdf1_rdla();

                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void btnf1_rblb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F1rdlb"]==null)
            {
                dt.Columns.AddRange( new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionf1_varName_rdlb.Text.Trim(), txtconditionf1_fieldname_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F1rdlb"] = dt;
                grdf1_rdlb();

            }
            else
            {
                DataTable dt2 = ViewState["F1rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionf1_varName_rdlb.Text.Trim(), txtconditionf1_fieldname_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F1rdlb"] = dt2;
                grdf1_rdlb();


            }
        }
        //filed 1 end
        //filed 2start
        protected void btnF2_rdla_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F2rdla"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionf1_varName_rdlb.Text.Trim(), txtconditionf1_fieldname_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F2rdla"] = dt;
                fun_grdF2_rdla();

            }
            else
            {
                DataTable dt2 = ViewState["F2rdla"] as DataTable;
                dt2.Rows.Add(txtconditionf1_varName_rdlb.Text.Trim(), txtconditionf1_fieldname_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F1rdlb"] = dt2;
                fun_grdF2_rdla();


            }

            
        }
        public void fun_grdF2_rdla()
        {
            DataTable dt = ViewState["F2rdla"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF2_rdla.DataSource = null;
                    grdF2_rdla.DataSource = dt;
                    grdF2_rdla.DataBind();
                }
                else
                {
                    grdF2_rdla.DataSource = null;
                    grdF2_rdla.DataBind();
                }

            }
        }

        protected void btnsubmitF2_rdlb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F2rdlb"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionvalnameF2_rdlb.Text.Trim(), txtconditionvalfieldnoF2_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F2rdlb"] = dt;
                fun_grdF2_rdlb();

            }
            else
            {
                DataTable dt2 = ViewState["F2rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionvalnameF2_rdlb.Text.Trim(), txtconditionvalfieldnoF2_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F1rdlb"] = dt2;
                fun_grdF2_rdlb();


            }

        }

        public void fun_grdF2_rdlb()
        {
            DataTable dt = ViewState["F2rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF2_rdlb.DataSource = null;
                    grdF2_rdlb.DataSource = dt;
                    grdF2_rdlb.DataBind();
                }
                else
                {
                    grdF2_rdlb.DataSource = null;
                    grdF2_rdlb.DataBind();
                }

            }
        }
        protected void grdF2_rdla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F2rdla"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F2rdla"] = dt;
                    fun_grdF2_rdla();

                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void grdF2_rdlb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F2rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F2rdlb"] = dt;
                    fun_grdF2_rdlb();

                }
            }
            catch (Exception ex)
            {

            }

        }
        //filed 2end
        //filed 3start
        protected void btnF3_rdla_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F3rdla"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionvarNameF3_rdla.Text.Trim(), txtconditionvarFieldnameF3_rdla.Text.Trim());

                dt.AcceptChanges();
                ViewState["F3rdla"] = dt;
                grdF3_rdla_Bind();

            }
            else
            {
                DataTable dt2 = ViewState["F3rdla"] as DataTable;
                dt2.Rows.Add(txtconditionvarNameF3_rdla.Text.Trim(), txtconditionvarFieldnameF3_rdla.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F3rdla"] = dt2;
                grdF3_rdla_Bind();


            }


        }

        protected void grdF3_rdla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F3rdla"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F3rdla"] = dt;
                    grdF3_rdla_Bind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void grdF3_rdla_Bind()
        {
            DataTable dt = ViewState["F3rdla"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF3_rdla.DataSource = null;
                    grdF3_rdla.DataSource = dt;
                    grdF3_rdla.DataBind();
                }
                else
                {
                    grdF3_rdla.DataSource = null;
                    grdF3_rdla.DataBind();
                }

            }
        }

        protected void btnsubmitF3_rdlb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F3rdlb"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionVarNameF3_rdlb.Text.Trim(), txtconditionVarFieldNoF3_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F3rdlb"] = dt;
                Fun_grdF3_rdlb();

            }
            else
            {
                DataTable dt2 = ViewState["F3rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionVarNameF3_rdlb.Text.Trim(), txtconditionVarFieldNoF3_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F3rdlb"] = dt2;
                Fun_grdF3_rdlb();


            }

        }

        protected void grdF3_rdlb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F3rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F3rdlb"] = dt;
                    Fun_grdF3_rdlb();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Fun_grdF3_rdlb()
        {
            DataTable dt = ViewState["F3rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF3_rdlb.DataSource = null;
                    grdF3_rdlb.DataSource = dt;
                    grdF3_rdlb.DataBind();
                }
                else
                {
                    grdF3_rdlb.DataSource = null;
                    grdF3_rdlb.DataBind();
                }

            }

        }

        //filed 3 end


        //Field 4 start

        protected void btnsubmitF4_rdla_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F4rdla"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtConditionVarNameF4_rdla.Text.Trim(), txtConditionVarFieldnoF4_rdla.Text.Trim());

                dt.AcceptChanges();
                ViewState["F4rdla"] = dt;
                Fun_grdF4_rdla();

            }
            else
            {
                DataTable dt2 = ViewState["F4rdla"] as DataTable;
                dt2.Rows.Add(txtConditionVarNameF4_rdla.Text.Trim(), txtConditionVarFieldnoF4_rdla.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F4rdla"] = dt2;
                Fun_grdF4_rdla();


            }

        }

        protected void grdF4_rdla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F4rdla"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F4rdla"] = dt;
                    Fun_grdF4_rdla();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Fun_grdF4_rdla()
        {
            DataTable dt = ViewState["F4rdla"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF4_rdla.DataSource = null;
                    grdF4_rdla.DataSource = dt;
                    grdF4_rdla.DataBind();
                }
                else
                {
                    grdF4_rdla.DataSource = null;
                    grdF4_rdla.DataBind();
                }

            }
        }

        protected void btnSubmitF4_rdlb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F4rdlb"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionVarNameF4_rdlb.Text.Trim(), txtconditionvanFieldNoF4_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F4rdlb"] = dt;
                fun_grdF4_rdlb(); 
            }
            else
            {
                DataTable dt2 = ViewState["F4rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionVarNameF4_rdlb.Text.Trim(), txtconditionvanFieldNoF4_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F4rdlb"] = dt2;
                fun_grdF4_rdlb(); 


            }
            
        }
        

        protected void grdF4_rdlb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F4rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F4rdlb"] = dt;
                    fun_grdF4_rdlb();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void fun_grdF4_rdlb()
        {
            DataTable dt = ViewState["F4rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF4_rdlb.DataSource = null;
                    grdF4_rdlb.DataSource = dt;
                    grdF4_rdlb.DataBind();
                }
                else
                {
                    grdF4_rdlb.DataSource = null;
                    grdF4_rdlb.DataBind();
                }

            }

        }
        //Field 4 End

        //Field 5 start
        protected void btnSubmitF5_rdla_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F5rdla"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionVarNameF4_rdlb.Text.Trim(), txtconditionvanFieldNoF4_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F5rdla"] = dt;
                fun_grdF5_rdlaBind();
            }
            else
            {
                DataTable dt2 = ViewState["F5rdla"] as DataTable;
                dt2.Rows.Add(txtconditionVarNameF4_rdlb.Text.Trim(), txtconditionvanFieldNoF4_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F5rdla"] = dt2;
                fun_grdF5_rdlaBind();


            }


        }

        protected void grdF5_rdla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F5rdla"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F5rdla"] = dt;
                    fun_grdF5_rdlaBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void fun_grdF5_rdlaBind()
        {
            DataTable dt = ViewState["F5rdla"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF5_rdla.DataSource = null;
                    grdF5_rdla.DataSource = dt;
                    grdF5_rdla.DataBind();
                }
                else
                {
                    grdF5_rdla.DataSource = null;
                    grdF5_rdla.DataBind();
                }

            }

        }

        protected void btnSubmitF5_rdlb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F5rdlb"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionVarNameF5_rdlb.Text.Trim(), txtconditionvarFieldNoF5_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F5rdlb"] = dt;
                Fun_grdF5_rdlb();
            }
            else
            {
                DataTable dt2 = ViewState["F5rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionVarNameF5_rdlb.Text.Trim(), txtconditionvarFieldNoF5_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F5rdlb"] = dt2;
                Fun_grdF5_rdlb();


            }

        }
        protected void grfF5_rdlb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F5rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F5rdlb"] = dt;
                    Fun_grdF5_rdlb();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public void Fun_grdF5_rdlb()
        {
            DataTable dt = ViewState["F5rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grfF5_rdlb.DataSource = null;
                    grfF5_rdlb.DataSource = dt;
                    grfF5_rdlb.DataBind();
                }
                else
                {
                    grfF5_rdlb.DataSource = null;
                    grfF5_rdlb.DataBind();
                }

            }

            
        }
        //Field 5 End

        // field 6 start
        protected void grdF6_rdla_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F6rdla"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F6rdla"] = dt;
                    Fun_grdF6_rdla();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnsubmitF6_rdla_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F6rdla"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtcomndtitionvarNameF6_rdla.Text.Trim(), txtcomndtitionvarFieldNoF6_rdla.Text.Trim());

                dt.AcceptChanges();
                ViewState["F6rdla"] = dt;
                Fun_grdF6_rdla();
            }
            else
            {
                DataTable dt2 = ViewState["F6rdla"] as DataTable;
                dt2.Rows.Add(txtcomndtitionvarNameF6_rdla.Text.Trim(), txtcomndtitionvarFieldNoF6_rdla.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F6rdla"] = dt2;
                Fun_grdF6_rdla();


            }

        }
        public void Fun_grdF6_rdla()
        {
            DataTable dt = ViewState["F6rdla"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF6_rdla.DataSource = null;
                    grdF6_rdla.DataSource = dt;
                    grdF6_rdla.DataBind();
                }
                else
                {
                    grdF6_rdla.DataSource = null;
                    grdF6_rdla.DataBind();
                }

            }



        }




        protected void btnsubmitF6_rdlb_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["F6rdlb"] == null)
            {
                dt.Columns.AddRange(new DataColumn[2] {

                    new DataColumn("VariableName"),
                    new DataColumn("VariableFieldNo")
                });
                dt.Rows.Add(txtconditionvarNameF6_rdlb.Text.Trim(), txtconditionvarFieldNoF6_rdlb.Text.Trim());

                dt.AcceptChanges();
                ViewState["F6rdlb"] = dt;
                Fun_grdF6_rdlb();
            }
            else
            {
                DataTable dt2 = ViewState["F6rdlb"] as DataTable;
                dt2.Rows.Add(txtconditionvarNameF6_rdlb.Text.Trim(), txtconditionvarFieldNoF6_rdlb.Text.Trim());

                dt2.AcceptChanges();
                ViewState["F6rdlb"] = dt2;
                Fun_grdF6_rdlb();


            }

        }

        protected void grdF6_rdlb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int s_no = int.Parse(e.CommandArgument.ToString());

                    DataTable dt = ViewState["F6rdlb"] as DataTable;

                    dt.Rows[s_no - 1].Delete();
                    dt.AcceptChanges();
                    ViewState["F6rdlb"] = dt;
                    Fun_grdF6_rdlb();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Fun_grdF6_rdlb()
        {
            DataTable dt = ViewState["F6rdlb"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0 || dt.Rows != null)
                {
                    grdF6_rdlb.DataSource = null;
                    grdF6_rdlb.DataSource = dt;
                    grdF6_rdlb.DataBind();
                }
                else
                {
                    grdF6_rdlb.DataSource = null;
                    grdF6_rdlb.DataBind();
                }

            }
        }
        // Field 6 End

        protected void btnsave_Click(object sender, EventArgs e)
        {
           

        }
        
    }
}