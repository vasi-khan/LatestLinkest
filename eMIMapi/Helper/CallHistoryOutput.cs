using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class CallHistoryOutput
    {
        public string statusCode;
        public CallHistoryData data;

    }

    public class CallHistoryData
    {
        public string dialed;
        public string connected;
        public string upcoming;
        public Gdata_Dialed gdata_dialed;
        public Gdata_Connected gdata_connected;
    }

    public class Gdata_Dialed
    {
        public string day1;
        public string day2;
        public string day3;
        public string day4;
        public string day5;
        public string day6;

    }

    public class Gdata_Connected
    {
        public string day1;
        public string day2;
        public string day3;
        public string day4;
        public string day5;
        public string day6;

    }

    public class SMSHistoryOutput
    {
        public string statusCode;
        public SMSHistoryData data;

    }

    public class SMSHistoryData
    {
        public string sent;
        public string received;
        public string schedule;
        public GSMSdata_Sent gsmsdata_sent;
        public GSMSdata_Received gsmsdata_received;
    }

    public class GSMSdata_Sent
    {
        public string day1;
        public string day2;
        public string day3;
        public string day4;
        public string day5;
        public string day6;

    }
    public class GSMSdata_Received
    {
        public string day1;
        public string day2;
        public string day3;
        public string day4;
        public string day5;
        public string day6;

    }

    public class SMSList
    {
        public string mobile;
        public string sentRcvdOn;
        public string smsText;
    }

    public class CallList
    {
        public string mobile;
        public string callStartedAt;
        public string remarks;
        public string id;
    }
    public class userList
    {
        public string mobile;
        public string userId;
        public string status;
    }

    public class RegRequestUnApprove
    {
        public string mobile;
        public string name;

    }
}