using eMIMapi.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/callingApp")]
    public class CallingController : ApiController
    {

        [Route("SaveRegistrationDetail")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveRegistrationDetail(RegistrationDtl reg)
        {
            bool isResendOtp = false;
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            reg.apiKey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            if (reg.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (reg.secret == "") { yourJson += "Invalid API Credentials"; serverResponse = "002"; }
            if (yourJson == "")
                if (reg.clientId == "") { yourJson += "Invalid clientID"; serverResponse = "003"; }
            if (yourJson == "")
                if (reg.name == "") { yourJson += "Invalid name"; serverResponse = "004"; }
            if (yourJson == "")
                if (reg.mobile == "") { yourJson += "Invalid Mobile No"; serverResponse = "005"; }
            if (reg.emailId == "") { yourJson += "Invalid Email ID"; serverResponse = "006"; }
            if (yourJson == "")
                if (reg.compName == "") { yourJson += "Invalid company Name"; serverResponse = "007"; }

            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == reg.apiKey.Trim() && Convert.ToString(dt.Rows[0]["Secrete"]) == reg.secret.Trim())
                {
                    if (yourJson == "")
                    {
                        string _clientid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select ClientID from clientMast where ClientID='{0}'", reg.clientId)));
                        if (_clientid != reg.clientId.Trim()) { yourJson += "Invalid clientID "; serverResponse = "003"; }
                    }
                    if (yourJson == "")
                    {
                        int _mobile = Convert.ToInt16(Callingdatabase.GetScalarValue(string.Format("select count(*) from employee where MobileNo='{0}' and ClientID='{1}' ", reg.mobile, reg.clientId)));
                        if (_mobile == 0) { yourJson += "Mobile No not applicable for registration"; serverResponse = "005"; }
                    }
                    if (yourJson == "")
                    {
                        int _email = Convert.ToInt16(Callingdatabase.GetScalarValue(string.Format("select count(*) from employee where EmailID='{0}' and ClientID='{1}' ", reg.emailId, reg.clientId)));
                        if (_email == 0) { yourJson += "Email ID not applicable for registration"; serverResponse = "006"; }
                    }
                    if (yourJson == "")
                    {
                        int _mobile = Convert.ToInt16(Callingdatabase.GetScalarValue(string.Format("select count(*) from RegnRequest where MobileNo='{0}'  and ClientID='{1}' ", reg.mobile, reg.clientId)));
                        if (_mobile > 0)
                        {
                            int cnt = Convert.ToInt16(Callingdatabase.GetScalarValue(string.Format("select count(*) from RegnRequest where MobileNo='{0}'  and ClientID='{1}' and MobileVerifiedOn is not null", reg.mobile, reg.clientId)));
                            if (cnt > 0)
                            { yourJson += "Mobile No already registered"; serverResponse = "009";  }
                            else
                            {
                                isResendOtp = true;
                            }
                        }
                    }

                    flag = true;
                    if (isResendOtp)
                    {
                        string _otp = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select OTP from RegnRequest where MobileNo='{0}'  and ClientID='{1}'", reg.mobile, reg.clientId)));

                        string msg = "Your SMS verification code is:" + _otp;
                        Util ob = new Util();
                        ob.SendSMSthroughAPICallingApp(reg.mobile, msg, reg.clientId);

                        yourJson = "success";
                        serverResponse = "200";
                        flag = false;
                    }
                   
                    if (flag && yourJson == "")
                    {
                        Int64 rnd = (new Random()).Next(1, 99999);
                        string segment = Guid.NewGuid().ToString();
                        segment = segment.Substring(segment.Length - 10).Replace("-", "");

                        string empcode = Convert.ToString(Callingdatabase.GetScalarValue("Select Empcode from Employee where mobileno='" + reg.mobile + "' and clientid='" + reg.clientId + "'"));
                        string sql = string.Format("declare @ReqId varchar(12)='0'; select @ReqId = CAST(ISNULL(MAX(CAST(requestID as INT)),'0') as int) + 1 from RegnRequest where ClientID = '{0}'", reg.clientId);
                        string sql1 = string.Format("; Insert into RegnRequest(ClientID,RequestId,RequestDate,Name,MobileNo,emailID,CompanyName,CountryCode,otp,emailVerificationSegment,Empcode) " +
                            "Values('{0}',@ReqId,GETDATE(),'{1}','{2}','{3}','{4}',(select top 1 CountryCode from Employee WHERE MobileNo='{5}'),'{6}','{7}','{8}')", reg.clientId, reg.name, reg.mobile, reg.emailId, reg.compName, reg.mobile, rnd, segment, empcode);
                        sql += sql1;
                        Callingdatabase.ExecuteNonQuery(sql);

                        // string msg = "Dear " + reg.Name + ", " + rnd.ToString() + " is OTP for your mobile number verification.";
                        string msg = "Your SMS verification code is:" + rnd.ToString();
                        Util ob = new Util();
                        ob.SendSMSthroughAPICallingApp(reg.mobile, msg, reg.clientId);
                        //http://localhost:61451 http://linkext
                        string mailmsg = "Please click on the following link to verify your Email ID - http://linkext.co.in/api/CallingApp/VerifyEmail?segment=" + segment;

                        string res = ob.SendEmailCallingApp(reg.emailId, "Email Verification for Calling App", mailmsg, "noreply@textiyapa.com", "IP#396395", "smtpout.secureserver.net");


                        yourJson = "success";

                        serverResponse = "200";

                    }
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }

            #region Comment DataTableReturn
            //DataTable dt1 = new DataTable();
            //dt1.Columns.Add("ServerResponse");
            //dt1.Columns.Add("Result");
            //dt1.Columns.Add("Description");
            //DataRow dr = dt1.NewRow();
            //dr[0] = serverResponse;
            //dr[1] = yourJson;
            //dr[2] = yourJson;
            //dt1.Rows.Add(dr);
            //yourJson = JsonConvert.SerializeObject(dt1);



            //DataTable dtData = new DataTable();
            //dtData.Columns.Add("Result");
            //dtData.Columns.Add("Description");
            //dtData.Columns.Add("Apikey");

            //DataRow drData = dtData.NewRow();
            //drData[0] = yourJson;
            //drData[1] = yourJson;
            //if (yourJson == "success")
            //    drData[2] = reg.Apikey;
            //else
            //    drData[2] = "";

            //dtData.Rows.Add(drData);

            //DataTable dt2 = new DataTable();
            //dt2.Columns.Add("StatusCode");
            //dt2.Columns.Add("Data", typeof(DataTable));
            //DataRow dr = dt2.NewRow();
            //dr[0] = serverResponse;
            //dr[1] = dtData;
            //dt2.Rows.Add(dr);

            #endregion

            SaveRegnDataApi da = new SaveRegnDataApi()
            {
                result = yourJson,
                description = yourJson
            };
            if (yourJson == "success")
                da.apiKey = reg.apiKey;
            else
                da.apiKey = "";

            SaveRegnOutput output = new SaveRegnOutput();
            output.statusCode = serverResponse;
            output.data = da;


            var json = new JavaScriptSerializer().Serialize(output);


            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("VerifyOTP")]
        [HttpPost]
        public async Task<HttpResponseMessage> VerifyOTP(RegistrationDtl reg)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (reg.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (reg.secret == "") { yourJson += "Invalid API Credentials"; serverResponse = "002"; }
            if (yourJson == "")
                if (reg.clientId == "") { yourJson += "Invalid clientID"; serverResponse = "003"; }
            if (yourJson == "")
                if (reg.mobile == "") { yourJson += "Invalid Mobile No"; serverResponse = "005"; }
            if (yourJson == "")
                if (reg.otp == "") { yourJson += "Invalid OTP "; serverResponse = "008"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == reg.apiKey.Trim() && Convert.ToString(dt.Rows[0]["Secrete"]) == reg.secret.Trim())
            {
                if (yourJson == "")
                {
                    string _clientid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select ClientID from clientMast where ClientID='{0}'", reg.clientId)));
                    if (_clientid != reg.clientId.Trim()) { yourJson += "Invalid clientID "; serverResponse = "003"; }
                }
                if (yourJson == "")
                {
                    string _mobile = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select MobileNo from RegnRequest where MobileNo='{0}'", reg.mobile)));
                    if (_mobile != reg.mobile.Trim()) { yourJson += "Mobile No does not exist "; serverResponse = "005"; }
                }
                if (yourJson == "")
                {
                    string _otp = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select OTP from RegnRequest where OTP='{0}'", reg.otp)));
                    if (_otp != reg.otp.Trim()) { yourJson += "Invalid OTP "; serverResponse = "008"; }
                    flag = true;
                    if (flag && yourJson == "")
                    {
                        string sql = string.Format("UPDATE RegnRequest SET MobileVerifiedOn = GETDATE() WHERE ClientID = '{0}' AND MobileNo ='{1}' ", reg.clientId, reg.mobile);
                        Callingdatabase.ExecuteNonQuery(sql);
                        yourJson = "Verified successfully";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            //DataTable dt1 = new DataTable();
            //dt1.Columns.Add("ServerResponse");
            //dt1.Columns.Add("Result");
            //dt1.Columns.Add("Description");
            //DataRow dr = dt1.NewRow();
            //dr[0] = serverResponse;
            //dr[1] = yourJson;
            //dr[2] = yourJson;
            //dt1.Rows.Add(dr);
            //yourJson = JsonConvert.SerializeObject(dt1);


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("ResendOTP")]
        [HttpGet]
        public async Task<HttpResponseMessage> ResendOTP(string mobile)
        {
            string serverResponse = "";
            string yourJson = "";

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            if (mobile == "") { yourJson += "Invalid Mobile No"; serverResponse = "005"; }

            if (yourJson == "")
            {
                string _mobile = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select MobileNo from RegnRequest where MobileNo='{0}'", mobile.Trim())));
                if (_mobile != mobile.Trim()) { yourJson += "Mobile No does not exist "; serverResponse = "005"; }
            }
            if (yourJson == "")
            {
                string _otp = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select OTP from RegnRequest where MobileNo='{0}'", mobile.Trim())));
                string clientId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select clientid from RegnRequest where MobileNo='{0}'", mobile.Trim())));

                string msg = "Your SMS verification code is:" + _otp;
                Util ob = new Util();
                ob.SendSMSthroughAPICallingApp(mobile, msg, clientId);

                yourJson = "success";
                serverResponse = "200";

            }

            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }



        [Route("VerifyEmail")]
        [HttpGet]
        public async Task<HttpResponseMessage> VerifyEmail(string segment)
        {
            string serverResponse = "";
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (yourJson == "")
            {
                string _segment = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select emailVerificationSegment from RegnRequest where emailVerificationSegment='{0}'", segment.Trim())));
                if (_segment != segment.Trim()) { yourJson += "Invalid Segment "; serverResponse = "012"; }
            }
            if (yourJson == "")
            {
                string _EmailVerifiedOn = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select ISNULL(CONVERT(VARCHAR, EmailVerifiedOn,106),'') from RegnRequest where emailVerificationSegment='{0}'", segment.Trim())));
                if (_EmailVerifiedOn != "") { yourJson += "Already verified "; serverResponse = "011"; }

            }
            if (yourJson == "")
            {
                string sql = string.Format("UPDATE RegnRequest SET EmailVerifiedOn = GETDATE() where emailVerificationSegment ='{0}' ", segment.Trim());
                Callingdatabase.ExecuteNonQuery(sql);
                yourJson = "Verified successfully ";
                serverResponse = "200";
            }

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("serverresponse");
            dt1.Columns.Add("result");
            dt1.Columns.Add("description");
            DataRow dr = dt1.NewRow();
            dr[0] = serverResponse;
            dr[1] = yourJson;
            dr[2] = yourJson;
            dt1.Rows.Add(dr);
            yourJson = JsonConvert.SerializeObject(dt1);

            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("Login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login(LoginDtl lgn)
        {
            string serverResponse = "";
            string yourJson = "";
            string result1 = "";
            bool isError = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            lgn.apikey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            if (lgn.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true; }
            if (yourJson == "")
                if (lgn.secret == "") { yourJson += "Invalid API Credentials"; serverResponse = "002"; isError = true; }
            if (yourJson == "")
                if (lgn.userId == "") { yourJson += "Invalid UserId"; serverResponse = "013"; isError = true; }
            if (yourJson == "")
                if (lgn.password == "") { yourJson += "Invalid Password"; serverResponse = "014"; isError = true; }


            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete,scheduleAfter From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == lgn.apikey.Trim() && Convert.ToString(dt.Rows[0]["Secrete"]) == lgn.secret.Trim())
            {

                if (yourJson == "")
                {
                    string sql = string.Format("SELECT IIF(Count(UserID)>0,'Success','Fail')  FROM RegnRequest WHERE UserID = '{0}' AND Password ='{1}' ", lgn.userId, lgn.password);
                    string result = Convert.ToString(Callingdatabase.GetScalarValue(sql));

                    string sql1 = string.Format("SELECT IIF(Count(UserID)>0,'Success','Fail')  FROM RegnRequest WHERE UserID = '{0}' AND Password ='{1}' and isadmin=1 ", lgn.userId, lgn.password);
                    result1 = Convert.ToString(Callingdatabase.GetScalarValue(sql1));

                    if (result == "Success")
                    {
                        yourJson = "success";
                        serverResponse = "200";


                        string sql2 = string.Format("SELECT top 1 Name,MobileNo,EmailID,profileimage FROM RegnRequest WHERE UserID = '{0}' AND Password ='{1}'", lgn.userId, lgn.password);
                        DataTable dt1 = Callingdatabase.GetDataTable(sql2);

                        string name = dt1.Rows[0]["Name"].ToString();
                        string mobileNo = dt1.Rows[0]["MobileNo"].ToString();
                        string mailid = dt1.Rows[0]["EmailID"].ToString();
                        string profileimg = Convert.ToString(dt1.Rows[0]["profileimage"]);
                        byte[] arr = Encoding.UTF8.GetBytes(profileimg);
                        DataApi da = new DataApi()
                        {
                            result = yourJson,
                            description = yourJson,
                            userId = lgn.userId,
                            name = name,
                            emailId = mailid,
                            mobile = mobileNo,
                            scheduleAfter = dt.Rows[0]["scheduleAfter"].ToString(),
                            image = Convert.ToBase64String(arr)
                        };
                        if (yourJson == "success")
                            da.apiKey = lgn.apikey;
                        else
                            da.apiKey = "";
                        if (result1 == "Success") da.isAdmin = "y"; else da.isAdmin = "n";

                        Output output = new Output();
                        output.statusCode = serverResponse;
                        output.data = da;

                        var json = new JavaScriptSerializer().Serialize(output);
                        response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        yourJson += "Invalid Credentials ";
                        serverResponse = "001";
                        isError = true;
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
                isError = true;
            }

            if (isError)
            {
                DataVerifyOTP daa = new DataVerifyOTP()
                {
                    result = yourJson,
                    description = yourJson
                };

                VerifyOutput errorOut = new VerifyOutput();
                errorOut.statusCode = serverResponse;
                errorOut.data = daa;

                var json = new JavaScriptSerializer().Serialize(errorOut);
                response = this.Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return response;

        }

        [Route("Logout")]
        [HttpGet]
        public async Task<HttpResponseMessage> Logout(string apiKey, string userId)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (userId == "") { yourJson += "Invalid userId"; serverResponse = "003"; }


            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apiKey.Trim())
            {
                if (yourJson == "")
                {
                    string _userId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userId from RegnRequest where userId='{0}'", userId.Trim())));
                    if (_userId.ToUpper() != userId.Trim().ToUpper()) { yourJson += "UserId does not exist "; serverResponse = "005"; }
                }
                flag = true;
                if (flag && yourJson == "")
                {
                    yourJson = "success";
                    serverResponse = "200";
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("CallHistory")]
        [HttpPost]
        public async Task<HttpResponseMessage> CallHistory(CallHistoryInput call)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (call.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (call.userId == "") { yourJson += "Invalid API Credentials"; serverResponse = "013"; }


            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == call.apikey.Trim())
                {
                    flag = true;
                    if (flag && yourJson == "")
                    {
                        string qUpComing = "select Count(ClientID) from CallsHistory where ";

                        DataTable dt1 = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");

                        yourJson = "success";

                        serverResponse = "200";

                    }
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }


            CallHistoryOutput output = new CallHistoryOutput();
            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("SaveCall")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveCall(SaveInputCall call)
        {
            string serverResponse = "";
            string yourJson = "";
            string callId = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            // call.Apikey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            if (call.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }

            if (yourJson == "")
                if (call.userId == "") { yourJson += "Invalid UserID"; serverResponse = "013"; }
            if (yourJson == "")
                if (call.name == "") { yourJson += "Invalid Name"; serverResponse = "018"; }
            if (yourJson == "")
                if (call.calledmobile == "") { yourJson += "Invalid CalledMobile"; serverResponse = "004"; }
            if (yourJson == "")
                if (call.callinitiateat == "") { yourJson += "Invalid CallInitiateAt"; serverResponse = "005"; }
            if (call.callstartedat == "") { yourJson += "Invalid CallStartedAt"; serverResponse = "006"; }
            if (yourJson == "")
                if (call.callendedat == "") { yourJson += "Invalid CallEndedAt"; serverResponse = "007"; }
            if (yourJson == "")
                if (call.feedback == "") { yourJson += "Invalid FeedBack"; serverResponse = "007"; }
            if (yourJson == "")
                if (call.callrecording == "") { yourJson += "Invalid CallRecording"; serverResponse = "007"; }
            //if (yourJson == "")
            //    if (call.callstatus == "") { yourJson += "Invalid CallStatus"; serverResponse = "007"; }

            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == call.apikey.Trim())
                {
                    var callInSeconds = (Convert.ToDateTime(call.callendedat) - Convert.ToDateTime(call.callstartedat)).TotalSeconds;
                    // var diffInSeconds = (dateTime1 - dateTime2).TotalSeconds;

                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", call.userId.Trim());

                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);

                    string sql2 = string.Format("select e.CountryCode,e.EmpCode,e.EmpName from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID" +
                        " where r.UserID = '{0}' AND e.MobileNo='{1}'", call.userId.Trim(), dt1.Rows[0]["mobileno"].ToString());
                    DataTable dt2 = Callingdatabase.GetDataTable(sql2);

                    flag = true;
                    if (flag && yourJson == "")
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            string EmpName = "", CountryCode = "", EmpCode = "", CalledOrReceived = "C";
                            if (dt2.Rows.Count > 0)
                            {
                                //EmpName = dt2.Rows[0]["EmpName"].ToString();
                                CountryCode = dt2.Rows[0]["CountryCode"].ToString();
                                EmpCode = dt2.Rows[0]["EmpCode"].ToString();
                            }
                            callId = Convert.ToString(Callingdatabase.GetScalarValue("select ISNULL(MAX(convert(numeric(10),callID)), 0)+1 from callsHistory"));
                            string sqlIn = "";
                            if (call.scheduleMode.ToUpper() == "NONE")
                            {
                                sqlIn = string.Format("insert into callsHistory(clientID,CallID,Name,CountryCode,CalledMobile,callinitAt,CallStartedAt,CallEndedAt,CallDuration,EmpCode,Feedback,scheduleMode,CallRecordingFile,CalledOrReceived,CallStatus) " +
                               "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", dt1.Rows[0]["clientID"], callId, call.name.Trim(), CountryCode, call.calledmobile.Trim(), call.callinitiateat.Trim(), call.callstartedat.Trim(), call.callendedat.Trim(), callInSeconds, EmpCode, call.feedback,
                               call.callrecording, CalledOrReceived, call.scheduleMode, call.callstatus);
                            }
                            else
                            {
                                sqlIn = string.Format("insert into callsHistory(clientID,CallID,Name,CountryCode,CalledMobile,callinitAt,CallStartedAt," +
                                    "CallEndedAt,CallDuration,EmpCode,Feedback,CallRecordingFile,CalledOrReceived,CallStatus,scheduleMode) " +
                               "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", dt1.Rows[0]["clientID"], callId, call.name.Trim(), CountryCode, call.calledmobile.Trim(), call.callinitiateat.Trim(), call.callstartedat.Trim(), call.callendedat.Trim(), callInSeconds, EmpCode, call.feedback,
                               call.callrecording, CalledOrReceived, call.callstatus, call.scheduleMode);
                            }

                            Callingdatabase.ExecuteNonQuery(sqlIn);

                            yourJson = "success";
                            serverResponse = "200";
                        }
                        else
                        {
                            yourJson = "Invalid UserID";
                            serverResponse = "013";
                        }

                    }
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }

            SaveCallOut daa = new SaveCallOut()
            {
                result = yourJson,
                description = yourJson,
                callId = callId
            };

            SaveCallOutput output = new SaveCallOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("callSchedule")]
        [HttpPost]
        public async Task<HttpResponseMessage> callSchedule(SaveScheduleCall call)
        {
            string serverResponse = "";
            string yourJson = "";

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            // call.Apikey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            if (call.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (call.userId == "") { yourJson += "Invalid UserID"; serverResponse = "013"; }
            if (yourJson == "")
                if (call.scheduleMode == "") { yourJson += "Invalid scheduleMode"; serverResponse = "018"; }
            if (yourJson == "")
                if (string.IsNullOrEmpty(call.scheduleDate )) { yourJson += "Invalid scheduleDate"; serverResponse = "004"; }
            if (yourJson == "")
                if (call.scheduleTime == "") { yourJson += "Invalid scheduleTime"; serverResponse = "005"; }

            string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", call.userId.Trim())));
            if (_userid.ToUpper() != call.userId.Trim().ToUpper()) { yourJson += "userID does not exist"; serverResponse = "013"; }
            string _scheduleId = "0";
            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == call.apikey.Trim())
                {
                     string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", call.userId.Trim());
                    //  string sql1 = string.Format("select r.ClientId,r.EmpCode,c.CallId from RegnRequest r INNER JOIN CallsHistory c ON r.ClientId=c.ClientId and r.EmpCode=c.EmpCode where UserID = '{0}'", call.userId.Trim());
                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                    //[CallID],
                    string sqlIns = string.Format("Insert into CallSchedule ([ClientID],[EmpCode],[userid],[ScheduleDate],[ScheduleTime],[scheduleMode],[remarks],CallId,clientName,clientMobile,clientEmail)" +
                       "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", dt1.Rows[0]["clientID"], dt1.Rows[0]["EmpCode"], call.userId.Trim(),
                       call.scheduleDate, call.scheduleTime, call.scheduleMode, call.remarks,call.callId,call.clientName,call.clientMobile,call.clientEmail);
                    Callingdatabase.ExecuteNonQuery(sqlIns);

                    _scheduleId = Convert.ToString(Callingdatabase.GetScalarValue("select IDENT_CURRENT('CallSchedule')"));
                    if(call.attendee.Length>0)
                    foreach(attendee at in call.attendee)
                    {
                        sqlIns = string.Format("Insert into Attendee ([ClientID],[ScheduleId],[AttendeeName],[AttendeeMobile],AttendeeEmail)" +
                       "values('{0}','{1}','{2}','{3}','{4}')", dt1.Rows[0]["clientID"], _scheduleId, at.attendeeName.Trim(), at.attendeeMobile, at.attendeeEmail);
                        Callingdatabase.ExecuteNonQuery(sqlIns);
                    }
                    yourJson = "success";
                    serverResponse = "200";
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }

            DataScheduleCall daa = new DataScheduleCall()
            {
                result = yourJson,
                description = yourJson
               // scheduleId = _scheduleId
            };

            ScheduleCallOutput output = new ScheduleCallOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;

        }

        [Route("addAttendee")]
        [HttpPost]
        public async Task<HttpResponseMessage> addAttendee(SaveAttendee call)
        {
            string serverResponse = "";
            string yourJson = "";

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
             
            if (call.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (call.userId == "") { yourJson += "Invalid UserID"; serverResponse = "013"; }
            if (yourJson == "")
                if (call.scheduleId == "") { yourJson += "Invalid scheduleId"; serverResponse = "018"; }
            if (yourJson == "")
                if (call.attendeeName == "") { yourJson += "Invalid attendeeName"; serverResponse = "004"; }
            if (yourJson == "")
                if (call.attendeeMobile == "") { yourJson += "Invalid attendeeMobile"; serverResponse = "005"; }

            string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", call.userId.Trim())));
            if (_userid.ToUpper() != call.userId.Trim().ToUpper()) { yourJson += "userID does not exist"; serverResponse = "013"; }
            
            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == call.apikey.Trim())
                {
                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", call.userId.Trim());
                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                     
                    string sqlIns = string.Format("Insert into Attendee ([ClientID],[ScheduleId],[AttendeeName],[AttendeeMobile])" +
                       "values('{0}','{1}','{2}','{3}')", dt1.Rows[0]["clientID"], call.scheduleId, call.attendeeName.Trim(),call.attendeeMobile);
                    Callingdatabase.ExecuteNonQuery(sqlIns);
                    
                    yourJson = "success";
                    serverResponse = "200";
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }

            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;

        }


        [Route("GetCallList")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCallList(string apikey, string userId, string status)
        {
            List<CallList> target = new List<CallList>();
            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }

            if (yourJson == "")
                if (userId == "") { yourJson += "Invalid UserId"; serverResponse = "013"; }
            if (yourJson == "")
                if (status == "") { yourJson += "Invalid Status"; serverResponse = "014"; }

            string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", userId.Trim())));
            if (_userid.ToUpper() != userId.Trim().ToUpper()) { yourJson += "Invalid userID "; serverResponse = "013"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apikey.Trim())
            {
                string mobileno = "";
                string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", userId.Trim());

                DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                if (dt1.Rows.Count > 0)
                    mobileno = dt1.Rows[0]["mobileno"].ToString();

                string sql2 = string.Format("select e.EmpCode,r.clientid from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", userId.Trim(), mobileno);
                DataTable dt2 = Callingdatabase.GetDataTable(sql2);
                string empCode = Convert.ToString(dt2.Rows[0]["EmpCode"]);
                string clientId = Convert.ToString(dt2.Rows[0]["clientid"]);
                flag = true;
                if (flag && yourJson == "")
                {
                    string Sql = "";
                    if (status.ToUpper() == "DIALED")
                        Sql = "select CalledMobile,convert(varchar,CallStartedAt,120) as CallStartedAt ,Feedback,callid from CallsHistory" +
                           " where CallStartedAt is not null and EmpCode='" + empCode + "' and clientid='" + clientId + "' Order by CallStartedAt desc";

                    else if (status.ToUpper() == "CONNECTED")
                        Sql = "select CalledMobile,convert(varchar,CallStartedAt,120) as CallStartedAt ,Feedback,callid from CallsHistory" +
                      " where CallStartedAt is not null and EmpCode='" + empCode + "' and clientid='" + clientId + "' and isnull(callstatus,'')='" + status + "' Order by CallStartedAt desc";
                    else if (status.ToUpper() == "UPCOMING")
                       // Sql = "select CalledMobile,replace(convert(varchar,scheduleDate,102),'.','-') +  ' ' + scheduleTime as CallStartedAt ,Feedback,callid from CallsHistory" +
                       //" where CallStartedAt is not null and EmpCode='" + empCode + "' and clientid='" + clientId + "' AND " +
                       //" Convert(datetime,replace(convert(varchar,scheduleDate,102),'.','-') +  ' ' + scheduleTime ) > getdate() Order by CallStartedAt desc";
                        Sql = "SELECT ISNULL(CLIENTMOBILE,'') AS CalledMobile,replace(convert(varchar,scheduleDate,102),'.','-') +  ' ' + CONVERT (VARCHAR(5),scheduleTime,108) as CallStartedAt , REMARKS AS Feedback,callid from CallSchedule" +
                       " where  EmpCode='" + empCode + "' and clientid='" + clientId + "' AND " +
                       " Convert(datetime,replace(convert(varchar,scheduleDate,102),'.','-') +  ' ' + CONVERT (VARCHAR(5),scheduleTime,108) ) > getdate() Order by CallStartedAt desc";
                    DataTable dtData = Callingdatabase.GetDataTable(Sql);

                    target = dtData.AsEnumerable()
                   .Select(row => new CallList
                   {
                       mobile = row.Field<string>(0),
                       callStartedAt = row.Field<string>(1),
                       remarks = row.Field<string>(2),
                       id = row.Field<string>(3)
                   }).ToList();

                    var serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(target);

                    if (dtData.Rows.Count > 0)
                    {
                        yourJson = "success";
                        serverResponse = "200";
                    }
                    else
                    {
                        yourJson = "No Data Found";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }

            OutputCallList da = new OutputCallList()
            {
                statusCode = serverResponse,
                data = new DataCallList()
                {
                    result = yourJson,
                    description = yourJson,
                    callLists = target
                }
            };

            var json2 = new JavaScriptSerializer().Serialize(da);

            response.Content = new StringContent(json2, Encoding.UTF8, "application/json");
            return response;

        }


        [Route("Dashboard_Call")]
        [HttpGet]
        public async Task<HttpResponseMessage> Dashboard_Call(string apikey, string userId)
        {
            CallHistoryInput call = new CallHistoryInput();
            call.apikey = apikey;
            call.userId = userId;

            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool isError = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (call.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true; }
            if (yourJson == "")
                if (call.userId == "") { yourJson += "Invalid API Credentials"; serverResponse = "013"; isError = true; }

            if (yourJson == "")
            {
                string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", call.userId.Trim())));
                if (_userid.ToUpper() != call.userId.Trim().ToUpper()) { yourJson += "Invalid userID "; serverResponse = "013"; isError = true; }

                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == call.apikey.Trim() && yourJson == "")
                {
                    string mobileno = "";
                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", call.userId.Trim());

                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                    if (dt1.Rows.Count > 0)
                        mobileno = dt1.Rows[0]["mobileno"].ToString();

                    string sql2 = string.Format("select e.EmpCode from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", call.userId.Trim(), mobileno);
                    string empCode = Convert.ToString(Callingdatabase.GetScalarValue(sql2));

                    //string qTdyDial = string.Format("select COUNT(clientId) [DialedCall] from callsHistory where CallinitAt IS NOT NULL and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    string qTdyDial = string.Format("select COUNT(clientId) [DialedCall] from callsHistory where cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    //string qTdyConneted = string.Format("select COUNT(clientId) [ConnectedCall] from callsHistory where CallDuration>0 and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    string qTdyConneted = string.Format("select COUNT(clientId) [ConnectedCall] from callsHistory where cast(insertdate as Date) = cast(getdate() as Date) and isnull(callStatus, '') ='Connected' and EmpCode='{0}'", empCode);
                    string qTdyUpcom = string.Format("select COUNT(scheduleid) [UpcomingCall] from CallSchedule where Convert(datetime,replace(convert(varchar,scheduleDate,102),'.','-') +  ' ' + convert(varchar,scheduleTime,108) ) > getdate() and EmpCode='{0}'", empCode);

                    string sqlTdyDsh = string.Format("{0};{1};{2}", qTdyDial, qTdyConneted, qTdyUpcom);

                    DataSet dsTdy = Callingdatabase.GetDataSet(sqlTdyDsh);

                    string qWeekDial = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekDial = qWeekDial + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekDial = qWeekDial + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM callsHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " and EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string qWeekConneted = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekConneted = qWeekConneted + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekConneted = qWeekConneted + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM callsHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " AND isnull(callStatus, '') ='Connected' and EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string sqlDsh = string.Format("{0};{1};", qWeekDial, qWeekConneted);

                    DataSet dsWeek = Callingdatabase.GetDataSet(sqlDsh);

                    CallHistoryData data = new CallHistoryData();
                    data.dialed = Convert.ToString(dsTdy.Tables[0].Rows[0]["DialedCall"]);
                    data.connected = Convert.ToString(dsTdy.Tables[1].Rows[0]["ConnectedCall"]);
                    data.upcoming = Convert.ToString(dsTdy.Tables[2].Rows[0]["UpcomingCall"]);

                    CallHistoryOutput output = new CallHistoryOutput();
                    output.statusCode = "200";
                    output.data = data;

                    Gdata_Dialed gDataDial = new Gdata_Dialed();
                    gDataDial.day1 = Convert.ToString(dsWeek.Tables[0].Rows[0]["cnt"]);
                    gDataDial.day2 = Convert.ToString(dsWeek.Tables[0].Rows[1]["cnt"]);
                    gDataDial.day3 = Convert.ToString(dsWeek.Tables[0].Rows[2]["cnt"]);
                    gDataDial.day4 = Convert.ToString(dsWeek.Tables[0].Rows[3]["cnt"]);
                    gDataDial.day5 = Convert.ToString(dsWeek.Tables[0].Rows[4]["cnt"]);
                    gDataDial.day6 = Convert.ToString(dsWeek.Tables[0].Rows[5]["cnt"]);

                    Gdata_Connected gDataeConn = new Gdata_Connected();
                    gDataeConn.day1 = Convert.ToString(dsWeek.Tables[1].Rows[0]["cnt"]);
                    gDataeConn.day2 = Convert.ToString(dsWeek.Tables[1].Rows[1]["cnt"]);
                    gDataeConn.day3 = Convert.ToString(dsWeek.Tables[1].Rows[2]["cnt"]);
                    gDataeConn.day4 = Convert.ToString(dsWeek.Tables[1].Rows[3]["cnt"]);
                    gDataeConn.day5 = Convert.ToString(dsWeek.Tables[1].Rows[4]["cnt"]);
                    gDataeConn.day6 = Convert.ToString(dsWeek.Tables[1].Rows[5]["cnt"]);

                    output.data.gdata_dialed = gDataDial;
                    output.data.gdata_connected = gDataeConn;
                    json = new JavaScriptSerializer().Serialize(output);
                }
                else
                {
                    yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true;
                }
            }

            if (isError)
            {
                DataVerifyOTP daa = new DataVerifyOTP()
                {
                    result = yourJson,
                    description = yourJson
                };

                VerifyOutput errorOut = new VerifyOutput();
                errorOut.statusCode = serverResponse;
                errorOut.data = daa;

                json = new JavaScriptSerializer().Serialize(errorOut);
                response = this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("Dashboard_SMS")]
        [HttpGet]
        public async Task<HttpResponseMessage> Dashboard_SMS(string apikey, string userId)
        {
            CallHistoryInput sms = new CallHistoryInput();
            sms.apikey = apikey;
            sms.userId = userId;

            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool isError = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (sms.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true; }
            if (yourJson == "")
                if (sms.userId == "") { yourJson += "Invalid API Credentials"; serverResponse = "002"; isError = true; }

            if (yourJson == "")
            {

                string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", sms.userId.Trim())));
                if (_userid.ToUpper() != sms.userId.Trim().ToUpper()) { yourJson += "Invalid userID "; serverResponse = "003"; isError = true; }

                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == sms.apikey.Trim() && yourJson == "")
                {
                    string mobileno = "";
                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", sms.userId.Trim());

                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                    if (dt1.Rows.Count > 0)
                        mobileno = dt1.Rows[0]["mobileno"].ToString();

                    string sql2 = string.Format("select e.EmpCode from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", sms.userId.Trim(), mobileno);
                    string empCode = Convert.ToString(Callingdatabase.GetScalarValue(sql2));

                    string qTdySent = string.Format("select COUNT(clientId) [SentSms] from SMSHistory where SentRcvd='S'  and SentRcvdOn IS NOT NULL and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    string qTdyRec = string.Format("select COUNT(clientId) [ReceivedSms] from SMSHistory where SentRcvd='R'  and SentRcvdOn IS NOT NULL and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    string qTdyUpcom = string.Format("select COUNT(clientId) [ScheduleSms] from SMSHistory where cast(ScheduleDate as Date) > cast(getdate() as Date) and SentRcvdOn IS NULL and EmpCode='{0}'", empCode);

                    string sqlTdyDsh = string.Format("{0};{1};{2}", qTdySent, qTdyRec, qTdyUpcom);

                    DataSet dsTdy = Callingdatabase.GetDataSet(sqlTdyDsh);

                    string qWeekSent = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekSent = qWeekSent + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekSent = qWeekSent + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM SMSHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " AND SentRcvd='S'  and SentRcvdOn IS NOT NULL and EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string qWeekReceived = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekReceived = qWeekReceived + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekReceived = qWeekReceived + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM SMSHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " AND SentRcvd='R' and SentRcvdOn IS NOT NULL AND EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string sqlDsh = string.Format("{0};{1};", qWeekSent, qWeekReceived);

                    DataSet dsWeek = Callingdatabase.GetDataSet(sqlDsh);

                    SMSHistoryData data = new SMSHistoryData();
                    data.sent = Convert.ToString(dsTdy.Tables[0].Rows[0]["SentSms"]);
                    data.received = Convert.ToString(dsTdy.Tables[1].Rows[0]["ReceivedSms"]);
                    data.schedule = Convert.ToString(dsTdy.Tables[2].Rows[0]["ScheduleSms"]);

                    SMSHistoryOutput output = new SMSHistoryOutput();
                    output.statusCode = "200";
                    output.data = data;

                    GSMSdata_Sent gSmsDataSent = new GSMSdata_Sent();
                    gSmsDataSent.day1 = Convert.ToString(dsWeek.Tables[0].Rows[0]["cnt"]);
                    gSmsDataSent.day2 = Convert.ToString(dsWeek.Tables[0].Rows[1]["cnt"]);
                    gSmsDataSent.day3 = Convert.ToString(dsWeek.Tables[0].Rows[2]["cnt"]);
                    gSmsDataSent.day4 = Convert.ToString(dsWeek.Tables[0].Rows[3]["cnt"]);
                    gSmsDataSent.day5 = Convert.ToString(dsWeek.Tables[0].Rows[4]["cnt"]);
                    gSmsDataSent.day6 = Convert.ToString(dsWeek.Tables[0].Rows[5]["cnt"]);

                    GSMSdata_Received gSmsDataRec = new GSMSdata_Received();
                    gSmsDataRec.day1 = Convert.ToString(dsWeek.Tables[1].Rows[0]["cnt"]);
                    gSmsDataRec.day2 = Convert.ToString(dsWeek.Tables[1].Rows[1]["cnt"]);
                    gSmsDataRec.day3 = Convert.ToString(dsWeek.Tables[1].Rows[2]["cnt"]);
                    gSmsDataRec.day4 = Convert.ToString(dsWeek.Tables[1].Rows[3]["cnt"]);
                    gSmsDataRec.day5 = Convert.ToString(dsWeek.Tables[1].Rows[4]["cnt"]);
                    gSmsDataRec.day6 = Convert.ToString(dsWeek.Tables[1].Rows[5]["cnt"]);

                    output.data.gsmsdata_sent = gSmsDataSent;
                    output.data.gsmsdata_received = gSmsDataRec;
                    json = new JavaScriptSerializer().Serialize(output);
                }
                else
                {
                    yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true;
                }
            }

            if (isError)
            {
                DataVerifyOTP daa = new DataVerifyOTP()
                {
                    result = yourJson,
                    description = yourJson
                };

                VerifyOutput errorOut = new VerifyOutput();
                errorOut.statusCode = serverResponse;
                errorOut.data = daa;

                json = new JavaScriptSerializer().Serialize(errorOut);
                response = this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("SaveSMS")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveSMS(SaveInputSMS sms)
        {
            sms.sentRcvd = "s";
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            // call.Apikey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            if (sms.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (sms.name == "") { yourJson += "Invalid Name"; serverResponse = "018"; }
            if (yourJson == "")
                if (sms.mobileNo == "") { yourJson += "Invalid Mobile No"; serverResponse = "004"; }
            if (yourJson == "")
                if (sms.sentRcvd == "") { yourJson += "Invalid SentRcvd"; serverResponse = "005"; }
            if (sms.sentRcvdOn == "") { yourJson += "Invalid SentRcvdOn"; serverResponse = "006"; }

            if (yourJson == "")
                if (sms.smsText == "") { yourJson += "Invalid SMSText"; serverResponse = "007"; }

            if (yourJson == "")
            {
                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == sms.apiKey.Trim())
                {

                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", sms.userId.Trim());

                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);

                    string sql2 = string.Format("select r.clientID, e.CountryCode,e.EmpCode,e.EmpName from Employee e INNER JOIN RegnRequest r " +
                        "ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", sms.userId.Trim(), dt1.Rows[0]["mobileno"].ToString());
                    DataTable dt2 = Callingdatabase.GetDataTable(sql2);

                    flag = true;
                    if (flag && yourJson == "")
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            string EmpName = "", CountryCode = "", EmpCode = "", clientId = "";
                            if (dt2.Rows.Count > 0)
                            {
                                //EmpName = dt2.Rows[0]["EmpName"].ToString();
                                CountryCode = dt2.Rows[0]["CountryCode"].ToString();
                                EmpCode = dt2.Rows[0]["EmpCode"].ToString();
                                clientId = dt2.Rows[0]["clientID"].ToString();
                            }

                            string sqlIn = string.Format("insert into smsHistory(clientID,Name,CountryCode,MobileNo,SentRcvd,SentRcvdOn,EmpCode,SMSText) " +
                                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", clientId, sms.name.Trim(), CountryCode, sms.mobileNo.Trim(), sms.sentRcvd.Trim(), sms.sentRcvdOn.Trim(), EmpCode,
                                sms.smsText);
                            Callingdatabase.ExecuteNonQuery(sqlIn);

                            yourJson = "success";
                            serverResponse = "200";
                        }
                        else
                        {
                            yourJson = "Invalid UserID";
                            serverResponse = "013";
                        }

                    }
                }
                else
                {
                    yourJson += " Invalid API Credentials";
                    serverResponse = "001";
                }
            }

            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("GetSMSList")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSMSList(string apikey, string userId)
        {
            List<SMSList> target = new List<SMSList>();
            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }

            if (yourJson == "")
                if (userId == "") { yourJson += "Invalid UserId"; serverResponse = "013"; }
            //if (yourJson == "")
            //    if (date == "") { yourJson += "Invalid date"; serverResponse = "014"; }

            string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", userId.Trim())));
            if (_userid.ToUpper() != userId.Trim().ToUpper()) { yourJson += "Invalid userID "; serverResponse = "013"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apikey.Trim())
            {
                string mobileno = "";
                string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", userId.Trim());

                DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                if (dt1.Rows.Count > 0)
                    mobileno = dt1.Rows[0]["mobileno"].ToString();

                string sql2 = string.Format("select e.EmpCode from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", userId.Trim(), mobileno);
                string empCode = Convert.ToString(Callingdatabase.GetScalarValue(sql2));

                flag = true;
                if (flag && yourJson == "")
                {
                    DataTable dtData = Callingdatabase.GetDataTable("select mobileno,convert(varchar,sentrcvdon,120) as sentRcvdOn ,smstext from smsHistory" +
                        " where sentrcvdon is not null and EmpCode='" + empCode + "' Order by  sentrcvdon desc");

                    target = dtData.AsEnumerable()
                   .Select(row => new SMSList
                   {
                       mobile = row.Field<string>(0),
                       sentRcvdOn = row.Field<string>(1),
                       smsText = row.Field<string>(2),
                   }).ToList();

                    var serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(target);

                    if (dtData.Rows.Count > 0)
                    {
                        yourJson = "success";
                        serverResponse = "200";
                    }
                    else
                    {
                        yourJson = "No Data Found";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }

            OutputSmsList da = new OutputSmsList()
            {
                statusCode = serverResponse,
                data = new DataSmsList()
                {
                    result = yourJson,
                    description = yourJson,
                    smsLists = target
                }
            };
            var json2 = new JavaScriptSerializer().Serialize(da);

            response.Content = new StringContent(json2, Encoding.UTF8, "application/json");
            return response;

        }


        [Route("Dashboard_Email")]
        [HttpPost]
        public async Task<HttpResponseMessage> Dashboard_Email(CallHistoryInput email)
        {
            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool isError = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (email.apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true; }
            if (yourJson == "")
                if (email.userId == "") { yourJson += "Invalid API Credentials"; serverResponse = "002"; isError = true; }

            if (yourJson == "")
            {

                string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", email.userId.Trim())));
                if (_userid != email.userId.Trim()) { yourJson += "Invalid userID "; serverResponse = "003"; isError = true; }

                DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
                if (Convert.ToString(dt.Rows[0]["APIKey"]) == email.apikey.Trim() && yourJson == "")
                {
                    string mobileno = "";

                    string sql1 = string.Format("select * from RegnRequest where UserID = '{0}'", email.userId.Trim());
                    DataTable dt1 = Callingdatabase.GetDataTable(sql1);
                    if (dt1.Rows.Count > 0)
                        mobileno = dt1.Rows[0]["mobileno"].ToString();

                    string sql2 = string.Format("select e.EmpCode from Employee e INNER JOIN RegnRequest r ON e.MobileNo=r.MobileNo AND r.clientID=e.clientID where r.UserID = '{0}' AND e.MobileNo='{1}'", email.userId.Trim(), mobileno);
                    string empCode = Convert.ToString(Callingdatabase.GetScalarValue(sql2));

                    string qTdySent = string.Format("select COUNT(clientId) [SentEmail] from MailHistory where CallinitAt IS NOT NULL and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);
                    string qTdyRec = string.Format("select COUNT(clientId) [RecEmail] from MailHistory where CallDuration>0 and cast(insertdate as Date) = cast(getdate() as Date) and EmpCode='{0}'", empCode);

                    string sqlTdyDsh = string.Format("{0};{1}", qTdySent, qTdyRec);

                    DataSet dsTdy = Callingdatabase.GetDataSet(sqlTdyDsh);

                    string qWeekSent = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekSent = qWeekSent + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekSent = qWeekSent + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM MailHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " AND CallinitAt IS NOT NULL and EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string qWeekRec = "select insertdate, sum(cnt) cnt from  ( ";
                    for (int i = 0; i < 6; i++)
                        qWeekRec = qWeekRec + " select convert(varchar, dateadd(day,-" + i.ToString() + ",getdate()),106) as insertdate, 0 as cnt, convert(varchar, dateadd(day, -" + i.ToString() + ", getdate()), 102) as orddate " +
                     " union all ";
                    qWeekRec = qWeekRec + " SELECT Convert(varchar, insertdate, 106)[insertdate],COUNT(clientId)cnt, Convert(varchar, insertdate, 102) as orddate " +
                    " FROM MailHistory " +
                    " WHERE insertdate >= DATEADD(day, -6, GETDATE()) AND CONVERT(varchar, insertdate,102) <= CONVERT(varchar, GETDATE(), 102) " +
                    " AND CallDuration>0 and EmpCode='" + empCode + "'" +
                   " group by  Convert(varchar, insertdate, 106), Convert(varchar, insertdate, 102) " +
                   " ) x group by insertdate, orddate " +
                    " ORDER BY orddate ";

                    string sqlDsh = string.Format("{0};{1};", qWeekSent, qWeekRec);

                    DataSet dsWeek = Callingdatabase.GetDataSet(sqlDsh);

                    CallHistoryData data = new CallHistoryData();
                    data.dialed = Convert.ToString(dsTdy.Tables[0].Rows[0]["DialedCall"]);
                    data.connected = Convert.ToString(dsTdy.Tables[1].Rows[0]["ConnectedCall"]);
                    data.upcoming = Convert.ToString(dsTdy.Tables[2].Rows[0]["UpcomingCall"]);

                    CallHistoryOutput output = new CallHistoryOutput();
                    output.statusCode = "200";
                    output.data = data;

                    Gdata_Dialed gDataDial = new Gdata_Dialed();
                    gDataDial.day1 = Convert.ToString(dsWeek.Tables[0].Rows[0]["cnt"]);
                    gDataDial.day2 = Convert.ToString(dsWeek.Tables[0].Rows[1]["cnt"]);
                    gDataDial.day3 = Convert.ToString(dsWeek.Tables[0].Rows[2]["cnt"]);
                    gDataDial.day4 = Convert.ToString(dsWeek.Tables[0].Rows[3]["cnt"]);
                    gDataDial.day5 = Convert.ToString(dsWeek.Tables[0].Rows[4]["cnt"]);
                    gDataDial.day6 = Convert.ToString(dsWeek.Tables[0].Rows[5]["cnt"]);

                    Gdata_Connected gDataeConn = new Gdata_Connected();
                    gDataeConn.day1 = Convert.ToString(dsWeek.Tables[1].Rows[0]["cnt"]);
                    gDataeConn.day2 = Convert.ToString(dsWeek.Tables[1].Rows[1]["cnt"]);
                    gDataeConn.day3 = Convert.ToString(dsWeek.Tables[1].Rows[2]["cnt"]);
                    gDataeConn.day4 = Convert.ToString(dsWeek.Tables[1].Rows[3]["cnt"]);
                    gDataeConn.day5 = Convert.ToString(dsWeek.Tables[1].Rows[4]["cnt"]);
                    gDataeConn.day6 = Convert.ToString(dsWeek.Tables[1].Rows[5]["cnt"]);

                    output.data.gdata_dialed = gDataDial;
                    output.data.gdata_connected = gDataeConn;
                    json = new JavaScriptSerializer().Serialize(output);
                }
                else
                {
                    yourJson = "Invalid API Credentials"; serverResponse = "001"; isError = true;
                }
            }

            if (isError)
            {
                DataVerifyOTP daa = new DataVerifyOTP()
                {
                    result = yourJson,
                    description = yourJson
                };

                VerifyOutput errorOut = new VerifyOutput();
                errorOut.statusCode = serverResponse;
                errorOut.data = daa;

                json = new JavaScriptSerializer().Serialize(errorOut);
                response = this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("GetRegnRequest")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetRegnRequest(string apiKey, string userId)
        {

            List<RegRequestUnApprove> target = new List<RegRequestUnApprove>();
            string serverResponse = "";
            string json = "";
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apiKey)
            {
                string _userid = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where UserID = '{0}'", userId.Trim())));
                if (_userid.ToUpper() != userId.Trim().ToUpper()) { yourJson += "Invalid userID "; serverResponse = "013"; }

                if (yourJson == "")
                {
                    DataTable dtData = Callingdatabase.GetDataTable("select mobileNo ,Name from RegnRequest where UserID is null and Password is null and EmailVerifiedOn is not null and MobileVerifiedOn is not null");

                    target = dtData.AsEnumerable()
                   .Select(row => new RegRequestUnApprove
                   {
                       mobile = row.Field<string>(0),
                       name = row.Field<string>(1),

                   }).ToList();

                    var serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(target);

                    if (dtData.Rows.Count > 0)
                    {
                        yourJson = "success";
                        serverResponse = "200";
                    }
                    else
                    {
                        yourJson = "No Data Found";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }

            OutputRegnList da = new OutputRegnList()
            {
                statusCode = serverResponse,
                data = new DataRegnList()
                {
                    result = yourJson,
                    description = yourJson,
                    regnLists = target
                }
            };


            var json2 = new JavaScriptSerializer().Serialize(da);

            response.Content = new StringContent(json2, Encoding.UTF8, "application/json");
            return response;

        }

        [Route("GetRegnRequestDetail")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetRegnRequestDetail(string apiKey, string mobile, string name)
        {
            string serverResponse = "";
            string yourJson = "";
            string email = "", emailVerifed = "", mobileVerifed = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apiKey)
            {
                string _mobile = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select mobileNo from RegnRequest where mobileNo = '{0}'", mobile.Trim())));
                if (_mobile != mobile) { yourJson += "Invalid Mobile "; serverResponse = "013"; }

                if (yourJson == "")
                {
                    DataTable dtData = Callingdatabase.GetDataTable(string.Format("select top 1 EmailID ,format(MobileVerifiedOn,'yyyy-MM-dd hh:mm:ss') MobileVerifiedOn,format(EmailVerifiedOn,'yyyy-MM-dd hh:mm:ss') EmailVerifiedOn from RegnRequest where mobileNo = '{0}'", mobile.Trim()));

                    if (dtData.Rows.Count > 0)
                    {
                        email = Convert.ToString(dtData.Rows[0]["EmailID"]);
                        emailVerifed = Convert.ToString(dtData.Rows[0]["EmailVerifiedOn"]);
                        mobileVerifed = Convert.ToString(dtData.Rows[0]["MobileVerifiedOn"]);

                        yourJson = "success";
                        serverResponse = "200";
                    }
                    else
                    {
                        yourJson = "No Data Found";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }

            OutputRegnDetail da = new OutputRegnDetail()
            {
                statusCode = serverResponse,
                data = new DataRegnDetail()
                {
                    result = yourJson,
                    description = yourJson,
                    emailId = email,
                    mobileVerifiedOn = emailVerifed,
                    emailVerifiedOn = mobileVerifed
                }
            };


            var json2 = new JavaScriptSerializer().Serialize(da);

            response.Content = new StringContent(json2, Encoding.UTF8, "application/json");
            return response;

        }


        [Route("CreateUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(CreateUser cu)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (cu.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (cu.mobile == "") { yourJson += "Invalid Mobile No"; serverResponse = "005"; }
            if (yourJson == "")
                if (cu.userId == "") { yourJson += "Invalid userId"; serverResponse = "003"; }
            if (yourJson == "")
                if (cu.password == "") { yourJson += "Invalid Password "; serverResponse = "008"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == cu.apiKey.Trim())
            {

                if (yourJson == "")
                {
                    string _mobile = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select MobileNo from RegnRequest where MobileNo='{0}'", cu.mobile)));
                    if (_mobile != cu.mobile.Trim()) { yourJson += "Mobile No does not exist "; serverResponse = "005"; }
                }
                if (yourJson == "")
                {
                    string _userId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userid from RegnRequest where userid='{0}'", cu.userId)));
                    if (_userId.ToUpper() == cu.userId.Trim().ToUpper()) { yourJson += "UserId already exist "; serverResponse = "003"; }
                }
                flag = true;
                if (flag && yourJson == "")
                {
                    string sql = string.Format("UPDATE RegnRequest SET userid = '{0}', Password='{1}' WHERE MobileNo ='{2}' ", cu.userId, cu.password, cu.mobile);
                    Callingdatabase.ExecuteNonQuery(sql);
                    yourJson = "success";
                    serverResponse = "200";
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("GetUserList")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetUserList(string apikey)
        {
            List<userList> target = new List<userList>();
            string json = "";
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (apikey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }


            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apikey.Trim())
            {

                flag = true;
                if (flag && yourJson == "")
                {
                    DataTable dtData = Callingdatabase.GetDataTable("select mobileNo,userid,Cast(isnull(active,0) as varchar)[status] from regnrequest");

                    target = dtData.AsEnumerable()
                   .Select(row => new userList
                   {
                       mobile = row.Field<string>(0),
                       userId = row.Field<string>(1),
                       status = row.Field<string>(2),
                   }).ToList();

                    var serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(target);

                    if (dtData.Rows.Count > 0)
                    {
                        yourJson = "success";
                        serverResponse = "200";
                    }
                    else
                    {
                        yourJson = "No Data Found";
                        serverResponse = "200";
                    }
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }

            OutputUserList da = new OutputUserList()
            {
                statusCode = serverResponse,
                data = new DataUserList()
                {
                    result = yourJson,
                    description = yourJson,
                    userLists = target
                }
            };

            var json2 = new JavaScriptSerializer().Serialize(da);

            response.Content = new StringContent(json2, Encoding.UTF8, "application/json");
            return response;


        }

        [Route("UpdateUserStatus")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateUserStatus(UpdateUserStatus user)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (user.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (user.userId == "") { yourJson += "Invalid userId"; serverResponse = "003"; }
            if (yourJson == "")
                if (user.status == "") { yourJson += "Invalid Status "; serverResponse = "008"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == user.apiKey.Trim())
            {

                if (yourJson == "")
                {
                    string _userId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userId from RegnRequest where userId='{0}'", user.userId)));
                    if (_userId.ToUpper() != user.userId.Trim().ToUpper()) { yourJson += "UserId does not exist "; serverResponse = "005"; }

                }

                flag = true;
                if (flag && yourJson == "")
                {
                    string sql = string.Format("UPDATE RegnRequest SET Active = '{0}', StatusDate=GETDATE() WHERE UserId ='{1}' ", user.status, user.userId);
                    Callingdatabase.ExecuteNonQuery(sql);
                    yourJson = "success";
                    serverResponse = "200";
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


        [Route("UploadUserImage")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadUserImage(UploadUserImages upl)
        {
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (upl.apiKey == "") { yourJson = "Invalid API Credentials"; serverResponse = "001"; }
            if (yourJson == "")
                if (upl.userId == "") { yourJson += "Invalid userId"; serverResponse = "003"; }
            if (yourJson == "")
                if (upl.profileImage == "") { yourJson += "Invalid Profile Image "; serverResponse = "008"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == upl.apiKey.Trim())
            {

                if (yourJson == "")
                {
                    string _userId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userId from RegnRequest where userId='{0}'", upl.userId)));
                    if (_userId.ToUpper() != upl.userId.Trim().ToUpper()) { yourJson += "UserId does not exist "; serverResponse = "005"; }
                }

                flag = true;
                if (flag && yourJson == "")
                {
                    // byte[] imageByte = Encoding.UTF8.GetBytes(user.profileImage);
                    string sql = string.Format("UPDATE RegnRequest SET profileimage =CAST('{0}' as varbinary) WHERE UserId ='{1}' ", upl.profileImage, upl.userId);
                    Callingdatabase.ExecuteNonQuery(sql);
                    yourJson = "success";
                    serverResponse = "200";
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("ForgetPassword")]
        [HttpPost]
        public async Task<HttpResponseMessage> ForgetPassword(ForgetPasswords fgt)
        {
            string apiKey = "FB4E63A6EE314B8B9485FAC4A2B71FCF";
            string serverResponse = "";
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (yourJson == "")
                if (fgt.userId == "") { yourJson += "Invalid userId"; serverResponse = "003"; }
            if (yourJson == "")
                if (fgt.mobile == "") { yourJson += "Invalid Mobile"; serverResponse = "004"; }
            if (yourJson == "")
                if (fgt.email == "") { yourJson += "Invalid Email "; serverResponse = "008"; }

            DataTable dt = Callingdatabase.GetDataTable("select APIKey,Secrete From SettingMast");
            if (Convert.ToString(dt.Rows[0]["APIKey"]) == apiKey)
            {
                if (yourJson == "")
                {
                    string _userId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select userId from RegnRequest where userId='{0}'", fgt.userId)));
                    if (_userId.ToUpper() != fgt.userId.Trim().ToUpper()) { yourJson += "UserId does not exist "; serverResponse = "005"; }
                }
                if (yourJson == "")
                {
                    string _mobile = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select MobileNo from RegnRequest where MobileNo='{0}'", fgt.mobile)));
                    if (_mobile != fgt.mobile.Trim()) { yourJson += "Mobile No does not exist "; serverResponse = "005"; }
                }

                if (yourJson == "")
                {
                    string _email = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select EmailID from RegnRequest where EmailID='{0}'", fgt.email)));
                    if (_email != fgt.email.Trim()) { yourJson += "Email Id does not registered"; serverResponse = "010"; }

                }
                flag = true;
                if (flag && yourJson == "")
                {
                    string _otp = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select OTP from RegnRequest where MobileNo='{0}'", fgt.mobile.Trim())));
                    string clientId = Convert.ToString(Callingdatabase.GetScalarValue(string.Format("select clientid from RegnRequest where MobileNo='{0}'", fgt.mobile.Trim())));

                    string msg = "Your SMS verification code is:" + _otp;
                    Util ob = new Util();
                    ob.SendSMSthroughAPICallingApp(fgt.mobile, msg, clientId);

                    yourJson = "success";
                    serverResponse = "200";
                }
            }
            else
            {
                yourJson += "Invalid API Credentials ";
                serverResponse = "001";
            }


            DataVerifyOTP daa = new DataVerifyOTP()
            {
                result = yourJson,
                description = yourJson
            };

            VerifyOutput output = new VerifyOutput();
            output.statusCode = serverResponse;
            output.data = daa;

            var json = new JavaScriptSerializer().Serialize(output);

            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }


    }

}
