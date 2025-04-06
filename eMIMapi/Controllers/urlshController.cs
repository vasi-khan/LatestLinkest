using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using eMIMapi.Helper;


namespace eMIMapi.Controllers
{
    [RoutePrefix("api/mim/url")]

    public class urlshController : ApiController
    {
        bool bSecure = ConfigurationManager.AppSettings["SECURITY"].ToString() == "Y" ? true : false;

        [Route("shortner")]
        [HttpGet]
        public string shortner(string userid, string pwd, string longURL)
        {
            if (string.IsNullOrEmpty(userid)) return "Invalid User ID";
            if (string.IsNullOrEmpty(pwd)) return "Invalid Password";
            if (string.IsNullOrEmpty(longURL)) return "Invalid Long URL";
            string shurl = longURL;

            if (!shurl.StartsWith("http://") && !shurl.StartsWith("https://")) return "Invalid Long URL. It should start with http:// or https://";

            string sql = string.Format("insert into APIURLLog (userId,longurl) values('{0}','{1}')", userid, shurl.Replace("'", "''"));
            database.ExecuteNonQuery(sql);

            Util ob = new Util();

            DataTable dt;
            if (bSecure)
            {
                dt = ob.GetUserParameterSecure(userid, pwd);//For ApiKey
                if (dt.Rows.Count <= 0)
                {
                    return "Invalid Credentials";
                }
            }
            else
            {
                dt = ob.GetUserParameter(userid);

                if (dt.Rows.Count <= 0) return "Invalid User ID";
                if (pwd != dt.Rows[0]["APIKEY"].ToString()) return "Incorrect Password";
            }
            string domain = dt.Rows[0]["domainname"].ToString();

            

            double bal = Convert.ToDouble(dt.Rows[0]["balance"]);
            double rate = Convert.ToDouble(dt.Rows[0]["urlrate"]);

            if (bal - rate < 0)
            {
                return "Insufficient Balance. Cannot create Short URL.";
            }

            string lblShortURL = "";
            string segment = ob.GetShortURLofLongURL(userid, longURL);
            if (segment == "")
            {
                string sUrl = ob.NewShortURLfromSQL(domain);
                ob.SaveShortURL(userid, shurl, "", sUrl, "N", "Y", domain);
                lblShortURL = domain + sUrl;
            }
            else
                lblShortURL = segment;

            string bal2 = ob.UdateAndGetURLbal1(userid, lblShortURL);
            //lblURLbal.Text = bal;
            // Session["SMSBAL"] = bal2;

            return "Short URL: " + lblShortURL;
        }


    }
}

