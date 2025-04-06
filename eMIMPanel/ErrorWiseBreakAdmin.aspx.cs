using eMIMPanel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class ErrorWiseBreakAdmin : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            Util obj = new Util();
            string DltNo = "";
            if (usertype == "ADMIN")
            {
                DltNo = Convert.ToString(database.GetScalarValue("SELECT DLTNO FROM CUSTOMER WHERE USERNAME = '" + user + "'"));
            }
            DataTable dt = obj.GetErrorCode(usertype, DltNo, user);
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
        }
        protected void GridFormat(DataTable dt)
        {
            grv.UseAccessibleHeader = true;
            grv.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grv.TopPagerRow != null)
            {
                grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grv.BottomPagerRow != null)
            {
                grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
            {
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}
