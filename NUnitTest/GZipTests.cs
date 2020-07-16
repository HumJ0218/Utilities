using HumJ.Utilities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace NUnitTest
{
    public class GZipTests
    {
        private int count;
        private Random random;
        private Encoding encoding;

        [SetUp]
        public void Setup()
        {
            count = 256;

            random = new Random();
            encoding = Encoding.Default;
        }

        [Test]
        public void ByteArray_Compress_Decompress()
        {
            for (int i = 0; i < count; i++)
            {
                byte[] bytes = new byte[random.Next(65536)];
                random.NextBytes(bytes);

                byte[] testValue = bytes.GZipCompress().GZipDecompress();

                Assert.AreEqual(bytes, testValue);
            }
        }

        [Test]
        public void String_Compress_Decompress()
        {
            for (int i = 0; i < count; i++)
            {

                string str1 = string.Join("", Enumerable.Range(0, random.Next(65536)).Select(m => (char)(32 + random.Next(32767 - 32))));
                string testValue1 = str1.GZipCompressFromString().GZipDecompressToString();
                Assert.AreEqual(str1, testValue1);

                string str2 = string.Join("", Enumerable.Range(0, random.Next(65536)).Select(m => (char)(32 + random.Next(32767 - 32))));
                string testValue2 = str2.GZipCompressFromString(encoding).GZipDecompressToString(encoding);
                Assert.AreEqual(str2, testValue2);
            }
        }
    }
}