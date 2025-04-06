using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SubmitToTran
{
    public partial class Form1 : Form
    {
      
        private System.Timers.Timer timerPROCESS;
        private System.Timers.Timer timerEXCEP;
        string dbname = Convert.ToString(ConfigurationManager.AppSettings["dbname"]);
        Int64 NoOfRecordForTemplateCheck = 10;

        EMIM_Util emi = new EMIM_Util();
        Util obU = new Util();
        bool IsRunning = false;
        public void Initialize_Session()
        {
            obU.Info("Service started.");
            //create dynamic timer for each session. 
            try
            {
                NoOfRecordForTemplateCheck = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfRecordForTemplateCheck"]);

                timerPROCESS = new System.Timers.Timer();
                timerPROCESS.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESS_INTERVAL"]);
                this.timerPROCESS.Elapsed += new System.Timers.ElapsedEventHandler(this.timerPROCESS_Tick);
                timerPROCESS.Enabled = true;
                this.timerPROCESS.Start();

                //timerEXCEP = new System.Timers.Timer();
                //timerEXCEP.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PROCESS_INTERVAL_EXCEP"]);
                //this.timerEXCEP.Elapsed += new System.Timers.ElapsedEventHandler(this.timerEXCEP_Tick);
                //timerEXCEP.Enabled = true;
                //this.timerEXCEP.Start();
            }
            catch (Exception ex)
            {
                throw;
            }

            
        }

        private void timerPROCESS_Tick(object sender, ElapsedEventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;                
                Processsending();
                IsRunning = false;
            }
        }

        public void Processsending()
        {
            try
            {
                DataTable dt = obU.GetFileRecords();
                if (dt.Rows.Count>0)
                {
                    obU._Log("Start FileProcess : "+dt.Rows[0]["id"].ToString());
                    Send(dt);
                    obU._Log("End FileProcess : " + dt.Rows[0]["id"].ToString());
                }
            }
            catch (Exception ex)
            {
                obU._Log("Error Processsending : " + ex);
            }
        }

        public void Send(DataTable dt)
        {
            try
            {
                forwardToTran(dt); 
            }
            catch (Exception ex)
            {
                obU._Log("Error Send : " + ex + "Datatable" + dt);
            }
        }

        public Form1()
        {
            InitializeComponent();
            this.Text = Convert.ToString(ConfigurationManager.AppSettings["FORMCAPTION"]);
        }

        public void forwardToTran(DataTable tbl)
        {
            try
            {
                DataRow dr = tbl.Rows[0];

                if (dr["methodname"].ToString()== "Insert_SMS_BULK_4url")
                {
                    Insert_SMS_BULK_4url(dr); 
                }
                else if (dr["methodname"].ToString() == "InsertTemplateSMSrecordsFromUSERTMP")
                {
                    InsertTemplateSMSrecordsFromUSERTMP(dr);
                }
                else if (dr["methodname"].ToString() == "InsertSMSrecordsFromUSERTMP")
                {
                    InsertSMSrecordsFromUSERTMP(dr);
                }
                else if (dr["methodname"].ToString() == "Schedule_SMS_BULK")
                {
                    Schedule_SMS_BULK(dr);
                }
            }
            catch (Exception ex)
            {
                obU._Log("Error forwardToTran : " + ex + "DataTable" + tbl);
            }
        }

        

        public void Schedule_SMS_BULK(DataRow dr)
        {
            try
            {
                //string dltno = Convert.ToString(database.GetScalarValue("select dltno from customer where username='" + dr["ProfileId"].ToString() + "'"));

                //DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNT(dltno, dr["ProfileId"].ToString());
                DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNTCountry(Convert.ToString(dr["ProfileId"]), Convert.ToString(dr["ccode"]));

                DataTable dtSMPPAC = new DataTable();

                string smstype = dr["smstype"].ToString();

                if (smstype == "6")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtCm = emi.GetPromotionAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                }  // for promo
                else if (smstype == "3")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "Y";
                    DataTable dtCm = emi.GetCampaignAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for camp
                else if (smstype == "7") // for google rcs
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetGoogleRCSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for rcs
                else if (smstype == "8") // for flash 
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");

                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetFlashSMSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);

                }
                else // for file  
                {
                    dtSMPPAC = DTSMPPAC1;
                }
                List<string> MobList = new List<string>();
                Global.Istemplatetest = true;
                //string IsScratchedAvail = "N";
                //if (Convert.ToBoolean(dr["scratchcard"]) == true)
                //{
                //    IsScratchedAvail = "Y";
                //}
                emi.Schedule_SMS_BULK(dr, dr["profileid"].ToString(), dr["msg"].ToString(), dr["sender"].ToString(), Convert.ToDateTime(dr["scheduletime"]).ToString("yyyy-MM-dd HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture), int.Parse(dr["shorturlid"].ToString()), dr["shorturl"].ToString(), dr["domainname"].ToString(),
                    Convert.ToDouble(dr["rate"].ToString()), dr["smstype"].ToString(), dr["fileName"].ToString(), dr["Fileext"].ToString(), dtSMPPAC, dr["campname"].ToString(), bool.Parse(dr["ucs2"].ToString()), MobList, "Bulk",
                    dr["templateid"].ToString(), dr["ccode"].ToString(), double.Parse(dr["prevbalance"].ToString()), double.Parse(dr["availablebalance"].ToString()), dr["fileName"].ToString(), dr["tblname"].ToString(), dbname, int.Parse(dr["id"].ToString()), long.Parse(dr["noofrecord"].ToString()), Convert.ToBoolean(dr["scratchcard"]));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void InsertTemplateSMSrecordsFromUSERTMP(DataRow dr)
        {
            try
            {
                //string dltno = Convert.ToString(database.GetScalarValue("select dltno from customer where username='"+ dr["ProfileId"].ToString() + "'"));

                //DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNT(dltno, dr["ProfileId"].ToString());

                DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNTCountry(Convert.ToString(dr["ProfileId"]), Convert.ToString(dr["ccode"]));
                DataTable dtSMPPAC = new DataTable();

                string smstype = dr["smstype"].ToString();

                if (smstype == "6")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtCm = emi.GetPromotionAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                }  // for promo
                else if (smstype == "3")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "Y";
                    DataTable dtCm = emi.GetCampaignAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for camp
                else if (smstype == "7") // for google rcs
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetGoogleRCSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for rcs
                else if (smstype == "8") // for flash 
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");

                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetFlashSMSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);

                }
                else // for file  
                {
                    dtSMPPAC = DTSMPPAC1;
                }

                List<string> MobList = new List<string>();
                List<string> templateflds = new List<string>();

                string[] arr;

                string sqgetmappedtemp = "select m.mapfieldname from  " + dbname + ".dbo.FileProcess F with (nolock) JOIN " + dbname + ".dbo.mapfields m with (nolock) on m.id=f.id where tblname='" + dr["tblname"].ToString() + "'" ;
                DataTable dtmappedFields = database.GetDataTable(sqgetmappedtemp);

                //ListBox LstMappedField = new ListBox();

                List<string> LstMappedField = new List<string>();


                foreach (DataRow item in dtmappedFields.Rows)
                {
                    LstMappedField.Add(item["mapfieldname"].ToString());
                    arr = item["mapfieldname"].ToString().Replace("---->>",";").Split(';');
                    templateflds.Add(arr[0]);
                }

                DataTable dtCols = new DataTable();

                Global.Istemplatetest = true;
                //if ((Convert.ToString(dr["ccode"]).Trim() == "91") && long.Parse(dr["noofrecord"].ToString()) > NoOfRecordForTemplateCheck) // Test template before sending
                //{
                //    string peid = Convert.ToString(database.GetScalarValue("select peid from customer with (nolock) where username='" + dr["profileid"].ToString() + "'"));
                //    Global.Istemplatetest = emi.TestSmsbeforeSend(Convert.ToString(dr["profileid"]), Convert.ToString(dr["templateid"]), Convert.ToString(dr["msg"]), Convert.ToString(dr["sender"]), peid);
                //}


                emi.InsertTemplateSMSrecordsFromUSERTMP(dr, dr["profileid"].ToString(), dr["sender"].ToString(), dr["smstype"].ToString(), dr["msg"].ToString(), dr["fileName"].ToString(), dr["Fileext"].ToString(), dtSMPPAC, dtCols, LstMappedField, dr["campname"].ToString()
                    ,bool.Parse(dr["ucs2"].ToString()), Convert.ToDouble(dr["rate"].ToString()), int.Parse(dr["noofsms"].ToString()), dr["scheduletime"].ToString(), templateflds, dr["templateid"].ToString(), dr["ccode"].ToString(),
                     dr["shorturl"].ToString(), dr["domainname"].ToString(), int.Parse(dr["shorturlid"].ToString()), double.Parse(dr["prevbalance"].ToString()), double.Parse(dr["availablebalance"].ToString()), dr["fileName"].ToString(), 
                     dr["tblname"].ToString(), dbname,int.Parse(dr["id"].ToString()), long.Parse(dr["noofrecord"].ToString()));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void InsertSMSrecordsFromUSERTMP(DataRow dr)
        {
            try
            {
                //string dltno = Convert.ToString(database.GetScalarValue("select dltno from customer where username='" + dr["ProfileId"].ToString() + "'"));

                //DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNT(dltno, dr["ProfileId"].ToString());

                DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNTCountry(Convert.ToString(dr["ProfileId"]), Convert.ToString(dr["ccode"]));


                DataTable dtSMPPAC = new DataTable();

                string smstype = dr["smstype"].ToString();

                if (smstype == "6")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtCm = emi.GetPromotionAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                }  // for promo
                else if (smstype == "3")
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "Y";
                    DataTable dtCm = emi.GetCampaignAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtCm.Rows.Count; j++)
                        smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for camp
                else if (smstype == "7") // for google rcs
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");
                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetGoogleRCSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);
                } // for rcs
                else if (smstype == "8") // for flash 
                {
                    dtSMPPAC.Columns.Add("DLTNO");
                    dtSMPPAC.Columns.Add("GSM");
                    dtSMPPAC.Columns.Add("smppaccountid");
                    dtSMPPAC.Columns.Add("smppaccountidall");

                    DataRow drn = dtSMPPAC.NewRow();
                    drn["DLTNO"] = "";
                    drn["GSM"] = "N";
                    DataTable dtRCS = emi.GetFlashSMSAccounts();
                    string smppac = "'0'";
                    for (int j = 0; j < dtRCS.Rows.Count; j++)
                        smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                    drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                    drn["smppaccountidall"] = smppac;
                    dtSMPPAC.Rows.Add(drn);

                }
                else // for file  
                {
                    dtSMPPAC = DTSMPPAC1;

                }


                List<string> MobList = new List<string>();
                List<string> templateflds = new List<string>();

                string[] arr;

                string sqgetmappedtemp = "select m.mapfieldname from  " + dbname + ".dbo.FileProcess F with (nolock) JOIN " + dbname + ".dbo.mapfields m with (nolock) on m.id=f.id where tblname='" + dr["tblname"].ToString() + "'";
                DataTable dtmappedFields = database.GetDataTable(sqgetmappedtemp);

                ListBox LstMappedField = new ListBox();

                foreach (DataRow item in dtmappedFields.Rows)
                {
                    LstMappedField.Items.Add(item["mapfieldname"].ToString());
                    arr = item["mapfieldname"].ToString().Replace("---->>", ";").Split(';');
                    templateflds.Add(arr[0]);
                }

                DataTable dtCols = new DataTable();
                
                Global.Istemplatetest = true;
                //if ((Convert.ToString(dr["ccode"]).Trim() == "91") && long.Parse(dr["noofrecord"].ToString()) > NoOfRecordForTemplateCheck) // Test template before sending
                //{
                //    string peid = Convert.ToString(database.GetScalarValue("select peid from customer with (nolock) where username='" + dr["profileid"].ToString() + "'"));
                //    Global.Istemplatetest = emi.TestSmsbeforeSend(Convert.ToString(dr["profileid"]), Convert.ToString(dr["templateid"]), Convert.ToString(dr["msg"]), Convert.ToString(dr["sender"]), peid);
                //}

                emi.InsertSMSrecordsFromUSERTMP(dr, dr["profileid"].ToString(), dr["sender"].ToString(), dr["smstype"].ToString(), dr["msg"].ToString(), dr["fileName"].ToString(), dr["Fileext"].ToString(),
                    dtSMPPAC, dr["campname"].ToString(), bool.Parse(dr["ucs2"].ToString()), int.Parse(dr["noofsms"].ToString()), double.Parse(dr["Rate"].ToString()), MobList, "Bilk", "", dr["templateid"].ToString(), dr["ccode"].ToString(),
                      double.Parse(dr["prevbalance"].ToString()), double.Parse(dr["availablebalance"].ToString()), dr["fileName"].ToString(), dr["tblname"].ToString(), dbname, int.Parse(dr["id"].ToString()),long.Parse(dr["noofrecord"].ToString()));

            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public void Insert_SMS_BULK_4url(DataRow dr)
        {
            DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNTCountry(Convert.ToString(dr["ProfileId"]), Convert.ToString(dr["ccode"]));

            //string dltno = Convert.ToString(database.GetScalarValue("select dltno from customer where username='" + dr["ProfileId"].ToString() + "'"));

            //DataTable DTSMPPAC1 = emi.GetUserSMPPACCOUNT(dltno, dr["ProfileId"].ToString());

            DataTable dtSMPPAC = new DataTable();

            string smstype = dr["smstype"].ToString();

            if (smstype == "6") 
            {
                dtSMPPAC.Columns.Add("DLTNO");
                dtSMPPAC.Columns.Add("GSM");
                dtSMPPAC.Columns.Add("smppaccountid");
                dtSMPPAC.Columns.Add("smppaccountidall");
                DataRow drn = dtSMPPAC.NewRow();
                drn["DLTNO"] = "";
                drn["GSM"] = "N";
                DataTable dtCm = emi.GetPromotionAccounts();
                string smppac = "'0'";
                for (int j = 0; j < dtCm.Rows.Count; j++)
                    smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                drn["smppaccountidall"] = smppac;
                dtSMPPAC.Rows.Add(drn);
            }  // for promo
            else if (smstype == "3")
            {
                dtSMPPAC.Columns.Add("DLTNO");
                dtSMPPAC.Columns.Add("GSM");
                dtSMPPAC.Columns.Add("smppaccountid");
                dtSMPPAC.Columns.Add("smppaccountidall");
                DataRow drn = dtSMPPAC.NewRow();
                drn["DLTNO"] = "";
                drn["GSM"] = "Y";
                DataTable dtCm = emi.GetCampaignAccounts();
                string smppac = "'0'";
                for (int j = 0; j < dtCm.Rows.Count; j++)
                    smppac = smppac + ",'" + dtCm.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                drn["smppaccountid"] = dtCm.Rows[0]["SMPPACCOUNTID"].ToString();
                drn["smppaccountidall"] = smppac;
                dtSMPPAC.Rows.Add(drn);
            } // for camp
            else if (smstype == "7") // for google rcs
            {
                dtSMPPAC.Columns.Add("DLTNO");
                dtSMPPAC.Columns.Add("GSM");
                dtSMPPAC.Columns.Add("smppaccountid");
                dtSMPPAC.Columns.Add("smppaccountidall");
                DataRow drn = dtSMPPAC.NewRow();
                drn["DLTNO"] = "";
                drn["GSM"] = "N";
                DataTable dtRCS = emi.GetGoogleRCSAccounts();
                string smppac = "'0'";
                for (int j = 0; j < dtRCS.Rows.Count; j++)
                    smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                drn["smppaccountidall"] = smppac;
                dtSMPPAC.Rows.Add(drn);
            } // for rcs
            else if (smstype == "8") // for flash 
            {
                dtSMPPAC.Columns.Add("DLTNO");
                dtSMPPAC.Columns.Add("GSM");
                dtSMPPAC.Columns.Add("smppaccountid");
                dtSMPPAC.Columns.Add("smppaccountidall");

                DataRow drn = dtSMPPAC.NewRow();
                drn["DLTNO"] = "";
                drn["GSM"] = "N";
                DataTable dtRCS = emi.GetFlashSMSAccounts();
                string smppac = "'0'";
                for (int j = 0; j < dtRCS.Rows.Count; j++)
                    smppac = smppac + ",'" + dtRCS.Rows[j]["SMPPACCOUNTID"].ToString() + "'";
                drn["smppaccountid"] = dtRCS.Rows[0]["SMPPACCOUNTID"].ToString();
                drn["smppaccountidall"] = smppac;
                dtSMPPAC.Rows.Add(drn);

            }
            else // for file  
            {
                dtSMPPAC = DTSMPPAC1;

            }


            List<string> MobList = new List<string>();
            List<string> templateflds = new List<string>();

            string[] arr;

            string sqgetmappedtemp = "select m.mapfieldname from  " + dbname + ".dbo.FileProcess F with (nolock) JOIN " + dbname + ".dbo.mapfields m with (nolock) on m.id=f.id where tblname='" + dr["tblname"].ToString() + "'";
            DataTable dtmappedFields = database.GetDataTable(sqgetmappedtemp);

            ListBox LstMappedField = new ListBox();

            foreach (DataRow item in dtmappedFields.Rows)
            {
                LstMappedField.Items.Add(item["mapfieldname"].ToString());
                arr = item["mapfieldname"].ToString().Replace("---->>", ";").Split(';');
                templateflds.Add(arr[0]);
            }

            DataTable dtCols = new DataTable();

            Global.Istemplatetest = true;

            //if ((Convert.ToString(dr["ccode"]).Trim() == "91") && long.Parse(dr["noofrecord"].ToString()) > NoOfRecordForTemplateCheck) // Test template before sending
            //{
            //    string peid = Convert.ToString(database.GetScalarValue("select peid from customer with (nolock) where username='" + dr["profileid"].ToString() + "'"));
            //    Global.Istemplatetest = emi.TestSmsbeforeSend(Convert.ToString(dr["profileid"]), Convert.ToString(dr["templateid"]), Convert.ToString(dr["msg"]), Convert.ToString(dr["sender"]), peid);
            //}

            //string IsScratchedAvail = "N";
            //if (Convert.ToBoolean(dr["scratchcard"]) == true)
            //{
            //    IsScratchedAvail = "Y";
            //}

            emi.Insert_SMS_BULK_4url(dr, dr["profileid"].ToString(), dr["msg"].ToString(), dr["sender"].ToString(), dr["scheduletime"].ToString(), int.Parse(dr["shorturlid"].ToString()), dr["shorturl"].ToString(), dr["domainname"].ToString(),
               Convert.ToDouble(dr["rate"].ToString()), dr["smstype"].ToString(), dr["fileName"].ToString(), dr["Fileext"].ToString(), dtSMPPAC,dr["campname"].ToString(), bool.Parse(dr["ucs2"].ToString()), int.Parse(dr["noofsms"].ToString())
             ,MobList,"","", dr["templateid"].ToString(), dr["ccode"].ToString(),double.Parse(dr["prevbalance"].ToString()), double.Parse(dr["availablebalance"].ToString()), dr["fileName"].ToString(), dr["tblname"].ToString(), dbname, int.Parse(dr["id"].ToString()), long.Parse(dr["noofrecord"].ToString()), Convert.ToBoolean(dr["scratchcard"]));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //DataTable dt = new DataTable();
            //dt = database.GetDataTable("SELECT  * FROM FILEPROCESS where tblname='MIM2301085_20231025163214'");
            //DataRow dr = dt.Rows[0];

            //InsertTemplateSMSrecordsFromUSERTMP(dr);

            Initialize_Session();
            //Processsending();

            //DataTable dt = database.GetDataTable("SELECT * FROM FILEPROCESS where tblname='MIM2101277_20231211112027'");  //1,2,3,4,5     
            //if (dt.Rows.Count > 0)
            //{
            //    //InsertHEROdata(dt.Rows[0]);
            //    //InsertSMSrecordsFromUSERTMP(dt.Rows[0]);
            //    //Schedule_SMS_BULK(dt.Rows[0]);
            //    Insert_SMS_BULK_4url(dt.Rows[0]);
            //    //InsertTemplateSMSrecordsFromUSERTMP(dt.Rows[0]);
            //}
        }
    }
}
