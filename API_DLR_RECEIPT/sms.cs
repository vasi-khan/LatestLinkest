using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMPP_APP
{
    public class sms
    {
        public string ID { get; set; }
        public string PROVIDER { get; set; }
        public int SMPPACCOUNTID { get; set; }
        public string PROFILEID { get; set; }
        public string MSGTEXT { get; set; }
        public string TOMOBILE { get; set; }
        public string SENDERID { get; set; }
        public DateTime CREATEDAT { get; set; }
    }
}
