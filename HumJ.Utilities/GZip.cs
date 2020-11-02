using System.IO;
using System.IO.Compression;
using System.Text;

namespace HumJ.Utilities
{
    public static class GZip
    {
        /// <summary>
        /// 使用 GZip 压缩字节数组
        /// </summary>
        public static byte[] GZipCompress(this byte[] bytes)
        {
            byte[] zipBytes;
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                }

                zipBytes = compressStream.ToArray();
            }

            return zipBytes;
        }

        /// <summary>
        /// 使用 GZip 解压缩字节数组
        /// </summary>
        public static byte[] GZipDecompress(this byte[] zipBytes)
        {
            using MemoryStream zipms = new MemoryStream(zipBytes);
            using GZipStream decompressedStream = new GZipStream(zipms, CompressionMode.Decompress);
            using MemoryStream ms = new MemoryStream();
            decompressedStream.CopyTo(ms);

            byte[] bytes = ms.ToArray();
            return bytes;
        }

        /// <summary>
        /// 使用 GZip 压缩字符串（UTF-8 编码）
        /// </summary>
        public static byte[] GZipCompressFromString(this string str)
        {
            return str.GZipCompressFromString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 GZip 压缩字符串（自定义编码）
        /// </summary>
        public static byte[] GZipCompressFromString(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GZipCompress();
        }

        /// <summary>
        /// 使用 GZip 解压缩字符串（UTF-8 编码）
        /// </summary>
        public static string GZipDecompressToString(this byte[] zipBytes)
        {
            return zipBytes.GZipDecompressToString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 GZip 解压缩字符串（自定义编码）
        /// </summary>
        public static string GZipDecompressToString(this byte[] zipBytes, Encoding encoding)
        {
            return encoding.GetString(zipBytes.GZipDecompress());
        }
    }
}