using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HumJ.Utilities
{
    public static class SHA256
    {
        private static readonly HashAlgorithm ha = System.Security.Cryptography.SHA256.Create();

        /// <summary>
        /// 计算字节数组的 SHA256
        /// </summary>
        public static byte[] GetSHA256Hash(this byte[] bytes)
        {
            return ha.ComputeHash(bytes);
        }

        /// <summary>
        /// 计算字节流的 SHA256
        /// </summary>
        public static byte[] GetSHA256Hash(this Stream stream)
        {
            return ha.ComputeHash(stream);
        }

        /// <summary>
        /// 计算字符串的 SHA256（UTF-8 编码）
        /// </summary>
        public static byte[] GetSHA256Hash(this string str)
        {
            return str.GetSHA256Hash(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 SHA256（自定义编码）
        /// </summary>
        public static byte[] GetSHA256Hash(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GetSHA256Hash();
        }

        /// <summary>
        /// 计算文件的 SHA256
        /// </summary>
        public static byte[] GetSHA256Hash(this FileInfo file)
        {
            using FileStream s = file.OpenRead();
            return s.GetSHA256Hash();
        }
    }
}