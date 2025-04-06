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
    public partial class DeactivateAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (Convert.ToString(Session["UserType"]) != "SYSADMIN")
            {
                Response.Redirect("login.aspx");
            }
            Util obj = new Util();
            DataTable ndt = obj.GetInactiveAccounts();
            grv.DataSource = ndt;
            grv.DataBind();
            GridFormat(ndt);
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

        protected void lblActivate_Click(object sender, EventArgs e)
        {
            Util obj = new Util();
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int indexrow = gvr.RowIndex;
            string userid = Convert.ToString((grv.Rows[indexrow].FindControl("hdnuid") as HiddenField).Value);
            DataTable dt = obj.GetInactiveAccountsByUser(userid);
            DateTime LastActiveDate = Convert.ToDateTime(dt.Rows[0]["LastActiveDate"]);
            DateTime LastUsedDate = Convert.ToDateTime(dt.Rows[0]["LastUsedDate"]);
            DateTime insertdateTime = Convert.ToDateTime(dt.Rows[0]["insertdateTime"]);
            if (dt != null && dt.Rows.Count > 0)
            {
                string Msg = obj.InsertInactiveAccounts(userid, LastActiveDate, LastUsedDate, Convert.ToString(dt.Rows[0]["LastSubmitted"]),
                 Convert.ToString(dt.Rows[0]["Last3MonthAvg"]), insertdateTime, "LOG");
                if (Msg != "")
                {
                    obj.ActivateAccount(userid);//DELETE RECORD
                }
            }
            DataTable ndt = obj.GetInactiveAccounts();
            grv.DataSource = ndt;
            grv.DataBind();
            GridFormat(ndt);
        }
    }
}