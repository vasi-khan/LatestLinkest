using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class DownloadZipFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

          string filename=  Request.QueryString["filename"].ToString();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
            try
            {


                if (fileInfo.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryReport.zip");
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.Flush();
                    Response.TransmitFile(fileInfo.FullName);
                    Response.End();
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }


            if (File.Exists(filename))
                File.Delete(filename);

            Response.Redirect("CustomReportDownload.aspx");

        }
    }
}