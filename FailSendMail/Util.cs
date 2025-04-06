using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
//using Inetlab.SMPP.PDU;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

namespace FailSendMail
{
    public class Util
    {

        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();

        public void Info_Err3(string msg)
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
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }
     
        public void FetchfailOverRecord()
        {
            try
            {
                //string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("Select LastSMSFailOverOBDtime from settings with (nolock) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                DataTable dts = database.GetDataTable("select * from settings with(nolock)");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //fetch Failed Records of SMS
                string sql = @"select * from WebLinks with(nolock) where MailSent=0 and Active=1";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var client = new RestClient(dr["URL"].ToString());
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        IRestResponse response = client.Execute(request);
                        string StatusCode = response.StatusCode.ToString();

                        Info_Err3("SITE - " + dr["URL"].ToString() + " resp-" + StatusCode  );

                        if (StatusCode.ToUpper() != "OK")
                        {
                            string body = dr["URL"].ToString();
                            string Status = SendEmail(dts.Rows[0]["MailTo"].ToString(), dts.Rows[0]["Subject"].ToString(), body, dts.Rows[0]["MailFromId"].ToString(), dts.Rows[0]["Pwd"].ToString(), dts.Rows[0]["Host"].ToString(), dts.Rows[0]["Portno"].ToString(), dts.Rows[0]["CC"].ToString(), "");
                            if (Status.Contains("Successfully") == true)
                            {
                                string Update = @"Update WebLinks set LastFailDate='" + currTime + "',MailSent=1 where url='"+ dr["URL"].ToString() + "'";
                                database.ExecuteNonQuery(Update);
                            }

                        }
 
                    }
                }
            }
            catch (Exception ex)
            {
                Info_Err3("fetchSMSFailOver- " + ex.Message);
            }
        }



        public string SendEmail(string toAddress, string subject, string body, string UserId, string Pwd, string Host, string Port, string CC, string Path)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = UserId; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                message.IsBodyHtml = true;
                //for (int i = 0; i < CC.Count; i++)
                //{
                //    message.CC.Add(CC[i]);
                //}
                message.CC.Add(CC);

                // message.CC.Add("support@myinboxmedia.com");

                if (Path != "")
                {
                    Attachment item = new Attachment(Path);
                    message.Attachments.Add(item);
                }

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);

            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                Log("error on sending email - " + ex.Message + " -- " + ex.StackTrace);

                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
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



    }
   
}





