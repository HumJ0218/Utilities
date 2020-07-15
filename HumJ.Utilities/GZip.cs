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
        public static byte[] GZip_Compress(this byte[] bytes)
        {
            byte[] gzBytes;
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                }

                gzBytes = compressStream.ToArray();
            }

            return gzBytes;
        }

        /// <summary>
        /// 使用 GZip 解压缩字节数组
        /// </summary>
        public static byte[] GZip_Decompress(this byte[] gzBytes)
        {
            using MemoryStream gzms = new MemoryStream(gzBytes);
            using GZipStream decompressedStream = new GZipStream(gzms, CompressionMode.Decompress);
            using MemoryStream ms = new MemoryStream();
            decompressedStream.CopyTo(ms);

            byte[] bytes = ms.ToArray();
            return bytes;
        }

        /// <summary>
        /// 使用 GZip 压缩字符串（UTF-8 编码）
        /// </summary>
        public static byte[] GZip_CompressFromString(this string str)
        {
            return str.GZip_CompressFromString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 GZip 压缩字符串（自定义编码）
        /// </summary>
        public static byte[] GZip_CompressFromString(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).GZip_Compress();
        }

        /// <summary>
        /// 使用 GZip 解压缩字符串（UTF-8 编码）
        /// </summary>
        public static string GZip_DecompressToString(this byte[] gzBytes)
        {
            return gzBytes.GZip_DecompressToString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 GZip 解压缩字符串（自定义编码）
        /// </summary>
        public static string GZip_DecompressToString(this byte[] gzBytes, Encoding encoding)
        {
            return encoding.GetString(gzBytes.GZip_Decompress());
        }
    }
}