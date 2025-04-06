using eMIMPanel.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace eMIMPanel
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ChecWebsite();
            //SendWabaSMSthroughAPI("919305334312", "", "91");
        }
        

       
        public class wabaMessage
        {
            public string statusCode { get; set; }
            public string statusDesc { get; set; }
            public string mid { get; set; }
        }

        public void ChecWebsite() {
             
            try
            {

                var client = new RestClient("http://103.205.64.220:17250/mimfootball/mim_footbal.aspx");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                string StatusCode = response.StatusCode.ToString();
                try
                { 

                    int statusCode = (int)response.StatusCode;
                    if (statusCode != 200 ) //Good requests
                    {
                         
                    }
                    
                }
                catch (WebException webException)
                {

                }
                finally
                {
                    
                }
            }
            catch (Exception ex)
            {

            }

           
        }
        public void SendWabaSMSthroughAPI(string mob, string msg, string CountryCode)
        {
            string APIKey = "";
            var body = "";
            var client = new RestClient("https://waba.myinboxmedia.in/api/sendwaba");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            if (CountryCode == "91")
            {
                APIKey = "m1nd492wq";
                body = @"{
              ""ProfileId"": ""MIM2200006"",
              ""APIKey"": """ + APIKey + @""",
              ""MobileNumber"": """  + mob + @""",
              ""templateName"": ""newacc_linkext2"",
              ""Parameters"": [
              ],
              ""HeaderType"":""TEXT""
              }
             }";
            }
            else if (CountryCode == "971")
            {
                APIKey = "0A42$D7C_8D6";
                body = @"{
              ""ProfileId"": ""MIM2200036"",
              ""APIKey"": """ + APIKey + @""",
              ""MobileNumber"": """ + "91" + mob + @""",
              ""templateName"": ""tmpnovfortext"",
              ""Parameters"": [
                ""123""
              ],
              ""HeaderType"":""TEXT""
              }
             }";
            }
           


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();
            try
            {
                if (pStatus.ToUpper() == "OK")
                {
                    wabaMessage res = new wabaMessage();

                   
                    res = JsonConvert.DeserializeObject<wabaMessage>(response.Content);

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                pStatus = ex.Message;
                //new Util().LogTemp("KarixTextTemplate $ Payload : " + Convert.ToString(body) + "$ Response:" + Convert.ToString(response.Content) + " $ Status :" + Convert.ToString(pStatus));
                
            }

        }
    }
}