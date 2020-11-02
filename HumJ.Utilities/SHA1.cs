using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HumJ.Utilities
{
    public static class SHA1
    {
        private static readonly HashAlgorithm ha = System.Security.Cryptography.SHA1.Create();

        /// <summary>
        /// 计算字节数组的 SHA1
        /// </summary>
        public static byte[] GetSHA1Hash(this byte[] bytes)
        {
            return ha.ComputeHash(bytes);
        }

        /// <summary>
        /// 计算字节流的 SHA1
        /// </summary>
        public static byte[] GetSHA1Hash(this Stream stream)
        {
            return ha.ComputeHash(stream);
        }

        /// <summary>
        /// 计算字符串的 SHA1（UTF-8 编码）
        /// </summary>
        public static byte[] GetSHA1Hash(this string str)
        {
            return str.GetSHA1Hash(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 SHA1（自定义编码）
        /// </summary>
        public static byte[] GetSHA1Hash(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GetSHA1Hash();
        }

        /// <summary>
        /// 计算文件的 SHA1
        /// </summary>
        public static byte[] GetSHA1Hash(this FileInfo file)
        {
            using FileStream s = file.OpenRead();
            return s.GetSHA1Hash();
        }
    }
}