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
            count = 1024;

            random = new Random();
            encoding = Encoding.Default;
        }

        [Test]
        public void ByteArray_Compress_Decompress()
        {
            for (var i = 0; i < count; i++)
            {
                var bytes = new byte[random.Next(65536)];
                random.NextBytes(bytes);

                var testValue = bytes.GZip_Compress().GZip_Decompress();

                Assert.AreEqual(bytes, testValue);
            }
        }

        [Test]
        public void String_Compress_Decompress()
        {
            for (var i = 0; i < count; i++)
            {

                var str1 = string.Join("", Enumerable.Range(0, random.Next(65536)).Select(m => (char)(32 + random.Next(32767 - 32))));
                var testValue1 = str1.GZip_CompressFromString().GZip_DecompressToString();
                Assert.AreEqual(str1, testValue1);

                var str2 = string.Join("", Enumerable.Range(0, random.Next(65536)).Select(m => (char)(32 + random.Next(32767 - 32))));
                var testValue2 = str2.GZip_CompressFromString(encoding).GZip_DecompressToString(encoding);
                Assert.AreEqual(str2, testValue2);
            }
        }
    }
}