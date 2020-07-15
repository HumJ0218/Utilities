using HumJ.Utilities;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class ExcelTests
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
        public void ColumnLetter_ColumnNumber()
        {
            for (int i = 0; i < count; i++)
            {
                int value = random.Next(65536);

                int testValue = value.Excel_ConvertToColumnLetter().Excel_ConvertToColumnNumber();

                Assert.AreEqual(value, testValue);
            }
        }
    }
}