using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class sms_reports_u_download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MOBILEDATA"];

            //GridView gv = new GridView();
            //gv.DataSource = dt;
            //gv.DataBind();

            //string attachment = "attachment; filename=" + Convert.ToString(Session["FILENAME"]);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Charset = "";

            //StringWriter oStringWriter = new StringWriter();
            //HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            ////exportToExcel.RenderControl(oHtmlTextWriter);
            //gv.RenderControl(oHtmlTextWriter);
            //Response.Write(oStringWriter.ToString());

            //Response.End();
            //Session["MOBILEDATA"] = null;


           
                try
                {
                    Response.Clear();
                    Response.ClearHeaders(); //use by me
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + Convert.ToString(Session["FILENAME"]).Replace(".xls", ".csv"));
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                //    Response.ContentEncoding = Encoding.UTF8;
                //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                StringBuilder columnbind = new StringBuilder();
                    for (int k = 0; k < ((Session["MOBILEDATA"] as DataTable)).Columns.Count; k++)
                    {

                        columnbind.Append((Session["MOBILEDATA"] as DataTable).Columns[k].ColumnName + ',');
                    }

                    columnbind.Append("\r\n");
                    for (int i = 0; i < (Session["MOBILEDATA"] as DataTable).Rows.Count; i++)
                    {
                        for (int k = 0; k < (Session["MOBILEDATA"] as DataTable).Columns.Count; k++)
                        {

                            columnbind.Append((Session["MOBILEDATA"] as DataTable).Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }

                        columnbind.Append("\r\n");
                    }

               

                Response.Output.Write(columnbind.ToString());
                Response.Flush();
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Session["MOBILEDATA"] = null;
            }
                catch (Exception ex1)
                {
                    string str = ex1.Message;
                }

            

        }
    }
}