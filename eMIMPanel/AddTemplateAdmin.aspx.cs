using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eMIMPanel
{

    public partial class AddTemplateAdmin : System.Web.UI.Page
    {

        
        Helper.Util ob;
        string user;
        string usertype;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if(IsPostBack && (Convert.ToInt16(ViewState["clear_after_add"])) == 1)
            {
                //ViewState["clear_after_add"] = 0;
                ViewState["TempDataRows"] = null;
                grdtemp.DataSource = null;
                grdtemp.DataBind();
            }*/

            ob = new Helper.Util();

            usertype = Convert.ToString(Session["UserType"]);
            user = Convert.ToString(Session["User"]);
            if (user == "") Response.Redirect("login.aspx");
            //if (usertype != "SYSADMIN") Response.Redirect("index2.aspx");
            if (usertype.ToUpper() == "ADMIN")
            {
                foreach (ListItem item in rblType.Items)
                {
                    if (item.Text == "API")
                    {
                        item.Enabled = false;
                        item.Attributes.Add("style", "color:#999;");
                        break;
                    }
                }
            }
            HideDiv();
        }


        private void HideDiv()
        {
            rblType.Items[0].Attributes.CssStyle.Add("margin-right", "5px;");
            if (rblType.SelectedValue == "API")
            {
                divUser.Style.Add("display", "none");
                divTempWord.Style.Add("display", "show");
                divSenderId.Style.Add("display", "show");
            }
            else
            {
                divTempWord.Style.Add("display", "none");
                divUser.Style.Add("display", "show");
                divSenderId.Style.Add("display", "none");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }


        private bool ValidateTemplate()
        {

            if (txtTempId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template ID');", true);
                return false;
            }

            if (txtTempName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Name');", true);
                return false;
            }

            if (txtTemplateContent.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Content Text');", true);
                return false;
            }

            if (rblType.SelectedValue == "API")
            {
                if (txtSenderId.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Sender Id');", true);
                    return false;
                }
                /*
                if (txtTempWord.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Template Variable Text');", true);
                    return false;
                }
                string tempWords = txtTempWord.Text.Trim().ToUpper();
                if (tempWords.Contains("#VAR"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please add semicolon(;) insted of {#var#} in Template Phrase');", true);
                    return false;
                }
                */
                DataTable dt = ob.ValidateTemplateIdforAPI(txtTempId.Text.Trim(), txtTempName.Text, txtSenderId.Text.Trim());
                if (Convert.ToInt16(dt.Rows[0]["TemplateId"]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template ID already exist');", true);
                    return false;
                }
            }
            else
            {
                if (usertype != "SYSADMIN")
                {
                    if (ob.GetDLTofUser(txtUser.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id');", true);
                        return false;
                    }
                    if (txtuserid1.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid1.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 1st Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid2.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid2.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 2nd Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid3.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid3.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 3rd Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid4.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid4.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 4th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid5.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid5.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 5th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid6.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid6.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 6th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid7.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid7.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 7th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid8.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid8.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 8th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid9.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid9.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 9th Text Box');", true);
                            return false;
                        }
                    }
                    if (txtuserid10.Text != "")
                    {
                        if (ob.GetDLTofUser(txtuserid10.Text).ToUpper() != Convert.ToString(Session["DLT"]).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id on 10th Text Box');", true);
                            return false;
                        }
                    }
                }
                if (txtUser.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter User Name');", true);
                    return false;
                }
                DataSet ds = ob.ValidateTemplateRequest(txtTempId.Text.Trim(), txtTempName.Text.Trim(), txtUser.Text.Trim());

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["TemplateId"]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template ID already exist');", true);
                    return false;
                }
                if (Convert.ToInt16(ds.Tables[1].Rows[0]["Templatename"]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Name already exist');", true);
                    return false;
                }
            }
            return true;
        }

/*


        protected void clearcontrols()
        {
            txtSenderId.Text = "";
            txtTemplateContent.Text = "";
            txtTempName.Text = "";
            txtTempId.Text = "";
            txtTempWord.Text = "";
        }

            */


        /*



            protected void btnSubmit_Click(object sender, EventArgs e)
        {

          








            string senderId = txtSenderId.Text.Trim();
            string tempMsg = txtTemplateContent.Text;
            string tempWord = txtTempWord.Text;
            string tempName = txtTempName.Text;
            string tempId = txtTempId.Text.Trim();

            if (ValidateTemplate())
            {    // {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[5]{
                new DataColumn("tempId"),
                new DataColumn("tempName"),
                new DataColumn("tempMsg"),
                new DataColumn("tempsenderId"),
                new DataColumn("tempWord")
            });


                if (ViewState["TempDataRows"] != null)
                {
                    dt = (DataTable)ViewState["TempDataRows"];
                }
                dt.Rows.Add(tempId, tempName, tempMsg, senderId, tempWord);
                dt.AcceptChanges();
                ViewState["TempDataRows"] = dt;



               // grdtemp.DataSource = dt;

               // grdtemp.DataBind();

                clearcontrols();
            }
 
        }

            */ 










        protected void btnSave_Click(object sender, EventArgs e)
        {

            string[] tempIdlist = new string[10];
            string[] tempNamelist = new string[10];
            string[] tempMsglist = new string[10];
            //string mq= txtTempId.Text.Trim(); 
            int i = 0;
            tempIdlist[i] = txtTempId.Text.Trim();
            tempNamelist[i] = txtTempName.Text.Trim();
            tempMsglist[i] = txtTemplateContent.Text.Trim();

            if (txtTempId1.Text.Trim() != "" && txtTempName1.Text.Trim() != "" && txtTemplateContent1.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId1.Text.Trim();
                tempNamelist[i] = txtTempName1.Text.Trim();
                tempMsglist[i] = txtTemplateContent1.Text.Trim();

            }

            if (txtTempId2.Text.Trim() != "" && txtTempName2.Text.Trim() != "" && txtTemplateContent2.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId2.Text.Trim();
                tempNamelist[i] = txtTempName2.Text.Trim();
                tempMsglist[i] = txtTemplateContent2.Text.Trim();
            }

            if (txtTempId3.Text.Trim() != "" && txtTempName3.Text.Trim() != "" && txtTemplateContent3.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId3.Text.Trim();
                tempNamelist[i] = txtTempName3.Text.Trim();
                tempMsglist[i] = txtTemplateContent3.Text.Trim();
            }


            if (txtTempId4.Text.Trim() != "" && txtTempName4.Text.Trim() != "" && txtTemplateContent4.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId4.Text.Trim();
                tempNamelist[i] = txtTempName4.Text.Trim();
                tempMsglist[i] = txtTemplateContent4.Text.Trim();
            }


            if (txtTempId5.Text.Trim() != "" && txtTempName5.Text.Trim() != "" && txtTemplateContent5.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId5.Text.Trim();
                tempNamelist[i] = txtTempName5.Text.Trim();
                tempMsglist[i] = txtTemplateContent5.Text.Trim();
            }

            if (txtTempId6.Text.Trim() != "" && txtTempName6.Text.Trim() != "" && txtTemplateContent6.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId6.Text.Trim();
                tempNamelist[i] = txtTempName6.Text.Trim();
                tempMsglist[i] = txtTemplateContent6.Text.Trim();
            }

            if (txtTempId7.Text.Trim() != "" && txtTempName7.Text.Trim() != "" && txtTemplateContent7.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId7.Text.Trim();
                tempNamelist[i] = txtTempName7.Text.Trim();
                tempMsglist[i] = txtTemplateContent7.Text.Trim();
            }
            if (txtTempId8.Text.Trim() != "" && txtTempName8.Text.Trim() != "" && txtTemplateContent8.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId8.Text.Trim();
                tempNamelist[i] = txtTempName8.Text.Trim();
                tempMsglist[i] = txtTemplateContent8.Text.Trim();
            }

            if (txtTempId9.Text.Trim() != "" && txtTempName9.Text.Trim() != "" && txtTemplateContent9.Text.Trim() != "")
            {
                i = i + 1;
                tempIdlist[i] = txtTempId9.Text.Trim();
                tempNamelist[i] = txtTempName9.Text.Trim();
                tempMsglist[i] = txtTemplateContent9.Text.Trim();
            }












            if (rblType.SelectedValue == "API")
            {



                //New code Beg
                /*string senderId = dr.Field<string>(3);
                string tempMsg = dr.Field<string>(2);
                string tempWord = dr.Field<string>(4);
                string tempName = dr.Field<string>(1);
                string tempId = dr.Field<string>(0);

                txtSenderId.Text = senderId;
                txtTemplateContent.Text = tempMsg;
                txtTempWord.Text = tempWord;
                txtTempName.Text = tempName;
                txtTempId.Text = tempId;
                //New code End
                */
                List<string> mySenderId = new List<string>();
                if (txtuserid1.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid1.Text.Trim());
                }
                if (txtuserid2.Text != "" && txtuserid2.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid2.Text.Trim());
                }
                if (txtuserid3.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid3.Text.Trim());
                }
                if (txtuserid4.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid4.Text.Trim());
                }
                if (txtuserid5.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid5.Text.Trim());
                }
                if (txtuserid6.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid6.Text.Trim());
                }
                if (txtuserid7.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid7.Text.Trim());
                }
                if (txtuserid8.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid8.Text.Trim());
                }
                if (txtuserid9.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid9.Text.Trim());
                }
                if (txtuserid10.Text != "" && txtuserid1.Text.Length <= 6)
                {
                    mySenderId.Add(txtuserid10.Text.Trim());
                }
                for (int k = 0; k < tempIdlist.Count(); k++)
                {
                    string senderId = txtSenderId.Text.Trim();
                    string tempMsg = tempMsglist[k];
                    string tempWord = "";
                    string tempName = tempNamelist[k];
                    string tempId = tempIdlist[k];
                    if (tempIdlist[k] == null)
                    {
                    //    ClearControl();
                        return;
                    }
                    txtTempId.Text = tempIdlist[k];
                    txtTempName.Text = tempNamelist[k];
                    txtTemplateContent.Text = tempMsglist[k];


                    if (ValidateTemplate())
                    {

                        if (mySenderId.Count > 0)
                        {
                            for (int r = 0; r < mySenderId.Count; r++)
                            {
                                string SenderId = mySenderId[r].ToString().Trim();
                                string AllSender = "";
                                if (SenderId == "HYNDAI")
                                {
                                    AllSender = "HMISVR";
                                }
                                else if (SenderId == "HMISVR")
                                {
                                    AllSender = "HMISVR";
                                }
                                ob.SaveTemplateInTemplateId(mySenderId[r], tempMsg, tempWord, tempName, tempId, AllSender);
                                //ob.SaveTemplateInTemplateId(mySenderId[i], tempMsg, tempWord, tempName, tempId, AllSender);
                            }
                        }


                        string SenderId2 = senderId.ToString().Trim();
                        string AllSender2 = "";
                        if (SenderId2 == "HYNDAI")
                        {
                            AllSender2 = "HMISVR";
                        }
                        else if (SenderId2 == "HMISVR")
                        {
                            AllSender2 = "HMISVR";
                        }
                        if (senderId.Length <= 6)
                            ob.SaveTemplateInTemplateId(senderId, tempMsg, tempWord, tempName, tempId, AllSender2);
                        //ob.SaveTemplateInTemplateId(senderId, tempMsg, tempWord, tempName, tempId, AllSender2);
                        // ClearControl();
                        //  Clear();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Request generated successfully. for API');window.location ='AddTemplateAdmin.aspx';", true);
                    }
                }
            }
            else
            {
                List<string> myList = new List<string>();
                if (txtUser.Text != "")
                {

                    myList.Add(txtUser.Text.Trim());
                }


                if (txtuserid1.Text != "")
                {
                    myList.Add(txtuserid1.Text.Trim());
                }
                if (txtuserid2.Text != "")
                {
                    myList.Add(txtuserid2.Text.Trim());
                }
                if (txtuserid3.Text != "")
                {
                    myList.Add(txtuserid3.Text.Trim());
                }
                if (txtuserid4.Text != "")
                {
                    myList.Add(txtuserid4.Text.Trim());
                }
                if (txtuserid5.Text != "")
                {
                    myList.Add(txtuserid5.Text.Trim());
                }
                if (txtuserid6.Text != "")
                {
                    myList.Add(txtuserid6.Text.Trim());
                }
                if (txtuserid7.Text != "")
                {
                    myList.Add(txtuserid7.Text.Trim());
                }
                if (txtuserid8.Text != "")
                {
                    myList.Add(txtuserid8.Text.Trim());
                }
                if (txtuserid9.Text != "")
                {
                    myList.Add(txtuserid9.Text.Trim());
                }
                if (txtuserid10.Text != "")
                {
                    myList.Add(txtuserid10.Text.Trim());
                }


                for (int c = 0; c < tempIdlist.Length; c++)
                {
                    if (tempIdlist[c] == null)
                    {
                       // ClearControl();
                        return;
                    }
                    txtTempId.Text = tempIdlist[c];
                    txtTempName.Text = tempNamelist[c];
                    txtTemplateContent.Text = tempMsglist[c];

                    if (ValidateTemplate())
                    {
                        string _user = txtUser.Text.Trim();

                        string fileName = "TemplateRequest.txt";

                        string tempName = tempNamelist[c];
                        string msg = tempMsglist[c];
                        string tempId = tempIdlist[c];

                        if (myList.Count > 0)
                        {
                            for (int v = 0; v < myList.Count; v++)
                            {

                                ob.SaveTemplateRequest(myList[v].Trim(), msg, fileName, tempName, tempId, true);
                            }
                        }
                    }
                    

                }
                /*
                if (txtuserid1.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid1.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid2.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid2.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid3.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid3.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid4.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid4.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid5.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid5.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid6.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid6.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid7.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid7.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid8.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid8.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid9.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid9.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                if (txtuserid10.Text != "")
                {
                    ob.SaveTemplateRequest(txtuserid10.Text.Trim(), msg, fileName, tempName, tempId, true);
                }
                */
                //grdtemp.DataSource = null;
                //ob.SaveTemplateRequest(_user, msg, fileName, tempName, tempId, true);
                //ClearControl();
                //Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template Request generated successfully.');window.location ='AddTemplateAdmin.aspx';", true);

                BindData();
            }
            //ClearControl();

        }
                
            
    
        private void ClearControl()
        {
            
                    txtTempId.Text = "";
                    txtTempName.Text = "";
                    txtTemplateContent.Text = "";
              
                    txtTempId1.Text = "";
                    txtTempName1.Text = "";
                    txtTemplateContent1.Text = "";
               
                    txtTempId2.Text = "";
                    txtTempName2.Text = "";
                    txtTemplateContent2.Text = "";
             
                    txtTempId3.Text = "";
                    txtTempName3.Text = "";
                    txtTemplateContent3.Text = "";
             
                    txtTempId4.Text = "";
                    txtTempName4.Text = "";
                    txtTemplateContent4.Text = "";
               
                    txtTempId5.Text = "";
                    txtTempName5.Text = "";
                    txtTemplateContent5.Text = "";
                
                    txtTempId6.Text = "";
                    txtTempName6.Text = "";
                    txtTemplateContent6.Text = "";
                
                    txtTempId7.Text = "";
                    txtTempName7.Text = "";
                    txtTemplateContent7.Text = "";
               
                    txtTempId8.Text = "";
                    txtTempName8.Text = "";
                    txtTemplateContent8.Text = "";
               
                    txtTempId9.Text = "";
                    txtTempName9.Text = "";
                    txtTemplateContent9.Text = "";


                    txtuserid1.Text = "";
                    txtuserid2.Text = "";
                    txtuserid3.Text = "";
                    txtuserid4.Text = "";
                    txtuserid5.Text = "";
                    txtuserid6.Text = "";
                    txtuserid7.Text = "";
                    txtuserid8.Text = "";
                    txtuserid9.Text = "";
                    txtuserid10.Text = "";






        }
        

        private void BindData()
        {
            grvTemplate.DataSource = null;
            if (rblType.SelectedValue == "API")
            {
                ViewState["SenderId"] = txtSenderId.Text.Trim();
                DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
            else
            {
                ViewState["_userName"] = txtUser.Text;
                DataTable dt = ob.GetTemplateList(txtUser.Text);
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
        }

        protected void lnkShow_Click(object sender, EventArgs e)
        {
            if(usertype != "SYSADMIN")
            {
                if(ob.GetDLTofUser(txtUser.Text).ToUpper()!=Convert.ToString(Session["DLT"]).ToUpper())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Valid User Id');", true);
                    return;
                }
            }
            BindData();
            filtershow.Visible = true;
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "API")
            {
                lblusr1.Text = "Sender Id";
                lblusr2.Text = "Sender Id";
                lblusr3.Text = "Sender Id";
                lblusr4.Text = "Sender Id";
                lblusr5.Text = "Sender Id";
                lblusr6.Text = "Sender Id";
                lblusr7.Text = "Sender Id";
                lblusr8.Text = "Sender Id";
                lblusr9.Text = "Sender Id";
                lblusr10.Text = "Sender Id";
            }

            if (rblType.SelectedValue == "PANEL")
            {
                lblusr1.Text = "User Id";
                lblusr2.Text = "User Id";
                lblusr3.Text = "User Id";
                lblusr4.Text = "User Id";
                lblusr5.Text = "User Id";
                lblusr6.Text = "User Id";
                lblusr7.Text = "User Id";
                lblusr8.Text = "User Id";
                lblusr9.Text = "User Id";
                lblusr10.Text = "User Id";
            }
            HideDiv();
            grvTemplate.DataSource = null;
            grvTemplate.DataBind();
        }

        protected void grvTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTemplate.PageIndex = e.NewPageIndex;
            grvTemplate.DataSource = null;
            if (rblType.SelectedValue == "API")
            {

                grvTemplate.DataSource = ob.GetTemplateListOfAPI(Convert.ToString(ViewState["SenderId"]));
                grvTemplate.DataBind();
            }
            else
            {
                grvTemplate.DataSource = ob.GetTemplateList(Convert.ToString(ViewState["_userName"]));
                grvTemplate.DataBind();
            }

        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;

            GridViewRow gvro = (GridViewRow)btn.NamingContainer;

            Label lblTemplateId = (Label)gvro.FindControl("lblTemplateId");

            if (rblType.SelectedValue == "API")
            {
                string senderId = Convert.ToString(ViewState["SenderId"]);
                ob.DeleteTemplateInTemplateId(senderId, lblTemplateId.Text);
                //Request
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template deleted successfully. for API');", true);

                grvTemplate.DataSource = ob.GetTemplateListOfAPI(Convert.ToString(ViewState["SenderId"]));
                grvTemplate.DataBind();
            }
            else
            {
                string userName = Convert.ToString(ViewState["_userName"]);
                ob.DeleteTemplateInRequest(userName, lblTemplateId.Text);
                //Request
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template deleted successfully.');", true);
                grvTemplate.DataSource = ob.GetTemplateList(Convert.ToString(ViewState["_userName"]));
                grvTemplate.DataBind();
            }
        }

        private void BindSenderId(string userId)
        {
            DataTable dt = ob.GetSenderId(userId);
            //ddlLinkrSender.Items.Clear();
            ddlLinkrSender.DataSource = dt;
            ddlLinkrSender.DataTextField = "senderid";
            ddlLinkrSender.DataValueField = "senderid";
            ddlLinkrSender.DataBind();
            //ListItem objListItem = new ListItem("--Select--", "0");
            //ddlLinkrSender.Items.Insert(0, objListItem);
            //if (dt.Rows.Count == 1)
            //    ddlLinkrSender.SelectedIndex = 1;
            //else
            //    ddlLinkrSender.SelectedIndex = 0;
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "API")
            {
                if (txtTestUser.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Username');", true);
                    return;
                }
                DataTable dt = ob.GetUserParameter(txtTestUser.Text);
                string password = Convert.ToString(dt.Rows[0]["APIKEY"]);

                string message = txtTestMessage.Text;
                message = message.Replace("&", "%26").Replace("+", "%2B");

                string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + txtTestUser.Text + "&pwd=" + password + "&mobile=" + txtTestMobile.Text + "&sender=" + txtTestSender.Text + "&msg=" + message + "&msgtype=13";
                string getResponseTxt = "";
                string getStatus = "";
                WinHttp.WinHttpRequest objWinRq;
                objWinRq = new WinHttp.WinHttpRequest();
                try
                {
                    objWinRq.Open("GET", url, false);
                    objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
                    objWinRq.Send(null);

                    while (!(getStatus != "" && getResponseTxt != ""))
                    {
                        getStatus = objWinRq.Status + objWinRq.StatusText;
                        getResponseTxt = objWinRq.ResponseText;
                    }
                    getResponseTxt = "[" + getResponseTxt + "]";
                }
                catch (Exception EX)
                {
                    throw EX;
                }

                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                System.Threading.Thread.Sleep(2000);
                int submitted = ob.GetCountForSubmittedTemplate(txtTestUser.Text, txtSenderId.Text.Trim(), currentTime);
                int msgLength = message.Length;

                lblTotalMessage.Text = "Total Message : " + submitted.ToString();
                lblMessageLength.Text = "Message Length :" + msgLength.ToString() + " character";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Message Send successfully. for API');", true);
                pnlPopUp_NUMBER_ModalPopupExtender.Show();
            }
        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            GridViewRow gvro = (GridViewRow)btn.NamingContainer;

            TextBox txtTemplate = (TextBox)gvro.FindControl("txtTemplate");

            lblTotalMessage.Text = "";
            lblMessageLength.Text = "";

            if (rblType.SelectedValue == "API")
            {
                txtTestMessage.Text = txtTemplate.Text;
                txtTestSender.Text = Convert.ToString(ViewState["SenderId"]);
                pnlPopUp_NUMBER_ModalPopupExtender.Show();
            }
            else
            {
                Label lblTemplateId = (Label)gvro.FindControl("lblTemplateId");
                ViewState["TemplateId"] = lblTemplateId.Text;
                txtLinkMessage.Text = txtTemplate.Text;
                string userName = Convert.ToString(ViewState["_userName"]);
                txtLinkUser.Text = userName;
                BindSenderId(userName);
                pnlPopUp_Linkext_ModalPopupExtender.Show();
            }

        }

        protected void btnLinkSend_Click(object sender, EventArgs e)
        {
            string userName = Convert.ToString(ViewState["_userName"]);
            DataTable dt = ob.GetUserParameter(userName);
            string smppAcountId = ob.GetTemplateTestAccounts();
            string profileId = userName;
            string msg = txtLinkMessage.Text;
            string mobile = txtLinkMobile.Text;
            string senderId = ddlLinkrSender.SelectedValue;
            string fileId = "108";
            string peid = Convert.ToString(dt.Rows[0]["peid"]);

            string templId = Convert.ToString(ViewState["TemplateId"]);

            string dataCode = "Default";
            string q = msg;
            if (q.Any(c => c > 126))
            {
                dataCode = "UCS2";
            }

            ob.InsertMsgTomsgtranForTemplateTest(smppAcountId, profileId, msg, mobile, senderId, fileId, peid, templId, dataCode);

        }


        //start

   




        //end

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (ViewState["Filter"] == null)
            {
                if (rblType.SelectedValue == "API")
                {
                    if (txtSenderId.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Sender Id');", true);
                        return;
                    }
                    DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                    //DatatableToExcel(dt);
                    //ExportToExcel2(dt);


                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("INSERTDATETIME");
                        Session["TemplateData"] = dt;
                        Session["FILENAME2"] = "TemplateReport.xls";
                        Response.Redirect("DownloadTemplate.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data Found.');", true);
                    }
                }
                else
                {
                    if (txtUser.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter User Name');", true);
                        return;
                    }

                    DataTable dt = ob.GetTemplateList(txtUser.Text);
                    //Session["MOBILEDATA"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        //dt.Columns.Remove("INSERTDATETIME");
                        //Session["FILENAME"] = "TemplateReport.xls";
                        //Response.Redirect("sms-reports_u_download.aspx");
                        dt.Columns.Remove("INSERTDATETIME");
                        Session["TemplateData"] = dt;
                        Session["FILENAME2"] = "TemplateReport.xls";
                        Response.Redirect("DownloadTemplate.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data Found.');", true);
                    }
                    //DatatableToExcel(dt);
                    //ExportToExcel2(dt);
                }
            }
            else
            {
                if (rblType.SelectedValue == "API")
                {
                    if (txtSenderId.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter Sender Id');", true);
                        return;
                    }
                    // DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                    string TempId = txtTemplateId.Text.ToString();
                    string TempName = txtTemplateName.Text.ToString();
                    string TemplateTxt = txtTemplate.Text.ToString();
                    DataTable dt = ob.filterTemplaterequest(TempId, TempName, TemplateTxt, "0", txtSenderId.Text.Trim(), "NA");
                    //DatatableToExcel(dt);
                    //ExportToExcel2(dt);


                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("INSERTDATETIME");
                        Session["TemplateData"] = dt;
                        Session["FILENAME2"] = "TemplateReport.xls";
                        Response.Redirect("DownloadTemplate.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data Found.');", true);
                    }
                }
                else
                {
                    if (txtUser.Text.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter User Name');", true);
                        return;
                    }

                    //DataTable dt = ob.GetTemplateList(txtUser.Text);
                    string TempId = txtTemplateId.Text.ToString();
                    string TempName = txtTemplateName.Text.ToString();
                    string TemplateTxt = txtTemplate.Text.ToString();
                    DataTable dt = ob.filterTemplaterequest(TempId, TempName, TemplateTxt, txtUser.Text, "0", "templaterequest");
                    //Session["MOBILEDATA"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        //dt.Columns.Remove("INSERTDATETIME");
                        //Session["FILENAME"] = "TemplateReport.xls";
                        //Response.Redirect("sms-reports_u_download.aspx");
                        dt.Columns.Remove("INSERTDATETIME");
                        Session["TemplateData"] = dt;
                        Session["FILENAME2"] = "TemplateReport.xls";
                        Response.Redirect("DownloadTemplate.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data Found.');", true);
                    }
                    //DatatableToExcel(dt);
                    //ExportToExcel2(dt);
                }
                ViewState["Filter"] = null;
            }
        }

        private void ExportToExcel()
        {
            string attachment = "attachment; filename=TemplateReport.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter oStringWriter = new StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            grvTemplate.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        private void ExportToExcel2(DataTable dt)
        {
            try
            {
                Response.Clear();
                Response.ClearHeaders(); //use by me
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + "TemplateReport.xls");
                Response.Charset = "";
                Response.ContentType = "application/text";
                //    Response.ContentEncoding = Encoding.UTF8;
                //Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                StringBuilder columnbind = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
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
                //Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                HttpContext.Current.Response.End();

            }
            catch (Exception ex1)
            {
                string str = ex1.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void DatatableToExcel(DataTable dt)
        {
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt, "Customers");

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=CampaignReport.xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            string attachment = "attachment; filename=TemplateReport.xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            string tab = "";

            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    string leadingChar = i == 0 ? "'" : "";
                    HttpContext.Current.Response.Write(tab + leadingChar + dr[i].ToString().Replace('\n', ' ').Replace(Convert.ToChar(10), ' ').Replace(Convert.ToChar(13), ' '));
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            // Response.End();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void GridFormat(DataTable dt)
        {
            grvTemplate.UseAccessibleHeader = true;
            grvTemplate.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (grvTemplate.TopPagerRow != null)
            {
                grvTemplate.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvTemplate.BottomPagerRow != null)
            {
                grvTemplate.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                grvTemplate.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void Clear()
        {
            txtuserid1.Text = "";
            txtuserid2.Text = "";
            txtuserid3.Text = "";
            txtuserid4.Text = "";
            txtuserid5.Text = "";
            txtuserid6.Text = "";
            txtuserid7.Text = "";
            txtuserid8.Text = "";
            txtuserid9.Text = "";
            txtuserid10.Text = "";
        }

        protected void chkHeader_CheckedChanged(object sender, EventArgs e)
        {
            int noOfDeleteTemplate = 0;
            string TempID = "";
            CheckBox chkHeader = grvTemplate.HeaderRow.FindControl("chkHeader") as CheckBox;
            foreach (GridViewRow gr in grvTemplate.Rows)
            {
                int indexRow = gr.RowIndex;
                CheckBox chkRowItem = grvTemplate.Rows[indexRow].Cells[1].FindControl("chkitem") as CheckBox;
                //Label templeteID= grvTemplate.Rows[indexRow].Cells[1].FindControl("lblTemplateId") as Label;

                if (chkHeader.Checked)
                {
                    chkRowItem.Checked = true;
                    //noOfDeleteTemplate= noOfDeleteTemplate + 1;
                    //TempID = TempID + templeteID+",";
                }
                else
                {
                    chkRowItem.Checked = false;
                    //noOfDeleteTemplate = 0;
                    //TempID = "";
                }
            }
            //TempID = TempID.TrimEnd(',');
            //Session["TempID"] = TempID;
            //Session["TotalTemplateIDdelete"] = noOfDeleteTemplate;
        }

        protected void multileDelete_Click(object sender, EventArgs e)
        {
            int noOfDeleteTemplate = 0;
            string TempID = "";
            foreach (GridViewRow gr in grvTemplate.Rows)
            {
                int rowindex = gr.RowIndex;
                CheckBox chkRowItem = grvTemplate.Rows[rowindex].Cells[1].FindControl("chkitem") as CheckBox;
                Label templeteID = grvTemplate.Rows[rowindex].Cells[1].FindControl("lblTemplateId") as Label;

                if (chkRowItem.Checked)
                {
                    noOfDeleteTemplate = noOfDeleteTemplate + 1;
                    TempID = TempID + templeteID.Text + ",";
                }
            }
            TempID = TempID.TrimEnd(',');
            Session["TempID"] = TempID;
            Session["TotalTemplateIDdelete"] = noOfDeleteTemplate;

            if ((Convert.ToString(Session["TotalTemplateIDdelete"]) == "0") && Convert.ToString(Session["TempID"]) == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Nothing Template Deleted')", true);
                return;
            }
            if (rblType.SelectedValue == "API")
            {
                string senderId = Convert.ToString(ViewState["SenderId"]);
                string[] templateID = Convert.ToString(Session["TempID"]).Split(',');
                for (int i = 0; i < templateID.Length; i++)
                {
                    ob.DeleteTemplateInTemplateId(senderId, templateID[i]);
                }

                //Request
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template deleted successfully. for API');", true);

                grvTemplate.DataSource = ob.GetTemplateListOfAPI(Convert.ToString(ViewState["SenderId"]));
                grvTemplate.DataBind();
            }
            else
            {
                string userName = Convert.ToString(ViewState["_userName"]);
                string[] templateID = Convert.ToString(Session["TempID"]).Split(',');
                for (int i = 0; i < templateID.Length; i++)
                {

                    ob.DeleteTemplateInRequest(userName, templateID[i]);
                }

                //ob.DeleteTemplateInRequest(userName, lblTemplateId.Text);Request
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Template deleted successfully.');", true);
                grvTemplate.DataSource = ob.GetTemplateList(Convert.ToString(ViewState["_userName"]));
                grvTemplate.DataBind();
            }
        }

        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            string TempId = txtTemplateId.Text.ToString();
            string TempName = txtTemplateName.Text.ToString();
            string TemplateTxt = txtTemplate.Text.ToString();
            grvTemplate.DataSource = null;
            if (rblType.SelectedValue == "API")
            {
                ViewState["SenderId"] = txtSenderId.Text.Trim();
                DataTable dt = ob.filterTemplaterequest(TempId, TempName, TemplateTxt, "0", txtSenderId.Text.Trim(), "NA");
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
            else
            {
                ViewState["_userName"] = txtUser.Text;
                DataTable dt = ob.filterTemplaterequest(TempId, TempName, TemplateTxt, txtUser.Text, "0", "templaterequest");
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
            ViewState["Filter"] = "Filter";
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            txtTemplateId.Text = "";
            txtTemplateName.Text = "";
            txtTemplate.Text = "";
            grvTemplate.DataSource = null;
            if (rblType.SelectedValue == "API")
            {
                ViewState["SenderId"] = txtSenderId.Text.Trim();
                DataTable dt = ob.GetTemplateListOfAPI(txtSenderId.Text.Trim());
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
            else
            {
                ViewState["_userName"] = txtUser.Text;
                DataTable dt = ob.GetTemplateList(txtUser.Text);
                grvTemplate.DataSource = dt;
                grvTemplate.DataBind();
                GridFormat(dt);
            }
        }

        protected void lnkDelete_Click1(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(sender as LinkButton).Parent.Parent;
            int rowsindex = gvr.RowIndex;
            

            DataTable dt = ViewState["TempDataRows"] as DataTable;
            dt.Rows[rowsindex].Delete();
            dt.AcceptChanges();
          //  grdtemp.DataSource = dt;
            //grdtemp.DataBind();
        }

     
    }
}