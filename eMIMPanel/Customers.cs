using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMPanel
{
    class Customers
    {
       // public string Sln           { get; set; }
        public string CompName    { get; set; }
        public string SenderID     { get; set; }
        public string Mobile1      { get; set; }
        public string Email     { get; set; }
        public string Balance      { get; set; }
        public string CreatedBy { get; set; }
        public string Actions   { get; set; }
    }

    class CustomersWithBalance
    {
        public string Sln { get; set; }
        public string CompName { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string SenderID { get; set; }
        public string Mobile1 { get; set; }
        public string Email { get; set; }
        public string Balance { get; set; }
        public string Actions { get; set; }
        public string rate_normalsms { get; set; }
        public string rate_smartsms { get; set; }
        public string rate_campaign { get; set; }
        public string rate_otp { get; set; }
        public string urlrate { get; set; }
        public string dltcharge { get; set; }
    }

    class clickcnt
    {
        public string sln { get; set; }
        public string username { get; set; }
        public string shorturl { get; set; }
        public string clickcount { get; set; }
        public string Actions { get; set; }
    }
}