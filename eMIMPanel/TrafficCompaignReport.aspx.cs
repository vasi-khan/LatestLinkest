using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace eMIMPanel
{
    public partial class TrafficCompaignReport : System.Web.UI.Page
    {
        List<SqlParameter> pram = new List<SqlParameter>();
        Helper.Util obj = new Helper.Util();
        string _user = "";
        string usertype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //_user = Convert.ToString(Session["UserID"]);
            //if (_user == "") Response.Redirect("Login.aspx");
            usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["User"]);
            _user = Convert.ToString(Session["User"]);
            if (user == "")
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                bindCUstomerGridView();
            }
        }

        private void bindCUstomerGridView()
        {
            pram.Clear();
            pram.Add(new SqlParameter("@profileID", txtprofileID.Text.Trim()));
            pram.Add(new SqlParameter("@fullname", txtfullname.Text.Trim()));
            pram.Add(new SqlParameter("@compname", txtcompname.Text.Trim()));
            DataTable dt = obj.GetDataTableProc("SP_GetDetailCustomer", pram);
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    GrdViewCustInfo.DataSource = dt;
                    GrdViewCustInfo.DataBind();
                    GridFormat(dt);
                }
                else
                {
                    GrdViewCustInfo.DataSource = null;
                    GrdViewCustInfo.DataBind();
                }

            }



        }

        protected void Filtergo_Click(object sender, EventArgs e)
        {
            bindCUstomerGridView();
            ModalPopupex.Show();

        }

        protected void btn_FindViewrecord_Click(object sender, EventArgs e)
        {
            try { 
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int rowindex = gvr.RowIndex;
                Label profileID = GrdViewCustInfo.Rows[rowindex].FindControl("grdlblUsername") as Label;
                if (profileID.Text.Trim()!="")
                {
                    txtcust.Text = profileID.Text.Trim();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;

            }
            ModalPopupex.Hide();
        }

       
        protected void finalSubmit_Click(object sender, EventArgs e)
        {

            if (txtmonth.Text.Trim()=="")
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert()","alert('Enter Month **')",true);
                return;

            }
            if (txtcust.Text.Trim()!="")
            {
                DataTable dt2 = Helper.database.GetDataTable("select COUNT(*) c from CUSTOMER where username='" + txtcust.Text.Trim() + "'");
                if (dt2!=null)
                {
                    int count = Convert.ToInt16(dt2.Rows[0]["c"].ToString());
                    if (count==0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert()", "alert('Invalid Customer ID')", true);
                        return;

                    }

                }

            }
            
            pram.Clear();
            pram.Add(new SqlParameter("@username", txtcust.Text.Trim()=="" ?null : txtcust.Text.Trim()));
            pram.Add(new SqlParameter("@month2", txtmonth.Text.Trim()));            
            DataTable dt = obj.GetDataTableProc("SP_GettrafficReport", pram);
           if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    //btnXL.Visible = true;
                    ViewState["btnXLS"] = dt;
                    grd.DataSource = dt;
                    grd.DataBind();
                    grd.Visible = true;
                    GridFormat2(dt);

                }
                else
                {
                    //btnXL.Visible = false;
                    grd.DataSource = null;
                    grd.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert()","alert('Data Not Found')",true);
                    return;
                }


            }
            
            


        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            ModalPopupex.Show();
        }

        protected void btbclose_Click(object sender, EventArgs e)
        {
            ModalPopupex.Hide();
        }      
        public void DownloadXLS(DataTable dt)
        {
                Session["MOBILEDATA"] = dt;

                if (dt.Rows.Count > 0)
                {
                    Session["FILENAME"] = "TrafficCompaignReport.xls";
                    Response.Redirect("sms-reports_u_download.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);
                }
            
        }       
        protected void btnXL_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["btnXLS"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {                
                    DownloadXLS(dt); 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data to show.');", true);

            }
            
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrafficCompaignReport.aspx");
        }
        protected void GridFormat2(DataTable dt)
        {
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
        protected void GridFormat(DataTable dt)
        {
            GrdViewCustInfo.UseAccessibleHeader = true;
            GrdViewCustInfo.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (GrdViewCustInfo.TopPagerRow != null)
            {
                GrdViewCustInfo.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (GrdViewCustInfo.BottomPagerRow != null)
            {
                GrdViewCustInfo.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                GrdViewCustInfo.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
}