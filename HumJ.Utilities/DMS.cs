using System;
using System.Text.RegularExpressions;

namespace HumJ.Utilities
{
    public static class DMS
    {
        /// <summary>
        /// 字符串格式的度分秒转换为浮点数格式的角度
        /// </summary>
        public static double DMS_Convert(this string dms)
        {
            var minus = Regex.IsMatch(dms, "-(?=\\d)");
            var numbers = Regex.Matches(dms, "\\d+(\\.\\d+)?");

            var d = numbers.Count > 0 ? double.Parse(numbers[0].Value) : 0;
            var m = numbers.Count > 1 ? double.Parse(numbers[1].Value) : 0;
            var s = numbers.Count > 2 ? double.Parse(numbers[2].Value) : 0;

            var sign = minus ? -1 : 1;
            var result = sign * (d + m / 60 + s / 3600);

            return result;
        }

        /// <summary>
        /// 浮点数格式的角度转换为字符串格式的度分秒
        /// </summary>
        public static string DMS_Convert(this double dms, string format = "{0}°{1}′{2}″")
        {
            var minus = dms < 0;
            var abs = Math.Abs(dms);

            var d = (int)abs;
            abs = (abs - d) * 60;
            var m = (int)abs;
            abs = (abs - m) * 60;
            var s = abs;

            var result = string.Format(format, (minus ? "-" : "") + d, m, s);
            return result;
        }
    }
}