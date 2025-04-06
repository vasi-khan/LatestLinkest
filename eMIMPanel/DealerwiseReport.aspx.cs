using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace eMIMPanel
{
    public partial class DealerwiseReport : System.Web.UI.Page
    {
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["UserID"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1800;
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                //string year = DateTime.Now.Year.ToString();
                //string month = DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                //string day = DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                //txtFrm.Text = month + "/" + day + "/" + year;
                //txtTo.Text = month + "/" + day + "/" + year;
               // GridBind();
            }
          
        }

        public void GridBind()
        {
            if (hdntxtFrm.Value.Trim()=="")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter From date');", true);
                return;
            }
            if (hdntxtTo.Value.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter To date');", true);
                return;
            }
            string GroupByCode = "";
            if (rdbdealerwise.Checked)
            {
                GroupByCode = "D";
            }
            else if (rdbDealerCampaignwise.Checked)
            {
                GroupByCode = "C";
            }
            else if (rdbdealerDatewise.Checked)
            {
                GroupByCode = "DD";
            }
            DataTable dt =  ob.GetDealerWiseList(user, GroupByCode,  Helper.database.Setdate(hdntxtFrm.Value.Trim()), Helper.database.Setdate(hdntxtTo.Value.Trim()), txtDealerCode.Text.Trim());
            if (dt!=null && dt.Rows.Count>0)
            {
                if (rdbdealerwise.Checked || rdbdealerDatewise.Checked)
                {
                    dt.Columns.Add("CampaignName");
                    dt.Columns["CampaignName"].DefaultValue = "";
                }
                if (rdbdealerDatewise.Checked==false)
                {
                    dt.Columns.Add("SENTDATE");
                    dt.Columns["SENTDATE"].DefaultValue = "";
                }
               
                grvDtl.DataSource = dt;
                grvDtl.DataBind();
                if (rdbdealerwise.Checked || rdbdealerDatewise.Checked)
                {
                    grvDtl.Columns[2].Visible = false;
                }
                else
                {
                    grvDtl.Columns[2].Visible = true;
                }
                 if (rdbdealerDatewise.Checked)
                {
                    grvDtl.Columns[3].Visible = true;
                }
                else
                {
                    grvDtl.Columns[3].Visible = false;
                }
                lnkDownload.Visible = true;
                }
            else
            {
                grvDtl.DataSource = null;
                grvDtl.DataBind();
                lnkDownload.Visible = false;
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            GridBind();
              txtFrm.Text = hdntxtFrm.Value;
            txtTo.Text = hdntxtTo.Value;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "loadscrq();", true);
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
           
                ExportToExcel();
            
        }
        private void ExportToExcel()
        {
            string attachment = "attachment; filename=DealerwiseReport.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grvDtl.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}