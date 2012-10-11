using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel.Utils
{
    public static class DateTimeExtensions
    {       
        public static bool IsPersistable(this DateTime datetime)
        {
            return datetime >= Constants.MinPersistableDate && datetime <= Constants.MaxPersistableDate;
        }

        public static int ToInt(this string input)
        {
            int val = 0;
            if (Int32.TryParse(input, out val))
                return val;
            return val;
        }

        public static long ToLong(this string input)
        {
            long val = 0;
            if (long.TryParse(input, out val))
                return val;
            return val;
        }

        public static decimal ToDecimal(this string input)
        {
            decimal val = 0;
            if (decimal.TryParse(input, out val))
                return val;
            return val;
        }
    }
}
