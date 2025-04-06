using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp
{
    public static class clsCheck
    {
        public static string stmid = "1702157302357160700";
        public static string stmidT = "1302157493670013444";
        public static byte[] tmid ;
        public static byte[] tmid_T;
        public static byte[] tmid1 = System.Text.Encoding.UTF8.GetBytes("1702157302357160700");
        public static bool inProcess = false;
        public static bool[] inprocess = {
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,

            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,

            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,

            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,

            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false,
        };

       /* public static int[] procTime = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
         0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
         0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
         0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
         0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
         ) */
        public static DateTime[] prodDt =
        {
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,
            DateTime.Now, DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now,DateTime.Now
        };
        public static uint mysequence = 0;
    }
}
