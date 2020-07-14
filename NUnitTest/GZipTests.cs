using HumJ.Utilities;
using NUnit.Framework;
using System;
using System.Linq;

namespace NUnitTest
{
    public class GZipTests
    {
        private Random random;
        private int count;

        [SetUp]
        public void Setup()
        {
            count = 1024;

            random = new Random();
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
                var str = string.Join("", Enumerable.Range(0, random.Next(65536)).Select(m => (char)(32 + random.Next(96))));

                var testValue = str.GZip_CompressFromString().GZip_DecompressToString();

                Assert.AreEqual(str, testValue);
            }
        }
    }
}