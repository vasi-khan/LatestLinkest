using Shortnr.Web.Business;
using Shortnr.Web.Entities;
using eMIMPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using System.Web.Hosting;
using System.ComponentModel;
using Shortnr.Web.Data;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using RestSharp;
using System.Web.Configuration;
using System.Net;
using eMIMPanel.Helper;
using System.Web.Script.Serialization;
using System.IO;
using MIMPwdUtility;


namespace eMIMPanel.Controllers
{
    public class UrlController : Controller
    {
        public string GeoLocationlicense_key = "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";
        private IUrlManager _urlManager;
        Helper.Util ob = new Helper.Util();

        public UrlController(IUrlManager urlManager)
        {
            this._urlManager = urlManager;
        }
        public UrlController()
        {
        }
        // GET: Url
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Click(string segment)
        {
            try
            {
                string SourceIP = String.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? Request.ServerVariables["REMOTE_ADDR"] : Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
                var u = Request.Headers["User-Agent"].ToString().ToLower();
                bool User_AgentAutoClick = false;
                string User_Agent = "";
                if (u.Contains("snippet") || u.Contains("googlebot") || u.Contains("antispam")
                    || u.Contains("whatsapp") || u.Contains("facebot") || u.Contains("okhttp")
                    || u.Contains("apache") || u.Contains("firefox"))
                {
                    //User_AgentAutoClick = true;
                    User_Agent = u;
                    //return this.RedirectPermanent("LinkExpired.html");

                    #region Start
                    string referer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : string.Empty;
                    DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
                    DeviceDetectorSettings.RegexesDirectory = HostingEnvironment.MapPath("~/bin/");
                    var userAgent = Request.UserAgent;
                    var result = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);
                    string browser = "Unknown", platform = "Unknown", ismobile = "false", manuf = "Unknown", model = "Unknown";
                    if (!result.Success)
                    {}
                    else
                    {
                        string[] st = result.ToString().Split(';');
                        ismobile = Convert.ToString(st[0]).Trim().ToUpper() != "DESKTOP" ? "true" : "false";
                        manuf = Convert.ToString(st[1]).Trim() == "" ? "Unknown" : Convert.ToString(st[1]).Trim();
                        model = Convert.ToString(st[2]).Trim() == "" ? "Unknown" : Convert.ToString(st[2]).Trim();
                        platform = Convert.ToString(st[4]).Trim() == "" ? "Unknown" : Convert.ToString(st[4]).Trim();
                        browser = Convert.ToString(st[3]).Trim() == "" ? "Unknown" : Convert.ToString(st[3]).Trim();
                    }                    
                    string lurl = "";
                    bool callBackApplicable = false;
                    try
                    {
                        callBackApplicable = Convert.ToString(ConfigurationManager.AppSettings["CLICK_CALLBACK_APPLICABLE"]) == "Y" ? true : false;
                    }
                    catch (Exception) { }
                    lurl = ob.InsertInMobStatusClick(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, callBackApplicable, User_AgentAutoClick, User_Agent);
                    #endregion

                }

                {
                    if (ob.restrictClick(SourceIP))
                    {
                        return this.RedirectPermanent("LinkExpired.html");
                    }
                    else
                    {
                        if (segment == "apollopharma")
                        {
                            ob.InsertInApollo();
                            return this.RedirectPermanent("https://spinthewheel.apollopharmacy.in/apollopharma.aspx");
                        }
                        else if (segment == "aviorn")
                        {
                            string sit = Convert.ToString(database.GetScalarValue("SELECT long_url FROM short_urls WITH(NOLOCK) WHERE segment='" + segment + "'"));
                            sit = sit.Trim() + "?id=" + Security.Encrypt(DateTime.Now.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                            return this.RedirectPermanent(sit);
                        }
                        else
                        {
                            string ws = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, "/");
                            //string ws = string.Format("{1}{2}", Request.Url.Authority, "/");
                            //ws = "http://m1m.io/";
                            if (segment.Substring(0, 2) == "v-" && segment.Length == 8)
                            {
                                // if verification then -
                                bool verified = ob.Valid_VerificationLink(segment);
                                if (verified)
                                    Response.Redirect("LinkVerified.html");
                                else
                                    Response.Redirect("LinkExpired.html");

                                return this.RedirectPermanent("LinkExpired.html");
                            }
                            else
                            {
                                bool NotExistsUrl = ob.IsURLExists(segment, ws);
                                if (NotExistsUrl)
                                {
                                    Response.Redirect("Unavailable.aspx");
                                }

                                bool showUrl = ob.IsShowURL(segment, ws);
                                if (!showUrl)
                                {
                                    Response.Redirect("TempUnavailable.html");
                                }
                                string referer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : string.Empty;

                                //Gettng device information
                                DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
                                //DeviceDetectorSettings.RegexesDirectory = Server.MapPath("~/bin/");
                                DeviceDetectorSettings.RegexesDirectory = HostingEnvironment.MapPath("~/bin/");

                                var userAgent = Request.UserAgent;
                                var result = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);
                                string browser = "Unknown", platform = "Unknown", ismobile = "false", manuf = "Unknown", model = "Unknown";

                                if (!result.Success)
                                {
                                    //return Content("Unknown");
                                }
                                else
                                {
                                    string[] st = result.ToString().Split(';');
                                    ismobile = Convert.ToString(st[0]).Trim().ToUpper() != "DESKTOP" ? "true" : "false";
                                    manuf = Convert.ToString(st[1]).Trim() == "" ? "Unknown" : Convert.ToString(st[1]).Trim();
                                    model = Convert.ToString(st[2]).Trim() == "" ? "Unknown" : Convert.ToString(st[2]).Trim();
                                    platform = Convert.ToString(st[4]).Trim() == "" ? "Unknown" : Convert.ToString(st[4]).Trim();
                                    browser = Convert.ToString(st[3]).Trim() == "" ? "Unknown" : Convert.ToString(st[3]).Trim();
                                }

                                string shorturlid = "";
                                string lurl = "";
                                bool callBackApplicable = false;
                                bool alreadyclicked = false; 

                                if (Convert.ToString(ConfigurationManager.AppSettings["HOMECR"]) == "Y")
                                {
                                    try
                                    { callBackApplicable = Convert.ToString(ConfigurationManager.AppSettings["CLICK_CALLBACK_APPLICABLE"]) == "Y" ? true : false; }
                                    catch (Exception) { }
                                    lurl = ob.SaveMobStatus(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, callBackApplicable, User_AgentAutoClick, User_Agent);
                                }
                                else
                                {
                                    if ((segment.Length != 8) && (!(segment.ToLower().Contains("oncquest") && segment.Length == 16)))
                                    {
                                        lurl = ob.SaveStatus(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, User_AgentAutoClick, User_Agent);
                                        SaveLocation(segment, Request.UserHostAddress, "N", SourceIP, Convert.ToString(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]), Convert.ToString(Request.ServerVariables["REMOTE_ADDR"]), User_AgentAutoClick);
                                        shorturlid = ob.GetUrlIDforReDirect(segment);
                                    }
                                    else
                                    {
                                        if ((segment.Length == 8) && segment.Substring(6, 2).ToUpper() == "_Q")
                                        {
                                            lurl = ob.SaveStatus(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, User_AgentAutoClick, User_Agent);
                                            shorturlid = ob.GetUrlIDforReDirect(segment);
                                        }
                                        else
                                        {
                                            try
                                            { callBackApplicable = Convert.ToString(ConfigurationManager.AppSettings["CLICK_CALLBACK_APPLICABLE"]) == "Y" ? true : false; }
                                            catch (Exception) { }

                                            alreadyclicked = ob.CheckAlreadyClicked(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, callBackApplicable, User_AgentAutoClick, User_Agent);
                                            lurl = ob.SaveMobStatus(segment, referer, Request.UserHostAddress, browser, platform, ismobile, manuf, model, callBackApplicable, User_AgentAutoClick, User_Agent);
                                            try
                                            {
                                                SaveLocation(segment, Request.UserHostAddress, "Y", SourceIP, Convert.ToString(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]), Convert.ToString(Request.ServerVariables["REMOTE_ADDR"]), User_AgentAutoClick);
                                            }
                                            catch (Exception ex)
                                            {
                                                ob.Log(ex.ToString());
                                            }
                                            shorturlid = ob.GetMobTrkUrlIDforReDirect(segment);
                                        }
                                    }
                                }

                                if (!alreadyclicked)
                                {
                                    string sql = @"SELECT * FROM SendWABAOnLinkClick WITH(NOLOCK) WHERE Active=1 AND shorturlid ='" + shorturlid + "'";
                                    DataTable dt = database.GetDataTable(sql);
                                    if (dt.Rows.Count > 0)
                                    {
                                        string mobnum = ob.GetMobileFromSegment(segment);
                                        string WABATempName = dt.Rows[0]["WABATemplateName"].ToString().Trim();
                                        string WABAUserID = dt.Rows[0]["WABAUserID"].ToString().Trim();
                                        string WABAApyKey = dt.Rows[0]["WABAUserAPIKey"].ToString().Trim();
                                        string WABATempType = dt.Rows[0]["WABATemplateTempType"].ToString().Trim();
                                        string URL = dt.Rows[0]["Url"].ToString().Trim();
                                        string WABAVariables = dt.Rows[0]["WabaParamWithDelimiter"].ToString().Trim();

                                        MIMPwdUtility.MIMUtil MIMPwd = new MIMUtil();
                                        string LogWabaAPI = await MIMPwd.SendWABAthroughAPI(mobnum, WABATempName, WABAVariables, WABAUserID, WABAApyKey, WABATempType, URL);
                                        ob.logAPIwaba(LogWabaAPI);
                                        //ob.SendWABAthroughAPI(mobnum, WABATempName, WABAVariables, WABAUserID, WABAApyKey, WABATempType, URL);
                                        if (lurl.Contains(ws + "MEDIAhtml/"))
                                        {
                                            return this.RedirectPermanent(lurl + "?ShortsSegment=" + segment);
                                        }
                                        else
                                        {
                                            return this.RedirectPermanent(lurl);
                                        }
                                    }
                                }

                                if (lurl == "https://linkext.io/Thankyou.html")
                                {
                                    //GET MOBILE 
                                    string mobnum = ob.GetMobileFromSegment(segment);
                                    string WABAVariables = "Welcome";
                                    ob.SendWABAthroughAPI(mobnum, "tempfeb7", WABAVariables, "MIM2200038", "FE1F$FD9_738", "IMAGE", "https://linkext.io/IMG/MIMIMG.JPEG");
                                    return this.RedirectPermanent(lurl);
                                }
                                else if (lurl == "https://m1m.io/CFCC32")
                                {
                                    //GET MOBILE 
                                    string mobnum = ob.GetMobileFromSegment(segment);
                                    string WABAVariables = "";
                                    ob.SendWABAthroughAPI(mobnum, "campaign17feb", WABAVariables, "MIM2200057", "75CD$F99_4DE", "IMAGE", "https://waba.linkext.io/uploads/Header_20230217021026.png");
                                    return this.RedirectPermanent(lurl);
                                }
                                else
                                {
                                    if (segment == "MiM2")
                                        return this.RedirectPermanent("m1m.aspx?s=" + shorturlid);
                                    else if (segment == "Merc-Benz")
                                        return this.RedirectPermanent("m1m2.aspx?s=" + shorturlid);
                                    else
                                    {
                                        if (callBackApplicable)
                                        {
                                            string[] str = lurl.Split('~');
                                            lurl = str[3];
                                            System.Threading.Thread t1 = new System.Threading.Thread(() =>
                                            {
                                                processCallBackClick(str[0], str[1], str[2], ws + segment, lurl, segment);
                                            });
                                            t1.Start();
                                        }
                                        if (lurl.Contains(ws + "MEDIAhtml/"))
                                        {
                                            return this.RedirectPermanent(lurl + "?ShortsSegment=" + segment);
                                        }
                                        else
                                        {
                                            return this.RedirectPermanent(lurl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void processCallBackClick(string userid, string mobile, string senttime, string shorturl, string longurl, string segment)
        {
            try
            {
                string url = Convert.ToString(database.GetScalarValue("Select ClickDataPushHookAPI from customer with (nolock) where username='" + userid + "'"));
                if (url != "")
                {
                    string sql = "Select Profileid,tomobile,msgid,msgtext,smstext into #t from msgsubmitted m with (nolock) where tomobile='" + mobile + "' /* and sentdatetime>= convert(date,getdate()) */ ; " +
                        " Select top 1 msgid from #t where msgtext like '%" + segment + "%' ";
                    string msgid = Convert.ToString(database.GetScalarValue(sql));
                    string clkTime = Convert.ToString(database.GetScalarValue("select convert(varchar,getdate(),106) + ' ' + convert(varchar,getdate(),108)"));
                    clkCallBackAPI(url, mobile, senttime, clkTime, shorturl, longurl, msgid);
                }
            }
            catch (Exception ex) { }
        }

        public void clkCallBackAPI(string url, string mobile, string senttime, string clkTime, string shorturl, string longurl, string msgid)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            //request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{
                            ""STATUS"" : ""SUCCESS"",
                            ""LONG_URL"": """ + longurl + @""",
                            ""SHORT_URL"": """ + shorturl + @""",
                            ""MOBILE"": """ + mobile + @""",
                            ""MSGID"": """ + msgid + @""",
                            ""SMS_DATE"": """ + senttime + @""",
                            ""CLICK_DATE"": """ + clkTime + @"""
                        }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            insertClickCallBackCalled(url, mobile, senttime, clkTime, shorturl, longurl, msgid, Convert.ToString(response.Content));
            //WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);
            Root res = JsonConvert.DeserializeObject<Root>(response.Content);

            try
            {
                LogErrClkCallBack(url, body, res.STATUS);
            }
            catch (Exception EX)
            {
                if (response.ErrorException.InnerException.Message != null)
                    LogCBErr("url- " + url + " body- " + body + " excep-" + EX.Message + " RespException-" + Convert.ToString(response.ErrorException.InnerException.Message));
                else
                    LogCBErr("url- " + url + " body- " + body + " excep-" + EX.Message + " Response-" + Convert.ToString(response.Content));
            }
        }
        public void LogErrClkCallBack(string url, string body, string status)
        {
            string fn = ConfigurationManager.AppSettings["LOGPATH"].ToString();
            try
            {

                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"URLCallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {

                FileStream filestrm = new FileStream(fn + @"URLcatch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + " URL-" + url + " status-" + status + " body-" + body);
                strmwriter.Flush();
                strmwriter.Close();
            }
        }
        public void LogCBErr(string msg)
        {
            string fn = ConfigurationManager.AppSettings["LOGPATH"].ToString();
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_CallBack_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception exx) { }
            }
        }
        public void insertClickCallBackCalled(string url, string mobile, string senttime, string clkTime, string shorturl, string longurl, string msgid, string response)
        {
            string sql = "";
            try
            {
                if (response.Length > 4000) response = response.Substring(0, 3998);
                sql = "insert into clickcallbackCalled (longurl,shorturl,mobile,msgid,senttime,clkTime,URL,APIRESP) " +
                    "values ('" + longurl + "','" + shorturl + "','" + mobile + "','" + msgid + "','" + senttime + "','" + clkTime + "','" + url + "','" + response + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                { LogCBErr("insert into clickcallbackCalled - " + ex.Message); }
            }
        }
        public void SaveLocation(string segment, string IP, string MobStat, string origIP, string ipall, string iprem, bool isBotClick)
        {

            if (segment.Length == 8)
            {
                string mob = ob.GetMobileFromSegment(segment);

                //if (mob.Length == 12 && mob.Substring(0, 3) == "971") return;

                if (mob.Length == 10)
                {
                    mob = "91" + mob;
                }
                //string APIAUTHCODE = "Basic " + WebConfigurationManager.AppSettings["APIAUTHCODE"];
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //var client = new RestClient("https://dm3gpv.api.infobip.com/number/1/query");
                //client.Timeout = -1;
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("Authorization", APIAUTHCODE);
                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Accept", "application/json");
                //request.AddParameter("application/json", "{\"to\":[\""+ mob + "\"]}", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //var result = new JavaScriptSerializer().Deserialize<Root>(response.Content);
                //ob.Log(result.results[0].originalNetwork.networkName);
                //string operators = ""; string cityName = "";
                //if (result.results[0].originalNetwork.networkName.Contains("Other networks"))
                //{
                //    operators = "Other networks";
                //    cityName = "India";
                //}
                //else
                //{
                //    operators = result.results[0].originalNetwork.networkName.Replace("//", "/").Split('/')[0].Trim();
                //    cityName = result.results[0].originalNetwork.networkName.Replace("//", "/").Split('/')[1].Trim();
                //}

                //string countryName =result.results[0].originalNetwork.countryName;
                //bool ported = result.results[0].ported;
                //bool roaming = result.results[0].roaming;
                //bool permanent = result.results[0].error.permanent;

                //DataTable dt = ob.GetOperatorAndLocation(mob);
                //if (dt.Rows.Count > 0)
                //ob.SaveIPLocationNew(mob, segment, Convert.ToString(dt.Rows[0]["operator"]), false, false, false, Convert.ToString(dt.Rows[0]["country"]), Convert.ToString(dt.Rows[0]["regionName"]));

                string country = "";
                if (isBotClick) country = "USA";
                else
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var client = new RestClient("https://geolite.info/geoip/v2.1/country/" + IP + "?pretty");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Basic MTA0MDI0MToyUTQyTmlfUGNzT2RvZFBBbElHN0RISVZQd1FUNEdFd1lEMzdfbW1r");
                    IRestResponse response = client.Execute(request);
                    var responsjson = response.Content.ToString();

                    geoLite mygeoLite = JsonConvert.DeserializeObject<geoLite>(responsjson);

                    country = mygeoLite.country.names.en;
                }
                ob.SaveIPLocationNew(mob, segment, "", false, false, false, country, "");
                // ob.SaveIPLocationNew(mob, segment, operators, ported, roaming, permanent, countryName, cityName);

            }

            //string apikey = ob.getIPapikey();
            //string url = "https://pro.ip-api.com/json/" + IP + "?key=" + apikey;
            //string getResponseTxt = "";
            //string getStatus = "";
            //WinHttp.WinHttpRequest objWinRq;
            //objWinRq = new WinHttp.WinHttpRequest();
            //try
            //{
            //    objWinRq.Open("GET", url, false);
            //    objWinRq.SetTimeouts(30000, 30000, 30000, 30000);
            //    objWinRq.Send(null);

            //    while (!(getStatus != "" && getResponseTxt != ""))
            //    {
            //        getStatus = objWinRq.Status + objWinRq.StatusText;
            //        getResponseTxt = objWinRq.ResponseText;
            //    }
            //    getResponseTxt = "[" + getResponseTxt + "]";
            //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(getResponseTxt, (typeof(DataTable)));

            //    ob.SaveIPLocation(dt, segment, MobStat, origIP, ipall, iprem);
            //}
            //catch (Exception EX)
            //{
            //    throw EX;
            //}
        }
    }
    public class Root
    {
        public string STATUS { get; set; }
    }

    // geoLite myDeserializedClass = JsonConvert.DeserializeObject<geoLite>(myJsonResponse);
    public class Continent
    {
        public string code { get; set; }
        public int geoname_id { get; set; }
        public Names names { get; set; }
    }

    public class Country
    {
        public string iso_code { get; set; }
        public int geoname_id { get; set; }
        public Names names { get; set; }
    }

    public class Names
    {
        public string es { get; set; }
        public string fr { get; set; }
        public string ja { get; set; }

        [JsonProperty("pt-BR")]
        public string ptBR { get; set; }
        public string ru { get; set; }

        [JsonProperty("zh-CN")]
        public string zhCN { get; set; }
        public string de { get; set; }
        public string en { get; set; }
    }

    public class RegisteredCountry
    {
        public string iso_code { get; set; }
        public int geoname_id { get; set; }
        public Names names { get; set; }
    }

    public class geoLite
    {
        public Continent continent { get; set; }
        public Country country { get; set; }
        public RegisteredCountry registered_country { get; set; }
        public Traits traits { get; set; }
    }

    public class Traits
    {
        public string ip_address { get; set; }
        public string network { get; set; }
    }


}