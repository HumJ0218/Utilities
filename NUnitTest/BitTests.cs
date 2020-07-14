using HumJ.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTest
{
    public class BitTests
    {
        private int count;
        private Random random;
        private Dictionary<int, char> sampleChar;

        [SetUp]
        public void Setup()
        {
            count = 1024;

            random = new Random();
            sampleChar = new Dictionary<int, char> { { 0, '0' }, { 1, '1' } };
        }

        [Test]
        public void Reverse()
        {
            for (var i = 0; i < count; i++)
            {
                var sample = string.Join("", Enumerable.Range(0, sizeof(byte) * 8).Select(m => sampleChar[random.Next(2)]));
                var sampleValue = Convert.ToByte(sample, 2);

                var sampleReverse = string.Join("", sample.Reverse());
                var sampleReverseValue = Convert.ToByte(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (var i = 0; i < count; i++)
            {
                var sample = string.Join("", Enumerable.Range(0, sizeof(ushort) * 8).Select(m => sampleChar[random.Next(2)]));
                var sampleValue = Convert.ToUInt16(sample, 2);

                var sampleReverse = string.Join("", sample.Reverse());
                var sampleReverseValue = Convert.ToUInt16(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (var i = 0; i < count; i++)
            {
                var sample = string.Join("", Enumerable.Range(0, sizeof(uint) * 8).Select(m => sampleChar[random.Next(2)]));
                var sampleValue = Convert.ToUInt32(sample, 2);

                var sampleReverse = string.Join("", sample.Reverse());
                var sampleReverseValue = Convert.ToUInt32(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (var i = 0; i < count; i++)
            {
                var sample = string.Join("", Enumerable.Range(0, sizeof(ulong) * 8).Select(m => sampleChar[random.Next(2)]));
                var sampleValue = Convert.ToUInt64(sample, 2);

                var sampleReverse = string.Join("", sample.Reverse());
                var sampleReverseValue = Convert.ToUInt64(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }
        }
    }
}