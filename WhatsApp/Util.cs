using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Inetlab.SMPP.PDU;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mail;
using System.Data.SqlClient;

namespace WhatsApp
{
    public class Util
    {
        //public string sql = "";
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public int LogErr = 1;
        public string Ldays = "15";
        public static DataTable dtsettings;
        public static DataTable dtWAChatProcess;
        public static DataTable templateparam;

        public void _Log(string msg)
        {
            try
            {
              
                FileStream filestrm = new FileStream(fn + @"_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
               
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn  + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex4) { }
            }
        }

        public void InfoTest(string msg) {
        }

        public void InfoTest2(string msg)
        {
            
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"test\" + @"_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"test\" + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex4) { }
            }
        }
        public void Info(string msg) { }
        public void InfoRight(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"conn\" + @"_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"conn\" + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex4) { }
            }

        } //{ LogError(msg); }
        public void Info2(string msg) { LogError(msg); }
        public void InfoEr2(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Timr_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_Timr_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex4) { }
            }
        }

        public void Info_Client(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { throw ex2; }
            }
        }
        public void Info_Submit(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogSubmit_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogSubmit_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        }
        public void Info_Failed(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogFailed_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogFailed_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { }
            }
        }
        public void Info_Err(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }
        public void Info_Err2(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_2_Err_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_Log_2_Err_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }
        public void Info_Err3(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_"  + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"catch_LogErr_"  + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }
        public void Info_Client18(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"18_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"18_catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { throw ex2; }
            }
        }
        public void Info_Client25(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"25_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"25_catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                { throw ex2; }
            }
        }

        public void LogError(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Conn_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                FileStream filestrm = new FileStream(fn + @"catch_Conn_Log" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
        }

        public void LogDlvError(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"Dlv_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    FileStream filestrm = new FileStream(fn + @"Dlv_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2) { }
            }
        }

        public DataTable GetSMPPAccounts()
        {
            string sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE,S.SMSExpiryMinute, row_number() over (order by  n.sessionid) as rownum " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid " +
                " where 1=1 " +
                " and (s.BINDINGMODE='Transceiver' or s.BINDINGMODE='Transmiter') " +
                " and s.active=1 and n.active=1 " +
                //" and n.sessionid='205' " +
                " order by smppaccountid" ;
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMPPAccounts4TX()
        {
            string sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.active=1 and n.active=1 AND N.MODE='TX' order by smppaccountid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetAllSMPPAccounts()
        {
            string sql = "Select row_number() over (order by s.smppaccountid) as slno, n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where 1=1 order by s.smppaccountid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }
        public DataTable GetSMPPAccountsTEST()
        {
            string sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE " +
                " from smppsetting s inner join smppsession n on s.smppaccountid=n.smppaccountid where s.smppaccountid='9'  order by smppaccountid";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSRecordsTEST(int acId, int no_of_sms )
        {
            InfoTest("sms data req from - " + acId);
            string sender = @"declare @sender varchar(6) 
                            select top 1 @sender = SENDERID from MSGTRAN_tmp where SMPPACCOUNTID = '" + acId + "' AND picked_datetime IS NULL  group by SENDERID ORDER BY MIN(CREATEDAT) ";
            string s = " select top " + no_of_sms.ToString() + " * from MSGTRAN_tmp WITH (TABLOCK, HOLDLOCK) where SMPPACCOUNTID = '" + acId + "' AND senderid = @sender and picked_datetime IS NULL ORDER BY CREATEDAT";
            string sql = @"
declare @cnt numeric(7)
select @cnt=count(*) from MSGTRAN_tmp WHERE SMPPACCOUNTID = '" + acId + @"' AND PICKED_DATETIME IS NOT NULL
IF @cnt >0 
    set @cnt=0
else
begin
select @cnt=count(*) from MSGTRAN_tmp WHERE SMPPACCOUNTID = '" + acId + @"' and PICKED_DATETIME IS NULL
IF @cnt >0 
BEGIN
    
                    BEGIN TRY
                        BEGIN TRANSACTION; " +
                            sender + s + "; " + " ;WITH CTE AS  (" + s + ") UPDATE CTE SET picked_datetime=getdate() " +
                        @" COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                    END CATCH 
END
end";
            DataTable dt = database.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSMSRecords(int acId, int no_of_sms, string picktime)
        {
            DataTable dt = new DataTable();
            try
            {
                InfoTest("sms data req start from - " + acId);
                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NOT NULL"));
                if (c == 0)
                {
                    c = Convert.ToInt64(database.GetScalarValue("select count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_sms.ToString() + @" * from MSGTRAN
                                     where SMPPACCOUNTID = '" + acId + @"' and picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from MSGTRAN where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                        dt = database.GetDataTable(sql);
                        InfoTest("sms data req start end - " + acId + " dtcount=" + dt.Rows.Count);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err("fetchsms- " + ex.Message, acId);
                return dt;
            }

            #region < Commented >
            //DataTable dt = new DataTable();
            //try
            //{
            //    InfoTest("sms data req start from - " + acId);
            //    Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NOT NULL"));
            //    if (c == 0)
            //    {
            //        c = Convert.ToInt64(database.GetScalarValue("select count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NULL"));
            //        if (c > 0)
            //        {
            //            string sql = @"
            //        declare @cnt numeric(10)
            //        select @cnt = stopFileUpload from SettingMast   
            //        if @cnt = 0 or '" + acId + @"'='301'
            //        begin
            //            BEGIN TRY
            //                BEGIN TRANSACTION; 
            //                    declare @sender varchar(11) 
            //                    select top 1 @sender = SENDERID from MSGTRAN where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime IS NULL  
            //                    group by SENDERID ORDER BY MIN(CREATEDAT) ;
            //                    WITH CTE AS  
            //                    (select top " + no_of_sms.ToString() + @" * from MSGTRAN
            //                    where SMPPACCOUNTID = '" + acId + @"' and picked_datetime IS NULL AND senderid = @sender ORDER BY CREATEDAT 
            //                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
            //                    select * from MSGTRAN where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime ='" + picktime + @"' and senderid = @sender ORDER BY CREATEDAT
            //                COMMIT TRANSACTION;
            //            END TRY
            //            BEGIN CATCH
            //                ROLLBACK TRANSACTION;
            //            END CATCH 
            //        end";
            //            dt = database.GetDataTable(sql);
            //            InfoTest("sms data req start end - " + acId + " dtcount=" + dt.Rows.Count);
            //        }
            //    }
            //    return dt;
            //}
            //catch (Exception ex)
            //{
            //    Info_Err("fetchsms- " + ex.Message, acId);
            //    return dt;
            //}
            #endregion
            #region <Commented >
            /*
            DataTable dt = new DataTable();
            try
            {
                InfoTest("sms data req start from - " + acId);

                string sql = @"
                    declare @cnt numeric(10)
                    select @cnt=count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + @"' AND PICKED_DATETIME IS NOT NULL
                    IF @cnt >0 
                        set @cnt=0
                    else
                    begin
                        select @cnt=count(*) from MSGTRAN WHERE SMPPACCOUNTID = '" + acId + @"' and PICKED_DATETIME IS NULL
                        IF @cnt >0 
                        BEGIN
                            select @cnt = stopFileUpload from SettingMast   
                            if @cnt = 0 or '" + acId + @"'='301'
                            begin
                                BEGIN TRY
                                    BEGIN TRANSACTION; 
                                        declare @sender varchar(6) 
                                        select top 1 @sender = SENDERID from MSGTRAN where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime IS NULL  
                                        group by SENDERID ORDER BY MIN(CREATEDAT) ;
                                        WITH CTE AS  
                                        (select top " + no_of_sms.ToString() + @" * from MSGTRAN
                                        where SMPPACCOUNTID = '" + acId + @"' and picked_datetime IS NULL AND senderid = @sender ORDER BY CREATEDAT 
                                        ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                        select * from MSGTRAN where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime ='" + picktime + @"' and senderid = @sender ORDER BY CREATEDAT
                                    COMMIT TRANSACTION;
                                END TRY
                                BEGIN CATCH
                                    ROLLBACK TRANSACTION;
                                END CATCH 
                            end
                        END
                    END";
                dt = database.GetDataTable(sql);
                InfoTest("sms data req start end - " + acId + " dtcount=" + dt.Rows.Count);
            
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err("fetchsms- " + ex.Message, acId);
                return dt;
            }
            */
            #endregion
        }

        public DataTable GetAllSMSRecords(string picktime)
        {
            InfoTest("all sms data req start ");

            string sql = @"
declare @cnt numeric(7)
select @cnt=count(*) from MSGTRAN 
IF @cnt <=0 
    set @cnt=0
else
BEGIN
    select @cnt = stopFileUpload from SettingMast   
    if @cnt = 0 
    BEGIN
        BEGIN TRY
            UPDATE MSGTRAN SET PICKED_DATETIME='" + picktime + @"'
            SELECT * FROM MSGTRAN WHERE PICKED_DATETIME ='" + picktime + @"'             
        END TRY
        BEGIN CATCH
            UPDATE MSGTRAN SET PICKED_DATETIME=NULL
        END CATCH 
    END
END";
            DataTable dt = database.GetDataTable(sql);
            InfoTest("all sms data req end - dtcount=" + dt.Rows.Count);
            return dt;
        }

        public void UpdateMsgTran(DataTable dtSMS)
        {
            for (int y = 0; y < dtSMS.Rows.Count; y++)
            {
                string sql = "update msgtran set picked_datetime =null where id=" + dtSMS.Rows[y]["id"].ToString();
                database.ExecuteNonQuery(sql);
            }
        }

        public int getFileUploadStop()
        {
            string sql = "select stopFileUpload from SettingMast";
            return Convert.ToInt16(database.GetScalarValue(sql));
        }

        public void AddInMsgStopped(DataRow dr)
        {
            //try
            //{
            string sql = "Insert into msgsubmitted_STOP (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,PEID,TEMPLATEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString().Replace("'", "''") + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',NULL,'','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','" + (dr["PEID"] == DBNull.Value ? "" : dr["PEID"].ToString()) + "','" + (dr["TEMPLATEID"] == DBNull.Value ? "" : dr["TEMPLATEID"].ToString()) + "')";
                database.ExecuteNonQuery(sql);
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        string st = "";
            //        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
            //        else st = ex.StackTrace;
            //        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,ERRMSG,FILEID) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //    '" + dr["PROVIDER"].ToString() + @"',
            //    '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //    '" + dr["PROFILEID"].ToString() + @"',
            //    '" + dr["MSGTEXT"].ToString() + @"',
            //    '" + dr["TOMOBILE"].ToString() + @"',
            //    '" + dr["SENDERID"].ToString() + @"',
            //    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',NULL,'STOPPED','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2) { }
            //}
        }
        public void AddInMsgTEST(DataRow dr)
        {
            //try
            //{
            string sql = "Insert into msgsubmitted_TEST (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',NULL,'','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
                database.ExecuteNonQuery(sql);
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        string st = "";
            //        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
            //        else st = ex.StackTrace;
            //        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,ERRMSG,FILEID) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //    '" + dr["PROVIDER"].ToString() + @"',
            //    '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //    '" + dr["PROFILEID"].ToString() + @"',
            //    '" + dr["MSGTEXT"].ToString() + @"',
            //    '" + dr["TOMOBILE"].ToString() + @"',
            //    '" + dr["SENDERID"].ToString() + @"',
            //    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',NULL,'STOPPED','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2) { }
            //}
        }

        public void AddInMsgSubmitted(string msgid, DataRow dr, DateTime sendTime)
        {
            //try
            //{
            string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
                database.ExecuteNonQuery(sql);
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        string st = "";
            //        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
            //        else st = ex.StackTrace;
            //        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,ERRMSG,FILEID) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //    '" + dr["PROVIDER"].ToString() + @"',
            //    '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //    '" + dr["PROFILEID"].ToString() + @"',
            //    '" + dr["MSGTEXT"].ToString() + @"',
            //    '" + dr["TOMOBILE"].ToString() + @"',
            //    '" + dr["SENDERID"].ToString() + @"',
            //    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
            //    '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "')";
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2) { }
            //}
        }
       
        //
        public void AddInMsgSubmittedInvalidSenderNew(SubmitSmResp resp, DataTable dt, DateTime sendTime)
        {
            //try
            //{
            string msgid = Guid.NewGuid().ToString();
            string mobn = resp.Request.DestinationAddress.Address;
            string msgt = resp.Request.MessageText.ToString();
            DataRow[] dr = dt.Select("ToMobile = '" + mobn + "'");
            if (dr.Length > 0)
            {
                string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,FILEID,PEID,TEMPLATEID,smstext,datacode,msgid2) " +
                    "values ('" + dr[0]["id"].ToString() + @"',
                '" + dr[0]["PROVIDER"].ToString() + @"',
                '" + dr[0]["SMPPACCOUNTID"].ToString() + @"',
                '" + dr[0]["PROFILEID"].ToString() + @"',
                N'" + msgt.Replace("'", "''") + @"',
                '" + mobn + @"',
                '" + dr[0]["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr[0]["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + (dr[0]["MSGID2"]== DBNull.Value ? msgid : Convert.ToString(dr[0]["MSGID2"])) + "','ESME_RINVSRCADR - INVALID SENDERID','" + (dr[0]["FILEID"] == DBNull.Value ? "0" : dr[0]["FILEID"].ToString()) + "','" + (dr[0]["PEID"] == DBNull.Value ? "" : dr[0]["PEID"].ToString()) + "','" + (dr[0]["TEMPLATEID"] == DBNull.Value ? "" : dr[0]["TEMPLATEID"].ToString()) + "',N'" + (dr[0]["MSGTEXT"] == DBNull.Value ? "" : dr[0]["MSGTEXT"].ToString().Replace("'", "''")) + "','" + dr[0]["DATACODE"].ToString() + @"','" + (dr[0]["MSGID2"] == DBNull.Value ? "" :  msgid ) + "') ; ";
                sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + (dr[0]["MSGID2"] == DBNull.Value ? msgid : Convert.ToString(dr[0]["MSGID2"])) + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";
                database.ExecuteNonQuery(sql);
            }
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        string st = "";
            //        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
            //        else st = ex.StackTrace;
            //        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,ERRMSG,FILEID) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //    '" + dr["PROVIDER"].ToString() + @"',
            //    '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //    '" + dr["PROFILEID"].ToString() + @"',
            //    '" + dr["MSGTEXT"].ToString() + @"',
            //    '" + dr["TOMOBILE"].ToString() + @"',
            //    '" + dr["SENDERID"].ToString() + @"',
            //    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
            //    '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','ESME_RINVSRCADR - INVALID SENDERID','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";

            //        sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + msgid + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";
            //        database.ExecuteNonQuery(sql);
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2) { }
            //}
        }

        public void AddInMsgSubmittedInvalidSender(string msgid, DataRow dr, DateTime sendTime)
        {
            //try
            //{
            string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString() + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','ESME_RINVSRCADR - INVALID SENDERID','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";
                sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + msgid + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";
                database.ExecuteNonQuery(sql);

            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        string st = "";
            //        if (ex.StackTrace.ToString().Length > 1500) st = ex.StackTrace.ToString().Substring(0, 1500);
            //        else st = ex.StackTrace;
            //        sql = "Insert into msgsubmitted_ERR (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,ERRMSG,FILEID) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //    '" + dr["PROVIDER"].ToString() + @"',
            //    '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //    '" + dr["PROFILEID"].ToString() + @"',
            //    '" + dr["MSGTEXT"].ToString() + @"',
            //    '" + dr["TOMOBILE"].ToString() + @"',
            //    '" + dr["SENDERID"].ToString() + @"',
            //    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
            //    '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" + msgid + "','ESME_RINVSRCADR - INVALID SENDERID','" + ex.Message + " - " + st + "','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";

            //        sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS) values ('ESME_RINVSRCADR - INVALID SENDERID','" + msgid + "','" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','FAILED') ";
            //        database.ExecuteNonQuery(sql);
            //        database.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex2) { }
            //}
        }

        public void RemoveFromMsgTran(string SMPPAccountID)
        {
            string sql = "Delete from msgtran where SMPPACCOUNTID='" + SMPPAccountID + "' and picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }
        public void RemoveAllFromMsgTran(string picktime)
        {
            string sql = "INSERT INTO MSGTRANPROCESSED (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,PICKED_DATETIME,FILEID) " +
                "SELECT PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,PICKED_DATETIME,FILEID FROM msgtran where picked_datetime ='" + picktime +"' ; " +
                "Delete from msgtran where picked_datetime ='" + picktime + "'  ";
            database.ExecuteNonQuery(sql);
        }
        public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt, string errcd)
        {
            string d = "";
            try
            {
                if (txt.Contains(" done date:"))
                {
                    int p = txt.IndexOf(" done date:") + 11;
                    d = txt.Substring(p, 12);
                    //if (d.Substring(10, 2).ToUpper() == " S") d.ToUpper().Replace(" S", "00");
                    if (d.Substring(10, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    if (d.Substring(d.Length - 2, 2).ToUpper() == " S") d = d.ToUpper().Replace(" S", "00").Replace(" s", "00");
                    d = "20" + d.Substring(0, 2) + "-" + d.Substring(2, 2) + "-" + d.Substring(4, 2) + " " + d.Substring(6, 2) + ":" + d.Substring(8, 2) + ":" + d.Substring(10, 2);
                }
            }
            catch (Exception ex)
            {
                d = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            }

            try
            {
                string sql;
                string msgid2 = "";
                //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                msgid2 = Convert.ToString(database.GetScalarValue("select top 1 msgid from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                if (msgid2 != "")
                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) values ('" + txt.Replace("'", "''").Replace(msgId,msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                else
                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code) values ('" + txt.Replace("'","''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex2)
            {
                try
                {
                    LogDlvError("Dlv Err - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    string sql;
                    string msgid2 = "";
                    //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK)  where msgid2='" + msgId + "'"));
                    msgid2 = Convert.ToString(database.GetScalarValue("select top 1 msgid from msgsubmitted WITH (NOLOCK)  where msgid2='" + msgId + "'"));
                    if (msgid2 != "")
                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code) values ('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                    else
                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code) values ('" + txt.Replace("'", "''") + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "')";
                    try
                    {
                        database.ExecuteNonQuery(sql);
                    }
                    catch(Exception e4)
                    { }
                }
                catch (Exception ex6) {
                    //LogDlvError("Dlv Err 6 - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                }
            }
        }

        public int GetDuplicateCount(string mob, string txt, string fileid, string profileid)
        {
            if(fileid=="1") return 0;
            if (profileid.ToUpper().Trim() == "MIM2101371")
            {
                return 0;
            }
            else
            {
                if (fileid == "") fileid = "0";
                string sql = @"select count(*) from MSGSUBMITTED with (nolock) where TOMOBILE = '" + mob + @"' ";
                sql = sql + "and convert(varchar,INSERTDATE,102)=convert(varchar,getdate(),102) ";
                sql = sql + "and fileid = '" + fileid + "' and dbo.AlphaNumericOnly(MSGTEXT)= dbo.AlphaNumericOnly('" + txt.Replace("'", "") + "')";
                int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
                return mcnt;
            }
        }

        public Int64 Check2SendMSG(DataRow dr)
        {
            return 0;

            if (dr["SMPPACCOUNTID"].ToString().Substring(0, 2) == "11") return 0;
            string sql = "";
            //if (dr["PROFILEID"] == "MIM2002107")
            //{
            sql = @"SELECT COUNT(*) FROM DlvErrDATA D INNER JOIN errorcode C ON D.err_code=C.code WHERE C.TOCHECK=1 AND  
            D.SENTDATETIME > DATEADD(DAY,-1 * C.days, GETDATE()) and D.TOMOBILE = '" + dr["TOMOBILE"].ToString() + "' ";
            //}
            //else
            //{
            //     sql = @"SELECT COUNT(*) FROM DlvErrDATA1 D INNER JOIN errorcode C ON D.err_code=C.code WHERE C.TOCHECK=1 AND  
            //D.SENTDATETIME > DATEADD(DAY,-1 * C.days, GETDATE()) and D.TOMOBILE = '" + dr["TOMOBILE"].ToString() + "' and D.SENDERID='" + dr["SENDERID"].ToString() + "'";
            //}
            Int64 mcnt = Convert.ToInt16(database.GetScalarValue(sql));

            if (mcnt > 0)
            {
                int noofsms = GetNoOfSMS(dr["MSGTEXT"].ToString().Length, dr["DataCode"].ToString());
                bool ucs2 = (dr["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                for (int x = 0; x < noofsms; x++)
                {
                    string smsTex = GetSMSText(dr["MSGTEXT"].ToString(), x + 1, noofsms, ucs2);

                    sql = @"declare @id varchar(100)  select @id = newid()
                    Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND,smstext) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                N'" + smsTex.Replace("'", "''") + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
                '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1',N'" + dr["MSGTEXT"].ToString().Replace("'", "''") + @"') ; " +
                "Insert into NOTSUBMITTED (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                N'" + smsTex.Replace("'", "''") + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
                '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1') ; ";
                    database.ExecuteNonQuery(sql);
                }
            }

            if (dr["SMPPACCOUNTID"].ToString().Contains("90") && mcnt == 0)
            {
                sql = @"SELECT COUNT(*) FROM DlvErrDATA D INNER JOIN errorcode C ON D.err_code=C.code WHERE C.TOCHECKPROMO=1 AND  
                D.SENTDATETIME > DATEADD(DAY,-1 * C.days, GETDATE()) and D.TOMOBILE = '" + dr["TOMOBILE"].ToString() + "' ";

                mcnt = Convert.ToInt16(database.GetScalarValue(sql));

                if (mcnt > 0)
                {
                    int noofsms = GetNoOfSMS(dr["MSGTEXT"].ToString().Length, dr["DataCode"].ToString());
                    bool ucs2 = (dr["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
                    for (int x = 0; x < noofsms; x++)
                    {
                        string smsTex = GetSMSText(dr["MSGTEXT"].ToString(), x + 1, noofsms, ucs2);

                        sql = @"declare @id varchar(100)  select @id = newid()
                    Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND,smstext) " +
                    "values ('" + dr["id"].ToString() + @"',
                    '" + dr["PROVIDER"].ToString() + @"',
                    '" + dr["SMPPACCOUNTID"].ToString() + @"',
                    '" + dr["PROFILEID"].ToString() + @"',
                    N'" + smsTex.Replace("'", "''") + @"',
                    '" + dr["TOMOBILE"].ToString() + @"',
                    '" + dr["SENDERID"].ToString() + @"',
                    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
                    '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1',N'" + dr["MSGTEXT"].ToString().Replace("'", "''") + @"') ; " +
                    "Insert into NOTSUBMITTED (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND) " +
                        "values ('" + dr["id"].ToString() + @"',
                    '" + dr["PROVIDER"].ToString() + @"',
                    '" + dr["SMPPACCOUNTID"].ToString() + @"',
                    '" + dr["PROFILEID"].ToString() + @"',
                    N'" + smsTex.Replace("'", "''") + @"',
                    '" + dr["TOMOBILE"].ToString() + @"',
                    '" + dr["SENDERID"].ToString() + @"',
                    '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
                    '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1') ; ";
                        database.ExecuteNonQuery(sql);
                    }
                }
            }
            return mcnt;
        }
        public string GetSMSText(string msg, int x, int noofsms, bool ucs2)
        {
            string ret = "";
            msg = msg.Replace("''", "'");
            if (ucs2)
            {
                if (noofsms == 1) ret = msg;
                if (x == 1) { if (msg.Length > 70) ret = msg.Substring(0, 70); else ret = msg.Substring(0); }
                if (x == 2) { if (msg.Length > 134) ret = msg.Substring(70, 64); else ret = msg.Substring(70); }
                if (x == 3) { if (msg.Length > 201) ret = msg.Substring(134, 67); else ret = msg.Substring(134); }
                if (x == 4) { if (msg.Length > 268) ret = msg.Substring(201, 67); else ret = msg.Substring(201); }
                if (x == 5) { if (msg.Length > 335) ret = msg.Substring(268, 67); else ret = msg.Substring(268); }
                if (x == 6) { if (msg.Length > 402) ret = msg.Substring(335, 67); else ret = msg.Substring(335); }
                if (x == 7) { if (msg.Length > 469) ret = msg.Substring(402, 67); else ret = msg.Substring(402); }
                if (x == 8) { if (msg.Length > 536) ret = msg.Substring(469, 67); else ret = msg.Substring(469); }
                if (x == 9) { if (msg.Length > 603) ret = msg.Substring(536, 67); else ret = msg.Substring(536); }
                if (x == 10) { if (msg.Length > 670) ret = msg.Substring(603, 67); else ret = msg.Substring(603); }
            }
            else
            {
                if (noofsms == 1) ret = msg;
                if (x == 1) { if (msg.Length > 160) ret = msg.Substring(0, 160); else ret = msg.Substring(0); }
                if (x == 2) { if (msg.Length > 306) ret = msg.Substring(160, 146); else ret = msg.Substring(160); }
                if (x == 3) { if (msg.Length > 459) ret = msg.Substring(306, 153); else ret = msg.Substring(306); }
                if (x == 4) { if (msg.Length > 612) ret = msg.Substring(459, 153); else ret = msg.Substring(459); }
                if (x == 5) { if (msg.Length > 765) ret = msg.Substring(612, 153); else ret = msg.Substring(612); }
                if (x == 6) { if (msg.Length > 918) ret = msg.Substring(765, 153); else ret = msg.Substring(765); }
                if (x == 7) { if (msg.Length > 1071) ret = msg.Substring(918, 153); else ret = msg.Substring(918); }
                if (x == 8) { if (msg.Length > 1224) ret = msg.Substring(1071, 153); else ret = msg.Substring(1071); }
                if (x == 9) { if (msg.Length > 1377) ret = msg.Substring(1224, 153); else ret = msg.Substring(1224); }
                if (x == 10) { if (msg.Length > 1530) ret = msg.Substring(1377, 153); else ret = msg.Substring(1377); }
            }
            return ret.Replace("'", "''");
        }
        public int GetNoOfSMS(int qlen, string datacode)
        {
            int noofsms = 0;
            if (datacode.ToString().ToUpper() == "DEFAULT")
            {
                if (qlen >= 1) noofsms = 1;
                if (qlen > 160) noofsms = 2;
                if (qlen > 306) noofsms = 3;
                if (qlen > 459) noofsms = 4;
                if (qlen > 612) noofsms = 5;
                if (qlen > 765) noofsms = 6;
                if (qlen > 918) noofsms = 7;
                if (qlen > 1071) noofsms = 8;
                if (qlen > 1224) noofsms = 9;
                if (qlen > 1377) noofsms = 10;
                if (qlen > 1530) noofsms = 11;
                if (qlen > 1683) noofsms = 12;
            }
            else
            {
                if (qlen >= 1) noofsms = 1;
                if (qlen > 70) noofsms = 2;
                if (qlen > 134) noofsms = 3;
                if (qlen > 201) noofsms = 4;
                if (qlen > 268) noofsms = 5;
                if (qlen > 335) noofsms = 6;
                if (qlen > 402) noofsms = 7;
                if (qlen > 469) noofsms = 8;
                if (qlen > 536) noofsms = 9;
                if (qlen > 603) noofsms = 10;
            }
            return noofsms;
        }

        public byte[] getTMID(string provider)
        {
            if (provider == "AIRTEL") return clsCheck.tmid;
            else if (provider == "AIRTEL_T") return clsCheck.tmid_T;
            else return System.Text.Encoding.UTF8.GetBytes("");
        }

        public byte[] getPEID(string provider, string peid)
        {
            string sp = peid;
            try
            {
                if (provider.Contains("AIRTEL"))
                {
                    if (peid.Length > 0) sp = peid.Substring(0, 19);
                }
                return System.Text.Encoding.UTF8.GetBytes(sp);
            }
            catch (Exception ex)
            {
                return System.Text.Encoding.UTF8.GetBytes(sp);
            }
        }

        public byte[] getTEMPLATEID(string provider, string tid)
        {
            string st = tid;
            try
            {
                if (provider.Contains("AIRTEL"))
                {
                    if (tid.Length > 0) st = tid.Substring(0, 19);
                }
                return System.Text.Encoding.UTF8.GetBytes(st);
            }
            catch (Exception ex)
            {
                return System.Text.Encoding.UTF8.GetBytes(st);
            }
        }

        //

        public DataTable GetOBDRecords(int no_of_record, string picktime)
        {
            DataTable dt = new DataTable();
            try
            {
               //LogErr("OBD data req start from - " + picktime);

                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from OBDTRAN WHERE PICKED_DATETIME IS NOT NULL"));
                if (c == 0)
                {
                    c = Convert.ToInt64(database.GetScalarValue("select count(*) from OBDTRAN WHERE PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_record.ToString() + @" * from OBDTRAN
                                     where picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from OBDTRAN where picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                        dt = database.GetDataTable(sql);
                        //InfoTest2("sms data req start end - dtcount=" + dt.Rows.Count);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err3("fetchobd- " + ex.Message);
                return dt;
            }



        }

        public DataTable GetWARecords(int no_of_record, string picktime)
        {
            DataTable dt = new DataTable();
            try
            {
                //InfoTest2("WA data req start from - " + picktime);

                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from WAMSGTRAN WHERE PICKED_DATETIME IS NOT NULL"));
                if (c == 0)
                {
                    c = Convert.ToInt64(database.GetScalarValue("select count(*) from WAMSGTRAN WHERE PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_record.ToString() + @" * from WAMSGTRAN
                                     where picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from WAMSGTRAN where picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                        dt = database.GetDataTable(sql);
                        //InfoTest2("sms data req start end - dtcount=" + dt.Rows.Count);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err3("fetchobd- " + ex.Message);
                return dt;
            }



        }

        public DataTable GetRCSRecords(int no_of_record, string picktime)
        {
            DataTable dt = new DataTable();
            try
            {
                //InfoTest2("WA data req start from - " + picktime);

                //Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from RCSMSGTRAN WHERE PICKED_DATETIME IS NOT NULL"));
                //if (c == 0)
                //{
                Int64  c = Convert.ToInt64(database.GetScalarValue("select count(*) from RCSMSGTRAN WHERE PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_record.ToString() + @" * from RCSMSGTRAN
                                     where picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from RCSMSGTRAN where picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                        dt = database.GetDataTable(sql);
                        //InfoTest2("sms data req start end - dtcount=" + dt.Rows.Count);
                    }
                //}
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err3("fetchobd- " + ex.Message);
                return dt;
            }



        }

        public DataTable GetEmailRecords(int no_of_record, string picktime)
        {
            DataTable dt = new DataTable();
            try
            {
                //InfoTest2("WA data req start from - " + picktime);

                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from emailtran WHERE PICKED_DATETIME IS NOT NULL"));
                if (c == 0)
                {
                    c = Convert.ToInt64(database.GetScalarValue("select count(*) from emailtran WHERE PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_record.ToString() + @" * from emailtran
                                     where picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from emailtran where picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
                            END TRY
                            BEGIN CATCH
                                set @cnt=0
                            END CATCH 
                        end";
                        dt = database.GetDataTable(sql);
                        //InfoTest2("sms data req start end - dtcount=" + dt.Rows.Count);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Info_Err3("fetchobd- " + ex.Message);
                return dt;
            }



        }

        public class obdresp
        {
            public int order_id { get; set; }
            public string result { get; set; }
            public int status_code { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class OBDStatus
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class OBDMessage
        {
            public string to { get; set; }
            public OBDStatus status { get; set; }
            public string messageId { get; set; }
        }

        public class obdresponse
        {
            public string bulkId { get; set; }
            public List<OBDMessage> messages { get; set; }
        }

        public void obd_knowlarityapi(string mob, string obdtext, string apikey, string authkey, string profileid, int fileid, int i,string body)
        {
            System.Threading.Thread.Sleep(50);
            var client = new RestClient("https://kpi.knowlarity.com/Basic/v1/account/call/campaign");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-api-key", apikey);
            request.AddHeader("Authorization", authkey);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            body = body.Replace("###STARTTIME###", DateTime.Now.AddMinutes(2).ToString("yyy-MM-dd HH:mm"));
            body = body.Replace("###MOBILENUMBER###", mob);
            body = body.Replace("###MESSAGE###", obdtext);


            //            var body = @"{
            //        " + "\n" +
            //            @"""ivr_id"": ""1000065122"",
            //        " + "\n" +
            //            @"""timezone"": ""Asia/Kolkata"",
            //        " + "\n" +
            //            @"""priority"": ""1"",
            //        " + "\n" +
            //            @"""order_throttling"": ""10"",
            //        " + "\n" +
            //            @"""retry_duration"": ""15"",
            //        " + "\n" +
            //            @"""start_time"": """ + DateTime.Now.AddMinutes(2).ToString("yyy-MM-dd HH:mm") + @""",
            //        " + "\n" +
            //            @"""max_retry"": ""2"",
            //        " + "\n" +
            //            @"""call_scheduling"": ""[1, 1, 1, 1, 1, 0, 0]"",
            //        " + "\n" +
            //@"""call_scheduling_start_time"": ""09:00"",
            //        " + "\n" +
            //@"""call_scheduling_stop_time"": ""21:00"",
            //        " + "\n" +
            //         @"""k_number"": ""+918047275779"",
            //        " + "\n" +
            //            @"""additional_number"": """ + mob + @"," + obdtext + @""",
            //        " + "\n" +
            //            @"""is_transactional"": ""True""
            //        " + "\n" +
            //            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            obdresp res = JsonConvert.DeserializeObject<obdresp>(response.Content);

            try
            {
                string sql = @"
                        insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                        select '" + res.order_id + "','" + mob + "',N'" + obdtext + "','" + res.result + "','" + res.status_code + "','" + profileid + "','" + fileid + "' ," + i + "";
              
                //    string sql = @"
                //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void obd_infobip_api(string mob, string obdtext, string apikey, string authkey, string profileid, int fileid, int i, string body)
        {

            System.Threading.Thread.Sleep(50);
            var client = new RestClient("https://vj1kpv.api.infobip.com/tts/3/single");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);
            request.AddHeader("Authorization", authkey);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            //body = body.Replace("###STARTTIME###", DateTime.Now.AddMinutes(2).ToString("yyy-MM-dd HH:mm"));
            body = body.Replace("###MOBILENUMBER###", mob);
            body = body.Replace("###MESSAGE###", obdtext);
            //var body = @"{
            //            ""text"": """+obdtext+@""",
            //            ""language"": ""en-in"",
            //            ""voice"": {
            //                ""name"": ""Raveena"",
            //                ""gender"": ""female""
            //            },
            //            ""from"": ""912271897425"",
            //            ""to"": """+ mob + @"""
            //            }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            obdresponse res = JsonConvert.DeserializeObject<obdresponse>(response.Content);

            try
            {

                OBDMessage ms = new OBDMessage();
                ms = res.messages[0];

                OBDStatus st = new OBDStatus();
                st = ms.status;

                string sql = @"
                insert into [obdapisubmitted](bulkId,messageId,tomobile,obdtext,groupId,groupName,Id,name,description,profileid,fileid,cnt)
                select '" + res.bulkId + "','" + ms.messageId + "','" + mob + "',N'" + obdtext + "'," + st.groupId + ",'" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "','" + profileid + "','" + fileid + "' ," + i + "";
                //    string sql = @"
                //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                database.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void RemoveFromOBDTran()
        {
            string sql = "Delete from OBDTRAN where picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }

        public int GetOBDDuplicateCount(string mob,  string fileid, string profileid)
        {
           
                string sql = @"select count(*) from obdapisubmitted with (nolock) where TOMOBILE = '" + mob + @"' ";
                sql = sql + "and fileid = '" + fileid + "'";
                int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
                return mcnt;
            
        }
        
        public class To
        {
            public string phoneNumber { get; set; }
        }
        public class Status
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Message
        {
            public To to { get; set; }
            public Status status { get; set; }
            public string messageId { get; set; }
        }

        public class WARoot
        {
            public List<Message> messages { get; set; }
        }



        public class RCSMessage
        {
            public string to { get; set; }
            public int messageCount { get; set; }
            public string messageId { get; set; }
            public Status status { get; set; }
        }

        public class RCSRoot
        {
            public List<RCSMessage> messages { get; set; }
        }


        public class ValidationErrors
        {
            public List<string> whatsApp { get; set; }
        }

        public class ServiceException
        {
            public string messageId { get; set; }
            public string text { get; set; }
            public ValidationErrors validationErrors { get; set; }
        }

        public class RequestError
        {
            public ServiceException serviceException { get; set; }
        }

        public class WaErrorRoot
        {
            public RequestError requestError { get; set; }
        }

        public class WAButton
        {
            public string type { get; set; }
            public string id { get; set; }
            public string title { get; set; }
        }
        public class WAoption
        {
            public string id { get; set; }
            public string title { get; set; }
        }

        public void waapi(string mob, string watext, string apikey, string authkey, string profileid, int fileid, int i)
        {

            var client = new RestClient("https://dm3gpv.api.infobip.com/omni/1/advanced");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            request.AddHeader("Authorization", ""+ authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{

                          ""scenarioKey"": """+apikey+ @""",
                          ""destinations"": [
                            {
                              ""to"": {
                                ""phoneNumber"": """+mob+@"""
                              }
                        }
                          ],
                            ""whatsApp"": {
                            ""text"": """+watext+@"""
                          }
                        }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);

            try
            {
                if (res.messages.Count > 0)
                {
                    Message ms = new Message();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;


                    string sql = @"
                    insert into wasubmitted(messageId,tomobile,watext,groupId,groupName,Id,name,profileid,description,fileid,cnt)
                    select '" + ms.messageId + "','" + mob + "',N'" + watext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + profileid + "','" + st.description + "','" + fileid + "' ," + i + "";
                    //    string sql = @"
                    //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                    //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                    database.ExecuteNonQuery(sql);
                }


            }
            catch (Exception)
            {

                throw;
            }


        }

        public void watemplateapi(string mob,string watext,  string apikey, string authkey, string profileid, int fileid, int i, string templatename, List<string> FieldList)
        {

            string json = JsonConvert.SerializeObject(FieldList);

            if (FieldList.Count<=0)
            {
                json = "[]";
            }
            var client = new RestClient("https://dm3gpv.api.infobip.com/omni/1/advanced");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{

                          ""scenarioKey"": """ + apikey + @""",
                          ""destinations"": [
                            {
                              ""to"": {
                                ""phoneNumber"": """ + mob + @"""
                              }
                        }
                          ],
                            ""whatsApp"": {
                            ""templateName"": """ + templatename + @""",
                            ""templateData"": "+ json + @",
                           ""language"": ""en""
                          }
                        }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            WARoot res = new WARoot();
            WaErrorRoot WAErr = new WaErrorRoot();
            if (response.Content.Contains("BAD_REQUEST"))
            {
                WAErr = JsonConvert.DeserializeObject<WaErrorRoot>(response.Content);

            }
            else
            {
                res = JsonConvert.DeserializeObject<WARoot>(response.Content);

            }

            try
            {

                
                if (res.messages != null && res.messages.Count>0)
                    {
                        Message ms = new Message();
                        ms = res.messages[0];

                        Status st = new Status();
                        st = ms.status;

                        string sql = @"insert into wasubmitted(messageId,tomobile,watext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";

                        string selectsql = @"select '" + ms.messageId + "','" + mob + "',N'" + watext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + profileid + "','" + st.description + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'" + templatename + "' ";
                        for (int n = 1; n <= FieldList.Count; n++)
                        {
                            sql = sql + "," + "Col" + n.ToString();
                            selectsql = selectsql + ",N'" + FieldList[n - 1] + "'";
                        }
                        sql = sql + ") ";

                        database.ExecuteNonQuery(sql + selectsql);
                    }
                else
                {
                    ServiceException Se = new ServiceException();
                    RequestError ReqErr = new RequestError();

                    ReqErr = WAErr.requestError;
                    Se = WAErr.requestError.serviceException;
                    
                    string sql = @"insert into wasubmitted(messageId,tomobile,watext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";
                    string selectsql = "";
                    if (Se.validationErrors!=null)
                    {
                         selectsql = @"select newid(),'" + mob + "',N'" + watext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','" + Se.validationErrors.whatsApp[0] + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'" + templatename + "' ";

                    }
                    else
                    {
                        selectsql = @"select newid(),'" + mob + "',N'" + watext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'" + templatename + "' ";

                    }
                    for (int n = 1; n <= FieldList.Count; n++)
                    {
                        sql = sql + "," + "Col" + n.ToString();
                        selectsql = selectsql + ",N'" + FieldList[n-1] + "'";
                    }
                    sql = sql + ") ";

                    database.ExecuteNonQuery(sql + selectsql);

                }


            }
            catch (Exception ex)
            {
                Info_Client("watemplateapi: Body :"+body+" " + ex.Message,0);
                throw;
            }

        }

        public void RemoveFromwaTran()
        {
            string sql = "Delete from WAMSGTRAN where picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }
        public void RemoveFromRCSTran()
        {
            string sql = "Delete from RCSMSGTRAN where picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }
        public void RemoveFromemailTran()
        {
            string sql = "Delete from emailtran where picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }
        public int GetwaDuplicateCount(string mob, string fileid, string profileid)
        {

            string sql = @"select count(*) from wasubmitted with (nolock) where TOMOBILE = '" + mob + @"' ";
            sql = sql + "and fileid = '" + fileid + "'";
            int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
            return mcnt;

        }
        public int GetemailDuplicateCount(string email, string fileid, string profileid)
        {

            string sql = @"select count(*) from Emailsubmitted with (nolock) where toemail = '" + email + @"' ";
            sql = sql + "and fileid = '" + fileid + "'";
            int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
            return mcnt;

        }

        public string SendEmailSVH(string MailFrom, string Pwd, string Host, string toAddress, string subject, string body,List<string> CC=null)
        {
            //string toAddress = "dilip@myinboxmedia.com";
            //string subject = "Testing emailing";
            //string body = "Test Mail Body";

            //string MailFrom = "noreply@techxure.io";
            //string Pwd = "techXure@123";
            //string Host = "smtpout.secureserver.net";

            string result = "Message Sent Successfully..!!";
            string senderID = MailFrom; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                message.IsBodyHtml = true;
                if (CC!=null && CC.Count>0)
                {
                    for (int i = 0; i < CC.Count; i++)
                    {
                        message.CC.Add(CC[i]);
                    }
                }

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                //ErrLog("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }

        public void rcsimg(string mob,string authkey,string imgurl)
        {
            var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body_img = @"{
             ""from"": ""myinbox"",
             ""to"": """ + mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                ""file"": {
                    ""url"": """+ imgurl + @"""
                },
                ""thumbnail"": {
                     ""url"": """ + imgurl + @"""
                },
            ""type"": ""FILE""
            }
           }";


            request.AddParameter("application/json", body_img, ParameterType.RequestBody);
            IRestResponse response_img = client.Execute(request);

        }

        public void RCSApi(string mob, string msgtext, string suggetiontext, string postbackdata, string notifyUrl,string callbackdata,string msgid, string authkey, string profileid, int fileid, int i, List<string> FieldList,string imgurl="")
        {

            var client = new RestClient("https://lz6q85.api.infobip.com/ott/rcs/1/message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            var body = @"{
             ""from"": ""myinbox"",
             ""to"": """+ mob + @""",
             ""validityPeriod"": 15,
             ""validityPeriodTimeUnit"": ""MINUTES"",
             ""content"": {
                           ""text"": """ + msgtext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n") + @""",
               ""type"": ""TEXT""
             }
           }";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            rcsimg(mob, authkey,imgurl);
            RCSRoot res = new RCSRoot();
            WaErrorRoot WAErr = new WaErrorRoot();
            if (response.Content.Contains("BAD_REQUEST"))
            {
                WAErr = JsonConvert.DeserializeObject<WaErrorRoot>(response.Content);

            }
            else
            {
                res = JsonConvert.DeserializeObject<RCSRoot>(response.Content);
            }

            try
            {
                if (res.messages != null && res.messages.Count > 0)
                {
                    RCSMessage ms = new RCSMessage();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;

                    string sql = @"insert into RCSsubmitted(messageId,tomobile,msgtext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";

                    string selectsql = @"select '" + ms.messageId + "','" + mob + "',N'" + msgtext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + profileid + "','" + st.description + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";
                    for (int n = 1; n <= FieldList.Count; n++)
                    {
                        sql = sql + "," + "Col" + n.ToString();
                        selectsql = selectsql + ",N'" + FieldList[n - 1] + "'";
                    }
                    sql = sql + ") ";

                    database.ExecuteNonQuery(sql + selectsql);
                }
                else
                {
                    ServiceException Se = new ServiceException();
                    RequestError ReqErr = new RequestError();

                    ReqErr = WAErr.requestError;
                    Se = WAErr.requestError.serviceException;

                    string sql = @"insert into RCSsubmitted(messageId,tomobile,msgtext,groupId,groupName,Id,name,profileid,description,fileid,cnt,varcount,templatename ";
                    string selectsql = "";
                    if (Se.validationErrors != null)
                    {
                        selectsql = @"select '"+msgid+"','" + mob + "',N'" + msgtext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','" + Se.validationErrors.whatsApp[0] + "','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";

                    }
                    else
                    {
                        selectsql = @"select '" + msgid + "','" + mob + "',N'" + msgtext + "','0',N'" + Se.text + "',0,N'" + Se.text + "','" + profileid + "','','" + fileid + "' ," + i + "," + FieldList.Count.ToString() + ",'' ";

                    }
                    for (int n = 1; n <= FieldList.Count; n++)
                    {
                        sql = sql + "," + "Col" + n.ToString();
                        selectsql = selectsql + ",N'" + FieldList[n - 1] + "'";
                    }
                    sql = sql + ") ";

                    database.ExecuteNonQuery(sql + selectsql);

                }

            }
            catch (Exception ex)
            {
                Info_Err("RCSApi "+body+" "+ ex.Message,0);

                throw;
            }


           
        }
        public DataTable GetWabaChatRecordForSession()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = @"
SELECT contact_name,frommob MobileNo,MAX( inserttime) replytime FROM [10.10.31.35].[SMPPMAIN_TX].[dbo].[WABAIncMsg] 
WHERE Message_Type='TEXT' and Message_text='hi' and inserttime>=DATEADD(MINUTE,-15,GETDATE()) GROUP BY contact_name,frommob  order by MAX( inserttime) desc";
                dt = database.GetDataTable(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetWabaInProcessRecord()
        {
            string LastProcessedTime = "";
            //LastProcessedTime = Convert.ToString(database.GetScalarValue("select WabaChatLastProcessedTime from SettingMast"));

            DataTable dt = new DataTable();
            try
            {
                string sql = @"
                declare @cDate datetime 
                set @cDate=(select getdate())
                SELECT MESSAGE_TEXT,MESSAGE_TITLE,FROMMOB MobileNo,INSERTTIME,Isnull(ProcessId,'0') ProcessId,msgid from [SMPPMAIN_TX].[dbo].[WABAIncMsg] rep left join WabaSession w on rep.FROMMOB=w.MobileNo
                where inserttime>(select WabaChatLastProcessedTime from SettingMast) and inserttime<=@cDate and fromMob not in (select CustomerNumber from LiveSession)
                and fromMob not in (select MobileNo from MiM_WABA_CHAT.dbo.wabasession)
and frommob in ('919870333974','917678936834','919305334312') order by inserttime 
                Update SettingMast set WabaChatLastProcessedTime=@cDate ;";
                dt = database.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                _Log("GetWabaInProcessRecord : " + ex.Message);
                throw;
            }
        }

        public void ResetWabaSession(string mobileno)
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ResetWabaSession", cnn))
                {
                    cnn.Open();
                    cmd.CommandTimeout = 600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("MobileNo", mobileno);
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }

        }
        public int SetSession(string MobileNo, string msgtext)
        {
            int FlowId = 0;
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CreateWabaSession", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("msgtext", msgtext);
                        cmd.Parameters.AddWithValue("FlowId", 0);
                        cmd.Parameters["FlowId"].Direction = ParameterDirection.InputOutput;
                        cmd.Parameters["FlowId"].Size = 0x100;
                        
                        cmd.ExecuteNonQuery();

                        FlowId = Convert.ToInt32(cmd.Parameters["FlowId"].Value.ToString());

                        cnn.Close();
                    }
                }
                return FlowId;
            }
            catch (Exception ex)
            {

                _Log("SetSession : " + ex.Message);
                return 0;
            }

        }
        public void SetSession(string MobileNo)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CreateWabaSession", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                
                _Log("SetSession : "+ex.Message);
            }
           
        }

        public void WaChatProcess(int ProcessId, string MobileNo, string apikey, string authkey,string MESSAGE_TEXT="")
        {
          //  string msg = dtWAChatProcess.Select("ProcessNo=" + ProcessId + "")[0]["msgtext"].ToString();

            //if (ProcessId==1)
            //{
            //    MESSAGE_TEXT = MESSAGE_TEXT.Replace("Name ", MESSAGE_TEXT);
            //}
            if (MESSAGE_TEXT.Trim()!="")
            {
                wachatapi(ProcessId, MobileNo, MESSAGE_TEXT, apikey, authkey);
            }
        }

        public void Reply_Process(string MobileNo, int ProcessId, string MESSAGE_TEXT, string MESSAGE_Title, string apikey, string authkey,string MsgId)
        {
            try
            {
                int FlowId = 0;

                FlowId = SetSession(MobileNo, (MESSAGE_TEXT + MESSAGE_Title).Trim());
                if (FlowId == 0) { return; }
                
                string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,MsgType,SessionId)
                    select " + ProcessId + ",'" + MsgId + "','" + MobileNo + "',N'" + MESSAGE_TEXT+ MESSAGE_Title + "',0,'',0,'','' ,'Received' ,ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + MobileNo + "'),0)";
                    database.ExecuteNonQuery(sql);

                 bool IsRedirect = false;

                string NextProcessNo = "0";
                string msgtext = "";

                DataTable dt= GetNextProcessNo(ProcessId, MESSAGE_TEXT + MESSAGE_Title.ToLower().Trim(),FlowId);
                if (dt!=null && dt.Rows.Count>0)
                {
                    NextProcessNo = dt.Rows[0]["ProcessNo"].ToString();
                    msgtext = dt.Rows[0]["msgtext"].ToString();
                    IsRedirect = bool.Parse(dt.Rows[0]["IsRedirect"].ToString());

                    if (NextProcessNo=="2")
                    {
                        string sql1 = @"IF EXISTS(SELECT 1 FROM CustomerMast  WHERE MobileNumber='" + MobileNo + "')BEGIN UPDATE CustomerMast SET Name='" + MESSAGE_TEXT + "'   WHERE MobileNumber='" + MobileNo + "' END  ELSE  BEGIN insert into CustomerMast(MobileNumber,Name) select '" + MobileNo + "','" + MESSAGE_TEXT + "' END";
                        database.ExecuteNonQuery(sql1);
                    }
                }
             
                WaChatProcess(ProcessId, MobileNo, apikey, authkey, msgtext);

                database.ExecuteNonQuery("UPDATE WABASESSION set Processid=" + NextProcessNo + " where MobileNo='" + MobileNo + "' and flowId='"+FlowId+"'");

                if (IsRedirect)
                {
                    string AgentNumber = GetAgentForAlignChat("");
                    if (AgentNumber != "")
                    {
                        msgtext = "";
                        SetLiveSession(AgentNumber, MobileNo, "", "C", msgtext);
                    }
                }

            }
            catch (Exception ex)
            {
                _Log(ex.Message);
                throw;
            }
        }

        public void wachatapi(int SessionId,string mob, string watext, string apikey, string authkey)
        {

            var client = new RestClient("https://dm3gpv.api.infobip.com/omni/1/advanced");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{

                          ""scenarioKey"": """ + apikey + @""",
                          ""destinations"": [
                            {
                              ""to"": {
                                ""phoneNumber"": """ + mob + @"""
                              }
                        }
                          ],
                            ""whatsApp"": {
                            ""text"": """ + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @" \n ") + @"""
                          }
                        }";

           
            request.AddParameter("application/json", body, ParameterType.RequestBody);
          
            IRestResponse response = client.Execute(request);

            
            try
            {
                WARoot res = JsonConvert.DeserializeObject<WARoot>(response.Content);

                if (res.messages.Count > 0)
                {
                    Message ms = new Message();
                    ms = res.messages[0];

                    Status st = new Status();
                    st = ms.status;


                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + SessionId + "','" + ms.messageId + "','" + mob + "',N'" + watext + "','" + st.groupId + "','" + st.groupName + "'," + st.id + ",'" + st.name + "','" + st.description + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "'),0) ";
                    //    string sql = @"
                    //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                    //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                    database.ExecuteNonQuery(sql);
                }


            }
            catch (Exception EX)
            {
                // mail for error
                _Log("wachatapi : Body " + body + " : Response " + response.Content);

                throw;
            }


        }

        public class wchatStatus
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public object action { get; set; }
        }
        public class wchatRoot
        {
            public string to { get; set; }
            public int messageCount { get; set; }
            public string messageId { get; set; }
            public wchatStatus status { get; set; }
        }

        public void wachatapi_with_buttons(int ProcessId, string mob, string watext, string apikey, string authkey,List<WAButton> btnlist)
        {

            string json = JsonConvert.SerializeObject(btnlist);

            if (btnlist.Count <= 0)
            {
                json = "[]";
            }

            var client = new RestClient("https://dm3gpv.api.infobip.com/whatsapp/1/message/interactive/buttons");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{
            ""from"": ""919289013414"",
            ""to"": """+mob+@""",
            ""content"": {
                ""body"": {
                    ""text"": """+watext+@"""
                    },
                ""action"": {
                    ""buttons"": "+ json + @"
                    }
                }
            }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            wchatRoot res = JsonConvert.DeserializeObject<wchatRoot>(response.Content);

            try
            {
                if (res.status.ToString()!="")
                {
                    wchatStatus ms = new wchatStatus();
                    ms = res.status;
                    
                    string sql = @"
                    insert into wabachat(Processid,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + res.messageId + "','" + mob + "',N'" + watext + "','" + ms.groupId + "','" + ms.groupName + "'," + ms.id + ",'" + ms.name + "','" + ms.description + "' ,ISNULL((SELECT MAX(ID) FROM LogWABASESSION WHERE MOBILENO='" + mob + "'),0)+1";
                    //    string sql = @"
                    //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                    //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                    database.ExecuteNonQuery(sql);
                }


            }
            catch (Exception ex)
            {
                _Log("wachatapi_with_buttons : Body : " + body+" Response "+ response.Content);

                throw;
            }


        }

        public void wachatapi_Interective_List(int ProcessId, string mob, string watext, string apikey, string authkey, List<WAoption> optionlist)
        {

            string json = JsonConvert.SerializeObject(optionlist);

            if (optionlist.Count <= 0)
            {
                json = "[]";
            }

            var client = new RestClient("https://dm3gpv.api.infobip.com/whatsapp/1/message/interactive/list");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            //request.AddHeader("x-api-key", apikey);FF0A1CFB44BC6BF5BBBF03CC97A104FF
            request.AddHeader("Authorization", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = @"{
            ""from"": ""919289013414"",
            ""to"": """ + mob + @""",
            ""content"": {
                ""body"": {
                    ""text"": """ + watext + @"""
                    },
                ""action"": {
                ""title"": ""Choose one"",
                ""sections"": [
                                {
                                    ""rows"": " + json + @"
                                }
                             ]
                  }
                }
            }";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            wchatRoot res = JsonConvert.DeserializeObject<wchatRoot>(response.Content);

            try
            {
                if (res.status.ToString() != "")
                {
                    wchatStatus ms = new wchatStatus();
                    ms = res.status;



                    string sql = @"
                    insert into wabachat(Processid,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + res.messageId + "','" + mob + "',N'" + watext + "','" + ms.groupId + "','" + ms.groupName + "'," + ms.id + ",'" + ms.name + "','" + ms.description + "',ISNULL((SELECT MAX(ID) FROM LogWABASESSION WHERE MOBILENO='" + mob + "'),0)+1";
                    //    string sql = @"
                    //insert into obdapisubmitted(order_id,tomobile,obdtext,result,status_code,profileid,fileid,cnt)
                    //select '0','" + mob + "','" + obdtext + "','',0 ,'" + profileid + "','" + fileid + "'," + i + "";
                    database.ExecuteNonQuery(sql);
                }


            }
            catch (Exception ex)
            {
                _Log("wachatapi_Interective_List : Body " + body + " : Response " + response.Content);
                throw;
            }


        }

        public DataTable GetNextProcessNo(int processno,string typetext,int flowId)
        {
            string PROCESSNO = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetNextProcessNo_test", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        SqlDataAdapter da = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("ProcessId", processno);
                        cmd.Parameters.AddWithValue("typeText", typetext);
                        cmd.Parameters.AddWithValue("flowId", flowId);

                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        cnn.Close();
                    }
                }
               

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public String GetAgentForAlignChat(string ServiceType)
        {
            string Mob = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetAgentForAlignChat", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("ServiceType", ServiceType);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        cnn.Close();
                    }
                }
                if (dt!=null && dt.Rows.Count>0)
                {
                    Mob = dt.Rows[0]["AgentNumber"].ToString();
                }

            }
            catch (Exception ex)
            {
                
            }
            return Mob;
        }

        public void SetLiveSession(string AgentNumber,string CustomerNumber,string CustomerName,string LastMessageFrom,string LastMessage)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SetLiveSession", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("AgentNumber", AgentNumber);
                        cmd.Parameters.AddWithValue("CustomerNumber", CustomerNumber);
                        cmd.Parameters.AddWithValue("CustomerName", CustomerName);
                        cmd.Parameters.AddWithValue("LastMessageFrom", LastMessageFrom);
                        cmd.Parameters.AddWithValue("LastMessage", LastMessage);
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log("SetSession : " + ex.Message);
                throw;
            }

        }

        public DataTable GetIncomingMessage()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetIncomingMessage", cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = 600;
                        SqlDataAdapter da = new SqlDataAdapter();
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        cnn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log("GetSaveIncomingmessage : " + ex.Message);
                throw;
            }
            return dt;
        }

        #region<kerix>
        public class KerixRoot
        {
            public string statusCode { get; set; }
            public string statusDesc { get; set; }
            public string mid { get; set; }
        }


        public void wachatapi_karix(int ProcessId, string mob, string watext, string apikey, string authkey, string ApiUrl = "", string acid = "", int flowid = 0)
        {
            string EndUrl = "";

            Guid obj = Guid.NewGuid();
            string custRef = Convert.ToString(obj);

            var client = new RestClient(ApiUrl + EndUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authentication", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");

            var body = @"{
        ""message"": {
        ""channel"": ""WABA"",
        ""content"": {
                ""preview_url"": false,
            ""text"": """ + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @""",
            ""type"": ""TEXT""
        },
        ""recipient"": {
                ""to"": """ + mob + @""",
            ""recipient_type"": ""individual"",
            ""reference"": {
                    ""cust_ref"": """ + custRef + @""",
                ""messageTag1"": ""Message Tag Val1"",
                ""conversationId"": ""Some Optional Conversation ID""
            }
            },
        ""sender"": {
                ""from"": """ + acid + @"""
        },
        ""preferences"": {
                ""webHookDNId"": ""1001""
        }
        },
    ""metaData"": {
        ""version"": ""v1.0.9""
    }
}";

            // var body = @"{
            //  ""from"": """ + acid + @""",
            //  ""to"": """ + mob + @""",

            //  ""content"": {
            //                ""text"": """ + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @"""

            //  }
            //}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            try
            {
                KerixRoot res = new KerixRoot();

                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<KerixRoot>(response.Content);
                    if (res.mid == "")
                    {
                        res.mid = custRef;
                    }
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + res.mid + "','" + mob + "',N'" + watext + "','0','" + res.statusCode + "',0,'" + res.statusCode + "','" + res.statusDesc + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";
                    database.ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + custRef + "','" + mob + "',N'" + watext + "','0','',0,'" + res.statusCode + "','" + response.Content.Replace(@"""", @"") + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";
                    database.ExecuteNonQuery(sql);

                    _Log("wachatapi_kerix : Body " + body + " : Response " + response.Content);
                }


            }
            catch (Exception EX)
            {
                // mail for error
                _Log("wachatapi_kerix : Body " + body + " : Response " + response.Content);

                throw;
            }


        }
        public void wachatapiList_karix(int ProcessId, string mob, string watext, string apikey, string authkey, string ApiUrl = "https://dm3gpv.api.infobip.com", string acid = "", int flowid = 0, string Content = "[]")
        {
            string EndUrl = "";
            Guid obj = Guid.NewGuid();
            string custRef = Convert.ToString(obj);

            var client = new RestClient(ApiUrl + EndUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authentication", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");

            //""header"": {
            //    ""type"": ""text"",
            //        ""text"": ""
            //    },
            // ""footer"": {
            //    ""text"": ""
            //    }

            var body = @"{
    ""message"": {
        ""channel"": ""WABA"",
        ""content"": {
            ""preview_url"": false,
            ""shorten_url"": false,
            ""type"": ""INTERACTIVE"",
            ""interactive"": {
                ""type"": ""list"",
              
                ""body"": {
                    ""text"":""" + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @"""

                },
               
                ""action"":" + Content + @"
            }
        },
        ""recipient"": {
            ""to"": """ + mob + @""",
            ""recipient_type"": ""individual"",
            ""reference"": {
                ""cust_ref"": """ + custRef + @""",
                ""messageTag1"": ""Message Tag 001"",
                ""conversationId"": ""Conv_123""
            }
        },
        ""sender"": {
            ""from"": """ + acid + @"""
        },
        ""preferences"": {
            ""webHookDNId"": ""1001""
        }
    },
    ""metaData"": {
        ""version"": ""v1.0.9""
    }
}";

            //var body = @"{
            //""from"": """ + acid + @""",
            //""to"": """ + mob + @""",
            //""content"": {
            //    ""body"": {
            //        ""text"": """ + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @"""
            //        },
            //    ""action"": " + Content + @"
            //    }
            //}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            try
            {
                KerixRoot res = new KerixRoot();

                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<KerixRoot>(response.Content);
                    if (res.mid == "")
                    {
                        res.mid = custRef;
                    }
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + res.mid + "','" + mob + "',N'" + watext + "','0','" + res.statusCode + "',0,'" + res.statusCode + "','" + res.statusDesc + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";

                    database.ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + custRef + "','" + mob + "',N'" + watext + "','0','',0,'" + res.statusCode + "','" + response.Content.Replace(@"""", @"") + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";
                    database.ExecuteNonQuery(sql);

                    _Log("wachatapiList_kerix : Body " + body + " : Response " + response.Content);
                }
            }
            catch (Exception EX)
            {
                _Log("wachatapiList_kerix : Body " + body + " : Response " + response.Content);
                throw;
            }


        }
        public void wachatapiButton_karix(int ProcessId, string mob, string watext, string apikey, string authkey, string ApiUrl = "https://dm3gpv.api.infobip.com", string acid = "", int flowid = 0, string Content = "[]")
        {
            string EndUrl = "";
            Guid obj = Guid.NewGuid();
            string custRef = Convert.ToString(obj);

            var client = new RestClient(ApiUrl + EndUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authentication", "" + authkey + "");
            request.AddHeader("Content-Type", "application/json");

            //""header"": {
            //    ""type"": ""text"",
            //        ""text"": ""
            //    },
            // ""footer"": {
            //    ""text"": ""
            //    }

            var body = @"{
    ""message"": {
        ""channel"": ""WABA"",
        ""content"": {
            ""preview_url"": false,
            ""shorten_url"": false,
            ""type"": ""INTERACTIVE"",
            ""interactive"": {
                ""type"": ""button"",
              
                ""body"": {
                    ""text"":""" + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @"""

                },
               
                ""action"":" + Content + @"
            }
        },
        ""recipient"": {
            ""to"": """ + mob + @""",
            ""recipient_type"": ""individual"",
            ""reference"": {
                ""cust_ref"": """ + custRef + @""",
                ""messageTag1"": ""Message Tag 001"",
                ""conversationId"": ""Conv_123""
            }
        },
        ""sender"": {
            ""from"": """ + acid + @"""
        },
        ""preferences"": {
            ""webHookDNId"": ""1001""
        }
    },
    ""metaData"": {
        ""version"": ""v1.0.9""
    }
}";

            //var body = @"{
            //""from"": """ + acid + @""",
            //""to"": """ + mob + @""",
            //""content"": {
            //    ""body"": {
            //        ""text"": """ + watext.Replace(Convert.ToString(Convert.ToString(Convert.ToChar(13)) + Convert.ToString(Convert.ToChar(10))), @"\n").Replace(Convert.ToString(Convert.ToChar(10)), @"\n").Replace(@"""", @"\""") + @"""
            //        },
            //    ""action"": " + Content + @"
            //    }
            //}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string pStatus = response.StatusCode.ToString();

            try
            {
                KerixRoot res = new KerixRoot();

                if (pStatus.ToUpper() == "OK")
                {
                    res = JsonConvert.DeserializeObject<KerixRoot>(response.Content);
                    if (res.mid == "")
                    {
                        res.mid = custRef;
                    }
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + res.mid + "','" + mob + "',N'" + watext + "','0','" + res.statusCode + "',0,'" + res.statusCode + "','" + res.statusDesc + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";

                    database.ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = @"
                    insert into wabachat(ProcessId,messageId,tomobile,watext,groupId,groupName,Id,name,description,SessionId)
                    select '" + ProcessId + "','" + custRef + "','" + mob + "',N'" + watext + "','0','',0,'" + res.statusCode + "','" + response.Content.Replace(@"""", @"") + "',ISNULL((SELECT MAX(ID) FROM WABASESSION WHERE MOBILENO='" + mob + "' and FlowId=" + flowid + " ),0) ";
                    database.ExecuteNonQuery(sql);

                    _Log("wachatapiList_kerix : Body " + body + " : Response " + response.Content);
                }
            }
            catch (Exception EX)
            {
                _Log("wachatapiList_kerix : Body " + body + " : Response " + response.Content);
                throw;
            }


        }

        #endregion
    }
}
