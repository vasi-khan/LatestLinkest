using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace eMIMPanel
{
    public partial class sms_reports_download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            Helper.Util ob = new Helper.Util();
            string s = "";
            if (Request.QueryString["x"] != null)
            {
                s = Request.QueryString["x"].ToString();
            }
            else
            {
                s = Session["concat"].ToString();
            }
            //string s = Request.QueryString["x"].ToString();
            //string s = Session["concat"].ToString();
            string[] p = s.Split('$');
            if (Convert.ToString(Session["Today"]) == "True")
            {
                Session["Today"] = "";
                DataTable dt = ob.GetUserSMSReportDetailtoday(p[0], p[1], p[2], p[3], p[6]);
                try
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=SMSReport.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    //    Response.ContentEncoding = Encoding.UTF8;
                    //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    StringBuilder columnbind = new StringBuilder();
                    for (int k = 0; k < (dt).Columns.Count; k++)
                    {

                        columnbind.Append(dt.Columns[k].ColumnName + ',');
                    }

                    columnbind.Append("\r\n");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {

                            columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }

                        columnbind.Append("\r\n");
                    }



                    Response.Output.Write(columnbind.ToString());
                    Response.Flush();
                    Response.End();
                    Session["MOBILEDATA"] = null;
                }

                catch (Exception ex1)
                {
                    string str = ex1.Message;
                }
                
            }
            else if (Convert.ToString(p[4]) == "sms-reports")
            {
                DataTable dt = ob.GetUserSMSReportDetail(p[0], p[1], p[2], p[3]);
                try
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=SMSReport.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    StringBuilder columnbind = new StringBuilder();
                    for (int k = 0; k < (dt).Columns.Count; k++)
                    {
                        columnbind.Append(dt.Columns[k].ColumnName + ',');
                    }

                    columnbind.Append("\r\n");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)

                        {
                            columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                        }
                        columnbind.Append("\r\n");
                    }
                    Response.Output.Write(columnbind.ToString());
                    Response.Flush();
                    Response.End();
                    Session["MOBILEDATA"] = null;
                }
                catch (Exception ex1)
                {
                    string str = ex1.Message;
                }
            }
            else
            {
                if (Convert.ToString(Session["DATEWISE"]) == "DATEWISE")
                {
                    Session["DATEWISE"] = "";
                    DataTable dt = ob.GetUserSMSReportDetail(p[0], p[1], p[2], p[3],p[4],p[5],p[6],p[7],p[8],p[9], p[10], p[11]); 
                    //GridView gv = new GridView();
                    //gv.DataSource = dt;
                    //gv.DataBind();

                    //string attachment = "attachment; filename=SMSReport.xls";
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.AddHeader("content-disposition", attachment);
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.Charset = "";

                    //StringWriter oStringWriter = new StringWriter();
                    //HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                    ////exportToExcel.RenderControl(oHtmlTextWriter);
                    //gv.RenderControl(oHtmlTextWriter);
                    //Response.Write(oStringWriter.ToString());

                    //Response.End();




                    try
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=SMSReport.csv");
                        Response.Charset = "";
                        Response.ContentType = "application/text";
                        //    Response.ContentEncoding = Encoding.UTF8;
                        //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                        StringBuilder columnbind = new StringBuilder();
                        for (int k = 0; k < (dt).Columns.Count; k++)
                        {

                            columnbind.Append(dt.Columns[k].ColumnName + ',');
                        }

                        columnbind.Append("\r\n");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int k = 0; k < dt.Columns.Count; k++)
                            {

                                columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                            }

                            columnbind.Append("\r\n");
                        }



                        Response.Output.Write(columnbind.ToString());
                        Response.Flush();
                        Response.End();
                        Session["MOBILEDATA"] = null;
                    }

                    catch (Exception ex1)
                    {
                        string str = ex1.Message;
                    }
                }
                else
                {
                    DataTable dt = ob.GetUserSMSReportDetailSingle(p[0], p[1], p[2], p[3] ,p[4], p[5], p[6], p[7], p[8], p[9], p[10], p[11]);
                    //GridView gv = new GridView();
                    //gv.DataSource = dt;
                    //gv.DataBind();

                    //string attachment = "attachment; filename=SMSReport.xls";
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.AddHeader("content-disposition", attachment);
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.Charset = "";

                    //StringWriter oStringWriter = new StringWriter();
                    //HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                    ////exportToExcel.RenderControl(oHtmlTextWriter);
                    //gv.RenderControl(oHtmlTextWriter);
                    //Response.Write(oStringWriter.ToString());

                    //Response.End();



                    try
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=SMSReport.csv");
                        Response.Charset = "";
                        Response.ContentType = "application/text";
                        //    Response.ContentEncoding = Encoding.UTF8;
                        //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                        StringBuilder columnbind = new StringBuilder();
                        for (int k = 0; k < (dt).Columns.Count; k++)
                        {

                            columnbind.Append(dt.Columns[k].ColumnName + ',');
                        }

                        columnbind.Append("\r\n");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int k = 0; k < dt.Columns.Count; k++)
                            {

                                columnbind.Append(dt.Rows[i][k].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(10))), @" ") + ',');
                            }

                            columnbind.Append("\r\n");
                        }



                        Response.Output.Write(columnbind.ToString());
                        Response.Flush();
                        Response.End();
                        Session["MOBILEDATA"] = null;
                    }

                    catch (Exception ex1)
                    {
                        string str = ex1.Message;
                    }
                }
            }
        }
    }
}