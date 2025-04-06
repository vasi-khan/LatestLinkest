using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class clsSMS2
    {
        public string userid { get; set; }
        public string pwd { get; set; }
        public string mobile { get; set; }
        public string sender { get; set; }
        public string msg { get; set; }
        public string msgtype { get; set; }
        public string peid { get; set; }
        public string templateid { get; set; }
        public string subclientcode { get; set; }
        public string failover { get; set; }
        public string WABATemplateName { get; set; }
        public string WABAVariables { get; set; }
        public string emailFrom { get; set; }
        public string emailTo { get; set; }
        public string emailCC { get; set; }
        public string emailSubject { get; set; }

        public string sendEmail { get; set; }   //   Y / N, y / n
        public string failOver2 { get; set; }

        public string param_1 { get; set; }
        public string param_1v { get; set; }
        public string param_2 { get; set; }
        public string param_2v { get; set; }
        public string param_3 { get; set; }
        public string param_3v { get; set; }
        public string param_4 { get; set; }
        public string param_4v { get; set; }

    }
    public class SMSLink
    {
        public string userid { get; set; }
        public string pwd { get; set; }
        public string apiKey { get; set; }
        public string mobile { get; set; }
        public string sender { get; set; }
        public string msg { get; set; }
        public string msgtype { get; set; }
        public string peid { get; set; }
        public string templateid { get; set; }
        public string longUrl { get; set; }
        public string existingUrl { get; set; }
        public string subclientcode { get; set; }
    }
}