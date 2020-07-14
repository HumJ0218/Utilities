using HumJ.Utilities;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class DMSTests
    {
        private int count;
        private Random random;

        [SetUp]
        public void Setup()
        {
            count = 8192;

            random = new Random();
        }

        [Test]
        public void Convert()
        {
            for (var i = 0; i < count; i++)
            {
                var value = (random.NextDouble() - 0.5) * 180;
                var str = value.DMS_Convert();
                var testValue = str.DMS_Convert();

                var diff = Math.Abs(testValue - value);
                var threshold = 1.0 / 3600 / 1000000;

                Assert.LessOrEqual(diff, threshold);
            }
        }
    }
}