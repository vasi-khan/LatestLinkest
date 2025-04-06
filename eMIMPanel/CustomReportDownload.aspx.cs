using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.IO;
using System.Globalization;
using Ionic.Zip;
using System.IO.Compression;

namespace eMIMPanel
{
    public partial class CustomReportDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 3600;
            string user = Convert.ToString(Session["User"]);
            if (user == "")
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                ddlreportdelect();

            }

        }



        public void ddlreportdelect()
        {
            string sp_name = "sp_reportcustomer";
            DataTable dt = Helper.database.GetDataTable(sp_name);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ddlreport.DataSource = dt;
                    ddlreport.DataTextField = "ReportName";
                    ddlreport.DataValueField = "id";

                    ddlreport.DataBind();
                    ddlreport.Items.Insert(0, new ListItem("--Select__", "0"));
                }

            }

        }
        public class Addcontrol
        {
            public int Id { get; set; }
            public string Lable { get; set; }
        }






        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            DataTable dtn = (ViewState["Dltdata"]) as DataTable;
            //foreach (DataRow dr in dtn.Rows)

            // validation all  AllTextBox start
            if (dtn.Rows[0]["FT1"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt1.Text.Trim()))
                {
                    string date = txt1.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }



                }

            }

            if (dtn.Rows[0]["FT2"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt2.Text.Trim()))
                {
                    string date = txt2.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }



                }

            }

            if (dtn.Rows[0]["FT3"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt3.Text.Trim()))
                {
                    string date = txt3.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT4"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt4.Text.Trim()))
                {
                    string date = txt4.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT5"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt5.Text.Trim()))
                {
                    string date = txt5
.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT6"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt6.Text.Trim()))
                {
                    string date = txt6
.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT7"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt7.Text.Trim()))
                {
                    string date = txt7.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT8"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt8.Text.Trim()))
                {
                    string date = txt8
.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT9"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt9.Text.Trim()))
                {
                    string date = txt9.Text.Trim();
                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (dtn.Rows[0]["FT10"].ToString() == "DateTime")
            {
                if (!String.IsNullOrEmpty(txt10.Text.Trim()))
                {
                    string date = txt10.Text.Trim();
                    //bool da = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DatatTimeStyleNone, out parsed);

                    string[] arg = date.Split('-');
                    string year = arg[0];
                    string month = arg[1];
                    string day = arg[2];

                    if (year.Length != 4 || month.Length != 2 || day.Length != 2 || int.Parse(month) > 12 || int.Parse(month) < 0 || int.Parse(day) > 31 || int.Parse(day) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Invalid Date');", true);
                        return;

                    }




                }

            }

            if (txt1.Visible == true)
            {
                if (string.IsNullOrEmpty(txt1.Text.Trim()))
                {
                    string msg = "Enter " + ldl1.InnerText;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }


            if (txt2.Visible == true)
            {
                if (string.IsNullOrEmpty(txt2.Text.Trim()))
                {
                    string msg = "Enter " + lbl2.InnerText.ToString().Replace("<br/>", "");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }

            if (txt3.Visible == true)
            {
                if (string.IsNullOrEmpty(txt3.Text.Trim()))
                {
                    string msg = "Enter " + lbl3.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }


            if (txt4.Visible == true)
            {
                if (string.IsNullOrEmpty(txt4.Text.Trim()))
                {
                    string msg = "Enter " + lbl4.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }

            if (txt5.Visible == true)
            {
                if (string.IsNullOrEmpty(txt5.Text.Trim()))
                {
                    string msg = "Enter " + lbl5.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }

            if (txt6.Visible == true)
            {
                if (string.IsNullOrEmpty(txt6.Text.Trim()))
                {
                    string msg = "Enter " + lbl6.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }
            if (txt7.Visible == true)
            {
                if (string.IsNullOrEmpty(txt7.Text.Trim()))
                {
                    string msg = "Enter " + lbl7.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }


            if (txt8.Visible == true)
            {
                if (string.IsNullOrEmpty(txt8.Text.Trim()))
                {
                    string msg = "Enter " + lbl8.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }

            if (txt9.Visible == true)
            {
                if (string.IsNullOrEmpty(txt9.Text.Trim()))
                {
                    string msg = "Enter " + lbl9.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }

            if (txt10.Visible == true)
            {
                if (string.IsNullOrEmpty(txt10.Text.Trim()))
                {
                    string msg = "Enter " + lbl10.InnerText.ToString().Replace("<br/>", " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;

                }


            }


            // validation all  AllTextBox End
            List<SqlParameter> pram = new List<SqlParameter>();
            DataTable dttt = (ViewState["Dltdata"]) as DataTable;
            string ReportName = dttt.Rows[0]["Rp_name"].ToString();

            foreach (DataRow dr in dttt.Rows)
            {

                {
                    if (!String.IsNullOrEmpty(dr["FN1"].ToString()))
                    {
                        if (dr["FT1"].ToString() == "DateTime")
                        {
                            string FN1 = dr["FN1"].ToString();
                            pram.Add(new SqlParameter(FN1, txt1.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt1.Text.Trim())));
                        }
                        else if (dr["FT1"].ToString() == "Bit")
                        {
                            string FN1 = dr["FN1"].ToString();
                            pram.Add(new SqlParameter(FN1, txt1.Text == "" ? "" : (txt1.Text.Trim())));

                        }
                        else if (dr["FT1"].ToString() == "nvarchar")
                        {
                            string FN1 = dr["FN1"].ToString();
                            pram.Add(new SqlParameter(FN1, txt1.Text == "" ? "" : txt1.Text.Trim()));

                        }
                        else
                        {
                            string FN1 = dr["FN1"].ToString();
                            pram.Add(new SqlParameter(FN1, txt1.Text == "" ? "" : txt1.Text.Trim()));
                        }

                    }



                    if (!String.IsNullOrEmpty(dr["FN2"].ToString()))
                    {
                        if (dr["FT2"].ToString() == "DateTime")
                        {
                            string FN1 = dr["FN2"].ToString();
                            pram.Add(new SqlParameter(FN1, txt1.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt2.Text.Trim())));
                        }
                        else if (dr["FT2"].ToString() == "Bit")
                        {
                            string FN2 = dr["FN2"].ToString();
                            pram.Add(new SqlParameter(FN2, txt1.Text == "" ? "" : (txt2.Text.Trim())));

                        }
                        else if (dr["FT2"].ToString() == "nvarchar")
                        {
                            string FN2 = dr["FN2"].ToString();
                            pram.Add(new SqlParameter(FN2, txt2.Text == "" ? "" : txt2.Text.Trim()));
                        }
                        else
                        {
                            string FN2 = dr["FN2"].ToString();
                            pram.Add(new SqlParameter(FN2, txt2.Text == "" ? "" : txt2.Text.Trim()));
                        }

                    }

                    if (!String.IsNullOrEmpty(dr["FN3"].ToString()))
                    {
                        if (dr["FT3"].ToString() == "DateTime")
                        {
                            string FN3 = dr["FN3"].ToString();
                            pram.Add(new SqlParameter(FN3, txt3.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt3.Text.Trim())));
                        }
                        else if (dr["FT3"].ToString() == "Bit")
                        {
                            string FN3 = dr["FN3"].ToString();
                            pram.Add(new SqlParameter(FN3, txt3.Text == "" ? "" : (txt3.Text.Trim())));

                        }
                        else if (dr["FT3"].ToString() == "nvarchar")
                        {
                            string FN3 = dr["FN3"].ToString();
                            pram.Add(new SqlParameter(FN3, txt3.Text == "" ? "" : txt3.Text.Trim()));
                        }
                        else
                        {
                            string FN3 = dr["FN3"].ToString();
                            pram.Add(new SqlParameter(FN3, txt3.Text == "" ? "" : txt3.Text.Trim()));
                        }

                    }

                    if (!String.IsNullOrEmpty(dr["FN4"].ToString()))
                    {
                        if (dr["FT4"].ToString() == "DateTime")
                        {
                            String FN4 = (dr["FN4"].ToString());
                            pram.Add(new SqlParameter(FN4, txt4.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : ((txt4.Text.Trim()))));
                        }
                        else if (dr["FT4"].ToString() == "Bit")
                        {
                            string FN4 = dr["FN4"].ToString();
                            pram.Add(new SqlParameter(FN4, txt4.Text == "" ? "" : (txt4.Text.Trim())));

                        }
                        else if (dr["FT4"].ToString() == "nvarchar")
                        {
                            string FN4 = dr["FN4"].ToString();
                            pram.Add(new SqlParameter(FN4, txt4.Text == "" ? "" : txt4.Text.Trim()));
                        }
                        else
                        {
                            string FN4 = dr["FN4"].ToString();
                            pram.Add(new SqlParameter(FN4, txt4.Text == "" ? "" : txt4.Text.Trim()));
                        }

                    }


                    if (!String.IsNullOrEmpty(dr["FN5"].ToString()))
                    {
                        if (dr["FT5"].ToString() == "DateTime")
                        {
                            string FN5 = dr["FN5"].ToString();
                            pram.Add(new SqlParameter(FN5, txt5.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt5.Text.Trim())));
                        }
                        else if (dr["FT5"].ToString() == "Bit")
                        {
                            string FN5 = dr["FN5"].ToString();
                            pram.Add(new SqlParameter(FN5, txt5.Text == "" ? "" : (txt5.Text.Trim())));

                        }
                        else if (dr["FT5"].ToString() == "nvarchar")
                        {
                            string FN5 = dr["FN5"].ToString();
                            pram.Add(new SqlParameter(FN5, txt5.Text == "" ? "" : txt5.Text.Trim()));
                        }
                        else
                        {
                            string FN5 = dr["FN5"].ToString();
                            pram.Add(new SqlParameter(FN5, txt5.Text == "" ? "" : txt5.Text.Trim()));
                        }

                    }




                    if (!String.IsNullOrEmpty(dr["FN6"].ToString()))
                    {
                        if (dr["FT6"].ToString() == "DateTime")
                        {
                            string FN6 = dr["FN6"].ToString();
                            pram.Add(new SqlParameter(FN6, txt6.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt6.Text.Trim())));
                        }
                        else if (dr["FT6"].ToString() == "Bit")
                        {
                            string FN6 = dr["FN6"].ToString();
                            pram.Add(new SqlParameter(FN6, txt6.Text == "" ? "" : (txt6.Text.Trim())));

                        }
                        else if (dr["FT6"].ToString() == "nvarchar")
                        {

                            string FN6 = dr["FN6"].ToString();
                            pram.Add(new SqlParameter(FN6, txt6.Text == "" ? "" : txt6.Text.Trim()));

                        }
                        else
                        {
                            string FN6 = dr["FN6"].ToString();
                            pram.Add(new SqlParameter(FN6, txt6.Text == "" ? "" : txt6.Text.Trim()));
                        }

                    }

                    if (!String.IsNullOrEmpty(dr["FN7"].ToString()))
                    {
                        if (dr["FT7"].ToString() == "DateTime")
                        {
                            string FN7 = dr["FN7"].ToString();
                            pram.Add(new SqlParameter(FN7, txt7.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt7.Text.Trim())));
                        }
                        else if (dr["FT7"].ToString() == "Bit")
                        {
                            string FN7 = dr["FN7"].ToString();
                            pram.Add(new SqlParameter(FN7, txt7.Text == "" ? "" : (txt7.Text.Trim())));

                        }
                        else if (dr["FT7"].ToString() == "nvarchar")
                        {
                            string FN7 = dr["FN7"].ToString();
                            pram.Add(new SqlParameter(FN7, txt7.Text == "" ? "" : txt7.Text.Trim()));

                        }
                        else
                        {
                            string FN7 = dr["FN7"].ToString();
                            pram.Add(new SqlParameter(FN7, txt7.Text == "" ? "" : txt7.Text.Trim()));
                        }

                    }


                    if (!String.IsNullOrEmpty(dr["FN8"].ToString()))
                    {
                        if (dr["FT8"].ToString() == "DateTime")
                        {
                            string FN8 = dr["FN8"].ToString();
                            pram.Add(new SqlParameter(FN8, txt8.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt8.Text.Trim())));
                        }
                        else if (dr["FT8"].ToString() == "Bit")
                        {
                            string FN8 = dr["FN8"].ToString();
                            pram.Add(new SqlParameter(FN8, txt8.Text == "" ? "" : (txt8.Text.Trim())));

                        }
                        else if (dr["FT8"].ToString() == "nvarchar")
                        {
                            string FN8 = dr["FN8"].ToString();
                            pram.Add(new SqlParameter(FN8, txt8.Text == "" ? "" : txt8.Text.Trim()));
                        }
                        else
                        {
                            string FN8 = dr["FN8"].ToString();
                            pram.Add(new SqlParameter(FN8, txt8.Text == "" ? "" : txt8.Text.Trim()));
                        }

                    }

                    if (!String.IsNullOrEmpty(dr["FN9"].ToString()))
                    {
                        if (dr["FT9"].ToString() == "DateTime")
                        {
                            string FN9 = dr["FN9"].ToString();
                            pram.Add(new SqlParameter(FN9, txt9.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt9.Text.Trim())));
                        }
                        else if (dr["FT9"].ToString() == "Bit")
                        {
                            string FN9 = dr["FN9"].ToString();
                            pram.Add(new SqlParameter(FN9, txt9.Text == "" ? "" : (txt9.Text.Trim())));

                        }
                        else if (dr["FT9"].ToString() == "nvarchar")
                        {
                            string FN9 = dr["FN9"].ToString();
                            pram.Add(new SqlParameter(FN9, txt9.Text == "" ? "" : txt9.Text.Trim()));
                        }
                        else
                        {
                            string FN9 = dr["FN9"].ToString();
                            pram.Add(new SqlParameter(FN9, txt9.Text == "" ? "" : txt9.Text.Trim()));
                        }

                    }

                    if (!String.IsNullOrEmpty(dr["FN10"].ToString()))
                    {
                        if (dr["FT10"].ToString() == "DateTime")
                        {
                            string FN10 = dr["FN10"].ToString();
                            pram.Add(new SqlParameter(FN10, txt10.Text.Trim() == "" ? DateTime.Now.ToString("1874-01-01") : (txt10.Text.Trim())));
                        }
                        else if (dr["FT10"].ToString() == "Bit")
                        {
                            string FN10 = dr["FN10"].ToString();
                            pram.Add(new SqlParameter(FN10, txt10.Text == "" ? "" : (txt10.Text.Trim())));

                        }
                        else if (dr["FT10"].ToString() == "nvarchar")
                        {
                            string FN10 = dr["FN10"].ToString();
                            pram.Add(new SqlParameter(FN10, txt10.Text == "" ? "" : txt10.Text.Trim()));
                        }
                        else
                        {
                            string FN10 = dr["FN10"].ToString();
                            pram.Add(new SqlParameter(FN10, txt10.Text == "" ? "" : txt10.Text.Trim()));
                        }

                    }





                }

            }




            string sp_name = Convert.ToString(ViewState["procedure"]);


            Helper.Util db = new Helper.Util();
            DataTable dt = db.ProcedureDatatable(sp_name, pram);

            if (rdoshow_Grid.SelectedValue == "Grid")
            {
                bindgridView(dt);

            }
            else if (rdoshow_Grid.SelectedValue == "CSV")
            {
                string mappath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + "Report" + DateTime.Now.ToString("_yyyyMMddHHmmss");
                //WriteData(@"D:\project\restaurantNEW\RestaurantOrder\ZipFiles\", dt, 1, ReportName);
                WriteData(mappath, dt, 1, ReportName);
            }


        }
        public void WriteData(string mappath, DataTable dt, int fn, string ReportName)
        {
            DataView dv = new DataView(dt);
            //DataTable dtDates = dv.ToTable(true, "SMSdate");

            bool exists = System.IO.Directory.Exists(mappath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(mappath);
            }

            if (dt.Rows.Count > 0)
            {
                DataTable data = dt.AsEnumerable().CopyToDataTable();
                //  string mydate = dtDates.Rows[0]["SMSdate"].ToString();

                StringBuilder sbText = new StringBuilder();
                //write column header names
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        sbText.Append(",");
                    }
                    sbText.Append(data.Columns[i].ColumnName);
                }
                sbText.Append(Environment.NewLine);

                //Write data
                foreach (DataRow row in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbText.Append(",");
                        }
                        if (row[i].ToString().Contains(","))
                        {
                            sbText.Append(String.Format("\"{0}\"", row[i].ToString()));
                        }
                        else
                        {
                            if (i == 2)
                                sbText.Append(String.Format("'{0}", row[i].ToString()));
                            else
                                sbText.Append(row[i].ToString());
                        }
                        // sbText.Append(row[i].ToString());
                    }
                    sbText.Append(Environment.NewLine);
                }
                //
                StreamWriter sw = new StreamWriter(mappath + @"\" + ReportName + " " + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".csv", false, new UTF8Encoding(true));
                {
                    sw.Write(sbText.ToString());
                    sw.Close();
                }
                sbText.Clear();
                sw.Close();

            }
    
            string startPath = mappath;//folder to add
            string ZipFileName = "Report" + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".zip";
            string ZipFileName1 = "CSVReports/" + ZipFileName;
            string zipPath = System.Configuration.ConfigurationManager.AppSettings["REPORTPATH"].ToString() + ZipFileName;//URL for your ZIP file
            //string zipPath = Server.MapPath(mappath  + "REPORT" + ".zip");//URL for your ZIP file
            if (File.Exists(zipPath)) File.Delete(zipPath);
            System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            string filename = Server.MapPath(ZipFileName1);//URL for your ZIP file
            System.IO.Directory.Delete(mappath, true);
            // string filename = zipPath;
            //Response.Redirect("DownloadZipFile.aspx?filename=" + filename);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryReport.zip");
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.Flush();
            Response.TransmitFile(fileInfo.FullName);
            Response.End();


            //System.IO.Directory.Delete(filename , true)


        }




        public void bindgridView(DataTable dt)

        {
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        grd_report.DataSource = dt;
                        grd_report.DataBind();
                        btndownload.Visible = true;

                    }
                    else
                    {
                        grd_report.DataSource = null;
                        grd_report.DataBind();
                        btndownload.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void ResetAllValue_LablAndTextBox()
        {
            btndownload.Visible = false;
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            txt4.Text = "";
            txt5.Text = "";
            txt6.Text = "";
            txt7.Text = "";
            txt8.Text = "";
            txt9.Text = "";
            txt10.Text = "";
            txt1.Visible = false;
            txt2.Visible = false;
            txt3.Visible = false;
            txt4.Visible = false;
            txt5.Visible = false;
            txt6.Visible = false;
            txt7.Visible = false;
            txt8.Visible = false;
            txt9.Visible = false;
            txt10.Visible = false;


            lbl2.InnerText = "";
            ldl1.InnerText = "";
            lbl3.InnerText = "";
            lbl4.InnerText = "";
            lbl5.InnerText = "";
            lbl6.InnerText = "";
            lbl7.InnerText = "";
            lbl8.InnerText = "";
            lbl9.InnerText = "";
            lbl10.InnerText = "";

            lbl2.Visible = false;
            ldl1.Visible = false;
            lbl3.Visible = false;
            lbl4.Visible = false;
            lbl5.Visible = false;
            lbl6.Visible = false;
            lbl7.Visible = false;
            lbl8.Visible = false;
            lbl9.Visible = false;
            lbl10.Visible = false;

        }
        protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
        {


            ResetAllValue_LablAndTextBox();
            rdoshow_Grid.Visible = true;
            string sp_name = "SP_getCustomerWiseReport";
            List<SqlParameter> pram = new List<SqlParameter>();
            pram.Add(new SqlParameter("@nid", Convert.ToInt32(ddlreport.SelectedValue)));
            DataTable dt = new DataTable();
            Helper.Util db = new Helper.Util();
            dt = db.ProcedureDatatable(sp_name, pram);
            ViewState["Dltdata"] = dt;
            ViewState["procedure"] = dt.Rows[0]["procedurename"];



            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT1"].ToString()))
            {

                string date = "";
                if (dt.Rows[0]["FT1"].ToString() == "DateTime")
                {
                    //txt1.TextMode = TextBoxMode.Date;
                    date = "YYYY-MM-DD";
                    ldl1.InnerHtml = dt.Rows[0]["FLT1"].ToString() + "<br/>" + date;




                }
                else if (dt.Rows[0]["FT1"].ToString() == "Bit")
                {

                    ldl1.InnerHtml = dt.Rows[0]["FLT1"].ToString();

                }
                else
                {
                    ldl1.InnerHtml = dt.Rows[0]["FLT1"].ToString();
                }



                txt1.Visible = true;
                ldl1.Visible = true;



            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT2"].ToString()))
            {

                string date = "";

                if (dt.Rows[0]["FT2"].ToString() == "DateTime")
                {
                    date = "YYYY-MM-DD";
                    //txt2.TextMode = TextBoxMode.Date;
                    lbl2.InnerHtml = dt.Rows[0]["FLT2"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT2"].ToString() == "Bit")
                {

                    lbl2.InnerHtml = dt.Rows[0]["FLT2"].ToString();
                }
                else
                {
                    lbl2.InnerHtml = dt.Rows[0]["FLT2"].ToString();
                }



                txt2.Visible = true;
                lbl2.Visible = true;


            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT3"].ToString()))
            {

                string date = "";
                if (dt.Rows[0]["FT3"].ToString() == "DateTime")
                {
                    date = "YYYY-MM-DD";
                    lbl3.InnerHtml = dt.Rows[0]["FLT3"].ToString() + "<br/>" + date;


                }
                else if (dt.Rows[0]["FT3"].ToString() == "Bit")
                {

                    lbl3.InnerHtml = dt.Rows[0]["FLT3"].ToString();

                }
                else
                {
                    lbl3.InnerHtml = dt.Rows[0]["FLT3"].ToString();
                }






                txt3.Visible = true;
                lbl3.Visible = true;
            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT4"].ToString()))
            {
                string date = "";
                if (dt.Rows[0]["FT4"].ToString() == "DateTime")
                {
                    date = "YYYY-MM-DD";
                    //txt4.TextMode = TextBoxMode.Date;
                    lbl4.InnerHtml = dt.Rows[0]["FLT4"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT4"].ToString() == "Bit")
                {

                    lbl4.InnerHtml = dt.Rows[0]["FLT4"].ToString();
                }
                else
                {
                    lbl4.InnerHtml = dt.Rows[0]["FLT4"].ToString();

                }




                txt4.Visible = true;
                lbl4.Visible = true;

            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT5"].ToString()))
            {
                string date = "";
                if (dt.Rows[0]["FT5"].ToString() == "DateTime")
                {
                    //txt5.TextMode = TextBoxMode.Date;
                    date = "YYYY-MM-DD";
                    lbl5.InnerHtml = dt.Rows[0]["FLT5"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT5"].ToString() == "Bit")
                {
                    lbl5.InnerHtml = dt.Rows[0]["FLT5"].ToString();
                }
                else
                {
                    lbl5.InnerHtml = dt.Rows[0]["FLT5"].ToString();
                }


                txt5.Visible = true;
                lbl5.Visible = true;

            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT6"].ToString()))
            {

                string date = "";
                if (dt.Rows[0]["FT6"].ToString() == "DateTime")
                {
                    date = "YYYY-MM-DD";
                    // txt6.TextMode = TextBoxMode.Date;
                    lbl6.InnerHtml = dt.Rows[0]["FLT6"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT6"].ToString() == "Bit")
                {
                    lbl6.InnerHtml = dt.Rows[0]["FLT6"].ToString();
                }
                else
                {
                    lbl6.InnerHtml = dt.Rows[0]["FLT6"].ToString() + date;

                }




                txt6.Visible = true;

                lbl6.Visible = true;

            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT7"].ToString()))
            {


                string date = "";


                if (dt.Rows[0]["FT7"].ToString() == "DateTime")
                {
                    //txt7.TextMode = TextBoxMode.Date;
                    date = "YYYY-MM-DD";
                    lbl6.InnerHtml = dt.Rows[0]["FLT7"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT7"].ToString() == "Bit")
                {
                    lbl7.InnerHtml = dt.Rows[0]["FLT7"].ToString();
                }
                else
                {
                    lbl7.InnerHtml = dt.Rows[0]["FLT7"].ToString();
                }


                txt7.Visible = true;
                lbl7.Visible = true;

            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT8"].ToString()))
            {
                string date = "";
                if (dt.Rows[0]["FT8"].ToString() == "DateTime")
                {

                    //txt8.TextMode = TextBoxMode.Date;
                    date = "YYYY-MM-DD";
                    lbl8.InnerHtml = dt.Rows[0]["FLT8"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT8"].ToString() == "Bit")
                {
                    lbl8.InnerHtml = dt.Rows[0]["FLT8"].ToString();
                }
                else
                {
                    lbl8.InnerHtml = dt.Rows[0]["FLT8"].ToString();

                }



                txt8.Visible = true;
                lbl8.Visible = true;

            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT9"].ToString()))
            {
                string date = "";
                if (dt.Rows[0]["FT9"].ToString() == "DateTime")
                {
                    //txt9.TextMode = TextBoxMode.Date;
                    date = "YYYY-MM-DD";
                    lbl9.InnerHtml = dt.Rows[0]["FLT9"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT9"].ToString() == "Bit")
                {
                    lbl9.InnerHtml = dt.Rows[0]["FLT9"].ToString();
                }
                else
                {
                    lbl9.InnerHtml = dt.Rows[0]["FLT9"].ToString();
                }



                txt9.Visible = true;
                lbl9.Visible = true;
            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["FLT10"].ToString()))
            {
                string date = "";
                if (dt.Rows[0]["FT10"].ToString() == "DateTime")
                {
                    date = "YYYY-MM-DD";
                    //txt10.TextMode = TextBoxMode.Date;
                    lbl10.InnerHtml = dt.Rows[0]["FLT10"].ToString() + "<br/>" + date;

                }
                else if (dt.Rows[0]["FT10"].ToString() == "Bit")
                {
                    lbl10.InnerHtml = dt.Rows[0]["FLT10"].ToString();
                }
                else
                {
                    lbl10.InnerHtml = dt.Rows[0]["FLT10"].ToString();

                }



                txt10.Visible = true;
                lbl10.Visible = true;


            }




            btnreset.Visible = true;
            btnsubmit.Visible = true;


        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            ddlreport.SelectedValue = "0";
            ResetAllValue_LablAndTextBox();
            grd_report.DataSource = null;
            grd_report.DataBind();

        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            Response.Clear();
            string attachment = "attachment;filename=CustomReport.xls";
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grd_report.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();


        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
