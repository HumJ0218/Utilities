using System.Linq;

namespace HumJ.Utilities
{
    public static class ByteArray
    {
        /// <summary>
        /// 获取字节数组的十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="join">分隔符号</param>
        public static string ToHexString(this byte[] bytes, string join = null)
        {
            join ??= "";
            return string.Join(join, bytes.Select(b => b.ToString("X2")));
        }
    }
}