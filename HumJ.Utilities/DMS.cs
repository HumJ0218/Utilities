using System;
using System.Text.RegularExpressions;

namespace HumJ.Utilities
{
    public static class DMS
    {
        /// <summary>
        /// 字符串格式的度分秒转换为浮点数格式的角度
        /// </summary>
        public static double ConvertDmsToDeg(this string dms)
        {
            bool minus = Regex.IsMatch(dms, "-(?=\\d)");
            MatchCollection numbers = Regex.Matches(dms, "\\d+(\\.\\d+)?");

            double d = numbers.Count > 0 ? double.Parse(numbers[0].Value) : 0;
            double m = numbers.Count > 1 ? double.Parse(numbers[1].Value) : 0;
            double s = numbers.Count > 2 ? double.Parse(numbers[2].Value) : 0;

            int sign = minus ? -1 : 1;
            double result = sign * (d + m / 60 + s / 3600);

            return result;
        }

        /// <summary>
        /// 浮点数格式的角度转换为字符串格式的度分秒
        /// </summary>
        public static string ConvertDegToDms(this double deg, string format = "{0}°{1}′{2}″")
        {
            bool minus = deg < 0;
            double abs = Math.Abs(deg);

            int d = (int)abs;
            abs = (abs - d) * 60;
            int m = (int)abs;
            abs = (abs - m) * 60;
            double s = abs;

            string result = string.Format(format, (minus ? "-" : "") + d, m, s);
            return result;
        }
    }
}