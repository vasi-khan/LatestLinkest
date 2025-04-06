using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
namespace eMIMPanel
{
    public partial class account_list1 : System.Web.UI.Page
    {
        //Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                GetData();
            }
        }

        protected void grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grv.PageIndex = e.NewPageIndex;
            GetData();
        }

        public void GetData()
        {
            //string date = reportrange.Text;
            //string d_start = (date.Split('-'))[0];
            //string d_end = (date.Split('-'))[1];
            //string start = d_start + " 00:00:00.000";
            //string end = d_end + " 23:59:59.999";

            DataTable dt = new DataTable();
            dt.Columns.Add("Compname");
            dt.Columns.Add("sender");
            dt.Columns.Add("mobile");
            dt.Columns.Add("email");
            dt.Columns.Add("bal");
            dt.Columns.Add("createdby");

            DataRow dr = dt.NewRow();
            dr["Compname"] = "SURENDRA ENTERPRISES";
            dr["sender"] = "SURENDRA";
            dr["mobile"] = "919988998899";
            dr["email"] = "surendra@gmail.com";
            dr["bal"] = "93784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "MAHENDRA ENTERPRISES";
            dr["sender"] = "MAHENDRA";
            dr["mobile"] = "919988998888";
            dr["email"] = "mahendra@gmail.com";
            dr["bal"] = "93799";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "PRAKASH ENTERPRISES";
            dr["sender"] = "PRAKASH";
            dr["mobile"] = "919988998891";
            dr["email"] = "prakash@gmail.com";
            dr["bal"] = "93781";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "MANOHAR AND SONS ENTERPRISES";
            dr["sender"] = "MANOHAR";
            dr["mobile"] = "919999999999";
            dr["email"] = "manohar@gmail.com";
            dr["bal"] = "93724";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "RAJENDRAPRAKASH AND SATYAPRAKASH SOFTWARE SOLUTIONS PRIVATE LIMITED";
            dr["sender"] = "RAJPRAK";
            dr["mobile"] = "9197777777777";
            dr["email"] = "rajendraprakashandsatyaprakash@gmail.com";
            dr["bal"] = "95784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "ABHISHEK ENTERPRISES";
            dr["sender"] = "ABHISHEK";
            dr["mobile"] = "915588998899";
            dr["email"] = "abhishek@gmail.com";
            dr["bal"] = "53784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "SHYAMLAL AND SONS ENTERPRISES";
            dr["sender"] = "SHYAMLAL";
            dr["mobile"] = "919988998899";
            dr["email"] = "shyamlal@gmail.com";
            dr["bal"] = "93784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "MAHANAGAR ENTERPRISES";
            dr["sender"] = "MAHANAGA";
            dr["mobile"] = "917888998899";
            dr["email"] = "mahanagar@gmail.com";
            dr["bal"] = "93784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "TRIVEDI ENTERPRISES";
            dr["sender"] = "TRIVEDI";
            dr["mobile"] = "919900998899";
            dr["email"] = "trivedi@gmail.com";
            dr["bal"] = "93784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "ARUNLAL AND ASSOCIATES";
            dr["sender"] = "ARUNLAL";
            dr["mobile"] = "919988448899";
            dr["email"] = "arunlal@gmail.com";
            dr["bal"] = "96784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "RAJENDRAPRAKASH AND SATYAPRAKASH SOFTWARE SOLUTIONS PRIVATE LIMITED";
            dr["sender"] = "RAJPRAK";
            dr["mobile"] = "9197777777777";
            dr["email"] = "rajendraprakashandsatyaprakash@gmail.com";
            dr["bal"] = "95784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "ABHISHEK ENTERPRISES";
            dr["sender"] = "ABHISHEK";
            dr["mobile"] = "915588998899";
            dr["email"] = "abhishek@gmail.com";
            dr["bal"] = "53784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "RAJENDRAPRAKASH AND SOFTWARE SOLUTIONS PRIVATE LIMITED";
            dr["sender"] = "RAJPRAKS";
            dr["mobile"] = "9197777777799";
            dr["email"] = "rajendraprakashsatyaprakash@gmail.com";
            dr["bal"] = "95784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Compname"] = "ABHISHEKRAJ ENTERPRISES";
            dr["sender"] = "ABHISHE";
            dr["mobile"] = "915588993399";
            dr["email"] = "abhishekRAJ@gmail.com";
            dr["bal"] = "53784";
            dr["createdby"] = "31-Aug-2020";
            dt.Rows.Add(dr);

            grv.DataSource = dt;
            grv.DataBind();

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

            grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string s1 = h1.Value;
            string s2 = h2.Value;
            string s = "";
        }

        
    }
}