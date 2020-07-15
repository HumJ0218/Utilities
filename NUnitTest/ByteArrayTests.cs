using HumJ.Utilities;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class ByteArrayTests
    {
        private int count;
        private Random random;

        [SetUp]
        public void Setup()
        {
            count = 1024;

            random = new Random();
        }

        [Test]
        public void ToHexString()
        {
            for (int i = 0; i < count; i++)
            {
                byte[] bytes = new byte[random.Next(65536)];
                random.NextBytes(bytes);

                string ba = bytes.ByteArray_ToHexString("-");
                string bas = BitConverter.ToString(bytes);

                Assert.AreEqual(ba, bas);
            }
        }
    }
}