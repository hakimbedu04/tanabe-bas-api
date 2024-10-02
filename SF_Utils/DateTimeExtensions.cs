using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek));
            int finDiff = diff%7;
            return dt.AddDays(-1 * finDiff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek));
            int finDiff = diff % 7;
            return dt.AddDays(finDiff).Date;
        }
    }
}
