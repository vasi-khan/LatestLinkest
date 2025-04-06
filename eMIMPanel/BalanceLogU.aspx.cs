using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class BalanceLogU : System.Web.UI.Page
    {
        string s1 = "";
        string s2 = "";         
        string user = "";        

        protected void Page_Load(object sender, EventArgs e)
        {             
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            GetData();
        }
        public void GetData()
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.999";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }

            if(Convert.ToDateTime(s1) < Convert.ToDateTime("2021-11-11"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found before 11-Nov-2021.From date should be on or after 11-Nov-2021 !!')", true);
                return;
            }

            Helper.Util ob = new Helper.Util();
            user = Convert.ToString(Session["UserID"]);
            DataTable dt = ob.GetBalananceLogs(user,s1, s2);
            grv.DataSource = null;
            grv.DataSource = dt;
            //SetFooterValue(dt);
            grv.DataBind();

            GridFormat(dt);
            //Session["analyticsdata"] = dt;
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
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        private void SetFooterValue(DataTable copyDataTable)
        {
            object sumSubmitted;
            sumSubmitted = copyDataTable.Compute("Sum(amount)", string.Empty);
            object sumDelivered;
            sumDelivered = copyDataTable.Compute("Sum(Expenditure)", string.Empty);
            object sumFailed;
            sumFailed = copyDataTable.Compute("Sum(NetAmount)", string.Empty); 

            grv.Columns[1].FooterText = "Total : ";
            grv.Columns[2].FooterText = sumSubmitted.ToString();
            grv.Columns[3].FooterText = sumDelivered.ToString();
            grv.Columns[4].FooterText = sumFailed.ToString();
            

        }

    }
}