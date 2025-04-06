using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{
    public partial class Notificationcentre : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        Helper.Util ob = new Helper.Util();
        Helper.smpp ob1 = new Helper.smpp();
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["Userid"]);
            if (user == "") Response.Redirect("login.aspx");
            if (!Page.IsPostBack)
            {
                string paisa = Convert.ToString(Session["SUBCURRENCY"]);
                DataTable dtRate = ob.GetSettingDetails();

                lblSMS1.Text = Convert.ToString(dtRate.Rows[0]["NotificationSMSRate"]) + " " + paisa;
                lblEMAIL1.Text = Convert.ToString(dtRate.Rows[0]["NotificationEmailRate"]) + " " + paisa;
                lblWHATSAPP1.Text = Convert.ToString(dtRate.Rows[0]["NotificationWhatsappRate"]) + " " + paisa;
                lblVOICE1.Text = Convert.ToString(dtRate.Rows[0]["NotificationOBDRate"]) + " " + paisa; 


                string sql = "Select * from Notification where userName='" + user + "'";
                dt = ob1.DataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    SetInitialRow();
                }
                else
                {
                    string sql1 = " Select ROW_NUMBER() OVER (ORDER BY name ASC) AS Rownumber,Name as column1,Mobile as column2,Email as column3 from notificationmember where userName='" + user + "' ";
                    txtbalance.Text = dt.Rows[0]["MinBal"].ToString();
                    txtper.Text = dt.Rows[0]["lastrechargeper"].ToString();
                    if (txtper.Text != "0")
                    {
                        Sen6.Attributes.Add("style", "display:d-none;");
                    }
                    chksms.Checked = Convert.ToBoolean(dt.Rows[0]["sms"].ToString());
                    chkemail.Checked = Convert.ToBoolean(dt.Rows[0]["emailid"].ToString());
                    chkvoice.Checked = Convert.ToBoolean(dt.Rows[0]["voice"].ToString());
                    chkwhatsapp.Checked = Convert.ToBoolean(dt.Rows[0]["whatsapp"].ToString());
                    if (chksms.Checked && chkemail.Checked && chkvoice.Checked && chkwhatsapp.Checked == true)
                    {
                        chkall.Checked = true;
                    }
                    DataTable dt1 = ob1.DataTable(sql1);
                    ViewState["CurrentTable"] = dt1;
                    grv.DataSource = dt1;
                    grv.DataBind();
                }
            }
        }
        private void SetInitialRow()
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataTable dt = new DataTable();
            if (dtCurrentTable == null)
            {

                dt.Columns.Add(new DataColumn("RowNumber", typeof(int)));//for TextBox value
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));//for TextBox value   
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));//for TextBox value   
                dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            }

            ViewState["CurrentTable"] = dt;
            grv.DataSource = dt;
            grv.DataBind();
        }
        protected void button5_Click(object sender, EventArgs e)
        {
            Sen6.Attributes.Add("style", "display:d-none;");
        }
        protected void button6_Click(object sender, EventArgs e)
        {
            //txtper.Text = "";
            Sen6.Attributes.Add("style", "display:none;");
        }
        private bool ValidateEmail()
        {
            string email = txtemail.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Email address');", true);
                return false;
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            int num = 0;
            // if (!ValidateEmail() == true) return;
            //if (txtmobileno.Text.Length == 10 || txtmobileno.Text.Length==12)
            //{


            //}
            //else
            //{

            //}
            if (string.IsNullOrEmpty(txtmobileno.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Mobile  No.');", true);
                return;
            }


            //if (grv.Rows.Count > 0)
            //{
            //    foreach (GridViewRow row in grv.Rows)
            //    {
            //        Label TextBox1 = (Label)row.FindControl("lblmobile");
            //        if (TextBox1.Text == txtmobileno.Text)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile No already Exit');", true);
            //            return;
            //        }
            //        Label TextBox2 = (Label)row.FindControl("lblemail");
            //        if (TextBox2.Text == txtemail.Text)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email already Exit');", true);
            //            return;
            //        }
            //    }
            //}



            if (btnsubmit.Text == "Submit")
            {
                AddNewRowToGrid();
            }
            else
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                num = Convert.ToInt32(Session["num"]);
                dt.Rows.Remove(dt.Rows[num - 1]);
                ResetRowID(dt);
                ViewState["CurrentTable"] = dt;
                AddNewRowToGrid();
            }
            refresh();
        }
        protected void refresh()
        {
            txtname.Text = "";
            txtmobileno.Text = "";
            txtemail.Text = "";
            btnsubmit.Text = "Submit";
            Session["num"] = null;
        }
        private void AddNewRowToGrid()
        {
            try
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable == null)
                {
                    SetInitialRow();
                }
                else
                {
                    drCurrentRow = dtCurrentTable.NewRow();

                    string mob = txtmobileno.Text.Trim();
                    string email = txtemail.Text.Trim();

                    string findMob = string.Format("column2 = '{0}'", mob);
                    var foundMobRows = dtCurrentTable.Select(findMob);

                    string findEmail = string.Format("column3 = '{0}'", email);
                    DataRow[] foundEmailRows = dtCurrentTable.Select(findEmail);

                    if (foundMobRows.Length != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Mobile No already Exit');", true);
                        return;
                    }
                    if (foundEmailRows.Length != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email already Exit');", true);
                        return;
                    }

                    drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                    drCurrentRow["Column1"] = txtname.Text;
                    drCurrentRow["Column2"] = mob;
                    drCurrentRow["Column3"] = email;
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    grv.DataSource = dtCurrentTable;
                    grv.DataBind();
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message + ".');", true);
            }
        }
        protected void chkall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkall.Checked)
            {
                chksms.Checked = true;
                chkemail.Checked = true;
                chkvoice.Checked = true;
                chkwhatsapp.Checked = true;
            }
            else
            {
                chksms.Checked = false;
                chkemail.Checked = false;
                chkvoice.Checked = false;
                chkwhatsapp.Checked = false;
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            int rowID = gvRow.RowIndex;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                dt.Rows.Remove(dt.Rows[rowID]);
                ResetRowID(dt);
                ViewState["CurrentTable"] = dt;
                grv.DataSource = dt;
                grv.DataBind();
            }

        }

        private void ResetRowID(DataTable dt)
        {
            int rowNumber = 1;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row[0] = rowNumber;
                    rowNumber++;
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            string Name = ((Label)gvRow.FindControl("lblname")).Text;
            string mobile = ((Label)gvRow.FindControl("lblmobile")).Text;
            string email = ((Label)gvRow.FindControl("lblemail")).Text;
            string rowno = ((Label)gvRow.FindControl("lblSeq")).Text;
            Session["num"] = Convert.ToInt32(rowno);
            txtname.Text = Name;
            txtmobileno.Text = mobile;
            txtemail.Text = email;
            btnsubmit.Text = "Update";
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (txtbalance.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Fill Minimun Balance.');", true);
                return;
            }
            if (txtper.Text == "")
            {
                txtper.Text = "0";

            }
            //if (grv.Rows.Count == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Add atleast one member.');", true);
            //    return;
            //}
            ob1.updatenotification(Convert.ToString(Session["Userid"]));
            ob1.NotificationCentre(Convert.ToString(Session["Userid"]), Convert.ToDouble(txtbalance.Text), Convert.ToDouble(txtper.Text), chksms.Checked, chkemail.Checked, chkvoice.Checked, chkwhatsapp.Checked);
            foreach (GridViewRow row in grv.Rows)
            {
                Label t1 = (Label)row.FindControl("lblName");
                Label t2 = (Label)row.FindControl("lblmobile");
                Label t3 = (Label)row.FindControl("lblEmail");
                ob1.addmember(Convert.ToString(Session["Userid"]), t1.Text, t2.Text, t3.Text);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Save Successfully');window.location='Notificationcentre.aspx';", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Reset Successfully');window.location='Notificationcentre.aspx';", true);
        }
    }

    //protected void ddlbalance_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if(ddlbalance.SelectedItem.Value=="1")
    //    {
    //        ddlbalance1.SelectedItem.Value = "2";
    //    }
    //    else
    //    {
    //        ddlbalance1.SelectedItem.Value = "1";
    //    }
    //}
}