using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace smsSummary.Helper
{
    public class Util
    {
        public DataTable GetTempIdAndName(string usr)
        {
            DataTable dt = new DataTable("dt");
            string sql = "SELECT templateid,templateid+'('+tempname+')' AS tempname FROM templaterequest WHERE ISNULL(templateid,'')<>''";
            if (usr != "")
            {
                sql = sql + " AND username='" + usr + "'";
            }
            sql = sql + " ORDER BY templateid";

            dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetTemplateWiseSMSSummary(string user, string f, string t, string TempIdAndName = "", string Action = "")
        {
            string sql = "";
            string dlt = "";
            if (Action == "1")
            {
                sql = @"SELECT Max(s.MSGTEXT) MSGTEXT, convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
FROM customer c with(nolock) inner join MSGSUBMITTED s with(nolock)  ON c.username = s.profileid
LEFT JOIN delivery d with(nolock) ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106)";
            }
            else if (Action == "2")
            {
                sql = @"SELECT Max(s.MSGTEXT) MSGTEXT, convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
FROM customer c with(nolock)  inner join MSGSUBMITTED s with(nolock) ON c.username = s.profileid
LEFT JOIN delivery d with(nolock) ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid UNION ALL ";
                sql = sql + @"SELECT Max(s.MSGTEXT) MSGTEXT,convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
FROM customer c with(nolock) inner join MSGSUBMITTEDLOG s with(nolock) ON c.username = s.profileid
LEFT JOIN DELIVERYLOG d with(nolock) ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106) desc";
            }
            else if (Action == "3")
            {
                sql = @"SELECT Max(s.MSGTEXT) MSGTEXT,convert(varchar,s.sentdatetime,106) as smsDate1,convert(varchar,s.sentdatetime,106) SMSDATE,s.profileid AS userid,s.templateid AS senderid,COUNT(s.id) submitted,
SUM(case when ISNULL(d.dlvrstatus, '') = 'Delivered' then 1 else 0 end) as delivered,
SUM(case when ISNULL(d.dlvrstatus, '') <> 'Delivered' AND d.dlvrstatus IS NOT NULL then 1 else 0 end) as failed,
SUM(case when d.dlvrstatus IS NULL then 1 else 0 end) as unknown 
FROM customer c with(nolock) inner join MSGSUBMITTEDLOG s with(nolock) ON c.username = s.profileid
LEFT JOIN DELIVERYLOG d with(nolock) ON s.msgid = d.msgid WHERE s.sentdatetime between '" + f + "' and '" + t + @"' ";

                if (user != "")
                {
                    sql = sql + " and s.profileid = '" + user + "' ";
                }
                if (TempIdAndName != "")
                {
                    sql = sql + " and s.templateid = '" + TempIdAndName + "' ";
                }
                sql = sql + "GROUP BY convert(varchar,s.sentdatetime,106),s.profileid,s.templateid ORDER BY convert(varchar,s.sentdatetime,106) desc";
            }
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
    }
}