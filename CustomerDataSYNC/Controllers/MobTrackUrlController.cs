using CustomerDataSYNC.Helper;
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

namespace CustomerDataSYNC.Controllers
{
    [RoutePrefix("ApiShortsURL")]
    public class MobTrackUrlController : ApiController
    {
        Util ob = new Util();
        [Route("MobTrackURL")]
        [HttpGet]
        public HttpResponseMessage InsertMobTrackURL(string ShortsSegment)
        {
            string request = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            List<Dictionary<string, string>> segmentsList = new List<Dictionary<string, string>>();
            try
            {
                DataTable dtMainShortsURL = new DataTable();
                dtMainShortsURL = database.GetDataTable(@"SELECT su.id AS ShortsURLID,mob.mobile AS MobileNo,mob.fileid AS Fileid,
                                                          mob.templateid AS TemplateID,su.RichMediaGroupRows
                                                          FROM mobtrackurl mob WITH(NOLOCK)
                                                          INNER JOIN short_urls su WITH(NOLOCK) ON mob.urlid = su.id
                                                          WHERE mob.segment = '" + ShortsSegment + "'");
                if (dtMainShortsURL.Rows.Count > 0)
                {
                    string ShortsURLID = Convert.ToString(dtMainShortsURL.Rows[0]["ShortsURLID"]);
                    string Fileid = Convert.ToString(dtMainShortsURL.Rows[0]["Fileid"]);
                    string MobileNo = Convert.ToString(dtMainShortsURL.Rows[0]["MobileNo"]);
                    string TemplateID = Convert.ToString(dtMainShortsURL.Rows[0]["TemplateID"]);
                    string RichMediaGroupRows = Convert.ToString(dtMainShortsURL.Rows[0]["RichMediaGroupRows"]);
                    if (RichMediaGroupRows != "")
                    {
                        DataTable dtButtonShortsUrl = new DataTable();
                        dtButtonShortsUrl = database.GetDataTable("SELECT id,segment FROM short_urls WITH(NOLOCK) WHERE RichMediaGroupRows='" + RichMediaGroupRows + "' AND ID <> '" + ShortsURLID + "'");
                        if (dtButtonShortsUrl.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtButtonShortsUrl.Rows.Count; i++)
                            {
                                string ButtonShortsURLID = Convert.ToString(dtButtonShortsUrl.Rows[i]["id"]);
                                string segment = Convert.ToString(dtButtonShortsUrl.Rows[i]["segment"]);
                                string guidString = Convert.ToString(Guid.NewGuid());
                                string NewSegment = guidString.Substring(guidString.Length - 8);
                                string sql = @"INSERT INTO mobtrackurl(urlid, mobile, sentdate, segment,fileid,templateid) 
                                   SELECT '" + ButtonShortsURLID + "', '" + MobileNo + "' , GETDATE(),'" + NewSegment + "', '" + Fileid + @"',
                                   '" + TemplateID + "'";

                                segmentsList.Add(new Dictionary<string, string>
                                {
                                  { "OldSegment", segment },
                                  { "NewSegment", NewSegment }
                                });
                            }
                        }
                    }
                }
                ob.Log("InsertMobTrackURL " + request);
            }
            catch (Exception ex)
            {
                ob.Log("InsertMobTrackURLError " + ex);
                request = Convert.ToString(ex);
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(segmentsList), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
