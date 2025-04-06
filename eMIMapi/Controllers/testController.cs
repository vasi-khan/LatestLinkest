using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using eMIMapi.Helper;
namespace eMIMapi.Controllers
{
    [RoutePrefix("api/misscall")]
    public class testController : ApiController
    {
        //[Route("GetMissedCallData")]
        //[HttpGet]
        //public string GetMissedCallData(string dispnumber, string caller_id, string call_id, string start_time, string end_time, string timezone, string call_duration, string destination, string action, string extension)
        //{
        //    string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion)" +
        //        " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", dispnumber, caller_id, call_id, start_time, end_time, timezone, call_duration, destination, action, extension);

        //    dbSony.ExecuteNonQuery(sql);

        //    return "success";

        //}

        //[Route("GetMissedCallData")]
        //[HttpGet]
        //public string GetMissedCallData(string dispnumber, string extension, string callid, string call_duration, string destination, string caller_id, string PWD, string end_time, string action, string timezone, string resource_url, string type, string start_time, string UID)
        //{
        //    string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion)" +
        //        " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", dispnumber, caller_id, callid, start_time, end_time, timezone, call_duration, destination, action, extension);

        //    dbSony.ExecuteNonQuery(sql);

        //    return "success";

        //}



        // GET: api/test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //MissCall ob=null;

            //bool log = false;
            //string contestid = "2";
            //DataTable dt = dbSony.GetDataTable("Select * from usermast where contestid='" + contestid + "'");
            ////check date
            //if (Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)) >= Convert.ToDateTime(dt.Rows[0]["ContestFrom"]) &&
            //   Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)) <= Convert.ToDateTime(dt.Rows[0]["ContestTo"]))
            //{
            //    string fld = "ContestDay" + DateTime.Now.DayOfWeek.ToString().Substring(0, 3);
            //    if (Convert.ToInt16(dt.Rows[0][fld]) == 1)
            //    {
            //        if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture)) >= Convert.ToDateTime(dt.Rows[0]["ContestTimeFrom"]) &&
            //       Convert.ToDateTime(DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture)) <= Convert.ToDateTime(dt.Rows[0]["ContestTimeTo"]))
            //        {
            //            if (Convert.ToInt16(dt.Rows[0]["OneCallInADay"]) == 1)
            //            {
            //                if (Convert.ToInt16(dbSony.GetScalarValue("Select count(*) from MissedCallData where contestid='" + contestid + "' AND caller_id='" + ob.caller_id.Trim() + "' and convert(varchar,inserttime,102) = convert(varchar,getdate(),102) and replace(caller_id,'+91','') not in (Select Mobile from TestCheck) ")) > 0)
            //                {
            //                    //Helper.Util ob1 = new Util();
            //                    //ob1.SendSMSthroughAPI(ob.caller_id.Trim().Replace("+91", ""), dt.Rows[0]["SMSTEXTONREMISSCALL"].ToString(), dt.Rows[0]["PROFILEID"].ToString(), dt.Rows[0]["apikey"].ToString(), dt.Rows[0]["senderid"].ToString(), dt.Rows[0]["peid"].ToString());

            //                }
            //            }

            //            string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
            //                " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid);

            //            dbSony.ExecuteNonQuery(sql);

            //            string mob = ob.dispnumber.Replace("+91", "");
            //            string dtmf = "0";
            //            if (mob == "8010558822") dtmf = "1";
            //            if (mob == "8010558833") dtmf = "2";
            //            //insert into OBDRESPONSE 
            //            sql = "Insert into obdresponse (CAMPAIGNID,MSISDN,CLI,FLAG,STATUS,STARTTIME,DURATION,DTMF,INSERTDATE,SMSTOBESENT,SMSSENT,RETRYNO,contestid) " +
            //                " select '2','" + ob.caller_id.Trim() + "','2','','ANSWERED',convert(varchar,getdate(),120),'1','" + dtmf + "',getdate(),'0','0','0','" + contestid + "'";
            //            dbSony.ExecuteNonQuery(sql);

            //            if (Convert.ToInt16(dt.Rows[0]["SMSToBeSentAftCall"]) == 1)
            //            {
            //                Helper.Util ob1 = new Util();
            //                ob1.SendSMSthroughAPI(ob.caller_id.Trim().Replace("+91", ""), dt.Rows[0]["SMSText"].ToString(), dt.Rows[0]["PROFILEID"].ToString(), dt.Rows[0]["apikey"].ToString(), dt.Rows[0]["senderid"].ToString(), dt.Rows[0]["peid"].ToString());
            //            }

            //        }
            //        else log = true;
            //    }
            //    else log = true;
            //}
            //else log = true;

            //if (log)
            //{
            //    string sql = string.Format("Insert into MissedCallDataLog (dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
            //                " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid);

            //    dbSony.ExecuteNonQuery(sql);
            //}


            return new string[] { "value1", "value2" };
        }

        // GET: api/test/5
        [HttpGet]
        public string Get(int d)
        {
            return "value";
        }
        //[Route("GetMissedCallData3")]
        //[HttpPost]
        ////POST: api/test
        //public string Post([FromBody]string value)
        //{
        //    dbSony.ExecuteNonQuery("Insert into tempT values ('" + value + "') ");
        //    return "OK";
        //}

        //[Route("GetMissedCallData4")]
        //[HttpPost]
        //// POST: api/test
        //public string Post([FromBody]string dispnumber, string caller_id, string call_id, string start_time, string end_time, string timezone, string call_duration, string destination, string action, string extension)
        //{
        //    string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion)" +
        //        " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", dispnumber, caller_id, call_id, start_time, end_time, timezone, call_duration, destination, action, extension);

        //    dbSony.ExecuteNonQuery(sql);

        //    return "success";

        //}


        //[Route("GetMissedCallData2")]
        //[HttpPost]
        //// POST: api/test
        //public string Post([FromBody]MissCall ob)
        //{
        //    string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion)" +
        //       " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension);

        //    dbSony.ExecuteNonQuery(sql);

        //    return "success";
        //    //return "OK";
        //}

        [Route("GetMissedCallData")]
        [HttpPost]
        public string GetMissedCallData(MissCall ob)  //USED FOR SONY TEN CONTEST ....
        {
            DataTable dt = dbSony.GetDataTable("Select * from usermast where contestid='1'");

            if (Convert.ToInt16(dt.Rows[0]["OneCallInADay"]) == 1)
            {
                if (Convert.ToInt16(dbSony.GetScalarValue("Select count(*) from MissedCallData where contestid='1' and caller_id='" + ob.caller_id.Trim() + "' and convert(varchar,inserttime,102) = convert(varchar,getdate(),102) and replace(caller_id,'+91','') not in (Select Mobile from TestCheck) ")) > 0)
                {
                    Helper.Util ob1 = new Util();
                    ob1.SendSMSthroughAPI(ob.caller_id.Trim().Replace("+91", ""), dt.Rows[0]["SMSTEXTONREMISSCALL"].ToString(), dt.Rows[0]["PROFILEID"].ToString(), dt.Rows[0]["apikey"].ToString(), dt.Rows[0]["senderid"].ToString(), dt.Rows[0]["peid"].ToString());
                    return "success";
                }
            }

            string sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
                " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, "1");

            dbSony.ExecuteNonQuery(sql);

            return "success";
        }


        [Route("GetMissedCallData1")]
        [HttpPost]
        public string GetMissedCallData1(MissCall ob)  //USED FOR SONY BAALVEER CONTEST ....
        {
            string sql = "";
            bool log = false;
            string contestid = "2";
            DataTable dt = dbSony.GetDataTable("Select * from usermast where contestid='" + contestid + "'");
            //check date
            if (Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)) >= Convert.ToDateTime(dt.Rows[0]["ContestFrom"]) &&
               Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)) <= Convert.ToDateTime(dt.Rows[0]["ContestTo"]))
            {
                string fld = "ContestDay" + DateTime.Now.DayOfWeek.ToString().Substring(0, 3);
                if (Convert.ToInt16(dt.Rows[0][fld]) == 1)
                {
                    if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture)) >= Convert.ToDateTime(dt.Rows[0]["ContestTimeFrom"]) &&
                   Convert.ToDateTime(DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture)) <= Convert.ToDateTime(dt.Rows[0]["ContestTimeTo"]))
                    {
                        if (Convert.ToInt16(dt.Rows[0]["OneCallInADay"]) == 1)
                        {
                            if (Convert.ToInt16(dbSony.GetScalarValue("Select count(*) from MissedCallData where contestid='" + contestid + "' AND caller_id='" + ob.caller_id.Trim() + "' and convert(varchar,inserttime,102) = convert(varchar,getdate(),102) and replace(caller_id,'+91','') not in (Select Mobile from TestCheck) ")) > 0)
                            {
                                sql = string.Format("Insert into MissedCallDataLog (dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID,WithinTime)" +
                                " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid, "1");

                                dbSony.ExecuteNonQuery(sql);

                                //Helper.Util ob1 = new Util();
                                //ob1.SendSMSthroughAPI(ob.caller_id.Trim().Replace("+91", ""), dt.Rows[0]["SMSTEXTONREMISSCALL"].ToString(), dt.Rows[0]["PROFILEID"].ToString(), dt.Rows[0]["apikey"].ToString(), dt.Rows[0]["senderid"].ToString(), dt.Rows[0]["peid"].ToString());
                                return "success";
                            }
                        }

                        sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
                            " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid);

                        dbSony.ExecuteNonQuery(sql);

                        string mob = ob.dispnumber.Replace("+91", "");
                        string dtmf = "0";
                        if (mob == "8010558822") dtmf = "1";
                        if (mob == "8010558833") dtmf = "2";
                        //insert into OBDRESPONSE 
                        sql = "Insert into obdresponse (CAMPAIGNID,MSISDN,CLI,FLAG,STATUS,STARTTIME,DURATION,DTMF,INSERTDATE,SMSTOBESENT,SMSSENT,RETRYNO,contestid) " +
                            " select '2','" + ob.caller_id.Trim() + "','2','','ANSWERED',convert(varchar,getdate(),120),'1','" + dtmf + "',getdate(),'0','0','0','" + contestid + "'";
                        dbSony.ExecuteNonQuery(sql);

                        if (Convert.ToInt16(dt.Rows[0]["SMSToBeSentAftCall"]) == 1)
                        {
                            Helper.Util ob1 = new Util();
                            ob1.SendSMSthroughAPI(ob.caller_id.Trim().Replace("+91", ""), dt.Rows[0]["SMSText"].ToString(), dt.Rows[0]["PROFILEID"].ToString(), dt.Rows[0]["apikey"].ToString(), dt.Rows[0]["senderid"].ToString(), dt.Rows[0]["peid"].ToString());
                        }
                        return "success";
                    }
                    else log = true;
                }
                else log = true;
            }
            else log = true;

            if (log)
            {
                sql = string.Format("Insert into MissedCallDataLog (dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
                            " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid);

                dbSony.ExecuteNonQuery(sql);
            }
            return "success";
        }

        [Route("GetMissedCallDataAd360")]
        [HttpPost]
        public string GetMissedCallDataAd360(MissCall ob)  //USED FOR AD360 Global CONTEST ....
        {
            string resp = "";
            if (string.IsNullOrEmpty(ob.caller_id))
            {
                resp = "Caller Id can not be blank";
            }
            string sql = "";
            string contestid = "1";
            if (resp == "")
            {
                sql = string.Format("Insert into MissedCallData(dispnumber,caller_id,call_id,start_time,end_time,timezone,call_duration,destination,action,extentsion,CONTESTID)" +
                          " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", ob.dispnumber, ob.caller_id, ob.call_id, ob.start_time, ob.end_time, ob.timezone, ob.call_duration, ob.destination, ob.action, ob.extension, contestid);

                dbAD360Global.ExecuteNonQuery(sql);

                DataTable dt = dbAD360Global.GetDataTable("select * from templateSetting");
                Util u = new Util();
                if (dt.Rows.Count > 0)
                {
                    string uid = Convert.ToString(dt.Rows[0]["userID"]);
                    string PWD = Convert.ToString(dt.Rows[0]["PWD"]);
                    string senderid = Convert.ToString(dt.Rows[0]["senderid"]);
                    string peid = Convert.ToString(dt.Rows[0]["peid"]);
                    string templateid = Convert.ToString(dt.Rows[0]["templateid"]);
                    string templateText = Convert.ToString(dt.Rows[0]["templateText"]);
                    string mob = ob.caller_id.Replace("+91", "");

                    u.SendSMSthroughAPI(uid, PWD, mob, senderid, templateText, peid, templateid);
                }

            }

            return "success";
        }


        // PUT: api/test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/test/5
        public void Delete(int id)
        {
        }
    }
}
