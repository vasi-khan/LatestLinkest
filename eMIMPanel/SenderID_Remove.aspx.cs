using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;
using System.Data.SqlClient;
using System.Data;

namespace eMIMPanel
{
    public partial class SenderID_Remove : System.Web.UI.Page
    {
        Helper.QA_Util ob = new Helper.QA_Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string userid = "";

            if (string.IsNullOrEmpty(txtSenderID.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert","alert('Enter UserID !!')",true);
                return;

            }
            BindData();






        }

       public void BindData()
        {
            string sql = @"select id,senderid,countrycode from [dbo].[senderidmast] where userid='" + txtSenderID.Text.Trim() + "'";

            DataTable dt = ob.GetRecord(sql);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    
                    grd.DataSource = dt;
                    grd.DataBind();
                    grd.UseAccessibleHeader = true;
                    grd.HeaderRow.TableSection = TableRowSection.TableHeader;

                    if (grd.TopPagerRow != null)
                    {
                        grd.TopPagerRow.TableSection = TableRowSection.TableHeader;
                    }
                    if (grd.BottomPagerRow != null)
                    {
                        grd.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                    }
                    if (dt.Rows.Count > 0)
                        grd.FooterRow.TableSection = TableRowSection.TableFooter;
                }
                else
                {
                    grd.DataSource = null;
                    grd.DataBind();
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert","alert('SenderID Not Found')",true);
                return;
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void finSub_Click(object sender, EventArgs e)
        {
            if (finSub.Text=="Show")
            {

                if (string.IsNullOrEmpty(txtSenderID.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter UserID !!')", true);
                    return;

                }
                BindData();
                finSub.Text = "Submit";
            }
            else if(finSub.Text == "Submit")
            {
                //int[] a= new int[grd.Rows.Count];
                if (string.IsNullOrEmpty(txtSenderID.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Enter UserID !!')", true);
                    return;

                }
                int i = 0;
                string value = "";
                foreach (GridViewRow gr in grd.Rows)
                {
                    CheckBox chk = gr.Cells[2].FindControl("chk") as CheckBox;
                    if (chk.Checked)
                    {

                        int IndexRow = gr.RowIndex;
                        string ID = (grd.Rows[IndexRow].FindControl("HD_ID") as HiddenField).Value;
                        value = value + ID + ",";

                    }

                }
                value = value.TrimEnd(',');
                if (string.IsNullOrEmpty(value))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select AtLeast One SenderID !!')", true);
                    return;
                }
                 string result=ob.RemoveSenderID(value);
                if (result.Contains("SuccessFully"))
                {
                    BindData();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('SuccessFully Remove SenderID !!')", true);
                }
                
            }
           


        }
    }
}