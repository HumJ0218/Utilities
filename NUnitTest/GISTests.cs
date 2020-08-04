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
            {
                (double, double) wgs = (39.907333, 116.391083);
                Assert.AreEqual((215825, 99330), GIS.GetGisTileXY(wgs, 18));
            }

            {
                (float, float) ll = (42, 122);

                (double lat, double lon) ll1 = GIS.Gauss_LL(GIS.LL_Gauss(ll, GIS.Coordinate.BJ54), GIS.Coordinate.BJ54);
                Assert.AreEqual(ll, ((float)ll1.lat, (float)ll1.lon));

                (double lat, double lon) ll2 = GIS.Gauss_LL(GIS.LL_Gauss(ll, GIS.Coordinate.WGS84), GIS.Coordinate.WGS84);
                Assert.AreEqual(ll, ((float)ll2.lat, (float)ll2.lon));

                (double lat, double lon) ll3 = GIS.Gauss_LL(GIS.LL_Gauss(ll, GIS.Coordinate.XA80), GIS.Coordinate.XA80);
                Assert.AreEqual(ll, ((float)ll3.lat, (float)ll3.lon));
            }
        }
    }
}