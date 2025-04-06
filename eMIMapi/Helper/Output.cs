using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class Output
    {
        public string statusCode;
        public DataApi data;
    }

    public class DataApi
    {
        public string result;
        public string description;
        public string apiKey;
        public string userId;
        public string isAdmin;
        public string name;
        public string mobile;
        public string emailId;
        public string scheduleAfter;
        public string image;

    }

    public class SaveRegnOutput
    {
        public string statusCode;
        public SaveRegnDataApi data;
    }

    public class SaveRegnDataApi
    {
        public string result;
        public string description;
        public string apiKey;       

    }
    public class DataVerifyOTP
    {
        public string result;
        public string description;
       
    }
    public class SaveCallOut
    {
        public string result;
        public string description;
        public string callId;
    }
    public class SaveCallOutput
    {
        public string statusCode;
        public SaveCallOut data;
    }
    public class VerifyOutput
    {
        public string statusCode;
        public DataVerifyOTP data;
    }

    public class ScheduleCallOutput
    {
        public string statusCode;
        public DataScheduleCall data;
    }

    public class DataScheduleCall
    {
        public string result;
        public string description;
        //public string scheduleId;
    }

    public class OutputCallList
    {
        public string statusCode;
        public DataCallList data;

    }
    public class DataCallList
    {
        public string result;
        public string description;
        public List<CallList> callLists;
    }

    public class OutputUserList
    {
        public string statusCode;
        public DataUserList data;

    }

    public class DataUserList
    {
        public string result;
        public string description;
        public List<userList> userLists;
    }


    public class OutputRegnList
    {
        public string statusCode;
        public DataRegnList data;

    }
    public class DataRegnList
    {
        public string result;
        public string description;
        public List<RegRequestUnApprove> regnLists;
    }

    public class OutputRegnDetail
    {
        public string statusCode;
        public DataRegnDetail data;
    }
    public class DataRegnDetail
    {
        public string result;
        public string description;
        public string emailId;
        public string mobileVerifiedOn;
        public string emailVerifiedOn;
    }

    public class OutputSmsList
    {
        public string statusCode;
        public DataSmsList data;

    }
    public class DataSmsList
    {
        public string result;
        public string description;
        public List<SMSList> smsLists;
    }


}
