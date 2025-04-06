using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMPanel
{
    class smsreport
    {
        public string sln { get; set; }
        public string UserName { get; set; }
        public string SenderID { get; set; }
        public string Submitted { get; set; }
        public string Delivered { get; set; }
        public string Failed { get; set; }
        public string Unknown { get; set; }
    }

    class smsreportuser
    {
        public string sln { get; set; }
        public string msgid { get; set; }
        public string mobile { get; set; }
        public string senderid { get; set; }
        public string msgtext { get; set; }
        public string senttime { get; set; }
        public string dlrstat { get; set; }
    }

    class smsreportuser_new
    {
        public string sln { get; set; }
        public string msgid { get; set; }
        public string mobile { get; set; }
        public string senderid { get; set; }
        public string msgtext { get; set; }
        public string senttime { get; set; }
        public string dlrtime { get; set; }
        public string dlrstat { get; set; }
        public string dlrresp { get; set; }
        public string Seenstatus { get; set; }
        public string SeenDate { get; set; }

    }
}