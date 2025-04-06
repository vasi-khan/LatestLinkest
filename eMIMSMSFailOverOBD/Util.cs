
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Inetlab.SMPP.PDU;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

namespace MimSenddata
{
    public class Message
    {
        public string to { get; set; }
        public Status status { get; set; }
        public string messageId { get; set; }
    }

    public class OBDRoot
    {
        public string bulkId { get; set; }
        public List<Message> messages { get; set; }
    }

    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class WARoot
    {
        public List<Message> messages { get; set; }
    }

    public class RCSMessage
    {
        public string to { get; set; }
        public int messageCount { get; set; }
        public string messageId { get; set; }
        public Status status { get; set; }
    }


    public class WABARoot
    {
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
        public string mid { get; set; }
    }

    public class RCSRoot
    {
        public List<RCSMessage> messages { get; set; }
    }

    public class ValidationErrors
    {
        public List<string> whatsApp { get; set; }
    }

    public class ServiceException
    {
        public string messageId { get; set; }
        public string text { get; set; }
        public ValidationErrors validationErrors { get; set; }
    }

    public class RequestError
    {
        public ServiceException serviceException { get; set; }
    }

    public class WaErrorRoot
    {
        public RequestError requestError { get; set; }
    }

    public class WAButton
    {
        public string type { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }

    public class WAoption
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ServiceExceptionU
    {
        public string messageId { get; set; }
        public string text { get; set; }
    }

    public class RequestErrorU
    {
        public ServiceExceptionU serviceExceptionU { get; set; }
    }

    public class RootU
    {
        public RequestErrorU requestErrorU { get; set; }
    }

    public class Util
    {

        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        string Check = ConfigurationManager.AppSettings["OBD"].ToString();
        string OBDINTERVALSEC = System.Configuration.ConfigurationManager.AppSettings["OBDINTERVALSEC"].ToString();


        public void RCSApi(string mob, string msgtext, string suggetiontext, string postbackdata, string notifyUrl, string callbackdata, string msgid, string authkey, string profileid, int fileid, int i, List<string> FieldList, string imgurl = "")
        {

            var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body = @"{
             ""from"": ""myinbox"",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n") + @""",
               ""type"": ""TEXT""
             }
           }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            // rcsimg(mob, authkey, imgurl);
            RCSRoot res = new RCSRoot();
            WaErrorRoot WAErr = new WaErrorRoot();
            if (response.Content.Contains("BAD_REQUEST"))
            {
                WAErr = JsonConvert.DeserializeObject<WaErrorRoot>(response.Content);

            }
            if (response.Content.Contains("BAD_REQUEST"))
            {
                WAErr = JsonConvert.DeserializeObject<WaErrorRoot>(response.Content);

            }
            else
            {
                res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
            }

            try
            {
                if (res.messages != null && res.messages.Count > 0)
                {
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into RCSsubmitted(messageId,tomobile,msgtext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";

                    string selectsql = @"select '" + ms.messageId + "','" + mob + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + profileid + "','" + st.description + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";
                    for (int n = 1; n <= FieldList.Count; n++)
                    {
                        sql = sql + "," + "Col" + n.ToString();
                        selectsql = selectsql + ",N'" + FieldList[n - 1] + "'";
                    }
                    sql = sql + ") ";

                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    ServiceException Se = new ServiceException();
                    RequestError ReqErr = new RequestError();

                    ReqErr = WAErr.requestError;
                    Se = WAErr.requestError.serviceException;

                    string sql = @"insert into RCSsubmitted(messageId,tomobile,msgtext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";
                    string selectsql = "";
                    if (Se.validationErrors != null)
                    {
                        selectsql = @"select '" + msgid + "','" + mob + "',N'" + msgtext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','" + Se.validationErrors.whatsApp[0] + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";

                    }
                    else
                    {
                        selectsql = @"select '" + msgid + "','" + mob + "',N'" + msgtext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";

                    }
                    for (int n = 1; n <= FieldList.Count; n++)
                    {
                        sql = sql + "," + "Col" + n.ToString();
                        selectsql = selectsql + ",N'" + FieldList[n - 1] + "'";
                    }
                    sql = sql + ") ";

                    database.ExecuteNonQuery(sql + selectsql);

                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }



        }

        public void Info_Err(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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

        public void logAPI(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"API_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"API_catch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void logAPIwaba(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"wabaAPI_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"wabaAPI_catch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        public void logAPIEmail(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"EmailAPI_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"EmailAPI_catch_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
        //string mob, string msgtext, string suggetiontext, string postbackdata, string notifyUrl, string callbackdata, string msgid, string authkey, string profileid, int fileid, int i, List<string> FieldList, string imgurl = "")
        public void rcsimg(string mob, string authkey, string imgurl)
        {
            var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body_img = @"{
             ""from"": ""myinbox"",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                ""file"": {
                    ""url"": """ + imgurl + @"""
                },
                ""thumbnail"": {
                     ""url"": """ + imgurl + @"""
                },
            ""type"": ""FILE""
            }
           }";


            request.AddParameter("application/json", body_img, ParameterType.RequestBody);
            IRestResponse response_img = client.Execute(request);

        }

        public void RCSApiAN_Test(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pUserId, string SessionId, string acid, string apiurl)
        {


            //msgtext = HttpUtility.UrlEncode(msgtext);
            //msgtext = "text by ajay";
            //msgtext = Replace(Replace([msgtext], Chr(10), ""), Chr(13), ""));
            // msgtext.Replace(Convert.ToChar(13), @"\n");

            //var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body = @"{
             ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n") + @""",
""suggestions"":[""],

""type"": ""TEXT""
             }
           }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            string pStatus = "OK";// response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    //res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();

                    Status st = new Status();

                    Random rn = new Random();
                    int num = rn.Next(1, 3);
                    string dlrStatus = "";
                    if (num == 1)
                    {
                        dlrStatus = "DELIVERED";
                    }
                    else
                    {
                        dlrStatus = "UNDELIVERABLE";
                    }
                    string sql = @"
declare @msgid varchar(500)
set @msgid=(select newid())
insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";

                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',@msgid,'" + ms.to + "',N'" + msgtext + "','1','',1,'','OK','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqldlr = " insert into tblRCSMSGDELIVERY(bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime) " +
                                        " select '',0,'','','','" + dlrStatus + "','','','','','','','','',0,@MSGID,getdate(),1,getdate(),'" + mob + "','',getdate() ";
                    string sqlF = sql + selectsql + sqldlr;
                    database.ExecuteNonQuery(sqlF);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','Bad Request'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }
        }
        public void rcsimgAN_Test(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string imgurl, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            var body_img = @"{
            ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                ""file"": {
                    ""url"": """ + imgurl + @"""
                },
                ""thumbnail"": {
                     ""url"": """ + imgurl + @"""
                },
            ""type"": ""FILE""
            }
           }";

            body = body_img;
            request.AddParameter("application/json", body_img, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            string pStatus = "OK";// response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    //res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    // ms = res.messages[0];

                    Status st = new Status();
                    //st = ms.status;

                    Random rn = new Random();
                    int num = rn.Next(1, 3);
                    string dlrStatus = "";
                    if (num == 1)
                    {
                        dlrStatus = "DELIVERED";
                    }
                    else
                    {
                        dlrStatus = "UNDELIVERABLE";
                    }
                    string sql = @"
declare @msgid varchar(500)
set @msgid=(select newid())
insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";

                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',@msgid,'" + ms.to + "',N'" + msgtext + "','1','',1,'','OK','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqldlr = " insert into tblRCSMSGDELIVERY(bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime) " +
                                        " select '',0,'','','','" + dlrStatus + "','','','','','','','','',0,@MSGID,getdate(),1,getdate(),'" + mob + "','',getdate() ";
                    string sqlF = sql + selectsql + sqldlr;
                    database.ExecuteNonQuery(sqlF);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','Bad Request'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }


        }
        public void RCSCard_Test(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""orientation"":""" + dt.Rows[0]["CardOrientation"].ToString() + @""",
                  ""alignment"": """ + dt.Rows[0]["CardAlignment"].ToString() + @""",
                  ""content"":{";
            DataTable dtCardSuggetion = new DataTable();
            for (int j = 0; j < dt.Rows.Count; j++)
            {

                body_img1 += @"
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
         ""media"": {
                        ""file"": {
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                   },
                     ""thumbnail"":{
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                    },
                        ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
                    },
                         ""suggestions"":[";
                dtCardSuggetion = GetCardSuggetion(dt.Rows[j]["id"].ToString());

                for (int p = 0; p < dtCardSuggetion.Rows.Count; p++)
                {
                    if (p > 0)
                    {
                        body_img1 += @",";
                    }

                    body_img1 += @"
                 {
        ""text"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                    {
                        body_img1 += @",
                    ""type"": ""REPLY""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                    {
                        body_img1 += @",
                                ""url"": """ + dtCardSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                    {
                        body_img1 += @",
                                ""phoneNumber"": """ + dtCardSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                    {
                        body_img1 += @",
                                ""latitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                    {
                        body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""";
                    }

                    body_img1 += @"}";
                }
                body_img1 += @"]";
                body_img1 += @"}";

            }

            body_imgF = @",
                       ""type"": ""CARD""},
                        ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            body = body_imgH + body_img1 + body_imgF;

            request.AddParameter("application/json", body_img, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            string pStatus = "OK";// response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    //res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    // ms = res.messages[0];

                    Status st = new Status();
                    //st = ms.status;

                    Random rn = new Random();
                    int num = rn.Next(1, 3);
                    string dlrStatus = "";
                    if (num == 1)
                    {
                        dlrStatus = "DELIVERED";
                    }
                    else
                    {
                        dlrStatus = "UNDELIVERABLE";
                    }
                    string sql = @"
declare @msgid varchar(500)
set @msgid=(select newid())
insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";

                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',@msgid,'" + ms.to + "',N'" + msgtext + "','1','',1,'','OK','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqldlr = " insert into tblRCSMSGDELIVERY(bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime) " +
                                        " select '',0,'','','','" + dlrStatus + "','','','','','','','','',0,@MSGID,getdate(),1,getdate(),'" + mob + "','',getdate() ";
                    string sqlF = sql + selectsql + sqldlr;
                    database.ExecuteNonQuery(sqlF);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','Bad Request'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }

        }
        public void RCSCarousalAN_Test(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            DataTable dtCardSuggetion = new DataTable();
            DataTable dtOuterSuggetion = new DataTable();
            dtOuterSuggetion = GetCrauselSuggetion(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""cardWidth"":""MEDIUM"",
                  ""contents"":[";

            for (int j = 0; j < dt.Rows.Count; j++)
            {

                if (j > 0)
                {
                    body_img1 += @",";
                }
                body_img1 += @"{
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",

        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""media"": {
                    ""file"":
                            {
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                            },
                ""thumbnail"": 
                            {
                       ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                            },
        ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
                   },
                        ""suggestions"":[";
                dtCardSuggetion = GetCardSuggetion(dt.Rows[j]["id"].ToString());

                for (int p = 0; p < dtCardSuggetion.Rows.Count; p++)
                {
                    if (p > 0)
                    {
                        body_img1 += @",";
                    }

                    body_img1 += @"
                 {
        ""text"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                    {
                        body_img1 += @",
                    ""type"": ""REPLY""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                    {
                        body_img1 += @",
                                ""url"": """ + dtCardSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                    {
                        body_img1 += @",
                                ""phoneNumber"": """ + dtCardSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                    {
                        body_img1 += @",
                                ""latitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                    {
                        body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""";
                    }

                    body_img1 += @"}";
                }
                body_img1 += @"]";
                body_img1 += @"}";
            }


            body_imgF = @"],
""suggestions"":[";

            for (int p = 0; p < dtOuterSuggetion.Rows.Count; p++)
            {
                if (p > 0)
                {
                    body_imgF += @",";
                }

                body_imgF += @"
                 {
        ""text"": """ + dtOuterSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                {
                    body_imgF += @",
                    ""type"": ""REPLY""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                {
                    body_imgF += @",
                                ""url"": """ + dtOuterSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                {
                    body_imgF += @",
                                ""phoneNumber"": """ + dtOuterSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                {
                    body_imgF += @",
                                ""latitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                {
                    body_imgF += @",
                    ""type"": ""REQUEST_LOCATION""";
                }

                body_imgF += @"}";
            }


            body_imgF += @"],
                       ""type"": ""CAROUSEL""";
            body_imgF += @"},

                    

            ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            // request.AddParameter("application/json", body_img, ParameterType.RequestBody);

            //IRestResponse response = client.Execute(request);

            string pStatus = "OK";// response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    //res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    //ms = res.messages[0];

                    Status st = new Status();
                    //st = ms.status;

                    Random rn = new Random();
                    int num = rn.Next(1, 3);
                    string dlrStatus = "";
                    if (num == 1)
                    {
                        dlrStatus = "DELIVERED";
                    }
                    else
                    {
                        dlrStatus = "UNDELIVERABLE";
                    }
                    string sql = @"
declare @msgid varchar(500)
set @msgid=(select newid())
insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";

                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',@msgid,'" + ms.to + "',N'" + msgtext + "','1','',1,'','OK','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqldlr = " insert into tblRCSMSGDELIVERY(bulkid,pricePerMessage,currency,status_id,status_groupId,status_groupName,status_name,status_description,status_action,error_id,error_name,error_description,error_groupId,error_groupName,error_permanent,MSGID,doneAt,messageCount,sentAt,toMob,channel,inserttime) " +
                                        " select '',0,'','','','" + dlrStatus + "','','','','','','','','',0,@MSGID,getdate(),1,getdate(),'" + mob + "','',getdate() ";
                    string sqlF = sql + selectsql + sqldlr;
                    database.ExecuteNonQuery(sqlF);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','Bad Request'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }

        }

        public void RCSApiAN(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pUserId, string SessionId, string acid, string apiurl, string pTempletId = "")
        {


            //msgtext = HttpUtility.UrlEncode(msgtext);
            //msgtext = "text by ajay";
            //msgtext = Replace(Replace([msgtext], Chr(10), ""), Chr(13), ""));
            // msgtext.Replace(Convert.ToChar(13), @"\n");

            //var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            DataTable dtOuterSuggetion = new DataTable();
            dtOuterSuggetion = GetCrauselSuggetion(pTempletId);

            var body = @"{
             ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n") + @""",
               ""suggestions"":[";

            for (int p = 0; p < dtOuterSuggetion.Rows.Count; p++)
            {
                if (p > 0)
                {
                    body += @",";
                }

                body += @"
                 {
        ""text"": """ + dtOuterSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                {
                    body += @",
                    ""type"": ""REPLY""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                {
                    body += @",
                                ""url"": """ + dtOuterSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                {
                    body += @",
                                ""phoneNumber"": """ + dtOuterSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                {
                    body += @",
                                ""latitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                {
                    body += @",
                    ""type"": ""REQUEST_LOCATION""";
                }

                body += @"}";
            }

            body += @"],

                        ""type"": ""TEXT""
             }
           }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            RCSRoot res = new RCSRoot();

            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                    //Info_Err(response.Content, 0);
                }


            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }
        }
        public void rcsimgAN(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string imgurl, string pUserId, string SessionId, string acid, string apiurl, string pTempletId = "")
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");

            //request.AddHeader("Accept", "application/json");
            var body = "";
            var body_img = @"{
            ""from"": """ + acid + @""",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                ""file"": {
                    ""url"": """ + imgurl + @"""
                },
                ""thumbnail"": {
                     ""url"": """ + imgurl + @"""
                },

            ""type"": ""FILE""
            }
           }";

            body = body_img;
            request.AddParameter("application/json", body_img, ParameterType.RequestBody);
            IRestResponse response_img = client.Execute(request);

            string pStatus = response_img.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    RCSRoot res = new RCSRoot();
                    res = JsonConvert.DeserializeObject<RCSRoot>(response_img.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;


                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response_img.Content + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }



        }
        public void RCSCard(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""orientation"":""" + dt.Rows[0]["CardOrientation"].ToString() + @""",
                  ""alignment"": """ + dt.Rows[0]["CardAlignment"].ToString() + @""",
                  ""content"":{";
            DataTable dtCardSuggetion = new DataTable();
            for (int j = 0; j < dt.Rows.Count; j++)
            {

                body_img1 += @"
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
         ""media"": {
                        ""file"": {
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                   },
                     ""thumbnail"":{
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                    },
                        ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
                    },
                         ""suggestions"":[";
                dtCardSuggetion = GetCardSuggetion(dt.Rows[j]["id"].ToString());

                for (int p = 0; p < dtCardSuggetion.Rows.Count; p++)
                {
                    if (p > 0)
                    {
                        body_img1 += @",";
                    }

                    body_img1 += @"
                 {
        ""text"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                    {
                        body_img1 += @",
                    ""type"": ""REPLY""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                    {
                        body_img1 += @",
                                ""url"": """ + dtCardSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                    {
                        body_img1 += @",
                                ""phoneNumber"": """ + dtCardSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                    {
                        body_img1 += @",
                                ""latitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                    {
                        body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""";
                    }

                    body_img1 += @"}";
                }
                body_img1 += @"]";
                body_img1 += @"}";

            }

            body_imgF = @",
                       ""type"": ""CARD""},
                        ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            body = body_imgH + body_img1 + body_imgF;

            request.AddParameter("application/json", body_img, ParameterType.RequestBody);

            //string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
            //string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',newid(),'" + mob + "',N'" + msgtext + "','1','NA',1,'NA','NA','" + pUserId + "','OK' ," + SessionId + "; ";
            //string sqlF = sql + selectsql;
            //database.ExecuteNonQuery(sql + selectsql);

            IRestResponse response = client.Execute(request);

            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content.ToString() + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }

        }
        public void RCSCarousalAN(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            DataTable dtCardSuggetion = new DataTable();
            DataTable dtOuterSuggetion = new DataTable();
            dtOuterSuggetion = GetCrauselSuggetion(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""cardWidth"":""MEDIUM"",
                  ""contents"":[";

            for (int j = 0; j < dt.Rows.Count; j++)
            {

                if (j > 0)
                {
                    body_img1 += @",";
                }
                body_img1 += @"{
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",

        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""media"": {
                    ""file"":
                            {
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                            },
                ""thumbnail"": 
                            {
                       ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                            },
        ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
                   },
                        ""suggestions"":[";
                dtCardSuggetion = GetCardSuggetion(dt.Rows[j]["id"].ToString());

                for (int p = 0; p < dtCardSuggetion.Rows.Count; p++)
                {
                    if (p > 0)
                    {
                        body_img1 += @",";
                    }

                    body_img1 += @"
                 {
        ""text"": """ + dtCardSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                    {
                        body_img1 += @",
                    ""type"": ""REPLY""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                    {
                        body_img1 += @",
                                ""url"": """ + dtCardSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                    {
                        body_img1 += @",
                                ""phoneNumber"": """ + dtCardSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                    {
                        body_img1 += @",
                                ""latitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtCardSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                    }

                    if (dtCardSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                    {
                        body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""";
                    }

                    body_img1 += @"}";
                }
                body_img1 += @"]";
                body_img1 += @"}";
            }


            body_imgF = @"],
""suggestions"":[";

            for (int p = 0; p < dtOuterSuggetion.Rows.Count; p++)
            {
                if (p > 0)
                {
                    body_imgF += @",";
                }

                body_imgF += @"
                 {
        ""text"": """ + dtOuterSuggetion.Rows[p]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REPLY")
                {
                    body_imgF += @",
                    ""type"": ""REPLY""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "OPEN_URL")
                {
                    body_imgF += @",
                                ""url"": """ + dtOuterSuggetion.Rows[p]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "DIAL_PHONE")
                {
                    body_imgF += @",
                                ""phoneNumber"": """ + dtOuterSuggetion.Rows[p]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "SHOW_LOCATION")
                {
                    body_imgF += @",
                                ""latitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dtOuterSuggetion.Rows[p]["SuggestionLongitude"].ToString() + @""",
                                ""type"": ""SHOW_LOCATION""";
                }

                if (dtOuterSuggetion.Rows[p]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                {
                    body_imgF += @",
                    ""type"": ""REQUEST_LOCATION""";
                }

                body_imgF += @"}";
            }


            body_imgF += @"],
                       ""type"": ""CAROUSEL""";
            body_imgF += @"},

                    

            ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            request.AddParameter("application/json", body_img, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }

        }

        public void RCSCard_b422APR(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""orientation"":""" + dt.Rows[0]["CardOrientation"].ToString() + @""",
                  ""alignment"": """ + dt.Rows[0]["CardAlignment"].ToString() + @""",
                  ""content"":{";

            for (int j = 0; j < dt.Rows.Count; j++)
            {

                body_img1 += @"
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
         ""media"": {
                        ""file"": {
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                   },
                     ""thumbnail"":{
                                ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                                    },
                        ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
                    },
                         ""suggestions"":[{
                         ""text"": """ + dt.Rows[j]["SuggestionText"].ToString() + @""",
                         ""postbackData"": ""examplePostbackData""";
                if (dt.Rows[j]["SuggestionType"].ToString() == "REPLY")
                {
                    body_img1 += @",
                    ""type"": ""REPLY""                    
                   ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "OPEN_URL")
                {
                    body_img1 += @",
                                ""url"": """ + dt.Rows[j]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""
                                 ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "DIAL_PHONE")
                {
                    body_img1 += @",
                                ""phoneNumber"": """ + dt.Rows[j]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""
                                ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "SHOW_LOCATION")
                {
                    body_img1 += @",
                                ""latitude"": """ + dt.Rows[j]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dt.Rows[j]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dt.Rows[j]["SuggestionLongitude"].ToString() + @""",
                                 ""type"": ""SHOW_LOCATION""
                                ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                {
                    body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""
                   ";
                }

                body_img1 += @"}]"; // Suggession end

                body_img1 += @"}";
            }

            body_imgF = @",
                       ""type"": ""CARD""},
                        ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            body = body_imgH + body_img1 + body_imgF;

            request.AddParameter("application/json", body_img, ParameterType.RequestBody);

            //string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
            //string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',newid(),'" + mob + "',N'" + msgtext + "','1','NA',1,'NA','NA','" + pUserId + "','OK' ," + SessionId + "; ";
            //string sqlF = sql + selectsql;
            //database.ExecuteNonQuery(sql + selectsql);

            IRestResponse response = client.Execute(request);

            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {

                    RCSRoot res = new RCSRoot();
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content.ToString() + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }

        }
        public void RCSCarousalAN_b422APR(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
            DataTable dt = u1.GetCrausaldetail(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """ + acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {


      ""cardWidth"":""MEDIUM"",
      ""contents"":[";

            for (int j = 0; j < dt.Rows.Count; j++)
            {

                if (j > 0)
                {
                    body_img1 += @",";

                }
                body_img1 += @"{
        ""title"": """ + dt.Rows[j]["CardTitle"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",

        ""description"": """ + dt.Rows[j]["CardDesc"].ToString().Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
        ""media"": {
                    ""file"": {
                        ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                        
                    },
          ""thumbnail"": {
                       ""url"": """ + dt.Rows[j]["FileUrl"].ToString() + @"""
                       
          },
          ""height"": """ + dt.Rows[j]["CardHeight"].ToString() + @"""
        },
        ""suggestions"":[{
        ""text"": """ + dt.Rows[j]["SuggestionText"].ToString() + @""",
        ""postbackData"": ""examplePostbackData""";
                if (dt.Rows[j]["SuggestionType"].ToString() == "REPLY")
                {
                    body_img1 += @",
                    ""type"": ""REPLY""                    
                   ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "OPEN_URL")
                {
                    body_img1 += @",
                                ""url"": """ + dt.Rows[j]["SuggestionUrl"].ToString() + @""",
                                ""type"": ""OPEN_URL""
                                 ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "DIAL_PHONE")
                {
                    body_img1 += @",
                                ""phoneNumber"": """ + dt.Rows[j]["SuggestionPhone"].ToString() + @""",
                                ""type"": ""DIAL_PHONE""
                                ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "SHOW_LOCATION")
                {
                    body_img1 += @",
                                ""latitude"": """ + dt.Rows[j]["SuggestionLatitude"].ToString() + @""",
                                ""longitude"": """ + dt.Rows[j]["SuggestionLongitude"].ToString() + @""",
                                ""label"": """ + dt.Rows[j]["SuggestionLongitude"].ToString() + @""",
                                 ""type"": ""SHOW_LOCATION""
                                ";
                }

                if (dt.Rows[j]["SuggestionType"].ToString() == "REQUEST_LOCATION")
                {
                    body_img1 += @",
                    ""type"": ""REQUEST_LOCATION""
                   ";
                }

                body_img1 += @"}]";
                body_img1 += @"}";
            }

            body_imgF = @"],
                       ""type"": ""CAROUSEL""},
                        ""callbackData"":""Callback data""

                            }";

            body_img = body_imgH + body_img1 + body_imgF;
            body = body_imgH + body_img1 + body_imgF;
            request.AddParameter("application/json", body_img, ParameterType.RequestBody);

            //string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
            //string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "',newid(),'" + mob + "',N'" + msgtext + "','1','NA',1,'NA','NA','" + pUserId + "','OK' ,"+ SessionId + "; ";
            //string sqlF = sql + selectsql;
            //database.ExecuteNonQuery(sql + selectsql);

            IRestResponse response = client.Execute(request);

            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    RCSRoot res = new RCSRoot();
                    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];
                    Status st = new Status();
                    st = ms.status;
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId,SentStatus,SessionId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "','" + pStatus + "','" + SessionId + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,UserId,SentStatus,SessionId,description)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pUserId + "','" + pStatus + "','" + SessionId + "','" + response.Content + "'; ";
                    string sqlF = sql + selectsql;
                    database.ExecuteNonQuery(sql + selectsql);
                }
            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }
        }

        public void RCSApiANBKP(string authkey, string mob, string msgtext, string pRcsMsgRcvdId, string pUserId)
        {

            var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body = @"{
             ""from"": ""myinbox"",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(@"""", @"\""") + @""",
               ""type"": ""TEXT""
             }
           }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            // rcsimg(mob, authkey, imgurl);
            RCSRoot res = new RCSRoot();
            WaErrorRoot WAErr = new WaErrorRoot();
            if (response.Content.Contains("BAD_REQUEST"))
            {
                WAErr = JsonConvert.DeserializeObject<WaErrorRoot>(response.Content);
            }
            //else if (response.Content.Contains("UNAUTHORIZED"))
            //{
            //    res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
            //}
            else
            {
                res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
            }
            try
            {
                if (res.messages != null && res.messages.Count > 0)
                {
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId)";
                    string selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + ms.messageId + "','" + ms.to + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + pUserId + "'; ";
                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    ServiceException Se = new ServiceException();
                    RequestError ReqErr = new RequestError();

                    ReqErr = WAErr.requestError;
                    Se = WAErr.requestError.serviceException;

                    string sql = @"insert into tblRCSMSGSUBMITTED( RcsMsgRcvdId,MobNo,messageId,tomobile,msgtext,groupId,groupName,Id,name,description,UserId)";

                    string selectsql = "";
                    if (Se.validationErrors != null)
                    {
                        selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + pRcsMsgRcvdId + "','" + mob + "',N'" + msgtext + "','" + "0" + "','" + Se.text + "'," + "0" + ",'" + Se.text + "','" + Se.validationErrors.whatsApp[0] + "','" + pUserId + "'; ";
                    }
                    else
                    {
                        selectsql = @"select '" + pRcsMsgRcvdId + "','" + mob + "','" + 0 + "','" + mob + "',N'" + msgtext + "','" + "0" + "','" + Se.text + "'," + "0" + ",'" + Se.text + "','" + "" + "','" + pUserId + "'; ";

                    }
                }
            }
            catch (Exception ex)
            {
                Info_Err("RCSApi " + body + " " + ex.Message, 0);

                throw;
            }
        }

        public void UpdateDashboardData()
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                using (SqlCommand cmd = new SqlCommand("GetDashboardData1", cnn))
                {
                    cnn.Open();
                    cmd.CommandTimeout = 600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        public DataTable GetRCSRecords(int no_of_record, string picktime, int SessionId)
        {
            DataTable dt = new DataTable();
            try
            {
                //LogErr("RCS data req start from - " + picktime);

                //Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from rcstran WHERE PICKED_DATETIME IS NOT NULL"));
                //if (c == 0)
                //{
                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from rcstran WITH(NOLOCK) WHERE sessionid=" + SessionId + @" and PickedTime IS NULL"));
                if (c > 0)
                {
                    string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_record.ToString() + @" * from rcstran WITH(NOLOCK) where sessionid=" + SessionId + @" and PickedTime IS NULL ORDER BY CreatedDate 
                                    ) UPDATE CTE SET PickedTime='" + picktime + @"' where sessionid=" + SessionId + @"
                                    select * from rcstran WITH(NOLOCK) where PickedTime ='" + picktime + @"' and sessionid=" + SessionId + @" ORDER BY CreatedDate
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                    dt = database.GetDataTable(sql);
                    //InfoTest2("sms data req start end - dtcount=" + dt.Rows.Count);
                }
                //}
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err3("fetchrcs- " + ex.Message);
                return dt;
            }
        }

        public int GetRCSDuplicateCount(string mob, string fileid)
        {
            string sql = @"select count(*) from tblRCSMSGSUBMITTED with (nolock) where mobno = '" + mob + @"' ";
            sql = sql + "and RcsMsgRcvdId = '" + fileid + "'";
            int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
            return mcnt;

        }
        public void RemoveFromRCSTran(int i)
        {
            string sql = "Delete from rcstran where SessionId=" + i + " and PickedTime is not null ";
            database.ExecuteNonQuery(sql);
        }

        public void FetchfailOverRecord()
        {
            try
            {
                string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("Select LastSMSFailOverOBDtime from settings with (nolock) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                database.ExecuteNonQuery("Update settings set LastSMSFailOverOBDtime='" + currTime + "'");

                //fetch Failed Records of SMS
                // isHMILOTP=0
                string sql = @"select m.tomobile,m.smstext,m.Profileid,S.msgid from MSGSUBMITTED M WITH (NOLOCK) INNER JOIN DELIVERY D WITH (NOLOCK) ON M.MSGID=D.MSGID 
                inner join SMSFailOverOBD S ON M.MSGID=S.MSGID
                where d.insertDATE>='" + LastProcessedTime + "' and d.insertDATE<'" + currTime + @"' and d.dlvrstatus<>'DELIVERED' 
                and ISNULL(isHMILOTP,0)=0 GROUP BY m.tomobile,m.smstext,m.Profileid,S.MSGID ";

                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(Check) == "Knowlarity")
                        {
                            SendOBDthroughAPIOnCall_Knowlarity(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "");
                        }
                        if (Convert.ToString(Check) == "infobip")
                        {
                            SendOBDthroughAPIOnCall(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "");
                        }
                        if (Convert.ToString(Check) == "VCon")
                        {
                            SendOBDthroughAPIOnCall_VCon(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "");
                        }
                    }
                }


                // isHMILOTP=1
                string OBDINTERVALSEC = System.Configuration.ConfigurationManager.AppSettings["OBDINTERVALSEC"].ToString();

                string sql = @"select m.tomobile,m.smstext,m.Profileid,S.msgid from MSGSUBMITTED M WITH (NOLOCK) 
                left JOIN DELIVERY D WITH (NOLOCK) ON M.MSGID=D.MSGID  
                inner join SMSFailOverOBD S ON M.MSGID=S.MSGID 
                left join SMSFailOverOBDsent t ON M.MSGID=t.MSGID 
                inner join customer C ON S.PROFILEID=C.USERNAME 
                where s.SentDateTime >= DateAdd(Minute,-10,getdate()) and (( d.insertDATE>='" + LastProcessedTime + "' " +
                "and d.insertDATE<'" + currTime + @"' and d.dlvrstatus<>'DELIVERED' ) 
                or (datediff(second, s.SENTDATETIME,getdate())>=" + OBDINTERVALSEC + ") AND ISNULL(d.dlvrstatus,'')<>'DELIVERED' )" +
                " and t.msgid is null and" +
                " isHMILOTP=1 GROUP BY m.tomobile,m.smstext,m.Profileid,S.MSGID ";

                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(Check) == "Knowlarity")
                        {
                            SendOBDthroughAPIOnCall_Knowlarity(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "","1");
                        }
                        if (Convert.ToString(Check) == "infobip")
                        {
                            SendOBDthroughAPIOnCall(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "","1");
                        }
                        if (Convert.ToString(Check) == "VCon")
                        {
                            SendOBDthroughAPIOnCall_VCon(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["MSGID"].ToString(), "", "","1");
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Info_Err3("fetchSMSFailOverOBD- " + ex.Message);
            }
        }




        public void FetchfailOverRecordWABA()
        {
            try
            {
                string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("Select LastSMSFailOverWABAtime from settings with (nolock) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                database.ExecuteNonQuery("Update settings set LastSMSFailOverWABAtime='" + currTime + "'");

                //int FailOverWabaSec = Convert.ToInt32(database.GetScalarValue("Select FailOverWabaSecond from Customer with (nolock) "));

                //fetch Failed Records of SMS
                string sql = @"select m.tomobile,m.smstext,m.Profileid,s.msgid,s.WABATemplateName,s.WABAVariables,c.wabaProfileId,c.wabaPwd,S.FailOver
                FROM MSGSUBMITTED M WITH (NOLOCK) left JOIN DELIVERY D WITH (NOLOCK) ON M.MSGID=D.MSGID 
                inner join SMSFailOverWABA S ON M.MSGID=S.MSGID
                left join SMSFailOverWABAsent t ON M.MSGID=t.MSGID 
                inner join customer C ON S.PROFILEID=C.USERNAME
                where s.SentDateTime >= DateAdd(Minute,-10,getdate()) and (( d.insertDATE>='" + LastProcessedTime + "' and d.insertDATE<'" + currTime + @"' and d.dlvrstatus<>'DELIVERED' ) or
                (datediff(second, s.SENTDATETIME,getdate())>=C.FailOverWabaSecond) AND ISNULL(d.dlvrstatus,'')<>'DELIVERED' ) 
                and t.msgid is null
                GROUP BY m.tomobile,m.smstext,m.Profileid,s.msgid,s.WABATemplateName,s.WABAVariables,c.wabaProfileId,c.wabaPwd,S.FailOver ";

                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SendWABAthroughAPI(dr["tomobile"].ToString(), dr["smstext"].ToString(), dr["Profileid"].ToString(), dr["msgid"].ToString(), dr["WABATemplateName"].ToString(), dr["WABAVariables"].ToString(), dr["wabaProfileId"].ToString(), dr["wabaPwd"].ToString(), Convert.ToString(dr["FailOver"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Info_Err3("fetchSMSFailOverWABA- " + ex.Message);
            }
        }

        public void SendOBDthroughAPIOnCall_Knowlarity(string mob, string msgT, string CustomerId, string msgid, string PageName = "", string authkey = "", string hmilotp="")
        {
            string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //string time = DateTime.Now.AddMinutes(2) 
            var client = new RestClient("https://kpi.knowlarity.com/Basic/v1/account/call/campaign");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("x-api-key", "GesxeTJGz52ReWg8UBb8w7fTtqaCy1107E6bNZmG");
            request.AddHeader("Authorization", "a86d7c03-abb4-11e6-982f-066beb27a027");
            var body = @"{" + "\n" +
            @"    ""ivr_id"": ""1000065122""," + "\n" +
            @"    ""timezone"": ""Asia/Kolkata""," + "\n" +
            @"    ""priority"": ""1""," + "\n" +
            @"    ""order_throttling"": ""10""," + "\n" +
            @"    ""retry_duration"": ""15""," + "\n" +
            @"    ""start_time"": """ + DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture) + @"""," + "\n" +
            @"    ""max_retry"": ""2""," + "\n" +
            @"    ""call_scheduling"": ""[1, 1, 1, 1, 1, 0, 0]""," + "\n" +
            @"    ""call_scheduling_start_time"": ""09:00""," + "\n" +
            @"    ""call_scheduling_stop_time"": ""21:00""," + "\n" +
            @"    ""k_number"": ""+918047275779""," + "\n" +
            @"    ""additional_number"": ""+" + mob + @"," + msgT + @""", " + "\n" +
            @"    ""is_transactional"": ""True""" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            logAPI("req-" + body + " resp-" + response.Content);
            //Console.WriteLine(response.Content);

            try
            {
                OBDRoot res = JsonConvert.DeserializeObject<OBDRoot>(response.Content);
                Message msg = new Message();
                msg = res.messages[0];
                Status st = new Status();
                st = msg.status;

                String EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                SaveApiCallResponse(msg.messageId, mob, st.groupName, st.description, response.Content, StartTime, EndTime, CustomerId, PageName, msgid, msgT, SentTime, msg.status, hmilotp);
            }
            catch (Exception ex)
            {
                //saveapiresp("", mob, "", "", response.Content, ScenarioKey, FileId, SessionId, StartTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            //Console.WriteLine(response.Content);
        }

        public void SendOBDthroughAPIOnCall(string mob, string msgT, string CustomerId, string msgid, string PageName = "", string authkey = "", string hmilotp = "")
        {
            string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var client = new RestClient("https://vj1kpv.api.infobip.com/tts/3/single");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ConfigurationManager.AppSettings["AUTHKEYOBD"].ToString());
            var body = @"{
              " + "\n" +
            @"    ""text"": """ + msgT + @""",
              " + "\n" +
            @"    ""language"": ""en-in"",
              " + "\n" +
            @"    ""voice"": {
              " + "\n" +
            @"        ""name"": ""Heera"",
              " + "\n" +
            @"        ""gender"": ""female""
              " + "\n" +
            @"    },
              " + "\n" +
            @"    ""from"": """ + ConfigurationManager.AppSettings["DID"].ToString() + @""",
              " + "\n" +
            @"    ""to"": """ + mob + @"""
              " + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            logAPI("req-" + body + " resp-" + response.Content);
            //Console.WriteLine(response.Content);

            try
            {
                OBDRoot res = JsonConvert.DeserializeObject<OBDRoot>(response.Content);
                Message msg = new Message();
                msg = res.messages[0];
                Status st = new Status();
                st = msg.status;

                String EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                SaveApiCallResponse(msg.messageId, mob, st.groupName, st.description, response.Content, StartTime, EndTime, CustomerId, PageName, msgid, msgT, SentTime, msg.status, hmilotp);
            }
            catch (Exception ex)
            {
                Info_Err3("SendOBDthroughAPIOnCall 1 - " + ex.Message);
                //saveapiresp("", mob, "", "", response.Content, ScenarioKey, FileId, SessionId, StartTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            //Console.WriteLine(response.Content);
        }

        public void SendWABAthroughAPI(string mob, string msgT, string CustomerId, string msgid, string WABATemplateName, string WABAVariables, string wabaProfileId, string wabaPwd, string FailOver)
        {
            try
            {
                string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                SaveWaBaApiCallRequestResponse(CustomerId, msgT, mob, SentTime, msgid, "", "", "", "REQUEST");

                string[] separatingStrings = { "$," };
                string[] ar = WABAVariables.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                string param = "";
                for (int i = 0; i < ar.Length; i++)
                {
                    param = param + @"""" + ar[i].Replace(',', ' ') + @"""";
                    if (i != ar.Length - 1) param = param + ",";
                }

                var client = new RestClient("http://103.205.64.220:17250/wabaapi/api/sendwaba");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
" + "\n" +
                @"    ""ProfileId"": """ + wabaProfileId + @""",
" + "\n" +
                @"    ""APIKey"": """ + wabaPwd + @""",
" + "\n" +
                @"    ""MobileNumber"": """ + mob + @""",
" + "\n" +
                @"    ""templateName"": """ + WABATemplateName + @""",
" + "\n" +
                @"    ""Parameters"": [ " + param + @"
" + "\n" +
                @"    ],
" + "\n" +
                @"    ""HeaderType"": ""TEXT"",
" + "\n" +
                @"    ""Text"": """",
" + "\n" +
                @"    ""MediaUrl"": """",
" + "\n" +
                @"    ""isTemplate"": ""true"",
" + "\n" +
                @"    ""FailOver"": """ + FailOver + @"""
" + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                logAPIwaba("req-" + body + " resp-" + response.Content);
                //Console.WriteLine(response.Content);
                try
                {
                    WABARoot res = JsonConvert.DeserializeObject<WABARoot>(response.Content);
                    //OBDRoot res = JsonConvert.DeserializeObject<OBDRoot>(response.Content);

                    string st = res.statusCode;
                    SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string wabamsgid = res.mid;
                    string statdesc = res.statusDesc;
                    SaveWaBaApiCallRequestResponse(CustomerId, msgT, mob, SentTime, msgid, Convert.ToString(st), wabamsgid, statdesc, "RESPONSE");
                }
                catch (Exception ex)
                {
                    Info_Err3("SendWABAthroughAPI 1 - " + ex.Message);
                }


            }
            catch (Exception e1) { Info_Err3("SendWABAthroughAPI 2 - " + e1.Message); }
        }


        public void SaveWaBaApiCallRequestResponse(string profileid, string msgtext, string mobile, string sentdate, string msgid, string dlvrstatus, string wabamsgid, string statdesc, string strType)
        {
            string tblname = (strType == "REQUEST" ? "SMSFailOverWABAsent" : "SMSFailOverWABAsentResponse");
            try
            {
                string sql = @"insert into " + tblname + @" (profileid,msgtext,tomobile,sentdatetime,msgid,delivery_status,wabamsgid,statusDesc)
                values('" + profileid + "','" + msgtext + "','" + mobile + "','" + sentdate + "','" + msgid + "','" + dlvrstatus + "','" + wabamsgid + "','" + statdesc + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Info_Err3("SaveWaBaApiCallRequestResponse 1 - " + ex.Message);
                throw;
            }
        }

        public void SaveApiCallResponse(string messageId, string mob, string groupName, string description, string resp, string StartTime, string EndTime, string CustomerId, string PageName, string msgid, string msgtext = "", string SentTime = "", string status = "", string hmilotp="")
        {
            string str = "";
            try
            {
                string sql = @"insert into ObdCallResponse (msgid,mobno,groupName,description,response,StartTime,EndTime,CustomerId,PageName,smsMSGID)
                values('" + messageId + "','" + mob + "','" + groupName + "','" + description + "','" + resp + "','" + StartTime + "','" + EndTime + "','" + CustomerId + "','" + PageName + "','" + msgid + "')";
                database.ExecuteNonQuery(sql);

                if (hmilotp == "1")
                {
                    string sql1 = @"insert into SMSFailOverOBDsent (profileid,msgtext,tomobile,sentdatetime,msgid,delivery_status,obdmsgid,statusDesc)
                    values('" + CustomerId + "','" + msgtext + "','" + mob + "','" + SentTime + "','" + messageId + "','" + status + "','" + msgid + "','" + description + "')";
                    database.ExecuteNonQuery(sql1);
                }
            }
            catch (Exception ex)
            {
                Info_Err3("SaveApiCallResponse 1 - " + ex.Message);
                throw;
            }
        }


        public string SendSMSthroughAPI(string profileid, string msg, string senderId, string peid, string mob)
        {
            string pwd = Convert.ToString(database.GetScalarValue("select pwd from CUSTOMER with (nolock) where username='" + profileid + "'"));
            string url = "https://hci.myinboxmedia.in/api/mim/SendSMS?userid=" + profileid + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + senderId + "&msg=" + msg + "&msgtype=13&peid=" + peid;
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
                return getResponseTxt;
            }
            catch (Exception EX)
            {
                throw EX;
            }
            return "";
        }

        public DataTable GetCrauselSuggetion(string pTemplateId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = @"SELECT  CASE
                    WHEN SuggetionType = '1' THEN 'REPLY'
                    WHEN SuggetionType = '2' THEN 'OPEN_URL'

                    WHEN SuggetionType = '3' THEN 'DIAL_PHONE'
                    WHEN SuggetionType = '4' THEN 'SHOW_LOCATION'

                    WHEN SuggetionType = '5' THEN 'REQUEST_LOCATION'
                END AS SuggestionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude FROM CarouselSuggetion where CarouselId='" + pTemplateId + @"' and isnull(active,0)=1 ";
                dt = database.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                //_Log("GetWabaInProcessRecord : " + ex.Message);
                throw;
            }
        }

        public DataTable GetCardSuggetion(string CardId)
        {

            DataTable dt = new DataTable();
            try
            {
                string sql = @"SELECT  CASE
                    WHEN SuggetionType = '1' THEN 'REPLY'
                    WHEN SuggetionType = '2' THEN 'OPEN_URL'

                    WHEN SuggetionType = '3' THEN 'DIAL_PHONE'
                    WHEN SuggetionType = '4' THEN 'SHOW_LOCATION'

                    WHEN SuggetionType = '5' THEN 'REQUEST_LOCATION'
                END AS SuggestionType, SuggestionText, SuggestionUrl, SuggestionPhone, SuggestionLatitude, SuggestionLongitude FROM CardSuggetion where CardId='" + CardId + @"' and isnull(active,0)=1 ";
                dt = database.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                //_Log("GetWabaInProcessRecord : " + ex.Message);
                throw;
            }
        }

        #region Add By Vikas

        public void FetchfailOverRecordEmail()
        {
            try
            {
                string EmailDbName = ConfigurationManager.AppSettings["mimEMAIL"].ToString();
                string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("SELECT LastSMSFailOverEmailtime FROM settings WOTH(NOLOCK) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                database.ExecuteNonQuery("UPDATE settings SET LastSMSFailOverEmailtime='" + currTime + "'");

                //fetch Failed Records of EMAIL
                string sql = @"SELECT * FROM SMSFailOverEMAIL WITH(NOLOCK) INNER JOIN CUSTOMER WITH(NOLOCK) ON profileid=username WHERE SENTDATETIME>='" + LastProcessedTime + "' AND SENTDATETIME<'" + currTime + @"'";

                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string EmailUserId = Convert.ToString(dr["EmailUserId"]);
                        string ApiKey = Convert.ToString(database.GetScalarValue("SELECT UM_ApiKEY FROM " + EmailDbName + "..USER_MASTER WOTH(NOLOCK) WHERE UM_Profile_Id='" + EmailUserId + "'"));
                        SendEMAILthroughAPI(EmailUserId, ApiKey, dr["EMAIL_FROM"].ToString(), dr["EMAIL_TO"].ToString(), dr["EMAIL_CC"].ToString(), dr["EMAIL_SUBJECT"].ToString(), dr["MSGTEXT"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Info_Err3("FetchfailOverRecordEmail- " + ex.Message);
            }
        }

        public void SendEMAILthroughAPI(string profileid, string ApiKey, string From, string To, string CC, string Subject, string msgtext)
        {
            try
            {
                string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var client = new RestClient("http://emailapi.myinboxmedia.in/api/SendEmailAPI");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
" + "\n" +
                @"    ""ProfileId"": """ + profileid + @""",
" + "\n" +
                @"    ""APIKey"": """ + ApiKey + @""",
" + "\n" +
                @"    ""From"": """ + From + @""",
" + "\n" +
                @"    ""To"": """ + To + @""",
" + "\n" +
                @"    ""CC"": """ + CC + @""",
" + "\n" +
                @"    ""Subject"": """ + Subject + @""",
" + "\n" +
                @"    ""Text"": """ + msgtext + @"""
" + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                logAPIEmail("req-" + body + " resp-" + response.Content);

                try
                {

                }
                catch (Exception ex)
                {
                    Info_Err3("SendEMAILthroughAPI 1 - " + ex.Message);
                }
            }
            catch (Exception e1) { Info_Err3("SendEMAILthroughAPI 2 - " + e1.Message); }
        }

        public void SendOBDthroughAPIOnCall_VCon(string mob, string msgT, string CustomerId, string msgid, string PageName = "", string authkey = "", string hmilotp="")
        {
            string SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (mob.Length >= 10)
            {
                mob = mob.Substring(mob.Length - 10);
            }
            var client = new RestClient("http://103.73.188.151/OBDAPI/webresources/CreateOBDCampaignPost");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
            ""sourcetype"": ""0"",
            ""customivr"": true,
            ""campaigntype"": ""4"",
            ""filetype"": ""2"",
            ""ukey"": """ + ConfigurationManager.AppSettings["Vconukey"].ToString() + @""",
            ""serviceno"": """ + ConfigurationManager.AppSettings["VConserviceno"].ToString() + @""",
            ""ivrtemplateid"": """ + ConfigurationManager.AppSettings["VConivrtemplateid"].ToString() + @""",
            ""isrefno"": true,
            ""retryatmpt"": 1,
            ""retryduration"": 0,
            ""msisdnlist"": [{
                ""phoneno"": """ + mob + @""",
                ""Text"": """ + msgT + @"""
            }]
        }";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            String StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            logAPI("req-" + body + " resp-" + response.Content);
            try
            {
                //OBDRootVcon res = JsonConvert.DeserializeObject<OBDRootVcon>(response.Content);
                //string Responsemsgid = res.refno.First().Key + ": " + res.refno.First().Value;
                String EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                SaveApiCallResponse("", mob, "", "", response.Content, StartTime, EndTime, CustomerId, PageName, msgid, msgT, SentTime, msg.status, hmilotp);
            }
            catch (Exception ex)
            {
                Info_Err3("SendOBDthroughAPIOnCall_VCon  - " + ex.Message);
            }
        }

        public void FetchWABAfailOverOBDRecord()
        {
            try
            {
                string WABADBO = System.Configuration.ConfigurationManager.AppSettings["WABADBO"].ToString();
                string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("SELECT LastWABAFailOverOBDtime FROM settings WITH(NOLOCK) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                database.ExecuteNonQuery("UPDATE settings SET LastWABAFailOverOBDtime='" + currTime + "'");

                //fetch Failed Records of WABA
                string sql = @"SELECT M.tomobile,M.msgtext,M.profileid,S.msgid FROM " + WABADBO + @"tblWABASubmitted M WITH (NOLOCK) 
                LEFT JOIN " + WABADBO + @"tblWABADelivery D WITH (NOLOCK) ON M.messageId=D.msgid 
                INNER JOIN " + WABADBO + @"WABAFailOverOBD S ON M.messageId=S.MSGID
                LEFT JOIN " + WABADBO + @"WABAFailOverOBDsent t ON M.messageId=t.MSGID
                INNER JOIN customer C ON C.wabaProfileId=S.profileid
                WHERE (( D.inserttime>='" + LastProcessedTime + "' AND D.inserttime<'" + currTime + @"' AND D.status_groupname NOT IN ('QUEUED','READ','SENT','DELIVERED') )
                OR ( (datediff(second, M.CreatedDate,getdate())>=C.FailOverOBDSecond) AND D.status_groupname IS NULL ))
                and t.msgid is null
                GROUP BY M.tomobile,M.msgtext,M.profileid,S.MSGID ";
                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(Check) == "Knowlarity")
                        {
                            SendOBDthroughAPIOnCall_Knowlarity(dr["tomobile"].ToString(), dr["msgtext"].ToString(), dr["profileid"].ToString(), dr["msgid"].ToString(), "", "");
                        }
                        if (Convert.ToString(Check) == "infobip")
                        {
                            SendOBDthroughAPIOnCall(dr["tomobile"].ToString(), dr["msgtext"].ToString(), dr["profileid"].ToString(), dr["msgid"].ToString(), "", "");
                        }
                        if (Convert.ToString(Check) == "VCon")
                        {
                            SendOBDthroughAPIOnCall_VCon(dr["tomobile"].ToString(), dr["msgtext"].ToString(), dr["profileid"].ToString(), dr["msgid"].ToString(), "", "");
                        }
                        SaveOBDApiCallRequestResponse(dr["profileid"].ToString(), dr["msgtext"].ToString(), dr["tomobile"].ToString(), dr["msgid"].ToString(), "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Info_Err3("FetchWABAfailOverOBDRecord- " + ex.Message);
            }
        }

        public void SaveOBDApiCallRequestResponse(string profileid, string msgtext, string mobile, string msgid, string dlvrstatus, string statdesc)
        {
            try
            {
                string WABADBO = System.Configuration.ConfigurationManager.AppSettings["WABADBO"].ToString();
                string sql = @"INSERT INTO " + WABADBO + @"WABAFailOverOBDsent (profileid,msgtext,tomobile,msgid,delivery_status,statusDesc)
                VALUES('" + profileid + "','" + msgtext + "','" + mobile + "','" + msgid + "','" + dlvrstatus + "','" + statdesc + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Info_Err3("SaveOBDApiCallRequestResponse 1 - " + ex.Message);
                throw;
            }
        }
        #endregion
    }

    public class Util1
    {
        public DataTable GetCrausaldetail(string pTemplateId)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = @"select * from dbo.Fn_GetTemplate('" + pTemplateId + @"')";
                dt = database.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                //_Log("GetWabaInProcessRecord : " + ex.Message);
                throw;
            }
        }
    }

    public class OBDRootVcon
    {
        public string Status { get; set; }
        public Dictionary<string, long> refno { get; set; }
        public int inserted { get; set; }
        public int rejected { get; set; }
        public string Value { get; set; }
        public string value { get; set; }
        public string status { get; set; }
        public long leadid { get; set; }
    }
}