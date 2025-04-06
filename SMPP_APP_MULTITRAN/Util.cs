using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Inetlab.SMPP.PDU;
using System.Data.SqlClient;
using System.Net.Mail;

namespace SMPP_APP
{
    public class Util
    {
        //public string sql = "";
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public string fn2 = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString() + @"LOG2\";
        public int LogErr = 1;
        public string Ldays = "15";
        public static DataTable tblerror = null;
        public static DataTable dtDLRCallBackCust;

        public void InfoTest(string msg) { }

        public void InfoTest2(string msg)
        {
            

            //try
            //{
            //    //if (LogErr == 1)
            //    //{
            //    FileStream filestrm = new FileStream(fn + @"test\" + @"_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn + @"test\" + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex4) { }
            //}
        }

        public void Info(string msg)
        {
            //try
            //{
            //    //if (LogErr == 1)
            //    //{
            //    FileStream filestrm = new FileStream(fn2 + @"Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn2 + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex4) { }
            //}
        }
        public void InfoDelay(string msg)
        {
            //try
            //{
            //    //if (LogErr == 1)
            //    //{
            //    FileStream filestrm = new FileStream(fn2 + @"Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn2 + @"catch_Log" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex4) { }
            //}
        }
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
        public void LogEXEClose(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"EXECLOSE" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"EXECLOSEcth" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                FileStream filestrm = new FileStream(fn + @"Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2)
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"catch2_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception ex3)
                    {
                        try
                        {
                            FileStream filestrm = new FileStream(fn + @"catch3_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter strmwriter = new StreamWriter(filestrm);
                            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                            strmwriter.Flush();
                            strmwriter.Close();
                        }
                        catch (Exception ex4)
                        {
                            try
                            {
                                FileStream filestrm = new FileStream(fn + @"catch4_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                                StreamWriter strmwriter = new StreamWriter(filestrm);
                                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                                strmwriter.Flush();
                                strmwriter.Close();
                            }
                            catch (Exception ex5)
                            {
                                throw ex5;
                            }

                        }
                    }
                }
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
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"catch2_LogSubmit_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception ex3)
                    { }

                }
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
                {
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"catch2_LogFailed_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception ex3)
                    { }

                }
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
                    try
                    {
                        FileStream filestrm = new FileStream(fn + @"catch2_LogErr_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter strmwriter = new StreamWriter(filestrm);
                        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                        strmwriter.Flush();
                        strmwriter.Close();
                    }
                    catch (Exception ex3)
                    {
                        throw ex3;
                    }
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
        public void Info_Excepn(string msg, int client)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"Log_Exception_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"catch_Log_Exception_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
            //try
            //{
            //    FileStream filestrm = new FileStream(fn + @"18_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn + @"18_catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex2)
            //    { throw ex2; }
            //}
        }
        public void Info_Client25(string msg, int client)
        {
            //try
            //{
            //    FileStream filestrm = new FileStream(fn + @"25_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn + @"25_catch_Log_" + client.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex2)
            //    { throw ex2; }
            //}
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
            //try
            //{
            //    //if (LogErr == 1)
            //    //{
            //    FileStream filestrm = new FileStream(fn + @"Dlv_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn + @"Dlv_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex2) { }
            //}
        }
        public void LogDlvError2(string msg)
        {
            //try
            //{
            //    //if (LogErr == 1)
            //    //{
            //    FileStream filestrm = new FileStream(fn + @"ErrTmp_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    StreamWriter strmwriter = new StreamWriter(filestrm);
            //    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //    strmwriter.Flush();
            //    strmwriter.Close();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        FileStream filestrm = new FileStream(fn + @"ErrTmp_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            //        StreamWriter strmwriter = new StreamWriter(filestrm);
            //        strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            //        strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            //        strmwriter.Flush();
            //        strmwriter.Close();
            //    }
            //    catch (Exception ex2) { }
            //}
        }



        public DataTable GetSMPPAccounts()
        {
            string sql = "Select n.sessionid as smppaccountid, s.PDUSIZE,s.PROVIDER,s.ACCOUNTTYPE,s.HOSTNAME,s.PORT,s.USESSL,s.SYSTEMID,s.PASSWORD,s.BINDINGMODE,s.SYSTEMTYPE,s.ADDRESS_TON,s.ADDRESS_NPI,s.SOURCE_ADDRESS,s.TON_S,s.NPI_S,s.SERVICE,s.DESTNATION_ADDRESS,s.TON_D,s.NPI_D,s.DATACODING,s.MODE,s.CREATEDAT,s.ACTIVE,S.SMSExpiryMinute,S.TRANTABLENAME, row_number() over (order by  n.sessionid) as rownum, smsFromTime,smsToTime " +
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

        public DataTable GetSMSRecords(int acId, int no_of_sms, string picktime, string tranTableName)
        {
            DataTable dt = new DataTable();
            try
            {
                InfoTest("sms data req start from - " + acId);
                Int64 c = Convert.ToInt64(database.GetScalarValue("select count(*) from "+ tranTableName +" WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NOT NULL"));
                if (c == 0)
                {
                    c = Convert.ToInt64(database.GetScalarValue("select count(*) from " + tranTableName + " WHERE SMPPACCOUNTID = '" + acId + "' AND PICKED_DATETIME IS NULL"));
                    if (c > 0)
                    {
                        string sql = @"
                        declare @cnt numeric(10)
                        set @cnt = 0 ;
                        if @cnt = 0 
                        begin
                            BEGIN TRY
                                
                                    WITH CTE AS  
                                    (select top " + no_of_sms.ToString() + @" * from " + tranTableName + @"
                                     where SMPPACCOUNTID = '" + acId + @"' and picked_datetime IS NULL ORDER BY CREATEDAT 
                                    ) UPDATE CTE SET picked_datetime='" + picktime + @"'
                                    select * from " + tranTableName + @" where SMPPACCOUNTID = '" + acId + @"' AND picked_datetime ='" + picktime + @"' ORDER BY CREATEDAT
                                
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
        public void AddInMsgSubmittedNew(SubmitSmResp resp, DataTable dt, DateTime sendTime)
        {
            //try
            //{
            //if (resp.MessageId != "")
            //{
                string mobn = resp.Request.DestinationAddress.Address;
                string msgt = resp.Request.MessageText.ToString();
                DataRow[] dr = dt.Select("ToMobile = '" + mobn + "'");
            if (dr.Length > 0)
            {
                string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,PEID,TEMPLATEID,smstext,DATACODE,DELIVERY_STATUS,msgid2) " +
                        "values ('" + dr[0]["id"].ToString() + @"',
                            '" + dr[0]["PROVIDER"].ToString() + @"',
                            '" + dr[0]["SMPPACCOUNTID"].ToString() + @"',
                            '" + dr[0]["PROFILEID"].ToString() + @"',
                            N'" + msgt.Replace("'", "''") + @"',
                            '" + mobn + @"',
                            '" + dr[0]["SENDERID"].ToString() + @"',
                            '" + Convert.ToDateTime(dr[0]["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',
                            '" + sendTime.ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"','" +
                         (dr[0]["MSGID2"] == DBNull.Value ? Convert.ToString(resp.MessageId) : Convert.ToString(dr[0]["MSGID2"])) + "','" +
                        (dr[0]["FILEID"] == DBNull.Value ? "0" : dr[0]["FILEID"].ToString()) + "','" +
                        (dr[0]["PEID"] == DBNull.Value ? "" : dr[0]["PEID"].ToString()) + "','" +
                        (dr[0]["TEMPLATEID"] == DBNull.Value ? "" : dr[0]["TEMPLATEID"].ToString()) + "',N'" +
                        (dr[0]["MSGTEXT"] == DBNull.Value ? "" : dr[0]["MSGTEXT"].ToString().Replace("'", "''")) + "','" +
                        dr[0]["DATACODE"].ToString() + @"','" + Convert.ToString(resp.Header.Status) + "','" +
                        (dr[0]["MSGID2"] == DBNull.Value ? "" : Convert.ToString(resp.MessageId)) + "')";
                database.ExecuteNonQuery(sql);

                bool isNumeric = long.TryParse(Convert.ToString(resp.Header.Status).Trim(), out long n);
                if (isNumeric)
                {
                    //process Delivery
                    sql = " Insert into DELIVERY(DLVRTEXT, MSGID, DLVRTIME, donedate, DLVRSTATUS, err_code) " +
                    " select 'id:" + resp.MessageId + " sub:001 dlvrd:000 submit date:' + RIGHT(cONVERT(VARCHAR,getdate(),112),6) + REPLACE(cONVERT(VARCHAR,getdate(),108),':','') + " +
                    "' done date:' + RIGHT(cONVERT(VARCHAR, GETDATE(), 112), 6) + REPLACE(cONVERT(VARCHAR, GETDATE(), 108), ':', '') + ' stat:UNDELIV err:" + Convert.ToString(resp.Header.Status) + " text:' AS DLVRTEXT," +
                    " '" + resp.MessageId + "', GETDATE(),GETDATE(),'Undeliverable','" + Convert.ToString(resp.Header.Status).Trim() + "' ; ";
                    try
                    {
                        database.ExecuteNonQuery(sql);
                    }
                    catch(Exception e1)
                    {
                        LogError("inDLR-" + e1.Message + " " + sql);
                    }
                }
            }
            //}
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
        public void EtisalatSubmitReject(DataRow dr)
        {
            string newid = Guid.NewGuid().ToString();
            string sql = "Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,DELIVERY_STATUS,FILEID) " +
                    "values ('" + dr["id"].ToString() + @"',
                '" + dr["PROVIDER"].ToString() + @"',
                '" + dr["SMPPACCOUNTID"].ToString() + @"',
                '" + dr["PROFILEID"].ToString() + @"',
                '" + dr["MSGTEXT"].ToString().Replace("'","''") + @"',
                '" + dr["TOMOBILE"].ToString() + @"',
                '" + dr["SENDERID"].ToString() + @"',
                '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',getdate(),'" + newid + "','BEYOND_TIME','" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "') ; ";
            sql = sql + " Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,DLVRSTATUS,ERR_CODE) values ('BEYOND_TIME','" + newid + "',GETDATE(),'FAILED','2051') ";
            try
            {
                database.ExecuteNonQuery(sql);
            }
            catch (Exception ex) { }
            
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
                try
                {
                    database.ExecuteNonQuery(sql);
                }
                catch (Exception ex) { }
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

        public void RemoveFromMsgTran(string SMPPAccountID, string tranTableName)
        {
            string sql = "Delete from " + tranTableName + " where SMPPACCOUNTID='" + SMPPAccountID + "' and picked_datetime is not null ";
            database.ExecuteNonQuery(sql);
        }
        public void RemoveAllFromMsgTran(string picktime)
        {
            string sql = "INSERT INTO MSGTRANPROCESSED (PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,PICKED_DATETIME,FILEID) " +
                "SELECT PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,PICKED_DATETIME,FILEID FROM msgtran where picked_datetime ='" + picktime +"' ; " +
                "Delete from msgtran where picked_datetime ='" + picktime + "'  ";
            database.ExecuteNonQuery(sql);
        }
        public void UpdateDelivery(string msgId, DateTime donedate, string dlvStat, string txt, string errcd,string MCCMNC, string sid, string destno,string dlrCallBack)
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

            string msgid2 = "", templateid = "", peid = "", msgtext = "", profileid = "", sentdatetime = DateTime.Now.ToString();

            try
            {
                string sql;
                
                //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));



                // 04/07/2022
                // msgid2 = Convert.ToString(database.GetScalarValue("select top 1 msgid from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'"));
                DataTable dtDl = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'");
                if (dtDl.Rows.Count>0)
                {
                    msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                    templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                    peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                    msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                    profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                    sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                }

                if (msgid2 != "")
                {
                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                        "('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";

                    DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                    if (drCallBack.Length != 0)
                        sql = sql + "; Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                        "('" + profileid + "','" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                }
                else
                {
                    sql = "Insert into DELIVERY (DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                        "('" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                    if (drCallBack.Length != 0)
                        sql = sql + "; Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,DLVRTIME,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                        "('" + profileid + "','" + txt.Replace("'", "''") + "','" + msgId + "','" + d + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                }
                database.ExecuteNonQuery(sql);

                // Rabi for template block email 05/07/2022
                try
                {
                    DataRow[] founderror = tblerror.Select("err_code = '" + errcd + "'");
                    if (founderror.Length != 0)
                    {
                        if(dtDl.Rows.Count == 0)
                        {
                            DataTable dtDl1 = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                            if (dtDl1.Rows.Count > 0)
                            {
                                msgid2 = Convert.ToString(dtDl1.Rows[0]["msgid"]);
                                templateid = Convert.ToString(dtDl1.Rows[0]["templateid"]);
                                peid = Convert.ToString(dtDl1.Rows[0]["peid"]);
                                msgtext = Convert.ToString(dtDl1.Rows[0]["msgtext"]);
                                profileid = Convert.ToString(dtDl1.Rows[0]["profileid"]);
                                sentdatetime = Convert.ToString(dtDl1.Rows[0]["sentdatetime"]);
                            }
                        }

                        string sqlerr = "Insert into errorlog (dlrtime,msgId,mobileno,senderid,templateid,peid,msgtext,profileid,senddate,errorcode,errordesc,errortype) " +
                        "values ('" + d + "','" + msgId + "','" + destno + @"','" +
                        sid + "','" + templateid + "','" + peid + "','" + msgtext.Replace("'", "''") + "','" + profileid + "','" + Convert.ToDateTime(sentdatetime).ToString("dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "','" + errcd + "','" + founderror[0]["descreption"].ToString() + "','')";
                        database.ExecuteNonQuery(sqlerr);
                    
                        //UpdateOpenTemplate();

                        send_errormail(d, msgId, msgid2, destno, sid, templateid, peid, msgtext.Replace("'", "''"), profileid, sentdatetime, errcd, founderror[0]["descreption"].ToString());
                    }
                }
                catch (Exception e4)
                {
                    LogDlvError2("DlrEr2-" + e4.Message);
                    //throw;
                }

            }
            catch (Exception ex2)
            {
                try
                {
                    LogDlvError("Dlv Err - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                    string sql;
                    
                    //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 isnull(msgid,'') from msgsubmitted WITH (NOLOCK)  where msgid2='" + msgId + "'"));
                    //msgid2 = Convert.ToString(database.GetScalarValue("select top 1 msgid from msgsubmitted WITH (NOLOCK)  where msgid2='" + msgId + "'"));
                    DataTable dtDl = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid2='" + msgId + "'");
                    if (dtDl.Rows.Count > 0)
                    {
                        msgid2 = Convert.ToString(dtDl.Rows[0]["msgid"]);
                        templateid = Convert.ToString(dtDl.Rows[0]["templateid"]);
                        peid = Convert.ToString(dtDl.Rows[0]["peid"]);
                        msgtext = Convert.ToString(dtDl.Rows[0]["msgtext"]);
                        profileid = Convert.ToString(dtDl.Rows[0]["profileid"]);
                        sentdatetime = Convert.ToString(dtDl.Rows[0]["sentdatetime"]);
                    }

                    if (msgid2 != "")
                    {
                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                            "('" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                        DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                        if (drCallBack.Length != 0)
                            sql = sql + "; Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                            "('" + profileid + "','" + txt.Replace("'", "''").Replace(msgId, msgid2) + "','" + msgid2 + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    }
                    else
                    {
                        sql = "Insert into DELIVERY (DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                            "('" + txt.Replace("'", "''") + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                        DataRow[] drCallBack = dtDLRCallBackCust.Select("username = '" + profileid + "'");
                        if (drCallBack.Length != 0)
                            sql = sql + "; Insert into DELIVERYcallback (PROFILEID,DLVRTEXT,MSGID,donedate,DLVRSTATUS,err_code,sid,destno,mccmnc) values " +
                            "('" + profileid + "','" + txt.Replace("'", "''") + "','" + msgId + "','" + donedate.ToString("dd/MMM/yyyy HH:mm:ss.fff") + "','" + dlvStat + "','" + errcd + "','" + sid + "','" + destno + "','" + MCCMNC + "')";
                    }
                    try
                    {
                        database.ExecuteNonQuery(sql);

                        // Rabi for template block email 05/07/2022
                        try
                        {
                            DataRow[] founderror = tblerror.Select("err_code = '" + errcd + "'");
                            if (founderror.Length != 0)
                            {
                                if (dtDl.Rows.Count == 0)
                                {
                                    DataTable dtDl1 = database.GetDataTable("select top 1 msgid,templateid,peid,msgtext,profileid,sentdatetime from msgsubmitted WITH (NOLOCK) where msgid='" + msgId + "'");
                                    if (dtDl1.Rows.Count > 0)
                                    {
                                        msgid2 = Convert.ToString(dtDl1.Rows[0]["msgid"]);
                                        templateid = Convert.ToString(dtDl1.Rows[0]["templateid"]);
                                        peid = Convert.ToString(dtDl1.Rows[0]["peid"]);
                                        msgtext = Convert.ToString(dtDl1.Rows[0]["msgtext"]);
                                        profileid = Convert.ToString(dtDl1.Rows[0]["profileid"]);
                                        sentdatetime = Convert.ToString(dtDl1.Rows[0]["sentdatetime"]);
                                    }
                                }

                                string sqlerr = "Insert into errorlog (dlrtime,msgId,mobileno,senderid,templateid,peid,msgtext,profileid,senddate,errorcode,errordesc,errortype) " +
                                "values ('" + d + "','" + msgId + "','" + destno + @"','" +
                                sid + "','" + templateid + "','" + peid + "','" + msgtext.Replace("'", "''") + "','" + profileid + "','" + Convert.ToDateTime(sentdatetime).ToString("dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "','" + errcd + "','" + founderror[0]["descreption"].ToString() + "','')";
                                database.ExecuteNonQuery(sqlerr);

                                //UpdateOpenTemplate();

                                send_errormail(d, msgId, msgid2, destno, sid, templateid, peid, msgtext.Replace("'", "''"), profileid, sentdatetime, errcd, founderror[0]["descreption"].ToString());
                            }
                        }
                        catch (Exception e4)
                        {
                            LogDlvError2("DlrEr2-" + e4.Message);
                            //throw;
                        }
                    }
                    catch(Exception e4)
                    { }
                }
                catch (Exception ex6) {
                    //LogDlvError("Dlv Err 6 - " + ex2.Message + " - " + msgId + " - " + dlvStat + " - " + errcd);
                }
            }
        }
        public void UpdateOpenTemplate()
        {
            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cnn.Open();
                    //SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = 100;
                    cmd.Connection = cnn;
                    cmd.CommandText = "SP_UpdateOpenTemplate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cnn.Close();
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

        public int GetDuplicateCount_HOMECREDIT(string mob, string txt, string fileid, string profileid)
        {
            string sql = @"select count(*) from MSGSUBMITTED with (nolock) where TOMOBILE = '" + mob + @"' ";
                sql = sql + "and convert(date,sentdatetime)=convert(date,getdate()) ";
                sql = sql + "and dbo.AlphaNumericOnly(SMSTEXT)= dbo.AlphaNumericOnly('" + txt.Replace("'", "") + "')";
                int mcnt = Convert.ToInt16(database.GetScalarValue(sql));
                return mcnt;
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
            #region < Commented >
            //if (dr["SMPPACCOUNTID"].ToString().Contains("90") && mcnt == 0)
            //{
            //    sql = @"SELECT COUNT(*) FROM DlvErrDATA D INNER JOIN errorcode C ON D.err_code=C.code WHERE C.TOCHECKPROMO=1 AND  
            //    D.SENTDATETIME > DATEADD(DAY,-1 * C.days, GETDATE()) and D.TOMOBILE = '" + dr["TOMOBILE"].ToString() + "' ";

            //    mcnt = Convert.ToInt16(database.GetScalarValue(sql));

            //    if (mcnt > 0)
            //    {
            //        int noofsms = GetNoOfSMS(dr["MSGTEXT"].ToString().Length, dr["DataCode"].ToString());
            //        bool ucs2 = (dr["DataCode"].ToString().ToUpper() == "UCS2" ? true : false);
            //        for (int x = 0; x < noofsms; x++)
            //        {
            //            string smsTex = GetSMSText(dr["MSGTEXT"].ToString(), x + 1, noofsms, ucs2);

            //            sql = @"declare @id varchar(100)  select @id = newid()
            //        Insert into msgsubmitted (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND,smstext) " +
            //        "values ('" + dr["id"].ToString() + @"',
            //        '" + dr["PROVIDER"].ToString() + @"',
            //        '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //        '" + dr["PROFILEID"].ToString() + @"',
            //        N'" + smsTex.Replace("'", "''") + @"',
            //        '" + dr["TOMOBILE"].ToString() + @"',
            //        '" + dr["SENDERID"].ToString() + @"',
            //        '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
            //        '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1',N'" + dr["MSGTEXT"].ToString().Replace("'", "''") + @"') ; " +
            //        "Insert into NOTSUBMITTED (ID,PROVIDER,SMPPACCOUNTID,PROFILEID,MSGTEXT,TOMOBILE,SENDERID,CREATEDAT,SENTDATETIME,MSGID,FILEID,NSEND) " +
            //            "values ('" + dr["id"].ToString() + @"',
            //        '" + dr["PROVIDER"].ToString() + @"',
            //        '" + dr["SMPPACCOUNTID"].ToString() + @"',
            //        '" + dr["PROFILEID"].ToString() + @"',
            //        N'" + smsTex.Replace("'", "''") + @"',
            //        '" + dr["TOMOBILE"].ToString() + @"',
            //        '" + dr["SENDERID"].ToString() + @"',
            //        '" + Convert.ToDateTime(dr["CREATEDAT"]).ToString("dd/MMM/yyyy HH:mm:ss.fff") + @"',GETDATE(),@id,
            //        '" + (dr["FILEID"] == DBNull.Value ? "0" : dr["FILEID"].ToString()) + "','1') ; ";
            //            database.ExecuteNonQuery(sql);
            //        }
            //    }
            //}
            #endregion
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

        public DataTable GetDLRCallBackCustomers()
        {
            DataTable dt = database.GetDataTable("Select UserName from Customer where DLRPushHookAPI <> ''");
            return dt;
        }

        // changes for dlt error BD mail 03/11/2022

        public void send_errormail(string dlrtime, string msgId, string msgClientId, string mobileno, string senderid, string templateid, string peid, string msgtext, string profileid, string senddate, string errorcode, string errordesc)
        {
            StringBuilder sb = new StringBuilder();

            List<string> To = new List<string>();
            List<string> CC = new List<string>();

            DataTable dt = database.GetDataTable("SELECT * FROM [SettingMast] with (nolock)");

            DataTable dtTo = database.GetDataTable(@"select e.EmailId BDEmailId,r.EmailId ManagerEmailId,COMPNAME from emimpanel..customer c with (nolock) 
join emimpanel..employee e with (nolock) on c.EmpCode=e.EmployeeCode 
left join emimpanel..employee r with (nolock) on e.ReportToId=r.EmployeeCode and e.ReportToId is not null
where username='" + profileid + "'");

            string Subject = errorcode + " " + errordesc;
            Subject = Subject.Replace('\r', ' ').Replace('\n', ' ');

            sb.Append("<b>dlrtime :</b>" + dlrtime + "<br>");
            sb.Append("<b>msgId :</b>" + msgId + "<br>");
            sb.Append("<b>msgClientId :</b>" + msgClientId + "<br>");
            sb.Append("<b>senderid :</b>" + senderid + "<br>");
            sb.Append("<b>templateid :</b>" + templateid + "<br>");
            sb.Append("<b>peid :</b>" + peid + "<br>");
            sb.Append("<b>msgtext :</b>" + msgtext + "<br>");
            sb.Append("<b>profileid :</b>" + profileid + "<br>");
            sb.Append("<b>senddate :</b>" + senddate + "<br>");
            sb.Append("<b>errorcode :</b>" + errorcode + "<br>");
            sb.Append("<b>errordesc :</b>" + errordesc + "<br><br><br>");

            sb.Append("This is a system generated email.<br><br>");
            sb.Append("Generated at : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));

            if (dtTo.Rows.Count > 0)
            {
                if (dtTo.Rows[0]["BDEmailId"] != null && dtTo.Rows[0]["BDEmailId"].ToString() != "")
                {
                    To.Add(dtTo.Rows[0]["BDEmailId"].ToString().Trim());
                }
                if (dtTo.Rows[0]["ManagerEmailId"] != null && dtTo.Rows[0]["ManagerEmailId"].ToString() != "")
                {
                    To.Add(dtTo.Rows[0]["ManagerEmailId"].ToString().Trim());
                }
                if (To.Count < 1) To.Add(dt.Rows[0]["CC1"].ToString().Trim());
            }
            else
            {
                To.Add(dt.Rows[0]["CC1"].ToString().Trim());
            }

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CC1"] != null && dt.Rows[0]["CC1"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC1"].ToString().Trim());
                }
                if (dt.Rows[0]["CC2"] != null && dt.Rows[0]["CC2"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC2"].ToString().Trim());
                }
                if (dt.Rows[0]["CC3"] != null && dt.Rows[0]["CC3"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC3"].ToString().Trim());
                }
                if (dt.Rows[0]["CC4"] != null && dt.Rows[0]["CC4"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC4"].ToString().Trim());
                }
                if (dt.Rows[0]["CC5"] != null && dt.Rows[0]["CC5"].ToString() != "")
                {
                    CC.Add(dt.Rows[0]["CC5"].ToString().Trim());
                }
            }
            if (To.Count > 0)
            {
                string str = SendEmail_(Subject, sb.ToString(), dt.Rows[0]["UserId"].ToString().Trim(), dt.Rows[0]["Password"].ToString().Trim(), dt.Rows[0]["host"].ToString().Trim(), dt.Rows[0]["PortNo"].ToString().Trim(),To, CC);
            }
        }
        
        public string SendEmail_(string subject, string body, string UserId, string Pwd, string Host, string Port, List<string> To, List<string> CC, List<string> BCC = null)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = UserId; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                //MailMessage message = new MailMessage();

                MailMessage message = new MailMessage(senderID, Convert.ToString(To[0]), subject, body);

                message.IsBodyHtml = true;

                if (To != null)
                {
                    for (int i = 1; i < To.Count; i++)
                    {
                        message.To.Add(To[i]);
                    }
                }

                if (CC != null)
                {
                    for (int i = 0; i < CC.Count; i++)
                    {
                        message.CC.Add(CC[i]);
                    }
                }

                if (BCC != null)
                {
                    for (int i = 0; i < BCC.Count; i++)
                    {
                        message.Bcc.Add(BCC[i]);
                    }
                }

                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                LogDlvError_mail("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }


        public string SendEmail(string toAddress, string subject, string body, string UserId, string Pwd, string Host, string Port, List<string> CC)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = UserId; // "info@emim.in";
            string senderPassword = Pwd; // "info";
            try
            {
                //Host = "mymail2889.com",

                SmtpClient smtp = new SmtpClient
                {
                    Host = Host,
                    Port = int.Parse(Port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                message.IsBodyHtml = true;
                for (int i = 0; i < CC.Count; i++)
                {
                    message.CC.Add(CC[i]);
                }
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!! " + ex.Message;
                LogDlvError_mail("error on sending email - " + ex.Message + " -- " + ex.StackTrace);
            }
            return result;
        }
        public void LogDlvError_mail(string msg)
        {
            try
            {
                //if (LogErr == 1)
                //{
                FileStream filestrm = new FileStream(fn + @"EMAIL_Dlv_Log_" + DateTime.Now.ToString("ddMMMyyyyHHmm") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
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
                    FileStream filestrm = new FileStream(fn + @"EMAIL_Dlv_Log_catch_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter strmwriter = new StreamWriter(filestrm);
                    strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                    strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                    strmwriter.Flush();
                    strmwriter.Close();
                }
                catch (Exception ex2) { }
            }
        }

    }
}
