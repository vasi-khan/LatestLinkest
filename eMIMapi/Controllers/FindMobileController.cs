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

namespace eMIMapi.Controllers
{
    [RoutePrefix("api/FindMobile")]
    public class FindMobileController : ApiController
    {
        [Route("MobileList")]
        [HttpGet]
        public async Task<HttpResponseMessage> MobileList(string clientId)
        {           
            string yourJson = "";
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            if (clientId == "") { yourJson = "Invalid ClientID";  }

            if (yourJson == "")
            {
                string _clientid = Convert.ToString(dbWAContact.GetScalarValue(string.Format("select clientId from ContactList where clientId='{0}'", clientId)));
                if (_clientid != clientId.Trim()) { yourJson = "Invalid clientID "; }
            }

            if (yourJson == "")
            {
                string sql = string.Format("select Mobile,Name from ContactList where clientId ='{0}' ", clientId.Trim());
                DataTable dt = dbWAContact.GetDataTable(sql);               
                yourJson = JsonConvert.SerializeObject(dt);
            }
            else 
            {
                DataTable dt1 = new DataTable();               
                dt1.Columns.Add("Result");
                DataRow dr = dt1.NewRow();               
                dr[0] = yourJson;
                dt1.Rows.Add(dr);
                yourJson = JsonConvert.SerializeObject(dt1);
            }

            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
            return response;
        }


    }
}
