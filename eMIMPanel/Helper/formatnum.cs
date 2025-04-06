using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMIMPanel.Helper
{
    public class formatnum
    {

        public static string FormatNumber(long num)
        {
            num = MaxThreeSignificantDigits(num);

            if (num >= 100000000)
                return (num / 1000000D).ToString("0.#M");
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##M");
            if (num >= 100000)
                return (num / 1000D).ToString("0k");
            if (num >= 100000)
                return (num / 1000D).ToString("0.#k");
            if (num >= 1000)
                return (num / 1000D).ToString("0.##k");
            return num.ToString("#,0");
        }


        public static long MaxThreeSignificantDigits(long x)
        {
            int i = (int)Math.Log10(x);
            i = Math.Max(0, i - 2);
            i = (int)Math.Pow(10, i);
            return x / i * i;
        }

    }

}