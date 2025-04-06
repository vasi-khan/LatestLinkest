
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

namespace MimSenddata
{
    public class To
    {
        public string phoneNumber { get; set; }
    }

    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Message
    {
        public To to { get; set; }
        public Status status { get; set; }
        public string messageId { get; set; }
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
                    if (num==1)
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
                                        " select '',0,'','','','"+dlrStatus+"','','','','','','','','',0,@MSGID,getdate(),1,getdate(),'" + mob + "','',getdate() ";
                    string sqlF = sql + selectsql+ sqldlr;
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

        public void RCSApiAN(string pRcsMsgRcvdId, string mob, string authkey,  string msgtext,  string pUserId,string SessionId, string acid,string apiurl,string pTempletId="")
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
        public void rcsimgAN(string pRcsMsgRcvdId, string mob, string authkey, string msgtext,string imgurl, string pUserId,string SessionId,string acid, string apiurl, string pTempletId = "")
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

            string pStatus=response_img.StatusCode.ToString();
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
        public void RCSCard(string pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId, string pUserId, string SessionId,string acid, string apiurl)
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
                  ""from"": """+ acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {
                  ""orientation"":"""+dt.Rows[0]["CardOrientation"].ToString()+@""",
                  ""alignment"": """+dt.Rows[0]["CardAlignment"].ToString()+@""",
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
                                ""label"": """+dtCardSuggetion.Rows[p]["SuggestionText"].ToString()+@""",
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
        public void RCSCarousalAN_b422APR(string  pRcsMsgRcvdId, string mob, string authkey, string msgtext, string pTempletId,string pUserId,string SessionId, string acid, string apiurl)
        {
            var client = new RestClient(apiurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            var body = "";
            Util1 u1 = new Util1();
           DataTable dt= u1.GetCrausaldetail(pTempletId);
            var body_img = "";
            var body_img1 = "";
            var body_imgF = "";
            var body_imgH = @"{
                  ""from"": """+ acid + @""",
                  ""to"": """ + mob + @""",
                  ""validityPeriod"": 15,
                  ""validityPeriodTimeUnit"": ""MINUTES"",
                  ""content"": {


      ""cardWidth"":""MEDIUM"",
      ""contents"":[";

            for (int j = 0; j < dt.Rows.Count; j++)
                {

                if (j>0)
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
            body= body_imgH + body_img1 + body_imgF;
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

        public DataTable GetRCSRecords(int no_of_record, string picktime,int SessionId)
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
                                    (select top " + no_of_record.ToString() + @" * from rcstran WITH(NOLOCK) where sessionid=" + SessionId+ @" and PickedTime IS NULL ORDER BY CreatedDate 
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
            string sql = "Delete from rcstran where SessionId="+i+" and PickedTime is not null ";
            database.ExecuteNonQuery(sql);
        }

        public void FetchfailOverRecord()
        {
            try
            {
                string LastProcessedTime = Convert.ToDateTime(database.GetScalarValue("Select LastRCSFAILOVERSMStime from settings with (nolock) ")).ToString("yyyy-MM-dd HH:mm:ss.fff");
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                database.ExecuteNonQuery("Update settings set LastRCSFAILOVERSMStime='" + currTime + "'");

                

                //fetch Failed Records of SMS
                string sql = @"select m.tomobile,m.messageId,m.msgtext,m.UserId from tblRCSMSGSUBMITTED M WITH (NOLOCK) INNER JOIN DELIVERYRCS D WITH (NOLOCK) ON M.messageId=D.MSGID 
                where d.inserttime>='" + LastProcessedTime + "' and d.inserttime<'" + currTime + @"' and d.status_groupName<>'DELIVERED'";

                DataTable dt = database.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SendSMSthroughAPI(dr["UserId"].ToString(), dr["msgtext"].ToString(), "HOMECR", "110100001881", dr["tomobile"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Info_Err3("fetchrcsFailOver- " + ex.Message);
            }
        }

        public string SendSMSthroughAPI(string profileid, string msg, string senderId, string peid, string mob)
        {

            string pwd = Convert.ToString(database.GetScalarValue("select pwd from CUSTOMER with (nolock) where username='" + profileid + "'"));
            string url = "https://hci.myinboxmedia.in/api/mim/SendSMS?userid=" + profileid + "&pwd=" + pwd + "&mobile=" + mob + "&sender=" + senderId + "&msg=" + msg + "&msgtype=13&peid=" + peid ;
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
    }

    public class Util1
    {

     //   public DataTable GetCrausaldetail(string pTemplateId)
     //   {

     //       DataTable dt = new DataTable();
     //       try
     //       {
     //           string sql = @"
     //           select
     //           CASE
     //               WHEN CardWidth = '1' THEN 'MEDIUM'
     //               WHEN CardWidth = '2' THEN 'SMALL'
     //           END AS CardWidth,
     //           CardTitle,
     //           CardDesc,
     //           FileUrl,
     //           FilePath,
     //           CASE
     //               WHEN CardHeight = '1' THEN 'SHORT'
     //               WHEN CardHeight = '2' THEN 'MEDIUM'
     //               WHEN CardHeight = '3' THEN 'TALL'
     //           END AS CardHeight,
	    //         CASE
     //               WHEN CardOrientation = '1' THEN 'HORIZONTAL'
     //               WHEN CardOrientation = '2' THEN 'VERTICAL'
     //           END AS CardOrientation,
				 //CASE
     //               WHEN CardAlignment = '1' THEN 'LEFT'
     //               WHEN CardAlignment = '2' THEN 'RIGHT'
     //           END AS CardAlignment,
     //           SuggestionText,
     //           CASE
     //               WHEN SuggestionType = '1' THEN 'REPLY'
     //               WHEN SuggestionType = '2' THEN 'OPEN_URL'

     //               WHEN SuggestionType = '3' THEN 'DIAL_PHONE'
     //               WHEN SuggestionType = '4' THEN 'SHOW_LOCATION'

     //               WHEN SuggestionType = '5' THEN 'REQUEST_LOCATION'
     //           END AS SuggestionType,
     //           SuggestionUrl,
     //           FilePath SuggestionUrlPath,
     //           SuggestionPhone,
     //           SuggestionLatitude,
     //           SuggestionLongitude,
     //           SuggestionLabel
     //           from RcsTemplateDetail with(nolock) Where TemplateID= '" + pTemplateId + @"'";
     //           //string sql = @"SELECT distinct MobileNo,Id FROM WABASESSION where LASTRECEIVEDAT>=DATEADD(HOUR,-24,GETDATE()) and mobileno='917678936834'";
     //           dt = database.GetDataTable(sql);
     //           return dt;
     //       }
     //       catch (Exception ex)
     //       {
     //           //_Log("GetWabaInProcessRecord : " + ex.Message);
     //           throw;
     //       }
     //   }
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


}





