using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class MissCall
    {
        public string dispnumber { get; set; }
        public string caller_id { get; set; }
        public string call_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string timezone { get; set; }
        public string call_duration { get; set; }
        public string destination { get; set; }
        public string action { get; set; }
        public string extension { get; set; }
    }
}