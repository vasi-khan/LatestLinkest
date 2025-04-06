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
    public partial class sms_report_u_download2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)Session["MOBILEDATA"];

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                for (int i = 0; i < gv.Rows.Count; i++)
                {

                    gv.Rows[i].Cells[2].Style.Add("mso-number-format", "0");
                   
                }
                Response.Clear();
                Response.ClearHeaders(); 
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + Convert.ToString(Session["FILENAME"]));
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                StringWriter oStringWriter = new StringWriter();
                HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                //exportToExcel.RenderControl(oHtmlTextWriter);
                gv.RenderControl(oHtmlTextWriter);
                string style = @"<style> .text { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Write(oStringWriter.ToString());

                Response.Flush();
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Session["MOBILEDATA"] = null;
                Response.Redirect("sms_reports_usr_DLR_new.aspx");
            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }

        }
    }
}