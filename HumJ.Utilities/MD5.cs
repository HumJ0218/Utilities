using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HumJ.Utilities
{
    public static class MD5
    {
        private static readonly HashAlgorithm ha = System.Security.Cryptography.MD5.Create();

        /// <summary>
        /// 计算字节数组的 MD5
        /// </summary>
        public static byte[] GetMD5Hash(this byte[] bytes)
        {
            return ha.ComputeHash(bytes);
        }

        /// <summary>
        /// 计算字节流的 MD5
        /// </summary>
        public static byte[] GetMD5Hash(this Stream stream)
        {
            return ha.ComputeHash(stream);
        }

        /// <summary>
        /// 计算字符串的 MD5（UTF-8 编码）
        /// </summary>
        public static byte[] GetMD5Hash(this string str)
        {
            return str.GetMD5Hash(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 MD5（自定义编码）
        /// </summary>
        public static byte[] GetMD5Hash(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GetMD5Hash();
        }

        /// <summary>
        /// 计算文件的 MD5
        /// </summary>
        public static byte[] GetMD5Hash(this FileInfo file)
        {
            using FileStream s = file.OpenRead();
            return s.GetMD5Hash();
        }
    }
}