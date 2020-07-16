using System;
using System.Linq;

namespace HumJ.Utilities
{
    public static class Excel
    {
        /// <summary>
        /// Excel 列号数字转字母
        /// </summary>
        /// <param name="iCol">列号，1起始</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/office/troubleshoot/excel/convert-excel-column-numbers"/>
        public static string ConvertToExcelColumnLetter(this int iCol)
        {
            int a, b;
            string ConvertToLetter = "";
            while (iCol > 0)
            {
                a = (iCol - 1) / 26;
                b = (iCol - 1) % 26;
                ConvertToLetter = (char)(b + 65) + ConvertToLetter;
                iCol = a;
            }

            return ConvertToLetter;
        }

        /// <summary>
        /// Excel 列号字母转数字（1 起始）
        /// </summary>
        /// <param name="iCol">列号，A起始</param>
        public static int ConvertToExcelColumnNumber(this string iCol)
        {
            iCol = iCol.ToUpper();

            if (iCol.Any(c => c < 'A' || c > 'Z'))
            {
                throw new ArgumentException("Excel 列号无效", nameof(iCol));
            }

            int i = 0;
            foreach (char c in iCol)
            {
                i *= 26;
                i += c - 65 + 1;
            }

            return i;
        }
    }
}