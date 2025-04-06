using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class SaveInputCall
    {
        public string apikey { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string calledmobile { get; set; }
        public string callinitiateat { get; set; }
        public string callstartedat { get; set; }
        public string callendedat { get; set; }
        public string callstatus { get; set; }
        public string feedback { get; set; }
        //public string scheduleDate { get; set; }
        //public string scheduleTime { get; set; }
        public string scheduleMode { get; set; }
        public string callrecording { get; set; }
    }

    public class SaveScheduleCall
    {
        public string apikey { get; set; }
        public string userId { get; set; }
        public string scheduleMode { get; set; }
        public string scheduleDate { get; set; }
        public string scheduleTime { get; set; }       
        public string remarks { get; set; }
        public string callId { get; set; }
        public string clientName { get; set; }
        public string clientMobile { get; set; }
        public string clientEmail { get; set; }
        public attendee[] attendee { get; set; }

    }

    public class attendee
    {
        public string attendeeName { get; set; }
        public string attendeeMobile { get; set; }
        public string attendeeEmail { get; set; }
    }

    public class SaveAttendee
    {
        public string apikey { get; set; }
        public string userId { get; set; }
        public string scheduleId { get; set; }
        public string attendeeName { get; set; }
        public string attendeeMobile { get; set; }        
    }

    public class SaveInputSMS
    {
        public string apiKey { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string sentRcvd { get; set; }
        public string sentRcvdOn { get; set; }
        public string smsText { get; set; }
        
    }

  
}