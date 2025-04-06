using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class DLRreport : System.Web.UI.Page
    {
        string path1 = "";
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["Time"] = DateTime.Now.ToString();

            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
                GetData(user);
            if (Session["UserID"].ToString() == "MIM2201185")
            {
                Response.Redirect("index_u2.aspx");
            }
        }
        //protected void Page_PreRender(object sender, EventArgs e)
        //{
        //    ViewState["Time"] = Session["Time"];
        //}


        protected void rbTdy_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-none");
        }

        protected void rbHis_CheckedChanged(object sender, EventArgs e)
        {
            divOld.Attributes.Add("class", "form-group row d-block");
        }



        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (rbTdy.Checked)
            {

                s1 = Convert.ToDateTime(DateTime.Now, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(DateTime.Now, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";

            }
            if (rbHis.Checked)
            {
                txtFrm.Text = hdntxtFrm.Value;
                txtTo.Text = hdntxtTo.Value;
                if (Convert.ToDateTime(txtFrm.Text, CultureInfo.InvariantCulture) > Convert.ToDateTime(txtTo.Text, CultureInfo.InvariantCulture))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('From Date cannot be set above To Date.');", true);
                    return;
                }

                s1 = Convert.ToDateTime(txtFrm.Text, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(txtTo.Text, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";


                string m1 = s1.Split('-')[1];
                string m2 = s2.Split('-')[1];
                if (m1 != m2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('From Date and To Date should be the same month.');", true);
                    return;
                }
            }

            collapseOne.Attributes.Add("class", "collapse show");
            string user = Convert.ToString(Session["UserID"]);
            string RequestType = string.Empty;
            if (rbSbmtd.Checked)
            {
                RequestType = "Submitted";
            }
            if (rbDlvr.Checked)
            {
                RequestType = "Delivered";
            }
            if (rbFailed.Checked)
            {
                RequestType = "Failed";
            }
            ob.DLRINSER(user, s1, s2,rdbselectlist.SelectedValue, RequestType);

            GetData(user);

        }
        public void GetData(string user)
        {
            if (hdntxtFrm.Value.Trim() == "")
            {
                s1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            else
            {
                s1 = Convert.ToDateTime(hdntxtFrm.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                s2 = Convert.ToDateTime(hdntxtTo.Value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 23:59:59.998";
            }
            DataTable dt = ob.GetReportDLR(user);
            grv.DataSource = null;
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
                grv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        protected void btnlink_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hdnUrl1 = (HiddenField)gvr.FindControl("hdnid");
            string url1 = hdnUrl1.Value;

            string filename = url1;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DLRReport.zip");
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(fileInfo.FullName);
                //Response.End();
            }

        }
    }
}