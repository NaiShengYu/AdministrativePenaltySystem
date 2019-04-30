using System;
using System.Collections.Generic;
using System.Text;

namespace WTONewProject.Tools
{
    public class TimeUtils
    {
        public static string DateTime2YMD(DateTime? time)
        {
            if (time == null)
            {
                return "";
            }
            return time.Value.ToString("yyyy-MM-dd");
        }

        public static string DateTime2YMDHM(DateTime time)
        {
            if (time == null)
            {
                return "";
            }
            return time.ToString("yyyy-MM-dd HH:mm");
        }

        public static string DateTime2YMDHMSNowrap(DateTime time)
        {
            if (time == null)
            {
                return "";
            }
            return time.ToString("yyyyMMddHHmmss");
        }

        public static string DateTime2YMDHMS(DateTime time)
        {
            if (time == null)
            {
                return "";
            }
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string Time2HHmm(TimeSpan time)
        {
            if (time == null)
            {
                return "";
            }
            return time.ToString("HH:mm");
        }
    }
}
