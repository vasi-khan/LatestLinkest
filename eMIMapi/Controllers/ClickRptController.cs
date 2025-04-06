using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using eMIMapi.Helper;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/clk")]
    public class ClickRptController : ApiController
    {
        bool bSecure = ConfigurationManager.AppSettings["SECURITY"].ToString() == "Y" ? true : false;

        [Route("GetRpt")]
        [HttpGet]
        public HttpResponseMessage GetClickReport(string userid, string pwd, string apikey, string datefrom, string dateto)
        {
        
    //check user name and password
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            if (dateto == null) { yourJson = "Invalid dateto"; ret = true; }
            if (datefrom == null) { yourJson = "Invalid datefrom"; ret = true; }
            if (apikey == null) { yourJson = "Invalid apikey"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            DateTime dtm;
            string[] formats = { "yyyy-MM-dd" };
            if (!ret)
                if (!DateTime.TryParseExact(datefrom, formats,
                            System.Globalization.CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dtm))
            { yourJson = "Invalid datefrom format. It should be yyyy-MM-dd"; ret = true; }

            if (!ret)
                if (!DateTime.TryParseExact(dateto, formats,
                           System.Globalization.CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out dtm))
            { yourJson = "Invalid dateto format. It should be yyyy-MM-dd"; ret = true; }

            Util ob = new Util();
            DataTable dt = new DataTable();
            if (!ret)
            {
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid Credentials"; ret = true; }
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }

                    if (!ret)
                        if (pwd != dt.Rows[0]["pwd"].ToString()) { yourJson = "Incorrect Password"; ret = true; }

                    if (!ret)
                        if (apikey != dt.Rows[0]["apikey"].ToString()) { yourJson = "Incorrect API KEY"; ret = true; }
                }
            }

            if (!ret)
            { 
                DataTable dt1 = ob.GetClickReport(userid, datefrom, dateto);
                if(dt1.Rows.Count>0)
                {
                    yourJson = JsonConvert.SerializeObject(dt1);
                }
                else
                { yourJson = "No Data Found"; ret = true; }
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;

        }

        [Route("GetClkRpt")]
        [HttpGet]
        public HttpResponseMessage GetClkReport(string userid, string pwd, string apikey, string datefrom, string dateto)
        {

            //check user name and password
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            bool ret = false;

            if (dateto == null) { yourJson = "Invalid dateto"; ret = true; }
            if (datefrom == null) { yourJson = "Invalid datefrom"; ret = true; }
            if (apikey == null) { yourJson = "Invalid apikey"; ret = true; }
            if (pwd == null) { yourJson = "Invalid Password"; ret = true; }
            if (userid == null) { yourJson = "Invalid User ID"; ret = true; }

            DateTime dtm;
            string[] formats = { "yyyy-MM-dd" };
            if (!ret)
                if (!DateTime.TryParseExact(datefrom, formats,
                            System.Globalization.CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dtm))
                { yourJson = "Invalid datefrom format. It should be yyyy-MM-dd"; ret = true; }

            if (!ret)
                if (!DateTime.TryParseExact(dateto, formats,
                           System.Globalization.CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out dtm))
                { yourJson = "Invalid dateto format. It should be yyyy-MM-dd"; ret = true; }

            Util ob = new Util();
            DataTable dt = new DataTable();
            if (!ret)
            {
                if (bSecure)
                {
                    dt = ob.GetUserParameterWithAPIKeySecure(userid, pwd, apikey);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid Credentials"; ret = true; }
                }
                else
                {
                    dt = ob.GetUserParameter(userid);
                    if (dt.Rows.Count <= 0) { yourJson = "Invalid User ID"; ret = true; }

                    if (!ret)
                        if (pwd != dt.Rows[0]["pwd"].ToString()) { yourJson = "Incorrect Password"; ret = true; }

                    if (!ret)
                        if (apikey != dt.Rows[0]["apikey"].ToString()) { yourJson = "Incorrect API KEY"; ret = true; }
                }
            }

            if (!ret)
            {
                DataTable dt1 = ob.GetClickReport2(userid, datefrom, dateto);
                if (dt1.Rows.Count > 0)
                {
                    yourJson = JsonConvert.SerializeObject(dt1);
                }
                else
                { yourJson = "No Data Found"; ret = true; }
            }
            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;

        }
    }
}
