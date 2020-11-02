using System;
using System.IO;
using System.Text;

namespace HumJ.Utilities
{
    public static class Base64
    {
        /// <summary>
        /// 字节数组转换为 Base64
        /// </summary>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64 转换为字节数组
        /// </summary>
        public static byte[] ByteArrayFromBase64String(this string str)
        {
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// 计算字符串的 Base64（UTF-8 编码）
        /// </summary>
        public static string ToBase64String(this string str)
        {
            return str.ToBase64String(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 Base64（自定义编码）
        /// </summary>
        public static string ToBase64String(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).ToBase64String();
        }

        /// <summary>
        /// 计算 Base64 的字符串（UTF-8 编码）
        /// </summary>
        public static string StringFromBase64String(this string str)
        {
            return str.StringFromBase64String(Encoding.UTF8);
        }

        /// <summary>
        /// 计算 Base64 的字符串（自定义编码）
        /// </summary>
        public static string StringFromBase64String(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).ToBase64String();
        }

        /// <summary>
        /// 计算文件的 Base64
        /// </summary>
        public static string ToBase64String(this FileInfo file)
        {
            return File.ReadAllBytes(file.FullName).ToBase64String();
        }

        /// <summary>
        /// 输出 Base64 原始文件
        /// </summary>
        public static void FileFromBase64String(this string str, FileInfo file)
        {
            File.WriteAllBytes(file.FullName, str.ByteArrayFromBase64String());
        }
    }
}