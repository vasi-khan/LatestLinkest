using eMIMPanel.Helper;
using Shortnr.Web.Business.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Notification : System.Web.UI.Page
    {

        string usertype = "";
        string user = "";
        Helper.Util ob = new Helper.Util();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 1200;
                user = Convert.ToString(Session["UserID"]);
                if (user == "") Response.Redirect("Login.aspx");
                string count = "";
                count = @"select count(*) as cnt from Notification where Username='" + user + "'";
                DataTable d = new DataTable("d");
                d = database.GetDataTable(count);

                if (Convert.ToInt16(d.Rows[0]["cnt"]) == 1)
                {
                    DataTable dt = ob.GetNotification(user);
                    
                    txtEmail.Text = Convert.ToString(dt.Rows[0]["EmailDetails"].ToString());
                    txtMobile.Text = Convert.ToString(dt.Rows[0]["MobileDetails"].ToString());
                    bool dal = Convert.ToBoolean(dt.Rows[0]["Daily"]);
                    bool Week = Convert.ToBoolean(dt.Rows[0]["Weekly"]);
                    bool Month = Convert.ToBoolean(dt.Rows[0]["Monthly"]);
                    if (txtEmail.Text == "")
                    {
                        chmail.Checked = false;
                    }
                    else
                    {
                        chmail.Checked = true;
                    }
                    if (txtMobile.Text == "")
                    {
                        chmob.Checked = false;
                    }
                    else
                    {
                        chmob.Checked = true;
                    }
                    if (dal == true)
                    {
                        chDaily.Checked = true;
                    }
                    else
                    {
                        chDaily.Checked = false;
                    }
                    if (Week == true)
                    {
                        chWeekly.Checked = true;
                    }
                    else
                    {
                        chWeekly.Checked = false;
                    }
                    if (Month == true)
                    {
                        chMonthly.Checked = true;
                    }
                    else
                    {
                        chMonthly.Checked = false;
                    }
                }
                else
                {
                    user = Convert.ToString(Session["UserID"]);
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            InsertData();
        }
        public void InsertData()
        {
            bool Daily;
            bool Monthly;
            bool Weekly;
            string usertype = Convert.ToString(Session["UserType"]);
            string user = Convert.ToString(Session["UserID"]);
            if (chDaily.Checked == true)
            {
                Daily = true;
            }
            else
            {
                Daily = false;
            }
            if (chWeekly.Checked == true)
            {
                Weekly = true;
            }
            else
            {
                Weekly = false;
            }

            if (chMonthly.Checked == true)
            {
                Monthly = true;
            }
            else
            {
                Monthly = false;
            }
            string[] mobList = txtMobile.Text.Split(',');

            DataTable dt = ob.UpdateNotification(user, "", txtEmail.Text, txtMobile.Text, Daily, Weekly, Monthly);

        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            DataTable dt = ob.Daily();
        }

        protected void btnWeekly_Click(object sender, EventArgs e)
        {
            DataTable dt = ob.Weekly();
        }

        protected void btnMonthly_Click(object sender, EventArgs e)
        {
            DataTable dt = ob.Monthly();
        }
    }
}