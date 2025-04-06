using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eMIMPanel.Helper
{
    public class smpp
    {
        public void NotificationCentre(string User, double min, double lastrecharge, bool sms, bool email, bool voice, bool wa)
        {
            string sql = string.Format("INSERT INTO Notification(Username,minbal,lastrechargeper,sms,emailid,voice,whatsapp) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", User, min, lastrecharge, sms, email, voice, wa);
            database.ExecuteNonQuery(sql);
        }
        public void addmember(string User, string name, string mobile, string email)
        {
            string sql = string.Format("INSERT INTO NotificationMember(Username,Name,Mobile,Email) VALUES('{0}','{1}','{2}','{3}')", User, name, mobile, email);
            database.ExecuteNonQuery(sql);
        }
        public void updatenotification(string User)
        {
            string qry = "IF EXISTS(select userName from Notification where userName='" + User + "')" +
                       " delete from Notification where userName='" + User + "';" +
                       " delete from Notificationmember where userName = '" + User + "'";
            database.ExecuteNonQuery(qry);
        }
        public DataTable DataTable(string sql)
        {

            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
    }
}