using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Data;
using QRCodeEncoderDecoderLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using Shortnr.Web.Data;
using System.Security.Policy;
using Shortnr.Web.Business;
using Shortnr.Web.Entities;
using eMIMPanel.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using System.Web.Hosting;
using System.ComponentModel;
using System.Net.Mail;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace eMIMPanel.Helper
{
    public class QA_Util
    {
        public DataTable dt;
        public SqlConnection con;
        public SqlCommand cmd;
        public SqlDataAdapter sda;


        public DataTable GetRecord(string sql)
        {
            return database.GetDataTable(sql);

        }


        public string RemoveSenderID(string value)
        {
            string result = "";
            try
            {
                string[] a = (value.Split(','));
                database.RemoveSenderID(a);
                result = "SuccessFully Remove SenderID";

            }
            
            catch (Exception ex)
            {
                result = "Not Remove";
                throw ex;

            }

            return result;


        }
    
    }
}