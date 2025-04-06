using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMapi.Helper
{
    public class RegistrationDtl
    {
        public string apiKey { get; set; }
        public string secret { get; set; }
        public string clientId { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string emailId { get; set; }
        public string compName { get; set; }
        public string otp { get; set; }
    }

    public class CreateUser
    {
        public string apiKey { get; set; }
        public string mobile { get; set; }
        public string userId { get; set; }
        public string password { get; set; }

    }
    public class UpdateUserStatus
    {
        public string apiKey { get; set; }
        public string userId { get; set; }
        public string status { get; set; }

    }

    public class UploadUserImages
    {
        public string apiKey { get; set; }
        public string userId { get; set; }
        public string profileImage { get; set; }

    }

    public class ForgetPasswords
    {
        public string userId { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }

    }

}