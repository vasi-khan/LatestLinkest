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
    [RoutePrefix("api/call")]
    public class callController : ApiController
    {
        // GET: api/test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/test/5
        [HttpGet]
        public string Get(int d)
        {
            return "value";
        }

        [Route("smsdata")]
        [HttpGet]               // USED FOR MINAVO LONGCODE - SAVE LONGCODE DETAILS IN 128 SERVER - SMPPSERVER
        public string smsdata( string longCodeNo, string mobileNo, string receivedDateTime, string msgText)
        {
            //if (Convert.ToInt16(dbClient128.GetScalarValue("Select count(*) from Settingmast where apikey='" + apikey + "' and pwd='" + pwd + "' ")) > 0)
            //{
                string sql = string.Format("Insert into smsLongCode (longCodeNo,mobileNo,receivedDateTime,msgText) " +
                   " Values('{0}','{1}','{2}','{3}')", longCodeNo, mobileNo, receivedDateTime, msgText.Replace("'", "''"));

                dbClient128.ExecuteNonQuery(sql);
                return "Success";
            //}
            //else
            //{
            //    return "Invalid Credentials.";
            //}
        }

        [Route("smsdata2")]
        [HttpGet]               // USED FOR MINAVO LONGCODE - SAVE LONGCODE DETAILS IN 128 SERVER - SMPPSERVER
        public string smsdata2(string apikey, string pwd, string longCodeNo, string mobileNo, string receivedDateTime, string msgText)
        {
            if (Convert.ToInt16(dbClient128.GetScalarValue("Select count(*) from Settingmast where apikey='" + apikey + "' and pwd='" + pwd + "' ")) > 0)
            {
                string sql = string.Format("Insert into smsLongCode (longCodeNo,mobileNo,receivedDateTime,msgText) " +
                   " Values('{0}','{1}','{2}','{3}')", longCodeNo, mobileNo, receivedDateTime, msgText.Replace("'", "''"));

                dbClient128.ExecuteNonQuery(sql);
                return "Success";
            }
            else
            {
                return "Invalid Credentials.";
            }
        }

        [Route("calldata")]
        [HttpGet]               // USED FOR MINAVO CLICK TO CALL - SAVE CALL DETAILS
        public string calldata(string apikey, string pwd, string mobile1, string mobile2, string landlinenumber, string callrequesttime, string callstatus, string callstarttime, string callendtime, string callduration, string calldisconnectby, string callrecordingurl)
        {
            if (Convert.ToInt16(dbClient.GetScalarValue("Select count(*) from setting where apikey='" + apikey + "' and pwd='" + pwd + "' ")) > 0)
            {
                string sql = string.Format("Insert into CallData (mobile1,mobile2,landlinenumber,callrequesttime,callstatus,callstarttime,callendtime,callduration,calldisconnectby,callrecordingurl) " +
                   " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", mobile1, mobile2, landlinenumber, callrequesttime, callstatus, callstarttime, callendtime, callduration, calldisconnectby, callrecordingurl);

                dbClient.ExecuteNonQuery(sql);
                return "Success";
            }
            else
            {
                return "Invalid Credentials.";
            }
        }

        [Route("calldata")]
        [HttpGet]               // USED FOR MINAVO CLICK TO CALL - SAVE CALL DETAILS WITH LANGUAGE
        public string calldata(string apikey, string pwd, string mobile1, string mobile2, string landlinenumber, string callrequesttime, string callstatus, string callstarttime, string callendtime, string callduration, string calldisconnectby, string language, string callrecordingurl)
        {
            if (Convert.ToInt16(dbClient.GetScalarValue("Select count(*) from setting where apikey='" + apikey + "' and pwd='" + pwd + "' ")) > 0)
            {
                string sql = string.Format("Insert into CallData (mobile1,mobile2,landlinenumber,callrequesttime,callstatus,callstarttime,callendtime,callduration,calldisconnectby,callrecordingurl,language) " +
                   " Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", mobile1, mobile2, landlinenumber, callrequesttime, callstatus, callstarttime, callendtime, callduration, calldisconnectby, callrecordingurl, language);

                dbClient.ExecuteNonQuery(sql);

                CallAPI(mobile2, language);

                return "Success";
            }
            else
            {
                return "Invalid Credentials.";
            }
        }

        public void CallAPI(string mobile, string lang)
        {
            string url = "";
            //if (lang.Trim().ToLower() == "hindi")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=कैस्ट्रोल सुपर मैकेनिक कॉन्टेस्ट में आपका स्वागत है। व्हाट्सएप पर अपडेट के लिए क्लिक करें:- https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403225080972";

            //if (lang.Trim().ToLower() == "marathi")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=कैस्ट्रोल सुपर मेकॅनिक कॉन्टेस्ट मध्ये आपलं स्वागत आहे। व्हाट्सअँपवर अपडेट करिता क्लिक करा:- https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403230418846";

            //if (lang.Trim().ToLower() == "gujrati" || lang.Trim().ToLower() == "gujarati")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=કેસ્ટ્રોલ સુપર મેકેનિક કોન્ટેસ્ટમાં આપનું સ્વાગત છે. whatsapp પર માહિતી મેળવવા માટે ક્લિક કરો. https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403238600216";

            //if (lang.Trim().ToLower() == "bengali")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=ক্যাস্ট্রল সুপার মেকানিক প্রতিযোগিতায় আপনাকে স্বাগত। হোয়াটসঅ্যাপে আপডেটের জন্য নিচে দেওয়া লিংকে ক্লিক করুন https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403244944168";

            //if (lang.Trim().ToLower() == "tamil")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=காஸ்ட்ரோல் சூப்பர் மெக்கானிக் போட்டிக்கு வரவேற்கிறோம். வாட்ஸ்அப்பில் புதுப்பிப்புகளுக்கு கிளிக் செய்யவும்:- https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403255425580";

            //if (lang.Trim().ToLower() == "telgu")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=కాస్ట్రోల్ సూపర్ మెకానిక్ కాంటెస్ట్‌కు స్వాగతం. వాట్సాప్‌లో అప్డేట్స్ పొందడానికి క్రింద లింక్‌ క్లిక్‌ చేయండి:- https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403261496326";

            //if (lang.Trim().ToLower() == "kannada")
            //    url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=ಕ್ಯಾಸ್ಟ್ರೋಲ್ ಸೂಪರ್ ಮೆಕ್ಯಾನಿಕ್ ಸ್ಪರ್ಧೆಗೆ ಸ್ವಾಗತ. ವಾಟ್ಸಾಪ್‌ನಲ್ಲಿ ಅಪ್‌ಡೇಟ್‌ಗಳಿಗಾಗಿ https://m1m.io/801D2567 ಕ್ಲಿಕ್ ಮಾಡಿ TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403269816940";

            if (lang.Trim().ToLower() == "hindi")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=धन्यवाद कैस्ट्रोल सुपर मैकेनिक में भाग लेने के लिए कॉल करें 1800-5325-999 और व्हाट्सएप अपडेट के लिए क्लिक करें https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163549094746062";

            if (lang.Trim().ToLower() == "marathi")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=धन्यवाद कॅस्ट्रॉल सुपर मेकॅनिकमध्ये सहभागी होण्यासाठी 1800-5325-999 वर कॉल करा आणि WhatsApp अपडेटसाठी क्लिक करा https://m1m.io/801D2567 TVNINE &msgtype=13&peid=1701159828169631307&templateid=1707163549032457860";

            if (lang.Trim().ToLower() == "gujrati" || lang.Trim().ToLower() == "gujarati")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=આભાર કેસ્ટ્રોલ સુપર મિકેનિકમાં હાજરી આપવા માટે 1800-5325-999 પર કૉલ કરો અને WhatsApp અપડેટ માટે ક્લિક કરો https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163549040392131";

            if (lang.Trim().ToLower() == "bengali")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=ধন্যবাদ ক্যাস্ট্রল সুপার মেকানিক কনটেস্টে অংশগ্রহণের জন্য কল করুন ১৮০০-৫৩২৫-৯৯৯ অথবা হোয়াটস্যাপে আপডেট পেতে নীচের লিংকে ক্লিক করুন https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163549082669577";

            if (lang.Trim().ToLower() == "tamil")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=நன்றி காஸ்ட்ரோல் சூப்பர் மெக்கானிக்கில் கலந்துகொள்ளவதற்கு 1800-5325-999 என்ற எண்ணை அழைக்கவும் மற்றும் வாட்ஸ்அப் அப்டேட்டை கிளிக் செய்யவும் https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163549088059158";

            if (lang.Trim().ToLower() == "telgu")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=ధన్యవాదాలు.. కాస్ట్రోల్ సూపర్ మెకానిక్ కాంటెస్ట్‌లో పాల్గొనడానికి 1800-5325-999కి కాల్ చేయండి మరియు వాట్సాప్‌లో అప్‌డేట్‌ల కోసం క్లిక్ చేయండి https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163549057674910";

            if (lang.Trim().ToLower() == "kannada")
                url = "https://myinboxmedia.in/api/mim/SendSMS?userid=MIM2102181&pwd=45C8$D2D_778&mobile=" + mobile + "&sender=TVNINE&msg=Hello, Welcome to Castrol Super Mechanic Contest. To receive updates on WhatsApp please click on the link below:- https://m1m.io/801D2567 TVNINE&msgtype=13&peid=1701159828169631307&templateid=1707163403109539735";


            //string url = "https://myinboxmedia.in/api/mim/SendSMS?userid=" + username.Trim() + "&pwd=" + pwd.Trim() + "&mobile=" + mob.Trim() + "&sender=" + senderid.Trim() + "&msg=" + msg + "&msgtype=13&peid=" + pid.Trim() + "&templateid=" + tempateid.Trim();
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
                //Log("", getResponseTxt);
            }
            catch (Exception EX)
            {
                throw EX;
            }

        }


        [Route("GetMissedCallData")]
        [HttpPost]    //USED FOR KNOWLARITY DATA. API HOOKED TO KNOWLARITY PANEL.
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
