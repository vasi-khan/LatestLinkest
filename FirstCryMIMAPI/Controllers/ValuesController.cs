using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstCryMIMAPI.Helper;

namespace FirstCryMIMAPI.Controllers
{
    [RoutePrefix("api/mim")]
    public class ValuesController : ApiController
    {
        //with PEID & TemplateID  for FirstCryOnly 
        [Route("SendSMS")]
        [HttpGet]
        public string SendSMSFc(string userid, string pwd, string mobile, string sender, string msg, string msgtype, string peid, string templateid)
        {
            try
            {
                if (userid == "MIM2201009" || userid == "MIM2201010" || userid == "MIM2201011" || userid == "MIM2201194")
                {
                    if (pwd.Trim().ToUpper() != "FIRSTCRY321") { return "Invalid Password"; }

                    string msgid = GetMsgID();

                    string sql = "insert into MSGQUEUE_FC (PROFILEID,MSGTEXT,TOMOBILE,SENDERID,peid,templateid,msgidClient) " +
                    " VALUES ('" + userid + "',N'" + msg.Replace("'", "''") + "','" + mobile + "','" + sender + "','" + peid + "','" + templateid + "','" + msgid + "')";
                    database.ExecuteNonQuery(sql);

                    return "SMS Submitted Successfully. Message ID: " + msgid;
                }
                else return "Invalid User ID";
            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }
        public string GetMsgID()
        {
            return 'S' + Convert.ToString(Guid.NewGuid());
        }
    }
}
