using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;

namespace eMIMPanel
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public void IsWABAExist(string username)
        {
            string result = "";
            Helper.Util ob = new Helper.Util();
            DataTable dtW = ob.GetUserParameter(username);
            if (dtW.Rows.Count > 0)
            {
                if (dtW.Rows[0]["WABARCS"] == DBNull.Value || Convert.ToBoolean(dtW.Rows[0]["WABARCS"]) == false)
                    result = "NO";
                else
                    result = "YES";
            }
            Context.Response.Write(result);
        }


        [WebMethod]
        public void GetCustomers()
        {
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetCustomers("", "", "", "");
            var customer = new List<Customers>();
            foreach (DataRow dr in dt.Rows)
            {
                var cust = new Customers
                {
                    //Sln =dr[0].ToString(),
                    CompName = dr[0].ToString(),
                    SenderID = dr[1].ToString(),
                    Mobile1 = dr[2].ToString(),
                    Email = dr[3].ToString(),
                    Balance = dr[4].ToString(),
                    CreatedBy = dr[5].ToString(),
                    Actions = dr[6].ToString()

                };
                customer.Add(cust);
            }
            var js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(customer));
        }

        [WebMethod]
        public void GetCustomersWithBalance(string dater)
        {
            Helper.Util ob = new Helper.Util();
            string user = "";
            string dlt = "";
            string usertype = "";
            if (dater != "")
            {
                string[] dtr = dater.Split('$');
                user = dtr[0];
                dlt = dtr[1];
                usertype = dtr[2];
            }
            DataTable dt = ob.GetCustomersWithBalance(user, dlt, usertype);
            var customer = new List<CustomersWithBalance>();
            foreach (DataRow dr in dt.Rows)
            {
                var cust = new CustomersWithBalance
                {
                    Sln = dr[0].ToString(),
                    CompName = dr[1].ToString(),
                    Name = dr[2].ToString(),
                    UserName = dr[3].ToString(),
                    SenderID = dr[4].ToString(),
                    Mobile1 = dr[5].ToString(),
                    Email = dr[6].ToString(),
                    Balance = dr[7].ToString(),
                    Actions = dr[8].ToString(),
                    rate_normalsms = dr[9].ToString(),
                    rate_smartsms = dr[10].ToString(),
                    rate_campaign = dr[11].ToString(),
                    rate_otp = dr[12].ToString(),
                    urlrate = dr[13].ToString(),
                    dltcharge = dr[14].ToString(),
                };
                customer.Add(cust);
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(customer));
        }

        [WebMethod]
        public void GetSMSReport(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetSMSReport(dtr[0], dtr[1], "", "","");
            var smsrpt = new List<smsreport>();
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreport
                {
                    sln = dr[0].ToString(),
                    UserName = dr[1].ToString(),
                    SenderID = dr[2].ToString(),
                    Submitted = dr[3].ToString(),
                    Delivered = dr[4].ToString(),
                    Failed = dr[5].ToString(),
                    Unknown = dr[6].ToString()

                };
                smsrpt.Add(srpt);
            }
            var js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(smsrpt));
        }

        [WebMethod]
        public void GetSMSReportUser(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            //DataTable dt = ob.GetSMSReport(dtr[0], dtr[1], "", "");
            DataTable dt = ob.GetSMSReport_user(dtr[0], dtr[1], "", dtr[2]);
            var smsrpt = new List<smsreportuser>();
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreportuser
                {
                    sln = dr[0].ToString(),
                    msgid = dr[1].ToString(),
                    mobile = dr[2].ToString(),
                    senderid = dr[3].ToString(),
                    msgtext = dr[4].ToString(),
                    senttime = dr[5].ToString(),
                    dlrstat = dr[6].ToString()

                };
                smsrpt.Add(srpt);
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(smsrpt));
        }

        [WebMethod]
        public void GetSMSReportDetailUserNew(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            //DataTable dt = ob.GetSMSReport(dtr[0], dtr[1], "", "");
            //DataTable GetSMSReportDetail_user_new(string userid, string fileid, string sender, string f, string t)
            DataTable dt = ob.GetSMSReportDetail_user_new(dtr[2], dtr[3], dtr[4], dtr[0], dtr[1], dtr[5]);
            var smsrpt = new List<smsreportuser_new>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreportuser_new
                {
                    sln = i.ToString(),
                    msgid = dr["MessageId"].ToString(),
                    mobile = dr["MobileNo"].ToString(),
                    senderid = dr["Sender"].ToString(),
                    senttime = dr["SentDate"].ToString(),
                    dlrtime = dr["DeliveredDate"].ToString(),
                    msgtext = dr["Message"].ToString(),
                    dlrstat = dr["MessageState"].ToString(),
                    dlrresp = dr["RESPONSE"].ToString()

                };
                smsrpt.Add(srpt);
                i++;
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(smsrpt));
        }
        [WebMethod]
        public void GetCRDRLog(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetCRDRLog(dtr[0], dtr[1], dtr[2], dtr[3], dtr[4],dtr[5]);
            var ocrdrlog = new List<crdrlog>();
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new crdrlog
                {
                    sln = dr[0].ToString(),
                    username = dr[1].ToString(),
                    SenderID = dr[2].ToString(),
                    CompName = dr[3].ToString(),
                    Email = dr[4].ToString(),
                    Mobile1 = dr[5].ToString(),
                    bal = dr[6].ToString(),
                    cr = dr[7].ToString(),
                    dr = dr[8].ToString(),
                    dat = dr[9].ToString(),
                    Actions = ""
                };
                ocrdrlog.Add(srpt);
            }
            var js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(ocrdrlog));
        }
        [WebMethod]
        public void GetClickReport(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetClickReort(dtr[0], dtr[1], dtr[2], dtr[3], dtr[4],dtr[5]);
            var oclickcnt = new List<clickcnt>();
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new clickcnt
                {
                    sln = dr[0].ToString(),
                    username = dr[1].ToString(),
                    shorturl = dr[2].ToString(),
                    clickcount = dr[3].ToString(),
                    Actions = ""
                };
                oclickcnt.Add(srpt);
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            Context.Response.Write(js.Serialize(oclickcnt));
        }

        [WebMethod]
        public void GetWAReport(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetWADetail(dtr[0], dtr[1]);
            var smsrpt = new List<smsreportuser_new>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreportuser_new
                {
                    sln = i.ToString(),
                    msgid = dr["MessageId"].ToString(),
                    mobile = dr["MobileNo"].ToString(),
                    //senderid = dr["Sender"].ToString(),
                    senttime = dr["SentDate"].ToString(),
                    dlrtime = dr["DeliveredDate"].ToString(),
                    msgtext = dr["Message"].ToString(),
                    dlrstat = dr["MessageState"].ToString(),
                    dlrresp = dr["RESPONSE"].ToString()

                };
                smsrpt.Add(srpt);
                i++;
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(smsrpt));
        }

        [WebMethod]
        public void GetRCSReport(string dater)
        {
            string[] dtr = dater.Split('$');
            Helper.Util ob = new Helper.Util();
            DataTable dt = ob.GetRCSDetail(dtr[0], dtr[1]);
            var smsrpt = new List<smsreportuser_new>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreportuser_new
                {
                    sln = i.ToString(),
                    msgid = dr["MessageId"].ToString(),
                    mobile = dr["MobileNo"].ToString(),
                    //senderid = dr["Sender"].ToString(),
                    senttime = dr["SentDate"].ToString(),
                    dlrtime = dr["DeliveredDate"].ToString(),
                    msgtext = dr["Message"].ToString(),
                    dlrstat = dr["MessageState"].ToString(),
                    dlrresp = dr["RESPONSE"].ToString()

                };
                smsrpt.Add(srpt);
                i++;
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(smsrpt));
        }

        [WebMethod]
        public void GetRCSReportDetailUserNew(string fid)
        {
            //string[] dtr = dater.Split('$');
            rcscode.Util ob = new rcscode.Util();
            //DataTable dt = ob.GetSMSReport(dtr[0], dtr[1], "", "");
            //DataTable GetSMSReportDetail_user_new(string userid, string fileid, string sender, string f, string t)
            DataTable dt = ob.RcsReportViewDetails(fid);
            var smsrpt = new List<smsreportuser_new>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                var srpt = new smsreportuser_new
                {
                    sln = i.ToString(),
                    msgid = dr["messageId"].ToString(),
                    mobile = dr["MobNo"].ToString(),
                    senttime = dr["SendDate"].ToString(),
                    dlrtime = dr["DeliveryDate"].ToString(),
                    msgtext = dr["MsgText"].ToString(),
                    dlrstat = dr["sentstatus"].ToString(),
                    dlrresp = dr["DeliveryResponse"].ToString(),
                    Seenstatus = dr["seenstatus"].ToString(),
                    SeenDate = dr["seentime"].ToString()

                };
                smsrpt.Add(srpt);
                i++;
            }
            var js = new JavaScriptSerializer();
            js.MaxJsonLength = 2147483647;
            Context.Response.Write(js.Serialize(smsrpt));
        }

        [WebMethod(EnableSession = true)]
        public string GetCTime(string id)
        {
            string sql = "select getdate()";
            string date1 = Convert.ToString(Helper.database.GetScalarValue(sql));
            return date1;
        }
    } 

    public class UserId
    {
        public string user { get; set; }
    }
}
