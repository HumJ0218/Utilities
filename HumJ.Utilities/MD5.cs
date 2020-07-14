using System.IO;
using System.Text;

namespace HumJ.Utilities
{
    public static class MD5
    {
        private static readonly System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        /// <summary>
        /// 计算字节数组的 MD5
        /// </summary>
        public static byte[] MD5_ComputeHash(this byte[] bytes)
        {
            return md5.ComputeHash(bytes);
        }

        /// <summary>
        /// 计算字节流的 MD5
        /// </summary>
        public static byte[] MD5_ComputeHash(this Stream stream)
        {
            return md5.ComputeHash(stream);
        }

        /// <summary>
        /// 计算字符串的 MD5（UTF-8 编码）
        /// </summary>
        public static byte[] MD5_ComputeHash(this string str)
        {
            return str.MD5_ComputeHash(Encoding.UTF8);
        }

        /// <summary>
        /// 计算字符串的 MD5（自定义编码）
        /// </summary>
        public static byte[] MD5_ComputeHash(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).MD5_ComputeHash();
        }

        /// <summary>
        /// 计算文件的 MD5
        /// </summary>
        public static byte[] MD5_ComputeHash(this FileInfo file)
        {
            return file.OpenRead().MD5_ComputeHash();
        }
    }
}