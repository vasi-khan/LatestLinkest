using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMPanel.Helper
{
    public class Output
    {
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class waRoot
    {
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
        public string mid { get; set; }
    }
   


    public class OriginalNetwork
    {
        public string networkName { get; set; }
        public string networkPrefix { get; set; }
        public string countryName { get; set; }
        public string countryPrefix { get; set; }
    }

    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Error
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool permanent { get; set; }
    }

    public class Result
    {
        public string to { get; set; }
        public string mccMnc { get; set; }
        public string imsi { get; set; }
        public OriginalNetwork originalNetwork { get; set; }
        public bool ported { get; set; }
        public bool roaming { get; set; }
        public string servingMSC { get; set; }
        public Status status { get; set; }
        public Error error { get; set; }
    }

    public class Root
    {
        public List<Result> results { get; set; }
    }


}