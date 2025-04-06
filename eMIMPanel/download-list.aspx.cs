using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class download_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Helper.Util ob = new Helper.Util();
            string s = Request.QueryString["x"].ToString();
            string[] p = s.Split('$');
            DataTable dt = ob.GetUserReportDetail(p[0], p[1]);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            string attachment = "attachment; filename=ClickList.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            //exportToExcel.RenderControl(oHtmlTextWriter);
            gv.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());

            Response.End();
            
        }
    }
}