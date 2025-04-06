using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using ClosedXML.Excel;

namespace eMIMPanel
{
    public partial class AccountList : System.Web.UI.Page
    {
        Helper.common.mlogin mobj;
        Helper.Util obj = new Helper.Util();
        public string proc = "SP_RequestformProcedure";

        List<SqlParameter> pram = new List<SqlParameter>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("login.aspx");

            }
            if (!IsPostBack)
            {

                if (rbl1.Checked)
                {
                    //BindGridView();
                    pram.Clear();
                    pram.Add(new SqlParameter("@action", "GetRecordCustomerWise"));
                    pram.Add(new SqlParameter("@empcode", Session["EMPCODE"].ToString()));
                    DataTable dt = obj.GetRecord(proc, pram);
                    dt.Columns.Add("LASTRECHARGEAMOUNT");
                    dt.Columns.Add("LASTRECHARGEDATE");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            div2.Visible = true;
                            lblheading.Text = "Total Accounts :";
                            lbltotact.Text = dt.Rows.Count.ToString();
                            grdview.DataSource = dt;
                            grdview.DataBind();
                            GridFormat(dt);
                            Session["data"] = dt;

                        }
                        else
                        {
                            grdview.DataSource = null;
                            grdview.DataBind();
                            Session["data"] = null;

                        }
                    }
                    Session["data"] = dt;

                }



                //Session["empcode"] = (Session["USER"] as Helper.common.mlogin).usernmae;
                Session["empcode"] = Convert.ToString(Session["EMPCODE"]);

                //fill text box 

                txtrbl2.Text = "7";
                txtrbl3.Text = "1000";
            }
           
            

        }
        public void BindGridView(DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    grdview.DataSource = dt;
                    Session["data"] = dt;
                    grdview.DataBind();
                    GridFormat(dt);


                }
                else
                {
                    grdview.DataSource = null;
                    grdview.DataBind();
                    Session["data"] = null;

                }

            }

        }



        //protected void GridFormat(DataTable dt)
        //{
        //    grdview.UseAccessibleHeader = true;

        //    if (grdview.TopPagerRow != null)
        //    {
        //        grdview.HeaderRow.TableSection = TableRowSection.TableHeader;
        //    }

        //    if (grdview.TopPagerRow != null)
        //    {
        //        grdview.TopPagerRow.TableSection = TableRowSection.TableHeader;
        //    }
        //    if (grdview.BottomPagerRow != null)
        //    {
        //        grdview.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        //    }
        //    if (dt.Rows.Count > 0)
        //        grdview.FooterRow.TableSection = TableRowSection.TableFooter;
        //}


        protected void GridFormat(DataTable dt)
        {
            grdview.UseAccessibleHeader = true;
            grdview.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grdview.TopPagerRow != null)
            {
                grdview.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grdview.BottomPagerRow != null)
            {
                grdview.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grdview.FooterRow.TableSection = TableRowSection.TableFooter;
        }



        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (rbl1.Checked)
            {
                //BindGridView();
                pram.Clear();
                pram.Add(new SqlParameter("@action", "GetRecordCustomerWise"));
                pram.Add(new SqlParameter("@empcode", Session["EMPCODE"].ToString()));
                DataTable dt = obj.GetRecord(proc,pram);
                dt.Columns.Add("LASTRECHARGEAMOUNT");
                dt.Columns.Add("LASTRECHARGEDATE");
                if (dt!= null)
                {
                    if (dt.Rows.Count>0)
                    {
                        div2.Visible = true;
                        lblheading.Text = "Total Accounts :";
                        lbltotact.Text = dt.Rows.Count.ToString();
                        grdview.DataSource = dt;
                        grdview.DataBind();
                        GridFormat(dt);
                        Session["data"] = dt;

                    }
                    else
                    {
                        grdview.DataSource = null;
                        grdview.DataBind();
                        Session["data"] = null;

                    }
                }
                Session["data"] = dt;

            }
            else if (rbl2.Checked)
            {
                pram.Clear();
                pram.Add(new SqlParameter("@action", "GetiactiveAccountdetail"));
                pram.Add(new SqlParameter("@empcode", Session["EMPCODE"].ToString()));
                pram.Add(new SqlParameter("@noofdays",txtrbl2.Text.Trim()));

                DataTable dt = obj.GetRecord(proc, pram);
                dt.Columns.Add("LASTRECHARGEAMOUNT");
                dt.Columns.Add("LASTRECHARGEDATE");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        div2.Visible = true;
                        lblheading.Text = "Inactive Accounts :";
                        lbltotact.Text = dt.Rows.Count.ToString(); ;
                        grdview.DataSource = dt;
                        grdview.DataBind();
                        GridFormat(dt);
                        Session["data"] = dt;

                    }
                    else
                    {
                        grdview.DataSource = null;
                        grdview.DataBind();
                        Session["data"] = null;

                    }
                }

                Session["Dlt"] =dt;
                

            }
            else if (rbl3.Checked)
            {
                pram.Clear();
                pram.Add(new SqlParameter("@action", "GetLowBalanceAccountDetail"));
                pram.Add(new SqlParameter("@empcode", Session["EMPCODE"].ToString()));
                pram.Add(new SqlParameter("@balace",txtrbl3.Text.Trim()));
                DataTable dt = obj.GetRecord(proc, pram);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        grdview.Columns[8].Visible = true;
                        grdview.Columns[9].Visible = true;
                        div2.Visible = true;
                        lblheading.Text = "Low Balance Accounts :";
                        lbltotact.Text = dt.Rows.Count.ToString(); 
                        grdview.DataSource = dt;
                        grdview.DataBind();
                        GridFormat(dt);
                        Session["data"] = dt;

                    }
                    else
                    {
                        grdview.DataSource = null;
                        grdview.DataBind();
                        Session["data"] = null;

                    }
                }






            }

        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            if (grdview.Rows.Count<=0)
            {
                ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"alert","alert('Data Not Found');",true);
                return;

            }
            else
            {
                //DataTable dt = Session["GridData"] as DataTable;
                Session["FILENAME"] = "AccountList.xls";
                Session["MOBILEDATA"] = Session["data"];
                Response.Redirect("sms-reports_u_download.aspx");
                //ExportToExcel2();

            }
            
        }

        private void ExportToExcel2()
        {
            try
            {
                string attachment = "attachment; filename=AccountList.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.ContentType.ToString();
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    grdview.HeaderRow.BackColor = Color.White;
                    for (int i = 0; i < grdview.HeaderRow.Cells.Count; i++)
                    {
                        grdview.HeaderRow.Cells[i].Style.Add("background-color", "#eab012");
                        grdview.HeaderRow.Cells[i].Style.Add(" ForeColor", "#ffffff");

                    }
                    for (int i = 0; i < grdview.Rows.Count; i++)
                    {

                        grdview.Rows[i].Cells[4].Style.Add("mso-number-format", "0");
                        grdview.Rows[i].Cells[6].Style.Add("mso-number-format", "0");

                    }

                    grdview.RenderControl(hw);

                    Response.ContentType = "application/text";

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    Response.Clear();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            

            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt);

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=AccountList.xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }


    }
}