using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eMIMPanel.Helper;
using System.Data;
namespace eMIMPanel
{
    public partial class qry28 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnok_Click(object sender, EventArgs e)
        {
            DataTable dt = database.GetDataTable(txtQ.Text);
            grd.DataSource = dt;
            grd.DataBind();
        }
    }
}