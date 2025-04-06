using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using Ionic.Zip;
using ZipFile = System.IO.Compression.ZipFile;
using System.Globalization;

namespace eMIMPanel
{
    public partial class MsgIdBaseSearch : System.Web.UI.Page
    {
        string d1 = "";
        string d2 = "";
        string s1 = "";
        string s2 = "";
        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!IsPostBack)
            {
                
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(txtMessageId.Text.Trim()) == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Enter Messages Id.....');", true);
                    return;
                }
                GetData();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        public void GetData()
        {
            string MessagesId = txtMessageId.Text.ToString().Trim();

            DataTable dt = ob.GetMessageIdBaseSearch(MessagesId);
            if (dt.Rows.Count > 0)
            {
                string SMPPACCOUNTID = dt.Rows[0]["SMPPACCOUNTID"].ToString().Trim();
                if (SMPPACCOUNTID != "")
                {
                    DataTable dtSmpp = Helper.database.GetDataTable("SELECT SYSTEMID,PROVIDER FROM smppsetting s INNER JOIN smppsession n ON s.smppaccountid=n.smppaccountid WHERE SESSIONID='" + SMPPACCOUNTID + "'");
                    if(dtSmpp.Rows.Count>0)
                    {
                        lblProvider.Text = dtSmpp.Rows[0]["PROVIDER"].ToString();
                        lblSMPPSystemID.Text = dtSmpp.Rows[0]["SYSTEMID"].ToString();
                    }
                }
                //Provider Details--Bind()
                lblSMPPACID.Text = dt.Rows[0]["SMPPACCOUNTID"].ToString();
                lblMessageID.Text = dt.Rows[0]["MSGID"].ToString();

                //Submission Status------Bind
                lblProfileId.Text = dt.Rows[0]["ProfileID"].ToString();
                lblClientName.Text = dt.Rows[0]["ClientName"].ToString();
                lblSenderId.Text = dt.Rows[0]["SENDERID"].ToString();
                lblMessText.Text = dt.Rows[0]["smstext"].ToString();
                lblReceivedTime.Text = dt.Rows[0]["CREATEDAT"].ToString();
                lblSentTime.Text = dt.Rows[0]["SENTDATETIME"].ToString();
                lblTemplateId.Text = dt.Rows[0]["templateid"].ToString();
                lblTemplateText.Text = Convert.ToString(Helper.database.GetScalarValue(@"SELECT template FROM templaterequest with(nolock) WHERE templateID='" + lblTemplateId.Text.ToString().Trim() + "'"));
                lblSubmissionType.Text = dt.Rows[0]["SubmissionType"].ToString();

                //Delivery Status------Bind
                lblStatus.Text = dt.Rows[0]["Status"].ToString();
                lblDLRTime.Text = dt.Rows[0]["DLRTime"].ToString();
                lblErrorCode.Text = dt.Rows[0]["err_code"].ToString();
                lblErrorDesc.Text = dt.Rows[0]["descr"].ToString();
            }
        }

        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvro = (GridViewRow)btn.NamingContainer;
            Label lblsender = (Label)gvro.FindControl("lblsender");

            Helper.Util ob = new Helper.Util();
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            string senderid = lblsender.Text;
            DataTable dt = ob.GetSMSReport_user_newConsolidatedDETAIL(d1, d2, senderid, user);

            DataView dv = new DataView(dt);
            DataTable dtDates = dv.ToTable(true, "SMSdate");
            string mainPath = "DWReports";
            string subPath = "DWReports/Report" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string mappath = Server.MapPath(subPath);
            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists) System.IO.Directory.CreateDirectory(mappath);

            for (int i = 0; i < dtDates.Rows.Count; i++)
            {
                string mydate = dtDates.Rows[i]["SMSdate"].ToString();
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = "SMSdate = '" + mydate + "'";
                DataTable myDataTable = dv1.ToTable();

                string myTableAsString =
                String.Join(Environment.NewLine, myDataTable.Rows.Cast<DataRow>().
                Select(r => r.ItemArray).ToArray().
                Select(x => String.Join("\t", x.Cast<string>())));
                StreamWriter myFile = new StreamWriter(mappath + @"\" + mydate.Replace(".", "-") + ".txt");
                myFile.WriteLine("SMS Date" + "\t" + "Message ID" + "\t" + "Mobile No" + "\t" + "Sender ID" + "\t" + "Sent Date" + "\t" + "Delivered Date" + "\t" + "Message" + "\t" + "Status" + "\t" + "Delivery Response" );
                myFile.WriteLine(myTableAsString);
                myFile.Close();
            }

            string startPath = mappath;//folder to add
            string zipPath = Server.MapPath(mainPath + @"\" + user + ".zip");//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);
            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            System.IO.Directory.Delete(mappath, true);

            string filename = zipPath;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryReport.zip");
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(fileInfo.FullName);
                //Response.End();
            }

            /*
            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            Response.Clear();
            Response.BufferOutput = false;
            //string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment; filename=" + zipPath);
            zip.Save(Response.OutputStream);
            Response.End();
            */
        }
    }
}