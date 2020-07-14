using HumJ.Utilities;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class ByteArrayTests
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
        public void ToHexString()
        {
            for (var i = 0; i < count; i++)
            {
                var bytes = new byte[random.Next(65536)];
                random.NextBytes(bytes);

                var ba = bytes.ByteArray_ToHexString("-");
                var bas = BitConverter.ToString(bytes);

                Assert.AreEqual(ba, bas);
            }
        }
    }
}