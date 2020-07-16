using System;

namespace HumJ.Utilities
{
    public static partial class GIS
    {
        public static (int x, int y) GIS_GetTileXY((double lat, double lon) point, int z)
        {

            var lat = point.lat;
            var lon = point.lon;

            var lat_rad = lat * Math.PI / 180;

            var x = Math.Floor((lon + 180) / 360 * Math.Pow(2, z));
            var y = Math.Floor((1 - Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad)) / Math.PI) * Math.Pow(2, z - 1));

            return ((int)x, (int)y);
        }
    }
}