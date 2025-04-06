using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class click_reports_detail_usr : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetData();
            }
        }

        public void GetData()
        {
            DataTable dt = (DataTable)Session["rptDetail"];

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1' class='table table - striped table - bordered dt - responsive nowrap' width='100 %' cellspacing='0'>");

            //Building the Header row.

            html.Append(@" < thead >
                                        < tr >
                                            < th > Sr.No </ th >
                                            < th > MSG ID </ th >
   
                                               < th > Mobile No </ th >
      
                                                  < th > Sender Id </ th >
         
                                                     < th > Submit Date Time</ th >
            
                                                        < th > Status Date Time</ th >
               
                                                           < th > Message </ th >
               
                                                           < th > Status </ th >
               
                                                           < th > Server Response </ th >
                  
                                                          </ tr >
                  
                                                      </ thead > ");
            //html.Append("<tr>");
            //foreach (DataColumn column in dt.Columns)
            //{
            //    html.Append("<th>");
            //    html.Append(column.ColumnName);
            //    html.Append("</th>");
            //}
            //html.Append("</tr>");
            html.Append("<tbody>");
            //Building the Data rows.
            Int16 i = 1;
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                html.Append("<td>"); html.Append(i.ToString()); html.Append("</td>");

                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
            //grv.DataSource = null;
        }
    }
}