using HumJ.Utilities;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class ExcelTests
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
        public void ColumnLetter_ColumnNumber()
        {
            for (var i = 0; i < count; i++)
            {
                var value = random.Next(65536);

                var testValue = value.Excel_ConvertToColumnLetter().Excel_ConvertToColumnNumber();

                Assert.AreEqual(value, testValue);
            }
        }
    }
}