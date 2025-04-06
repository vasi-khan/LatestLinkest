using MIMPwdUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ParkPlusTemplateAddition.Helper
{
    public class Util
    {
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        MIMUtil MIM = new MIMUtil();
        public string InsertTempAPI(string TempId, string TempName, string MsgText, string SenderId, string TempWords, string userid, string CountryCode)
        {
            string Msg = "";

            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertTemplateAdditionAPI";
                    cmd.Parameters.AddWithValue("@TempId", TempId);
                    cmd.Parameters.AddWithValue("@TempName", TempName);
                    cmd.Parameters.AddWithValue("@MsgText", MsgText);
                    cmd.Parameters.AddWithValue("@SenderId", SenderId);
                    cmd.Parameters.AddWithValue("@TempWords", TempWords);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    cmd.ExecuteNonQuery();
                    Msg = "API";
                }
                catch (Exception ex)
                {
                    Log("InsertTempAPI : " + ex);
                }
            }

            return Msg;
        }

        public string InsertTempPanel(string TempId, string TempName, string MsgText, string SenderId, string UserID)
        {
            string Msg = "";

            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertTemplateAdditionPANEL";
                    cmd.Parameters.AddWithValue("@TempId", TempId);
                    cmd.Parameters.AddWithValue("@TempName", TempName);
                    cmd.Parameters.AddWithValue("@MsgText", MsgText);
                    cmd.Parameters.AddWithValue("@SenderId", SenderId);
                    cmd.Parameters.AddWithValue("@UserId", UserID);
                    cmd.ExecuteNonQuery();
                    Msg = "PANEL";
                }
                catch (Exception ex)
                {
                    Log("InsertTempPanel : " + ex);
                }
            }

            return Msg;
        }

        public DataSet ValidateTemplateRequest(string tempId, string templateName, string user)
        {
            string sql = string.Format("Select Count(templateid) [TemplateId] from templaterequest where username='{0}' AND templateid='{1}' ;", user, tempId);
            string sql1 = string.Format("Select Count(tempname) [TemplateName] from templaterequest where username='{0}' AND tempname='{1}' ;", user, templateName);
            return database.GetDataSet(sql + sql1);
        }

        public DataTable ValidateTemplateIdforAPI(string tempId, string templateName, string senderId)
        {
            string sql = string.Format("Select Count(templateid) [TemplateId] from TemplateID where senderid='{0}' AND templateid='{1}' ;", senderId, tempId);
            return database.GetDataTable(sql);
        }

        public DataTable GetLoginDetails(string UserName, string Password)
        {
            DataTable dt = new DataTable();
            string userid = Convert.ToString(database.GetScalarValue(@"SELECT Username from CUSTOMER with(nolock) where Username='" + UserName + "'"));
            if (userid != "")
            {
                bool IsValid = MIM.isPwdMatched("customer", "username", UserName.Trim(), "PWD", Password.Trim(), database.GetConnectstring());
                if (IsValid == true)
                {
                    dt = MIM.GetUserParameter("customer", "username", UserName.Trim(), "PWD", Password.Trim(), database.GetConnectstring());
                }
            }
            else
            {
                dt = null;
            }
            return dt;
        }
        public DataTable GetUserInfo(string userid)
        {
            string sql = "select * from customer with(nolock) where username ='" + userid + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }


        public DataTable GetTemplteAPI(string user, string TemplateId = "")
        {
            string sql = @" select templateID , templateName, msgtext from TemplateID t with(nolock) 
 inner join senderidmast s  with(nolock) on  t.senderid=s.senderid where userid='" + user + "' and " +
 "TemplateID = CASE WHEN ISNULL('" + TemplateId + "', '')= '' then TemplateID else '" + TemplateId + "' end group by templateName, msgtext, templateID";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplatePanel(string user, string TemplateId = "", string TempName = "")
        {
            string sql = @"select templateid, tempname as templateName, template as msgtext from templaterequest 
        with(nolock) where username = '" + user + "' and " +
        "templateid = CASE WHEN ISNULL('" + TemplateId + "', '')= '' then templateid else '" + TemplateId + "' end and " +
        "tempName = CASE WHEN ISNULL('" + TempName + "', '')= '' then tempName else '" + TempName + "' end " +
        "group by templateid, tempname, template";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSender(string user)
        {
            string sql = "select * from senderidmast  with(nolock) where userid = '" + user + "'";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public void Log(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public int GetCountForSubmittedTemplate(string username, string senderId, string currentTime)
        {
            string sql = string.Format("Select Count(id) [Total] from msgsubmitted with(nolock) where senderid='{0}' AND profileid='{1}' AND insertDate>'{2}' ;", senderId, username, currentTime);
            return Convert.ToInt32(database.GetScalarValue(sql));

        }
        public string Insert_MSGTRAN_91M(string PROFILEID, string MSGTEXT, string TOMOBILE, string SENDERID, string peid, string templateid)
        {
            string Msg = "";
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Insert_MSGTRAN_91M";
                    cmd.Parameters.AddWithValue("@PROFILEID", PROFILEID);
                    cmd.Parameters.AddWithValue("@MSGTEXT", MSGTEXT);
                    cmd.Parameters.AddWithValue("@TOMOBILE", TOMOBILE);
                    cmd.Parameters.AddWithValue("@SENDERID", SENDERID);
                    cmd.Parameters.AddWithValue("@peid", peid);
                    cmd.Parameters.AddWithValue("@templateid", templateid);
                    cmd.ExecuteNonQuery();
                    Msg = "Saved Successfully !";
                }
                catch (Exception ex)
                {
                    Log("Insert_MSGTRAN_91M : " + ex.ToString());
                }
                return Msg;
            }
        }
    }
}