using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using eMIMapi.Helper;
using Newtonsoft.Json;
using System.Globalization;

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/record")]
    public class RecordingController : ApiController
    {
        [Route("SaveRecordings")]
        [HttpPost]

        public async Task<HttpResponseMessage> SaveRecordings([FromBody] Recording obj)
        {
            string yourJson = "";
            bool flag = false;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            string uId;
            string apiKey;
            string recDt;
            string recData;

            uId = obj.UserId;
            apiKey = obj.APIKey;

            recDt = obj.RecordingTime;
            recData = obj.RecordingData;

            if (recDt == "") { yourJson = "Invalid RecordingTime"; }
            if (recData == "") { yourJson = "Invalid RecordingData"; }

            if (uId == "ae71648119a1" && apiKey == "D17800F984F04B56929A3CEF78B7ED30")
            {
                flag = true;
                if (flag && yourJson == "")
                {
                    string sql = string.Format("Insert into SaveRecording values('{0}',CAST('{1}' AS VARBINARY(MAX)))", recDt, recData);
                    dbClient.ExecuteNonQuery(sql);
                    yourJson = "success";
                }
            }
            else
                yourJson = "Invalid UId/API Key";


            DataTable dt1 = new DataTable("dt");
            dt1.Columns.Add("msg");
            DataRow dr = dt1.NewRow();
            dr[0] = yourJson;
            dt1.Rows.Add(dr);
            yourJson = JsonConvert.SerializeObject(dt1);

            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }

    }
}
