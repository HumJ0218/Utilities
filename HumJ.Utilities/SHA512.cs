using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HumJ.Utilities
{
    public static class SHA512
    {
        private static readonly HashAlgorithm ha = System.Security.Cryptography.SHA512.Create();

        /// <summary>
        /// 计算字节数组的 SHA512
        /// </summary>
        public static byte[] GetSHA512Hash(this byte[] bytes)
        {
            return ha.ComputeHash(bytes);
        }

        /// <summary>
        /// 计算字节流的 SHA512
        /// </summary>
        public static byte[] GetSHA512Hash(this Stream stream)
        {
            return ha.ComputeHash(stream);
        }

        /// <summary>
        /// 计算字符串的 SHA512（UTF-8 编码）
        /// </summary>
        public static byte[] GetSHA512Hash(this string str)
        {
            return str.GetSHA512Hash(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 SHA512（自定义编码）
        /// </summary>
        public static byte[] GetSHA512Hash(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GetSHA512Hash();
        }

        /// <summary>
        /// 计算文件的 SHA512
        /// </summary>
        public static byte[] GetSHA512Hash(this FileInfo file)
        {
            using FileStream s = file.OpenRead();
            return s.GetSHA512Hash();
        }
    }
}