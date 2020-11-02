using System.IO;
using System.IO.Compression;
using System.Text;

namespace HumJ.Utilities
{
    public static class Deflate
    {
        /// <summary>
        /// 使用 Deflate 压缩字节数组
        /// </summary>
        public static byte[] DeflateCompress(this byte[] bytes)
        {
            byte[] zipBytes;
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (DeflateStream zipStream = new DeflateStream(compressStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                }

                zipBytes = compressStream.ToArray();
            }

            return zipBytes;
        }

        /// <summary>
        /// 使用 Deflate 解压缩字节数组
        /// </summary>
        public static byte[] DeflateDecompress(this byte[] zipBytes)
        {
            using MemoryStream zipms = new MemoryStream(zipBytes);
            using DeflateStream decompressedStream = new DeflateStream(zipms, CompressionMode.Decompress);
            using MemoryStream ms = new MemoryStream();
            decompressedStream.CopyTo(ms);

            byte[] bytes = ms.ToArray();
            return bytes;
        }

        /// <summary>
        /// 使用 Deflate 压缩字符串（UTF-8 编码）
        /// </summary>
        public static byte[] DeflateCompressFromString(this string str)
        {
            return str.DeflateCompressFromString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 Deflate 压缩字符串（自定义编码）
        /// </summary>
        public static byte[] DeflateCompressFromString(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str).DeflateCompress();
        }

        /// <summary>
        /// 使用 Deflate 解压缩字符串（UTF-8 编码）
        /// </summary>
        public static string DeflateDecompressToString(this byte[] zipBytes)
        {
            return zipBytes.DeflateDecompressToString(Encoding.UTF8);
        }

        /// <summary>
        /// 使用 Deflate 解压缩字符串（自定义编码）
        /// </summary>
        public static string DeflateDecompressToString(this byte[] zipBytes, Encoding encoding)
        {
            return encoding.GetString(zipBytes.DeflateDecompress());
        }
    }
}