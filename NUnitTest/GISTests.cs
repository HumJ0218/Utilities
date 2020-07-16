using HumJ.Utilities;
using NUnit.Framework;

namespace NUnitTest
{
    public class GISTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToHexString()
        {
            (double, double) wgs = (39.907333, 116.391083);
            Assert.AreEqual((215825, 99330), GIS.GIS_GetTileXY(wgs, 18));
        }
    }
}