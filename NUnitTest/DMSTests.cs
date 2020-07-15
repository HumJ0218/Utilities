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
            for (int i = 0; i < count; i++)
            {
                double value = (random.NextDouble() - 0.5) * 180;
                string str = value.DMS_Convert();
                double testValue = str.DMS_Convert();

                double diff = Math.Abs(testValue - value);
                double threshold = 1.0 / 3600 / 1000000;

                Assert.LessOrEqual(diff, threshold);
            }
        }
    }
}