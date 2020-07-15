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
            for (int i = 0; i < count; i++)
            {
                string sample = string.Join("", Enumerable.Range(0, sizeof(byte) * 8).Select(m => sampleChar[random.Next(2)]));
                byte sampleValue = Convert.ToByte(sample, 2);

                string sampleReverse = string.Join("", sample.Reverse());
                byte sampleReverseValue = Convert.ToByte(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (int i = 0; i < count; i++)
            {
                string sample = string.Join("", Enumerable.Range(0, sizeof(ushort) * 8).Select(m => sampleChar[random.Next(2)]));
                ushort sampleValue = Convert.ToUInt16(sample, 2);

                string sampleReverse = string.Join("", sample.Reverse());
                ushort sampleReverseValue = Convert.ToUInt16(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (int i = 0; i < count; i++)
            {
                string sample = string.Join("", Enumerable.Range(0, sizeof(uint) * 8).Select(m => sampleChar[random.Next(2)]));
                uint sampleValue = Convert.ToUInt32(sample, 2);

                string sampleReverse = string.Join("", sample.Reverse());
                uint sampleReverseValue = Convert.ToUInt32(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }

            for (int i = 0; i < count; i++)
            {
                string sample = string.Join("", Enumerable.Range(0, sizeof(ulong) * 8).Select(m => sampleChar[random.Next(2)]));
                ulong sampleValue = Convert.ToUInt64(sample, 2);

                string sampleReverse = string.Join("", sample.Reverse());
                ulong sampleReverseValue = Convert.ToUInt64(sampleReverse, 2);

                Assert.AreEqual(sampleValue.Bit_Reverse(), sampleReverseValue);
            }
        }
    }
}