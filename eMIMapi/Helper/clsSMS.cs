using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class clsSMS
    { 
        public string userid { get; set; }
        public string pwd { get; set; }
        public string mobile { get; set; }
        public string sender { get; set; }
        public string msg { get; set; }
        public string msgtype { get; set; }
        public string peid { get; set; }
    }
    
    public class clsSchedule
    {
        public string userid { get; set; }
        public string pwd { get; set; }
        public List <mobile> mobile { get; set; }
        public string sender { get; set; }
        public string msg { get; set; }
        public string msgtype { get; set; }
        public string scheduleDateTime { get; set; }
        public string peid { get; set; }
        public string templateid { get; set; }
    }

    public class mobile
    {
        public string to { get; set; }
    }

    public class clsCancelSchedule
    {
        public string userid { get; set; }
        public string pwd { get; set; }
        public string scheduleDateTimeFrom { get; set; }
        public string scheduleDateTimeTo { get; set; }
    }
}
